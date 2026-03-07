using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using PureRotation.Core.CombatLog;
/*
 Current Changes:
 * added NeedRefresh
 * added NeedMeleeRefresh
 * added NeedCasterRefresh
 * NeedRefresh - Looks first for your class specc to choose the correct check
 * NeedMeleeRefresh - checks for AP, Crit, MeleeHasty, Mastery
 * NeedCasterRefresh - checks for SP,Crit, CasterHaste, Mastery
*/
namespace PureRotation.Helpers
{
    [UsedImplicitly]
    internal static class DoTTracker
    {
        #region structures

        public struct StatInfo
        {
            public int SpellId;
            public float AttackPower;
            public float CritChance;
            public float Mastery;
            public float Intellect;
            public float SpellPower;
            public float MeleeHaste;
            public float SpellHaste;

            public DateTime TimeTracked { get; set; }

            /// <summary>
            /// Will create an info with empty stats
            /// </summary>
            public StatInfo(int SpellId, float AttackPower, float CritChance, float Mastery, float Intellect, float SpellPower, float MeleeHaste, float SpellHaste, DateTime TimeTracked)
                : this()
            {
                this.SpellId = 0;
                this.AttackPower = 0;
                this.CritChance = 0;
                this.Mastery = 0;
                this.Intellect = 0;
                this.SpellPower = 0;
                this.MeleeHaste = 0;
                this.SpellHaste = 0;
                this.TimeTracked = new DateTime();
            }
        }

        #endregion structures

        # region variables

        public static bool Initialized;

        private static readonly Dictionary<int, StatInfo> StatInfos = new Dictionary<int, StatInfo>();

        private static bool _combatLogAttached;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static void Initialize()
        {
            AttachCombatLogEvent();
            Initialized = true;
        }

        public static void Pulse()
        {
        }

        public static void Shutdown()
        {
            if (!Initialized) return;
            StatInfos.Clear();
            Initialized = false;
        }

        private static void UpdateStatInfo(ulong destGuid, int spellId)
        {
            // Add the new damage taken data
            try
            {
                Lua._secondaryStats.Refresh();
                var hash = SpellUnitHash(destGuid, spellId);
                if (!StatInfos.ContainsKey(hash))
                {
                    var di = new StatInfo { AttackPower = Lua._secondaryStats.AttackPower, Intellect = Lua._secondaryStats.Intellect, SpellPower = Lua._secondaryStats.SpellPower, TimeTracked = DateTime.Now, CritChance = Lua._secondaryStats.Crit, MeleeHaste = Lua._secondaryStats.MeleeHaste, SpellHaste = Lua._secondaryStats.SpellHaste, Mastery = Lua._secondaryStats.Mastery };
                    Logger.DebugLog("Added {0} with stats AP {1}, Int {2}, Crit {3}, MeleeHaste {4}, SpellHaste {5}, Mastery {6}, SpellPower {7}",
                        spellId,
                        Lua._secondaryStats.AttackPower,
                        Lua._secondaryStats.Intellect,
                        Lua._secondaryStats.Crit,
                        Lua._secondaryStats.MeleeHaste,
                        Lua._secondaryStats.SpellHaste,
                        Lua._secondaryStats.Mastery,
                        Lua._secondaryStats.SpellPower);
                    StatInfos.Add(hash, di);
                }
                else
                {
                    Logger.DebugLog("Updated {0} with stats AP {1}, Int {2}, Crit {3}, MeleeHaste {4}, SpellHaste {5}, Mastery {6}, SpellPower {7}",
                        spellId,
                        Lua._secondaryStats.AttackPower,
                        Lua._secondaryStats.Intellect,
                        Lua._secondaryStats.Crit,
                        Lua._secondaryStats.MeleeHaste,
                        Lua._secondaryStats.SpellHaste,
                        Lua._secondaryStats.Mastery,
                        Lua._secondaryStats.SpellPower);
                    StatInfo di = StatInfos[hash];
                    di.TimeTracked = DateTime.Now;
                    di.SpellId = spellId;
                    di.AttackPower = Lua._secondaryStats.AttackPower;
                    di.Intellect = Lua._secondaryStats.Intellect;
                    di.TimeTracked = DateTime.Now;
                    di.CritChance = Lua._secondaryStats.Crit;
                    di.MeleeHaste = Lua._secondaryStats.MeleeHaste;
                    di.SpellHaste = Lua._secondaryStats.SpellHaste;
                    di.Mastery = Lua._secondaryStats.Mastery;
                    di.SpellPower = Lua._secondaryStats.SpellPower;
                    StatInfos[hash] = di;
                }
            }
            catch (Exception ex)
            {
                Logger.DebugLog("UpdateStatInfo : {0}", ex);
            }
        }

