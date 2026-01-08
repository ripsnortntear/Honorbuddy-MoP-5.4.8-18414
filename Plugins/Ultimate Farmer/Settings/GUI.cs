using System;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Styx;
using Styx.WoWInternals.WoWObjects;


namespace UltimateFarmer.Settings
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            pgSettings.SelectedObject = UFSettings.Instance;
            UFSettings.Instance.Load();
        }
				
        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            UFSettings.Instance.Save();
          
        }
		
		private void pgSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (pgSettings.SelectedObject != null && pgSettings.SelectedObject is UFSettings)
                ((UFSettings)pgSettings.SelectedObject).Save();
        }
		
        private void pgSettings_Click(object sender, EventArgs e)
        {

        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks! All donations are really welcome");
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=S46L4ZUWT7D4C");
        }

		private void NameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Redirecting to HB Forum Link");
            Process.Start("http://www.thebuddyforum.com/honorbuddy-forum/plugins/combat/148369-plugin-ultimate-farmer-multi-puller-honorbuddy-plugin-farmers.html");
        }
        
		
		
		
		
		
				
    }
}
