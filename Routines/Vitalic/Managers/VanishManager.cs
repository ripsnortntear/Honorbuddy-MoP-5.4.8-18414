using System;
using System.Globalization;
using System.Collections; // <-- pour IList
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class VanishManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // MoP IDs
        private const int VanishId = 1856;
        private const int ShadowmeldId = 58984;
        private const int StealthId = 1784;

        // Subterfuge (talent MoP) — plusieurs auras vues dans v.zip
        private static readonly int[] SubterfugeIds = new[] { 115192, 115193, 112942 };

        private static bool _hooksInstalled;
        private static bool _wasStealthed;

        // Noms localisés récupérés depuis l’ID (évite les soucis de langue)
        private static string _vanishName = string.Empty;
        private static string _shadowmeldName = string.Empty;

        public static void Initialize()
        {
            if (_hooksInstalled) return;

            try
            {
                _vanishName = SafeGetSpellName(VanishId);
                _shadowmeldName = SafeGetSpellName(ShadowmeldId);

                Lua.Events.AttachEvent("UNIT_SPELLCAST_SENT", OnUnitSpellcastSent);
                _hooksInstalled = true;
                Logger.Write("[Vanish] Hooks installed (UNIT_SPELLCAST_SENT).");
            }
            catch (Exception ex)
            {
                Logger.Write("[Vanish] Initialize error: " + ex.Message);
            }
        }

        public static void Shutdown()
        {
            if (!_hooksInstalled) return;
            try
            {
                Lua.Events.DetachEvent("UNIT_SPELLCAST_SENT", OnUnitSpellcastSent);
                _hooksInstalled = false;
                Logger.Write("[Vanish] Hooks removed.");
            }
            catch (Exception ex)
            {
                Logger.Write("[Vanish] Shutdown error: " + ex.Message);
            }
        }

        // Hook utilisable dans la BT (ne bloque pas la rotation)
        public static Composite Build()
        {
            return new Action(ret =>
            {
                Execute();
                return RunStatus.Failure;
            });
        }

        // Tick — suivi de l’état furtif (détection entrée/sortie)
        public static void Execute()
        {
            try
            {
                if (Me == null || !Me.IsAlive) return;

                bool stealthedNow = IsStealthed();
                if (stealthedNow != _wasStealthed)
                {
                    if (stealthedNow) OnEnterStealth();
                    else OnExitStealth();

                    _wasStealthed = stealthedNow;
                }
            }
            catch { }
        }

        // ===================== Evénements =====================

        private static void OnUnitSpellcastSent(object sender, LuaEventArgs args)
        {
            try
            {
                // UNIT_SPELLCAST_SENT(unit, spell, rank, target)
                if (args == null) return;

                IList list = args.Args as IList;
                if (list == null || list.Count < 2) return;

                string unit = list[0] as string;
                string spell = list[1] as string;

                if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(spell))
                    return;

                if (!string.Equals(unit, "player", StringComparison.OrdinalIgnoreCase))
                    return;

                bool isVanish = !string.IsNullOrEmpty(_vanishName) && string.Equals(spell, _vanishName, StringComparison.OrdinalIgnoreCase);
                bool isShadowmeld = !string.IsNullOrEmpty(_shadowmeldName) && string.Equals(spell, _shadowmeldName, StringComparison.OrdinalIgnoreCase);

                if (isVanish || isShadowmeld)
                {
                    // Comportement v.zip : stoppe l'auto-attaque dès l’envoi
                    Lua.DoString("StopAttack()");
                    Logger.Write("[Vanish] Detected (" + spell + "): stop auto-attack.");
                }
            }
            catch { }
        }

        // ===================== Helpers =====================

        private static bool IsStealthed()
        {
            if (Me == null || !Me.IsAlive) return false;

            try
            {
                foreach (WoWAura a in Me.GetAllAuras())
                {
                    if (a == null) continue;
                    int id = a.SpellId;
                    if (id == StealthId) return true;
                    for (int i = 0; i < SubterfugeIds.Length; i++)
                        if (id == SubterfugeIds[i]) return true;
                }
            }
            catch { }

            return false;
        }

        private static void OnEnterStealth()
        {
            // Dans v.zip, l’entrée en furtif réinitialise certains états/timers.
            try
            {
                // Reset opener timestamps for rotations (PvE & PvP) to allow fresh opener sequence
                var pve = typeof(VitalicRotation.Managers.PvERotation).GetField("_lastOpenerUtc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (pve != null) pve.SetValue(null, DateTime.MinValue);
                var pvp = typeof(VitalicRotation.Managers.PvPRotation).GetField("_lastOpenerUtc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (pvp != null) pvp.SetValue(null, DateTime.MinValue);
            }
            catch { }
            Logger.Write("[Vanish] Entered Stealth/Subterfuge.");
        }

        private static void OnExitStealth()
        {
            // Sortie de furtif — on peut aussi reset certains délais (ex: HemoDelay gating)
            try
            {
                // Forcing opener timestamp far past ensures we don't block next entry logic inadvertently
                var pve = typeof(VitalicRotation.Managers.PvERotation).GetField("_lastOpenerUtc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (pve != null) pve.SetValue(null, DateTime.MinValue);
                var pvp = typeof(VitalicRotation.Managers.PvPRotation).GetField("_lastOpenerUtc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (pvp != null) pvp.SetValue(null, DateTime.MinValue);
            }
            catch { }
            Logger.Write("[Vanish] Left Stealth/Subterfuge.");
        }

        private static string SafeGetSpellName(int spellId)
        {
            try
            {
                // GetSpellInfo retourne un nom localisé
                string lua = "local n=GetSpellInfo(" + spellId.ToString(CultureInfo.InvariantCulture) + "); return n and n or ''";
                return Lua.GetReturnVal<string>(lua, 0);
            }
            catch { return string.Empty; }
        }
    }
}
