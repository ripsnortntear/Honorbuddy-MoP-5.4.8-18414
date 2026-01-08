namespace TakeControl.Gui
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
            this.labVersion = new System.Windows.Forms.Label();
            this.btnHbPause = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numBlTargetTime = new System.Windows.Forms.NumericUpDown();
            this.btnBlTarget = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numBlAllObjectsTime = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numBlAllObjectsRadius = new System.Windows.Forms.NumericUpDown();
            this.btnBlAllObjects = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnHbStartOrStop = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnHbRestart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlTargetTime)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlAllObjectsTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlAllObjectsRadius)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "TakeControl!";
            // 
            // labVersion
            // 
            this.labVersion.AutoSize = true;
            this.labVersion.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labVersion.Location = new System.Drawing.Point(178, 23);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(32, 14);
            this.labVersion.TabIndex = 1;
            this.labVersion.Text = "Ver.";
            // 
            // btnHbPause
            // 
            this.btnHbPause.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHbPause.Location = new System.Drawing.Point(6, 25);
            this.btnHbPause.Name = "btnHbPause";
            this.btnHbPause.Size = new System.Drawing.Size(203, 23);
            this.btnHbPause.TabIndex = 2;
            this.btnHbPause.Text = "Press to bind";
            this.btnHbPause.UseVisualStyleBackColor = true;
            this.btnHbPause.Click += new System.EventHandler(this.BtnHbControlBindingClick);
            this.btnHbPause.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BtnHbControlBindingKeyDown);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(80, 525);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "~ ZenLulz ~";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnHbPause);
            this.groupBox1.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 60);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HonorBuddy Suspend/Resume";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numBlTargetTime);
            this.groupBox2.Controls.Add(this.btnBlTarget);
            this.groupBox2.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 206);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 106);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Blacklist Current Target";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "Time to blacklist (second)";
            // 
            // numBlTargetTime
            // 
            this.numBlTargetTime.Location = new System.Drawing.Point(6, 73);
            this.numBlTargetTime.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numBlTargetTime.Name = "numBlTargetTime";
            this.numBlTargetTime.Size = new System.Drawing.Size(203, 20);
            this.numBlTargetTime.TabIndex = 8;
            this.numBlTargetTime.ValueChanged += new System.EventHandler(this.NumBlTargetTimeValueChanged);
            // 
            // btnBlTarget
            // 
            this.btnBlTarget.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBlTarget.Location = new System.Drawing.Point(6, 25);
            this.btnBlTarget.Name = "btnBlTarget";
            this.btnBlTarget.Size = new System.Drawing.Size(203, 23);
            this.btnBlTarget.TabIndex = 2;
            this.btnBlTarget.Text = "Press to bind";
            this.btnBlTarget.UseVisualStyleBackColor = true;
            this.btnBlTarget.Click += new System.EventHandler(this.BtnBlTargetClick);
            this.btnBlTarget.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BtnBlTargetKeyDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.numBlAllObjectsTime);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numBlAllObjectsRadius);
            this.groupBox3.Controls.Add(this.btnBlAllObjects);
            this.groupBox3.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 53);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 147);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Blacklist All Objects";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "Time to blacklist (second)";
            // 
            // numBlAllObjectsTime
            // 
            this.numBlAllObjectsTime.Location = new System.Drawing.Point(6, 116);
            this.numBlAllObjectsTime.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numBlAllObjectsTime.Name = "numBlAllObjectsTime";
            this.numBlAllObjectsTime.Size = new System.Drawing.Size(203, 20);
            this.numBlAllObjectsTime.TabIndex = 6;
            this.numBlAllObjectsTime.ValueChanged += new System.EventHandler(this.NumBlAllObjectsTimeValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Radius (yard)";
            // 
            // numBlAllObjectsRadius
            // 
            this.numBlAllObjectsRadius.Location = new System.Drawing.Point(6, 71);
            this.numBlAllObjectsRadius.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numBlAllObjectsRadius.Name = "numBlAllObjectsRadius";
            this.numBlAllObjectsRadius.Size = new System.Drawing.Size(203, 20);
            this.numBlAllObjectsRadius.TabIndex = 4;
            this.numBlAllObjectsRadius.ValueChanged += new System.EventHandler(this.NumBlAllObjectsRadiusValueChanged);
            // 
            // btnBlAllObjects
            // 
            this.btnBlAllObjects.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBlAllObjects.Location = new System.Drawing.Point(6, 25);
            this.btnBlAllObjects.Name = "btnBlAllObjects";
            this.btnBlAllObjects.Size = new System.Drawing.Size(203, 23);
            this.btnBlAllObjects.TabIndex = 2;
            this.btnBlAllObjects.Text = "Press to bind";
            this.btnBlAllObjects.UseVisualStyleBackColor = true;
            this.btnBlAllObjects.Click += new System.EventHandler(this.BtnBlAllObjectsClick);
            this.btnBlAllObjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BtnBlAllObjectsKeyDown);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnHbStartOrStop);
            this.groupBox5.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(12, 384);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(215, 60);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "HonorBuddy Start/Stop";
            // 
            // btnHbStartOrStop
            // 
            this.btnHbStartOrStop.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHbStartOrStop.Location = new System.Drawing.Point(6, 25);
            this.btnHbStartOrStop.Name = "btnHbStartOrStop";
            this.btnHbStartOrStop.Size = new System.Drawing.Size(203, 23);
            this.btnHbStartOrStop.TabIndex = 2;
            this.btnHbStartOrStop.Text = "Press to bind";
            this.btnHbStartOrStop.UseVisualStyleBackColor = true;
            this.btnHbStartOrStop.Click += new System.EventHandler(this.BtnHbStartOrStopClick);
            this.btnHbStartOrStop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BtnHbStartOrStopKeyDown);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnHbRestart);
            this.groupBox6.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(12, 450);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(215, 60);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "HonorBuddy Restart";
            // 
            // btnHbRestart
            // 
            this.btnHbRestart.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHbRestart.Location = new System.Drawing.Point(6, 25);
            this.btnHbRestart.Name = "btnHbRestart";
            this.btnHbRestart.Size = new System.Drawing.Size(203, 23);
            this.btnHbRestart.TabIndex = 2;
            this.btnHbRestart.Text = "Press to bind";
            this.btnHbRestart.UseVisualStyleBackColor = true;
            this.btnHbRestart.Click += new System.EventHandler(this.BtnHbRestartClick);
            this.btnHbRestart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BtnHbRestartKeyDown);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 548);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labVersion);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TakeControl! ~ ZenLulz";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlTargetTime)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlAllObjectsTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlAllObjectsRadius)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.Button btnHbPause;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBlTarget;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numBlAllObjectsRadius;
        private System.Windows.Forms.Button btnBlAllObjects;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBlAllObjectsTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnHbStartOrStop;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnHbRestart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numBlTargetTime;
    }
}