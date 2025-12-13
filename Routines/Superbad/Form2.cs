#region

using System;
using System.Windows.Forms;
using Styx.Common;

#endregion

namespace Superbad
{
    public partial class SuperbadConfig : Form
    {
        public SuperbadConfig()
        {
            InitializeComponent();
        }

        private void SuperbadConfig_Load(object sender, EventArgs e)
        {
            switch (SuperbadSettings.Instance.Form)
            {
                case SuperbadSettings.Shapeshift.CAT:
                    comboBox1.SelectedIndex = 0;
                    break;
                case SuperbadSettings.Shapeshift.BEAR:
                    comboBox1.SelectedIndex = 1;
                    break;
                case SuperbadSettings.Shapeshift.AUTO:
                    comboBox1.SelectedIndex = 2;
                    break;
                case SuperbadSettings.Shapeshift.MANUAL:
                    comboBox1.SelectedIndex = 3;
                    break;
            }
            numericUpDown1.Value = SuperbadSettings.Instance.AddsBearSwitch;
            numericUpDown2.Value = SuperbadSettings.Instance.HealthBearSwitch;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.NONE)
                comboBox2.SelectedIndex = 0;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox2.SelectedIndex = 1;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox2.SelectedIndex = 2;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.LCTRL)
                comboBox2.SelectedIndex = 3;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.RCTRL)
                comboBox2.SelectedIndex = 4;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.LALT)
                comboBox2.SelectedIndex = 5;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.RALT)
                comboBox2.SelectedIndex = 6;
            checkBox1.Checked = SuperbadSettings.Instance.PrintMsg;
            numericUpDown3.Value = SuperbadSettings.Instance.Trinket1Percent;
            numericUpDown4.Value = SuperbadSettings.Instance.Trinket2Percent;
            comboBox6.SelectedIndex = SuperbadSettings.Instance.Trinket1Usage;
            comboBox7.SelectedIndex = SuperbadSettings.Instance.Trinket2Usage;
            checkBox2.Checked = SuperbadSettings.Instance.UseMovement;
            checkBox3.Checked = SuperbadSettings.Instance.UseTargeting;
            numericUpDown18.Value = SuperbadSettings.Instance.CatAoe;
            numericUpDown19.Value = SuperbadSettings.Instance.BearAoe;
            checkBox10.Checked = SuperbadSettings.Instance.Mdw;
            checkBox11.Checked = SuperbadSettings.Instance.Mdwgroup;
            numericUpDown9.Value = SuperbadSettings.Instance.RestHealth;
            numericUpDown10.Value = SuperbadSettings.Instance.RestMana;
            checkBox5.Checked = SuperbadSettings.Instance.MightyBash;
            checkBox6.Checked = SuperbadSettings.Instance.UseBlink;
            checkBox7.Checked = SuperbadSettings.Instance.UseBerserk;
            checkBox8.Checked = SuperbadSettings.Instance.UseHotW;
            checkBox9.Checked = SuperbadSettings.Instance.UseTaunt;
            checkBox13.Checked = SuperbadSettings.Instance.UseSavageDefense;
            checkBox14.Checked = SuperbadSettings.Instance.UseFrenziedRegen;
            checkBox15.Checked = SuperbadSettings.Instance.UseDash;
            checkBox17.Checked = SuperbadSettings.Instance.Rooted;
            numericUpDown14.Value = SuperbadSettings.Instance.Barkskin;
            numericUpDown13.Value = SuperbadSettings.Instance.Frenzied;
            numericUpDown5.Value = SuperbadSettings.Instance.MightofUrsoc;
            numericUpDown11.Value = SuperbadSettings.Instance.Renewal;
            numericUpDown12.Value = SuperbadSettings.Instance.CenarionWard;
            numericUpDown7.Value = SuperbadSettings.Instance.Survival;
            numericUpDown8.Value = SuperbadSettings.Instance.Predatory;
            numericUpDown17.Value = SuperbadSettings.Instance.HealthStone;
            numericUpDown16.Value = SuperbadSettings.Instance.OoCHealingTouch;
            numericUpDown15.Value = SuperbadSettings.Instance.OoCReju;
            numericUpDown20.Value = SuperbadSettings.Instance.LifeSpirit;
            checkBox18.Checked = SuperbadSettings.Instance.StayInStealth;
            checkBox19.Checked = SuperbadSettings.Instance.SavageFarm;
            checkBox20.Checked = SuperbadSettings.Instance.UsePotion;
            checkBox22.Checked = SuperbadSettings.Instance.HealOthers;
            checkBox23.Checked = SuperbadSettings.Instance.RakeCycle;
            checkBox24.Checked = SuperbadSettings.Instance.UseFff;
            checkBox25.Checked = SuperbadSettings.Instance.PullStealth;
            checkBox27.Checked = SuperbadSettings.Instance.UseStampedingRoar;
            checkBox28.Checked = SuperbadSettings.Instance.UseRebirth;
            if (SuperbadSettings.Instance.StealthOpener == 1)
                radioButton2.Select();
            if (SuperbadSettings.Instance.StealthOpener == 0)
                radioButton1.Select();
            if (SuperbadSettings.Instance.RebirthMode == 0)
                radioButton3.Select();
            if (SuperbadSettings.Instance.RebirthMode == 1)
                radioButton4.Select();
            if (SuperbadSettings.Instance.RebirthMode == 2)
                radioButton5.Select();
            if (SuperbadSettings.Instance.RebirthMode == 3)
                radioButton6.Select();
            numericUpDown21.Value = SuperbadSettings.Instance.HealingTouchCombat;
            numericUpDown22.Value = SuperbadSettings.Instance.RejuvenationCombat;
            checkBox29.Checked = SuperbadSettings.Instance.WrathSpam;
            checkBox30.Checked = SuperbadSettings.Instance.SymbTarget;
            checkBox31.Checked = SuperbadSettings.Instance.SymbSpell;
            checkBox32.Checked = SuperbadSettings.Instance.Update;
            checkBox12.Checked = SuperbadSettings.Instance.WaitSickness;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.NONE)
                comboBox3.SelectedIndex = 0;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox3.SelectedIndex = 1;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox3.SelectedIndex = 2;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.LCTRL)
                comboBox3.SelectedIndex = 3;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.RCTRL)
                comboBox3.SelectedIndex = 4;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.LALT)
                comboBox3.SelectedIndex = 5;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.RALT)
                comboBox3.SelectedIndex = 6;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.NONE)
                comboBox4.SelectedIndex = 0;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox4.SelectedIndex = 1;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox4.SelectedIndex = 2;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.LCTRL)
                comboBox4.SelectedIndex = 3;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.RCTRL)
                comboBox4.SelectedIndex = 4;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.LALT)
                comboBox4.SelectedIndex = 5;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.RALT)
                comboBox4.SelectedIndex = 6;
            checkBox21.Checked = SuperbadSettings.Instance.UseBurst;
            checkBox33.Checked = SuperbadSettings.Instance.UseBurstTigers;
            checkBox34.Checked = SuperbadSettings.Instance.UseBurstBerserk;
            checkBox35.Checked = SuperbadSettings.Instance.UseBurstVigil;
            checkBox36.Checked = SuperbadSettings.Instance.UseBurstIncar;
            checkBox37.Checked = SuperbadSettings.Instance.UseBurstBerserking;
            checkBox38.Checked = SuperbadSettings.Instance.UseBurstFeralSpirit;
            checkBox39.Checked = SuperbadSettings.Instance.UseBurstHotw;
            checkBox40.Checked = SuperbadSettings.Instance.UseBurstGloves;
            checkBox41.Checked = SuperbadSettings.Instance.UseBurstTrinket1;
            checkBox42.Checked = SuperbadSettings.Instance.UseBurstTrinket2;
            checkBox43.Checked = SuperbadSettings.Instance.UseBurstVirmens;
            checkBox44.Checked = SuperbadSettings.Instance.UseBurstLifeBlood;
            checkBox45.Checked = SuperbadSettings.Instance.UseBearHug;
            checkBox46.Checked = SuperbadSettings.Instance.UseAquatic;
            checkBox47.Checked = SuperbadSettings.Instance.UseTravel;
            checkBox48.Checked = SuperbadSettings.Instance.UseFoN;
            checkBox49.Checked = SuperbadSettings.Instance.UseAoeKey;
            checkBox50.Checked = SuperbadSettings.Instance.InterruptAnyone;
            checkBox51.Checked = SuperbadSettings.Instance.InterruptRandomize;
            numericUpDown23.Value = SuperbadSettings.Instance.InterruptFailPercentage;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    SuperbadSettings.Instance.Form = SuperbadSettings.Shapeshift.CAT;
                    break;
                case 1:
                    SuperbadSettings.Instance.Form = SuperbadSettings.Shapeshift.BEAR;
                    break;
                case 2:
                    SuperbadSettings.Instance.Form = SuperbadSettings.Shapeshift.AUTO;
                    break;
                case 3:
                    SuperbadSettings.Instance.Form = SuperbadSettings.Shapeshift.MANUAL;
                    break;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.AddsBearSwitch = (int) numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealthBearSwitch = (int) numericUpDown2.Value;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.NONE;
            if (comboBox2.SelectedIndex == 1)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox2.SelectedIndex == 2)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox2.SelectedIndex == 3)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox2.SelectedIndex == 4)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox2.SelectedIndex == 5)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.LALT;
            if (comboBox2.SelectedIndex == 6)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.RALT;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.NONE;
            if (comboBox3.SelectedIndex == 1)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox3.SelectedIndex == 2)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox3.SelectedIndex == 3)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox3.SelectedIndex == 4)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox3.SelectedIndex == 5)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.LALT;
            if (comboBox3.SelectedIndex == 6)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.RALT;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.NONE;
            if (comboBox4.SelectedIndex == 1)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox4.SelectedIndex == 2)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox4.SelectedIndex == 3)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox4.SelectedIndex == 4)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox4.SelectedIndex == 5)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.LALT;
            if (comboBox4.SelectedIndex == 6)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.RALT;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.PrintMsg = checkBox1.Checked;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket1Percent = (int) numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket2Percent = (int) numericUpDown4.Value;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket1Usage = comboBox6.SelectedIndex;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket2Usage = comboBox7.SelectedIndex;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseMovement = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTargeting = checkBox3.Checked;
        }

        private void numericUpDown18_ValueChanged_1(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.CatAoe = (int) numericUpDown18.Value;
        }

        private void numericUpDown19_ValueChanged_1(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BearAoe = (int) numericUpDown19.Value;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Mdw = checkBox10.Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Mdwgroup = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.WaitSickness = checkBox12.Checked;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RestHealth = (int) numericUpDown9.Value;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RestMana = (int) numericUpDown10.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Save();
            SuperbadSettings.printSettings();
            Logging.Write("Config saved!");
            Close();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.MightyBash = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBlink = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBerserk = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseHotW = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTaunt = checkBox9.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseSavageDefense = checkBox13.Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFrenziedRegen = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseDash = checkBox15.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Rooted = checkBox17.Checked;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Barkskin = (int) numericUpDown14.Value;
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Frenzied = (int) numericUpDown13.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.MightofUrsoc = (int) numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Renewal = (int) numericUpDown11.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.CenarionWard = (int) numericUpDown12.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Survival = (int) numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Predatory = (int) numericUpDown8.Value;
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealthStone = (int) numericUpDown17.Value;
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.OoCHealingTouch = (int) numericUpDown16.Value;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.OoCReju = (int) numericUpDown15.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Save();
            SuperbadSettings.printSettings();
            Logging.Write("Config saved!");
            Close();
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.LifeSpirit = (int) numericUpDown20.Value;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.StayInStealth = checkBox18.Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.SavageFarm = checkBox19.Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealOthers = checkBox22.Checked;
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RakeCycle = checkBox23.Checked;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFff = checkBox24.Checked;
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.PullStealth = checkBox25.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                SuperbadSettings.Instance.StealthOpener = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                SuperbadSettings.Instance.StealthOpener = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Save();
            SuperbadSettings.printSettings();
            Logging.Write("Config saved!");
            Close();
        }

        private void checkBox20_CheckedChanged_1(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UsePotion = checkBox20.Checked;
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseStampedingRoar = checkBox27.Checked;
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseRebirth = checkBox28.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                SuperbadSettings.Instance.RebirthMode = 0;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                SuperbadSettings.Instance.RebirthMode = 1;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
                SuperbadSettings.Instance.RebirthMode = 2;
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealingTouchCombat = (int) numericUpDown21.Value;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RejuvenationCombat = (int) numericUpDown22.Value;
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.WrathSpam = checkBox29.Checked;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                SuperbadSettings.Instance.RebirthMode = 3;
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.SymbTarget = checkBox30.Checked;
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.SymbSpell = checkBox31.Checked;
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Update = checkBox32.Checked;
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurst = checkBox21.Checked;
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstTigers = checkBox33.Checked;
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstBerserk = checkBox34.Checked;
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstVigil = checkBox35.Checked;
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstIncar = checkBox36.Checked;
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstBerserking = checkBox37.Checked;
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstFeralSpirit = checkBox38.Checked;
        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstHotw = checkBox39.Checked;
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstGloves = checkBox40.Checked;
        }

        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstTrinket1 = checkBox41.Checked;
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstTrinket2 = checkBox42.Checked;
        }

        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstVirmens = checkBox43.Checked;
        }

        private void checkBox44_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstLifeBlood = checkBox44.Checked;
        }

        private void checkBox45_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBearHug = checkBox45.Checked;
        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseAquatic = checkBox46.Checked;
        }

        private void checkBox47_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTravel = checkBox47.Checked;
        }

        private void checkBox48_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFoN = checkBox48.Checked;
        }

        private void checkBox49_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseAoeKey = checkBox49.Checked;
        }

        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.InterruptAnyone = checkBox50.Checked;
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.InterruptFailPercentage = (int) numericUpDown23.Value;
        }

        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.InterruptRandomize = checkBox51.Checked;
        }
    }
}