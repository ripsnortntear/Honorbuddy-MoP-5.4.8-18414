using Bots.Grind;
using CommonBehaviors.Actions;
using Levelbot.Actions.Death;
using Levelbot.Decorators.Death;
using Styx;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using System; // For DateTime
using Templar.GUI.Tabs; // For MailSettings and VendorSettings
namespace Templar.Helpers
    {
        public class Composites
        {
            private static DateTime _lastBagCheck = DateTime.MinValue;
            private
            const int BagCheckIntervalMinutes = 5;
            public static Composite CreateRoot()
            {
                return new PrioritySelector(DeathRoutine(), PreCombatRoutine(), PullRoutine(), CombatRoutine());
            }
            private static Composite DeathRoutine()
            {
                return new Decorator(ctx => StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost, new PrioritySelector(new DecoratorNeedToRelease(new ActionReleaseFromCorpse()), new DecoratorNeedToMoveToCorpse(LevelBot.CreateDeathBehavior()), new DecoratorNeedToTakeCorpse(LevelBot.CreateDeathBehavior()), new ActionSuceedIfDeadOrGhost()));
            }
            private static Composite PreCombatRoutine()
            {
                return new Decorator(ctx => !StyxWoW.Me.Combat && !StyxWoW.Me.IsActuallyInCombat, new PrioritySelector(new Sequence(RoutineManager.Current.RestBehavior, new ActionAlwaysSucceed()), new Sequence(RoutineManager.Current.PreCombatBuffBehavior, new ActionAlwaysSucceed()), new Decorator(ctx => (DateTime.Now - _lastBagCheck).TotalMinutes >= BagCheckIntervalMinutes, new Styx.TreeSharp.Action(ctx =>
                {
                    Mail.CheckBags();
                    Vendor.CheckBags();
                    _lastBagCheck = DateTime.Now;
                    return RunStatus.Success;
                })), new Decorator(ctx => MailSettings.Instance.Mail && Variables.MailList.Count > 0, new Sequence(new Styx.TreeSharp.Action(ctx => Mail.HandleMailing()), new ActionAlwaysSucceed())), new Decorator(ctx => VendorSettings.Instance.Vendor && Variables.NeedToVendor, new Sequence(new Styx.TreeSharp.Action(ctx => Vendor.HandleVendoring()), new ActionAlwaysSucceed()))));
            }
            public static Composite PullRoutine()
            {
                return new Decorator(ctx => !StyxWoW.Me.IsFlying, new Decorator(ctx => StyxWoW.Me.CurrentTarget != null && Variables.NextMob != null && StyxWoW.Me.CurrentTarget == Variables.NextMob && PriorityTreeState.TreeState == PriorityTreeState.State.Pulling && Variables.NeedToPull, new Sequence(RoutineManager.Current.PullBehavior, new ActionAlwaysFail())));
            }
            private static Composite CombatRoutine()
            {
                return new Decorator(ctx => StyxWoW.Me.Combat, new PrioritySelector(new Sequence(RoutineManager.Current.HealBehavior, new ActionAlwaysFail()), new Sequence(RoutineManager.Current.CombatBuffBehavior, new ActionAlwaysFail()), new Sequence(RoutineManager.Current.CombatBehavior, new ActionAlwaysFail())));
            }
        }
	}
