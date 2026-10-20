using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Threading;
using Styx;
using Styx.Helpers;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Diagnostics;

namespace Prospecting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProspectingSettings.myPrefs.Load();
            numericUpDownJump.Value = new decimal(ProspectingSettings.myPrefs.Jump);
            jumpTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set a default ore type to prevent watOre being 0 if no radio button is selected
            radioButtonCopper.Checked = true;
            watOre = 2770;
        }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public int watOre = 0;

        public int serpent = 90407;

        public Stopwatch jumpTimer = new Stopwatch();

        //public static void Sleep(int millisecondsTimeout) { }

        public static void slog(string format, params object[] args)
        {
            Logging.Write(Colors.Yellow, format, args);
        }
        public static void slogg(string format, params object[] args)
        {
            Logging.Write(Colors.SeaGreen, format, args);
        }
        public static void slogo(string format, params object[] args)
        {
            Logging.Write(Colors.Orange, format, args);
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            ProspectingSettings.myPrefs.Save();
            Close();
        }

        private void buttonProspect_Click(object sender, EventArgs e)
		{		
			bool hasOresToProspect = false;
			
			// Get all matching ore items in bags with >=5 stack
			var oresToProspect = Me.BagItems.Where(item => item != null && item.IsValid && item.Entry == watOre && item.StackCount >= 5).ToList();
			
			foreach (WoWItem myOre in oresToProspect)
			{
				hasOresToProspect = true;
				while (myOre.IsValid && myOre.StackCount >= 5)
				{
					// Use a Lua macro to properly prospect: cast the spell and target the specific ore by entry ID
					Lua.DoString("RunMacroText(\"/cast Prospecting\\n/use " + myOre.Entry + "\")");
					Thread.Sleep(4500);  // Wait for cast to complete
			
					// Jump occasionally to avoid AFK
					if (jumpTimer.Elapsed.Minutes >= ProspectingSettings.myPrefs.Jump)
					{
						jumpTimer.Reset();
						KeyboardManager.KeyUpDown(' ');
						jumpTimer.Start();
					}
				}
			}

			if (!hasOresToProspect)
			{
				slog("No ores to prospect");
			}

			// Creating of serpent's eyes
			foreach (WoWItem item in Me.BagItems)
			{
				if (item != null && item.IsValid)
				{
					if (item.Entry == serpent && item.StackCount >= 10)
					{
						item.PickUp();
						while (item.IsValid && item.StackCount >= 10)
						{
							Lua.DoString("UseItemByName(\"" + item.Name + "\")");
							StyxWoW.SleepForLagDuration();
						}
					}
				}
			}
			ProspectingSettings.myPrefs.Save();
			Close();
		}

        // Existing MoP ores
        private void radioButtonGhostIron_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 72092;
        }

        private void radioButtonKyparite_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 72093;
        }

        private void radioButtonWhiteTrillium_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 72103;
        }

        private void radioButtonBlackTrillium_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 72094;
        }

        // New ores: Cataclysm
        private void radioButtonObsidium_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 53038;
        }

        private void radioButtonElementium_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 52185;
        }

        private void radioButtonPyrite_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 52183;
        }

        // New ores: Wrath of the Lich King
        private void radioButtonCobalt_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 36909;
        }

        private void radioButtonSaronite_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 36912;
        }

        private void radioButtonTitanium_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 36910;
        }

        // New ores: The Burning Crusade
        private void radioButtonFelIron_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 23424;
        }

        private void radioButtonAdamantite_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 23425;
        }

        // New ores: Vanilla (Classic)
        private void radioButtonCopper_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 2770;
        }

        private void radioButtonTin_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 2771;
        }

        private void radioButtonIron_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 2772;
        }

        private void radioButtonMithril_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 3858;
        }

        private void radioButtonThorium_CheckedChanged(object sender, EventArgs e)
        {
            watOre = 10620;
        }

        private void numericUpDownJump_ValueChanged(object sender, EventArgs e)
        {
            ProspectingSettings.myPrefs.Jump = (int)numericUpDownJump.Value;
        }
    }
}