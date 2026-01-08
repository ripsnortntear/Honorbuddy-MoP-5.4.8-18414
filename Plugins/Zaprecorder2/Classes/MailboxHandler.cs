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
    class ZapMailbox
    {

        private LocalPlayer intMe = StyxWoW.Me; 
        
        private List<WoWPoint> mailboxList = new List<WoWPoint>();

        public List<WoWPoint> Mailboxes
        {
            get
            {
                return mailboxList;
            }
        }

        public int Count
        {
            get
            {
                return mailboxList.Count;
            }
        }

        public void Add()
        {
            mailboxList.Add(intMe.Location);
        }

        public void Add(WoWPoint newMailbox)
        {
            mailboxList.Add(newMailbox);
        }

        public void Add(int index)
        {
            mailboxList.Insert(index, intMe.Location);
        }

        public void Add(WoWPoint newMailbox, int index)
        {
            mailboxList.Insert(index, newMailbox);
        }

        public void Add(string mailboxString)
        {

            try
            {
                mailboxList.Add(WoWPointFromMailbox(mailboxString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Clear()
        {
            mailboxList.Clear();
        }

        public void Delete(int indexToDelete)
        {
            if ((indexToDelete < 0) || ((indexToDelete + 1) > mailboxList.Count))
            {
                throw new Exception("Tried to call Mailbox.Delete with invalid index");
            }

            mailboxList.RemoveAt(indexToDelete);
        }

        public void Replace(int index)
        {
            mailboxList[index] = intMe.Location;
        }

        public void Replace(int index, WoWPoint newPoint)
        {
            mailboxList[index] = newPoint;
        }

        // TODO - THIS WON'T WORK..
        public bool Exists(WoWPoint mailboxToCheck)
        {
            return mailboxList.Contains(mailboxToCheck);
        }

        public bool Exists(string mailboxAsString)
        {
            return mailboxList.Contains(WoWPointFromMailbox(mailboxAsString));
        }

        //TODO - Fix Mailbox GOTO
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
            if ((index < 0) || ((index + 1) > mailboxList.Count))
            {
                throw new Exception("Tried to fetch mailbox with invalid index");
            }

            return mailboxList[index];
        }

        public string WowPointToHotspot(WoWPoint inPoint)
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-US"), "<Mailbox X=\"{0}\" Y=\"{1}\" Z=\"{2}\" />", inPoint.X, inPoint.Y, inPoint.Z);
        }

        public WoWPoint WoWPointFromMailbox(string hotspotString)
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
