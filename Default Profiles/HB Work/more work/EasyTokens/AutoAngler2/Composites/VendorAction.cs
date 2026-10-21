using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Action = Styx.TreeSharp.Action;
using Styx.Helpers;
using Styx;
using Styx.WoWInternals;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace HighVoltz.Composites
{
    public class VendorAction:Action
    {
         LocalPlayer _me = StyxWoW.Me;

         protected override RunStatus Run(object context)
         {
             Vendor ven = BotPoi.Current.AsVendor;
             WoWUnit vendor = ObjectManager.GetObjectsOfType<WoWUnit>().
                        FirstOrDefault(m => m.Entry == ven.Entry || m.Entry == ven.Entry2);
             WoWPoint loc = vendor != null ? vendor.Location : ven.Location;
             if (_me.Location.Distance(loc) > 4)
             {
                 if (AutoAngler.Instance.MySettings.Fly)
                     Flightor.MoveTo(WoWMathHelper.CalculatePointFrom(_me.Location, loc, 4));
                 else
                 {
                     if (!StyxWoW.Me.Mounted && Mount.ShouldMount(loc) && Mount.CanMount())
                         Mount.MountUp(() => loc);
                     Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(_me.Location, loc, 4));
                 }
             }
             else
             {
                 if (MerchantFrame.Instance == null || !MerchantFrame.Instance.IsVisible)
                 {
                     if (vendor == null)
                     {
                         AutoAngler.Instance.Log("No vendor found at location {0}. hearth + logging out instead", loc);
                         BotPoi.Current = new BotPoi(PoiType.InnKeeper);
                         return RunStatus.Failure;
                     }
                     vendor.Interact();
                 }
                 else
                 {
                     // sell all poor and common items not in protected Items list.
                     List<WoWItem> itemList = _me.BagItems.Where(i => !ProtectedItemsManager.Contains(i.Entry) &&
                         !i.IsSoulbound && !i.IsConjured && 
                         (i.Quality == WoWItemQuality.Poor || i.Quality == WoWItemQuality.Common)).ToList();
                     foreach (var item in itemList)
                     {
                         item.UseContainerItem();
                     }
                     MerchantFrame.Instance.RepairAllItems();
                     BotPoi.Current = new BotPoi(PoiType.None);
                 }
             }
             return RunStatus.Success;
         }
    }
}
