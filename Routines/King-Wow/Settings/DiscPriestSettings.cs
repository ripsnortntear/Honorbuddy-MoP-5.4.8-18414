using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class DiscPriestSettings : Settings
    {
        public static DiscPriestSettings Instance = new DiscPriestSettings();

        
        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/DiscPriest/DiscPriest-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public DiscPriestSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/DiscPriest/DiscPriest-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
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
        [DefaultValue(50)]
        [Category("Settings SELF HEALING")]
        [DisplayName("SelfHealingPriorityHP")]
        [Description("if enabled Self Healing Priority: healing you before other in party until below this HP%")]
        public float SelfHealingPriorityHP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings CD")]
        [DisplayName("Use Archangel")]
        [Description("Cast Archangel at 5 stacks of evangelism")]
        public bool UseArchangel { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings CD")]
        [DisplayName("Use Inner Focus")]
        [Description("Use Inner Focus every CD")]
        public bool UseInnerFocus { get; set; }

        [Setting]
        [DefaultValue(Rotation.PVE)]
        [Category("Settings ROTATION")]
        [DisplayName("Rotation Type")]
        [Description("use PVE or PVP or QUESTING rotation tipe")]
        public Rotation RotationType { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings ROTATION")]
        [DisplayName("Auto switch Atonement/Normal healing")]
        [Description("Automatic switch between Atonement and Normal healing (use Normal healing when there is nothing to attack")]
        public bool AutoSwitchAtonementNormal { get; set; }

        public enum Rotation
        {
            PVE,
            PVP,
            QUESTING
        }

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

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Auto PW:Fortitude")]
        [Description("Cast Automaticaly PW:Fortitude on party")]
        public bool AutoPWFortitude { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings BUFF")]
        [DisplayName("Use Preyer of Mending")]
        [Description("Enable/Disable Preyer of Mending")]
        public bool Use_PoM { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings BUFF")]
        [DisplayName("Use Inner will moving")]
        [Description("chose to use inner will if moving")]
        public bool InnerWillOnMoving { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Use talent for speed on moving")]
        [Description("chose to use AngelicFaether/PW:Shield if talented and if moving")]
        public bool BurstSpeedMoving { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Use Fade")]
        [Description("chose to use fade on aggro")]
        public bool UseFade { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings DISPELLS")]
        [DisplayName("Use Purify ")]
        [Description("Use Purify for cure disease and dispell magic")]
        public bool UsePurify { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings DISPELLS")]
        [DisplayName("Use UseMassDispell ")]
        [Description("Use UseMassDispell for cure disease and dispell magic on many players")]
        public bool UseMassDispell { get; set; }

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
        [DefaultValue(false)]
        [Category("Settings SPECIAL")]
        [DisplayName("Show Message healing autoswitch")]
        [Description("Show a chat Message when healing autoswitch from atonement to rormal and viceversa")]
        public bool ShowMessageSwitching { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings SPECIAL")]
        [DisplayName("Heal targetted NPC Threshold")]
        [Description("Set Threshold for heal targetted NPC over party member: CC will heal the targetted NPC if noone party/raid member in healing range below this value")]
        public int HealNPC_threshold { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("use OOC healing")]
        [Description("enable-disable use of OutOfCombat healing")]
        public bool OOCHealing { get; set; }
        

        [Setting]
        [DefaultValue(false)]
        [Category("Settings BUFF")]
        [DisplayName("Use Fear Ward")]
        [Description("enable use of Fear Ward on CD")]
        public bool UseFearWard { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings DAMAGE REDUCTION")]
        [DisplayName("Auto use Pain Suppression")]
        [Description("enable use of Pain Suppression")]
        public bool UsePainSuppression { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TANK")]
        [DisplayName("Use Pain Suppression ONLY on TANK")]
        [Description("enable use of Pain Suppression ONLY on TANK or on you when in solo")]
        public bool UsePainSuppressionOnlyOnTank { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings OFF_TANK")]
        [DisplayName("Use Pain Suppression on OFF_TANK too")]
        [Description("enable use of Pain Suppression on OFF_TANK or on you when in solo")]
        public bool UsePainSuppressionOnOffTank { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings OFF_TANK")]
        [DisplayName("Use OFF_TANK healing")]
        [Description("enable/disable OffTank Healing priority (same features of main tank except keep grace) but main tank will be always first")]
        public bool OffTankHealing { get; set; }
        

        [Setting]
        [DefaultValue(50)]
        [Category("Settings DAMAGE REDUCTION")]
        [DisplayName("Pain Suppression hp%")]
        [Description("Cast Pain Suppression at target % hp")]
        public int PainSuppressionPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings DAMAGE REDUCTION")]
        [DisplayName("Use_SS_on_POM")]
        [Description("Use Spirit Shell everytime CC going to use Prayer Of Healing")]
        public bool UseSS_on_POM { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings TANK")]
        [DisplayName("Priorize Tank Healing")]
        [Description("Priorize Tank healing over all except an SoS heal")]
        public bool PriorizeTankHealing { get; set; }
        
        
        [Setting]
        [DefaultValue(75)]
        [Category("Settings MANA REGENERATION")]
        [DisplayName("ShadowFiend/Mindbender mana%")]
        [Description("Cast ShadowFiend/Mindbender at % mana")]
        public int ShadowFiendPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings MANA MANAGEMENT")]
        [DisplayName("TrySaveMana")]
        [Description("Try save mana: interrupt no needed heal")]
        public bool TrySaveMana { get; set; }

        [Setting]
        [DefaultValue(100)]
        [Category("Settings TANK")]
        [DisplayName("PW: Shield on Tank hp%")]
        [Description("Cast PW: Shield on Tank at % hp set to 100 for always up shield on tank")]
        public int TankShieldPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings TANK")]
        [DisplayName("Mantain Renew on Tank")]
        [Description("Mantain Renew on main Tank")]
        public bool RenewOnMainTank { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings OFF_TANK")]
        [DisplayName("Mantain Renew on OffTank")]
        [Description("Mantain Renew on OffTank")]
        public bool RenewOnOffTank { get; set; }

        [Setting]
        [DefaultValue(55)]
        [Category("Settings SHIELDS")]
        [DisplayName("PW: Shield hp%")]
        [Description("Cast PW: Shield on Player not Tank at % hp")]
        public int PWShieldPercent { get; set; }


        //NORMAL (NOT ATONEMENT) HEALING TUNING
        [Setting]
        [DefaultValue(80)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Penance hp%")]
        [Description("Cast Penance at target % hp")]
        public int NoAtonementPenancePercent { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Binding heal hp%")]
        [Description("Cast Binding Heal if target and me hp % lower than this value")]
        public int NoAtonementBindingHealPercent { get; set; }

        [Setting]
        [DefaultValue(90)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Heal hp%")]
        [Description("Cast Heal at target % hp")]
        public int NoAtonementHealPercent { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Greater Heal hp%")]
        [Description("Cast Greater Heal at target % hp")]
        public int NoAtonementGHPercent { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Flash Heal hp%")]
        [Description("Cast Flash Heal at target % hp")]
        public int NoAtonementFlashHealPercent { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Surge of Ligth %hp")]
        [Description("Cast Surge of Ligth if player hp < %. SoL will cast on lower hp player same if buff ending")]
        public int NoAtonementSoLPercent { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Desperate Prayer hp%")]
        [Description("Cast Desperate Prayer if we lower than % hp")]
        public int NoAtonementDesperatePrayerPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Use Renew on moving")]
        [Description("Cast Renew at RenewPercent if moving")]
        public bool NoAtonementUseRenewOnMoving { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings NORMAL (NOT ATONEMENT) HEAL TUNING")]
        [DisplayName("Renew hp%")]
        [Description("Cast Renew at hp%")]
        public int NoAtonementRenewPercent { get; set; }

        /*[Setting]
        [DefaultValue(true)]
        [Category("Settings SHIELDS")]
        [DisplayName("AutoTriggerSS")]
        [Description("AutoTrigger Spirit Shell: when buff detected autospam prayer of healing")]
        public bool AutoTriggerSS { get; set; }*/

        //ATONEMENT HEALING TUNING
        [Setting]
        [DefaultValue(50)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Penance hp%")]
        [Description("Cast Penance at target % hp")]
        public int AtonementPenancePercent { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Binding heal hp%")]
        [Description("Cast Binding Heal if target and me hp % lower than this value")]
        public int AtonementBindingHealPercent { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Heal hp%")]
        [Description("Cast Heal at target % hp")]
        public int AtonementHealPercent { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Greater Heal hp%")]
        [Description("Cast Greater Heal at target % hp")]
        public int AtonementGHPercent { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Flash Heal hp%")]
        [Description("Cast Flash Heal at target % hp")]
        public int AtonementFlashHealPercent { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Surge of Ligth %hp")]
        [Description("Cast Surge of Ligth if player hp < %. SoL will cast on lower hp player same if buff ending")]
        public int AtonementSoLPercent { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Desperate Prayer hp%")]
        [Description("Cast Desperate Prayer if we lower than % hp")]
        public int AtonementDesperatePrayerPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Use Renew on moving")]
        [Description("Cast Renew at RenewPercent if moving")]
        public bool AtonementUseRenewOnMoving { get; set; }

        [Setting]
        [DefaultValue(75)]
        [Category("Settings ATONEMENT HEAL TUNING")]
        [DisplayName("Renew hp%")]
        [Description("Cast Renew at hp%")]
        public int AtonementRenewPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings DAMAGE REDUCTION")]
        [DisplayName("Auto use PW: Barrier")]
        [Description("enable automatic use of PW: Barrier")]
        public bool AutoUsePWBarrier { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Settings DAMAGE REDUCTION")]
        [DisplayName("PW: Barrier hp%")]
        [Description("Cast PW: Barrier if Player below %h >= #PWBarrierNumber")]
        public int PWBarrierPercent { get; set; }

        [Setting]
        [DefaultValue(6)]
        [Category("Settings DAMAGE REDUCTION")]
        [DisplayName("PW: Barrier players number")]
        [Description("Cast PW: Barrier if Player below PWBarrierPercent >= this number")]
        public int PWBarrierNumber { get; set; }

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
        [DefaultValue(false)]
        [Category("Settings TANK")]
        [DisplayName("Keep Grace on Tank")]
        [Description("Try to keep and mantain 3 stacks of grace on Tank")]
        public bool keepGraceOnTank { get; set; }

        [Setting]
        [DefaultValue(4500)]
        [Category("Settings TANK")]
        [DisplayName("Time Limit Refresh Grace")]
        [Description("Try to keep and mantain 3 stacks of grace on Tank: if grace stacks >=2 and Time grace to expire <= this value (milliseconds) CC will cast a heal just to refresh buff")]
        public int keepGraceOnTankTime { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SOS")]
        [DisplayName("use SoS routine")]
        [Description("Interrupt any other spell cast (except SpiritShell) if anyone lower than SoS heal % hp")]
        public bool SoSEnabled { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Settings SOS")]
        [DisplayName("SoS hp%")]
        [Description("Cast SoS heal if anyone in party lower than % hp")]
        public int SoSPercent { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Settings AOE")]
        [DisplayName("Prayer Of healing hp%")]
        [Description("Cast Prayer Of Healing if player lower than % hp >= PrayerOfHealingNumber")]
        public int PrayerOfHealingPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Prayer Of healing players number")]
        [Description("Cast Prayer Of Healing if player lower than PrayerOfHealingPercent >= this value")]
        public int PrayerOfHealingNumber { get; set; }

        [Setting]
        [DefaultValue(85)]
        [Category("Settings AOE")]
        [DisplayName("Cascade/Halo/DivineStar %hp")]
        [Description("Cast Cascade/Halo/DivineStar if player lower than % hp >= CascadeHaloDivineStarNumber")]      
        public int CascadeHaloDivinestarPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Cascade/halo/DivineStar players number")]
        [Description("Cast Cascade/halo/DivineStar if player lower than CascadeHaloDivinestarPercent >= this value")]
        public int CascadeHaloDivineStarNumber { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("Offensive DivineStar Use")]
        [Description("Cast Divine Star in offensive mode if you are facing CascadeHaloDivineStarNumber enemy")]
        public bool DivineStarOffensive { get; set; }

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

        /*[Setting]
        [DefaultValue(true)]
        [Category("Settings ATONEMENT")]
        [DisplayName("UseAtonement")]
        [Description("Chose to heal dpsing or not")]
        public bool UseAtonement { get; set; }*/

        [Setting]
        [DefaultValue(100)]
        [Category("Settings ATONEMENT")]
        [DisplayName("Atonement hp%")]
        [Description("Start Atonement healing only if someone on group has hp% < this value")]
        public int AtonementHp { get; set; }

        [Setting]
        [DefaultValue(10)]
        [Category("Settings ATONEMENT")]
        [DisplayName("Atonement Mana Threshold")]
        [Description("Cast Smite,Penance and Holy Fire to heal if mana bigger than this value. If you dont want atonement set this value to 100")]
        public int AtonementManaThreshold { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings ATONEMENT")]
        [DisplayName("use Penance in dps")]
        [Description("use penance in dps rotation end in atonement rotation")]
        public bool UsePenanceInDPS { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings VOID SHIFT")]
        [DisplayName("UseVoidShift")]
        [Description("enable/Disable Void Shift ")]
        public bool UseVoidShift { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TANK")]
        [DisplayName("UseVoidShiftOnTank")]
        [Description("enable/Disable cast Void Shift only on tank")]
        public bool UseVoidShiftOnlyOnTank { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings OFF_TANK")]
        [DisplayName("UseVoidShiftOnOffTank")]
        [Description("enable/Disable cast Void Shift on offtank too")]
        public bool UseVoidShiftOnOffTank { get; set; }

        [Setting]
        [DefaultValue(100)]
        [Category("Settings OFF_TANK")]
        [DisplayName("PW: Shield on OffTank hp%")]
        [Description("Cast PW: Shield on OffTank at % hp set to 100 for always up shield on tank")]
        public int OffTankShieldPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("UsePsychicScream")]
        [Description("Use Psychic scream if enemy in range")]
        public bool UsePsychicScream { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("Do not Attack CrowdControlled enemy")]
        [Description("Do not Attack CrowdControlled enemy NB: may slow performance!")]
        public bool AvoidCC { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("UseVoidTendrils")]
        [Description("Use Void Tendrils if enemy in range")]
        public bool UseVoidTendrils { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("UsePsyfiend")]
        [Description("Use Psyfiend if enemy in range")]
        public bool UsePsyfiend { get; set; }

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
        
       
        [Setting]
        [DefaultValue(true)]
        [Category("Settings PVP")]
        [DisplayName("UseDispellMagic")]
        [Description("Use UseDispellMagic on current target: NB no logic implemented, so CC will dispell all dispellable aura")]
        public bool UseDispellMagic { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings PVP")]
        [DisplayName("Use SW: Death")]
        [Description("Use SW: Death in PVP rotation")]
        public bool UseSWD { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings PVP")]
        [DisplayName("Use SW: Pain")]
        [Description("Use SW: Pain in PVP rotation")]
        public bool UseSWP { get; set; }
        
        

    }
}