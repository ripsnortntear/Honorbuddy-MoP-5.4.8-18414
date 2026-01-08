using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eclipse.EclipsePlugins.Views
{
    public partial class NewQuestingProfile : Form
    {
        public EclipseProfile _dt = new EclipseProfile();
        public NewQuestingProfile(EclipseProfile dt)
        {
            dt = _dt;
            InitializeComponent();
        }

        private void NewProfile_Load(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateProfileName();
        }

        private void GenerateProfileName()
        {
            var faction1 = "";
            var faction2 = "";
            if (checkBoxAlliance.Checked) faction1 = "A";
            if (checkBoxHorde.Checked) faction2 = "H";
            if (checkBoxAlliance.Checked && checkBoxHorde.Checked)
            {
                faction1 = "ALL";
                faction2 = "";
            }
            var zone = "Unknown Zone";
            if (StyxWoW.Me != null)
            {
                zone = StyxWoW.Me.ZoneText;
            }
            tbProfileName.Text = string.Format("[{0}{1}_{2}-{3}] Eclipse Profile for {4}.xml", faction1, faction2, tbMinLevel.Text, tbMaxLevel.Text, zone);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tbProfileName.Text.Length > 0)
            {
                _dt.Name = tbProfileName.Text;

                _dt.MinLevel = tbMinLevel.Text;
                _dt.MaxLevel = tbMaxLevel.Text;
                _dt.SellGrey = checkSellGrey.Checked;
                _dt.SellGrey = checkSellGrey.Checked;
                _dt.SellWhite = checkSellWhite.Checked;
                _dt.SellGreen = checkSellGreen.Checked;
                _dt.SellBlue = checkSellBlue.Checked;
                _dt.SellPurple = checkSellPurple.Checked;
                _dt.MailGrey = checkMailGrey.Checked;
                _dt.MailWhite = checkMailWhite.Checked;
                _dt.MailGreen = checkMailGreen.Checked;
                _dt.MailBlue = checkMailBlue.Checked;
                _dt.MailPurple = checkMailPurple.Checked;
                this.Close();
            }
            else
            {
                GenerateProfileName();
                MessageBox.Show("Since you did not choos a profile name one has been generated for you. If this is acceptable press the create button again, otherwise, specify a name.");
            }
        }
    }
}
