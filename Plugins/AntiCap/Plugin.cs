using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Styx;
using Styx.Common;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot.Frames;

namespace AntiCap
{
    public class Plugin : HBPlugin
    {
        private Stopwatch _buyDelay = new Stopwatch();
        private Stopwatch _moveDelay = new Stopwatch();
        public override void Pulse()
        {
            if (!Styx.StyxWoW.IsInGame || !Styx.StyxWoW.IsInWorld)
                return;
            if (AntiCapForm.Instance != null && AntiCapForm.Instance.IsAdding)
            {
                using(Styx.StyxWoW.Memory.AcquireFrame())
                {
                    var cursorHasMerchantItem = Lua.GetReturnVal<int>("cursorType,_ = GetCursorInfo() if cursorType==\"merchant\" then return 1 else return 0 end", 0) == 1;
                    if (cursorHasMerchantItem)
                    {
                        var cursorInfo = Lua.GetReturnValues("return GetCursorInfo()");
                        var index = uint.Parse(cursorInfo[1]);
                        var itemInfo = Lua.GetReturnValues(string.Format("return GetMerchantItemInfo({0})", index));
                        var item = new Item();
                        item.ItemID = Lua.GetReturnVal<uint>(string.Format("id = string.match(GetMerchantItemLink({0}), \"item:[%d]+\") id=string.sub(id, 6) return id", index), 0);
                        item.Amount = int.Parse(itemInfo[3]);
                        var existingItem = Settings.Instance.BuyItems.FirstOrDefault(i => i.ItemID == item.ItemID);
                        if (existingItem != null)
                        {
                            item.Amount += existingItem.Amount;
                            if (AntiCapForm.Instance.NeedAmounts.ContainsKey(item.ItemID))
                                AntiCapForm.Instance.NeedAmounts[item.ItemID] = NeedAmount(item);
                            else
                                AntiCapForm.Instance.NeedAmounts.Add(item.ItemID, NeedAmount(item));
                            existingItem.Amount = item.Amount;
                            existingItem.NotifyPropertyChanged("ItemName");
                            existingItem.NotifyPropertyChanged("PricesString");
                        }
                        else
                        {
                            item.ItemName = itemInfo[0];
                            var currencyCount = Lua.GetReturnVal<int>(string.Format("return GetMerchantItemCostInfo({0})", index), 0);
                            item.Prices = new BindingList<Price>();
                            for (int i = 1; i <= currencyCount; i++)
                            {
                                var currencyId = (from currency in Item.AllCurrencies
                                                  let lua =
                                                      string.Format(
                                                          "_,_,_,curName = GetMerchantItemCostItem({0},{1}) otherCurName,_ = GetCurrencyInfo({2}) if curName == otherCurName then return 1 else return 0 end",
                                                          index, i, currency)
                                                  where Lua.GetReturnVal<int>(lua, 0) == 1
                                                  select currency).FirstOrDefault();

                                var price = new Price();
                                var currencyInfo = Lua.GetReturnValues(string.Format("return GetMerchantItemCostItem({0},{1})", index, i));
                                price.Amount = int.Parse(currencyInfo[1]);
                                price.CurrencyName = currencyInfo[3];
                                price.Currency = currencyId;
                                item.Prices.Add(price);
                            }
                            //item.VendorID = MerchantFrame.Instance.Merchant.Entry;
                            //item.VendorX = MerchantFrame.Instance.Merchant.Location.X;
                            //item.VendorY = MerchantFrame.Instance.Merchant.Location.Y;
                            //item.VendorZ = MerchantFrame.Instance.Merchant.Location.Z;
                            item.VendorID = StyxWoW.Me.CurrentTarget.Entry;
                            item.VendorX = StyxWoW.Me.CurrentTarget.Location.X;
                            item.VendorY = StyxWoW.Me.CurrentTarget.Location.Y;
                            item.VendorZ = StyxWoW.Me.CurrentTarget.Location.Z;
                            item.Quality = Lua.GetReturnVal<uint>(string.Format("return GetItemInfo({0})", item.ItemID), 2);
                            if (AntiCapForm.Instance.NeedAmounts.ContainsKey(item.ItemID))
                                AntiCapForm.Instance.NeedAmounts[item.ItemID] = NeedAmount(item);
                            else
                                AntiCapForm.Instance.NeedAmounts.Add(item.ItemID, NeedAmount(item));
                            Settings.Instance.BuyItems.Add(item);
                        }
                        Lua.DoString("ClearCursor()");
                        Settings.SaveSettings();
                    }
                }
            }
            else
            {
                
                if (Styx.StyxWoW.Me.IsMoving)
                    return;
                var itemToBuy = ItemToBuy;
                if (itemToBuy != null)
                {
                    var npc = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == itemToBuy.VendorID);
                    //if (MerchantFrame.Instance == null || !MerchantFrame.Instance.IsVisible || MerchantFrame.Instance.Merchant.Entry != npc.Entry)
                    if (MerchantFrame.Instance == null || !MerchantFrame.Instance.IsVisible || (MerchantFrame.Instance.IsVisible && StyxWoW.Me.CurrentTarget.Entry != npc.Entry))
                    {
                        if (!_moveDelay.IsRunning)
                            _moveDelay.Restart();
                        if (_moveDelay.IsRunning && _moveDelay.ElapsedMilliseconds > 3000)
                        {
                            if (npc.Distance > npc.InteractRange)
                            {
                                Logging.Write("Moving to {0} to buy {1}", npc.Name, itemToBuy.ItemName);
                                Navigator.MoveTo(WoWMovement.CalculatePointFrom(npc.Location, npc.InteractRange - 1));
                            }
                            else
                            {
                                npc.Target();
                                npc.Interact();
                            }
                        }
                    }
                    else
                    {
                        if (!_buyDelay.IsRunning)
                            _buyDelay.Restart();
                        if (_buyDelay.IsRunning && _buyDelay.ElapsedMilliseconds > 3000)
                        {
                            _buyDelay.Reset();
                            BuyItem(itemToBuy.ItemID);
                        }
                    }
                }
            }
        }

