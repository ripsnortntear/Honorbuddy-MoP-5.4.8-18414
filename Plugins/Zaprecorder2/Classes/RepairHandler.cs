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
    class RepairMerchant
    {

        private WoWPoint Location;
        private string Name;
        public uint Entry;
        private LocalPlayer intMe = StyxWoW.Me;
        private static string Type = "Repair";

        public RepairMerchant(WoWUnit targetUnit)
        {
            Name = targetUnit.Name;
            Entry = targetUnit.Entry;
            Location = targetUnit.Location;
        }

        public RepairMerchant(string newName, uint newEntry, WoWPoint newLocation)
        {
            Name = newName;
            Entry = newEntry;
            Location = newLocation;
        }
        
        public string ToHotspot()
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-US"), "<Vendor Name=\"{0}\" Entry=\"{1}\" Type=\"{2}\" X=\"{3}\" Y=\"{4}\" Z=\"{5}\" />",
                        Name, Entry.ToString(), Type, Location.X.ToString(), Location.Y.ToString(),
                        Location.Z.ToString());
        }
    }
    
    class ZapRepair
    {

        private List<RepairMerchant> merchantList = new List<RepairMerchant>();
        private LocalPlayer intMe = StyxWoW.Me;

        public List<RepairMerchant> Merchants
        {
            get
            {
                return merchantList;
            }
        }

        public int Count
        {
            get
            {
                return merchantList.Count;
            }
        }

        public void Add()
        {
            merchantList.Add(new RepairMerchant(intMe.CurrentTarget));
        }

        public void Add(WoWUnit targetUnit)
        {
            merchantList.Add(new RepairMerchant(targetUnit));
        }

        public void Add(string repairString)
        {

            try
            {
                merchantList.Add(RepairMerchantfromProfileSring(repairString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Clear()
        {
            merchantList.Clear();
        }

        public void Delete(int indexToDelete)
        {
            if ((indexToDelete < 0) || ((indexToDelete + 1) > merchantList.Count))
            {
                throw new Exception("Tried to call ZapRepair.Delete with invalid index");
            }

            merchantList.RemoveAt(indexToDelete);
        }

        public void Replace(int index)
        {
            merchantList[index] = new RepairMerchant(intMe.CurrentTarget);
        }

        public void Replace(int index, WoWUnit newUnit)
        {
            merchantList[index] = new RepairMerchant(newUnit); ;
        }
        
        public bool Exists(WoWUnit unitToCheck)
        {
            foreach (RepairMerchant singleMerchant in this.Merchants)
            {
                // Entry is probably most reliable (uint instead of string)
                if (singleMerchant.Entry == unitToCheck.Entry)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Exists(RepairMerchant merchantToCheck)
        {
            foreach (RepairMerchant singleMerchant in this.Merchants)
            {
                // Entry is probably most reliable (uint instead of string)
                if (singleMerchant.Entry == merchantToCheck.Entry)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Exists(string merchantAsString)
        {
            return Exists(RepairMerchantfromProfileSring(merchantAsString));
        }


        //TODO - Fix ZapRepair GOTO
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

        public RepairMerchant AtIndex(int index)
        {
            if ((index < 0) || ((index + 1) > merchantList.Count))
            {
                throw new Exception("Tried to fetch Repair NPC with invalid index");
            }

            return merchantList[index];
        }


        public RepairMerchant RepairMerchantfromProfileSring(string repairString)
        {
            WoWPoint tempPoint = new WoWPoint();

            Match xMatch = Regex.Match(repairString, @"X=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);
            Match yMatch = Regex.Match(repairString, @"Y=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);
            Match zMatch = Regex.Match(repairString, @"Z=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);
            Match entryMatch = Regex.Match(repairString, @"Entry=\""([A-Za-z0-9\-.]+)\""", RegexOptions.IgnoreCase);
            Match nameMatch = Regex.Match(repairString, @"Name=\""([A-Za-z0-9\s\-.']+)\""", RegexOptions.IgnoreCase);

            if ((xMatch.Success == true) && (yMatch.Success == true) && (zMatch.Success == true) && (entryMatch.Success == true) && (nameMatch.Success == true))
            {

                tempPoint.X = xMatch.Groups[1].Value.ToFloat();
                tempPoint.Y = yMatch.Groups[1].Value.ToFloat();
                tempPoint.Z = zMatch.Groups[1].Value.ToFloat();

                return new RepairMerchant(nameMatch.Value.ToString(),entryMatch.Value.ToUInt32(),tempPoint);
            }
            else
            {
                throw new Exception("Unable to parse RepairNPC from " + repairString);
            }
        }
    }
    
}
