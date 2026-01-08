//Mr.GearBuyer - Created by CodenameGamma - 4/26/11 - For WoW Version 4.0.3+
//Updated for Mist of Panderia WoW Version 5.0.4+ on 9/12/12
//Updated for Patch 5.2 on 3/19/13
//Updated for Patch 5.4 on 10/8/13
//www.honorbuddy.com
//this is a free plugin, and should not be sold, or repackaged.
//Donations Accepted. 
//Version 3.3.1


using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MrGearBuyer.GUI;
using System;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.IO;
using System.Xml.Linq;
using Styx.Plugins;
using Styx;
using Styx.CommonBot;
using Styx.Common.Helpers;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Database;
using Styx.Common;
using Styx.CommonBot.Frames;

// ReSharper disable CheckNamespace
namespace MrGearBuyer
// ReSharper restore CheckNamespace
{
    public partial class MrGearBuyer : HBPlugin
    {
        #region Override of HBPlugin

        //Normal Stuff.
        public override string Name { get { return "Mr.GearBuyer 3.3"; } }
        public override string Author { get { return "CnG & Bambam922"; } }
        public override Version Version { get { return new Version(3, 3, 1); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "MGB Settings"; } }
        public bool WasInBG;
        private bool _initialized;
        public bool ShouldFly = true;
        public static ListOfVendors LoadedVendors = new ListOfVendors();

        public override void OnEnable()
        {
            if (_initialized)
                return;

            Vendors.OnBuyItems += BuyItems;
            // Loading item from settings file.
            LoadSettings();
            // Should subscribe to listchanged after loading items. So any add/remove will save the settings file behind the scene.
            BuyItemList.RaiseListChangedEvents = true;
            BuyItemList.ListChanged += BuyItemListListChanged;
            CacheVendors();
            Slog( Name + " Done Loading.");
// ReSharper disable CompareOfFloatsByEqualityOperator
            if (HomeZ == 0 && HomeZ == 0 && HomeX == 0 || !HPSet)
// ReSharper restore CompareOfFloatsByEqualityOperator
            {
                Slog("My HomePoint is not set, Please set a HomePoint Location to use HomePoint Features!");
                Dlog("HomePoint MapID is {0}", MapID.ToString(CultureInfo.InvariantCulture));
            }
            if (!RepairSet)
            {
                Slog("No Prefered Repair Vendor Set");
            }
            _initialized = true;
            WasInBG = false;
        }

        private Form _myForm;
        public override void OnButtonPress()
        {
            if (!_initialized)
                OnEnable();

            var form = _myForm ?? (_myForm = new MGBConfig());
            form.ShowDialog();
        }
        private static readonly WaitTimer PulseTimer = new WaitTimer(TimeSpan.FromSeconds(4));

        public bool MoveToHome = false;

        public override void Pulse()
        {
            if (BotManager.Current.Name == "BGBuddy")
            {
                
                RepairPoiSwap(); //Allows us to use the repair vendors WE want.

                var myHomePointLocation = new WoWPoint(HomeX, HomeY, HomeZ);
                if (Me.HealthPercent > 20 && !Battlegrounds.IsInsideBattleground && HPSet && Me.MapId == MapID)
                {
                    if (MoveToHome == false && Me.MapId == MapID && BotPoi.Current.Type == PoiType.None && Me.Location.Distance(myHomePointLocation) >= 4)
                    {
                        Slog("I Moved Away from my HomePoint! Moving back!");
                        MoveToHome = true;
                    }

                    if (MoveToHome)
                    {
                        if (Me.IsInInstance)
                        {
                            Dlog("IsInBattleground Or Instance. fail on move");
                            MoveToHome = false;
                            return;
                        }
                        if (Me.MapId != MapID)
                        {
                            Dlog("MapID Does not Match Stored MapID. fail on move");
                            MoveToHome = false;
                            return;
                        }
                        if (BotPoi.Current.Type != PoiType.None)
                        {
                            Dlog("Poi is not none. fail on move");
                            MoveToHome = false;
                            return;
                        }
                        DismountAtDestination(myHomePointLocation);
                        MoveTo(myHomePointLocation);
                        
                    }
                }
           
            }
            
            if (!PulseTimer.IsFinished)
                return;

            PulseTimer.Reset();

     

            if (LogoutAtCap && WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints).Amount > 3750 && WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints).Amount > 3750)
            {
                Slog("HonorPoints and Justice Points are Capped! Logging out!");
                Lua.DoString("Logout()");
                return;
            }
            if (LogOutHPCap && WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints).Amount > 3750)
            {
                Slog("Honor Points Are Capped. Logging Out!");
                Lua.DoString("Logout()");
                return;
            }
            if (LogOutJPCap && WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints).Amount > 3750)
            {
                Slog("Justice Points are Capped! Logging out!");
                Lua.DoString("Logout()");
                return;
            }
            // We should avoid overriding any poi set by the core
            if (BotPoi.Current.Type != PoiType.None)
                return;

            // There are no vendors in battlegrounds or dungeons !
            if (Battlegrounds.IsInsideBattleground || Me.IsInInstance)
                return;

            // First item in the list. We should check item by item so we don't end up buying the last item in the list with lower cost.
            var firstItem = BuyItemList.FirstOrDefault();

            // BuyItemList looks to be empty. Wait for user to populate the list
            if (firstItem == null)
                return;

            // Should check if we have enough currency.
            var currencyType = Enum.Parse(typeof (WoWCurrencyType), firstItem.ItemCostType);

            // Something went wrong with parsing. We should avoid buying that item.
            if (!(currencyType is WoWCurrencyType))
            {
                Slog("Couldn't parse item's cost type ({0}). Please consult to the plugin writer", firstItem.ItemCostType);
                BuyItemList.Remove(firstItem);
                
                return;
            }

            // Actually checking if we have enough of that currency now.
            var currency = WoWCurrency.GetCurrencyByType((WoWCurrencyType)currencyType);
            var currencyJp = WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints);
            var currencyHp = WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints);
            WoWCurrency.GetCurrencyByType(WoWCurrencyType.ValorPoints);
            WoWCurrency.GetCurrencyByType(WoWCurrencyType.ConquestPoints);
            if (currency == null)
                return;

            if (currency.Amount < firstItem.ItemCost) //This makes sure we have enough points for the items we're buying if not it gets sent back to top.
            {
                // Don't ever buy justice points to buy honor points and vice versa. Otherwise we will enter in an endless loop which will drop 
                // the total of our points.
                if(Me.MapId == 0 || Me.MapId == 1) // added to make sure point conversion items dont get added if not near a vendor that supports them.
                {
                    if (firstItem.ItemId != 392 && firstItem.ItemId != 395 && BuyOppositePointToBuildUp)
                    {
                        //sometimes a toon wont have points.
                        if (currencyHp != null)
                        {
                            if (currency.CurrencyType == WoWCurrencyType.JusticePoints && currencyHp.Amount >= 500)
                            {
                                // We set this to true here. So we don't end up spending all our honor points if the Only remove hp/jp points when capped is true
                                _forceAddedPoints = true;
                                if (Me.IsAlliance && Me.MapId == 0) //Making sure Map is EK thats where this vendor is.
                                {
                                    var buyJusticePoint = new BuyItemInfo
                                                              {
                                                                  ItemCost = 500,
                                                                  ItemName = "Justice Points",
                                                                  ItemSupplierId = 52029,
                                                                  ItemId = 395,
                                                                  ItemCostType = WoWCurrencyType.HonorPoints.ToString()
                                                              };
                                    Slog(
                                        "Adding Justice Point to the Buy List so we can build up Justice Points to buy {0}.",
                                        firstItem.ItemName);
                                    BuyItemList.Insert(0, buyJusticePoint);
                                }
                                if (Me.IsHorde && Me.MapId == 1) //Making sure Map is Kali thats where this vendor is.
                                {
                                    var buyJusticePoint = new BuyItemInfo
                                                              {
                                                                  ItemCost = 500,
                                                                  ItemName = "Justice Points",
                                                                  ItemSupplierId = 52033,
                                                                  ItemId = 395,
                                                                  ItemCostType = WoWCurrencyType.HonorPoints.ToString()
                                                              };
                                    Slog(
                                        "Adding Justice Point to the Buy List so we can build up Justice Points to buy {0}.",
                                        firstItem.ItemName);
                                    BuyItemList.Insert(0, buyJusticePoint);
                                }
                            }
                        }
                        //sometimes a toon wont have both.
                        if (currencyJp != null)
                        {
                            if (currency.CurrencyType == WoWCurrencyType.HonorPoints && currencyJp.Amount >= 500)
                            {
                                // We set this to true here. So we don't end up spending all our justice points if the Only remove hp/jp points when capped is true
                                _forceAddedPoints = true;
                                if (Me.IsAlliance && Me.MapId == 0) //Making sure Map is EK thats where this vendor is.
                                {
                                    var buyHonorPoint = new BuyItemInfo
                                                            {
                                                                ItemCost = 500,
                                                                ItemName = "Honor Points",
                                                                ItemSupplierId = 52028,
                                                                ItemId = 392,
                                                                ItemCostType = WoWCurrencyType.JusticePoints.ToString()
                                                            };
                                    Slog("Adding HonorPoint to the Buy List so we can build up HonorPoints to buy {0}.",
                                         firstItem.ItemName);
                                    BuyItemList.Insert(0, buyHonorPoint);
                                }
                                if (Me.IsHorde && Me.MapId == 1) //Making sure Map is Kali thats where this vendor is.
                                {
                                    var buyHonorPoint = new BuyItemInfo
                                                            {
                                                                ItemCost = 500,
                                                                ItemName = "Honor Points",
                                                                ItemSupplierId = 52034,
                                                                ItemId = 392,
                                                                ItemCostType = WoWCurrencyType.JusticePoints.ToString()
                                                            };
                                    Slog("Adding HonorPoint to the Buy List so we can build up HonorPoints to buy {0}.",
                                         firstItem.ItemName);
                                    BuyItemList.Insert(0, buyHonorPoint);
                                }
                            }
                        }
                    }
                }
                return;
            }

          
            // We need to find the vendor
            var vendorAsUnit =
                ObjectManager.GetObjectsOfType<WoWUnit>(false, false).FirstOrDefault(
                    u => u.Entry == firstItem.ItemSupplierId);
            Vendor vendor;
            // Vendor is not around. This won't work
            if (vendorAsUnit == null)
            {
                Slog("Mr.GearBuyer Could Not Find NPC in Range. Checking Honorbuddy's NPC List to see if we can Find him");
                // Check the database for the vendor as a second hope
                NpcResult npc = NpcQueries.GetNpcById(firstItem.ItemSupplierId);
               
                if (npc != null)
                {
                    
                    vendor = new Vendor(npc.Entry, npc.Name, Vendor.VendorType.Unknown, npc.Location);
                }
                else
                {
                    Slog("Mr.GearBuyer Could Not Find NPC in Honorbuddy's NPC Database, Checking Mr.GearBuyers Vendor List for Information");
                    if (LoadedVendors.Count == 0)
                    {
                        Slog("We didnt cache vendors for some reason, doing so now");
                        CacheVendors();
                    }
// ReSharper disable PossibleNullReferenceException
                    ManualVendor myFoundVendor = LoadedVendors.FindById(npc.Entry);
// ReSharper restore PossibleNullReferenceException
                    if (myFoundVendor != null)
                    {

                        vendor = new Vendor(myFoundVendor.NpcId, myFoundVendor.NpcName, Vendor.VendorType.Unknown, myFoundVendor.NpcLocation);
                        Dlog("We Found {0} in Our Cache", myFoundVendor.NpcName);
                    }
                    else
                    {
                        Slog("MrGearBuyer could not locate vendor in MrGearBuyer's Vendor List. please move MrGearBuyer Closer to the NPC.");
                        Slog("Dont forget to set your HomePoint closer as well.");
                        return;
                    }
                   
                }
            }
            else
            {
               
                vendor = new Vendor(vendorAsUnit, Vendor.VendorType.Unknown);
            }

            // Setting ItemToBuy here so VendorBehavior knows which item we want.
            ItemToBuy = firstItem;

            //We need to make sure vender is usable, so removing blacklist. 
            try
            {
// ReSharper disable PossibleNullReferenceException
                if (Blacklist.Contains(vendorAsUnit.Guid, BlacklistFlags.All))
// ReSharper restore PossibleNullReferenceException
                {
                    Slog("For whatever reason vendor is blacklisted, Clearing Blacklist.");
                    Blacklist.Flush();
                }
            }
            catch (Exception ex)
            {
                Dlog(ex.StackTrace);
            }



            // Finally setting the poi
            BotPoi.Current = new BotPoi(vendor, PoiType.Buy);
            //BotPoi.Current = new BotPoi(Me.Location, PoiType.Hotspot);
        }

        #endregion

        #region Methods

        //Logging Class for your conviance
        public static void Slog(string format, params object[] args)
        {
            Logging.Write("[Mr.GearBuyer 3.3]: " + format, args);
        }
        public static void Dlog(string format, params object[] args)
        {
            Logging.WriteDiagnostic("[MGB 3.3 - DEBUG]: " + format, args);
        }

        private static void BuyItemListListChanged(object sender, ListChangedEventArgs e)
        {
            SaveSettings();
        }

        private static void BuyItems(BuyItemsEventArgs args)
        {
            if (ItemToBuy == null)
                return;
            //yet another Add to keep it from buying JP or HP from vendors when not on the proper contients. 
            if ((Me.MapId != 0 || Me.MapId != 1) && (ItemToBuy.ItemId == 392 || ItemToBuy.ItemId == 395))
            {
                BuyItemList.Remove(ItemToBuy);
                Slog("Justice or Honorpoints are on your buy list with no vendors avalible on your contient. so the item has been removed");
            }
            // Little hack here for just Honor and Justice points. Core fails to buy these 2 
            if (ItemToBuy.ItemId == 392 || ItemToBuy.ItemId == 395)
            {
                var item = MerchantFrame.Instance.GetAllMerchantItems().FirstOrDefault(i => i.ItemId == ItemToBuy.ItemId);

                if (item == null)
                {
                    Slog("Oops! MrGearBuyer Could not locate the item the your trying to buy {0}[{1}]. Are you sure the Vendor has this item?", ItemToBuy.ItemName, ItemToBuy.ItemId);
                }
                else
                {
                    Lua.DoString("BuyMerchantItem(" + (item.Index + 1) + ")");
                    Slog("Bought item \"{0}\" for {1} {2}.", ItemToBuy.ItemName, ItemToBuy.ItemCost.ToString(CultureInfo.InvariantCulture), ItemToBuy.ItemCostType);
                    // Adding a sleep here to let WoW update itself with currencies. So we don't end up buying something wrong.
                    Thread.Sleep(1000);
                    PulseTimer.Reset();
                }
            }
            else
            {
                var item = MerchantFrame.Instance.GetAllMerchantItems().FirstOrDefault(i => i.ItemId == ItemToBuy.ItemId);
                if (item == null)
                {
                    Slog("Oops! Something went wrong while trying to buy {0}[{1}]. Please consult to the profile writer", ItemToBuy.ItemName, ItemToBuy.ItemId);
                }
                else
                {
                    Lua.DoString("BuyMerchantItem(" + (item.Index + 1) + ")");
                    Slog("Bought item \"{0}\".", ItemToBuy.ItemName);
                    // Adding a sleep here to let WoW update itself with currencies. So we don't end up buying something wrong.
                    Thread.Sleep(1000);

                    BuyItemList.Remove(ItemToBuy);

                    if (EquipAfterBuy && (item.ItemInfo.EquipSlot == InventoryType.Head || item.ItemInfo.EquipSlot == InventoryType.Neck ||
                        item.ItemInfo.EquipSlot == InventoryType.Shoulder || item.ItemInfo.EquipSlot == InventoryType.Cloak ||
                        item.ItemInfo.EquipSlot == InventoryType.Chest || item.ItemInfo.EquipSlot == InventoryType.Wrist || item.ItemInfo.EquipSlot == InventoryType.Hand ||
                        item.ItemInfo.EquipSlot == InventoryType.Waist || item.ItemInfo.EquipSlot == InventoryType.Legs || item.ItemInfo.EquipSlot == InventoryType.Feet ||
                        item.ItemInfo.EquipSlot == InventoryType.WeaponMainHand || item.ItemInfo.EquipSlot == InventoryType.WeaponOffHand))
                    {
                        Thread.Sleep(2000); // Adding Extra sleep if we're equiping things. we wanna make sure the item gets added to inventory. 
                        ObjectManager.Update();
                        var bagItem = Me.BagItems.FirstOrDefault(i => i.ItemInfo.Id == item.ItemId);
                        //Lua.DoString("EquipItemByName(" + (item.ItemId) + ")");
                        EquipItem(bagItem);
                        Slog("Equiping \"{0}\".", item.Name);
                        // Adding a sleep here to let WoW update itself with items. So we don't end up equiping bad things. .
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Dlog("Will not AutoEquip {0} Ether Feature Disabled or Not Supported InventoryType {1}", item.Name, item.ItemInfo.EquipSlot.ToString());
                    }
                    PulseTimer.Reset();
                }
            }

            
           
            if (!RemoveHPJPWhenCapped || (ItemToBuy.ItemId != 392 && ItemToBuy.ItemId != 395) || _forceAddedPoints)
            {
                Slog("Removing {0} from List Since we bought the Item", ItemToBuy.ItemName);
                BuyItemList.Remove(ItemToBuy);
                _forceAddedPoints = false;
            }
            else
            {
                if (ItemToBuy.ItemId == 392 && WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints).Amount >= 3750)
                {
                    Slog("Removing Honor Points from List Since Capped");
                    BuyItemList.Remove(ItemToBuy);
                }
                else if (ItemToBuy.ItemId == 395 && WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints).Amount >= 3750)
                {
                    Slog("Removing Justice Points from List Since Capped");
                    BuyItemList.Remove(ItemToBuy);
                }

            }

            ItemToBuy = null;
        }

        #endregion
        private static void EquipItem(int bagIndex, int bagSlot, int targetSlot)
        {
            Lua.DoString(
                "ClearCursor(); PickupContainerItem({0}, {1}); EquipCursorItem({2}); if StaticPopup1Button1 and StaticPopup1Button1:IsVisible() then StaticPopup1Button1:Click(); end;",
                bagIndex + 1, bagSlot + 1, targetSlot);
        }

        private static void EquipItem(WoWItem item)
        {
            EquipItem(item.BagIndex, item.BagSlot, (int)item.ItemInfo.EquipSlot);
        }

        #region Properties and Fields




        private static bool _forceAddedPoints;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static BuyItemInfo ItemToBuy { get; set; }

        public static readonly BindingList<BuyItemInfo> BuyItemList = new BindingList<BuyItemInfo>();

        private static bool _logoutAtCap = true;
        public static bool LogoutAtCap { get { return _logoutAtCap; } set { _logoutAtCap= value; SaveSettings(); } }

        private static bool _removeHPJPWhenCapped = true;
        public static bool RemoveHPJPWhenCapped { get { return _removeHPJPWhenCapped; } set { _removeHPJPWhenCapped = value; SaveSettings(); } }

        private static bool _buyOppositePointToBuildUp = true;
        public static bool BuyOppositePointToBuildUp { get { return _buyOppositePointToBuildUp; } set { _buyOppositePointToBuildUp = value; SaveSettings(); } }


        private static float _homeX;
        public static float HomeX { get { return _homeX; } set { _homeX = value; SaveSettings(); } }

        private static float _homeY;
        public static float HomeY { get { return _homeY; } set { _homeY = value; SaveSettings(); } }

        private static float _homeZ;
        public static float HomeZ { get { return _homeZ; } set { _homeZ = value; SaveSettings(); } }

        private static int _mapID = -1;
        public static int MapID { get { return _mapID; } set { _mapID = value; SaveSettings(); } }

        private static bool _hpSet;
        public static bool HPSet { get { return _hpSet; } set { _hpSet = value; SaveSettings(); } }

        private static string _repairName = "";
        public static string RepairName { get { return _repairName; } set { _repairName = value; SaveSettings(); } }

        private static float _repairX;
        public static float RepairX { get { return _repairX; } set { _repairX = value; SaveSettings(); } }

        private static float _repairY;
        public static float RepairY { get { return _repairY; } set { _repairY = value; SaveSettings(); } }

        private static float _repairZ;
        public static float RepairZ { get { return _repairZ; } set { _repairZ = value; SaveSettings(); } }

        private static int _repairMapID;
        public static int RepairMapID { get { return _repairMapID; } set { _repairMapID = value; SaveSettings(); } }

        private static Int32 _repairID;
        public static Int32 RepairID { get { return _repairID; } set { _repairID = value; SaveSettings(); } }

        private static bool _repairSet;
        public static bool RepairSet { get { return _repairSet; } set { _repairSet = value; SaveSettings(); } }

        private static bool _repairSwap;
        public static bool RepairSwap { get { return _repairSwap; } set { _repairSwap = value; SaveSettings(); } }

        private static bool _equipAfterBuy;
        public static bool EquipAfterBuy { get { return _equipAfterBuy; } set { _equipAfterBuy = value; SaveSettings(); } }

        private static bool _myMount;
        public static bool MyMount { get { return _myMount; } set { _myMount = value; SaveSettings(); } }

        private static bool _logOutHPCap;
        public static bool LogOutHPCap { get { return _logOutHPCap; } set { _logOutHPCap = value; SaveSettings(); } }

        private static bool _logOutJPCap;
        public static bool LogOutJPCap { get { return _logOutJPCap; } set { _logOutJPCap = value; SaveSettings(); } }

        #endregion

        #region Settings

        private static string SavePath
        {
            get { return string.Format("{0}\\Settings\\MrGearBuyerSettings_{1}.xml", AppDomain.CurrentDomain.BaseDirectory, StyxWoW.Me.Name); }
        }

        private static void LoadSettings()
        {
            if (!File.Exists(SavePath))
                return;

            var xml = XElement.Load(SavePath);

            // Loading settings first.
            var settingsXml = xml.Element("Settings");

            if (settingsXml != null)
            {
                var logoutAtCap = settingsXml.Element("LogoutAtCap");
                if (logoutAtCap != null)
                    _logoutAtCap = Convert.ToBoolean(logoutAtCap.Value);

                var removeHPJPWhenCapped = settingsXml.Element("RemoveHPJPWhenCapped");
                if (removeHPJPWhenCapped != null)
                    _removeHPJPWhenCapped = Convert.ToBoolean(removeHPJPWhenCapped.Value);

                var buyOppositePointToBuildUp = settingsXml.Element("BuyOppositePointToBuildUp");
                if (buyOppositePointToBuildUp != null)
                    _buyOppositePointToBuildUp = Convert.ToBoolean(buyOppositePointToBuildUp.Value);

                var mapID = settingsXml.Element("MapID");
                if (mapID != null)
                    _mapID = Convert.ToInt32(mapID.Value);

                var homeX = settingsXml.Element("HomeX");
                if (homeX != null)
                    _homeX = Convert.ToSingle(homeX.Value, CultureInfo.InvariantCulture);

                var homeY = settingsXml.Element("HomeY");
                if (homeY != null)
                    _homeY = Convert.ToSingle(homeY.Value, CultureInfo.InvariantCulture);

                var homeZ = settingsXml.Element("HomeZ");
                if (homeZ != null)
                    _homeZ = Convert.ToSingle(homeZ.Value, CultureInfo.InvariantCulture);

                var repairX = settingsXml.Element("RepairX");
                if (repairX != null)
                    _repairX = Convert.ToSingle(repairX.Value, CultureInfo.InvariantCulture);

                var repairY = settingsXml.Element("RepairY");
                if (repairY != null)
                    _repairY = Convert.ToSingle(repairY.Value, CultureInfo.InvariantCulture);

                var repairZ = settingsXml.Element("RepairZ");
                if (repairZ != null)
                    _repairZ = Convert.ToSingle(repairZ.Value, CultureInfo.InvariantCulture);

                var hpSet = settingsXml.Element("HPSet");
                if (hpSet != null)
                    _hpSet = Convert.ToBoolean(hpSet.Value);

                var repairName = settingsXml.Element("RepairName");
                if (repairName != null)
                    _repairName = repairName.Value;

                var repairMapID = settingsXml.Element("RepairMapID");
                if (repairMapID != null)
                    _repairMapID = Convert.ToInt32(repairMapID.Value);

                var repairID = settingsXml.Element("RepairID");
                if (repairID != null)
                    _repairID = Convert.ToInt32(repairID.Value);

                var repairSet = settingsXml.Element("RepairSet");
                if (repairSet != null)
                    _repairSet = Convert.ToBoolean(repairSet.Value);

                var repairSwap = settingsXml.Element("RepairSwap");
                if (repairSwap != null)
                    _repairSwap = Convert.ToBoolean(repairSwap.Value);

                var equipAfterBuy = settingsXml.Element("EquipAfterBuy");
                if (equipAfterBuy != null)
                    _equipAfterBuy = Convert.ToBoolean(equipAfterBuy.Value);

                var myMount = settingsXml.Element("MyMount");
                if (myMount != null)
                    _myMount = Convert.ToBoolean(myMount.Value);

                var logOutHPCap = settingsXml.Element("LogOutHPCap");
                if (logOutHPCap != null)
                    _logOutHPCap = Convert.ToBoolean(logOutHPCap.Value);

                var logOutJPCap = settingsXml.Element("LogOutJPCap");
                if (logOutJPCap != null)
                    _logOutJPCap = Convert.ToBoolean(logOutJPCap.Value);

            }

            var items = xml.Element("Items");

            if (items != null)
            {
                foreach (var t in items.Elements("Item"))
                {
                    BuyItemList.Add(new BuyItemInfo(t));
                }
            }
        }
        public static void Save()
        {
            SaveSettings();
        }
        private static void SaveSettings()
        {
// ReSharper disable AssignNullToNotNullAttribute
            if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
// ReSharper restore AssignNullToNotNullAttribute

            var xml = new XElement("MrGearBuyerConfig",
                            new XElement("Settings",
                                new XElement("LogoutAtCap", LogoutAtCap),
                                new XElement("RemoveHPJPWhenCapped", RemoveHPJPWhenCapped),
                                new XElement("BuyOppositePointToBuildUp", BuyOppositePointToBuildUp),
                                new XElement("MapID", MapID),
                                new XElement("HomeX", HomeX),
                                new XElement("HomeY", HomeY),
                                new XElement("HomeZ", HomeZ),
                                new XElement("HPSet", HPSet),
                                new XElement("RepairName", RepairName),
                                new XElement("RepairX", RepairX),
                                new XElement("RepairY", RepairY),
                                new XElement("RepairZ", RepairZ),
                                new XElement("RepairMapID", RepairMapID),
                                new XElement("RepairID", RepairID),
                                new XElement("RepairSet", RepairSet),
                                new XElement("RepairSwap", RepairSwap),
                                new XElement("EquipAfterBuy", EquipAfterBuy),
                                new XElement("MyMount", MyMount),
                                new XElement("LogOutHPCap", LogOutHPCap),
                                new XElement("LogOutJPCap", LogOutJPCap)
                                ),
                            new XElement("Items",
                                from i in BuyItemList
                                select i.ToXml()));

            xml.Save(SavePath);
        }

        #endregion

        #region Nested Class BuyItemInfo

        public class BuyItemInfo : INotifyPropertyChanged
        {
            #region Constructors

            public BuyItemInfo()
            { }

            public BuyItemInfo(XElement xml)
            {
                // ReSharper disable PossibleNullReferenceException
                ItemId = Convert.ToUInt32(xml.Element("ItemId").Value);
                ItemName = xml.Element("ItemName").Value;
                ItemCost = Convert.ToInt32(xml.Element("ItemCost").Value);
                ItemCostType = xml.Element("ItemCostType").Value;
                ItemSupplierId = Convert.ToUInt32(xml.Element("ItemSupplierId").Value);
                // ReSharper restore PossibleNullReferenceException
            }

            #endregion

            #region Properties

            private string _itemName;

            public string ItemName
            {
                get { return _itemName; }
                set
                {
                    _itemName = value;
                    OnPropertyChanged("ItemName");
                }
            }

            private uint _itemId;
            [Browsable(false)]
            public uint ItemId
            {
                get { return _itemId; }
                set
                {
                    _itemId = value;
                    OnPropertyChanged("ItemID");
                }
            }

            private int _itemCost;
            public int ItemCost
            {
                get { return _itemCost; }
                set
                {
                    _itemCost = value;
                    OnPropertyChanged("ItemCost");
                }
            }

            private string _itemCostType;
            public string ItemCostType
            {
                get { return _itemCostType; }
                set
                {
                    _itemCostType = value;
                    OnPropertyChanged("ItemCostType");
                }
            }

            private uint _itemSupplierId;
            [Browsable(false)]
            public uint ItemSupplierId
            {
                get { return _itemSupplierId; }
                set
                {
                    _itemSupplierId = value;
                    OnPropertyChanged("ItemSupplierId");
                }
            }

            #endregion

            #region Methods

            public XElement ToXml()
            {
                return new XElement(
                            "Item",
                            new XElement("ItemId", ItemId),
                            new XElement("ItemName", ItemName),
                            new XElement("ItemCost", ItemCost),
                            new XElement("ItemCostType", ItemCostType),
                            new XElement("ItemSupplierId", ItemSupplierId)
                            );
            }

            #endregion

            #region Overrides

            public override string ToString()
            {
                return string.Format("ItemId:{0} ItemName:{1} ItemCost:{2} ItemCostType:{3} ItemSupplierId:{4}", ItemId, ItemName, ItemCost, ItemCostType, ItemSupplierId);
            }

            #endregion

            #region Implementation of INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string fieldName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(fieldName));
                }
            }

            #endregion
        }

        #endregion
    }
}

