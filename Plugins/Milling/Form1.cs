using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx;
using Styx.Common;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading;
using Styx.Helpers;
using Styx.CommonBot;

namespace Milling
{
    public partial class Form1 : Form
    {
        
        public string Waiting { get; set; }
        Stopwatch wait = new Stopwatch();
        Stopwatch jumpTimer = new Stopwatch();
        private decimal stacks;

        public Form1()
        {
            InitializeComponent();
            CRSettings.myPrefs.Load();
            comboBox1.SelectedIndex = CRSettings.myPrefs.Herbie;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.HerbToMill = (string)comboBox1.SelectedItem;
            CRSettings.myPrefs.Herbie = comboBox1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Save();

            jumpTimer.Start();

            foreach (WoWItem item in StyxWoW.Me.BagItems)
            {
                 
                if (item != null && item.Name == CRSettings.myPrefs.HerbToMill && item.BagSlot != -1 && StyxWoW.Me.FreeNormalBagSlots >= 2)
                {
                    stacks = (item.StackCount / 5);
                    if (Math.Floor(stacks) > 0)
                    {
                        
                        for (int i = 0; i < Math.Floor(stacks); i++)
                        { 
                            SpellManager.Cast(51005);
                            item.Use();
                            Thread.Sleep(CRSettings.myPrefs.wachtTijd);
                        }
                    }
                     
                }
                if (jumpTimer.IsRunning && jumpTimer.ElapsedMilliseconds >= 60000)
                {
                    KeyboardManager.KeyUpDown(' ');
                    jumpTimer.Restart();
                }
                Thread.Sleep(2000);
            }
            Logging.Write(Colors.Yellow, ">>> F I N I S H E D with " + CRSettings.myPrefs.HerbToMill + " <<<");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.wachtTijd = (int)numericUpDown1.Value;
        }
    }
}
