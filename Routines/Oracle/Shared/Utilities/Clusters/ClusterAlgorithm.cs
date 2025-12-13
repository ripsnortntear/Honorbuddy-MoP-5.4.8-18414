#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/ClusterAlgorithm.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.Shared.Utilities.Clusters.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Shared.Utilities.Clusters
{
    /*
      Clusters (Dictionary of clusters)
        -->  Cluster (holds a list of points and a centre point)
               -->  Points (List of Points)
                    -->  CentrePoint (the point that is the centre of the Cluster)
      */

    public abstract class ClusterAlgorithm
    {
        //id, Cluster
        protected readonly Dictionary<string, Cluster> Clusters = new Dictionary<string, Cluster>();

        protected List<Points> AllPoints; // all points

        protected ClusterAlgorithm()
        {
        }

        protected ClusterAlgorithm(List<Points> points)
        {
            if (points == null || points.Count == 0)
                throw new ApplicationException(
                    string.Format("List of Points is null or empty"));

            AllPoints = points;
        }

        public abstract List<Points> GetCluster(int clusterRadius, ClusterType type);

        protected List<Points> GetClusterResult()
        {
            Profile.Output("GetClusterResult begin");

            // collect used Clusters and return the result
            var clusterPoints = new List<Points>();
            foreach (var item in Clusters)
            {
                var cluster = item.Value;
                if (cluster.IsUsed)
                {
                    cluster.CentrePoint.Size = cluster.Points.Count;
                    if (cluster.ClusterType == ClusterType.Party)
                        cluster.CentrePoint.GroupSize = cluster.Points.Count;

                    // Assign the clustertype.
                    cluster.CentrePoint.PointClusterType = cluster.ClusterType;

                    // Add the avg health of the units to the centres AvgHealthPct property
                    cluster.CentrePoint.AvgHealthPct = cluster.Points.Count == 0 ? (int)cluster.CentrePoint.HealthPercent : (int)cluster.Points.Average(player => player.HealthPercent);

                    // Add the healthpercents of the units and the actual unit to the centres Healthpercents property.
                    foreach (var point in cluster.Points)
                    {
                        cluster.CentrePoint.HealthPctList.Add(point.HealthPercent);
                        cluster.CentrePoint.ClusterMembers.Add(point.Player);
                    }

                    // finally add it to the list of clusterPoints.
                    clusterPoints.Add(cluster.CentrePoint);
                }
            }

            return clusterPoints;
        }

        private Points GetClosestPoint(Points from, List<Points> list) //O(n)
        {
            double min = double.MaxValue;
            Points closests = null;
            foreach (Points p in list)
            {
                double d = MathTool.Distance(from, p);
                if (d >= min)
                    continue;

                // update
                min = d;
                closests = p;
            }
            return closests;
        }

        public Points GetLongestPoint(Points from, List<Points> list) //O(n)
        {
            double max = -double.MaxValue;
            Points longest = null;
            foreach (Points p in list)
            {
                double d = MathTool.Distance(from, p);
                if (d <= max)
                    continue;

                // update
                max = d;
                longest = p;
            }
            return longest;
        }

        // assign all points to nearest cluster
        protected void UpdatePointsByCentre(int clusterRadius, ClusterType clusterType) //O(n*k)
        {
            Profile.Output("UpdatePointsByCentre begin");

            // clear points in the clusters, they will be re-inserted
            foreach (var cluster in Clusters.Values)
                cluster.Points.Clear();

            foreach (Points point in AllPoints)
            {
                double minDist = (clusterType == ClusterType.NearbyLowestHealth) ? clusterRadius : Double.MaxValue;
                string index = string.Empty;

                foreach (string i in Clusters.Keys)
                {
                    var cluster = Clusters[i];
                    if (cluster.IsUsed == false)
                        continue;

                    Points centrePoint = cluster.CentrePoint;
                    double dist = MathTool.Distance(point, centrePoint);
                    if (dist < minDist)
                    {
                        // update
                        minDist = dist;
                        index = i;
                    }
                }

                //update color for point to match the centrePoint and re-insert
                if (index == string.Empty) continue;
                var closestCluster = Clusters[index];
                point.Color = closestCluster.CentrePoint.Color;
                closestCluster.Points.Add(point);
            }
        }

        // update centrePoint location to nearest point,
        // e.g. if you want to show cluster point on a real existing point area
        // O(n)
        private void UpdateCentrePointToNearestContainingPoint(Cluster cluster)
        {
            Profile.Output("UpdateCentrePointToNearestContainingPoint begin");
            if (cluster == null || cluster.CentrePoint == null ||
                cluster.Points == null || cluster.Points.Count == 0)
                return;

            Points closest = GetClosestPoint(cluster.CentrePoint, cluster.Points);
            cluster.CentrePoint.X = closest.X;
            cluster.CentrePoint.Y = closest.Y;
        }

        //O(k*n)
        public void UpdateAllCentrePointsToNearestContainingPoint()
        {
            Profile.Output("UpdateAllCentreToNearestContainingPoint begin");
            foreach (var cluster in Clusters.Values)
                UpdateCentrePointToNearestContainingPoint(cluster);
        }
    }
}