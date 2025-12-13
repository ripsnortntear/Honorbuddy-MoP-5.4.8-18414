#region

using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    /*
     * Targeting works like so, in order of being called
     * 
     * GetInitialObjectList - Return a list of initial objects for the targeting to use.
     * RemoveTargetsFilter - Remove anything that doesn't belong in the list.
     * IncludeTargetsFilter - If you want to include units regardless of the remove filter
     * WeighTargetsFilter - Weigh each target in the list.     
     *
     */


    internal class TankManager : Targeting
    {
        internal static HashSet<Tuple<int, int>> ReconingUseQualifiers = new HashSet<Tuple<int, int>>
        {
            // Tuple<AuraId, StackCount> (will want to double check AuraIds)
            Tuple.Create(143436, SuperbadSettings.Instance.BossImmerseus), // "Corrosive Blast" (http://wowhead.com/spell=143436)
            Tuple.Create(146124, SuperbadSettings.Instance.BossAmalgam), // "Self Doubt" (http://wowhead.com/spell=146124)
            Tuple.Create(144358, SuperbadSettings.Instance.BossSha), // "Wounded Pride" (http://wowhead.com/spell=144358)
            Tuple.Create(147029, SuperbadSettings.Instance.BossGalakras), // "Flames of Galakrond" (http://wowhead.com/spell=147029)
            Tuple.Create(144467, SuperbadSettings.Instance.BossIronJuggernaut), // "Ignite Armor" (http://wowhead.com/spell=144467)
            Tuple.Create(144215, SuperbadSettings.Instance.BossEarthBreakerHaromm), // "Froststorm Strike" (http://wowhead.com/spell=144215)
            Tuple.Create(143494, SuperbadSettings.Instance.BossNazgrim), // "Sundering Blow" (http://wowhead.com/spell=143494)
            Tuple.Create(142990, SuperbadSettings.Instance.BossMalkorok), // "Fatal Strike" (http://wowhead.com/spell=142990)
            Tuple.Create(143766, SuperbadSettings.Instance.BossThok), // "Panic" (http://wowhead.com/spell=143766)
            Tuple.Create(143780, SuperbadSettings.Instance.BossThok), // "Acid Breath" (http://wowhead.com/spell=143780)
            Tuple.Create(143773, SuperbadSettings.Instance.BossThok), // "Freezing Breath" (http://wowhead.com/spell=143773)
            Tuple.Create(143767, SuperbadSettings.Instance.BossThok), // "Scorching Breath" (http://wowhead.com/spell=143767)
            Tuple.Create(143385, SuperbadSettings.Instance.BossBlackfuse), // "Electrostatic Charge" (http://wowhead.com/spell=143385)
            Tuple.Create(145183, SuperbadSettings.Instance.BossGarrosh), // "Gripping Despair" (http://wowhead.com/spell=145183)
            Tuple.Create(145195, SuperbadSettings.Instance.BossGarrosh), // "Empowered Gripping Despair" (http://wowhead.com/spell=145195)
        };

        public static readonly WaitTimer TargetingTimer = new WaitTimer(TimeSpan.FromSeconds(1));

        static TankManager()
        {
            Instance = new TankManager {NeedToTaunt = new List<WoWUnit>()};
        }

        public new static TankManager Instance { get; set; }
        public List<WoWUnit> NeedToTaunt { get; private set; }

        internal static bool IsReconingCastDesired()
        {
            // Make certain we have a target, and the target is a boss...
            if (!StyxWoW.Me.GotTarget || !StyxWoW.Me.CurrentTarget.IsBoss)
            {
                return false;
            }

            // Make certain focus target is valid, and it makes sense to use Reconing...
            WoWUnit focusedUnit = StyxWoW.Me.FocusedUnit;
            if (focusedUnit == null || !focusedUnit.IsValid || focusedUnit.IsDead)
            {
                return false;
            }

            // Are any of the Reconing use constraints met?
            return
                focusedUnit.Auras.Values
                    .Any(
                        aura =>
                            ReconingUseQualifiers.Any(
                                t =>
                                    (aura.SpellId == t.Item1) && (aura.StackCount >= t.Item2) &&
                                    !StyxWoW.Me.HasAura(aura.SpellId)));
        }

        protected override List<WoWObject> GetInitialObjectList()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Cast<WoWObject>().ToList();
        }

        protected override void DefaultRemoveTargetsFilter(List<WoWObject> units)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (!units[i].IsValid)
                {
                    units.RemoveAt(i);
                    continue;
                }

                WoWUnit u = units[i].ToUnit();

                if (u.IsFriendly || u.IsDead || u.IsPet || !u.Combat || Unit.IsCrowdControlled(u))
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (u.DistanceSqr > 40*40)
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (Unit.IgnoreMobs.Contains(u.Entry))
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (u.CurrentTarget == null)
                    continue;

                WoWUnit tar = u.CurrentTarget;
                if (!tar.IsPlayer || !tar.IsHostile)
                    continue;

                units.RemoveAt(i);
            }
        }

        protected override void DefaultIncludeTargetsFilter(List<WoWObject> incomingUnits,
            HashSet<WoWObject> outgoingUnits)
        {
            foreach (WoWObject i in incomingUnits)
            {
                outgoingUnits.Add(i);
            }
        }

        protected override void DefaultTargetWeight(List<TargetPriority> units)
        {
            NeedToTaunt.Clear();
            List<WoWPlayer> members = StyxWoW.Me.GroupInfo.IsInRaid ? StyxWoW.Me.RaidMembers : StyxWoW.Me.PartyMembers;
            foreach (TargetPriority p in units)
            {
                WoWUnit u = p.Object.ToUnit();

                // I have 1M threat -> nearest party has 990k -> leaves 10k difference. Subtract 10k
                // I have 1M threat -> nearest has 400k -> Leaves 600k difference -> subtract 600k
                // The further the difference, the less the unit is weighted.
                // If they have MORE threat than I do, the number is -10k -> which subtracted = +10k weight.
                int aggroDiff = GetAggroDifferenceFor(u, members);
                p.Score -= aggroDiff;

                // If we have NO threat on the mob. Taunt the fucking thing.
                // Don't taunt fleeing mobs!
                if (aggroDiff < 0 && !u.Fleeing)
                {
                    NeedToTaunt.Add(u);
                }
            }
        }

        private static int GetAggroDifferenceFor(WoWUnit unit, IEnumerable<WoWPlayer> partyMembers)
        {
            uint myThreat = unit.ThreatInfo.ThreatValue;
            uint highestParty = (from p in partyMembers
                let tVal = unit.GetThreatInfoFor(p).ThreatValue
                orderby tVal descending
                select tVal).FirstOrDefault();

            int result = (int) myThreat - (int) highestParty;
            return result;
        }
    }
}