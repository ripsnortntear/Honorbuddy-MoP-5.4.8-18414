using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.CommonBot;

namespace AntiCap
{
    public partial class AntiCapForm : Form
    {
        private const string AddItemsString = "Add Items";
        private const string StopAddItemsString = "Stop Adding";
        public Dictionary<uint, int> NeedAmounts; 
        public bool IsAdding
        {
            get { return addItemsButton.Text.Equals(StopAddItemsString); }
        }
        public static AntiCapForm Instance { get; private set; }
        private string _oldBotBase;
        public AntiCapForm()
        {
            NeedAmounts = new Dictionary<uint, int>();
            using (Styx.StyxWoW.Memory.AcquireFrame())
            {
                foreach (var buyItem in Settings.Instance.BuyItems)
                {
                    NeedAmounts.Add(buyItem.ItemID, Plugin.NeedAmount(buyItem));
                }
            }
            InitializeComponent();
            Instance = this;
            buyItemsList.AutoGenerateColumns = false;
            buyItemsList.DataSource = Settings.Instance.BuyItems;
            var nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "ItemName";
            nameColumn.Name = "ItemName";
            nameColumn.HeaderText = "Item Name";
            nameColumn.ReadOnly = true;
            nameColumn.Width = 205;

            var amountColumn = new DataGridViewTextBoxColumn();
            amountColumn.DataPropertyName = "Amount";
            amountColumn.Name = "Amount";
            amountColumn.HeaderText = "Amount";

            var priceColumn = new DataGridViewTextBoxColumn();
            priceColumn.DataPropertyName = "PricesString";
            priceColumn.Name = "PricesString";
            priceColumn.HeaderText = "Price";
            priceColumn.ReadOnly = true;

            buyItemsList.Columns.Add(nameColumn);
            buyItemsList.Columns.Add(amountColumn);
            buyItemsList.Columns.Add(priceColumn);
        }

        private void addItemsButton_Click(object sender, EventArgs e)
        {
            if (addItemsButton.Text.Equals(AddItemsString))
            {
                if (MessageBox.Show(this, "To add items, please follow these steps:\n - Interact with the vendor (open the vendor window).\n - Left click each of the items you want the plugin to buy.\nWhen you are done adding items, please stop Honorbuddy or click 'Stop Adding'.\n\nWhen you left click an item, it is 'picked up' to your cursor. The plugin will detect this, and add that item to the list, and clear the cursor so you can click the next item.\n\nWARNING: Do not spam-click an item. It will freeze HB. If you want to buy many of an item, simply change the amount in the list after you are done adding.\n\nIf Combat Bot is not your currently selected botbase, HB might freeze for some seconds when you click yes. Just wait it out!\n\nAre you ready to add items now?", "Instructions", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (!TreeRoot.IsRunning)
                    {
                        _oldBotBase = BotManager.Current.Name;
                        foreach (var botBase in BotManager.Instance.Bots)
                        {
                            if (botBase.Key.ToLower().Contains("combat bot"))
                            {
                                BotManager.Instance.SetCurrent(botBase.Value);
                            }
                        }
                        TreeRoot.Start();
                    }
                    BotEvents.OnBotStopped += BotEvents_OnBotStopped;
                    addItemsButton.Text = StopAddItemsString;
                }
            }
            else
            {
                TreeRoot.Stop();
                stopAdding();
            }
        }

        private void stopAdding()
        {
            BotEvents.OnBotStopped -= BotEvents_OnBotStopped;
            if (!string.IsNullOrEmpty(_oldBotBase))
            {
                foreach (var botBase in BotManager.Instance.Bots)
                {
                    if (botBase.Key.Equals(_oldBotBase))
                    {
                        BotManager.Instance.SetCurrent(botBase.Value);
                    }
                }
            }
            addItemsButton.Text = AddItemsString;
            _oldBotBase = null;
        }

        private void BotEvents_OnBotStopped(EventArgs args)
        {
            stopAdding();
        }

        private static Color DarkGray
        {
            get { return Color.FromArgb(24, 24, 24); }
        }
        private static Color LightGray
        {
            get { return Color.FromArgb(46, 46, 46); }
        }
        private static Color TextColor
        {
            get { return Color.FromArgb(221, 221, 221); }
        }
        private static Color CommonColor
        {
            get { return TextColor; }
        }
        private static Color PoorColor
        {
            get { return Color.FromArgb(157, 157, 157); }
        }
        private static Color UncommonColor
        {
            get { return Color.FromArgb(30, 255, 0); }
        }
        private static Color RareColor
        {
            get { return Color.FromArgb(0, 112, 255); }
        }
        private static Color EpicColor
        {
            get { return Color.FromArgb(163, 53, 238); }
        }
        private static Color LegendaryColor
        {
            get { return Color.FromArgb(255, 128, 0); }
        }
        private static Color HeirloomColor
        {
            get { return Color.FromArgb(230, 204, 128); }
        }
        private void buyItemsList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.BackColor = DarkGray;
            e.CellStyle.SelectionBackColor = LightGray;
            e.CellStyle.ForeColor = TextColor;
            if (buyItemsList.Columns["ItemName"].Index == e.ColumnIndex)
            {
                switch (Settings.Instance.BuyItems[e.RowIndex].Quality)
                {
                    case 0:
                        e.CellStyle.ForeColor = PoorColor;
                        break;
                    case 1:
                        e.CellStyle.ForeColor = CommonColor;
                        break;
                    case 2:
                        e.CellStyle.ForeColor = UncommonColor;
                        break;
                    case 3:
                        e.CellStyle.ForeColor = RareColor;
                        break;
                    case 4:
                        e.CellStyle.ForeColor = EpicColor;
                        break;
                    case 5:
                        e.CellStyle.ForeColor = LegendaryColor;
                        break;
                    case 6:
                        e.CellStyle.ForeColor = HeirloomColor;
                        break;
                    case 7:
                        e.CellStyle.ForeColor = HeirloomColor;
                        break;
                }
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
            var id = Settings.Instance.BuyItems[e.RowIndex].ItemID;
            if (NeedAmounts[id] == 0)
            {
                var oldFont = e.CellStyle.Font;
                var newFont = new Font(oldFont, FontStyle.Strikeout);
                e.CellStyle.Font = newFont;
            }
        }

        private void buyItemsList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Settings.SaveSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Item List";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = "xml";
            openFileDialog.Filter = "Items List (*.xml)|*.xml";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Settings.Instance = ObjectXMLSerializer<Settings>.Load(openFileDialog.FileName);
                Settings.SaveSettings();
            }
        }
    }
}
