namespace Oracle.UI
{
    partial class Config
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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pgClass = new System.Windows.Forms.PropertyGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.grpHealcalc = new System.Windows.Forms.GroupBox();
            this.btnHealCalc = new System.Windows.Forms.Button();
            this.lblHealcalc = new System.Windows.Forms.Label();
            this.cmboHealCalc = new System.Windows.Forms.ComboBox();
            this.txtHealCalculation = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDumpAuras = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectPlayers = new System.Windows.Forms.Button();
            this.grpCombatLog = new System.Windows.Forms.GroupBox();
            this.btnCombatLog = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lblDispelInformation = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveXml = new System.Windows.Forms.Button();
            this.btnExportXML = new System.Windows.Forms.Button();
            this.btnLoadXml = new System.Windows.Forms.Button();
            this.dataGridDispel = new System.Windows.Forms.DataGridView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.webReadme = new System.Windows.Forms.WebBrowser();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.lblPlusRep = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logo = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnLoadBossEncounter = new System.Windows.Forms.Button();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.grpHealcalc.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpCombatLog.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDispel)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.logoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Controls.Add(this.tabPage3);
            this.tabMain.Controls.Add(this.tabPage4);
            this.tabMain.Controls.Add(this.tabPage5);
            this.tabMain.Controls.Add(this.tabPage6);
            this.tabMain.Location = new System.Drawing.Point(2, 169);
            this.tabMain.Multiline = true;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(347, 550);
            this.tabMain.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.pgMain);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(339, 521);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // pgMain
            // 
            this.pgMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMain.HelpBackColor = System.Drawing.Color.White;
            this.pgMain.Location = new System.Drawing.Point(3, 3);
            this.pgMain.Name = "pgMain";
            this.pgMain.SelectedItemWithFocusBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pgMain.Size = new System.Drawing.Size(333, 515);
            this.pgMain.TabIndex = 0;
            this.pgMain.ToolbarVisible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.pgClass);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(339, 521);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Class Specific";
            // 
            // pgClass
            // 
            this.pgClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgClass.Location = new System.Drawing.Point(3, 3);
            this.pgClass.Name = "pgClass";
            this.pgClass.SelectedItemWithFocusBackColor = System.Drawing.SystemColors.ControlDark;
            this.pgClass.Size = new System.Drawing.Size(333, 515);
            this.pgClass.TabIndex = 0;
            this.pgClass.ToolbarVisible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.grpHealcalc);
            this.tabPage3.Controls.Add(this.txtHealCalculation);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(339, 521);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Heal Calcs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // grpHealcalc
            // 
            this.grpHealcalc.Controls.Add(this.btnHealCalc);
            this.grpHealcalc.Controls.Add(this.lblHealcalc);
            this.grpHealcalc.Controls.Add(this.cmboHealCalc);
            this.grpHealcalc.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpHealcalc.Location = new System.Drawing.Point(3, 3);
            this.grpHealcalc.Name = "grpHealcalc";
            this.grpHealcalc.Size = new System.Drawing.Size(333, 54);
            this.grpHealcalc.TabIndex = 6;
            this.grpHealcalc.TabStop = false;
            this.grpHealcalc.Text = "Calculation Control";
            // 
            // btnHealCalc
            // 
            this.btnHealCalc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnHealCalc.Location = new System.Drawing.Point(240, 21);
            this.btnHealCalc.Name = "btnHealCalc";
            this.btnHealCalc.Size = new System.Drawing.Size(75, 23);
            this.btnHealCalc.TabIndex = 8;
            this.btnHealCalc.Text = "Calculate";
            this.btnHealCalc.UseVisualStyleBackColor = true;
            this.btnHealCalc.Click += new System.EventHandler(this.btnHealCalc_Click);
            // 
            // lblHealcalc
            // 
            this.lblHealcalc.AutoSize = true;
            this.lblHealcalc.Location = new System.Drawing.Point(5, 25);
            this.lblHealcalc.Name = "lblHealcalc";
            this.lblHealcalc.Size = new System.Drawing.Size(66, 13);
            this.lblHealcalc.TabIndex = 7;
            this.lblHealcalc.Text = "Select Spell:";
            // 
            // cmboHealCalc
            // 
            this.cmboHealCalc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmboHealCalc.FormattingEnabled = true;
            this.cmboHealCalc.Location = new System.Drawing.Point(79, 21);
            this.cmboHealCalc.Name = "cmboHealCalc";
            this.cmboHealCalc.Size = new System.Drawing.Size(121, 21);
            this.cmboHealCalc.TabIndex = 6;
            this.cmboHealCalc.SelectedIndexChanged += new System.EventHandler(this.txtHealCalculation_SelectedIndexChanged);
            // 
            // txtHealCalculation
            // 
            this.txtHealCalculation.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtHealCalculation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHealCalculation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtHealCalculation.Location = new System.Drawing.Point(3, 63);
            this.txtHealCalculation.Multiline = true;
            this.txtHealCalculation.Name = "txtHealCalculation";
            this.txtHealCalculation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHealCalculation.Size = new System.Drawing.Size(333, 455);
            this.txtHealCalculation.TabIndex = 2;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnLoadBossEncounter);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.btnDumpAuras);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.grpCombatLog);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(339, 521);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Tools";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 473);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Weight Watcher";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDumpAuras
            // 
            this.btnDumpAuras.Location = new System.Drawing.Point(194, 473);
            this.btnDumpAuras.Name = "btnDumpAuras";
            this.btnDumpAuras.Size = new System.Drawing.Size(111, 23);
            this.btnDumpAuras.TabIndex = 3;
            this.btnDumpAuras.Text = "Dump Target Auras";
            this.btnDumpAuras.UseVisualStyleBackColor = true;
            this.btnDumpAuras.Click += new System.EventHandler(this.btnDumpAuras_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectPlayers);
            this.groupBox2.Location = new System.Drawing.Point(6, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 95);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Players to Heal";
            // 
            // btnSelectPlayers
            // 
            this.btnSelectPlayers.Location = new System.Drawing.Point(92, 42);
            this.btnSelectPlayers.Name = "btnSelectPlayers";
            this.btnSelectPlayers.Size = new System.Drawing.Size(145, 23);
            this.btnSelectPlayers.TabIndex = 1;
            this.btnSelectPlayers.Text = "Select Players";
            this.btnSelectPlayers.UseVisualStyleBackColor = true;
            this.btnSelectPlayers.Click += new System.EventHandler(this.btnSelectPlayers_Click);
            // 
            // grpCombatLog
            // 
            this.grpCombatLog.Controls.Add(this.btnCombatLog);
            this.grpCombatLog.Location = new System.Drawing.Point(3, 3);
            this.grpCombatLog.Name = "grpCombatLog";
            this.grpCombatLog.Size = new System.Drawing.Size(336, 71);
            this.grpCombatLog.TabIndex = 0;
            this.grpCombatLog.TabStop = false;
            this.grpCombatLog.Text = "Combat Log";
            // 
            // btnCombatLog
            // 
            this.btnCombatLog.Location = new System.Drawing.Point(95, 30);
            this.btnCombatLog.Name = "btnCombatLog";
            this.btnCombatLog.Size = new System.Drawing.Size(145, 23);
            this.btnCombatLog.TabIndex = 0;
            this.btnCombatLog.Text = "Open Logged Spells";
            this.btnCombatLog.UseVisualStyleBackColor = true;
            this.btnCombatLog.Click += new System.EventHandler(this.btnCombatLog_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lblDispelInformation);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Controls.Add(this.dataGridDispel);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(339, 521);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Dispel";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lblDispelInformation
            // 
            this.lblDispelInformation.AutoSize = true;
            this.lblDispelInformation.ForeColor = System.Drawing.Color.Maroon;
            this.lblDispelInformation.Location = new System.Drawing.Point(10, 407);
            this.lblDispelInformation.Name = "lblDispelInformation";
            this.lblDispelInformation.Size = new System.Drawing.Size(59, 13);
            this.lblDispelInformation.TabIndex = 10;
            this.lblDispelInformation.Text = "Information";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSaveXml);
            this.groupBox1.Controls.Add(this.btnExportXML);
            this.groupBox1.Controls.Add(this.btnLoadXml);
            this.groupBox1.Location = new System.Drawing.Point(6, 445);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 73);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import/Export/Save Dispelable Spells";
            // 
            // btnSaveXml
            // 
            this.btnSaveXml.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveXml.Location = new System.Drawing.Point(230, 29);
            this.btnSaveXml.Name = "btnSaveXml";
            this.btnSaveXml.Size = new System.Drawing.Size(71, 27);
            this.btnSaveXml.TabIndex = 11;
            this.btnSaveXml.Text = "Save";
            this.btnSaveXml.UseVisualStyleBackColor = true;
            this.btnSaveXml.Click += new System.EventHandler(this.btnSaveXml_Click);
            // 
            // btnExportXML
            // 
            this.btnExportXML.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportXML.Location = new System.Drawing.Point(127, 29);
            this.btnExportXML.Name = "btnExportXML";
            this.btnExportXML.Size = new System.Drawing.Size(71, 27);
            this.btnExportXML.TabIndex = 10;
            this.btnExportXML.Text = "Export";
            this.btnExportXML.UseVisualStyleBackColor = true;
            this.btnExportXML.Click += new System.EventHandler(this.btnExportXML_Click);
            // 
            // btnLoadXml
            // 
            this.btnLoadXml.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadXml.Location = new System.Drawing.Point(24, 29);
            this.btnLoadXml.Name = "btnLoadXml";
            this.btnLoadXml.Size = new System.Drawing.Size(71, 27);
            this.btnLoadXml.TabIndex = 9;
            this.btnLoadXml.Text = "Import";
            this.btnLoadXml.UseVisualStyleBackColor = true;
            this.btnLoadXml.Click += new System.EventHandler(this.btnLoadXml_Click);
            // 
            // dataGridDispel
            // 
            this.dataGridDispel.AllowUserToOrderColumns = true;
            this.dataGridDispel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDispel.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridDispel.Location = new System.Drawing.Point(0, 0);
            this.dataGridDispel.Name = "dataGridDispel";
            this.dataGridDispel.RowHeadersVisible = false;
            this.dataGridDispel.Size = new System.Drawing.Size(339, 399);
            this.dataGridDispel.TabIndex = 0;
            this.dataGridDispel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDispel_CellContentClick);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.webReadme);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(339, 521);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Readme";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // webReadme
            // 
            this.webReadme.AllowNavigation = false;
            this.webReadme.AllowWebBrowserDrop = false;
            this.webReadme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webReadme.IsWebBrowserContextMenuEnabled = false;
            this.webReadme.Location = new System.Drawing.Point(0, 0);
            this.webReadme.MinimumSize = new System.Drawing.Size(20, 20);
            this.webReadme.Name = "webReadme";
            this.webReadme.ScriptErrorsSuppressed = true;
            this.webReadme.Size = new System.Drawing.Size(339, 521);
            this.webReadme.TabIndex = 0;
            // 
            // logoPanel
            // 
            this.logoPanel.BackColor = System.Drawing.Color.White;
            this.logoPanel.Controls.Add(this.lblPlusRep);
            this.logoPanel.Controls.Add(this.lblVersion);
            this.logoPanel.Controls.Add(this.panel1);
            this.logoPanel.Controls.Add(this.logo);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(352, 163);
            this.logoPanel.TabIndex = 2;
            // 
            // lblPlusRep
            // 
            this.lblPlusRep.AutoSize = true;
            this.lblPlusRep.Location = new System.Drawing.Point(3, 145);
            this.lblPlusRep.Name = "lblPlusRep";
            this.lblPlusRep.Size = new System.Drawing.Size(193, 13);
            this.lblPlusRep.TabIndex = 5;
            this.lblPlusRep.Text = "Enjoying Oracle? Click here to +rep me!";
            this.lblPlusRep.Click += new System.EventHandler(this.lblPlusRep_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(247, 145);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version: 1.0.0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Location = new System.Drawing.Point(0, 161);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 2);
            this.panel1.TabIndex = 3;
            // 
            // logo
            // 
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logo.Dock = System.Windows.Forms.DockStyle.Top;
            this.logo.Location = new System.Drawing.Point(0, 0);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(352, 160);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.logo.TabIndex = 2;
            this.logo.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(242, 725);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 29);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadSettings.Location = new System.Drawing.Point(9, 725);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(98, 29);
            this.btnLoadSettings.TabIndex = 4;
            this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveSettings.Location = new System.Drawing.Point(125, 725);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(98, 29);
            this.btnSaveSettings.TabIndex = 5;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnLoadBossEncounter
            // 
            this.btnLoadBossEncounter.Location = new System.Drawing.Point(98, 278);
            this.btnLoadBossEncounter.Name = "btnLoadBossEncounter";
            this.btnLoadBossEncounter.Size = new System.Drawing.Size(145, 23);
            this.btnLoadBossEncounter.TabIndex = 5;
            this.btnLoadBossEncounter.Text = "Load Boss Encounter";
            this.btnLoadBossEncounter.UseVisualStyleBackColor = true;
            this.btnLoadBossEncounter.Click += new System.EventHandler(this.btnLoadBossEncounter_Click);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(352, 762);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.logoPanel);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Config";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Oracle Configuration";
            this.Load += new System.EventHandler(this.ConfigurationFormLoad);
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.grpHealcalc.ResumeLayout(false);
            this.grpHealcalc.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.grpCombatLog.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDispel)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            this.logoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid pgClass;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoadSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.PropertyGrid pgMain;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblPlusRep;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtHealCalculation;
        private System.Windows.Forms.GroupBox grpHealcalc;
        private System.Windows.Forms.Button btnHealCalc;
        private System.Windows.Forms.Label lblHealcalc;
        private System.Windows.Forms.ComboBox cmboHealCalc;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox grpCombatLog;
        private System.Windows.Forms.Button btnCombatLog;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.WebBrowser webReadme;
        public System.Windows.Forms.DataGridView dataGridDispel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExportXML;
        private System.Windows.Forms.Button btnLoadXml;
        private System.Windows.Forms.Button btnSaveXml;
        private System.Windows.Forms.Label lblDispelInformation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectPlayers;
        private System.Windows.Forms.Button btnDumpAuras;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLoadBossEncounter;

    }
}