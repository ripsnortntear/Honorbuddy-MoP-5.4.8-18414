#region Revision info

/*
 * $Author: worklifebalance $
 * $Date: 2013-09-21 16:32:20 +0200 (Sa, 21 Sep 2013) $
 * $ID$
 * $Revision: 1733 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Core/Unit.cs $
 * $LastChangedBy wulf$
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Classes;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Core
{
    internal static class Unit
    {
        //PLEASE READ: Never use CacheAttackableUnits.Where(u => u.IsWithinMeleeRange); <-- IsWithinMeleeRange will crash wow everytime!!

        #region Core Unit Checks - these should all be incorperated into the individual class checks below.

        // This is the core check..filter everthing else related to hostiles of UnfriendlyUnits
        internal static IEnumerable<WoWUnit> AttackableUnits
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u.Attackable && u.CanSelect && !u.IsFriendly && !u.IsDead && !u.IsNonCombatPet && !u.IsCritter && u.Distance <= 60); }
        }

        internal static IEnumerable<WoWUnit> AttackableMeleeUnits
        {
            get { return NearbyAttackableUnits(StyxWoW.Me.Location, 10f); } 
        }

        internal static IEnumerable<WoWUnit> NearbyAttackableUnits(WoWPoint fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance);
        }

        internal static IEnumerable<WoWUnit> NearbyAttackableUnitsAttackingUs(WoWPoint fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance && (x.IsTargetingMyPartyMember || x.IsTargetingMeOrPet || x.IsTargetingAnyMinion || x.IsTargetingMyRaidMember || x.IsTargetingPet || x.IsMechanical));
        }

        internal static IEnumerable<WoWPlayer> GroupMembers
        {
            get { return ObjectManager.GetObjectsOfType<WoWPlayer>(true, true).Where(u => u.CanSelect && !u.IsDead && u.IsInMyPartyOrRaid); }
        }

        #endregion Core Unit Checks - these should all be incorperated into the individual class checks below.

        #region Cached methods

        internal static IEnumerable<WoWUnit> CachedAttackableMeleeUnits
        {
            get { return NearbyAttackableUnits(StyxWoW.Me.Location, 10f); }
        }

        internal static IEnumerable<WoWUnit> CachedNearbyAttackableUnits(WoWPoint fromLocation, double radius)
        {
            var hostile = CachedUnits.AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance);
        }

        internal static IEnumerable<WoWUnit> CachedNearbyAttackableUnitsAttackingUs(WoWPoint fromLocation, double radius)
        {
            var hostile = CachedUnits.AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance && (x.IsTargetingMyPartyMember || x.IsTargetingMeOrPet || x.IsTargetingAnyMinion || x.IsTargetingMyRaidMember || x.IsTargetingPet || x.IsMechanical || x.IsDummy()));
        }

        #endregion Cached methods

        #region MultiDotting

        internal static WoWUnit GetMultiDoTTarget(WoWUnit unit, string debuff, double radius, double refreshDurationRemaining)
        {
            //var result = CachedNearbyAttackableUnitsAttackingUs(unit.Location, radius)
            //    .Where(x => x != null && !x.HasMyAura(debuff) && x.InLineOfSpellSight)
            //    .OrderByDescending(x => x.HealthPercent);

            //foreach (var woWUnit in result)
            //{
            //    Logger.InfoLog(woWUnit.ToString());
            //}


            // find unit without our debuff
            var dotTarget = CachedNearbyAttackableUnitsAttackingUs(unit.Location, radius)
                .Where(x => x != null)
                .OrderByDescending(x => x.HealthPercent)
                .FirstOrDefault(x => !x.HasMyAura(debuff) && x.InLineOfSpellSight);

            if (dotTarget == null)
            {
                // If we couldn't find one without our debuff, then find ones where debuff is about to expire.
                dotTarget = CachedNearbyAttackableUnitsAttackingUs(unit.Location, radius)
                            .Where(x => x != null)
                            .OrderByDescending(x => x.HealthPercent)
                            .FirstOrDefault(x => (x.HasMyAura(debuff) && Spell.GetMyAuraTimeLeft(debuff, x) < refreshDurationRemaining) && x.InLineOfSpellSight);
            }
            return dotTarget;
        }

        #endregion MultiDotting

        #region Cluster Targets checks

        // Copy Pasted from Singular -- wulf.
        public static
        IEnumerable<WoWUnit> GetRadiusCluster(WoWUnit target, IEnumerable<WoWUnit> otherUnits, float radius)
        {
            if (target != null)
            {
                var targetLoc = target.Location;
                return otherUnits.Where(u => u.Location.DistanceSqr(targetLoc) <= radius * radius);
            }
            return null;
        }

        // Stolen from Singular with some amendments to tidy code -- Millz
        public static WoWUnit GetBestUnitForCluster(IEnumerable<WoWUnit> units, float clusterRange)
        {
            if (units != null && units.Any())
            {
                return (from u in units
                        select new { Count = GetRadiusClusterCount(u, units, clusterRange), Unit = u })
                    .OrderByDescending(a => a.Count)
                    .FirstOrDefault().Unit;
            }

            return null;
        }

        private static int GetRadiusClusterCount(WoWUnit target, IEnumerable<WoWUnit> otherUnits, float radius)
        {
            var targetLoc = target.Location;
            return otherUnits.Count(u => u.Location.DistanceSqr(targetLoc) <= radius * radius);
        }

        #endregion Cluster Targets checks

        #region Warrior - Putting all warrior specific Unit checks in here..inherits from the core unit checks above.

        /// <summary>Check for players to use Rallying Cry on (Warrior Heal)</summary>
        public static bool NeedRallyingCry { get { return GroupMembers.Any(u => u.HealthPercent < 20 && !u.ActiveAuras.ContainsKey("Rallying Cry") && u.DistanceSqr < 30 * 30); } }

        public static WoWPlayer SafeguardLowHealthResult;
        public static WoWPlayer SafeguardLowHealth 
        { 
            get 
            { 
                
                var gm = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p =>

                    !p.IsMe
                    && p.IsInMyPartyOrRaid
                    && p.IsAlive 
                    && p.InLineOfSight 
                    && p.Distance <= 25 
                    && p.HealthPercent <= 25);

                SafeguardLowHealthResult = gm.FirstOrDefault();
                return SafeguardLowHealthResult; 
            } 
        }

        public static List<WoWPlayer> Tanks
        {
            get
            {
                if (!StyxWoW.Me.GroupInfo.IsInParty)
                    return new List<WoWPlayer>(); ;

                return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank))
                    .Select(p => p.ToPlayer())
                    .Union(new[] { RaFHelper.Leader })
                    .Where(p => p != null && p.IsValid)
                    .Distinct()
                    .ToList();
            }
        }

        public static WoWPlayer SafeguardScatterShotResult;
        public static WoWPlayer SafeguardScatterShot 
        { 
            get 
            { 
                var gm = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p =>
                    
                    !p.IsMe
                    && p.IsInMyPartyOrRaid
                    && p.InLineOfSight
                    && p.Distance <= 25
                    && p.HasAura(SpellBook.ScatterShot));

                SafeguardScatterShotResult = gm.FirstOrDefault();
                return SafeguardScatterShotResult;
            }
        }

        public static WoWPlayer LeapScatterShotResult;
        public static WoWPlayer LeapScatterShot
        {
            get
            {
                var gm = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p =>

                    !p.IsMe
                    && p.IsInMyPartyOrRaid
                    && p.InLineOfSight
                    && p.Distance >= 8
                    && p.Distance <= 40
                    && p.HasAura(SpellBook.ScatterShot));

                LeapScatterShotResult = gm.FirstOrDefault();
                return LeapScatterShotResult;
            }
        }

        public static HashSet<string> HS_Banners = new HashSet<string>()
        {
            "Mocking Banner",
            "Skull Banner",
            "Demoralizing Banner"
        };
        public static WoWUnit BestUnitForSafeguardResult;
        public static WoWUnit BestUnitForSafeguard
        {
            get
            {
                BestUnitForSafeguardResult = null;

                var banners = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(u =>
                    u.CreatedByUnitGuid == StyxWoW.Me.Guid
                    && u.Distance <= 25
                    && u.InLineOfSight
                    && HS_Banners.Contains(u.Name));

                if (banners.Any())
                {
                    BestUnitForSafeguardResult = banners.FirstOrDefault();
                    return BestUnitForSafeguardResult;
                }

                var members = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(u =>
                    u.IsInMyPartyOrRaid
                    && !u.IsMe
                    && u.Distance <= 25
                    && u.InLineOfSight);

                    BestUnitForSafeguardResult = members.FirstOrDefault();
                    return BestUnitForSafeguardResult;
            }
        }

        public static WoWUnit BestUnitForInterveneCCBreakResult;
        public static WoWUnit BestUnitForInterveneCCBreak
        {
            get
            {
                BestUnitForInterveneCCBreakResult = null;

                var members = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(u =>
                    u.IsInMyPartyOrRaid
                    && !u.IsMe
                    && u.Distance <= 25
                    && u.InLineOfSight);

                BestUnitForInterveneCCBreakResult = members.FirstOrDefault();
                return BestUnitForInterveneCCBreakResult;
            }
        }

        public static HashSet<string> HS_TotemsToKill = new HashSet<string>()
        {
            "Spirit Link Totem",
            "Capacitor Totem",
            "Earthgrab Totem",
            "Earthbind Totem"
        };

        public static WoWUnit TotemToKillResult;
        public static WoWUnit TotemToKill
        {
            get
            {
                

                var totems = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(u =>
                    u.IsTotem
                    && u.Distance <= 30
                    && !u.ControllingPlayer.IsInMyPartyOrRaid
                    && HS_TotemsToKill.Contains(u.Name)
                    && u.InLineOfSight);

                TotemToKillResult = null;

                if (totems.Any())
                {
                    TotemToKillResult = totems.FirstOrDefault();
                    return TotemToKillResult;
                }

                return TotemToKillResult;
            }
        }

        public static WoWPlayer DemoBannerLowHealthResult;
        public static WoWPlayer DemoBannerLowHealth { get { DemoBannerLowHealthResult = null; DemoBannerLowHealthResult = GroupMembers.FirstOrDefault(p => p.IsAlive && p.Combat && p.HealthPercent <= 25 && p.Distance <= 30); return DemoBannerLowHealthResult; } }

        public static WoWPlayer VigilanceLowHealthResult;
        public static WoWPlayer VigilanceLowHealth { get { VigilanceLowHealthResult = null; VigilanceLowHealthResult = GroupMembers.FirstOrDefault(p => !p.IsMe && p.IsAlive && p.Combat && p.HealthPercent <= 25 && p.Distance <= 40); return VigilanceLowHealthResult; } }

        #endregion Warrior - Putting all warrior specific Unit checks in here..inherits from the core unit checks above.

        #region Paladin - Putting all Paladin specific Unit checks in here..inherits from the core unit checks above.

        /// <summary>Check for players to use Holy Prism or Lights Hammer on (Paladin Heal)</summary> // TODO: Implemented settings but not sure to use it or not.
        public static int HolyPrismLightsHammerPlayers { get { return GroupMembers.Count(u => u.HealthPercent < PRSettings.Instance.Paladin.HolyPrismLightsHammerPercent && u.DistanceSqr < 30 * 30); } }

        #endregion Paladin - Putting all Paladin specific Unit checks in here..inherits from the core unit checks above.

        #region Rogue - Putting all Rogue specific Unit checks in here.. inherits from the core unit checks above.

        public static WoWUnit BestTricksTarget
        {
            get
            {
                if (!PRSettings.Instance.Rogue.UseTricksOfTheTrade) return null;
                if (StyxWoW.Me.GroupInfo.IsInParty)
                {
                    // If the player has a focus target set, use it instead.
                    if (StyxWoW.Me.FocusedUnitGuid != 0 && StyxWoW.Me.FocusedUnit.IsAlive && !StyxWoW.Me.FocusedUnit.HasAura("Tricks of the Trade"))
                    {
                        return StyxWoW.Me.FocusedUnit;
                    }
                }
                return null;
            }
        }

        private static readonly HashSet<int> WeakenedArmor = new HashSet<int>
            {
            113746, // Weakened Armor
        };

        public static bool UnitHasWeakenedArmor(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => WeakenedArmor.Contains(x.Value.SpellId));
        }

        #endregion Rogue - Putting all Rogue specific Unit checks in here.. inherits from the core unit checks above.

        #region DeathKnight - Putting all Deathknight Specific Unit check in here.. inherits from the core unit checks above.

        #endregion DeathKnight - Putting all Deathknight Specific Unit check in here.. inherits from the core unit checks above.

        #region ShadowPriest - Putting all ShadowPriest Specific Unit check in here.. inherits from the core unit checks above.

        public static HashSet<uint> IgnoreMultiDotting = new HashSet<uint>
            {
            60240,
            211364
        };

        #endregion ShadowPriest - Putting all ShadowPriest Specific Unit check in here.. inherits from the core unit checks above.

        #region Extensions

        public static bool ShowPlayerNames { get; set; }

        public static bool IsTrainingDummy(this WoWUnit unit)
        {
            if (!unit.IsMechanical)
                return false;

            return BossList.TrainingDummies.Contains(unit.Entry);
        }
        internal static bool IsOverrideDnD(this WoWUnit thisUnit)
        {
            return thisUnit != null && (BossList.OverrideDnD.Contains(thisUnit.Entry));
        }

        internal static bool IsBoss(this WoWUnit thisUnit)
        {
            return thisUnit != null && (thisUnit.IsBoss || BossList.BossIds.Contains(thisUnit.Entry));
        }

        internal static bool IsShredBoss(this WoWUnit thisUnit)
        {
            return thisUnit != null && (BossList.CanShred.Contains(thisUnit.Entry));
        }

        internal static bool IsDummy(this WoWUnit thisUnit)
        {
            return thisUnit != null && (BossList.TrainingDummies.Contains(thisUnit.Entry));
        }

        private static bool IsAerialTarget(this WoWUnit unit)
        {
            if (unit.MaxHealth == 1) return false; // training dummy..:/
            float height = HeightOffTheGround(unit);
            if (height > 5f && height < Single.MaxValue)
                return true;
            return false;
        }

        private static float HeightOffTheGround(this WoWUnit unit)
        {
            float minDiff = Single.MaxValue;
            var pt = new WoWPoint(unit.Location.X, unit.Location.Y, unit.Location.Z);

            List<float> listMeshZ = Navigator.FindHeights(pt.X, pt.Y);
            foreach (var meshZ in listMeshZ)
            {
                var diff = pt.Z - meshZ;
                if (diff >= 0 && diff < minDiff)
                {
                    minDiff = diff;
                    pt.Z = meshZ;
                }
            }

            return minDiff;
        }

        /* Example Code for integrating a Hotkey for cooldown usage!  Example usage: WoWUnit.IsBoss(HotKeyManager.IsCooldown) */

        internal static bool IsBoss(this WoWUnit thisUnit, bool hotkey)
        {
            return thisUnit != null && (hotkey || thisUnit.IsBoss || BossList.BossIds.Contains(thisUnit.Entry));
        }

        public static bool IsInGroup(this LocalPlayer me)
        {
            return me.GroupInfo.IsInParty || me.GroupInfo.IsInRaid;
        }

        public static float SpellPower(this LocalPlayer me)
        {
            Lua._secondaryStats.Refresh();
            return Lua._secondaryStats.SpellPower;
        }

        public static bool IsMelee(this WoWUnit unit)
        {
            {
                if (unit != null)
                {
                    switch (StyxWoW.Me.Class)
                    {
                        case WoWClass.Warrior:
                        case WoWClass.Rogue:
                        case WoWClass.DeathKnight:
                            return true;
                        case WoWClass.Paladin:
                            return StyxWoW.Me.Specialization != WoWSpec.PaladinHoly;
                        case WoWClass.Hunter:
                            return false;
                        case WoWClass.Priest:
                            return false;
                        case WoWClass.Shaman:
                            return StyxWoW.Me.Specialization == WoWSpec.ShamanEnhancement;
                        case WoWClass.Mage:
                            return false;
                        case WoWClass.Warlock:
                            return false;
                        case WoWClass.Druid:
                            return StyxWoW.Me.Specialization != WoWSpec.DruidRestoration && StyxWoW.Me.Specialization != WoWSpec.DruidBalance;
                        case WoWClass.Monk:
                            return StyxWoW.Me.Specialization != WoWSpec.MonkMistweaver;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// get the effective distance between two mobs accounting for their 
        /// combat reaches (hitboxes)
        /// </summary>
        /// <param name="unit">unit</param>
        /// <param name="other">Me if null, otherwise second unit</param>
        /// <returns></returns>
        public static float SpellDistance(this WoWUnit unit, WoWUnit other = null)
        {
            // abort if mob null
            if (unit == null)
                return 0;

            // optional arg implying Me, then make sure not Mob also
            if (other == null)
                other = StyxWoW.Me;

            // pvp, then keep it close
            float dist = other.Location.Distance(unit.Location);
            dist -= other.CombatReach + unit.CombatReach;
            return Math.Max(0, dist);
        }

        public static bool IsSensitiveDamage(this WoWUnit u, float range)
        {
            if (u == StyxWoW.Me.CurrentTarget)
                return false;

            if (!u.Combat && !u.IsPlayer && u.IsNeutral)
                return true;

            if (u.SpellDistance() > range)
                return false;

            return u.IsCrowdControlled();
        }
        #endregion Extensions
    }
}