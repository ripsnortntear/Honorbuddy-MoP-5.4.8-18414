using System;
using System.Windows.Forms;

namespace Oracle.Healing.Chronicle.UI
{
    public partial class ChronicleUI : Form
    {
        public ChronicleUI()
        {
            InitializeComponent();

            PopulateList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            if (ChronicleHealing.AvailableChronicleSpells.Count == 0) return;
            this.textBox1.Text += Environment.NewLine + this.comboBox1.SelectedItem.ToString().Replace("\r\n", "\n").Replace("\n", Environment.NewLine); ;
        }

        private void PopulateList()
        {
            if (ChronicleHealing.AvailableChronicleSpells.Count == 0) return;
            comboBox1.DataSource = new BindingSource(ChronicleHealing.AvailableChronicleSpells, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            textBox1.Text = Environment.NewLine + this.comboBox1.SelectedItem.ToString().Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }
    }
}