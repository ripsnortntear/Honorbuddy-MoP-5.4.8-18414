using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.Common;

namespace PureRotation.Settings.GUI
{
    public partial class ConfigurationForm : Form
    {
        private Form _newdebugui;
        private Form _newhealdebugui;

        public ConfigurationForm()
        {
            InitializeComponent();
            Text = "PureRotation";
            string IconToLoad = "";
            try
            {
                IconToLoad = string.Format("{0}\\Routines\\PureRotation\\Settings\\GUI\\icon.ico", Utilities.AssemblyDirectory);
                if (File.Exists(IconToLoad))
                    Icon = new Icon(IconToLoad);
                IconToLoad = string.Format("{0}\\Routines\\PureRotation\\Settings\\GUI\\folder.ico", Utilities.AssemblyDirectory);
                if (File.Exists(IconToLoad))
                    advancedtoolStripMenuItem1.Image = new Bitmap(IconToLoad);
                IconToLoad = string.Format("{0}\\Routines\\PureRotation\\Settings\\GUI\\browse.ico", Utilities.AssemblyDirectory);
                if (File.Exists(IconToLoad))
                    browseToolStripMenuItem.Image = new Bitmap(IconToLoad);
                IconToLoad = string.Format("{0}\\Routines\\PureRotation\\Settings\\GUI\\save.ico", Utilities.AssemblyDirectory);
                if (File.Exists(IconToLoad))
                    saveToolStripMenuItem.Image = new Bitmap(IconToLoad);
                IconToLoad = string.Format("{0}\\Routines\\PureRotation\\Settings\\GUI\\close.ico", Utilities.AssemblyDirectory);
                if (File.Exists(IconToLoad))
                    closeToolStripMenuItem.Image = new Bitmap(IconToLoad);

                pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
                IconToLoad = string.Format("{0}\\Routines\\PureRotation\\Settings\\GUI\\logo.ico", Utilities.AssemblyDirectory);
                if (File.Exists(IconToLoad))
                    pictureBox1.ImageLocation = IconToLoad;
            }
            catch (Exception e)
            {
                Logging.WriteDiagnostic("Couldn't load one of the Files ({0})", IconToLoad);
                Logging.WriteDiagnostic("Thrown Exception: {0}", e.ToString());
            }

            //Mode
            modecbo.Items.Add(new CboItem((int)Mode.Auto, "Automatic Mode"));
            modecbo.Items.Add(new CboItem((int)Mode.SemiAuto, "Semi-Auto Mode"));
            modecbo.Items.Add(new CboItem((int)Mode.Hotkey, "Hotkey Mode"));

            //Mod Key

            altkeyCBO.Items.Add(new CboItem((int)Styx.Common.ModifierKeys.Alt, "Alt - Mod"));
            altkeyCBO.Items.Add(new CboItem((int)Styx.Common.ModifierKeys.Control, "Ctrl - Mod"));
            altkeyCBO.Items.Add(new CboItem((int)Styx.Common.ModifierKeys.Shift, "Shift - Mod"));
            altkeyCBO.Items.Add(new CboItem((int)Styx.Common.ModifierKeys.Win, "Win - Mod"));

            //Special Key
            specialcbo.Items.Add(new CboItem((int)Keys.XButton1, "Mouse button 4"));
            specialcbo.Items.Add(new CboItem((int)Keys.XButton2, "Mouse button 5"));
            specialcbo.Items.Add(new CboItem((int)Keys.D1, "1 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D2, "2 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D3, "3 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D4, "4 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D5, "5 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D6, "6 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D7, "7 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D8, "8 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D9, "9 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.D0, "0 (no numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad1, "1 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad2, "2 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad3, "3 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad4, "4 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad5, "5 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad6, "6 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad7, "7 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad8, "8 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad9, "9 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.NumPad0, "0 (numpad)"));
            specialcbo.Items.Add(new CboItem((int)Keys.F1, "F1"));
            specialcbo.Items.Add(new CboItem((int)Keys.F2, "F2"));
            specialcbo.Items.Add(new CboItem((int)Keys.F3, "F3"));
            specialcbo.Items.Add(new CboItem((int)Keys.F4, "F4"));
            specialcbo.Items.Add(new CboItem((int)Keys.F5, "F5"));
            specialcbo.Items.Add(new CboItem((int)Keys.F6, "F6"));
            specialcbo.Items.Add(new CboItem((int)Keys.F7, "F7"));
            specialcbo.Items.Add(new CboItem((int)Keys.F8, "F8"));
            specialcbo.Items.Add(new CboItem((int)Keys.F9, "F9"));
            specialcbo.Items.Add(new CboItem((int)Keys.F10, "F10"));
            specialcbo.Items.Add(new CboItem((int)Keys.F11, "F11"));
            specialcbo.Items.Add(new CboItem((int)Keys.F12, "F12"));
            specialcbo.Items.Add(new CboItem((int)Keys.Q, "Q"));
            specialcbo.Items.Add(new CboItem((int)Keys.E, "E"));
            specialcbo.Items.Add(new CboItem((int)Keys.P, "P"));
            specialcbo.Items.Add(new CboItem((int)Keys.F, "F"));
            specialcbo.Items.Add(new CboItem((int)Keys.R, "R"));
            specialcbo.Items.Add(new CboItem((int)Keys.None, "No Hotkey"));

            //Cooldown Key
            cooldowncbo.Items.Add(new CboItem((int)Keys.XButton1, "Mouse button 4"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.XButton2, "Mouse button 5"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D1, "1 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D2, "2 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D3, "3 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D4, "4 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D5, "5 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D6, "6 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D7, "7 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D8, "8 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D9, "9 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.D0, "0 (no numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad1, "1 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad2, "2 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad3, "3 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad4, "4 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad5, "5 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad6, "6 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad7, "7 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad8, "8 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad9, "9 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.NumPad0, "0 (numpad)"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F1, "F1"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F2, "F2"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F3, "F3"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F4, "F4"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F5, "F5"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F6, "F6"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F7, "F7"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F8, "F8"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F9, "F9"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F10, "F10"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F11, "F11"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F12, "F12"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.Q, "Q"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.E, "E"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.P, "P"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.F, "F"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.R, "R"));
            cooldowncbo.Items.Add(new CboItem((int)Keys.None, "No Hotkey"));

            //AoE/Single Switch Key
            switchcbo.Items.Add(new CboItem((int)Keys.XButton1, "Mouse button 4"));
            switchcbo.Items.Add(new CboItem((int)Keys.XButton2, "Mouse button 5"));
            switchcbo.Items.Add(new CboItem((int)Keys.D1, "1 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D2, "2 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D3, "3 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D4, "4 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D5, "5 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D6, "6 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D7, "7 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D8, "8 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D9, "9 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.D0, "0 (no numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad1, "1 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad2, "2 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad3, "3 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad4, "4 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad5, "5 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad6, "6 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad7, "7 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad8, "8 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad9, "9 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.NumPad0, "0 (numpad)"));
            switchcbo.Items.Add(new CboItem((int)Keys.F1, "F1"));
            switchcbo.Items.Add(new CboItem((int)Keys.F2, "F2"));
            switchcbo.Items.Add(new CboItem((int)Keys.F3, "F3"));
            switchcbo.Items.Add(new CboItem((int)Keys.F4, "F4"));
            switchcbo.Items.Add(new CboItem((int)Keys.F5, "F5"));
            switchcbo.Items.Add(new CboItem((int)Keys.F6, "F6"));
            switchcbo.Items.Add(new CboItem((int)Keys.F7, "F7"));
            switchcbo.Items.Add(new CboItem((int)Keys.F8, "F8"));
            switchcbo.Items.Add(new CboItem((int)Keys.F9, "F9"));
            switchcbo.Items.Add(new CboItem((int)Keys.F10, "F10"));
            switchcbo.Items.Add(new CboItem((int)Keys.F11, "F11"));
            switchcbo.Items.Add(new CboItem((int)Keys.F12, "F12"));
            switchcbo.Items.Add(new CboItem((int)Keys.Q, "Q"));
            switchcbo.Items.Add(new CboItem((int)Keys.E, "E"));
            switchcbo.Items.Add(new CboItem((int)Keys.P, "P"));
            switchcbo.Items.Add(new CboItem((int)Keys.F, "F"));
            switchcbo.Items.Add(new CboItem((int)Keys.R, "R"));
            switchcbo.Items.Add(new CboItem((int)Keys.None, "No Hotkey"));

            //Pause Key
            pausecbo.Items.Add(new CboItem((int)Keys.XButton1, "Mouse button 4"));
            pausecbo.Items.Add(new CboItem((int)Keys.XButton2, "Mouse button 5"));
            pausecbo.Items.Add(new CboItem((int)Keys.D1, "1 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D2, "2 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D3, "3 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D4, "4 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D5, "5 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D6, "6 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D7, "7 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D8, "8 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D9, "9 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.D0, "0 (no numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad1, "1 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad2, "2 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad3, "3 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad4, "4 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad5, "5 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad6, "6 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad7, "7 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad8, "8 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad9, "9 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.NumPad0, "0 (numpad)"));
            pausecbo.Items.Add(new CboItem((int)Keys.F1, "F1"));
            pausecbo.Items.Add(new CboItem((int)Keys.F2, "F2"));
            pausecbo.Items.Add(new CboItem((int)Keys.F3, "F3"));
            pausecbo.Items.Add(new CboItem((int)Keys.F4, "F4"));
            pausecbo.Items.Add(new CboItem((int)Keys.F5, "F5"));
            pausecbo.Items.Add(new CboItem((int)Keys.F6, "F6"));
            pausecbo.Items.Add(new CboItem((int)Keys.F7, "F7"));
            pausecbo.Items.Add(new CboItem((int)Keys.F8, "F8"));
            pausecbo.Items.Add(new CboItem((int)Keys.F9, "F9"));
            pausecbo.Items.Add(new CboItem((int)Keys.F10, "F10"));
            pausecbo.Items.Add(new CboItem((int)Keys.F11, "F11"));
            pausecbo.Items.Add(new CboItem((int)Keys.F12, "F12"));
            pausecbo.Items.Add(new CboItem((int)Keys.Q, "Q"));
            pausecbo.Items.Add(new CboItem((int)Keys.E, "E"));
            pausecbo.Items.Add(new CboItem((int)Keys.P, "P"));
            pausecbo.Items.Add(new CboItem((int)Keys.F, "F"));
            pausecbo.Items.Add(new CboItem((int)Keys.R, "R"));
            pausecbo.Items.Add(new CboItem((int)Keys.None, "No Hotkey"));

            //Rotation Key
            rotationCBO.Items.Add(new CboItem((int)Keys.XButton1, "Mouse button 4"));
            rotationCBO.Items.Add(new CboItem((int)Keys.XButton2, "Mouse button 5"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D1, "1 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D2, "2 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D3, "3 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D4, "4 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D5, "5 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D6, "6 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D7, "7 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D8, "8 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D9, "9 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.D0, "0 (no numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad1, "1 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad2, "2 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad3, "3 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad4, "4 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad5, "5 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad6, "6 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad7, "7 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad8, "8 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad9, "9 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.NumPad0, "0 (numpad)"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F1, "F1"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F2, "F2"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F3, "F3"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F4, "F4"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F5, "F5"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F6, "F6"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F7, "F7"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F8, "F8"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F9, "F9"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F10, "F10"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F11, "F11"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F12, "F12"));
            rotationCBO.Items.Add(new CboItem((int)Keys.Q, "Q"));
            rotationCBO.Items.Add(new CboItem((int)Keys.E, "E"));
            rotationCBO.Items.Add(new CboItem((int)Keys.P, "P"));
            rotationCBO.Items.Add(new CboItem((int)Keys.F, "F"));
            rotationCBO.Items.Add(new CboItem((int)Keys.R, "R"));
            rotationCBO.Items.Add(new CboItem((int)Keys.G, "G"));
            rotationCBO.Items.Add(new CboItem((int)Keys.None, "No Hotkey"));

            SetComboBoxEnum(modecbo, (int)HotkeySettings.Instance.ModeChoice);
            SetComboBoxEnum(altkeyCBO, (int)HotkeySettings.Instance.ModKeyChoice);
            SetComboBoxEnum(pausecbo, (int)HotkeySettings.Instance.PauseKeyChoice);
            SetComboBoxEnum(cooldowncbo, (int)HotkeySettings.Instance.CooldownKeyChoice);
            SetComboBoxEnum(switchcbo, (int)HotkeySettings.Instance.SwitchKeyChoice);
            SetComboBoxEnum(specialcbo, (int)HotkeySettings.Instance.SpecialKeyChoice);
            SetComboBoxEnum(rotationCBO, (int)HotkeySettings.Instance.RotationKeyChoice);
        }

        private static void SetComboBoxEnum(ComboBox cb, int e)
        {
            CboItem item;
            for (var i = 0; i < cb.Items.Count; i++)
            {
                item = (CboItem)cb.Items[i];
                if (item.E != e) continue;
                cb.SelectedIndex = i;
                return;
            }
            item = (CboItem)cb.Items[0];
            Logging.Write("Dialog Error: combobox {0} does not have enum({1}) in list, defaulting to enum({2})",
                          cb.Name, e, item.E);
            cb.SelectedIndex = 0;
        }

        private static int GetComboBoxEnum(ComboBox cb)
        {
            var item = (CboItem)cb.Items[cb.SelectedIndex];
            return item.E;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void CloseToolStripMenuItemClick(object sender, EventArgs e)
        {
            Dispose(true);
            Close();
        }

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            HotkeySettings.Instance.SwitchKeyChoice = (Keys)GetComboBoxEnum(switchcbo);
            HotkeySettings.Instance.PauseKeyChoice = (Keys)GetComboBoxEnum(pausecbo);
            HotkeySettings.Instance.CooldownKeyChoice = (Keys)GetComboBoxEnum(cooldowncbo);
            HotkeySettings.Instance.ModeChoice = (Mode)GetComboBoxEnum(modecbo);
            HotkeySettings.Instance.SpecialKeyChoice = (Keys)GetComboBoxEnum(specialcbo);
            HotkeySettings.Instance.RotationKeyChoice = (Keys)GetComboBoxEnum(rotationCBO);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
            PRSettings.Instance.Save();
            HotkeySettings.Instance.Save();
            MessageBox.Show("Honorbuddy has successfully saved your settings.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ((Styx.Helpers.Settings)pgMain.SelectedObject).Save();
            if (pgClass.SelectedObject != null)
            {
                ((Styx.Helpers.Settings)pgClass.SelectedObject).Save();
            }
            PRSettings.Instance.LogSettings();
            Close();
        }

        private void ConfigurationFormLoad(object sender, EventArgs e)
        {
            // setup the bindings
            pgMain.SelectedObject = PRSettings.Instance;
            PRSettings main = PRSettings.Instance;
            Styx.Helpers.Settings toSelect = null;
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Warrior:
                    toSelect = main.Warrior;
                    break;

                case WoWClass.Paladin:
                    toSelect = main.Paladin;
                    break;

                case WoWClass.Hunter:
                    toSelect = main.Hunter;
                    break;

                case WoWClass.Rogue:
                    toSelect = main.Rogue;
                    break;

                case WoWClass.Priest:
                    toSelect = main.Priest;
                    break;

                case WoWClass.DeathKnight:
                    toSelect = main.DeathKnight;
                    break;

                case WoWClass.Shaman:
                    toSelect = main.Shaman;
                    break;

                case WoWClass.Mage:
                    toSelect = main.Mage;
                    break;

                case WoWClass.Warlock:
                    toSelect = main.Warlock;
                    break;

                case WoWClass.Druid:
                    toSelect = main.Druid;
                    break;

                case WoWClass.Monk:
                    toSelect = main.Monk;
                    break;
            }

            if (toSelect != null)
            {
                pgClass.SelectedObject = toSelect;
            }
            InitializePlainStyle();
        }

        private void InitializePlainStyle()
        {
            pgMain.LineColor = GetClassColor(); // Category
            pgMain.CategoryForeColor = GetClassForeColor();

            pgClass.LineColor = GetClassColor(); // Category
            pgClass.CategoryForeColor = GetClassForeColor();

            panel4.BackColor = GetClassColor();

            // hide the toolbar
            pgMain.ToolbarVisible = false;
            pgClass.ToolbarVisible = false;

            // Collapse All Grid Items
            pgMain.CollapseAllGridItems();
            pgClass.CollapseAllGridItems();
        }

        private Color GetClassForeColor()
        {
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Rogue:
                case WoWClass.Priest:
                case WoWClass.Paladin:
                case WoWClass.Mage:
                case WoWClass.Hunter:
                case WoWClass.Druid:
                case WoWClass.DeathKnight:
                    return Color.FromArgb(0, 0, 0);
                default:
                    return Color.FromArgb(255, 255, 255);
            }
        }

        private Color GetClassColor()
        {
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.DeathKnight:
                    return Color.FromArgb(196, 30, 59);
                case WoWClass.Druid:
                    return Color.FromArgb(255, 124, 10);
                case WoWClass.Hunter:
                    return Color.FromArgb(170, 211, 114);
                case WoWClass.Mage:
                    return Color.FromArgb(104, 204, 239);
                case WoWClass.Monk:
                    return Color.FromArgb(0, 132, 93);
                case WoWClass.Paladin:
                    return Color.FromArgb(244, 140, 186);
                case WoWClass.Priest:
                    return Color.FromArgb(255, 255, 255);
                case WoWClass.Rogue:
                    return Color.FromArgb(255, 244, 104);
                case WoWClass.Shaman:
                    return Color.FromArgb(35, 89, 221);
                case WoWClass.Warlock:
                    return Color.FromArgb(147, 130, 170);
                case WoWClass.Warrior:
                    return Color.FromArgb(199, 156, 87);
                default:
                    return Color.FromArgb(47, 47, 47);
            }
        }

        private void AssemblieToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                Styx.CommonBot.Routines.RoutineManager.ReloadRoutineAssemblies();
                MessageBox.Show("Honorbuddy has successfully reloaded your Assemblies.", "Assembly", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception xException)
            {
                Logging.WriteDiagnostic("AssemblieToolStripMenuItemClick Thrown Exception: {0}", xException.ToString());
                throw;
            }
        }

        private void RoutineManagerToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                Styx.CommonBot.Routines.RoutineManager.Reload();
                MessageBox.Show("Honorbuddy has sucessfully reloaded the RoutineManager.", "RoutineManager", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception xException)
            {
                Logging.WriteDiagnostic("RoutineManagerToolStripMenuItemClick Thrown Exception: {0}", xException.ToString());
                throw;
            }
        }

        private void AlwyasOnTopToolStripMenuItemClick(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            alwyasOnTopToolStripMenuItem.Checked = TopMost;
            MessageBox.Show("Configuration Window set to TopMost. WoW must be running in Windowed mode.", "TopMost", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ThreadToolStripMenuItemClick(object sender, EventArgs e)
        {
            Process.Start("http://www.thebuddyforum.com/forum.php");
        }

        private void ReloadSettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            PRSettings.Instance.Load();
        }

        private void ResetHotkeysToolStripMenuItemClick(object sender, EventArgs e)
        {
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void DebugToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_newdebugui == null || _newdebugui.IsDisposed || _newdebugui.Disposing) _newdebugui = new Debug();
            if (_newdebugui != null || _newdebugui.IsDisposed) _newdebugui.ShowDialog();
        }

        private void modecbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Mode) GetComboBoxEnum(modecbo) == Mode.Auto)
            {
                lblAoE.Visible = false;
                lblCD.Visible = false;
                lblMod.Visible = false;
                lblPause.Visible = false;
                lblRotation.Visible = false;
                lblSpecial.Visible = false;

                cooldowncbo.Visible = false;
                switchcbo.Visible = false; // AoE key
                pausecbo.Visible = false;
                specialcbo.Visible = false;
                altkeyCBO.Visible = false; // Mod key
                rotationCBO.Visible = false;
            }

            if ((Mode) GetComboBoxEnum(modecbo) == Mode.SemiAuto)
            {

                lblAoE.Visible = false;
                lblCD.Visible = true;
                lblMod.Visible = true;
                lblPause.Visible = true;
                lblRotation.Visible = true;
                lblSpecial.Visible = true;

                switchcbo.Visible = false; // AoE key
                cooldowncbo.Visible = true;
                pausecbo.Visible = true;
                specialcbo.Visible = true;
                altkeyCBO.Visible = true; // Mod key
                rotationCBO.Visible = true;

            }

            if ((Mode) GetComboBoxEnum(modecbo) == Mode.Hotkey)
            {
                lblAoE.Visible = true;
                lblCD.Visible = true;
                lblMod.Visible = true;
                lblPause.Visible = true;
                lblRotation.Visible = true;
                lblSpecial.Visible = true;

                switchcbo.Visible = true; // AoE key
                cooldowncbo.Visible = true;
                pausecbo.Visible = true;
                specialcbo.Visible = true;
                altkeyCBO.Visible = true; // Mod key
                rotationCBO.Visible = true;
            }

            HotkeySettings.Instance.ModeChoice = (Mode)GetComboBoxEnum(modecbo);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void cooldowncbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HotkeySettings.Instance.CooldownKeyChoice = (Keys)GetComboBoxEnum(cooldowncbo);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void switchcbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HotkeySettings.Instance.SwitchKeyChoice = (Keys)GetComboBoxEnum(switchcbo);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void pausecbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HotkeySettings.Instance.PauseKeyChoice = (Keys)GetComboBoxEnum(pausecbo);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void specialcbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HotkeySettings.Instance.SpecialKeyChoice = (Keys)GetComboBoxEnum(specialcbo);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void altkeyCBO_SelectedIndexChanged(object sender, EventArgs e)
        {
            HotkeySettings.Instance.ModKeyChoice = (ModifierKeys)GetComboBoxEnum(altkeyCBO);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void rotationCBO_SelectedIndexChanged(object sender, EventArgs e)
        {
            HotkeySettings.Instance.RotationKeyChoice = (Keys)GetComboBoxEnum(rotationCBO);
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
        }

        private void healingDebugerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_newhealdebugui == null || _newhealdebugui.IsDisposed || _newhealdebugui.Disposing) _newhealdebugui = new HealingDebug();
            if (_newhealdebugui != null || _newhealdebugui.IsDisposed) _newhealdebugui.ShowDialog();
        }

    }
}