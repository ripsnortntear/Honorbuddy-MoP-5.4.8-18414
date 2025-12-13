namespace Oracle.UI
{
    partial class SettingsDialog
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
            this.btnGeneral = new System.Windows.Forms.Button();
            this.btnClass = new System.Windows.Forms.Button();
            this.lblSettingsInformation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGeneral
            // 
            this.btnGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGeneral.Location = new System.Drawing.Point(47, 75);
            this.btnGeneral.Name = "btnGeneral";
            this.btnGeneral.Size = new System.Drawing.Size(118, 51);
            this.btnGeneral.TabIndex = 0;
            this.btnGeneral.Text = "General Settings";
            this.btnGeneral.UseVisualStyleBackColor = true;
            this.btnGeneral.Click += new System.EventHandler(this.btnGeneral_Click);
            // 
            // btnClass
            // 
            this.btnClass.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClass.Location = new System.Drawing.Point(202, 75);
            this.btnClass.Name = "btnClass";
            this.btnClass.Size = new System.Drawing.Size(118, 51);
            this.btnClass.TabIndex = 1;
            this.btnClass.Text = "Class Settings";
            this.btnClass.UseVisualStyleBackColor = true;
            this.btnClass.Click += new System.EventHandler(this.btnClass_Click);
            // 
            // lblSettingsInformation
            // 
            this.lblSettingsInformation.AutoSize = true;
            this.lblSettingsInformation.Location = new System.Drawing.Point(44, 9);
            this.lblSettingsInformation.Name = "lblSettingsInformation";
            this.lblSettingsInformation.Size = new System.Drawing.Size(222, 13);
            this.lblSettingsInformation.TabIndex = 2;
            this.lblSettingsInformation.Text = "Please Choose which setting file to work with.";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 138);
            this.Controls.Add(this.lblSettingsInformation);
            this.Controls.Add(this.btnClass);
            this.Controls.Add(this.btnGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose a Settings File";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGeneral;
        private System.Windows.Forms.Button btnClass;
        private System.Windows.Forms.Label lblSettingsInformation;
    }
}