#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/DistanceCluster.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.Shared.Utilities.Clusters.Utility;
using Styx;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Oracle.Shared.Utilities.Clusters
{
    public class DistanceCluster : ClusterAlgorithm
    {
        public DistanceCluster(List<Points> points)
            : base(points)
        {
        }

        public override List<Points> GetCluster(int clusterRadius, ClusterType type)
        {
            var cluster = RunClusterAlgorithm(clusterRadius, type);
            return cluster;
        }

        // O(k*n)
        private List<Points> RunClusterAlgorithm(int clusterRadius, ClusterType clusterType)
        {
            if (!OracleRoutine.IsViable(StyxWoW.Me)) return new List<Points>();

            // GetGroupNumber is a major performance drain.
            var localPlayer = new Points(StyxWoW.Me.X, StyxWoW.Me.Y, StyxWoW.Me, 100, -1 /*(int)StyxWoW.Me.GetGroupNumber()*/);

            // put points in Clusters
            int allPointsCount = AllPoints.Count;
            Points firstPoint = (clusterType == ClusterType.NearbyLowestHealth) ? AllPoints.Find(a => a.Player == StyxWoW.Me) : AllPoints[0];

            if (firstPoint == null) return new List<Points> { localPlayer }; // just return us so we dont get null refs.

            firstPoint.Color = GetRandomColor();
            string firstId = 0.ToString(CultureInfo.InvariantCulture);
            var firstCluster = new Cluster(firstId) { CentrePoint = firstPoint, ClusterType = clusterType };
            Clusters.Add(firstId, firstCluster);

            for (int i = 1; i < allPointsCount; i++)
            {
                var set = new HashSet<string>(); //cluster candidate list
                var point = AllPoints[i];

                // iterate clusters and collect candidates
                foreach (var cluster in Clusters.Values)
                {
                    bool isInCluster = MathTool.DistWithin(point, cluster.CentrePoint, clusterRadius);
                    if (!isInCluster)
                        continue;

                    bool isInGroup = point.GroupNumber == cluster.CentrePoint.GroupNumber;
                    if (clusterType == ClusterType.Party && !isInGroup)
                        continue;

                    set.Add(cluster.Id);
                    //use first, short dist will be calc at last step before returning data
                    break;
                }

                // if not within box area, then make new cluster
                if (set.Count == 0 && clusterType != ClusterType.NearbyLowestHealth)
                {
                    string pid = i.ToString(CultureInfo.InvariantCulture);
                    point.Color = GetRandomColor();
                    var newCluster = new Cluster(pid) { CentrePoint = point, ClusterType = clusterType };
                    Clusters.Add(pid, newCluster);
                }
            }

            //important, align all points to closest cluster point
            UpdatePointsByCentre(clusterRadius, clusterType);

            return GetClusterResult();
        }

        public static readonly Random Rand = new Random();

        public static string GetRandomColor()
        {
            int r = Rand.Next(10, 250);
            int g = Rand.Next(10, 250);
            int b = Rand.Next(10, 250);
            return string.Format("'rgb({0},{1},{2})'", r, g, b);
        }
    }
}