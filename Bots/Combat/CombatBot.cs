using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Levelbot.Actions.Combat;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;
using Styx.WoWInternals;

namespace Styx.Bot.CustomBots
{
    public class CombatBot : BotBase
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        #region Overrides of BotBase

        public override bool RequiresProfile { get { return false; } }
        public override string Name { get { return "Combat Bot"; } }

        private Composite _root;
        public override Composite Root
        {
            get
            {
                return _root ?? (_root = new PrioritySelector(
                    CreateCombatBehavior()
                    //, CreateFollowBehavior()
                ));
            }
        }

        public override void Initialize()
        {
            BotEvents.Player.OnMapChanged += Player_OnMapChanged;
        }

        private void Player_OnMapChanged(BotEvents.Player.MapChangedEventArgs args)
        {
            _root = null;
        }

        public override void Pulse()
        {
            // Optimized: keep Pulse lightweight for max tick rate
            // Uncomment for manual target POI
            // if ((GetAsyncKeyState(System.Windows.Forms.Keys.NumPad0) & 1) != 0)
            //     BotPoi.Current = new BotPoi(StyxWoW.Me.CurrentTarget, PoiType.Kill);
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All & ~(PulseFlags.Looting | PulseFlags.CharacterManager); }
        }

        private bool _oldLogoutForInactivity;
        public override void Start()
        {
            Targeting.Instance.IncludeTargetsFilter += IncludeTargetsFilter;
            _oldLogoutForInactivity = GlobalSettings.Instance.LogoutForInactivity;
            GlobalSettings.Instance.LogoutForInactivity = false;
        }

        public override void Stop()
        {
            Targeting.Instance.IncludeTargetsFilter -= IncludeTargetsFilter;
            GlobalSettings.Instance.LogoutForInactivity = _oldLogoutForInactivity;
        }

        #endregion

        #region Targeting Filter

        private static void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            var me = StyxWoW.Me;
            var target = me.CurrentTarget;
            if (me.GotTarget && target != null && target.Attackable)
            {
                outgoingUnits.Add(target);
            }
        }

        #endregion

        #region Behaviors

        #region Combat Behavior

        private static bool NeedPull(object context)
        {
            var me = StyxWoW.Me;
            var target = me.CurrentTarget;
            if (target == null || !target.InLineOfSight || target.Distance > Targeting.PullDistance)
                return false;
            return true;
        }

        private static Composite CreateCombatBehavior()
        {
            // Cache for faster access
            return new PrioritySelector(
                // Out of combat: Rest, Buffs, Pull
                new Decorator(ret => !StyxWoW.Me.Combat,
                    new PrioritySelector(
                        // Rest
                        new Decorator(ctx =>
                            RoutineManager.Current.RestBehavior != null && RoutineManager.Current.NeedRest,
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Resting"),
                                RoutineManager.Current.RestBehavior
                            )
                        ),
                        // PreCombatBuffs
                        new Decorator(ctx =>
                            RoutineManager.Current.PreCombatBuffBehavior != null && RoutineManager.Current.NeedPreCombatBuffs,
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Applying pre-combat buffs"),
                                RoutineManager.Current.PreCombatBuffBehavior
                            )
                        ),
                        // Pull
                        new Decorator(ret => BotPoi.Current.Type == PoiType.Kill && NeedPull(null),
                            new PrioritySelector(
                                new Decorator(ctx => RoutineManager.Current.PullBuffBehavior != null,
                                    RoutineManager.Current.PullBuffBehavior
                                ),
                                new Decorator(ctx => RoutineManager.Current.PullBehavior != null,
                                    RoutineManager.Current.PullBehavior
                                ),
                                new ActionPull()
                            )
                        )
                    )
                ),
                // In combat: Heal, Buffs, Combat
                new Decorator(ret => StyxWoW.Me.Combat,
                    new PrioritySelector(
                        // Heal (only if needed)
                        new Decorator(ctx =>
                            RoutineManager.Current.HealBehavior != null && RoutineManager.Current.NeedHeal,
                            new Sequence(
                                RoutineManager.Current.HealBehavior,
                                new Action(delegate { return RunStatus.Success; })
                            )
                        ),
                        // Combat Buffs (only if needed)
                        new Decorator(ctx =>
                            RoutineManager.Current.CombatBuffBehavior != null && RoutineManager.Current.NeedCombatBuffs,
                            new Sequence(
                                RoutineManager.Current.CombatBuffBehavior,
                                new Action(delegate { return RunStatus.Success; })
                            )
                        ),
                        // Main Combat
                        new Decorator(ctx => RoutineManager.Current.CombatBehavior != null,
                            new PrioritySelector(
                                RoutineManager.Current.CombatBehavior,
                                new Action(delegate { return RunStatus.Success; })
                            )
                        ),
                        new Sequence(
                            new Action(ret => TreeRoot.StatusText = "Combat"),
                            new Action(ret => RoutineManager.Current.Combat())
                        )
                    )
                )
            );
        }

        #endregion

        #region Follow Behavior

        private static WoWUnit _followMe;
        private static bool _isInitialized;

        private static WoWUnit FollowMe
        {
            get
            {
                if (!_isInitialized && _followMe != null)
                {
                    _followMe.OnInvalidate += _followMe_OnInvalidate;
                    _isInitialized = true;
                }

                if (_followMe == null || !_followMe.IsValid)
                {
                    var me = StyxWoW.Me;
                    if (me.IsInInstance)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            string role = Lua.GetReturnVal<string>(
                                string.Format("return UnitGroupRolesAssigned('party{0}')", i), 0);
                            if (role == "TANK")
                            {
                                _followMe = ObjectManager.GetObjectByGuid<WoWPlayer>(
                                    me.GetPartyMemberGuid(i - 1));
                                break;
                            }
                        }
                    }
                    else
                    {
                        _followMe = RaFHelper.Leader ?? me.PartyMembers.FirstOrDefault();
                    }
                    if (_followMe != null)
                        RaFHelper.SetLeader(_followMe.Guid);
                }

                if (_followMe != null && RaFHelper.Leader != null && _followMe.Guid != RaFHelper.Leader.Guid)
                    _followMe = RaFHelper.Leader;

                if (_followMe == null)
                    Logging.Write("Could not find suitable unit to follow!");

                return _followMe;
            }
        }

        private static void _followMe_OnInvalidate()
        {
            _followMe = null;
        }

        private static Composite CreateFollowBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.GroupInfo.IsInParty &&
                                    FollowMe != null &&
                                    (FollowMe.Distance > 10 || !FollowMe.InLineOfSight),
                    new Action(ret => Navigator.MoveTo(FollowMe.Location))
                )
            );
        }

        #endregion

        #endregion
    }
}
