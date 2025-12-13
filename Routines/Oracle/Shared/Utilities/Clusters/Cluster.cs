#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Cluster.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Utilities.Clusters.Data;
using System.Collections.Generic;

namespace Oracle.Shared.Utilities.Clusters
{
    public class Cluster
    {
        private bool _isUsed;

        public Cluster(string id)
        {
            IsUsed = true;
            CentrePoint = null;
            Points = new List<Points>();
            Id = id;
            ClusterType = ClusterType.None;
        }

        public ClusterType ClusterType { get; set; }

        public Points CentrePoint { get; set; }

        public string Id { get; private set; }

        public bool IsUsed
        {
            get { return _isUsed && CentrePoint != null; }
            private set { _isUsed = value; }
        }

        public List<Points> Points { get; private set; }
    }
}