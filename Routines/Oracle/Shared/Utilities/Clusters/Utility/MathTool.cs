#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Utility/MathTool.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using System;

namespace Oracle.Shared.Utilities.Clusters.Utility
{
    [UsedImplicitly]
    public static class MathTool
    {
        private const double Exp = 2; // 2=euclid, 1=manhatten

        //Minkowski dist
        public static double Distance(Points a, Points b)
        {
            return Math.Pow(Math.Pow(Math.Abs(a.X - b.X), Exp) +
                            Math.Pow(Math.Abs(a.Y - b.Y), Exp), 1.0 / Exp);
        }

        public static double Min(double a, double b)
        {
            return a <= b ? a : b;
        }

        public static double Max(double a, double b)
        {
            return a >= b ? a : b;
        }

        public static bool DistWithin(Points a, Points b, double d)
        {
            double dist = Distance(a, b);
            return dist < d;
        }

        public static bool BoxWithin(Points a, Points b, double boxsize)
        {
            double d = boxsize / 2;
            bool withinX = a.X - d <= b.X && a.X + d >= b.X;
            bool withinY = a.Y - d <= b.Y && a.Y + d >= b.Y;
            return withinX && withinY;
        }

        public static float CalculatePct(this float amnt, int pct)
        {
            return (amnt * pct / 100.0f);
        }

        public static float AddPct(this float amnt, int pct)
        {
            amnt += CalculatePct(amnt, pct);
            return amnt;
        }

        public static float ApplyPct(this float amnt, int pct)
        {
            amnt = CalculatePct(amnt, pct);
            return amnt;
        }
    }
}