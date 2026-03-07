#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-25 12:44:43 +0200 (So, 25 Aug 2013) $
 * $ID$
 * $Revision: 1699 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/MonkSettings.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC

using System.ComponentModel;
using Styx.Helpers;
using PureRotation.Core;
namespace PureRotation.Settings.Settings
{
    internal class MonkSettings : Styx.Helpers.Settings
    {
        public MonkSettings()
            : base(PRSettings.SettingsPath + "_Monk.xml")
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

        #region Common  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Healthstone Percent")]
        [Description("Will use a Healthstone for self heal at this healthpercent.")]
        public int HealthstonePercent
        {
            get;
            set;
        }

        #endregion Common  - Checked (29/12/2012)

        #region Brewmaster  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Brewmaster")]
        [DisplayName("Healing Sphere Percent")]
        [Description("Will use Healing Sphere at the set Health Percent.")]
        public int BrewHealingSpherePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(100)]
        [Category("Brewmaster")]
        [DisplayName("Chi Wave Percent")]
        [Description("Will use Chi Wave at the set Health Percent. default 100 to maintain constantly")]
        public int BrewChiWavePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Brewmaster")]
        [DisplayName("Fortifying Brew Percent")]
        [Description("Will use Fortifying Brew at the set Health Percent.")]
        public int BrewFortifyingBrewPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Brewmaster")]
        [DisplayName("Dampen Harm Percent")]
        [Description("Will use Dampen Harm at the set Health Percent.")]
        public int BrewDampenHarmPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Brewmaster")]
        [DisplayName("Zen Meditation Percent")]
        [Description("Will use Zen Meditation at the set Health Percent.")]
        public int BrewZenMeditationPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Brewmaster")]
        [DisplayName("Elusive Brew Count")]
        [Description("Will use Elusive Brew at the set stack count.")]
        public int BrewElusiveBrewCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Brewmaster")]
        [DisplayName("Spinning Crane Kick Count")]
        [Description("Will use Spinning Crane Kick when the mob count is greater than the set count.")]
        public int BrewSpinningCraneKickCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(100)]
        [Category("Brewmaster")]
        [DisplayName("Zen Sphere")]
        [Description("Will use Zen Sphere when health drops below this percentage, default 100 to maintain constantly")]
        public int BrewWithZenSphere
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(5000)]
        [Category("Brewmaster")]
        [DisplayName("Shuffle refresh")]
        [Description("Will refresh shuffle when the milliseconds left is less than the value. i.e: 5000ms = 5 seconds.")]
        public int BrewShuffleRefreshMilliseconds
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Brewmaster")]
        [DisplayName("Sanctuary of the Ox")]
        [Description("Enable the automatic placement of Sanctuary of the Ox")]
        public bool BrewEnableSanctuaryoftheOx
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Brewmaster")]
        [DisplayName("Guard HP Percent")]
        [Description("Will cast guard when your HP percent is below this value.")]
        public int GuardHPPercent
        {
            get;
            set;
        }

        #endregion Brewmaster  - Checked (29/12/2012)

        #region WindWalker  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Windwalker")]
        [DisplayName("Touch of Karma Percent")]
        [Description("Will use Touch of Karma at the set Health Percent. default 40 to maintain constantly")]
        public int TouchofKarmaHPWW
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Windwalker")]
        [DisplayName("Fortifying Brew Percent")]
        [Description("Will use Fortifying Brew at the set Health Percent. default 40 to maintain constantly")]
        public int FortifyingBrewPercentWW
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Windwalker")]
        [DisplayName("Dampen Harm Percent")]
        [Description("Will use Dampen Harm at the set Health Percent. default 40 to maintain constantly")]
        public int DampenHarmPercentWW
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Windwalker")]
        [DisplayName("Diffuse Magic Percent")]
        [Description("Will use Diffuse Magic at the set Health Percent. default 40 to maintain constantly")]
        public int DiffuseMagicPercentWW
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(TeBUsage.None)]
        [Category("Windwalker")]
        [DisplayName("Tigereye Brew usage")]
        [Description("Will use Tigereye Brew: None (Never), Cooldown (all the Time When no TeB up and Stacks are above your setting), Heroism (when BL,Hero,TW is up), RoRo on RoRo procc 2 secs left")]
        public TeBUsage TigerEyeBrewUsage
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(10)]
        [Category("Windwalker")]
        [DisplayName("Tigereye Brew Count")]
        [Description("Will use Tigereye Brew at the set stack count.")]
        public int TigerEyeBrewCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Windwalker")]
        [DisplayName("Expel Harm Percent")]
        [Description("Will use Expel Harm at the set Health Percent. default 30")]
        public int ExpelHarmPercentWW
        {
            get;
            set;
        }


        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Windwalker")]
        [DisplayName("Invoke Xuen, the White Tiger")]
        [Description("Will cast Xuen on cooldown, when target is a boss.")]
        public bool UseXuen
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Windwalker")]
        [DisplayName("Touch of Karma")]
        [Description("Will cast Touch of Karma if enabled.")]
        public bool EnableTouchofKarmaWW 
        {
            get;
            set;
        }


                [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Windwalker")]
        [DisplayName("Nimble Brew")]
        [Description("Will cast Nimble Brew if specced.")]
        
        public bool UseNimbleBrewWW

        {
            get;
            set;
        }



        

           

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Windwalker")]
        [DisplayName("Expel Harm Enable")]
        [Description("Will enable usage of Expel Harm as defensive cooldown")]
        public bool EnableExpelHarm
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Windwalker")]
        [DisplayName("Use Touch of Death")]
        [Description("Will cast Touch of Death, if enabled.")]
        public bool UseTouchOfDeath
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Windwalker")]
        [DisplayName("Use Rushing Jade Wind")]
        [Description("Will cast RJW on cooldown, when target is a boss.")]
        public bool UseRJW
        {
            get;
            set;
        }

        #endregion WindWalker  - Checked (29/12/2012)

        #region MistWeaver  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("MistWeaver")]
        [DisplayName("Dampen Harm Percent")]
        [Description("Will use Dampen Harm at the set Health Percent.")]
        public int MistDampenHarmPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("MistWeaver")]
        [DisplayName("Surging/VitalMist Percent")]
        [Description("Will use Surging Mist at the set Health Percent when the Vital Mist Stack is 5")]
        public int SurgingVitalMistPercent
        {
            get;
            set;
        }

         [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("MistWeaver")]
        [DisplayName("Healing Sphere Percent")]
        [Description("Will use Healing Sphere at the set Health Percent")]
        public int HealingSpherePercent
        {
            get;
            set;
        }

        

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("MistWeaver")]
        [DisplayName("Life Cocoon Percent")]
        [Description("Will use Life Cocoon on the tank at the set Health Percent.")]
        public int MistLifeCocoonPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("MistWeaver")]
        [DisplayName("Fortifying Brew Percent")]
        [Description("Will use Fortifying Brew at the set Health Percent.")]
        public int MistFortifyingBrewPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("MistWeaver")]
        [DisplayName("Expel Harm Percent")]
        [Description("Will use Expel Harm at the set Health Percent.")]
        public int MistExpelHarmPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(99)]
        [Category("MistWeaver")]
        [DisplayName("Mana Tea Percent")]
        [Description("Will use Mana Tea at the set Mana Percent.")]
        public int MistManaTeaPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(1)]
        [Category("MistWeaver")]
        [DisplayName("Mana Tea StackCount")]
        [Description("Will use Mana Tea when the StackCount is greater to or equal to the set StackCount")]
        public int MistManaTeaStackCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("MistWeaver")]
        [DisplayName("Renewing Mist Percent")]
        [Description("Will use Renewing Mist at the set Health Percent.")]
        public int MistRenewingMistPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("MistWeaver")]
        [DisplayName("Soothing Mist Percent")]
        [Description("Will use Soothing Mist at the set Health Percent.")]
        public int MistSoothingMistPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("MistWeaver")]
        [DisplayName("Surging Mist Percent")]
        [Description("Will use Surging Mist at the set Health Percent.")]
        public int MistSurgingMistPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(55)]
        [Category("MistWeaver")]
        [DisplayName("Enveloping Mist Percent")]
        [Description("Will use Enveloping Mist at the set Health Percent.")]
        public int MistEnvelopingMistPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("MistWeaver")]
        [DisplayName("ChiFinishers Percent")]
        [Description("Will use Chi Finishers (FistWeaver) when our target to heal is greater than the set Health Percent")]
        public int MistChiFinishersPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(1)]
        [Category("MistWeaver")]
        [DisplayName("ChiFinishers StackCount")]
        [Description("Will use Chi Finishers (FistWeaver) when our Chi is greater than or equal to the set Chi Stackcount")]
        public int MistChiFinishersStackCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("MistWeaver")]
        [DisplayName("ChiBuilders Percent")]
        [Description("Will use Chi Builders (FistWeaver) when the our target to heal is greater than the set Health Percent")]
        public int MistChiBuildersPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("MistWeaver")]
        [DisplayName("ChiBuilders StackCount")]
        [Description("Will use Chi Builders (FistWeaver) when our Chi is less than or equal to the set Chi Stackcount")]
        public int MistChiBuildersStackCount
        {
            get;
            set;
        }
        [Setting]
        [Styx.Helpers.DefaultValue(65)]
        [Category("MistWeaver")]
        [DisplayName("MistweaveFist")]
        [Description("Will use Fistweaving when mana is greater than this value in the Mistweaving rotation(special enabled)")]
        public int FistWeaveMana
        {
            get;
            set;
        }


        #endregion MistWeaver  - Checked (29/12/2012)
    }
}