#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-30 20:20:43 +0200 (Fr, 30 Aug 2013) $
 * $ID$
 * $Revision: 1705 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/DeathKnightSettings.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC
using System.ComponentModel;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class DeathKnightSettings : Styx.Helpers.Settings
    {
        public DeathKnightSettings()
            : base(PRSettings.SettingsPath + "_DeathKnight.xml")
        {
        }

        internal enum RotationSelection
        {
            PvP,
            PvETest,
            None
        }

        [Setting]
        [Styx.Helpers.DefaultValue(DeathKnightSettings.RotationSelection.PvP)]
        [Category("Rotation Selection")]
        [DisplayName("Rotation")]
        [Description("Select the Rotation to toggle too when Rotation Hotkey is pressed.")]
        public RotationSelection RotationSelections
        {
            get;
            set;
        }

        #region Category : AoECount

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Common")]
        [DisplayName("AoE Add Count")]
        [Description("Will use AoE on Count number you choose.)")]
        public int AoECount { get; set; }

        #endregion Category : AoECount

        #region Common  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Icebound Fortitude")]
        [Description("Will use Icebound Fortitude. (Self Healing must be enabled as well.)")]
        public bool UseIceboundFortitude
        {
            get;
            set;
        }        
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Death Grip")]
        [Description("Will use Death Grip when enabled and target not in melee range.")]
        public bool UseDeathGrip
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Unholy Blight)")]
        [Description("If set to true Pure will use Unholy Blight)")]
        public bool EnableUnholyBlight
        {
            get;
            set;
        }
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Anti-Magic Shell")]
        [Description("Will use Anti-Magic Shell when the current target is Channeling/Casting a harmful spell. (Self Healing (General Tab) must be enabled as well.)")]
        public bool UseAntiMagicShell
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Healthstone Percent")]
        [Description("Will use a Healthstone for self heal at this healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        public int HealthstonePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Common")]
        [DisplayName("Soulreaper Percent")]
        [Description("Will use SoulReaper at this percent")]
        public int SoulReaperHP
        {
            get;
            set;
        }
     

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Empower Rune Weapon")]
        [Description("Will use Empower Rune Weapon. Warning! - Shared setting across all specs")]
        public bool UseEmpowerRuneWeapon
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Death Strike Percent")]
        [Description("Will use Death Strike at the set Healthpercent")]
        public int DeathStrikePercentCommon
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Pet Sacrifice (Death Pact)")]
        [Description("If set to true Pure will use Death Pact.")]
        public bool UsePetSacrifice
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Pet Sacrifice (Death Pact) Percent")]
        [Description("Will use Death Pact at the set Healthpercent. ")]
        public int PetSacrificePercent
        {
            get;
            set;
        }
        #endregion Common  - Checked (29/12/2012)

        #region Blood - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Icebound Fortitude Percent")]
        [Description("Will use Icebound Fortitude at the set Health Percent. (Self Healing (General Tab) must be enabled as well.)")]
        public int BloodIceboundFortitudePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Bone Shield Defensively")]
        [Description("Will use Bone Shield Defensively during combat.")]
        public bool UseBoneShieldDefensively
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Dancing Rune Weapon")]
        [Description("Will use Dancing Rune Weapon Defensively during combat.")]
        public bool UseDancingRuneWeapon
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Blood")]
        [DisplayName("Dancing Rune Weapon Percent")]
        [Description("Will use Dancing Rune Weapon at the set Health Percent. (Self Healing (General Tab) must be enabled as well.)")]
        public int DancingRuneWeaponPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Lichborne")]
        [Description("Will use Lichborne. (Self Healing (General Tab) must be enabled as well.)")]
        public bool UseLichborne
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Lichborne Percent")]
        [Description("Will use Lichborne at this HealthPercent. (Self Healing (General Tab) must be enabled as well.)")]
        public int LichbornePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("DeathCoil with Lichborne Percent")]
        [Description("Will use DeathCoil at this HealthPercent when Lichborne is active for self heal. (Self Healing (General Tab) must be enabled as well.)")]
        public int DeathCoilHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Vampiric Blood")]
        [Description("If set to true CLU will use Vampiric Blood. (Self Healing (General Tab) must be enabled as well.)")]
        public bool UseVampiricBlood
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Vampiric Blood Percent")]
        [Description("Will use Vampiric Blood at the set Healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        public int VampiricBloodPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("RuneTap & WoTN")]
        [Description("If set to true CLU will use RuneTap when Will of the Necropolis procs. (Self Healing (General Tab) must be enabled as well.)")]
        public bool UseRuneTapWoTN
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Blood")]
        [DisplayName("RuneTap & WoTN Percent")]
        [Description("Will use RuneTap when Will of the Necropolis procs at the set Healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        public int RuneTapWoTNPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("RuneTap")]
        [Description("If set to true PureRotation will use RuneTap.")]
        public bool UseRuneTap
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Blood")]
        [DisplayName("RuneTap Percent")]
        [Description("Will use RuneTap at the set Healthpercent.")]
        public int RuneTapPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Blood")]
        [DisplayName("Death Strike for Blood Shield Percent")]
        [Description("Will use Death Strike at the set Healthpercent and at the time [Death Strike Blood Shield Time Remaining] setting specifies. eg: If set to BS health = 101 and BS remaining = 2, Bloodshield will be maintained constantly")]
        public int DeathStrikeBloodShieldPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Blood")]
        [DisplayName("Death Strike Blood Shield Time Remaining")]
        [Description("Will use Death Strike if the players bloodshield has less than the set time left and at the Healthpercent [Death Strike for Blood Shield Percent] setting specifies eg: If set to BS health = 101 and BS remaining = 2, Bloodshield will be maintained constantly")]
        public int DeathStrikeBloodShieldTimeRemaining
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Blood")]
        [DisplayName("Death Strike Percent")]
        [Description("Will use Death Strike at the set Healthpercent regardless of blood shield")]
        public int DeathStrikePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("RuneStrike RunicPower Percent")]
        [Description("Will use RuneStrike when RunicPower Percent is greater thant the set value")]
        public int RuneStrikePercent
        {
            get;
            set;
        }

        #endregion Blood - Checked (29/12/2012)

        #region Frost  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Frost")]
        [DisplayName("Icebound Fortitude Percent")]
        [Description("Will use Icebound Fortitude at the set Health Percent. (Self Healing (General Tab) must be enabled as well.)")]
        public int FrostIceboundFortitudePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Frost")]
        [DisplayName("Anti Magic Shell Percent")]
        [Description("Will use Anti Magic Shell at the set Health Percent. (Self Healing (General Tab) must be enabled as well.)")]
        public int AntiMagicShellvalue
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Deaths Advance")]
        [Description("If set to true Pure will use Death Advance.)")]
        public bool EnableDeathsAdvance
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Anti Magic Shield On Health")]
        [Description("If set to true Pure will use Anti Magic Shell on Health Percent (Self Healing (General Tab) must be enabled as well.)")]
        public bool AntiMagicShellOnHealth
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Pillar of Frost Cooldown")]
        [Description("If set to true Pure will use Pillar of Frost on Cooldown)")]
        public bool UsePillarOfFrostOnCooldown
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Pillar of Frost Boss")]
        [Description("If set to true Pure will use Pillar of Frost on Boss")]
        public bool UsePillarOfFrostOnBoss
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Raise Dead Both")]
        [Description("If set to true Pure will use Raise Dead on both")]
        public bool RaiseDeadBoth
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Raise Dead Pillar")]
        [Description("If set to true Pure will use Raise Dead on Pillar of Frost only")]
        public bool RaiseDeadPillar
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Raise Dead Unholy")]
        [Description("If set to true Pure will use Raise Dead on Unholy buff only")]
        public bool RaiseDeadUnholy
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Empower Runic Weapon Cooldown)")]
        [Description("If set to true Pure will use Empower Rune Weapon on Cooldown")]
        public bool EmpowerOnCooldown
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Empower Runic Power Boss")]
        [Description("If set to true Pure will use Empower Runic Weapon on Boss")]
        public bool EmpowerOnBoss
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Dark Succor")]
        [Description("If set to true Pure will use Dark Succor proc")]
        public bool EnableDarkSuccor
        {
            get;
            set;
        }

        #endregion Frost  - Checked (29/12/2012)

        #region Unholy
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Unholy")]
        [DisplayName("Use Gargoyle in AoE")]
        [Description("If set to true Pure will use Gargoyle Cooldown in AoE.")]
        public bool UseGargoyleAoE
        {
            get;
            set;
        }

        #endregion Unholy
    }
}