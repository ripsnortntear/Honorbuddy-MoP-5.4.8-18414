#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/DataStores/BossList.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.WoWObjects;
using Oracle.Shared.Logging;
using Styx;
using Styx.CommonBot.Database;
using Styx.Patchables;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.DataStores
{
    public static class BossList
    {
        static BossList()
        {
            // contains the list of all the 5 man and raid bosses.
            CurrentMapBosses = new HashSet<string>();

            CurrentMapBossIDs = new HashSet<uint>() {
                71543 /*Immerseus*/,
                71475 /*Rook Stonetoe*/,
                71479 /*He Softfoot*/,
                71480 /*Sun Tenderheart*/,
                72276 /*Norushen*/,
                71734 /*Sha of Pride*/,
                71466 /*Galakras*/,
                72276 /*Iron Juggernaut*/,
                71859 /*Earthbreaker Haromm*/,
                71858 /*Wavebinder Kardris*/,
                71515 /*General Nazgrim*/,
                71454 /*Malkorok*/,
                71889 /*Spoils of Pandaria*/,
                71529 /*Thok the Bloodthirsty*/,
                71504 /*Siegecrafter Blackfuse*/,
                71161 /*Kil'ruk the Wind-Reaver*/,
                71157 /*Xaril the Poisoned Mind*/,
                71156 /*Kaz'tik the Manipulator*/,
                71155 /*Korven the Prime*/,
                71160 /*Iyyokuk the Lucid*/,
                71154 /*Ka'roz the Locust*/,
                71152 /*Skeer the Bloodseeker*/,
                71158 /*Rik'kal the Dissector*/,
                71153 /*Hisek the Swarmkeeper*/,
                71865 /*Garrosh Hellscream*/,
                // older bosses
                68476 /*Horridon*/,
                69465 /*Jin'rokh the Breaker*/,
                69134, /*Kazra'jin*/
                69017, // promord
            };
        }

        public static bool IsBossNearby { get; set; }

        public static string NearbyBossName { get; set; }

        public static HashSet<string> CurrentMapBosses { get; private set; }

        public static HashSet<uint> CurrentMapBossIDs { get; private set; }

        public static void Update()
        {
            IsBossNearby = false;

            CurrentMapBosses = new HashSet<string>(StyxWoW.Db[ClientDb.DungeonEncounter].Where(r => r.GetField<int>(1) == StyxWoW.Me.MapId).Select(r => r.GetStringField(5)));

            NearbyBossCheck();

            //foreach (var boss in CurrentMapBosses)
            //{
            //    Logger.Output("- boss {0} {1}", boss, boss == NearbyBossName ? "is Nearby" : "");
            // }
            Logger.Output("BossList: contains {0} entries", CurrentMapBosses.Count());

            foreach (var boss in CurrentMapBossIDs)
            {
                if (NpcNearby(boss, true)) Logger.Output("You got a boss bro!");
            }
        }

        private static void NearbyBossCheck()
        {
            var boss = Unit.SearchAreaUnits().FirstOrDefault(unit => CurrentMapBosses.Contains(unit.Name) && unit.Distance < 60);
            IsBossNearby = (OracleRoutine.IsViable(boss));
            if (OracleRoutine.IsViable(boss)) NearbyBossName = boss.Name;
        }

        #region NPCQueries

        public static NpcResult GetNpcResult(uint entry)
        {
            return NpcQueries.GetNpcById(entry);
        }

        public static bool NpcNearby(uint entry, bool output = false)
        {
            var result = GetNpcResult(entry);

            if (result == null) return false;

            if (output) OutputNpcResult(result);

            return result.Location.Distance(StyxWoW.Me.Location) < 60;
        }

        public static void OutputNpcResult(NpcResult npcResult)
        {
            if (npcResult == null) return;

            Logger.Output(" Faction: {0}", npcResult.Faction);
            Logger.Output(" Location: {0}", npcResult.Location);
            Logger.Output(" IsNearby: {0}", (npcResult.Location.Distance(StyxWoW.Me.Location) < 60));
            Logger.Output(" MapId: {0}", npcResult.MapId);
            Logger.Output(" Name: {0}", npcResult.Name);
            Logger.Output(" NpcFlags: {0}", npcResult.NpcFlags);
            Logger.Output(" Title: {0}", npcResult.Title);
            Logger.Output(" Faction: {0}", npcResult.Faction);
        }

        #endregion NPCQueries
    }
}