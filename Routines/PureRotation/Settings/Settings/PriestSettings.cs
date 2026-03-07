#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-06-24 08:52:50 +0200 (Mo, 24 Jun 2013) $
 * $ID$
 * $Revision: 1563 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/PriestSettings.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC

using System.ComponentModel;
using PureRotation.Core;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class PriestSettings : Styx.Helpers.Settings
    {
        public PriestSettings()
            : base(PRSettings.SettingsPath + "_Priest.xml")
        {
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
        [DisplayName("Fade")]
        [Description("When this is set to true, Pure will automaticly fade when Threat during combat")]
        public bool UseFade
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Power Word: Fortitude")]
        [Description("Use Power Word: Fortitude buff")]
        public bool UsePowerWordFortitude
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Healthstone Percent")]
        [Description("Will use a Healthstone for self heal at this healthpercent. ")]
        public int HealthstonePercent
        {
            get;
            set;
        }

        //[Setting]
        //[Styx.Helpers.DefaultValue(40)]
        //[Category("Common")]
        //[DisplayName("Healthstone HP")]
        //[Description("Healthstone will be consumed at this Healthpercent")]
        //public int UseHealthstone
        //{
        //    get;
        //    set;
        //}

        #endregion Common  - Checked (29/12/2012)

        #region Common - Mana Regen  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Shadowfiend Mana")]
        [Description("Shadowfiend will be used at this value")]
        public int ShadowfiendMana
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Common")]
        [DisplayName("Hymn of Hope Mana")]
        [Description("Hymn of Hope will be used at this value (UseCooldowns needs to be enabled)")]
        public int HymnofHopeMana
        {
            get;
            set;
        }

        #endregion Common - Mana Regen  - Checked (29/12/2012)

        #region Shadow  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(MultiDoTMethod.AroundTarget)]
        [Category("Shadow")]
        [DisplayName("Multi-DoT: Method")]
        [Description("AroundTarget: Will DoT all units near your target. EverythingAroundMe will DoT everything that is attacking you or a party member. None will disable multi-dotting.")]
        public MultiDoTMethod MultiDoTStyle
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Shadow")]
        [DisplayName("Multi-DoT: Max Units to DoT VT")]
        [Description("Maximum number of units to DoT with Vampiric Touch")]
        public int ShadowMaxUnitsToMultiDoTVT
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Shadow")]
        [DisplayName("Multi-DoT: Mana Percent Before VT")]
        [Description("Mana percent before multi-dotting with Vampiric Touch")]
        public int ShadowManaPercentToMultiDoTVT
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(8)]
        [Category("Shadow")]
        [DisplayName("Multi-DoT: Max Units to DoT SW:P")]
        [Description("Maximum number of units to DoT with SW:P")]
        public int ShadowMaxUnitsToMultiDoTSWP
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Shadow")]
        [DisplayName("Vampiric Embrace Percent")]
        [Description("Will cast VE when your HP is below this value")]
        public int ShadowVampiricEmbracePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Shadow")]
        [DisplayName("Power Word: Shield HP")]
        [Description("Power Word: Shield will be used at this Healthpercent")]
        public int ShadowPowerWordShieldPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Shadow")]
        [DisplayName("Dispersion %")]
        [Description("Dispersion will be used at this Mana Percent")]
        public int ShadowDispersionPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Shadow")]
        [DisplayName("Use Void Shift")]
        [Description("Will use Void Shift defensively. *WARNING* this could cast on a tank/healer and cause them to die.")]
        public bool ShadowUseVoidShift
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Shadow")]
        [DisplayName("Use Halo")]
        [Description("Will use Halo when your range is >= 23 and <= 29 yards")]
        public bool ShadowUseHalo
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Shadow")]
        [DisplayName("Mind Sear Count")]
        [Description("Mind Sear will be used on mobs greater or equal to this value. Less than this value will default back to single target spells (i.e. mind flay).")]
        public int MindSearCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Shadow")]
        [DisplayName("Multi-DoT: Use in Single Target Rotation")]
        [Description("Will multi-DoT in single target (providing mana % higher than setting, and unit count less than setting).")]
        public bool MultiDoTSingleTarget
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Shadow")]
        [DisplayName("Re-Apply DoTs when stat buff active")]
        [Description("Will DoTs when either int/mastery/spell power is increased. Will introduce lag, so disable if bad fps.")]
        public bool ReapplyDoTsWhenBuffed
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(1500)]
        [Category("Shadow")]
        [DisplayName("Re-Apply DoTs stat buff value")]
        [Description("When you have a buff which increases your stats by this value or more, will force refreshing of dots (when setting enabled to allow this behavior).")]
        public int Shadow_ReapplyValue
        {
            get;
            set;
        }

        #endregion Shadow  - Checked (29/12/2012)

        #region Discipline  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Discipline")]
        [DisplayName("Enable Offensive DPS.")]
        [Description("Enables offensive DPS in the rotation.")]
        public bool EnableOffensiveDPS
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Discipline")]
        [DisplayName("Power Word: Shield - HealthPercent")]
        [Description("Power Word: Shield will be used at this Healthpercent")]
        public int PowerWordShieldPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Discipline")]
        [DisplayName("Pain Suppression - HealthPercent")]
        [Description("Pain Suppression will be used at this Healthpercent")]
        public int PainSuppressionPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Discipline")]
        [DisplayName("Greater Heal - HealthPercent")]
        [Description("Greater Heal will be used at this Healthpercent")]
        public int GreaterHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Discipline")]
        [DisplayName("Desperate Prayer")]
        [Description("Desperate Prayer will be used at this value")]
        public int DesperatePrayerPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Discipline")]
        [DisplayName("Flash Heal")]
        [Description("Flash Heal will be used at this value")]
        public int FlashHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Discipline")]
        [DisplayName("Flash Heal - Surge of Light")]
        [Description("Flash Heal will be used at this value when Surge of Light procs")]
        public int FlashHealSoLPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Discipline")]
        [DisplayName("Heal")]
        [Description("Heal will be used at this value")]
        public int HealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Discipline")]
        [DisplayName("Void Shift")]
        [Description("Void Shift will be used when the heal target reaches this value")]
        public int VoidShiftPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("Discipline")]
        [DisplayName("Prayer of Mending")]
        [Description("Prayer of Mending will be used at this value")]
        public int PrayerofMendingPercent
        {
            get;
            set;
        }

        #endregion Discipline  - Checked (29/12/2012)

        #region Holy  - Checked (29/12/2012)

        public enum Chakra
        {
            Sanctuary,
            Serenity
        }

        [Setting]
        [Styx.Helpers.DefaultValue(Chakra.Serenity)]
        [Category("Holy")]
        [DisplayName("Chakra")]
        [Description("Which Chakra to use")]
        public Chakra HolyChakra
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Holy")]
        [DisplayName("Desperate Prayer")]
        [Description("Desperate Prayer will be used at this value")]
        public int HolyDesperatePrayerPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Holy")]
        [DisplayName("Greater Heal - HealthPercent")]
        [Description("Greater Heal will be used at this Healthpercent")]
        public int HolyGreaterHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Holy")]
        [DisplayName("Renew")]
        [Description("Renew will be used at this value")]
        public int HolyRenewHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Holy")]
        [DisplayName("Flash Heal")]
        [Description("Flash Heal will be used at this value")]
        public int HolyFlashHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Holy")]
        [DisplayName("Flash Heal - Surge of Light")]
        [Description("Flash Heal will be used at this value when Surge of Light procs")]
        public int HolyFlashHealSoLPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Holy")]
        [DisplayName("Power Word: Shield - HealthPercent")]
        [Description("Power Word: Shield will be used at this Healthpercent")]
        public int HolyPowerWordShieldPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Holy")]
        [DisplayName("Heal")]
        [Description("Heal will be used at this value")]
        public int HolyHealPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Holy")]
        [DisplayName("Void Shift")]
        [Description("Void Shift will be used when the heal target reaches this value")]
        public int HolyVoidShiftPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("Holy")]
        [DisplayName("Prayer of Mending")]
        [Description("Prayer of Mending will be used at this value")]
        public int HolyPrayerofMendingPercent
        {
            get;
            set;
        }

        //[Setting]
        //[Styx.Helpers.DefaultValue(true)]
        //[Category("Holy Spec")]
        //[DisplayName("Place Lightwell")]
        //[Description("When this is set to true, CLU will automaticly place a Lightwell on the ground during combat")]
        //public bool PlaceLightwell
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(ChakraStance.Serenity)]
        //[Category("Holy Spec")]
        //[DisplayName("Chakra Stance")]
        //[Description("Choose a Chakra Stance (Default is Serenity)")]
        //public ChakraStance ChakraStanceSelection
        //{
        //    get;    // was HandleChakra
        //    set;
        //}

        #endregion Holy  - Checked (29/12/2012)
    }
}