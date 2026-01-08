using Styx;
using Styx.TreeSharp;
using Styx.CommonBot; // for SpellManager.GlobalCooldown
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class PreparationManager
    {
        private const int MicroThrottleMs = 200;
        private const string ThrottleKey = "Preparation.Cast";

        // IDs MoP confirmés par v.zip (Class77.hashSet_0)
        private const int Vanish = SpellBook.Vanish;        // 1856
        private const int Evasion = SpellBook.Evasion;      // 5277
        private const int Dismantle = SpellBook.Dismantle;  // 51722
        private const int Preparation = SpellBook.Preparation; // 14185

        public static Composite Build()
        {
            return new Action(delegate
            {
                Execute();
                return RunStatus.Failure; // important : ne pas « manger » le tick
            });
        }

        public static void Execute()
        {
            try
            {
                var S = VitalicSettings.Instance;
                if (!S.AutoPreparation)
                    return;

                var me = StyxWoW.Me;
                if (me == null || !me.IsAlive)
                    return;

                // Micro-throttle dans l'esprit v.zip
                if (!Throttle.Check(ThrottleKey, MicroThrottleMs))
                    return;

                // Pas pendant GCD / cast / channel
                if (SpellManager.GlobalCooldown || me.IsCasting || me.IsChanneling)
                    return;

                if (!SpellBook.CanCast(Preparation))
                    return;

                // Parité v.zip: ne pas lancer si les PV sont sous le seuil BurstHealth
                try
                {
                    int hpThresh = VitalicSettings.Instance.BurstHealth;
                    if (hpThresh > 0 && me.HealthPercent <= hpThresh)
                        return;
                }
                catch { }

                // v.zip: utilisera Preparation lorsque Vanish, Evasion et Dismantle sont en cooldown
                bool vanishCd = SecondsLeft(Vanish) > 0;
                bool evasionCd = SecondsLeft(Evasion) > 0;
                bool dismantleCd = SecondsLeft(Dismantle) > 0;
                if (!(vanishCd && evasionCd && dismantleCd))
                    return;

                if (SpellBook.Cast(Preparation))
                {
                    // v.zip : pas de libellé spécifique, on reste sobre
                    UiCompat.Notify("Preparation");
                    // Les caches de CD seront rafraîchis automatiquement (v.zip le fait côté événements sur 14185)
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "PreparationManager");
            }
        }

        // Copie locale (style CooldownManager) : secondes restantes via GetSpellCooldown
        private static double SecondsLeft(int spellId)
        {
            try
            {
                const double fudge = 0.050; // 50ms cooldown fudge (E23)
                string lua =
                    "local s,d,enable=GetSpellCooldown(" + spellId + "); " +
                    "if not s or not d then return 0 end " +
                    "if s==0 or d==0 then return 0 end " +
                    "local now=GetTime(); local rem=(s+d)-now; if rem<0 then rem=0 end; return rem;";
                double rem = Lua.GetReturnVal<double>(lua, 0);
                if (rem > 0 && rem <= fudge)
                {
                    if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    {
                        try { Logger.Write("[Diag][Fudge][Prep] spellId={0} rem={1:0.000}s -> 0", spellId, rem); } catch { }
                    }
                    return 0.0;
                }
                return rem;
            }
            catch { return 0.0; }
        }
    }
}
