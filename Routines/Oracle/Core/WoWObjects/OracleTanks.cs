using Oracle.Core.DataStores;
using Oracle.Core.Groups;
using Oracle.Core.Spells.Auras;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.WoWObjects
{
    public enum TankMode { Automatic = 0, LowestHealthTank, MainTank, OffTank, TankWithDebuff, Focus, TankSwap }

    internal static class OracleTanks
    {
        public static Dictionary<ulong, TankCache> Tanks = new Dictionary<ulong, TankCache>();

        #region settings

        private static OracleSettings Settings { get { return OracleSettings.Instance; } }

        private static TankMode TankMode { get { return Settings.TankMode; } }

        private static bool EnableProvingGrounds { get { return Settings.EnableProvingGrounds; } }

        #endregion settings

        private static bool IsActiveTank(this WoWUnit tank)
        {
            return (OracleRoutine.IsViable(tank) && (OracleRoutine.IsViable(tank.CurrentTarget) && tank.CurrentTarget.IsBoss && tank.CurrentTarget.CurrentTargetGuid == tank.Guid));
        }

        public static WoWUnit MainTank
        {
            get
            {
                if (EnableProvingGrounds)
                {
                    var oto = Unit.ProvingGroundNPCs().FirstOrDefault(u => u.Entry == 72218);
                    return OracleRoutine.IsViable(oto) ? oto : StyxWoW.Me;
                }

                var tankList = Tanks.Where(t => t.Value.IsMainTank && OracleRoutine.IsViable(t.Value.Tank)).Select(u => u.Value.Tank).ToList();
                var maintTank = tankList.FirstOrDefault(u => !u.IsDead && u.Distance < 40);
                return OracleRoutine.IsViable(maintTank) ? maintTank : StyxWoW.Me;
            }
        }

        public static WoWUnit AssistTank
        {
            get
            {
                var tankList = Tanks.Where(t => t.Value.IsAssistTank && OracleRoutine.IsViable(t.Value.Tank)).Select(u => u.Value.Tank).ToList();
                var assistTank = tankList.FirstOrDefault(u => !u.IsDead && u.Distance < 40);
                return OracleRoutine.IsViable(assistTank) ? assistTank : StyxWoW.Me;
            }
        }

        public static WoWUnit PrimaryTank
        {
            get
            {
                switch (TankMode)
                {
                    case TankMode.Automatic:
                        return (OracleRoutine.IsViable(MainTank) && !MainTank.IsMe ? MainTank : OracleRoutine.IsViable(AssistTank) && !AssistTank.IsMe ? AssistTank : StyxWoW.Me);

                    case TankMode.Focus:
                        return StyxWoW.Me.FocusedUnit ?? (OracleRoutine.IsViable(MainTank) && !MainTank.IsMe ? MainTank : OracleRoutine.IsViable(AssistTank) && !AssistTank.IsMe ? AssistTank : StyxWoW.Me);

                    case TankMode.LowestHealthTank:
                        var mtHP = OracleRoutine.IsViable(MainTank) && !MainTank.IsMe ? MainTank.HealthPercent : 100;
                        var atHP = OracleRoutine.IsViable(AssistTank) && !AssistTank.IsMe ? AssistTank.HealthPercent : 100;
                        return ((atHP < 100) && (atHP < mtHP)) ? AssistTank : MainTank;

                    case TankMode.MainTank:
                        return MainTank;

                    case TankMode.OffTank:
                        return AssistTank;

                    case TankMode.TankSwap:
                        return (OracleRoutine.IsViable(MainTank) && !MainTank.IsMe && MainTank.IsActiveTank() ? MainTank : OracleRoutine.IsViable(AssistTank) && !AssistTank.IsMe ? AssistTank : StyxWoW.Me);

                    case TankMode.TankWithDebuff:
                        return (OracleRoutine.IsViable(MainTank) && MainTank.HasAnyAura(HashSets.TankDebuffs) ? MainTank : OracleRoutine.IsViable(AssistTank) && AssistTank.HasAnyAura(HashSets.TankDebuffs) ? AssistTank : StyxWoW.Me);
                }
                return null;
            }
        }

        public static Dictionary<ulong, TankCache> GetMainTanks()
        {
            const WoWPartyMember.GroupRole tankLeader = WoWPartyMember.GroupRole.Tank | WoWPartyMember.GroupRole.Leader;
            var infos = Unit.SearchAreaPlayers();
            var results = Group.WoWPartyMembers;
            var i = 0;

            //Logger.Output("====== Checking Party Roles ======");
            //foreach (var partyMember in results)
            //{
            //    if (!OracleRoutine.IsViable(partyMember.ToPlayer())) continue;
            //    Logger.Output("[{2}] {0} is {1} [MT: {3}] [AT: {4}]", partyMember.Specialization, partyMember.Role, i, partyMember.IsMainTank, partyMember.IsMainAssist);
            //    i++;
            //}

            Tanks.Clear();

            foreach (var player in infos.Where(p => ((int)p.GetRole() == 50) || (p.GetRole() == WoWPartyMember.GroupRole.Tank) || (p.GetRole() & tankLeader) == tankLeader))
            {
                //Tanks.Add(player.Guid, new TankCache(player.Guid, player, player.IsAssistTank(), player.IsMainTank()));

                // A little cheat until HB Fixs their Maintank detection
                if (Tanks.Count > 0)
                {
                    Tanks.Add(player.Guid, new TankCache(player.Guid, player, true, false)); continue;
                }
                Tanks.Add(player.Guid, new TankCache(player.Guid, player, false, true));
            }

            if (Tanks.Count == 0) Tanks.Add(StyxWoW.Me.Guid, new TankCache(StyxWoW.Me.Guid, StyxWoW.Me, StyxWoW.Me.IsAssistTank(), StyxWoW.Me.IsMainTank()));

            Logger.Output("======== {0} Tank(s) ====", OracleTanks.Tanks.Count);
            foreach (var tank in OracleTanks.Tanks.Values.ToList())
            {
                if (tank.Tank.Name != null) Logger.Output("[*] {0} [MT: {1}] [AT: {2}] ", tank.Tank.SafeName, tank.IsMainTank, tank.IsAssistTank);
            }
            Logger.Output("======================");

            return Tanks;
        }

        public struct TankCache
        {
            public TankCache(ulong guid, WoWUnit tank, bool isAssist, bool isMain, bool isPrime = false)
                : this()
            {
                GUID = guid;
                Tank = tank;
                IsAssistTank = isAssist;
                IsMainTank = isMain;
                IsPrimaryTank = isPrime;
            }

            private ulong GUID { get; set; }

            public bool IsAssistTank { get; private set; }

            public bool IsMainTank { get; set; }

            public bool IsPrimaryTank { get; set; }

            public WoWUnit Tank { get; private set; }
        }
    }
}