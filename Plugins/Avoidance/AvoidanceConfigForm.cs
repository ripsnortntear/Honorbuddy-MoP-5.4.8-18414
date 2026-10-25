using System;
using System.Drawing;
using System.Windows.Forms;

namespace Avoidance
{
    public class AvoidanceConfigForm : Form
    {
        private CheckBox _chkEnabled;
        private CheckBox _chkUseCtm;
        private CheckBox _chkRestoreCtm;
        private NumericUpDown _numSafetyBuffer;
        private CheckBox _chkAvoidDynObj;
        private CheckBox _chkAvoidAreaTrigger;
        private CheckBox _chkAvoidGroundAuras;
        private CheckBox _chkOnlyInCombat;
        private CheckBox _chkDebugLogging;
        private TextBox _txtWhitelist;
        private Button _btnSave;
        private Button _btnCancel;

        public AvoidanceConfigForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            Text = "Avoidance Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(380, 420);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            var y = 15;

            _chkEnabled = new CheckBox
            {
                Text = "Enable Avoidance Plugin",
                Location = new Point(20, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point)
            };
            Controls.Add(_chkEnabled);
            y += 30;

            _chkUseCtm = new CheckBox
            {
                Text = "Use Click To Move (CTM) during avoidance",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(_chkUseCtm);
            y += 25;

            _chkRestoreCtm = new CheckBox
            {
                Text = "Restore CTM to disabled state once safe (if originally off)",
                Location = new Point(35, y),
                AutoSize = true
            };
            Controls.Add(_chkRestoreCtm);
            y += 30;

            var lblBuffer = new Label
            {
                Text = "Safety Buffer Distance (yards beyond danger radius):",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(lblBuffer);
            y += 22;

            _numSafetyBuffer = new NumericUpDown
            {
                Location = new Point(23, y),
                Width = 80,
                DecimalPlaces = 1,
                Increment = 0.5M,
                Minimum = 0.5M,
                Maximum = 15.0M,
                Value = 2.5M
            };
            Controls.Add(_numSafetyBuffer);
            y += 35;

            _chkAvoidDynObj = new CheckBox
            {
                Text = "Avoid hostile Dynamic Objects (Void zones, Blizzard, puddles)",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(_chkAvoidDynObj);
            y += 25;

            _chkAvoidAreaTrigger = new CheckBox
            {
                Text = "Avoid Area Triggers (moving hazards, fire lines, traps)",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(_chkAvoidAreaTrigger);
            y += 25;

            _chkAvoidGroundAuras = new CheckBox
            {
                Text = "Avoid environmental ground auras on player (excludes status debuffs/DoTs)",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(_chkAvoidGroundAuras);
            y += 25;

            _chkOnlyInCombat = new CheckBox
            {
                Text = "Only avoid when player is in combat",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(_chkOnlyInCombat);
            y += 25;

            _chkDebugLogging = new CheckBox
            {
                Text = "Enable verbose debug logging",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(_chkDebugLogging);
            y += 30;

            var lblWhitelist = new Label
            {
                Text = "Spell Whitelist (IDs or names, comma separated):",
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(lblWhitelist);
            y += 20;

            _txtWhitelist = new TextBox
            {
                Location = new Point(22, y),
                Width = 338
            };
            Controls.Add(_txtWhitelist);
            y += 35;

            _btnSave = new Button
            {
                Text = "Save",
                Location = new Point(190, y),
                Size = new Size(80, 28),
                DialogResult = DialogResult.OK
            };
            _btnSave.Click += BtnSave_Click;
            Controls.Add(_btnSave);

            _btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(280, y),
                Size = new Size(80, 28),
                DialogResult = DialogResult.Cancel
            };
            _btnCancel.Click += (s, e) => Close();
            Controls.Add(_btnCancel);

            AcceptButton = _btnSave;
            CancelButton = _btnCancel;
        }

        private void LoadSettings()
        {
            var s = AvoidanceSettings.Instance;
            s.Load();
            _chkEnabled.Checked = s.Enabled;
            _chkUseCtm.Checked = s.UseClickToMove;
            _chkRestoreCtm.Checked = s.RestoreClickToMove;
            _numSafetyBuffer.Value = Math.Max(_numSafetyBuffer.Minimum, Math.Min(_numSafetyBuffer.Maximum, (decimal)s.SafetyBuffer));
            _chkAvoidDynObj.Checked = s.AvoidDynamicObjects;
            _chkAvoidAreaTrigger.Checked = s.AvoidAreaTriggers;
            _chkAvoidGroundAuras.Checked = s.AvoidGroundAuras;
            _chkOnlyInCombat.Checked = s.OnlyInCombat;
            _chkDebugLogging.Checked = s.DebugLogging;
            _txtWhitelist.Text = s.CustomWhitelist ?? "26573, 81297, 110701, 118283, Consecration";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var s = AvoidanceSettings.Instance;
            s.Enabled = _chkEnabled.Checked;
            s.UseClickToMove = _chkUseCtm.Checked;
            s.RestoreClickToMove = _chkRestoreCtm.Checked;
            s.SafetyBuffer = (float)_numSafetyBuffer.Value;
            s.AvoidDynamicObjects = _chkAvoidDynObj.Checked;
            s.AvoidAreaTriggers = _chkAvoidAreaTrigger.Checked;
            s.AvoidGroundAuras = _chkAvoidGroundAuras.Checked;
            s.OnlyInCombat = _chkOnlyInCombat.Checked;
            s.DebugLogging = _chkDebugLogging.Checked;
            s.CustomWhitelist = _txtWhitelist.Text.Trim();
            s.Save();
            Close();
        }
    }
}
