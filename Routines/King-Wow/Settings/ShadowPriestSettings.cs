using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class ShadowPriestSettings : Settings
    {
        public static ShadowPriestSettings Instance = new ShadowPriestSettings();

        
        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/ShadowPriest/ShadowPriest-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public ShadowPriestSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/ShadowPriest/ShadowPriest-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
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
        [Description("if enabled Self Healing Priority: healing you until below this HP%")]
        public float SelfHealingPriorityHP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings CD")]
        [DisplayName("Use Inner Focus")]
        [Description("Use Inner Focus every CD")]
        public bool UseInnerFocus { get; set; }

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
        [DefaultValue(5)]
        [Category("Settings AOE HEALING")]
        [DisplayName("Cascade_Halo_Number")]
        [Description("Use Cascade/Halo if players below Cascade_Halo_HP >= this value")]
        public int Cascade_Halo_Number { get; set; }

        [Setting]
        [DefaultValue(65)]
        [Category("Settings AOE HEALING")]
        [DisplayName("Cascade_Halo_HP")]
        [Description("Use Cascade/Halo if players with HPbelow this value >= Cascade_Halo_Number")]
        public int Cascade_Halo_HP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Use Fade")]
        [Description("chose to use fade on aggro")]
        public bool UseFade { get; set; }
        

        [Setting]
        [DefaultValue(false)]
        [Category("Settings DISPELLS")]
        [DisplayName("Dispell ASAP ")]
        [Description("try to dispell asap avoiding normal rotation priority")]
        public bool DispellASAP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings DISPELLS")]
        [DisplayName("Dispell Only Majror")]
        [Description("dispell only player affecterd by major game note debuff: If you set this to false CC will dispell EVERYTHING")]
        public bool DispellOnlyMajor { get; set; }

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
        [Category("Settings TANK")]
        [DisplayName("Use TANK help healing")]
        [Description("enable/disable Tank help Healing")]
        public bool TankHealing { get; set; }

        [Setting]
        [DefaultValue(15)]
        [Category("Settings DISPERSION")]
        [DisplayName("Use Dispersion at hp%")]
        [Description("Use Dispersion at hp%")]
        public int DispersionHP { get; set; }

        [Setting]
        [DefaultValue(10)]
        [Category("Settings DISPERSION")]
        [DisplayName("Use Dispersion at mana%")]
        [Description("Use Dispersion at mana%")]
        public int DispersionMana { get; set; }

        [Setting]
        [DefaultValue(65)]
        [Category("Settings VAMPIRIC EMBRANCE")]
        [DisplayName("VampiricEmbranceHP")]
        [Description("Use Vampiric Embrance if my HP or VampiricEmbranceNumber players below this value")]
        public int VampiricEmbranceHP { get; set; }

        [Setting]
        [DefaultValue(5)]
        [Category("Settings VAMPIRIC EMBRANCE")]
        [DisplayName("VampiricEmbranceNumber")]
        [Description("Use Vampiric Embrance players with HP <= VampiricEmbranceHP equal or greater this value")]
        public int VampiricEmbranceNumber { get; set; }
        
        [Setting]
        [DefaultValue(90)]
        [Category("Settings MANA REGENERATION")]
        [DisplayName("ShadowFiend/Mindbender mana%")]
        [Description("Cast ShadowFiend/Mindbender at % mana")]
        public int ShadowFiendPercent { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings TANK")]
        [DisplayName("PW: Shield on Tank hp%")]
        [Description("Cast PW: Shield on Tank at % hp set to 100 for always up shield on tank")]
        public int TankShieldPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings TANK")]
        [DisplayName("Mantain Renew on Tank")]
        [Description("Mantain Renew on main Tank")]
        public bool RenewOnTank { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings SHIELDS")]
        [DisplayName("PW: Shield hp%")]
        [Description("Cast PW: Shield on Player at % hp")]
        public int PWShieldPercent { get; set; }

        //HEALING TUNING
        [Setting]
        [DefaultValue(50)]
        [Category("Settings HEAL TUNING")]
        [DisplayName("Greater Heal hp%")]
        [Description("Cast Greater Heal at target % hp")]
        public int GHPercent { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Settings HEAL TUNING")]
        [DisplayName("Flash Heal hp%")]
        [Description("Cast Flash Heal at target % hp")]
        public int FlashHealPercent { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Settings HEAL TUNING")]
        [DisplayName("Desperate Prayer hp%")]
        [Description("Cast Desperate Prayer if we lower than % hp")]
        public int DesperatePrayerPercent { get; set; }

        [Setting]
        [DefaultValue(75)]
        [Category("Settings HEAL TUNING")]
        [DisplayName("Renew hp%")]
        [Description("Cast Renew at hp%")]
        public int RenewPercent { get; set; }

        public enum PowerInfusionUseType
        {
            COOLDOWN,
            BOSS,
            MANUAL
        }

        [Setting]
        [DefaultValue(PowerInfusionUseType.BOSS)]
        [Category("Settings CD")]
        [DisplayName("Power Infusion use")]
        [Description("Cast Power Infusion on BOSS on COOLDOWN or MANUAL type")]
        public PowerInfusionUseType PowerInfusionUse { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings AOE")]
        [DisplayName("Cascade/halo/DivineStar enemies number")]
        [Description("Cast Cascade/halo/DivineStar if player lower than CascadeHaloDivinestarPercent >= this value")]
        public int CascadeHaloDivineStarNumber { get; set; }

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
        [Category("Settings VOID SHIFT")]
        [DisplayName("UseVoidShift")]
        [Description("enable/Disable Void Shift ")]
        public bool UseVoidShift { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TANK")]
        [DisplayName("UseVoidShiftOnTank")]
        [Description("enable/Disable cast Void Shift only on tank")]
        public bool UseVoidShiftOnTank { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TANK")]
        [DisplayName("UseVoidShiftOnOffTank")]
        [Description("enable/Disable cast Void Shift on offtank too")]
        public bool UseVoidShiftOnOffTank { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings TANK")]
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

        public enum TargetType
        {
            MANUAL,
            SEMIAUTO,
            AUTO
        }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TARGETTING")]
        [DisplayName("AutoFacingTarget")]
        [Description("Enable/Disable autofacing current target")]
        public bool AutofaceTarget { get; set; }

        [Setting]
        [DefaultValue(TargetType.AUTO)]
        [Category("Settings TARGETTING")]
        [DisplayName("TargetTypeSelected")]
        [Description("AUTO/MANUAL enable/disable autotargeting SEMIAUTO: same logic of AUTO but no switch selected target")]
        public TargetType TargetTypeSelected { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Multidot_SW_Pain_EnemyNumberMin")]
        [Description("Enable Multidot of ShadowWord: Pain rotation if Enemy in range withouut this dot >= this value")]
        public int Multidot_SW_Pain_EnemyNumberMin { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Multidot_SW_Pain_EnemyNumberMax")]
        [Description("Disable Multidot rotation if dotted Enemy with ShadowWors: Pain> this value")]
        public int Multidot_SW_Pain_EnemyNumberMax { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Multidot_VampiricTouch_EnemyNumberMin")]
        [Description("Enable Multidot of VampiricTouch rotation if Enemy in range withouut this dot >= this value")]
        public int Multidot_VampiricTouch_EnemyNumberMin { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Multidot_VampiricTouch_EnemyNumberMax")]
        [Description("Disable Multidot rotation if dotted Enemy with VampiricTouch > this value")]
        public int Multidot_VampiricTouch_EnemyNumberMax { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Enable Multidot Rotation")]
        [Description("Enable/Disable use of Multidot rotation")]
        public bool MultidotEnabled { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Avoid Crowd Controlled in Multidot Rotation")]
        [Description("Avoid Crowd Controlled in Multidot Rotation")]
        public bool MultidotAvoidCC { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings INTERRUPT")]
        [DisplayName("autointerrupt")]
        [Description("Cast counterspell if target casting and interrumpible")]
        public bool AutoInterrupt { get; set; }

        [Setting]
        [DefaultValue(5)]
        [Category("Settings AOE")]
        [DisplayName("Use Mind Sear EnemyNumberMin")]
        [Description("Use Mind Sear if enemies around Tank or OffTank > this value")]
        public int MindSearEnemyNumberMin { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("Use Mind Sear")]
        [Description("Enable/Disable use of MindSear")]
        public bool UseMindSear { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("MindSearAlsoOnEnemies")]
        [Description("use of MindSear olso on current enemy target if condition verified")]
        public bool MindSearAlsoOnEnemies { get; set; }

        [Setting]
        [DefaultValue(1000)]
        [Category("Settings SPECIAL")]
        [DisplayName("mindFlayDuration")]
        [Description("Set mindFlayDuration - 250 ms")]
        public int mindFlayDuration { get; set; }
        
        [Setting]
        [DefaultValue(2000)]
        [Category("Settings SPECIAL")]
        [DisplayName("mindSearDuration")]
        [Description("Set mindSearDuration - 250 ms")]
        public int mindSearDuration { get; set; }    
        

    }
}