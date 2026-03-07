using System.Windows.Forms;
using FuryUnleashed.Core.Helpers;
using Styx.Common;
using Styx.Helpers;
using System.ComponentModel;

namespace FuryUnleashed.Interfaces.Settings
{
    internal class SettingsH : Styx.Helpers.Settings
    {
        public static SettingsH Instance = new SettingsH();

        public SettingsH()
            : base(InternalSettings.SettingsPath + "_Hotkey.xml")
        {
        }

        // ========================================================================================

        #region Hotkeys
        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Casting Type Hotkeys")]
        [DisplayName("Demoralizing Banner Key")]
        [Description("Select the preferred hotkey for Demoralizing Banner (Will cast on your mouse location).")]
        public Keys DemoBannerChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Casting Type Hotkeys")]
        [DisplayName("Heroic Leap Key")]
        [Description("Select the preferred hotkey for Heroic Leap (Will cast on your mouse location).")]
        public Keys HeroicLeapChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Casting Type Hotkeys")]
        [DisplayName("Mocking Banner Key")]
        [Description("Select the preferred hotkey for Mocking Banner (Will cast on your mouse location).")]
        public Keys MockingBannerChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Casting Type Hotkeys")]
        [DisplayName("Shattering Throw Key")]
        [Description("Select the preferred hotkey for Shattering Throw (Casts on keypress).")]
        public Keys ShatteringThrowChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Casting Type Hotkeys")]
        [DisplayName("Tier 4 Ability Key")]
        [Description("Select the preferred hotkey for your Tier 4 abilities (Casts on keypress).")]
        public Keys Tier4Choice { get; set; }

        // ========================================================================================
        
        [Setting]
        [Styx.Helpers.DefaultValue(Enum.Mode.Auto)]
        [Category("Toggle Type Hotkeys")]
        [DisplayName("Mode Selector")]
        [Description("Select the hotkey mode: Auto, Semi Hotkey (Only Pause & Cooldowns) or Hotkey (All hotkeys).")]
        public Enum.Mode ModeSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(ModifierKeys.Alt)]
        [Category("Toggle Type Hotkeys")]
        [DisplayName("Modifier Key")]
        [Description("Select the preferred modifier key for the hotkeys.")]
        public ModifierKeys ModKeyChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Toggle Type Hotkeys")]
        [DisplayName("Pause Key")]
        [Description("Select the preferred key to toggle pause the routine.")]
        public Keys PauseKeyChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Toggle Type Hotkeys")]
        [DisplayName("Cooldown Key")]
        [Description("Select the preferred key to toggle cooldowns.")]
        public Keys CooldownKeyChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Toggle Type Hotkeys")]
        [DisplayName("Multi-Target Key (AoE)")]
        [Description("Select the preferred key to toggle AoE (Multi-Target).")]
        public Keys MultiTgtKeyChoice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.None)]
        [Category("Toggle Type Hotkeys")]
        [DisplayName("Special Key")]
        [Description("Arms: None - Fury: None - Prot: None.")]
        public Keys SpecialKeyChoice { get; set; }
        #endregion

        // ========================================================================================
    }
}
