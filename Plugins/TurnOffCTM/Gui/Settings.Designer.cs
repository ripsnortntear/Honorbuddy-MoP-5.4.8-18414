namespace TurnOffCTM.Gui
{
    partial class Settings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboType = new System.Windows.Forms.ComboBox();
            this.CheckBotException = new System.Windows.Forms.CheckBox();
            this.TextBotName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(106, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "TurnOffCTM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type of desactivation :";
            // 
            // ComboType
            // 
            this.ComboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboType.FormattingEnabled = true;
            this.ComboType.Items.AddRange(new object[] {
            "OnBotStopped",
            "OnBotExited"});
            this.ComboType.Location = new System.Drawing.Point(171, 46);
            this.ComboType.Name = "ComboType";
            this.ComboType.Size = new System.Drawing.Size(150, 21);
            this.ComboType.TabIndex = 2;
            this.ComboType.SelectedIndexChanged += new System.EventHandler(this.ComboTypeSelectedIndexChanged);
            // 
            // CheckBotException
            // 
            this.CheckBotException.AutoSize = true;
            this.CheckBotException.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBotException.Location = new System.Drawing.Point(17, 77);
            this.CheckBotException.Name = "CheckBotException";
            this.CheckBotException.Size = new System.Drawing.Size(309, 21);
            this.CheckBotException.TabIndex = 3;
            this.CheckBotException.Text = "Disable when using                                   bot";
            this.CheckBotException.UseVisualStyleBackColor = true;
            this.CheckBotException.CheckedChanged += new System.EventHandler(this.CheckBotExceptionCheckedChanged);
            // 
            // TextBotName
            // 
            this.TextBotName.Enabled = false;
            this.TextBotName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBotName.Location = new System.Drawing.Point(171, 77);
            this.TextBotName.Name = "TextBotName";
            this.TextBotName.Size = new System.Drawing.Size(121, 23);
            this.TextBotName.TabIndex = 4;
            this.TextBotName.TextChanged += new System.EventHandler(this.TextBotNameTextChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 124);
            this.Controls.Add(this.TextBotName);
            this.Controls.Add(this.CheckBotException);
            this.Controls.Add(this.ComboType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TurnOffCTM ~ ZenLulz";
            this.Load += new System.EventHandler(this.SettingsLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboType;
        private System.Windows.Forms.CheckBox CheckBotException;
        private System.Windows.Forms.TextBox TextBotName;
    }
}