        public static StatInfo SpellStats(WoWUnit u, int spellId)
        {
            if (u!=null && StatInfos.ContainsKey(SpellUnitHash(u, spellId)))
            {
                return StatInfos[SpellUnitHash(u, spellId)];
            }
            return new StatInfo();
        }

        private static void AttachCombatLogEvent()
        {
            if (_combatLogAttached)
                return;
            CombatLogHandler.Register("SPELL_AURA_APPLIED", HandleAura);
            CombatLogHandler.Register("SPELL_AURA_REFRESH", HandleAura);
            CombatLogHandler.Register("SPELL_AURA_REMOVED", HandleAura);

            Logger.InfoLog("[DotTracker] Attached to combat log");
            _combatLogAttached = true;
        }
        private static void HandleAura(CombatLogEventArgs args)
        {
            UpdateStatInfo(args.DestGuid, args.SpellId);
        }
        private static void RemoveAura(CombatLogEventArgs args)
        {
            RemoveDotDamage(args.DestGuid, args.SpellId);
        }
        private static void RemoveDotDamage(ulong destGuid, int spellId)
        {
            TimeSpan lastSeconds = TimeSpan.FromSeconds(600); // our window

            try
            {
                //directly removing throws errors, so we have to do a workaround
                Dictionary<int, StatInfo> RemoveList = new Dictionary<int, StatInfo>();

                // Remove any data older than lastSeconds
                foreach (var entry in StatInfos.Where(entry => entry.Value.TimeTracked < DateTime.Now - lastSeconds))
                {
                    RemoveList.Add(entry.Key, entry.Value);

                    //DotInfos.Remove(entry.Key);
                }
                foreach (var entry in RemoveList)
                {
                    StatInfos.Remove(entry.Key);
                }

                //Remove aura from tracking if faded from target
                var hash = SpellUnitHash(destGuid, spellId);
                if (StatInfos.ContainsKey(hash)) StatInfos.Remove(hash);
            }
            catch (Exception ex)
            {
                Logger.DebugLog("RemoveDotDamage : {0}", ex);
            }
        }

        private static int SpellUnitHash(WoWUnit u, int spellId)
        {
            return u!=null?SpellUnitHash(u.Guid, spellId):0;
        }

        private static int SpellUnitHash(ulong guid, int spellId)
        {
            string toHash = guid.ToString(CultureInfo.InvariantCulture) + '_' + spellId.ToString(CultureInfo.InvariantCulture);
            return toHash.GetHashCode();
        }
        public static bool NeedRefresh(WoWUnit u, int spellId)
        {
            var _class = Me.Class;
            var _specc = Me.Specialization;
            if(_class==WoWClass.None ||
               _class==WoWClass.DeathKnight ||
               _class==WoWClass.Hunter || 
               _class==WoWClass.Rogue ||
               _class==WoWClass.Warrior ||
               (_class==WoWClass.Paladin && (_specc==WoWSpec.PaladinProtection || _specc==WoWSpec.PaladinRetribution)) ||
               (_class==WoWClass.Druid &&(_specc==WoWSpec.DruidFeral || _specc==WoWSpec.DruidGuardian)) ||
                (_class==WoWClass.Monk &&(_specc==WoWSpec.MonkBrewmaster || _specc==WoWSpec.MonkWindwalker)) ||
                (_class==WoWClass.Shaman && _specc==WoWSpec.ShamanEnhancement)
                )
                return NeedMeleeRefresh(u,spellId);
            if (_class == WoWClass.Priest ||
                _class == WoWClass.Mage ||
               _class == WoWClass.Warlock ||
               (_class == WoWClass.Paladin && (_specc == WoWSpec.PaladinHoly)) ||
               (_class == WoWClass.Druid && (_specc == WoWSpec.DruidBalance || _specc == WoWSpec.DruidRestoration)) ||
                (_class == WoWClass.Monk && _specc == WoWSpec.MonkMistweaver) ||
                (_class == WoWClass.Shaman && _specc == WoWSpec.ShamanRestoration || _specc==WoWSpec.ShamanElemental)
                )
                return NeedCasterRefresh(u, spellId);
            return false;
        }

