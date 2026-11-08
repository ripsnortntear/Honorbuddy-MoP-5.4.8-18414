using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bots.Grind;
using CommonBehaviors.Actions;
using CommonBehaviors.Decorators;
using Levelbot.Actions.Combat;
using Levelbot.Resources;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.AreaManagement;
using Styx.CommonBot.Frames;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace Bots.GrindBuddy
{
    public class GrindBuddy : BotBase
    {
        private Composite _root;
        private readonly object _lootLock = new object();
        private ulong _lastLootGuid;
        private int _lootAttemptCounter;
        private ulong _lootAttemptLastGuid;
        private PoiType _lootAttemptLastType = PoiType.None;

        private static ICombatRoutine Routine
        {
            get { return RoutineManager.Current; }
        }

        public override string Name
        {
            get { return "GrindBuddy"; }
        }

        public override Composite Root
        {
            get { return _root ?? (_root = CreateGrindBotBrain()); }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override bool RequiresProfile
        {
            get { return true; }
        }

        public override bool RequiresProfileScope
        {
            get { return true; }
        }

        private static bool PoiIsLootable
        {
            get
            {
                var poi = BotPoi.Current;
                if (poi == null)
                    return false;

                var obj = poi.AsObject;
                if (obj == null || !obj.IsValid)
                    return false;

                switch (poi.Type)
                {
                    case PoiType.Loot:
                        var unit = obj as WoWUnit;
                        return unit != null && unit.CanLoot;
                    case PoiType.Skin:
                        var skinUnit = obj as WoWUnit;
                        return skinUnit != null && skinUnit.CanSkin;
                    case PoiType.Harvest:
                        var go = obj as WoWGameObject;
                        return go != null && go.CanLoot;
                    default:
                        return false;
                }
            }
        }

        private static bool CleanPoi
        {
            get
            {
                var poi = BotPoi.Current;
                if (poi == null)
                    return true;
                return poi.Type == PoiType.None || poi.Type == PoiType.Hotspot;
            }
        }

        private static WoWObject FirstLoot
        {
            get { return LootTargeting.Instance.FirstObject; }
        }

        private static WoWUnit FirstTarget
        {
            get { return Targeting.Instance.FirstUnit; }
        }

        private static Profile CurrentProfile
        {
            get { return ProfileManager.CurrentProfile; }
        }

        private static IEnumerable<WoWUnit> GetGrindUnits()
        {
            var profile = ProfileManager.CurrentProfile;
            var me = StyxWoW.Me;
            if (profile == null || me == null || !me.IsValid)
                return Enumerable.Empty<WoWUnit>();

            var grindArea = StyxWoW.AreaManager.CurrentGrindArea;
            var grindFactions = grindArea != null ? grindArea.Factions : null;
            var grindMobIds = grindArea != null ? grindArea.MobIDs : null;

            return ObjectManager.GetObjectsOfType<WoWUnit>()
                .Where(ret =>
                    ret != null && ret.IsValid
                    && ret.IsAlive
                    && !ret.IsCritter
                    && !ret.TaggedByOther
                    && !Blacklist.Contains(ret.Guid, BlacklistFlags.Combat | BlacklistFlags.Pull)
                    && (ret.OwnedByUnit == null || !ret.OwnedByUnit.IsPlayer)
                    && IsGrindTarget(ret, profile, grindFactions, grindMobIds))
                .OrderBy(ret => ret.DistanceSqr);
        }

        private static bool IsGrindTarget(WoWUnit unit, Profile profile, IList<int> grindFactions, IList<int> grindMobIds)
        {
            if (profile != null && profile.Factions != null && profile.Factions.Contains(unit.FactionId))
                return true;
            if (grindFactions != null && grindFactions.Contains((int)unit.FactionId))
                return true;
            if (grindMobIds != null && grindMobIds.Contains((int)unit.Entry))
                return true;
            return false;
        }

        private static WoWUnit GetSkinUnit()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>()
                .Where(ret =>
                    ret != null && ret.IsValid
                    && ret.SkinType == WoWCreatureSkinType.Leather
                    && !Blacklist.Contains(ret, BlacklistFlags.Node)
                    && ret.Skinnable
                    && ret.CanSkin
                    && !IsOutsideLootRange(ret))
                .OrderBy(ret => ret.DistanceSqr)
                .FirstOrDefault();
        }

        private static List<WoWUnit> GetAroundUnits()
        {
            var me = StyxWoW.Me;
            if (me == null || !me.IsValid)
                return new List<WoWUnit>();

            return ObjectManager.GetObjectsOfType<WoWUnit>()
                .Where(ret =>
                    ret != null && ret.IsValid
                    && ret.IsAlive
                    && !ret.TaggedByOther
                    && !Blacklist.Contains(ret.Guid, BlacklistFlags.Combat)
                    && Math.Abs(me.Level - ret.Level) <= 3
                    && ret.Distance < 15.0
                    && (ret.OwnedByUnit == null || !ret.OwnedByUnit.IsPlayer)
                    && ret.IsHostile)
                .OrderBy(ret => ret.DistanceSqr)
                .ToList();
        }

        private static Composite CreateCombatBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me != null && !StyxWoW.Me.Combat && Routine != null,
                    new PrioritySelector(
                        Routine.RestBehavior,
                        Routine.PreCombatBuffBehavior,
                        new Decorator(ret => BotPoi.Current != null && BotPoi.Current.Type == PoiType.Kill,
                            new PrioritySelector(
                                new Decorator(ret => !StyxWoW.Me.GotTarget,
                                    new Action(ret =>
                                    {
                                        var target = FirstTarget ?? (BotPoi.Current != null ? BotPoi.Current.AsObject as WoWUnit : null);
                                        if (target != null && target.IsValid)
                                            target.Target();
                                    })),
                                new PrioritySelector(
                                    new Decorator(ret => Routine != null && Routine.MoveToTargetBehavior != null, Routine.MoveToTargetBehavior),
                                    new Decorator(ret => BotPoi.Current != null && BotPoi.Current.Location.Distance(StyxWoW.Me.Location) > Targeting.PullDistance, new ActionMoveToPoi())),
                                Routine.PullBuffBehavior,
                                Routine.PullBehavior)))),
                new Decorator(ret => StyxWoW.Me != null && StyxWoW.Me.Combat && BotPoi.Current != null && BotPoi.Current.Type == PoiType.Kill && Routine != null,
                    new PrioritySelector(
                        Routine.HealBehavior,
                        Routine.CombatBuffBehavior,
                        Routine.CombatBehavior)));
        }

        public override void Initialize()
        {
            BotEvents.Player.OnMapChanged += OnMapChanged;
        }

        private void OnMapChanged(BotEvents.Player.MapChangedEventArgs args)
        {
            _root = null;
        }

        public override void Start()
        {
            _root = CreateGrindBotBrain();
            SetDefaultHooks();
            Targeting.Instance.IncludeTargetsFilter += IncludeTargetsFilter;
            LootTargeting.Instance.IncludeTargetsFilter += IncludeLootsFilter;
        }

        public override void Stop()
        {
            Targeting.Instance.IncludeTargetsFilter -= IncludeTargetsFilter;
            LootTargeting.Instance.IncludeTargetsFilter -= IncludeLootsFilter;
            TreeHooks.Instance.RemoveHook("GrindBuddy_Combat", CreateCombatBehavior());
            _root = null;
            _lootAttemptCounter = 0;
            _lootAttemptLastGuid = 0UL;
            _lootAttemptLastType = PoiType.None;
        }

        private static void SetDefaultHooks()
        {
            // Avoid ClearAll() - preserve other hook consumers; only (re)register our hook.
            var combat = CreateCombatBehavior();
            TreeHooks.Instance.RemoveHook("GrindBuddy_Combat", combat);
            TreeHooks.Instance.AddHook("GrindBuddy_Combat", combat);
        }

        private static void LootAllItems()
        {
            if (!LootTargeting.LootFrameIsOpen)
                return;

            try
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    var frame = LootFrame.Instance;
                    if (frame == null)
                        return;

                    var carriedItems = StyxWoW.Me != null ? StyxWoW.Me.CarriedItems : null;
                    for (int i = 0; i < frame.LootItems; i++)
                    {
                        uint itemId = frame.GetItemId(i);
                        if (carriedItems != null)
                        {
                            foreach (WoWItem item in carriedItems)
                            {
                                if (item != null && item.IsValid && item.Entry == itemId)
                                {
                                    ItemInfo itemInfo = item.ItemInfo;
                                    if (itemInfo != null && (itemInfo.UniqueCount == 1 || itemInfo.BeginQuestId != 0))
                                    {
                                        if (BotPoi.Current != null)
                                            Blacklist.Add(BotPoi.Current.Guid, BlacklistFlags.Loot, TimeSpan.FromHours(3.0), "Looting completed (unique/quest item already owned)");
                                        break;
                                    }
                                }
                            }
                        }

                        frame.Loot(i);
                        Lua.DoString("ConfirmLootSlot({0})", i + 1);
                    }

                    Lua.DoString("CloseLoot();");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[GrindBuddy] LootAllItems failed: {0}", ex.Message);
            }
        }

        private Composite CreateLootBehavior()
        {
            return new DecoratorIsPoiType(new[] { PoiType.Loot, PoiType.Harvest, PoiType.Skin },
                new Sequence(
                    new DecoratorContinue(ret => BotPoi.Current != null && StyxWoW.Me != null && BotPoi.Current.Location.Distance(StyxWoW.Me.Location) > 4f, new ActionMoveToPoi()),
                    new DecoratorContinue(ret => StyxWoW.Me != null && StyxWoW.Me.IsMoving, new Action(ret => Navigator.PlayerMover.MoveStop())),
                    new Action(ret =>
                    {
                        var obj = BotPoi.Current != null ? BotPoi.Current.AsObject : null;
                        if (obj != null && obj.IsValid)
                            obj.Interact();
                    }),
                    new Wait(4, ret => LootTargeting.LootFrameIsOpen || !PoiIsLootable, new Action(ret => LootAllItems())),
                    new DecoratorContinue(ret => NeedToBlacklistCurrentLootable(),
                        new Sequence(
                            new Action(ret =>
                            {
                                if (BotPoi.Current != null)
                                    Blacklist.Add(BotPoi.Current.Guid, BlacklistFlags.Loot, TimeSpan.FromDays(10.0), "Tried to loot unsuccessfully too many times.");
                            }),
                            new ActionClearPoi("Tried to loot too many times. Blacklisting!")))));
        }

        private bool NeedToBlacklistCurrentLootable()
        {
            var poi = BotPoi.Current;
            if (poi == null)
            {
                _lootAttemptCounter = 0;
                _lootAttemptLastGuid = 0UL;
                _lootAttemptLastType = PoiType.None;
                return false;
            }

            if (_lootAttemptLastType != poi.Type || _lootAttemptLastGuid != poi.Guid)
            {
                _lootAttemptCounter = 1;
                _lootAttemptLastGuid = poi.Guid;
                _lootAttemptLastType = poi.Type;
                return false;
            }

            _lootAttemptCounter++;
            return _lootAttemptCounter >= 3;
        }

        private static PoiType GetPoiTypeForLootable(WoWObject obj)
        {
            if (obj is WoWGameObject)
                return PoiType.Harvest;
            var unit = obj as WoWUnit;
            if (unit != null && unit.CanSkin)
                return PoiType.Skin;
            return PoiType.Loot;
        }

        private Composite CreateGrindBotBrain()
        {
            return new PrioritySelector(
                new Decorator(ret =>
                {
                    var poi = BotPoi.Current;
                    if (poi == null || poi.Type != PoiType.Kill)
                        return false;
                    var unit = poi.AsObject as WoWUnit;
                    return unit == null || !unit.IsValid || unit.IsDead;
                },
                    new Sequence(
                        new ActionClearPoi("Dead target with POI type as Kill"),
                        new Action(ret => { if (StyxWoW.Me != null) StyxWoW.Me.ClearTarget(); }),
                        new DecoratorContinue(ret => StyxWoW.Me != null && Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30f) == 0 && CharacterSettings.Instance.LootMobs,
                            new Action(ret =>
                            {
                                Thread.Sleep(300);
                                WaitTimer waitTimer = new WaitTimer(TimeSpan.FromMilliseconds(1500.0));
                                waitTimer.Reset();
                                while (!waitTimer.IsFinished && FirstLoot == null)
                                    Thread.Sleep(100);
                            })))),

                new DecoratorIsPoiType(new[] { PoiType.Loot, PoiType.Harvest, PoiType.Skin },
                    new Decorator(ret =>
                    {
                        var skinUnit = GetSkinUnit();
                        var firstLoot = FirstLoot;
                        var poi = BotPoi.Current;
                        return ((skinUnit == null && LootTargeting.SkinMobs) || !LootTargeting.SkinMobs)
                            && (firstLoot == null || poi == null || firstLoot.Guid != poi.Guid || !PoiIsLootable);
                    },
                        new ActionClearPoi("Finished looting!"))),

                new DecoratorIsPoiType(new[] { PoiType.Loot, PoiType.Harvest, PoiType.Skin },
                    new Decorator(ret => StyxWoW.Me != null && BotPoi.Current != null && !Navigator.CanNavigateFully(StyxWoW.Me.Location, BotPoi.Current.Location),
                        new Sequence(
                            new Action(ret =>
                            {
                                if (BotPoi.Current != null)
                                    Blacklist.Add(BotPoi.Current.Guid, BlacklistFlags.Loot, TimeSpan.FromDays(10.0), "Unable to reach lootable target.");
                            }),
                            new ActionClearPoi("Can't reach lootable!")))),

                new DecoratorIsPoiType(new[] { PoiType.Loot, PoiType.Harvest, PoiType.Skin },
                    new Decorator(ret => StyxWoW.Me != null && StyxWoW.Me.Combat,
                        new ActionClearPoi("Got thrown in combat while looting!"))),

                LevelBot.CreateDeathBehavior(),
                LevelBot.CreateVendorBehavior(),
                CreateLootBehavior(),

                new Decorator(ret => StyxWoW.Me != null && StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsDead,
                    new Action(ret => StyxWoW.Me.ClearTarget())),

                new Decorator(ret => CleanPoi,
                    new PrioritySelector(
                        new DecoratorIsNotPoiType(new[] { PoiType.Kill, PoiType.Loot, PoiType.Skin, PoiType.Harvest },
                            new Decorator(ret => GetGrindUnits().Any(),
                                new PrioritySelector(
                                    new Decorator(ret =>
                                    {
                                        var unit = GetGrindUnits().FirstOrDefault();
                                        return unit != null && unit.IsValid && unit.Distance < Targeting.PullDistance;
                                    },
                                        new Sequence(
                                            new Action(ret =>
                                            {
                                                var unit = GetGrindUnits().FirstOrDefault();
                                                if (unit != null && unit.IsValid)
                                                    unit.Target();
                                            }),
                                            new Wait(5, ret => StyxWoW.Me != null && StyxWoW.Me.GotTarget, new ActionIdle()),
                                            new ActionSetPoi(ret => (StyxWoW.Me != null && StyxWoW.Me.CurrentTarget != null) ? new BotPoi(StyxWoW.Me.CurrentTarget, PoiType.Kill) : null))),
                                    new Decorator(ret =>
                                    {
                                        var unit = GetGrindUnits().FirstOrDefault();
                                        return unit != null && unit.IsValid && unit.Distance >= Targeting.PullDistance;
                                    },
                                        new Sequence(
                                            new Action(ret =>
                                            {
                                                var unit = GetGrindUnits().FirstOrDefault();
                                                if (unit != null && unit.IsValid)
                                                    unit.Target();
                                            }),
                                            new Wait(5, ret => StyxWoW.Me != null && StyxWoW.Me.GotTarget, new ActionIdle()),
                                            new Decorator(ret => NeedToMoveToTarget(StyxWoW.Me != null ? StyxWoW.Me.CurrentTarget : null),
                                                new Sequence(
                                                    new Decorator(ret =>
                                                    {
                                                        var unit = GetGrindUnits().FirstOrDefault();
                                                        return unit != null && unit.IsValid && Mount.ShouldMount(unit.Location);
                                                    },
                                                        new Action(ret =>
                                                        {
                                                            var unit = GetGrindUnits().FirstOrDefault();
                                                            if (unit != null && unit.IsValid)
                                                                Mount.StateMount(() => unit.Location);
                                                        })),
                                                    new ActionMoveToTarget())))))))),

                new HookExecutor("GrindBuddy_BeforeCombat"),
                new HookExecutor("GrindBuddy_Combat"),
                new HookExecutor("GrindBuddy_AfterCombat"),

                new PrioritySelector(
                    new Decorator(ctx => RoutineManager.Current != null && RoutineManager.Current.MoveToTargetBehavior != null, RoutineManager.Current.MoveToTargetBehavior),
                    new Decorator(ret => NeedToMoveToTarget(StyxWoW.Me != null ? StyxWoW.Me.CurrentTarget : null), new ActionMoveToTarget())),

                new Decorator(ret => GetAroundUnits().Count > 0,
                    new ActionSetPoi(ret =>
                    {
                        var units = GetAroundUnits();
                        return units.Count > 0 ? new BotPoi(units[0], PoiType.Kill) : null;
                    })),

                new Decorator(ret =>
                {
                    var me = StyxWoW.Me;
                    var firstTarget = FirstTarget;
                    return me != null && me.Combat && firstTarget != null && firstTarget.IsValid
                        && ((me.Mounted && Math.Abs(me.Level - firstTarget.Level) <= 3) || !me.Mounted);
                },
                    new ActionSetPoi(ret =>
                    {
                        var target = FirstTarget;
                        return target != null && target.IsValid ? new BotPoi(target, PoiType.Kill) : null;
                    })),

                new DecoratorIsNotPoiType(new[] { PoiType.Kill, PoiType.Loot, PoiType.Skin, PoiType.Harvest },
                    new Decorator(ret =>
                    {
                        var skinUnit = GetSkinUnit();
                        return BotPoi.Current != null && BotPoi.Current.Type != PoiType.Skin && skinUnit != null && LootTargeting.SkinMobs && !Blacklist.Contains(skinUnit, BlacklistFlags.Node);
                    },
                        new ActionSetPoi(ret =>
                        {
                            var skinUnit = GetSkinUnit();
                            return skinUnit != null ? new BotPoi(skinUnit, PoiType.Skin) : null;
                        }))),

                new DecoratorIsNotPoiType(PoiType.Kill,
                    new Decorator(ret => NeedToMoveToHotspot(),
                        new Action(ret =>
                        {
                            var grindArea = StyxWoW.AreaManager.CurrentGrindArea;
                            if (grindArea == null)
                                return;
                            var hotspot = grindArea.CurrentHotSpot;
                            if (Mount.ShouldMount(hotspot))
                                Mount.StateMount(() => hotspot);
                            TreeRoot.StatusText = "GrindBuddy: Moving to hotspot";
                            Navigator.MoveTo(hotspot);
                        }))),

                new DecoratorIsNotPoiType(new[] { PoiType.Kill },
                    new Decorator(ret => StyxWoW.Me != null && BotPoi.Current != null && StyxWoW.Me.Location.Distance(BotPoi.Current.Location) > 4f,
                        new ActionMoveToPoi())),

                new ActionAlwaysSucceed()));
        }

        private static bool IsOutsideLootRange(WoWObject obj)
        {
            if (obj == null || !obj.IsValid || StyxWoW.Me == null)
                return false;

            try
            {
                WoWPoint mySpot = StyxWoW.Me.Location;
                WoWPoint[] path = Navigator.GeneratePath(mySpot, obj.Location);
                if (path == null || path.Length == 0)
                    return false;

                if (path.Any(spot => mySpot.Distance(spot) > 80f))
                    return true;

                if (path[path.Length - 1].Distance(obj.Location) > 5f)
                {
                    Blacklist.Add(obj.Guid, BlacklistFlags.Loot, TimeSpan.FromHours(1.0), "Target is not within loot range.");
                    return true;
                }
            }
            catch
            {
                // Path generation can throw; don't blacklist aggressively on transient errors.
            }

            return false;
        }

        private static bool NeedToMoveToHotspot()
        {
            if (StyxWoW.AreaManager.CurrentGrindArea != null)
                return StyxWoW.AreaManager.CurrentGrindArea.HotspotChanged;

            Logging.Write("GrindBuddy: No valid grind area for current level/area. Check your profile.");
            return false;
        }

        private static bool NeedToMoveToTarget(WoWUnit moveTarget)
        {
            if (moveTarget == null)
                moveTarget = Targeting.Instance.FirstUnit;

            if (moveTarget == null || !moveTarget.IsValid)
                return false;

            var me = StyxWoW.Me;
            if (me == null)
                return false;

            if (me.IsFlying)
            {
                if (moveTarget.DistanceSqr <= 3.0 && moveTarget.InLineOfSpellSight)
                    return false;
            }
            else if (moveTarget.DistanceSqr < Targeting.PullDistance * Targeting.PullDistance && moveTarget.InLineOfSpellSight)
            {
                return false;
            }

            return true;
        }

        private void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            Profile currentProfile = ProfileManager.CurrentProfile;
            if (currentProfile == null || StyxWoW.Me == null)
                return;

            if (StyxWoW.Me.Combat)
                return;

            HashSet<uint> factions = new HashSet<uint>();
            var currentGrindArea = StyxWoW.AreaManager.CurrentGrindArea;
            if (currentGrindArea != null && currentGrindArea.Factions != null && currentGrindArea.Factions.Count > 0)
            {
                foreach (int faction in currentGrindArea.Factions)
                    factions.Add((uint)faction);
            }
            else if (currentProfile.Factions != null)
            {
                foreach (uint faction in currentProfile.Factions)
                    factions.Add(faction);
            }

            foreach (WoWObject incomingUnit in incomingUnits)
            {
                var woWUnit = incomingUnit as WoWUnit;
                if (woWUnit == null || woWUnit is WoWPlayer || !woWUnit.IsValid)
                    continue;

                if (currentProfile.AvoidMobs != null && currentProfile.AvoidMobs.Contains(woWUnit.Entry))
                    continue;
                if (!woWUnit.IsAlive)
                    continue;
                if (IsTooNearBlackspot(currentProfile.Blackspots, woWUnit.Location))
                    continue;

                bool matches = factions.Contains(woWUnit.FactionId)
                    || (currentGrindArea != null && currentGrindArea.MobIDs != null && currentGrindArea.MobIDs.Contains((int)woWUnit.Entry));
                if (matches)
                    outgoingUnits.Add(incomingUnit);
            }
        }

        private void IncludeLootsFilter(List<WoWObject> incomingObjects, HashSet<WoWObject> outgoingObjects)
        {
            var profile = ProfileManager.CurrentProfile;
            var me = StyxWoW.Me;
            if (profile == null || me == null)
                return;

            for (int i = 0; i < incomingObjects.Count; i++)
            {
                var obj = incomingObjects[i];
                if (obj == null || !obj.IsValid)
                    continue;

                var woWUnit = obj as WoWUnit;
                if (woWUnit != null)
                {
                    if (LootTargeting.LootMobs
                        && woWUnit.Distance <= LootTargeting.LootRadius
                        && woWUnit.IsDead
                        && !Blacklist.Contains(woWUnit.Guid, BlacklistFlags.Loot | BlacklistFlags.Node)
                        && ((woWUnit.KilledByMe && woWUnit.CanLoot)
                            || (woWUnit.CanSkin && LootTargeting.SkinMobs && (CharacterSettings.Instance.NinjaSkin || woWUnit.KilledByMe))))
                    {
                        outgoingObjects.Add(woWUnit);
                    }
                    continue;
                }

                var woWGameObject = obj as WoWGameObject;
                if (woWGameObject == null)
                    continue;

                if (woWGameObject.Distance <= LootTargeting.LootRadius
                    && ((woWGameObject.IsHerb && LootTargeting.HarvestHerbs)
                        || (woWGameObject.IsMineral && LootTargeting.HarvestMinerals)
                        || (woWGameObject.IsChest && LootTargeting.LootChests))
                    && woWGameObject.CanLoot
                    && !Blacklist.Contains(woWGameObject.Guid, BlacklistFlags.Node))
                {
                    if (IsTooNearBlackspot(profile.Blackspots, woWGameObject.Location))
                    {
                        Blacklist.Add(woWGameObject.Guid, BlacklistFlags.Node, TimeSpan.FromDays(3.0), "Target is too close to a blackspot.");
                    }
                    else
                    {
                        outgoingObjects.Add(woWGameObject);
                    }
                }
            }
        }

        private static bool IsTooNearBlackspot(IEnumerable<Blackspot> blackspots, WoWPoint point)
        {
            if (blackspots == null)
                return false;
            return blackspots.Any(t => point.Distance2DSqr(t.Location) <= t.Radius * t.Radius);
        }
    }
}
