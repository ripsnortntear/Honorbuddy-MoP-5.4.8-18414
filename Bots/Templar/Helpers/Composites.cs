using Bots.Grind;
using CommonBehaviors.Actions;
using Levelbot.Actions.Death;
using Levelbot.Decorators.Death;
using Styx;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;

namespace Templar.Helpers {
    public class Composites {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static Composite CreateRoot() {
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

        private static Composite DeathRoutine() {
            return new Decorator(ctx => StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost,
                new PrioritySelector(
                    new DecoratorNeedToRelease(new ActionReleaseFromCorpse()),
                    new DecoratorNeedToMoveToCorpse(LevelBot.CreateDeathBehavior()),
                    new DecoratorNeedToTakeCorpse(LevelBot.CreateDeathBehavior()),
                    new ActionSuceedIfDeadOrGhost()
                )
            );
        }

        private static Composite PreCombatRoutine() {
            System.Collections.Generic.List<Composite> children = new System.Collections.Generic.List<Composite>();

            if (RoutineManager.Current != null) {
                if (RoutineManager.Current.RestBehavior != null) {
                    children.Add(new Decorator(ctx => RoutineManager.Current.NeedRest,
                        new Sequence(
                            RoutineManager.Current.RestBehavior,
                            new ActionAlwaysSucceed()
                        )
                    ));
                }

                if (RoutineManager.Current.PreCombatBuffBehavior != null) {
                    children.Add(new Decorator(ctx => RoutineManager.Current.NeedPreCombatBuffs,
                        new Sequence(
                            RoutineManager.Current.PreCombatBuffBehavior,
                            new ActionAlwaysSucceed()
                        )
                    ));
                }
            }

            return new Decorator(ctx => !StyxWoW.Me.IsDead && !StyxWoW.Me.IsGhost && !StyxWoW.Me.Combat && !StyxWoW.Me.IsActuallyInCombat,
                new PrioritySelector(children.ToArray())
            );
        }

        public static Composite PullRoutine() {
            if (RoutineManager.Current == null || RoutineManager.Current.PullBehavior == null) {
                return new ActionAlwaysFail();
            }

            return new Decorator(ctx => !StyxWoW.Me.IsDead && !StyxWoW.Me.IsGhost && !StyxWoW.Me.IsFlying,
                new Decorator(ctx => StyxWoW.Me.CurrentTarget != null && Variables.NextMob != null && StyxWoW.Me.CurrentTarget == Variables.NextMob && PriorityTreeState.TreeState == PriorityTreeState.State.Pulling && Variables.NeedToPull,
                    new Sequence(
                        RoutineManager.Current.PullBehavior,
                        new ActionAlwaysFail()
                    )
                )
            );
        }

        private static Composite CombatRoutine() {
            System.Collections.Generic.List<Composite> children = new System.Collections.Generic.List<Composite>();

            if (RoutineManager.Current != null) {
                if (RoutineManager.Current.HealBehavior != null) {
                    children.Add(new Sequence(
                        RoutineManager.Current.HealBehavior,
                        new ActionAlwaysFail()
                    ));
                }

                if (RoutineManager.Current.CombatBuffBehavior != null) {
                    children.Add(new Sequence(
                        RoutineManager.Current.CombatBuffBehavior,
                        new ActionAlwaysFail()
                    ));
                }

                if (RoutineManager.Current.CombatBehavior != null) {
                    children.Add(new Sequence(
                        RoutineManager.Current.CombatBehavior,
                        new ActionAlwaysFail()
                    ));
                }
            }

            return new Decorator(ctx => !StyxWoW.Me.IsDead && !StyxWoW.Me.IsGhost && StyxWoW.Me.Combat,
                new PrioritySelector(children.ToArray())
            );
        }
    }
}
