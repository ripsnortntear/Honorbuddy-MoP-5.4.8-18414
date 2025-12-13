#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-15 12:34:43 +1000 (Sun, 15 Sep 2013) $
 * $ID$
 * $Revision: 212 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Data/GeneratedData.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info


using Oracle.Core;
using System.Linq;
using Oracle.Core.Encounters;
using Oracle.Core.Groups;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Shared.Utilities.Clusters.Utility;
using Oracle.UI.Settings;
using Styx;
using System.Collections.Generic;

namespace Oracle.Shared.Utilities.Clusters.Data
{
    public static class GeneratedData
    {
   
        #region settings

        private static OracleSettings Settings { get { return OracleSettings.Instance; } }

        private static int MAX_AOE_HP { get { return Settings.MAX_AOE_HP; } }

        private static bool EnableProvingGrounds { get { return Settings.EnableProvingGrounds; } }

        private static bool MalkorokEncounter { get { return Settings.MalkorokEncounter; } }

        #endregion settings

        public static void CreateDataSet(DataSetType type) //O(n)
        {
            switch (type)
            {
                case DataSetType.Units:
                    CreateDataSetAreaUnits();
                    break;

                case DataSetType.Players:
                    CreateDataSetAreaPlayers();
                    break;

                case DataSetType.HealingPriorities:
                    CreateDataSetHealingPriorities();
                    break;

                case DataSetType.Test:
                    break;

                case DataSetType.None:
                    break;
            }
        }

        private static void CreateDataSetAreaPlayers()
        {
            //var test1 = new PerformanceTester(() => CreateDataSet());
            //test1.MeasureExecTime();
            //Logger.Output(string.Format("[VoidPerformance] 1.1.1. Create DataSet Executed in {0} ms", test1.TotalTime.TotalMilliseconds));

            //Create DataSet Executed in 4.3344 ms
            CreateDataSet();

            // OutputPoints();
        }

        public static readonly Dictionary<ulong, int> GroupNumbers = new Dictionary<ulong, int>();

