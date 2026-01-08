using Eclipse.EclipsePlugins.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eclipse.EclipsePlugins
{
    public partial class ProfileTypeSelector : Form
    {
        public ProfileTypeSelector()
        {
            InitializeComponent();
        }

        private void ProfileTypeSelector_Load(object sender, EventArgs e)
        {
            pbDungeonBuddy.ImageLocation = "http://static3.wikia.nocookie.net/__cb20110217210755/wowwiki/images/thumb/0/08/The_Deadmines_loading_screen.jpg/221px-The_Deadmines_loading_screen.jpg";
            pbEclipse.ImageLocation = "http://arachnidcreations.com/HonorBuddy/Image.aspx?image=Supernova-Eclipse2.jpg";
        }

        private void pbQuesting_Click(object sender, EventArgs e)
        {
            QuestingBuddy ep = new QuestingBuddy();
            ep.Show();
        }

        private void pbDungeonBuddy_Click(object sender, EventArgs e)
        {
            DungeonBuddy db = new DungeonBuddy();
            db.Show();
        }
        private void pbProfessionBuddy_Click(object sender, EventArgs e)
        {
            NotImplemented();
        }
        public void NotImplemented()
        {
            MessageBox.Show("This feature is not yet implemented.");
        }
    }
}
