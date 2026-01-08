using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jumpy.Properties;
using System.IO;

namespace Jumpy
{
	public partial class JumpyWindow : Form
	{

        public Jumpy jumpy;
		public JumpyWindow(Jumpy _jumpy)
		{
            jumpy = _jumpy;
			InitializeComponent();
            this.Icon = new Icon(Path.Combine(Application.StartupPath, "Plugins/Jumpy/Jumpy/gnome.ico"), new Size(256, 256));
            toolTip.SetToolTip(boxMountedJumpSpan, "Base Span: " + Constants.BaseTimeTip + "\r\n" + "Rand Span: " + Constants.RandTimeTip);
            toolTip.SetToolTip(nudBaseMountJumpSpan, Constants.BaseTimeTip);
            toolTip.SetToolTip(nudBaseWalkJumpSpan, Constants.BaseTimeTip);
            toolTip.SetToolTip(boxWalkingJumpSpan, "Base Span: " + Constants.BaseTimeTip + "\r\n" + "Rand Span: " + Constants.RandTimeTip);
            toolTip.SetToolTip(boxStandingJump, Constants.StandingJumpTip);
            toolTip.SetToolTip(chkStandJumpInCombat, Constants.StandingJumpTip);
            toolTip.SetToolTip(chkStandJumpOutCombat, Constants.StandingJumpTip);
            toolTip.SetToolTip(chkUsePlayerRange, Constants.PlayerInRangeTip);
            toolTip.SetToolTip(boxPlayerRange, Constants.PlayerInRangeTip);
            toolTip.SetToolTip(tabPage1, Constants.RoutineSelectionTip);
            toolTip.SetToolTip(lbCombatRoutines, Constants.RoutineSelectionTip);
            toolTip.SetToolTip(lbMountedRoutines, Constants.RoutineSelectionTip);
            toolTip.SetToolTip(lbWalkingRoutines, Constants.RoutineSelectionTip);
			LoadSettings();

            //this.Opacity = 0;
            //Show();
            //Log("Initializing Log");
            //Hide();
            //this.Opacity = 1;
		}

		private void UsePlayerRange_CheckedChanged(object sender, EventArgs e)
		{
			nudPlayerRange.Enabled = chkUsePlayerRange.Checked;
		}


		public void SaveSettings()
		{
			Storage.Package.CombatRoutines.Clear();
			Storage.Package.WalkingRoutines.Clear();
			Storage.Package.MountedRoutines.Clear();
			foreach (string profile in lbCombatRoutines.SelectedItems)
			{
				Storage.Package.CombatRoutines.Add(profile);
			}
			foreach (string profile in lbWalkingRoutines.SelectedItems)
			{
				Storage.Package.WalkingRoutines.Add(profile);
			}
			foreach (string profile in lbMountedRoutines.SelectedItems)
			{
				Storage.Package.MountedRoutines.Add(profile);
			}

			Storage.Package.UsePlayerRange = chkUsePlayerRange.Checked;
			Storage.Package.PlayerRange = (double)nudPlayerRange.Value;
		    Storage.Package.BaseWalkJumpSpan = (double)nudBaseWalkJumpSpan.Value;
			Storage.Package.BaseMountJumpSpan = (double)nudBaseMountJumpSpan.Value;
			Storage.Package.RandWalkJumpSpan = (double)nudRandWalkJumpSpan.Value;
			Storage.Package.RandMountJumpSpan = (double)nudRandMountJumpSpan.Value;
			Storage.Package.StandJumpInCombat = chkStandJumpInCombat.Checked;
			Storage.Package.StandJumpOutCombat = chkStandJumpOutCombat.Checked;
			Storage.Save();
		}


		private Dictionary<string, List<Routine>> AllRoutines = new Dictionary<string, List<Routine>>();
		public Dictionary<string, List<Routine>> CombatRoutines = new Dictionary<string, List<Routine>>();
		public Dictionary<string, List<Routine>> WalkingRoutines = new Dictionary<string, List<Routine>>();
		public Dictionary<string, List<Routine>> MountedRoutines = new Dictionary<string, List<Routine>>();


