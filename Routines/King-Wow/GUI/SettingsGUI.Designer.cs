namespace KingWoW
{
    partial class KingHealSettingsGUIFORM
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ClassSpecificPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.UtilityGrid = new System.Windows.Forms.PropertyGrid();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(460, 320);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ClassSpecificPropertyGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(452, 294);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Spec Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ClassSpecificPropertyGrid
            // 
            this.ClassSpecificPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClassSpecificPropertyGrid.Location = new System.Drawing.Point(7, 17);
            this.ClassSpecificPropertyGrid.Name = "ClassSpecificPropertyGrid";
            this.ClassSpecificPropertyGrid.Size = new System.Drawing.Size(439, 281);
            this.ClassSpecificPropertyGrid.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.UtilityGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(452, 294);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Utility Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // UtilityGrid
            // 
            this.UtilityGrid.Location = new System.Drawing.Point(7, 7);
            this.UtilityGrid.Name = "UtilityGrid";
            this.UtilityGrid.Size = new System.Drawing.Size(439, 281);
            this.UtilityGrid.TabIndex = 1;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(397, 334);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(16, 338);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Load";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(244, 334);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save as new settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // KingHealSettingsGUIFORM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "KingHealSettingsGUIFORM";
            this.Text = "King-WoW Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.KingHealSettingsGUIFORM_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PropertyGrid ClassSpecificPropertyGrid;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.PropertyGrid UtilityGrid;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}