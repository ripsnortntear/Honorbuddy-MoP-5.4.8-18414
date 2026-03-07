#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-17 10:23:04 +0200 (Fr, 17 Mai 2013) $
 * $ID$
 * $Revision: 1424 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/RogueSettings.cs $
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
    internal class RogueSettings : Styx.Helpers.Settings
    {
        public RogueSettings()
            : base(PRSettings.SettingsPath + "_Rogue.xml")
        {
        }

        internal enum RotationSelection
        {
            PvP,
            PvERupture,
            None
        }

        [Setting]
        [Styx.Helpers.DefaultValue(RotationSelection.PvP)]
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
        [Styx.Helpers.DefaultValue(6)]
        [Category("Common")]
        [DisplayName("AoE Add Count")]
        [Description("Will use AoE on Count number you choose.)")]
        public int AoECount { get; set; }

        #endregion Category : AoECount

        #region Common - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(MHPoisonType.Deadly)]
        [Category("Common")]
        [DisplayName("Main Hand Poison")]
        [Description("Main Hand Poison")]
        public MHPoisonType MainHandPoison
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(OHPoisonType.Crippling)]
        [Category("Common")]
        [DisplayName("Off Hand Poison")]
        [Description("Off Hand Poison")]
        public OHPoisonType OffHandPoison
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Tricks Of The Trade")]
        [Description("Use Tricks Of The Trade")]
        public bool UseTricksOfTheTrade
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Use Expose Armor")]
        [Description("Uses Expose Armor with Glyph on")]
        public bool UseExposeArmor
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Use Recuperate")]
        [Description("Allow the use of Recuperate in combat?")]
        public bool UseRecuperate { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Common")]
        [DisplayName("Recuperate Percent")]
        [Description("Maximum health percent threshold at which to start using Recuperate. Only used if Use Recuperation is set to true.")]
        public int RecuperatePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("Common")]
        [DisplayName("Recuperate ComboPoints")]
        [Description("When to use Recuperate at ComboPoints to start using Recuperate. Only use 1-5 Points")]
        public int RecuperatePoints { get; set; }

        #endregion Common - Checked (29/12/2012)

        #region Combat

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Combat Spec")]
        [DisplayName("Use Blade Flurry")]
        [Description("Will use Blade Flurry when enabled and aggroed mob count is equal to or higher configured value.")]
        public bool CombatUseBladeFlurry { get; set; }
        
        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Combat Spec")]
        [DisplayName("Blade Flurry Count")]
        [Description("Will use Blade Flurry when aggroed mob count is equal to or higher than this value.")]
        public int CombatBladeFlurryCount { get; set; }
        #endregion Combat

        #region Subtlety - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(SubtletyRogueRotation.Algorithmic)]
        [Category("Subtlety Spec")]
        [DisplayName("Rotation Type")]
        [Description(@"Choose what rotation type you prefer. Algorithmic is meant to be fully optimized,
                        but does a lot more work and is in active development.  Estimated is a standard
                        rotation with estimated times and is not meant to be optimized for every
                        situation.")]
        public SubtletyRogueRotation SubtletyRotation { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(7)]
        [Category("Subtlety Spec")]
        [DisplayName("Fan of Knives Add Count")]
        [Description("Will use Fan of Knives when agro mob count is equal to or higher then this value.")]
        public int SubtletyFanOfKnivesCount
        {
            get;
            set;
        }

        #endregion Subtlety - Checked (29/12/2012)

        #region Assassination - Checked (29/12/2012)

        //[Setting]
        //[Styx.Helpers.DefaultValue(5)]
        //[Category("Assasination Spec")]
        //[DisplayName("Fan of Knives Add Count")]
        //[Description("Will use Fan of Knives when agro mob count is equal to or higher then this value.")]
        //public int AssasinationFanOfKnivesCount
        //{
        //    get;
        //    set;
        //}

        #endregion Assassination - Checked (29/12/2012)
    }
}