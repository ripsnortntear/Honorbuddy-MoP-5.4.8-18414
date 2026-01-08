namespace AntiCap
{
    partial class AntiCapForm
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
            this.buyItemsList = new System.Windows.Forms.DataGridView();
            this.addItemsButton = new System.Windows.Forms.Button();
            this.loadItemsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.buyItemsList)).BeginInit();
            this.SuspendLayout();
            // 
            // buyItemsList
            // 
            this.buyItemsList.AllowUserToAddRows = false;
            this.buyItemsList.AllowUserToResizeColumns = false;
            this.buyItemsList.AllowUserToResizeRows = false;
            this.buyItemsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.buyItemsList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.buyItemsList.Location = new System.Drawing.Point(12, 12);
            this.buyItemsList.MultiSelect = false;
            this.buyItemsList.Name = "buyItemsList";
            this.buyItemsList.RowHeadersVisible = false;
            this.buyItemsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.buyItemsList.ShowEditingIcon = false;
            this.buyItemsList.Size = new System.Drawing.Size(409, 324);
            this.buyItemsList.TabIndex = 0;
            this.buyItemsList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.buyItemsList_CellFormatting);
            this.buyItemsList.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.buyItemsList_RowsRemoved);
            // 
            // addItemsButton
            // 
            this.addItemsButton.Location = new System.Drawing.Point(346, 351);
            this.addItemsButton.Name = "addItemsButton";
            this.addItemsButton.Size = new System.Drawing.Size(75, 23);
            this.addItemsButton.TabIndex = 1;
            this.addItemsButton.Text = "Add Items";
            this.addItemsButton.UseVisualStyleBackColor = true;
            this.addItemsButton.Click += new System.EventHandler(this.addItemsButton_Click);
            // 
            // loadItemsButton
            // 
            this.loadItemsButton.Location = new System.Drawing.Point(13, 351);
            this.loadItemsButton.Name = "loadItemsButton";
            this.loadItemsButton.Size = new System.Drawing.Size(75, 23);
            this.loadItemsButton.TabIndex = 2;
            this.loadItemsButton.Text = "Load Items";
            this.loadItemsButton.UseVisualStyleBackColor = true;
            this.loadItemsButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // AntiCapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 390);
            this.Controls.Add(this.loadItemsButton);
            this.Controls.Add(this.addItemsButton);
            this.Controls.Add(this.buyItemsList);
            this.Name = "AntiCapForm";
            this.Text = "AntiCapForm";
            ((System.ComponentModel.ISupportInitialize)(this.buyItemsList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView buyItemsList;
        public System.Windows.Forms.Button addItemsButton;
        private System.Windows.Forms.Button loadItemsButton;
    }
}