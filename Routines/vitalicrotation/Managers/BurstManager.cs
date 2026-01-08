using System;
using Styx;
using Styx.CommonBot; // SpellManager here
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class BurstManager
    {
        private static DateTime _lastBurstExecution = DateTime.MinValue;
        private static DateTime _combatEnteredUtc = DateTime.MinValue;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static WoWUnit Target { get { return Me != null ? Me.CurrentTarget as WoWUnit : null; } }

        private static int RequiredBurstEnergy
        {
            get
            {
                try { return VitalicSettings.Instance.BurstEnergy; } catch { return 90; }
            }
        }

        private static bool ShouldExecuteBurst
        {
            get
            {
                var me = Me; var t = Target; if (me == null || t == null || !t.IsValid || !t.IsAlive) return false;
                if (!ToggleState.IsBurstOn) return false;
                if (SpellManager.GlobalCooldown) return false;
                if (me.IsCasting || me.IsChanneling) return false;
                if (me.Specialization != WoWSpec.RogueCombat) return false; // Parité plan 2.1 (Combat)
                int energy = 0; try { energy = (int)me.CurrentEnergy; } catch { }
                return energy >= RequiredBurstEnergy;
            }
        }

        private static bool ShouldExecuteBurstWithKidney
        {
            get
            {
                var me = Me; var t = Target; if (me == null || t == null || !t.IsValid || !t.IsAlive) return false;
                if (!ToggleState.IsBurstOn) return false;
                if (me.Specialization != WoWSpec.RogueCombat) return false;
                // Fenêtre de préparation: autorise burst anticipé avec stun DR favorable
                int prep = 0; try { prep = VitalicSettings.Instance.BurstPreparation; } catch { }
                if (prep <= 0) return false;
                double since = GetTimeSinceCombatStarted();
                if (since > prep) return false;
                // DR favorable pour Stun
                try { return DRTracker.CanApplyStun(t, VitalicSettings.Instance.BurstStunDR); } catch { return false; }
            }
        }

        public static Composite Build()
        {
            return new Action(delegate
            {
                try { Execute(); } catch { }
                return RunStatus.Failure;
            });
        }

        public static void Execute()
        {
            try
            {
                var me = Me; var t = Target; if (me == null || t == null || !t.IsValid || !t.IsAlive) { TrackCombat(); return; }
                TrackCombat();

                if (ShouldExecuteBurstWithKidney)
                    ExecuteBurstWithKidney();
                else if (ShouldExecuteBurst)
                    ExecuteNormalBurst();
            }
            catch { }
        }

        private static void ExecuteBurstWithKidney()
        {
            var t = Target; if (t == null || !t.IsValid) return;
            // Redirect+Kidney si possible, puis enchaîner la séquence offensive
            try { TeamCCManager.TryKidney(t, VitalicSettings.Instance.KidneyShotCPs, true); } catch { }
            TriggerOffensiveCooldowns();
            MarkBurstUsed();
        }

        private static void ExecuteNormalBurst()
        {
            TriggerOffensiveCooldowns();
            MarkBurstUsed();
        }

        private static void TriggerOffensiveCooldowns()
        {
            var me = Me; var t = Target; if (me == null || t == null || !t.IsValid) return;

            // Séquençage Combat: Shadow Blades > Adrenaline Rush > Killing Spree
            // 1) Shadow Blades (si pas de NoBlades)
            if (!ToggleState.IsNoShadowBlades)
            {
                try
                {
                    if (SpellBook.CanCast(SpellBook.ShadowBlades))
                    {
                        if (SpellBook.Cast(SpellBook.ShadowBlades))
                        {
                            Logger.Write("[Burst] Shadow Blades");
                        }
                    }
                }
                catch { }
            }

            // 2) Adrenaline Rush — retarder si pas Deep Insight, sauf fenêtre autorisée
            bool deep = HasAura(me, 84747); // Deep Insight
            bool allowAr = deep;
            try
            {
                if (!allowAr)
                {
                    // Exceptions autorisées: fenêtre d'opener (<= BurstPreparation) ou énergie très élevée
                    double since = GetTimeSinceCombatStarted();
                    int prep = 0; try { prep = VitalicSettings.Instance.BurstPreparation; } catch { }
                    int e = (int)me.CurrentEnergy;
                    if (prep > 0 && since <= prep) allowAr = true;
                    else if (e >= RequiredBurstEnergy) allowAr = true;
                }
            }
            catch { }
            if (allowAr)
            {
                try
                {
                    if (SpellBook.CanCast(SpellBook.AdrenalineRush))
                    {
                        if (SpellBook.Cast(SpellBook.AdrenalineRush))
                        {
                            Logger.Write("[Burst] Adrenaline Rush");
                        }
                    }
                }
                catch { }
            }

            // 3) Killing Spree — éviter si cible très basse (paramètre BurstHealth)
            int hpGate = 0; try { hpGate = VitalicSettings.Instance.BurstHealth; } catch { }
            bool targetOk = true;
            try { targetOk = hpGate <= 0 || Target.HealthPercent > hpGate; } catch { targetOk = true; }
            if (targetOk)
            {
                try
                {
                    if (SpellBook.CanCast(SpellBook.KillingSpree))
                    {
                        if (SpellBook.Cast(SpellBook.KillingSpree))
                        {
                            Logger.Write("[Burst] Killing Spree");
                        }
                    }
                }
                catch { }
            }
        }

        private static void MarkBurstUsed()
        {
            _lastBurstExecution = DateTime.UtcNow;
        }

        private static double GetTimeSinceCombatStarted()
        {
            TrackCombat();
            if (_combatEnteredUtc == DateTime.MinValue) return 9999.0;
            return (DateTime.UtcNow - _combatEnteredUtc).TotalSeconds;
        }
        private static void TrackCombat()
        {
            var me = Me;
            if (me == null) { _combatEnteredUtc = DateTime.MinValue; return; }
            if (me.Combat)
            {
                if (_combatEnteredUtc == DateTime.MinValue) _combatEnteredUtc = DateTime.UtcNow;
            }
            else
            {
                _combatEnteredUtc = DateTime.MinValue;
            }
        }

        private static bool HasAura(WoWUnit u, int spellId)
        {
            if (u == null || !u.IsValid) return false;
            try
            {
                var auras = u.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null) continue; if (a.SpellId == spellId) return true;
                }
            }
            catch { }
            return false;
        }
    }
}