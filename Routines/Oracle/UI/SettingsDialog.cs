#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/SettingsDialog.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Windows.Forms;

namespace Oracle.UI
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();

            lblSettingsInformation.Text = @" Settings are Split into two files. General Settings and Class
 Specific Settings. Please Choose which settings file you
 would like to work with.";

            btnClass.BackColor = Config.GetClassColor();
        }

        private void btnGeneral_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void btnClass_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}