#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Point.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Utilities.Clusters.Data;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Oracle.Shared.Utilities.Clusters
{
    [Serializable]
    public class Points : IComparable, ISerializable
    {
        private const int ROUND = 6;

        public Points()
        {
        }

        public Points(double x, double y, WoWUnit player, double healthPct, int grpNum)
        {
            X = x;
            Y = y;
            Color = "'rgb(0,0,0)'"; //default
            Player = player;
            HealthPercent = healthPct;
            GroupNumber = grpNum;
            HealthPctList = new List<double>();
            ClusterMembers = new List<WoWUnit>();
        }

        public Points(Points p) //clone
        {
            X = p.X;
            Y = p.Y;
            Color = p.Color;
            Size = p.Size;
            Player = p.Player;
            HealthPercent = p.HealthPercent;
            GroupNumber = p.GroupNumber;
            PointClusterType = p.PointClusterType;
            HealthPctList = new List<double>();
            ClusterMembers = new List<WoWUnit>();
        }

        public Points(SerializationInfo info, StreamingContext ctxt)
        {
            X = (double)info.GetValue("X", typeof(double));
            Y = (double)info.GetValue("Y", typeof(double));
            Color = (string)info.GetValue("Color", typeof(string));
            Size = (int)info.GetValue("Size", typeof(int));
        }

        public int AvgHealthPct { get; set; }

        public List<double> HealthPctList { get; private set; }

        public List<WoWUnit> ClusterMembers { get; private set; }

        public string Color { get; set; }

        public int GroupNumber { get; set; }

        public int GroupSize { get; set; }

        public double HealthPercent { get; set; }

        public WoWUnit Player { get; set; }

        public ClusterType PointClusterType { get; set; }

        public int Size { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double AverageHealth(int count, int hp)
        {
            var results = HealthPctList.FindAll(health => health < hp);

            var divisor = results.Count;

            var sumHealthPercent = results.Sum();

            if (divisor >= count && divisor > 0)
                return Math.Round(sumHealthPercent / divisor, 0);

            return 0;
        }

        public int CompareTo(object o) // if used in sorted list
        {
            if (Equals(o))
                return 0;

            var other = (Points)o;
            if (X > other.X)
                return -1;
            if (X < other.X)
                return 1;

            return 0;
        }

        public override bool Equals(Object o)
        {
            if (o == null)
                return false;
            var other = o as Points;
            if (other == null)
                return false;

            // rounding could be skipped
            // depends on granularity of wanted decimal precision
            // note, 2 points with same x,y is regarded as being equal
            bool x = Math.Round(X, ROUND) == Math.Round(other.X, ROUND);
            bool y = Math.Round(Y, ROUND) == Math.Round(other.Y, ROUND);
            return x && y;
        }

        // random distinct selection of cluster point
        public override int GetHashCode()
        {
            double x = X * 10000; //make the decimals be important
            double y = Y * 10000;
            double r = x * 17 + y * 37;
            return (int)r;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("X", X);
            info.AddValue("Y", Y);
            info.AddValue("Color", Color);
            info.AddValue("Size", Size);
        }
    }
}