using Eclipse.EclipsePlugins.Models;
using Styx;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eclipse.EclipsePlugins.Views
{
    public partial class CustomBehaviorForm : Form
    {
        public CustomBehavior cb = new CustomBehavior();
        public CustomBehaviorForm()
        {
            InitializeComponent();
            cb.type = QuestOrder.QOType.CustomBehavior;
            if (StyxWoW.Me != null)
            {
                tbQX.Text = StyxWoW.Me.X.ToString();
                tbQY.Text = StyxWoW.Me.Y.ToString();
                tbQZ.Text = StyxWoW.Me.Z.ToString();
                cbQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                cbQuests.DisplayMember = "Name";
                cbQuests.ValueMember = "Id";


            }

            
        }

        private void CustomBehavior_Load(object sender, EventArgs e)
        {
            cbFile.DataSource = CustomBehavior.File;
            cbAttributes.DataSource = CustomBehavior.Attributes;
            label4.Text = @"Explanation:\r\nCertain behaviors are not addressed as a simple autonomous call to honorbuddy like 'turnin' and 'pickup' etc. Some require you to do something else, like listen to the panda drone on and on for 10 minutes. These behaviours are how you handle things like that \r\n Example: use File 'TalkToAndListenToStory' and add attribute QuestId and NpcID Followed by a file of 'WaitTimer' with value of '600000' (in milliseconds) to listen to someone talk for a minute before moving on to the next part of the quest.
                Note: This will require adding TWO custom behaviors. One to initiate the conversation at the required NPC and another to wait from him to shuttup.
                ";

        }

        private void btnAddAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                cb.file = cbFile.Text;
                cb.attributes.Add(cbAttributes.Text, tbAttValue.Text);
                previewCB();
            }
            catch (Exception err){
                MessageBox.Show("That attribute has already been added.");
            }
        }
        private void previewCB()
        {
            var atts = string.Empty;
            foreach (var att in cb.attributes){
                atts += string.Format(" {0}='{1}'", att.Key, att.Value);
            }
            tbOutput.Text = string.Format("<CustomBehavior File='{0}' {1} />", cb.file, atts);
        }

        private void cbFile_SelectedValueChanged(object sender, EventArgs e)
        {
            cb.file = cbFile.Text;
            previewCB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EclipseProfile.CustomBehaviors.Add(cb);
        }

        private void getTargetIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget != null)
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

        private void getTargetNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget != null)
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
        private void btnInteractWith_Click(object sender, EventArgs e)
        {
            cb = new CustomBehavior();
            cb.file = "InteractWith";
            cb.attributes.Add("QuestId", textBox2.Text);
            cb.attributes.Add("QuestObjectiveIndex", textBox3.Text);
            cb.attributes.Add("MobId", textBox1.Text);
            cb.attributes.Add("GossipOptions", textBox5.Text);
            cb.attributes.Add("NonCompeteDistance", textBox4.Text);
            cb.attributes.Add("X", tbQX.Text);
            cb.attributes.Add("Y", tbQY.Text);
            cb.attributes.Add("Z", tbQZ.Text);
            previewCB();

        }

        private void cbQuests_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = ((PlayerQuest)cbQuests.SelectedItem).Id.ToString();
            textBox6.Text = ((PlayerQuest)cbQuests.SelectedItem).Id.ToString();
            textBox11.Text = ((PlayerQuest)cbQuests.SelectedItem).Id.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cb = new CustomBehavior();
            cb.file = "InteractWith";
            cb.attributes.Add("QuestId", textBox6.Text);
            cb.attributes.Add("MobId", textBox9.Text);
            cb.attributes.Add("NumOfTimes", textBox7.Text);
            cb.attributes.Add("WaitTime", textBox8.Text);
            cb.attributes.Add("IgnoreMobsInBlackspots", textBox10.Text);
            cb.attributes.Add("X", tbQX.Text);
            cb.attributes.Add("Y", tbQY.Text);
            cb.attributes.Add("Z", tbQZ.Text);
            previewCB();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            cb = new CustomBehavior();
            cb.file = "CastSpellOn";
            cb.attributes.Add("QuestId", textBox6.Text);
            cb.attributes.Add("MobId", textBox14.Text);
            cb.attributes.Add("NumOfTimes", textBox12.Text);
            cb.attributes.Add("Range", textBox13.Text);
            cb.attributes.Add("MinRange", textBox17.Text);
            cb.attributes.Add("MobHPPercentLeft", textBox15.Text);
            cb.attributes.Add("SpellId", textBox16.Text);
            cb.attributes.Add("X", tbQX.Text);
            cb.attributes.Add("Y", tbQY.Text);
            cb.attributes.Add("Z", tbQZ.Text);
            previewCB();
        }


    }
}
