#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-18 20:18:58 +1000 (Wed, 18 Sep 2013) $
 * $ID$
 * $Revision: 230 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/DruidSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Classes;
using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class DruidSettings : Styx.Helpers.Settings
    {
        public DruidSettings()
            : base(OracleSettings.SettingsPath + "_Druid.xml")
        {
        }

        #region Category: Common

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Common")]
        [DisplayName("Urgent Health Percentage")]
        [Description("we ignore all settings and start healing like hell!!!")]
        // we ignore all settings and start healing like hell!!!
        public int UrgentHealthPercentage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Disable Nourish")]
        [Description("Disable nourish for High end Druids like Mirabis ;)")]
        public bool DisableNourish { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Enable Tier 16 Support")]
        [Description("Enable/Disable T16 Support, sagemender, etc")]
        public bool EnableTier16Support { get; set; }

        

        [Setting]
        [Styx.Helpers.DefaultValue(HandleTankBuff.Always)]
        [Category("Common")]
        [DisplayName("LifeBloom on Tank")]
        [Description(" Enable Disable Life Bloom on tank, useful when tank is not in LoS.")]
        public HandleTankBuff HandleBuffonTank { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Tree of Life (Incarnation)")]
        [Description("Mana Percent to use Incarnation (Tree of Life) for mana conservation (Innervate must be on cooldown.)")]
        public int ToLConserveManaPct { get; set; }

       

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Common")]
        [DisplayName("Tree of Life (Incarnation)")]
        [Description("Amount of stacks to stack on Healtargets during Tree of Life (Incarnation)")]
        public int LifebloomstacksForToL { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Common")]
        [DisplayName("Innervate")]
        [Description("Mana Percent to use Innervate on yourself")]
        public int InnervatePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("Common")]
        [DisplayName("Innervate")]
        [Description("Health Percent to use Innervate on During Hymn of Hope")]
        public int InnervateHymnOfHopePercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Common")]
        [DisplayName("Regrowth")]
        [Description("Health Percent to use Regrowth During Tree Of Life")]
        public int RegrowtToLPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Dynamic spell prioritys")]
        [DisplayName("Regrowth")]
        [Description("Health Percent must be greater than this for Regrowth to be added to Dynamic spell prioritys.")]
        public int RegrowthPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Dynamic spell prioritys")]
        [DisplayName("Healing Touch")]
        [Description("Health Percent must be greater than this for Healing Touch to be added to Dynamic spell prioritys.")]
        public int HealingTouchPct { get; set; }

        
        [Setting]
        [Styx.Helpers.DefaultValue(96)]
        [Category("Common")]
        [DisplayName("Regrowth")]
        [Description("Health Percent to use Regrowth when you have the Clearcasting Proc")]
        public int RegrowtClearcastingPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Common")]
        [DisplayName("Natures Swiftness")]
        [Description("Health Percent to use Natures Swiftness for Healing Touch")]
        public int NaturesSwiftnessHealingTouchPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Common")]
        [DisplayName("Cenarion Ward")]
        [Description("Health Percent to use Cenarion Ward on HealTarget")]
        public int CenarionWardPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("Common")]
        [DisplayName("Renewal")]
        [Description("Health Percent to use Renewal")]
        public int RenewalPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Barkskin")]
        [Description("Health Percent to use Barkskin on yourself")]
        public int BarkskinPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("Common")]
        [DisplayName("Iron Bark")]
        [Description("Health Percent to use Iron Bark on Healtarget")]
        public int IronBarkPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(20)]
        [Category("Common")]
        [DisplayName("Might of Ursoc")]
        [Description("Health Percent to use Might of Ursoc on yourself")]
        public int MightofUrsocPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Common")]
        [DisplayName("Force Of Nature")]
        [Description("Health Percent to use Force Of Nature on Healtarget (SingleTarget)")]
        public int ForceOfNaturePct { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("Common")]
        [DisplayName("Force Of Nature")]
        [Description("Wait *at least* this long before summoning another treant, I think Efflorescence lasts around 6 seconds so wait for this time before we shoot out another.")]
        public int ForceOfNatureWaitTime { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Rejuvenation Blanket Mode")]
        [DisplayName("Rejuvenation Count")]
        [Description("Amount of players to blanket with Rejuvenation")]
        public int RejuvBlanketCount { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Rejuvenation Blanket Mode")]
        [DisplayName("Rejuvenation Mana Pct")]
        [Description("Mana Percent Must be greater than this setting for Rejuvenation blanket mode")]
        public int RejuvBlanketManaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Rejuvenation Blanket Mode")]
        [DisplayName("Enable/Disable")]
        [Description("Enable/Disable the use of Rejuvenation Blanket Mode")]
        public bool RejuvBlanketEnable { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(UnitBuffSelection.Tank)]
        [Category("Mushroom Efflorescence")]
        [DisplayName("Target")]
        [Description(" When this is enabled (ie: not None) it will cast Wild Mushroom on the target. It chooses the target that you select.")]
        public UnitBuffSelection UnitBuffSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(4)]
        [Category("Mushroom Efflorescence")]
        [DisplayName("Waitime between checks")]
        [Description("Check for wild mushroom placement every x seconds.")]
        public int ShroomTimecheck { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(12)]
        [Category("Mushroom Efflorescence")]
        [DisplayName("Max Distance from target")]
        [Description("The Max distance allowed before moving the mushroom closer to the target")]
        public int ShroomMaxDistancecheck { get; set; }

        #endregion Category: Common

        #region Category: Spec

        #endregion Category: Spec

        #region Category: AoE Healing Spells

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        [Setting]
        [Styx.Helpers.DefaultValue(70)]
        [Category("AoE Healing Spells")]
        [DisplayName("Tranquility")]
        [Description("Average health percent to use Tranquility. Players must be below this amount to be included in the average.")]
        public int TranquilityPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        [Category("AoE Healing Spells")]
        [DisplayName("Tranquility")]
        [Description("Number of players that have the average health percent specified above")]
        public int TranquilityLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(90)]
        [Category("AoE Healing Spells")]
        [DisplayName("Wild Growth")]
        [Description("Average health percent to use Wild Growth. Players must be below this amount to be included in the average.")]
        public int WildGrowthPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Wild Growth")]
        [Description("Number of players that have the average health percent specified above")]
        public int WildGrowthLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(98)]
        [Category("AoE Healing Spells")]
        [DisplayName("Swiftmend for AoE")]
        [Description("Average health percent to use Swiftmend for AoE. Players must be below this amount to be included in the average.")]
        public int SwiftmendAoEPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Swiftmend for AoE")]
        [Description("Number of players that have the average health percent specified above")]
        public int SwiftmendAoeLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(85)]
        [Category("AoE Healing Spells")]
        [DisplayName("Wild Mushroom")]
        [Description("Average health percent to use Wild Mushroom. Players must be below this amount to be included in the average.")]
        public int WildMushroomPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("AoE Healing Spells")]
        [DisplayName("Wild Mushroom")]
        [Description("Number of players that have the average health percent specified above")]
        public int WildMushroomLimit { get; set; }

        #endregion Category: AoE Healing Spells

        #region Category: Emergency Cooldowns

        // Oh shit moments!

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Natures Vigil")]
        [Description("Average health percent to use Natures Vigil. Players must be below this amount to be included in the average.")]
        public int NaturesVigilPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Natures Vigil")]
        [Description("Number of players that have the average health percent specified above")]
        public int NaturesVigilLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(75)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Force Of Nature for AoE")]
        [Description("Average health percent to use Force Of Nature for AoE. Players must be below this amount to be included in the average.")]
        public int ForceOfNatureAoEPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Force Of Nature for AoE")]
        [Description("Number of players that have the average health percent specified above")]
        public int ForceOfNatureAoeLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Incarnation")]
        [Description("Average health percent to use Incarnation (Tree of Life). Players must be below this amount to be included in the average.")]
        public int IncarnationPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Incarnation")]
        [Description("Number of players that have the average health percent specified above")]
        public int IncarnationLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(65)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Genesis")]
        [Description("Average health percent to use Genesis. Players must be below this amount to be included in the average.")]
        public int GenesisPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Genesis")]
        [Description("Number of players that have the average health percent specified above")]
        public int GenesisLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Genesis")]
        [Description("Number of players that need to have Rejuvenation buff")]
        public int GenesisbuffLimit { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(65)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Heart Of The Wild")]
        [Description("Average health percent to use Heart Of The Wild. Players must be below this amount to be included in the average.")]
        public int HeartOfTheWildPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Emergency Cooldowns")]
        [DisplayName("Heart Of The Wild")]
        [Description("Number of players that have the average health percent specified above")]
        public int HeartOfTheWildLimit { get; set; }




        

        #endregion Category: Emergency Cooldowns
    }
}