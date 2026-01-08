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
    class ZapBlackspot
    {
        private LocalPlayer intMe = StyxWoW.Me;

        private List<WoWPoint> blackspotList = new List<WoWPoint>();

        public List<WoWPoint> Blackspots
        {
            get
            {
                return blackspotList;
            }
        }

        public int Count
        {
            get
            {
                return blackspotList.Count;
            }
        }

        public void Add()
        {
            blackspotList.Add(intMe.Location);
        }

        public void Add(WoWPoint newBlackspot)
        {
            blackspotList.Add(newBlackspot);
        }

        public void Add(int index)
        {
            blackspotList.Insert(index, intMe.Location);
        }

        public void Add(WoWPoint newBlackspot, int index)
        {
            blackspotList.Insert(index, newBlackspot);
        }

        public void Add(string blackspotString)
        {

            try
            {
                blackspotList.Add(WoWPointFromBlackspot(blackspotString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Clear()
        {
            blackspotList.Clear();
        }

        public void Delete(int indexToDelete)
        {
            if ((indexToDelete < 0) || ((indexToDelete + 1) > blackspotList.Count))
            {
                throw new Exception("Tried to call Blackspot.Delete with invalid index");
            }

            blackspotList.RemoveAt(indexToDelete);
        }

        public void Replace(int index)
        {
            blackspotList[index] = intMe.Location;
        }

        public void Replace(int index, WoWPoint newPoint)
        {
            blackspotList[index] = newPoint;
        }

        public bool Exists(WoWPoint blackspotToCheck)
        {
            return blackspotList.Contains(blackspotToCheck);
        }

        public bool Exists(string blackspotAsString)
        {
            return blackspotList.Contains(WoWPointFromBlackspot(blackspotAsString));
        }

        //TODO - Fix Blackspot GOTO
        public void GoTo(WoWPoint goToPoint)
        {
            Styx.WoWInternals.WoWMovement.MoveStop();

            int loopCounter = 0;
            int max_loops = 100;
            while (!StyxWoW.Me.ToUnit().IsMoving && (loopCounter < max_loops))
            {
                Flightor.MoveTo(goToPoint);
                Thread.Sleep(5);
                loopCounter++;
            }

            //WoWPoint moveTo = new WoWPoint(targetXLocation, targetYLocation, targetZLocation);
            //Flightor.MoveTo(moveTo);
        }

        public WoWPoint AtIndex(int index)
        {
            if ((index < 0) || ((index + 1) > blackspotList.Count))
            {
                throw new Exception("Tried to fetch blackspot with invalid index");
            }

            return blackspotList[index];
        }

        public string WowPointToBlackspot(WoWPoint inPoint)
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-US"), "<Blackspot X=\"{0}\" Y=\"{1}\" Z=\"{2}\" Radius=\"{3}\" />", inPoint.X, inPoint.Y, inPoint.Z, Properties.Settings.Default.blackspotRadius);
        }

        public WoWPoint WoWPointFromBlackspot(string hotspotString)
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
    }
}
