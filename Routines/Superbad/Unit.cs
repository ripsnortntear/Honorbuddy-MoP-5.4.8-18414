#region

using System;
using System.Collections.Generic;
using System.Linq;
using NewMixedMode;
using Styx;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    internal static class Unit
    {
        public static HashSet<uint> IgnoreMobs = new HashSet<uint>
        {
            52288,
            // Venomous Effusion (NPC near the snake boss in ZG. Its the green lines on the ground. We want to ignore them.)
            52302, // Venomous Effusion Stalker (Same as above. A dummy unit)
            52320, // Pool of Acid
            52525, // Bloodvenom
            52387, // Cave in stalker - Kilnara
            72050, // Iron Juggernaut - Crawler Mines
        };


        public static IEnumerable<WoWUnit> NearbyUnfriendlyUnits2
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                        .Where(
                            u =>
                                u.IsValid && ((u.Attackable && u.CanSelect && !u.IsDead &&
                                               !u.IsNonCombatPet && !u.IsCritter && u.Distance <= 225)
                                              || (u.IsCasting && u.CastingSpellId == 145599)) &&
                                !IgnoreMobs.Contains(u.Entry) && !u.IsFriendly
                        ).ToList();
            }
        }

        public static IEnumerable<WoWUnit> NearbyUnfriendlyUnits
        {
            get { return UnfriendlyUnits(25); }
        }

        public static float MeleeRange
        {
            get { return StyxWoW.Me.CurrentTarget == null ? 0 : StyxWoW.Me.CurrentTarget.MeleeDistance(); }
        }

        public static IEnumerable<WoWPlayer> GroupMembers
        {
            get
            {
                // Grab party+raid member + myself GUIDs
                ulong[] guids =
                    StyxWoW.Me.GroupInfo.RaidMemberGuids.Union(StyxWoW.Me.GroupInfo.PartyMemberGuids)
                        .Union(new[] {StyxWoW.Me.Guid})
                        .Distinct()
                        .ToArray();

                return (
                    from p in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
                    where p.IsFriendly && guids.Any(g => g == p.Guid)
                    select p).ToList();
            }
        }

        public static IEnumerable<WoWPlayer> NearbyGroupMembers
        {
            get { return GroupMembers.Where(p => p.DistanceSqr <= 30*30).ToList(); }
        }

        public static IEnumerable<WoWUnit> UnfriendlyUnits(int maxSpellDist)
        {
            bool dungeonBot = IsBotInUse("DungeonBuddy");
            bool questBot = IsBotInUse("Quest");

            bool useTargeting = (dungeonBot || (questBot && StyxWoW.Me.IsInInstance));

            if (useTargeting)
            {
                return
                    Targeting.Instance.TargetList.Where(
                        u => u != null && ValidUnit(u) && u.SpellDistance() < maxSpellDist);
            }

            Type typeWoWUnit = typeof (WoWUnit);
            Type typeWoWPlayer = typeof (WoWPlayer);
            List<WoWObject> objectList = ObjectManager.ObjectList;
            return (from t1 in objectList
                let type = t1.GetType()
                where type == typeWoWUnit || type == typeWoWPlayer
                select t1 as WoWUnit
                into t
                where t != null && ValidUnit(t) && t.SpellDistance() < maxSpellDist
                select t).ToList();
        }

        public static bool IsBotInUse(params string[] nameSubstrings)
        {
            string botName = GetBotName().ToUpper();
            return nameSubstrings.Any(s => botName.Contains(s.ToUpper()));
        }

        public static string GetBotName()
        {
            BotBase bot = null;

            if (TreeRoot.Current != null)
            {
                if (!(TreeRoot.Current is MixedModeEx))
                    bot = TreeRoot.Current;
                else
                {
                    var mmb = (MixedModeEx) TreeRoot.Current;
                    if (mmb != null)
                    {
                        if (mmb.SecondaryBot != null && mmb.SecondaryBot.RequirementsMet)
                            return "Mixed:" + mmb.SecondaryBot.Name;
                        return mmb.PrimaryBot != null ? "Mixed:" + mmb.PrimaryBot.Name : "Mixed:[primary null]";
                    }
                }
            }

            return bot.Name;
        }

        internal static bool IsBoss(WoWUnit thisUnit)
        {
            return thisUnit != null && (BossList.BossIds.Contains(thisUnit.Entry));
        }

        internal static bool IsDummy(WoWUnit thisUnit)
        {
            return thisUnit != null && (BossList.TrainingDummies.Contains(thisUnit.Entry));
        }

        /*public static bool ValidUnit(WoWUnit p)
        {
            if (p == null)
                return false;
            if (!p.IsValid)
                return false;
            if (IgnoreMobs.Contains(p.Entry))
                return false;
            if (!p.CanSelect)
                return false;
            if (p.IsFriendly)
                return false;
            if (p.IsPet || p.OwnedByRoot != null)
                return false;
            return !p.IsNonCombatPet;
        }*/


        public static bool ValidUnit(WoWUnit p, bool showReason = false)
        {
            if (p == null || !p.IsValid)
                return false;

            if (StyxWoW.Me.IsInInstance && IgnoreMobs.Contains(p.Entry))
            {
                return false;
            }

            // Ignore shit we can't select
            if (!p.CanSelect)
            {
                return false;
            }

            // Ignore shit we can't attack
            if (!p.Attackable)
            {
                return false;
            }

            // Duh
            if (p.IsDead)
            {
                return false;
            }

            // check for enemy players here as friendly only seems to work on npc's
            if (p.IsPlayer)
                return p.ToPlayer().IsHorde != StyxWoW.Me.IsHorde;


            // Dummies/bosses are valid by default. Period.
            if (IsDummy(p) || IsBoss(p))
                return true;

            // If it is a pet/minion/totem, lets find the root of ownership chain
            WoWUnit pOwner = GetPlayerParent(p);

            // ignore if owner is player, alive, and not blacklisted then ignore (since killing owner kills it)
            if (pOwner != null && pOwner.IsAlive && !Blacklist.Contains(pOwner, BlacklistFlags.Combat))
            {
                return false;
            }

            // And ignore critters (except for those ferocious ones) /non-combat pets
            if (p.IsNonCombatPet)
            {
                return false;
            }

            // And ignore critters (except for those ferocious ones) /non-combat pets
            if (p.IsCritter && p.ThreatInfo.ThreatValue == 0 && !p.IsTargetingMyRaidMember)
            {
                return false;
            }

            // Ignore friendlies!
            if (p.IsFriendly)
            {
                return false;
            }
            return true;
        }


        public static bool IsCrowdControlled(WoWUnit unit)
        {
            if (StyxWoW.Me.CurrentTarget == null)
                return false;
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

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
                     a.Spell.Name == "Hex"
                );
        }

        public static bool IsAboveTheGround(WoWUnit u)
        {
            float height = HeightOffTheGround(u);
            if (height == float.MaxValue)
                return false; // make this true if better to assume aerial 

            return height > MeleeRange;
        }

        public static bool IsInGroup(this LocalPlayer me)
        {
            return me.GroupInfo.IsInParty || me.GroupInfo.IsInRaid;
        }

        public static WoWUnit GetPlayerParent(WoWUnit unit)
        {
            // If it is a pet/minion/totem, lets find the root of ownership chain
            WoWUnit pOwner = unit;
            while (true)
            {
                if (pOwner.OwnedByUnit != null)
                    pOwner = pOwner.OwnedByRoot;
                else if (pOwner.CreatedByUnit != null)
                    pOwner = pOwner.CreatedByUnit;
                else if (pOwner.SummonedByUnit != null)
                    pOwner = pOwner.SummonedByUnit;
                else
                    break;
            }

            if (unit != pOwner && pOwner.IsPlayer)
                return pOwner;

            return null;
        }

        public static float MeleeDistance(this WoWUnit mob)
        {
            return mob.IsPlayer ? 3.5f : Math.Max(5f, StyxWoW.Me.CombatReach + 1.3333334f + mob.CombatReach);
        }

        public static float HeightOffTheGround(WoWUnit u)
        {
            if (u != null)
            {
                var unitLoc = new WoWPoint(u.Location.X, u.Location.Y, u.Location.Z);
                IEnumerable<float> listMeshZ = Navigator.FindHeights(unitLoc.X, unitLoc.Y).Where(h => h <= unitLoc.Z);
                float[] meshZ = listMeshZ as float[] ?? listMeshZ.ToArray();
                if (meshZ.Any())
                    return unitLoc.Z - meshZ.Max();
            }
            return float.MaxValue;
        }
    }
}