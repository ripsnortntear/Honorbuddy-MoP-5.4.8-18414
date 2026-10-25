using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowAndBotPersistence
{
    public class SettingsForm : Form
    {
        private CheckBox chkRememberBotBase;
        private CheckBox chkRememberWindow;
        private Button btnResetWindow;
        private Button btnSave;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.chkRememberBotBase = new CheckBox();
            this.chkRememberWindow = new CheckBox();
            this.btnResetWindow = new Button();
            this.btnSave = new Button();
            this.SuspendLayout();

            // chkRememberBotBase
            this.chkRememberBotBase.AutoSize = true;
            this.chkRememberBotBase.Location = new Point(20, 20);
            this.chkRememberBotBase.Name = "chkRememberBotBase";
            this.chkRememberBotBase.Size = new Size(260, 17);
            this.chkRememberBotBase.TabIndex = 0;
            this.chkRememberBotBase.Text = "Remember Last Selected BotBase (Per-Character)";
            this.chkRememberBotBase.UseVisualStyleBackColor = true;

            // chkRememberWindow
            this.chkRememberWindow.AutoSize = true;
            this.chkRememberWindow.Location = new Point(20, 50);
            this.chkRememberWindow.Name = "chkRememberWindow";
            this.chkRememberWindow.Size = new Size(275, 17);
            this.chkRememberWindow.TabIndex = 1;
            this.chkRememberWindow.Text = "Remember Window Position & Size (Per-Character)";
            this.chkRememberWindow.UseVisualStyleBackColor = true;

            // btnResetWindow
            this.btnResetWindow.Location = new Point(20, 85);
            this.btnResetWindow.Name = "btnResetWindow";
            this.btnResetWindow.Size = new Size(260, 26);
            this.btnResetWindow.TabIndex = 2;
            this.btnResetWindow.Text = "Reset Window to Center (550 x 450)";
            this.btnResetWindow.UseVisualStyleBackColor = true;
            this.btnResetWindow.Click += new EventHandler(this.btnResetWindow_Click);

            // btnSave
            this.btnSave.Location = new Point(180, 125);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 26);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save && Close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // Form
            this.ClientSize = new Size(310, 165);
            this.Controls.Add(this.chkRememberBotBase);
            this.Controls.Add(this.chkRememberWindow);
            this.Controls.Add(this.btnResetWindow);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Window & Bot Persistence";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadSettings()
        {
            var settings = WindowAndBotPersistenceSettings.Instance;
            chkRememberBotBase.Checked = settings.RememberBotBase;
            chkRememberWindow.Checked = settings.RememberWindowPosition;
        }

        private void btnResetWindow_Click(object sender, EventArgs e)
        {
            var settings = WindowAndBotPersistenceSettings.Instance;
            settings.WindowX = 200;
            settings.WindowY = 100;
            settings.WindowWidth = 550;
            settings.WindowHeight = 450;
            settings.WindowState = System.Windows.WindowState.Normal;
            settings.Save();

            WindowAndBotPersistencePlugin.ResetMainWindowBounds(200, 100, 550, 450);
            MessageBox.Show("Main window reset to default 550x450 position.", "Window Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = WindowAndBotPersistenceSettings.Instance;
            settings.RememberBotBase = chkRememberBotBase.Checked;
            settings.RememberWindowPosition = chkRememberWindow.Checked;
            settings.Save();
            this.Close();
        }
    }
}
