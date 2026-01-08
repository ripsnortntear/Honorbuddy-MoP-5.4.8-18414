using Eclipse.EclipsePlugins.Controllers;
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
    public partial class NewDBProfile : Form
    {
        public EclipseDBProfile _dt = new EclipseDBProfile();
        public NewDBProfile(EclipseDBProfile dt)
        {
            dt = _dt;
            InitializeComponent();
        }

        private void NewProfile_Load(object sender, EventArgs e)
        {
            if (StyxWoW.Me != null)
            {
                _dt.DungeonId = StyxWoW.Me.ZoneId.ToString();
                _dt.DungeonName = StyxWoW.Me.ZoneText;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateProfileName();
        }

        private void GenerateProfileName()
        {
            var zone = "Unknown Zone";
            if (StyxWoW.Me != null)
            {
                zone = StyxWoW.Me.ZoneText;
            }
            tbProfileName.Text = string.Format("[DB Profile]Eclipse Profile for {4}.xml", zone);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tbProfileName.Text.Length > 0)
            {
                _dt.Name = tbProfileName.Text;
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
