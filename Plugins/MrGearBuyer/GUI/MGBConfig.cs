using System;
using System.ComponentModel;
using System.Windows.Forms;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.WoWInternals;

namespace MrGearBuyer.GUI
{
    public partial class MGBConfig : Form
    {
        private readonly BindingList<MrGearBuyer.BuyItemInfo> _venderList = new BindingList<MrGearBuyer.BuyItemInfo>();

        public MGBConfig()
        {
            InitializeComponent();
            LogoutAtCap.Checked = MrGearBuyer.LogoutAtCap;
            RemoveJPHPWhenCapped.Checked = MrGearBuyer.RemoveHPJPWhenCapped;
            BuyOppositePointToBuildUp.Checked = MrGearBuyer.BuyOppositePointToBuildUp;
            EquipAfterBuy.Checked = MrGearBuyer.EquipAfterBuy;
            MyMount.Checked = MrGearBuyer.MyMount;
            LogOutJPCap.Checked = MrGearBuyer.LogOutJPCap;
            LogOutHPCap.Checked = MrGearBuyer.LogOutHPCap;
            RepairSwap.Checked = MrGearBuyer.RepairSwap;
        }

        private BindingList<MrGearBuyer.BuyItemInfo> BuyItemList
        {
            get { return MrGearBuyer.BuyItemList; }
        }

        private void Hc2ConfigLoad(object sender, EventArgs e)
        {
            dgvBuyList.DataSource = BuyItemList;
            dgvVenderList.DataSource = _venderList;

// ReSharper disable CompareOfFloatsByEqualityOperator
            if (MrGearBuyer.HomeZ == 0 && MrGearBuyer.HomeZ == 0 && MrGearBuyer.HomeX == 0 || !MrGearBuyer.HPSet)
// ReSharper restore CompareOfFloatsByEqualityOperator

            {
                HomePointTEXT.Text = "My Current HomePoint is 0,0,0. Please Set a HomePoint to Use HomePoint Features!";
                MrGearBuyer.Slog("My HomePoint is not set, Please set a HomePoint Location to use HomePoint Features!");
            }
            else
            {
                HomePointTEXT.Text = "My Current HomePoint is X= " + MrGearBuyer.HomeX + " Y= " + MrGearBuyer.HomeY +
                                     " Z= " + MrGearBuyer.HomeZ + " ";
                MrGearBuyer.Dlog("HomePoint is Enabled ");
            }
            if (MrGearBuyer.RepairSet == false)
            {
                MrGearBuyer.Slog("Prefered Repair Vendor Not Set");
                RepairInfo.Text = "Prefered Repair Vendor Not Set";
            }
            else
            {
                MrGearBuyer.Slog("Our Prefered Repair Vendor Is {0}", MrGearBuyer.RepairName);
                RepairInfo.Text = "Our Prefered Repair Vendor is " + MrGearBuyer.RepairName;
            }
        }

        private void DgvBuyListCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            RemoveBuyClick(null, null);
        }

        private void RemoveBuyClick(object sender, EventArgs e)
        {
            foreach (object item in dgvBuyList.SelectedRows)
            {
                var row = (DataGridViewRow) item;
                BuyItemList.RemoveAt(row.Index);
            }
        }

