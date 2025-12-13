#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/PaladinSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Healing;
using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class PaladinSettings : Styx.Helpers.Settings
    {
        public PaladinSettings()
            : base(OracleSettings.SettingsPath + "_Paladin.xml")
        {
        }

        #region Category: Common


        [Setting]
        [Styx.Helpers.DefaultValue(BeaconUnitSelection.Automatic)]
        [Category("Common")]
        [DisplayName("Beacon Unit Selection")]
        [Description("Choose how beacon of light should be handled.")]
        public BeaconUnitSelection BeaconUnitSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Common")]
        [DisplayName("Divine Plea")]
        [Description("Mana Percent to use Divine Plea")]
        public int DivinePleaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Eternal Flame")]
        [DisplayName("EF Tank @ Holy Power")]
        [Description("Cast Eternal Flame on the Tank at this Holy Power")]
        public int EternalFlameTankHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(1)]
        [Category("Eternal Flame")]
        [DisplayName("EF Player @ Holy Power")]
        [Description("Cast Eternal Flame on a Player at this holy power")]
        public int EternalFlamePlayerHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Eternal Flame")]
        [DisplayName("EF Blanket Mode Count")]
        [Description("Amount of players to blanket with Eternal Flame when not doing anything")]
        public int EternalFlameBlanketCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("Eternal Flame")]
        [DisplayName("EF Blanket Mode HP")]
        [Description("Eternal Flame on a Player when Holy Power is greater than or equal too this settings")]
        public int EternalFlameBlanketHPCount { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Divine Light IoL")]
        [Description("Health Percent to use Divine Light with Infusion of Light")]
        public int DivineLightPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Common")]
        [DisplayName("Flash of Light")]
        [Description("Health Percent to use Flash of Light on tank")]
        public int FlashofLightTankPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Common")]
        [DisplayName("Hand Of Protection")]
        [Description("Health Percent to use Hand Of Protection")]
        public int HandOfProtectionPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(55)]
        [Category("Common")]
        [DisplayName("Hand Of Salvation")]
        [Description("Health Percent to use Hand Of Salvation")]
        public int HandOfSalvationPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Common")]
        [DisplayName("Hand of Purity")]
        [Description("Health Percent to use Hand of Purity")]
        public int HandofPurityPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Common")]
        [DisplayName("Hand Of Sacrifice")]
        [Description("Health Percent to use Hand Of Sacrifice")]
        public int HandOfSacrificePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Damage")]
        [DisplayName("Crusader Strike")]
        [Description("Will cast for holy power, it drains your mana, doesn't generate.  Only good on fights you can stand in melee and need a lot of EF")]
        public bool UseCrusaderStrike { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Hand Of Freedom")]
        [Description("When enabled Oracle will cast Hand Of Freedom on you if Rooted or Slowed.")]
        public bool UseHandOfFreedom { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Common")]
        [DisplayName("Lay On Hands")]
        [Description("Health Percent to use Lay On Hands on the Tank!!")]
        public int LayOnHandsTankPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Common")]
        [DisplayName("Lay On Hands")]
        [Description("Health Percent to use Lay On Hands on Heal Target")]
        public int LayOnHandsHealTargetPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Common")]
        [DisplayName("Illuminated Healing")]
        [Description("Mana Percent to use Divine Light to build Illuminated Healing and  holy power on beaconed unit")]
        public int BuildIlluminatedHealingManaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Common")]
        [DisplayName("Guardian of Ancient Kings")]
        [Description("Health Percent to use Guardian of Ancient Kings to save single healtarget or Tank")]
        public int GuardianofAncientKingsPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Divine Protection")]
        [Description("Health Percent to use Divine Protection on ourselves")]
        public int DivineProtectionPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Common")]
        [DisplayName("Execution Sentence")]
        [Description("Health Percent of the tank to use Execution Sentence")]
        public int ExecutionSentencePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Divine Shield")]
        [Description("Health Percent to use Divine Shield on ourselves")]
        public int DivineShieldPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Common")]
        [DisplayName("Urgent Health Percentage")]
        [Description("we ignore all settings and start healing like hell!!!")]
        // we ignore all settings and start healing like hell!!!
        public int UrgentHealthPercentage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Low Mana Pct")]
        [Description("Low Mana setting (not used yet)")]
        public int LowManaPct { get; set; }

        #endregion Category: Common

        #region Category: Spec

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Dynamic Spell Priority")]
        [DisplayName("Holy Light")]
        [Description("Include Holy Light in the Dynamic Spell Priority when targets Healthpercent is below this setting")]
        public int HolyLightDSPPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Dynamic Spell Priority")]
        [DisplayName("Flash of Light")]
        [Description("Include Flash of Light in the Dynamic Spell Priority when targets Healthpercent is below this setting")]
        public int FlashofLightDSPPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Dynamic Spell Priority")]
        [DisplayName("Divine Light")]
        [Description("Include Divine Light in the Dynamic Spell Priority when targets Healthpercent is below this setting")]
        public int DivineLightDSPPct { get; set; }


        #endregion Category: Spec

        #region Category: AoE Healing Spells

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Prism")]
        [Description("When enabled Oracle will cast Holy Prism for Single target healing as well as AoE")]
        public bool UseHolyPrismSingleTarget { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Prism")]
        [Description("Average health percent to use Holy Prism. Players must be below this amount to be included in the average.")]
        public int HolyPrismPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Prism")]
        [Description("Number of players that have the average health percent specified above")]
        public int HolyPrismLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("AoE Healing Spells")]
        [DisplayName("Lights Hammer")]
        [Description("Average health percent to use Lights Hammer. Players must be below this amount to be included in the average.")]
        public int LightsHammerPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Lights Hammer")]
        [Description("Number of players that have the average health percent specified above")]
        public int LightsHammerLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Radiance")]
        [Description("Average health percent to use Holy Radiance. Players must be below this amount to be included in the average.")]
        public int HolyRadiancePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Radiance")]
        [Description("Number of players that have the average health percent specified above")]
        public int HolyRadianceLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(98)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Radiance (SH)")]
        [Description("Average health percent to use Holy Radiance WITH 3 stacks of Selfless healer. Players must be below this amount to be included in the average.")]
        public int HolyRadianceSHPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("AoE Healing Spells")]
        [DisplayName("Holy Radiance (SH)")]
        [Description("Number of players that have the average health percent specified above")]
        public int HolyRadianceSHLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("AoE Healing Spells")]
        [DisplayName("Light of Dawn")]
        [Description("Average health percent to use Light of Dawn. Players must be below this amount to be included in the average.")]
        public int LightofDawnPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Light of Dawn")]
        [Description("Number of players that have the average health percent specified above")]
        public int LightofDawnLimit { get; set; }

        #endregion Category: AoE Healing Spells

        #region Category: Emergency Cooldowns

        // Oh shit moments!
        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Divine Favor")]
        [Description("Average health percent to use Divine Favor. Players must be below this amount to be included in the average.")]
        public int DivineFavorPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Divine Favor")]
        [Description("Number of players that have the average health percent specified above")]
        public int DivineFavorLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Avenging Wrath")]
        [Description("Average health percent to use Avenging Wrath. Players must be below this amount to be included in the average.")]
        public int AvengingWrathPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Avenging Wrath")]
        [Description("Number of players that have the average health percent specified above")]
        public int AvengingWrathLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Devotion Aura")]
        [Description("Average health percent to use Devotion Aura. Players must be below this amount to be included in the average.")]
        public int DevotionAuraPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Devotion Aura")]
        [Description("Number of players that have the average health percent specified above")]
        public int DevotionAuraLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Guardian of Ancient Kings")]
        [Description("Average health percent to use Guardian of Ancient Kings. Players must be below this amount to be included in the average.")]
        public int GuardianofAncientKingsAoEPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Guardian of Ancient Kings")]
        [Description("Number of players that have the average health percent specified above")]
        public int GuardianofAncientKingsAoELimit { get; set; }

        #endregion Category: Emergency Cooldowns
    }
}