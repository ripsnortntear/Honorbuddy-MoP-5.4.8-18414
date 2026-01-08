using Eclipse.EclipsePlugins.Controllers;
using Eclipse.EclipsePlugins.Models;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
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
    public partial class DungeonBuddy : Form
    {
        EclipseDBProfile db = new EclipseDBProfile();
        public DungeonBuddy()
        {
            InitializeComponent();
        }

        private void DungeonBuddy_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://arachnidcreations.com/HonorBuddy/Image.aspx?image=Supernova-Eclipse2.jpg";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string target = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=GAXEB25MFRKXE";
            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch
                (
                 System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            //ToDo: put back before export
            //try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                if (tbFileName.Text.Length == 0)
                {
                    ofd.ShowDialog();
                    tbFileName.Text = ofd.FileName;
                }
                db = new EclipseDBProfile(tbFileName.Text);
                loadData();
                EC.log("Loaded Profile Data.");
                tbFileName.Text = "";
            }
            //catch (Exception err)
            //{
            //   MessageBox.Show(err.Message + "|" + err.StackTrace + "|" + err.Source);
            //   EC.log(string.Format("Profile Load Error! {0}", err.ToString()));
            //}
        }
        private void loadData()
        {
            lblProfileName.DataBindings.Clear();
            lblProfileName.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            lblProfileName.DataBindings.Add("Text", db, "Name");
            lbBosses.DataSource = null;
            lbBosses.DataSource = EclipseDBProfile.Bosses;
            lbBosses.DisplayMember = "Name";

        }

        private void btnNewProfile_Click(object sender, EventArgs e)
        {
            EC.log(string.Format("Showing new profile form."));
            NewDBProfile np = new NewDBProfile(new EclipseDBProfile());
            np.Show();
            db = np._dt;
            np.FormClosed += np_FormClosed;
        }

        private void np_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadData();
        }

        private void lbBosses_SelectedValueChanged(object sender, EventArgs e)
        {
            var boss = (Boss)lbBosses.SelectedItem;
            if (boss != null)
            {
                if (boss.Path != null)
                {
                    lbHotSpots.DataSource = boss.Path.HotSpots;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnGetTarget_Click(object sender, EventArgs e)
        {
            var t = Target();
            if (t != null)
            {
                
                tbBossId.Text = t.Entry.ToString();
                tbBossName.Text = t.Name;
            }
        }
        private WoWUnit Target()
        {
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget == null) ObjectManager.Update();
                var target = StyxWoW.Me.CurrentTarget;
                return target;
            }
            return null;
        }

        private void btnAddBoss_Click(object sender, EventArgs e)
        {
            Boss boss = new Boss();
            boss.Name = tbBossName.Text;
            boss.Id = tbBossId.Text;
            boss.KillOrder = tbKillOrder.Text;
            boss.X = tbQX.Text;
            boss.Y = tbQY.Text;
            boss.Z = tbQZ.Text;
            boss.Optional = checkBoxOptional.Checked.ToString();
            boss.isFinal = checkBoxLastBoss.Checked.ToString();
            EclipseDBProfile.Bosses.Add(boss);

        }
    }
}
