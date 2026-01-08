using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Pathing;
using Styx.Helpers;

namespace ZapRecorder2
{
    class ZapHotspot
    {
        private List<WoWPoint> hotspotList = new List<WoWPoint>();
        private LocalPlayer intMe = StyxWoW.Me;

        public List<WoWPoint> Hotspots
        {
           get 
           { 
              return hotspotList; 
           }
        }

        public int Count
        {
            get
            {
                return hotspotList.Count;
            }
        }

        public void Add()
        {
            hotspotList.Add(intMe.Location);
        }

        public void Add(WoWPoint newHotspot)
        {
            hotspotList.Add(newHotspot);
        }

        public void Add(int index)
        {
            hotspotList.Insert(index, intMe.Location);
        }
        
        public void Add(WoWPoint newHotspot, int index)
        {
            hotspotList.Insert(index, newHotspot);
        }

        public void Add(string hotspotString)
        {

            try
            {
                hotspotList.Add(WoWPointFromHotspot(hotspotString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Clear()
        {
            hotspotList.Clear();
        }

        public void Delete(int indexToDelete)
        {
            if ((indexToDelete < 0) || ((indexToDelete + 1) > hotspotList.Count))
            {
                throw new Exception("Tried to call Hotspot.Delete with invalid index");
            }

            hotspotList.RemoveAt(indexToDelete);
        }

        public void Reverse()
        {
            hotspotList.Reverse();
        }

        public void Replace(int index)
        {
            hotspotList[index] = intMe.Location;
        }

        public void Replace(int index, WoWPoint newPoint)
        {
            hotspotList[index] = newPoint;
        }

        public bool Exists(WoWPoint hotspotToCheck)
        {
            return hotspotList.Contains(hotspotToCheck);
        }

        public bool Exists(string hotspotAsString)
        {
            return hotspotList.Contains(WoWPointFromHotspot(hotspotAsString));
        }

        //TODO - Fix Hotspot GOTO
        public void GoTo(WoWPoint goToPoint)
        {
            //Styx.WoWInternals.WoWMovement.MoveStop();

            //int loopCounter = 0;
            //int max_loops = 100;

            Logging.Write("Attempting to move to X=" + goToPoint.X + ", Y=" + goToPoint.Y + ", Z=" + goToPoint.Z);

            Flightor.MoveTo(goToPoint); 
            
            /*while (!StyxWoW.Me.ToUnit().IsMoving)
            {
                if (loopCounter > max_loops)
                {
                    throw new Exception("Flightor.Move to failed after " + max_loops + " attemps.");
                }
                Flightor.MoveTo(goToPoint);
                Thread.Sleep(10);
                loopCounter++;
            }
             * */

            //WoWPoint moveTo = new WoWPoint(targetXLocation, targetYLocation, targetZLocation);
            //Flightor.MoveTo(moveTo);
        }

        public WoWPoint AtIndex(int index)
        {
            if ((index < 0) || ((index + 1) > hotspotList.Count))
            {
                throw new Exception("Tried to fetch hotspot with invalid index");
            }

            return hotspotList[index];
        }

        public string WowPointToHotspot(WoWPoint inPoint)
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-US"), "<Hotspot X=\"{0}\" Y=\"{1}\" Z=\"{2}\" />", inPoint.X, inPoint.Y, inPoint.Z);
        }

        public WoWPoint WoWPointFromHotspot(string hotspotString)
        {
            WoWPoint tempPoint = new WoWPoint();

            Match xMatch = Regex.Match(hotspotString, @"X=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);
            Match yMatch = Regex.Match(hotspotString, @"Y=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);
            Match zMatch = Regex.Match(hotspotString, @"Z=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);

            if ((xMatch.Success == true) && (yMatch.Success == true) && (zMatch.Success == true))
            {

                tempPoint.X = xMatch.Groups[1].Value.ToFloat();
                tempPoint.Y = yMatch.Groups[1].Value.ToFloat();
                tempPoint.Z = zMatch.Groups[1].Value.ToFloat();

                return tempPoint;
            }
            else
            {
                throw new Exception("Unable to parse WoWPoint from " + hotspotString);
            }
        }

        private float PlayerDistanceTo(WoWPoint hotspot)
        {

            return intMe.GetPosition().Distance(hotspot);
        }

    }
}
