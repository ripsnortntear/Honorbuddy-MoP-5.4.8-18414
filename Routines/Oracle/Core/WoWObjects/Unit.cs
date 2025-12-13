#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/WoWObjects/Unit.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Classes.Priest;
using Oracle.Core.DataStores;
using Oracle.Core.Groups;
using Oracle.Core.Spells.Auras;
using Oracle.Shared.Utilities;
using Oracle.UI;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.WoWObjects
{
    internal static class Unit
    {
        #region settings

        private static OracleSettings Settings { get { return OracleSettings.Instance; } }

        private static bool EnableProvingGrounds { get { return Settings.EnableProvingGrounds; } }

        private static bool PvPSupport { get { return Settings.PvPSupport; } }

        #endregion settings

        static Unit()
        {
            EnemyPriorities = new List<WoWUnit>();
            FriendlyPriorities = new List<WoWUnit>();
            NPCPriorities = new List<WoWUnit>();
        }

        public static List<WoWUnit> EnemyPriorities { get; private set; }

        public static List<WoWUnit> FriendlyPriorities { get; private set; }

        public static List<WoWUnit> NPCPriorities { get; private set; }

        public static void PulseEnemy()
        {
            EnemyPriorities.Clear();

            foreach (var u in SearchAreaUnits())
            {
                if (!OracleRoutine.IsViable(u)) continue;

                // Dead. Ignore 'em
                if (u.IsDead || u.Distance > 40)
                    continue;

                if (!u.Attackable || !u.CanSelect)
                    continue;

                if (u.IsFriendly)
                    continue;

                if (u.IsNonCombatPet && u.IsCritter)
                    continue;

                EnemyPriorities.Add(u);
            }
        }

        public static void PulseFriendly()
        {
            FriendlyPriorities.Clear();

            var results = new List<WoWUnit>();

            if (EnableProvingGrounds)
            {
                foreach (var p in Unit.ProvingGroundNPCs())
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    results.Add(p);
                    Group.UpdateOraclePartyMember(p.Guid, 1000); // Cache party member GUIDS.
                }

                FriendlyPriorities = new List<WoWUnit>(results.OrderByDescending(u => u.HealthPercent(u.GetHPCheckType())));
            }
            else
            {
                foreach (var p in SearchAreaPlayers())
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    // Dead. Ignore 'em
                    if (p.IsDead || p.IsGhost || p.Distance > 40) // Max heal distance is 40.
                        continue;

                    Group.OraclePartyMember guidresults;
                    if (Group.OraclePartyMemberEntries.TryGetValue(p.Guid, out guidresults))
                    {
                        // Logger.Output("-> {1} cached Party member found at [{0}]", guidresults.GUID, guidresults.CurrentTime);
                    }
                    else
                    {
                        if (!p.IsInMyPartyOrRaid)
                            continue;

                        //Logger.Output(" -> Adding {1} Party member at [{0}] ", p.Guid, DateTime.Now);
                    }

                    if (!p.IsMe && !OracleLineOfSight.InLineOfSight(p)) // LoS..again Ignore.
                        continue;

                    results.Add(p);
                    Group.UpdateOraclePartyMember(p.Guid, 1000); // Cache party member GUIDS.
                }

                FriendlyPriorities = new List<WoWUnit>(results.OrderByDescending(u => u.HealthPercent(u.GetHPCheckType())));
            }
        }

        public static void PulseNPC()
        {
            NPCPriorities.Clear();

            foreach (var npc in SearchAreaUnits())
            {
                if (!OracleRoutine.IsViable(npc)) continue;

                if (!HashSets.HealableNPCs.Contains((int)npc.Entry))
                    continue;

                // Dead. Ignore 'em
                if (npc.IsDead || npc.Distance > 60) // Max heal distance is 40.
                    continue;

                if (!npc.IsMe && !OracleLineOfSight.InLineOfSight(npc)) // LoS..again Ignore.
                    continue;

                NPCPriorities.Add(npc);
            }
        }

        private static bool DoNotDamage(WoWUnit unit)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            if (unit.Entry == 71515) return unit.HasAura(143593); // General Nazgrim do not attack when defensive stance.
            return PvPSupport && unit.HasAnyAura(HashSets.DoNotDamageList);
        }

        public static bool FaceTarget(WoWUnit unit)
        {
            using (new PerformanceLogger("FaceTarget"))
            {
                if (!OracleRoutine.IsViable(unit) || unit.Entry == 60410) return true;

                if (StyxWoW.Me.IsMoving) return true;

                if (!StyxWoW.Me.IsSafelyFacing(unit))
                {
                    unit.Face();
                }
                return true;
            }
        }

        //public static bool IsInLoS(WoWUnit unit)
        //{
        //    if (unit != null && !unit.IsValid) return new TimeCachedValue<bool>(TimeSpan.FromMilliseconds(5), () => unit.InLineOfSight);
        //    return false;
        //}

        public static int GetbuffCount(int buff)
        {
            using (new PerformanceLogger("GetbuffCount"))
            {
                var results = new List<WoWUnit>();

                if (EnableProvingGrounds)
                {
                    foreach (var p in SearchAreaUnits())
                    {
                        if (!OracleRoutine.IsViable(p)) continue;

                        if (!HealingSelector.SelectiveHealingPlayers.ContainsKey(p.Guid))
                            continue;

                        if (!p.HasAura(buff))
                            continue;

                        results.Add(p);
                    }
                }
                else
                {
                    foreach (var p in FriendlyPriorities.Union(NPCPriorities))
                    {
                        if (!OracleRoutine.IsViable(p)) continue;

                        if (!p.HasAura(buff))
                            continue;

                        results.Add(p);
                    }
                }

                //Logger.Output(" {0} count is : {1}", WoWSpell.FromId(buff).Name, results.Count);

                return results.Count;
            }
        }

        public static WoWUnit GetEnemy
        {
            get
            {
                using (new PerformanceLogger("GetEnemy"))
                {
                    if (StyxWoW.Me.Class == WoWClass.Priest && PriestCommon.HasTalent(PriestTalents.TwistOfFate))
                    {
                        var bitchSlapUnit = GetLowHealthUnit();
                        if (OracleRoutine.IsViable(bitchSlapUnit) && !DoNotDamage(bitchSlapUnit))
                            return bitchSlapUnit;
                    }

                    if (OracleRoutine.IsViable(StyxWoW.Me.FocusedUnit) && StyxWoW.Me.FocusedUnit.IsHostile &&
                        StyxWoW.Me.FocusedUnit.Distance < 40 && !DoNotDamage(StyxWoW.Me.FocusedUnit))
                    {
                        return StyxWoW.Me.FocusedUnit;
                    }

                    if (OracleRoutine.IsViable(StyxWoW.Me.CurrentTarget) && StyxWoW.Me.CurrentTarget.IsHostile &&
                        StyxWoW.Me.CurrentTarget.Distance < 40 && !DoNotDamage(StyxWoW.Me.CurrentTarget))
                    {
                        return StyxWoW.Me.CurrentTarget;
                    }

                    var tank = OracleTanks.PrimaryTank;
                    if (OracleRoutine.IsViable(tank) && OracleRoutine.IsViable(tank.CurrentTarget) &&
                        tank.CurrentTarget.Distance < 40 && !DoNotDamage(tank.CurrentTarget)) return tank.CurrentTarget;

                    return null;
                }
            }
        }

        public static WoWUnit EnemyFromPointLocation(WoWPoint pt, int dist)
        {
            var maxDistance = dist * dist;

            foreach (var u in EnemyPriorities)
            {
                if (!OracleRoutine.IsViable(u)) continue;

                if (pt.Distance(u.Location) <= maxDistance)
                    return u;
            }
            return null;
        }

        public static WoWUnit GetLowHealthUnit()
        {
            foreach (var u in EnemyPriorities)
            {
                if (!OracleRoutine.IsViable(u)) continue;

                if (HashSets.InvalidNPCs.Contains((int)u.Entry)) continue;

                if (u.HealthPercent(u.GetHPCheckType()) < 35) // 5.4 changed from 20%
                    return u;
            }
            return null;
        }

        public static WoWUnit GetActiveUnbuffedTarget(string withoutBuff)
        {
            using (new PerformanceLogger("GetUnbuffedTarget"))
            {
                var results = new List<WoWUnit>();

                var units = FriendlyPriorities.Union(NPCPriorities);

                foreach (var p in units)
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    if (p.ActiveAuras.ContainsKey(withoutBuff))
                        continue;

                    results.Add(p);
                }

                return results.OrderByDescending(u => u.HealthPercent(u.GetHPCheckType())).FirstOrDefault();
            }
        }

        public static WoWUnit GetUnbuffedTarget(HashSet<int> withoutBuff)
        {
            using (new PerformanceLogger("GetUnbuffedTarget"))
            {
                var results = new List<WoWUnit>();

                var units = FriendlyPriorities.Union(NPCPriorities);

                foreach (var p in units)
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    if (p.HasAnyAura(withoutBuff))
                        continue;

                    results.Add(p);
                }

                return results.OrderByDescending(u => u.HealthPercent(u.GetHPCheckType())).FirstOrDefault();
            }
        }

        public static WoWUnit GetUnbuffedTarget(int withoutBuff)
        {
            using (new PerformanceLogger("GetUnbuffedTarget"))
            {
                var results = new List<WoWUnit>();

                var units = FriendlyPriorities.Union(NPCPriorities);

                foreach (var p in units)
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    if (p.HasAura(withoutBuff))
                        continue;

                    results.Add(p);
                }

                return results.OrderByDescending(u => u.HealthPercent).FirstOrDefault();
            }
        }

        public static WoWUnit GetUnbuffedTarget(string withoutBuff)
        {
            using (new PerformanceLogger("GetUnbuffedTarget"))
            {
                var results = new List<WoWUnit>();

                var units = FriendlyPriorities.Union(NPCPriorities);

                foreach (var p in units)
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    if (p.HasAura(withoutBuff))
                        continue;

                    results.Add(p);
                }

                return results.OrderByDescending(u => u.HealthPercent(u.GetHPCheckType())).FirstOrDefault();
            }
        }

        public static IEnumerable<WoWUnit> ProvingGroundNPCs()
        {
            using (new PerformanceLogger("ProvingGroundNPCs"))
            {
                var results = new List<WoWUnit>();

                foreach (var p in SearchAreaUnits())
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    if (!HealingSelector.SelectiveHealingPlayers.ContainsKey(p.Guid))
                        continue;

                    results.Add(p);
                }

                return results;
            }
        }

        public static IEnumerable<WoWPlayer> SearchAreaPlayers()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>();
        }

        public static IEnumerable<WoWUnit> SearchAreaUnits()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>();
        }
    }
}