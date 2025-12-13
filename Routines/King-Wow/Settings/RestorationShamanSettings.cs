using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class RestorationShamanSettings : Settings
    {
        public static RestorationShamanSettings Instance = new RestorationShamanSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/RestorationShaman/RestorationShaman-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public RestorationShamanSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/RestorationShaman/RestorationShaman-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

        public enum ImbueType
        {
            DEFAULT,
            MANUAL
        }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings SOS")]
        [DisplayName("UseSoS")]
        [Description("enable SoS healing: It require ANCESTRAL SWIFTNESS otherwise will cast a simple HEALING SURGE")]
        public bool UseSoS { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SOS")]
        [DisplayName("SoS_healing_only_on_tank")]
        [Description("enable SoS routine only on tank")]
        public bool SoS_healing_only_on_tank { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings SOS")]
        [DisplayName("SoS HP%")]
        [Description("Cast SoS heal only if sos healing target hp% below this value")]
        public int SoSHP { get; set; }

        [Setting]
        [DefaultValue(ImbueType.DEFAULT)]
        [Category("Settings IMBUE")]
        [DisplayName("ImbueType")]
        [Description("Imbue method")]
        public ImbueType imbueType { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MOVEMENT")]
        [DisplayName("AutoFacingTarget")]
        [Description("Enable/Disable autofacing current target")]
        public bool AutofaceTarget { get; set; }

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
        [DefaultValue(50)]
        [Category("Settings SELF HEALING")]
        [DisplayName("SelfHealingPriorityHP")]
        [Description("if enabled Self Healing Priority: healing you before other in party until below this HP%")]
        public float SelfHealingPriorityHP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings INTERRUPT")]
        [DisplayName("autointerrupt")]
        [Description("Cast WindShear if target casting")]
        public bool AutoInterrupt { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("Use Hex")]
        [Description("Use Hex on focused target")]
        public bool UseHex { get; set; }
        

        /*[Setting]
        [DefaultValue(0)]
        [Category("Settings ROTATION")]
        [DisplayName("Rotation Type")]
        [Description("Use 0 for NORMAL ROTATION, Use 1 For First Tank and SINGLE TARGET ROTATION, Use 2 for AOE PRIORITY ROTATION, use 3 for PVP ROTATION")]
        public int RotationType { get; set; }*/

        public enum CDUseType
        {
            COOLDOWN,
            AT_CONDITION,
            MANUAL
        }

        [Setting]
        [DefaultValue(CDUseType.AT_CONDITION)]
        [Category("Settings ASCENDANCE")]
        [DisplayName("WhenUseAscendance ")]
        [Description("choose when cast Ascendance CD")]
        public CDUseType WhenUseAscendance { get; set; }

        [Setting]
        [DefaultValue(65)]
        [Category("Settings ASCENDANCE")]
        [DisplayName("AscendanceHP")]
        [Description("cast Ascendance if AT_CONDITION choosen and AscendanceNumber players below this value HP% verified ")]
        public int AscendanceHP { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings ASCENDANCE")]
        [DisplayName("AscendanceNumber")]
        [Description("cast Ascendance if AT_CONDITION choosen and players below AscendanceHP number high or equal this number verified ")]
        public int AscendanceNumber { get; set; }



        [Setting]
        [DefaultValue(true)]
        [Category("Settings DISPELLS")]
        [DisplayName("Use Dispell ")]
        [Description("enable/diable dispell")]
        public bool UseDispell { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use Resurrection")]
        [Description("enable use of Resurrection out of combat")]
        public bool UseResurrection { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Autocast Spiritwalker's Grace")]
        [Description("enable use Autocast Spiritwalker's Grace when moving on combat (it is activated when CC found at least a player whith less than healing wave percent HP")]
        public bool Autocast_Spirit_Walk { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings SPECIAL")]
        [DisplayName("Mantain TidalWaves buff")]
        [Description("Cast Riptide on lowest heal target if no tidalWaves buff active")]
        public bool MantainTidalWaves { get; set; }

        [Setting]
        [DefaultValue(85)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Riptide hp%")]
        [Description("Cast Riptide at target % hp")]
        public int RiptidePercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("RiptideAlwaysUPOnTank")]
        [Description("Try to mantain Riptide always UP on tank")]
        public bool RiptideAlwaysUPOnTank { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("HealingWave hp%")]
        [Description("Cast HealingWave at target % hp")]
        public int HealingWavePercent { get; set; }

        [Setting]
        [DefaultValue(75)]
        [Category("Settings MANA REGENERATION")]
        [DisplayName("Mana Tide Totem mana%")]
        [Description("Mana Tide Totem at % mana")]
        public int ManaTideTotemPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MANA MANAGEMENT")]
        [DisplayName("TrySaveMana")]
        [Description("Try save mana: interrupt no needed heal")]
        public bool TrySaveMana { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MANA MANAGEMENT")]
        [DisplayName("LIGHTNING_BOLT to rec mana (only glyphed)")]
        [Description("Cast LIGHTNING_BOLT in no other heal needed to rec mana: used ONLY if \"Telluric Currents\" glyphed")]
        public bool LB_for_rec_mana { get; set; }
        

        [Setting]
        [DefaultValue(50)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Greater Healing Wave hp%")]
        [Description("Cast Greater Healing Wave at target % hp")]
        public int GreaterHealingWavePercent { get; set; }

        [Setting]
        [DefaultValue(85)]
        [Category("Settings AOE")]
        [DisplayName("Healing Rain hp%")]
        [Description("Cast Healing Rain if Player below %h >= #HealingRainNumber")]
        public int HealingRainPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("Healing Rain players number")]
        [Description("Cast Healing Rain if Player below HealingRainPercent >= this number")]
        public int HealingRainNumber { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("HealingRainOnlyOnTankLocation")]
        [Description("Cast Healing Rain only on tank location if condition verified")]
        public bool HealingRainOnlyOnTankLocation { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings SPELLS HEAL TUNING")]
        [DisplayName("Healing Surge hp%")]
        [Description("Cast Healing Surge at target % hp")]
        public int HealingSurgePercent { get; set; }

        [Setting]
        [DefaultValue(75)]
        [Category("Settings AOE")]
        [DisplayName("Chain Heal hp%")]
        [Description("Cast Chain Heal if player lower than % hp >= ChainHealNumber")]
        public int ChainHealPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("Chain Heal players number")]
        [Description("Cast Chain Heal if player lower than ChainHealPercent >= this value")]
        public int ChainHealNumber { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("ChainHealOnlyFromTank")]
        [Description("Cast Chain heal only from tank location if condition verified")]
        public bool ChainHealOnlyFromTank { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings AOE")]
        [DisplayName("Spirit Link hp%")]
        [Description("Cast Spirit Link if player lower than % hp >= SpiritLinkNumber")]
        public int SpiritLinkPercent { get; set; }

        [Setting]
        [DefaultValue(4)]
        [Category("Settings AOE")]
        [DisplayName("Spirit Link players number")]
        [Description("Cast Spirit Link if player lower than SpiritLinkPercent >= this value")]
        public int SpiritLinkNumber { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings REST")]
        [DisplayName("Drink mana%")]
        [Description("Drink at % mana")]
        public int ManaPercent { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings REST")]
        [DisplayName("Eat hp%")]
        [Description("Eat at % hp")]
        public int HealthPercent { get; set; }

        public enum healingTotemCastType
        {
            ALWAYS,
            AT_CONDITION,
            MANUAL
        }

        [Setting]
        [DefaultValue(healingTotemCastType.ALWAYS)]
        [Category("Settings STREAM_TOTEM")]
        [DisplayName("WhenCastStreamTotem")]
        [Description("Always will cast on ccondown, conditios will cast stream totem when heal percent and number verified")]
        public healingTotemCastType WhenCastStreamTotem { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings STREAM_TOTEM")]
        [DisplayName("Stream Totem hp%")]
        [Description("Cast Stream Totem if player lower than % hp >= StreamTotemNumber")]
        public int StreamTotemPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings STREAM_TOTEM")]
        [DisplayName("Stream Totem players number")]
        [Description("Cast Stream Totem if player lower than StreamTotemPercent >= this value")]
        public int StreamTotemNumber { get; set; }

        [Setting]
        [DefaultValue(healingTotemCastType.AT_CONDITION)]
        [Category("Settings HEALING_TIDE_TOTEM")]
        [DisplayName("WhenCastHealingTideTotem")]
        [Description("Always will cast on ccondown, conditios will cast stream totem when heal percent and number verified")]
        public healingTotemCastType WhenCastHealingTideTotem { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings HEALING_TIDE_TOTEM")]
        [DisplayName("HealingTideTotem hp%")]
        [Description("Cast Stream Totem if player lower than % hp >= StreamTotemNumber")]
        public int HealingTideTotemPercent { get; set; }

        [Setting]
        [DefaultValue(4)]
        [Category("Settings HEALING_TIDE_TOTEM")]
        [DisplayName("HealingTideTotem players number")]
        [Description("Cast HealingTideTotem if player lower than StreamTotemPercent >= this value")]
        public int HealingTideTotemNumber { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("use OOC healing")]
        [Description("enable-disable use of OutOfCombat healing")]
        public bool OOCHealing { get; set; }
    }
}