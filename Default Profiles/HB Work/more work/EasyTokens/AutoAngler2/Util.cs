using System;
using System.Diagnostics;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace HighVoltz
{
    static class Utils
    {
        private static readonly LocalPlayer Me = StyxWoW.Me;
        private static uint _ping = Lua.GetReturnVal<uint>("return GetNetStats()", 3);
        private static readonly Stopwatch PingSW = new Stopwatch();

        public static bool IsLureOnPole
        {
            get
            {
			
				var lure = StyxWoW.Me.BagItems.FirstOrDefault(r => r.Entry == 85973);
				if (AutoAngler.Instance.MySettings.Poolfishing && lure != null && !Me.HasAura(125167))
				{
					return false;
				}
			
                //if poolfishing, dont need lure say we have one
                if (AutoAngler.Instance.MySettings.Poolfishing && !AutoAngler.FishAtHotspot)
                    return true;
					

				
                var ret = Lua.GetReturnValues("return GetWeaponEnchantInfo()");
                return ret != null && ret.Count > 0 && ret[0]  == "1";
            }
        }

        /// <summary>
        /// Returns WoW's ping, refreshed every 30 seconds.
        /// </summary>
        public static uint WoWPing
        {
            get
            {
                if (!PingSW.IsRunning)
                    PingSW.Start();
                if (PingSW.ElapsedMilliseconds > 30000)
                {
                    _ping = Lua.GetReturnVal<uint>("return GetNetStats()", 3);
                    PingSW.Reset();
                    PingSW.Start();
                }
                return _ping;
            }
        }

        public static bool IsItemInBag(uint entry)
        {
            return Me.BagItems.Any(i => i.Entry == entry);
        }

        public static WoWItem GetIteminBag(uint entry)
        {
            return StyxWoW.Me.BagItems.FirstOrDefault(i => i.Entry == entry);
        }

        public static void EquipWeapon()
        {
            bool is2Hand = false;
            // equip right hand weapon
            uint mainHandID = AutoAngler.Instance.MySettings.MainHand;
            WoWItem mainHand = StyxWoW.Me.Inventory.Equipped.MainHand;
            if (mainHand == null || (mainHand.Entry != mainHandID && Utils.IsItemInBag(mainHandID)))
            {
                var weapon = StyxWoW.Me.BagItems.FirstOrDefault(i => i.Entry == AutoAngler.Instance.MySettings.MainHand);
                is2Hand = weapon.ItemInfo.InventoryType == InventoryType.TwoHandWeapon || weapon.ItemInfo.InventoryType == InventoryType.Ranged;
                Utils.EquipItemByID(AutoAngler.Instance.MySettings.MainHand);
            }

            // equip left hand weapon
            uint offhandID = AutoAngler.Instance.MySettings.OffHand;
            WoWItem offhand = StyxWoW.Me.Inventory.Equipped.OffHand;

            if ((!is2Hand && offhandID > 0 &&
                 (offhand == null || (offhand.Entry != offhandID && Utils.IsItemInBag(offhandID)))))
            {
                Utils.EquipItemByID(AutoAngler.Instance.MySettings.OffHand);
            }
        }

        public static void UseItemByID(int id)
        {
            Lua.DoString("UseItemByName(\"" + id + "\")");
        }

        public static void EquipItemByName(String name)
        {
            Lua.DoString("EquipItemByName (\"" + name + "\")");
        }

        public static void EquipItemByID(uint id)
        {
            Lua.DoString("EquipItemByName ({0})", id);
        }

        public static void BlacklistPool(WoWGameObject pool, TimeSpan time, string reason)
        {
            Blacklist.Add(pool.Guid, time);
            AutoAngler.Instance.Log("Blacklisting {0} for {1} Reason: {2}", pool.Name, time, reason);
            BotPoi.Current = new BotPoi(PoiType.None);
        }
    }
}