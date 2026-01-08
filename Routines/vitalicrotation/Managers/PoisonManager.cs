using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    internal static class PoisonManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static DateTime _lastAttempt = DateTime.MinValue;
        private const int RetryMs = 2500; // anti-spam

        // === IDs (MoP) ===
        private const int DeadlyPoisonId    = 2823;
        private const int WoundPoisonId     = 8679;
        private const int CripplingPoisonId = 3408;
        private const int MindNumbingId     = 5761;

        private static readonly int[] LethalPoisonIds    = new[] { DeadlyPoisonId, WoundPoisonId };
        private static readonly int[] NonLethalPoisonIds = new[] { CripplingPoisonId, MindNumbingId };

        private static DateTime _lastApply = DateTime.MinValue;
        private const int ApplyThrottleMs = 1500;

        public static void Execute()
        {
            try
            {
                if (Me == null || !Me.IsValid || Me.IsDead) return;

                // Pause offense: éviter le spam pendant un cast manuel récent (même OOC)
                if (EventHandlers.ShouldPauseOffense()) return;

                // OOC seulement (comme v.zip) pour éviter conflits
                if (Me.Combat || Me.IsCasting || Me.IsChanneling || Me.Mounted || Me.IsOnTransport || Me.IsFlying)
                    return;

                if ((DateTime.UtcNow - _lastAttempt).TotalMilliseconds < RetryMs)
                    return;

                // Normaliser les réglages (sélecteur 1..5 -> spellId), sinon garder l'ID
                int mhId = NormalizePoisonSetting(VitalicSettings.Instance.MainHandPoison, true);
                int ohId = NormalizePoisonSetting(VitalicSettings.Instance.OffHandPoison, false);

                // Résoudre un "choix préféré" par groupe
                int preferredLethal = ResolveLethal(mhId, ohId);       // Deadly(2823) ou Wound(8679)
                int preferredNonLethal = ResolveNonLethal(mhId, ohId);    // Crippling/MindNumbing/Leeching

                // Présence par groupe (indépendant langue)
                bool lethalUp = HasAny(Me, LethalPoisonIds);
                bool nonLethalUp = HasAny(Me, NonLethalPoisonIds);

                // Un cast par tick, priorité Lethal manquant
                if (!lethalUp && preferredLethal > 0)
                {
                    if (CastById(preferredLethal))
                    {
                        Logger.Write("[Poisons] Lethal -> " + SafeName(preferredLethal));
                        _lastAttempt = DateTime.UtcNow;
                        return;
                    }
                }

                if (!nonLethalUp && preferredNonLethal > 0)
                {
                    if (CastById(preferredNonLethal))
                    {
                        Logger.Write("[Poisons] Non-Lethal -> " + SafeName(preferredNonLethal));
                        _lastAttempt = DateTime.UtcNow;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("[PoisonManager] Exception: " + ex.Message);
            }
        }

        public static void ExecuteOoc()
        {
            try
            {
                if (Me == null || !Me.IsAlive) return;
                if (Me.Combat) return;
                if ((DateTime.UtcNow - _lastApply).TotalMilliseconds < ApplyThrottleMs) return;

                bool pvp = false;
                try { pvp = VitalicSettings.Instance.PveMode == false; } catch { }

                int lethal = DeadlyPoisonId; // MoP: Deadly par défaut
                int nonLethal = pvp ? CripplingPoisonId : MindNumbingId;

                bool lethalOk = HasAny(Me, LethalPoisonIds);
                bool nonLethalOk = HasAny(Me, NonLethalPoisonIds);

                if (!lethalOk && SpellBook.CanCast(lethal, Me))
                {
                    if (SpellBook.Cast(lethal, Me)) { _lastApply = DateTime.UtcNow; return; }
                }
                if (!nonLethalOk && SpellBook.CanCast(nonLethal, Me))
                {
                    if (SpellBook.Cast(nonLethal, Me)) { _lastApply = DateTime.UtcNow; return; }
                }
            }
            catch { }
        }

        // ===== Added helper for snare logic =====
        public static bool HasCripplingEquipped()
        {
            try
            {
                bool cripUp = HasAuraId(Me, CripplingPoisonId);
                if (cripUp) return true;
                int oh = NormalizePoisonSetting(VitalicSettings.Instance.OffHandPoison, false);
                return oh == CripplingPoisonId;
            }
            catch { return true; }
        }

        // ===== Helpers =====

        private static string SafeName(int spellId)
        {
            try { return SpellBook.GetSpellName(spellId); } catch { return spellId.ToString(); }
        }

        /// <summary>
        /// 1..5 => IDs MoP ; 0/neg => fallback (Deadly pour MH, Crippling pour OH) ; sinon déjà un spellId.
        /// 1: Deadly(2823), 2: Wound(8679), 3: Crippling(3408), 4: Mind-Numbing(5761), 5: Leeching(108211)
        /// </summary>
        private static int NormalizePoisonSetting(int val, bool isMain)
        {
            if (val <= 0)
                return isMain ? SpellBook.DeadlyPoison : SpellBook.CripplingPoison;

            if (val >= 1 && val <= 5)
            {
                switch (val)
                {
                    case 1: return SpellBook.DeadlyPoison;      // 2823
                    case 2: return SpellBook.WoundPoison;       // 8679
                    case 3: return SpellBook.CripplingPoison;   // 3408
                    case 4: return SpellBook.MindNumbingPoison; // 5761
                    case 5: return SpellBook.LeechingPoison;    // 108211
                }
            }
            return val; // déjà un spellId
        }

        private static bool IsLethal(int spellId)
        {
            return spellId == SpellBook.DeadlyPoison || spellId == SpellBook.WoundPoison;
        }

        private static bool IsNonLethal(int spellId)
        {
            return spellId == SpellBook.CripplingPoison
                || spellId == SpellBook.MindNumbingPoison
                || spellId == SpellBook.LeechingPoison;
        }

        private static int ResolveLethal(int a, int b)
        {
            if (IsLethal(a)) return a;
            if (IsLethal(b)) return b;
            return SpellBook.DeadlyPoison; // fallback
        }

        private static int ResolveNonLethal(int a, int b)
        {
            if (IsNonLethal(a)) return a;
            if (IsNonLethal(b)) return b;
            return SpellBook.CripplingPoison; // fallback
        }

        private static bool HasAuraId(WoWUnit u, int spellId)
        {
            if (u == null) return false;
            try
            {
                var auras = u.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i];
                    if (a != null && a.IsActive && a.SpellId == spellId)
                        return true;
                }
            }
            catch { }
            return false;
        }

        private static bool HasAny(WoWUnit u, int[] ids)
        {
            if (u == null) return false;
            for (int i = 0; i < ids.Length; i++)
                if (HasAuraId(u, ids[i])) return true;
            return false;
        }

        /// <summary>
        /// Cast forcé via Lua — évite les faux négatifs OOC.
        /// </summary>
        private static bool CastById(int spellId)
        {
            try
            {
                Lua.DoString("ClearCursor();");
                bool gcd = false;
                try { gcd = SpellManager.GlobalCooldown; } catch { }
                if (gcd) return false;

                int r = Lua.GetReturnVal<int>("CastSpellByID(" + spellId + "); return 1", 0);
                return r == 1;
            }
            catch { return false; }
        }
    }
}