		public void LoadSettings()
		{
			string[] profiles = Directory.GetFiles(Application.StartupPath + "/Plugins", "*.jump", SearchOption.AllDirectories).Select(file => Path.GetFileNameWithoutExtension(file)).Distinct().ToArray();
			lbCombatRoutines.Items.Clear();
			lbWalkingRoutines.Items.Clear();
			lbMountedRoutines.Items.Clear();
			lbCombatRoutines.SelectedItems.Clear();
			lbWalkingRoutines.SelectedItems.Clear();
			lbMountedRoutines.SelectedItems.Clear();
			lbCombatRoutines.Items.AddRange(profiles);
			lbWalkingRoutines.Items.AddRange(profiles);
			lbMountedRoutines.Items.AddRange(profiles);
            if (Storage.FirstTimeLoaded)
            {
                foreach (ListBox lb in new List<ListBox>() { lbMountedRoutines, lbCombatRoutines, lbWalkingRoutines })
                    foreach (string profile in profiles)
                        lb.SelectedItems.Add(profile);
            }
            foreach (string profile in Storage.Package.CombatRoutines)
                if (profiles.Any(file => file.ToLower() == profile.ToLower()))
                    lbCombatRoutines.SelectedItems.Add(profile);
            foreach (string profile in Storage.Package.WalkingRoutines)
                if (profiles.Any(file => file.ToLower() == profile.ToLower()))
                    lbWalkingRoutines.SelectedItems.Add(profile);
            foreach (string profile in Storage.Package.MountedRoutines)
                if (profiles.Any(file => file.ToLower() == profile.ToLower()))
                    lbMountedRoutines.SelectedItems.Add(profile);

			string[] voidProfiles = AllRoutines.Keys.Except(profiles).ToArray();
			foreach (string profile in voidProfiles)
			{
				AllRoutines.Remove(profile);
			}


			foreach (string profilePath in Directory.GetFiles(Application.StartupPath + "/Plugins", "*.jump", SearchOption.AllDirectories))
			{
				using (StreamReader reader = new StreamReader(profilePath))
				{
					string profile = Path.GetFileNameWithoutExtension(profilePath);
                    if (!AllRoutines.ContainsKey(profile))
                    {
                        AllRoutines[profile] = new List<Routine>();
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (line.Trim() == string.Empty)
                                continue;
                            AllRoutines[profile].Add(profile, line, this);
                        }
                    }
				}
			}


			CombatRoutines.Clear();
			WalkingRoutines.Clear();
			MountedRoutines.Clear();
			foreach (string profile in lbCombatRoutines.SelectedItems)
			{
				CombatRoutines[profile] = AllRoutines[profile];
			}
			foreach (string profile in lbWalkingRoutines.SelectedItems)
			{
				WalkingRoutines[profile] = AllRoutines[profile];
			}
			foreach (string profile in lbMountedRoutines.SelectedItems)
			{
				MountedRoutines[profile] = AllRoutines[profile];
			}

			chkUsePlayerRange.Checked = Storage.Package.UsePlayerRange;
			nudPlayerRange.Enabled = Storage.Package.UsePlayerRange;
			nudPlayerRange.Value = (decimal)Storage.Package.PlayerRange;
			nudBaseWalkJumpSpan.Value = (decimal)Storage.Package.BaseWalkJumpSpan;
			nudBaseMountJumpSpan.Value = (decimal)Storage.Package.BaseMountJumpSpan;
			nudRandWalkJumpSpan.Value = (decimal)Storage.Package.RandWalkJumpSpan;
			nudRandMountJumpSpan.Value = (decimal)Storage.Package.RandMountJumpSpan;
			chkStandJumpInCombat.Checked = Storage.Package.StandJumpInCombat;
			chkStandJumpOutCombat.Checked = Storage.Package.StandJumpOutCombat;

		}

		private string LastMessage = string.Empty;
		public void Log(string message)
		{
			Log(message, Color.Black);
		}
		public void Log(string message, Color color)
		{
			Log(message, FontStyle.Regular, color);
		}
		public void Log(string message, FontStyle fontStyle)
		{
			Log(message, fontStyle, Color.Black);
		}
		public void Log(string message, FontStyle fontstyle, Color color)
		{
            if (!formShown)
                return;
			if (message != LastMessage)
			{
				LastMessage = message;
				tbLog.SelectionColor = color;
				tbLog.SelectionFont = new Font(tbLog.Font, fontstyle);
				tbLog.AppendText(message);
				tbLog.ScrollToCaret();
			}
		}

		private void JumpyWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true; // this cancels the close event.
            //Reload();
		}

		private void JumpyWindow_VisibleChanged(object sender, EventArgs e)
		{
			Reload();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			Reload();
		}

        bool formShown = false;
		private void JumpyWindow_Shown(object sender, EventArgs e)
		{
            if (!formShown)
            {
                tabContainer.SelectedTab = tabContainer.TabPages[2];
                tabContainer.SelectedTab = tabContainer.TabPages[0];
                formShown = true;
            }
		}

		void Reload()
		{
			SaveSettings();
			LoadSettings();
		}

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void JumpyWindow_Load(object sender, EventArgs e)
        {
 
        }



	



	}


	
}
