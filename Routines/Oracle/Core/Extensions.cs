using JetBrains.Annotations;
using Oracle.Core.Encounters;
using Oracle.Core.Groups;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities;
using Oracle.UI.Settings;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;

namespace Oracle.Core
{
    public enum HPCheck
    {
        None = 0,
        Tank,
        AoE,
        Single
    }

    public static class OracleExtensions
    {
        #region settings

        private static OracleSettings Settings { get { return OracleSettings.Instance; } }

        private static bool MalkorokEncounter { get { return Settings.MalkorokEncounter; } }

        private static bool UsePredictedHealth { get { return Settings.UsePredictedHealth; } }

        #endregion settings

        private const WoWPartyMember.GroupRole TankLeader = WoWPartyMember.GroupRole.Tank | WoWPartyMember.GroupRole.Leader;
        private const WoWPartyMember.GroupRole HealerLeader = WoWPartyMember.GroupRole.Healer | WoWPartyMember.GroupRole.Leader;
        private const WoWPartyMember.GroupRole DamageLeader = WoWPartyMember.GroupRole.Damage | WoWPartyMember.GroupRole.Leader;
        private const WoWPartyMember.GroupRole Tank50 = (WoWPartyMember.GroupRole)50;

        public static double HealthPercent(this WoWUnit unit, HPCheck check)
        {
            using (new PerformanceLogger("HealthPercent"))
            {
                if (!OracleRoutine.IsViable(unit)) return 100;

                var result = 100.0;

                switch (check)
                {
                    case HPCheck.AoE:
                    case HPCheck.Tank:
                        result = MalkorokEncounter ? Malkorok.GetAncientBarrierHealth(unit) : unit.HealthPercent;
                        break;

                    case HPCheck.Single:
                    case HPCheck.None:
                        result = MalkorokEncounter
                            ? Malkorok.GetAncientBarrierHealth(unit)
                            : (UsePredictedHealth ? unit.GetPredictedHealthPercent() : unit.HealthPercent);
                        break;
                }

                return result;
            }
        }

        public static double CurrentHealth(this WoWUnit unit, HPCheck check)
        {
            using (new PerformanceLogger("CurrentHealth"))
            {
                if (!OracleRoutine.IsViable(unit)) return 400000;

                double result = unit.MaxHealth;

                switch (check)
                {
                    case HPCheck.AoE:
                    case HPCheck.Tank:
                        result = MalkorokEncounter ? (result * (Malkorok.GetAncientBarrierHealth(unit) / 100)) : unit.CurrentHealth;
                        break;

                    case HPCheck.Single:
                    case HPCheck.None:
                        result = MalkorokEncounter ? (result * (Malkorok.GetAncientBarrierHealth(unit) / 100)) 
                            : (UsePredictedHealth ? unit.GetPredictedHealth() : unit.CurrentHealth);
                        break;
                }

                return result;
            }
        }

        // So getting the role of every person is expensive in 25 man so lets just cache their role for 2minutes.
        public static HPCheck GetHPCheckType(this WoWUnit unit, HPCheck check = HPCheck.None)
        {
            using (new PerformanceLogger("GetHPCheckType"))
            {
                if (!OracleRoutine.IsViable(unit)) return HPCheck.None;

                var role = WoWPartyMember.GroupRole.None;

                RoleCache results;
                if (RoleCacheEntries.TryGetValue(unit.Guid, out results))
                {
                    role = results.RoleCachePlayerRole;
                    //Logger.Output("-> {1} Role Updated cached [{0}]", role, results.RoleCachePlayerName);
                }
                else
                {
                    role = unit.ToPlayer().GetRole();
                    //Logger.Output(" -> {1} Role Updated call [{0}] ", role, unit.Name);
                }

                switch (role)
                {
                    case Tank50:
                    case TankLeader:
                    case WoWPartyMember.GroupRole.Tank:
                        UpdateRoleCacheEntries(unit.Guid, unit.Name, role, 120);
                        return HPCheck.Tank;

                    case HealerLeader:
                    case DamageLeader:
                    case WoWPartyMember.GroupRole.Healer:
                    case WoWPartyMember.GroupRole.Damage:
                    case WoWPartyMember.GroupRole.Leader:
                    case WoWPartyMember.GroupRole.None:
                        UpdateRoleCacheEntries(unit.Guid, unit.Name, role, 120);
                        return check != HPCheck.None ? check : HPCheck.None;
                }

                return check;
            }
        }

        #region RoleCache

        private static readonly Dictionary<ulong, RoleCache> RoleCacheEntries = new Dictionary<ulong, RoleCache>();

        public static void OutputRoleCacheEntries()
        {
            foreach (var spell in RoleCacheEntries)
            {
                Logger.Output(spell.Value.RoleCachePlayerName + " Role: " + spell.Value.RoleCachePlayerRole + " time: " + spell.Value.RoleCacheCurrentTime);
            }
        }

        public static void PulseRoleCacheEntries()
        {
            RoleCacheEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.RoleCacheCurrentTime).TotalSeconds >= t.RoleCacheExpiryTime);
        }

        private static void UpdateRoleCacheEntries(ulong guid, string playerName, WoWPartyMember.GroupRole role, double expiryTime)
        {
            //Logger.Output("Updated Rolecheck: {0}", DateTime.Now);
            if (RoleCacheEntries.ContainsKey(guid)) RoleCacheEntries[guid] = new RoleCache(guid, playerName, role, expiryTime, DateTime.UtcNow);
            if (!RoleCacheEntries.ContainsKey(guid)) RoleCacheEntries.Add(guid, new RoleCache(guid, playerName, role, expiryTime, DateTime.UtcNow));
        }

        private struct RoleCache
        {
            public RoleCache(ulong guid, string playerName, WoWPartyMember.GroupRole role, double expiryTime, DateTime currentTime)
                : this()
            {
                RoleCachePlayerName = playerName;
                RoleCacheExpiryTime = expiryTime;
                RoleCacheCurrentTime = currentTime;
                RoleCachePlayerGuid = guid;
                RoleCachePlayerRole = role;
            }

            public DateTime RoleCacheCurrentTime { get; private set; }

            public double RoleCacheExpiryTime { get; private set; }

            public string RoleCachePlayerName { get; private set; }

            private ulong RoleCachePlayerGuid { [UsedImplicitly] get; set; }

            public WoWPartyMember.GroupRole RoleCachePlayerRole { get; private set; }
        }

        #endregion RoleCache
    }
}