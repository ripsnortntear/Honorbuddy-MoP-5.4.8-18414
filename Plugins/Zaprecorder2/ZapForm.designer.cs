namespace ZapRecorder2
{
    partial class ZapMainForm
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.btnAddHotspot = new System.Windows.Forms.Button();
            this.btnLocRefresh = new System.Windows.Forms.Button();
            this.btnAddBlackspot = new System.Windows.Forms.Button();
            this.btnClearEverything = new System.Windows.Forms.Button();
            this.btnAddMailbox = new System.Windows.Forms.Button();
            this.btnRecording = new System.Windows.Forms.Button();
            this.btnAddRepair = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblMailboxes = new System.Windows.Forms.Label();
            this.lblVendors = new System.Windows.Forms.Label();
            this.lblHotspots = new System.Windows.Forms.Label();
            this.lblBlackspots = new System.Windows.Forms.Label();
            this.tabProfileInfo = new System.Windows.Forms.TabPage();
            this.btnSaveAsDefault = new System.Windows.Forms.Button();
            this.btnLoadDefault = new System.Windows.Forms.Button();
            this.txtFactions = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMaxLevel = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMinLevel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtProfileAuthor = new System.Windows.Forms.TextBox();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnOpenProfile = new System.Windows.Forms.Button();
            this.txtMinFreeBagSlots = new System.Windows.Forms.TextBox();
            this.txtMinDurability = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.minimumDurabilityLabel = new System.Windows.Forms.Label();
            this.sellPurple = new System.Windows.Forms.CheckBox();
            this.sellBlue = new System.Windows.Forms.CheckBox();
            this.sellGreen = new System.Windows.Forms.CheckBox();
            this.sellWhite = new System.Windows.Forms.CheckBox();
            this.sellGrey = new System.Windows.Forms.CheckBox();
            this.mailPurple = new System.Windows.Forms.CheckBox();
            this.mailBlue = new System.Windows.Forms.CheckBox();
            this.mailGreen = new System.Windows.Forms.CheckBox();
            this.mailWhite = new System.Windows.Forms.CheckBox();
            this.mailGrey = new System.Windows.Forms.CheckBox();
            this.tabHotspots = new System.Windows.Forms.TabPage();
            this.btnLoadHotspots = new System.Windows.Forms.Button();
            this.btnReverseHotspots = new System.Windows.Forms.Button();
            this.btnClearSelectedHotspots = new System.Windows.Forms.Button();
            this.btnClearHotspots = new System.Windows.Forms.Button();
            this.listHotspots = new System.Windows.Forms.ListBox();
            this.tabBlackspots = new System.Windows.Forms.TabPage();
            this.btnLoadBlackspots = new System.Windows.Forms.Button();
            this.btnAddBlackspot2 = new System.Windows.Forms.Button();
            this.btnClearSelectedBlackspot = new System.Windows.Forms.Button();
            this.btnClearBlackspots = new System.Windows.Forms.Button();
            this.listBlackspots = new System.Windows.Forms.ListBox();
            this.tabMailboxes = new System.Windows.Forms.TabPage();
            this.btnLoadMailboxes = new System.Windows.Forms.Button();
            this.btnAddMailbox2 = new System.Windows.Forms.Button();
            this.btnClearSelectedMailboxes = new System.Windows.Forms.Button();
            this.btnClearAllMailboxes = new System.Windows.Forms.Button();
            this.listMailboxes = new System.Windows.Forms.ListBox();
            this.tabRepair = new System.Windows.Forms.TabPage();
            this.btnLoadRepair = new System.Windows.Forms.Button();
            this.btnAddRepair2 = new System.Windows.Forms.Button();
            this.btnClearSelectedRepair = new System.Windows.Forms.Button();
            this.btnClearAllRepair = new System.Windows.Forms.Button();
            this.listRepair = new System.Windows.Forms.ListBox();
            this.tabTest = new System.Windows.Forms.TabPage();
            this.btnAddHotspotAtIndex = new System.Windows.Forms.Button();
            this.btnReplaceHotspot = new System.Windows.Forms.Button();
            this.btnDeleteHotspot = new System.Windows.Forms.Button();
            this.btnPrevHotspot = new System.Windows.Forms.Button();
            this.btnGoToHotspot = new System.Windows.Forms.Button();
            this.btnNextHotspot = new System.Windows.Forms.Button();
            this.listTestHotspots = new System.Windows.Forms.ListBox();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.chkChatMessage = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.addIfMovedTextbox = new System.Windows.Forms.TextBox();
            this.recordingTimeTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.onTop = new System.Windows.Forms.CheckBox();
            this.playSound = new System.Windows.Forms.CheckBox();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.helpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabProfileInfo.SuspendLayout();
            this.tabHotspots.SuspendLayout();
            this.tabBlackspots.SuspendLayout();
            this.tabMailboxes.SuspendLayout();
            this.tabRepair.SuspendLayout();
            this.tabTest.SuspendLayout();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabMain);
            this.tabControl1.Controls.Add(this.tabProfileInfo);
            this.tabControl1.Controls.Add(this.tabHotspots);
            this.tabControl1.Controls.Add(this.tabBlackspots);
            this.tabControl1.Controls.Add(this.tabMailboxes);
            this.tabControl1.Controls.Add(this.tabRepair);
            this.tabControl1.Controls.Add(this.tabTest);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(0, -4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(604, 174);
            this.tabControl1.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.btnAddHotspot);
            this.tabMain.Controls.Add(this.btnLocRefresh);
            this.tabMain.Controls.Add(this.btnAddBlackspot);
            this.tabMain.Controls.Add(this.btnClearEverything);
            this.tabMain.Controls.Add(this.btnAddMailbox);
            this.tabMain.Controls.Add(this.btnRecording);
            this.tabMain.Controls.Add(this.btnAddRepair);
            this.tabMain.Controls.Add(this.groupBox1);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(596, 148);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Main";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // btnAddHotspot
            // 
            this.btnAddHotspot.Location = new System.Drawing.Point(152, 18);
            this.btnAddHotspot.Name = "btnAddHotspot";
            this.btnAddHotspot.Size = new System.Drawing.Size(111, 21);
            this.btnAddHotspot.TabIndex = 20;
            this.btnAddHotspot.Text = "Add Single Hotspot";
            this.btnAddHotspot.UseVisualStyleBackColor = true;
            this.btnAddHotspot.Click += new System.EventHandler(this.btnAddHotspot_Click);
            // 
            // btnLocRefresh
            // 
            this.btnLocRefresh.Location = new System.Drawing.Point(285, 124);
            this.btnLocRefresh.Name = "btnLocRefresh";
            this.btnLocRefresh.Size = new System.Drawing.Size(111, 21);
            this.btnLocRefresh.TabIndex = 19;
            this.btnLocRefresh.Text = "Refresh location";
            this.btnLocRefresh.UseVisualStyleBackColor = true;
            this.btnLocRefresh.Click += new System.EventHandler(this.btnLocRefresh_Click);
            // 
            // btnAddBlackspot
            // 
            this.btnAddBlackspot.Location = new System.Drawing.Point(17, 48);
            this.btnAddBlackspot.Name = "btnAddBlackspot";
            this.btnAddBlackspot.Size = new System.Drawing.Size(111, 21);
            this.btnAddBlackspot.TabIndex = 0;
            this.btnAddBlackspot.Text = "Add blackspot";
            this.btnAddBlackspot.UseVisualStyleBackColor = true;
            this.btnAddBlackspot.Click += new System.EventHandler(this.btnAddBlackspot_Click);
            // 
            // btnClearEverything
            // 
            this.btnClearEverything.Location = new System.Drawing.Point(152, 109);
            this.btnClearEverything.Name = "btnClearEverything";
            this.btnClearEverything.Size = new System.Drawing.Size(111, 21);
            this.btnClearEverything.TabIndex = 18;
            this.btnClearEverything.Text = "Clear Everything";
            this.btnClearEverything.UseVisualStyleBackColor = true;
            this.btnClearEverything.Click += new System.EventHandler(this.btnClearEverything_Click);
            // 
            // btnAddMailbox
            // 
            this.btnAddMailbox.Location = new System.Drawing.Point(17, 109);
            this.btnAddMailbox.Name = "btnAddMailbox";
            this.btnAddMailbox.Size = new System.Drawing.Size(111, 21);
            this.btnAddMailbox.TabIndex = 5;
            this.btnAddMailbox.Text = "Add Mailbox";
            this.btnAddMailbox.UseVisualStyleBackColor = true;
            this.btnAddMailbox.Click += new System.EventHandler(this.btnAddMailbox_Click);
            // 
            // btnRecording
            // 
            this.btnRecording.Location = new System.Drawing.Point(17, 18);
            this.btnRecording.Name = "btnRecording";
            this.btnRecording.Size = new System.Drawing.Size(111, 21);
            this.btnRecording.TabIndex = 0;
            this.btnRecording.Text = "Record";
            this.btnRecording.UseVisualStyleBackColor = true;
            this.btnRecording.Click += new System.EventHandler(this.btnRecording_Click);
            // 
            // btnAddRepair
            // 
            this.btnAddRepair.Location = new System.Drawing.Point(17, 78);
            this.btnAddRepair.Name = "btnAddRepair";
            this.btnAddRepair.Size = new System.Drawing.Size(111, 21);
            this.btnAddRepair.TabIndex = 2;
            this.btnAddRepair.Text = "Add Repair NPC";
            this.btnAddRepair.UseVisualStyleBackColor = true;
            this.btnAddRepair.Click += new System.EventHandler(this.btnAddRepair_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblLocation);
            this.groupBox1.Controls.Add(this.lblMailboxes);
            this.groupBox1.Controls.Add(this.lblVendors);
            this.groupBox1.Controls.Add(this.lblHotspots);
            this.groupBox1.Controls.Add(this.lblBlackspots);
            this.groupBox1.Location = new System.Drawing.Point(285, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 101);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profile Info";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Location: ";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(6, 67);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(0, 16);
            this.lblLocation.TabIndex = 4;
            // 
            // lblMailboxes
            // 
            this.lblMailboxes.AutoSize = true;
            this.lblMailboxes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMailboxes.Location = new System.Drawing.Point(130, 33);
            this.lblMailboxes.Name = "lblMailboxes";
            this.lblMailboxes.Size = new System.Drawing.Size(83, 16);
            this.lblMailboxes.TabIndex = 3;
            this.lblMailboxes.Text = "Mailboxes: 0";
            // 
            // lblVendors
            // 
            this.lblVendors.AutoSize = true;
            this.lblVendors.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendors.Location = new System.Drawing.Point(130, 15);
            this.lblVendors.Name = "lblVendors";
            this.lblVendors.Size = new System.Drawing.Size(72, 16);
            this.lblVendors.TabIndex = 2;
            this.lblVendors.Text = "Vendors: 0";
            // 
            // lblHotspots
            // 
            this.lblHotspots.AutoSize = true;
            this.lblHotspots.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHotspots.Location = new System.Drawing.Point(6, 15);
            this.lblHotspots.Name = "lblHotspots";
            this.lblHotspots.Size = new System.Drawing.Size(75, 16);
            this.lblHotspots.TabIndex = 1;
            this.lblHotspots.Text = "Hotspots: 0";
            // 
            // lblBlackspots
            // 
            this.lblBlackspots.AutoSize = true;
            this.lblBlackspots.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlackspots.Location = new System.Drawing.Point(6, 33);
            this.lblBlackspots.Name = "lblBlackspots";
            this.lblBlackspots.Size = new System.Drawing.Size(88, 16);
            this.lblBlackspots.TabIndex = 0;
            this.lblBlackspots.Text = "Blackspots: 0";
            // 
            // tabProfileInfo
            // 
            this.tabProfileInfo.Controls.Add(this.btnSaveAsDefault);
            this.tabProfileInfo.Controls.Add(this.btnLoadDefault);
            this.tabProfileInfo.Controls.Add(this.txtFactions);
            this.tabProfileInfo.Controls.Add(this.label11);
            this.tabProfileInfo.Controls.Add(this.txtMaxLevel);
            this.tabProfileInfo.Controls.Add(this.label10);
            this.tabProfileInfo.Controls.Add(this.txtMinLevel);
            this.tabProfileInfo.Controls.Add(this.label9);
            this.tabProfileInfo.Controls.Add(this.txtProfileAuthor);
            this.tabProfileInfo.Controls.Add(this.txtProfileName);
            this.tabProfileInfo.Controls.Add(this.label8);
            this.tabProfileInfo.Controls.Add(this.label7);
            this.tabProfileInfo.Controls.Add(this.btnSaveProfile);
            this.tabProfileInfo.Controls.Add(this.btnOpenProfile);
            this.tabProfileInfo.Controls.Add(this.txtMinFreeBagSlots);
            this.tabProfileInfo.Controls.Add(this.txtMinDurability);
            this.tabProfileInfo.Controls.Add(this.label3);
            this.tabProfileInfo.Controls.Add(this.minimumDurabilityLabel);
            this.tabProfileInfo.Controls.Add(this.sellPurple);
            this.tabProfileInfo.Controls.Add(this.sellBlue);
            this.tabProfileInfo.Controls.Add(this.sellGreen);
            this.tabProfileInfo.Controls.Add(this.sellWhite);
            this.tabProfileInfo.Controls.Add(this.sellGrey);
            this.tabProfileInfo.Controls.Add(this.mailPurple);
            this.tabProfileInfo.Controls.Add(this.mailBlue);
            this.tabProfileInfo.Controls.Add(this.mailGreen);
            this.tabProfileInfo.Controls.Add(this.mailWhite);
            this.tabProfileInfo.Controls.Add(this.mailGrey);
            this.tabProfileInfo.Location = new System.Drawing.Point(4, 22);
            this.tabProfileInfo.Name = "tabProfileInfo";
            this.tabProfileInfo.Size = new System.Drawing.Size(596, 148);
            this.tabProfileInfo.TabIndex = 2;
            this.tabProfileInfo.Text = "Profile Info";
            this.tabProfileInfo.UseVisualStyleBackColor = true;
            // 
            // btnSaveAsDefault
            // 
            this.btnSaveAsDefault.Location = new System.Drawing.Point(490, 124);
            this.btnSaveAsDefault.Name = "btnSaveAsDefault";
            this.btnSaveAsDefault.Size = new System.Drawing.Size(95, 21);
            this.btnSaveAsDefault.TabIndex = 33;
            this.btnSaveAsDefault.Text = "Save As Default Profile";
            this.btnSaveAsDefault.UseVisualStyleBackColor = true;
            // 
            // btnLoadDefault
            // 
            this.btnLoadDefault.Location = new System.Drawing.Point(389, 124);
            this.btnLoadDefault.Name = "btnLoadDefault";
            this.btnLoadDefault.Size = new System.Drawing.Size(95, 21);
            this.btnLoadDefault.TabIndex = 32;
            this.btnLoadDefault.Text = "Load Default Profile";
            this.btnLoadDefault.UseVisualStyleBackColor = true;
            // 
            // txtFactions
            // 
            this.txtFactions.Location = new System.Drawing.Point(528, 100);
            this.txtFactions.Name = "txtFactions";
            this.txtFactions.Size = new System.Drawing.Size(57, 21);
            this.txtFactions.TabIndex = 31;
            this.txtFactions.Text = "99999";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(432, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 30;
            this.label11.Text = "Factions";
            // 
            // txtMaxLevel
            // 
            this.txtMaxLevel.Location = new System.Drawing.Point(554, 77);
            this.txtMaxLevel.Name = "txtMaxLevel";
            this.txtMaxLevel.Size = new System.Drawing.Size(31, 21);
            this.txtMaxLevel.TabIndex = 29;
            this.txtMaxLevel.Text = "91";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(432, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "Highest Level";
            // 
            // txtMinLevel
            // 
            this.txtMinLevel.Location = new System.Drawing.Point(554, 56);
            this.txtMinLevel.Name = "txtMinLevel";
            this.txtMinLevel.Size = new System.Drawing.Size(31, 21);
            this.txtMinLevel.TabIndex = 27;
            this.txtMinLevel.Text = "1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(432, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "Lowest Level";
            // 
            // txtProfileAuthor
            // 
            this.txtProfileAuthor.Location = new System.Drawing.Point(81, 60);
            this.txtProfileAuthor.Name = "txtProfileAuthor";
            this.txtProfileAuthor.Size = new System.Drawing.Size(182, 21);
            this.txtProfileAuthor.TabIndex = 25;
            // 
            // txtProfileName
            // 
            this.txtProfileName.Location = new System.Drawing.Point(81, 39);
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(182, 21);
            this.txtProfileName.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Profile Author";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "Profile Name:";
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.Location = new System.Drawing.Point(135, 12);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(111, 21);
            this.btnSaveProfile.TabIndex = 21;
            this.btnSaveProfile.Text = "Save Profile";
            this.btnSaveProfile.UseVisualStyleBackColor = true;
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
            // 
            // btnOpenProfile
            // 
            this.btnOpenProfile.Location = new System.Drawing.Point(8, 11);
            this.btnOpenProfile.Name = "btnOpenProfile";
            this.btnOpenProfile.Size = new System.Drawing.Size(111, 21);
            this.btnOpenProfile.TabIndex = 20;
            this.btnOpenProfile.Text = "Open Profile";
            this.btnOpenProfile.UseVisualStyleBackColor = true;
            this.btnOpenProfile.Click += new System.EventHandler(this.btnOpenProfile_Click);
            // 
            // txtMinFreeBagSlots
            // 
            this.txtMinFreeBagSlots.Location = new System.Drawing.Point(554, 34);
            this.txtMinFreeBagSlots.Name = "txtMinFreeBagSlots";
            this.txtMinFreeBagSlots.Size = new System.Drawing.Size(31, 21);
            this.txtMinFreeBagSlots.TabIndex = 18;
            this.txtMinFreeBagSlots.Text = "1";
            // 
            // txtMinDurability
            // 
            this.txtMinDurability.Location = new System.Drawing.Point(554, 13);
            this.txtMinDurability.Name = "txtMinDurability";
            this.txtMinDurability.Size = new System.Drawing.Size(31, 21);
            this.txtMinDurability.TabIndex = 17;
            this.txtMinDurability.Text = "0.4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(432, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Minimum bag slots";
            // 
            // minimumDurabilityLabel
            // 
            this.minimumDurabilityLabel.AutoSize = true;
            this.minimumDurabilityLabel.Location = new System.Drawing.Point(432, 16);
            this.minimumDurabilityLabel.Name = "minimumDurabilityLabel";
            this.minimumDurabilityLabel.Size = new System.Drawing.Size(53, 12);
            this.minimumDurabilityLabel.TabIndex = 13;
            this.minimumDurabilityLabel.Text = "Minimum Durability";
            // 
            // sellPurple
            // 
            this.sellPurple.AutoSize = true;
            this.sellPurple.Location = new System.Drawing.Point(355, 101);
            this.sellPurple.Name = "sellPurple";
            this.sellPurple.Size = new System.Drawing.Size(75, 17);
            this.sellPurple.TabIndex = 9;
            this.sellPurple.Text = "Sell Purples";
            this.sellPurple.UseVisualStyleBackColor = true;
            // 
            // sellBlue
            // 
            this.sellBlue.AutoSize = true;
            this.sellBlue.Location = new System.Drawing.Point(355, 79);
            this.sellBlue.Name = "sellBlue";
            this.sellBlue.Size = new System.Drawing.Size(75, 17);
            this.sellBlue.TabIndex = 8;
            this.sellBlue.Text = "Sell Blues";
            this.sellBlue.UseVisualStyleBackColor = true;
            // 
            // sellGreen
            // 
            this.sellGreen.AutoSize = true;
            this.sellGreen.Location = new System.Drawing.Point(355, 58);
            this.sellGreen.Name = "sellGreen";
            this.sellGreen.Size = new System.Drawing.Size(75, 17);
            this.sellGreen.TabIndex = 7;
            this.sellGreen.Text = "Sell Greens";
            this.sellGreen.UseVisualStyleBackColor = true;
            // 
            // sellWhite
            // 
            this.sellWhite.AutoSize = true;
            this.sellWhite.Checked = true;
            this.sellWhite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sellWhite.Location = new System.Drawing.Point(355, 37);
            this.sellWhite.Name = "sellWhite";
            this.sellWhite.Size = new System.Drawing.Size(75, 17);
            this.sellWhite.TabIndex = 6;
            this.sellWhite.Text = "Sell Whites";
            this.sellWhite.UseVisualStyleBackColor = true;
            // 
            // sellGrey
            // 
            this.sellGrey.AutoSize = true;
            this.sellGrey.Checked = true;
            this.sellGrey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sellGrey.Location = new System.Drawing.Point(355, 16);
            this.sellGrey.Name = "sellGrey";
            this.sellGrey.Size = new System.Drawing.Size(75, 17);
            this.sellGrey.TabIndex = 5;
            this.sellGrey.Text = "Sell Greys";
            this.sellGrey.UseVisualStyleBackColor = true;
            // 
            // mailPurple
            // 
            this.mailPurple.AutoSize = true;
            this.mailPurple.Checked = true;
            this.mailPurple.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mailPurple.Location = new System.Drawing.Point(269, 101);
            this.mailPurple.Name = "mailPurple";
            this.mailPurple.Size = new System.Drawing.Size(75, 17);
            this.mailPurple.TabIndex = 4;
            this.mailPurple.Text = "Mail Purples";
            this.mailPurple.UseVisualStyleBackColor = true;
            // 
            // mailBlue
            // 
            this.mailBlue.AutoSize = true;
            this.mailBlue.Checked = true;
            this.mailBlue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mailBlue.Location = new System.Drawing.Point(269, 79);
            this.mailBlue.Name = "mailBlue";
            this.mailBlue.Size = new System.Drawing.Size(75, 17);
            this.mailBlue.TabIndex = 3;
            this.mailBlue.Text = "Mail Blues";
            this.mailBlue.UseVisualStyleBackColor = true;
            // 
            // mailGreen
            // 
            this.mailGreen.AutoSize = true;
            this.mailGreen.Checked = true;
            this.mailGreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mailGreen.Location = new System.Drawing.Point(269, 58);
            this.mailGreen.Name = "mailGreen";
            this.mailGreen.Size = new System.Drawing.Size(75, 17);
            this.mailGreen.TabIndex = 2;
            this.mailGreen.Text = "Mail Greens";
            this.mailGreen.UseVisualStyleBackColor = true;
            // 
            // mailWhite
            // 
            this.mailWhite.AutoSize = true;
            this.mailWhite.Checked = true;
            this.mailWhite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mailWhite.Location = new System.Drawing.Point(269, 37);
            this.mailWhite.Name = "mailWhite";
            this.mailWhite.Size = new System.Drawing.Size(75, 17);
            this.mailWhite.TabIndex = 1;
            this.mailWhite.Text = "Mail Whites";
            this.mailWhite.UseVisualStyleBackColor = true;
            // 
            // mailGrey
            // 
            this.mailGrey.AutoSize = true;
            this.mailGrey.Location = new System.Drawing.Point(269, 16);
            this.mailGrey.Name = "mailGrey";
            this.mailGrey.Size = new System.Drawing.Size(75, 17);
            this.mailGrey.TabIndex = 0;
            this.mailGrey.Text = "Mail Greys";
            this.mailGrey.UseVisualStyleBackColor = true;
            // 
            // tabHotspots
            // 
            this.tabHotspots.Controls.Add(this.btnLoadHotspots);
            this.tabHotspots.Controls.Add(this.btnReverseHotspots);
            this.tabHotspots.Controls.Add(this.btnClearSelectedHotspots);
            this.tabHotspots.Controls.Add(this.btnClearHotspots);
            this.tabHotspots.Controls.Add(this.listHotspots);
            this.tabHotspots.Location = new System.Drawing.Point(4, 22);
            this.tabHotspots.Name = "tabHotspots";
            this.tabHotspots.Padding = new System.Windows.Forms.Padding(3);
            this.tabHotspots.Size = new System.Drawing.Size(596, 148);
            this.tabHotspots.TabIndex = 3;
            this.tabHotspots.Text = "Hotspots";
            this.tabHotspots.UseVisualStyleBackColor = true;
            // 
            // btnLoadHotspots
            // 
            this.btnLoadHotspots.Location = new System.Drawing.Point(490, 100);
            this.btnLoadHotspots.Name = "btnLoadHotspots";
            this.btnLoadHotspots.Size = new System.Drawing.Size(95, 21);
            this.btnLoadHotspots.TabIndex = 19;
            this.btnLoadHotspots.Text = "Load Hotspots";
            this.helpTooltip.SetToolTip(this.btnLoadHotspots, "Load hotspots from an existing profile");
            this.btnLoadHotspots.UseVisualStyleBackColor = true;
            this.btnLoadHotspots.Click += new System.EventHandler(this.btnLoadHotspots_Click);
            // 
            // btnReverseHotspots
            // 
            this.btnReverseHotspots.Location = new System.Drawing.Point(490, 73);
            this.btnReverseHotspots.Name = "btnReverseHotspots";
            this.btnReverseHotspots.Size = new System.Drawing.Size(95, 21);
            this.btnReverseHotspots.TabIndex = 18;
            this.btnReverseHotspots.Text = "Reverse Hotspots";
            this.btnReverseHotspots.UseVisualStyleBackColor = true;
            this.btnReverseHotspots.Click += new System.EventHandler(this.btnReverseHotspots_Click);
            // 
            // btnClearSelectedHotspots
            // 
            this.btnClearSelectedHotspots.Location = new System.Drawing.Point(490, 19);
            this.btnClearSelectedHotspots.Name = "btnClearSelectedHotspots";
            this.btnClearSelectedHotspots.Size = new System.Drawing.Size(95, 21);
            this.btnClearSelectedHotspots.TabIndex = 17;
            this.btnClearSelectedHotspots.Text = "Clear Selected Hotspots";
            this.btnClearSelectedHotspots.UseVisualStyleBackColor = true;
            this.btnClearSelectedHotspots.Click += new System.EventHandler(this.btnClearSelectedHotspots_Click);
            // 
            // btnClearHotspots
            // 
            this.btnClearHotspots.Location = new System.Drawing.Point(490, 46);
            this.btnClearHotspots.Name = "btnClearHotspots";
            this.btnClearHotspots.Size = new System.Drawing.Size(95, 21);
            this.btnClearHotspots.TabIndex = 15;
            this.btnClearHotspots.Text = "Clear all Hotspots";
            this.btnClearHotspots.UseVisualStyleBackColor = true;
            this.btnClearHotspots.Click += new System.EventHandler(this.btnClearHotspots_Click);
            // 
            // listHotspots
            // 
            this.listHotspots.FormattingEnabled = true;
            this.listHotspots.ItemHeight = 12;
            this.listHotspots.Location = new System.Drawing.Point(12, 19);
            this.listHotspots.Name = "listHotspots";
            this.listHotspots.Size = new System.Drawing.Size(472, 112);
            this.listHotspots.TabIndex = 16;
            // 
            // tabBlackspots
            // 
            this.tabBlackspots.Controls.Add(this.btnLoadBlackspots);
            this.tabBlackspots.Controls.Add(this.btnAddBlackspot2);
            this.tabBlackspots.Controls.Add(this.btnClearSelectedBlackspot);
            this.tabBlackspots.Controls.Add(this.btnClearBlackspots);
            this.tabBlackspots.Controls.Add(this.listBlackspots);
            this.tabBlackspots.Location = new System.Drawing.Point(4, 22);
            this.tabBlackspots.Name = "tabBlackspots";
            this.tabBlackspots.Size = new System.Drawing.Size(596, 148);
            this.tabBlackspots.TabIndex = 4;
            this.tabBlackspots.Text = "Blackspots";
            this.tabBlackspots.UseVisualStyleBackColor = true;
            // 
            // btnLoadBlackspots
            // 
            this.btnLoadBlackspots.Enabled = false;
            this.btnLoadBlackspots.Location = new System.Drawing.Point(490, 100);
            this.btnLoadBlackspots.Name = "btnLoadBlackspots";
            this.btnLoadBlackspots.Size = new System.Drawing.Size(95, 21);
            this.btnLoadBlackspots.TabIndex = 21;
            this.btnLoadBlackspots.Text = "Load Blackspots";
            this.btnLoadBlackspots.UseVisualStyleBackColor = true;
            this.btnLoadBlackspots.Click += new System.EventHandler(this.btnLoadBlackspots_Click);
            // 
            // btnAddBlackspot2
            // 
            this.btnAddBlackspot2.Location = new System.Drawing.Point(490, 19);
            this.btnAddBlackspot2.Name = "btnAddBlackspot2";
            this.btnAddBlackspot2.Size = new System.Drawing.Size(95, 21);
            this.btnAddBlackspot2.TabIndex = 20;
            this.btnAddBlackspot2.Text = "Add Blackspot";
            this.btnAddBlackspot2.UseVisualStyleBackColor = true;
            this.btnAddBlackspot2.Click += new System.EventHandler(this.btnAddBlackspot2_Click);
            // 
            // btnClearSelectedBlackspot
            // 
            this.btnClearSelectedBlackspot.Location = new System.Drawing.Point(490, 46);
            this.btnClearSelectedBlackspot.Name = "btnClearSelectedBlackspot";
            this.btnClearSelectedBlackspot.Size = new System.Drawing.Size(95, 21);
            this.btnClearSelectedBlackspot.TabIndex = 19;
            this.btnClearSelectedBlackspot.Text = "Clear Selected Blackspots";
            this.btnClearSelectedBlackspot.UseVisualStyleBackColor = true;
            this.btnClearSelectedBlackspot.Click += new System.EventHandler(this.btnClearSelectedBlackspot_Click);
            // 
            // btnClearBlackspots
            // 
            this.btnClearBlackspots.Location = new System.Drawing.Point(490, 73);
            this.btnClearBlackspots.Name = "btnClearBlackspots";
            this.btnClearBlackspots.Size = new System.Drawing.Size(95, 21);
            this.btnClearBlackspots.TabIndex = 17;
            this.btnClearBlackspots.Text = "Clear all Blackspots";
            this.btnClearBlackspots.UseVisualStyleBackColor = true;
            this.btnClearBlackspots.Click += new System.EventHandler(this.btnClearBlackspots_Click);
            // 
            // listBlackspots
            // 
            this.listBlackspots.FormattingEnabled = true;
            this.listBlackspots.ItemHeight = 12;
            this.listBlackspots.Location = new System.Drawing.Point(12, 19);
            this.listBlackspots.Name = "listBlackspots";
            this.listBlackspots.Size = new System.Drawing.Size(472, 112);
            this.listBlackspots.TabIndex = 18;
            // 
            // tabMailboxes
            // 
            this.tabMailboxes.Controls.Add(this.btnLoadMailboxes);
            this.tabMailboxes.Controls.Add(this.btnAddMailbox2);
            this.tabMailboxes.Controls.Add(this.btnClearSelectedMailboxes);
            this.tabMailboxes.Controls.Add(this.btnClearAllMailboxes);
            this.tabMailboxes.Controls.Add(this.listMailboxes);
            this.tabMailboxes.Location = new System.Drawing.Point(4, 22);
            this.tabMailboxes.Name = "tabMailboxes";
            this.tabMailboxes.Size = new System.Drawing.Size(596, 148);
            this.tabMailboxes.TabIndex = 5;
            this.tabMailboxes.Text = "Mailboxes";
            this.tabMailboxes.UseVisualStyleBackColor = true;
            // 
            // btnLoadMailboxes
            // 
            this.btnLoadMailboxes.Enabled = false;
            this.btnLoadMailboxes.Location = new System.Drawing.Point(490, 100);
            this.btnLoadMailboxes.Name = "btnLoadMailboxes";
            this.btnLoadMailboxes.Size = new System.Drawing.Size(95, 21);
            this.btnLoadMailboxes.TabIndex = 23;
            this.btnLoadMailboxes.Text = "Load Mailboxes";
            this.btnLoadMailboxes.UseVisualStyleBackColor = true;
            this.btnLoadMailboxes.Click += new System.EventHandler(this.btnLoadMailboxes_Click);
            // 
            // btnAddMailbox2
            // 
            this.btnAddMailbox2.Location = new System.Drawing.Point(490, 19);
            this.btnAddMailbox2.Name = "btnAddMailbox2";
            this.btnAddMailbox2.Size = new System.Drawing.Size(95, 21);
            this.btnAddMailbox2.TabIndex = 22;
            this.btnAddMailbox2.Text = "Add Mailbox";
            this.btnAddMailbox2.UseVisualStyleBackColor = true;
            this.btnAddMailbox2.Click += new System.EventHandler(this.btnAddMailbox2_Click);
            // 
            // btnClearSelectedMailboxes
            // 
            this.btnClearSelectedMailboxes.Location = new System.Drawing.Point(490, 46);
            this.btnClearSelectedMailboxes.Name = "btnClearSelectedMailboxes";
            this.btnClearSelectedMailboxes.Size = new System.Drawing.Size(95, 21);
            this.btnClearSelectedMailboxes.TabIndex = 21;
            this.btnClearSelectedMailboxes.Text = "Clear Selected Mailboxes";
            this.btnClearSelectedMailboxes.UseVisualStyleBackColor = true;
            this.btnClearSelectedMailboxes.Click += new System.EventHandler(this.btnClearSelectedMailboxes_Click);
            // 
            // btnClearAllMailboxes
            // 
            this.btnClearAllMailboxes.Location = new System.Drawing.Point(490, 73);
            this.btnClearAllMailboxes.Name = "btnClearAllMailboxes";
            this.btnClearAllMailboxes.Size = new System.Drawing.Size(95, 21);
            this.btnClearAllMailboxes.TabIndex = 19;
            this.btnClearAllMailboxes.Text = "Clear all Mailboxes";
            this.btnClearAllMailboxes.UseVisualStyleBackColor = true;
            this.btnClearAllMailboxes.Click += new System.EventHandler(this.btnClearAllMailboxes_Click);
            // 
            // listMailboxes
            // 
            this.listMailboxes.FormattingEnabled = true;
            this.listMailboxes.ItemHeight = 12;
            this.listMailboxes.Location = new System.Drawing.Point(12, 19);
            this.listMailboxes.Name = "listMailboxes";
            this.listMailboxes.Size = new System.Drawing.Size(472, 112);
            this.listMailboxes.TabIndex = 20;
            // 
            // tabRepair
            // 
            this.tabRepair.Controls.Add(this.btnLoadRepair);
            this.tabRepair.Controls.Add(this.btnAddRepair2);
            this.tabRepair.Controls.Add(this.btnClearSelectedRepair);
            this.tabRepair.Controls.Add(this.btnClearAllRepair);
            this.tabRepair.Controls.Add(this.listRepair);
            this.tabRepair.Location = new System.Drawing.Point(4, 22);
            this.tabRepair.Name = "tabRepair";
            this.tabRepair.Size = new System.Drawing.Size(596, 148);
            this.tabRepair.TabIndex = 6;
            this.tabRepair.Text = "Repair";
            this.tabRepair.UseVisualStyleBackColor = true;
            // 
            // btnLoadRepair
            // 
            this.btnLoadRepair.Enabled = false;
            this.btnLoadRepair.Location = new System.Drawing.Point(490, 100);
            this.btnLoadRepair.Name = "btnLoadRepair";
            this.btnLoadRepair.Size = new System.Drawing.Size(95, 21);
            this.btnLoadRepair.TabIndex = 22;
            this.btnLoadRepair.Text = "Load Repaid NPC";
            this.btnLoadRepair.UseVisualStyleBackColor = true;
            this.btnLoadRepair.Click += new System.EventHandler(this.btnLoadRepair_Click);
            // 
            // btnAddRepair2
            // 
            this.btnAddRepair2.Location = new System.Drawing.Point(490, 19);
            this.btnAddRepair2.Name = "btnAddRepair2";
            this.btnAddRepair2.Size = new System.Drawing.Size(95, 21);
            this.btnAddRepair2.TabIndex = 21;
            this.btnAddRepair2.Text = "Add Repair NPC";
            this.btnAddRepair2.UseVisualStyleBackColor = true;
            this.btnAddRepair2.Click += new System.EventHandler(this.btnAddRepair2_Click);
            // 
            // btnClearSelectedRepair
            // 
            this.btnClearSelectedRepair.Location = new System.Drawing.Point(490, 46);
            this.btnClearSelectedRepair.Name = "btnClearSelectedRepair";
            this.btnClearSelectedRepair.Size = new System.Drawing.Size(95, 21);
            this.btnClearSelectedRepair.TabIndex = 20;
            this.btnClearSelectedRepair.Text = "Clear Repair NPC";
            this.btnClearSelectedRepair.UseVisualStyleBackColor = true;
            this.btnClearSelectedRepair.Click += new System.EventHandler(this.btnClearSelectedRepair_Click);
            // 
            // btnClearAllRepair
            // 
            this.btnClearAllRepair.Location = new System.Drawing.Point(490, 73);
            this.btnClearAllRepair.Name = "btnClearAllRepair";
            this.btnClearAllRepair.Size = new System.Drawing.Size(95, 21);
            this.btnClearAllRepair.TabIndex = 18;
            this.btnClearAllRepair.Text = "Clear all Repair NPC";
            this.btnClearAllRepair.UseVisualStyleBackColor = true;
            this.btnClearAllRepair.Click += new System.EventHandler(this.btnClearAllRepair_Click);
            // 
            // listRepair
            // 
            this.listRepair.FormattingEnabled = true;
            this.listRepair.ItemHeight = 12;
            this.listRepair.Location = new System.Drawing.Point(12, 19);
            this.listRepair.Name = "listRepair";
            this.listRepair.Size = new System.Drawing.Size(472, 112);
            this.listRepair.TabIndex = 19;
            // 
            // tabTest
            // 
            this.tabTest.Controls.Add(this.btnAddHotspotAtIndex);
            this.tabTest.Controls.Add(this.btnReplaceHotspot);
            this.tabTest.Controls.Add(this.btnDeleteHotspot);
            this.tabTest.Controls.Add(this.btnPrevHotspot);
            this.tabTest.Controls.Add(this.btnGoToHotspot);
            this.tabTest.Controls.Add(this.btnNextHotspot);
            this.tabTest.Controls.Add(this.listTestHotspots);
            this.tabTest.Location = new System.Drawing.Point(4, 22);
            this.tabTest.Name = "tabTest";
            this.tabTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabTest.Size = new System.Drawing.Size(596, 148);
            this.tabTest.TabIndex = 7;
            this.tabTest.Text = "Test Profile";
            this.tabTest.UseVisualStyleBackColor = true;
            // 
            // btnAddHotspotAtIndex
            // 
            this.btnAddHotspotAtIndex.Enabled = false;
            this.btnAddHotspotAtIndex.Location = new System.Drawing.Point(490, 96);
            this.btnAddHotspotAtIndex.Name = "btnAddHotspotAtIndex";
            this.btnAddHotspotAtIndex.Size = new System.Drawing.Size(95, 21);
            this.btnAddHotspotAtIndex.TabIndex = 25;
            this.btnAddHotspotAtIndex.Text = "Add Hotspot";
            this.btnAddHotspotAtIndex.UseVisualStyleBackColor = true;
            this.btnAddHotspotAtIndex.Click += new System.EventHandler(this.btnAddHotspotAtIndex_Click);
            // 
            // btnReplaceHotspot
            // 
            this.btnReplaceHotspot.Location = new System.Drawing.Point(490, 70);
            this.btnReplaceHotspot.Name = "btnReplaceHotspot";
            this.btnReplaceHotspot.Size = new System.Drawing.Size(95, 21);
            this.btnReplaceHotspot.TabIndex = 24;
            this.btnReplaceHotspot.Text = "Replace Hotspot";
            this.btnReplaceHotspot.UseVisualStyleBackColor = true;
            this.btnReplaceHotspot.Click += new System.EventHandler(this.btnReplaceHotspot_Click);
            // 
            // btnDeleteHotspot
            // 
            this.btnDeleteHotspot.Location = new System.Drawing.Point(490, 122);
            this.btnDeleteHotspot.Name = "btnDeleteHotspot";
            this.btnDeleteHotspot.Size = new System.Drawing.Size(95, 21);
            this.btnDeleteHotspot.TabIndex = 23;
            this.btnDeleteHotspot.Text = "Delete Hotspot";
            this.btnDeleteHotspot.UseVisualStyleBackColor = true;
            this.btnDeleteHotspot.Click += new System.EventHandler(this.btnDeleteHotspot_Click);
            // 
            // btnPrevHotspot
            // 
            this.btnPrevHotspot.Location = new System.Drawing.Point(490, 43);
            this.btnPrevHotspot.Name = "btnPrevHotspot";
            this.btnPrevHotspot.Size = new System.Drawing.Size(42, 21);
            this.btnPrevHotspot.TabIndex = 22;
            this.btnPrevHotspot.Text = "Last Hotspot";
            this.btnPrevHotspot.UseVisualStyleBackColor = true;
            this.btnPrevHotspot.Click += new System.EventHandler(this.btnPrevHotspot_Click);
            // 
            // btnGoToHotspot
            // 
            this.btnGoToHotspot.Location = new System.Drawing.Point(490, 19);
            this.btnGoToHotspot.Name = "btnGoToHotspot";
            this.btnGoToHotspot.Size = new System.Drawing.Size(95, 21);
            this.btnGoToHotspot.TabIndex = 21;
            this.btnGoToHotspot.Text = "GoTo Hotspot";
            this.btnGoToHotspot.UseVisualStyleBackColor = true;
            this.btnGoToHotspot.Click += new System.EventHandler(this.btnGoToHotspot_Click);
            // 
            // btnNextHotspot
            // 
            this.btnNextHotspot.Location = new System.Drawing.Point(543, 43);
            this.btnNextHotspot.Name = "btnNextHotspot";
            this.btnNextHotspot.Size = new System.Drawing.Size(42, 21);
            this.btnNextHotspot.TabIndex = 20;
            this.btnNextHotspot.Text = "Next Hotspot";
            this.btnNextHotspot.UseVisualStyleBackColor = true;
            this.btnNextHotspot.Click += new System.EventHandler(this.btnNextHotspot_Click);
            // 
            // listTestHotspots
            // 
            this.listTestHotspots.FormattingEnabled = true;
            this.listTestHotspots.ItemHeight = 12;
            this.listTestHotspots.Location = new System.Drawing.Point(12, 19);
            this.listTestHotspots.Name = "listTestHotspots";
            this.listTestHotspots.Size = new System.Drawing.Size(472, 112);
            this.listTestHotspots.TabIndex = 17;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.chkChatMessage);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.numericUpDown1);
            this.tabSettings.Controls.Add(this.label6);
            this.tabSettings.Controls.Add(this.addIfMovedTextbox);
            this.tabSettings.Controls.Add(this.recordingTimeTextbox);
            this.tabSettings.Controls.Add(this.label5);
            this.tabSettings.Controls.Add(this.label4);
            this.tabSettings.Controls.Add(this.onTop);
            this.tabSettings.Controls.Add(this.playSound);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(596, 148);
            this.tabSettings.TabIndex = 8;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // chkChatMessage
            // 
            this.chkChatMessage.AutoSize = true;
            this.chkChatMessage.Checked = true;
            this.chkChatMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChatMessage.Location = new System.Drawing.Point(249, 67);
            this.chkChatMessage.Name = "chkChatMessage";
            this.chkChatMessage.Size = new System.Drawing.Size(75, 17);
            this.chkChatMessage.TabIndex = 31;
            this.chkChatMessage.Text = "Chat Messages";
            this.chkChatMessage.UseVisualStyleBackColor = true;
            this.chkChatMessage.CheckedChanged += new System.EventHandler(this.chkChatMessage_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(450, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "Close windows when saving is complete";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(130, 56);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown1.TabIndex = 29;
            this.numericUpDown1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "Blackspot Radius";
            // 
            // addIfMovedTextbox
            // 
            this.addIfMovedTextbox.Location = new System.Drawing.Point(130, 33);
            this.addIfMovedTextbox.Name = "addIfMovedTextbox";
            this.addIfMovedTextbox.Size = new System.Drawing.Size(100, 21);
            this.addIfMovedTextbox.TabIndex = 27;
            this.addIfMovedTextbox.Text = "30";
            // 
            // recordingTimeTextbox
            // 
            this.recordingTimeTextbox.Location = new System.Drawing.Point(130, 12);
            this.recordingTimeTextbox.Name = "recordingTimeTextbox";
            this.recordingTimeTextbox.Size = new System.Drawing.Size(100, 21);
            this.recordingTimeTextbox.TabIndex = 26;
            this.recordingTimeTextbox.Text = "2000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 25;
            this.label5.Text = "添加位移";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "记录暂离时间";
            // 
            // onTop
            // 
            this.onTop.AutoSize = true;
            this.onTop.Checked = true;
            this.onTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onTop.Location = new System.Drawing.Point(105, 96);
            this.onTop.Name = "onTop";
            this.onTop.Size = new System.Drawing.Size(51, 17);
            this.onTop.TabIndex = 13;
            this.onTop.Text = "Attack";
            this.onTop.UseVisualStyleBackColor = true;
            this.onTop.CheckedChanged += new System.EventHandler(this.onTop_CheckedChanged);
            // 
            // playSound
            // 
            this.playSound.AutoSize = true;
            this.playSound.Checked = true;
            this.playSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSound.Location = new System.Drawing.Point(19, 96);
            this.playSound.Name = "playSound";
            this.playSound.Size = new System.Drawing.Size(75, 17);
            this.playSound.TabIndex = 12;
            this.playSound.Text = "Play Sound";
            this.playSound.UseVisualStyleBackColor = true;
            // 
            // dlgSaveFile
            // 
            this.dlgSaveFile.DefaultExt = "xml";
            this.dlgSaveFile.Filter = "XML Profiles|*.xml";
            this.dlgSaveFile.Title = "Save a Profile";
            // 
            // txtLog
            // 
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Location = new System.Drawing.Point(0, 176);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(604, 154);
            this.txtLog.TabIndex = 20;
            this.txtLog.Text = "";
            // 
            // ZapMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 330);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ZapMainForm";
            this.Text = "ZapRecorder2";
            this.tabControl1.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabProfileInfo.ResumeLayout(false);
            this.tabProfileInfo.PerformLayout();
            this.tabHotspots.ResumeLayout(false);
            this.tabBlackspots.ResumeLayout(false);
            this.tabMailboxes.ResumeLayout(false);
            this.tabRepair.ResumeLayout(false);
            this.tabTest.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabProfileInfo;
        private System.Windows.Forms.CheckBox sellPurple;
        private System.Windows.Forms.CheckBox sellBlue;
        private System.Windows.Forms.CheckBox sellGreen;
        private System.Windows.Forms.CheckBox sellWhite;
        private System.Windows.Forms.CheckBox sellGrey;
        private System.Windows.Forms.CheckBox mailPurple;
        private System.Windows.Forms.CheckBox mailBlue;
        private System.Windows.Forms.CheckBox mailGreen;
        private System.Windows.Forms.CheckBox mailWhite;
        private System.Windows.Forms.CheckBox mailGrey;
        private System.Windows.Forms.TextBox txtMinFreeBagSlots;
        private System.Windows.Forms.TextBox txtMinDurability;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label minimumDurabilityLabel;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Button btnAddBlackspot;
        private System.Windows.Forms.Button btnClearEverything;
        private System.Windows.Forms.Button btnAddMailbox;
        private System.Windows.Forms.Button btnRecording;
        private System.Windows.Forms.Button btnAddRepair;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblMailboxes;
        private System.Windows.Forms.Label lblVendors;
        private System.Windows.Forms.Label lblHotspots;
        private System.Windows.Forms.Label lblBlackspots;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.TabPage tabHotspots;
        private System.Windows.Forms.TabPage tabBlackspots;
        private System.Windows.Forms.TabPage tabMailboxes;
        private System.Windows.Forms.Button btnReverseHotspots;
        private System.Windows.Forms.Button btnClearSelectedHotspots;
        private System.Windows.Forms.Button btnClearHotspots;
        private System.Windows.Forms.ListBox listHotspots;
        private System.Windows.Forms.Button btnLoadBlackspots;
        private System.Windows.Forms.Button btnAddBlackspot2;
        private System.Windows.Forms.Button btnClearSelectedBlackspot;
        private System.Windows.Forms.Button btnClearBlackspots;
        private System.Windows.Forms.ListBox listBlackspots;
        private System.Windows.Forms.Button btnLoadMailboxes;
        private System.Windows.Forms.Button btnAddMailbox2;
        private System.Windows.Forms.Button btnClearSelectedMailboxes;
        private System.Windows.Forms.Button btnClearAllMailboxes;
        private System.Windows.Forms.ListBox listMailboxes;
        private System.Windows.Forms.TabPage tabRepair;
        private System.Windows.Forms.Button btnLoadRepair;
        private System.Windows.Forms.Button btnAddRepair2;
        private System.Windows.Forms.Button btnClearSelectedRepair;
        private System.Windows.Forms.Button btnClearAllRepair;
        private System.Windows.Forms.ListBox listRepair;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnLocRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddHotspot;
        private System.Windows.Forms.TabPage tabTest;
        private System.Windows.Forms.ListBox listTestHotspots;
        private System.Windows.Forms.Button btnReplaceHotspot;
        private System.Windows.Forms.Button btnPrevHotspot;
        private System.Windows.Forms.Button btnGoToHotspot;
        private System.Windows.Forms.Button btnNextHotspot;
        private System.Windows.Forms.ToolTip helpTooltip;
        private System.Windows.Forms.Button btnLoadHotspots;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox addIfMovedTextbox;
        private System.Windows.Forms.TextBox recordingTimeTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox onTop;
        private System.Windows.Forms.CheckBox playSound;
        private System.Windows.Forms.TextBox txtProfileAuthor;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnOpenProfile;
        private System.Windows.Forms.Button btnAddHotspotAtIndex;
        private System.Windows.Forms.Button btnDeleteHotspot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMinLevel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkChatMessage;
        private System.Windows.Forms.TextBox txtMaxLevel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFactions;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSaveAsDefault;
        private System.Windows.Forms.Button btnLoadDefault;
    }
}

