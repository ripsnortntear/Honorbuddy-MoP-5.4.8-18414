// Behavior originally contributed by mastahg, heavily modified by mjj23 and optimized for Azyul profiles suite.
#pragma warning disable 0618, 0672

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using Action = Styx.TreeSharp.Action;

namespace Honorbuddy.Quest_Behaviors.Azyul
{
    [CustomBehaviorFileName(@"SwitchTarget")]
    [CustomBehaviorFileName(@"EncounterUtility")]
    [CustomBehaviorFileName(@"RangePullBoss")]
    [CustomBehaviorFileName(@"KelThuzad")]
    [CustomBehaviorFileName(@"Thaddeus")]
    [CustomBehaviorFileName(@"KaelThas")]
    public class SwitchTarget : CustomForcedBehavior
    {
        public SwitchTarget(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                Location = GetAttributeAsNullable<WoWPoint>("", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                AuraId = GetAttributeAsNullable<int>("AuraId", false, ConstrainAs.MobId, new[] { "NpcId", "NpcID" }) ?? 0;
                BossId = GetAttributeAsNullable<int>("BossId", false, ConstrainAs.MobId, new[] { "NpcId", "NpcID" }) ?? 0;
                AddIds = GetNumberedAttributesAsArray<int>("AddId", 0, ConstrainAs.MobId, new[] { "NpcId" }) ?? new int[0];
                FollowMobIds = GetNumberedAttributesAsArray<int>("FollowMobId", 0, ConstrainAs.MobId, null) ?? new int[0];
            }
            catch (Exception except)
            {
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message + "\n" + except.StackTrace);
                IsAttributeProblem = true;
            }
        }

        public WoWPoint Location { get; private set; }
        private int[] AddIds { get; set; }
        private int[] FollowMobIds { get; set; }
        private int BossId { get; set; }
        private int AuraId { get; set; }

        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;
        private readonly Stopwatch _runTimer = Stopwatch.StartNew();

        private LocalPlayer Me { get { return StyxWoW.Me; } }

        private WoWUnit Adds
        {
            get
            {
                if (AddIds == null || AddIds.Length == 0) return null;
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                    .Where(u => AddIds.Contains((int)u.Entry) && !u.IsDead && !u.IsFriendly)
                    .OrderBy(u => u.Distance)
                    .FirstOrDefault();
            }
        }

        private WoWUnit Boss
        {
            get
            {
                if (BossId == 0) return null;
                return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == BossId);
            }
        }

        private WoWUnit FollowTarget
        {
            get
            {
                if (FollowMobIds == null || FollowMobIds.Length == 0) return null;
                return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => FollowMobIds.Contains((int)u.Entry) && !u.IsDead);
            }
        }

        ~SwitchTarget()
        {
            Dispose(false);
        }

        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;
                base.Dispose();
            }
            _isDisposed = true;
        }

        public bool IsMobDead()
        {
            // If there are still active adds alive, encounter is not finished
            if (Adds != null)
                return false;

            if (BossId != 0)
            {
                var livingBoss = ObjectManager.GetObjectsOfType<WoWUnit>()
                    .FirstOrDefault(u => u.Entry == BossId && !u.IsDead && !u.IsFriendly);
                if (livingBoss != null)
                    return false;

                // Boss was found and is dead
                var deadBoss = ObjectManager.GetObjectsOfType<WoWUnit>()
                    .FirstOrDefault(u => u.Entry == BossId && u.IsDead);
                if (deadBoss != null)
                    return true;

                // If no living adds and no boss is visible in the area for > 5 seconds, assume finished or already dead
                if (_runTimer.ElapsedMilliseconds > 5000)
                    return true;

                return false;
            }

            return Adds == null;
        }

        public Composite DoneYet
        {
            get
            {
                return new Decorator(ret => IsMobDead(), new Action(delegate
                {
                    TreeRoot.StatusText = "Finished encounter!";
                    _isBehaviorDone = true;
                    return RunStatus.Success;
                }));
            }
        }

        public Composite DoDps
        {
            get
            {
                return new PrioritySelector(
                    RoutineManager.Current.CombatBuffBehavior,
                    RoutineManager.Current.CombatBehavior);
            }
        }

        public Composite KillAdds
        {
            get
            {
                return new Decorator(r => Adds != null && (Boss == null || !Boss.Attackable || (AuraId > 0 && Boss.HasAura(AuraId))),
                    new PrioritySelector(
                        new Decorator(r => !Me.GotTarget || Me.CurrentTarget != Adds, new Action(r => Adds.Target())),
                        DoDps
                    ));
            }
        }

        public Composite KillBoss
        {
            get
            {
                return new Decorator(r => Boss != null && Boss.Attackable && (AuraId == 0 || !Boss.HasAura(AuraId)),
                    new PrioritySelector(
                        new Decorator(r => !Me.GotTarget || Me.CurrentTarget != Boss, new Action(r => Boss.Target())),
                        DoDps
                    ));
            }
        }

        public Composite FollowMob
        {
            get
            {
                return new Decorator(r => FollowTarget != null && FollowTarget.Distance > 10,
                    new Action(r => Navigator.MoveTo(FollowTarget.Location)));
            }
        }

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone,
                new PrioritySelector(DoneYet, KillAdds, KillBoss, FollowMob)));
        }

        [Obsolete("Use OnFinished instead.")]
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override bool IsDone
        {
            get { return _isBehaviorDone; }
        }
    }
}
