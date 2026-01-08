using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.Plugins;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Patchables;
using System.Collections.Specialized;
using Tripper.Tools.Math;

namespace ZapRecorder2
{
    class Route
    {
        public Route(uint continentID, string name, List<WoWPoint> highPoints, List<WoWPoint> lowPoints)
        {
            this.ContinentID = continentID;
            this.Name = name;
            this.HighPoints = highPoints;
            this.LowPoints = lowPoints;
        }
        public uint ContinentID { get; private set; }
        public string Name { get; private set; }
        public List<WoWPoint> HighPoints { get; private set; }
        public List<WoWPoint> LowPoints { get; private set; }
        public override string ToString()
        {
            return Name;
        }
    }
    
    class RoutesBuddy
    {

        public static RoutesBuddy Instance { get; private set; }
        public static System.Random Rng = new Random();
        public event EventHandler OnImportDone;

        //public RoutesBuddySettings MySettings;
        LocalPlayer Me = StyxWoW.Me;
        //MainForm Gui;
        public List<Route> Routes;
        public List<List<string>> RawImport;
        enum Height { High, Low };
        public RoutesBuddy() { Instance = this; }

        public void ImportRoutes()
        {
            Routes = new List<Route>();
            string table = RandomString;
            using (StyxWoW.Memory.AcquireFrame())
            {
                string lua1 = string.Format("if RoutesDB then {0} = {{}} for k,v in pairs(RoutesDB.global.routes) do for k1,v1 in pairs (v) do if #v1.route > 0 then table.insert({0},{{k,k1,unpack(v1.route)}}) end end end return #{0} end return 0 ", table);
                int routeNum = Lua.GetReturnVal<int>(lua1, 0);
                if (routeNum == 0)
                {
                    Logging.Write(Colors.Red, "No Routes found");
                    return;
                }
                RawImport = new List<List<string>>();
                for (int i = 1; i <= routeNum; i++)
                {
                    List<string> retVal = Lua.GetReturnValues(string.Format("return unpack({0}[{1}])", table, i));
                    if (retVal != null && retVal.Count >= 3)
                    {
                        RawImport.Add(retVal);
                    }
                }
                Lua.DoString(table + " = {}");
            }

            try
            {
                uint myContinent = StyxWoW.Me.MapId;
                for (int i = 0; i < RawImport.Count; i++)
                {
                    //TODO - this g
                    Logging.Write(RawImport[i][0].ToString());
                    uint mapID = FindMapId(RawImport[i][0].ToString());
                    //uint mapID = 0;
                    uint continentID = GetContinentId(mapID);
                    if (myContinent != continentID)
                    {
                        Logging.Write("Skipping {0} because its on a different continent", RawImport[i][1]);
                    }
                    else
                    {
                        if (mapID > 0)
                        {
                            List<WoWPoint> highPoints = RouteToWoWPoint(RawImport[i], mapID, Height.High, i);
                            List<WoWPoint> lowPoints = RouteToWoWPoint(RawImport[i], mapID, Height.Low, i);

                            if (highPoints.Count > 0 && lowPoints.Count > 0)
                                Routes.Add(new Route(mapID, string.Format("{0}-{1}", RawImport[i][0], RawImport[i][1]),
                                    ProcessPoints(highPoints, Height.High), ProcessPoints(lowPoints, Height.Low)));
                        }
                    }
                }
            }
            catch (Exception ex) { Logging.Write(Colors.Red, ex.ToString()); }
            if (OnImportDone != null)
                OnImportDone(Instance, null);
        }

        public static string RandomString
        {
            get
            {
                int size = Rng.Next(6, 15);
                StringBuilder sb = new StringBuilder(size);
                for (int i = 0; i < size; i++)
                {
                    // random upper/lowercase character using ascii code
                    sb.Append((char)(Rng.Next(2) == 1 ? Rng.Next(65, 91) + 32 : Rng.Next(65, 91)));
                }
                return sb.ToString();
            }
        }

        List<WoWPoint> RouteToWoWPoint(List<string> rawPoints, uint mapId, Height ht, int status)
        {
            List<WoWPoint> points = new List<WoWPoint>();
            for (int n = 2; n < rawPoints.Count; n++)
            {
                float x, y;
                uint coord;
                uint.TryParse(rawPoints[n], out coord);
                //local ex, ey = floor(point / 10000) / 10000, (point % 10000) / 10000
                x = (float)Math.Floor((float)coord / 10000f) / 10000f;
                y = ((float)coord % 10000f) / 10000f;
                points.Add(mapToWorldCoords(x, y, mapId, ht));
                int retCnt = RawImport.Count;

                /* TODO - REENABLE THIS
                int process1 = (status * 100) / retCnt;
                int process2 = ((status + 1) * 100) / retCnt;
                int process3 = (n * (process2 - process1) / rawPoints.Count) + process1;
                Gui.UpdateProgressBar(process3);
                 * 
                 * */
            }
            return points;
        }

