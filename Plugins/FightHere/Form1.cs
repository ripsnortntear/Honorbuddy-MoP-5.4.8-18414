using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;
using Styx;
using FightHere;
using Styx.Common;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Text.RegularExpressions;


namespace FightHere
{
    public class FHConfig : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public FHConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.trkRadius = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCenter = new System.Windows.Forms.Label();
            this.chkMobs = new System.Windows.Forms.CheckedListBox();
            this.lblRadius = new System.Windows.Forms.Label();
            this.chkChests = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(580, 376);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 32);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Choose which mobs to fight:";
            // 
            // trkRadius
            // 
            this.trkRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trkRadius.LargeChange = 50;
            this.trkRadius.Location = new System.Drawing.Point(12, 332);
            this.trkRadius.Maximum = 400;
            this.trkRadius.Name = "trkRadius";
            this.trkRadius.Size = new System.Drawing.Size(648, 69);
            this.trkRadius.SmallChange = 10;
            this.trkRadius.TabIndex = 5;
            this.trkRadius.TickFrequency = 50;
            this.trkRadius.Value = 200;
            this.trkRadius.Scroll += new System.EventHandler(this.trkRadius_Scroll);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 309);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Pull radius:";
            // 
            // lblCenter
            // 
            this.lblCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCenter.AutoSize = true;
            this.lblCenter.Location = new System.Drawing.Point(109, 276);
            this.lblCenter.Name = "lblCenter";
            this.lblCenter.Size = new System.Drawing.Size(41, 20);
            this.lblCenter.TabIndex = 7;
            this.lblCenter.Text = "cccc";
            // 
            // chkMobs
            // 
            this.chkMobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMobs.FormattingEnabled = true;
            this.chkMobs.Location = new System.Drawing.Point(16, 39);
            this.chkMobs.Name = "chkMobs";
            this.chkMobs.Size = new System.Drawing.Size(645, 214);
            this.chkMobs.TabIndex = 9;
            // 
            // lblRadius
            // 
            this.lblRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRadius.AutoSize = true;
            this.lblRadius.Location = new System.Drawing.Point(109, 309);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(29, 20);
            this.lblRadius.TabIndex = 10;
            this.lblRadius.Text = "tttt";
            // 
            // chkChests
            // 
            this.chkChests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkChests.AutoSize = true;
            this.chkChests.Location = new System.Drawing.Point(22, 376);
            this.chkChests.Name = "chkChests";
            this.chkChests.Size = new System.Drawing.Size(157, 24);
            this.chkChests.TabIndex = 11;
            this.chkChests.Text = "Also open chests";
            this.chkChests.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Center:";
            // 
            // FHConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 417);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkChests);
            this.Controls.Add(this.lblRadius);
            this.Controls.Add(this.chkMobs);
            this.Controls.Add(this.lblCenter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.trkRadius);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FHConfig";
            this.ShowIcon = false;
            this.Text = "FightHere by Kamilche";
            this.Load += new System.EventHandler(this.FHConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trkRadius)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TrackBar trkRadius;
        private Label label2;
        private Label lblCenter;
        private CheckedListBox chkMobs;
        private Label lblRadius;
        private bool running = false;
        private CheckBox chkChests;
        private Button btnSave;
        private Label label3;
        private List<FightHere.Mob> list = new List<FightHere.Mob>();

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save the range and mobs to the class
            FightHere.Range = trkRadius.Value;
            FightHere.Chests = chkChests.Checked;
            for (int i = 0; i < chkMobs.Items.Count; i++)
                list[i].want = chkMobs.GetItemChecked(i);
            FightHere.Mob.SetMobs(list);
            Close();
        }

        private void FHConfig_Load(object sender, EventArgs e)
        {
            // Load the range and mobs from the class
            LoadData();
            lblCenter.Text = string.Format("{0:0.00} {1:0.00} {2:0.00}", FightHere.center.X, FightHere.center.Y, FightHere.center.Z);
            trkRadius.Value = FightHere.Range;
            lblRadius.Text = FightHere.Range.ToString();
            chkChests.Checked = FightHere.Chests;
            for (int i = 0; i < chkMobs.Items.Count; i++)
                chkMobs.SetItemChecked(i, list[i].want);
        }

        private void trkRadius_Scroll(object sender, EventArgs e)
        {
            if (running == true)
                return;
            running = true;
            FightHere.Range = trkRadius.Value;
            lblRadius.Text = FightHere.Range.ToString();
            LoadData();
            for (int i = 0; i < chkMobs.Items.Count; i++)
                chkMobs.SetItemChecked(i, list[i].want);
            running = false;
        }

        private void LoadData()
        {
            ObjectManager.Update();
            FightHere.center = StyxWoW.Me.Location;
            list = FightHere.Mob.GetMobs();
            if (list != null && list.Count > 0)
            {
                try 
                { 
                    ((ListBox)chkMobs).DataSource = list; 
                }
                catch (Exception ex)
                {
                    Logging.Write(System.Windows.Media.Colors.Red, "DataSource Error = " + ex.Message);
                    throw;
                }


                try
                {
                    ((ListBox)chkMobs).DisplayMember = "name";
                }
                catch (Exception ex)
                {
                    Logging.Write(System.Windows.Media.Colors.Red, "DisplayMember Error = " + ex.Message);
                    throw;
                }

                try
                {
                    ((ListBox)chkMobs).ValueMember = "faction";
                }
                catch (Exception ex)
                {
                    Logging.Write(System.Windows.Media.Colors.Red, "Faction Error = " + ex.Message);
                    throw;
                }

            }
        }
    }
}






