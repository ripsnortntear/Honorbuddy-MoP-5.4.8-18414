using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;

//
//  TAHNK YOU SINGULAR!
//

namespace DWCC
{
    internal static class Unit
    {
        public static HashSet<uint> IgnoreMobs = new HashSet<uint>
            {
                52288, // Venomous Effusion (NPC near the snake boss in ZG. Its the green lines on the ground. We want to ignore them.)
                52302, // Venomous Effusion Stalker (Same as above. A dummy unit)
                52320, // Pool of Acid
                52525, // Bloodvenom

                52387, // Cave in stalker - Kilnara
            };

        public static bool IsUndeadOrDemon(this WoWUnit unit)
        {
            return unit.CreatureType == WoWCreatureType.Undead
                    || unit.CreatureType == WoWCreatureType.Demon;
        }

        /// <summary>
        /// List of WoWPlayer in your Group. Deals with Party / Raid in a list independent manner and does not restrict distance
        /// </summary>
        public static IEnumerable<WoWPlayer> GroupMembers
        {
            get
            {
                // Grab party+raid member + myself GUIDs
                ulong[] guids =
                    StyxWoW.Me.GroupInfo.RaidMemberGuids.Union(StyxWoW.Me.GroupInfo.PartyMemberGuids).Union(new[] { StyxWoW.Me.Guid }).Distinct().ToArray();

                return (
                    from p in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
                    where p.IsFriendly && guids.Any(g => g == p.Guid)
                    select p).ToList();
            }
        }


        /// <summary>
        /// List of WoWPartyMember in your Group. Deals with Party / Raid in a list independent manner and does not restrict distance
        /// </summary>
        public static IEnumerable<WoWPartyMember> GroupMemberInfos
        {
            get { return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers).Distinct(); }
        }


        public static IEnumerable<WoWPlayer> NearbyGroupMembers
        {
            get
            {
                return GroupMembers.Where(p => p.DistanceSqr <= 40 * 40).ToList();
            }
        }

