#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Chronicle.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.CombatLog;
using Oracle.Core.DataStores;
using Oracle.Shared.Utilities;
using Styx;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Healing.Chronicle
{
    internal class Chronicle
    {
        // A factual written account of important or historical events in the order of their occurrence.

        private static DateTime _starttime;

        public static Chronicle Instance { get; private set; }

        public static Dictionary<string, Encounter> Encounters;

        public static Encounter Current { get; private set; }

        public static Encounter Total { get; private set; }

        public static bool Disabled { get; private set; }

        static Chronicle()
        {
            // Make sure we have a singleton instance!
            Instance = new Chronicle();
        }

        public static void Initialize()
        {
            _starttime = DateTime.Now;

            Encounters = new Dictionary<string, Encounter>();
            Shields = new Dictionary<string, Shield>();

            Disabled = false; // TODO: setting here.

            CombatLogHandler.Initialize();

            CombatLogHandler.Register("SPELL_DAMAGE", SPELL_DAMAGE);
            CombatLogHandler.Register("SPELL_HEAL", SPELL_HEAL);
            CombatLogHandler.Register("SPELL_PERIODIC_HEAL", SPELL_HEAL);
            CombatLogHandler.Register("SPELL_AURA_APPLIED", SPELL_AURA_APPLIED);
            CombatLogHandler.Register("SPELL_AURA_REFRESH", SPELL_AURA_REFRESH);
            CombatLogHandler.Register("SPELL_AURA_REMOVED", SPELL_AURA_REMOVED);

            StartEncounter();
        }

        public static void Shutdown()
        {
            CombatLogHandler.Shutdown();
        }

        #region Methods

        private static Player FindPlayer(Encounter encounter, ulong playerId)
        { return encounter != null ? (from p in encounter.Players where p.Key == playerId select p.Value).FirstOrDefault() : null; }

        public static Player GetPlayer(Encounter encounter, ulong playerId, string playername)
        {
            using (new PerformanceLogger("GetPlayer"))
            {
                var delimiter = new[] { '-' };

                var player = FindPlayer(encounter, playerId);

                if (player == null)
                {
                    var pname = playername.Split(delimiter)[0] ?? playername;

                    var unitClass = "Uknown";//todo: Styx.WoWInternals.Lua.GetReturnVal<string>("return select(2, UnitClass(" + pname + "))", 0);
                    player = new Player { Id = playerId, Class = unitClass, Name = pname, First = DateTime.Now };

                    if (encounter != null) encounter.Players.Add(playerId, player);
                }

                //  The total encounter clears out first and last timestamps.
                if (player.First.HasValue) player.First = DateTime.Now;

                player.First = DateTime.Now;

                return player;
            }
        }

        protected static double GetPlayerActiveTime(Encounter encounter, Player player)
        {
            using (new PerformanceLogger("GetPlayerActiveTime"))
            {
                Double maxTime = 0;

                // Add recorded time (for total encounter)
                if (player.Duration > 0) maxTime = player.Duration;

                var duration = (TimeSpan)(player.Last - player.First);

                // Add in-progress time if encounter is not ended.
                if (!encounter.EndTime.HasValue && player.First.HasValue) maxTime = maxTime + duration.TotalSeconds;

                return maxTime;
            }
        }

        private static void SetPlayerActiveTime(Encounter encounter)
        {
            using (new PerformanceLogger("SetPlayerActiveTime"))
            {
                foreach (var p in encounter.Players)
                {
                    if (!p.Value.Last.HasValue) continue;
                    var duration = (TimeSpan)(p.Value.Last - p.Value.First);
                    p.Value.Duration = p.Value.Duration + duration.TotalSeconds;
                }
            }
        }

        private static Encounter FindEncounter(string encounter)
        {
            using (new PerformanceLogger("FindEncounter"))
            {
                if (encounter == "Current" && Chronicle.Current.Active) return Chronicle.Current;
                if (encounter == "Total" && Chronicle.Total.Active) return Chronicle.Total;
                return null;
            }
        }

        public void ResetEncounter()
        {
            using (new PerformanceLogger("ResetEncounter"))
            {
                Chronicle.Current = new Encounter();
                Chronicle.Total = new Encounter();
                Encounters = new Dictionary<string, Encounter>();
            }
        }

        private static void DeleteEncounter(Encounter encounter)
        {
            using (new PerformanceLogger("DeleteEncounter"))
            {
                foreach (var enc in Encounters.ToList())
                {
                    if (enc.Key == encounter.Name) Encounters.Remove(encounter.Name);
                }
            }
        }

        public static void Pulse()
        {
            var inCombat = StyxWoW.Me.Combat;

            if (!Disabled && !inCombat) EndEncounter();

            if (inCombat)
                OnCombatStarted();

            //ChronicleHealing.Report(Current);
            //ChronicleHealing.Report(Total);
        }

        public static void OnCombatStarted()
        {
            if (!Disabled /* && !Chronicle.Current.Active*/) StartEncounter();
        }

        public static void StartEncounter()
        {
            /*if (!Chronicle.Current.Active)*/
            Chronicle.Current = CreateEncounter("Current");
            /*if (!Chronicle.Total.Active) */
            Chronicle.Total = CreateEncounter("Total");
        }

        private static Encounter CreateEncounter(string encounterName)
        {
            var encounter = new Encounter { Name = encounterName, StartTime = DateTime.Now, Duration = 0, Active = true }; return encounter;
        }

        private static void EndEncounter()
        {
            using (new PerformanceLogger("EndEncounter"))
            {
                if ( /*Chronicle.Current.GotBoss && Chronicle.Current.MobName != null &&*/
                    DateTime.Now - Chronicle.Current.StartTime > TimeSpan.FromSeconds(5))
                {
                    Chronicle.Current.EndTime = DateTime.Now;

                    var duration = (TimeSpan)(Chronicle.Current.EndTime - Chronicle.Current.StartTime);
                    Chronicle.Current.Duration = duration.TotalSeconds;

                    SetPlayerActiveTime(Chronicle.Current);

                    if (Chronicle.Current.MobName != null) Chronicle.Current.Name = Chronicle.Current.MobName;

                    var encountersName = Chronicle.Current.Name + ", " + Chronicle.Current.StartTime + " to " +
                                         Chronicle.Current.EndTime;
                    Encounters.Add(encountersName, Chronicle.Current);
                }

                // Add time spent to total set as well.
                Chronicle.Total.Duration = Chronicle.Total.Duration + Chronicle.Current.Duration;

                SetPlayerActiveTime(Chronicle.Total);

                // Set player.first and player.last to null in total set.
                // Neccessary since first and last has no relevance over an entire raid.
                foreach (var p in Chronicle.Total.Players)
                {
                    p.Value.First = null;
                    p.Value.Last = null;
                }

                Chronicle.Current = new Encounter(); // Reset current encounter
            }
        }

        #endregion Methods

        #region Combat Log

        private static void SPELL_DAMAGE(CombatLogEventArgs args)
        {
            // We use this to track bossnames
            var destFlags = args.DestFlags;
            var dstName = args.DestName;

            if (!destFlags.HasFlag(SourceFlags.ReactionHostile)) return;

            if (!Chronicle.Current.GotBoss && BossList.CurrentMapBosses.Contains(dstName))
            {
                Chronicle.Current.MobName = dstName;
                Chronicle.Current.GotBoss = true;
            }
            Chronicle.Current.MobName = dstName; // JUST RECORD A HOSTILE!
        }

        private static Heal heal;

        private static void SPELL_HEAL(CombatLogEventArgs args)
        {
            heal.DstName = args.DestName;
            heal.Playerid = args.SourceGuid;
            heal.Playername = args.SourceName;
            heal.Spellid = args.SpellId;
            heal.Spellname = args.SpellName;
            heal.Amount = args.Amount;
            heal.Overhealing = args.Overhealing;
            heal.Critical = args.Critical;
            heal.Absorbed = args.Absorbed;

            ChronicleHealing.LogHeal(Current, heal);
            ChronicleHealing.LogHeal(Total, heal);
        }

        private static Dictionary<string, Shield> Shields;

        private static void SPELL_AURA_APPLIED(CombatLogEventArgs args)
        {
            var amount = args.Amount;

            if (amount == 0) return;

            var dstName = args.DestName;
            var spellId = args.SpellId;
            var srcName = args.SourceName;
            var key = dstName + spellId + srcName;

            Shield result;
            if (Shields.TryGetValue(key, out result))
            {
                result.Amount = amount;
                return;
            }

            Shields[key] = new Shield { DstName = dstName, Spellid = spellId, Playername = srcName, Amount = amount };
        }

        private static void SPELL_AURA_REFRESH(CombatLogEventArgs args)
        {
            var amount = args.Amount;

            if (amount == 0) return;

            var dstName = args.DestName;
            var spellId = args.SpellId;
            var srcName = args.SourceName;
            var key = dstName + spellId + srcName;

            Shield result;
            if (Shields.TryGetValue(key, out result))
            {
                var prevShield = result.Amount;

                if (prevShield > amount)
                {
                    heal.DstName = args.DestName;
                    heal.Playerid = args.SourceGuid;
                    heal.Playername = args.SourceName;
                    heal.Spellid = args.SpellId;
                    heal.Spellname = args.SpellName;
                    heal.Amount = prevShield - amount;
                    heal.Overhealing = 0;
                    heal.Critical = false;
                    heal.Absorbed = 0;

                    ChronicleHealing.LogHeal(Current, heal, true);
                    ChronicleHealing.LogHeal(Total, heal, true);

                    return;
                }
            }

            Shields[key] = new Shield { DstName = dstName, Spellid = spellId, Playername = srcName, Amount = amount };
        }

        private static void SPELL_AURA_REMOVED(CombatLogEventArgs args)
        {
            var amount = args.Amount;

            if (amount == 0) return;

            var dstName = args.DestName;
            var spellId = args.SpellId;
            var srcName = args.SourceName;
            var key = dstName + spellId + srcName;

            Shield result;
            if (Shields.TryGetValue(key, out result))
            {
                var prevShield = result.Amount;

                if (prevShield > amount)
                {
                    heal.DstName = args.DestName;
                    heal.Playerid = args.SourceGuid;
                    heal.Playername = args.SourceName;
                    heal.Spellid = args.SpellId;
                    heal.Spellname = args.SpellName;
                    heal.Amount = prevShield;
                    heal.Overhealing = args.Amount;
                    heal.Critical = false;
                    heal.Absorbed = 0;

                    ChronicleHealing.LogHeal(Current, heal, true);
                    ChronicleHealing.LogHeal(Total, heal, true);

                    return;
                }
            }

            Shields.Remove(key);
        }

        #endregion Combat Log

        #region Classes / Structs

        internal class Encounter
        {
            public int Id { get; set; }

            public bool Active { get; set; }

            public string Name { get; set; }

            public DateTime? StartTime { get; set; }

            public DateTime? EndTime { get; set; }

            public double Duration { get; set; }

            public Dictionary<ulong, Player> Players { get; private set; }

            public bool GotBoss { get; set; }

            public string MobName { get; set; }

            public int Healing { get; set; }                                 // Total healing

            public int Shielding { get; set; }                               // Total shields

            public int Overhealing { get; set; }                            // Overheal total

            public int Healingabsorbed { get; set; }                      // Absorbed total

            public int Absorbed { get; set; }

            public Encounter()
            {
                Players = new Dictionary<ulong, Player>();
                Active = false;
                GotBoss = false;
            }
        }

        internal class Player
        {
            public ulong Id { get; set; }

            public string Class { get; set; }

            public string Name { get; set; }

            public DateTime? First { get; set; }

            public DateTime? Last { get; set; }

            public double Duration { get; set; }

            public Dictionary<string, Healed> PlayerHealed { get; set; }          // Stored healing per recipient

            public int Healing { get; set; }                                // Total healing

            public int Shielding { get; set; }                              // Total shields

            public Dictionary<string, Spell> Healingspells { get; set; }    // Healing spells

            public int Overhealing { get; set; }                            // Overheal total

            public int Healingabsorbed { get; set; }                        // Absorbed total

            public Player()
            {
                Healingspells = new Dictionary<string, Spell>();
                PlayerHealed = new Dictionary<string, Healed>();
            }
        }

        internal class Spell
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int Hits { get; set; }

            public int Healing { get; set; }

            public int Overhealing { get; set; }

            public int Absorbed { get; set; }

            public int Shielding { get; set; }

            public int Critical { get; set; }

            public int Min { get; set; }

            public int Max { get; set; }

            public int MaxHeal { get; set; }
        }

        internal struct Heal
        {
            public string DstName;
            public ulong Playerid;
            public string Playername;
            public int Spellid;
            public string Spellname;
            public int Amount;
            public int Overhealing;
            public bool Critical;
            public int Absorbed;
        }

        internal struct Shield
        {
            public string DstName;
            public string Playername;
            public int Spellid;
            public int Amount;
        }

        internal struct Healed
        {
            public string Class;
            public int Shielding;
            public int Amount;
        }

        #endregion Classes
    }
}