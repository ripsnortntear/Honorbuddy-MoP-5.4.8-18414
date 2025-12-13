#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Utility/Profile.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Logging;

namespace Oracle.Shared.Utilities.Clusters.Utility
{
    internal static class Profile
    {
        public const bool UseProfiling = false; //debug, output time spend

        public static void Output(string s) //O(1)
        {
            if (!UseProfiling)
                return;
            //double timespend = DateTime.Now.Subtract(ClusterManager.Starttime).TotalSeconds;
            Logger.Output(s);
        }
    }
}