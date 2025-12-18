using System.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Templar.GUI.Tabs;

namespace Templar.Helpers {
    public class Vendor {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static void CheckBags() {
            Variables.VendorSellList.Clear();

            foreach(var bagItem in StyxWoW.Me.BagItems.Where(bagItem =>
                    ProtectedItemSettings.Instance.ProtectedItems.All(pi => pi.Entry != bagItem.Entry) &&
                    !ProtectedItemsManager.GetAllItemIds().Contains(bagItem.Entry) &&
                    bagItem.ItemInfo.SellPrice > 0 && bagItem.ItemInfo.BeginQuestId == 0 && bagItem.ItemInfo.Bond != WoWItemBondType.Quest)) {

                switch(bagItem.Quality) {
                    case WoWItemQuality.Poor:
                        if(VendorSettings.Instance.SellGrays) {
                            Variables.VendorSellList.Add(bagItem);
                        }

                        break;

                    case WoWItemQuality.Common:
                        if(VendorSettings.Instance.SellWhites) {
                            Variables.VendorSellList.Add(bagItem);
                        }

                        break;

                    case WoWItemQuality.Uncommon:
                        if(VendorSettings.Instance.SellGreens) {
                            Variables.VendorSellList.Add(bagItem);
                        }

                        break;

                    case WoWItemQuality.Rare:
                        if(VendorSettings.Instance.SellBlues) {
                            Variables.VendorSellList.Add(bagItem);
                        }

                        break;

                    case WoWItemQuality.Epic:
                        if(VendorSettings.Instance.SellPurples) {
                            Variables.VendorSellList.Add(bagItem);
                        }

                        break;
                }
            }
        }

        public static void HandleVendoring() {
            if(CheckVendorMount() != null) {
                if(!CheckVendorMount().CanMount) {
                    return;
                }

                if(!IsOnVendorMount()) {
                    if(Variables.MountUpStopwatch.IsRunning && Variables.MountUpStopwatch.ElapsedMilliseconds < 5000) {
                        return;
                    }

                    Mount.SummonMount(CheckVendorMount().CreatureSpellId);
                    Variables.MountUpStopwatch.Restart();
                } else {
                    HandleCloseVendor();
                }
            } else {
                if(Variables.CloseRepairVendor != null) {
                    HandleCloseVendor();
                    return;
                }

                if(Variables.FarRepairVendor != null) {
                    HandleFarVendor();
                    return;
                }

                CustomLog.Normal("Could not find any repair vendors in the cache.");
                CustomLog.Normal("Stopping the bot to prevent that we get stuck here.");
                CustomLog.Normal("To prevent this from happening, manually find a repair vendor on this continent and start the bot near it.");
                CustomLog.Normal("Then you can go back to your spot and start the botbase again.");
                TreeRoot.Stop("Could not find any repair vendor on this continent in the cache.");
            }
        }


        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static Mount.MountWrapper CheckVendorMount() {
            return StyxWoW.Me.IsAlliance
                ? Mount.GroundMounts.FirstOrDefault(
                    mount => mount.CreatureSpellId == Variables.AllianceTundraMammothSpell || mount.CreatureSpellId == Variables.ExpeditionYakSpell)
                : Mount.GroundMounts.FirstOrDefault(
                    mount => mount.CreatureSpellId == Variables.HordeTundraMammothSpell || mount.CreatureSpellId == Variables.ExpeditionYakSpell);
        }

        private static bool IsOnVendorMount() {
            return StyxWoW.Me.HasAura(Variables.AllianceTundraMammothSpell) || StyxWoW.Me.HasAura(Variables.HordeTundraMammothSpell) ||
                   StyxWoW.Me.HasAura(Variables.ExpeditionYakSpell);
        }

        private static void HandleFarVendor() {
            if(Variables.FarRepairVendor == null) {
                CustomLog.Normal("Could not find far repair vendor.");
            } else {
                //CustomLog.Normal("We have a repair vendor! Name: {0}, Distance: {1}", repairVendor.Name, repairVendor.Location.Distance(StyxWoW.Me.Location));

                if(Variables.FarRepairVendor.Location.Distance(StyxWoW.Me.Location) > 30) {
                    Flightor.MoveTo(Variables.FarRepairVendor.Location, true);
                } else {
                    HandleCloseVendor();
                }
            }
        }

        private static void HandleCloseVendor() {
            if(Variables.CloseRepairVendor == null) {
                CustomLog.Normal("Could not find close repair vendor.");
            } else {
                //CustomLog.Normal("We have a repair vendor! Name: {0}, Distance: {1}", nearRepairVendor.Name, nearRepairVendor.Location.Distance(StyxWoW.Me.Location));

                if(!Variables.CloseRepairVendor.WithinInteractRange) {
                    Flightor.MoveTo(Variables.CloseRepairVendor.Location, true);
                } else {
                    if(!MerchantFrame.Instance.IsVisible) {
                        Variables.CloseRepairVendor.Interact();
                    } else {
                        HandleRepairAndSales();
                    }
                }
            }
        }

        private static void HandleRepairAndSales() {
            foreach(var bagItem in StyxWoW.Me.BagItems.Where(bagItem => Variables.VendorSellList.Any(sellItem => sellItem == bagItem))) {
                MerchantFrame.Instance.SellItem(bagItem);
            }

            if(Variables.HasEnoughForRepairs) {
                MerchantFrame.Instance.RepairAllItems();
            } else {
                CustomLog.Normal("Did not have enough gold to repair.");
                TreeRoot.Stop("Did not have enough gold to repair.");
            }
        }
    }
}