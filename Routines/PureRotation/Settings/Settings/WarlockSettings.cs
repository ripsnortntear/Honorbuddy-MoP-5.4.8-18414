#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-06-26 17:50:47 +0200 (Mi, 26 Jun 2013) $
 * $ID$
 * $Revision: 1575 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/WarlockSettings.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

using System.ComponentModel;
using Styx.Helpers;
using PureRotation.Core;

namespace PureRotation.Settings.Settings
{
    internal class WarlockSettings : Styx.Helpers.Settings
    {
        public WarlockSettings()
            : base(PRSettings.SettingsPath + "_Warlock.xml")
        {
        }

        #region PvE: Common

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Common")]
        [DisplayName("AoE Add Count")]
        [Description("Will use AoE on Count number you choose.)")]
        public int AoECount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Summon Doomguard")]
        [Description("When this is set to true, Pure will Summon Doomguard only on boss")]
        public bool UseSummonDoomguard
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Summon Infernal")]
        [Description("When this is set to true, Pure will cast Summon Infernal")]
        public bool UseSummonInferno
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Twilight Ward Percent")]
        [Description("Twilight Ward will be used at this Health Percent if the target is currently casting somthing at the player")]
        public int TwilightWardPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Common")]
        [DisplayName("Unending Resolve Percent")]
        [Description("Unending Resolve will be used at this Health Percent.")]
        public int UnendingResolvePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Dark Bargain Percent")]
        [Description("Dark Bargain will be used at this Health Percent.")]
        public int DarkBargainPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Mortal Coil Percent")]
        [Description("Mortal Coil will be used at this Health Percent.")]
        public int MortalCoilPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Common")]
        [DisplayName("Infernal Count")]
        [Description("Will use Summon Infernal at this number given.")]
        public int InfernoCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Common")]
        [DisplayName("Shadow Bulwark Percent")]
        [Description("Shadow Bulwark will be used at this Health Percent.")]
        public int ShadowBulwarkPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Burning Rush")]
        [Description("Will activate/deactivate Burning Rush when moving at specified health percentage.")]
        public bool UseBurningRush
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Burning Rush: Auto Cancel")]
        [Description("Will cancel Burning Rush when stopped moving or HP is below specified percentage, even when manually activated. Set to false to manually cancel.")]
        public bool AutoBurningRushCancel
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Only use Curse of the Elements on bosses")]
        [Description("Will only use CoE on bosses.")]
        public bool OnlyCoEOnBoss
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Burning Rush Percent")]
        [Description("Health percentage must be over this value to use Burning Rush. Will cancel when under.")]
        public int BurningRushPercentage
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Healthstone Percent")]
        [Description("Will use healthstone at this percent HP.")]
        public int HealthstonePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Create Soulwell")]
        [Description("Will create soulwell whilst buffing. Does not cast if already holding healthstone.")]
        public bool CastSoulwellDuringBuffing
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Create Healthstone")]
        [Description("Will create healthstone whilst buffing. Does not cast if already holding healthstone.")]
        public bool CastHealthstoneDuringBuffing
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Soulshatter")]
        [Description("Will use soulshatter when our threat on current target is over 95%")]
        public bool UseSoulshatter
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Soulstone Self at HP Percent")]
        [Description("Will use soulstone when your HP drops below this value and have agro. Set to 0 to disable.")]
        public int SoulstonePercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(WarlockPet.Auto)]
        [Category("Common")]
        [DisplayName("Pet Selection")]
        [Description("Which pet to use.")]
        public WarlockPet WarlockPet
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Dark Regeneration at HP Percent")]
        [Description("Will cast Dark Regeneration when your HP drops below this value. Set to 0 to disable.")]
        public int DarkRegenerationPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Howl of Terror at HP Percent")]
        [Description("Will cast Howl of Terror when your HP drops below this value and [Setting:Howl of Terror Unit Count] in range.")]
        public int HowlofTerrorPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Common")]
        [DisplayName("Howl of Terror unit count")]
        [Description("Will cast Howl of Terror when HP below setting value, and this number of units in range of us.")]
        public int HowlofTerrorUnitCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Unbound Will")]
        [Description("Will cast Unbound Will (when talented) and we've lost control of our character")]
        public bool UseUnboundWill
        {
            get;
            set;
        }



        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Cast Grimoire of Sacrifice")]
        [Description("Will auto cast Grimoire of Sacrifice if you have the talent.")]
        public bool UseGrimoireOfSacrifice
        {
            get;
            set;
        }

        #endregion Common

        #region PvE: Affliction

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Affliction")]
        [DisplayName("Dark Soul: Misery")]
        [Description("When this is set to true, Pure will use " +
                     "Dark Soul: Misery on cooldown")]
        public bool UseDarkSoulMisery
        {
            get;
            set;
        }

