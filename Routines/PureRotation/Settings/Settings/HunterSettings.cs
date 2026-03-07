#region Revision info

/*
 * $Author: treek $
 * $Date: 2013-06-09 07:53:04 +0200 (So, 09 Jun 2013) $
 * $ID$
 * $Revision: 1506 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/HunterSettings.cs $
 * $LastChangedBy: treek $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC
/*****************
2013-02-17 - Moved many settings from BM to common
			 Uncommented and added a few additional settings
*****************/

using System.ComponentModel;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class HunterSettings : Styx.Helpers.Settings
    {
        public HunterSettings()
            : base(PRSettings.SettingsPath + "_Hunter.xml")
        {
        }

        internal enum RotationSelection
        {
            PvETest,
            None
        }

        [Setting]
        [Styx.Helpers.DefaultValue(RotationSelection.PvETest)]
        [Category("Rotation Selection")]
        [DisplayName(@"Rotation")]
        [Description("Select the Rotation to toggle to when Rotation Hotkey is pressed.")]
        public RotationSelection RotationSelections { get; set; }

        #region Category : AoECount

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Common")]
        [DisplayName(@"AoE Add Count")]
        [Description("Will use AoE on Count number you choose.)")]
        public int AoECount { get; set; }

        [Styx.Helpers.DefaultValue(3)]
        [Category("Common")]
        [DisplayName(@"AoE Add Count For Explosive Trap")]
        [Description("Will use AoE Explosive Trap on Count number you choose.)")]
        public int ExplosiveTrapCount { get; set; }

        #endregion Category : AoECount

        #region Pet  - Checked (29/12/2012)

        //[Setting]
        //[Styx.Helpers.DefaultValue((int)PetSlot.FirstSlot)]
        //[Category("Pet")]
        //[DisplayName("Pet Slot")]
        //[Description("CLU will attempt to call the pet in the specified pet slot.")]
        //public PetSlot PetSlotSelection
        //{
        //    get;
        //    set;
        //}

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Pet")]
        [DisplayName(@"Mend Pet Percent")]
        [Description("Pure will use Mend Pet at this Pet Health Percent.")]
        public double MendPetPercent
        {
            get;
            set;
        }

        //[Setting]
        //[Styx.Helpers.DefaultValue(false)]
        //[Category("Pet")]
        //[DisplayName("Revive Pet in Combat")]
        //[Description("If set to true CLU Will Revive Pet in combat")]
        //public bool ReviveInCombat
        //{
        //    get;
        //    set;
        //}

        //[Setting]
        //[Styx.Helpers.DefaultValue(true)]
        //[Category("Pet")]
        //[DisplayName("Use Heart of the Phoenix")]
        //[Description("If set to true CLU attempt to bring your pet back to lige usinf Heart of the Phoenix")]
        //public bool UseHeartofthePhoenix
        //{
        //    get;
        //    set;
        //}

        #endregion Pet  - Checked (29/12/2012)

        #region Common  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName(@"Misdirection")]
        [Description("If set to true Pure will use Misdirection on high threat with the following target priority: Focus, Tank, Pet")]
        public bool UseMisdirection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName(@"Tranquilizing Shot")]
        [Description("If set to true Pure Will use Tranquilizing Shot when your target has a clearable buff.")]
        public bool UseTranq { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName(@"Use Revive Pet")]
        [Description("Will be used")]
        public bool UseRevivePet { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(40)]
        [Category("Common")]
        [DisplayName(@"Healthstone Percent")]
        [Description("Will use Healthstone at the set Health Percent")]
        public int HealthstonePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Common")]
        [DisplayName(@"Health Potion Percent")]
        [Description("Will use Master Healing Potion at the set Health Percent")]
        public int PotionPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Common")]
        [DisplayName(@"Deterrence Percent")]
        [Description("Will use Deterrence at the set Health Percent")]
        public int DeterrencePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName(@"Use Exhilaration")]
        [Description("Will be used")]
        public bool UseExhilaration { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Common")]
        [DisplayName(@"Exhilaration Percent")]
        [Description("Will use Exhilaration at the set Health Percent")]
        public int ExhilarationHP { get; set; }
		
        [Setting]
        [Styx.Helpers.DefaultValue(300)]
        [Category("Common")]
        [DisplayName(@"Wait Time")]
        [Description("How long to delay your shots (in MS) for higher priority shots (Kill Command/Chimera Shot/Explosive Shot")]
        public int WaitTime { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName(@"Interrupt all units")]
        [Description("Scans and attempts to interrupt all near-by units")]
        public bool UseUrgentInterrupts { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName(@"Force Aspect of the (Iron) Hawk")]
        [Description("Forces Aspect of the (Iron) Hawk during combat")]
        public bool ForceHawk { get; set; }

        #endregion Common  - Checked (29/12/2012)
        #region ShotSelection

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use Stampede")]
        [Description("If set to true Pure will use Stampede")]
        public bool UseStampede { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use RapidFire")]
        [Description("If set to true Pure will use Rapid Fire")]
        public bool UseRapidFire { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use Readiness")]
        [Description("If set to true Pure will use Readiness")]
        public bool UseReadiness { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use Glaive Toss")]
        [Description("If set to true Pure will use Glaive Toss")]
        public bool UseGlaiveToss { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use Rabid")]
        [Description("If set to true Pure will use Rabid")]
        public bool UseRabid { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use Dire Beast")]
        [Description("If set to true Pure will use Dire Beast")]
        public bool UseDireBeast { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ShotSelection")]
        [DisplayName(@"Use Lynx Rush")]
        [Description("If set to true Pure will use Lynx Rush")]
        public bool UseLynxRush { get; set; }

        #endregion ShotSelection

        #region Marksmanship  - Checked (29/12/2012)

        #endregion Marksmanship  - Checked (29/12/2012)

        #region BeastMastery - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("PVE:BeastMastery")]
        [DisplayName(@"Use Bestial Wrath")]
        [Description("If set to true Pure will use Bestial Wrath")]
        public bool UseBestialWrath
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("PVE:BeastMastery")]
        [DisplayName(@"Hold Frenzy stacks for Bestial Wrath")]
        [Description("Hold stacks of Frenzy when Bestial Wraths cooldown is X seconds or shorter")]
        public int SecsToHoldFrenzyStacks
        {
            get;
            set;
        }
		
        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("PVE:BeastMastery")]
        [DisplayName(@"Quasi-AoE Count")]
        [Description("Will add Explosive Trap and Multi-Shot to the single target rotation when agro mob count is equal to or higher then this value (and less than AoE count).")]
        public int BMMultiShotCount { get; set; }

        #endregion BeastMastery - Checked (29/12/2012)

        #region Survival  - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(2)]
        [Category("PVE:Survival")]
        [DisplayName(@"Quasi-AoE Count")]
        [Description("Will replace Black Arrow and Arcane Shot in the single target rotation with Explosive Trap and Multi Shot when agro mob count is equal to or higher then this value (and less than AoE count).")]
        public int SurvMultiShotCount { get; set; }

        #endregion Survival  - Checked (29/12/2012)
    }
}