namespace MageHelper
{
    partial class UI
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
            this.Save = new System.Windows.Forms.Button();
            this.useblink = new System.Windows.Forms.CheckBox();
            this.useslowfall = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.invisadds = new System.Windows.Forms.NumericUpDown();
            this.invishp = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.useinvis = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbIceBarrier = new System.Windows.Forms.CheckBox();
            this.cbBlinkKill = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bSaveClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.invisadds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invishp)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(18, 324);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // useblink
            // 
            this.useblink.AutoSize = true;
            this.useblink.Location = new System.Drawing.Point(12, 12);
            this.useblink.Name = "useblink";
            this.useblink.Size = new System.Drawing.Size(117, 17);
            this.useblink.TabIndex = 2;
            this.useblink.Text = "Use Blink - To Loot";
            this.useblink.UseVisualStyleBackColor = true;
            this.useblink.CheckedChanged += new System.EventHandler(this.useblink_CheckedChanged);
            // 
            // useslowfall
            // 
            this.useslowfall.AutoSize = true;
            this.useslowfall.Location = new System.Drawing.Point(12, 77);
            this.useslowfall.Name = "useslowfall";
            this.useslowfall.Size = new System.Drawing.Size(90, 17);
            this.useslowfall.TabIndex = 3;
            this.useslowfall.Text = "Use Slow Fall";
            this.useslowfall.UseVisualStyleBackColor = true;
            this.useslowfall.CheckedChanged += new System.EventHandler(this.useslowfall_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(161, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "<--Blinks to Loot POI";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(137, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "<--Slow Fall cast if Falling";
            // 
            // invisadds
            // 
            this.invisadds.Location = new System.Drawing.Point(6, 107);
            this.invisadds.Name = "invisadds";
            this.invisadds.Size = new System.Drawing.Size(87, 20);
            this.invisadds.TabIndex = 5;
            this.invisadds.ValueChanged += new System.EventHandler(this.invisadds_ValueChanged);
            // 
            // invishp
            // 
            this.invishp.Location = new System.Drawing.Point(116, 107);
            this.invishp.Name = "invishp";
            this.invishp.Size = new System.Drawing.Size(80, 20);
            this.invishp.TabIndex = 6;
            this.invishp.ValueChanged += new System.EventHandler(this.invishp_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 65);
            this.label3.TabIndex = 9;
            this.label3.Text = "Will cast Invisbility in combat.\r\nThis will depend on how many adds\r\nare targetti" +
                "ng you, and at what percent\r\nof health you want to us it at.\r\nThese two settings" +
                " follow.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Number of Mobs";
            // 
            // useinvis
            // 
            this.useinvis.AutoSize = true;
            this.useinvis.BackColor = System.Drawing.SystemColors.Control;
            this.useinvis.Location = new System.Drawing.Point(6, 19);
            this.useinvis.Name = "useinvis";
            this.useinvis.Size = new System.Drawing.Size(92, 17);
            this.useinvis.TabIndex = 4;
            this.useinvis.Text = "Use Invisibility";
            this.useinvis.UseVisualStyleBackColor = false;
            this.useinvis.CheckedChanged += new System.EventHandler(this.useinvis_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(116, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Percent Health";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.useinvis);
            this.groupBox1.Controls.Add(this.invishp);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.invisadds);
            this.groupBox1.Location = new System.Drawing.Point(12, 166);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 152);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Invisibility";
            // 
            // cbIceBarrier
            // 
            this.cbIceBarrier.AutoSize = true;
            this.cbIceBarrier.Location = new System.Drawing.Point(12, 100);
            this.cbIceBarrier.Name = "cbIceBarrier";
            this.cbIceBarrier.Size = new System.Drawing.Size(119, 17);
            this.cbIceBarrier.TabIndex = 13;
            this.cbIceBarrier.Text = "Keep Up Ice Barrier";
            this.cbIceBarrier.UseVisualStyleBackColor = true;
            this.cbIceBarrier.CheckedChanged += new System.EventHandler(this.cbIceBarrier_CheckedChanged);
            // 
            // cbBlinkKill
            // 
            this.cbBlinkKill.AutoSize = true;
            this.cbBlinkKill.Location = new System.Drawing.Point(12, 35);
            this.cbBlinkKill.Name = "cbBlinkKill";
            this.cbBlinkKill.Size = new System.Drawing.Size(143, 17);
            this.cbBlinkKill.TabIndex = 14;
            this.cbBlinkKill.Text = "Use Blink - To Kill Target";
            this.cbBlinkKill.UseVisualStyleBackColor = true;
            this.cbBlinkKill.CheckedChanged += new System.EventHandler(this.cbBlinkKill_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(174, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "<--Blink to Kill POI";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(148, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "<--Ice Barrier in Combat";
            // 
            // bSaveClose
            // 
            this.bSaveClose.Location = new System.Drawing.Point(99, 324);
            this.bSaveClose.Name = "bSaveClose";
            this.bSaveClose.Size = new System.Drawing.Size(163, 23);
            this.bSaveClose.TabIndex = 17;
            this.bSaveClose.Text = "Save + Close";
            this.bSaveClose.UseVisualStyleBackColor = true;
            this.bSaveClose.Click += new System.EventHandler(this.bSaveClose_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 359);
            this.Controls.Add(this.bSaveClose);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbBlinkKill);
            this.Controls.Add(this.cbIceBarrier);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.useslowfall);
            this.Controls.Add(this.useblink);
            this.Controls.Add(this.Save);
            this.Name = "UI";
            this.Text = "Mage Helper Config";
            this.Load += new System.EventHandler(this.MHelperConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.invisadds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invishp)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.CheckBox useblink;
        private System.Windows.Forms.CheckBox useslowfall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown invisadds;
        private System.Windows.Forms.NumericUpDown invishp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox useinvis;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbIceBarrier;
        private System.Windows.Forms.CheckBox cbBlinkKill;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bSaveClose;
    }
}