        private static void CreateDataSet()
        {
            if (EnableProvingGrounds)
            {
                foreach (var pgNpc in Unit.ProvingGroundNPCs())
                {
                    double x = pgNpc.X;
                    double y = pgNpc.Y;

                    // Limit AoE HP
                    var healthPct = pgNpc.HealthPercent(HPCheck.AoE);
                    if (healthPct > MAX_AOE_HP)
                        continue;

                    ClusterManager.PartyPoints.Add(new Points(x, y, pgNpc, healthPct, 0));
                    ClusterManager.GroundPoints.Add(new Points(x, y, pgNpc, healthPct, 0));
                    ClusterManager.NearbyPoints.Add(new Points(x, y, pgNpc, healthPct, 0));
                    ClusterManager.PoximityPoints.Add(new Points(x, y, pgNpc, healthPct, 0));

                    //foreach (var p in ClusterManager.PartyPoints)
                    //{
                    //    if (p.Player != null) Logger.Output("{0}",p.Player.Name);
                    //}
                }
            }
            else
            {
                var results = Unit.FriendlyPriorities.Union(Unit.NPCPriorities);

                foreach (var p in results.ToList())
                {
                    double x = p.X;
                    double y = p.Y;
                    int grpNum, groupNumber;

                    // Limit AoE HP
                    var healthPct = p.HealthPercent(HPCheck.AoE); 
                    if (healthPct > MAX_AOE_HP)
                        continue;

                    if (GroupNumbers.TryGetValue(p.Guid, out groupNumber))
                    {
                        grpNum = groupNumber;
                    }
                    else
                    {
                        //Logger.Output("Checked GroupNumber");
                        grpNum = (int)p.ToPlayer().GetGroupNumber();
                        GroupNumbers.Add(p.Guid, grpNum);
                    }

                    //122370 -> Reshaper Life
                    // if (p.HasAura(122370))
                    //     continue;

                    // add us if we have no units..saves null ref when not partied.

                    ClusterManager.PartyPoints.Add(new Points(x, y, p, healthPct, grpNum));
                    ClusterManager.GroundPoints.Add(new Points(x, y, p, healthPct, grpNum));
                    ClusterManager.NearbyPoints.Add(new Points(x, y, p, healthPct, grpNum));
                    ClusterManager.PoximityPoints.Add(new Points(x, y, p, healthPct, grpNum));
                }
            }

            // add us if we have no units..saves null ref when not partied.
            if (ClusterManager.PartyPoints.Count == 0) ClusterManager.PartyPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.GroundPoints.Count == 0) ClusterManager.GroundPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.NearbyPoints.Count == 0) ClusterManager.NearbyPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.PoximityPoints.Count == 0) ClusterManager.PoximityPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
        }

        private static void CreateDataSetAreaUnits()
        {
            foreach (var u in Unit.SearchAreaUnits())
            {
                double x = u.X;
                double y = u.Y;
                var grpNum = -1;
                var healthPct = (int)u.HealthPercent;

                // Dead. Ignore 'em
                if (u.IsDead || u.Distance > 60)
                    continue;

                if (!u.Attackable || !u.CanSelect)
                    continue;

                if (u.IsFriendly)
                    continue;

                if (u.IsNonCombatPet && u.IsCritter)
                    continue;

                // Limit AoE HP
                if (healthPct > MAX_AOE_HP)
                    continue;

                ClusterManager.PartyPoints.Add(new Points(x, y, u, healthPct, grpNum));
                ClusterManager.GroundPoints.Add(new Points(x, y, u, healthPct, grpNum));
                ClusterManager.NearbyPoints.Add(new Points(x, y, u, healthPct, grpNum));
                ClusterManager.PoximityPoints.Add(new Points(x, y, u, healthPct, grpNum));
            }

            // add us if we have no units..saves null ref when not partied.
            if (ClusterManager.PartyPoints.Count == 0) ClusterManager.PartyPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.GroundPoints.Count == 0) ClusterManager.GroundPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.NearbyPoints.Count == 0) ClusterManager.NearbyPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.PoximityPoints.Count == 0) ClusterManager.PoximityPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));

            // OutputPoints();
        }

        private static void CreateDataSetHealingPriorities()
        {
            foreach (var p in OracleHealTargeting.HealingPriorities)
            {
                double x = p.X;
                double y = p.Y;
                var healthPct = (int)p.HealthPercent(HPCheck.AoE);

                // Limit AoE HP
                if (healthPct > MAX_AOE_HP && !p.IsMe)
                    continue;

                int grpNum, groupNumber;
                if (GroupNumbers.TryGetValue(p.Guid, out groupNumber))
                {
                    grpNum = groupNumber;
                }
                else
                {
                    //Logger.Output("Checked GroupNumber");
                    grpNum = (int)p.ToPlayer().GetGroupNumber();
                    GroupNumbers.Add(p.Guid, grpNum);
                }

                ClusterManager.PartyPoints.Add(new Points(x, y, p, healthPct, grpNum));
                ClusterManager.GroundPoints.Add(new Points(x, y, p, healthPct, grpNum));
                ClusterManager.NearbyPoints.Add(new Points(x, y, p, healthPct, grpNum));
                ClusterManager.PoximityPoints.Add(new Points(x, y, p, healthPct, grpNum));
            }

            // add us if we have no units..saves null ref when not partied.
            if (ClusterManager.PartyPoints.Count == 0) ClusterManager.PartyPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.GroundPoints.Count == 0) ClusterManager.GroundPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.NearbyPoints.Count == 0) ClusterManager.NearbyPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));
            if (ClusterManager.PoximityPoints.Count == 0) ClusterManager.PoximityPoints.Add(new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, StyxWoW.Me.HealthPercent, -1));

            // OutputPoints();
        }

        private static void OutputPoints()
        {
            foreach (var name in ClusterManager.PartyPoints)
            {
                Profile.Output(string.Format("{0}", name.Player));
            }
            Profile.Output(string.Format("{0}", ClusterManager.PartyPoints.Count));
        }
    }
}