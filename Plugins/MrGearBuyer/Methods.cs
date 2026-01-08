using System.Globalization;
using Styx;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Styx.CommonBot;


// ReSharper disable CheckNamespace
namespace MrGearBuyer
// ReSharper restore CheckNamespace
{
    public partial class MrGearBuyer
    {
        
      public class ManualVendor
        {
            public int NpcId;
            public string NpcName;
            public WoWPoint NpcLocation;

            public ManualVendor Next;
        }
       public class ListOfVendors
        {
            private int _size;

            public ListOfVendors()
            {
                _size = 0;
                Head = null;
            }

            public int Count
            {
                get { return _size; }
            }

            public ManualVendor Head;

            public int Add(ManualVendor newItem)
            {
                ManualVendor sample = newItem;
                sample.Next = Head;
                Head = sample;
                return _size++;
            }
            public ManualVendor Retrieve(int position)
            {
                ManualVendor current = Head;

                for (int i = 0; i < position && current != null; i++)
                    current = current.Next;
                return current;
            }
            
            public ManualVendor FindById(int vendorID)
            {
                var foundVendor = new ManualVendor();
                for (int i = 0; i < LoadedVendors.Count; i++)
                {
                    ManualVendor vendor = Retrieve(i);
                    if (vendor.NpcId == vendorID)
                    {
                        return foundVendor;
                    }
                }
                return null;
            }
        }
            
            public void CacheVendors()
            {
                var vendor = new ListOfVendors();
                //TestVendor
                var newVendor = new ManualVendor {NpcId = 99, NpcName = "CatBug", NpcLocation = new WoWPoint(8, 00, 85)};
                vendor.Add(newVendor);

                //PandaLand NPC's as of patch 5.2 
                //Panderia  MapID is 870
                //<Vendor Name="Hayden Christophen" Entry="69981" Type="Repair" X="203.0226" Y="2197.023" Z="272.3725" />
                newVendor = new ManualVendor
                {
                    NpcId = 69981,
                    NpcName = "Hayden Christophen",
                    NpcLocation = new WoWPoint(203.0226, 2197.023, 272.3725)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Lucan Malory" Entry="69967" Type="Repair" X="194.5747" Y="2200.321" Z="272.4135" />
                newVendor = new ManualVendor
                {
                    NpcId = 69967,
                    NpcName = "Lucan Malory",
                    NpcLocation = new WoWPoint(194.5747, 2200.321, 272.4135)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Ethan Natice" Entry="69968" Type="Repair" X="197.5226" Y="2209.101" Z="272.3725" />
                newVendor = new ManualVendor
                {
                    NpcId = 69968,
                    NpcName = "Ethan Natice",
                    NpcLocation = new WoWPoint(197.5226, 2209.101, 272.3725)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Armsmaster Holinka" Entry="70101" Type="Repair" X="206.3177" Y="2206.078" Z="272.4135" />
                newVendor = new ManualVendor
                {
                    NpcId = 70101,
                    NpcName = "Armsmaster Holinka",
                    NpcLocation = new WoWPoint(206.3177, 2206.078, 272.4135)
                };
                vendor.Add(newVendor);

                //Stormwind
                //<Vendor Name="Captain Dirgehammer" Entry="69975" Type="Repair" X="-8781.12" Y="420.3802" Z="105.2327" />
                newVendor = new ManualVendor
                {
                    NpcId = 69975,
                    NpcName = "Captain Dirgehammer",
                    NpcLocation = new WoWPoint(-8781.12, 420.3802, 105.2327)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Edlan Halsing" Entry="52029" Type="Repair" X="-8788.16" Y="425.4879" Z="105.2329" />
                newVendor = new ManualVendor
                {
                    NpcId = 52029,
                    NpcName = "Edlan Halsing",
                    NpcLocation = new WoWPoint(-8788.16, 425.4879, 105.2329)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Liliana Emberfrost" Entry="52030" Type="Repair" X="-8779.444" Y="432.4097" Z="105.2324" />
                newVendor = new ManualVendor
                {
                    NpcId = 52030,
                    NpcName = "Liliana Emberfrost",
                    NpcLocation = new WoWPoint(-8779.444, 432.4097, 105.2324)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Knight-Lieutenant T'Maire Sydes" Entry="69974" Type="Repair" X="-8771.236" Y="422.2639" Z="103.9212" />
                newVendor = new ManualVendor
                {
                    NpcId = 69974,
                    NpcName = "Knight-Lieutenant T'Maire Sydes",
                    NpcLocation = new WoWPoint(-8771.236, 422.2639, 103.9212)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Master Sergeant Biggins" Entry="12781" Type="Repair" X="-8767.535" Y="417.8889" Z="103.9207" />
                newVendor = new ManualVendor
                {
                    NpcId = 12781,
                    NpcName = "Master Sergeant Biggins",
                    NpcLocation = new WoWPoint(-8767.535, 417.8889, 103.9207)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Sergeant Major Clate" Entry="12785" Type="Repair" X="-8774.203" Y="410.3507" Z="103.9212" />
                newVendor = new ManualVendor
                {
                    NpcId = 12785,
                    NpcName = "Sergeant Major Clate",
                    NpcLocation = new WoWPoint(-8774.203, 410.3507, 103.9212)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Lieutenant Jackspring" Entry="12784" Type="Repair" X="-8776.241" Y="413.2465" Z="103.9219" />
                newVendor = new ManualVendor
                {
                    NpcId = 12784,
                    NpcName = "Lieutenant Jackspring",
                    NpcLocation = new WoWPoint(-8776.241, 413.2465, 103.9219)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Lieutenant Karter" Entry="12783" Type="Repair" X="-8753.438" Y="400.2535" Z="101.0562" />
                newVendor = new ManualVendor
                {
                    NpcId = 12783,
                    NpcName = "Lieutenant Karter",
                    NpcLocation = new WoWPoint(-8753.438, 400.2535, 101.0562)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Larisse Pembraux" Entry="52027" Type="Repair" X="-8802.316" Y="343.9184" Z="107.0503" />
                newVendor = new ManualVendor
                {
                    NpcId = 52027,
                    NpcName = "Larisse Pembraux",
                    NpcLocation = new WoWPoint(-8802.316, 343.9184, 107.0503)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Faldren Tillsdale" Entry="44245" Type="Repair" X="-8799.98" Y="349.335" Z="109.1347" />
                newVendor = new ManualVendor
                {
                    NpcId = 44245,
                    NpcName = "Faldren Tillsdale",
                    NpcLocation = new WoWPoint(-8799.98, 349.335, 109.1347)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Magatha Silverton" Entry="44246" Type="Repair" X="-8802.37" Y="352.036" Z="109.1351" />
                newVendor = new ManualVendor
                {
                    NpcId = 44246,
                    NpcName = "Magatha Silverton",
                    NpcLocation = new WoWPoint(-8802.37, 352.036, 109.1351)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Toren Landow" Entry="58154" Type="Repair" X="-8808.223" Y="350.8542" Z="107.0501" />
                newVendor = new ManualVendor
                {
                    NpcId = 58154,
                    NpcName = "Toren Landow",
                    NpcLocation = new WoWPoint(-8808.223, 350.8542, 107.0501)
                };
                vendor.Add(newVendor);

                //<Vendor Name="Talric Forthright" Entry="52028" Type="Repair" X="-8814.497" Y="354.9618" Z="107.0501" />
                newVendor = new ManualVendor
                {
                    NpcId = 52028,
                    NpcName = "Talric Forthright",
                    NpcLocation = new WoWPoint(-8814.497, 354.9618, 107.0501)
                };
                vendor.Add(newVendor);

                Slog("Manually Added {0} Vendors to Mr.GearBuyer Cache", vendor.Count.ToString(CultureInfo.InvariantCulture));
                LoadedVendors = vendor;
            }
            public void RepairPoiSwap()
            {
                if(RepairSet && RepairSwap)
                {
                if (BotPoi.Current.Type == PoiType.Repair && BotPoi.Current.AsVendor.Entry != RepairID)
                {
                    Slog("Was Repairing at {0}, Cearing to Set Our Repair Vendor", BotPoi.Current.AsVendor.Name);
                    BotPoi.Clear();
                    var repairLocation = new WoWPoint(RepairX, RepairY, RepairZ);
                    var repairVendor = new Vendor(RepairID, RepairName, Vendor.VendorType.Unknown, repairLocation);
                    BotPoi.Current = new BotPoi(repairVendor, PoiType.Repair);
                    Slog("Swapped out old repair Point of Interest for the Repair Point we want to repair from");
                }
                }
            }
            public void MoveTo(WoWPoint location)
            {
                if (MyMount)
                {
                    if (!StyxWoW.Me.Mounted)
                    {
                        Flightor.MountHelper.MountUp();
                    }
                    Flightor.MoveTo(location);
                    if (Mount.CanMount())
                    {
                        Flightor.MountHelper.MountUp();
                    }
                }
                else
                {
                    if (!StyxWoW.Me.Mounted && Mount.ShouldMount(location) && Mount.CanMount())
                    {
                        Slog("Point is a long way away, Mounting Up!");
                        Mount.MountUp(() => location);
                    }
                    Navigator.MoveTo(location);
                }
            }
            public void DismountAtDestination(WoWPoint location)
            {
                if (Me.Location.Distance(location) <= 1)
                {
                    Dlog("Reached My Location.");
                    if (Me.Mounted && MyMount)
                    {
                        Flightor.MountHelper.Dismount();
                        Dlog("Destination Reached. Dismounting");
                    }
                    MoveToHome = false;
                }
            }
    }
}
