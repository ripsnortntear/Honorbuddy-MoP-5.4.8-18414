using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PokehbuddyLogicCreator
{
    public partial class BuffBox : Form
    {
        private static List<string> Buffs { get; set; }

        public BuffBox(List<string> buffs, string BuffType)
        {
            Buffs = buffs;
            InitializeComponent();
            this.Text = BuffType;
        }

        private void BuffBox_Load(object sender, EventArgs e)
        {
            if (Buffs.Count == 0)
                return;

            richTextBox1.Text = string.Empty;
            richTextBox1.Lines = Buffs.ToArray();
        }
    }
}
