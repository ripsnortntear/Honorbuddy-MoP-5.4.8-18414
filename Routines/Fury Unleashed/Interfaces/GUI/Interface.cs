using FuryUnleashed.Core.Managers;
using FuryUnleashed.Core.Utilities;
using FuryUnleashed.Interfaces.Settings;
using Styx;
using Styx.Common;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Enum = FuryUnleashed.Core.Helpers.Enum;

namespace FuryUnleashed.Interfaces.GUI
{
    public partial class Interface : Form
    {
        /* Form Dragging Support */
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern bool ReleaseCapture();

        private void Fu_GuiDragDrop(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    ReleaseCapture();
                    SendMessage(Handle, 0xa1, 0x2, 0);
                    break;
            }
        }

        /* Requirements for Combobox Enumerations */
        private static void SetComboBoxEnum(ComboBox cb, int e)
        {
            Enum.FuCboItem item;
            for (var i = 0; i < cb.Items.Count; i++)
            {
                item = (Enum.FuCboItem)cb.Items[i];
                if (item.E != e) continue;
                cb.SelectedIndex = i;
                return;
            }
            item = (Enum.FuCboItem)cb.Items[0];
            Logger.DiagLogPu("Dialog Error: Combobox {0} does not have enum({1}) in list, defaulting to enum({2})",
                          cb.Name, e, item.E);
            cb.SelectedIndex = 0;
        }

        private static int GetComboBoxEnum(ComboBox cb)
        {
            var item = (Enum.FuCboItem)cb.Items[cb.SelectedIndex];
            return item.E;
        }

