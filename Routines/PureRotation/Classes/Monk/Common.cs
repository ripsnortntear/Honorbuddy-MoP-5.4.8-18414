using System.Linq;
using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Classes.Monk
{
    internal static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Tank

        internal static bool CanApplyShuffle { get { return (!Me.HasAura(115307) || Me.HasAura(115307, 0, false, PRSettings.Instance.Monk.BrewShuffleRefreshMilliseconds)); } }

        internal static bool CanUsePurifyingBrew { get { return (Me.HasAura("Moderate Stagger") && Me.HasAura(115307, 0, false, 9000) || (Me.HasAura(115307, 0, false, PRSettings.Instance.Monk.BrewShuffleRefreshMilliseconds) && Me.HasAura("Heavy Stagger"))); } }

        internal static bool CanApplyDizzyingHaze { get { return Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 8).Any(x => !x.HasAura("Dizzying Haze") && !x.IsBoss() && !x.IsFlying) && Me.HasAura("Shuffle"); } }

        internal static bool CanApplyBreathofFire { get { return Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 12).Any(x => !x.HasAura("Breath of Fire") && Me.IsSafelyFacing(x)); } }

        internal static bool CanPlaceBlackOxStatue { get { return PRSettings.Instance.Monk.BrewEnableSanctuaryoftheOx && Me.CurrentTarget != null && !Me.HasAura("Sanctuary of the Ox") && !Me.CurrentTarget.IsFlying && !Me.IsOnTransport; } }

        internal static Composite ClearDizzyingHaze()
        {
            return new Decorator(ret => StyxWoW.Me.HasPendingSpell("Dizzying Haze") && CanApplyDizzyingHaze, new Action(ret => Lua.DoString("SpellStopTargeting()")));
        }

        #endregion Tank

        #region integers

        public static uint Chi { get { return (uint)Helpers.Lua.PlayerUnitPower("SPELL_POWER_CHI"); } } //Me.CurrentChi <-- FIX THIS SHIT HB DEVS -- wulf.

        public static int MaxChi { get { return TalentManager.HasTalent(8) ? 5 : 4; } } // Me.MaxChi <-- this is slow too --wulf.

        #endregion

        #region Misc

        internal static bool UnitIsFleeing { get { return Me.CurrentTarget != null && ((Me.CurrentTarget.IsPlayer || Me.CurrentTarget.Fleeing) && Me.CurrentTarget.MovementInfo.RunSpeed > 3.5); } }

        #endregion Misc
    }
}