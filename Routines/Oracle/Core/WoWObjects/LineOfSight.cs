using JetBrains.Annotations;
using Oracle.Core.CombatLog;
using Oracle.Core.DataStores;
using Oracle.Core.Spells;
using Oracle.Shared.Logging;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.WoWObjects
{
    [UsedImplicitly]
    internal static class OracleLineOfSight
    {
        static OracleLineOfSight()
        {
            // Cache dat shit..
            MeGuid = StyxWoW.Me.Guid;
            OracleBlackListEntries = new Dictionary<ulong, OracleBlackListedUnit>();
            PopulatelocalizedConstants();
        }

        #region WoW Constants

        private static readonly Dictionary<string, string> LocalizedFailTypeConstants = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> LocalizedInvalidTargetsConstants = new Dictionary<string, string>();

        private static void PopulatelocalizedConstants()
        {
            foreach (String ft in HashSets.FailType)
            {
                var result = GetSymbolicLocalizeValue(ft);

                //Logger.Output("{1} :: {0}", result, ft);

                if (!string.IsNullOrEmpty(result)) LocalizedFailTypeConstants.Add(result, ft);
            }

            foreach (String ft in HashSets.InvalidTargets)
            {
                var result = GetSymbolicLocalizeValue(ft);

                // Logger.Output("{1} :: {0}", result, ft);

                if (!string.IsNullOrEmpty(result)) LocalizedInvalidTargetsConstants.Add(result, ft);
            }
        }

        private static string GetSymbolicLocalizeValue(string symbolicName)
        {
            var localString = Lua.GetReturnVal<string>("return " + symbolicName, 0);
            return localString;
        }

        #endregion WoW Constants

        private static ulong MeGuid { get; set; }

        public static bool InLineOfSight(this WoWUnit unit)
        {
            return OracleRoutine.IsViable(unit) && !OracleBlackListEntries.ContainsKey(unit.Guid);
        }

        public static void Initialize()
        {
            CombatLogHandler.Register("SPELL_FAILED_LINE_OF_SIGHT", HandleCombatLogEvent);
            CombatLogHandler.Register("SPELL_CAST_FAILED", HandleCombatLogEvent);
        }

        public static void Shutdown()
        {
            CombatLogHandler.Remove("SPELL_FAILED_LINE_OF_SIGHT");
            CombatLogHandler.Remove("SPELL_CAST_FAILED");
        }

        public static void BlackListUnit(CombatLogEventArgs args)
        {
            if (Spell.LastTarget == null) return; // we cant do shit without the last known target :/

            var reason = args.FailedType;
            var guid = Spell.LastTarget.Guid;
            var name = Spell.LastTarget.Name;

            string result1;
            if (LocalizedInvalidTargetsConstants.TryGetValue(reason, out result1) && (args.SourceGuid == MeGuid) && guid != MeGuid)
            {
                Logger.Warning(" [LOS] : Detected LoS issue with [{0}:{2}] @ {1}", name, DateTime.Now, guid);
                UpdateOracleBlackListEntries(guid, name, 1500);
            }
        }

        #region Blacklist Entries

        private static readonly Dictionary<ulong, OracleBlackListedUnit> OracleBlackListEntries = new Dictionary<ulong, OracleBlackListedUnit>();

        public static void OutputOracleBlackListEntries()
        {
            if (OracleBlackListEntries.Values.Count < 1) return;
            Logger.Output(" --> We have {0} targets blacklisted for {1}", OracleBlackListEntries.Count, OracleBlackListEntries.Values.FirstOrDefault().OracleBlackListedUnitName);
        }

        internal static void PulseOracleBlackListEntries()
        {
            OracleBlackListEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.OracleBlackListedUnitCurrentTime).TotalMilliseconds >= t.OracleBlackListedUnitExpiryTime);
        }

        private static void UpdateOracleBlackListEntries(ulong key, string name, double expiryTime)
        {
            if (!OracleBlackListEntries.ContainsKey(key)) OracleBlackListEntries.Add(key, new OracleBlackListedUnit(key, name, expiryTime, DateTime.UtcNow));
        }

        private struct OracleBlackListedUnit
        {
            public OracleBlackListedUnit(ulong key, string name, double expiryTime, DateTime currentTime)
                : this()
            {
                OracleBlackListedUnitKey = key;
                OracleBlackListedUnitName = name;
                OracleBlackListedUnitExpiryTime = expiryTime;
                OracleBlackListedUnitCurrentTime = currentTime;
            }

            public DateTime OracleBlackListedUnitCurrentTime { get; private set; }

            public double OracleBlackListedUnitExpiryTime { get; set; }

            public ulong OracleBlackListedUnitKey { get; set; }

            public string OracleBlackListedUnitName { get; set; }
        }

        #endregion Blacklist Entries

        private static void HandleCombatLogEvent(CombatLogEventArgs args)
        {
            switch (args.Event)
            {
                case "SPELL_FAILED_LINE_OF_SIGHT":
                case "SPELL_CAST_FAILED":
                    if (args.SourceGuid == MeGuid)
                        BlackListUnit(args);
                    break;
            }
        }
    }
}