        private void BtnMoveUpClick(object sender, EventArgs e)
        {
            int count = dgvBuyList.SelectedRows.Count;

            if (count == 0)
            {
                MessageBox.Show("Please select a row first", "Select a row", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (count >= 2)
            {
                MessageBox.Show("Please select only one row to move it up", "Select only one row", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            int index = dgvBuyList.SelectedRows[0].Index;

            // Dont do anything if its the first item.
            if (index == 0)
                return;

            MrGearBuyer.BuyItemInfo item = BuyItemList[index];

            BuyItemList.RemoveAt(index);

            int newIndex = index - 1;

            BuyItemList.Insert(newIndex, item);

            foreach (object row in dgvBuyList.Rows)
            {
                ((DataGridViewRow) row).Selected = false;
            }

            dgvBuyList.Rows[newIndex].Selected = true;
        }

        private void BtnMoveDownClick(object sender, EventArgs e)
        {
            int count = dgvBuyList.SelectedRows.Count;

            if (count == 0)
            {
                MessageBox.Show("Please select a row first", "Select a row", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (count >= 2)
            {
                MessageBox.Show("Please select only one row to move it down", "Select only one row",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            int index = dgvBuyList.SelectedRows[0].Index;

            // Dont do anything if its the last item.
            if (index == BuyItemList.Count - 1)
                return;

            MrGearBuyer.BuyItemInfo item = BuyItemList[index];

            BuyItemList.RemoveAt(index);

            int newIndex = index + 1;

            BuyItemList.Insert(newIndex, item);

            foreach (object row in dgvBuyList.Rows)
            {
                ((DataGridViewRow) row).Selected = false;
            }

            dgvBuyList.Rows[newIndex].Selected = true;
        }

        private void DgvVenderListCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AddtoBuyClick(null, null);
        }

        private void AddtoBuyClick(object sender, EventArgs e)
        {
            foreach (object item in dgvVenderList.SelectedRows)
            {
                BuyItemList.Add((MrGearBuyer.BuyItemInfo) ((DataGridViewRow) item).DataBoundItem);
            }
        }

        private void FetchClick(object sender, EventArgs e)
        {
            ObjectManager.Update();
            if (!MerchantFrame.Instance.IsVisible)
            {
                MrGearBuyer.Slog("No Vender Frame was Open, Talk to a Vender to see the goods, then try again");
                return;
            }

            _venderList.Clear();
            foreach (MerchantItem vendorItem in MerchantFrame.Instance.GetAllMerchantItems())
            {
                int fixedIndex = vendorItem.Index + 1;

                String name = vendorItem.Name;

                var costTypeLua = Lua.GetReturnVal<string>(
                    "return GetMerchantItemCostItem(" + fixedIndex + ", GetMerchantItemCostInfo(" + fixedIndex + "))", 0);

                if (string.IsNullOrEmpty(costTypeLua))
                {
                    MrGearBuyer.Dlog("Vender item is not bought with points, Skipping. [{0}]", name);
                    continue;
                }

                string costType = "";
                if (costTypeLua.ToUpperInvariant().Contains("HONOR"))
                    costType = "HonorPoints";
                else if (costTypeLua.ToUpperInvariant().Contains("JUSTICE"))
                    costType = "JusticePoints";
                else if (costTypeLua.ToUpperInvariant().Contains("CONQUEST"))
                    costType = "ConquestPoints";

                var honorCost =
                    Lua.GetReturnVal<int>(
                        "return GetMerchantItemCostItem(" + fixedIndex + ", GetMerchantItemCostInfo(" + fixedIndex +
                        "))", 1);

                // Sometimes name returns empty from lua. Do another one to make sure its valid
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(costType) || honorCost == 0)
                {
                    MrGearBuyer.Dlog("Vender item not usable, Skipping. [{0}]", name);
                    continue;
                }
                // If the core fails at getting the name, try to do it manually
                if (name == "(null)")
                {
                    // Buying HP with JP
                    if (vendorItem.ItemId == 392)
                        name = "Honor Points";
                        // Buying JP with HP
                    else if (vendorItem.ItemId == 395)
                        name = "Justice Points";
                    else
                    {
                        // Core fails. Manual look-up
                        name = Lua.GetReturnVal<String>(
                            "return GetMerchantItemInfo(" + fixedIndex + ")", 0);

                        // If it still fails, skip it
                        if (name == "(null)")
                        {
                            MrGearBuyer.Slog(
                                "Can not find the name for itemID [{0}]. Please inform the plugin writers and attach your log file",
                                vendorItem.ItemId);
                            continue;
                        }
                    }
                }
                var item = new MrGearBuyer.BuyItemInfo
                {
                    ItemCost = honorCost,
                    ItemName = name,
                    ItemSupplierId = StyxWoW.Me.CurrentTarget.Entry,
                    ItemId = vendorItem.ItemId,
                    ItemCostType = costType.Replace(" ", "")
                };
                _venderList.Add(item);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void LogoutAtCapCheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.LogoutAtCap = LogoutAtCap.Checked;
        }

        private void RemoveJphpWhenCappedCheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.RemoveHPJPWhenCapped = RemoveJPHPWhenCapped.Checked;
        }

        private void BuyOppositePointToBuildUp_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.BuyOppositePointToBuildUp = BuyOppositePointToBuildUp.Checked;
        }

        private void RemoveFromBL_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me.CurrentTarget != null)
            {
                //We need to make sure vender is usable, so removing blacklist. 
                if (Blacklist.Contains(StyxWoW.Me.CurrentTarget.Guid, BlacklistFlags.All))
                {
                    MrGearBuyer.Slog("{0} was Found to be Blacklisted, Clearing Blacklist.",
                        StyxWoW.Me.CurrentTarget.Name);
                    Blacklist.Flush();
                }
            }
        }

        private void HomePointSet_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();
            WoWPoint point = StyxWoW.Me.Location;
            MrGearBuyer.HomeX = point.X;
            MrGearBuyer.HomeY = point.Y;
            MrGearBuyer.HomeZ = point.Z;
            MrGearBuyer.MapID = (int) StyxWoW.Me.MapId;
            MrGearBuyer.HPSet = true;
            HomePointTEXT.Text = "My Current HomePoint is X=" + MrGearBuyer.HomeX + " Y=" + MrGearBuyer.HomeY + " Z=" +
                                 MrGearBuyer.HomeZ + " ";
            MrGearBuyer.Slog("My HomePoint is now Set");
            MrGearBuyer.Save();
        }

        private void MGBConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            MrGearBuyer.Save();
            MrGearBuyer.Dlog("MrGearBuyer Config is Closing, Saving Settings");
        }

        private void EquipAfterBuy_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.EquipAfterBuy = EquipAfterBuy.Checked;
        }

        private void MyMount_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.MyMount = MyMount.Checked;
        }

        private void LogOutHPCap_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.LogOutHPCap = LogOutHPCap.Checked;
        }

        private void LogOutJPCap_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.LogOutJPCap = LogOutJPCap.Checked;
        }

        private void RepairSwap_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.RepairSwap = RepairSwap.Checked;
        }

        private void RepairSet_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();
            if (StyxWoW.Me.CurrentTarget.IsRepairMerchant)
            {
                MrGearBuyer.RepairName = StyxWoW.Me.CurrentTarget.Name;
                MrGearBuyer.RepairID = (Int32) StyxWoW.Me.CurrentTarget.Entry;
                MrGearBuyer.RepairX = StyxWoW.Me.CurrentTarget.Location.X;
                MrGearBuyer.RepairY = StyxWoW.Me.CurrentTarget.Location.Y;
                MrGearBuyer.RepairZ = StyxWoW.Me.CurrentTarget.Location.Z;
                MrGearBuyer.RepairMapID = (int) StyxWoW.Me.MapId;
                MrGearBuyer.RepairSet = true;
                RepairInfo.Text = "My Current Repair Vendor is " + MrGearBuyer.RepairName;
                MrGearBuyer.Slog("My Prefered Repair Vendor Set to {0}", MrGearBuyer.RepairName);
                MrGearBuyer.Save();
            }
            else
            {
                MrGearBuyer.Slog("You Must Target a Valid Repair Vendor to Set Vendor As Prefered Repair Vendor");
            }
        }
    }
}