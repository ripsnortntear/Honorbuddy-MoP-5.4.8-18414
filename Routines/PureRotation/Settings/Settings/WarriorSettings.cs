#region Revision info

/*
 * $Author: worklifebalance $
 * $Date: 2013-09-21 16:32:20 +0200 (Sa, 21 Sep 2013) $
 * $ID$
 * $Revision: 1733 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/WarriorSettings.cs $
 * $LastChangedBy: worklifebalance $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC

using System.ComponentModel;
using PureRotation.Core;
using Styx.Helpers;
using System.Windows.Forms;

namespace PureRotation.Settings.Settings
{
    internal class WarriorSettings : Styx.Helpers.Settings
    {
        public WarriorSettings()
            : base(PRSettings.SettingsPath + "_Warrior.xml")
        {
        }

      
        #region Category : AoECount

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Common")]
        [DisplayName("AoE Add Count")]
        [Description("Will use AoE on Count number you choose.)")]
        public int AoECount { get; set; }

        #endregion Category : AoECount

        #region Common - Checked (29/12/2012)

        //[Browsable(false)]
        //[Setting]
        //[Styx.Helpers.DefaultValue(ShapeshiftForm.BattleStance)]
        //[Category("Common")]
        //[DisplayName("Warrior Stance Selector")]
        //[Description("Choose a Warrior Stance.")]
        //public ShapeshiftForm StanceSelection { get; set; }

        [Browsable(false)]
        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Healthstone Percent")]
        [Description(
            "Will use a Healthstone for self heal at this healthpercent. (Self Healing (General Tab) must be enabled as well.)"
            )]
        public int HealthstonePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Impending Victory Percent")]
        [Description("Will use Impending Victory or Victory Rush when health percent is less than or equal to the set value")]
        public int ImpendingVictoryPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Bloodbath on CD")]
        [Description("When enabled Purerotation will use Bloodbath.")]
        public bool UseBloodbath { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Recklessness on bosses")]
        [Description("When enabled Purerotation will use Recklessness")]
        public bool UseRecklessness { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Shattering Throw on bosses")]
        [Description("When enabled we will use Shattering Throw on bosses.")]
        public bool UseShatteringThrow { get; set; }

        #endregion Common - Checked (29/12/2012)

        #region Protection - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(WarriorShout.Commanding)]
        [Category("Protection")]
        [DisplayName("Warrior Shout Selector")]
        [Description("Choose Warrior Shout.")]
        public WarriorShout ProtShoutSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Protection")]
        [DisplayName("Use Spell Reflection")]
        [Description("When enabled we will use Spell Reflection")]
        public bool UseSpellReflection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Protection")]
        [DisplayName("Prot AoE Count")]
        [Description(
            "Will use AoE abillites (Thunder Clap,Cleave,ShockWave) when agro mob count is equal to or higher then this value."
            )]
        public int ProtAoECount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Protection")]
        [DisplayName("Enraged Regeneration Percent")]
        [Description("Will use Enraged Regeneration when Rage percent is greater than or equal to the set value")]
        public int ProtEnragedRegenerationRagePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Protection")]
        [DisplayName("Use Berserker Rage ")]
        [Description("When enabled Purerotation will Berserker Rage")]
        public bool UseBerserkerRage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Protection")]
        [DisplayName("Shield Wall Percent")]
        [Description("Will use Shield Wall when health percent is less than or equal to the set value")]
        public int ShieldWallPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Protection")]
        [DisplayName("Rallying Cry Percent")]
        [Description("Will use Rallying Cry when my health percent is greater than or equal to the set value")]
        public int RallyingCryPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Protection")]
        [DisplayName("Last Stand Percent")]
        [Description("Will use Last Stand  when health percent is less than or equal to the set value")]
        public int LastStandPercent { get; set; }

        #endregion Protection - Checked (29/12/2012)

        #region DPS - Checked (29/12/2012)

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("Use Damage Cooldowns")]
        //[Description("True / False if you would like the cc to use damage cooldowns")]
        //public bool UseWarriorDpsCooldowns { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("Use Interupts")]
        //[Description("True / False if you would like the cc to use Interupts")]
        //public bool UseWarriorInterupts { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("true for Battle Shout, false for Commanding")]
        //[Description("True / False if you would like the cc to use Battleshout/Commanding")]
        //public bool UseWarriorShouts { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("Slows")]
        //[Description("True / False if you would like the cc to use slows ie. Hammstring, Piercing Howl")]
        //public bool UseWarriorSlows { get; set; }

        //[Setting]
        //[DefaultValue(false)]
        //[Category("DPS")]
        //[DisplayName("Basic Rotation Only")]
        //[Description("True / False if you would like the cc to use just the basic DPS rotation only")]
        //public bool UseWarriorBasicRotation { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("Use AOE")]
        //[Description("True / False if you would like the cc to use AOE with more than 3 mobs")]
        //public bool UseWarriorAOE { get; set; }

        //[Setting]
        //[DefaultValue(false)]
        //[Category("DPS")]
        //[DisplayName("T12 2-Piece")]
        //[Description("True / False if you have the T12 2-piece set bonus")]
        //public bool UseWarriorT12 { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("Force proper stance?")]
        //[Description("True / False on whether you would like the cc to keep the toon in the proper stance for the spec. Arms:Battle, Fury:Berserker")]
        //public bool UseWarriorKeepStance { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("DPS")]
        //[DisplayName("Use Charge/Intercept/Heroic Leap?")]
        //[Description("True / False if you would like the cc to use any gap closers")]
        //public bool UseWarriorCloser { get; set; }

        #endregion DPS - Checked (29/12/2012)

        #region Arms - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Arms")]
        [DisplayName("Force PVP Rotation")]
        [Description("When enabled will always use the PVP rotation.")]
        public bool AP_ForcePVP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms")]
        [DisplayName("Use Impending Victory")]
        [Description("When enabled will use Impending Victory or Victory Rush as healing spells")]
        public bool ArmsImpVic { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Arms")]
        [DisplayName("Impending Victory Percent")]
        [Description("Will use Impending Victory or Victory Rush when health percent is less than or equal to the set value")]
        public int ArmsImpVicPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(WarriorShout.Battle)]
        [Category("Arms")]
        [DisplayName("Warrior Shout Selector")]
        [Description("Choose Warrior Shout.")]
        public WarriorShout ArmsShoutSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms")]
        [DisplayName("Use Rallying Cry")]
        [Description("When enabled will use Rallying Cry")]
        public bool ArmsRallyingCry { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms")]
        [DisplayName("Use Die by the Sword")]
        [Description("When enabled will use Die by the Sword")]
        public bool ArmsDiebytheSword { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Arms")]
        [DisplayName("Die by the Sword Percent")]
        [Description("Will use Die by the Sword when health percent is less than or equal to the set value")]
        public int ArmsDiebytheSwordPercent { get; set; }

        #endregion Arms - Checked (29/12/2012)

        #region Arms PVP

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Post Spell Landing Mechanics")]
        [Description("This will do tricky things like cast defensive stance or intervene at the same time as stuns and the like.")]
        public bool AP_CL_Mechanics { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Auto Disarm")]
        [Description("This will disarm certain classes automatically when they are bursting with CDs.")]
        public bool ArmsDisarm { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Auto Vigilance")]
        [Description("This will cast vigilance on party members at or below 25% health.")]
        public bool AP_UseVigilance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Focus Pummel")]
        [Description("This pummel your focus target if they need it.")]
        public bool ArmsFocusPummel { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Focus Charge")]
        [Description("This charge your focus target if they need it.")]
        public bool ArmsFocusCharge { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Storm Bolt Interrupt")]
        [Description("This use storm bolt as an interrupt on current target or focused unit.")]
        public bool AP_UseStormBoltInterrupt { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Stampeding Shout (Symbiosis)")]
        [Description("This will use Stampeding Shout if you are rooted or snared.")]
        public bool ArmsStampedingShout { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Arms PVP")]
        [DisplayName("Enable Auto Banner Intervene Safeguard freedom.")]
        [Description("This will place mocking or demoralizing banner on your current target if needed and or intervene or safeguard if your movement impaired.")]
        public bool AP_EnableBannerFreedom { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Use Shattering Throw.")]
        [Description("This will Shattering Throw current target if invulnerable.")]
        public bool AP_UseShatter { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Use Berserker Rage on Fear.")]
        [Description("This will cast Berserker Rage if you get feared.")]
        public bool AP_UseBerserkerRage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Arms PVP")]
        [DisplayName("Heroic Throw & Target Totems")]
        [Description("This will Heroic Throw & target certain totems. Usefule in Arena.")]
        public bool AP_EnableHeroicThrowTotem { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Auto Spell Reflection")]
        [Description("This will automatically spell reflect crowd control spells.")]
        public bool AP_EnableSpellReflect { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Auto Mass Spell Reflection")]
        [Description("This will automatically mass spell reflect crowd control spells.")]
        public bool AP_EnableMassSpellReflect { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Arms PVP")]
        [DisplayName("Enable Hamstring or Piercing Howl")]
        [Description("This will hamstring or piercing howl your current target if they are not already slowed.")]
        public bool AP_EnableHamstring { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(600)]
        [Category("Arms PVP")]
        [DisplayName("Interrupt Cast Time Left (MS)")]
        [Description("Specify the amount of time left in the cast before an interrupt is used.")]
        public int ArmsCastTimeLeft { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Arms PVP")]
        [DisplayName("Enable Stance Dance")]
        [Description("This will switch between Battle and Defensive based on units targeting you and health. Expensive setting.")]
        public bool ArmsStanceDance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.C)]
        [Category("Arms PVP")]
        [DisplayName("Heroic Leap HotKey")]
        [Description("Do not select a modifier, it will stop working. Hard coded to use the left shift key in combination with the hotkey you choose here.")]
        public Keys ArmsHLMod { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.F8)]
        [Category("Arms PVP")]
        [DisplayName("Spell Reflection HotKey")]
        [Description("Do not select a modifier, it will stop working.")]
        public Keys ArmsSRMod { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(Keys.F9)]
        [Category("Arms PVP")]
        [DisplayName("Shield Wall HotKey")]
        [Description("Do not select a modifier, it will stop working.")]
        public Keys ArmsSWMod { get; set; }

        #endregion

        #region Fury - Checked (29/12/2012)

        //[Setting]
        //[DefaultValue(false)]
        //[Category("Fury")]
        //[DisplayName("Use SMF Rotation")]
        //[Description("True / False if you would like the cc to use a SMF rotation")]
        //public bool UseWarriorSMF { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(WarriorShout.Battle)]
        [Category("Fury")]
        [DisplayName("Warrior Shout Selector")]
        [Description("Choose Warrior Shout.")]
        public WarriorShout FuryShoutSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Enables auto-attack on units")]
        [Description("When enabled PureRotation wil persistently start AutoAttack.")]
        public bool FuryAutoAttack { get; set; }

        // Demoralizing Banner
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Use Demoralizing Banner")]
        [Description("When enabled PureRotation will use Demoralizing Banner")]
        public bool FuryDemoBanner { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Fury")]
        [DisplayName("Demoralizing Banner Percent")]
        [Description("Will use Demoralizing Banner when health percent is less than or equal to the set value")]
        public int FuryDemoBannerPercent { get; set; }

        // Die by the Sword
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Use Die by the Sword")]
        [Description("When enabled PureRotation will use Die by the Sword")]
        public bool FuryDiebytheSword { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Fury")]
        [DisplayName("Last Stand Percent")]
        [Description("Will use Die by the Sword when health percent is less than or equal to the set value")]
        public int FuryDiebytheSwordPercent { get; set; }

        // Enraged Regeneration
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Use Die by the Sword")]
        [Description("When enabled PureRotation will use Enraged Regeneration")]
        public bool FuryEnragedRegen { get; set; }

        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Use Intimidating Shout")]
        [Description("When enabled PureRotation will use Intimidating Shout when Glyphed")]
        public bool FuryIntiShout { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Fury")]
        [DisplayName("Enraged Regeneration Percent")]
        [Description("Will use Enraged Regeneration when health percent is less than or equal to the set value")]
        public int FuryEnragedRegenPercent { get; set; }

        // Impending Victory
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Rotational Impending Victory")]
        [Description("When enabled PureRotation will use Impending Victory as DPS filler")]
        public bool FuryImpVicRot { get; set; }

        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Use Impending Victory")]
        [Description("When enabled PureRotation will use Impending Victory or Victory Rush as healing spells")]
        public bool FuryImpVic { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Fury")]
        [DisplayName("Impending Victory Percent")]
        [Description("Will use Impending Victory or Victory Rush when health percent is less than or equal to the set value")]
        public int FuryImpVicPercent { get; set; }

        // Rallying Cry
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Use Rallying Cry")]
        [Description("When enabled PureRotation will use Rallying Cry")]
        public bool FuryRallyingCry { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Fury")]
        [DisplayName("Rallying Cry Percent")]
        [Description("Will use Rallying Cry when health percent is less than or equal to the set value")]
        public int FuryRallyingCryPercent { get; set; }

        // Heroic Throw
        [Styx.Helpers.DefaultValue(true)]
        [Category("Fury")]
        [DisplayName("Rotational Heroic Throw")]
        [Description("When enabled PureRotation will use Heroic Throw")]
        public bool FuryHeroicThrow { get; set; }

        #endregion Fury - Checked (29/12/2012)

        #region PvP - Checked (29/12/2012)

        #endregion PvP - Checked (29/12/2012)
    }
}