        List<WoWPoint> ProcessPoints(List<WoWPoint> points, Height ht)
        {
            List<WoWPoint> newPoints = new List<WoWPoint>();
            newPoints.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                List<WoWPoint> tempPoints = CreatePathSegment(points[i - 1], points[i], ht);
                /* TODO - RENABLE THIS
                if (Gui.smoothCheck.Checked)
                {
                    tempPoints = SmoothOut3dSegment(tempPoints);
                }
                 * */
                newPoints.AddRange(tempPoints);
            }
            return newPoints;
        }

        List<WoWPoint> CreatePathSegment(WoWPoint from, WoWPoint to, Height ht)
        {
            List<WoWPoint> segment = new List<WoWPoint>();
            WoWPoint point = from;
            float step = 50;
            float noMeshStep = 5;
            for (float i = from.Distance(to) - step; i > 0; )
            {
                point = WoWMathHelper.CalculatePointFrom(from, to, i);
                try
                {
                    float z = ht == Height.High ? Navigator.FindHeights(point.X, point.Y).Max() :
                        Navigator.FindHeights(point.X, point.Y).Min();
                    i -= step;
                    /* TODO - REENABLE THISif (Gui.smoothCheck.Checked && z > point.Z)
                    {
                        point.Z = z;
                    }
                     * */
                    segment.Add(point);
                }
                catch { i -= noMeshStep; }
            }
            segment.Add(to);
            return segment;
        }

        List<WoWPoint> SmoothOut3dSegment(List<WoWPoint> path)
        {
            WoWPoint end = path[path.Count - 1];
            int startI = 0;
            int maxIndex = startI;
            while (startI < path.Count - 1)
            {
                WoWPoint endV = path[startI].GetDirectionTo(end);
                float highestZ = endV.Z;
                for (int i = startI; i < path.Count; i++)
                {
                    WoWPoint pointV = path[startI].GetDirectionTo(path[i]);
                    if (pointV.Z >= highestZ)
                    {
                        highestZ = pointV.Z;
                        maxIndex = i;
                    }
                }
                if (maxIndex == startI)
                    maxIndex = path.Count - 1;
                float multiplier = (path[maxIndex].Z - path[startI].Z) / (path[startI].Distance2D(path[maxIndex]));
                for (int n = startI + 1; n < maxIndex; n++)
                {
                    WoWPoint point = path[n];
                    point.Z = (path[startI].Distance2D(path[n]) * multiplier) + path[startI].Z;
                    path[n] = point;
                }
                startI = maxIndex;
            }
            return path;
        }

        WoWPoint mapToWorldCoords(float x, float y, uint mapId, Height ht)
        {
            WoWPoint worldPoint = new WoWPoint();
            WoWDb.DbTable worldMapArea = StyxWoW.Db[ClientDb.WorldMapArea];
            WoWDb.Row worldMapAreaFields = worldMapArea.GetRow(mapId);
            float ay = worldMapAreaFields.GetField<float>(4);
            float by = worldMapAreaFields.GetField<float>(5);
            float ax = worldMapAreaFields.GetField<float>(6);
            float bx = worldMapAreaFields.GetField<float>(7);
            worldPoint.X = ax + (y * (bx - ax));
            worldPoint.Y = ay + (x * (by - ay));
            try
            {
                worldPoint.Z = ht == Height.High ? Navigator.FindHeights(worldPoint.X, worldPoint.Y).Max() :
                    Navigator.FindHeights(worldPoint.X, worldPoint.Y).Min();
            }
            catch { return TryGetHeight(worldPoint, ht); }
            return worldPoint;
        }

        WoWPoint TryGetHeight(WoWPoint point, Height ht)
        {
            float PIx2 = (float)Math.PI * 2f;
            int step = 20;
            for (int d = 5; d <= 50; d += 5)
            {
                for (int i = 0; i < 20; i++)
                {
                    WoWPoint newPoint = point.RayCast((i * PIx2) / step, d);
                    try
                    {
                        newPoint.Z = ht == Height.High ? Navigator.FindHeights(newPoint.X, newPoint.Y).Max() :
                            Navigator.FindHeights(newPoint.X, newPoint.Y).Min();
                        return newPoint;
                    }
                    catch { }
                }
            }
            return point;
        }

        uint FindMapId(string localMapName)
        {
            WoWDb.DbTable t = StyxWoW.Db[ClientDb.WorldMapArea];
            int max = t.MaxIndex;
            for (int i = t.MinIndex; i <= max; )
            {
                WoWDb.Row r = t.GetRow((uint)i);
                string mapName = StyxWoW.Memory.ReadString(r.GetField<IntPtr>(3), Encoding.ASCII);
                if (mapName == localMapName)
                    return (uint)i;
                if (i < max)
                    i = r.GetField<int>(14);
                else
                    break;
            }
            return 0;
        }


        uint GetContinentId(uint mapID)
        {
            WoWDb.DbTable t = StyxWoW.Db[ClientDb.WorldMapArea];
            WoWDb.Row r = t.GetRow(mapID);
            return r.GetField<uint>(1);
        }

    }
}
