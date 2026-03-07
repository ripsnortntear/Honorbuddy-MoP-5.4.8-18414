#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-06-21 09:58:01 +0200 (Fr, 21 Jun 2013) $
 * $ID$
 * $Revision: 1546 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/DruidSettings.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC
using System.ComponentModel;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class DruidSettings : Styx.Helpers.Settings
    {
        public DruidSettings()
            : base(PRSettings.SettingsPath + "_Druid.xml")
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
        [DisplayName("Innervate Mana")]
        [Description("Innervate will be used when your mana drops below this value")]
        public int InnervateMana { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Auto Rebirth")]
        [Description("If enabled, will auto rebirth tank")]
        public bool AutoRebirth { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Common")]
        [DisplayName("Rejuvenation")]
        [Description("Rejuvenation at x %")]
        public int HP_Rejuvenation { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Common")]
        [DisplayName("Healing Touch")]
        [Description("Healing Touch at x % ")]
        public int HP_Healing_Touch { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Barkskin")]
        [Description("Barkskin at % ")]
        public int HP_Barkskin { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Might of Ursoc")]
        [Description("Might of Ursoc at % ")]
        public int HP_MoUrsoc { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Tranquility HP")]
        [Description("Tranquility at x % ")]
        public int HP_Tranquility { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Common")]
        [DisplayName("Tranquility Units")]
        [Description("Tranquility at x units ")]
        public int Units_Tranquility { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Mushroom HP")]
        [Description("Detonate/Bloom at x % ")]
        public static int HP_Mushroom { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Common")]
        [DisplayName("Mushroom Units")]
        [Description("Detonate/Bloom Mushroom at x units ")]
        public int Units_DMushroom { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Wild Mushrooms")]
        [Description("When this is set to true, Pure will automaticly use Wild Mushrooms")]
        public bool UseMushrooms
        {
            get;
            set;
        }

        #endregion Common  - Checked (29/12/2012)

        #region Balance  - Checked (29/12/2012)

        /* Removed - Millz
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Balance")]
        [DisplayName("Starfall")]
        [Description("Use Starfall.")]
        public bool UseStarfall { get; set; }
        */

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Force of Nature")]
        [Description("When this is set to true, Pure will automaticly use Force of Nature")]
        public bool UseForceofNature
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Incarnation")]
        [Description("When this is set to true, Pure will automaticly use Incarnation")]
        public bool UseIncarnation
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Nature's Vigil")]
        [Description("When this is set to true, Pure will automaticly use Nature's Vigil")]
        public bool UseNvigil
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Nature's Swiftness")]
        [Description("When this is set to true, Pure will automaticly use Nature's Swiftness")]
        public bool UsNSwiftness
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Celestial")]
        [Description("When this is set to true, Pure will automaticly use Celestial Alignment")]
        public bool UseCelestial
        {
            get;
            set;
        }
        /*
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Symbiosis")]
        [Description("When this is set to true, Pure will automaticly use Symbiosis")]
        public bool UseSymbiosis
        {
            get;
            set;
        }*/

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Multi-DoT In Single Target")]
        [Description("When this is set to true, Pure will multi-dot while in single target mode.")]
        public bool MultiDoTInSingleTarget
        {
            get;
            set;
        }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Astral Communion")]
        [Description("Use Astral Communion before battle starts.")]
        public bool UseAstralCommunion
        {
            get;
            set;
        }
        /*
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Balance")]
        [DisplayName("Use Heart of the Wild")]
        [Description("Use Heart of the Wild")]
        public bool UseHotW
        {
            get;
            set;
        }*/

        #endregion Balance  - Checked (29/12/2012)

        #region Resto  - Checked (18/5/2012)
         //Last Edited by Mirabis
        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Restoration")]
        [DisplayName("Incarnation HP")]
        [Description("Incarnation at x % ")]
        public int HP_Incarnation { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Restoration")]
        [DisplayName("Incarnation Units")]
        [Description("Incarnation at x units ")]
        public int Units_Incarnation { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Restoration")]
        [DisplayName("Nature's Vigil HP")]
        [Description("Nature's Vigil at x HP ")]
        public int HP_NVigil { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Restoration")]
        [DisplayName("Nature's Vigil Units")]
        [Description("Nature's Vigil at x units ")]
        public int Units_NVigil { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Force of Nature HP")]
        [Description("Force of Nature at x % ")]
        public int HP_FoN { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Common")]
        [DisplayName("Force of Nature Units")]
        [Description("Force of Nature at x units ")]
        public int Units_FoN { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("Restoration")]
        [DisplayName("Wild Growth HP")]
        [Description("Wild Growth at x % ")]
        public int HP_WG { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Restoration")]
        [DisplayName("Wild Growth Units")]
        [Description("Wild Growth at x units ")]
        public int Units_WG { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Swiftmend HP")]
        [Description("Swiftmend at x % ")]
        public int HP_Swiftmend { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Nourish HP")]
        [Description("Nourish at x % ")]
        public int HP_Nourish { get; set; }

        //Lifebloom - Health
        [Setting]
        [Styx.Helpers.DefaultValue(3000)]
        [Category("Restoration")]
        [DisplayName("Lifebloom Refresh")]
        [Description("*milliseconds of Aura time left before refreshing Lifebloom")]
        public int RLifebloom { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Restoration")]
        [DisplayName("Cenarion Ward")]
        [Description("% to cast Cenarion Ward")]
        public int HP_CW { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Restoration")]
        [DisplayName("Regrowth")]
        [Description("Regrowth at x % ")]
        public int HP_Regrowth { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Restoration")]
        [DisplayName("Ironbark")]
        [Description("Ironbark at % ")]
        public int HP_IronBark { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Restoration")]
        [DisplayName("Nature's Swiftness")]
        [Description("Nature's Swiftness + HT at % ")]
        public int HP_NSHT { get; set; }

        //[Setting]
        //[DefaultValue(3)]
        //[Category("Restoration")]
        //[DisplayName("Tranquility Count")]
        //[Description("Tranquility will be used when count of party members whom health is below Tranquility health mets this value ")]
        //public int TranquilityCount { get; set; }

        //[Setting]
        //[DefaultValue(65)]
        //[Category("Restoration")]
        //[DisplayName("Swiftmend Health")]
        //[Description("Swiftmend will be used at this value")]
        //public int Swiftmend { get; set; }

        //[Setting]
        //[DefaultValue(80)]
        //[Category("Restoration")]
        //[DisplayName("Wild Growth Health")]
        //[Description("Wild Growth will be used at this value")]
        //public int WildGrowthHealth { get; set; }

        //[Setting]
        //[DefaultValue(2)]
        //[Category("Restoration")]
        //[DisplayName("Wild Growth Count")]
        //[Description("Wild Growth will be used when count of party members whom health is below Wild Growth health mets this value ")]
        //public int WildGrowthCount { get; set; }

        //[Setting]
        //[DefaultValue(70)]
        //[Category("Restoration")]
        //[DisplayName("Regrowth Health")]
        //[Description("Regrowth will be used at this value")]
        //public int Regrowth { get; set; }

        //[Setting]
        //[DefaultValue(60)]
        //[Category("Restoration")]
        //[DisplayName("Healing Touch Health")]
        //[Description("Healing Touch will be used at this value")]
        //public int HealingTouch { get; set; }

        //[Setting]
        //[DefaultValue(75)]
        //[Category("Restoration")]
        //[DisplayName("Nourish Health")]
        //[Description("Nourish will be used at this value")]
        //public int Nourish { get; set; }

        //[Setting]
        //[DefaultValue(90)]
        //[Category("Restoration")]
        //[DisplayName("Rejuvenation Health")]
        //[Description("Rejuvenation will be used at this value")]
        //public int Rejuvenation { get; set; }

        //[Setting]
        //[DefaultValue(80)]
        //[Category("Restoration")]
        //[DisplayName("Tree of Life Health")]
        //[Description("Tree of Life will be used at this value")]
        //public int TreeOfLifeHealth { get; set; }

        //[Setting]
        //[DefaultValue(3)]
        //[Category("Restoration")]
        //[DisplayName("Tree of Life Count")]
        //[Description("Tree of Life will be used when count of party members whom health is below Tree of Life health mets this value ")]
        //public int TreeOfLifeCount { get; set; }

        //[Setting]
        //[DefaultValue(70)]
        //[Category("Restoration")]
        //[DisplayName("Barkskin Health")]
        //[Description("Barkskin will be used at this value")]
        //public int Barkskin { get; set; }

        #endregion Resto  - Checked (29/12/2012)

        #region Feral  - Checked (29/12/2012)

        //[Setting]
        //[DefaultValue(50)]
        //[Category("Feral")]
        //[DisplayName("Barkskin Health")]
        //[Description("Barkskin will be used at this value. Set this to 100 to enable on cooldown usage.")]
        //public int FeralBarkskin { get; set; }

        //[Setting]
        //[DefaultValue(55)]
        //[Category("Feral")]
        //[DisplayName("Survival Instincts Health")]
        //[Description("SI will be used at this value. Set this to 100 to enable on cooldown usage. (Recommended: 55)")]
        //public int SurvivalInstinctsHealth { get; set; }

        //[Setting]
        //[DefaultValue(30)]
        //[Category("Feral PvP")]
        //[DisplayName("Frenzied Regeneration Health")]
        //[Description("FR will be used at this value. Set this to 100 to enable on cooldown usage. (Recommended: 30 if glyphed. 15 if not.)")]
        //public int FrenziedRegenerationHealth { get; set; }

        //[Setting]
        //[DefaultValue(0)]
        //[Category("Form Selection")]
        //[DisplayName("Form Selection")]
        //[Description("Form Selection!")]
        //public int Shapeform { get; set; }

        //[Setting]
        //[DefaultValue(60)]
        //[Category("Feral")]
        //[DisplayName("Predator's Swiftness heal")]
        //[Description("Healing with Predator's Swiftness will be used at this value")]
        //public int NonRestoprocc { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Disable Healing for Balance and Feral")]
        //public bool RaidCatProwl { get; set; }

        //[Setting]
        //[DefaultValue(15)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Predator's Swiftness (Balance and Feral)")]
        //public int RaidCatProccHeal { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Interrupt")]
        //[Description("Automatically interrupt spells while in an instance if this value is set to true.")]
        //public bool Interrupt { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Buff raid with Motw")]
        //[Description("If set to true, we will buff the raid automatically.")]
        //public bool BuffRaidWithMotw { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Warn if not behind boss")]
        //public bool CatRaidWarning { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Use dash as gap closer")]
        //public bool CatRaidDash { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Use Stampeding Roar")]
        //[Description("If set to true, it will cast Stampeding Roar to close gap to target.")]
        //public bool CatRaidStampeding { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral PvP")]
        //[DisplayName("Cat - Stealth Pull")]
        //[Description("Always try to pull while in stealth. If disabled it pulls with FFF instead.")]
        //public bool CatNormalPullStealth { get; set; }

        //[Setting]
        //[DefaultValue(4)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Adds to AOE")]
        //[Description("Number of adds needed to start Aoe rotation.")]
        //public int CatRaidAoe { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Auto Berserk")]
        //[Description("If set to true, it will cast Berserk automatically to do max dps.")]
        //public bool CatRaidBerserk { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Auto Tiger's Fury")]
        //[Description("If set to true, it will cast Tiger's Fury automatically to do max dps.")]
        //public bool CatRaidTigers { get; set; }

        //[Setting]
        //[DefaultValue(false)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Rebuff infight")]
        //[Description("If set to true, it will rebuff Mark of the Wild infight.")]
        //public bool CatRaidRebuff { get; set; }

        //[Setting]
        //[DefaultValue(false)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Rez infight")]
        //[Description("If set to true, it will rez while infight.")]
        //public bool CatRaidRezz { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Cat - Feral Charge")]
        //[Description("Use Feral Charge to close gaps. It should handle bosses where charge is not" +
        //             "possible || best solution automatically.")]
        //public bool CatRaidUseFeralCharge { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Bear - Feral Charge")]
        //[Description("Use Feral Charge to close gaps. It should handle bosses where charge is not" +
        //             "possible || best solution automatically.")]
        //public bool BearRaidUseFeralCharge { get; set; }

        //[Setting]
        //[DefaultValue(2)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Bear - Adds to AOE")]
        //[Description("Number of adds needed to start Aoe rotation.")]
        //public int BearRaidAoe { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Auto Berserk")]
        //[Description("If set to true, it will cast Berserk automatically to do max threat.")]
        //public bool BearRaidBerserk { get; set; }

        //[Setting]
        //[DefaultValue(false)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Bear - Berserk Burst")]
        //[Description("If set to true, it will SPAM MANGLE FOR GODS SAKE while Berserk is active.")]
        //public bool BearRaidBerserkFun { get; set; }

        //[Setting]
        //[DefaultValue(true)]
        //[Category("Feral Raid / Instance")]
        //[DisplayName("Bear - Auto defensive cooldowns")]
        //[Description("If set to true, it will cast defensive cooldowns automatically.")]
        //public bool BearRaidCooldown { get; set; }

        #endregion Feral  - Checked (29/12/2012)

        #region GuardianDruid

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Guardian")]
        [DisplayName("Frenzied Regeneration Health")]
        [Description("FR will be used at this value. Set this to 100 to enable on cooldown usage. (Recommended: 30 if glyphed. 15 if not.)")]
        public int FrenziedHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Guardian")]
        [DisplayName("Might of Ursoc HP")]
        [Description("Might of Ursoc will be used at this HP %)")]
        public int UrsocHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Guardian")]
        [DisplayName("Renewal Health")]
        [Description("Renewal will be used at this % HP.)")]
        public int RenewalHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Guardian")]
        [DisplayName("Survival Instinct Health")]
        [Description("Survival Instinct will be used at this HP %)")]
        public int SiHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Guardian")]
        [DisplayName("Barkskin Health")]
        [Description("Barkskin will be used at this HP %")]
        public int BarkskinHP { get; set; }

        #endregion GuardianDruid
    }
}