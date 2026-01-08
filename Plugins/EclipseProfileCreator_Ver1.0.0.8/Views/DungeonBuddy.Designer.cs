namespace Eclipse.EclipsePlugins.Views
{
    partial class DungeonBuddy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.lblProfileName = new System.Windows.Forms.Label();
            this.btnNewProfile = new System.Windows.Forms.Button();
            this.btnLoadProfile = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.pbEclipse = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.lbBosses = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbHotSpots = new System.Windows.Forms.ListBox();
            this.btnAddBoss = new System.Windows.Forms.Button();
            this.checkBoxLastBoss = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetTarget = new System.Windows.Forms.Button();
            this.tbBossName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBossId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbKillOrder = new System.Windows.Forms.TextBox();
            this.checkBoxOptional = new System.Windows.Forms.CheckBox();
            this.btnGetMobInfo = new System.Windows.Forms.Button();
            this.btnGetPosition = new System.Windows.Forms.Button();
            this.tbQZ = new System.Windows.Forms.TextBox();
            this.tbQY = new System.Windows.Forms.TextBox();
            this.tbQX = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.lblQY = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbEclipse)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProfileName
            // 
            this.lblProfileName.BackColor = System.Drawing.Color.Transparent;
            this.lblProfileName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileName.ForeColor = System.Drawing.Color.DarkMagenta;
            this.lblProfileName.Location = new System.Drawing.Point(0, 177);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(463, 37);
            this.lblProfileName.TabIndex = 7;
            this.lblProfileName.Text = "Profile Name";
            this.lblProfileName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnNewProfile
            // 
            this.btnNewProfile.Location = new System.Drawing.Point(11, 114);
            this.btnNewProfile.Name = "btnNewProfile";
            this.btnNewProfile.Size = new System.Drawing.Size(86, 23);
            this.btnNewProfile.TabIndex = 8;
            this.btnNewProfile.Text = "New Profile";
            this.btnNewProfile.UseVisualStyleBackColor = true;
            this.btnNewProfile.Click += new System.EventHandler(this.btnNewProfile_Click);
            // 
            // btnLoadProfile
            // 
            this.btnLoadProfile.Location = new System.Drawing.Point(11, 142);
            this.btnLoadProfile.Name = "btnLoadProfile";
            this.btnLoadProfile.Size = new System.Drawing.Size(87, 23);
            this.btnLoadProfile.TabIndex = 9;
            this.btnLoadProfile.Text = "Load Profile";
            this.btnLoadProfile.UseVisualStyleBackColor = true;
            this.btnLoadProfile.Click += new System.EventHandler(this.btnLoadProfile_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(104, 145);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(351, 20);
            this.tbFileName.TabIndex = 6;
            // 
            // pbEclipse
            // 
            this.pbEclipse.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbEclipse.Location = new System.Drawing.Point(0, 0);
            this.pbEclipse.Name = "pbEclipse";
            this.pbEclipse.Size = new System.Drawing.Size(463, 177);
            this.pbEclipse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEclipse.TabIndex = 10;
            this.pbEclipse.TabStop = false;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Magenta;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.Color.Yellow;
            this.button7.Location = new System.Drawing.Point(380, 12);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 24);
            this.button7.TabIndex = 11;
            this.button7.Text = "Donate";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // lbBosses
            // 
            this.lbBosses.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBosses.FormattingEnabled = true;
            this.lbBosses.Location = new System.Drawing.Point(3, 16);
            this.lbBosses.Name = "lbBosses";
            this.lbBosses.Size = new System.Drawing.Size(191, 105);
            this.lbBosses.TabIndex = 12;
            this.lbBosses.SelectedValueChanged += new System.EventHandler(this.lbBosses_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Boss Name";
            // 
            // lbHotSpots
            // 
            this.lbHotSpots.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbHotSpots.FormattingEnabled = true;
            this.lbHotSpots.Location = new System.Drawing.Point(3, 16);
            this.lbHotSpots.Name = "lbHotSpots";
            this.lbHotSpots.Size = new System.Drawing.Size(445, 95);
            this.lbHotSpots.TabIndex = 12;
            // 
            // btnAddBoss
            // 
            this.btnAddBoss.Location = new System.Drawing.Point(353, 91);
            this.btnAddBoss.Name = "btnAddBoss";
            this.btnAddBoss.Size = new System.Drawing.Size(90, 23);
            this.btnAddBoss.TabIndex = 14;
            this.btnAddBoss.Text = "Add Boss";
            this.btnAddBoss.UseVisualStyleBackColor = true;
            this.btnAddBoss.Click += new System.EventHandler(this.btnAddBoss_Click);
            // 
            // checkBoxLastBoss
            // 
            this.checkBoxLastBoss.AutoSize = true;
            this.checkBoxLastBoss.Location = new System.Drawing.Point(200, 19);
            this.checkBoxLastBoss.Name = "checkBoxLastBoss";
            this.checkBoxLastBoss.Size = new System.Drawing.Size(78, 17);
            this.checkBoxLastBoss.TabIndex = 15;
            this.checkBoxLastBoss.Text = "Last Boss?";
            this.checkBoxLastBoss.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(330, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(133, 42);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Legend";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(11, 17);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(20, 23);
            this.button5.TabIndex = 14;
            this.button5.Text = "T";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "= in-game Target";
            // 
            // btnGetTarget
            // 
            this.btnGetTarget.Location = new System.Drawing.Point(200, 52);
            this.btnGetTarget.Name = "btnGetTarget";
            this.btnGetTarget.Size = new System.Drawing.Size(20, 23);
            this.btnGetTarget.TabIndex = 17;
            this.btnGetTarget.Text = "T";
            this.btnGetTarget.UseVisualStyleBackColor = true;
            this.btnGetTarget.Click += new System.EventHandler(this.btnGetTarget_Click);
            // 
            // tbBossName
            // 
            this.tbBossName.Location = new System.Drawing.Point(230, 61);
            this.tbBossName.Name = "tbBossName";
            this.tbBossName.Size = new System.Drawing.Size(100, 20);
            this.tbBossName.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(336, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Boss Id";
            // 
            // tbBossId
            // 
            this.tbBossId.Location = new System.Drawing.Point(339, 61);
            this.tbBossId.Name = "tbBossId";
            this.tbBossId.Size = new System.Drawing.Size(49, 20);
            this.tbBossId.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(391, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Kill Order";
            // 
            // tbKillOrder
            // 
            this.tbKillOrder.Location = new System.Drawing.Point(394, 61);
            this.tbKillOrder.Name = "tbKillOrder";
            this.tbKillOrder.Size = new System.Drawing.Size(49, 20);
            this.tbKillOrder.TabIndex = 18;
            // 
            // checkBoxOptional
            // 
            this.checkBoxOptional.AutoSize = true;
            this.checkBoxOptional.Location = new System.Drawing.Point(310, 19);
            this.checkBoxOptional.Name = "checkBoxOptional";
            this.checkBoxOptional.Size = new System.Drawing.Size(71, 17);
            this.checkBoxOptional.TabIndex = 15;
            this.checkBoxOptional.Text = "Optional?";
            this.checkBoxOptional.UseVisualStyleBackColor = true;
            // 
            // btnGetMobInfo
            // 
            this.btnGetMobInfo.Location = new System.Drawing.Point(20, 200);
            this.btnGetMobInfo.Name = "btnGetMobInfo";
            this.btnGetMobInfo.Size = new System.Drawing.Size(20, 24);
            this.btnGetMobInfo.TabIndex = 26;
            this.btnGetMobInfo.Text = "T";
            this.btnGetMobInfo.UseVisualStyleBackColor = true;
            // 
            // btnGetPosition
            // 
            this.btnGetPosition.Location = new System.Drawing.Point(44, 226);
            this.btnGetPosition.Name = "btnGetPosition";
            this.btnGetPosition.Size = new System.Drawing.Size(180, 22);
            this.btnGetPosition.TabIndex = 25;
            this.btnGetPosition.Text = "Get Current Position";
            this.btnGetPosition.UseVisualStyleBackColor = true;
            this.btnGetPosition.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbQZ
            // 
            this.tbQZ.Location = new System.Drawing.Point(189, 204);
            this.tbQZ.Name = "tbQZ";
            this.tbQZ.Size = new System.Drawing.Size(35, 20);
            this.tbQZ.TabIndex = 22;
            // 
            // tbQY
            // 
            this.tbQY.Location = new System.Drawing.Point(127, 204);
            this.tbQY.Name = "tbQY";
            this.tbQY.Size = new System.Drawing.Size(35, 20);
            this.tbQY.TabIndex = 23;
            // 
            // tbQX
            // 
            this.tbQX.Location = new System.Drawing.Point(64, 204);
            this.tbQX.Name = "tbQX";
            this.tbQX.Size = new System.Drawing.Size(35, 20);
            this.tbQX.TabIndex = 24;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(169, 207);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(14, 13);
            this.label18.TabIndex = 19;
            this.label18.Text = "Z";
            // 
            // lblQY
            // 
            this.lblQY.AutoSize = true;
            this.lblQY.Location = new System.Drawing.Point(107, 207);
            this.lblQY.Name = "lblQY";
            this.lblQY.Size = new System.Drawing.Size(14, 13);
            this.lblQY.TabIndex = 20;
            this.lblQY.Text = "Y";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(44, 207);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 13);
            this.label16.TabIndex = 21;
            this.label16.Text = "X";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbBosses);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnAddBoss);
            this.groupBox1.Controls.Add(this.checkBoxLastBoss);
            this.groupBox1.Controls.Add(this.checkBoxOptional);
            this.groupBox1.Controls.Add(this.btnGetTarget);
            this.groupBox1.Controls.Add(this.tbBossName);
            this.groupBox1.Controls.Add(this.tbKillOrder);
            this.groupBox1.Controls.Add(this.tbBossId);
            this.groupBox1.Location = new System.Drawing.Point(4, 268);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 124);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bosses";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbHotSpots);
            this.groupBox3.Location = new System.Drawing.Point(4, 398);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(451, 114);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Boss\' HotSpots";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(29, 524);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(403, 25);
            this.label2.TabIndex = 13;
            this.label2.Text = ">>PROFILES CANNOT BE SAVED YET!<<";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 564);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(394, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "This is to prevent damaging existing profiles by saving over them with flawed one" +
    "s.";
            // 
            // DungeonBuddy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 586);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetMobInfo);
            this.Controls.Add(this.btnGetPosition);
            this.Controls.Add(this.tbQZ);
            this.Controls.Add(this.tbQY);
            this.Controls.Add(this.tbQX);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.lblQY);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.lblProfileName);
            this.Controls.Add(this.btnNewProfile);
            this.Controls.Add(this.btnLoadProfile);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.pbEclipse);
            this.Name = "DungeonBuddy";
            this.Text = "DungeonBuddy";
            this.Load += new System.EventHandler(this.DungeonBuddy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEclipse)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProfileName;
        private System.Windows.Forms.Button btnNewProfile;
        private System.Windows.Forms.Button btnLoadProfile;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.PictureBox pbEclipse;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ListBox lbBosses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbHotSpots;
        private System.Windows.Forms.Button btnAddBoss;
        private System.Windows.Forms.CheckBox checkBoxLastBoss;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetTarget;
        private System.Windows.Forms.TextBox tbBossName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBossId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbKillOrder;
        private System.Windows.Forms.CheckBox checkBoxOptional;
        private System.Windows.Forms.Button btnGetMobInfo;
        private System.Windows.Forms.Button btnGetPosition;
        private System.Windows.Forms.TextBox tbQZ;
        private System.Windows.Forms.TextBox tbQY;
        private System.Windows.Forms.TextBox tbQX;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblQY;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
    }
}