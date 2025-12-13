#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 16:25:58 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 208 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Druid/DruidCommon.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI.Settings;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Classes.Druid
{
    public enum DruidTalents
    {
        FelineSwiftness = 1,
        DispacerBeast,
        WildCharge,
        YserasGift,
        Renewal,
        CenarionWard,
        FaerieSwarm,
        MassEntanglement,
        Typhoon,
        SoulOfTheForest,
        Incarnation,
        ForceOfNature,
        DisorientingRoar,
        UrsolsVortex,
        MightyBash,
        HeartOfTheWild,
        DreamOfCenarius,
        NaturesVigil
    }

    public static class DruidCommon
    {
       
        #region booleans

        public static bool IsTreeofLife { get { return StyxWoW.Me.Shapeshift == ShapeshiftForm.TreeOfLife; } }

        public static bool IsTreeofLifeOrSouloftheForest { get { return IsTreeofLife || StyxWoW.Me.HasAura(RotationBase.SouloftheForest); } }

        public static bool TreeofLifeConserveMana { get { return StyxWoW.Me.ManaPercent < ToLConserveManaPct && CooldownTracker.SpellOnCooldown(RotationBase.Innervate); } }

        #endregion booleans

        #region Settings

        private static DruidSettings Setting { get { return OracleSettings.Instance.Druid; } }

        public static HandleTankBuff HandleBuffonTank { get { return Setting.HandleBuffonTank; } }

        // rejuv blanket
        public static bool RejuvBlanketEnable { get { return Setting.RejuvBlanketEnable; } }

        public static int RejuvBlanketCount { get { return Setting.RejuvBlanketCount; } }

        public static int RejuvBlanketManaPct { get { return Setting.RejuvBlanketManaPct; } }

        public static int ToLConserveManaPct { get { return Setting.ToLConserveManaPct; } }

        public static int ShroomTimecheck { get { return Setting.ShroomTimecheck; } }

        public static int ShroomMaxDistancecheck { get { return Setting.ShroomMaxDistancecheck; } }

        public static UnitBuffSelection UnitBuffSelection { get { return Setting.UnitBuffSelection; } }

        public static int InnervatePercent { get { return Setting.InnervatePercent; } }

        public static int InnervateHymnOfHopePercent { get { return Setting.InnervateHymnOfHopePercent; } }

        public static int RegrowthToLPercent { get { return Setting.RegrowtToLPercent; } }

        public static int RegrowtClearcastingPercent { get { return Setting.RegrowtClearcastingPercent; } }

        public static int LifebloomstacksForToL { get { return Setting.LifebloomstacksForToL; } }

        public static int NaturesSwiftnessHealingTouchPct { get { return Setting.NaturesSwiftnessHealingTouchPct; } }

        public static int CenarionWardPct { get { return Setting.CenarionWardPct; } }

        public static int RenewalPct { get { return Setting.RenewalPct; } }

        public static int BarkskinPct { get { return Setting.BarkskinPct; } }

        public static int IronBarkPct { get { return Setting.IronBarkPct; } }

        public static int MightofUrsocPct { get { return Setting.MightofUrsocPct; } }

        public static int ForceOfNaturePct { get { return Setting.ForceOfNaturePct; } }

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)
        public static int TranquilityPct { get { return Setting.TranquilityPct; } }

        public static int TranquilityLimit { get { return Setting.TranquilityLimit; } }

        public static int WildGrowthPct { get { return (IsTreeofLifeOrSouloftheForest ? 100 : Setting.WildGrowthPct); } }

        public static int WildGrowthLimit { get { return (IsTreeofLifeOrSouloftheForest ? 0 : Setting.WildGrowthLimit); } }

        public static int SwiftmendAoEPct { get { return Setting.SwiftmendAoEPct; } }

        public static int SwiftmendAoeLimit { get { return Setting.SwiftmendAoeLimit; } }

        public static int WildMushroomPct { get { return Setting.WildMushroomPct; } }

        public static int WildMushroomLimit { get { return Setting.WildMushroomLimit; } }

        // we ignore all settings and start healing like hell!!!
        public static int UrgentHealthPercentage { get { return Setting.UrgentHealthPercentage; } }

        // Oh shit moments!

        public static int NaturesVigilPct { get { return Setting.NaturesVigilPct; } }

        public static int NaturesVigilLimit { get { return Setting.NaturesVigilLimit; } }

        public static int ForceOfNatureAoEPct { get { return Setting.ForceOfNatureAoEPct; } }

        public static int ForceOfNatureAoeLimit { get { return Setting.ForceOfNatureAoeLimit; } }

        public static int IncarnationPct { get { return TreeofLifeConserveMana ? 100 : Setting.IncarnationPct; } }

        public static int IncarnationLimit { get { return TreeofLifeConserveMana ? 0 : Setting.IncarnationLimit; } }

        public static int GenesisPct { get { return Setting.GenesisPct; } }

        public static int GenesisLimit { get { return Setting.GenesisLimit; } }

        public static int HeartOfTheWildPct { get { return Setting.HeartOfTheWildPct; } }

        public static int HeartOfTheWildLimit { get { return Setting.HeartOfTheWildLimit; } }

        #endregion Settings

        public static bool HasTalent(DruidTalents tal)
        {
            return TalentManager.IsSelected((int)tal);
        }

        #region Druid CreateClusteredHealBehavior

        public static void LoadClusterSpells()
        {
            OracleRoutine.Instance.ClusteredSpells.Clear();

            var key = 1;

            // What is this going to do ? its going to cast the shroom at the location of the cluster
            // ... so i will need to check that the shroom isnt already sitting inside the cluster.
            // ... if it hits the shroom amount it will bloom whatever is in it...
            // TODO: Add calculation to check current shrooms power.
            // TODO: Ensure we check that the bloom is ready before blowing.

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.WildMushroom, SpellType.GroundEffect, 2, 0, 95)); key++;
            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.WildMushroomBloom, SpellType.GroundEffect, WildMushroomLimit, 0, WildMushroomPct)); key++;

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Swiftmend, SpellType.GroundEffect, SwiftmendAoeLimit, 0, SwiftmendAoEPct)); key++;

            if (DruidCommon.HasTalent(DruidTalents.ForceOfNature))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.ForceOfNature, SpellType.Proximity, ForceOfNatureAoeLimit, 0, ForceOfNatureAoEPct)); key++; }

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.WildGrowth, SpellType.Proximity, WildGrowthLimit, 0, WildGrowthPct, ret => StyxWoW.Me.HasAura(RotationBase.Harmony) || IsTreeofLife)); key++;
            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Tranquility, SpellType.NearbyLowestHealth, TranquilityLimit, 0, TranquilityPct, ret => !IsTreeofLife));
        }

        #endregion Druid CreateClusteredHealBehavior

        #region Druid CreateCooldownBehavior

        public static void LoadCooldownSpells()
        {
            OracleRoutine.Instance.CooldownSpells.Clear();

            var key = 1;

            // Cooldowns capture the units around us that are in trouble (40yrds).
            // ClusterSpell accepts a delegate to pass into the composite during hook creation.

            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.Genesis, SpellType.NearbyLowestHealth, GenesisLimit, 0, GenesisPct)); key++;
            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.HeartOfTheWild, SpellType.NearbyLowestHealth, HeartOfTheWildLimit, 0, HeartOfTheWildPct)); key++;

            if (DruidCommon.HasTalent(DruidTalents.Incarnation))
            { OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.Incarnation, SpellType.NearbyLowestHealth, IncarnationLimit, 0, IncarnationPct)); key++; }
            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.NaturesVigil, SpellType.NearbyLowestHealth, NaturesVigilLimit, 0, NaturesVigilPct));
        }

        #endregion Druid CreateCooldownBehavior

        #region Efflorescence

        #region Unit Targets

        private static WoWUnit Tank { get { return OracleTanks.MainTank; } }

        private static WoWUnit SecondTank { get { return OracleTanks.AssistTank; } }

        private static WoWUnit UnitBuffSelectionTarget
        {
            get
            {
                switch (UnitBuffSelection)
                {
                    case UnitBuffSelection.None:
                        return null;
                    case UnitBuffSelection.Tank:
                        return (OracleRoutine.IsViable(Tank) && !Tank.IsMe ? Tank : null);
                    case UnitBuffSelection.SecondTank:
                        return (OracleRoutine.IsViable(SecondTank) && !SecondTank.IsMe ? SecondTank : null);
                    case UnitBuffSelection.You:
                        return StyxWoW.Me;
                }
                return null;
            }
        }

        #endregion Unit Targets

        public static readonly WaitTimer ShroomWaitTimer = new WaitTimer(TimeSpan.FromSeconds(ShroomTimecheck));

        public static WoWUnit Shroom { get; set; }

        private static WoWPoint ShroomPoint { get; set; }

        public static Composite CreateEfflorescenceBehaviour()
        {
            // 0. check that we has the glyph and we do not currently have a shroom out
            // Check the distance from our target if its > 25 move it. other wise goto 2.
            // 1. Place the mushrrom on the target
            // 2. check that it has been there for the specified time
            // 3. Detonate it to gain Effo .. no no..this is not needed !!
            // 4. retuirn to 1.

            const int WildMushroom = RotationBase.WildMushroom;
            const int WildMushroomBloom = RotationBase.WildMushroomBloom;

            return new Decorator(ret =>
            {
                
                if (!TalentManager.HasGlyph("Efflorescence"))
                    return false;

                if (UnitBuffSelection == UnitBuffSelection.None)
                    return false;

                if (UnitBuffSelectionTarget == null)
                    return false;
                
                // we only want to wait if the shroom is out.
                if (!ShroomWaitTimer.IsFinished)
                    return false;


                if (UnitBuffSelection != UnitBuffSelection.You && UnitBuffSelectionTarget != null && UnitBuffSelectionTarget.IsMe)
                    return false;

             
                return true;
            },
                new Sequence(
                    new Action(ret => Shroom = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(p => p.CreatedByUnitGuid == StyxWoW.Me.Guid && p.Entry == 47649)),
                // Begin
                    new PrioritySelector(new Decorator(ret => Shroom != null && UnitBuffSelectionTarget.Location.Distance(Shroom.Location) < ShroomMaxDistancecheck,
                            new Action(delegate
                            {
                                        //// ok we have a viable shroom, if the timer has finished then lets blow it!
                                        //if(!ShroomWaitTimer.IsFinished) return RunStatus.Success;

                                        //SpellManager.Cast(WildMushroomBloom);

                                        return RunStatus.Success;
                            })),
                            new Sequence(
                               new Action(delegate
                              {
                                  ShroomPoint = WoWPoint.Empty;

                                  //Logger.Output(" Shroommmmmmmmmmmmmmmmmmmmmmmmmmmmm point is: {0}", ShroomPoint);

                                  // Don't have the glyph gtfo
                                  if (!TalentManager.HasGlyph("the Sprouting Mushroom")) return RunStatus.Success;

                                  // dem ticks mhmm dem ticks!
                                  SpellManager.Cast("Lifeblood");

                                  // get a good position for the shroom
                                  var tpos = UnitBuffSelectionTarget.Location;
                                  var trot = UnitBuffSelectionTarget.Rotation;
                                  ShroomPoint = WoWMathHelper.CalculatePointInFront(tpos, trot, 3.2f);

                                  return RunStatus.Success;
                              }),
                                 
                              new PrioritySelector(new Decorator(ret => TalentManager.HasGlyph("the Sprouting Mushroom") && ShroomPoint != WoWPoint.Empty, 
                                  Spell.CastOnGround(WildMushroom, on => ShroomPoint)), Spell.Cast(WildMushroom, on => UnitBuffSelectionTarget)))
                      ),
                        //Finish
                      new Action(delegate
                      {
                          // always reset the timer - lets assume we blew the mushroom or placed a new one, if we blew it it will place another.
                          ShroomWaitTimer.Reset();

                          return RunStatus.Success;
                      }))
                    );
        }

        #endregion Efflorescence
    }
}