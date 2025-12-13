#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/DispelDialog.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Debuffs;
using Oracle.Shared.Logging;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Oracle.UI
{
    public partial class DispelDialog : Form
    {
        public DispelDialog()
        {
            InitializeComponent();
        }

        public bool ValidRecordFound { get; private set; }

        private SpellEntry CurrentRecord { get; set; }

        private bool NewRecordStarted { get; set; }

        public void Init(int id)
        {
            ValidRecordFound = false;
            NewRecordStarted = false;

            cmbDispelType.DataSource = Enum.GetValues(typeof(DispelType));
            cmbDisDelayType.DataSource = Enum.GetValues(typeof(DispelDelayType));

            // find the DispelableSpell
            CurrentRecord = DispelableSpell.Instance.SpellList.Spells.Find(s => s.Id == id);

            if (CurrentRecord == null) return;

            ValidRecordFound = true;

            UpdateRestrictedControls(CurrentRecord.DisType);

            // Populate the Form..
            txtID.Text = CurrentRecord.Id.ToString(CultureInfo.InvariantCulture);
            txtName.Text = CurrentRecord.Name;
            txtRange.Text = CurrentRecord.Range.ToString(CultureInfo.InvariantCulture);
            txtDelay.Text = CurrentRecord.Delay.ToString(CultureInfo.InvariantCulture);
            txtStackCount.Text = CurrentRecord.StackCount.ToString(CultureInfo.InvariantCulture);
            cmbDispelType.SelectedItem = CurrentRecord.DisType;
            cmbDisDelayType.SelectedItem = CurrentRecord.DisDelayType;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (NewRecordStarted)
            {
                CreartNewRecord();
                DialogResult = DialogResult.OK;
                return;
            }

            var result = DispelableSpell.Instance.SpellList.Spells.Find(s => s.Id == CurrentRecord.Id);
            if (result == null) return;

            // Save to memory..
            result.Id = Convert.ToInt32(txtID.Text);
            result.Name = txtName.Text;
            result.Range = Convert.ToInt32(txtRange.Text);
            result.Delay = Convert.ToInt32(txtDelay.Text);
            result.StackCount = Convert.ToInt32(txtStackCount.Text);
            result.DisType = GetDispelType();
            result.DisDelayType = GetDispelDelayType();

            Logger.Output(" Dispel changes applied for {0}", CurrentRecord.Id);

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewRecordStarted = true;

            // Populate the Form..
            txtID.Text = "0";
            txtName.Text = "";
            txtRange.Text = "0";
            txtDelay.Text = "0";
            txtStackCount.Text = "0";
            cmbDispelType.SelectedItem = DispelType.None;
            cmbDisDelayType.SelectedItem = DispelDelayType.None;
        }

        private void cmbDispelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DispelType dspType;
            Enum.TryParse(cmbDispelType.SelectedValue.ToString(), out dspType);

            UpdateRestrictedControls(dspType);
        }

        private void CreartNewRecord()
        {
            var Name = txtName.Text;
            var Id = Convert.ToInt32(txtID.Text);
            var DisType = GetDispelType();
            var StackCount = Convert.ToInt32(txtStackCount.Text);
            var Range = Convert.ToInt32(txtRange.Text);
            var Delay = Convert.ToInt32(txtDelay.Text);
            var DisDelayType = GetDispelDelayType();

            DispelableSpell.Instance.SpellList.Add(Id, Name, DisType, DisDelayType, StackCount, Range, Delay);
            Logger.Output(string.Format("Name: {0} Id: {1}  DisType: {2}, DisDelayType: {6} Range: {3} StackCount: {4} Delay: {5}", Name, Id, DisType, Range, StackCount, Delay, DisDelayType));
        }

        private DispelType GetDispelType()
        {
            DispelType dspType;
            Enum.TryParse(cmbDispelType.SelectedValue.ToString(), out dspType);

            return dspType;
        }

        private DispelDelayType GetDispelDelayType()
        {
            DispelDelayType dspType;
            Enum.TryParse(cmbDisDelayType.SelectedValue.ToString(), out dspType);

            return dspType;
        }

        private void UpdateRestrictedControls(DispelType disType)
        {
            switch (disType)
            {
                case DispelType.BlackList:
                    txtDelay.Enabled = false;
                    txtRange.Enabled = false;
                    txtStackCount.Enabled = false;
                    cmbDisDelayType.Enabled = false;
                    break;

                case DispelType.Priority:
                    txtDelay.Enabled = false;
                    txtRange.Enabled = false;
                    txtStackCount.Enabled = false;
                    cmbDisDelayType.Enabled = false;
                    break;

                case DispelType.Delay:
                    txtDelay.Enabled = true;
                    txtRange.Enabled = false;
                    txtStackCount.Enabled = false;
                    cmbDisDelayType.Enabled = true;
                    break;

                case DispelType.Range:
                    txtDelay.Enabled = false;
                    txtRange.Enabled = true;
                    txtStackCount.Enabled = false;
                    cmbDisDelayType.Enabled = false;
                    break;

                case DispelType.Stack:
                    txtDelay.Enabled = false;
                    txtRange.Enabled = false;
                    txtStackCount.Enabled = true;
                    cmbDisDelayType.Enabled = false;
                    break;

                case DispelType.None:
                    txtDelay.Enabled = false;
                    txtRange.Enabled = false;
                    txtStackCount.Enabled = false;
                    cmbDisDelayType.Enabled = false;
                    break;
            }
        }
    }
}