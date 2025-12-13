namespace Oracle.UI
{
    partial class HealingSelector
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
            this.lstSelectedPlayers = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblHealers = new System.Windows.Forms.Label();
            this.lblDamage = new System.Windows.Forms.Label();
            this.lblTanks = new System.Windows.Forms.Label();
            this.lblPlayers = new System.Windows.Forms.Label();
            this.groupNumbers = new System.Windows.Forms.GroupBox();
            this.rdioGroupAll = new System.Windows.Forms.RadioButton();
            this.rdioGroup5 = new System.Windows.Forms.RadioButton();
            this.rdioGroup4 = new System.Windows.Forms.RadioButton();
            this.rdioGroup3 = new System.Windows.Forms.RadioButton();
            this.rdioGroup2 = new System.Windows.Forms.RadioButton();
            this.rdioGroup1 = new System.Windows.Forms.RadioButton();
            this.grpFilterRole = new System.Windows.Forms.GroupBox();
            this.rdioGroupRoleAll = new System.Windows.Forms.RadioButton();
            this.rdioGroupDamage = new System.Windows.Forms.RadioButton();
            this.rdioGroupHealer = new System.Windows.Forms.RadioButton();
            this.rdioGroupTank = new System.Windows.Forms.RadioButton();
            this.groupNumbers.SuspendLayout();
            this.grpFilterRole.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstSelectedPlayers
            // 
            this.lstSelectedPlayers.Location = new System.Drawing.Point(0, 80);
            this.lstSelectedPlayers.Name = "lstSelectedPlayers";
            this.lstSelectedPlayers.Size = new System.Drawing.Size(676, 457);
            this.lstSelectedPlayers.TabIndex = 0;
            this.lstSelectedPlayers.UseCompatibleStateImageBehavior = false;
            this.lstSelectedPlayers.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstSelectedPlayers_ItemChecked);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.Location = new System.Drawing.Point(0, 592);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(676, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRefresh.Location = new System.Drawing.Point(0, 569);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(676, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblHealers
            // 
            this.lblHealers.AutoSize = true;
            this.lblHealers.Location = new System.Drawing.Point(323, 540);
            this.lblHealers.Name = "lblHealers";
            this.lblHealers.Size = new System.Drawing.Size(93, 13);
            this.lblHealers.TabIndex = 8;
            this.lblHealers.Text = "Healer Count: [12]";
            // 
            // lblDamage
            // 
            this.lblDamage.AutoSize = true;
            this.lblDamage.Location = new System.Drawing.Point(422, 540);
            this.lblDamage.Name = "lblDamage";
            this.lblDamage.Size = new System.Drawing.Size(102, 13);
            this.lblDamage.TabIndex = 7;
            this.lblDamage.Text = "Damage Count: [18]";
            // 
            // lblTanks
            // 
            this.lblTanks.AutoSize = true;
            this.lblTanks.Location = new System.Drawing.Point(236, 540);
            this.lblTanks.Name = "lblTanks";
            this.lblTanks.Size = new System.Drawing.Size(81, 13);
            this.lblTanks.TabIndex = 6;
            this.lblTanks.Text = "Tank Count: [2]";
            // 
            // lblPlayers
            // 
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.Location = new System.Drawing.Point(139, 540);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(91, 13);
            this.lblPlayers.TabIndex = 5;
            this.lblPlayers.Text = "Player Count: [25]";
            // 
            // groupNumbers
            // 
            this.groupNumbers.Controls.Add(this.rdioGroupAll);
            this.groupNumbers.Controls.Add(this.rdioGroup5);
            this.groupNumbers.Controls.Add(this.rdioGroup4);
            this.groupNumbers.Controls.Add(this.rdioGroup3);
            this.groupNumbers.Controls.Add(this.rdioGroup2);
            this.groupNumbers.Controls.Add(this.rdioGroup1);
            this.groupNumbers.Location = new System.Drawing.Point(12, 12);
            this.groupNumbers.Name = "groupNumbers";
            this.groupNumbers.Size = new System.Drawing.Size(253, 62);
            this.groupNumbers.TabIndex = 9;
            this.groupNumbers.TabStop = false;
            this.groupNumbers.Text = "Filter by Group Number";
            // 
            // rdioGroupAll
            // 
            this.rdioGroupAll.AutoSize = true;
            this.rdioGroupAll.Location = new System.Drawing.Point(206, 28);
            this.rdioGroupAll.Name = "rdioGroupAll";
            this.rdioGroupAll.Size = new System.Drawing.Size(36, 17);
            this.rdioGroupAll.TabIndex = 11;
            this.rdioGroupAll.TabStop = true;
            this.rdioGroupAll.Text = "All";
            this.rdioGroupAll.UseVisualStyleBackColor = true;
            this.rdioGroupAll.CheckedChanged += new System.EventHandler(this.rdioGroupAll_CheckedChanged);
            // 
            // rdioGroup5
            // 
            this.rdioGroup5.AutoSize = true;
            this.rdioGroup5.Location = new System.Drawing.Point(169, 28);
            this.rdioGroup5.Name = "rdioGroup5";
            this.rdioGroup5.Size = new System.Drawing.Size(31, 17);
            this.rdioGroup5.TabIndex = 10;
            this.rdioGroup5.TabStop = true;
            this.rdioGroup5.Text = "4";
            this.rdioGroup5.UseVisualStyleBackColor = true;
            this.rdioGroup5.CheckedChanged += new System.EventHandler(this.rdioGroup5_CheckedChanged);
            // 
            // rdioGroup4
            // 
            this.rdioGroup4.AutoSize = true;
            this.rdioGroup4.Location = new System.Drawing.Point(132, 28);
            this.rdioGroup4.Name = "rdioGroup4";
            this.rdioGroup4.Size = new System.Drawing.Size(31, 17);
            this.rdioGroup4.TabIndex = 9;
            this.rdioGroup4.TabStop = true;
            this.rdioGroup4.Text = "3";
            this.rdioGroup4.UseVisualStyleBackColor = true;
            this.rdioGroup4.CheckedChanged += new System.EventHandler(this.rdioGroup4_CheckedChanged);
            // 
            // rdioGroup3
            // 
            this.rdioGroup3.AutoSize = true;
            this.rdioGroup3.Location = new System.Drawing.Point(95, 28);
            this.rdioGroup3.Name = "rdioGroup3";
            this.rdioGroup3.Size = new System.Drawing.Size(31, 17);
            this.rdioGroup3.TabIndex = 8;
            this.rdioGroup3.TabStop = true;
            this.rdioGroup3.Text = "2";
            this.rdioGroup3.UseVisualStyleBackColor = true;
            this.rdioGroup3.CheckedChanged += new System.EventHandler(this.rdioGroup3_CheckedChanged);
            // 
            // rdioGroup2
            // 
            this.rdioGroup2.AutoSize = true;
            this.rdioGroup2.Location = new System.Drawing.Point(58, 28);
            this.rdioGroup2.Name = "rdioGroup2";
            this.rdioGroup2.Size = new System.Drawing.Size(31, 17);
            this.rdioGroup2.TabIndex = 7;
            this.rdioGroup2.TabStop = true;
            this.rdioGroup2.Text = "1";
            this.rdioGroup2.UseVisualStyleBackColor = true;
            this.rdioGroup2.CheckedChanged += new System.EventHandler(this.rdioGroup2_CheckedChanged);
            // 
            // rdioGroup1
            // 
            this.rdioGroup1.AutoSize = true;
            this.rdioGroup1.Location = new System.Drawing.Point(21, 28);
            this.rdioGroup1.Name = "rdioGroup1";
            this.rdioGroup1.Size = new System.Drawing.Size(31, 17);
            this.rdioGroup1.TabIndex = 6;
            this.rdioGroup1.TabStop = true;
            this.rdioGroup1.Text = "0";
            this.rdioGroup1.UseVisualStyleBackColor = true;
            this.rdioGroup1.CheckedChanged += new System.EventHandler(this.rdioGroup1_CheckedChanged);
            // 
            // grpFilterRole
            // 
            this.grpFilterRole.Controls.Add(this.rdioGroupRoleAll);
            this.grpFilterRole.Controls.Add(this.rdioGroupDamage);
            this.grpFilterRole.Controls.Add(this.rdioGroupHealer);
            this.grpFilterRole.Controls.Add(this.rdioGroupTank);
            this.grpFilterRole.Location = new System.Drawing.Point(271, 12);
            this.grpFilterRole.Name = "grpFilterRole";
            this.grpFilterRole.Size = new System.Drawing.Size(253, 62);
            this.grpFilterRole.TabIndex = 13;
            this.grpFilterRole.TabStop = false;
            this.grpFilterRole.Text = "Filter by Role";
            // 
            // rdioGroupRoleAll
            // 
            this.rdioGroupRoleAll.AutoSize = true;
            this.rdioGroupRoleAll.Location = new System.Drawing.Point(210, 28);
            this.rdioGroupRoleAll.Name = "rdioGroupRoleAll";
            this.rdioGroupRoleAll.Size = new System.Drawing.Size(36, 17);
            this.rdioGroupRoleAll.TabIndex = 11;
            this.rdioGroupRoleAll.TabStop = true;
            this.rdioGroupRoleAll.Text = "All";
            this.rdioGroupRoleAll.UseVisualStyleBackColor = true;
            this.rdioGroupRoleAll.CheckedChanged += new System.EventHandler(this.rdioGroupRoleAll_CheckedChanged);
            // 
            // rdioGroupDamage
            // 
            this.rdioGroupDamage.AutoSize = true;
            this.rdioGroupDamage.Location = new System.Drawing.Point(139, 28);
            this.rdioGroupDamage.Name = "rdioGroupDamage";
            this.rdioGroupDamage.Size = new System.Drawing.Size(65, 17);
            this.rdioGroupDamage.TabIndex = 8;
            this.rdioGroupDamage.TabStop = true;
            this.rdioGroupDamage.Text = "Damage";
            this.rdioGroupDamage.UseVisualStyleBackColor = true;
            this.rdioGroupDamage.CheckedChanged += new System.EventHandler(this.rdioGroupDamage_CheckedChanged);
            // 
            // rdioGroupHealer
            // 
            this.rdioGroupHealer.AutoSize = true;
            this.rdioGroupHealer.Location = new System.Drawing.Point(77, 28);
            this.rdioGroupHealer.Name = "rdioGroupHealer";
            this.rdioGroupHealer.Size = new System.Drawing.Size(56, 17);
            this.rdioGroupHealer.TabIndex = 7;
            this.rdioGroupHealer.TabStop = true;
            this.rdioGroupHealer.Text = "Healer";
            this.rdioGroupHealer.UseVisualStyleBackColor = true;
            this.rdioGroupHealer.CheckedChanged += new System.EventHandler(this.rdioGroupHealer_CheckedChanged);
            // 
            // rdioGroupTank
            // 
            this.rdioGroupTank.AutoSize = true;
            this.rdioGroupTank.Location = new System.Drawing.Point(21, 28);
            this.rdioGroupTank.Name = "rdioGroupTank";
            this.rdioGroupTank.Size = new System.Drawing.Size(50, 17);
            this.rdioGroupTank.TabIndex = 6;
            this.rdioGroupTank.TabStop = true;
            this.rdioGroupTank.Text = "Tank";
            this.rdioGroupTank.UseVisualStyleBackColor = true;
            this.rdioGroupTank.CheckedChanged += new System.EventHandler(this.rdioGroupTank_CheckedChanged);
            // 
            // HealingSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 615);
            this.Controls.Add(this.grpFilterRole);
            this.Controls.Add(this.groupNumbers);
            this.Controls.Add(this.lblHealers);
            this.Controls.Add(this.lblDamage);
            this.Controls.Add(this.lblTanks);
            this.Controls.Add(this.lblPlayers);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstSelectedPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HealingSelector";
            this.Text = "Please select players to Heal";
            this.groupNumbers.ResumeLayout(false);
            this.groupNumbers.PerformLayout();
            this.grpFilterRole.ResumeLayout(false);
            this.grpFilterRole.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstSelectedPlayers;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblHealers;
        private System.Windows.Forms.Label lblDamage;
        private System.Windows.Forms.Label lblTanks;
        private System.Windows.Forms.Label lblPlayers;
        private System.Windows.Forms.GroupBox groupNumbers;
        private System.Windows.Forms.RadioButton rdioGroupAll;
        private System.Windows.Forms.RadioButton rdioGroup5;
        private System.Windows.Forms.RadioButton rdioGroup4;
        private System.Windows.Forms.RadioButton rdioGroup3;
        private System.Windows.Forms.RadioButton rdioGroup2;
        private System.Windows.Forms.RadioButton rdioGroup1;
        private System.Windows.Forms.GroupBox grpFilterRole;
        private System.Windows.Forms.RadioButton rdioGroupRoleAll;
        private System.Windows.Forms.RadioButton rdioGroupDamage;
        private System.Windows.Forms.RadioButton rdioGroupHealer;
        private System.Windows.Forms.RadioButton rdioGroupTank;
    }
}