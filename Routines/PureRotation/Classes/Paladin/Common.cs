using System;
using System.Linq;
using PureRotation.Core;
using PureRotation.Managers;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Paladin
{
    internal static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Inquisition

        internal static bool HasInq
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura inquisition = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 84963);
                return inquisition != null && inquisition.TimeLeft <= TimeSpan.FromSeconds(2);
            }
        }

        #endregion Inquisition

        #region Misc

        internal static bool HasAncientP
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura ancientPower = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 86700);
                return ancientPower != null && ancientPower.StackCount >= 10;
            }
        }

        internal static WoWUnit BestHolyPrismTarget
        {
            get
            {
                if (Me.CurrentTarget == null) return null;
                WoWPlayer bestTarget = null;
                    bestTarget = Unit.GroupMembers.Where(a => Unit.NearbyAttackableUnits(a.Location, 15).Count(t => t.DistanceSqr < 12 * 12) > 2).OrderBy(a => a.Location.DistanceSqr(Me.Location) < 30f).FirstOrDefault();
                    if (bestTarget == null) bestTarget = Me;
                    return bestTarget != null ? bestTarget.CurrentTarget : null;
            }
        }

        internal static WoWUnit BestDjTarget
        {
            get
            {
                if (Me.CurrentTarget == null) return null;
                if (!TalentManager.HasGlyph("Double Jeopardy")) return Me.CurrentTarget;
                if (TalentManager.HasGlyph("Double Jeopardy") && !Me.HasAura(55003)) return Me.CurrentTarget;
                var bestTarget = Unit.AttackableMeleeUnits.Where(a => a.IsAlive).OrderBy(a => a.Location.DistanceSqr(Me.Location) < 25).FirstOrDefault();
                return bestTarget;
            }
        }

        internal static uint CurrentTargetEntry
        {
            get
            {
                return Me.CurrentTarget != null ? Me.CurrentTarget.Entry : 0;
            }
        }

        #endregion Misc
    }
}