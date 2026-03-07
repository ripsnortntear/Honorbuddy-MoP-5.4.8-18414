namespace FuryUnleashed.Interfaces.GUI
{
    partial class Interface
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
            this.GeneralGrid = new System.Windows.Forms.PropertyGrid();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripText = new System.Windows.Forms.ToolStripStatusLabel();
            this.LogoPicture = new System.Windows.Forms.PictureBox();
            this.debuggerpanel = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SpecGrid = new System.Windows.Forms.PropertyGrid();
            this.ComboHkDemoralizingBanner = new System.Windows.Forms.ComboBox();
            this.HkDemoBannerLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboHkHeroicLeap = new System.Windows.Forms.ComboBox();
            this.HkHeroicLeapLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.HkModeLabel = new System.Windows.Forms.Label();
            this.ComboHkMode = new System.Windows.Forms.ComboBox();
            this.ComboHkModifier = new System.Windows.Forms.ComboBox();
            this.ComboHkCooldown = new System.Windows.Forms.ComboBox();
            this.ComboHkMultiTgt = new System.Windows.Forms.ComboBox();
            this.ComboHkPause = new System.Windows.Forms.ComboBox();
            this.HkModifierLabel = new System.Windows.Forms.Label();
            this.HkCooldownLabel = new System.Windows.Forms.Label();
            this.HkAoELabel = new System.Windows.Forms.Label();
            this.HkPauseLabel = new System.Windows.Forms.Label();
            this.ComboHkShatteringThrow = new System.Windows.Forms.ComboBox();
            this.HkShattThrowLabel = new System.Windows.Forms.Label();
            this.ComboHkTier4Ability = new System.Windows.Forms.ComboBox();
            this.HkTier4AbilityLabel = new System.Windows.Forms.Label();
            this.ComboHkSpecial = new System.Windows.Forms.ComboBox();
            this.HkSpecialLabel = new System.Windows.Forms.Label();
            this.ComboHkMockingBanner = new System.Windows.Forms.ComboBox();
            this.HkMockingBannerLabel = new System.Windows.Forms.Label();
            this.HotkeyPanel = new System.Windows.Forms.Panel();
            this.BottomWhitePanel = new System.Windows.Forms.Panel();
            this.StatusStrip.SuspendLayout();
            this.BottomWhitePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPicture)).BeginInit();
            this.HotkeyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GeneralGrid
            // 
            this.GeneralGrid.CommandsBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.GeneralGrid.CommandsLinkColor = System.Drawing.Color.Black;
            this.GeneralGrid.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.GeneralGrid.HelpBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.GeneralGrid.Location = new System.Drawing.Point(8, 135);
            this.GeneralGrid.Name = "GeneralGrid";
            this.GeneralGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.GeneralGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.GeneralGrid.SelectedItemWithFocusForeColor = System.Drawing.Color.Black;
            this.GeneralGrid.Size = new System.Drawing.Size(448, 515);
            this.GeneralGrid.TabIndex = 1;
            this.GeneralGrid.ToolbarVisible = false;
            this.GeneralGrid.ViewBackColor = System.Drawing.Color.White;
            this.GeneralGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            // 
            // StatusStrip
            // 
            this.StatusStrip.AutoSize = false;
            this.StatusStrip.BackColor = System.Drawing.Color.White;
            this.StatusStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.StatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.StatusStrip.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripText});
            this.StatusStrip.Location = new System.Drawing.Point(8, 738);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Padding = new System.Windows.Forms.Padding(10, 6, 28, 8);
            this.StatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.StatusStrip.Size = new System.Drawing.Size(1572, 48);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 2;
            this.StatusStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Fu_GuiDragDrop);
            // 
            // StatusStripText
            // 
            this.StatusStripText.Font = new System.Drawing.Font("Century Gothic", 10.5F);
            this.StatusStripText.Name = "StatusStripText";
            this.StatusStripText.Size = new System.Drawing.Size(1557, 18);
            this.StatusStripText.Spring = true;
            this.StatusStripText.Text = "Fury Unleashed - The best you can get!";
            // 
            // LogoPicture
            // 
            this.LogoPicture.BackColor = System.Drawing.Color.White;
            this.LogoPicture.Location = new System.Drawing.Point(8, 10);
            this.LogoPicture.Name = "LogoPicture";
            this.LogoPicture.Size = new System.Drawing.Size(1572, 120);
            this.LogoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LogoPicture.TabIndex = 0;
            this.LogoPicture.TabStop = false;
            this.LogoPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Fu_GuiDragDrop);
            this.LogoPicture.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.LogoPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LogoPicture_MouseMove);
            // 
            // debuggerpanel
            // 
            this.debuggerpanel.BackColor = System.Drawing.Color.White;
            this.debuggerpanel.Location = new System.Drawing.Point(936, 652);
            this.debuggerpanel.Name = "debuggerpanel";
            this.debuggerpanel.Size = new System.Drawing.Size(632, 28);
            this.debuggerpanel.TabIndex = 5;
            this.debuggerpanel.Click += new System.EventHandler(this.debuggerpanel_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Font = new System.Drawing.Font("Century Gothic", 10.5F);
            this.SaveButton.Location = new System.Drawing.Point(8, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(624, 42);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save and Close";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            this.SaveButton.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.SaveButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SaveButton_MouseMove);
            // 
            // SpecGrid
            // 
            this.SpecGrid.CommandsBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.SpecGrid.CommandsLinkColor = System.Drawing.Color.Black;
            this.SpecGrid.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.SpecGrid.HelpBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.SpecGrid.Location = new System.Drawing.Point(464, 135);
            this.SpecGrid.Name = "SpecGrid";
            this.SpecGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.SpecGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.SpecGrid.SelectedItemWithFocusForeColor = System.Drawing.Color.Black;
            this.SpecGrid.Size = new System.Drawing.Size(448, 515);
            this.SpecGrid.TabIndex = 4;
            this.SpecGrid.ToolbarVisible = false;
            this.SpecGrid.ViewBackColor = System.Drawing.Color.White;
            this.SpecGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            // 
            // ComboHkDemoralizingBanner
            // 
            this.ComboHkDemoralizingBanner.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkDemoralizingBanner.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkDemoralizingBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkDemoralizingBanner.DropDownHeight = 110;
            this.ComboHkDemoralizingBanner.DropDownWidth = 200;
            this.ComboHkDemoralizingBanner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkDemoralizingBanner.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboHkDemoralizingBanner.FormattingEnabled = true;
            this.ComboHkDemoralizingBanner.IntegralHeight = false;
            this.ComboHkDemoralizingBanner.Location = new System.Drawing.Point(300, 48);
            this.ComboHkDemoralizingBanner.Name = "ComboHkDemoralizingBanner";
            this.ComboHkDemoralizingBanner.Size = new System.Drawing.Size(328, 26);
            this.ComboHkDemoralizingBanner.TabIndex = 0;
            this.ComboHkDemoralizingBanner.SelectedIndexChanged += new System.EventHandler(this.ComboHkDemoralizingBanner_SelectedIndexChanged);
            this.ComboHkDemoralizingBanner.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkDemoralizingBanner.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkDemoralizingBanner_MouseMove);
            // 
            // HkDemoBannerLabel
            // 
            this.HkDemoBannerLabel.AutoSize = true;
            this.HkDemoBannerLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkDemoBannerLabel.Location = new System.Drawing.Point(12, 50);
            this.HkDemoBannerLabel.Name = "HkDemoBannerLabel";
            this.HkDemoBannerLabel.Size = new System.Drawing.Size(141, 17);
            this.HkDemoBannerLabel.TabIndex = 4;
            this.HkDemoBannerLabel.Text = "Demoralizing Banner";
            this.HkDemoBannerLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkDemoBannerLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkDemoralizingBanner_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(568, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Hotkey Options - Casting Type";
            // 
            // ComboHkHeroicLeap
            // 
            this.ComboHkHeroicLeap.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkHeroicLeap.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkHeroicLeap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkHeroicLeap.DropDownHeight = 110;
            this.ComboHkHeroicLeap.DropDownWidth = 200;
            this.ComboHkHeroicLeap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkHeroicLeap.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkHeroicLeap.FormattingEnabled = true;
            this.ComboHkHeroicLeap.IntegralHeight = false;
            this.ComboHkHeroicLeap.Location = new System.Drawing.Point(300, 88);
            this.ComboHkHeroicLeap.Name = "ComboHkHeroicLeap";
            this.ComboHkHeroicLeap.Size = new System.Drawing.Size(328, 26);
            this.ComboHkHeroicLeap.TabIndex = 6;
            this.ComboHkHeroicLeap.SelectedIndexChanged += new System.EventHandler(this.ComboHkHeroicLeap_SelectedIndexChanged);
            this.ComboHkHeroicLeap.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkHeroicLeap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkHeroicLeap_MouseMove);
            // 
            // HkHeroicLeapLabel
            // 
            this.HkHeroicLeapLabel.AutoSize = true;
            this.HkHeroicLeapLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkHeroicLeapLabel.Location = new System.Drawing.Point(12, 90);
            this.HkHeroicLeapLabel.Name = "HkHeroicLeapLabel";
            this.HkHeroicLeapLabel.Size = new System.Drawing.Size(85, 17);
            this.HkHeroicLeapLabel.TabIndex = 7;
            this.HkHeroicLeapLabel.Text = "Heroic Leap";
            this.HkHeroicLeapLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkHeroicLeapLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkHeroicLeap_MouseMove);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 246);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(568, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Hotkey Options - Toggle Type";
            // 
            // HkModeLabel
            // 
            this.HkModeLabel.AutoSize = true;
            this.HkModeLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkModeLabel.Location = new System.Drawing.Point(12, 280);
            this.HkModeLabel.Name = "HkModeLabel";
            this.HkModeLabel.Size = new System.Drawing.Size(135, 17);
            this.HkModeLabel.TabIndex = 9;
            this.HkModeLabel.Text = "Select Hotkey Mode";
            this.HkModeLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkModeLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkMode_MouseMove);
            // 
            // ComboHkMode
            // 
            this.ComboHkMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkMode.DropDownHeight = 110;
            this.ComboHkMode.DropDownWidth = 200;
            this.ComboHkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkMode.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkMode.FormattingEnabled = true;
            this.ComboHkMode.IntegralHeight = false;
            this.ComboHkMode.Location = new System.Drawing.Point(300, 278);
            this.ComboHkMode.Name = "ComboHkMode";
            this.ComboHkMode.Size = new System.Drawing.Size(328, 26);
            this.ComboHkMode.TabIndex = 10;
            this.ComboHkMode.SelectedIndexChanged += new System.EventHandler(this.ComboHkMode_SelectedIndexChanged);
            this.ComboHkMode.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkMode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkMode_MouseMove);
            // 
            // ComboHkModifier
            // 
            this.ComboHkModifier.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkModifier.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkModifier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkModifier.DropDownHeight = 110;
            this.ComboHkModifier.DropDownWidth = 200;
            this.ComboHkModifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkModifier.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkModifier.FormattingEnabled = true;
            this.ComboHkModifier.IntegralHeight = false;
            this.ComboHkModifier.Location = new System.Drawing.Point(300, 318);
            this.ComboHkModifier.Name = "ComboHkModifier";
            this.ComboHkModifier.Size = new System.Drawing.Size(328, 26);
            this.ComboHkModifier.TabIndex = 11;
            this.ComboHkModifier.SelectedIndexChanged += new System.EventHandler(this.ComboHkModifier_SelectedIndexChanged);
            this.ComboHkModifier.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkModifier.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkModifier_MouseMove);
            // 
            // ComboHkCooldown
            // 
            this.ComboHkCooldown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkCooldown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkCooldown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkCooldown.DropDownHeight = 110;
            this.ComboHkCooldown.DropDownWidth = 200;
            this.ComboHkCooldown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkCooldown.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkCooldown.FormattingEnabled = true;
            this.ComboHkCooldown.IntegralHeight = false;
            this.ComboHkCooldown.Location = new System.Drawing.Point(300, 358);
            this.ComboHkCooldown.Name = "ComboHkCooldown";
            this.ComboHkCooldown.Size = new System.Drawing.Size(328, 26);
            this.ComboHkCooldown.TabIndex = 12;
            this.ComboHkCooldown.SelectedIndexChanged += new System.EventHandler(this.ComboHkCooldown_SelectedIndexChanged);
            this.ComboHkCooldown.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkCooldown.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkCooldown_MouseMove);
            // 
            // ComboHkMultiTgt
            // 
            this.ComboHkMultiTgt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkMultiTgt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkMultiTgt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkMultiTgt.DropDownHeight = 110;
            this.ComboHkMultiTgt.DropDownWidth = 200;
            this.ComboHkMultiTgt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkMultiTgt.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkMultiTgt.FormattingEnabled = true;
            this.ComboHkMultiTgt.IntegralHeight = false;
            this.ComboHkMultiTgt.Location = new System.Drawing.Point(300, 398);
            this.ComboHkMultiTgt.Name = "ComboHkMultiTgt";
            this.ComboHkMultiTgt.Size = new System.Drawing.Size(328, 26);
            this.ComboHkMultiTgt.TabIndex = 13;
            this.ComboHkMultiTgt.SelectedIndexChanged += new System.EventHandler(this.ComboHkMultiTgt_SelectedIndexChanged);
            this.ComboHkMultiTgt.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkMultiTgt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkMultiTgt_MouseMove);
            // 
            // ComboHkPause
            // 
            this.ComboHkPause.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkPause.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkPause.DropDownHeight = 110;
            this.ComboHkPause.DropDownWidth = 200;
            this.ComboHkPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkPause.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkPause.FormattingEnabled = true;
            this.ComboHkPause.IntegralHeight = false;
            this.ComboHkPause.Location = new System.Drawing.Point(300, 438);
            this.ComboHkPause.Name = "ComboHkPause";
            this.ComboHkPause.Size = new System.Drawing.Size(328, 26);
            this.ComboHkPause.TabIndex = 14;
            this.ComboHkPause.SelectedIndexChanged += new System.EventHandler(this.ComboHkPause_SelectedIndexChanged);
            this.ComboHkPause.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkPause.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkPause_MouseMove);
            // 
            // HkModifierLabel
            // 
            this.HkModifierLabel.AutoSize = true;
            this.HkModifierLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkModifierLabel.Location = new System.Drawing.Point(12, 320);
            this.HkModifierLabel.Name = "HkModifierLabel";
            this.HkModifierLabel.Size = new System.Drawing.Size(127, 17);
            this.HkModifierLabel.TabIndex = 15;
            this.HkModifierLabel.Text = "Select Modifier Key";
            this.HkModifierLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkModifierLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkModifier_MouseMove);
            // 
            // HkCooldownLabel
            // 
            this.HkCooldownLabel.AutoSize = true;
            this.HkCooldownLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkCooldownLabel.Location = new System.Drawing.Point(12, 360);
            this.HkCooldownLabel.Name = "HkCooldownLabel";
            this.HkCooldownLabel.Size = new System.Drawing.Size(146, 17);
            this.HkCooldownLabel.TabIndex = 16;
            this.HkCooldownLabel.Text = "Select Cooldown Key";
            this.HkCooldownLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkCooldownLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkCooldown_MouseMove);
            // 
            // HkAoELabel
            // 
            this.HkAoELabel.AutoSize = true;
            this.HkAoELabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkAoELabel.Location = new System.Drawing.Point(12, 400);
            this.HkAoELabel.Name = "HkAoELabel";
            this.HkAoELabel.Size = new System.Drawing.Size(169, 17);
            this.HkAoELabel.TabIndex = 17;
            this.HkAoELabel.Text = "Select Multi-TGT Key (AoE)";
            this.HkAoELabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkAoELabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkMultiTgt_MouseMove);
            // 
            // HkPauseLabel
            // 
            this.HkPauseLabel.AutoSize = true;
            this.HkPauseLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkPauseLabel.Location = new System.Drawing.Point(12, 440);
            this.HkPauseLabel.Name = "HkPauseLabel";
            this.HkPauseLabel.Size = new System.Drawing.Size(114, 17);
            this.HkPauseLabel.TabIndex = 18;
            this.HkPauseLabel.Text = "Select Pause Key";
            this.HkPauseLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkPauseLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkPause_MouseMove);
            // 
            // ComboHkShatteringThrow
            // 
            this.ComboHkShatteringThrow.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkShatteringThrow.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkShatteringThrow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkShatteringThrow.DropDownHeight = 110;
            this.ComboHkShatteringThrow.DropDownWidth = 200;
            this.ComboHkShatteringThrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkShatteringThrow.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkShatteringThrow.FormattingEnabled = true;
            this.ComboHkShatteringThrow.IntegralHeight = false;
            this.ComboHkShatteringThrow.Location = new System.Drawing.Point(300, 168);
            this.ComboHkShatteringThrow.Name = "ComboHkShatteringThrow";
            this.ComboHkShatteringThrow.Size = new System.Drawing.Size(328, 26);
            this.ComboHkShatteringThrow.TabIndex = 19;
            this.ComboHkShatteringThrow.SelectedIndexChanged += new System.EventHandler(this.ComboHkShatteringThrow_SelectedIndexChanged);
            this.ComboHkShatteringThrow.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkShatteringThrow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkShatteringThrow_MouseMove);
            // 
            // HkShattThrowLabel
            // 
            this.HkShattThrowLabel.AutoSize = true;
            this.HkShattThrowLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkShattThrowLabel.Location = new System.Drawing.Point(12, 170);
            this.HkShattThrowLabel.Name = "HkShattThrowLabel";
            this.HkShattThrowLabel.Size = new System.Drawing.Size(115, 17);
            this.HkShattThrowLabel.TabIndex = 20;
            this.HkShattThrowLabel.Text = "Shattering Throw";
            this.HkShattThrowLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkShattThrowLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkShatteringThrow_MouseMove);
            // 
            // ComboHkTier4Ability
            // 
            this.ComboHkTier4Ability.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkTier4Ability.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkTier4Ability.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkTier4Ability.DropDownHeight = 110;
            this.ComboHkTier4Ability.DropDownWidth = 200;
            this.ComboHkTier4Ability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkTier4Ability.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkTier4Ability.FormattingEnabled = true;
            this.ComboHkTier4Ability.IntegralHeight = false;
            this.ComboHkTier4Ability.Location = new System.Drawing.Point(300, 208);
            this.ComboHkTier4Ability.Name = "ComboHkTier4Ability";
            this.ComboHkTier4Ability.Size = new System.Drawing.Size(328, 26);
            this.ComboHkTier4Ability.TabIndex = 21;
            this.ComboHkTier4Ability.SelectedIndexChanged += new System.EventHandler(this.ComboHkTier4Ability_SelectedIndexChanged);
            this.ComboHkTier4Ability.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkTier4Ability.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkTier4Ability_MouseMove);
            // 
            // HkTier4AbilityLabel
            // 
            this.HkTier4AbilityLabel.AutoSize = true;
            this.HkTier4AbilityLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkTier4AbilityLabel.Location = new System.Drawing.Point(12, 210);
            this.HkTier4AbilityLabel.Name = "HkTier4AbilityLabel";
            this.HkTier4AbilityLabel.Size = new System.Drawing.Size(81, 17);
            this.HkTier4AbilityLabel.TabIndex = 22;
            this.HkTier4AbilityLabel.Text = "Tier 4 Ability";
            this.HkTier4AbilityLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkTier4AbilityLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkTier4Ability_MouseMove);
            // 
            // ComboHkSpecial
            // 
            this.ComboHkSpecial.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkSpecial.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkSpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkSpecial.DropDownHeight = 110;
            this.ComboHkSpecial.DropDownWidth = 200;
            this.ComboHkSpecial.Enabled = false;
            this.ComboHkSpecial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkSpecial.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkSpecial.FormattingEnabled = true;
            this.ComboHkSpecial.IntegralHeight = false;
            this.ComboHkSpecial.Location = new System.Drawing.Point(300, 478);
            this.ComboHkSpecial.Name = "ComboHkSpecial";
            this.ComboHkSpecial.Size = new System.Drawing.Size(328, 26);
            this.ComboHkSpecial.TabIndex = 25;
            this.ComboHkSpecial.SelectedIndexChanged += new System.EventHandler(this.ComboHkSpecial_SelectedIndexChanged);
            this.ComboHkSpecial.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkSpecial.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkSpecial_MouseMove);
            // 
            // HkSpecialLabel
            // 
            this.HkSpecialLabel.AutoSize = true;
            this.HkSpecialLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkSpecialLabel.Location = new System.Drawing.Point(12, 480);
            this.HkSpecialLabel.Name = "HkSpecialLabel";
            this.HkSpecialLabel.Size = new System.Drawing.Size(122, 17);
            this.HkSpecialLabel.TabIndex = 26;
            this.HkSpecialLabel.Text = "Select Special Key";
            this.HkSpecialLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkSpecialLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkSpecial_MouseMove);
            // 
            // ComboHkMockingBanner
            // 
            this.ComboHkMockingBanner.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ComboHkMockingBanner.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboHkMockingBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ComboHkMockingBanner.DropDownHeight = 110;
            this.ComboHkMockingBanner.DropDownWidth = 200;
            this.ComboHkMockingBanner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboHkMockingBanner.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.ComboHkMockingBanner.FormattingEnabled = true;
            this.ComboHkMockingBanner.IntegralHeight = false;
            this.ComboHkMockingBanner.Location = new System.Drawing.Point(300, 128);
            this.ComboHkMockingBanner.Name = "ComboHkMockingBanner";
            this.ComboHkMockingBanner.Size = new System.Drawing.Size(328, 26);
            this.ComboHkMockingBanner.TabIndex = 27;
            this.ComboHkMockingBanner.SelectedIndexChanged += new System.EventHandler(this.ComboHkMockingBanner_SelectedIndexChanged);
            this.ComboHkMockingBanner.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.ComboHkMockingBanner.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkMockingBanner_MouseMove);
            // 
            // HkMockingBannerLabel
            // 
            this.HkMockingBannerLabel.AutoSize = true;
            this.HkMockingBannerLabel.Font = new System.Drawing.Font("Century Gothic", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HkMockingBannerLabel.Location = new System.Drawing.Point(12, 130);
            this.HkMockingBannerLabel.Name = "HkMockingBannerLabel";
            this.HkMockingBannerLabel.Size = new System.Drawing.Size(111, 17);
            this.HkMockingBannerLabel.TabIndex = 28;
            this.HkMockingBannerLabel.Text = "Mocking Banner";
            this.HkMockingBannerLabel.MouseLeave += new System.EventHandler(this.Fu_MouseLeave);
            this.HkMockingBannerLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ComboHkMockingBanner_MouseMove);
            // 
            // HotkeyPanel
            // 
            this.HotkeyPanel.BackColor = System.Drawing.Color.White;
            this.HotkeyPanel.Controls.Add(this.HkMockingBannerLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkMockingBanner);
            this.HotkeyPanel.Controls.Add(this.HkSpecialLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkSpecial);
            this.HotkeyPanel.Controls.Add(this.HkTier4AbilityLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkTier4Ability);
            this.HotkeyPanel.Controls.Add(this.HkShattThrowLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkShatteringThrow);
            this.HotkeyPanel.Controls.Add(this.HkPauseLabel);
            this.HotkeyPanel.Controls.Add(this.HkAoELabel);
            this.HotkeyPanel.Controls.Add(this.HkCooldownLabel);
            this.HotkeyPanel.Controls.Add(this.HkModifierLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkPause);
            this.HotkeyPanel.Controls.Add(this.ComboHkMultiTgt);
            this.HotkeyPanel.Controls.Add(this.ComboHkCooldown);
            this.HotkeyPanel.Controls.Add(this.ComboHkModifier);
            this.HotkeyPanel.Controls.Add(this.ComboHkMode);
            this.HotkeyPanel.Controls.Add(this.HkModeLabel);
            this.HotkeyPanel.Controls.Add(this.label4);
            this.HotkeyPanel.Controls.Add(this.HkHeroicLeapLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkHeroicLeap);
            this.HotkeyPanel.Controls.Add(this.label2);
            this.HotkeyPanel.Controls.Add(this.HkDemoBannerLabel);
            this.HotkeyPanel.Controls.Add(this.ComboHkDemoralizingBanner);
            this.HotkeyPanel.Location = new System.Drawing.Point(936, 135);
            this.HotkeyPanel.Name = "HotkeyPanel";
            this.HotkeyPanel.Size = new System.Drawing.Size(640, 515);
            this.HotkeyPanel.TabIndex = 3;
            // 
            // BottomWhitePanel
            // 
            this.BottomWhitePanel.BackColor = System.Drawing.Color.White;
            this.BottomWhitePanel.Controls.Add(this.SaveButton);
            this.BottomWhitePanel.Location = new System.Drawing.Point(936, 682);
            this.BottomWhitePanel.Name = "BottomWhitePanel";
            this.BottomWhitePanel.Size = new System.Drawing.Size(640, 50);
            this.BottomWhitePanel.TabIndex = 6;
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(1588, 832);
            this.Controls.Add(this.debuggerpanel);
            this.Controls.Add(this.BottomWhitePanel);
            this.Controls.Add(this.SpecGrid);
            this.Controls.Add(this.HotkeyPanel);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.GeneralGrid);
            this.Controls.Add(this.LogoPicture);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Interface";
            this.ShowIcon = false;
            this.Text = "Fury Unleashed";
            this.Load += new System.EventHandler(this.FuInterface_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Fu_GuiDragDrop);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.BottomWhitePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogoPicture)).EndInit();
            this.HotkeyPanel.ResumeLayout(false);
            this.HotkeyPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid GeneralGrid;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.PictureBox LogoPicture;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.PropertyGrid SpecGrid;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripText;
        private System.Windows.Forms.Panel debuggerpanel;
        private System.Windows.Forms.ComboBox ComboHkDemoralizingBanner;
        private System.Windows.Forms.Label HkDemoBannerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboHkHeroicLeap;
        private System.Windows.Forms.Label HkHeroicLeapLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label HkModeLabel;
        private System.Windows.Forms.ComboBox ComboHkMode;
        private System.Windows.Forms.ComboBox ComboHkModifier;
        private System.Windows.Forms.ComboBox ComboHkCooldown;
        private System.Windows.Forms.ComboBox ComboHkMultiTgt;
        private System.Windows.Forms.ComboBox ComboHkPause;
        private System.Windows.Forms.Label HkModifierLabel;
        private System.Windows.Forms.Label HkCooldownLabel;
        private System.Windows.Forms.Label HkAoELabel;
        private System.Windows.Forms.Label HkPauseLabel;
        private System.Windows.Forms.ComboBox ComboHkShatteringThrow;
        private System.Windows.Forms.Label HkShattThrowLabel;
        private System.Windows.Forms.ComboBox ComboHkTier4Ability;
        private System.Windows.Forms.Label HkTier4AbilityLabel;
        private System.Windows.Forms.ComboBox ComboHkSpecial;
        private System.Windows.Forms.Label HkSpecialLabel;
        private System.Windows.Forms.ComboBox ComboHkMockingBanner;
        private System.Windows.Forms.Label HkMockingBannerLabel;
        private System.Windows.Forms.Panel HotkeyPanel;
        private System.Windows.Forms.Panel BottomWhitePanel;
    }
}