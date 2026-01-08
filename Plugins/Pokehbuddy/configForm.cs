using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System;
using Styx.Common.Helpers;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Styx.Plugins;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
//using Styx.Plugins.PluginClass;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Helpers;
using Styx;
//using Styx.Logic.Pathing;

using Styx.Helpers;



using System.Windows.Forms;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
/***************************************************************
TODO

-PetsAlive EnemyPetsAlive



***************************************************************/

//using System.Linq;


//using Styx.Logic.BehaviorTree;

//using Styx.Logic;


//using Styx.Logic.Combat;
//using Styx.Logic.Inventory;
//using Styx.WoWInternals.WoWObjects;
//using Styx.Logic.Inventory.Frames.LootFrame;
//using Styx.Logic.Inventory.Frames.Gossip;
using Styx.WoWInternals.World;
//using Styx.Logic.Profiles;
//using Styx.Logic.AreaManagement;
using Styx.Plugins;
using Styx.WoWInternals.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Color = System.Drawing.Color;

using System.Text;
using Bots.BGBuddy.Helpers;
using Styx;

using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
//using Microsoft.VisualBasic.Interaction;
using CalcEngine;




namespace Pokehbuddyplug
{
    partial class configForm
    {
        int initdone = 0;
        private void BlacklistSave()
        {
            listBox3.Sorted = true;
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\blacklist.txt";

            StreamWriter Write;
            try
            {
                Write = new StreamWriter(filename);
                for (int I = 0; I < listBox3.Items.Count; I++)
                {
                    Write.WriteLine(Convert.ToString(listBox3.Items[I]));
                }
                Write.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            Pokehbuddy.BlacklistLoad();

        }
        private void MacroLoad()
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\logicmacro.txt";

            listBox7.Items.Clear();

            try
            {
                StreamReader Read = new StreamReader(Convert.ToString(filename));
                while (Read.Peek() >= 0)
                {
                    string pline = Read.ReadLine();
                    if (pline != null)
                    {
                        listBox7.Items.Add(pline);

                    }
                }
                Read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            



        }

        private void MacroSave()
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\logicmacro.txt";

             listBox7.Sorted = true;
            

            StreamWriter Write;
            try
            {
                Write = new StreamWriter(filename);
                for (int I = 0; I < listBox7.Items.Count; I++)
                {
                    Write.WriteLine(Convert.ToString(listBox7.Items[I]));
                }
                Write.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            



        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void BlacklistLoad()
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\blacklist.txt";

            listBox3.Items.Clear();

            try
            {
                StreamReader Read = new StreamReader(Convert.ToString(filename));
                while (Read.Peek() >= 0)
                {
                    string pline = Read.ReadLine();
                    if (pline != null)
                    {
                        listBox3.Items.Add(pline.ToLower());

                    }
                }
                Read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            Pokehbuddy.BlacklistLoad();



        }
        private void WhitelistSave()
        {
            listBox4.Sorted = true;
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\whitelist.txt";

            StreamWriter Write;
            try
            {
                Write = new StreamWriter(filename);
                for (int I = 0; I < listBox4.Items.Count; I++)
                {
                    Write.WriteLine(Convert.ToString(listBox4.Items[I]));
                }
                Write.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            Pokehbuddy.WhitelistLoad();

        }
        private void WhitelistLoad()
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\whitelist.txt";

            listBox4.Items.Clear();

            try
            {
                StreamReader Read = new StreamReader(Convert.ToString(filename));
                while (Read.Peek() >= 0)
                {
                    string pline = Read.ReadLine();
                    if (pline != null)
                    {
                        listBox4.Items.Add(pline.ToLower());

                    }
                }
                Read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            Pokehbuddy.WhitelistLoad();


        }
        private void configForm_Load(object sender, EventArgs e)
        {
            LoadDefaults();
            BlacklistLoad();
            WhitelistLoad();
            MacroLoad();
            textBox3.Text = Pokehbuddy.MySettings.HPFormula;
            textBox4.Text = Pokehbuddy.MySettings.AdFormula;
            textBox5.Text = Pokehbuddy.MySettings.DisFormula;
            textBox6.Text = Pokehbuddy.MySettings.LevelFormula;

            checkBox5.Checked = Pokehbuddy.MySettings.UseBlackList;
            checkBox6.Checked = Pokehbuddy.MySettings.UseWhiteList;
            

            checkBox2.Checked = Pokehbuddy.MySettings.ForfeitIfNotInteresting;
            //checkBox3.Checked = Pokehbuddy.MySettings.UseBandagesToHeal;

            comboBox12.SelectedIndex = Pokehbuddy.MySettings.UseBandagesToHeal;
            comboBox10.SelectedIndex = Pokehbuddy.MySettings.UseHealSkill;

            checkBox4.Checked = Pokehbuddy.MySettings.DetailedLogging;

            trackBar1.Value = Pokehbuddy.MySettings.HPFactor;
            trackBar2.Value = Pokehbuddy.MySettings.LevelFactor;
            trackBar3.Value = Pokehbuddy.MySettings.AdFactor;
            trackBar4.Value = Pokehbuddy.MySettings.DisFactor;
            trackBar5.Value = Pokehbuddy.MySettings.Distance;

            checkBox1.Checked = Pokehbuddy.MySettings.DoPVP;
            comboBox5.SelectedIndex = Pokehbuddy.MySettings.BelowLevel;
            comboBox6.SelectedIndex = Pokehbuddy.MySettings.AboveLevel;
            comboBox7.SelectedIndex = Pokehbuddy.MySettings.PVPMinTime;

            comboBox14.SelectedIndex = Pokehbuddy.MySettings.MinPetsAlive - 1;

            comboBox8.Items.Clear();
            int dummy = 0;
            for (int i = 1; i < 60; i++)
            {
                dummy = Pokehbuddy.MySettings.PVPMinTime + 1;
                comboBox8.Items.Add(dummy + i);
            }


            comboBox8.SelectedIndex = Pokehbuddy.MySettings.PVPMaxTime;


            comboBox1.SelectedIndex = Pokehbuddy.MySettings.GetRarity - 1;
            initdone = 1;
            //Logging.Write("LAlalala");

            /* TheDungeonComboBox.SelectedIndex = Pokehbuddy.MySettings.TheDungeon;
             HeartstoneOutSetting.Text = Pokehbuddy.MySettings.HeartstoneAfter.ToString();
             WalkoutTimeSetting.Text = Pokehbuddy.MySettings.WalkOutAfter.ToString();
             MailEveryResetCheck.Checked = Pokehbuddy.MySettings.MailEveryReset;
             HordeCheck.Checked = Pokehbuddy.MySettings.AmHorde;
             IBSupport.Checked = Pokehbuddy.MySettings.IBSupport;*/
        }

        private void MailEveryResetCheck_CheckedChanged(object sender, EventArgs e)
        {
            //   Pokehbuddy.MySettings.MailEveryReset = MailEveryResetCheck.Checked;
        }
        private void HordeCheck_CheckedChanged(object sender, EventArgs e)
        {
            //   Pokehbuddy.MySettings.AmHorde = HordeCheck.Checked;
        }
        private void IBSupport_CheckedChanged(object sender, EventArgs e)
        {
            //   Pokehbuddy.MySettings.IBSupport = IBSupport.Checked;
        }



        private void WalkoutTimeSetting_Leave(object sender, EventArgs e)
        {
            //  WalkoutTimeSetting.Text = Pokehbuddy.MySettings.WalkOutAfter.ToString();
        }

        private void WalkoutTimeSetting_TextChanged(object sender, EventArgs e)
        {
            //   int n;
            //   int.TryParse(WalkoutTimeSetting.Text, out n);
            //   if (n < 1)
            //       n = 1;
            //   else if (n > 50)
            //       n = 50;
            //   Pokehbuddy.MySettings.WalkOutAfter = n;
        }

        private void HeartstoneOutSetting_TextChanged(object sender, EventArgs e)
        {
            //   int n;
            //   int.TryParse(HeartstoneOutSetting.Text, out n);
            //  if (n < 20)
            //       n = 20;
            //   else if (n > 100)
            //      n = 100;
            //    Pokehbuddy.MySettings.HeartstoneAfter = n;
        }

        private void HeartstoneOutSetting_Leave(object sender, EventArgs e)
        {
            //  HeartstoneOutSetting.Text = Pokehbuddy.MySettings.HeartstoneAfter.ToString();
        }

        private void configForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BlacklistSave();
            WhitelistSave();
            Pokehbuddy.MySettings.HPFormula = textBox3.Text;
            Pokehbuddy.MySettings.AdFormula = textBox4.Text;
            Pokehbuddy.MySettings.DisFormula = textBox5.Text;
            Pokehbuddy.MySettings.LevelFormula = textBox6.Text;

            Pokehbuddy.MySettings.Save();
        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, System.EventArgs e)
        {
            textBox3.Enabled = checkBox7.Checked;
            textBox4.Enabled = checkBox7.Checked;
            textBox5.Enabled = checkBox7.Checked;
            textBox6.Enabled = checkBox7.Checked;
            
        }

        private void checkBox9_CheckedChanged(object sender, System.EventArgs e)
        {
            checkBox10.Enabled = checkBox9.Checked;
        }

        private void button12_Click_1(object sender, System.EventArgs e)
        {
            /*//comboBox11.Text="SELECT ACTION";
            string macrostring = "Health(THISPET),ISLESSTHAN,NUMBER,0,25";
            string[] macrologic = macrostring.Split(',');

            comboBox2.Text = macrologic[0];
            comboBox3.Text = macrologic[1];
            comboBox4.Text = macrologic[2];
            textBox101.Text = macrologic[3];
            textBox102.Text = macrologic[4];

            button3.Enabled = true;
            button4.Enabled = true;*/
            groupBox9.Visible = !groupBox9.Visible;


        }

        private void button13_Click_1(object sender, System.EventArgs e)
        {
            groupBox9.Visible = false;
            groupBox4.Visible = false;
            listBox2.Visible = false;

        }

        private void button14_Click(object sender, System.EventArgs e)
        {
            if (comboBox15.SelectedIndex == -1) comboBox15.SelectedIndex = 0;
            if (comboBox16.SelectedIndex == -1) comboBox16.SelectedIndex = 0;
            loadpreviewimages();
           

            updatenumbers();
            






        }
        private int CalcLua(string s)
        {
            //if ("+s+") then return true end return false
            List<string> cnt = Lua.GetReturnValues("return ("+s+")");

            //if (cnt[1] != "0") return true;
            //return false;
            return int.Parse(cnt[0]);
        }

        private void updatenumbersLua()
        {
            
            string s = "1 + 1 * 3";


            int value = 0;
            int advantage = 0;
            int disadvantage = 0;
            var total = 0;


            //pet 1

            total = 0;
            disadvantage = 0;
            advantage = 0;

            s = textBox3.Text;
            s = s.Replace("petHP", Pokehbuddy.GetPetHPPreCombat(1).ToString()).Replace("HPFactor", trackBar1.Value.ToString());

            value = CalcLua(s);
            total = value;
            label50.Text = value.ToString();
            


            int mypet = Pokehbuddy.GetTypeByID(Pokehbuddy.ReadSlot(1));
            if (mypet == Pokehbuddy.DumbChoiceTakeMoreDMG(comboBox16.SelectedIndex + 1)) disadvantage = -2;
            if (mypet == Pokehbuddy.DumbChoiceDealLessDMG(comboBox16.SelectedIndex + 1)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == Pokehbuddy.SmartChoiceTakeLessDMG(comboBox16.SelectedIndex + 1)) advantage = 1;
            if (mypet == Pokehbuddy.SmartChoiceDealMoreDMG(comboBox16.SelectedIndex + 1)) advantage = advantage + 2;
            s = textBox4.Text;   //advantage * 50 * AdFactor
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", trackBar3.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label49.Text = value.ToString();

            s = textBox5.Text;   //advantage * 50 * AdFactor
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", trackBar4.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label48.Text = value.ToString();



            s = textBox6.Text;   //advantage * 50 * AdFactor
            s = s.Replace("petLevel", Pokehbuddy.GetPetLevelPreCombat(1).ToString()).Replace("enemylevel", (comboBox15.SelectedIndex + 1).ToString()).Replace("LevelFactor", trackBar2.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label47.Text = value.ToString();

            label46.Text = total.ToString();

            //pet 2

            total = 0;
            disadvantage = 0;
            advantage = 0;

            s = textBox3.Text;
            s = s.Replace("petHP", Pokehbuddy.GetPetHPPreCombat(2).ToString()).Replace("HPFactor", trackBar1.Value.ToString());

            value = CalcLua(s);
            total = value;
            label56.Text = value.ToString();


            mypet = Pokehbuddy.GetTypeByID(Pokehbuddy.ReadSlot(2));
            if (mypet == Pokehbuddy.DumbChoiceTakeMoreDMG(comboBox16.SelectedIndex + 1)) disadvantage = -2;
            if (mypet == Pokehbuddy.DumbChoiceDealLessDMG(comboBox16.SelectedIndex + 1)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == Pokehbuddy.SmartChoiceTakeLessDMG(comboBox16.SelectedIndex + 1)) advantage = 1;
            if (mypet == Pokehbuddy.SmartChoiceDealMoreDMG(comboBox16.SelectedIndex + 1)) advantage = advantage + 2;
            s = textBox4.Text;   //advantage * 50 * AdFactor
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", trackBar3.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label55.Text = value.ToString();

            s = textBox5.Text;   //advantage * 50 * AdFactor
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", trackBar4.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label54.Text = value.ToString();



            s = textBox6.Text;   //advantage * 50 * AdFactor
            s = s.Replace("petLevel", Pokehbuddy.GetPetLevelPreCombat(2).ToString()).Replace("enemylevel", (comboBox15.SelectedIndex + 1).ToString()).Replace("LevelFactor", trackBar2.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label53.Text = value.ToString();

            label52.Text = total.ToString();

            //pet 3

            total = 0;
            disadvantage = 0;
            advantage = 0;

            s = textBox3.Text;
            s = s.Replace("petHP", Pokehbuddy.GetPetHPPreCombat(3).ToString()).Replace("HPFactor", trackBar1.Value.ToString());

            value = CalcLua(s);
            total = value;
            label62.Text = value.ToString();


            mypet = Pokehbuddy.GetTypeByID(Pokehbuddy.ReadSlot(3));
            if (mypet == Pokehbuddy.DumbChoiceTakeMoreDMG(comboBox16.SelectedIndex + 1)) disadvantage = -2;
            if (mypet == Pokehbuddy.DumbChoiceDealLessDMG(comboBox16.SelectedIndex + 1)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == Pokehbuddy.SmartChoiceTakeLessDMG(comboBox16.SelectedIndex + 1)) advantage = 1;
            if (mypet == Pokehbuddy.SmartChoiceDealMoreDMG(comboBox16.SelectedIndex + 1)) advantage = advantage + 2;
            s = textBox4.Text;   //advantage * 50 * AdFactor
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", trackBar3.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label60.Text = value.ToString();

            s = textBox5.Text;   //advantage * 50 * AdFactor
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", trackBar4.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label59.Text = value.ToString();



            s = textBox6.Text;   //advantage * 50 * AdFactor
            s = s.Replace("petLevel", Pokehbuddy.GetPetLevelPreCombat(3).ToString()).Replace("enemylevel", (comboBox15.SelectedIndex + 1).ToString()).Replace("LevelFactor", trackBar2.Value.ToString());

            value = CalcLua(s);
            total = total + value;
            label58.Text = value.ToString();

            label57.Text = total.ToString();


        }

        private void updatenumbers()
        {
            var ce = new CalcEngine.CalcEngine();
            string s = "1 + 1 * 3";
            var x = ce.Parse(s);
            
            var value = x.Evaluate();
            int advantage = 0;
            int disadvantage = 0;
            var total = 0;
            
            
            //pet 1

            total = 0;
            disadvantage = 0;
            advantage = 0;

            s = textBox3.Text;
            s = s.Replace("petHP", Pokehbuddy.GetPetHPPreCombat(1).ToString()).Replace("HPFactor", trackBar1.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = int.Parse(value.ToString());
            label50.Text = value.ToString();
            Logging.Write("Lua test : " + CalcLua(s));


            int mypet = Pokehbuddy.GetTypeByID(Pokehbuddy.ReadSlot(1));
            if (mypet == Pokehbuddy.DumbChoiceTakeMoreDMG(comboBox16.SelectedIndex+1)) disadvantage = -2;
            if (mypet == Pokehbuddy.DumbChoiceDealLessDMG(comboBox16.SelectedIndex+1)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == Pokehbuddy.SmartChoiceTakeLessDMG(comboBox16.SelectedIndex+1)) advantage = 1;
            if (mypet == Pokehbuddy.SmartChoiceDealMoreDMG(comboBox16.SelectedIndex+1)) advantage = advantage + 2;
            s = textBox4.Text;   //advantage * 50 * AdFactor
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", trackBar3.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label49.Text = value.ToString();

            s = textBox5.Text;   //advantage * 50 * AdFactor
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", trackBar4.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label48.Text = value.ToString();



            s = textBox6.Text;   //advantage * 50 * AdFactor
            s = s.Replace("petLevel", Pokehbuddy.GetPetLevelPreCombat(1).ToString()).Replace("enemylevel", (comboBox15.SelectedIndex+1).ToString()).Replace("LevelFactor", trackBar2.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label47.Text = value.ToString();

            label46.Text = total.ToString();

            //pet 2

            total = 0;
            disadvantage = 0;
            advantage = 0;

            s = textBox3.Text;
            s = s.Replace("petHP", Pokehbuddy.GetPetHPPreCombat(2).ToString()).Replace("HPFactor", trackBar1.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = int.Parse(value.ToString());
            label56.Text = value.ToString();


            mypet = Pokehbuddy.GetTypeByID(Pokehbuddy.ReadSlot(2));
            if (mypet == Pokehbuddy.DumbChoiceTakeMoreDMG(comboBox16.SelectedIndex + 1)) disadvantage = -2;
            if (mypet == Pokehbuddy.DumbChoiceDealLessDMG(comboBox16.SelectedIndex + 1)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == Pokehbuddy.SmartChoiceTakeLessDMG(comboBox16.SelectedIndex + 1)) advantage = 1;
            if (mypet == Pokehbuddy.SmartChoiceDealMoreDMG(comboBox16.SelectedIndex + 1)) advantage = advantage + 2;
            s = textBox4.Text;   //advantage * 50 * AdFactor
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", trackBar3.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label55.Text = value.ToString();

            s = textBox5.Text;   //advantage * 50 * AdFactor
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", trackBar4.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label54.Text = value.ToString();



            s = textBox6.Text;   //advantage * 50 * AdFactor
            s = s.Replace("petLevel", Pokehbuddy.GetPetLevelPreCombat(2).ToString()).Replace("enemylevel", (comboBox15.SelectedIndex + 1).ToString()).Replace("LevelFactor", trackBar2.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label53.Text = value.ToString();

            label52.Text = total.ToString();

            //pet 3

            total = 0;
            disadvantage = 0;
            advantage = 0;

            s = textBox3.Text;
            s = s.Replace("petHP", Pokehbuddy.GetPetHPPreCombat(3).ToString()).Replace("HPFactor", trackBar1.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = int.Parse(value.ToString());
            label62.Text = value.ToString();


            mypet = Pokehbuddy.GetTypeByID(Pokehbuddy.ReadSlot(3));
            if (mypet == Pokehbuddy.DumbChoiceTakeMoreDMG(comboBox16.SelectedIndex + 1)) disadvantage = -2;
            if (mypet == Pokehbuddy.DumbChoiceDealLessDMG(comboBox16.SelectedIndex + 1)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == Pokehbuddy.SmartChoiceTakeLessDMG(comboBox16.SelectedIndex + 1)) advantage = 1;
            if (mypet == Pokehbuddy.SmartChoiceDealMoreDMG(comboBox16.SelectedIndex + 1)) advantage = advantage + 2;
            s = textBox4.Text;   //advantage * 50 * AdFactor
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", trackBar3.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label60.Text = value.ToString();

            s = textBox5.Text;   //advantage * 50 * AdFactor
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", trackBar4.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label59.Text = value.ToString();



            s = textBox6.Text;   //advantage * 50 * AdFactor
            s = s.Replace("petLevel", Pokehbuddy.GetPetLevelPreCombat(3).ToString()).Replace("enemylevel", (comboBox15.SelectedIndex + 1).ToString()).Replace("LevelFactor", trackBar2.Value.ToString());
            x = ce.Parse(s);
            value = x.Evaluate();
            total = total + int.Parse(value.ToString());
            label58.Text = value.ToString();

            label57.Text = total.ToString();


        }
        private void loadpreviewimages()
        {
            
            Logging.Write("loading image :" + ImgUrl(1));
            pictureBox6.ImageLocation = ImgUrl(1);

            Logging.Write("loading image :" + ImgUrl(2));
            pictureBox7.ImageLocation = ImgUrl(2);

            Logging.Write("loading image :" + ImgUrl(3));
            pictureBox8.ImageLocation = ImgUrl(3);


        }
        private string ImgUrl(int slotnum)
        {

            string theicon = Pokehbuddy.SlotIcon(slotnum);
            string baseurl = "http://wow.zamimg.com/images/wow/icons/large/";
            string replace1 = @"INTERFACE\ICONS\";

            string image = theicon.Replace(replace1, "").Replace(".BLP", "").ToLower();
            image = baseurl + image + ".jpg";
            return image;
        }

        private void trackBar2_Scroll(object sender, System.EventArgs e)
        {
            
        }

        private void tabPage3_Click(object sender, System.EventArgs e)
        {

        }

        private void button18_Click(object sender, System.EventArgs e)
        {
            listBox3.Items.Add(textBox7.Text);

        }

        private void button20_Click(object sender, System.EventArgs e)
        {
            listBox4.Items.Add(textBox7.Text);
        }

        private void button17_Click(object sender, System.EventArgs e)
        {
            if (listBox3.SelectedIndex > -1)
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
            }
        }

        private void button19_Click(object sender, System.EventArgs e)
        {
            if (listBox4.SelectedIndex > -1)
            {
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
            }
        }

        private void button26_Click(object sender, System.EventArgs e)
        {
            LoadDefaults();

        }
        private void LoadDefaults()
    {
        listBox6.Items.Clear();
            Pokehbuddy.LoadDefaultLogic("Default Logic");
            string dumdum = Pokehbuddy.DefaultLogicz.Logic;
            string[] PetLogics = dumdum.Split('@');
            foreach (string alogic in PetLogics)
            {
                listBox6.Items.Add(alogic);
            }

    }


        private void button23_Click(object sender, System.EventArgs e)
        {
            if (listBox6.SelectedIndex > -1)
            {
                listBox6.Items.RemoveAt(listBox6.SelectedIndex);
            }
        }

        private void button25_Click(object sender, System.EventArgs e)
        {
            SaveDefault();
        }
        private void SaveDefault()
        {
            int i = 0;
            string dummy = "";
            foreach (object item in listBox6.Items)
            {
                dummy = dummy + item.ToString();
                if (i < listBox6.Items.Count - 1) dummy = dummy + "@";
                i++;
            }
            Pokehbuddy.LoadDefaultLogic("Default Logic");
            Pokehbuddy.DefaultLogicz.Logic = dummy;
            Pokehbuddy.DefaultLogicz.Save();
        }

        private void button21_Click(object sender, System.EventArgs e)
        {
            MoveItemPetRotate(-1);
        }

        private void button22_Click(object sender, System.EventArgs e)
        {
            MoveItemPetRotate(1);
        }

        private void comboBox14_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Pokehbuddy.MySettings.MinPetsAlive = comboBox14.SelectedIndex + 1;

        }

        private void comboBox12_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Pokehbuddy.MySettings.UseBandagesToHeal = comboBox12.SelectedIndex;
        }

        private void comboBox10_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Pokehbuddy.MySettings.UseHealSkill = comboBox10.SelectedIndex;
        }

        private void textBox7_Enter(object sender, System.EventArgs e)
        {
            
        }

        private void textBox7_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void button28_Click(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                
                listBox6.Items.Add(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void label17_Click(object sender, System.EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, System.EventArgs e)
        {
            Pokehbuddy.MySettings.UseBlackList = checkBox5.Checked;

        }

        private void checkBox6_CheckedChanged(object sender, System.EventArgs e)
        {
            Pokehbuddy.MySettings.UseWhiteList = checkBox6.Checked;
        }

        private void button15_Click(object sender, System.EventArgs e)
        {
            if (comboBox13.Text == "default leveling")
            {


         textBox3.Text="petHP * HPFactor";
         textBox4.Text="advantage * 50 * AdFactor";
         textBox5.Text="disadvantage * 50 * DisFactor";
         textBox6.Text="(petLevel - enemylevel) * 4 * LevelFactor";

         trackBar1.Value = 8;
         trackBar2.Value = -50;
         trackBar3.Value = 4;
         trackBar4.Value = 4;
        







            }
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, System.EventArgs e)
        {
            
        }

        private void trackBar4_Scroll(object sender, System.EventArgs e)
        {
            
        }

        private void pictureBox1_DoubleClick(object sender, System.EventArgs e)
        {
            //button12.Enabled = true;
        }

        private void button30_Click(object sender, System.EventArgs e)
        {
                        //comboBox11.Text="SELECT ACTION";
            string macrostring = "Health(THISPET),ISLESSTHAN,NUMBER,0,25";
            //string[] macrologic = macrostring.Split(',');

            macrostring = comboBox2.Text + "," +
            comboBox3.Text + "," +
            comboBox4.Text + "," +
            textBox101.Text + "," +
            textBox102.Text;
            listBox7.Items.Add(macrostring);
            MacroSave();

        }
        private void SetBnB()
        {
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

            if (comboBox11.SelectedIndex > -1)
            {
                comboBox2.Enabled = true;
            }
            if (comboBox2.Text != "")
            {
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
            }
            if (comboBox4.Text != "")
            {
                button3.Enabled = true;
                if (comboBox11.SelectedIndex > -1 && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "") button4.Enabled = true;

            }
            if (textBox2.Text != "") button2.Enabled = true;

        }

        private void listBox7_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        
        }

        private void listBox7_DoubleClick(object sender, System.EventArgs e)
        {
            if (listBox7.SelectedIndex >-1) {
                            
            string macrostring = listBox7.SelectedItem.ToString();
            string[] macrologic = macrostring.Split(',');

            comboBox2.Text = macrologic[0];
            comboBox3.Text = macrologic[1];
            comboBox4.Text = macrologic[2];
            textBox101.Text = macrologic[3];
            textBox102.Text = macrologic[4];

            button3.Enabled = true;
            button4.Enabled = true;
            SetBnB();
            }
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {

        }

        private void button24_Click(object sender, System.EventArgs e)
        {
            if (listBox6.SelectedIndex == -1) return;
            string value = listBox6.SelectedItem.ToString();
            if (InputBox("Edit Logic", "Logic:", ref value) == DialogResult.OK)
            {
                int index = listBox6.SelectedIndex;
                listBox6.Items.RemoveAt(index);
                listBox6.Items.Insert(index, value);

            }
        }

        private void button27_Click(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            string value = listBox1.SelectedItem.ToString();
            if (InputBox("Edit Logic", "Logic:", ref value) == DialogResult.OK)
            {
                int index = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(index);
                listBox1.Items.Insert(index, value);

            }
        }

        private void button29_Click(object sender, System.EventArgs e)
        {
            listBox7.Items.RemoveAt(listBox7.SelectedIndex);
            MacroSave();
        }



    }

}

namespace Pokehbuddyplug
{
    public partial class configForm : Form
    {
        string[] options = { "COOLDOWN(SKILL(1))","EQUALS", "true,false",
	"COOLDOWN(SKILL(2))","EQUALS", "true,false", 
	"COOLDOWN(SKILL(3))","EQUALS", "true,false", 
	"Health(THISPET)","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,Health(ENEMYPET)", 
	"Health(ENEMYPET)","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,Health(THISPET)", 
	"MyPetLevel","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,EnemyPetLevel,EnemyPetLevel + NUMBER", 
	"EnemyPetLevel","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,MyPetLevel,MyPetLevel + NUMBER", 
	"ENEMYTYPE","EQUALS,ISNOT", "HUMANOID,DRAGONKIN,FLYING,UNDEAD,CRITTER,MAGIC,ELEMENTAL,BEAST,AQUATIC,MECHANICAL", 
	"HASBUFF(X)","EQUALS", "true,false", 
	"HASENEMYBUFF(X)","EQUALS","true,false",  
	"WEATHERBUFF(X)","EQUALS", "true,false", 
	"HASTEAMBUFF(X)","EQUALS", "true,false", 
	"ENEMYTEAMBUFF(X)","EQUALS","true,false", 
	"MYPETSPEED","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,ENEMYSPEED", 
	"ENEMYSPEED","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,MYPETSPEED",
"MyPetsAlive","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,EnemyPetsAlive",
"EnemyPetsAlive","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,MyPetsAlive"};


        /*MyPetsAlive
,
"","",""};
*/


        public configForm()
        {
            InitializeComponent();
            listBox2.Visible = false;
            this.pictureBox1.Image = new Bitmap(Application.StartupPath + "\\Plugins\\Pokehbuddy\\Images\\pb.jpg");
            this.pictureBox9.Image = new Bitmap(Application.StartupPath + "\\Plugins\\Pokehbuddy\\Images\\Broom.png");
            //this.pictureBox9.Image = new Bitmap(Application.StartupPath + "\\Plugins\\Pokehbuddy\\Images\\info.png");
        }

        private void TheDungeonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //      Pokehbuddy.MySettings.TheDungeon = TheDungeonComboBox.SelectedIndex;
            //BBLog("Dungeon " + TheDungeonComboBox.SelectedIndex);



        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.GetRarity = comboBox1.SelectedIndex + 1;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
            Pokehbuddy.MySettings.HPFactor = trackBar1.Value;



        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = trackBar2.Value.ToString();
            Pokehbuddy.MySettings.LevelFactor = trackBar2.Value;



        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            
            label3.Text = trackBar3.Value.ToString();
            Pokehbuddy.MySettings.AdFactor = trackBar3.Value;



        }
        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = trackBar4.Value.ToString();
            Pokehbuddy.MySettings.DisFactor = trackBar4.Value;



        }
        private void trackBar5_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = trackBar5.Value.ToString();
            Pokehbuddy.MySettings.Distance = trackBar5.Value;



        }
        private void ListBuffs_Clicked(object sender, EventArgs e)
        {
            Lua.DoString("for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,1,j)  print (buffid) end");


        }


        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == -1)
            {
                button3.Enabled = false;
                button4.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                comboBox4.Items.Clear();
            }
            if (comboBox11.SelectedIndex > -1)
            {
                comboBox2.Enabled = true;
                if (comboBox3.Text == "") comboBox3.Enabled = false;
                if (comboBox4.Text == "") comboBox4.Enabled = false;

                if (comboBox2.Text == "") comboBox2.Items.Clear();

                for (int i = 0; i < options.Count(); i++)
                {
                    comboBox2.Items.Add(options[i]);
                    i++;
                    i++;
                }
            }
            SetBnB();



        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = true;
            comboBox3.Text = "";
            comboBox3.Items.Clear();

            comboBox4.Enabled = true;
            comboBox4.Text = "";
            comboBox4.Items.Clear();


            string dumdumdum = comboBox2.Text;
            //Logging.Write("im here!!!"+dumdumdum);
            for (int i = 0; i < options.Count(); i++)
            {
                if (dumdumdum.Contains(options[i]))
                {

                    string[] equalizers = options[i + 1].Split(',');

                    foreach (string equalizer in equalizers)
                    {
                        comboBox3.Items.Add(equalizer);
                    }
                    string[] targetz = options[i + 2].Split(',');

                    foreach (string targ in targetz)
                    {
                        comboBox4.Items.Add(targ);
                    }

                }
                i++;
                i++;
            }






        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox4.Text.Contains("NUMBER"))
            {
                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }


            }




            if (comboBox4.Text.Contains("true"))
            {

                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }


            }
            if (comboBox4.Text.Contains("false"))
            {


                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }


            }

