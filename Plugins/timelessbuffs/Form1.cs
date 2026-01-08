using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimelessBuffs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = TimelessSettings.myPrefs;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimelessSettings.myPrefs.Save();
            Close();
        }
    }
}
