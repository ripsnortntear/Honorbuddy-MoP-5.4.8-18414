using System; // For Exception
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
        public class Vendor
        {
            private
            const int VendorActionDelayMs = 1000;
            public static void CheckBags()
            {
                Variables.VendorSellList.Clear();
                foreach(var bagItem in StyxWoW.Me.BagItems.Where(bagItem => ProtectedItemSettings.Instance.ProtectedItems.All(pi => pi.Entry != bagItem.Entry) && !ProtectedItemsManager.GetAllItemIds().Contains(bagItem.Entry) && bagItem.ItemInfo.SellPrice > 0 && bagItem.ItemInfo.BeginQuestId == 0 && bagItem.ItemInfo.Bond != WoWItemBondType.Quest))
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
            public static void HandleVendoring()
            {
                if (!VendorSettings.Instance.Vendor) return;
                var vendorMount = CheckVendorMount();
                if (vendorMount != null && vendorMount.CanMount)
                {
                    if (!IsOnVendorMount())
                    {
                        if (Variables.MountUpStopwatch.IsRunning && Variables.MountUpStopwatch.ElapsedMilliseconds < 5000) return;
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
                TreeRoot.Stop("Could not find any repair vendor on this continent in the cache.");
            }
            private static Mount.MountWrapper CheckVendorMount()
            {
                return StyxWoW.Me.IsAlliance ? Mount.GroundMounts.FirstOrDefault(mount => mount.CreatureSpellId == Variables.AllianceTundraMammothSpell || mount.CreatureSpellId == Variables.ExpeditionYakSpell) : Mount.GroundMounts.FirstOrDefault(mount => mount.CreatureSpellId == Variables.HordeTundraMammothSpell || mount.CreatureSpellId == Variables.ExpeditionYakSpell);
            }
            private static bool IsOnVendorMount()
            {
                return StyxWoW.Me.HasAura(Variables.AllianceTundraMammothSpell) || StyxWoW.Me.HasAura(Variables.HordeTundraMammothSpell) || StyxWoW.Me.HasAura(Variables.ExpeditionYakSpell);
            }
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
                        StyxWoW.Sleep(1000);
                    }
                    else
                    {
                        HandleRepairAndSales();
                    }
                }
            }
            private static void HandleRepairAndSales()
            {
                if (!MerchantFrame.Instance.IsVisible) return;
                foreach(var bagItem in Variables.VendorSellList.ToList())
                {
                    try
                    {
                        MerchantFrame.Instance.SellItem(bagItem);
                        Variables.VendorSellList.Remove(bagItem);
                        CustomLog.Normal("Sold item: {0}", bagItem.Name);
                        StyxWoW.Sleep(200);
                    }
                    catch (Exception ex)
                    {
                        CustomLog.Normal("Failed to sell item {0}: {1}", bagItem.Name, ex.Message);
                    }
                }
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