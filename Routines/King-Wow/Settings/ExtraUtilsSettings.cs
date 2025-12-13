using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    class ExtraUtilsSettings : Settings
    {
        public static ExtraUtilsSettings Instance = new ExtraUtilsSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/ExtraBuffUtilsSettings/ExtraBuffUtilsSettings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public ExtraUtilsSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/ExtraBuffUtilsSettings/ExtraBuffUtilsSettings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

        public enum FlaskType
        {
            Intellect,
            Spirit,
            Stamina,
            Strenght,
            Agility
        }

        [Setting]
        [DefaultValue(false)]
        [Category("ALCHEMY FLASK")]
        [DisplayName("Use Alchemy Flask")]
        [Description("Enable/disable use of alchemy flask")]
        public bool UseAlchemyFlask { get; set; }

        [Setting]
        [DefaultValue(FlaskType.Intellect)]
        [Category("ALCHEMY FLASK")]
        [DisplayName("Flask Type to use")]
        [Description("chose Flask type to use (try always to use best one)")]
        public FlaskType FlaskTypeToUse { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("HEALTHSTONE")]
        [DisplayName("Healthstone HP%")]
        [Description("Use healthstone if lower than HP%")]
        public float HealthsStoneHP { get; set; }

        public enum TrinketUseType
        {
            OnBoss,
            OnCrowdControlled,
            Manual,
            Always,
            At_HP,
            At_MANA
        }

        public enum GenericUseType
        {
            OnBoss,
            Manual,
            Always
        }

        [Setting]
        [DefaultValue(TrinketUseType.Manual)]
        [Category("TRINKET_1")]
        [DisplayName("Use Trinket_1")]
        [Description("Use trinket_1 on condition")]
        public TrinketUseType UseTrinket_1_OnCondition { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("TRINKET_1")]
        [DisplayName("HP%")]
        [Description("Use trinket_1 on condition At_HP if HP lower than this value")]
        public float Trinket_1_HP { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("SPIRIT")]
        [DisplayName("Life spirit HP%")]
        [Description("Use Life spirit if lower than HP%")]
        public float LifeSpiritHP { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("SPIRIT")]
        [DisplayName("Water spirit MANA%")]
        [Description("Use Water spirit if lower than MANA%")]
        public float WaterSpiritMANA { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("TRINKET_1")]
        [DisplayName("MANA%")]
        [Description("Use trinket_1 on condition At_MANA if MANA lower than this value")]
        public float Trinket_1_MANA { get; set; }

        [Setting]
        [DefaultValue(TrinketUseType.Manual)]
        [Category("TRINKET_2")]
        [DisplayName("Use Trinket_2")]
        [Description("Use trinket_2 on condition")]
        public TrinketUseType UseTrinket_2_OnCondition { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("TRINKET_2")]
        [DisplayName("HP%")]
        [Description("Use trinket_2 on condition At_HP if HP lower than this value")]
        public float Trinket_2_HP { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("TRINKET_2")]
        [DisplayName("MANA%")]
        [Description("Use trinket_2 on condition At_MANA if MANA lower than this value")]
        public float Trinket_2_MANA { get; set; }

        [Setting]
        [DefaultValue(Keyboardfunctions.Nothing)]
        [Category("KEYBINDS")]
        [DisplayName("AOE Healing")]
        [Description("Use this button to enable/disable AOE healing")]
        public Keyboardfunctions AOE_HealingButton { get; set; }

        [Setting]
        [DefaultValue(Keyboardfunctions.Nothing)]
        [Category("KEYBINDS")]
        [DisplayName("DISCIPLINE PRIEST Atonement/Normal healing switch")]
        [Description("Use this button to Switch Discipline priest between Normal and Atonement healing")]
        public Keyboardfunctions AtonementNormalHealingSwitchButton { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("SOUNDS")]
        [DisplayName("enabled ")]
        [Description("Enable/Disable CC sounds")]
        public bool SoundsEnabled { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("RACIAL")]
        [DisplayName("enabled ")]
        [Description("Enable/Disable Racial Ability")]
        public bool UseRacials { get; set; }

        [Setting]
        [DefaultValue(GenericUseType.Always)]
        [Category("PROFESSIONS")]
        [DisplayName("Use Engi. Gloves OnCondition")]
        [Description("Use Engi. Gloves OnCondition")]
        public GenericUseType UseGloves_OnCondition { get; set; }

        [Setting]
        [DefaultValue(GenericUseType.Always)]
        [Category("PROFESSIONS")]
        [DisplayName("Use Lifeblood OnCondition")]
        [Description("Use Lifeblood OnCondition")]
        public GenericUseType UseLifeblood_OnCondition { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("RACIAL")]
        [DisplayName("Gift of the Naaru HP%")]
        [Description("Cast Gift of the Naaru if your HP% lower than this value")]
        public int GigtOfTheNaaruHP { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("RACIAL")]
        [DisplayName("Arcane Torrent MANA%")]
        [Description("Cast Arcane Torrent if your MANA% lower than this value")]
        public int ArcaneTorrentMana { get; set; }
        
        
        

        public enum Keyboardfunctions
        {
            Nothing,                // - default
            IsAltKeyDown,           // - Returns whether an Alt key on the keyboard is held down.
            IsControlKeyDown,       // - Returns whether a Control key on the keyboard is held down
            IsLeftAltKeyDown,       // - Returns whether the left Alt key is currently held down
            IsLeftControlKeyDown,   // - Returns whether the left Control key is held down
            IsLeftShiftKeyDown,     // - Returns whether the left Shift key on the keyboard is held down
            IsModifierKeyDown,      // - Returns whether a modifier key is held down
            IsRightAltKeyDown,      // - Returns whether the right Alt key is currently held down
            IsRightControlKeyDown,  // - Returns whether the right Control key on the keyboard is held down
            IsRightShiftKeyDown,    // - Returns whether the right shift key on the keyboard is held down
            IsShiftKeyDown,         // - Returns whether a Shift key on the keyboard is held down
        }

        [Setting]
        [DefaultValue(false)]
        public bool DisableAOE_HealingRotation { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseDisciplineAtonementHealingRotation { get; set; }
    }
}
