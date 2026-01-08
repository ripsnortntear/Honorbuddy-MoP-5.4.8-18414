using System;
using System.Collections.Generic;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Styx.Plugins;

namespace GroupGreet
{
    public partial class GroupGreeterCFG : Form
    {

        public GroupGreeterCFG()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GroupGreeterSettings.Instance.Load();
            enable.Checked = GroupGreeterSettings.Instance.gg_enable;
            GreetText1.Text = GroupGreeterSettings.Instance.greeting1;
            GreetText2.Text = GroupGreeterSettings.Instance.greeting2;
            GreetText3.Text = GroupGreeterSettings.Instance.greeting3;
            GreetText4.Text = GroupGreeterSettings.Instance.greeting4;
            GreetText5.Text = GroupGreeterSettings.Instance.greeting5;
            g1.Checked = GroupGreeterSettings.Instance.g1_enable;
            g2.Checked = GroupGreeterSettings.Instance.g2_enable;
            g3.Checked = GroupGreeterSettings.Instance.g3_enable;
            g4.Checked = GroupGreeterSettings.Instance.g4_enable;
            g5.Checked = GroupGreeterSettings.Instance.g5_enable;
            tankmarking.Checked = GroupGreeterSettings.Instance.tank_marking;
        }

        

            private void save_Click(object sender, System.EventArgs e)
            {
                GroupGreeterSettings.Instance.gg_enable = enable.Checked;
                GroupGreeterSettings.Instance.greeting1 = GreetText1.Text;
                GroupGreeterSettings.Instance.greeting2 = GreetText2.Text;
                GroupGreeterSettings.Instance.greeting3 = GreetText3.Text;
                GroupGreeterSettings.Instance.greeting4 = GreetText4.Text;
                GroupGreeterSettings.Instance.greeting5 = GreetText5.Text;
                GroupGreeterSettings.Instance.g1_enable = g1.Checked;
                GroupGreeterSettings.Instance.g2_enable = g2.Checked;
                GroupGreeterSettings.Instance.g3_enable = g3.Checked;
                GroupGreeterSettings.Instance.g4_enable = g4.Checked;
                GroupGreeterSettings.Instance.g5_enable = g5.Checked;
                GroupGreeterSettings.Instance.tank_marking = tankmarking.Checked;
                GroupGreeterSettings.Instance.Save();
                Logging.Write("[GroupGreet]: Config saved!");
                Close();
            }

            private void GreetText_TextChanged(object sender, System.EventArgs e)
            {

            }
    }
}