        public static int NeedAmount(Item item)
        {
            return item.Amount - Lua.GetReturnVal<int>(string.Format("return GetItemCount({0}, true)", item.ItemID), 0);
        }

        public void BuyItem(uint itemId)
        {
            var lua = string.Format(
                "for i=1, GetMerchantNumItems() do " +
                "  local l=GetMerchantItemLink(i) " +
                "  if l then " +
                "    if l:find({0}) then " +
                "      BuyMerchantItem(i, 1) " +
                "    end " +
                "  end " +
                "end",
                itemId);
            Lua.DoString(lua);
        }

        public Item ItemToBuy
        {
            get
            {
                var itemsWithVendorNearby =
                    Settings.Instance.BuyItems.Where(
                        i => ObjectManager.GetObjectsOfType<WoWUnit>().Any(u => i.VendorID == u.Entry)).OrderBy(
                            i => i.VendorLocation.Distance(Styx.StyxWoW.Me.Location));
                if (!itemsWithVendorNearby.Any())
                    return null;
                Item itemToBuy;
                using (Styx.StyxWoW.Memory.AcquireFrame())
                {
                    itemToBuy = itemsWithVendorNearby.FirstOrDefault(i => CanAffordItem(i) && NeedAmount(i) > 0);
                }
                return itemToBuy;
            }
        }
        public bool CanAffordItem(Item item)
        {
            return item.Prices.All(price => GetCurrencyCount(price.Currency) >= price.Amount);
        }

        public int GetCurrencyCount(uint id)
        {
            return Lua.GetReturnVal<int>(string.Format("return GetCurrencyInfo({0})", id), 1);
        }

        public override string Name
        {
            get { return "Anti Cap"; }
        }

        public override string Author
        {
            get { return "Inrego"; }
        }

        public override Version Version
        {
            get { return new Version(1,0);}
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnEnable()
        {
            Settings.LoadSettings();
        }

        private static AntiCapForm _form;
        public override void OnButtonPress()
        {
            if (_form != null && _form.Visible)
            {
            }
            else
            {
                _form = new AntiCapForm();
                _form.Show();
            }
        }
    }
}
