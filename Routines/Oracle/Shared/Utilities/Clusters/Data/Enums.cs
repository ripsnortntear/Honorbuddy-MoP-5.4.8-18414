#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Data/Enums.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

namespace Oracle.Shared.Utilities.Clusters.Data
{
    public enum ClusterType { None = 0, Proximity, Party, NearbyLowestHealth, GroundEffect };

    public enum DataSetType { None = 0, HealingPriorities, Players, Units, Test };

    public enum SpellType { None = 0, Proximity, Party, NearbyLowestHealth, GroundEffect };

}