            if (comboBox11.SelectedIndex > -1)
            {
                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }

            }




        }
        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //AND
            string dummy = comboBox2.Text.Replace("(X)", "(" + textBox101.Text + ")") + " " + comboBox3.Text + " " + comboBox4.Text.Replace("NUMBER", "" + textBox102.Text + "") + " $ ";
            textBox1.Text = textBox1.Text + dummy;
            //comboBox11.Enabled = false;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            button3.Enabled = false;
            button4.Enabled = false;
            button2.Enabled = false;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //FINISH
            string dummy = comboBox11.Text + " " + textBox1.Text + comboBox2.Text.Replace("(X)", "(" + textBox101.Text + ")") + " " + comboBox3.Text + " " + comboBox4.Text.Replace("NUMBER", "" + textBox102.Text + "");
            textBox2.Text = dummy;
            textBox1.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            button2.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox2.Text);
            textBox2.Text = "";
            button2.Enabled = false;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            //Lua.DoString("for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,C_PetBattles.GetActivePet(1),j)  print (buffid) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,C_PetBattles.GetActivePet(1),j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');

            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;


        }
        private void button99_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            //Lua.DoString("for i=1, C_PetBattles.GetNumAuras(0,0) do local auraID = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(LE_BATTLE_PET_WEATHER,PET_BATTLE_PAD_INDEX) do  local buffid = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER,PET_BATTLE_PAD_INDEX,j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;
        }

        private void button919_Click(object sender, EventArgs e)
        {//               for i=1, C_PetBattles.GetNumAuras(1,0) do local auraID = C_PetBattles.GetAuraInfo(1, PET_BATTLE_PAD_INDEX, i) print(auraID) end
            listBox2.Items.Clear();
            //Lua.DoString("for i=1, C_PetBattles.GetNumAuras(1,0) do local auraID = C_PetBattles.GetAuraInfo(1, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(1,0) do  local buffid = C_PetBattles.GetAuraInfo(1,PET_BATTLE_PAD_INDEX,j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;


        }
        private void button929_Click(object sender, EventArgs e)
        {
            //get whole pet list
            /*		for (int intI = 1; intI < 597; intI++) {
                    List<string> cntlist = Lua.GetReturnValues("local stor = '' local petID, speciesID, _, _, _, _, _, name, icon  = C_PetJournal.GetPetInfoByIndex("+ intI +", false); stor = stor .. '*'..speciesID .. '*,*' .. name ..'*,*' .. icon .. '*,'  return stor");

                    Logging.Write(cntlist[0]);
                    }*/

            //


            listBox2.Items.Clear();
            //Lua.DoString("for i=1, C_PetBattles.GetNumAuras(2,0) do local auraID = C_PetBattles.GetAuraInfo(2, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(2,0) do  local buffid = C_PetBattles.GetAuraInfo(2,PET_BATTLE_PAD_INDEX,j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            //tabControl1.Visible=false;

            //Lua.DoString("for j=1,C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2)) do  local buffid = C_PetBattles.GetAuraInfo(2,C_PetBattles.GetActivePet(2),j)  print (buffid) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2)) do  local buffid = C_PetBattles.GetAuraInfo(2,C_PetBattles.GetActivePet(2),j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;
        }
        

        public void MoveItemPetRotate(int direction)
        {
            // Checking selected item
            if (listBox5.SelectedItem == null || listBox5.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox5.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox5.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox5.SelectedItem;

            // Removing removable element
            listBox5.Items.Remove(selected);
            // Insert it in new position
            listBox5.Items.Insert(newIndex, selected);
            // Restore selection
            listBox5.SetSelected(newIndex, true);
        }
        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox1.SelectedItem == null || listBox1.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox1.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox1.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox1.SelectedItem;

            // Removing removable element
            listBox1.Items.Remove(selected);
            // Insert it in new position
            listBox1.Items.Insert(newIndex, selected);
            // Restore selection
            listBox1.SetSelected(newIndex, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label71.Text + ".xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            listBox1.Items.Clear();
            Pokehbuddy pok = new Pokehbuddy();
            pok.LoadPetSettings(label71.Text, label22.Text);

            //string dumdum = "";
            string dumdum = Pokehbuddy.PetSettings.Logic;
            string[] PetLogics = dumdum.Split('@');
            foreach (string alogic in PetLogics)
            {
                listBox1.Items.Add(alogic);
            }


        }
        private void button5_Click(object sender, EventArgs e)
        {

            MoveItem(-1);

        }

        private void button6_Click(object sender, EventArgs e)
        {

            MoveItem(1);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1) listBox1.Items.Remove(listBox1.SelectedItem);

        }
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            textBox101.Text = listBox2.Text;
            listBox2.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex > -1)
            {
                listBox1.Items.Clear();
                Pokehbuddy pok = new Pokehbuddy();
                pok.LoadPetSettings(Pokehbuddy.ReadSlot(comboBox9.SelectedIndex + 1), Pokehbuddy.ReadSlotSpecies(comboBox9.SelectedIndex + 1));
                label22.Text = Pokehbuddy.ReadSlotSpecies(comboBox9.SelectedIndex + 1);
                label71.Text = Pokehbuddy.ReadSlot(comboBox9.SelectedIndex + 1);
                //string dumdum = "";

                string dumdum = Pokehbuddy.PetSettings.Logic;
                string[] PetLogics = dumdum.Split('@');
                foreach (string alogic in PetLogics)
                {
                    listBox1.Items.Add(alogic);
                }

                string theicon = Pokehbuddy.SlotIcon(comboBox9.SelectedIndex + 1);
                string baseurl = "http://wow.zamimg.com/images/wow/icons/large/";
                string replace1 = @"INTERFACE\ICONS\";

                string image = theicon.Replace(replace1, "").Replace(".BLP", "").ToLower();
                image = baseurl + image + ".jpg";
                Logging.Write("loading image :" + image);
                pictureBox2.ImageLocation = image;
                /*//5.1//
                label20.Text=pok.GetNameByID(label71.Text);
                List<string> cnt3 = Lua.GetReturnValues("local speciesID, customName, _, _, _,_, name, icon  = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")) if customName==nil then return 'No custom name' end	return customName");
                label22.Text=cnt3[0];
                //List<string> cnt4 = Lua.GetReturnValues("local speciesID, customName, _, _, _,_, name, icon, petType  = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")) return petType");
                //label22.Text=cnt4[0];
                5.1*/

            }
        }
        private string GetPetImage(int slot)
        {
            List<string> cnt2 = Lua.GetReturnValues("local petid = C_PetJournal.GetPetLoadOutInfo(" + slot + ") local speciesID, customName, _, _, _,_, name, icon  = C_PetJournal.GetPetInfoByPetID(petid) return icon");
            string baseurl = "http://wow.zamimg.com/images/wow/icons/large/";
            string replace1 = @"INTERFACE\ICONS\";

            string image = cnt2[0].Replace(replace1, "").Replace(".BLP", "").ToLower();
            image = baseurl + image + ".jpg";
            return (image);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
            {
                SavePetSpecific();
            }


            if (checkBox9.Checked)
            {
                SaveSpecies(); //Save Species & Similar
            }
            if (checkBox9.Checked && checkBox10.Checked)
            {
                SaveNOverwrite(); //Save Species & Similar & Overwrite
            }



        }
        private void SavePetSpecific()
        {
            if (label71.Text != "")
            {
                string dummy = "";
                Pokehbuddy pok = new Pokehbuddy();
                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }
                //local petID, ability1ID, ability2ID, ability3ID, locked = C_PetJournal.GetPetLoadOutInfo(i) return ability1ID, ability2ID, ability3ID
                


                pok.LoadPetSettings(label71.Text, label22.Text);
                List<string> cnt1 = Lua.GetReturnValues("for i=1,3 do local petID, ability1ID, ability2ID, ability3ID, locked = C_PetJournal.GetPetLoadOutInfo(i) if (tonumber(petID,16)==" + label71.Text + ") then return ability1ID, ability2ID, ability3ID end end return 0,0,0");
                string spelllayout = "ASSIGNABILITY1(" + cnt1[0] + ")@" +
                                     "ASSIGNABILITY2(" + cnt1[1] + ")@" +
                                     "ASSIGNABILITY3(" + cnt1[2] + ")";
                Pokehbuddy.PetSettings.SpellLayout = spelllayout;
                Pokehbuddy.PetSettings.Logic = dummy;
                Pokehbuddy.PetSettings.Save();

            }
        }

        private void SaveSpecies()
        {
            Pokehbuddy pok = new Pokehbuddy();
            if (label71.Text != "")
            {
                string dummy = "";

                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }
                pok.LoadPetSettingsBN(label22.Text);

                List<string> cntskillz = Lua.GetReturnValues("for i=1,3 do local petID, ability1ID, ability2ID, ability3ID, locked = C_PetJournal.GetPetLoadOutInfo(i) if (tonumber(petID,16)==" + label71.Text + ") then return ability1ID, ability2ID, ability3ID end end return 0,0,0");
                string spelllayout = "ASSIGNABILITY1(" + cntskillz[0] + ")@" +
                                     "ASSIGNABILITY2(" + cntskillz[1] + ")@" +
                                     "ASSIGNABILITY3(" + cntskillz[2] + ")";
                Pokehbuddy.PetSettings.SpellLayout = spelllayout;



                Pokehbuddy.PetSettings.Logic = dummy;
                Pokehbuddy.PetSettings.Save();







                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
                List<string> cnt1 = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + pok.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,_,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=ass end end return teller");
                int getal = 0;
                try
                {
                    getal = Convert.ToInt32(cnt1[0]);
                }
                catch (Exception exc)
                {

                }
                for (int intI = 1; intI < getal; intI++)
                {
                    List<string> cnt = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + pok.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,speciesID,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=speciesID end end return (retdata[" + intI + "])");
                    cnt[0] = pok.GetNameBySpeciesID(cnt[0]);
                    string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + cnt[0] + ".xml";
                    if (!File.Exists(filename))
                    {

                        string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label22.Text + ".xml";
                        if (File.Exists(filename2))
                        {
                            File.Copy(filename2, filename);
                        }
                    }



                    Logging.Write(cnt[0]);
                }




                // BBLog(cnt[0]);
                //if (cnt[0]=="1") dummy=true;






            }
        }
        private void SaveNOverwrite()
        {
            Pokehbuddy pok = new Pokehbuddy();
            if (label71.Text != "")
            {
                string dummy = "";

                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }

                pok.LoadPetSettingsBN(label22.Text);

                List<string> cntskillz = Lua.GetReturnValues("for i=1,3 do local petID, ability1ID, ability2ID, ability3ID, locked = C_PetJournal.GetPetLoadOutInfo(i) if (tonumber(petID,16)==" + label71.Text + ") then return ability1ID, ability2ID, ability3ID end end return 0,0,0");
                string spelllayout = "ASSIGNABILITY1(" + cntskillz[0] + ")@" +
                                     "ASSIGNABILITY2(" + cntskillz[1] + ")@" +
                                     "ASSIGNABILITY3(" + cntskillz[2] + ")";
                Pokehbuddy.PetSettings.SpellLayout = spelllayout;



                Pokehbuddy.PetSettings.Logic = dummy;
                Pokehbuddy.PetSettings.Save();







                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
                List<string> cnt1 = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X'," + label71.Text + ")); local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,_,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=ass end end return teller");
                int getal = 0;
                try
                {
                    getal = Convert.ToInt32(cnt1[0]);
                }
                catch (Exception exc)
                {

                }
                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                for (int intI = 1; intI < getal; intI++)
                {
                    List<string> cnt = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + pok.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,speciesID,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=speciesID  end end return (retdata[" + intI + "])");
                    //Logging.Write(cnt[0]);
                    cnt[0] = pok.GetNameBySpeciesID(cnt[0]);
                    string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + cnt[0] + ".xml";
                    string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label22.Text + ".xml";
                    //Logging.Write("File 1 : "+filename+ " File 2 :"+filename2);
                    if (File.Exists(filename) && filename != filename2) File.Delete(filename);

                    //string filename2=Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\"+pok.GetNameByID(label71.Text)+".xml";
                    if (File.Exists(filename2) && filename != filename2)
                    {
                        File.Copy(filename2, filename);
                    }




                    Logging.Write(cnt[0]);
                }




                // BBLog(cnt[0]);
                //if (cnt[0]=="1") dummy=true;






            }

        }


        private void button44_Click(object sender, EventArgs e)
        {


        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.ForfeitIfNotInteresting = checkBox2.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            

        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.DetailedLogging = checkBox4.Checked;

        }




        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.BelowLevel = comboBox5.SelectedIndex;


            /*            comboBox6.Items.Clear();
                        int dummy=0;
                        for (int i = 1; i < 60; i++)
                        {
                            dummy=comboBox5.SelectedIndex+1;
                            comboBox6.Items.Add(dummy + i);
                        }*/
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.AboveLevel = comboBox6.SelectedIndex;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.PVPMinTime = comboBox7.SelectedIndex;
            if (initdone == 1)
            {
                comboBox8.Items.Clear();
                int dummy = 0;
                for (int i = 1; i < 60; i++)
                {
                    dummy = comboBox7.SelectedIndex + 1;
                    comboBox8.Items.Add(dummy + i);
                }
                comboBox8.SelectedIndex = 0;
            }

        }
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initdone == 1) Pokehbuddy.MySettings.PVPMaxTime = comboBox8.SelectedIndex;
            if (initdone == 0) comboBox8.SelectedIndex = Pokehbuddy.MySettings.PVPMaxTime;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.DoPVP = checkBox1.Checked;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            listBox2.Visible = false;
            groupBox4.Visible = false;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            /*5.1
            //Pokehbuddy pok = new Pokehbuddy();
            int i=0;
               for (i=1;i<4;i++){
                List<string> cnt2 = Lua.GetReturnValues("local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")); local skillist = C_PetJournal.GetPetAbilityList(dummy1) name, icon = C_PetJournal.GetPetAbilityInfo(skillist["+i+"]); return icon");
                string baseurl="http://wow.zamimg.com/images/wow/icons/large/";
                string replace1=@"INTERFACE\ICONS\";
			
                string image = cnt2[0].Replace(replace1,"").Replace(".BLP","").ToLower();
                image=baseurl+image+".jpg";
                if (i==1) pictureBox3.ImageLocation=image;
                if (i==2) pictureBox4.ImageLocation=image;
                if (i==3) pictureBox5.ImageLocation=image;
                //Logging.Write("loading image" + image);
                List<string> cnt3 = Lua.GetReturnValues("local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")); local skillist = C_PetJournal.GetPetAbilityList(dummy1) name, icon = C_PetJournal.GetPetAbilityInfo(skillist["+i+"]); return name");
                if (i==1) label23.Text=cnt3[0];
                if (i==2) label28.Text=cnt3[0];
                if (i==3) label32.Text=cnt3[0];
			
            }
            label24.Visible=false;
            label25.Visible=false;
            label26.Visible=false;
            label27.Visible=false;
            label30.Visible=false;
            label29.Visible=false;
		
			
			
                //List<string> cnt3 = Lua.GetReturnValues("local speciesID, customName, _, _, _,_, name, icon  = C_PetJournal.GetPetInfoByPetID("+label71.Text+") if customName==nil then return 'No custom name' end	return customName");
                //label22.Text=cnt3[0];
                groupBox4.Visible=true;
            5.1*/
        }


        private void button45_Click(object sender, EventArgs e)
        {






        }

    }









}




