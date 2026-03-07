using FuryUnleashed.Core.Utilities;
using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Common;

namespace FuryUnleashed.Core.Helpers
{
    public class DamageTracker
    {
        #region DamageTracker

        private static Dictionary<DateTime, double> _damageTaken;
        private static bool AddingDamageTaken { get; set; }
        private static bool RemovingDamageTaken { get; set; }

        private static bool _pulseDamageTracker;

        public static void Initialize()
        {
            try
            {
                if (Unit.IgnoreDamageTracker) 
                    return;

                _damageTaken = new Dictionary<DateTime, double>();
                CombatLogHandler.Initialize();
                AttachCombatLogEvent();
                _pulseDamageTracker = true;
                Logger.CombatLogFb("Damage Tracker Initialized.");
            }
            catch (Exception ex)
            {
                Logger.DiagLogFb("FU: Damage Tracker failed to initialize - {0}", ex);
            }
        }

        public static void Stop()
        {
            try
            {
                DetachCombatLogEvent();
                CombatLogHandler.Shutdown();
                _pulseDamageTracker = false;
                Logger.CombatLogFb("Damage Tracker Stopped - Possibly for reinitialize.");
            }
            catch (Exception ex)
            {
                Logger.DiagLogFb("FU: Damage Tracker failed to stop - {0}", ex);
            }
        }

