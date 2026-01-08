namespace GroupGreet
{
    partial class GroupGreeterCFG
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.save = new System.Windows.Forms.Button();
            this.enable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GreetText1 = new System.Windows.Forms.TextBox();
            this.tankmarking = new System.Windows.Forms.CheckBox();
            this.g1 = new System.Windows.Forms.CheckBox();
            this.g2 = new System.Windows.Forms.CheckBox();
            this.GreetText2 = new System.Windows.Forms.TextBox();
            this.g3 = new System.Windows.Forms.CheckBox();
            this.GreetText3 = new System.Windows.Forms.TextBox();
            this.g4 = new System.Windows.Forms.CheckBox();
            this.GreetText4 = new System.Windows.Forms.TextBox();
            this.g5 = new System.Windows.Forms.CheckBox();
            this.GreetText5 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save.Location = new System.Drawing.Point(96, 199);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(92, 23);
            this.save.TabIndex = 0;
            this.save.Text = "save +  close";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // enable
            // 
            this.enable.AutoSize = true;
            this.enable.Location = new System.Drawing.Point(12, 36);
            this.enable.Name = "enable";
            this.enable.Size = new System.Drawing.Size(126, 17);
            this.enable.TabIndex = 1;
            this.enable.Text = "Enable GroupGreeter";
            this.enable.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(73, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "GroupGreeter";
            // 
            // GreetText1
            // 
            this.GreetText1.Location = new System.Drawing.Point(87, 57);
            this.GreetText1.MaxLength = 220;
            this.GreetText1.Name = "GreetText1";
            this.GreetText1.Size = new System.Drawing.Size(185, 20);
            this.GreetText1.TabIndex = 4;
            this.GreetText1.TextChanged += new System.EventHandler(this.GreetText_TextChanged);
            // 
            // tankmarking
            // 
            this.tankmarking.AutoSize = true;
            this.tankmarking.Enabled = false;
            this.tankmarking.Location = new System.Drawing.Point(12, 176);
            this.tankmarking.Name = "tankmarking";
            this.tankmarking.Size = new System.Drawing.Size(176, 17);
            this.tankmarking.TabIndex = 5;
            this.tankmarking.Text = "Enable Skull marking on bosses";
            this.tankmarking.UseVisualStyleBackColor = true;
            // 
            // g1
            // 
            this.g1.AutoSize = true;
            this.g1.Location = new System.Drawing.Point(12, 59);
            this.g1.Name = "g1";
            this.g1.Size = new System.Drawing.Size(69, 17);
            this.g1.TabIndex = 6;
            this.g1.Text = "Greeting:";
            this.g1.UseVisualStyleBackColor = true;
            // 
            // g2
            // 
            this.g2.AutoSize = true;
            this.g2.Location = new System.Drawing.Point(12, 83);
            this.g2.Name = "g2";
            this.g2.Size = new System.Drawing.Size(69, 17);
            this.g2.TabIndex = 8;
            this.g2.Text = "Greeting:";
            this.g2.UseVisualStyleBackColor = true;
            // 
            // GreetText2
            // 
            this.GreetText2.Location = new System.Drawing.Point(87, 81);
            this.GreetText2.MaxLength = 220;
            this.GreetText2.Name = "GreetText2";
            this.GreetText2.Size = new System.Drawing.Size(185, 20);
            this.GreetText2.TabIndex = 7;
            // 
            // g3
            // 
            this.g3.AutoSize = true;
            this.g3.Location = new System.Drawing.Point(12, 106);
            this.g3.Name = "g3";
            this.g3.Size = new System.Drawing.Size(69, 17);
            this.g3.TabIndex = 10;
            this.g3.Text = "Greeting:";
            this.g3.UseVisualStyleBackColor = true;
            // 
            // GreetText3
            // 
            this.GreetText3.Location = new System.Drawing.Point(87, 104);
            this.GreetText3.MaxLength = 220;
            this.GreetText3.Name = "GreetText3";
            this.GreetText3.Size = new System.Drawing.Size(185, 20);
            this.GreetText3.TabIndex = 9;
            // 
            // g4
            // 
            this.g4.AutoSize = true;
            this.g4.Location = new System.Drawing.Point(12, 129);
            this.g4.Name = "g4";
            this.g4.Size = new System.Drawing.Size(69, 17);
            this.g4.TabIndex = 12;
            this.g4.Text = "Greeting:";
            this.g4.UseVisualStyleBackColor = true;
            // 
            // GreetText4
            // 
            this.GreetText4.Location = new System.Drawing.Point(87, 127);
            this.GreetText4.MaxLength = 220;
            this.GreetText4.Name = "GreetText4";
            this.GreetText4.Size = new System.Drawing.Size(185, 20);
            this.GreetText4.TabIndex = 11;
            // 
            // g5
            // 
            this.g5.AutoSize = true;
            this.g5.Location = new System.Drawing.Point(12, 152);
            this.g5.Name = "g5";
            this.g5.Size = new System.Drawing.Size(69, 17);
            this.g5.TabIndex = 14;
            this.g5.Text = "Greeting:";
            this.g5.UseVisualStyleBackColor = true;
            // 
            // GreetText5
            // 
            this.GreetText5.Location = new System.Drawing.Point(87, 150);
            this.GreetText5.MaxLength = 220;
            this.GreetText5.Name = "GreetText5";
            this.GreetText5.Size = new System.Drawing.Size(185, 20);
            this.GreetText5.TabIndex = 13;
            // 
            // GroupGreeterCFG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 233);
            this.Controls.Add(this.g5);
            this.Controls.Add(this.GreetText5);
            this.Controls.Add(this.g4);
            this.Controls.Add(this.GreetText4);
            this.Controls.Add(this.g3);
            this.Controls.Add(this.GreetText3);
            this.Controls.Add(this.g2);
            this.Controls.Add(this.GreetText2);
            this.Controls.Add(this.g1);
            this.Controls.Add(this.tankmarking);
            this.Controls.Add(this.GreetText1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enable);
            this.Controls.Add(this.save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GroupGreeterCFG";
            this.Text = "GG v0.0.3.4 rev 11";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button save;
        private System.Windows.Forms.CheckBox enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GreetText1;
        private System.Windows.Forms.CheckBox tankmarking;
        private System.Windows.Forms.CheckBox g1;
        private System.Windows.Forms.CheckBox g2;
        private System.Windows.Forms.TextBox GreetText2;
        private System.Windows.Forms.CheckBox g3;
        private System.Windows.Forms.TextBox GreetText3;
        private System.Windows.Forms.CheckBox g4;
        private System.Windows.Forms.TextBox GreetText4;
        private System.Windows.Forms.CheckBox g5;
        private System.Windows.Forms.TextBox GreetText5;
    }
}

