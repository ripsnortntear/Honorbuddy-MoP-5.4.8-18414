using System;
using System.Windows.Forms;
using Styx.Common;

namespace Superbad
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            switch (SuperbadSettings.Instance.Form)
            {
                case SuperbadSettings.Shapeshift.CAT:
                    comboBox8.SelectedIndex = 0;
                    break;
                case SuperbadSettings.Shapeshift.BEAR:
                    comboBox8.SelectedIndex = 1;
                    break;
                case SuperbadSettings.Shapeshift.AUTO:
                    comboBox8.SelectedIndex = 2;
                    break;
                case SuperbadSettings.Shapeshift.MANUAL:
                    comboBox8.SelectedIndex = 3;
                    break;
            }
            checkBox14.Checked = SuperbadSettings.Instance.ShiftOnMobCount;
            numericUpDown6.Value = SuperbadSettings.Instance.AddsBearSwitch;
            checkBox15.Checked = SuperbadSettings.Instance.ShiftOnLowHealth;
            numericUpDown7.Value = SuperbadSettings.Instance.HealthBearSwitch;
            checkBox1.Checked = SuperbadSettings.Instance.UseMovement;
            checkBox2.Checked = SuperbadSettings.Instance.UseTargeting;
            checkBox3.Checked = SuperbadSettings.Instance.UseFacing;
            checkBox4.Checked = SuperbadSettings.Instance.Suspend;
            checkBox6.Checked = SuperbadSettings.Instance.UseTrinket1;
            checkBox7.Checked = SuperbadSettings.Instance.UseTrinket2;
            comboBox1.SelectedIndex = SuperbadSettings.Instance.Trinket1Usage;
            comboBox2.SelectedIndex = SuperbadSettings.Instance.Trinket2Usage;
            numericUpDown3.Value = SuperbadSettings.Instance.Trinket1Percent;
            numericUpDown4.Value = SuperbadSettings.Instance.Trinket2Percent;
            checkBox8.Checked = SuperbadSettings.Instance.UseGloves;
            checkBox9.Checked = SuperbadSettings.Instance.UseLifeBlood;
            checkBox5.Checked = SuperbadSettings.Instance.UseRest;
            numericUpDown1.Value = SuperbadSettings.Instance.RestHealth;
            numericUpDown2.Value = SuperbadSettings.Instance.RestMana;
            checkBox10.Checked = SuperbadSettings.Instance.UseRacial;
            checkBox29.Checked = SuperbadSettings.Instance.SymbTarget;
            checkBox30.Checked = SuperbadSettings.Instance.SymbSpell;
            checkBox31.Checked = SuperbadSettings.Instance.UseRebirth;
            comboBox9.SelectedIndex = SuperbadSettings.Instance.RebirthMode;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.NONE)
                comboBox4.SelectedIndex = 0;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox4.SelectedIndex = 1;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox4.SelectedIndex = 2;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.LCTRL)
                comboBox4.SelectedIndex = 3;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.RCTRL)
                comboBox4.SelectedIndex = 4;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.LALT)
                comboBox4.SelectedIndex = 5;
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.RALT)
                comboBox4.SelectedIndex = 6;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.NONE)
                comboBox5.SelectedIndex = 0;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox5.SelectedIndex = 1;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox5.SelectedIndex = 2;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.LCTRL)
                comboBox5.SelectedIndex = 3;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.RCTRL)
                comboBox5.SelectedIndex = 4;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.LALT)
                comboBox5.SelectedIndex = 5;
            if (SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.RALT)
                comboBox5.SelectedIndex = 6;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.NONE)
                comboBox7.SelectedIndex = 0;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox7.SelectedIndex = 1;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox7.SelectedIndex = 2;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.LCTRL)
                comboBox7.SelectedIndex = 3;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.RCTRL)
                comboBox7.SelectedIndex = 4;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.LALT)
                comboBox7.SelectedIndex = 5;
            if (SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.RALT)
                comboBox7.SelectedIndex = 6;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.NONE)
                comboBox6.SelectedIndex = 0;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.LSHIFT)
                comboBox6.SelectedIndex = 1;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.RSHIFT)
                comboBox6.SelectedIndex = 2;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.LCTRL)
                comboBox6.SelectedIndex = 3;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.RCTRL)
                comboBox6.SelectedIndex = 4;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.LALT)
                comboBox6.SelectedIndex = 5;
            if (SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.RALT)
                comboBox6.SelectedIndex = 6;
            checkBox13.Checked = SuperbadSettings.Instance.PrintMsg;
            checkBox32.Checked = SuperbadSettings.Instance.UseBurst;
            checkBox33.Checked = SuperbadSettings.Instance.UseAoeKey;
            checkBox26.Checked = SuperbadSettings.Instance.UseSkullBash;
            checkBox27.Checked = SuperbadSettings.Instance.InterruptAnyone;
            checkBox28.Checked = SuperbadSettings.Instance.InterruptRandomize;
            numericUpDown10.Value = SuperbadSettings.Instance.InterruptFailPercentage;
            checkedListBox1.SetItemCheckState(0,
                SuperbadSettings.Instance.UseBurstTigers ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox1.SetItemCheckState(1,
                SuperbadSettings.Instance.UseBurstBerserk ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox1.SetItemCheckState(2,
                SuperbadSettings.Instance.UseBurstVigil ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox1.SetItemCheckState(3,
                SuperbadSettings.Instance.UseBurstIncar ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox1.SetItemCheckState(4,
                SuperbadSettings.Instance.UseBurstBerserking ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox1.SetItemCheckState(5,
                SuperbadSettings.Instance.UseBurstVirmens ? CheckState.Checked : CheckState.Unchecked);
            
            
            checkedListBox2.SetItemCheckState(0,
                SuperbadSettings.Instance.UseBurstFeralSpirit ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox2.SetItemCheckState(1,
                SuperbadSettings.Instance.UseBurstHotw ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox2.SetItemCheckState(2,
                SuperbadSettings.Instance.UseBurstGloves ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox2.SetItemCheckState(3,
                SuperbadSettings.Instance.UseBurstTrinket1 ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox2.SetItemCheckState(4,
                SuperbadSettings.Instance.UseBurstTrinket2 ? CheckState.Checked : CheckState.Unchecked);
            checkedListBox2.SetItemCheckState(5,
                SuperbadSettings.Instance.UseBurstLifeBlood ? CheckState.Checked : CheckState.Unchecked);
            checkBox11.Checked = SuperbadSettings.Instance.Mdw;
            checkBox12.Checked = SuperbadSettings.Instance.Mdwgroup;
            checkBox34.Checked = SuperbadSettings.Instance.UseDash;
            comboBox10.SelectedIndex = SuperbadSettings.Instance.DashUsage;
            checkBox35.Checked = SuperbadSettings.Instance.UseStampedingRoar;
            comboBox11.SelectedIndex = SuperbadSettings.Instance.StampedingRoarUsage;
            checkBox37.Checked = SuperbadSettings.Instance.UseAquatic;
            checkBox38.Checked = SuperbadSettings.Instance.UseTravel;
            checkBox39.Checked = SuperbadSettings.Instance.Rooted;
            numericUpDown11.Value = SuperbadSettings.Instance.Barkskin;
            numericUpDown12.Value = SuperbadSettings.Instance.HealthStone;
            numericUpDown13.Value = SuperbadSettings.Instance.LifeSpirit;
            numericUpDown16.Value = SuperbadSettings.Instance.Survival;
            numericUpDown15.Value = SuperbadSettings.Instance.HealingTouchCombat;
            numericUpDown14.Value = SuperbadSettings.Instance.RejuvenationCombat;
            numericUpDown18.Value = SuperbadSettings.Instance.OoCHealingTouch;
            numericUpDown17.Value = SuperbadSettings.Instance.OoCReju;
            numericUpDown23.Value = SuperbadSettings.Instance.Predatory;
            checkBox21.Checked = SuperbadSettings.Instance.UseHotW;
            checkBox36.Checked = SuperbadSettings.Instance.WrathSpam;
            checkBox16.Checked = SuperbadSettings.Instance.UseBlink;
            numericUpDown8.Value = SuperbadSettings.Instance.Renewal;
            numericUpDown9.Value = SuperbadSettings.Instance.CenarionWard;
            checkBox18.Checked = SuperbadSettings.Instance.UseInca;
            checkBox25.Checked = SuperbadSettings.Instance.HealOthers;
            checkBox17.Checked = SuperbadSettings.Instance.UseWildCharge;
            checkBox19.Checked = SuperbadSettings.Instance.UseFoN;
            checkBox22.Checked = SuperbadSettings.Instance.MightyBash;
            checkBox20.Checked = SuperbadSettings.Instance.UseVigil;
            numericUpDown19.Value = SuperbadSettings.Instance.CatAoe;
            checkBox40.Checked = SuperbadSettings.Instance.RakeCycle;
            checkBox43.Checked = SuperbadSettings.Instance.UseBerserk;
            checkBox41.Checked = SuperbadSettings.Instance.UseFff;
            checkBox42.Checked = SuperbadSettings.Instance.UseThrash;
            comboBox12.SelectedIndex = SuperbadSettings.Instance.ThrashUsage;
            checkBox44.Checked = SuperbadSettings.Instance.PullStealth;
            comboBox13.SelectedIndex = SuperbadSettings.Instance.StealthOpener;
            checkBox45.Checked = SuperbadSettings.Instance.UsePotion;
            checkBox50.Checked = SuperbadSettings.Instance.StayInStealth;
            checkBox51.Checked = SuperbadSettings.Instance.SavageFarm;
            checkBox23.Checked = SuperbadSettings.Instance.WaitSickness;
            checkBox24.Checked = SuperbadSettings.Instance.Update;
            checkBox56.Checked = SuperbadSettings.Instance.Changelog;
            numericUpDown20.Value = SuperbadSettings.Instance.BearAoe;
            checkBox49.Checked = SuperbadSettings.Instance.LacerateCycle;
            numericUpDown21.Value = SuperbadSettings.Instance.Frenzied;
            numericUpDown22.Value = SuperbadSettings.Instance.MightofUrsoc;
            checkBox46.Checked = SuperbadSettings.Instance.UseBerserkBear;
            checkBox48.Checked = SuperbadSettings.Instance.UseFffBear;
            checkBox47.Checked = SuperbadSettings.Instance.UseThrashBear ;
            checkBox52.Checked = SuperbadSettings.Instance.UseTaunt;
            checkBox53.Checked = SuperbadSettings.Instance.UseBearHug;
            checkBox54.Checked = SuperbadSettings.Instance.UseSavageDefense;
            checkBox55.Checked = SuperbadSettings.Instance.UseFrenziedRegen;
            checkBox57.Checked = SuperbadSettings.Instance.UseTauntBosses;
            numericUpDown5.Value = SuperbadSettings.Instance.BossImmerseus;
            numericUpDown24.Value = SuperbadSettings.Instance.BossAmalgam;
            numericUpDown25.Value = SuperbadSettings.Instance.BossSha;
            numericUpDown26.Value = SuperbadSettings.Instance.BossGalakras;
            numericUpDown27.Value = SuperbadSettings.Instance.BossIronJuggernaut;
            numericUpDown28.Value = SuperbadSettings.Instance.BossEarthBreakerHaromm;
            numericUpDown29.Value = SuperbadSettings.Instance.BossNazgrim;
            numericUpDown30.Value = SuperbadSettings.Instance.BossMalkorok;
            numericUpDown31.Value= SuperbadSettings.Instance.BossThok;
            numericUpDown32.Value = SuperbadSettings.Instance.BossBlackfuse;
            numericUpDown33.Value = SuperbadSettings.Instance.BossGarrosh;
        }


        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox8.SelectedIndex)
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

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.ShiftOnMobCount = checkBox14.Checked;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.AddsBearSwitch = (int) numericUpDown6.Value;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.ShiftOnLowHealth = checkBox15.Checked;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealthBearSwitch = (int)numericUpDown7.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseMovement = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTargeting = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFacing = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Suspend = checkBox4.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTrinket1 = checkBox6.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket1Usage = comboBox1.SelectedIndex;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket1Percent = (int) numericUpDown3.Value;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTrinket2 = checkBox7.Checked;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket2Usage = comboBox2.SelectedIndex;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Trinket2Percent = (int)numericUpDown4.Value;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseGloves = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseLifeBlood = checkBox9.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseRest = checkBox5.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RestHealth = (int) numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RestMana = (int)numericUpDown2.Value;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseRacial = checkBox10.Checked;
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.SymbTarget = checkBox29.Checked;
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.SymbSpell = checkBox30.Checked;
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseRebirth = checkBox31.Checked;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RebirthMode = comboBox9.SelectedIndex;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.NONE;
            if (comboBox4.SelectedIndex == 1)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox4.SelectedIndex == 2)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox4.SelectedIndex == 3)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox4.SelectedIndex == 4)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox4.SelectedIndex == 5)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.LALT;
            if (comboBox4.SelectedIndex == 6)
                SuperbadSettings.Instance.PauseKey = SuperbadSettings.Keypress.RALT;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 0)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.NONE;
            if (comboBox5.SelectedIndex == 1)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox5.SelectedIndex == 2)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox5.SelectedIndex == 3)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox5.SelectedIndex == 4)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox5.SelectedIndex == 5)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.LALT;
            if (comboBox5.SelectedIndex == 6)
                SuperbadSettings.Instance.BurstKey = SuperbadSettings.Keypress.RALT;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedIndex == 0)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.NONE;
            if (comboBox7.SelectedIndex == 1)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox7.SelectedIndex == 2)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox7.SelectedIndex == 3)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox7.SelectedIndex == 4)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox7.SelectedIndex == 5)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.LALT;
            if (comboBox7.SelectedIndex == 6)
                SuperbadSettings.Instance.AoeKey = SuperbadSettings.Keypress.RALT;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == 0)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.NONE;
            if (comboBox6.SelectedIndex == 1)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.LSHIFT;
            if (comboBox6.SelectedIndex == 2)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.RSHIFT;
            if (comboBox6.SelectedIndex == 3)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.LCTRL;
            if (comboBox6.SelectedIndex == 4)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.RCTRL;
            if (comboBox6.SelectedIndex == 5)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.LALT;
            if (comboBox6.SelectedIndex == 6)
                SuperbadSettings.Instance.GrowlKey = SuperbadSettings.Keypress.RALT;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.PrintMsg = checkBox13.Checked;
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurst = checkBox32.Checked;
        }

        private void checkBox33_CheckedChanged_1(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseAoeKey = checkBox33.Checked;
        }


        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseSkullBash = checkBox26.Checked;
        }


        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.InterruptAnyone = checkBox27.Checked;
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.InterruptRandomize = checkBox28.Checked;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.InterruptFailPercentage = (int) numericUpDown10.Value;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstTigers = checkedListBox1.GetItemCheckState(0) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstBerserk = checkedListBox1.GetItemCheckState(1) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstVigil = checkedListBox1.GetItemCheckState(2) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstIncar = checkedListBox1.GetItemCheckState(3) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstBerserking = checkedListBox1.GetItemCheckState(4) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstVirmens = checkedListBox1.GetItemCheckState(5) == CheckState.Checked;
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBurstFeralSpirit = checkedListBox2.GetItemCheckState(0) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstHotw = checkedListBox2.GetItemCheckState(1) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstGloves = checkedListBox2.GetItemCheckState(2) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstTrinket1 = checkedListBox2.GetItemCheckState(3) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstTrinket2 = checkedListBox2.GetItemCheckState(4) == CheckState.Checked;
            SuperbadSettings.Instance.UseBurstLifeBlood = checkedListBox2.GetItemCheckState(5) == CheckState.Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Mdw = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Mdwgroup = checkBox12.Checked;
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseDash = checkBox34.Checked;
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.DashUsage = comboBox10.SelectedIndex;
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseStampedingRoar = checkBox35.Checked;
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.StampedingRoarUsage = comboBox11.SelectedIndex;
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseAquatic = checkBox37.Checked;
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTravel = checkBox38.Checked;
        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Rooted = checkBox39.Checked;
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Barkskin = (int) numericUpDown11.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealthStone = (int)numericUpDown12.Value;
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.LifeSpirit = (int)numericUpDown13.Value;
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Survival = (int)numericUpDown16.Value;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealingTouchCombat = (int)numericUpDown15.Value;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RejuvenationCombat = (int)numericUpDown14.Value;
        }

        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.OoCHealingTouch = (int)numericUpDown18.Value;
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.OoCReju = (int)numericUpDown17.Value;
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseHotW = checkBox21.Checked;
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.WrathSpam = checkBox36.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBlink = checkBox16.Checked;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Renewal = (int) numericUpDown8.Value;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseInca = checkBox18.Checked;
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.HealOthers = checkBox25.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseWildCharge = checkBox17.Checked;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.CenarionWard = (int)numericUpDown9.Value;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFoN = checkBox19.Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.MightyBash = checkBox22.Checked;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseVigil = checkBox20.Checked;
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Predatory = (int) numericUpDown23.Value;
        }

        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.CatAoe = (int) numericUpDown19.Value;
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.RakeCycle = checkBox40.Checked;
        }

        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBerserk = checkBox43.Checked;
        }

        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFff = checkBox41.Checked;
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseThrash = checkBox42.Checked;
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.ThrashUsage = comboBox12.SelectedIndex;
        }

        private void checkBox44_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.PullStealth = checkBox44.Checked;
        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.StealthOpener = comboBox13.SelectedIndex;
        }

        private void checkBox45_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UsePotion = checkBox45.Checked;
        }

        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.StayInStealth = checkBox50.Checked;
        }

        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.SavageFarm = checkBox51.Checked;
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BearAoe = (int) numericUpDown20.Value;
        }

        private void checkBox49_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.LacerateCycle = checkBox49.Checked;
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Frenzied = (int)numericUpDown21.Value;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.MightofUrsoc = (int)numericUpDown22.Value;
        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBerserkBear = checkBox46.Checked;
        }

        private void checkBox48_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFffBear = checkBox48.Checked;
        }

        private void checkBox47_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseThrashBear = checkBox47.Checked;
        }

        private void checkBox52_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTaunt = checkBox52.Checked;
        }

        private void checkBox53_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseBearHug = checkBox53.Checked;
        }

        private void checkBox54_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseSavageDefense = checkBox54.Checked;
        }

        private void checkBox55_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseFrenziedRegen = checkBox55.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Save();
            SuperbadSettings.printSettings();
            Logging.Write("Config saved!");
            Close();
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.WaitSickness = checkBox23.Checked;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Update = checkBox24.Checked;
        }

        private void checkBox56_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.Changelog = checkBox56.Checked;
        }

        private void checkBox57_CheckedChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.UseTauntBosses = checkBox57.Checked;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossImmerseus = (int) numericUpDown5.Value;
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossAmalgam = (int) numericUpDown24.Value;
        }

        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossSha = (int) numericUpDown25.Value;
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossGalakras = (int) numericUpDown26.Value;
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossIronJuggernaut = (int) numericUpDown27.Value;
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossEarthBreakerHaromm = (int) numericUpDown28.Value;
        }

        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossNazgrim = (int) numericUpDown29.Value;
        }

        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossMalkorok = (int) numericUpDown30.Value;
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossThok = (int)numericUpDown31.Value;
        }

        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossBlackfuse = (int)numericUpDown32.Value;
        }

        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            SuperbadSettings.Instance.BossGarrosh = (int)numericUpDown33.Value;
        }
    }
}
