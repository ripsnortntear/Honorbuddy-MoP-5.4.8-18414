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
                foreach (WoWItem myOre in Me.BagItems)
                {
                    if (myOre != null)
                    {
                        if (myOre.Entry == watOre && myOre.StackCount >= 5)
                        { 
                            myOre.PickUp();
                            Thread.Sleep(500);
                            while (myOre.StackCount >= 5)
                            {
                                Lua.DoString("CastSpellByName(\"" + "Prospecting" + "\")");
                                myOre.Use(); 
                                Thread.Sleep(4500);
                            }
                            if (jumpTimer.Elapsed.Minutes >= ProspectingSettings.myPrefs.Jump)
                            {
                                jumpTimer.Reset();
                                KeyboardManager.KeyUpDown(' ');
                                jumpTimer.Start();
                            }
                        }
                    }
                }
                slog("No more ores to prospect");
                
                // Creating of serpent's eyes
                foreach (WoWItem item in Me.BagItems)
                {
                    if (item != null)
                    {
                        if (item.Entry == serpent && item.StackCount >= 10)
                        {
                            item.PickUp();
                            while (item.StackCount >= 10)
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

        private void numericUpDownJump_ValueChanged(object sender, EventArgs e)
        {
            ProspectingSettings.myPrefs.Jump = (int)numericUpDownJump.Value;
        }
    }
}
