namespace MrGearBuyer.GUI
{
    partial class MGBConfig
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
            this.AddtoBuy = new System.Windows.Forms.Button();
            this.Fetch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvVenderList = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.dgvBuyList = new System.Windows.Forms.DataGridView();
            this.RemoveBuy = new System.Windows.Forms.Button();
            this.LogoutAtCap = new System.Windows.Forms.CheckBox();
            this.RemoveJPHPWhenCapped = new System.Windows.Forms.CheckBox();
            this.BuyOppositePointToBuildUp = new System.Windows.Forms.CheckBox();
            this.RemoveFromBL = new System.Windows.Forms.Button();
            this.HomePointTEXT = new System.Windows.Forms.Label();
            this.HomePointSet = new System.Windows.Forms.Button();
            this.EquipAfterBuy = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Tabs = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.RepairSet = new System.Windows.Forms.Button();
            this.RepairSwap = new System.Windows.Forms.CheckBox();
            this.LogOutJPCap = new System.Windows.Forms.CheckBox();
            this.LogOutHPCap = new System.Windows.Forms.CheckBox();
            this.MyMount = new System.Windows.Forms.CheckBox();
            this.RepairInfo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVenderList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddtoBuy
            // 
            this.AddtoBuy.Location = new System.Drawing.Point(1, 258);
            this.AddtoBuy.Name = "AddtoBuy";
            this.AddtoBuy.Size = new System.Drawing.Size(168, 23);
            this.AddtoBuy.TabIndex = 4;
            this.AddtoBuy.Text = "Add Selected Items To Buy List";
            this.AddtoBuy.UseVisualStyleBackColor = true;
            this.AddtoBuy.Click += new System.EventHandler(this.AddtoBuyClick);
            // 
            // Fetch
            // 
            this.Fetch.Location = new System.Drawing.Point(169, 258);
            this.Fetch.Name = "Fetch";
            this.Fetch.Size = new System.Drawing.Size(110, 23);
            this.Fetch.TabIndex = 5;
            this.Fetch.Text = "Fetch Vender Items";
            this.Fetch.UseVisualStyleBackColor = true;
            this.Fetch.Click += new System.EventHandler(this.FetchClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Fetch);
            this.groupBox1.Controls.Add(this.dgvVenderList);
            this.groupBox1.Controls.Add(this.AddtoBuy);
            this.groupBox1.Location = new System.Drawing.Point(291, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 293);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vender Controls";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // dgvVenderList
            // 
            this.dgvVenderList.AllowUserToAddRows = false;
            this.dgvVenderList.AllowUserToDeleteRows = false;
            this.dgvVenderList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVenderList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvVenderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVenderList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvVenderList.Location = new System.Drawing.Point(6, 19);
            this.dgvVenderList.Name = "dgvVenderList";
            this.dgvVenderList.ReadOnly = true;
            this.dgvVenderList.RowHeadersVisible = false;
            this.dgvVenderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVenderList.Size = new System.Drawing.Size(270, 233);
            this.dgvVenderList.TabIndex = 4;
            this.dgvVenderList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvVenderListCellMouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMoveDown);
            this.groupBox2.Controls.Add(this.btnMoveUp);
            this.groupBox2.Controls.Add(this.dgvBuyList);
            this.groupBox2.Controls.Add(this.RemoveBuy);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 293);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "My Buy List";
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(197, 258);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 4;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.BtnMoveDownClick);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(116, 258);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 3;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.BtnMoveUpClick);
            // 
            // dgvBuyList
            // 
            this.dgvBuyList.AllowUserToAddRows = false;
            this.dgvBuyList.AllowUserToDeleteRows = false;
            this.dgvBuyList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBuyList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvBuyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuyList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBuyList.Location = new System.Drawing.Point(6, 19);
            this.dgvBuyList.Name = "dgvBuyList";
            this.dgvBuyList.RowHeadersVisible = false;
            this.dgvBuyList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBuyList.Size = new System.Drawing.Size(270, 233);
            this.dgvBuyList.TabIndex = 2;
            this.dgvBuyList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvBuyListCellMouseDoubleClick);
            // 
            // RemoveBuy
            // 
            this.RemoveBuy.Location = new System.Drawing.Point(6, 258);
            this.RemoveBuy.Name = "RemoveBuy";
            this.RemoveBuy.Size = new System.Drawing.Size(104, 23);
            this.RemoveBuy.TabIndex = 1;
            this.RemoveBuy.Text = "Remove Selected Item";
            this.RemoveBuy.UseVisualStyleBackColor = true;
            this.RemoveBuy.Click += new System.EventHandler(this.RemoveBuyClick);
            // 
            // LogoutAtCap
            // 
            this.LogoutAtCap.AutoSize = true;
            this.LogoutAtCap.Location = new System.Drawing.Point(3, 8);
            this.LogoutAtCap.Name = "LogoutAtCap";
            this.LogoutAtCap.Size = new System.Drawing.Size(292, 17);
            this.LogoutAtCap.TabIndex = 10;
            this.LogoutAtCap.Text = "Logout when Both Honor and Justice Points are Capped";
            this.LogoutAtCap.UseVisualStyleBackColor = true;
            this.LogoutAtCap.CheckedChanged += new System.EventHandler(this.LogoutAtCapCheckedChanged);
            // 
            // RemoveJPHPWhenCapped
            // 
            this.RemoveJPHPWhenCapped.AutoSize = true;
            this.RemoveJPHPWhenCapped.Location = new System.Drawing.Point(3, 102);
            this.RemoveJPHPWhenCapped.Name = "RemoveJPHPWhenCapped";
            this.RemoveJPHPWhenCapped.Size = new System.Drawing.Size(323, 17);
            this.RemoveJPHPWhenCapped.TabIndex = 11;
            this.RemoveJPHPWhenCapped.Text = "Only Remove Justice And Honor Points from List when Capped";
            this.RemoveJPHPWhenCapped.UseVisualStyleBackColor = true;
            this.RemoveJPHPWhenCapped.CheckedChanged += new System.EventHandler(this.RemoveJphpWhenCappedCheckedChanged);
            // 
            // BuyOppositePointToBuildUp
            // 
            this.BuyOppositePointToBuildUp.AutoSize = true;
            this.BuyOppositePointToBuildUp.Location = new System.Drawing.Point(3, 79);
            this.BuyOppositePointToBuildUp.Name = "BuyOppositePointToBuildUp";
            this.BuyOppositePointToBuildUp.Size = new System.Drawing.Size(355, 17);
            this.BuyOppositePointToBuildUp.TabIndex = 12;
            this.BuyOppositePointToBuildUp.Text = "Allow Automatic Conversion of Honor and Justice Points to Buy Gear?";
            this.BuyOppositePointToBuildUp.UseVisualStyleBackColor = true;
            this.BuyOppositePointToBuildUp.CheckedChanged += new System.EventHandler(this.BuyOppositePointToBuildUp_CheckedChanged);
            // 
            // RemoveFromBL
            // 
            this.RemoveFromBL.Enabled = false;
            this.RemoveFromBL.Location = new System.Drawing.Point(373, 272);
            this.RemoveFromBL.Name = "RemoveFromBL";
            this.RemoveFromBL.Size = new System.Drawing.Size(197, 23);
            this.RemoveFromBL.TabIndex = 13;
            this.RemoveFromBL.Text = "Remove Current Target from Blacklist";
            this.RemoveFromBL.UseVisualStyleBackColor = true;
            this.RemoveFromBL.Visible = false;
            this.RemoveFromBL.Click += new System.EventHandler(this.RemoveFromBL_Click);
            // 
            // HomePointTEXT
            // 
            this.HomePointTEXT.AutoSize = true;
            this.HomePointTEXT.Location = new System.Drawing.Point(4, 328);
            this.HomePointTEXT.Name = "HomePointTEXT";
            this.HomePointTEXT.Size = new System.Drawing.Size(116, 13);
            this.HomePointTEXT.TabIndex = 14;
            this.HomePointTEXT.Text = "My Current Home Point";
            // 
            // HomePointSet
            // 
            this.HomePointSet.Location = new System.Drawing.Point(4, 240);
            this.HomePointSet.Name = "HomePointSet";
            this.HomePointSet.Size = new System.Drawing.Size(197, 23);
            this.HomePointSet.TabIndex = 15;
            this.HomePointSet.Text = "Set Current Location As Home Point.";
            this.HomePointSet.UseVisualStyleBackColor = true;
            this.HomePointSet.Click += new System.EventHandler(this.HomePointSet_Click);
            // 
            // EquipAfterBuy
            // 
            this.EquipAfterBuy.AutoSize = true;
            this.EquipAfterBuy.Location = new System.Drawing.Point(3, 125);
            this.EquipAfterBuy.Name = "EquipAfterBuy";
            this.EquipAfterBuy.Size = new System.Drawing.Size(203, 17);
            this.EquipAfterBuy.TabIndex = 16;
            this.EquipAfterBuy.Text = "Equip Major Gear Pieces After Buying";
            this.EquipAfterBuy.UseVisualStyleBackColor = true;
            this.EquipAfterBuy.CheckedChanged += new System.EventHandler(this.EquipAfterBuy_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Tabs);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 324);
            this.tabControl1.TabIndex = 17;
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.groupBox2);
            this.Tabs.Controls.Add(this.groupBox1);
            this.Tabs.Location = new System.Drawing.Point(4, 22);
            this.Tabs.Name = "Tabs";
            this.Tabs.Padding = new System.Windows.Forms.Padding(3);
            this.Tabs.Size = new System.Drawing.Size(576, 298);
            this.Tabs.TabIndex = 0;
            this.Tabs.Text = "Wish List";
            this.Tabs.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RepairSet);
            this.tabPage2.Controls.Add(this.RepairSwap);
            this.tabPage2.Controls.Add(this.LogOutJPCap);
            this.tabPage2.Controls.Add(this.LogOutHPCap);
            this.tabPage2.Controls.Add(this.RemoveFromBL);
            this.tabPage2.Controls.Add(this.MyMount);
            this.tabPage2.Controls.Add(this.BuyOppositePointToBuildUp);
            this.tabPage2.Controls.Add(this.HomePointSet);
            this.tabPage2.Controls.Add(this.EquipAfterBuy);
            this.tabPage2.Controls.Add(this.LogoutAtCap);
            this.tabPage2.Controls.Add(this.RemoveJPHPWhenCapped);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(576, 298);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // RepairSet
            // 
            this.RepairSet.Location = new System.Drawing.Point(4, 269);
            this.RepairSet.Name = "RepairSet";
            this.RepairSet.Size = new System.Drawing.Size(316, 23);
            this.RepairSet.TabIndex = 21;
            this.RepairSet.Text = "Set Currently Targeted Vendor As Our Prefered Repair Vendor.";
            this.RepairSet.UseVisualStyleBackColor = true;
            this.RepairSet.Click += new System.EventHandler(this.RepairSet_Click);
            // 
            // RepairSwap
            // 
            this.RepairSwap.AutoSize = true;
            this.RepairSwap.Location = new System.Drawing.Point(4, 172);
            this.RepairSwap.Name = "RepairSwap";
            this.RepairSwap.Size = new System.Drawing.Size(251, 17);
            this.RepairSwap.TabIndex = 20;
            this.RepairSwap.Text = "Use our repair vendor instead of Honorbuddy\'s?";
            this.RepairSwap.UseVisualStyleBackColor = true;
            this.RepairSwap.CheckedChanged += new System.EventHandler(this.RepairSwap_CheckedChanged);
            // 
            // LogOutJPCap
            // 
            this.LogOutJPCap.AutoSize = true;
            this.LogOutJPCap.Location = new System.Drawing.Point(3, 56);
            this.LogOutJPCap.Name = "LogOutJPCap";
            this.LogOutJPCap.Size = new System.Drawing.Size(218, 17);
            this.LogOutJPCap.TabIndex = 19;
            this.LogOutJPCap.Text = "Logout when Justice Points Are Capped.";
            this.LogOutJPCap.UseVisualStyleBackColor = true;
            this.LogOutJPCap.CheckedChanged += new System.EventHandler(this.LogOutJPCap_CheckedChanged);
            // 
            // LogOutHPCap
            // 
            this.LogOutHPCap.AutoSize = true;
            this.LogOutHPCap.Location = new System.Drawing.Point(3, 32);
            this.LogOutHPCap.Name = "LogOutHPCap";
            this.LogOutHPCap.Size = new System.Drawing.Size(210, 17);
            this.LogOutHPCap.TabIndex = 18;
            this.LogOutHPCap.Text = "Logout when HonorPoints are Capped.";
            this.LogOutHPCap.UseVisualStyleBackColor = true;
            this.LogOutHPCap.CheckedChanged += new System.EventHandler(this.LogOutHPCap_CheckedChanged);
            // 
            // MyMount
            // 
            this.MyMount.AutoSize = true;
            this.MyMount.Location = new System.Drawing.Point(3, 148);
            this.MyMount.Name = "MyMount";
            this.MyMount.Size = new System.Drawing.Size(248, 17);
            this.MyMount.TabIndex = 17;
            this.MyMount.Text = "Use Flying Mount When Moving to Home Point";
            this.MyMount.UseVisualStyleBackColor = true;
            this.MyMount.CheckedChanged += new System.EventHandler(this.MyMount_CheckedChanged);
            // 
            // RepairInfo
            // 
            this.RepairInfo.AutoSize = true;
            this.RepairInfo.Location = new System.Drawing.Point(343, 328);
            this.RepairInfo.Name = "RepairInfo";
            this.RepairInfo.Size = new System.Drawing.Size(240, 13);
            this.RepairInfo.TabIndex = 18;
            this.RepairInfo.Text = "Our Prefered Repair Vendor is A really long name.";
            this.RepairInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MGBConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 345);
            this.Controls.Add(this.RepairInfo);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.HomePointTEXT);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MGBConfig";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mr.GearBuyer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MGBConfig_FormClosing);
            this.Load += new System.EventHandler(this.Hc2ConfigLoad);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVenderList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.Tabs.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddtoBuy;
        private System.Windows.Forms.Button Fetch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button RemoveBuy;
        private System.Windows.Forms.DataGridView dgvBuyList;
        private System.Windows.Forms.DataGridView dgvVenderList;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.CheckBox LogoutAtCap;
        private System.Windows.Forms.CheckBox RemoveJPHPWhenCapped;
        private System.Windows.Forms.CheckBox BuyOppositePointToBuildUp;
        private System.Windows.Forms.Button RemoveFromBL;
        private System.Windows.Forms.Label HomePointTEXT;
        private System.Windows.Forms.Button HomePointSet;
        private System.Windows.Forms.CheckBox EquipAfterBuy;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Tabs;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox LogOutJPCap;
        private System.Windows.Forms.CheckBox LogOutHPCap;
        private System.Windows.Forms.CheckBox MyMount;
        private System.Windows.Forms.CheckBox RepairSwap;
        private System.Windows.Forms.Label RepairInfo;
        private System.Windows.Forms.Button RepairSet;
    }
}