        public static bool NeedMeleeRefresh(WoWUnit u, int spellId)
        {
            if (u != null)
            {
                bool raisedAP = (SpellStats(u, spellId).AttackPower + 5000 < Lua._secondaryStats.AttackPower);
                bool raisedCrit = (SpellStats(u, spellId).CritChance + 1000 < Lua._secondaryStats.Crit);
                bool raisedHaste = (SpellStats(u, spellId).MeleeHaste + 1000 < Lua._secondaryStats.MeleeHaste);
                bool raisedMastery = (SpellStats(u, spellId).Mastery + 1000 < Lua._secondaryStats.Mastery);
                //we tested it, we want to see the reason in the log!
                if (raisedAP || raisedCrit || raisedHaste || raisedMastery)
                {
                    Logger.DebugLog("[DotTracker] Spell {0} applied with {1} AP - {2} Crit - {3} Haste - {4} Mastery / stats now {5} AP - {6} Crit - {7} Haste - {8} Mastery",
                        spellId,
                        SpellStats(u, spellId).AttackPower,
                        SpellStats(u, spellId).CritChance,
                        SpellStats(u, spellId).MeleeHaste,
                        SpellStats(u, spellId).Mastery,
                        Lua._secondaryStats.AttackPower,
                        Lua._secondaryStats.Crit,
                        Lua._secondaryStats.MeleeHaste,
                        Lua._secondaryStats.Mastery);
                }
                return raisedAP || raisedCrit || raisedHaste || raisedMastery;
            }
            return false;
        }

        public static bool NeedCasterRefresh(WoWUnit u, int spellId)
        {
            if (u != null)
            {
                bool raisedSP = (SpellStats(u, spellId).SpellPower + 1000 < Lua._secondaryStats.SpellPower);
                bool raisedCrit = (SpellStats(u, spellId).CritChance + 1500 < Lua._secondaryStats.Crit);
                bool raisedHaste = (SpellStats(u, spellId).SpellHaste + 1500 < Lua._secondaryStats.SpellHaste);
                bool raisedMastery = (SpellStats(u, spellId).Mastery + 1500 < Lua._secondaryStats.Mastery);
                //we tested it, we want to see the reason in the log!
                if (raisedSP || raisedCrit || raisedHaste || raisedMastery)
                {
                    Logger.DebugLog("[DotTracker] Spell {0} applied with {1} SP - {2} Crit - {3} Haste - {4} Mastery / stats now {5} SP - {6} Crit - {7} Haste - {8} Mastery",
                        spellId,
                        SpellStats(u, spellId).SpellPower,
                        SpellStats(u, spellId).CritChance,
                        SpellStats(u, spellId).SpellHaste,
                        SpellStats(u, spellId).Mastery,
                        Lua._secondaryStats.AttackPower,
                        Lua._secondaryStats.Crit,
                        Lua._secondaryStats.MeleeHaste,
                        Lua._secondaryStats.Mastery);
                }
                return raisedSP || raisedCrit || raisedHaste || raisedMastery;
            }
            return false;
        }
        #endregion
    }
}