        public static void Pulse()
        {
            try
            {
                if (Unit.IgnoreDamageTracker || _pulseDamageTracker == false)
                    return;

                RemoveDamageTaken(DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.DiagLogFb("FU: Damage Tracker failed to RemoveDamageTaken - {0}", ex);
            }
        }

        private static void AttachCombatLogEvent()
        {
            CombatLogHandler.Register("SWING_DAMAGE", HandleCombatLog);
            CombatLogHandler.Register("SPELL_DAMAGE", HandleCombatLog);
            CombatLogHandler.Register("RANGE_DAMAGE", HandleCombatLog);
        }

        private static void DetachCombatLogEvent()
        {
            CombatLogHandler.Remove("SWING_DAMAGE");
            CombatLogHandler.Remove("SPELL_DAMAGE");
            CombatLogHandler.Remove("RANGE_DAMAGE");
        }

        private static void HandleCombatLog(CombatLogEventArgs args)
        {
            switch (args.Event)
            {
                case "SWING_DAMAGE":
                    if (args.DestGuid == Root.MyToonGuid)
                    {
                        object damage = args.Amount;
                        if (!AddingDamageTaken)
                            AddDamageTaken(DateTime.Now, (int)damage);
                    }
                    break;

                case "RANGE_DAMAGE":
                    if (args.DestGuid == Root.MyToonGuid)
                    {
                        object damage = args.Amount;
                        if (!AddingDamageTaken)
                            AddDamageTaken(DateTime.Now, (int)damage);
                    }
                    break;

                case "SPELL_DAMAGE":
                    if (args.DestGuid == Root.MyToonGuid && StyxWoW.Me.Specialization != WoWSpec.WarriorProtection)
                    {
                        bool countDamage = args.SourceName != null || (args.SpellName == "Spirit Link" && args.SourceName == "Spirit Link Totem");

                        object damage = args.Amount;
                        if (countDamage && !AddingDamageTaken)
                            AddDamageTaken(DateTime.Now, (int)damage);
                    }
                    break;
            }
        }

        private static void AddDamageTaken(DateTime timestamp, int damage)
        {
            try
            {
                if (RemovingDamageTaken) return;

                AddingDamageTaken = true;
                _damageTaken[timestamp] = damage;
                AddingDamageTaken = false;
                Logging.WriteDiagnostic(" Adding Damage amount {0}", damage);
            }
            catch (Exception ex)
            {
                _damageTaken[timestamp] = _damageTaken.ContainsKey(timestamp) ? damage : 0;
                AddingDamageTaken = false;

                Logger.DiagLogWh("AddDamageTaken : {0}", ex);
            }
        }

        private static void RemoveDamageTaken(DateTime timestamp)
        {
            TimeSpan lastSeconds = TimeSpan.FromSeconds(6);

            try
            {
                if (AddingDamageTaken) return;

                RemovingDamageTaken = true;
                Dictionary<DateTime, double> removeList = new Dictionary<DateTime, double>();

                foreach (var entry in _damageTaken.Where(entry => timestamp - entry.Key > lastSeconds + TimeSpan.FromSeconds(4)))
                {
                    removeList.Add(entry.Key, entry.Value);
                }

                foreach (var entry in removeList)
                {
                    _damageTaken.Remove(entry.Key);
                }
                RemovingDamageTaken = false;
            }
            catch (Exception ex)
            {
                Logger.DiagLogWh("RemovingDamageTaken : {0}", ex);
            }
        }

        internal static double GetDamageTaken(DateTime timestamp = default(DateTime), double lastseconds = 6)
        {
            DateTime current = timestamp;
            TimeSpan lastSeconds = TimeSpan.FromSeconds(lastseconds);

            return (from entry in _damageTaken
                    let diff = current - entry.Key
                    where diff <= lastSeconds && diff >= TimeSpan.FromSeconds(0)
                    select entry).Aggregate<KeyValuePair<DateTime, double>, double>(0, (current1, entry) => current1 + entry.Value);
        }
        #endregion

        #region Fury-Spec Functions
        public static bool CalculatePreferredStance()
        {
            using (new PerformanceLogger("CalculatePreferredStance"))
            {
                try
                {
                    var healthtopercent = StyxWoW.Me.MaxHealth / 100; // Calculate Health per 1%.
                    var damageoverthreeseconds = GetDamageTaken(DateTime.Now, 3); // Retrieve damage taken over 3 seconds.
                    var damagetorage = (damageoverthreeseconds / healthtopercent) / 3; // Generates 1 rage per 1% lost per second -> Getting % HP lost average per second over last 3 seconds.

                    var battlestancerage = Item.AttackSpeed * 3.5; // Weaponspeed * 3.5 to get normalized rage.
                    var berserkerstancerage = (battlestancerage * 0.5) + damagetorage; // Half of normalized rage + Rage from Damage.

                    Logger.DiagLogWh("FU: Battle Stance Rage: {0} - Berserker Stance Rage: {1}", battlestancerage, berserkerstancerage);

                    return berserkerstancerage > battlestancerage;
                }
                catch (Exception exstancecalc)
                {
                    Logger.DiagLogFb("FU: Failed CalculatePreferredStance - {0}", exstancecalc);
                }
            }
            return false;
        }
        #endregion

        #region Protection-Spec Functions
        internal const int ShieldBarrierSpellId = 112048;
        internal const int ShieldBlockSpellId = 132404;

        public static double CalculateEstimatedAbsorbValue()
        {
            using (new PerformanceLogger("CalculateEstimatedAbsorbValue"))
            {
                try
                {
                    double sumstam;

                    var attackpower = StyxWoW.Me.AttackPower;
                    var strength = StyxWoW.Me.Strength;
                    var stamina = StyxWoW.Me.Stamina;

                    var sumrage = StyxWoW.Me.CurrentRage > 60 ? 60 : StyxWoW.Me.CurrentRage;

                    if (sumrage < 20) sumrage = 20;

                    var ragemultiplier = sumrage / 60.0;

                    if (1.8 * (attackpower - 2 * strength) > stamina * 2.5) { sumstam = 2 * (attackpower - 2 * strength); }
                    else sumstam = stamina * 2.5;

                    var barrierresult = sumstam * ragemultiplier;

                    return barrierresult;
                }
                catch (Exception exabsorbcalc)
                {
                    Logger.DiagLogFb("FU: Failed CalculateEstimatedAbsorbValue - {0}", exabsorbcalc);
                }
            }
            return 0;
        }

        // Crit Block Formula: (http://www.wowwiki.com/Critical_Block)
        // Mastery Ratio is 272.7 (http://wowpedia.org/Mastery)
        //
        // var criticalBlockChance = (mastery * 2.2) / 100; // Shield Maid version ...
        public static double CalculateEstimatedBlockValue()
        {
            using (new PerformanceLogger("CalculateEstimatedBlockValue"))
            {
                try
                {
                    var damageoversixseconds = GetDamageTaken(DateTime.Now); // Verified and Correct
                    var mastery = StyxWoW.Me.Mastery; // Verified and Correct
                    var criticalBlockChance = (8 * 1.5) + (mastery / 272.7 * 1.5); // Verified and Correct
                    var blockresult = damageoversixseconds * criticalBlockChance * 0.6 + damageoversixseconds * (1 - criticalBlockChance) * 0.3;

                    return blockresult;
                }
                catch (Exception exblockcalc)
                {
                    Logger.DiagLogFb("FU: Failed CalculateEstimatedBlockValue - {0}", exblockcalc);
                }
            }
            return 1;
        }
        #endregion
    }
}
