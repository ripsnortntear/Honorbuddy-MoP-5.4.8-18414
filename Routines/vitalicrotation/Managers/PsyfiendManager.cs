using System;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using VitalicRotation.Settings;
using VitalicRotation.Helpers;

namespace VitalicRotation.Managers
{
    /// <summary>
    /// Handles automatic reaction to enemy Psyfiend (Priest talent) – destroys it quickly.
    /// Settings used:
    ///  HandlePsyfiend (enable)
    ///  PsyfiendArenasOnly (restrict to arenas/bgs)
    ///  PsyfiendFoKRange (radius for Fan of Knives usage)
    ///  PsyfiendThrottleMs (min interval between actions)
    /// </summary>
    internal static class PsyfiendManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _lastAction = DateTime.MinValue;
        private const int PsyfiendSummonSpellId = 108921; // Summon Psyfiend (MoP)

        public static Composite Build()
        {
            return new Styx.TreeSharp.Action(ret => { Execute(); return Styx.TreeSharp.RunStatus.Failure; });
        }

        public static void Execute()
        {
            try
            {
                var S = VitalicSettings.Instance; if (S == null || !S.HandlePsyfiend) return;
                if (Me == null || !Me.IsAlive) return;
                if (S.PsyfiendArenasOnly && !RuntimeToggles.IsPvpContext()) return;

                int throttle = S.PsyfiendThrottleMs > 0 ? S.PsyfiendThrottleMs : 1200;
                if ((DateTime.UtcNow - _lastAction).TotalMilliseconds < throttle) return;

                WoWUnit psy = FindPsyfiend(S.PsyfiendFoKRange);
                if (psy == null) return;

                // Prefer Fan of Knives if available (cleave, no retarget needed)
                if (SpellBook.CanCast(SpellBook.FanOfKnives))
                {
                    if (SpellBook.Cast(SpellBook.FanOfKnives))
                    {
                        _lastAction = DateTime.UtcNow;
                        Logger.Write("[Psyfiend] Fan of Knives -> {0}", psy.SafeName);
                        return;
                    }
                }

                // Direct target
                if (Me.CurrentTarget == null || Me.CurrentTarget.Guid != psy.Guid)
                {
                    try { psy.Target(); } catch { }
                }

                if (!psy.IsWithinMeleeRange)
                    return; // wait until in range

                if (TryPrimaryBuilder(psy))
                {
                    _lastAction = DateTime.UtcNow;
                    Logger.Write("[Psyfiend] Builder -> {0}", psy.SafeName);
                }
            }
            catch { }
        }

        private static WoWUnit FindPsyfiend(int range)
        {
            if (range <= 0) range = 10;
            try
            {
                var units = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < units.Count; i++)
                {
                    var u = units[i];
                    if (u == null || !u.IsValid || u.IsDead) continue;
                    if (!u.Attackable || u.IsFriendly) continue;
                    double d; try { d = u.Distance; } catch { continue; }
                    if (d > range) continue;

                    bool isPsy = false;
                    try { if (!string.IsNullOrEmpty(u.Name) && u.Name.IndexOf("Psyfiend", StringComparison.OrdinalIgnoreCase) >= 0) isPsy = true; } catch { }
                    if (!isPsy)
                    {
                        try { if ((int)u.CreatedBySpellId == PsyfiendSummonSpellId) isPsy = true; } catch { }
                    }
                    if (!isPsy) continue;
                    return u;
                }
            }
            catch { }
            return null;
        }

        private static bool TryPrimaryBuilder(WoWUnit target)
        {
            if (target == null || !target.IsValid) return false;
            try
            {
                switch (Me.Specialization)
                {
                    case WoWSpec.RogueAssassination:
                        if (SpellBook.CanCast(SpellBook.Mutilate, target)) return SpellBook.Cast(SpellBook.Mutilate, target);
                        break;
                    case WoWSpec.RogueCombat:
                        if (SpellBook.CanCast(SpellBook.SinisterStrike, target)) return SpellBook.Cast(SpellBook.SinisterStrike, target);
                        break;
                    case WoWSpec.RogueSubtlety:
                        if (SpellBook.CanCast(SpellBook.Backstab, target) && Me.IsSafelyBehind(target)) return SpellBook.Cast(SpellBook.Backstab, target);
                        if (SpellBook.CanCast(SpellBook.Hemorrhage, target)) return SpellBook.Cast(SpellBook.Hemorrhage, target);
                        break;
                }
                // Generic fallback
                if (SpellBook.CanCast(SpellBook.Mutilate, target)) return SpellBook.Cast(SpellBook.Mutilate, target);
                if (SpellBook.CanCast(SpellBook.SinisterStrike, target)) return SpellBook.Cast(SpellBook.SinisterStrike, target);
            }
            catch { }
            return false;
        }
    }
}
