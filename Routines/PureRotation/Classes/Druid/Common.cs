#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-26 21:34:50 +0200 (Do, 26 Sep 2013) $
 * $ID$
 * $Revision: 1740 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Druid/Common.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Core;
using PureRotation.Core.CombatLog;
using PureRotation.Helpers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Lua = Styx.WoWInternals.Lua;

namespace PureRotation.Classes.Druid
{
    /// <summary>
    ///     Common Druid Functions.
    /// </summary>
    public static class Common
    {
        private static bool _combatLogAttached;
        public static double mastery;
        public static IEnumerable<WoWUnit> _nearbyUnfriendlyUnits;

        /// <summary>
        ///     erm..me
        /// </summary>
        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        public static bool IsBehind
        {
            get { return Me.CurrentTarget != null && StyxWoW.Me.IsBehind(Me.CurrentTarget); }
        }

        #region Mushrooms

        internal static int MushroomCount
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>()
                                 .Where(o => o.Entry == 47649 && o.Distance <= 40)
                                 .Count(o => o.CreatedByUnitGuid == StyxWoW.Me.Guid);
            }
        }

        //Best AoE for Resto
        public static WoWUnit StackedUnit
        {
            get { return GetBestUnitForCluster(Unit.GroupMembers, ClusterType.Radius, 8f); }
        }

        internal static WoWUnit AnyMushrooom
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                                 .FirstOrDefault(
                                     o => o.Entry == 47649 && o.Distance <= 40 && o.CreatedByUnitGuid == StyxWoW.Me.Guid);
            }
        }

        internal static IEnumerable<WoWPlayer> MushroomUnits
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
                                 .Where(
                                     u =>
                                     !u.IsDead && u.CanSelect && u.IsInMyPartyOrRaid &&
                                     u.HealthPercent < DruidSettings.HP_Mushroom);
            }
        }

        /// <summary>
        ///     Best AoE for Balance
        /// </summary>
        internal static WoWUnit BestAoeTarget
        {
            get
            {
                return GetBestUnitForCluster(Unit.NearbyAttackableUnits(Me.Location, 40).Where(u => u.Combat),
                                             ClusterType.Radius, 8f);
            }
        }

        internal static WoWUnit GetBestUnitForCluster(IEnumerable<WoWUnit> units, ClusterType type, float clusterRange)
        {
            if (!units.Any())
                return null;

            switch (type)
            {
                case ClusterType.Radius:
                    return
                        (units.Select(u => new { Count = GetRadiusClusterCount(u, units, clusterRange), Unit = u }))
                            .OrderByDescending(a => a.Count).
                             FirstOrDefault().Unit;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        internal static IEnumerable<WoWUnit> GetRadiusCluster(WoWUnit target, IEnumerable<WoWUnit> otherUnits,
                                                              float radius)
        {
            WoWPoint targetLoc = target.Location;
            return otherUnits.Where(u => u.Location.DistanceSqr(targetLoc) <= radius * radius);
        }

        internal static int GetRadiusClusterCount(WoWUnit target, IEnumerable<WoWUnit> otherUnits, float radius)
        {
            return GetRadiusCluster(target, otherUnits, radius).Count();
        }

        #endregion Mushrooms

        public static void Initialize()
        {
            AttachCombatLogEvent();
        }

        private static void AttachCombatLogEvent()
        {
            CombatLogHandler.Register("SPELL_AURA_APPLIED", HandleCombatLog);
            CombatLogHandler.Register("SPELL_AURA_REFRESH", HandleCombatLog);
            CombatLogHandler.Register("SPELL_DAMAGE", HandleCombatLog);
            CombatLogHandler.Register("SPELL_AURA_REMOVED", HandleCombatLog);
        }

        private static void HandleCombatLog(CombatLogEventArgs args)
        {
            //var e = new CombatLogEventArgs(args.EventName, args.FireTimeStamp, args.Args);
            switch (args.Event)
            {
                case "SPELL_AURA_APPLIED":
                    if (args.SourceGuid != 0 && args.SourceGuid != 1 && args.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (args.SpellId == 1822)
                        {
                            // DO NOT EDIT THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
                            // FOR FUCKS SAKE: THIS IS CORRECT! :O
                            Feral._dot_rake_multiplier = Feral.rake_calc;
                        }
                        if (args.SpellId == 1079)
                        {
                            // DO NOT EDIT THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
                            // FOR FUCKS SAKE: THIS IS CORRECT! :O
                            Feral._ExtendedRip = 0;
                            Feral._dot_rip_multiplier = Feral.rip_calclogger;
                        }
                    }
                    break;

                case "SPELL_AURA_REFRESH":
                    if (args.SourceGuid != 0 && args.SourceGuid != 1 && args.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (args.SpellId == 1822)
                        {
                            // DO NOT EDIT THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
                            // FOR FUCKS SAKE: THIS IS CORRECT! :O
                            Feral._dot_rake_multiplier = Feral.rake_calc;
                        }
                        if (args.SpellId == 1079)
                        {
                            // DO NOT EDIT THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
                            // FOR FUCKS SAKE: THIS IS CORRECT! :O
                            Feral._ExtendedRip = 0;
                            Feral._dot_rip_multiplier = Feral.rip_calclogger;
                        }
                    }
                    break;

                case "SPELL_DAMAGE":
                    if (args.SourceGuid != 0 && args.SourceGuid != 1 && args.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.HasMyAura("Rip"))
                        {
                            if (args.SpellId == 5221 || args.SpellId == 114236 || args.SpellId == 102545 || args.SpellId == 33876)

                                //Normal Shred, Glyphed Shred, Ravage, Mangle
                                Feral._ExtendedRip = Feral._ExtendedRip + 1;
                        }
                    }
                    break;

                case "SPELL_AURA_REMOVED":
                    if (args.SourceGuid != 0 && args.SourceGuid != 1 && args.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (args.SpellId == 1822)
                        {
                            Feral._dot_rake_multiplier = 0;
                        }
                        if (args.SpellId == 1079)
                        {
                            Feral._ExtendedRip = 0;
                            Feral._dot_rip_multiplier = 0;
                        }
                    }
                    break;
            }
        }

        public static double DamageMultiplier()
        {
            double multiplier = 1;

            //TF
            if (buff.tigers_fury.up)
                multiplier = multiplier * 1.15;

            //Savage Roar
            if (buff.savage_roar.up)
                multiplier = multiplier * 1.3;

            //Doc
            if (buff.dream_of_cenarius_damage.up)
                multiplier = multiplier * 1.25;
            if (buff.natures_vigil.up)
                multiplier = multiplier * 1.2;
            return multiplier;
        }

        public static int Units()
        {
            _nearbyUnfriendlyUnits = UnfriendlyUnitsNearTarget(10f);
            return _nearbyUnfriendlyUnits.Count();
        }

        /*Get targets around us*/

        public static IEnumerable<WoWUnit> UnfriendlyUnitsNearTarget(float distance)
        {
            float dist = distance * distance;
            WoWPoint curTarLocation = StyxWoW.Me.CurrentTarget.Location;
            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(
                p => ValidUnit(p) && p.Location.DistanceSqr(curTarLocation) <= dist).ToList();
        }

        private static bool ValidUnit(WoWUnit p)
        {
            // Ignore shit we can't select/attack
            if (!p.CanSelect || !p.Attackable)
                return false;

            // Ignore friendlies!
            if (p.IsFriendly)
                return false;

            // Duh
            if (p.IsDead)
                return false;

            // Dummies/bosses are valid by default. Period.
            //if (Unit.UseCooldowns())
            //    return true;

            // If its a pet, lets ignore it please.
            if (p.IsPet || p.OwnedByRoot != null)
                return false;

            // And ignore critters/non-combat pets
            if (p.IsNonCombatPet || p.IsCritter)
                return false;

            return true;
        }

        ///// <summary>
        ///// Handles Shapeshift form.
        ///// </summary>
        //public static Composite HandleShapeshiftForm
        //{
        //    get
        //    {
        //        return new Decorator(
        //                 ret => PRSettings.Instance.EnableMovement,
        //                       new PrioritySelector(
        //                           Spell.CastSelfSpell("Bear Form", ret => TalentManager.CurrentSpec == WoWSpec.DruidGuardian && !Buff.PlayerHasBuff("Bear Form"), "Bear Form"),
        //                           Spell.CastSelfSpell("Cat Form", ret => TalentManager.CurrentSpec == WoWSpec.DruidFeral && !Buff.PlayerHasBuff("Cat Form") && !Buff.PlayerHasBuff("Might of Ursoc"), "Cat Form")));
        //    }
        //}

        ///// <summary>
        ///// Returns true if we have Incarnation up or we are stealthed.
        ///// </summary>
        //public static bool CanRavage
        //{
        //    get
        //    {
        //        return Buff.PlayerHasBuff("Incarnation: King of the Jungle") || Buff.PlayerHasBuff("Prowl");
        //    }
        //}

        //public static string lastEclipse;

        ///// <summary>
        ///// Checks which Eclipse we have if any, and returns which direction we are going
        ///// </summary>
        ///// <returns>A numeric value for Right/Left or Centered && our last gained Eclipse</returns>
        //public static int goingDir()
        //{
        //    //We have the Lunar buff and are casting at Starfire increments(Me.CurrentEclipse == -100 || Me.CurrentEclipse == -80 || Me.CurrentEclipse == -60 || Me.CurrentEclipse == -40 || Me.CurrentEclipse == -20))
        //    if (Buff.PlayerHasBuff("Eclipse (Lunar)") && Me.CurrentEclipse >= -100 && Me.CurrentEclipse <= -1)
        //    {
        //        lastEclipse = "Lunar";
        //        return 1;
        //    }
        //    //We have no buff but are still casting at Starfire increments(Me.CurrentEclipse == 0 || Me.CurrentEclipse == +40 || Me.CurrentEclipse == +80))
        //    if (!Buff.PlayerHasBuff("Eclipse (Lunar)") && !Buff.PlayerHasBuff("Eclipse (Solar)") && Me.CurrentEclipse >= 0 && lastEclipse == "Lunar")//and lastEclipse == "Lunar"
        //    {
        //        return 1;
        //    }
        //    //We have the solar buff and are casting at Wrath increments(Me.CurrentEclipse == +100 || Me.CurrentEclipse == +85 || Me.CurrentEclipse == +70 || Me.CurrentEclipse == +55 || Me.CurrentEclipse == +40 || Me.CurrentEclipse == +25 || Me.CurrentEclipse == +10))
        //    if (Buff.PlayerHasBuff("Eclipse (Solar)") && Me.CurrentEclipse <= 100 && Me.CurrentEclipse >= 1)
        //    {
        //        lastEclipse = "Solar";
        //        return -1;
        //    }
        //    //We have no buff and are still casting at Wrath increments(Me.CurrentEclipse == -5 || Me.CurrentEclipse == -35 || Me.CurrentEclipse == -65 || Me.CurrentEclipse == -95))
        //    if (!Buff.PlayerHasBuff("Eclipse (Solar)") && !Buff.PlayerHasBuff("Eclipse (Lunar)") && Me.CurrentEclipse <= 0 && lastEclipse == "Solar")//and lastEclipse == "Solar"
        //    {
        //        return -1;
        //    }
        //    //Assume we have yet to start combat and we can go either direction
        //    return 0;
        //}

        /*

                /// <summary>
                /// Returns the Eclipse direction we are going
                /// </summary>
                /// <returns>Eclipse direction</returns>
                public static int eclipseDir()
                {
                    return goingDir();
                }
        */
    }
}