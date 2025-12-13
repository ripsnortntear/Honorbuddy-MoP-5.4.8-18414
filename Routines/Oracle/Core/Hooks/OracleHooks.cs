#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Hooks/OracleHooks.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using Oracle.Classes;
using Oracle.Classes.Druid;
using Oracle.Classes.Shaman;
using Oracle.Core.DataStores;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Healing.Chronicle;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities.Clusters;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI.Settings;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Core.Hooks
{
    internal static class OracleHooks
    {
        #region settings

        private static OracleSettings Settings { get { return OracleSettings.Instance; } }

        private static int ExcludePlayersAboveHP { get { return Settings.ExcludePlayersAboveHP; } }

        private static bool UseExperimentalChainHeal { get { return Settings.UseExperimentalChainHeal; } }

        #endregion settings

        #region Instantiate Hooks

        private static readonly HookExecutor ClusteredHealBehaviorHook = new HookExecutor("ClusteredHealBehavior");
        private static readonly HookExecutor CooldownsBehaviorHook = new HookExecutor("CooldownsBehaviorHook");
        private static readonly HookExecutor SingleTargetHealBehaviorHook = new HookExecutor("SingleTargetHealBehaviorHook");

        public static Composite ExecClusteredHealHook()
        {
            return ClusteredHealBehaviorHook;
        }

        public static Composite ExecCooldownsHook()
        {
            return CooldownsBehaviorHook;
        }

        public static Composite ExecSingleTargetHealHook()
        {
            return SingleTargetHealBehaviorHook;
        }

        #endregion Instantiate Hooks

        #region Single Target hooks

        public static void PopulateSTHook(WoWUnit unit, string hookName)
        {
            if (unit.HealthPercent(unit.GetHPCheckType()) > ExcludePlayersAboveHP) // TODO: better way of stopping it from healing..
            {
                return;
            }

            // Do our Calculations
            ChronicleHealing.PopulateSingleTargetOrderedSpells(unit);

            // If we have no spells..gtfo.
            if (ChronicleHealing.SingleTargetOrderedSpells.Count < 1)
            {
                Logger.Output("SingleTargetOrderedSpells: No Spells.");
                return;
            }

            // Order spells by weight.
            var OrderedSpells = ChronicleHealing.SingleTargetOrderedSpells.OrderByDescending(t => t.Item5).ToList();

            // Build Hooks
            BuildSTHook(OrderedSpells, unit, hookName);
        }

        private static void BuildSTHook(List<Tuple<string, float, float, float, float, float, ChronicleSpellType, Tuple<bool>>> orderedSpells, WoWUnit unit, string hookName)
        {
            // Spellname, TotalCalculatedHeal - healthDeficit, ManaCost, CastTime, prio, Totalheal

            var prio = new PrioritySelector();

            foreach (var spell in orderedSpells)
            {
                var spelltype = spell.Item7;
                var combatloggedheal = spell.Rest.Item1;

                Logger.PrioLogging("---> [{0} : {5}] [Deficit {1}%] [MPS: {2}] [HPS: {3}] [Oracle prio: {4}] {6}", spell.Item1, spell.Item2, spell.Item3, Math.Round(spell.Item4, 0), spell.Item5, spell.Item6, combatloggedheal ? "[Combat Log Heal (Averaged)]" : "[Calculated Heal]");

                switch (spelltype)
                {
                    case ChronicleSpellType.PeriodicDamage:
                    case ChronicleSpellType.HybridDamage:
                    case ChronicleSpellType.DirectDamage:
                        var target = Unit.GetEnemy;
                        if (OracleRoutine.IsViable(target))
                        {
                            if (spell.Item1 == "Power Word: Solace")
                            {
                                // this is here because of HB's retarded Overloads. Why can't it check the cooldown of the parent Overload!!
                                prio.AddChild(Spell.Cast(spell.Item1, on => Unit.GetEnemy, ret => !Spell.SpellOnCooldown(RotationBase.PowerWordSolace) && OracleRoutine.IsViable(target) && Unit.FaceTarget(target)));
                                break;
                            }

                            prio.AddChild(Spell.Cast(spell.Item1, on => Unit.GetEnemy, ret => OracleRoutine.IsViable(target) && Unit.FaceTarget(target), 1, true, false, false, false));
                        }

                        break;

                    case ChronicleSpellType.DirectHeal:
                        if (spell.Item1 == "Holy Word: Chastise")
                        {
                            // this is here because of HB's retarded Overloads. Why can't it check the cooldown of the parent Overload!!
                            prio.AddChild(Spell.Cast(spell.Item1, on => unit, ret => !Spell.SpellOnCooldown(RotationBase.HolyWordSerenity)));
                            break;
                        }

                        if (spell.Item1 == "Surging Mist")
                        {
                            // Make sure we Channeling Soothing Mist
                            prio.AddChild(CreateSurgingEnvelopBehaviour(unit, spell.Item1));
                            break;
                        }

                        if (spell.Item1 == "Healing Sphere")
                        {
                            // These things get passed out like $2 hookers...
                            prio.AddChild(CreateHealingSphereBehaviour(unit, spell.Item1));
                            break;
                        }

                        prio.AddChild(Spell.Cast(spell.Item1, on => unit, null, 0.5, true, false, false, false));
                        break;

                    case ChronicleSpellType.PeriodicHeal:

                        if (spell.Item1 == "Soothing Mist")
                        {
                            // Make sure we Channeling Soothing Mist
                            prio.AddChild(CreateSoothingMistBehaviour(unit, spell.Item1));
                            break;
                        }

                        if (spell.Item1 == "Enveloping Mist")
                        {
                            // Make sure we Channeling Soothing Mist
                            prio.AddChild(CreateSurgingEnvelopBehaviour(unit, spell.Item1));
                            break;
                        }

                        if (spell.Item1 == "Swiftmend")
                        {
                            // Swiftmend
                            prio.AddChild(CooldownTracker.Cast(spell.Item1, on => unit, null, 0.5, true));
                            break;
                        }

                        prio.AddChild(Spell.HoT(spell.Item1, on => unit));
                        break;

                    case ChronicleSpellType.HybridHeal:

                        if (spell.Item1 == "Regrowth")
                        {
                            // this is here because of sometimes HB misses the check for if its casted already
                            prio.AddChild(Spell.Cast(spell.Item1, on => unit, ret => OracleRoutine.IsViable(unit) && !unit.HasMyAura(RotationBase.Regrowth), 0.5, true));
                            break;
                        }

                        if (spell.Item1 == "Eternal Flame")
                        {
                            // This is here so we dont put a weak EF on the tank.
                            prio.AddChild(Spell.HoT(spell.Item1, on => unit, 100, ret => OracleRoutine.IsViable(unit) && (OracleRoutine.IsViable(OracleTanks.PrimaryTank) ? OracleTanks.PrimaryTank.HealthPercent(HPCheck.Tank) > 35 && (unit.Guid != OracleTanks.PrimaryTank.Guid) : true)));
                            break;
                        }

                        prio.AddChild(Spell.HoT(spell.Item1, on => unit));
                        break;
                }
            }

            TreeHooks.Instance.AddHook(hookName, prio);
        }

        #endregion Single Target hooks

        #region Cluster Hooks

        public static void PopulateClusterHook(Dictionary<int, ClusterSpell> spells, string hookName)
        {
            // If we have no clusters..gtfo.
            if (!ClusterManager.Clusters.Any())
            {
                Logger.Output("CreateClusteredHealBehavior: No Clusters.");
                return;
            }

            // Order clusters. 2. Order Clusters Desc Executed in 0.0018 ms
            var clusterResults = ClusterManager.Clusters.OrderByDescending(u => u != null ? u.Size : 0).ThenBy(u => u != null ? u.AvgHealthPct : 0).ToArray();

            //ClusterManager.Output();

            // Build Hooks 3. Populate Hooks Executed in 0.0099 ms
            BuildClusterHook(clusterResults, spells, hookName);
        }

        public static WoWUnit GetNpc
        {
            get
            {
                var result = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(p => p.Entry == 47649);
                return result ?? null;
            }
        }

        private static void BuildClusterHook(Points[] clusterResults, Dictionary<int, ClusterSpell> spells, string hookName)
        {
            // keep track of used spells
            var usedSpells = new List<int>();
            var prio = new PrioritySelector();

            // Order our composites dependant on the ordering of the clusters returned...
            for (int i = 0; i < clusterResults.Length; i++)
            {
                int x = i;

                // Logger.Output("BuildClusterHook ############## {0}", clusterResults[i].Player.Name);

                switch (clusterResults[i].PointClusterType)
                {
                    case ClusterType.GroundEffect:
                        foreach (var spellsResult in spells.Where(spell => spell.Value.SpellType == SpellType.GroundEffect && !usedSpells.Contains(spell.Value.SpellId)))
                        {
                            //Logger.Output(string.Format("[nextGroundSpell: {7}] [Location: {0}] [AvgHealthPct: {1} : {4}] [Size: {2} : {5}] [GroupSize: {3} : {6}]", clusterResults[i].Player.Location, clusterResults[x].AverageHealth(spellsResult.Value.ClusterSize, spellsResult.Value.AvgHealthPct), clusterResults[i].Size, clusterResults[i].GroupSize, spellsResult.Value.AvgHealthPct, spellsResult.Value.ClusterSize, spellsResult.Value.GrpSize, spellsResult.Value.SpellName));
                            var result = spellsResult.Value;

                            if (clusterResults[x].AverageHealth(result.ClusterSize, result.AvgHealthPct) > 0 &&
                                 clusterResults[x].Size >= result.ClusterSize &&
                                 clusterResults[x].GroupSize >= result.GrpSize)
                            {
                                // Druids..so so special.
                                if (result.SpellId == RotationBase.Swiftmend)
                                {
                                    prio.AddChild(CreateSwiftmendBehaviour(clusterResults[x].Player, result.SpellId));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Druids..so so special.
                                if (result.SpellId == RotationBase.WildMushroom)
                                {
                                    // do not move the shroom around if we want to specificly place it somewher.

                                    if (DruidCommon.UnitBuffSelection != UnitBuffSelection.None) continue;

                                    var shroom = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(p => p.CreatedByUnitGuid == StyxWoW.Me.Guid && p.Entry == 47649);

                                    //if (shroom != null) Logger.Output(" --> Shroom's location from {0}'s position is {1} yrds away ", clusterResults[x].Player.Name, clusterResults[x].Player.Location.Distance(shroom.Location));

                                    if (shroom != null && clusterResults[x].Player.Location.Distance(shroom.Location) < 25f) continue;

                                    //Logger.Output(" --> About to move Shroom to {0}'s position {1} yrds away ",clusterResults[x].Player.Name,  shroom != null ? clusterResults[x].Player.Location.Distance(shroom.Location) : 99999);
                                    var tpos = clusterResults[x].Player.Location;
                                    var trot = clusterResults[x].Player.Rotation;
                                    var shroomPos = WoWMathHelper.CalculatePointInFront(tpos, trot, 3.2f);

                                    prio.AddChild(TalentManager.HasGlyph("the Sprouting Mushroom")
                                                                   ? Spell.CastOnGround(result.SpellId, on => shroomPos)
                                                                   : Spell.Cast(result.SpellId, on => clusterResults[x].Player));

                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Druids..so so special.
                                if (result.SpellId == RotationBase.WildMushroomBloom)
                                {
                                    prio.AddChild(Spell.Cast(result.SpellId));

                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Holy Priest..so so special.
                                if (result.SpellId == RotationBase.HolyWordSanctuary)
                                {
                                    prio.AddChild(Spell.CastOnGround(result.SpellId, on => clusterResults[x].Player.Location, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where))), 0, false, true, true));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Priest..also special.
                                if (result.SpellId == RotationBase.DivineStar)
                                {
                                    prio.AddChild(Spell.Cast(result.SpellId, on => clusterResults[x].Player, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where))) && Unit.FaceTarget(clusterResults[x].Player)));

                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Monk..also special.
                                if (result.SpellId == RotationBase.ChiBurst)
                                {
                                    prio.AddChild(Spell.Cast(result.SpellId, on => clusterResults[x].Player, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where))) && Unit.FaceTarget(clusterResults[x].Player)));

                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Shamans..also special.
                                if (result.SpellId == RotationBase.HealingRain)
                                {
                                    prio.AddChild(CreateHealingRainBehaviour(clusterResults[x].Player, result.SpellId));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                prio.AddChild(Spell.CastOnGround(result.SpellId, on => clusterResults[x].Player.Location, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where)))));
                            }

                            usedSpells.Add(result.SpellId);
                        }
                        break;

                    case ClusterType.NearbyLowestHealth:
                        foreach (var spellsResult in spells.Where(spell => spell.Value.SpellType == SpellType.NearbyLowestHealth && !usedSpells.Contains(spell.Value.SpellId)))
                        {
                            //Logger.Output(string.Format("[nextNearbySpell: {7}] [Location: {0}] [AvgHealthPct: {1} : {4}] [Size: {2} : {5}] [GroupSize: {3} : {6}]", clusterResults[i].Player.Location, clusterResults[x].AverageHealth(spellsResult.Value.ClusterSize, spellsResult.Value.AvgHealthPct), clusterResults[i].Size, clusterResults[i].GroupSize, spellsResult.Value.AvgHealthPct, spellsResult.Value.ClusterSize, spellsResult.Value.GrpSize, spellsResult.Value.SpellName));
                            var result = spellsResult.Value;

                            if (clusterResults[x].AverageHealth(result.ClusterSize, result.AvgHealthPct) > 0 &&
                                 clusterResults[x].Size >= result.ClusterSize &&
                                 clusterResults[x].GroupSize >= result.GrpSize)
                            {
                                // Druids..so so special.
                                if (result.SpellId == RotationBase.Incarnation)
                                {
                                    prio.AddChild(CreateCooldowns(result.SpellId));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                if (result.SpellId == RotationBase.Ascendance)
                                {
                                    // Stop it spamming ascendance Lol
                                    prio.AddChild(Spell.Cast(result.SpellId, on => clusterResults[x].Player, where => !StyxWoW.Me.HasAura("Ascendance")));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Druids..so so special.
                                if (result.SpellId == RotationBase.Genesis)
                                {
                                    prio.AddChild(CreateGenesisBehaviour());
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Cast on Enemy.
                                if (result.SpellId == RotationBase.InvokeXuentheWhiteTiger)
                                {
                                    prio.AddChild(Spell.Cast(result.SpellId, on => Unit.GetEnemy, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where)))));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                prio.AddChild(Spell.Cast(result.SpellId, on => clusterResults[x].Player, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where)))));
                            }

                            usedSpells.Add(result.SpellId);
                        }
                        break;

                    case ClusterType.Party:
                        foreach (var spellsResult in spells.Where(spell => spell.Value.SpellType == SpellType.Party && !usedSpells.Contains(spell.Value.SpellId)))
                        {
                            // Logger.Output(string.Format("[nextPartySpell: {7}] [Location: {0}] [AvgHealthPct: {1} : {4}] [Size: {2} : {5}] [GroupSize: {3} : {6}]", clusterResults[i].Player.Location, clusterResults[x].AverageHealth(spellsResult.Value.ClusterSize, spellsResult.Value.AvgHealthPct), clusterResults[i].Size, clusterResults[i].GroupSize, spellsResult.Value.AvgHealthPct, spellsResult.Value.ClusterSize, spellsResult.Value.GrpSize, spellsResult.Value.SpellName));
                            var result = spellsResult.Value;

                            if (clusterResults[x].AverageHealth(result.ClusterSize, result.AvgHealthPct) > 0 &&
                                clusterResults[x].Size >= result.ClusterSize &&
                                clusterResults[x].GroupSize >= result.GrpSize)
                            {
                                prio.AddChild(Spell.Cast(result.SpellId, on => clusterResults[x].Player, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where)))));
                            }

                            usedSpells.Add(result.SpellId);
                        }
                        break;

                    case ClusterType.Proximity:
                        foreach (var spellsResult in spells.Where(spell => spell.Value.SpellType == SpellType.Proximity && !usedSpells.Contains(spell.Value.SpellId)))
                        {
                            // Logger.Output(string.Format("[nextProximitySpell: {7}] [Location: {0}] [AvgHealthPct: {1} : {4}] [Size: {2} : {5}] [GroupSize: {3} : {6}]", clusterResults[i].Player.Location, clusterResults[x].AverageHealth(spellsResult.Value.ClusterSize, spellsResult.Value.AvgHealthPct), clusterResults[i].Size, clusterResults[i].GroupSize, spellsResult.Value.AvgHealthPct, spellsResult.Value.ClusterSize, spellsResult.Value.GrpSize, spellsResult.Value.SpellName));
                            var result = spellsResult.Value;

                            if (clusterResults[x].AverageHealth(result.ClusterSize, result.AvgHealthPct) > 0 &&
                                clusterResults[x].Size >= result.ClusterSize &&
                                clusterResults[x].GroupSize >= result.GrpSize)
                            {
                                if (result.SpellId == RotationBase.ForceOfNature)
                                {
                                    var Treant = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(p => p.CreatedByUnitGuid == StyxWoW.Me.Guid && p.Entry == 54983);

                                    // I think Efflorescence lasts around 6 seconds so wait for this time before we shoot out another.
                                    if (Treant != null && Spell.ForceofNatureWaitTimer.IsFinished)
                                    {
                                        // this is here because of HB's retarded Overloads. Why can't it check the cooldown of the parent Overload!!
                                        prio.AddChild(CooldownTracker.Cast(result.SpellId, on => clusterResults[x].Player,
                                            where => !CooldownTracker.SpellOnCooldown(RotationBase.ForceOfNature) && (result.Requirements == null || (result.Requirements != null && result.Requirements(where))), 2, true, true, false));
                                        continue;
                                    }
                                }

                                // Shamans..also special.
                                if (result.SpellId == RotationBase.ChainHeal)
                                {
                                    // Assign the centre of the cluster as the chaintarget...
                                    WoWUnit chainTarget = clusterResults[x].Player;

                                    if (UseExperimentalChainHeal)
                                    {
                                        // retrieve the proximity points...
                                        List<Points> points = ClusterManager.Clusters.Where(cluster => cluster.PointClusterType == ClusterType.Proximity).ToList();

                                        if (points.Count != 0)
                                        {
                                            // get the cluster members from the centroids player list...
                                            List<WoWUnit> clusterMembers = points.SelectMany(r => r.ClusterMembers).ToList();

                                            // Looking for the player in the cluster that has our riptide...
                                            var chainunit = clusterMembers.FirstOrDefault(unit => unit.HasMyAura(RotationBase.Riptide));

                                            // if no Riptide try for a Earthshield.
                                            if (!OracleRoutine.IsViable(chainunit)) chainunit = clusterMembers.FirstOrDefault(unit => unit.HasMyAura(RotationBase.EarthShield));

                                            if (OracleRoutine.IsViable(chainunit))
                                            {
                                                chainTarget = chainunit;

                                                //if (chainTarget != null) Logger.Output(String.Format("Chain Heal: Found {0} [{2:F1}%] as having Riptide in {1}'s [{3:F1}%] Cluster", OracleRoutine.IsViable(chainunit) ? chainunit.Name : "Unknown", OracleRoutine.IsViable(clusterResults[x].Player) ? clusterResults[x].Player.Name : "Unknown", OracleRoutine.IsViable(chainunit) ? chainunit.HealthPercent : 0, OracleRoutine.IsViable(clusterResults[x].Player) ? clusterResults[x].Player.HealthPercent : 0));
                                            }
                                        }

                                        //Logger.Output("Chain Heal: {0} will be our target", OracleRoutine.IsViable(chainTarget) ? chainTarget.Name : "Unknown");
                                    }

                                    prio.AddChild(CreateChainHealBehaviour(chainTarget, result.SpellId));
                                    usedSpells.Add(result.SpellId);
                                    continue;
                                }

                                // Paladin..also special.
                                if (result.SpellId == RotationBase.HolyPrism)
                                {
                                    var enemy = Unit.EnemyFromPointLocation(clusterResults[x].Player.Location, 15);
                                    if (enemy != null)
                                    {
                                        prio.AddChild(Spell.Cast(result.SpellId, on => enemy, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where)))));

                                        usedSpells.Add(result.SpellId);
                                        continue;
                                    }
                                }

                                prio.AddChild(Spell.Cast(result.SpellId, on => clusterResults[x].Player, where => (result.Requirements == null || (result.Requirements != null && result.Requirements(where)))));
                            }

                            usedSpells.Add(result.SpellId);
                        }
                        break;
                }
            }

            TreeHooks.Instance.AddHook(hookName, prio);
        }

        #endregion Cluster Hooks

        #region helper Composites

        #region Druid Specific

        // Bug change these too UnitDelegates.
        private static Composite CreateSwiftmendBehaviour(WoWUnit unit, int spell)
        {
            const int regrowth = RotationBase.Regrowth;
            const int rejuvenation = RotationBase.Rejuvenation;

            return new Decorator(ret => !TalentManager.HasGlyph("Efflorescence"),
                   new Sequence(
                        new PrioritySelector(
                            new Decorator(ret => unit.HasAura(rejuvenation) || unit.HasAura(regrowth),
                                new ActionAlwaysSucceed()), //gtfo
                            Spell.Cast(regrowth, on => unit, req => !TalentManager.HasGlyph("Regrowth") && (unit.HealthPercent(unit.GetHPCheckType()) < 50) && !unit.HasAura(regrowth), 0.5, true),
                            Spell.HoT(rejuvenation, on => unit)),
                   new Action(delegate
                    {
                        return !CooldownTracker.SpellOnCooldown(spell) ? RunStatus.Failure : RunStatus.Success;
                    }),
                    new Action(ret => Logger.Output(String.Format(" [DIAG] Casting {0} AOE on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, unit.SafeName, unit.Distance, unit.HealthPercent(unit.GetHPCheckType())))),
                    CooldownTracker.Cast(spell, on => unit)
                ));
        }

        private static Composite CreateGenesisBehaviour()
        {
            return new Action(delegate
                {
                    // we can't cast so meh..
                    if (Spell.GlobalCooldown()) return RunStatus.Failure;

                    const int genesis = RotationBase.Genesis;
                    const int rejuvenation = RotationBase.Rejuvenation;

                    WoWSpell castingSpell = StyxWoW.Me.CastingSpell;
                    // If we're casting something other than what we should be, stop casting it. (Channeling /stopcast stuff)
                    if (castingSpell != null && castingSpell.Id != genesis)
                        Lua.DoString("SpellStopCasting()");

                    var rejuvenationBuffCount = Unit.GetbuffCount(rejuvenation);

                    if (rejuvenationBuffCount < OracleSettings.Instance.Druid.GenesisbuffLimit)
                        return RunStatus.Failure;

                    SpellManager.Cast(genesis);

                    Logger.Output(String.Format(" [DIAG] Casting Genesis for {0} players", rejuvenationBuffCount));

                    return RunStatus.Success;
                });
        }

        #endregion Druid Specific

        private static Composite CreateHealingRainBehaviour(WoWUnit unit, int spell)
        {
            const int UnleashElements = RotationBase.UnleashElements;
            const int AncestralSwiftness = RotationBase.AncestralSwiftness;

            return new Decorator(ret =>
                {
                    if (!SpellManager.CanCast(spell))
                        return false;

                    return true;
                },
                new Sequence(
                    new PrioritySelector(new Decorator(ret => StyxWoW.Me.HasAura("Unleash Life") || CooldownTracker.SpellOnCooldown(UnleashElements), new ActionAlwaysSucceed()), CooldownTracker.Cast(UnleashElements, on => null)),
                    new PrioritySelector(new Decorator(ret => !ShamanCommon.HasTalent(ShamanTalents.AncestralSwiftness) || (ShamanCommon.HasTalent(ShamanTalents.AncestralSwiftness) && CooldownTracker.SpellOnCooldown(AncestralSwiftness)), new ActionAlwaysSucceed()), CooldownTracker.Cast(AncestralSwiftness, on => null)),
                    Spell.CreateWaitForLagDuration(),
                    CooldownTracker.CastOnGround(spell, on => unit.Location)));
        }

        private static Composite CreateChainHealBehaviour(WoWUnit unit, int spell)
        {
            const int Riptide = RotationBase.Riptide;

            return new Decorator(ret =>
                 {
                     if (!SpellManager.CanCast(spell))
                         return false;

                     return true;
                 },
            new Sequence(
                 new PrioritySelector(new Decorator(ret => unit.HasAura(Riptide) || (!TalentManager.HasGlyph("Riptide") && CooldownTracker.SpellOnCooldown(Riptide)), new ActionAlwaysSucceed()), CooldownTracker.Cast(Riptide, on => unit)),
                 new Action(ret => Logger.Output(String.Format(" [DIAG] Casting {0} AOE on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, unit.SafeName, unit.Distance, unit.HealthPercent(unit.GetHPCheckType())))),
                 new Action(ret => SpellManager.Cast(spell, unit))));
        }

        private static Composite CreateCooldowns(int spell)
        {
            return new Action(delegate
                {
                    WoWSpell castingSpell = StyxWoW.Me.CastingSpell;
                    // If we're casting something other than what we should be, stop casting it. (Channeling /stopcast stuff)
                    if (castingSpell != null && castingSpell.Id != spell)
                        Lua.DoString("SpellStopCasting()");

                    // Incarnation is a bitch...
                    if (HashSets.IgnoreCanCast.Contains(spell) && !CooldownTracker.SpellOnCooldown(spell) &&
                        !StyxWoW.Me.HasAnyAura("Incarnation: Tree of Life"))
                    {
                        Logger.Output(String.Format(" [DIAG] Casting {0} on Me at {1:F1}%", WoWSpell.FromId(spell).Name, StyxWoW.Me.HealthPercent));
                        SpellManager.Cast(spell);
                    }

                    if (!HashSets.IgnoreCanCast.Contains(spell))
                        CooldownTracker.Cast(spell, on => null);

                    return RunStatus.Failure;
                });
        }

        #region Mistweaver specific

        private static MonkSettings Setting { get { return OracleSettings.Instance.Monk; } }

        private static int SwitchMistPercent { get { return Setting.SwitchMistPercent; } }

        private static bool IgnoreSoothingMist { get { return Setting.IgnoreSoothingMist; } }

        public static readonly WaitTimer SoothingMistTimer = new WaitTimer(TimeSpan.FromSeconds(1.5));
        public static readonly WaitTimer HealingSphereTimer = new WaitTimer(TimeSpan.FromSeconds(2));

        private static WoWUnit CurrentSoothingMistTarget { get; set; }

        private static bool ChannellingSoothingMist { get { return (StyxWoW.Me.IsChanneling && StyxWoW.Me.CastingSpellId == RotationBase.SoothingMist); } }

        private static uint SoothingMistDifference
        {
            get
            {
                uint i = 0;
                if (OracleRoutine.IsViable(CurrentSoothingMistTarget))
                    i += (uint)CurrentSoothingMistTarget.HealthPercent(CurrentSoothingMistTarget.GetHPCheckType());

                if (OracleRoutine.IsViable(OracleHealTargeting.HealableUnit))
                    if (OracleHealTargeting.HealableUnit != null)
                        i -= (uint)OracleHealTargeting.HealableUnit.HealthPercent(OracleHealTargeting.HealableUnit.GetHPCheckType());

                return i;
            }
        }

        private static Composite CreateHealingSphereBehaviour(WoWUnit unit, string spell)
        {
            return new Decorator(ret =>
                {
                    // no unit? gtfo
                    if (!OracleRoutine.IsViable(unit))
                        return false;

                    if (!IgnoreSoothingMist && ChannellingSoothingMist)
                        return false;

                    if (!HealingSphereTimer.IsFinished)
                        return false;

                    return true;
                },
                new Sequence(
                Spell.CastOnGround(spell, on => unit.Location, reqs => true, 0.5, true, true, true),
                new Action(delegate
                    {
                        HealingSphereTimer.Reset();

                        return RunStatus.Success;
                    }),
                new WaitContinue(TimeSpan.FromMilliseconds(500), ret => StyxWoW.Me.CurrentPendingCursorSpell != null, new ActionAlwaysSucceed()),
                new Action(ret => Lua.DoString("SpellStopTargeting()")),
                new WaitContinue(TimeSpan.FromMilliseconds(750), ret => StyxWoW.Me.Combat || (Spell.GetSpellCooldown("Healing Sphere") == TimeSpan.Zero), new ActionAlwaysSucceed())
                ));
        }

        private static Composite CreateSoothingMistBehaviour(WoWUnit unit, string spell)
        {
            return new Action(delegate
                {
                    // no unit? gtfo
                    if (!OracleRoutine.IsViable(unit)) return RunStatus.Failure;

                    // we can't cast so meh..
                    if (Spell.GlobalCooldown()) return RunStatus.Success;

                    // Mana Check
                    if (StyxWoW.Me.CurrentMana < (StyxWoW.Me.BaseMana * 0.088))
                        return RunStatus.Failure;

                    WoWSpell castingSpell = StyxWoW.Me.CastingSpell;
                    // If we're casting something other than what we should be, stop casting it. (Channeling /stopcast stuff)
                    if (castingSpell != null && castingSpell.Name != spell)
                        Lua.DoString("SpellStopCasting()");

                    // So if we havnt been channeling for at least 1 tick then gtfo.
                    if (!SoothingMistTimer.IsFinished)
                    {
                        return RunStatus.Success;
                    }

                    // If we are already channelling SM and the difference between the current SM target
                    // .. and the new heal target is < 15% (default) then continue on..
                    if (ChannellingSoothingMist && SoothingMistDifference < SwitchMistPercent)
                        return RunStatus.Failure;

                    // We must need to channell lets go!
                    SpellManager.Cast(spell, unit);
                    Logger.Output(String.Format(" [DIAG] Casting {0} on {1} at {2:F1} yds at {3:F1}%", spell, unit.SafeName, unit.Distance, unit.HealthPercent(unit.GetHPCheckType())));

                    // Update the Soothing Mist target..
                    CurrentSoothingMistTarget = unit;

                    SoothingMistTimer.Reset();

                    return RunStatus.Success;
                });
        }

        private static Composite CreateSurgingEnvelopBehaviour(WoWUnit unit, string spell)
        {
            return new Action(delegate
                {
                    if (!OracleRoutine.IsViable(unit)) return RunStatus.Failure;

                    var result = unit.HealthPercent(unit.GetHPCheckType());

                    if (result > 85)
                        return RunStatus.Failure;

                    // Not channeling SM gtfo
                    if (!ChannellingSoothingMist)
                        return RunStatus.Failure;

                    // enough chi ?
                    if (spell == "Enveloping Mist" && StyxWoW.Me.CurrentChi < 3)
                        return RunStatus.Failure;

                    // We go dat aura sun!..we out.
                    if (spell == "Enveloping Mist" && unit.HasAura(RotationBase.EnvelopingMist))
                        return RunStatus.Failure;

                    SpellManager.Cast(spell, unit);
                    Logger.Output(String.Format(" [DIAG] Casting {0} on {1} at {2:F1} yds at {3:F1}%", spell, unit.SafeName, unit.Distance, unit.HealthPercent(unit.GetHPCheckType())));

                    return RunStatus.Success;
                });
        }

        #endregion Mistweaver specific

        #endregion helper Composites
    }
}