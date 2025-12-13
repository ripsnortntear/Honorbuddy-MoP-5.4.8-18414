#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-16 12:20:32 +1000 (Mon, 16 Sep 2013) $
 * $ID$
 * $Revision: 217 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/MonkSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class MonkSettings : Styx.Helpers.Settings
    {
        public MonkSettings()
            : base(OracleSettings.SettingsPath + "_Monk.xml")
        {
        }

        #region Category: Common/Mana

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Mana")]
        [DisplayName("Mana Tea Count")]
        [Description("Stack Count to use Mana Tea")]
        public int ManaTeaStackCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Mana")]
        [DisplayName("Mana Tea Pct")]
        [Description("Mana Percent to use Mana Tea")]
        public int ManaTeaPct { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Mana")]
        [DisplayName("Mana Tea Stop Channel")]
        [Description("Will Stop casting mana tea at this mana percent when channeling")]
        public int StopCastingManaTeapCt { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Mana")]
        [DisplayName("Mana Tea Stop tank")]
        [Description("Will Stop casting mana tea when the tank is less than this percent")]
        public int StopCastingHealthpCtforTank { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Healing Sphere")]
        [DisplayName("Mana")]
        [Description("Mana Percent must be greater than this value to use Healing Spheres.")]
        public int HealingSphereManaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Healing Sphere")]
        [DisplayName("Priority")]
        [Description("When your Mana Percent is greater than this setting Healing Sphere will be prioritised Higher than normal")]
        public int HealingSpherePrioManaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Healing Sphere")]
        [DisplayName("Ignore SM")]
        [Description("This ignores the fact we are channeling soothing mist and casts healing sphere anyway")]
        public bool IgnoreSoothingMist { get; set; }

        
        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Common")]
        [DisplayName("Soothing Mist (switch)")]
        [Description("Oracle will Switch Channeling Soothing mist on the current target " +
                     "when the difference in health percent (between the current target and " +
                     "the next healtarget) is less than the specified value.")]
        public int SwitchMistPercent { get; set; }       

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Thunder Focus Tea")]
        [Description("Use Thunder Focus Tea with Max Chi")]
        public bool UseThunderFocusTeaMaxChi { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Uplift")]
        [Description("Use Uplift with Max Chi")]
        public bool UseUpliftMaxChi { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Nimble Brew")]
        [Description("Use Nimble Brew?")]
        public bool UseNimbleBrew { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(1)]
        [Category("Common")]
        [DisplayName("Chi Brew Count")]
        [Description("Chi count to cast Chi brew")]
        public int ChiBrewCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("MistweaveFist")]
        [Description("Will use Fistweaving when mana is greater than this value.")]
        public int FistWeaveMana { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Thunder Focus Tea")]
        [Description("Use Thunder Focus Tea On Cooldown")]
        public bool ThunderFocusTeaOnCooldown { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Uplift Overide")]
        [DisplayName("Enable/Disable")]
        [Description("Enable Uplift Overide will cast uplift at the Chi specified in the Uplift Overide Chi Count")]
        public bool EnableUpliftOveride { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Uplift Overide")]
        [DisplayName("Chi Count")]
        [Description("will cast uplift when the current chi is greater than this setting")]
        public int ChiCountUpliftOveride { get; set; }

        
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Soothing Overide")]
        [DisplayName("Enable/Disable")]
        [Description("Enable SoothingMist spam for chi when healtargets > 100% hp")]
        public bool EnableSoothingSpam { get; set; }


        //[Setting]
        //[Styx.Helpers.DefaultValue(95)]
        //[Category("Common")]
        //[DisplayName("Soothing Mist")]
        //[Description("Health Percent to use Soothing Mist")]
        //public int SoothingMistPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Serpent Statue")]
        [Description("When enabled Oracle will automaticly place Serpent Statue at the Tanks Location. Disable for elegon!")]
        public bool EnableSerpentStatueUsage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Serpent Statue")]
        [Description("Maximum Distance away from us until we place a new statue.")]
        public int MaxSerpentStatueDistance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Serpent Statue")]
        [Description("Maximum time in seconds to elapse before we check statue placement")]
        public int SerpentStatueWaitTime { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Surging Mist")]
        [DisplayName("Health Percent")]
        [Description("Health Percent to use Surging Mist with Vital mists 5 stack")]
        public int SurgingVitalMistPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(99)]
        [Category("Common")]
        [DisplayName("Expel Harm")]
        [Description("Health Percent to use Expel Harm")]
        public int ExpelHarmPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Common")]
        [DisplayName("Expel Harm")]
        [Description("Use Expel Harm when Chi count is less than the set amount")]
        public int ExpelHarmChiCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Dampen Harm")]
        [Description("Health Percent to use Dampen Harm")]
        public int DampenHarmPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Common")]
        [DisplayName("Life Cocoon")]
        [Description("Health Percent to use Life Cocoon on the tank")]
        public int LifeCocoonPct { get; set; }

         [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Life Cocoon")]
        [Description("Enables the use of Life Cocoon on the tank")]
        public bool EnableLifeCocoon { get; set; }

        

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Fortifying Brew")]
        [Description("Health Percent to use Fortifying Brew")]
        public int FortifyingBrewPct { get; set; }

        #endregion Category: Common/Mana

        #region Category: Spec

        #endregion Category: Spec

        #region Category: AoE Healing Spells

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        [Setting]
        [Styx.Helpers.DefaultValue(98)]
        [Category("AoE Healing Spells")]
        [DisplayName("Chi Burst")]
        [Description("Average health percent to use Chi Burst. Players must be below this amount to be included in the average.")]
        public int ChiBurstPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("AoE Healing Spells")]
        [DisplayName("Ch Burst")]
        [Description("Number of players that have the average health percent specified above")]
        public int ChiBurstCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(98)]
        [Category("AoE Healing Spells")]
        [DisplayName("Rushing Jade Wind")]
        [Description("Average health percent to use Rushing Jade Wind. Players must be below this amount to be included in the average.")]
        public int RushingJadeWindPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("AoE Healing Spells")]
        [DisplayName("Rushing Jade Wind")]
        [Description("Number of players that have the average health percent specified above")]
        public int RushingJadeWindCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(100)]
        [Category("AoE Healing Spells")]
        [DisplayName("Renewing Mist")]
        [Description("Average health percent to use Renewing Mist. Players must be below this amount to be included in the average.")]
        public int RenewingMistPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("AoE Healing Spells")]
        [DisplayName("Renewing Mist")]
        [Description("Number of players that have the average health percent specified above")]
        public int RenewingMistLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(96)]
        [Category("AoE Healing Spells")]
        [DisplayName("Uplift")]
        [Description("Average health percent to use Uplift. Players must be below this amount to be included in the average.")]
        public int UpliftPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Uplift")]
        [Description("Number of players that have the average health percent specified above")]
        public int UpliftLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("AoE Healing Spells")]
        [DisplayName("Revival")]
        [Description("Average health percent to use Revival. Players must be below this amount to be included in the average.")]
        public int RevivalPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("AoE Healing Spells")]
        [DisplayName("Revival 10 man")]
        [Description("Number of players that have the average health percent specified above")]
        public int RevivalLimit10Man { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("AoE Healing Spells")]
        [DisplayName("Revival 25 man")]
        [Description("Number of players that have the average health percent specified above")]
        public int RevivalLimit25Man { get; set; }

        #endregion Category: AoE Healing Spells

        #region Category: Emergency Cooldowns

        //Oh Shit Moments!

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("AoE Healing Spells")]
        [DisplayName("InvokeXuentheWhiteTiger")]
        [Description("Average health percent to use InvokeXuentheWhiteTiger. Players must be below this amount to be included in the average.")]
        public int InvokeXuentheWhiteTigerPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("AoE Healing Spells")]
        [DisplayName("InvokeXuentheWhiteTiger")]
        [Description("Number of players that have the average health percent specified above")]
        public int InvokeXuentheWhiteTigerLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("AoE Healing Spells")]
        [DisplayName("Thunder Focus Tea")]
        [Description("Average health percent to use InvokeXuentheWhiteTiger. Players must be below this amount to be included in the average.")]
        public int ThunderFocusTeaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Thunder Focus Tea")]
        [Description("Number of players that have the average health percent specified above")]
        public int ThunderFocusTeaLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Common")]
        [DisplayName("Urgent Health Percentage")]
        [Description("we ignore all settings and start healing like hell!!!")]
        // we ignore all settings and start healing like hell!!!
        public int UrgentHealthPercentage { get; set; }

        #endregion Category: Emergency Cooldowns

        #region Category: Enable Dynamic Spells (Single Heal).

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Enveloping Mist")]
        [Description("When enabled Enveloping Mist will be included in the Dynamic Spell Priority single target list")]
        public bool EnvelopingMistPrioEnabled { get; set; }
        
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Dynamic Spells")]
        [DisplayName("Surging Mist")]
        [Description("When enabled Surging Mist will be included in the Dynamic Spell Priority single target list")]
        public bool SurgingVitalPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Soothing Mist")]
        [Description("When enabled Soothing Mist will be included in the Dynamic Spell Priority single target list")]
        public bool SoothingMistPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Healing Sphere")]
        [Description("When enabled Healing Sphere will be included in the Dynamic Spell Priority single target list")]
        public bool HealingSpherePrioEnabled { get; set; }

        #endregion

        #region Category: Enable Dynamic Spells (AoE Heal).

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Chi Burst")]
        [Description("When enabled Chi Burst will be included in the Dynamic Spell Priority AoE list")]
        public bool ChiBurstPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Renewing Mist")]
        [Description("When enabled Renewing Mist will be included in the Dynamic Spell Priority AoE list")]
        public bool RenewingMistPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Soothing Mist")]
        [Description("When enabled Soothing Mist will be included in the Dynamic Spell Priority AoE list")]
        public bool UpliftPrioEnabled { get; set; }

        #endregion

        #region Category: Enable Dynamic Spells (Cooldown Spells).

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Revival")]
        [Description("When enabled Revival will be included in the Dynamic Spell Priority Cooldown list")]
        public bool RevivalPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("ThunderFocusTea")]
        [Description("When enabled Thunder Focus Tea will be included in the Dynamic Spell Priority Cooldown list")]
        public bool ThunderFocusTeaPrioEnabled { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dynamic Spells")]
        [DisplayName("Xuen the White Tiger")]
        [Description("When enabled Xuen the White Tiger will be included in the Dynamic Spell Priority Cooldown list")]
        public bool InvokeXuentheWhiteTigerPrioEnabled { get; set; }

        #endregion
    }
}