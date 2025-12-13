using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class RetriPaladinSettings : Settings
    {
        public static RetriPaladinSettings Instance = new RetriPaladinSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/RetriPaladin/RetriPaladin-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public RetriPaladinSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/RetriPaladin/RetriPaladin-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

        public enum CDUseType
        {
            COOLDOWN,
            BOSS,
            MANUAL
        }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Avenging Wrath")]
        [Description("Chose when use Avenging Wrath CD")]
        public CDUseType CDUseAvengingWrath { get; set; }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Guardian of Ancient Kings")]
        [Description("Chose when use Guardian of Ancient Kings CD")]
        public CDUseType CDUseGuardian_of_Ancient_Kings { get; set; }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Holy Avenger")]
        [Description("Chose when use Holy Avenger CD")]
        public CDUseType CDUseHoly_Avenger { get; set; }
       

        public enum BlessingType
        {
            MANUAL,
            KING,
            MIGTH
        }

        public enum SealType
        {
            AUTO,
            INSIGHT,
            TRUTH,
            RIGHTEOUSNESS
        }

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
        [DefaultValue(false)]
        [Category("Settings MOVEMENT")]
        [DisplayName("UseEmancipate")]
        [Description("Enable/Disable UseEmancipate expecially in PVP")]
        public bool UseEmancipate { get; set; }

        

        [Setting]
        [DefaultValue(BlessingType.MANUAL)]
        [Category("Settings BUFF")]
        [DisplayName("Blessing Type")]
        [Description("Blessing to use")]
        public BlessingType BlessingToUse { get; set; }

        [Setting]
        [DefaultValue(10)]
        [Category("Settings BUFF")]
        [DisplayName("AP % Increment for SacredShield")]
        [Description("If Sacred shield buff is up and CC detect a specified % increment of Attack Power it recasts Sacred Shield for powerful absorb")]
        public double AttackPowerIncrement { get; set; }

        [Setting]
        [DefaultValue(SealType.AUTO)]
        [Category("Settings BUFF")]
        [DisplayName("Seal Type")]
        [Description("SealType to use: AUTO will use seal of insight and will cast seal of righteousness on AOE")]
        public SealType SealToUse { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings PULL")]
        [DisplayName("PullDistance")]
        [Description("Distance to pull from")]
        public float PullDistance { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Settings AUTOHEAL")]
        [DisplayName("Proc Autoheal EternalFlame/WordOfGlory HP")]
        [Description("Cast Autoheal spells at HP% if something proc")]
        public int ProcAutoHealHP_EF_WoG { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Settings AUTOHEAL")]
        [DisplayName("Proc Autoheal Selfless Healer HP")]
        [Description("Cast Autoheal spells at HP% if selfless healer proc")]
        public int ProcAutoHealHPSelfless { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings AUTOHEAL")]
        [DisplayName("Autoheal HP")]
        [Description("Cast Autoheal spells at HP%")]
        public int AutoHealHP { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings SOS")]
        [DisplayName("Divine Protection HP%")]
        [Description("Cast Divine Protection at HP%")]
        public int DivineProtection { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings SOS")]
        [DisplayName("HandOfPurity HP%")]
        [Description("Cast HandOfPurity at HP%")]
        public int HandOfPurity { get; set; }


        [Setting]
        [DefaultValue(10)]
        [Category("Settings SOS")]
        [DisplayName("Divine Shield HP%")]
        [Description("Cast Divine Shield at HP%")]
        public int DShp { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings INTERRUPT")]
        [DisplayName("autointerrupt")]
        [Description("Cast rebuke/fist_of_justice if target casting")]
        public bool AutoInterrupt { get; set; }


        [Setting]
        [DefaultValue(false)]
        [Category("Settings SOS PARTY")]
        [DisplayName("Use LOH for other party player")]
        [Description("Use Lay on hand for other party player")]
        public bool UseLoHOnPartyMember { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SOS PARTY")]
        [DisplayName("Use HoP for other party player")]
        [Description("Use Hand of Protection for other party player")]
        public bool UseHoPOnPartyMember { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings SOS PARTY")]
        [DisplayName("Use DS for other party player")]
        [Description("Use Divine Shield for other party player")]
        public bool UseDSOnPartyMember { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings SOS PARTY")]
        [DisplayName("Use Healing procs for other party player")]
        [Description("Use Word of Glory/Eternal Flame/etc.. for other party player")]
        public bool UseHealingPartyMember { get; set; }

        [Setting]
        [DefaultValue(10)]
        [Category("Settings SOS")]
        [DisplayName("Hand of Protection HP%")]
        [Description("Cast Hand of Protection at HP%)")]
        public int HoPHp { get; set; }

        [Setting]
        [DefaultValue(15)]
        [Category("Settings SOS")]
        [DisplayName("Lay on Hands HP%")]
        [Description("Cast Lay on Hands at HP%")]
        public int LoHhp { get; set; }

        [Setting]
        [DefaultValue(7)]
        [Category("Settings AOE")]
        [DisplayName("Seal of Righteousness Number")]
        [Description("If Autoseal enabled: cast  Seal of Righteousness if enemy around 8 will be greater or equal than this value")]
        public int AOE_Seal_Of_Righteousness { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("Hammer of the Righteous Number")]
        [Description("cast  Hammer of the Righteous if enemy around 8 will be greater or equal than this value")]
        public int AOE_Hammer_Of_The_Righteous { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("Divine Storm Number")]
        [Description("cast  Divine Storm if enemy around 8 will be greater or equal than this value")]
        public int AOE_Divine_Storm { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Settings REST")]
        [DisplayName("Drink mana%")]
        [Description("Drink at % mana")]
        public int ManaPercent { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings REST")]
        [DisplayName("Eat hp%")]
        [Description("Eat at % hp")]
        public int HealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use Redemption")]
        [Description("enable use of Redemption out of combat")]
        public bool UseRedemption { get; set; }



    }
}