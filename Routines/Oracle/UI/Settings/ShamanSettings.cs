#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/ShamanSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Classes;
using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class ShamanSettings : Styx.Helpers.Settings
    {
        public ShamanSettings()
            : base(OracleSettings.SettingsPath + "_Shaman.xml")
        {
        }

        #region Category: Common

        [Setting]
        [Styx.Helpers.DefaultValue(HandleTankBuff.Always)]
        [Category("Common")]
        [DisplayName("Earth Shield on Tank")]
        [Description(" Enable Disable Earth Shield on tank, useful when tank is not in LoS.")]
        public HandleTankBuff HandleBuffonTank { get; set; }
        
        [Setting]
        [Styx.Helpers.DefaultValue(UnitBuffSelection.None)]
        [Category("Healing Rain Overide")]
        [DisplayName("Target")]
        [Description(" When this is enabled (ie: not None) it will cast HR on cooldown. It chooses the target that you select.")]
        public UnitBuffSelection UnitBuffSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Healing Rain Overide")]
        [DisplayName("Mana Percent")]
        [Description(" Mana Percent to spam Healing Rain on target")]
        public int HealingRainOveridePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Urgent Health Percentage")]
        [Description(" we ignore all settings and start healing like hell!!!")]
        // we ignore all settings and start healing like hell!!!
        public int UrgentHealthPercentage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(99)]
        [Category("Common")]
        [DisplayName("Healing Stream Totem")]
        [Description("Health Percent to use Healing Stream Totem")]
        public int HealingStreamTotemPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Mana Tide Totem")]
        [Description("Mana Percent to use Mana Tide Totem")]
        public int ManaTideTotemPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Common")]
        [DisplayName("Lightning Bolt")]
        [Description("Mana Percent to use Lightning Bolt for Telluric Currents (must have Glyph of Telluric Currents)")]
        public int LightningBoltPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Astral Shift")]
        [Description("Health Percent to use AstralShift")]
        public int AstralShiftPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Earth Shield")]
        [Description("If enabled will apply Earth Shield to tank")]
        public bool HandleEarthShieldTarget { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Water Shield")]
        [Description("If enabled will apply Water Shield")]
        public bool EnableWaterShield { get; set; }

        
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Earth Elemental (Reinforce)")]
        [Description("If enabled will channell Reinforce (Must have Primal Elementalist Talent)")]
        public bool UseReinforcewithEarthElemental { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Healing Surge")]
        [Description("Health Percent to use Healing Surge")]
        public int HealingSurgePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Common")]
        [DisplayName("Stone Bulwark Totem")]
        [Description("Health Percent to use Stone Bulwark Totem")]
        public int StoneBulwarkTotemPercent { get; set; }

        #endregion Category: Common

        #region Category: Spec

        #endregion Category: Spec

        #region Category: AoE Healing Spells

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("AoE Healing Spells")]
        [DisplayName("Healing Rain")]
        [Description("Average health percent to use Healing Rain. Players must be below this amount to be included in the average.")]
        public int HealingRainPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("AoE Healing Spells")]
        [DisplayName("Healing Rain")]
        [Description("Number of players that have the average health percent specified above")]
        public int HealingRainLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(99)]
        [Category("AoE Healing Spells")]
        [DisplayName("Healing Rain Ascendance")]
        [Description("Average health percent to use Healing Rain with Ascendance. Players must be below this amount to be included in the average.")]
        public int HealingRainAscendancePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Healing Rain Ascendance")]
        [Description("Number of players that have the average health percent specified above")]
        public int HealingRainAscendanceLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(93)]
        [Category("AoE Healing Spells")]
        [DisplayName("Chain Heal")]
        [Description("Average health percent to use Chain Heal. Players must be below this amount to be included in the average.")]
        public int ChainHealPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Chain Heal")]
        [Description("Number of players that have the average health percent specified above")]
        public int ChainHealLimit { get; set; }

        #endregion Category: AoE Healing Spells

        #region Category: Emergency Cooldowns

        // Oh shit moments!
        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Healing Tide Totem")]
        [Description("Average health percent to use Healing Tide Totem. Players must be below this amount to be included in the average.")]
        public int HealingTideTotemPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Healing Tide Totem")]
        [Description("Number of players that have the average health percent specified above")]
        public int HealingTideTotemLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(65)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Spirit Link Totem")]
        [Description("Average health percent to use Spirit Link Totem. Players must be below this amount to be included in the average.")]
        public int SpiritLinkTotemPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Spirit Link Totem")]
        [Description("Number of players that have the average health percent specified above")]
        public int SpiritLinkTotemLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(65)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Earth Elemental Totem")]
        [Description("Average health percent to use Earth Elemental Totem. Players must be below this amount to be included in the average. Reccomended to have this the same as Ascendance.")]
        public int EarthElementalTotemPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Earth Elemental Totem")]
        [Description("Number of players that have the average health percent specified above.  Reccomended to have this the same as Ascendance.")]
        public int EarthElementalTotemLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(65)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Ascendance")]
        [Description("Average health percent to use Ascendance. Players must be below this amount to be included in the average.")]
        public int AscendancePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Ascendance")]
        [Description("Number of players that have the average health percent specified above")]
        public int AscendanceLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Spirit Walkers Grace")]
        [Description("Average health percent to use Spirit Walkers Grace. Players must be below this amount to be included in the average.")]
        public int SpiritwalkersGracePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Spirit Walkers Grace")]
        [Description("Number of players that have the average health percent specified above")]
        public int SpiritwalkersGraceLimit { get; set; }

        #endregion Category: Emergency Cooldowns

        #region Category: Enable Dynamic Spells (Single Heal).

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Dynamic Spells")]
        [DisplayName("Healing Surge")]
        [Description("When enabled Healing Surge will be included in the Dynamic Spell Priority single target list")]
        public bool HealingSurgePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Healing Wave")]
        [Description("When enabled Healing Wave will be included in the Dynamic Spell Priority single target list")]
        public bool HealingWavePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Greater Healing Wave")]
        [Description("When enabled Greater Healing Wave will be included in the Dynamic Spell Priority single target list")]
        public bool GreaterHealingWavePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Riptide")]
        [Description("When enabled Riptide will be included in the Dynamic Spell Priority single target list")]
        public bool RiptidePrioEnabled { get; set; }


        #endregion
    }
}