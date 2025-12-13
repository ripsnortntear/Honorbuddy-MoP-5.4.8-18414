using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class ProtPaladinSettings : Settings
    {
        public static ProtPaladinSettings Instance = new ProtPaladinSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/ProtPaladin/ProtPaladin-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public ProtPaladinSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/ProtPaladin/ProtPaladin-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

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

        public enum AvengersWrathType
        {
            COOLDOWN,
            CONDITION,
            MANUAL
        }

        [Setting]
        [DefaultValue(AvengersWrathType.COOLDOWN)]
        [Category("Settings AVENGER'S WRATH")]
        [DisplayName("Avenger's Wrath cast Type")]
        [Description("Avenger's Wrath cast Type: Cooldown,Manual, CONDITION: will cast at reached AttackPower for Avenger's Wrath")]
        public AvengersWrathType AvengersWrathTypeUseMode { get; set; }

        [Setting]
        [DefaultValue(25000)]
        [Category("Settings AVENGER'S WRATH")]
        [DisplayName("Attack Power for autocast Avenger's Wrath")]
        [Description("if AvengersWrathTypeUseMode is set to CONDITION CC will cast Avenger's Wrath if current Attack Power greater than this value")]
        public int AttackPowerForAvengersWrath { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MOVEMENT")]
        [DisplayName("AutoFacingTarget")]
        [Description("Enable/Disable autofacing current target")]
        public bool AutofaceTarget { get; set; }

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
        [DisplayName("Proc Autoheal HP")]
        [Description("Cast Autoheal spells at HP% if something proc")]
        public int ProcAutoHealHP { get; set; }

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
        [DefaultValue(50)]
        [Category("Settings SOS")]
        [DisplayName("Guardian Of AncientKing HP%")]
        [Description("Cast Guardian Of AncientKing at HP%")]
        public int GuardianOfAncientKing { get; set; }

        [Setting]
        [DefaultValue(10)]
        [Category("Settings SOS")]
        [DisplayName("Divine Shield HP%")]
        [Description("Cast Divine Shield at HP%")]
        public int DShp { get; set; }
        
        [Setting]
        [DefaultValue(30)]
        [Category("Settings SOS")]
        [DisplayName("ArdentDefender HP%")]
        [Description("Cast ArdentDefender at HP%")]
        public int ArdentDefender { get; set; }

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
        [DefaultValue(3)]
        [Category("Settings AOE")]
        [DisplayName("AOE Count")]
        [Description("AOE dps at # enemy")]
        public int AOECount { get; set; }

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
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use Redemption")]
        [Description("enable use of Redemption out of combat")]
        public bool UseRedemption { get; set; }
        
            

    }
}