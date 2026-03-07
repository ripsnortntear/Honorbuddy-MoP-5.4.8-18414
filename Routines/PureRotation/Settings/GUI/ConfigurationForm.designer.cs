using System.Windows.Forms;

namespace PureRotation.Settings.GUI
{
    partial class ConfigurationForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedtoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripComboBox = new System.Windows.Forms.ToolStripMenuItem();
            this.resetHotkeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwyasOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadRoutineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.routineManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatebtn = new System.Windows.Forms.ToolStripMenuItem();
            this.healingDebugerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repDevsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nomnomnomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.millzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.worklifebalanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apocToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bobby53ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblRotation = new System.Windows.Forms.Label();
            this.rotationCBO = new System.Windows.Forms.ComboBox();
            this.lblMod = new System.Windows.Forms.Label();
            this.altkeyCBO = new System.Windows.Forms.ComboBox();
            this.lblSpecial = new System.Windows.Forms.Label();
            this.specialcbo = new System.Windows.Forms.ComboBox();
            this.lblPause = new System.Windows.Forms.Label();
            this.lblAoE = new System.Windows.Forms.Label();
            this.lblCD = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pausecbo = new System.Windows.Forms.ComboBox();
            this.switchcbo = new System.Windows.Forms.ComboBox();
            this.cooldowncbo = new System.Windows.Forms.ComboBox();
            this.modecbo = new System.Windows.Forms.ComboBox();
            this.pgClass = new System.Windows.Forms.PropertyGrid();
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this.wulfToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stormchasingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.weishbierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.handnaviToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dagradtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mirabisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.advancedtoolStripMenuItem1,
            this.browseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(794, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.CheckOnClick = true;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItemClick);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.CheckOnClick = true;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItemClick);
            // 
            // advancedtoolStripMenuItem1
            // 
            this.advancedtoolStripMenuItem1.CheckOnClick = true;
            this.advancedtoolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripComboBox,
            this.alwyasOnTopToolStripMenuItem,
            this.reloadSettingsToolStripMenuItem,
            this.reloadRoutineToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.updatebtn,
            this.healingDebugerToolStripMenuItem});
            this.advancedtoolStripMenuItem1.Name = "advancedtoolStripMenuItem1";
            this.advancedtoolStripMenuItem1.Size = new System.Drawing.Size(72, 20);
            this.advancedtoolStripMenuItem1.Text = "Advanced";
            // 
            // stripComboBox
            // 
            this.stripComboBox.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetHotkeysToolStripMenuItem});
            this.stripComboBox.Name = "stripComboBox";
            this.stripComboBox.Size = new System.Drawing.Size(184, 22);
            this.stripComboBox.Text = "Hotkey";
            // 
            // resetHotkeysToolStripMenuItem
            // 
            this.resetHotkeysToolStripMenuItem.Name = "resetHotkeysToolStripMenuItem";
            this.resetHotkeysToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetHotkeysToolStripMenuItem.Text = "Reset Hotkeys";
            this.resetHotkeysToolStripMenuItem.Click += new System.EventHandler(this.ResetHotkeysToolStripMenuItemClick);
            // 
            // alwyasOnTopToolStripMenuItem
            // 
            this.alwyasOnTopToolStripMenuItem.Name = "alwyasOnTopToolStripMenuItem";
            this.alwyasOnTopToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.alwyasOnTopToolStripMenuItem.Text = "Always on top";
            this.alwyasOnTopToolStripMenuItem.Click += new System.EventHandler(this.AlwyasOnTopToolStripMenuItemClick);
            // 
            // reloadSettingsToolStripMenuItem
            // 
            this.reloadSettingsToolStripMenuItem.Name = "reloadSettingsToolStripMenuItem";
            this.reloadSettingsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.reloadSettingsToolStripMenuItem.Text = "Reload Settings";
            this.reloadSettingsToolStripMenuItem.Click += new System.EventHandler(this.ReloadSettingsToolStripMenuItemClick);
            // 
            // reloadRoutineToolStripMenuItem
            // 
            this.reloadRoutineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assemblieToolStripMenuItem,
            this.routineManagerToolStripMenuItem});
            this.reloadRoutineToolStripMenuItem.Name = "reloadRoutineToolStripMenuItem";
            this.reloadRoutineToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.reloadRoutineToolStripMenuItem.Text = "Reload Honorbuddy";
            // 
            // assemblieToolStripMenuItem
            // 
            this.assemblieToolStripMenuItem.Name = "assemblieToolStripMenuItem";
            this.assemblieToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.assemblieToolStripMenuItem.Text = "Assembly";
            this.assemblieToolStripMenuItem.Click += new System.EventHandler(this.AssemblieToolStripMenuItemClick);
            // 
            // routineManagerToolStripMenuItem
            // 
            this.routineManagerToolStripMenuItem.Name = "routineManagerToolStripMenuItem";
            this.routineManagerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.routineManagerToolStripMenuItem.Text = "RoutineManager";
            this.routineManagerToolStripMenuItem.Click += new System.EventHandler(this.RoutineManagerToolStripMenuItemClick);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.debugToolStripMenuItem.Text = "Debug";
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.DebugToolStripMenuItemClick);
            // 
            // updatebtn
            // 
            this.updatebtn.Enabled = false;
            this.updatebtn.Name = "updatebtn";
            this.updatebtn.Size = new System.Drawing.Size(184, 22);
            this.updatebtn.Text = "Updater";
            // 
            // healingDebugerToolStripMenuItem
            // 
            this.healingDebugerToolStripMenuItem.Name = "healingDebugerToolStripMenuItem";
            this.healingDebugerToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.healingDebugerToolStripMenuItem.Text = "Healing Debuger";
            this.healingDebugerToolStripMenuItem.Click += new System.EventHandler(this.healingDebugerToolStripMenuItem_Click);
            // 
            // browseToolStripMenuItem
            // 
            this.browseToolStripMenuItem.CheckOnClick = true;
            this.browseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.threadToolStripMenuItem,
            this.repDevsToolStripMenuItem,
            this.creditsToolStripMenuItem});
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.browseToolStripMenuItem.Text = "Browse";
            // 
            // threadToolStripMenuItem
            // 
            this.threadToolStripMenuItem.Name = "threadToolStripMenuItem";
            this.threadToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.threadToolStripMenuItem.Text = "Go to Forum";
            this.threadToolStripMenuItem.Click += new System.EventHandler(this.ThreadToolStripMenuItemClick);
            // 
            // repDevsToolStripMenuItem
            // 
            this.repDevsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wulfToolStripMenuItem1,
            this.stormchasingToolStripMenuItem1,
            this.weishbierToolStripMenuItem,
            this.alToolStripMenuItem,
            this.amaToolStripMenuItem,
            this.nomnomnomToolStripMenuItem,
            this.millzToolStripMenuItem,
            this.treekToolStripMenuItem,
            this.laoToolStripMenuItem,
            this.worklifebalanceToolStripMenuItem,
            this.handnaviToolStripMenuItem,
            this.dagradtToolStripMenuItem,
            this.mirabisToolStripMenuItem});
            this.repDevsToolStripMenuItem.Name = "repDevsToolStripMenuItem";
            this.repDevsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.repDevsToolStripMenuItem.Text = "PureRotation Dev\'s";
            // 
            // alToolStripMenuItem
            // 
            this.alToolStripMenuItem.Name = "alToolStripMenuItem";
            this.alToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.alToolStripMenuItem.Text = "Alxaw";
            // 
            // amaToolStripMenuItem
            // 
            this.amaToolStripMenuItem.Name = "amaToolStripMenuItem";
            this.amaToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.amaToolStripMenuItem.Text = "Ama";
            // 
            // laoToolStripMenuItem
            // 
            this.laoToolStripMenuItem.Name = "laoToolStripMenuItem";
            this.laoToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.laoToolStripMenuItem.Text = "Lao";
            // 
            // nomnomnomToolStripMenuItem
            // 
            this.nomnomnomToolStripMenuItem.Name = "nomnomnomToolStripMenuItem";
            this.nomnomnomToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.nomnomnomToolStripMenuItem.Text = "Nomnomnom";
            // 
            // millzToolStripMenuItem
            // 
            this.millzToolStripMenuItem.Name = "millzToolStripMenuItem";
            this.millzToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.millzToolStripMenuItem.Text = "Millz";
            // 
            // treekToolStripMenuItem
            // 
            this.treekToolStripMenuItem.Name = "treekToolStripMenuItem";
            this.treekToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.treekToolStripMenuItem.Text = "TreeK";
            // 
            // worklifebalanceToolStripMenuItem
            // 
            this.worklifebalanceToolStripMenuItem.Name = "worklifebalanceToolStripMenuItem";
            this.worklifebalanceToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.worklifebalanceToolStripMenuItem.Text = "Worklifebalance";
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.apocToolStripMenuItem,
            this.bobby53ToolStripMenuItem});
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.creditsToolStripMenuItem.Text = "Credits";
            // 
            // apocToolStripMenuItem
            // 
            this.apocToolStripMenuItem.Name = "apocToolStripMenuItem";
            this.apocToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.apocToolStripMenuItem.Text = "Apoc";
            // 
            // bobby53ToolStripMenuItem
            // 
            this.bobby53ToolStripMenuItem.Name = "bobby53ToolStripMenuItem";
            this.bobby53ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bobby53ToolStripMenuItem.Text = "Bobby53";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(78, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 27);
            this.label1.TabIndex = 3;
            this.label1.Text = "PureRotation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(202, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = " - The Ultimate Level 90 Combat Routine";
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(15, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 72);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(0, 477);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(794, 80);
            this.panel4.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.lblRotation);
            this.panel5.Controls.Add(this.rotationCBO);
            this.panel5.Controls.Add(this.lblMod);
            this.panel5.Controls.Add(this.altkeyCBO);
            this.panel5.Controls.Add(this.lblSpecial);
            this.panel5.Controls.Add(this.specialcbo);
            this.panel5.Controls.Add(this.lblPause);
            this.panel5.Controls.Add(this.lblAoE);
            this.panel5.Controls.Add(this.lblCD);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.pausecbo);
            this.panel5.Controls.Add(this.switchcbo);
            this.panel5.Controls.Add(this.cooldowncbo);
            this.panel5.Controls.Add(this.modecbo);
            this.panel5.Location = new System.Drawing.Point(4, 27);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(786, 56);
            this.panel5.TabIndex = 7;
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRotation.Location = new System.Drawing.Point(636, 7);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(80, 13);
            this.lblRotation.TabIndex = 13;
            this.lblRotation.Text = "Rotation Key";
            // 
            // rotationCBO
            // 
            this.rotationCBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rotationCBO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rotationCBO.FormattingEnabled = true;
            this.rotationCBO.Location = new System.Drawing.Point(639, 25);
            this.rotationCBO.Name = "rotationCBO";
            this.rotationCBO.Size = new System.Drawing.Size(77, 23);
            this.rotationCBO.TabIndex = 12;
            this.rotationCBO.SelectedIndexChanged += new System.EventHandler(this.rotationCBO_SelectedIndexChanged);
            // 
            // lblMod
            // 
            this.lblMod.AutoSize = true;
            this.lblMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMod.Location = new System.Drawing.Point(551, 7);
            this.lblMod.Name = "lblMod";
            this.lblMod.Size = new System.Drawing.Size(56, 13);
            this.lblMod.TabIndex = 11;
            this.lblMod.Text = "Mod Key";
            // 
            // altkeyCBO
            // 
            this.altkeyCBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.altkeyCBO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.altkeyCBO.FormattingEnabled = true;
            this.altkeyCBO.Location = new System.Drawing.Point(542, 25);
            this.altkeyCBO.Name = "altkeyCBO";
            this.altkeyCBO.Size = new System.Drawing.Size(77, 23);
            this.altkeyCBO.TabIndex = 10;
            this.altkeyCBO.SelectedIndexChanged += new System.EventHandler(this.altkeyCBO_SelectedIndexChanged);
            // 
            // lblSpecial
            // 
            this.lblSpecial.AutoSize = true;
            this.lblSpecial.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecial.Location = new System.Drawing.Point(436, 7);
            this.lblSpecial.Name = "lblSpecial";
            this.lblSpecial.Size = new System.Drawing.Size(74, 13);
            this.lblSpecial.TabIndex = 9;
            this.lblSpecial.Text = "Special Key";
            // 
            // specialcbo
            // 
            this.specialcbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specialcbo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.specialcbo.FormattingEnabled = true;
            this.specialcbo.Location = new System.Drawing.Point(439, 25);
            this.specialcbo.Name = "specialcbo";
            this.specialcbo.Size = new System.Drawing.Size(77, 23);
            this.specialcbo.TabIndex = 8;
            this.specialcbo.SelectedIndexChanged += new System.EventHandler(this.specialcbo_SelectedIndexChanged);
            // 
            // lblPause
            // 
            this.lblPause.AutoSize = true;
            this.lblPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPause.Location = new System.Drawing.Point(336, 7);
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(67, 13);
            this.lblPause.TabIndex = 7;
            this.lblPause.Text = "Pause Key";
            // 
            // lblAoE
            // 
            this.lblAoE.AutoSize = true;
            this.lblAoE.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAoE.Location = new System.Drawing.Point(237, 7);
            this.lblAoE.Name = "lblAoE";
            this.lblAoE.Size = new System.Drawing.Size(55, 13);
            this.lblAoE.TabIndex = 6;
            this.lblAoE.Text = "AoE Key";
            // 
            // lblCD
            // 
            this.lblCD.AutoSize = true;
            this.lblCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCD.Location = new System.Drawing.Point(122, 7);
            this.lblCD.Name = "lblCD";
            this.lblCD.Size = new System.Drawing.Size(93, 13);
            this.lblCD.TabIndex = 5;
            this.lblCD.Text = "Cool-Down Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Rotation Mode";
            // 
            // pausecbo
            // 
            this.pausecbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pausecbo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pausecbo.FormattingEnabled = true;
            this.pausecbo.Location = new System.Drawing.Point(330, 25);
            this.pausecbo.Name = "pausecbo";
            this.pausecbo.Size = new System.Drawing.Size(77, 23);
            this.pausecbo.TabIndex = 3;
            this.pausecbo.SelectedIndexChanged += new System.EventHandler(this.pausecbo_SelectedIndexChanged);
            // 
            // switchcbo
            // 
            this.switchcbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.switchcbo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.switchcbo.FormattingEnabled = true;
            this.switchcbo.Location = new System.Drawing.Point(229, 25);
            this.switchcbo.Name = "switchcbo";
            this.switchcbo.Size = new System.Drawing.Size(77, 23);
            this.switchcbo.TabIndex = 2;
            this.switchcbo.SelectedIndexChanged += new System.EventHandler(this.switchcbo_SelectedIndexChanged);
            // 
            // cooldowncbo
            // 
            this.cooldowncbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cooldowncbo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cooldowncbo.FormattingEnabled = true;
            this.cooldowncbo.Location = new System.Drawing.Point(127, 25);
            this.cooldowncbo.Name = "cooldowncbo";
            this.cooldowncbo.Size = new System.Drawing.Size(77, 23);
            this.cooldowncbo.TabIndex = 1;
            this.cooldowncbo.SelectedIndexChanged += new System.EventHandler(this.cooldowncbo_SelectedIndexChanged);
            // 
            // modecbo
            // 
            this.modecbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modecbo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modecbo.FormattingEnabled = true;
            this.modecbo.Location = new System.Drawing.Point(10, 25);
            this.modecbo.Name = "modecbo";
            this.modecbo.Size = new System.Drawing.Size(100, 23);
            this.modecbo.TabIndex = 0;
            this.modecbo.SelectedIndexChanged += new System.EventHandler(this.modecbo_SelectedIndexChanged);
            // 
            // pgClass
            // 
            this.pgClass.Location = new System.Drawing.Point(400, 89);
            this.pgClass.Name = "pgClass";
            this.pgClass.Size = new System.Drawing.Size(390, 384);
            this.pgClass.TabIndex = 9;
            this.pgClass.ToolbarVisible = false;
            // 
            // pgMain
            // 
            this.pgMain.Location = new System.Drawing.Point(4, 89);
            this.pgMain.Name = "pgMain";
            this.pgMain.Size = new System.Drawing.Size(390, 384);
            this.pgMain.TabIndex = 10;
            this.pgMain.ToolbarVisible = false;
            // 
            // wulfToolStripMenuItem1
            // 
            this.wulfToolStripMenuItem1.Name = "wulfToolStripMenuItem1";
            this.wulfToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.wulfToolStripMenuItem1.Text = "Wulf";
            // 
            // stormchasingToolStripMenuItem1
            // 
            this.stormchasingToolStripMenuItem1.Name = "stormchasingToolStripMenuItem1";
            this.stormchasingToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.stormchasingToolStripMenuItem1.Text = "Stormchasing";
            // 
            // weishbierToolStripMenuItem
            // 
            this.weishbierToolStripMenuItem.Name = "weishbierToolStripMenuItem";
            this.weishbierToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.weishbierToolStripMenuItem.Text = "Weischbier";
            // 
            // handnaviToolStripMenuItem
            // 
            this.handnaviToolStripMenuItem.Name = "handnaviToolStripMenuItem";
            this.handnaviToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.handnaviToolStripMenuItem.Text = "Handnavi";
            // 
            // dagradtToolStripMenuItem
            // 
            this.dagradtToolStripMenuItem.Name = "dagradtToolStripMenuItem";
            this.dagradtToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.dagradtToolStripMenuItem.Text = "Dagradt";
            // 
            // mirabisToolStripMenuItem
            // 
            this.mirabisToolStripMenuItem.Name = "mirabisToolStripMenuItem";
            this.mirabisToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.mirabisToolStripMenuItem.Text = "Mirabis";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(794, 555);
            this.Controls.Add(this.pgMain);
            this.Controls.Add(this.pgClass);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 583);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 583);
            this.Name = "ConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "ConfigurationForm";
            this.Load += new System.EventHandler(this.ConfigurationFormLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedtoolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem creditsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stripComboBox;
        private System.Windows.Forms.ToolStripMenuItem updatebtn;
        private System.Windows.Forms.ToolStripMenuItem apocToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetHotkeysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwyasOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadRoutineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem routineManagerToolStripMenuItem;
        private ToolStripMenuItem repDevsToolStripMenuItem;
        private ToolStripMenuItem alToolStripMenuItem;
        private ToolStripMenuItem amaToolStripMenuItem;
        private ToolStripMenuItem debugToolStripMenuItem;
        private Label label1;
        private ToolStripMenuItem laoToolStripMenuItem;
        private Label label2;
        private PictureBox pictureBox1;
        private Panel panel4;
        private ToolStripMenuItem nomnomnomToolStripMenuItem;
        private ToolStripMenuItem bobby53ToolStripMenuItem;
        private Panel panel5;
        private Label lblPause;
        private Label lblAoE;
        private Label lblCD;
        private Label label3;
        private ComboBox pausecbo;
        private ComboBox switchcbo;
        private ComboBox cooldowncbo;
        private ComboBox modecbo;
        private Label lblSpecial;
        private ComboBox specialcbo;
        private Label lblMod;
        private ComboBox altkeyCBO;
        private PropertyGrid pgClass;
        private PropertyGrid pgMain;
        private Label lblRotation;
        private ComboBox rotationCBO;
        private ToolStripMenuItem healingDebugerToolStripMenuItem;
        private ToolStripMenuItem millzToolStripMenuItem;
        private ToolStripMenuItem treekToolStripMenuItem;
        private ToolStripMenuItem worklifebalanceToolStripMenuItem;
        private ToolStripMenuItem wulfToolStripMenuItem1;
        private ToolStripMenuItem stormchasingToolStripMenuItem1;
        private ToolStripMenuItem weishbierToolStripMenuItem;
        private ToolStripMenuItem handnaviToolStripMenuItem;
        private ToolStripMenuItem dagradtToolStripMenuItem;
        private ToolStripMenuItem mirabisToolStripMenuItem;
    }
}