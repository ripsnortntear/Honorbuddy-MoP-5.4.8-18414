using System.Windows.Forms;
using PureRotation.Core;
using Styx;
using Styx.Common;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class HotkeySettings : Styx.Helpers.Settings
    {
        private static HotkeySettings _instance;

        public HotkeySettings()
            : base(SettingsPath + ".config")
        {
        }

        public static string SettingsPath
        {
            get
            {
                return string.Format("{0}\\Settings\\PureRotation\\HotkeySettings_{1}", Utilities.AssemblyDirectory,
                                     StyxWoW.Me.Name);
            }
        }

        public static HotkeySettings Instance
        {
            get { return _instance ?? (_instance = new HotkeySettings()); }
        }

        [Setting]
        [DefaultValue(Keys.D1)]
        public Keys PauseKeyChoice { get; set; }

        [Setting]
        [DefaultValue(ModifierKeys.Alt)]
        public ModifierKeys ModKeyChoice { get; set; }

        [Setting]
        [DefaultValue(Mode.Hotkey)]
        public Mode ModeChoice { get; set; }

        [Setting]
        [DefaultValue(Keys.Q)]
        public Keys CooldownKeyChoice { get; set; }

        [Setting]
        [DefaultValue(Keys.E)]
        public Keys SwitchKeyChoice { get; set; }

        [Setting]
        [DefaultValue(Keys.F)]
        public Keys SpecialKeyChoice { get; set; }

        [Setting]
        [DefaultValue(Keys.G)]
        public Keys RotationKeyChoice { get; set; }
    }
}