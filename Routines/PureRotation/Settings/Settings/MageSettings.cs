#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-06 10:11:30 +0200 (Di, 06 Aug 2013) $
 * $ID$
 * $Revision: 1675 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/MageSettings.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC

using System.ComponentModel;
using PureRotation.Core;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class MageSettings : Styx.Helpers.Settings
    {
        public MageSettings()
            : base(PRSettings.SettingsPath + "_Mage.xml")
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
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Use ManaGem")]
        [Description("Enables the usage of ManaGem")]
        public bool EnableManaGem { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("ManaGem Percent")]
        [Description("Use your mana gem at this mana value")]
        public int ManaGemPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(15)]
        [Category("Common")]
        [DisplayName("Evocation Percent")]
        [Description("Use Evocation at this mana value - this is seperate from maintaining Invoker's Energy")]
        public int EvocationPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Mirror Image")]
        [Description("Enables the usage of Mirror Image")]
        public bool EnableMirrorImage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Alter Time")]
        [Description("Enables the usage of Alter Time")]
        public bool EnableAlterTime { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Presence of Mind")]
        [Description("Enables the usage of Presence of Mind")]
        public bool EnablePresenceofMind { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Spellsteal")]
        [Description("Enables the usage of Spellsteal")]
        public bool UseSpellsteal { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(MultiDoTMethod.AroundTarget)]
        [Category("Common")]
        [DisplayName("Multi-DoT Method")]
        [Description("AroundTarget will DoT all units near your target. " +
                    "EverythingAroundMe will DoT everything that is attacking you or a party member. " +
                    "None will disable multi-dotting.")]
        public MultiDoTMethod MultiDoTStyle
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("MultiDoT in Single Target")]
        [Description("Enables multi-dotting in single target mode")]
        public bool MultiDoTSingleTarget { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Use Ice Block")]
        [Description("Will use Ice Block when enabled and necessary")]
        public bool useIceBlock { get; set; }
        #endregion Common  - Checked (29/12/2012)

        #region Frost  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Use Incanter's Ward")]
        [Description("Enables the automatic usage of Incanter's Ward")]
        public bool UseIncantersWard { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Ice Ward")]
        [Description("Casts Ice Ward on self when target within melee range")]
        public bool UceIceWard { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Frost Nova")]
        [Description("Casts Frost Nova when target within melee range")]
        public bool UseFrostNova { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Use Pet Freeze")]
        [Description("Casts Pet Freeze on non-boss targets.")]
        public bool UsePetFreeze { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Use Frozen Orb")]
        [Description("Casts Pet Freeze on non-boss targets.")]
        public bool UseFrozenOrb { get; set; }

        #endregion Frost  - Checked (29/12/2012)

        #region Fire  - Checked (29/12/2012)
        [Setting]
        [Styx.Helpers.DefaultValue(SpellUsage.OnCooldown)]
        [Category("Fire")]
        [DisplayName("Combustion usage")]
        [Description("Enables the usage of Combustion")]
        public SpellUsage CombustionType { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20000)]
        [Category("Fire")]
        [DisplayName("Combustion Ignite")]
        [Description("min Ignite for the usage of Combustion, when set to OnCondition")]
        public int IgniteCombustion { get; set; }
        #endregion Fire  - Checked (29/12/2012)

        #region Arcane  - Checked (29/12/2012)

        #endregion Arcane  - Checked (29/12/2012)


        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Experimental")]
        [DisplayName("Lag Reduction")]
        [Description("Likely cause issues if your in game latency is high.")]
        public bool EnableLagFix { get; set; }
    }
}