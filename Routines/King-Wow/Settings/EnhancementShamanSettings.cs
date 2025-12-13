using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class EnhancementShamanSettings : Settings
    {
        public static EnhancementShamanSettings Instance = new EnhancementShamanSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/EnhancementShaman/EnhancementShaman-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public EnhancementShamanSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/EnhancementShaman/EnhancementShaman-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

        [Setting]
        [DefaultValue(28)]
        [Category("Settings PULL")]
        [DisplayName("PullDistance")]
        [Description("Distance to pull from")]
        public float PullDistance { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Settings REST")]
        [DisplayName("Drink mana%")]
        [Description("Drink at % mana")]
        public int ManaPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings INTERRUPT")]
        [DisplayName("autointerrupt")]
        [Description("Cast WindShear if target casting")]
        public bool AutoInterrupt { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings REST")]
        [DisplayName("Eat hp%")]
        [Description("Eat at % hp")]
        public int HealthPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings FILLER")]
        [DisplayName("Use LB or CL as filler in rotation")]
        [Description("Use CHAIN_LIGHTNING or LIGHTNING_BOLT as filler in rotation if reach sufficent maelstrom stacks configurable")]
        public bool Use_LB_or_CL_as_filler { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings FILLER")]
        [DisplayName("Number of MAELSTROM for filler")]
        [Description("Use CHAIN_LIGHTNING or LIGHTNING_BOLT as filler in rotation if reach this number of maelstrom stacks")]
        public int maelstrom_fillet_count { get; set; }
        

        public enum ImbueType
        {
            DEFAULT,
            CHOOSEN,
            MANUAL
        }

        [Setting]
        [DefaultValue(ImbueType.DEFAULT)]
        [Category("Settings IMBUE")]
        [DisplayName("ImbueType")]
        [Description("Imbue method if CHOOSEN set MH and OH imbue type below")]
        public ImbueType imbueType { get; set; }

        public enum ImbueWeaponType
        {
            WINDFURY,
            FLAMETONGUE,
            FROSTBRAND,
            ROCKBITER,
            EARTHLIVING
        }

        [Setting]
        [DefaultValue(ImbueWeaponType.WINDFURY)]
        [Category("Settings IMBUE")]
        [DisplayName("MainHand_imbue")]
        [Description("If Imbue method is CHOOSEN set MH imbue to this value")]
        public ImbueWeaponType MainHand_imbue { get; set; }

        [Setting]
        [DefaultValue(ImbueWeaponType.FLAMETONGUE)]
        [Category("Settings IMBUE")]
        [DisplayName("OffHand_imbue")]
        [Description("If Imbue method is CHOOSEN set OH imbue to this value")]
        public ImbueWeaponType OffHand_imbue { get; set; }

        public enum CDUseType
        {
            COOLDOWN,
            BOSS,
            MANUAL
        }

        public enum AirTotemType
        {
            CAPACITOR,
            GROUNDIG,
            MANUAL
        }

        [Setting]
        [DefaultValue(AirTotemType.GROUNDIG)]
        [Category("Settings TOTEM")]
        [DisplayName("Air Totem")]
        [Description("Chose Air Totem to pup active as possible CD")]
        public AirTotemType AirTotemActive { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TOTEM")]
        [DisplayName("Use Totemic Recall")]
        [Description("Use Totemic Recall when OutOfCombat")]
        public bool Totemic_recall_OOC { get; set; }
        
        [Setting]
        [DefaultValue(true)]
        [Category("Settings TALENT")]
        [DisplayName("Elemental Blast if instant")]
        [Description("Use Elemental Blast only on five maelstrom")]
        public bool EB_on_five { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings TALENT")]
        [DisplayName("Astral Shift HP%")]
        [Description("Cast Astral Shift if HP% lower than this value")]
        public int AS_HP { get; set; }

        [Setting]
        [DefaultValue(CDUseType.MANUAL)]
        [Category("Settings CD")]
        [DisplayName("Use Fire Elemental")]
        [Description("Chose when use your Fire Elemental CD")]
        public CDUseType FireElementalCD { get; set; }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Feral Spirits")]
        [Description("Chose when use your Feral Spirits CD")]
        public CDUseType FeralSpiritCD { get; set; }

        [Setting]
        [DefaultValue(CDUseType.BOSS)]
        [Category("Settings CD")]
        [DisplayName("Use Storm Lash Totem")]
        [Description("Chose when use your Storm Lash Totem CD")]
        public CDUseType StormlLashTotemCD { get; set; }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Elemental Mastery")]
        [Description("Chose when use your Elemental Mastery CD")]
        public CDUseType ElementalMasteryCD { get; set; }
        

        [Setting]
        [DefaultValue(CDUseType.MANUAL)]
        [Category("Settings CD")]
        [DisplayName("Use Ascendance")]
        [Description("Chose when use your Ascendance CD")]
        public CDUseType AscendanceCD { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Chain Lightining mob #")]
        [Description("Use chain lightining when # mobs > this value")]
        public int ChainLightining_number { get; set; }

        [Setting]
        [DefaultValue(5)]
        [Category("Settings AOE")]
        [DisplayName("Magma Totem mob #")]
        [Description("Use Magma Totem when # mobs > this value")]
        public int MagmaTotem_number { get; set; }

        [Setting]
        [DefaultValue(5)]
        [Category("Settings AOE")]
        [DisplayName("Fire Nova mob #")]
        [Description("Use Fire Nova when # mobs > this value")]
        public int FireNova_number { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings HEALING")]
        [DisplayName("assist healing mode")]
        [Description("Enable/Disable assist healing mode")]
        public bool AssistHealing { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings HEALING")]
        [DisplayName("Full my HP when OutOfCombat")]
        [Description("Full my HP when OutOfCombat and my mana above 70")]
        public bool FullMeOOC { get; set; }
        
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MOVEMENT")]
        [DisplayName("AutoFacingTarget")]
        [Description("Enable/Disable autofacing current target")]
        public bool AutofaceTarget { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MOVEMENT")]
        [DisplayName("AutoTarget")]
        [Description("Enable/Disable AutoTarget enemy")]
        public bool AutoTarget { get; set; }
        
        

        [Setting]
        [DefaultValue(80)]
        [Category("Settings HEALING")]
        [DisplayName("HealingRain/Ancestral Guidance HP%")]
        [Description("Cast Healing Rain/Ancestral Guidance if players NEAR ME below this &hp >= HealingRain_AncestralGuidanceNumber ")]
        public int HealingRain_AncestralGuidanceHP { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings HEALING")]
        [DisplayName("HealingRain_AncestralGuidanceNumber of players")]
        [Description("Cast Healing Rain/Ancestral Guidance if players NEAR ME below this HealingRain_AncestralGuidanceHP HP% >= this value")]
        public int HealingRain_AncestralGuidanceNumber { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings HEALING")]
        [DisplayName("ShamanistcRage HP%")]
        [Description("Cast ShamanistcRage if player HP% below this value ")]
        public int ShamanistcRageHP { get; set; }

        

        [Setting]
        [DefaultValue(40)]
        [Category("Settings HEALING")]
        [DisplayName("HealingSurge HP%")]
        [Description("Cast HealingSurge if player hp <= this value and maelstrom >=3")]
        public int HealingSurgeHP { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings HEALING")]
        [DisplayName("AncestralGuidanceMyHP HP%")]
        [Description("Cast AncestralGuidance if player hp <= this value")]
        public int AncestralGuidanceMyHP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings HEALING")]
        [DisplayName("HealingSurgeOnlyOnMe")]
        [Description("Cast HealingSurge Only on Shaman or other player too")]
        public bool HealingSurgeOnlyOnMe { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings HEALING")]
        [DisplayName("UseHealingTotem")]
        [Description("Pop Healing stream totem or Healing tide totem on healing rain condition")]
        public bool UseHealingTotem { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("Use Hex")]
        [Description("Use Hex on focused target")]
        public bool UseHex { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings CC")]
        [DisplayName("Use only instant Hex ")]
        [Description("Use Hex on focused target only if instant cast")]
        public bool UseInstantHex { get; set; }
        
    }
}