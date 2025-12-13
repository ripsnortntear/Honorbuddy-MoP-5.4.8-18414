using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class HolyPriestSettings : Settings
    {
        public static HolyPriestSettings Instance = new HolyPriestSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/HolyPriest/HolyPriest-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public HolyPriestSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/HolyPriest/HolyPriest-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

        [Setting]
        [DefaultValue(28)]
        [Category("Settings PULL")]
        [DisplayName("PullDistance")]
        [Description("Distance to pull from")]
        public float PullDistance { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings SELF HEALING")]
        [DisplayName("Enabled Self Healing Priority")]
        [Description("Enable/Disable Self Healing Priority: HP% is set below")]
        public bool SelfHealingPriorityEnabled { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings SELF HEALING")]
        [DisplayName("SelfHealingPriorityHP")]
        [Description("if enabled Self Healing Priority: healing you before other in party until below this HP%")]
        public float SelfHealingPriorityHP { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings ROTATION")]
        [DisplayName("Rotation Type")]
        [Description("Use 0 for NORMAL ROTATION, Use 1 For First Tank and SINGLE TARGET ROTATION, Use 2 for AOE PRIORITY ROTATION, use 3 for PVP ROTATION")]
        public int RotationType { get; set; }

        public enum InnerType
        {
            FIRE,
            WILL
        }

        [Setting]
        [DefaultValue(InnerType.FIRE)]
        [Category("Settings BUFF")]
        [DisplayName("Use Inner")]
        [Description("chose inner to use")]
        public InnerType InnerToUse { get; set; }

        public enum ChakraType
        {
            CHASTISE,
            SANCTUARY,
            SERENITY
        }

        [Setting]
        [DefaultValue(ChakraType.SANCTUARY)]
        [Category("Settings BUFF")]
        [DisplayName("Use Chakra")]
        [Description("chose Chakra to use")]
        public ChakraType ChakraToUse { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Auto PW:Fortitude")]
        [Description("Cast Automaticaly PW:Fortitude on party")]
        public bool AutoPWFortitude { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Use Inner will moving")]
        [Description("chose to use inner will if moving")]
        public bool InnerWillOnMoving { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Use Angelic Feather moving")]
        [Description("chose to use Angelic Faether if talented and if moving")]
        public bool AngelicFeatherOnMoving { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Use AutoCastLightwell")]
        [Description("AutoCastLightwell in your position in combat")]
        public bool AutoCastLightwell { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings DISPELLS")]
        [DisplayName("Use Purify ")]
        [Description("Use Purify for cure disease and dispell magic")]
        public bool UsePurify { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings DISPELLS")]
        [DisplayName("Mass Dispell Count")]
        [Description("Set number of dispellable players for Mass Dispell")]
        public int MassDispellCount { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use Resurrection")]
        [Description("enable use of Resurrection out of combat")]
        public bool UseResurrection { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("use OOC healing")]
        [Description("enable-disable use of OutOfCombat healing")]
        public bool OOCHealing { get; set; }

        [Setting]
        [DefaultValue(15)]
        [Category("Settings SPECIAL")]
        [DisplayName("Guardian Spirit hp%")]
        [Description("Cast Guardian Spirit at target % hp")]
        public int GuardianSpiritPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings BUFF")]
        [DisplayName("Use Fear Ward")]
        [Description("enable use of Fear Ward on CD")]
        public bool UseFearWard { get; set; }

        [Setting]
        [DefaultValue(85)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Heal hp%")]
        [Description("Cast Heal at target % hp")]
        public int HealPercent { get; set; }

        [Setting]
        [DefaultValue(65)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Binding heal hp%")]
        [Description("Cast Binding Heal if target and me hp % lower than this value")]
        public int BindingHealPercent { get; set; }

        [Setting]
        [DefaultValue(75)]
        [Category("Settings MANA REGENERATION")]
        [DisplayName("ShadowFiend/Mindbender mana%")]
        [Description("Cast ShadowFiend/Mindbender at % mana")]
        public int ShadowFiendPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MANA REGENERATION")]
        [DisplayName("Use Power Word: Solace")]
        [Description("Enable use of PW: Solace if enabled to gain mana and heal")]
        public bool UsePW_Solace { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MANA MANAGEMENT")]
        [DisplayName("TrySaveMana")]
        [Description("Try save mana: interrupt no needed heal")]
        public bool TrySaveMana { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Greater Heal hp%")]
        [Description("Cast Greater Heal at target % hp")]
        public int GHPercent { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("HW: Serenity hp%")]
        [Description("Cast HW: Serenity at target % hp")]
        public int HWSerenityPercent { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings AOE")]
        [DisplayName("HW: Sanctuary hp%")]
        [Description("Cast HW: Sanctuary if Player below %h >= HWSanctuaryNumber")]
        public int HWSanctuaryPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("HW: Sanctuary players number")]
        [Description("Cast HW: Sanctuary if Player below HWSanctuaryPercent >= this number")]
        public int HWSanctuaryNumber { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings AOE")]
        [DisplayName("Divine Insight--->PreyerOfMending hp%")]
        [Description("Cast PreyerOfMending on proc of Divine Insight if Player below %h >= PrayerOfMendingNumber")]
        public int PrayerOfMendingPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Divine Insight--->PreyerOfMending players number")]
        [Description("Cast PreyerOfMending if Player below PrayerOfMendingPercent >= this number")]
        public int PrayerOfMendingNumber { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings AOE")]
        [DisplayName("Circle of Healing hp%")]
        [Description("Cast CircleOfHealing if Player below hp% >= CircleOfHealingNumber")]
        public int CircleOfHealingPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Circle of Healing players number")]
        [Description("Cast CircleOfHealing if Player below CircleOfHealingPercent >= this number")]
        public int CircleOfHealingNumber { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Settings AOE")]
        [DisplayName("Divine Hymn hp%")]
        [Description("Cast Divine Hymn if Player below %h >= DHNumber")]
        public int DHPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("Divine Hymn players number")]
        [Description("Cast Divine Hymn if Player below DHPercent >= this number")]
        public int DHNumber { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("HW: Sanctuary on TANK")]
        [Description("Cast HW: Sanctuary if Player below HWSerenityPercent NEAR TANK >= HWSanctuaryNumber")]
        public bool HWSanctuaryOnTank { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Flash Heal hp%")]
        [Description("Cast Flash Heal at target % hp")]
        public int FlashHealPercent { get; set; }

        [Setting]
        [DefaultValue(85)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Surge of Ligth %hp")]
        [Description("Cast Surge of Ligth if player hp < %. SoL will cast on lower hp player same if buff ending")]
        public int SoLPercent { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Settings BUFF")]
        [DisplayName("Power Infusion hp%")]
        [Description("Cast Power Infusion to empower healing if number of player lower than %hp >= PowerInfusionNumber")]
        public int PowerInfusionPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings BUFF")]
        [DisplayName("Power Infusion players number")]
        [Description("Cast Power Infusion to empower healing if number of player lower than PowerInfusionPercent hp% >= this value")]
        public int PowerInfusionNumber { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Settings BUFF")]
        [DisplayName("Power Infusion mana%")]
        [Description("Cast Power Infusion to lower mana cost spell if our mana is lower than this value")]
        public int PowerInfusionManaPercent { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Desperate Prayer hp%")]
        [Description("Cast Desperate Prayer if we lower than % hp")]
        public int DesperatePrayerPercent { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings AOE")]
        [DisplayName("Prayer Of healing hp%")]
        [Description("Cast Prayer Of Healing if player lower than % hp >= PrayerOfHealingNumber")]
        public int PrayerOfHealingPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("Prayer Of healing players number")]
        [Description("Cast Prayer Of Healing if player lower than PrayerOfHealingPercent >= this value")]
        public int PrayerOfHealingNumber { get; set; }

        [Setting]
        [DefaultValue(85)]
        [Category("Settings AOE")]
        [DisplayName("Cascade/Halo/DivineStar %hp")]
        [Description("Cast Cascade/Halo if player lower than % hp >= CascadeHaloDivineStarNumber")]
        public int CascadeHaloPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Cascade/Halo/DivineStar players number")]
        [Description("Cast Cascade/Halo if player lower than CascadeHaloDivinestarPercent >= this value")]
        public int CascadeHaloNumber { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("RenewAlwaysActiveOnTank")]
        [Description("try keep Renew always active on tank")]
        public bool RenewAlwaysActiveOnTank { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Use Renew on moving")]
        [Description("Cast Renew at RenewPercent if moving")]
        public bool UseRenewOnMoving { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Renew hp%")]
        [Description("Cast Renew at hp%")]
        public int RenewPercent { get; set; }

        [Setting]
        [DefaultValue(25)]
        [Category("Settings MANA REGENERATION")]
        [DisplayName("Hymn Of Hope mana%")]
        [Description("Cast Hymn Of Hope at % mana")]
        public int HymnOfHopePercent { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Settings REST")]
        [DisplayName("Drink mana%")]
        [Description("Drink at % mana")]
        public int ManaPercent { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings REST")]
        [DisplayName("Eat hp%")]
        [Description("Eat at % hp")]
        public int HealthPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings VOID SHIFT")]
        [DisplayName("UseVoidShift")]
        [Description("enable/Disable Void Shift ")]
        public bool UseVoidShift { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings VOID SHIFT")]
        [DisplayName("UseVoidShiftOnlyOnTank")]
        [Description("enable/Disable cast Void Shift only on tank")]
        public bool UseVoidShiftOnlyOnTank { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Settings VOID SHIFT")]
        [DisplayName("VoidShiftHPTarget")]
        [Description("cast Void Shift on target below of this HP%")]
        public int VoidShiftTarget { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings VOID SHIFT")]
        [DisplayName("VoidShiftHPMe")]
        [Description("cast Void Shift on target only if my HP greather than this value")]
        public int VoidShiftMe { get; set; }

        
    }
}