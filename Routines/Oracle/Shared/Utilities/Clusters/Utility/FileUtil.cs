#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Clusters/Utility/FileUtil.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Logging;
using Oracle.Shared.Utilities.Clusters.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Oracle.Shared.Utilities.Clusters.Utility
{
    public static class FileUtil
    {
        //------------------------------
        // USER CONFIG, CUSTOMIZE BELOW

        // where the files are saved or loaded
        public const string FolderPath = @"c:\temp\";

        // USER CONFIG, CUSTOMIZE ABOVE
        //------------------------------

        private const string ctx = "ctx"; // javascript canvas
        private const string DatasetSerializeName = "dataset.ser";
        private const string S = "G";
        private static readonly CultureInfo Culture_enUS = new CultureInfo("en-US");
        private static readonly string NL = Environment.NewLine;

        public static void GenerateJavascriptDrawFile(int clusterRadius, ClusterType clusterType)
        {
            var sb = new StringBuilder();

            // markers
            string head = "function drawMarkers(ctx) {" + NL;
            string tail = NL + "}" + NL;

            sb.Append(head);

            // if to many points, the canvas can not be drawn or is slow,
            // use max points and clusters for drawing
            const int max = 10000;

            // markers
            List<Points> points = null;
            switch (clusterType)
            {
                case ClusterType.None:
                    break;

                case ClusterType.Proximity:
                    points = ClusterManager.PoximityPoints;
                    break;

                case ClusterType.Party:
                    points = ClusterManager.PartyPoints;
                    break;

                case ClusterType.NearbyLowestHealth:
                    points = ClusterManager.NearbyPoints;
                    break;

                case ClusterType.GroundEffect:
                    points = ClusterManager.GroundPoints;
                    break;
            }

            if (points != null)
            {
                sb.Append(NL);
                for (int i = 0; i < points.Count; i++)
                {
                    Points p = points[i];
                    sb.Append(string.Format("drawMark({0}, {1}, {2}, {3});{4}",
                                            ToStringEN(p.X), ToStringEN(p.Y), p.Color, ctx, NL));
                    if (i > max)
                        break;
                }
            }
            string clusterInfo = "0";
            if (ClusterManager.Clusters != null)
            {
                sb.Append(NL);

                for (int i = 0; i < ClusterManager.Clusters.Count; i++)
                {
                    if (ClusterManager.Clusters[i].PointClusterType != clusterType) continue;

                    Points c = ClusterManager.Clusters[i];
                    sb.Append(string.Format(
                        "drawDistanceCluster({0}, {1}, {2}, {3}, {4}, {5});{6}",
                        ToStringEN(c.X), ToStringEN(c.Y), c.Color,
                        c.Size, clusterRadius, ctx, NL));

                    //if (i > max)
                    //  break;
                }

                clusterInfo = ClusterManager.Clusters.Count + string.Empty;
            }

            // bottom text
            sb.Append("ctx.fillStyle = 'rgb(0,0,0)';" + NL);
            sb.Append(string.Format("ctx.fillText('Clusters = ' + {0}, {1}, {2});{3}",
                                    clusterInfo, ToStringEN(10), ToStringEN(20), NL));

            sb.Append(tail);
            CreateFile(sb.ToString());
            //Logger.Output(sb.ToString());
        }

        public static string ToStringEN(double d)
        {
            double rounded = Math.Round(d, 3);
            return rounded.ToString(S, Culture_enUS);
        }

        public static bool WriteFile(string data, FileInfo fileInfo)
        {
            bool isSuccess = false;
            try
            {
                using (StreamWriter streamWriter =
                    File.CreateText(fileInfo.FullName))
                {
                    streamWriter.Write(data);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Output(ex.StackTrace + "\nPress a key ... ");
                Console.ReadKey();
            }
            return isSuccess;
        }

        private static void CreateFile(string s)
        {
            var path = new FileInfo(FolderPath + "draw.js");
            bool isCreated = WriteFile(s, path);
            Logger.Output(isCreated + " = File is Created");
        }

        // load points from file to mem
        private static List<Points> LoadDataSetFromFile()
        {
            var objectToSerialize = (DatasetToSerialize)
                                    (new Serializer().DeSerializeObject(FolderPath + DatasetSerializeName));
            return objectToSerialize.Dataset;
        }

        // save points from mem to file
        private static void SaveDataSetToFile(List<Points> dataset)
        {
            var objectToSerialize = new DatasetToSerialize { Dataset = dataset };
            new Serializer().SerializeObject(FolderPath + DatasetSerializeName, objectToSerialize);
        }

        [Serializable]
        public class DatasetToSerialize : ISerializable
        {
            private const string Name = "Dataset";

            public DatasetToSerialize()
            {
                Dataset = new List<Points>();
            }

            public DatasetToSerialize(SerializationInfo info, StreamingContext ctxt)
            {
                Dataset = (List<Points>)info.GetValue(Name, typeof(List<Points>));
            }

            public List<Points> Dataset { get; set; }

            public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
            {
                info.AddValue(Name, Dataset);
            }
        }
    }
}