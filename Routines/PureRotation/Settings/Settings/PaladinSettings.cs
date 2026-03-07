#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-17 10:23:04 +0200 (Fr, 17 Mai 2013) $
 * $ID$
 * $Revision: 1424 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/PaladinSettings.cs $
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
    internal class PaladinSettings : Styx.Helpers.Settings
    {
        public PaladinSettings()
            : base(PRSettings.SettingsPath + "_Paladin.xml")
        {
        }

        #region Common  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(PaladinBlessing.Kings)]
        [Category("Common")]
        [DisplayName("Paladin Blessing Selector")]
        [Description("Choose a Paladin Blessing. This is on applicable to solo play (ie: not in party or raid.)")]
        public PaladinBlessing BlessingSelection
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
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Seal Swapping")]
        [Description("Swap between Seals when Mob count is greater than this")]
        public bool UseSealSwapping
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Common")]
        [DisplayName("Seal Swapping Add Count")]
        [Description("Will use Seal of Righteousness when agro mob count is equal to or higher then this value.")]
        public int SealofRighteousnessCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Avenging Wrath")]
        [Description("If set to true Pure will use Avenging Wrath.")]
        public bool UseAvengingWrath
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Execution Sentence")]
        [Description("If set to true Pure will use Execution Sentence.")]
        public bool UseExecutionSentence
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(58)]
        [Category("Common")]
        [DisplayName("Execution Sentence Percent")]
        [Description("If set to true Pure will use Execution Sentence health reaches this value.")]
        public int UseExecutionSentenceHealthPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Protection")]
        [DisplayName("Holy Prism Health")]
        [Description("Holy Prism will be used at this value")]
        public int HolyPrismPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Protection")]
        [DisplayName("Holy Prism Health")]
        [Description("Holy Prism will be used at this value")]
        public int LightsHammerPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("HolyPrismLightsHammer Percent")]
        [Description("Will use Holy Prism or Lights Hammer for Raid heal when friendly player healthpercent reaches this value.")]
        public int HolyPrismLightsHammerPercent
        {
            get;
            set;
        }

        #endregion Common  - Checked (29/12/2012)

        #region Holy  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Holy")]
        [DisplayName("Hand of Salvation Percent")]
        [Description("Will use Hand of Salvation when Tank below this")]
        public int HandOfSalvationPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Holy")]
        [DisplayName("Hand of Protection Percent")]
        [Description("Will use Hand of Protection when HealTarget below this")]
        public int HandOfProtectionPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Holy")]
        [DisplayName("Divine Light Health")]
        [Description("Divine Light will be used at this value")]
        public int DivineLightPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Holy")]
        [DisplayName("Flash Light Health")]
        [Description("Flash Light will be used at this value")]
        public int FlashLightPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Holy")]
        [DisplayName("Lay on Hands Health")]
        [Description("Lay on Hands will be used at this value, this is only used with Retribution atm")]
        public int LayOnHandsPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Holy")]
        [DisplayName("Sacred Shield Health")]
        [Description("Sacred Shield will be used at this value, this is only used with Retribution atm")]
        public int SacredShieldPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Holy")]
        [DisplayName("Holy Light Health")]
        [Description("Holy Light will be used at this value")]
        public int HolyLightPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Holy")]
        [DisplayName("Word of Glory Health")]
        [Description("Word of Glory will be used at this value")]
        public int WordGloryPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Holy")]
        [DisplayName("Start Urgent Healing")]
        [Description("Will start special healing logic")]
        public int UrgentHealingPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Common")]
        [DisplayName("Guardian of Ancient Kings Health")]
        [Description("Guardian of Ancient Kings will be used at this value")]
        public int GuardianofAncientKingsHealth
        {
            get;
            set;
        }

        #endregion Holy  - Checked (29/12/2012)

        #region Protection  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("Protection")]
        [DisplayName("Prot AoE Count")]
        [Description("Will use AoE abillites when agro mob count is equal to or higher then this value.")]
        public int ProtAoECount { get; set; }

        [Browsable(false)] // TODO: Implemented settings but not sure to use it or not.
        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("Protection")]
        [DisplayName("HolyPrismLightsHammer Count")]
        [Description("Will use Holy Prism or Lights Hammer for Raid heal when friendly player count reachs.")]
        public int HolyPrismLightsHammerCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(55)]
        [Category("Protection")]
        [DisplayName("Word of Glory Health")]
        [Description("Word of Glory will be used at this value")]
        public int ProtWordofGloryPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Protection")]
        [DisplayName("Avenging Wrath for DPS/Threat")]
        [Description("If set to true Pure will use Avenging Wrath on Boss for Threat/DPS.")]
        public bool UseAvengingWrathforThreat
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(45)]
        [Category("Protection")]
        [DisplayName("Avenging Wrath Percent")]
        [Description("If set to true Pure will use Avenging Wrath when your health reaches this value.")]
        public int UseAvengingWrathHealthPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Protection")]
        [DisplayName("Execution Sentence for DPS/Threat")]
        [Description("If set to true Pure will use Execution Sentence on Boss for Threat/DPS.")]
        public bool UseExecutionSentenceforThreat
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Protection")]
        [DisplayName("Shield of the Righteous Health")]
        [Description("Shield of the Righteous will be used at this value")]
        public int ShoRPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Protection")]
        [DisplayName("Holy Prism Add Count")]
        [Description("Will use Holy Prism when agro mob count is equal to or higher then this value.")]
        public int HolyPrismCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Protection")]
        [DisplayName("Lights Hammer Add Count")]
        [Description("Will use Lights Hammer when agro mob count is equal to or higher then this value.")]
        public int LightsHammerCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Protection")]
        [DisplayName("Consecration Count")]
        [Description("Consecration will be used when agro mob count is equal to or higher then this value.")]
        public int ConsecrationCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Protection")]
        [DisplayName("Consecration Mana Percent")]
        [Description("Consecration will be used when your mana percent id greater than or equal to this value")]
        public int ConsecrationManaPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Protection")]
        [DisplayName("Divine Protection Percent")]
        [Description("Will use Divine Protection at this healthpercent.")]
        public int ProtectionDPPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Protection")]
        [DisplayName("Lay on Hands Percent")]
        [Description("Will use Lay on Hands at this healthpercent.")]
        public int ProtectionLoHPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Protection")]
        [DisplayName("Divine Shield Percent")]
        [Description("Will use Divine Shield at this healthpercent.")]
        public int ProtectionDSPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Protection")]
        [DisplayName("Holy Avenger Health")]
        [Description("Holy Avenger will be used at this value for additional survivability with ShoR buff.")]
        public int HolyAvengerHealthProt
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Protection")]
        [DisplayName("Guardian of Ancient Kings Health")]
        [Description("Guardian of Ancient Kings will be used at this value")]
        public int GuardianofAncientKingsHealthProt
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Protection")]
        [DisplayName("Ardent Defender Health")]
        [Description("Ardent Defender will be used at this value")]
        public int ArdentDefenderHealth
        {
            get;
            set;
        }

        #endregion Protection  - Checked (29/12/2012)

        #region Retribution  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Retribution")]
        [DisplayName("Ret AoE Count")]
        [Description("Will use AoE abillites when agro mob count is equal to or higher then this value.")]
        public int RetAoECount { get; set; }

        //[Setting]
        //[Styx.Helpers.DefaultValue(4)]
        //[Category("Retribution")]
        //[DisplayName("Hammer of the Righteous Add Count")]
        //[Description("Will use Hammer of the Righteous when agro mob count is equal to or higher then this value.")]
        //public int RetributionHoRCount
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(4)]
        //[Category("Retribution")]
        //[DisplayName("Divine Storm Add Count")]
        //[Description("Will use Divine Storm when agro mob count is equal to or higher then this value.")]
        //public int DivineStormCount
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(80)]
        //[Category("Retribution")]
        //[DisplayName("Divine Protection Percent")]
        //[Description("Will use Divine Protection at this healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        //public int RetributionDPPercent
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(30)]
        //[Category("Retribution")]
        //[DisplayName("Lay on Hands Percent")]
        //[Description("Will use Lay on Hands at this healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        //public int RetributionLoHPercent
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(25)]
        //[Category("Retribution")]
        //[DisplayName("Divine Shield Percent")]
        //[Description("Will use Divine Shield at this healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        //public int RetributionDSPercent
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(20)]
        //[Category("Retribution")]
        //[DisplayName("Hand of Protection Percent")]
        //[Description("Will use Hand of Protection at this healthpercent. (Self Healing (General Tab) must be enabled as well.)")]
        //public int RetributionHoPPercent
        //{
        //    get;
        //    set;
        //}
        //[Setting]
        //[Styx.Helpers.DefaultValue(2)]
        //[Category("Retribution")]
        //[DisplayName("Light's Hammer Count")]
        //[Description("Will use Light's Hammer if AddCount >= this value (AOE Must be enabled)")]
        //public int RetributionLightsHammerCount
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(true)]
        //[Category("Retribution")]
        //[DisplayName("Prevent Inquisition from falling off")]
        //[Description("Enable this to prevent Inquisition from fallin off")]
        //public bool RetributionInquisitionFallOffProtection
        //{
        //    get;
        //    set;
        //}

        #endregion Retribution  - Checked (29/12/2012)
    }
}