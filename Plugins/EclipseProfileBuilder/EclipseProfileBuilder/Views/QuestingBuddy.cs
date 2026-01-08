using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Xml.Linq;
using System.IO;
using Eclipse.EclipsePlugins.Models;
using Eclipse.EclipsePlugins.Views;
using System.Reflection;
using Eclipse.EclipsePlugins.Controllers;
using Eclipse.WoWDatabase;
using ArachnidCreations;

namespace Eclipse
{
    public partial class QuestingBuddy : Form
    {
        public static string ProfilePath = string.Empty;
        public static string MinLevel = string.Empty;
        public static string MaxLevel = string.Empty;
        private static LocalPlayer Me = StyxWoW.Me;
        EclipseProfile dt= new EclipseProfile();
        public QuestingBuddy()
        {
            InitializeComponent();
        }
        public void Pulse()
        {
            if (StyxWoW.Me != null)
            {
                dt.PosX = StyxWoW.Me.X.ToString();
                dt.PosY = StyxWoW.Me.Y.ToString();
                dt.PosZ = StyxWoW.Me.Z.ToString();
            }
            else
            {
                dt.PosX = 0.ToString();
                dt.PosY = 0.ToString();
                dt.PosZ = 0.ToString();

            }
        }
        private void EclipsePlugin_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://arachnidcreations.com/HonorBuddy/Image.aspx?image=Supernova-Eclipse2.jpg";
            if (LanguageCore.lang != Language.English) LanguageCore.PopulateControls(this);
            cbQuestOrderType.DataSource = Enum.GetValues(typeof(QuestOrder.QOType));
            cbQObjectiveType.DataSource = Enum.GetValues(typeof(QuestObjective.QuestType));
            if (StyxWoW.Me != null)
            {
                try
                {
                    tbQX.DataBindings.Clear();
                    tbQY.DataBindings.Clear();
                    tbQZ.DataBindings.Clear();
                    tbVendorX.DataBindings.Clear();
                    tbVendorY.DataBindings.Clear();
                    tbVendorZ.DataBindings.Clear();
                    tbBSX.DataBindings.Clear();
                    tbBSY.DataBindings.Clear();
                    tbBSZ.DataBindings.Clear();

                    tbQX.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbQY.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbQZ.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbVendorX.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbVendorY.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbVendorZ.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbBSX.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbBSY.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    tbBSZ.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

                    tbQX.DataBindings.Add("Text", this.dt, "PosX");// = StyxWoW.Me.X.ToString();
                    tbQY.DataBindings.Add("Text", this.dt, "PosY");// = StyxWoW.Me.X.ToString();
                    tbQZ.DataBindings.Add("Text", this.dt, "PosZ");// = StyxWoW.Me.X.ToString();

                    tbVendorX.DataBindings.Add("Text", this.dt, "PosX");// = StyxWoW.Me.X.ToString();
                    tbVendorY.DataBindings.Add("Text", this.dt, "PosY");// = StyxWoW.Me.X.ToString();
                    tbVendorZ.DataBindings.Add("Text", this.dt, "PosZ");// = StyxWoW.Me.X.ToString();

                    tbBSX.DataBindings.Add("Text", this.dt, "PosX");// = StyxWoW.Me.X.ToString();
                    tbBSY.DataBindings.Add("Text", this.dt, "PosY");// = StyxWoW.Me.X.ToString();
                    tbBSZ.DataBindings.Add("Text", this.dt, "PosZ");// = StyxWoW.Me.X.ToString();
                    EC.log("Loaded Profile Form.");
                }
                catch (Exception err){
                    MessageBox.Show(err.Message +"|"+ err.StackTrace);
                    EC.log(string.Format("Profile Load Error! {0}", err.ToString()));
                }
                
            }
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            //ToDo: put back before export
            //try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                if (tbFileName.Text.Length == 0)
                {
                    ofd.ShowDialog();
                    tbFileName.Text = ofd.FileName;
                }
                dt = new EclipseProfile(tbFileName.Text);
                loadData();
                EC.log("Loaded Profile Data.");
                tbFileName.Text = "";
            }
            //catch (Exception err)
            //{
            //   MessageBox.Show(err.Message + "|" + err.StackTrace + "|" + err.Source);
            //   log(string.Format("Profile Load Error! {0}", err.ToString()));
            //}
        }
        private void loadData()
        {

            lbAvoidMobs.DataSource = null;
            lbBlackList.DataSource = null;
            lbVendors.DataSource = null;
            //lbQuests.DataSource = null;
            lbQuestOrders.DataSource = null;
            cbLogic.DataSource = null;
            lblProfileName.DataBindings.Clear();
            lblProfileName.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            lblProfileName.DataBindings.Add("Text", dt, "Name");

            lbVendors.DataSource = EclipseProfile.Vendors;
            lbVendors.DisplayMember = "Name";
            lbAvoidMobs.DataSource = EclipseProfile.AvoidMobs;
            lbAvoidMobs.DisplayMember = "Name";
            lbBlackList.DataSource = EclipseProfile.BlackSpots;
            lbBlackList.DisplayMember = "Name";
            //lbQuests.DataSource = EclipseProfile.QuestOverrides;
            //lbQuests.DisplayMember = "Name";
            lbQuestOrders.DataSource = EclipseProfile.QuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
            cbLogic.DataSource = EclipseProfile.LogicBlocks;
            cbLogic.DisplayMember = "DisplayName";
            cbCustomBehaviors.DataSource = EclipseProfile.CustomBehaviors;
            cbCustomBehaviors.DisplayMember = "File";
            lbMailboxes.DataSource = EclipseProfile.MailBoxes;
            if (StyxWoW.Me != null)
            {
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
                tbQX.Text = StyxWoW.Me.X.ToString();
                tbQY.Text = StyxWoW.Me.Y.ToString();
                tbQZ.Text = StyxWoW.Me.Z.ToString();
            }
            if (ObjectManager.ObjectList != null)
            {
                lbNearbyMobs.DataSource =  ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= 40 * 40).ToList();
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            //try
            {
                if (Core.DataPath == string.Empty)
                {
                    Core.FindDB();
                }
                if (tbDataFile.Text == string.Empty) Core.DataPath = @"C:\Users\Twist\Documents\Honorbuddy\Bots\SkinbotV2\SkinbotV2\Data\EclipseWoWDB.edb";
                if (dt.FileName.Length == 0) dt.FileName = dt.Name;
                if (!dt.FileName.Contains("Eclipse")) dt.FileName = "[Eclipse]" + dt.FileName.Replace(".xml", "");
                btnSaveProfile.Enabled = false;
                btnSaveProfile.Text = "Saving your profile...";
                Task t = Task.Factory.StartNew(() =>
                {
                    dt.Save(dt.FileName);
                    MessageBox.Show("Your profile was saved as " + dt.FileName.Replace(".xml", ""));
                    EC.log("Your profile was saved as " + dt.FileName);

                });
                btnSaveProfile.Enabled = true;
                btnSaveProfile.Text = "Save Profile";

            }
            //catch (Exception err)
            //{
            //    EC.log(string.Format("Profile Save Error! {0}", err.ToString()));
            //    MessageBox.Show("Something went horribly wrong: " + err.ToString());
            //}
            
        }

        private void btnAddAvoid_Click(object sender, EventArgs e)
        {
            var me = StyxWoW.Me;
            var mob = new EclipseMob { Name = me.CurrentTarget.Name, Entry= me.CurrentTarget.Entry.ToString() };
            EclipseProfile.AvoidMobs.Add(mob);
            loadData();
            EC.log(string.Format("Added Avoid: {0}", mob.Name));

        }
        private void btnRemoveMob_Click(object sender, EventArgs e)
        {
            var avoid = (EclipseMob)lbAvoidMobs.SelectedItem;
            EclipseProfile.AvoidMobs.Remove(avoid);
            EC.log(string.Format("Removed Avoid: {0}", avoid.Name));
            loadData();
            
        }

        private void lbActiveQuests_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (PlayerQuest)lbActiveQuests.SelectedItem;
        }
        private void lbNearbyMobs_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (WoWUnit)lbNearbyMobs.SelectedItem;
        }
        private void lbQuests_SelectedValueChanged(object sender, EventArgs e)
        {
            //propertyGrid1.SelectedObject = (QuestOverride)lbQuests.SelectedItem;
        }
        private void cbQuestOrderType_SelectedValueChanged(object sender, EventArgs e)
        {
            QuestOrder.QOType type;
            Enum.TryParse<QuestOrder.QOType>(cbQuestOrderType.SelectedValue.ToString(), out type);
            QuestObjective.QuestType qtype = QuestObjective.QuestType.CollectItem;
            if (cbQObjectiveType.SelectedValue != null) Enum.TryParse<QuestObjective.QuestType>(cbQObjectiveType.SelectedValue.ToString(), out qtype);
            if (type == QuestOrder.QOType.Objective)
            {
                cbQObjectiveType.Enabled = true;
                cbQObjectiveType.SelectedValueChanged += cbQuestOrderType_SelectedValueChanged;
                tbQGiverName.Enabled = false;
                tbQGiverId.Enabled = false;
                tbQTurninId.Enabled = false;
                tbQTurninName.Enabled = false;
                if (qtype == QuestObjective.QuestType.CollectItem)
                {
                    tbQKillCount.Enabled = false;
                    tbQMobId.Enabled = false;
                    tbQItemId.Enabled = true;
                    cbQItemBags.Enabled = true;
                    tbQCollectCount.Enabled = true;
                }
                if (qtype == QuestObjective.QuestType.KillMob)
                {
                    tbQKillCount.Enabled = true;
                    tbQMobId.Enabled = true;
                    tbQMobName.Enabled = true;
                    tbQItemId.Enabled = false;
                    tbQCollectCount.Enabled = false;
                    tbQItemId.Enabled = false;
                    tbQCollectCount.Enabled = false;
                }
            }
            else if (type == QuestOrder.QOType.UseItem)
            {
                tbQGiverName.Enabled = false;
                tbQGiverId.Enabled = false;
                tbQTurninId.Enabled = false;
                tbQTurninName.Enabled = false;
                tbQMobId.Enabled = true;
                tbQMobName.Enabled = true;
                tbQItemId.Enabled = true;
                cbQItemBags.Enabled = true;
                tbQCollectCount.Enabled = false;
                lblQKillCount.Text = "NumOfTimes";
                tbQKillCount.Enabled = true;
            }
            else if (type == QuestOrder.QOType.PickUp)
            {
                tbQGiverName.Enabled = true;
                tbQGiverId.Enabled = true;
                tbQTurninId.Enabled = false;
                tbQTurninName.Enabled = false;
                tbQItemId.Enabled = false;
                tbQCollectCount.Enabled = false;
                tbQKillCount.Enabled = false;
                tbQMobId.Enabled = false;
                cbQObjectiveType.Enabled = false;
                cbQItemBags.Enabled = false;
                tbQMobName.Enabled = false;
            }
            else if (type == QuestOrder.QOType.TurnIn)
            {
                tbQGiverName.Enabled = false;
                tbQGiverId.Enabled = false;
                tbQTurninId.Enabled = true;
                tbQTurninName.Enabled = true;
                tbQItemId.Enabled = false;
                tbQCollectCount.Enabled = false;
                tbQKillCount.Enabled = false;
                tbQMobId.Enabled = false;
                cbQItemBags.Enabled = false;
                tbQMobName.Enabled = false;
            }
        }
        private void lbQuestOrders_SelectedValueChanged(object sender, EventArgs e)
        {
            var qo = (QuestOrder)lbQuestOrders.SelectedItem;
            //if (qo.type == QuestOrder.QOType.CustomBehavior) qo = (CustomBehavior)qo;
            propertyGrid1.SelectedObject = qo;
            if (qo != null)
            {
                tbQMobId.Text = qo.MobId;
                tbQCollectCount.Text = qo.CollectCount;
                tbQGiverId.Text = qo.GiverId;
                tbQGiverName.Text = qo.GiverName;
                tbQItemId.Text = qo.ItemId;
                tbQKillCount.Text = qo.NumOfTimes;
                cbQItemBags.Text = qo.ItemName;
                tbQKillCount.Text = qo.KillCount;
                tbQMobName.Text = qo.MobName;
                if (cbQObjectiveType.DataSource != null) cbQObjectiveType.SelectedItem = qo.objectiveType;
                tbQId.Text = qo.QuestId.ToString();
                tbQName.Text = qo.QuestName;
                tbQTurninId.Text = qo.TurnInId;
                tbQTurninName.Text = qo.TurnInName;
                if (cbQuestOrderType.DataSource != null) cbQuestOrderType.SelectedItem = qo.type;
                tbQX.Text = qo.X;
                tbQY.Text = qo.Y;
                tbQZ.Text = qo.Z;
            }

        }
        private void getFromTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null)
                {
                    ObjectManager.Update();

                }
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            sourceControl.Text = StyxWoW.Me.CurrentTarget.Name;
                        }
                    }
                }
            }
        }
        private void getFromSelectedQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbActiveQuests.SelectedItem != null)
            {
                var Quest = (PlayerQuest)lbActiveQuests.SelectedItem;
                tbQId.Text = Quest.Id.ToString();
                tbQName.Text = Quest.Name;
            }
        }
        private void btnNewProfile_Click(object sender, EventArgs e)
        {
            EC.log(string.Format("Showing new profile form."));
            NewQuestingProfile np = new NewQuestingProfile(new EclipseProfile());
            np.Show();
            dt = np._dt;
            np.FormClosed += np_FormClosed;
        }
        void np_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadData();
        }

        private void btnAddQuestOrder_Click(object sender, EventArgs e)
        {
            var qoType = (QuestOrder.QOType)Enum.Parse(typeof(QuestOrder.QOType), cbQuestOrderType.SelectedValue.ToString());
            var objType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), cbQObjectiveType.SelectedValue.ToString());
            if (qoType == QuestOrder.QOType.Objective && objType == QuestObjective.QuestType.CollectItem && tbQCollectCount.Text.Length == 0)
            {
                MessageBox.Show("You must have a CollectCount for a Collect Item Objective.");
                return;
            }
            if (qoType == QuestOrder.QOType.CustomBehavior)
            {
                var qo = (CustomBehavior)cbCustomBehaviors.SelectedItem;
                qo.type = QuestOrder.QOType.CustomBehavior;
                EclipseProfile.QuestOrders.Add(qo);
                EC.log(string.Format("Quest Order Added Type: {0}", qo.type));
                refreshQuestOrders();
            }
            else if (qoType == QuestOrder.QOType.LogicBlock)
            {
                var qoStart = (QuestOrderLogic)cbLogic.SelectedItem;
                qoStart.StartTag = true;
                EclipseProfile.QuestOrders.Add(qoStart);
                //Cloning objects is bad, but what can you do =/
                var qoEnd = new QuestOrderLogic { StartTag = false, LogicType=qoStart.LogicType, type = qoStart.type, Condition=qoStart.Condition  };
                qoEnd.StartTag = false;
                EclipseProfile.QuestOrders.Add(qoEnd);
                EC.log(string.Format("Quest Order Added Type: {0}", qoStart.type));
                refreshQuestOrders();
            }
            else
            {
                QuestOrder qo = new QuestOrder();
                qo.MobId = tbQMobId.Text;
                qo.CollectCount = tbQCollectCount.Text;
                qo.GiverId = tbQGiverId.Text;
                qo.GiverName = tbQGiverName.Text;
                qo.ItemId = tbQItemId.Text;
                qo.NumOfTimes = tbQKillCount.Text;
                qo.ItemName = cbQItemBags.Text;
                qo.KillCount = tbQKillCount.Text;
                qo.MobName = tbQMobName.Text;
                qo.objectiveType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), cbQObjectiveType.SelectedValue.ToString());
                qo.QuestId =  uint.Parse(tbQId.Text);
                qo.QuestName = tbQName.Text;
                qo.TurnInId = tbQTurninId.Text;
                qo.TurnInName = tbQTurninName.Text;
                qo.type = (QuestOrder.QOType)Enum.Parse(typeof(QuestOrder.QOType), cbQuestOrderType.SelectedValue.ToString());
                qo.X = tbQX.Text;
                qo.Y = tbQY.Text;
                qo.Z = tbQZ.Text;
                EclipseProfile.QuestOrders.Add(qo);
                EC.log(string.Format("Quest Order Added Type: {0}", qo.type));
                loadData();
            }
            ;
        }
        private void refreshQuestOrders()
        {
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = EclipseProfile.QuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
                EC.log(string.Format("Quests Refreshed."));
            }
        }
        private void btnAddVendor_Click(object sender, EventArgs e)
        {
            EclipseVendor vendor = new EclipseVendor { Name = tbVendorName.Text, Entry = tbVendorId.Text, Type = cbVendorType.Text, X = tbVendorX.Text.ToString(), Y = tbVendorY.Text.ToString(), Z = tbVendorZ.Text.ToString() };
            EclipseProfile.Vendors.Add(vendor);
            EC.log(string.Format("Vendor Added: {0}", vendor.Name));
            loadData();
        }
        private void btnRemoveVendor_Click(object sender, EventArgs e)
        {
            var vendor = (EclipseVendor)lbVendors.SelectedItem;
            EclipseProfile.Vendors.Remove(vendor);
            EC.log(string.Format("Quest Order Added Type: {0}", vendor.Name));
            loadData();
        }
        private void lbVendors_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (EclipseVendor)lbVendors.SelectedItem;
        }
        private void getMobFromTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null)
                {
                    ObjectManager.Update();

                }
                if (StyxWoW.Me.GotTarget)
                {
                    Control control = (Control)sender;
                    control.Text = StyxWoW.Me.CurrentTarget.Name;
                }
            }

        }
        private void getIdFromTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            sourceControl.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                        }
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                tbQX.Text = StyxWoW.Me.X.ToString();
                tbQY.Text = StyxWoW.Me.Y.ToString();
                tbQZ.Text = StyxWoW.Me.Z.ToString();
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            //sourceControl.Text = 
                        }
                    }
                }
            }
        }
        private void getItemsFromBagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    // Try to cast the sender to a ToolStripItem
                    ToolStripItem menuItem = sender as ToolStripItem;
                    if (menuItem != null)
                    {
                        // Retrieve the ContextMenuStrip that owns this ToolStripItem
                        ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                        if (owner != null)
                        {
                            // Get the control that is displaying this context menu
                            Control sourceControl = owner.SourceControl;
                            cbQItemBags.DataSource = StyxWoW.Me.BagItems;
                            cbQItemBags.DisplayMember = "Name";
                            cbQItemBags.ValueMember = "Entry";
                            cbQItemBags.ValueMemberChanged += cbQItemBags_ValueMemberChanged;
                            tbQItemId.Text = cbQItemBags.SelectedValue.ToString();
                            //sourceControl.Text = 
                        }
                    }
                }
            }
        }
        void cbQItemBags_ValueMemberChanged(object sender, EventArgs e)
        {
            tbQItemId.Text = cbQItemBags.SelectedValue.ToString();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                cbQItemBags.DataSource = null;
                cbQItemBags.DataSource = StyxWoW.Me.BagItems.OrderBy(b=>b.Name).ToList();
                cbQItemBags.DisplayMember = "Name";
                cbQItemBags.ValueMember = "Entry";
                EC.log(string.Format("Bag Items Refreshed."));
            }
        }
        private void btnAddQOAbove_Click(object sender, EventArgs e)
        {
            QuestOrder qo = createQuestOrder();
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
            EclipseProfile.QuestOrders.Insert(index, qo);
            EC.log(string.Format("Added Quest Order Type:{0} ABOVE {1}", qo.type, selectedQO.type));
            refreshQuestOrders();
        }
        private QuestOrder createQuestOrder()
        {
            QuestOrder qo = new QuestOrder();
            qo.MobId = tbQMobId.Text;
            qo.CollectCount = tbQCollectCount.Text;
            qo.GiverId = tbQGiverId.Text;
            qo.GiverName = tbQGiverName.Text;
            qo.ItemId = tbQItemId.Text;
            qo.ItemName = cbQItemBags.Text;
            qo.KillCount = tbQKillCount.Text;
            qo.MobName = tbQMobName.Text;
            qo.objectiveType = (QuestObjective.QuestType)Enum.Parse(typeof(QuestObjective.QuestType), cbQObjectiveType.SelectedValue.ToString());
            qo.QuestId =  uint.Parse(tbQId.Text);
            qo.QuestName = tbQName.Text;
            qo.TurnInId = tbQTurninId.Text;
            qo.TurnInName = tbQTurninName.Text;
            qo.type = (QuestOrder.QOType)Enum.Parse(typeof(QuestOrder.QOType), cbQuestOrderType.SelectedValue.ToString());
            qo.X = tbQX.Text;
            qo.Y = lblQY.Text;
            qo.Z = tbQZ.Text;
            return qo;
        }
        private void btnAddQOBelow_Click(object sender, EventArgs e)
        {
            QuestOrder qo = createQuestOrder();
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
            EclipseProfile.QuestOrders.Insert(index+1, qo);
            EC.log(string.Format("Added Quest Order Type:{0} BELOW {1}", qo.type, selectedQO.type));
            refreshQuestOrders();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            EclipseProfile.QuestOrders.Remove(selectedQO);
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = EclipseProfile.QuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
            EC.log(string.Format("Removed Quest Order Type:{0} ", selectedQO.type));
        }
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
                var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
                EclipseProfile.QuestOrders.Remove(selectedQO);
                EclipseProfile.QuestOrders.Insert(index - 1, selectedQO);
                lbQuestOrders.DataSource = null;
                lbQuestOrders.DataSource = EclipseProfile.QuestOrders;
                lbQuestOrders.DisplayMember = "DisplayName";
                lbQuestOrders.SelectedItem = selectedQO;

            }
            catch (ArgumentOutOfRangeException err)
            {
            }
        }
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            var selectedQO = (QuestOrder)lbQuestOrders.SelectedItem;
            try
            {
                
                var index = EclipseProfile.QuestOrders.IndexOf(selectedQO);
                EclipseProfile.QuestOrders.Remove(selectedQO);
                EclipseProfile.QuestOrders.Insert(index, selectedQO);
                lbQuestOrders.DataSource = null;
                lbQuestOrders.DataSource = EclipseProfile.QuestOrders;
                lbQuestOrders.DisplayMember = "DisplayName";
                lbQuestOrders.SelectedItem = selectedQO;
            }
            catch (ArgumentOutOfRangeException err)
            {
            }
        }
        private void btnRefreshNearby_Click(object sender, EventArgs e)
        {
            if (ObjectManager.ObjectList != null)
            {
                lbNearbyMobs.DataSource = null;
                lbNearbyMobs.DataSource = ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= 40 * 40).ToList();
                EC.log(string.Format("Getting nearby WowUnits."));
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            var qo = (QuestOrder)lbQuestOrders.SelectedItem;
            tbQMobId.Text = qo.MobId;
            tbQCollectCount.Text = qo.CollectCount;
            tbQGiverId.Text = qo.GiverId;
            tbQGiverName.Text = qo.GiverName;
            tbQItemId.Text = qo.ItemId;
            cbQItemBags.Text = qo.ItemName;
            tbQKillCount.Text = qo.KillCount;
            if (qo.MobName != null) tbQMobName.Text = qo.MobName.ToString();
            cbQObjectiveType.SelectedItem = qo.objectiveType;
            tbQId.Text=qo.QuestId.ToString();
            tbQName.Text = qo.QuestName;
            tbQTurninId.Text = qo.TurnInId;
            tbQTurninName.Text = qo.TurnInName;
            cbQuestOrderType.SelectedItem = qo.type;
            tbQX.Text = qo.X;
            lblQY.Text = qo.Y;
            tbQZ.Text = qo.Z;
        }
        private void btnCreateCB_Click(object sender, EventArgs e)
        {
            EC.log(string.Format("Showing Custom Behavior Form."));
            CustomBehaviorForm cb = new CustomBehaviorForm();
            cb.Show();
        }
        private void pbRefreshCBs_Click(object sender, EventArgs e)
        {

            cbCustomBehaviors.DataSource = null;
            cbCustomBehaviors.DataSource = EclipseProfile.CustomBehaviors;
            cbCustomBehaviors.DisplayMember = "file";
            cbLogic.DataSource = null;
            cbLogic.DataSource = EclipseProfile.LogicBlocks;
            cbLogic.DisplayMember = "DisplayName";
            if (StyxWoW.Me != null)
            {
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
            }
            EC.log(string.Format("Refreshing Custom Behaviors."));
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            loadData();
        }
        private void tbGetGiverInfo_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    EC.log("Getting Target Information.");
                    tbQGiverId.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                    tbQGiverName.Text = StyxWoW.Me.CurrentTarget.Name;
                    
                }
            }
        }

        private void btnGetTurninInfo_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me.CurrentTarget == null)
            {
                ObjectManager.Update();

            }
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.GotTarget)
                {
                    EC.log("Getting Target Information.");
                    tbQTurninId.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                    tbQTurninName.Text = StyxWoW.Me.CurrentTarget.Name;
                }
            }
        }

        private void btnGetMobInfo_Click_1(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                EC.log("Getting Target Information.");
                tbQX.Text = StyxWoW.Me.CurrentTarget.X.ToString();
                tbQY.Text = StyxWoW.Me.CurrentTarget.Y.ToString();
                tbQZ.Text = StyxWoW.Me.CurrentTarget.Z.ToString();

            }
        }

        private void cbQItemBags_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbQItemId.Text = cbQItemBags.SelectedValue.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string target = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=GAXEB25MFRKXE";
            try
                {
                 System.Diagnostics.Process.Start(target);
                }
            catch
                ( 
                 System.ComponentModel.Win32Exception noBrowser) 
                {
                 if (noBrowser.ErrorCode==-2147467259)
                  MessageBox.Show(noBrowser.Message);
                }
            catch (System.Exception other)
                {
                  MessageBox.Show(other.Message);
                }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            EC.log("Adding Mailbox.");
            EclipseProfile.MailBoxes.Add(new MailBox { X = dt.PosX, Y = dt.PosY, Z = dt.PosZ });
            loadData();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null) ObjectManager.Update();
                if (StyxWoW.Me.GotTarget)
                {
                    EC.log("Getting Target Information.");
                    tbQMobId.Text = StyxWoW.Me.CurrentTarget.Entry.ToString();
                    tbQMobName.Text = StyxWoW.Me.CurrentTarget.Name;
                }
            }
        }
        private void btnSearchNearby_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                EC.log("Searching Nearby Objects");
                //lbNearbyMailboxes
                var objs = ObjectManager.ObjectList.Where(n => n.Name.Contains(tbSearchNearby.Text) || n.SafeName.Contains(tbSearchNearby.Text)).ToList();
                lbNearbyGameObjects.DataSource = objs;
            }
            else
            {
                NotAttached();
            }
        }
        public void NotAttached()
        {
            MessageBox.Show("You are not attached to the game or your HB is not up to date. Press start on your HB client to update the in-game list.");
            EC.log("You are not attached to the game or your HB is not up to date. Press start on your HB client to update the in-game list.");
        }

        private void lbNearbyMailboxes_DoubleClick(object sender, EventArgs e)
        {
            var obj = (WoWObject)lbNearbyGameObjects.SelectedItem;
            if (obj != null)
            {
                tbQMobId.Text = obj.Entry.ToString();
                tbQMobName.Text = obj.Name;
            }

        }

        private void lbActiveQuests_SelectedValueChanged_1(object sender, EventArgs e)
        {
                var Quest = (PlayerQuest)lbActiveQuests.SelectedItem;
                if (Quest != null)
                {
                    tbQId.Text = Quest.Id.ToString();
                    tbQName.Text = Quest.Name;
                }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            WoWUnit target = null;
            if (StyxWoW.Me != null) target = StyxWoW.Me.CurrentTarget;
            if (target != null)
            {
                EC.log("Getting Target Information.");
                tbVendorId.Text = target.Entry.ToString();
                tbVendorName.Text = target.Name;
                tbVendorX.Text = target.X.ToString();
                tbVendorY.Text = target.Y.ToString();
                tbVendorZ.Text = target.Z.ToString();
            }
        }

        private void btnAddBlackspot_Click(object sender, EventArgs e)
        {
            EC.log("Adding BlackSpot");
            Blackspot bs = new Blackspot { Name=tbBSName.Text, Radius=tbBSRadius.Text, X = tbBSX.Text, Y=tbBSY.Text, Z=tbBSZ.Text };
            EclipseProfile.BlackSpots.Add(bs);
            loadData();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {

            if (StyxWoW.Me != null)
            {
                EC.log("Refreshing ActiveQuests.");
                lbActiveQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                lbActiveQuests.DisplayMember = "Name";
            }
        }

        private void btnRemoveQuest_Click(object sender, EventArgs e)
        {
            var qo = (QuestOrder)lbQuestOrders.SelectedItem;
            EclipseProfile.QuestOrders.Remove(qo);
            EC.log(string.Format("Removing Quest Type:{0}", qo.type));
            loadData();
        }

        private void btnCreateLogic_Click(object sender, EventArgs e)
        {
            try
            {
                EC.log("Showing LogicBlocks Screen");
                LogicBlocks lf = new LogicBlocks();
                lf.Show();
            }
            catch (Exception err)
            {
                EC.log(string.Format("Error Showing LogicBlocks! {0}", err.ToString()));
                MessageBox.Show(err.ToString());
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = EclipseProfile.QuestOrders;
            lbQuestOrders.DisplayMember = "DisplayName";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbQuestOrders.DataSource = null;
            lbQuestOrders.DataSource = EC.ReflectionFind(tbSearchText.Text);
            lbQuestOrders.DisplayMember = "DisplayName";
        }

        private void btnRemoveBlackSpot_Click(object sender, EventArgs e)
        {
            var bs = (Blackspot)lbBlackList.SelectedItem;
            EclipseProfile.BlackSpots.Remove(bs);
            loadData();
        }

        private void tbnZygor_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Warning this will replace your current profile information. Do you want to continue and lose any unsaved changes?", "Potentially lose changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "xml files (*.lua)|*.lua|All files (*.*)|*.*";
                if (tbFileName.Text.Length == 0)
                {
                    ofd.ShowDialog();
                    tbFileName.Text = ofd.FileName;
                }
                dt = new EclipseProfile();
                dt = ZygorReverseEngineer.ReadLua(dt, tbFileName.Text);
                loadData();
                EC.log("Loaded Profile Data.");
                tbFileName.Text = "";

            }
            else
            {

            }
        }
        private int count = 3;
        private string text = string.Empty;
        private void addChildren(Control control){
            
            foreach (Control cont in control.Controls)
            {
                if (cont.Text != string.Empty)
                {
                    count++;
                    text += string.Format("insert into Translations (id,language, key, value) values ({0}, 'CN', '{1}', '',{0});\r\n", count, cont.Text);
                    text += string.Format("insert into TranslationControls (id, name) values ({0}, '{1}');\r\n", count, cont.Name);
                }
                if (cont.HasChildren) addChildren(cont);
            }
            
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (Control cont in this.Controls)
            {
                if (cont.HasChildren) addChildren(cont);
            }
            new Display(text).Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Core.DataPath = tbDataFile.Text;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DAL.ExecuteSL3Query("drop table QuestOrders;");
        }

        private void btnRunSql_Click(object sender, EventArgs e)
        {
            DAL dal = new DAL();
            var dt = DAL.LoadSL3Data(tbSql.Text);
            dataGridView1.DataSource = dt;
        }

        private void btnGetTables_Click(object sender, EventArgs e)
        {
            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table';");
            lbTables.DataSource = dt;
            lbTables.DisplayMember = "Name";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tbSql_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbTables_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
