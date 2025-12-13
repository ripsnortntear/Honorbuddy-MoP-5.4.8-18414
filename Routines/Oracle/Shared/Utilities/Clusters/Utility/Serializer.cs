#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Utility/Serializer.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Logging;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Oracle.Shared.Utilities.Clusters.Utility
{
    internal class Serializer
    {
        public void SerializeObject(string filename, object objectToSerialize)
        {
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Create))
                {
                    var bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, objectToSerialize);
                }
            }
            catch (Exception ex)
            {
                Logger.Output(ex.StackTrace + "\nEnd ... ");
            }
        }

        public object DeSerializeObject(string filename)
        {
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Open))
                {
                    var bFormatter = new BinaryFormatter();
                    var objectToSerialize = bFormatter.Deserialize(stream);
                    return objectToSerialize;
                }
            }
            catch (Exception ex)
            {
                Logger.Output(ex.StackTrace + "\nEnd ... ");
            }
            return null;
        }
    }
}