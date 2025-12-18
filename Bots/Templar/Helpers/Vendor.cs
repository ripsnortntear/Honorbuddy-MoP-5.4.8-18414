using System.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Templar.GUI.Tabs;

namespace Templar.Helpers
{
    /// <summary>
    /// Handles vendor-related operations, including scanning bags for items to sell, repairing equipment, and interacting with vendors.
    /// Supports selling items based on quality, protection settings, and user preferences.
    /// </summary>
    public class Vendor
    {
        // Constants for delays (adjust as needed for MOP)
        private const int VendorActionDelayMs = 1000; // Delay between actions to avoid UI spam

        /// <summary>
        /// Scans the player's bags and populates Variables.VendorSellList with items eligible for selling.
        /// Items are filtered by quality, protection settings, sell price, and quest status.
        /// </summary>
        public static void CheckBags()
        {
            Variables.VendorSellList.Clear();

            foreach (var bagItem in StyxWoW.Me.BagItems.Where(bagItem =>
                ProtectedItemSettings.Instance.ProtectedItems.All(pi => pi.Entry != bagItem.Entry) &&
                !ProtectedItemsManager.GetAllItemIds().Contains(bagItem.Entry) &&
                bagItem.ItemInfo.SellPrice > 0 &&
                bagItem.ItemInfo.BeginQuestId == 0 &&
                bagItem.ItemInfo.Bond != WoWItemBondType.Quest))
            {
                bool shouldSell = false;
                switch (bagItem.Quality)
                {
                    case WoWItemQuality.Poor:
                        shouldSell = VendorSettings.Instance.SellGrays;
                        break;
                    case WoWItemQuality.Common:
                        shouldSell = VendorSettings.Instance.SellWhites;
                        break;
                    case WoWItemQuality.Uncommon:
                        shouldSell = VendorSettings.Instance.SellGreens;
                        break;
                    case WoWItemQuality.Rare:
                        shouldSell = VendorSettings.Instance.SellBlues;
                        break;
                    case WoWItemQuality.Epic:
                        shouldSell = VendorSettings.Instance.SellPurples;
                        break;
                }

                if (shouldSell && !Variables.VendorSellList.Contains(bagItem))
                {
                    Variables.VendorSellList.Add(bagItem);
                }
            }

            CustomLog.Normal("Vendor check complete. Items to sell: {0}", Variables.VendorSellList.Count);
        }

        /// <summary>
        /// Main entry point for handling vendoring logic. Checks for vendors, mounts if needed, and processes selling/repairing.
        /// If no vendor is found, stops the bot to prevent getting stuck.
        /// </summary>
        public static void HandleVendoring()
        {
            if (!VendorSettings.Instance.Vendor)
            {
                return; // Vendoring disabled
            }

            var vendorMount = CheckVendorMount();
            if (vendorMount != null && vendorMount.CanMount)
            {
                if (!IsOnVendorMount())
                {
                    if (Variables.MountUpStopwatch.IsRunning && Variables.MountUpStopwatch.ElapsedMilliseconds < 5000)
                    {
                        return; // Wait for mount
                    }
                    Mount.SummonMount(vendorMount.CreatureSpellId);
                    Variables.MountUpStopwatch.Restart();
                    return;
                }
                else
                {
                    HandleCloseVendor();
                    return;
                }
            }

            // No mount or not mountable; proceed with walking
            if (Variables.CloseRepairVendor != null)
            {
                HandleCloseVendor();
                return;
            }

            if (Variables.FarRepairVendor != null)
            {
                HandleFarVendor();
                return;
            }

            CustomLog.Normal("Could not find any repair vendors in the cache.");
            CustomLog.Normal("Stopping the bot to prevent getting stuck.");
            CustomLog.Normal("To prevent this, manually find a repair vendor on this continent and start the bot near it.");
            CustomLog.Normal("Then you can go back to your spot and start the botbase again.");
            TreeRoot.Stop("Could not find any repair vendor on this continent in the cache.");
        }

        /// <summary>
        /// Checks for a vendor mount (e.g., Tundra Mammoth or Expedition Yak) based on faction.
        /// </summary>
        private static Mount.MountWrapper CheckVendorMount()
        {
            return StyxWoW.Me.IsAlliance
                ? Mount.GroundMounts.FirstOrDefault(
                    mount => mount.CreatureSpellId == Variables.AllianceTundraMammothSpell || mount.CreatureSpellId == Variables.ExpeditionYakSpell)
                : Mount.GroundMounts.FirstOrDefault(
                    mount => mount.CreatureSpellId == Variables.HordeTundraMammothSpell || mount.CreatureSpellId == Variables.ExpeditionYakSpell);
        }

        /// <summary>
        /// Checks if the player is currently on a vendor mount.
        /// </summary>
        private static bool IsOnVendorMount()
        {
            return StyxWoW.Me.HasAura(Variables.AllianceTundraMammothSpell) ||
                   StyxWoW.Me.HasAura(Variables.HordeTundraMammothSpell) ||
                   StyxWoW.Me.HasAura(Variables.ExpeditionYakSpell);
        }

        /// <summary>
        /// Handles moving to and interacting with a far vendor.
        /// </summary>
        private static void HandleFarVendor()
        {
            if (Variables.FarRepairVendor == null)
            {
                CustomLog.Normal("Could not find far repair vendor.");
                return;
            }

            double distance = Variables.FarRepairVendor.Location.Distance(StyxWoW.Me.Location);
            CustomLog.Diagnostic("We have a repair vendor! Distance: {0}", distance);

            if (distance > 30)
            {
                Flightor.MoveTo(Variables.FarRepairVendor.Location, true);
            }
            else
            {
                HandleCloseVendor();
            }
        }

        /// <summary>
        /// Handles interacting with a close vendor and processing sales/repairs.
        /// </summary>
        private static void HandleCloseVendor()
        {
            if (Variables.CloseRepairVendor == null)
            {
                CustomLog.Normal("Could not find close repair vendor.");
                return;
            }

            if (!Variables.CloseRepairVendor.WithinInteractRange)
            {
                Flightor.MoveTo(Variables.CloseRepairVendor.Location, true);
            }
            else
            {
                if (!MerchantFrame.Instance.IsVisible)
                {
                    Variables.CloseRepairVendor.Interact();
                    StyxWoW.Sleep(1000); // Wait for frame to open
                }
                else
                {
                    HandleRepairAndSales();
                }
            }
        }

        /// <summary>
        /// Sells items from the sell list and repairs equipment if affordable.
        /// </summary>
        private static void HandleRepairAndSales()
        {
            if (!MerchantFrame.Instance.IsVisible)
            {
                return;
            }

            // Sell items
            foreach (var bagItem in Variables.VendorSellList.ToList()) // ToList() for safe removal
            {
                try
                {
                    MerchantFrame.Instance.SellItem(bagItem);
                    Variables.VendorSellList.Remove(bagItem);
                    CustomLog.Normal("Sold item: {0}", bagItem.Name);
                    StyxWoW.Sleep(200); // Small delay between sells
                }
                catch (Exception ex)
                {
                    CustomLog.Normal("Failed to sell item {0}: {1}", bagItem.Name, ex.Message);
                }
            }

            // Repair if possible
            if (Variables.HasEnoughForRepairs)
            {
                MerchantFrame.Instance.RepairAllItems();
                CustomLog.Normal("Repaired all items.");
                StyxWoW.Sleep(VendorActionDelayMs);
            }
            else
            {
                CustomLog.Normal("Did not have enough gold to repair.");
                TreeRoot.Stop("Did not have enough gold to repair.");
            }
        }
    }
}
