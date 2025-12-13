#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/OracleHealTargeting.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core;
using Oracle.Core.DataStores;
using Oracle.Core.Groups;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.Shared.Logging;
using Oracle.UI;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Healing
{
    public enum BeaconUnitSelection { Automatic = 0, TankOnly, Disable }

    public class OracleHealTargeting
    {
        #region Settings

        private static OracleSettings Setting { get { return OracleSettings.Instance; } }

        private static bool EnableProvingGrounds { get { return Setting.EnableProvingGrounds; } }

        private static bool EnableSelectiveHealing { get { return Setting.EnableSelectiveHealing; } }

        private static PaladinSettings PaladinSetting { get { return OracleSettings.Instance.Paladin; } }

        private static BeaconUnitSelection BeaconUnitSelectionSetting { get { return PaladinSetting.BeaconUnitSelection; } }

        #endregion settings

        public static OracleHealTargeting Instance { get; private set; }

        static OracleHealTargeting()
        {
            // Make sure we have a singleton instance!
            Instance = new OracleHealTargeting();

            HealingPriorities = new List<WoWUnit>();
            HealingPrioritiesPG = new List<WoWUnit>(); // BUG: REMOVE ASAAP
        }

        # region ProvingGrounds
        // BUG: Remove this hackish shit

        public static List<WoWUnit> HealingPrioritiesPG { get; private set; }

        public static List<Tuple<double, WoWUnit>> weightsPG = new List<Tuple<double, WoWUnit>>(); // BUG: REMOVE ASAAP

        #endregion

        #region Variables

        public const bool OutputHealingPriorities = false;

        public static WoWUnit HealableUnit
        {
            get
            {
                if (EnableProvingGrounds)
                {
                    return HealingPrioritiesPG.FirstOrDefault();
                }

                return HealingPriorities.FirstOrDefault();
            }
        }

        public static WoWUnit BeaconUnit
        {
            get
            {
                if (EnableProvingGrounds)
                {
                    return BeaconUnitSelectionSetting == BeaconUnitSelection.TankOnly ? OracleTanks.PrimaryTank : HealingPrioritiesPG.Skip(1).FirstOrDefault();
                }

                return BeaconUnitSelectionSetting == BeaconUnitSelection.TankOnly ? OracleTanks.PrimaryTank : HealingPriorities.Skip(1).FirstOrDefault();
            }
        }

        public static List<WoWUnit> HealingPriorities { get; private set; }

        #endregion Variables

        public void Pulse()
        {
            if (OracleTanks.Tanks.Count == 0 && !EnableProvingGrounds)
            {
                OracleTanks.Tanks = OracleTanks.GetMainTanks();
                Logger.Output("======== {0} Tank(s) ========", OracleTanks.Tanks.Count);
                foreach (var tank in OracleTanks.Tanks.Values.ToList())
                {
                    if (tank.Tank.Name != null) Logger.Output("[*] {0} : ({1})", tank.Tank.SafeName, tank.IsMainTank ? "Main Tank" : "Assist Tank");
                }
                Logger.Output("======================");
            }

            var weights = new List<Tuple<float, WoWUnit>>();

            weightsPG.Clear();

            if (EnableProvingGrounds)
            {
                foreach (var pgNPC in Unit.ProvingGroundNPCs())
                {
                    if (!OracleRoutine.IsViable(pgNPC)) continue;

                    double weight = 0f;
                    double hp = pgNPC.HealthPercent(pgNPC.GetHPCheckType()); // StyxWoW.Me.ManaPercent < 50 ? (float)(pgNPC.HealthPercent + pgNPC.TotalAbsorbs) :
                    weight -= hp * 5;

                    // Target is gonna be full health. Why bother adding it to the weights?
                    if (hp > 95)
                        continue;

                    if (pgNPC.Entry == 72218) // Oto
                    {
                        weight += 150;
                    }

                    if (pgNPC.HasAura(145263)) //Chomp
                    {
                        weight += 175;
                    }

                    weightsPG.Add(new Tuple<double, WoWUnit>(weight, pgNPC));
                }

                // add us if we have no units..saves null ref when not partied.
                if (weightsPG.Count == 0) weightsPG.Add(new Tuple<double, WoWUnit>(0, StyxWoW.Me));

                // Order by descending weights, and take the players out of it.
                HealingPrioritiesPG = weightsPG.OrderByDescending(w => w.Item1).Select(w => w.Item2).ToList();
            }
            else
            {
                var results = Unit.FriendlyPriorities.Union(Unit.NPCPriorities);

                foreach (var p in results.ToList())
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    if (EnableSelectiveHealing &&
                        HealingSelector.SelectiveHealingPlayers.ContainsKey(p.Guid) &&
                        !HealingSelector.SelectiveHealingPlayers[p.Guid].IsChecked)
                    {
                        Logger.Output(" You have chosen not too heal {0}", p.Name);
                        continue;
                    }

                    float weight = 0f;
                    float hp = (float)p.HealthPercent(p.GetHPCheckType());
                    weight -= hp * 5;

                    // Target is gonna be full health. Why bother adding it to the weights?
                    if (hp > 95)
                        continue;

                    if (OracleTanks.Tanks.ContainsKey(p.Guid))
                    {
                        weight += 150;

                        // AssistTank is not as important as Main Tank.
                        if (OracleTanks.Tanks.Any(T => p.Guid == T.Key && T.Value.IsAssistTank))
                            weight -= 70;
                    }

                    if (HashSets.HealableNPCs.Contains((int)p.Entry))
                    {
                        weight += 85;
                    }

                    //122370 -> Reshaper Life
                    //if (p.HasAura(122370))
                      //  continue;

                    //weight += ClassSpecificWeighTarget(this, p);

                    weights.Add(new Tuple<float, WoWUnit>(weight, p));
                }

                // add us if we have no units..saves null ref when not partied.
                if (weights.Count == 0) weights.Add(new Tuple<float, WoWUnit>(0, StyxWoW.Me));

                // Order by descending weights, and take the players out of it.
                HealingPriorities = weights.OrderByDescending(w => w.Item1).Select(w => w.Item2).ToList();
            }

            OutputPlayers();
        }

        private static bool HasHealingModifier(WoWPlayer player)
        {
            if (!OracleRoutine.IsViable(player)) return false;
            // -- Checking if unit receives more healing
            return player.HasAuraWithEffect(WoWApplyAuraType.ModHealingReceived, -20, 10, 200);  // unit receives less healing , -20, -10, 200
        }

        private static void SetClassSpecificWeighTarget()
        {
        }
        
        private void OutputPlayers()
        {
            if (!OutputHealingPriorities)
                return;

            var lookup = HealingPriorities.ToLookup(p => Math.Round(p.Distance / 5, MidpointRounding.AwayFromZero) * 5);

            foreach (IGrouping<double, WoWPlayer> player in lookup)
            {
                // Print the key value of the IGrouping.
                Logger.Output("{0} yds", player.Key);
                // Iterate through each value in the
                // IGrouping and print its value.
                foreach (WoWPlayer p in player)
                    Logger.Output(string.Format(" [{2} : {0}] at {3:F1} yds at {1:F1}%", p.Name, p.HealthPercent(p.GetHPCheckType()), p.GetRole(), p.Distance));
            }
        }

        #region Bools

        internal static bool HealthCheck(WoWUnit unit, int hp)
        {
            return OracleRoutine.IsViable(unit) && unit.HealthPercent(unit.GetHPCheckType()) <= hp;
        }

        #endregion Bools

        
    }
}