        /// <summary>
        ///   Gets the nearby friendly players within 40 yards.
        /// </summary>
        /// <value>The nearby friendly players.</value>
        public static IEnumerable<WoWPlayer> NearbyFriendlyPlayers
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWPlayer>(true, true).Where(p => p.DistanceSqr <= 40 * 40 && p.IsFriendly).ToList();
            }
        }

        /// <summary>
        ///   Gets the nearby unfriendly units within 40 yards.
        /// </summary>
        /// <value>The nearby unfriendly units.</value>
        public static IEnumerable<WoWUnit> NearbyUnfriendlyUnits
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => ValidUnit(p) && p.DistanceSqr <= 40 * 40).ToList(); }
        }

        public static IEnumerable<WoWUnit> NearbyUnitsInCombatWithMe
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => ValidUnit(p) && p.DistanceSqr <= 40 * 40 && p.Combat && p.TaggedByMe).ToList(); }
        }


        static bool ValidUnit(WoWUnit p)
        {
            if (IgnoreMobs.Contains(p.Entry))
                return false;

            // Ignore shit we can't select/attack
            if (!p.CanSelect || !p.Attackable)
                return false;

            // Ignore friendlies!
            if (p.IsFriendly)
                return false;

            // Duh
            if (p.IsDead)
                return false;

            // If its a pet, lets ignore it please.
            if (p.IsPet || p.OwnedByRoot != null)
                return false;

            // And ignore critters/non-combat pets
            if (p.IsNonCombatPet || p.IsCritter)
                return false;

            return true;
        }

        /// <summary>
        ///   Gets the nearby unfriendly units within *distance* yards.
        /// </summary>
        /// <param name="distance"> The distance to check from current target</param>
        /// <value>The nearby unfriendly units.</value>
        public static IEnumerable<WoWUnit> UnfriendlyUnitsNearTarget(float distance)
        {
            var dist = distance * distance;
            var curTarLocation = StyxWoW.Me.CurrentTarget.Location;
            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(
                        p => ValidUnit(p) && p.Location.DistanceSqr(curTarLocation) <= dist).ToList();
        }

        /// <summary>
        ///  Checks the aura by the name on specified unit.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, string aura)
        {
            return HasAura(unit, aura, 0);
        }

        /// <summary>
        ///  Checks the aura count by the name on specified unit.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <param name="stacks"> The stack count of the aura to return true. </param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, string aura, int stacks)
        {
            return HasAura(unit, aura, stacks, null);
        }


        public static bool HasAllMyAuras(this WoWUnit unit, params string[] auras)
        {
            return auras.All(unit.HasMyAura);
        }

        /// <summary>
        ///  Check the aura count thats created by yourself by the name on specified unit
        /// </summary>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <returns></returns>
        public static bool HasMyAura(this WoWUnit unit, string aura)
        {
            return HasMyAura(unit, aura, 0);
        }

        /// <summary>
        ///  Check the aura count thats created by yourself by the name on specified unit
        /// </summary>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="stacks"> The stack count of the aura to return true. </param>
        /// <returns></returns>
        public static bool HasMyAura(this WoWUnit unit, string aura, int stacks)
        {
            return HasAura(unit, aura, stacks, StyxWoW.Me);
        }

        private static bool HasAura(this WoWUnit unit, string aura, int stacks, WoWUnit creator)
        {
            return unit.GetAllAuras().Any(a => a.Name == aura && a.StackCount >= stacks && (creator == null || a.CreatorGuid == creator.Guid));
        }

        /// <summary>
        ///  Checks for the auras on a specified unit. Returns true if the unit has any aura in the auraNames list.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="auraNames"> Aura names to be checked. </param>
        /// <returns></returns>
        public static bool HasAnyAura(this WoWUnit unit, params string[] auraNames)
        {
            var auras = unit.GetAllAuras();
            var hashes = new HashSet<string>(auraNames);
            return auras.Any(a => hashes.Contains(a.Name));
        }

        /// <summary>
        ///  Checks for the auras on a specified unit. Returns true if the unit has any aura with any of the mechanics in the mechanics list.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="mechanics"> Mechanics to be checked. </param>
        /// <returns></returns>
        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWSpellMechanic[] mechanics)
        {
            var auras = unit.GetAllAuras();
            return auras.Any(a => mechanics.Contains(a.Spell.Mechanic));
        }

        /// <summary>
        ///  Checks for the auras on a specified unit. Returns true if the unit has any aura with any apply aura type in the list.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="mechanics"> Mechanics to be checked. </param>
        /// <returns></returns>
        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWApplyAuraType[] applyType)
        {
            var auras = unit.GetAllAuras();
            return auras.Any(a => a.Spell.SpellEffects.Any(se => applyType.Contains(se.AuraType)));
        }

        /// <summary>
        ///  Returns the timeleft of an aura by TimeSpan. Return TimeSpan.Zero if the aura doesn't exist.
        /// </summary>
        /// <param name="auraName"> The name of the aura in English. </param>
        /// <param name="onUnit"> The unit to check the aura for. </param>
        /// <param name="fromMyAura"> Check for only self or all buffs</param>
        /// <returns></returns>
        public static TimeSpan GetAuraTimeLeft(this WoWUnit onUnit, string auraName, bool fromMyAura)
        {
            WoWAura wantedAura =
                onUnit.GetAllAuras().Where(a => a.Name == auraName && (!fromMyAura || a.CreatorGuid == StyxWoW.Me.Guid)).FirstOrDefault();

            return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;
        }

        public static TimeSpan GetAuraTimeLeft(this WoWUnit onUnit, int auraID, bool fromMyAura)
        {
            WoWAura wantedAura =
                onUnit.GetAllAuras().Where(a => a.SpellId == auraID && (!fromMyAura || a.CreatorGuid == StyxWoW.Me.Guid)).FirstOrDefault();

            return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;
        }

        /// <summary>
        /// Returns a list of resurrectable players in a 40 yard radius
        /// </summary>
        public static List<WoWPlayer> ResurrectablePlayers
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWPlayer>().Where(
                    p => !p.IsMe && p.IsDead && p.IsFriendly && p.IsInMyPartyOrRaid &&
                         p.DistanceSqr < 40 * 40 && !Blacklist.Contains(p.Guid, BlacklistFlags.All)).ToList();
            }
        }

        public static bool IsCrowdControlled(this WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

#if AURAS_HAVE_MECHANICS
            return auras.Any(
                a => a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                     a.Spell.Mechanic == WoWSpellMechanic.Charmed ||
                     a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                     a.Spell.Mechanic == WoWSpellMechanic.Incapacitated ||
                     a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                     a.Spell.Mechanic == WoWSpellMechanic.Sapped ||
                     a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                     a.Spell.Mechanic == WoWSpellMechanic.Asleep ||
                     a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                     a.Spell.Mechanic == WoWSpellMechanic.Invulnerable ||
                     a.Spell.Mechanic == WoWSpellMechanic.Invulnerable2 ||
                     a.Spell.Mechanic == WoWSpellMechanic.Turned ||

                     // Really want to ignore hexed mobs.
                     a.Spell.Name == "Hex"

                     );
#else
            return unit.Stunned
                || unit.Rooted
                || auras.Any(
                    a => a.Spell != null
                        && a.Spell.SpellEffects.Any(
                            se => se.AuraType == WoWApplyAuraType.ModConfuse
                                || se.AuraType == WoWApplyAuraType.ModCharm
                                || se.AuraType == WoWApplyAuraType.ModFear
                                || se.AuraType == WoWApplyAuraType.ModDecreaseSpeed
                                || se.AuraType == WoWApplyAuraType.ModPacify
                                || se.AuraType == WoWApplyAuraType.ModPacifySilence
                                || se.AuraType == WoWApplyAuraType.ModPossess
                                || se.AuraType == WoWApplyAuraType.ModRoot
                                // aurse.AuraType == WoWApplyAuraType.ModSilence 
                                || se.AuraType == WoWApplyAuraType.ModStun
                            ));
#endif
        }

        public static bool HasAuraWithEffect(this WoWUnit unit, WoWApplyAuraType auraType, int miscValue, int basePointsMin, int basePointsMax)
        {
            var auras = unit.GetAllAuras();
            return (from a in auras
                    where a.Spell != null
                    let spell = a.Spell
                    from e in spell.GetSpellEffects()
                    // First check: Ensure the effect is... well... valid
                    where e != null &&
                        // Ensure the aura type is correct.
                    e.AuraType == auraType &&
                        // Check for a misc value. (Resistance types, etc)
                    (miscValue == -1 || e.MiscValueA == miscValue) &&
                        // Check for the base points value. (Usually %s for most debuffs)
                    e.BasePoints >= basePointsMin && e.BasePoints <= basePointsMax
                    select a).Any();
        }
        public static bool HasSunders(this WoWUnit unit)
        {
            // Remember; this is negative values [debuff]. So min is -12, max is -4. Duh.
            return unit.HasAuraWithEffect(WoWApplyAuraType.ModResistancePct, 1, -12, -4);

            //var auras = unit.GetAllAuras();
            //var tmp = (from a in auras
            //           where a.Spell != null
            //           from e in a.Spell.SpellEffects
            //           // Sunder, Faerie Fire, and another have -4% armor per-stack.
            //           // Expose Armor, and others, have a flat -12%
            //           // Ensure we check MiscValueA for 1, as thats the resistance index for physical (aka; armor)
            //           where
            //               e != null && e.AuraType == WoWApplyAuraType.ModResistancePct && e.MiscValueA == 1 &&
            //               (e.BasePoints == -4 || e.BasePoints == -12)
            //           select a).Any();

            //return tmp;
        }
        public static bool HasDemoralizing(this WoWUnit unit)
        {
            // don't try if the unit is out of range.
            if (unit.DistanceSqr > 25)
                return true;

            // Plain and simple, any effect with -damage is good. Ensure at least -1. Since 0 may be a buggy spell entry or something.
            var tmp = unit.HasAuraWithEffect(WoWApplyAuraType.ModDamagePercentDone, -1, int.MinValue, -1);

            return tmp;

            //var auras = unit.GetAllAuras();
            //var tmp = (from a in auras
            //           where a.Spell != null && a.Spell.SpellEffect1 != null
            //           let effect = a.Spell.SpellEffect1
            //           // Basically, all spells are -10% damage done that are demoralizing shout/roar/etc.
            //           // The aura type is damage % done. Just chekc for anything < 0. (There may be some I'm forgetting that aren't -10%, but stacks of like 2% or something
            //           where effect.AuraType == WoWApplyAuraType.ModDamagePercentDone && effect.BasePoints < 0
            //           select a).Any();
            //if (!tmp)
            //    Logger.Write(unit.Name + " does not have demoralizing!");
            //return tmp;
        }

        public static bool HasBleedDebuff(this WoWUnit unit)
        {
            return unit.HasAuraWithEffect(WoWApplyAuraType.ModMechanicDamageTakenPercent, 15, 0, int.MaxValue);
        }

        /// <summary>A temporary fix until the next build of HB.</summary>
        static SpellEffect[] GetSpellEffects(this WoWSpell spell)
        {
            SpellEffect[] effects = new SpellEffect[3];
            effects[0] = spell.GetSpellEffect(0);
            effects[1] = spell.GetSpellEffect(1);
            effects[2] = spell.GetSpellEffect(2);
            return effects;
        }

        public static bool IsInGroup(this LocalPlayer me)
        {
            return StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid;
        }

        private const int UnitIncomingHealsCountIndex = 1203;
        private const int UnitIncomingHealsListIndex = 1204;

    }
    public class TankManager : Targeting
    {
        static TankManager()
        {
            Instance = new TankManager { NeedToTaunt = new List<WoWUnit>() };
        }

        public new static TankManager Instance { get; set; }
        public List<WoWUnit> NeedToTaunt { get; private set; }

        public static readonly WaitTimer TargetingTimer = new WaitTimer(TimeSpan.FromSeconds(1));

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

                if (u.IsFriendly || u.IsDead || u.IsPet || !u.Combat || u.IsCrowdControlled())
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (u.Distance > 12f)
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

        protected override void DefaultIncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            foreach (WoWObject i in incomingUnits)
            {
                var unit = i as WoWUnit;

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

            int result = (int)myThreat - (int)highestParty;
            return result;
        }
    }
}
