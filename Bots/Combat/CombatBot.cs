using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
using Styx.WoWInternals;

namespace Styx.Bot.CustomBots
{
    public class CombatBot : BotBase
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public override bool RequiresProfile
        {
            get { return false; }
        }

        public override string Name
        {
            get { return "Combat Bot"; }
        }

        private Composite _root;

        public override Composite Root
        {
            get
            {
                return _root ?? (_root = new PrioritySelector(
                    CreateCombatBehavior()
                    // Uncomment if follow behavior is needed
                    // , CreateFollowBehavior()
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
            // Example usage of GetAsyncKeyState if needed:
            // if ((GetAsyncKeyState(Keys.NumPad0) & 1) != 0)
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

            // Load an empty profile if needed
            // ProfileManager.LoadEmpty();
        }

        public override void Stop()
        {
            Targeting.Instance.IncludeTargetsFilter -= IncludeTargetsFilter;
            GlobalSettings.Instance.LogoutForInactivity = _oldLogoutForInactivity;
        }

        private static void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            var me = StyxWoW.Me;
            var target = me.CurrentTarget;
            if (me.GotTarget && target != null && target.Attackable)
            {
                outgoingUnits.Add(target);
            }
        }

        private static bool NeedPull(object context)
        {
            var target = StyxWoW.Me.CurrentTarget;

            return target != null
                   && target.InLineOfSight
                   && target.Distance <= Targeting.PullDistance;
        }

        private static Composite CreateCombatBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => !StyxWoW.Me.Combat,
                    new PrioritySelector(
                        // Rest
                        new PrioritySelector(
                            new Decorator(ctx => RoutineManager.Current.RestBehavior != null,
                                RoutineManager.Current.RestBehavior),
                            new Decorator(ctx => RoutineManager.Current.NeedRest,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Resting"),
                                    new Action(ret => RoutineManager.Current.Rest())
                                ))
                        ),

                        // PreCombatBuffs
                        new PrioritySelector(
                            new Decorator(ctx => RoutineManager.Current.PreCombatBuffBehavior != null,
                                RoutineManager.Current.PreCombatBuffBehavior),
                            new Decorator(ctx => RoutineManager.Current.NeedPreCombatBuffs,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Applying pre-combat buffs"),
                                    new Action(ret => RoutineManager.Current.PreCombatBuff())
                                ))
                        ),

                        // Pull
                        new Decorator(ret => BotPoi.Current.Type == PoiType.Kill,
                            new PrioritySelector(
                                new Decorator(ret => Targeting.Instance.TargetList.Count != 0,
                                    new Decorator(ret => BotPoi.Current.AsObject != Targeting.Instance.FirstUnit,
                                        new Sequence(
                                            new Action(ret => BotPoi.Current = new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill)),
                                            new Action(ret => BotPoi.Current.AsObject.ToUnit().Target())
                                        ))),
                                new Decorator(NeedPull,
                                    new PrioritySelector(
                                        new Decorator(ctx => RoutineManager.Current.PullBuffBehavior != null,
                                            RoutineManager.Current.PullBuffBehavior),
                                        new Decorator(ctx => RoutineManager.Current.PullBehavior != null,
                                            RoutineManager.Current.PullBehavior),
                                        new ActionPull()
                                    ))
                            ))
                    )),

                new Decorator(ret => StyxWoW.Me.Combat,
                    new PrioritySelector(
                        // Heal
                        new PrioritySelector(
                            new Decorator(ctx => RoutineManager.Current.HealBehavior != null,
                                new Sequence(
                                    RoutineManager.Current.HealBehavior,
                                    new Action(ret => RunStatus.Success)
                                )),
                            new Decorator(ctx => RoutineManager.Current.NeedHeal,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Healing"),
                                    new Action(ret => RoutineManager.Current.Heal())
                                ))
                        ),

                        // Combat Buffs
                        new PrioritySelector(
                            new Decorator(ctx => RoutineManager.Current.CombatBuffBehavior != null,
                                new Sequence(
                                    RoutineManager.Current.CombatBuffBehavior,
                                    new Action(ret => RunStatus.Success)
                                )),
                            new Decorator(ctx => RoutineManager.Current.NeedCombatBuffs,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Applying Combat Buffs"),
                                    new Action(ret => RoutineManager.Current.CombatBuff())
                                ))
                        ),

                        // Combat
                        new PrioritySelector(
                            new Decorator(ctx => RoutineManager.Current.CombatBehavior != null,
                                new PrioritySelector(
                                    RoutineManager.Current.CombatBehavior,
                                    new Action(ret => RunStatus.Success)
                                )),
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Combat"),
                                new Action(ret => RoutineManager.Current.Combat())
                            )
                        )
                    ))
            );
        }

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
                    if (StyxWoW.Me.IsInInstance)
                    {
                        for (var i = 1; i < 5; i++)
                        {
                            var role = Lua.GetReturnVal<string>(string.Format("return UnitGroupRolesAssigned('party{0}')", i), 0);
                            if (role == "TANK")
                            {
                                _followMe = ObjectManager.GetObjectByGuid<WoWPlayer>(StyxWoW.Me.GetPartyMemberGuid(i - 1));
                                break;
                            }
                        }
                    }
                    else
                    {
                        _followMe = RaFHelper.Leader ?? StyxWoW.Me.PartyMembers.FirstOrDefault();
                    }
                    if (_followMe != null)
                        RaFHelper.SetLeader(_followMe.Guid);
                }

                if (!((_followMe != null && RaFHelper.Leader != null && _followMe.Guid == RaFHelper.Leader.Guid) || (_followMe == null && RaFHelper.Leader == null)))
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
                                    (FollowMe != null && (FollowMe.Distance > 10 || !FollowMe.InLineOfSight)),
                    new Action(ret => Navigator.MoveTo(FollowMe.Location))
                )
            );
        }

        #endregion
    }
}
