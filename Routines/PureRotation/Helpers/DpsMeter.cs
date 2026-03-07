#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-17 10:23:04 +0200 (Fr, 17 Mai 2013) $
 * $ID$
 * $Revision: 1424 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Helpers/DpsMeter.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Core;
using PureRotation.Managers;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Helpers
{
    /// <summary>
    /// Calculates DPS, and combat time left on nearby mobs in combat.
    /// DPS is not calculated as YOUR DPS. But instead; global DPS on the mob. (You and your buddies combined DPS on a single mob => RaidDPS)
    /// </summary>
    internal static class DpsMeter
    {
        private static readonly Dictionary<ulong, DpsInfo> DpsInfos = new Dictionary<ulong, DpsInfo>();
        public static bool DpsMeterInitialized;

        /// <summary>
        /// Starts the DpsMeter.
        /// </summary>
        /*
        public static void Initialize()
        {
            if (DpsMeterInitialized) return;
            DpsInfos.Clear();
            DpsMeterInitialized = true;
        }
        */

        public static void Shutdown()
        {
            if (!DpsMeterInitialized) return;
            DpsInfos.Clear();
            DpsMeterInitialized = false;
        }

        public static void Update()
        {
            if (!DpsMeterInitialized)
            {
                Logger.DebugLog("DpsMeter initialized!");
                DpsInfos.Clear();
                DpsMeterInitialized = true;
            }

            /*
            List<WoWUnit> availableUnits = (from u in ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                            where !u.IsDead && u.Attackable && !u.IsFriendly && u.Combat
                                            orderby u.DistanceSqr ascending
                                            select u).ToList();
            */

            // Poll for cached units. 
            CachedUnits.UpdateCache();

            // Use the cache. There's very very few scenarios where we'd need to check if a unit is going
            //   to die in the next second.
            List<WoWUnit> availableUnits = (from u in Unit.CachedNearbyAttackableUnits(Styx.StyxWoW.Me.Location, 60)
                                            where u.Combat
                                            select u).ToList();

            // Logger.InfoLog(string.Format("Dpsmeter availableUnits: {0}!", availableUnits.Count));

            foreach (WoWUnit unit in availableUnits)
            {
                if (!DpsInfos.ContainsKey(unit.Guid))
                {
                    var di = new DpsInfo
                        {
                            Unit = unit,
                            CombatStart = ConvDate2Timestam(DateTime.Now),
                            StartHealth = unit.CurrentHealth,
                            StartHealthMax = unit.MaxHealth
                        };

                    DpsInfos.Add(unit.Guid, di);
                }
                else
                {
                    DpsInfo di = DpsInfos[unit.Guid];

                    uint _currentLife = unit.CurrentHealth;
                    int _currentTime = ConvDate2Timestam(DateTime.Now);
                    int timeDiff = _currentTime - di.CombatStart;
                    uint hpDiff = di.StartHealth - _currentLife;

                    if (hpDiff > 0)
                    {
                        long fullTime = timeDiff*di.StartHealthMax/hpDiff;
                        long pastFirstTime = (di.StartHealthMax - di.StartHealth)*timeDiff/hpDiff;
                        long calcTime = di.CombatStart - pastFirstTime + fullTime - _currentTime;
                        if (calcTime < 1) calcTime = 1;
                        //calc_time is a int value for time to die (seconds) so there's no need to do SecondsToTime(calc_time)
                        long timeToDie = calcTime;
                        //Logging.Write("TimeToDeath: {0} (GUID: {1}, Entry: {2}) dies in {3}, you are dpsing with {4} dps", target.Name, target.Guid, target.Entry, timeToDie, dps);
                        di.CombatTimeLeft = new TimeSpan(0, 0, (int) timeToDie);
                    }

                    if (hpDiff <= 0)
                    {
                        //unit was healed,resetting the initial values
                        di.StartHealth = unit.CurrentHealth;
                        di.StartHealthMax = unit.MaxHealth;
                        di.CombatStart = ConvDate2Timestam(DateTime.Now);
                        //Lets do a little trick and calculate with seconds / u know Timestamp from unix? we'll do so too
                        //Logging.Write("TimeToDeath: {0} (GUID: {1}, Entry: {2}) was healed, resetting data.", target.Name, target.Guid, target.Entry);
                        di.CombatTimeLeft = new TimeSpan(0, 0, 10);
                    }

                    if (_currentLife == di.StartHealthMax)
                    {
                        di.CombatTimeLeft = new TimeSpan(0, 0, 10);
                    }

                    // .NET makes a copy of the struct when we grab it out of the collection.
                    // Make sure we put the updated version back in!
                    DpsInfos[unit.Guid] = di;
                    // Logger.InfoLog(string.Format("Dpsmeter ticking! {0} DPS/{1}s ({2})", di.CurrentDps, di.CombatTimeLeft, DateTime.Now)); // Just for observation purposes, can get removed when working!
                    KeyValuePair<ulong, DpsInfo>[] removeUnits = DpsInfos.Where(kv => !kv.Value.Unit.IsValid).ToArray();
                    foreach (var t in removeUnits)
                    {
                        DpsInfos.Remove(t.Key);
                        // Logger.InfoLog(string.Format("Dpsmeter removed bad units")); // Just for observation purposes, can get removed when working!
                    }
                }
            }
        }

        private static int ConvDate2Timestam(DateTime time)
        {
            {
                var date1 = new DateTime(1970, 1, 1);
                // Refernzdatum (festgelegt) 	1328 	var date1 = new DateTime(1970, 1, 1); // Refernzdatum (festgelegt)
                DateTime date2 = time;
                // jetztiges Datum / Uhrzeit 	1329 	DateTime date2 = time; // jetztiges Datum / Uhrzeit
                var ts = new TimeSpan(date2.Ticks - date1.Ticks);
                // das Delta ermitteln 	1330 	var ts = new TimeSpan(date2.Ticks - date1.Ticks); // das Delta ermitteln
                // Das Delta als gesammtzahl der sekunden ist der Timestamp 	1331 	// Das Delta als gesammtzahl der sekunden ist der Timestamp
                return (Convert.ToInt32(ts.TotalSeconds));
            }
        }

        /// <summary>
        /// Returns the estimated combat time left for this unit. (Time until death)
        /// If the unit is invalid; TimeSpan.MinValue is returned.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static TimeSpan GetCombatTimeLeft(WoWUnit u)
        {
            //If our current target is a dummy, we set a fixed value for combat time left (55).
            if (Styx.StyxWoW.Me.CurrentTarget.IsDummy())
                return new TimeSpan(0, 0, 55);
            return DpsInfos.ContainsKey(u.Guid) ? DpsInfos[u.Guid].CombatTimeLeft : TimeSpan.MinValue;
        }

        #region Nested type: DpsInfo

        private struct DpsInfo
        {
            public int CombatStart;
            public TimeSpan CombatTimeLeft;
            public uint StartHealth;
            public uint StartHealthMax;
            public WoWUnit Unit;
        }

        #endregion Nested type: DpsInfo
    }
}