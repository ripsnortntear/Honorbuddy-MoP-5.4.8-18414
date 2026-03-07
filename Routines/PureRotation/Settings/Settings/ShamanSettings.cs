#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-07-17 22:03:43 +0200 (Mi, 17 Jul 2013) $
 * $ID$
 * $Revision: 1605 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/ShamanSettings.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC

using System.ComponentModel;
using Styx.Helpers;
using Styx.WoWInternals;

namespace PureRotation.Settings.Settings
{
    internal class ShamanSettings : Styx.Helpers.Settings
    {
        public ShamanSettings()
            : base(PRSettings.SettingsPath + "_Shaman.xml")
        {
        }

        #region Category : AoECount

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Common")]
        [DisplayName("AoE Add Count")]
        [Description("Will use AoE at selected enemy unit count.")]
        public int AoECount { get; set; }

        #endregion Category : AoECount

        #region Totems - Checked (29/12/2012)
        [Browsable(false)]
        [Setting]
        [Styx.Helpers.DefaultValue(WoWTotem.None)]
        [Category("Restoration Totems")]
        [DisplayName("EarthTotem")]
        [Description("The totem to use for this slot. Select 'None' for automatic usage.")]
        public WoWTotem RestorationEarthTotem
        {
            get;
            set;
        }

        [Browsable(false)]
        [Setting]
        [Styx.Helpers.DefaultValue(WoWTotem.None)]
        [Category("Restoration Totems")]
        [DisplayName("WaterTotem")]
        [Description("The totem to use for this slot. Select 'None' for automatic usage.")]
        public WoWTotem RestorationWaterTotem
        {
            get;
            set;
        }

        [Browsable(false)]
        [Setting]
        [Styx.Helpers.DefaultValue(WoWTotem.None)]
        [Category("Restoration Totems")]
        [DisplayName("AirTotem")]
        [Description("The totem to use for this slot. Select 'None' for automatic usage.")]
        public WoWTotem RestorationAirTotem
        {
            get;
            set;
        }

        #endregion Totems - Checked (29/12/2012)

        #region Enhancement - Checked (29/12/2012)

        [Styx.Helpers.DefaultValue(false)]
        [Category("Enhancement")]
        [DisplayName("Force PVP Rotation")]
        [Description("When enabled will always use the PVP rotation.")]
        public bool EnhanceForcePVP { get; set; }

        #endregion Enhancement - Checked (29/12/2012)

        #region Elemental - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Elemental")]
        [DisplayName("Percentage for Self Healing CDs")]
        [Description("Will use self healing cooldowns on " +
                     "the percentage specified.")]
        public int SelfHealingPercentage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Use Fire Elemental")]
        [Description("Will use Fire Elemental")]
        public bool UseFireElemental { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Use Earth Elemental")]
        [Description("Will use Earth Elemental (when fire is on cooldown)")]
        public bool UseEarthElemental { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Use Magma Totem")]
        [Description("Will use Magma Totem during AoE")]
        public bool UseMagmaTotem { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Use Ghost Wolf")]
        [Description("Will use Ghost Wolf when moving")]
        public bool UseGhostWolf { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Use Spiritwalker's Grace")]
        [Description("Will automatically use Spiritwalker's grace when it's required")]
        public bool UseSpiritwalkersGrace { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Only Use Ascendance on Boss")]
        [Description("When set to true, will only use Ascendance on bosses.")]
        public bool EleAscendanceOnBoss { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Use Ascendance")]
        [Description("When set to true, will use Ascendance.")]
        public bool UseAscendance { get; set; }

        #endregion Elemental - Checked (29/12/2012)

        #region Restoration - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Restoration")]
        [DisplayName("Apply Earth Shield")]
        [Description("When this is set to true, PureRotation will automaticly select the appropriate target for Earth Shield")]
        public bool HandleEarthShieldTarget
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Restoration")]
        [DisplayName("Use Ascendance")]
        [Description("Automatically use Ascendance")]
        public bool RestoUseAscendance
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Restoration")]
        [DisplayName("Use Ghost Wolf")]
        [Description("Automatically use Ghost Wolf when moving")]
        public bool RestoUseGhostWolf
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Restoration")]
        [DisplayName("Healing Stream Totem - HealthPercent")]
        [Description("Healing Stream Totem will be used at this Healthpercent")]
        public int HealStreamTotemPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Restoration")]
        [DisplayName("Greater Healing Wave - HealthPercent")]
        [Description("Greater Healing Wave will be used at this Healthpercent")]
        public int GreaterHealWavePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Restoration")]
        [DisplayName("Healing Wave - HealthPercent")]
        [Description("Healing Wave will be used at this Healthpercent")]
        public int HealWavePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Restoration")]
        [DisplayName("Riptide - HealthPercent")]
        [Description("Riptide will be used at this Healthpercent")]
        public int RiptidePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Restoration")]
        [DisplayName("Healing Surge - HealthPercent")]
        [Description("Healing Surge will be used at this Healthpercent")]
        public int HealingSurgePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Restoration")]
        [DisplayName("Astral Shift - HealthPercent")]
        [Description("Astral Shift will be used at this Healthpercent")]
        public int AstralShiftPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Restoration")]
        [DisplayName("Earth Shield Totem - HealthPercent")]
        [Description("Earth Shield Totem will be used at this Healthpercent")]
        public int EarthShieldTotemPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Restoration")]
        [DisplayName("Unleash Elements - HealthPercent")]
        [Description("Unleash Elements will be used at this Healthpercent")]
        public int UnleashElementsPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Restoration")]
        [DisplayName("Mana Tide Totem - HealthPercent")]
        [Description("Mana Tide Totem will be used at this Healthpercent")]
        public int ManaTideTotemPercent
        {
            get;
            set;
        }

        #endregion Restoration - Checked (29/12/2012)

        #region PvP
        #endregion PvP
    }
}