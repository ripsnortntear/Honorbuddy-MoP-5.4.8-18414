using Bots.Grind;
using CommonBehaviors.Actions;
using Levelbot.Actions.Death;
using Levelbot.Decorators.Death;
using Styx;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using System; // Added for DateTime

namespace Templar.Helpers
{
    public class Composites
    {
        // Timer for periodic bag checks (to avoid constant scanning)
        private static DateTime _lastBagCheck = DateTime.MinValue;
        private const int BagCheckIntervalMinutes = 5; // Check bags every 5 minutes

        // ===========================================================
        // Methods
        // ===========================================================

        public static Composite CreateRoot()
        {
            return new PrioritySelector(
                DeathRoutine(),
                PreCombatRoutine(),
                PullRoutine(),
                CombatRoutine()
            );
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static Composite DeathRoutine()
        {
            return new Decorator(ctx => StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost,
                new PrioritySelector(
                    new DecoratorNeedToRelease(new ActionReleaseFromCorpse()),
                    new DecoratorNeedToMoveToCorpse(LevelBot.CreateDeathBehavior()),
                    new DecoratorNeedToTakeCorpse(LevelBot.CreateDeathBehavior()),
                    new ActionSuceedIfDeadOrGhost()
                )
            );
        }

        private static Composite PreCombatRoutine()
        {
            return new Decorator(ctx => !StyxWoW.Me.Combat && !StyxWoW.Me.IsActuallyInCombat,
                new PrioritySelector(
                    // Existing rest and buff behaviors
                    new Sequence(
                        RoutineManager.Current.RestBehavior,
                        new ActionAlwaysSucceed()
                    ),
                    new Sequence(
                        RoutineManager.Current.PreCombatBuffBehavior,
                        new ActionAlwaysSucceed()
                    ),
                    // New: Periodic bag check for mailing (low priority, runs if time has passed)
                    new Decorator(ctx => (DateTime.Now - _lastBagCheck).TotalMinutes >= BagCheckIntervalMinutes,
                        new Action(ctx =>
                        {
                            Mail.CheckBags();
                            _lastBagCheck = DateTime.Now;
                            return RunStatus.Success; // Allow tree to continue
                        })
                    ),
                    // New: Mail handling (only if enabled and items to mail)
                    new Decorator(ctx => MailSettings.Instance.Mail && Variables.MailList.Count > 0,
                        new Sequence(
                            new Action(ctx => Mail.HandleMailing()),
                            new ActionAlwaysSucceed() // Yield control back to tree after handling
                        )
                    )
                )
            );
        }

        public static Composite PullRoutine()
        {
            return new Decorator(ctx => !StyxWoW.Me.IsFlying,
                new Decorator(ctx => StyxWoW.Me.CurrentTarget != null && Variables.NextMob != null && StyxWoW.Me.CurrentTarget == Variables.NextMob && PriorityTreeState.TreeState == PriorityTreeState.State.Pulling && Variables.NeedToPull,
                    new Sequence(
                        RoutineManager.Current.PullBehavior,
                        new ActionAlwaysFail()
                    )
                )
            );
        }

        private static Composite CombatRoutine()
        {
            return new Decorator(ctx => StyxWoW.Me.Combat,
                new PrioritySelector(
                    new Sequence(
                        RoutineManager.Current.HealBehavior,
                        new ActionAlwaysFail()
                    ),
                    new Sequence(
                        RoutineManager.Current.CombatBuffBehavior,
                        new ActionAlwaysFail()
                    ),
                    new Sequence(
                        RoutineManager.Current.CombatBehavior,
                        new ActionAlwaysFail()
                    )
                )
            );
        }
    }
}
