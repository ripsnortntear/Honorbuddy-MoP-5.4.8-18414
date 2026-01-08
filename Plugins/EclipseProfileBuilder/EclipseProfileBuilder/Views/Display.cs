using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.EclipsePlugins.Views
{
    public partial class Display : Form
    {
        public Display(string text)
        {
            InitializeComponent();
            textBox1.Text = text;
        }

        private void Display_Load(object sender, EventArgs e)
        {
            
        }
    }
}
