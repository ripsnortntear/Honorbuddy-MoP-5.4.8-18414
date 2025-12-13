#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/ClusterManager.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using Oracle.Classes.Shaman;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.Shared.Utilities.Clusters.Utility;
using Styx;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Shared.Utilities.Clusters
{
    [UsedImplicitly]
    internal class ClusterManager
    {
        //------------------------------
        // USER CONFIG, CUSTOMIZE BELOW

        public const bool UseProfiling = true; //debug, output time spend

        // USER CONFIG, CUSTOMIZE ABOVE
        //------------------------------

        // Main Lists..
        public static List<Points> GroundPoints = new List<Points>();

        public static List<Points> PoximityPoints = new List<Points>();
        public static List<Points> NearbyPoints = new List<Points>();
        public static List<Points> PartyPoints = new List<Points>();

        // Utility...
        public static List<Points> Clusters = new List<Points>();

        public static DateTime Starttime;

        public static void Pulse()
        {
            Starttime = DateTime.Now;

            // Reset Lists.
            GroundPoints = new List<Points>();
            PoximityPoints = new List<Points>();
            NearbyPoints = new List<Points>();
            PartyPoints = new List<Points>();
            Clusters = new List<Points>();

            // create points in memory
            GeneratedData.CreateDataSet(DataSetType.Players);

            // 1.2. Populate Clusters Executed in 0.3797 ms
            Run();

            // Make sure you have created [C:\temp] folder - open canvas.html to view clusters.
            // Change the size and clustertype to view output...
            // FileUtil.GenerateJavascriptDrawFile(PARTYCLUSTERER_SIZE, ClusterType.Party); //create draw.js

            double timespend = DateTime.Now.Subtract(Starttime).TotalSeconds;
            // Logger.Output("[ClusterPerformance] Executed in {0} sec", timespend);
        }

        private static void Run()
        {
            var clusterRadius = GetClusterRadius();

            if (GroundPoints.Count != 0)
                Clusters.Add(GetCluster(GroundPoints, clusterRadius.ElementAt(0), ClusterType.GroundEffect));

            if (NearbyPoints.Count != 0)
                Clusters.Add(GetCluster(NearbyPoints, clusterRadius.ElementAt(1), ClusterType.NearbyLowestHealth));

            if (PoximityPoints.Count != 0)
                Clusters.Add(GetCluster(PoximityPoints, clusterRadius.ElementAt(2), ClusterType.Proximity));

            if (PartyPoints.Count != 0)
                Clusters.Add(GetCluster(PartyPoints, clusterRadius.ElementAt(3), ClusterType.Party));

            //Output();
        }

        private static Points GetCluster(List<Points> points, int clusterRadius, ClusterType clusterType)
        {
            return new DistanceCluster(points).GetCluster(clusterRadius, clusterType).OrderByDescending(u => u.Size).ThenBy(u => u.AvgHealthPct).FirstOrDefault();
        }

        public static void Output()
        {
            Logger.Output(" \n");
            Logger.Output("[Clusters]");
            foreach (Points point in Clusters.OrderByDescending(u => u.Size).ThenBy(u => u.AvgHealthPct))
            {
                Profile.Output(string.Format(" \n[{6}]  [{0} units] \n------------------------------- \nCentroid [{1}] at {2}% in group: {3} [{4}] [avgHP: {5}] ", point.Size, point.Player,
                                             point.HealthPercent, point.GroupNumber, point.GroupSize, point.AvgHealthPct, point.PointClusterType));
            }
        }

        // radius size i.e. cluster size
        private static List<int> GetClusterRadius()
        {
            var results = new List<int>();

            // Important: keep these in order!
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Paladin:
                    results.Add(10); // GROUNDCLUSTERER_SIZE
                    results.Add(30); // NEARBYCLUSTERER_SIZE
                    results.Add(15); // PROXIMITYCLUSTERER_SIZE (required to be 15 - Holy Prism is 15yards..Holy Radiance is 10..but meh..)
                    results.Add(30); // PARTYCLUSTERER_SIZE
                    break;

                case WoWClass.Priest:
                    if (StyxWoW.Me.Specialization == WoWSpec.PriestHoly)
                    {
                        results.Add(12); // GROUNDCLUSTERER_SIZE
                        results.Add(40); // NEARBYCLUSTERER_SIZE
                        results.Add(30); // PROXIMITYCLUSTERER_SIZE
                        results.Add(30); // PARTYCLUSTERER_SIZE
                    }

                    if (StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                    {
                        results.Add(15); // GROUNDCLUSTERER_SIZE
                        results.Add(40); // NEARBYCLUSTERER_SIZE
                        results.Add(30); // PROXIMITYCLUSTERER_SIZE
                        results.Add(30); // PARTYCLUSTERER_SIZE
                    }
                    break;

                case WoWClass.Shaman:
                    results.Add(15); // GROUNDCLUSTERER_SIZE
                    results.Add(40); // NEARBYCLUSTERER_SIZE
                    results.Add((int)ShamanCommon.ChainHealHopRange); // PROXIMITYCLUSTERER_SIZE
                    results.Add(30); // PARTYCLUSTERER_SIZE
                    break;

                case WoWClass.Druid:
                    results.Add(12); // GROUNDCLUSTERER_SIZE
                    results.Add(40); // NEARBYCLUSTERER_SIZE
                    results.Add(30); // PROXIMITYCLUSTERER_SIZE
                    results.Add(30); // PARTYCLUSTERER_SIZE
                    break;

                case WoWClass.Monk:
                    results.Add(12); // GROUNDCLUSTERER_SIZE
                    results.Add(40); // NEARBYCLUSTERER_SIZE
                    results.Add(30); // PROXIMITYCLUSTERER_SIZE
                    results.Add(30); // PARTYCLUSTERER_SIZE
                    break;
            }

            return results;
        }
    }
}