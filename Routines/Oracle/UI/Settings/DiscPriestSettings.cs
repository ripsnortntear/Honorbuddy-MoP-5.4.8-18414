#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-17 20:34:00 +1000 (Tue, 17 Sep 2013) $
 * $ID$
 * $Revision: 229 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/DiscPriestSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class DiscPriestSettings : Styx.Helpers.Settings
    {
        public DiscPriestSettings()
            : base(OracleSettings.SettingsPath + "_DiscPriest.xml")
        {
        }

        #region Category: Common

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Binding Heal")]
        [Description("Your Health percent must be below this setting before binding heal will be used.")]
        public int BindingHealPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Mana")]
        [DisplayName("Shadow Fiend / Mind Bender")]
        [Description("Mana Percent to use Shadow Fiend / Mind Bender")]
        public int ShadowfiendPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Mana")]
        [DisplayName("Hymn of Hope")]
        [Description("Mana Percent to use Hymn of Hope")]
        public int HymnofHopePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Movement Buff")]
        [Description(
            "When this is set to true, Oracle will buff us with Angelic Feather or PW:S (Body and Soul) as soon as we a re moving."
            )]
        public bool EnablePriestMovementBuff { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Power Word: Fortitude")]
        [Description("When this is set to true, Oracle will buff us with Power Word: Fortitude")]
        public bool UsePowerWordFortitude { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(100)]
        [Category("Common")]
        [DisplayName("Prayer Of Mending")]
        [Description("Health Percent to use Prayer Of Mending")]
        public int PrayerOfMendingPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Flash Heal SoL")]
        [Description("Health Percent to use Flash Heal with Surge of Light proc")]
        public int FlashHealSoLPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Fade")]
        [Description("When this is set to true, Pure will automaticly fade when Threat during combat")]
        public bool UseFade { get; set; }



        #endregion Category: Common

        #region Category: Discipline

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Discipline")]
        [DisplayName("Enable Offensive DPS")]
        [Description("Enables offensive DPS in the rotation. Smite, Holy Fire, Penance, etc")]
        public bool EnableOffensiveDps { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Discipline")]
        [Description("Enables the use of Penance on Enemy mobs and bosses")]
        public bool UsePenanceOnEnemy { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Discipline")]
        [DisplayName("Pain Suppression")]
        [Description("Health Percent to apply Pain Suppression to the tank.")]
        public int PainSuppressionPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Discipline")]
        [DisplayName("Greater Heal IF")]
        [Description("Health Percent to use Greater Heal with Inner Focus")]
        public int GreaterHealPct { get; set; }

        #endregion Category: Discipline

        #region Category: AoE Healing Spells

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("AoE Healing Spells")]
        [DisplayName("Divine Star")]
        [Description(
            "Average health percent to use Divine Star. Players must be below this amount to be included in the average."
            )]
        public int DivineStarPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Divine Star")]
        [Description("Number of players that have the average health percent specified above")]
        public int DivineStarCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("AoE Healing Spells")]
        [DisplayName("Cascade")]
        [Description(
            "Average health percent to use Cascade. Players must be below this amount to be included in the average.")]
        public int CascadePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Cascade")]
        [Description("Number of players that have the average health percent specified above")]
        public int CascadeCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("AoE Healing Spells")]
        [DisplayName("Halo")]
        [Description(
            "Average health percent to use Halo. Players must be below this amount to be included in the average.")]
        public int HaloPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Halo")]
        [Description("Number of players that have the average health percent specified above")]
        public int HaloCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(87)]
        [Category("AoE Healing Spells")]
        [DisplayName("Prayer of Healing")]
        [Description(
            "Average health percent to use Prayer of Healing. Players must be below this amount to be included in the average."
            )]
        public int PrayerofHealingPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Prayer of Healing")]
        [Description("Number of players that have the average health percent specified above")]
        public int PrayerofHealingCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(88)]
        [Category("AoE Healing Spells")]
        [DisplayName("Power Word: Barrier")]
        [Description(
            "Average health percent to use Power Word: Barrier. Players must be below this amount to be included in the average."
            )]
        public int PowerWordBarrierPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("AoE Healing Spells")]
        [DisplayName("Power Word: Barrier")]
        [Description("Number of players that have the average health percent specified above")]
        public int PowerWordBarrierCount { get; set; }

        #endregion Category: AoE Healing Spells

        #region Category: Emergency Cooldowns

        //Oh Shit Moments!

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Desperate Prayer")]
        [Description("Health Percent to use Desperate Prayer")]
        public int DesperatePrayerPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Spirit Shell")]
        [Description(
            "Average health percent to use Spirit Shell. Players must be below this amount to be included in the average."
            )]
        public int SpiritShellPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(10)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Spirit Shell")]
        [Description("Number of players that have the average health percent specified above")]
        public int SpiritShellCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Archangel")]
        [Description(
            "Average health percent to use Archangel. Players must be below this amount to be included in the average.")
        ]
        public int ArchangelPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Archangel")]
        [Description("Number of players that have the average health percent specified above")]
        public int ArchangelCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Power Infusion")]
        [Description(
            "Average health percent to use Power Infusion. Players must be below this amount to be included in the average."
            )]
        public int PowerInfusionPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Power Infusion")]
        [Description("Number of players that have the average health percent specified above")]
        public int PowerInfusionCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Void Shift")]
        [Description("Health Percent to use Void Shift")]
        public int VoidShiftPct { get; set; }

        // we ignore all settings and start healing like hell!!!

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Urgent Health Percentage")]
        [Description("we ignore all settings and start healing like hell!!!")]
        public int UrgentHealthPercentage { get; set; }

        #endregion Category: Emergency Cooldowns

        #region Category: Enable Dynamic Spells (Single Heal).



        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Dynamic Spells")]
        [DisplayName("Heal")]
        [Description("Include Heal in the Dynamic Spell Priority when targets Healthpercent is below this setting")]
        public int HealDSPPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Dynamic Spells")]
        [DisplayName("Flash Heal")]
        [Description("Include Flash Heal in the Dynamic Spell Priority when targets Healthpercent is below this setting"
            )]
        public int FlashHealDSPPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Dynamic Spells")]
        [DisplayName("Greater Heal")]
        [Description("When enabled Greater Heal will be included in the Dynamic Spell Priority single target list")]
        public bool GreaterHealPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Heal")]
        [Description("When enabled Heal will be included in the Dynamic Spell Priority single target list")]
        public bool HealPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("FlashHeal")]
        [Description("When enabled FlashHeal will be included in the Dynamic Spell Priority single target list")]
        public bool FlashHealPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Renew")]
        [Description("When enabled Renew will be included in the Dynamic Spell Priority single target list")]
        public bool RenewPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Smite")]
        [Description("When enabled Smite will be included in the Dynamic Spell Priority single target list")]
        public bool SmitePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Power Word: Solace")]
        [Description("When enabled Power Word: Solace will be included in the Dynamic Spell Priority single target list")]
        public bool PowerWordSolacePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Holy Fire")]
        [Description("When enabled Holy Fire will be included in the Dynamic Spell Priority single target list")]
        public bool HolyFirePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Binding Heal")]
        [Description("When enabled Binding Heal will be included in the Dynamic Spell Priority single target list")]
        public bool BindingHealPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Penance")]
        [Description("When enabled Penance will be included in the Dynamic Spell Priority single target list")]
        public bool PenancePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Power Word: Shield")]
        [Description("When enabled Power Word: Shield will be included in the Dynamic Spell Priority single target list")]
        public bool PowerWordShieldPrioEnabled { get; set; }

         [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Hyme of Hope")]
        [Description("Enable to use Hyme of Hope.")]
        public bool EnableHymeofHope { get; set; }


        #endregion

        #region Category: Enable Dynamic Spells (AoE Heal).

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Power Word: Barrier")]
        [Description("When enabled Power Word: Barrier will be included in the Dynamic Spell Priority AoE list")]
        public bool PowerWordBarrierPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Divine Star")]
        [Description("When enabled Divine Star will be included in the Dynamic Spell Priority AoE list")]
        public bool DivineStarPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Cascade")]
        [Description("When enabled Cascade will be included in the Dynamic Spell Priority AoE list")]
        public bool CascadePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Halo")]
        [Description("When enabled Halo will be included in the Dynamic Spell Priority AoE list")]
        public bool HaloPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Prayer Of Healing")]
        [Description("When enabled Prayer Of Healing will be included in the Dynamic Spell Priority AoE list")]
        public bool PrayerOfHealingPrioEnabled { get; set; }

        #endregion

        #region Category: Enable Dynamic Spells (Cooldown Spells).

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("SpiritShell")]
        [Description("When enabled SpiritShell will be included in the Dynamic Spell Priority Cooldown list")]
        public bool SpiritShellPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Inner Focus")]
        [Description("When enabled Inner Focus will be included in the Dynamic Spell Priority Cooldown list")]
        public bool InnerFocusPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Archangel")]
        [Description("When enabled Archangel will be included in the Dynamic Spell Priority Cooldown list")]
        public bool ArchangelPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Power Infusion")]
        [Description("When enabled Power Infusion will be included in the Dynamic Spell Priority Cooldown list")]
        public bool PowerInfusionPrioEnabled { get; set; }



        #endregion
    }
}