        /*
        [Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("Affliction")]
        [DisplayName("Haunt when Shards Exceeds")]
        [Description("Will only cast Haunt when shard count exceeds this value. " +
                     "0 to always use, 5 to never use. Higher for AoE fights, " +
                     "low for single target.")]
        public int HauntShardsExceeds
        {
            get;
            set;
        }
        */

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Affliction")]
        [DisplayName("Refresh DoTs on stat increase")]
        [Description("When this is set to true, Pure will refresh " +
                     "DoTs when a stat increasing buff is active.")]
        public bool RefreshDoTsOnStatIncrease
        {
            get;
            set;
        }

        #endregion Affliction

        #region PvE: Demonology

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Demonology")]
        [DisplayName("Dark Soul: Knowledge")]
        [Description("When this is set to true, Pure will use " +
                     "Dark Soul: Knowledge")]
        public bool UseDarkSoulKnowledge
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Demonology")]
        [DisplayName("Pet Felstorm Unit Count")]
        [Description("Pet needs to be near this many units before " +
                     "casting Felstorm")]
        public int PetFelstormCount
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(900)]
        [Category("Demonology")]
        [DisplayName("Fury Value - Cast Metamorphosis")]
        [Description("Value of demonic fury required before activating Metamorphosis. Must be HIGHER than Cancel value.")]
        public int MetamorphosisFuryValue
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(650)]
        [Category("Demonology")]
        [DisplayName("Fury Value - Cancel Metamorphosis")]
        [Description("Value of demonic fury required before canceling Metamorphosis. Must be LOWER than Cast value.")]
        public int CancelMetamorphosisValue
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Demonology")]
        [DisplayName("Dark Apotheosis: Tank Mode")]
        [Description("When this is set to true, Pure will use " +
                     "the 'tanking' routine. Setting only applies in Auto mode. " +
                     "Use 'Special' hotkey to enable/disable tank mode in Hotkey mode")]
        public bool TankMode
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Demonology")]
        [DisplayName("Face Units for Void Ray")]
        [Description("When this is set to true, Pure will face units to cast void ray.")]
        public bool FaceForVoidRay
        {
            get;
            set;
        }

        #endregion Demonology

        #region PvE: Destruction

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Destruction")]
        [DisplayName("Dark Soul: Instability")]
        [Description("When this is set to true, Pure will use " +
                     "Dark Soul: Instability")]
        public bool UseDarkSoulInstability
        {
            get;
            set;
        }

        
        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("Destruction")]
        [DisplayName("Dark Soul: Minimum Ember Count")]
        [Description("Number of embers before using Dark Soul (0 will cast off CD, greater than 4 will never cast). Will always trigger when more than 1 ember and Fire and Brimstone active.")]
        public double DarkSoulMinimumEmbers
        {
            get;
            set;
        }
        

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Destruction")]
        [DisplayName("Use Havoc")]
        [Description("When this is set to true, Pure will use Havoc on 2+ mobs")]
        public bool UseHavoc
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Destruction")]
        [DisplayName("Use Flames of Xoroth")]
        [Description("When this is set to true, Pure will Summon your last pet using Flames of Xoroths")]
        public bool UseFlamesofXoroth
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Destruction")]
        [DisplayName("EmberTap Percent")]
        [Description("EmberTap will be used at this Health Percent.")]
        public int EmberTapPercent
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Destruction")]
        [DisplayName("Rain of Fire: Single Target")]
        [Description("Will enable RoF in single target rotation. " +
                    "This is a minor dps increase when boss doesn't move.")]
        public bool SingleTargetRoF
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Destruction")]
        [DisplayName("Rain of Fire: All in Range")]
        [Description("Will cast RoF on all units that are attacking us (or party member). Will not work on target dummies. Warning: This may cause serious lag issues and/or pull units accidentally!")]
        public bool RoFEverythingInRange
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Destruction")]
        [DisplayName("Use Enhanced Single Target Mode")]
        [Description("Will enable Havoc and RoF to be used as AoE when in single target mode, performing single target dps on target, but keeping up RoF/Havoc where possible.")]
        public bool EnhancedSingleTargetMode
        {
            get;
            set;
        }

        #endregion Destruction

        #region PvE: Destruction: Kanrethad Ebonlocke

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Destruction: Kanrethad Ebonlocke")]
        [DisplayName("Kanrethad Ebonlocke")]
        [Description("This is for the warlock green fire quest. " +
                     "DISABLE IF NOT ON KANRETHAD EBONLOCKE FIGHT!")]
        public bool KanrethadEbonlockeFight
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Destruction: Kanrethad Ebonlocke")]
        [DisplayName("Use CC on Doom Lords")]
        [Description("Will use CC (Banish/Fear) on Doom Lords")]
        public bool KanrethadEbonlockeFightDoomLordCC
        {
            get;
            set;
        }

        #endregion PvE: Destruction: Kanrethad Ebonlocke

        #region PvP: Destruction


        #endregion

        #region Experimental

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Experimental")]
        [DisplayName("Lag Reduction")]
        [Description("Likely cause issues if your in game latency is high.")]
        public bool EnableLagFix { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Experimental")]
        [DisplayName("Enable Pet Abilities")]
        [Description("Will use Command Demon to control pet abilities.")]
        public bool UsePetAbilities_Experimental
        {
            get;
            set;
        }

        #endregion
    }
}