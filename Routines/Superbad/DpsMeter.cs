#region

using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    /// <summary>
    ///     Calculates DPS, and combat time left on nearby mobs in combat.
    ///     DPS is not calculated as YOUR DPS. But instead; global DPS on the mob. (You and your buddies combined DPS on a
    ///     single mob => RaidDPS)
    /// </summary>
    internal static class DpsMeter
    {
        private static readonly Dictionary<ulong, DpsInfo> DpsInfos = new Dictionary<ulong, DpsInfo>();
        public static bool DpsMeterInitialized;

        /// <summary>
        ///     Starts the DpsMeter.
        /// </summary>
        public static void Initialize()
        {
            if (DpsMeterInitialized) return;
            DpsInfos.Clear();
            DpsMeterInitialized = true;
        }

        public static void Shutdown()
        {
            if (!DpsMeterInitialized) return;
            DpsInfos.Clear();
            DpsMeterInitialized = false;
        }

        public static void Update()
        {
            List<WoWUnit> availableUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u != null && u.IsValid && u.DistanceSqr <= 400 && !u.IsDead)
                .ToList();
                if (StyxWoW.Me.GotTarget)
                    availableUnits.Add(StyxWoW.Me.CurrentTarget);

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

                    uint currentLife = unit.CurrentHealth;
                    int currentTime = ConvDate2Timestam(DateTime.Now);
                    int timeDiff = currentTime - di.CombatStart;
                    uint hpDiff = di.StartHealth - currentLife;

                    if (hpDiff > 0)
                    {
                        long fullTime = timeDiff*di.StartHealthMax/hpDiff;
                        long pastFirstTime = (di.StartHealthMax - di.StartHealth)*timeDiff/hpDiff;
                        long calcTime = di.CombatStart - pastFirstTime + fullTime - currentTime;
                        if (calcTime < 1) calcTime = 1;
                        //calc_time is a int value for time to die (seconds) so there's no need to do SecondsToTime(calc_time)
                        long timeToDie = calcTime;
                        di.CombatTimeLeft = new TimeSpan(0, 0, (int) timeToDie);
                    }

                    if (hpDiff <= 0)
                    {
                        //unit was healed,resetting the initial values
                        di.StartHealth = unit.CurrentHealth;
                        di.StartHealthMax = unit.MaxHealth;
                        di.CombatStart = ConvDate2Timestam(DateTime.Now);
                        //Lets do a little trick and calculate with seconds / u know Timestamp from unix? we'll do so too
                        if (unit.IsCritter || unit.MaxHealth <= StyxWoW.Me.MaxHealth/4)
                            di.CombatTimeLeft = new TimeSpan(0, 0, 5);
                        else
                            di.CombatTimeLeft = new TimeSpan(0, 0, 16);
                    }

                    if (currentLife == di.StartHealthMax)
                    {
                        di.StartHealth = unit.CurrentHealth;
                        di.StartHealthMax = unit.MaxHealth;
                        di.CombatStart = ConvDate2Timestam(DateTime.Now);
                        if (unit.IsCritter || unit.MaxHealth <= StyxWoW.Me.MaxHealth/4)
                            di.CombatTimeLeft = new TimeSpan(0, 0, 5);
                        else
                            di.CombatTimeLeft = new TimeSpan(0, 0, 16);
                    }


                    // .NET makes a copy of the struct when we grab it out of the collection.
                    // Make sure we put the updated version back in!
                    DpsInfos[unit.Guid] = di;
                    // Logger.InfoLog(string.Format("Dpsmeter ticking! {0} DPS/{1}s ({2})", di.CurrentDps, di.CombatTimeLeft, DateTime.Now)); // Just for observation purposes, can get removed when working!
                }
            }
            KeyValuePair<ulong, DpsInfo>[] removeUnits = DpsInfos.Where(kv => !kv.Value.Unit.IsValid).ToArray();
            foreach (var t in removeUnits)
            {
                DpsInfos.Remove(t.Key);
                // Logger.InfoLog(string.Format("Dpsmeter removed bad units")); // Just for observation purposes, can get removed when working!
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
        ///     Returns the estimated combat time left for this unit. (Time until death)
        ///     If the unit is invalid; TimeSpan.MinValue is returned.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static TimeSpan GetCombatTimeLeft(WoWUnit u)
        {
            if (u.Entry == 71865 || u.Entry == 71543)
                return new TimeSpan(0, 0, 35);
            //If our current target is a dummy, we set a fixed value for combat time left (55).
            if (Unit.IsDummy(StyxWoW.Me.CurrentTarget))
                return new TimeSpan(0, 0, 55);
            return DpsInfos.ContainsKey(u.Guid) ? DpsInfos[u.Guid].CombatTimeLeft : new TimeSpan(0, 0, 35);
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

        #endregion
    }
}