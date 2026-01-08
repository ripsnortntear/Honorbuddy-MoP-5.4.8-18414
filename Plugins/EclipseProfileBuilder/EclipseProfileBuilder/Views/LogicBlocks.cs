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
    public partial class LogicBlocks : Form
    {
        public LogicBlocks()
        {
            InitializeComponent();
        }


        
        private void LogicBlocks_Load(object sender, EventArgs e)
        {
            label8.Text = @"Use this screen to create if/while logic in your profile.  If you want for example to make sure you have one quest before picking another. Or want to make sure you ahve all the quests prior to going to a certain area. 
                    These can be used in both Quest orders and QuestOverrides";

            if (StyxWoW.Me != null)
            {
                try
                {
                    tbQX.Text = StyxWoW.Me.X.ToString();
                    tbQY.Text = StyxWoW.Me.Y.ToString();
                    tbQZ.Text = StyxWoW.Me.Z.ToString();

                    cbQuests.DataSource = StyxWoW.Me.QuestLog.GetAllQuests();
                    cbQuests.DisplayMember = "Name";
                    cbQuests.ValueMember = "Id";
                } catch (Exception err){
                    MessageBox.Show(err.ToString());
                }

            }
        }
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            cbComparer.Enabled = checkBox1.Checked;
            cbCondition2.Enabled = checkBox1.Checked;
        }
        private void updateLogicPreview()
        {
            var qid = "0";
            if (((PlayerQuest)cbQuests.SelectedItem) != null)
            {
                qid = ((PlayerQuest)cbQuests.SelectedItem).Id.ToString();
            }
            if (cbLogicType.Text == "If")
            {
                if (checkBox1.Checked) tbPreviewLogic.Text = string.Format("<If Condition='{0} {1} {2}'></If>", cbCondition1.Text.Replace("{questid}", qid), cbComparer.Text, cbCondition2.Text.Replace("{questid}", qid));
                else tbPreviewLogic.Text = string.Format("<If Condition='{0}'></If>", cbCondition1.Text.Replace("{questid}", qid));
            }
            else
            {
                if (checkBox1.Checked) tbPreviewLogic.Text = string.Format("<While Condition='{0} {1} {2}'></While>", cbCondition1.Text.Replace("{questid}", qid), cbComparer.Text, cbCondition2.Text.Replace("{questid}", qid));
                else tbPreviewLogic.Text = string.Format("<While Condition='{0}'></While>", cbCondition1.Text.Replace("{questid}", qid));
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            updateLogicPreview();
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            updateLogicPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var qid = "0";
            if (((PlayerQuest)cbQuests.SelectedItem) != null)
            {
                qid = ((PlayerQuest)cbQuests.SelectedItem).Id.ToString();
            }
            QuestOrderLogic qol = new QuestOrderLogic();
            qol.LogicType = cbLogicType.Text;

            qol.LogicType = cbLogicType.Text;
            if (checkBox1.Checked) qol.Condition = string.Format("{0} {1} {2}", cbCondition1.Text.Replace("{questid}", qid), cbComparer.Text, cbCondition2.Text.Replace("{questid}", qid));
            else qol.Condition = string.Format("{0}", cbCondition1.Text.Replace("{questid}", qid));

            qol.Description = tbDescription.Text;
            if (tbCustom.Text.Length > 0) qol.Condition = tbCustom.Text;
            qol.StartTag = true;
            qol.type = QuestOrder.QOType.LogicBlock;
            EclipseProfile.LogicBlocks.Add(qol);
            MessageBox.Show("Your Logic Block has been added!");
        }

        private void btnMoveToCustom_Click(object sender, EventArgs e)
        {
            var qid = "0";
            if (((PlayerQuest)cbQuests.SelectedItem) != null)
            {
                qid = ((PlayerQuest)cbQuests.SelectedItem).Id.ToString();
            }
              
            if (checkBox1.Checked) tbCustom.Text = string.Format("{0} {1} {2}", cbCondition1.Text.Replace("{questid}", qid), cbComparer.Text, cbCondition2.Text.Replace("{questid}", qid));
            else tbCustom.Text = string.Format("{0}", cbCondition1.Text.Replace("{questid}", qid));
        }
        
    }
}
