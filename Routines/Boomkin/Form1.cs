using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Boomkin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CRSettings.myPrefs.Load();

            checkBox1.Checked = CRSettings.myPrefs.Movement;
            checkBox2.Checked = CRSettings.myPrefs.Facing;
            checkBox3.Checked = CRSettings.myPrefs.Targeting;
            checkBox4.Checked = CRSettings.myPrefs.useLifeblood;

            comboBoxPauseKey.SelectedIndex = CRSettings.myPrefs.PauseKey;
            switch (CRSettings.myPrefs.PauseKey)
            {
                case 0:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.None;
                    break;
                case 1:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.LSHIFT;
                    break;
                case 2:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.RSHIFT;
                    break;
                case 3:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.LCTRL;
                    break;
                case 4:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.RCTRL;
                    break;
                case 5:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.LALT;
                    break;
                case 6:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.RALT;
                    break;
            }
            comboBoxAoePauseKey.SelectedIndex = CRSettings.myPrefs.AoePauseKey;
            switch (CRSettings.myPrefs.AoePauseKey)
            {
                case 0:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.None;
                    break;
                case 1:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.LSHIFT;
                    break;
                case 2:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.RSHIFT;
                    break;
                case 3:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.LCTRL;
                    break;
                case 4:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.RCTRL;
                    break;
                case 5:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.LALT;
                    break;
                case 6:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.RALT;
                    break;
            }

            comboBox7.SelectedIndex = CRSettings.myPrefs.jadepotion;
            comboBox6.SelectedIndex = CRSettings.myPrefs.intflask;
            comboBox5.SelectedIndex = CRSettings.myPrefs.useBerserking;
            comboBox4.SelectedIndex = CRSettings.myPrefs.engigloves;
            comboBox3.SelectedIndex = CRSettings.myPrefs.trinket2;
            comboBox2.SelectedIndex = CRSettings.myPrefs.trinket1;
            comboBox1.SelectedIndex = CRSettings.myPrefs.celestial;

            numericUpDown1.Value = new decimal(CRSettings.myPrefs.HealingTouch);
            numericUpDown2.Value = new decimal(CRSettings.myPrefs.Rejuvenation);
            numericUpDown3.Value = new decimal(CRSettings.myPrefs.BarskinPercent);
            numericUpDown4.Value = new decimal(CRSettings.myPrefs.CenarionWardPercent);
            numericUpDown5.Value = new decimal(CRSettings.myPrefs.Renewal);
            numericUpDown8.Value = new decimal(CRSettings.myPrefs.NaturesSwiftness);
            numericUpDown9.Value = new decimal(CRSettings.myPrefs.healthstonepercent);
            numericUpDown6.Value = new decimal(CRSettings.myPrefs.Eat);
            numericUpDown7.Value = new decimal(CRSettings.myPrefs.Drink);
            numericUpDown10.Value = new decimal(CRSettings.myPrefs.innervate);
            numericUpDown11.Value = new decimal(CRSettings.myPrefs.startAoe);
            numericUpDown12.Value = new decimal(CRSettings.myPrefs.typhoon);


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Movement = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Facing = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Targeting = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.useLifeblood = checkBox4.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Save();
            Close();
        }

        private void comboBoxPauseKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.PauseKey = comboBoxPauseKey.SelectedIndex;
            switch (comboBoxPauseKey.SelectedIndex)
            {
                case 0:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.None;
                    break;
                case 1:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.LSHIFT;
                    break;
                case 2:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.RSHIFT;
                    break;
                case 3:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.LCTRL;
                    break;
                case 4:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.RCTRL;
                    break;
                case 5:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.LALT;
                    break;
                case 6:
                    CRSettings.myPrefs.PauseKeys = CRSettings.Keypress.RALT;
                    break;
            }
        }

        private void comboBoxAoePauseKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.AoePauseKey = comboBoxAoePauseKey.SelectedIndex;
            switch (comboBoxAoePauseKey.SelectedIndex)
            {
                case 0:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.None;
                    break;
                case 1:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.LSHIFT;
                    break;
                case 2:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.RSHIFT;
                    break;
                case 3:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.LCTRL;
                    break;
                case 4:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.RCTRL;
                    break;
                case 5:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.LALT;
                    break;
                case 6:
                    CRSettings.myPrefs.AoePauseKeys = CRSettings.Keypress.RALT;
                    break;
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.jadepotion = comboBox7.SelectedIndex;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.intflask = comboBox6.SelectedIndex;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.useBerserking = comboBox5.SelectedIndex;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.engigloves = comboBox4.SelectedIndex;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.trinket2 = comboBox3.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.trinket1 = comboBox2.SelectedIndex;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.HealingTouch = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Rejuvenation = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.BarskinPercent = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.CenarionWardPercent = (int)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Renewal = (int)numericUpDown5.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.NaturesSwiftness = (int)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.healthstonepercent = (int)numericUpDown9.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Eat = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.Drink = (int)numericUpDown7.Value;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.innervate = (int)numericUpDown10.Value;
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.startAoe = (int)numericUpDown11.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.typhoon = (int)numericUpDown12.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CRSettings.myPrefs.celestial = comboBox1.SelectedIndex;
        }

         
    }
}