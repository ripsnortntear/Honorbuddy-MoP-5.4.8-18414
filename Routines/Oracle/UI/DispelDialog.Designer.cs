namespace Oracle.UI
{
    partial class DispelDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblRange = new System.Windows.Forms.Label();
            this.lblStackCount = new System.Windows.Forms.Label();
            this.lblDispelType = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.cmbDispelType = new System.Windows.Forms.ComboBox();
            this.txtRange = new System.Windows.Forms.TextBox();
            this.txtStackCount = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.lblDelayType = new System.Windows.Forms.Label();
            this.cmbDisDelayType = new System.Windows.Forms.ComboBox();
            this.lbldelayinfo = new System.Windows.Forms.Label();
            this.lbldelayinfo2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbldelayinfo2);
            this.groupBox1.Controls.Add(this.lbldelayinfo);
            this.groupBox1.Controls.Add(this.lblDelayType);
            this.groupBox1.Controls.Add(this.cmbDisDelayType);
            this.groupBox1.Controls.Add(this.lblDelay);
            this.groupBox1.Controls.Add(this.lblRange);
            this.groupBox1.Controls.Add(this.lblStackCount);
            this.groupBox1.Controls.Add(this.lblDispelType);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.lblID);
            this.groupBox1.Controls.Add(this.txtDelay);
            this.groupBox1.Controls.Add(this.cmbDispelType);
            this.groupBox1.Controls.Add(this.txtRange);
            this.groupBox1.Controls.Add(this.txtStackCount);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.txtID);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 298);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Edit/Delete";
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(20, 165);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(59, 13);
            this.lblDelay.TabIndex = 11;
            this.lblDelay.Text = "Delay (ms):";
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Location = new System.Drawing.Point(20, 137);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(52, 13);
            this.lblRange.TabIndex = 10;
            this.lblRange.Text = "Distance:";
            // 
            // lblStackCount
            // 
            this.lblStackCount.AutoSize = true;
            this.lblStackCount.Location = new System.Drawing.Point(20, 109);
            this.lblStackCount.Name = "lblStackCount";
            this.lblStackCount.Size = new System.Drawing.Size(66, 13);
            this.lblStackCount.TabIndex = 9;
            this.lblStackCount.Text = "StackCount:";
            // 
            // lblDispelType
            // 
            this.lblDispelType.AutoSize = true;
            this.lblDispelType.Location = new System.Drawing.Point(20, 80);
            this.lblDispelType.Name = "lblDispelType";
            this.lblDispelType.Size = new System.Drawing.Size(66, 13);
            this.lblDispelType.TabIndex = 8;
            this.lblDispelType.Text = "Dispel Type:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 52);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(64, 13);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Spell Name:";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(20, 24);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(47, 13);
            this.lblID.TabIndex = 6;
            this.lblID.Text = "Spell ID:";
            // 
            // txtDelay
            // 
            this.txtDelay.Location = new System.Drawing.Point(104, 161);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(159, 20);
            this.txtDelay.TabIndex = 5;
            // 
            // cmbDispelType
            // 
            this.cmbDispelType.FormattingEnabled = true;
            this.cmbDispelType.Location = new System.Drawing.Point(104, 76);
            this.cmbDispelType.Name = "cmbDispelType";
            this.cmbDispelType.Size = new System.Drawing.Size(159, 21);
            this.cmbDispelType.TabIndex = 4;
            this.cmbDispelType.SelectedIndexChanged += new System.EventHandler(this.cmbDispelType_SelectedIndexChanged);
            this.cmbDispelType.SelectionChangeCommitted += new System.EventHandler(this.cmbDispelType_SelectedIndexChanged);
            this.cmbDispelType.SelectedValueChanged += new System.EventHandler(this.cmbDispelType_SelectedIndexChanged);
            // 
            // txtRange
            // 
            this.txtRange.Location = new System.Drawing.Point(104, 133);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(159, 20);
            this.txtRange.TabIndex = 3;
            // 
            // txtStackCount
            // 
            this.txtStackCount.Location = new System.Drawing.Point(104, 105);
            this.txtStackCount.Name = "txtStackCount";
            this.txtStackCount.Size = new System.Drawing.Size(159, 20);
            this.txtStackCount.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(104, 48);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(159, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(104, 20);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(159, 20);
            this.txtID.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnApply.Location = new System.Drawing.Point(34, 326);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(198, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNew
            // 
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNew.Location = new System.Drawing.Point(116, 326);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lblDelayType
            // 
            this.lblDelayType.AutoSize = true;
            this.lblDelayType.Location = new System.Drawing.Point(20, 193);
            this.lblDelayType.Name = "lblDelayType";
            this.lblDelayType.Size = new System.Drawing.Size(64, 13);
            this.lblDelayType.TabIndex = 13;
            this.lblDelayType.Text = "Delay Type:";
            // 
            // cmbDisDelayType
            // 
            this.cmbDisDelayType.FormattingEnabled = true;
            this.cmbDisDelayType.Location = new System.Drawing.Point(104, 189);
            this.cmbDisDelayType.Name = "cmbDisDelayType";
            this.cmbDisDelayType.Size = new System.Drawing.Size(159, 21);
            this.cmbDisDelayType.TabIndex = 12;
            // 
            // lbldelayinfo
            // 
            this.lbldelayinfo.AutoSize = true;
            this.lbldelayinfo.Location = new System.Drawing.Point(28, 223);
            this.lbldelayinfo.Name = "lbldelayinfo";
            this.lbldelayinfo.Size = new System.Drawing.Size(233, 13);
            this.lbldelayinfo.TabIndex = 14;
            this.lbldelayinfo.Text = "CountingUp: is a Debuff that starts at 0 seconds";
            // 
            // lbldelayinfo2
            // 
            this.lbldelayinfo2.AutoSize = true;
            this.lbldelayinfo2.Location = new System.Drawing.Point(28, 260);
            this.lbldelayinfo2.Name = "lbldelayinfo2";
            this.lbldelayinfo2.Size = new System.Drawing.Size(240, 13);
            this.lbldelayinfo2.TabIndex = 15;
            this.lbldelayinfo2.Text = "CountingDown: is a Debuff that counts down to 0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Debuff time > Delay";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Debuff time <= Delay";
            // 
            // DispelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 364);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DispelDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dispelable Spells";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtStackCount;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.ComboBox cmbDispelType;
        private System.Windows.Forms.TextBox txtRange;
        private System.Windows.Forms.TextBox txtDelay;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.Label lblStackCount;
        private System.Windows.Forms.Label lblDispelType;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label lblDelayType;
        private System.Windows.Forms.ComboBox cmbDisDelayType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbldelayinfo2;
        private System.Windows.Forms.Label lbldelayinfo;
    }
}