        public Interface()
        {
            InitializeComponent();
            InternalSettings.Instance.Load();
            SettingsH.Instance.Load();

            // Key library: http://msdn.microsoft.com/en-us/library/aa245676(v=vs.60).aspx
            // Hotkey - Demoralizing Banner
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.LControlKey, "Left Control"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.RControlKey, "Right Control"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.LShiftKey, "Left Shift"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.RShiftKey, "Right Shift"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkDemoralizingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));

            // Hotkey - Heroic Leap
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.LControlKey, "Left Control"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.RControlKey, "Right Control"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.LShiftKey, "Left Shift"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.RShiftKey, "Right Shift"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkHeroicLeap.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));

            // Hotkey - Mocking Banner
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.LControlKey, "Left Control"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.RControlKey, "Right Control"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.LShiftKey, "Left Shift"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.RShiftKey, "Right Shift"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkMockingBanner.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));

            // Hotkey - Shattering Throw
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.LControlKey, "Left Control"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.RControlKey, "Right Control"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.LShiftKey, "Left Shift"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.RShiftKey, "Right Shift"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkShatteringThrow.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));

            // Hotkey - Tier 4 Ability
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.LControlKey, "Left Control"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.RControlKey, "Right Control"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.LShiftKey, "Left Shift"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.RShiftKey, "Right Shift"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkTier4Ability.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));

            // Hotkey Mode Choice
            ComboHkMode.Items.Add(new Enum.FuCboItem((int)Enum.Mode.Auto, "Automatic Mode"));
            ComboHkMode.Items.Add(new Enum.FuCboItem((int)Enum.Mode.Hotkey, "Hotkey Mode"));
            ComboHkMode.Items.Add(new Enum.FuCboItem((int)Enum.Mode.SemiHotkey, "Semi Mode"));

            // Hotkey Modifier Key
            ComboHkModifier.Items.Add(new Enum.FuCboItem((int)Styx.Common.ModifierKeys.Alt, "Alt - Mod"));
            ComboHkModifier.Items.Add(new Enum.FuCboItem((int)Styx.Common.ModifierKeys.Control, "Ctrl - Mod"));
            ComboHkModifier.Items.Add(new Enum.FuCboItem((int)Styx.Common.ModifierKeys.Shift, "Shift - Mod"));
            ComboHkModifier.Items.Add(new Enum.FuCboItem((int)Styx.Common.ModifierKeys.Win, "Win - Mod"));

            // Hotkey - Cooldowns
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.XButton1, "Mouse button 4"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.XButton2, "Mouse button 5"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.P, "P"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.F, "F"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.R, "R"));
            ComboHkCooldown.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));

            // Hotkey - AoE
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.XButton1, "Mouse button 4"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.XButton2, "Mouse button 5"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.P, "P"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.F, "F"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.R, "R"));
            ComboHkMultiTgt.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));

            // Hotkey - Pause
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.XButton1, "Mouse button 4"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.XButton2, "Mouse button 5"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.P, "P"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.F, "F"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.R, "R"));
            ComboHkPause.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));

            // Hotkey - Special
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.XButton1, "Mouse button 4"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.XButton2, "Mouse button 5"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D1, "1 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D2, "2 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D3, "3 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D4, "4 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D5, "5 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D6, "6 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D7, "7 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D8, "8 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D9, "9 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.D0, "0 (no numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad1, "1 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad2, "2 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad3, "3 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad4, "4 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad5, "5 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad6, "6 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad7, "7 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad8, "8 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad9, "9 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.NumPad0, "0 (numpad)"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F1, "F1"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F2, "F2"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F3, "F3"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F4, "F4"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F5, "F5"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F6, "F6"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F7, "F7"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F8, "F8"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F9, "F9"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F10, "F10"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F11, "F11"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F12, "F12"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.Q, "Q"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.E, "E"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.P, "P"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.F, "F"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.R, "R"));
            ComboHkSpecial.Items.Add(new Enum.FuCboItem((int)Keys.None, "No Hotkey"));

            SetComboBoxEnum(ComboHkDemoralizingBanner, (int)SettingsH.Instance.DemoBannerChoice);
            SetComboBoxEnum(ComboHkHeroicLeap, (int)SettingsH.Instance.HeroicLeapChoice);
            SetComboBoxEnum(ComboHkMockingBanner, (int)SettingsH.Instance.MockingBannerChoice);
            SetComboBoxEnum(ComboHkShatteringThrow, (int)SettingsH.Instance.ShatteringThrowChoice);
            SetComboBoxEnum(ComboHkTier4Ability, (int)SettingsH.Instance.Tier4Choice);

            SetComboBoxEnum(ComboHkMode, (int)SettingsH.Instance.ModeSelection);
            SetComboBoxEnum(ComboHkModifier, (int)SettingsH.Instance.ModKeyChoice);
            SetComboBoxEnum(ComboHkCooldown, (int)SettingsH.Instance.CooldownKeyChoice);
            SetComboBoxEnum(ComboHkMultiTgt, (int)SettingsH.Instance.MultiTgtKeyChoice);
            SetComboBoxEnum(ComboHkPause, (int)SettingsH.Instance.PauseKeyChoice);
            SetComboBoxEnum(ComboHkSpecial, (int)SettingsH.Instance.SpecialKeyChoice);

            if (File.Exists(Utilities.AssemblyDirectory + @"\Routines\Fury Unleashed\Interfaces\Images\RoutineLogo.png"))
                LogoPicture.ImageLocation =
                    string.Format(@"{0}\Routines\Fury Unleashed\Interfaces\Images\RoutineLogo.png",
                                  Utilities.AssemblyDirectory);
        }

        private void FuInterface_Load(object sender, EventArgs e)
        {
            GeneralGrid.SelectedObject = InternalSettings.Instance.General;
            InternalSettings internalSettings = InternalSettings.Instance;
            Styx.Helpers.Settings selectSpec = null;
            switch (StyxWoW.Me.Specialization)
            {
                case WoWSpec.WarriorArms:
                    if (InternalSettings.Instance.General.CrArmsRotVersion == Enum.ArmsRotationVersion.PvP)
                    { selectSpec = internalSettings.PvPArms; }
                    else
                    { selectSpec = internalSettings.Arms; }
                    break;

                case WoWSpec.WarriorFury:
                    //if (InternalSettings.Instance.General.CrFuryRotVersion == Enum.RotationVersion.PvP)
                    //{ selectSpec = internalSettings.PvPFury; }
                    //else
                    { selectSpec = internalSettings.Fury; }
                    break;

                case WoWSpec.WarriorProtection:
                    //if (InternalSettings.Instance.General.CrProtRotVersion == Enum.RotationVersion.PvP)
                    //{ selectSpec = internalSettings.PvPProtection; }
                    //else
                    { selectSpec = internalSettings.Protection; }
                    break;
            }

            if (selectSpec != null)
            {
                SpecGrid.SelectedObject = selectSpec;
            }

            int splitterPositionG = GeneralGrid.GetInternalLabelWidth();
            GeneralGrid.MoveSplitterTo(splitterPositionG + 125);
            int splitterPositionS = SpecGrid.GetInternalLabelWidth();
            SpecGrid.MoveSplitterTo(splitterPositionS + 125);
        }

        #region Hotkeys
        private void ComboHkDemoralizingBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.DemoBannerChoice = (Keys)GetComboBoxEnum(ComboHkDemoralizingBanner);
        }
        private void ComboHkHeroicLeap_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.HeroicLeapChoice = (Keys)GetComboBoxEnum(ComboHkHeroicLeap);
        }
        private void ComboHkMockingBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.MockingBannerChoice = (Keys)GetComboBoxEnum(ComboHkMockingBanner);
        }
        private void ComboHkShatteringThrow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.ShatteringThrowChoice = (Keys)GetComboBoxEnum(ComboHkShatteringThrow);
        }
        private void ComboHkTier4Ability_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.Tier4Choice = (Keys)GetComboBoxEnum(ComboHkTier4Ability);
        }

        private void ComboHkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.ModeSelection = (Enum.Mode)GetComboBoxEnum(ComboHkMode);
        }
        private void ComboHkModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.ModKeyChoice = (ModifierKeys)GetComboBoxEnum(ComboHkModifier);
        }
        private void ComboHkCooldown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.CooldownKeyChoice = (Keys)GetComboBoxEnum(ComboHkCooldown);
        }
        private void ComboHkMultiTgt_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.MultiTgtKeyChoice = (Keys)GetComboBoxEnum(ComboHkMultiTgt);
        }
        private void ComboHkPause_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.PauseKeyChoice = (Keys)GetComboBoxEnum(ComboHkPause);
        }
        private void ComboHkSpecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsH.Instance.SpecialKeyChoice = (Keys)GetComboBoxEnum(ComboHkSpecial);
        }
        #endregion

        #region Buttons
        private void debuggerpanel_Click(object sender, EventArgs e)
        {
            new DebuggerGui().Show();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            InternalSettings.Instance.Save();
            SettingsH.Instance.Save();
            ((Styx.Helpers.Settings)GeneralGrid.SelectedObject).Save();
            if (SpecGrid.SelectedObject != null)
            {
                ((Styx.Helpers.Settings)SpecGrid.SelectedObject).Save();
            }
            Logger.CombatLogOr("Settings for Fury Unleashed saved!");
            if (InternalSettings.Instance.General.InterruptMode == Enum.Interrupts.Constant)
            {
                MessageBox.Show("You have selected Constant as Interrupt mode.\r\n Due to the increased banrisk, I HIGHLY recommend using Random instead!");
            }
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
            HotKeyManager.HotkeyTimer(500);
            Logger.LogTimer(2000);
            Close();
        }
        #endregion

        #region MouseMoves & MouseLeaves
        private void Fu_MouseLeave(object sender, EventArgs e)
        {
            StatusStripText.Text = 
                "Fury Unleashed - The best you can get!";
        }

        private void LogoPicture_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Click and hold to drag the Fury Unleashed User Interface.";
        }

        private void SaveButton_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Click this button to save the settings and close the GUI.";
        }

        private void ComboHkDemoralizingBanner_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred hotkey for Demoralizing Banner (Will cast on your mouse location) - Casting Type.";
        }

        private void ComboHkHeroicLeap_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred hotkey for Heroic Leap (Will cast on your mouse location) - Casting Type.";
        }

        private void ComboHkMockingBanner_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred hotkey for Mocking Banner (Will cast on your mouse location) - Casting Type.";
        }

        private void ComboHkShatteringThrow_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred hotkey for Shattering Throw - Casting Type.";
        }

        private void ComboHkTier4Ability_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred hotkey for your Tier 4 abilities (Casts on keypress) - Casting Type.";
        }

        private void ComboHkMode_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the hotkey mode: Auto, Semi Hotkey (Only Pause & Cooldowns) or Hotkey (All hotkeys).";
        }

        private void ComboHkModifier_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred modifier key for the hotkeys - Toggle Type.";
        }

        private void ComboHkCooldown_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred key to toggle cooldowns - Toggle Type.";
        }

        private void ComboHkMultiTgt_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred key to toggle AoE (Multi-Target) - Toggle Type.";
        }

        private void ComboHkPause_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred key to toggle pause - Toggle Type.";
        }

        private void ComboHkSpecial_MouseMove(object sender, MouseEventArgs e)
        {
            StatusStripText.Text =
                "Select the preferred key to toggle special abilities - Toggle Type - Arms: None - Fury: None - Prot: Switch Shield Block or Barrier.";
        }
        #endregion
    }
}
