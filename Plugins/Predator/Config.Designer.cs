namespace Predator
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
            this.SkinMobs = new System.Windows.Forms.CheckBox();
			this.Pickpocket = new System.Windows.Forms.CheckBox();
			this.PickpocketOnly = new System.Windows.Forms.CheckBox();
            this.JustFarmCloth = new System.Windows.Forms.CheckBox();
            this.JustFarmLeather = new System.Windows.Forms.CheckBox();
            this.TimeToBlacklist = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
            this.BSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SkinMobs
            // 
            this.SkinMobs.Location = new System.Drawing.Point(12, 21);
            this.SkinMobs.Name = "SkinMobs";
            this.SkinMobs.Size = new System.Drawing.Size(100, 20);
            this.SkinMobs.TabIndex = 0;
            // 
            // JustFarmCloth
            // 
            this.JustFarmCloth.Location = new System.Drawing.Point(12, 58);
            this.JustFarmCloth.Name = "JustFarmCloth";
            this.JustFarmCloth.Size = new System.Drawing.Size(100, 20);
            this.JustFarmCloth.TabIndex = 1;
            // 
            // JustFarmLeather
            // 
            this.JustFarmLeather.Location = new System.Drawing.Point(12, 97);
            this.JustFarmLeather.Name = "JustFarmLeather";
            this.JustFarmLeather.Size = new System.Drawing.Size(100, 20);
            this.JustFarmLeather.TabIndex = 2;
            //
			// TimeToBlacklist
            // 
            this.TimeToBlacklist.Location = new System.Drawing.Point(12, 144);
            this.TimeToBlacklist.Name = "TimeToBlacklist";
            this.TimeToBlacklist.Size = new System.Drawing.Size(80, 20);
            this.TimeToBlacklist.TabIndex = 3;
			// 
            // Pickpocket
            // 
            this.Pickpocket.Location = new System.Drawing.Point(140, 21);
            this.Pickpocket.Name = "Pickpocket";
            this.Pickpocket.Size = new System.Drawing.Size(100, 20);
            this.Pickpocket.TabIndex = 10;
			// 
            // PickpocketOnly
            // 
            this.PickpocketOnly.Location = new System.Drawing.Point(140, 58);
            this.PickpocketOnly.Name = "PickpocketOnly";
            this.PickpocketOnly.Size = new System.Drawing.Size(100, 20);
            this.PickpocketOnly.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Skin Mobs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Just Farm Cloth";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Just Farm Leather";
			// 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Time To Blacklist Mob(Milliseconds)";
			// 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pickpocket Mobs Before Combat(Enable only if you are a rogue)";
			// 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(120, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Pickpocket Only Mode, Will Not Engage In Combat Or Skin Mobs.";
            // 
            // Save
            // 
            this.BSave.Location = new System.Drawing.Point(120, 175);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(75, 23);
            this.BSave.TabIndex = 8;
            this.BSave.Text = "Save";
            this.BSave.UseVisualStyleBackColor = true;
            this.BSave.Click += new System.EventHandler(this.BSave_Click);
            // 
            // PredatorGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 216);
            this.Controls.Add(this.BSave);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TimeToBlacklist);
            this.Controls.Add(this.SkinMobs);
            this.Controls.Add(this.JustFarmLeather);
			this.Controls.Add(this.JustFarmCloth);
			this.Controls.Add(this.Pickpocket);
			this.Controls.Add(this.PickpocketOnly);
            this.Name = "Config";
            this.Text = "Predator Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox SkinMobs;
        private System.Windows.Forms.CheckBox Pickpocket;
        private System.Windows.Forms.CheckBox PickpocketOnly;
        private System.Windows.Forms.CheckBox JustFarmCloth;
        private System.Windows.Forms.CheckBox JustFarmLeather;
        private System.Windows.Forms.TextBox TimeToBlacklist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BSave;
    }
}