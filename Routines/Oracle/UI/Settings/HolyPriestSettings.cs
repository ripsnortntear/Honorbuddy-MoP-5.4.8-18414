#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-17 12:14:14 +1000 (Tue, 17 Sep 2013) $
 * $ID$
 * $Revision: 227 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/HolyPriestSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class HolyPriestSettings : Styx.Helpers.Settings
    {
        public HolyPriestSettings()
            : base(OracleSettings.SettingsPath + "_HolyPriest.xml")
        {
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Lightwell")]
        [DisplayName("Lightwell Usage")]
        [Description("When enabled Oracle will automaticly place Lightwell at the Tanks Location. Disable for elegon!")]
        public bool EnableLightwellUsage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Lightwell")]
        [DisplayName("Lightwell Distance")]
        [Description("Maximum Distance away from us until we place a new Lightwell.")]
        public int MaxLightwellDistance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(180)]
        [Category("Lightwell")]
        [DisplayName("Lightwell Check")]
        [Description("Maximum time in seconds to elapse before we check Lightwell placement")]
        public int LightwellWaitTime { get; set; }

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
        [Description("When this is set to true, Oracle will buff us with Angelic Feather or PW:S (Body and Soul) as soon as we a re moving.")]
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
        [Styx.Helpers.DefaultValue(35)]
        [Category("Common")]
        [DisplayName("Guardian Spirit")]
        [Description("Health Percent to use Guardian Spirit on tank")]
        public int GuardianSpiritPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Fade")]
        [Description("When this is set to true, Pure will automaticly fade when Threat during combat")]
        public bool UseFade { get; set; }

        #endregion Category: Common

        #region Category: AoE Healing Spells

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("AoE Healing Spells")]
        [DisplayName("Divine Star")]
        [Description("Average health percent to use Divine Star. Players must be below this amount to be included in the average.")]
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
        [Description("Average health percent to use Cascade. Players must be below this amount to be included in the average.")]
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
        [Description("Average health percent to use Halo. Players must be below this amount to be included in the average.")]
        public int HaloPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Halo")]
        [Description("Number of players that have the average health percent specified above")]
        public int HaloCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(86)]
        [Category("AoE Healing Spells")]
        [DisplayName("Prayer of Healing")]
        [Description("Average health percent to use Prayer of Healing. Players must be below this amount to be included in the average.")]
        public int PrayerofHealingPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Prayer of Healing")]
        [Description("Number of players that have the average health percent specified above")]
        public int PrayerofHealingCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("AoE Healing Spells")]
        [DisplayName("Divine Hymn")]
        [Description("Average health percent to use Divine Hymn. Players must be below this amount to be included in the average.")]
        public int DivineHymnPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(8)]
        [Category("AoE Healing Spells")]
        [DisplayName("Divine Hymn")]
        [Description("Number of players that have the average health percent specified above")]
        public int DivineHymnCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(96)]
        [Category("AoE Healing Spells")]
        [DisplayName("Circle Of Healing")]
        [Description("Average health percent to use Circle Of Healing. Players must be below this amount to be included in the average.")]
        public int CircleOfHealingPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("AoE Healing Spells")]
        [DisplayName("Circle Of Healing")]
        [Description("Number of players that have the average health percent specified above")]
        public int CircleOfHealingCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Word: Sanctuary")]
        [Description("Average health percent to use Holy Word: Sanctuary. Players must be below this amount to be included in the average.")]
        public int HolyWordSanctuaryPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Word: Sanctuary")]
        [Description("Number of players that have the average health percent specified above")]
        public int HolyWordSanctuaryCount { get; set; }

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
        [Styx.Helpers.DefaultValue(80)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Power Infusion")]
        [Description("Average health percent to use Power Infusion. Players must be below this amount to be included in the average.")]
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
        [Styx.Helpers.DefaultValue(99)]
        [Category("Dynamic Spells")]
        [DisplayName("Heal")]
        [Description("Include Heal in the Dynamic Spell Priority when targets Healthpercent is below this setting")]
        public int HealDSPPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(99)]
        [Category("Dynamic Spells")]
        [DisplayName("Flash Heal")]
        [Description("Include Flash Heal in the Dynamic Spell Priority when targets Healthpercent is below this setting"
            )]
        public int FlashHealDSPPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
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
        [Styx.Helpers.DefaultValue(99)]
        [Category("Dynamic Spells")]
        [DisplayName("Renew")]
        [Description("Include Renew in the Dynamic Spell Priority when targets Healthpercent is below this setting")]
        public int RenewPrioPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Holy Word: Serenity")]
        [Description("When enabled Holy Word: Serenity will be included in the Dynamic Spell Priority single target list")]
        public bool HolyWordSerenityPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Binding Heal")]
        [Description("When enabled Binding Heal will be included in the Dynamic Spell Priority single target list")]
        public bool BindingHealPrioEnabled { get; set; }

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
        [DisplayName("Holy Word: Sanctuary")]
        [Description("When enabled Holy Word: Sanctuary will be included in the Dynamic Spell Priority AoE list")]
        public bool HolyWordSanctuaryPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Circle Of Healing")]
        [Description("When enabled Circle Of Healing will be included in the Dynamic Spell Priority AoE list")]
        public bool CircleOfHealingPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Divine Hymn")]
        [Description("When enabled Divine Hymn will be included in the Dynamic Spell Priority AoE list")]
        public bool DivineHymnPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Divine Star")]
        [Description("When enabled Divine Star will be included in the Dynamic Spell Priority AoE list")]
        public bool HolyDivineStarPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Cascade")]
        [Description("When enabled Cascade will be included in the Dynamic Spell Priority AoE list")]
        public bool HolyCascadePrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Halo")]
        [Description("When enabled Halo will be included in the Dynamic Spell Priority AoE list")]
        public bool HolyHaloPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Prayer Of Healing")]
        [Description("When enabled Prayer Of Healing will be included in the Dynamic Spell Priority AoE list")]
        public bool HolyPrayerOfHealingPrioEnabled { get; set; }

        #endregion

        #region Category: Enable Dynamic Spells (Cooldown Spells).

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Power Infusion")]
        [Description("When enabled Power Infusion will be included in the Dynamic Spell Priority Cooldown list")]
        public bool HolyPowerInfusionPrioEnabled { get; set; }



        #endregion

    }
}