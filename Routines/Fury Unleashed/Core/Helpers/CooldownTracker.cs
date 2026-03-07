using System;
using System.Collections.Generic;
using FuryUnleashed.Core.Utilities;
using Styx.CommonBot;
using Styx.WoWInternals;

namespace FuryUnleashed.Core.Helpers
{
    internal static class CooldownTracker
    {
        // ReSharper disable once InconsistentNaming
        internal static Dictionary<WoWSpell, DateTime> cooldowns = new Dictionary<WoWSpell, DateTime>();

        // Adding casted spells to dictionary with datetime
        internal static void SpellUsed(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                cooldowns[result] = DateTime.Now.Add(result.CooldownTimeLeft);
            }
        }

        internal static void SpellUsed(int spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                cooldowns[result] = DateTime.Now.Add(result.CooldownTimeLeft);
            }
        }

        // Get the CooldownTimeLeft on a spell, by default it will only return the true result at less than a second.
        public static TimeSpan GetSpellCooldown(string spell, int buffer = 1, bool cdoverride = true)
        {
            using (new PerformanceLogger("GetSpellCooldown"))
            {
                if (cdoverride) return CooldownTimeLeft(spell);

                SpellFindResults results;
                if (SpellManager.FindSpell(spell, out results))
                {
                    return results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft;
                }

                return TimeSpan.MaxValue;                
            }
        }

        public static TimeSpan GetSpellCooldown(int spell, int buffer = 1, bool cdoverride = true)
        {
            using (new PerformanceLogger("GetSpellCooldown"))
            {
                if (cdoverride) return CooldownTimeLeft(spell);

                SpellFindResults results;
                if (SpellManager.FindSpell(spell, out results))
                {
                    return results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft;
                }

                return TimeSpan.MaxValue;
            }
        }

        // Checks if spell is on cooldown.
        public static bool SpellOnCooldown(string spell, bool cdoverride = true)
        {
            using (new PerformanceLogger("SpellOnCooldown"))
            {
                if (cdoverride) return IsOnCooldown(spell);

                SpellFindResults results;
                if (SpellManager.FindSpell(spell, out results))
                {
                    return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
                }

                return false;
            }

        }

        public static bool SpellOnCooldown(int spell, bool cdoverride = true)
        {
            using (new PerformanceLogger("SpellOnCooldown"))
            {
                if (cdoverride) return IsOnCooldown(spell);

                SpellFindResults results;
                if (SpellManager.FindSpell(spell, out results))
                {
                    return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
                }

                return false;
            }
        }

        // Magic behind it ...
        private static bool IsOnCooldown(int spell, int buffer = 1)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.CooldownTrackerLog("FU: Checked cooldown for {0} at {1}", spell, DateTime.Now);
                            return result.Cooldown;
                        }
                        Logger.CooldownTrackerLog("FU: Found {0} but not ready to check at {1}", spell, DateTime.Now);
                        return true;
                    }
                }
                catch
                {
                    Logger.CooldownTrackerLog("FU: FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return false;
                }
            }
            else
            {
                Logger.CooldownTrackerLog("FU: FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return false;
        }

        private static bool IsOnCooldown(string spell, int buffer = 1)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.CooldownTrackerLog("FU: Checked cooldown for {0} at {1}", spell, DateTime.Now);
                            return result.Cooldown;
                        }

                        Logger.CooldownTrackerLog("FU: Found {0} but not ready to check at {1}", spell, DateTime.Now);
                        return true;
                    }
                }
                catch
                {
                    Logger.CooldownTrackerLog("FU: FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return false;
                }
            }
            else
            {
                Logger.CooldownTrackerLog("FU: FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return false;
        }

        private static TimeSpan CooldownTimeLeft(int spell, int buffer = 1)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.CooldownTrackerLog("FU: Checked CooldownTimeLeft for {0} at {1}", spell, DateTime.Now);
                            return result.CooldownTimeLeft;
                        }

                        Logger.CooldownTrackerLog("FU: Found {0} but not ready to check CooldownTimeLeft {1}", spell, DateTime.Now);
                        return TimeSpan.FromSeconds(10);
                    }
                }
                catch
                {
                    Logger.CooldownTrackerLog("FU: FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return TimeSpan.MaxValue;
                }
            }
            else
            {
                Logger.CooldownTrackerLog("FU: FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return TimeSpan.MaxValue;
        }

        private static TimeSpan CooldownTimeLeft(string spell, int buffer = 1)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.CooldownTrackerLog("FU: Checked CooldownTimeLeft for {0} at {1}", spell, DateTime.Now);
                            return result.CooldownTimeLeft;
                        }

                        Logger.CooldownTrackerLog("FU: Found {0} but not ready to check CooldownTimeLeft {1}", spell, DateTime.Now);
                        return TimeSpan.FromSeconds(10);
                    }
                }
                catch
                {
                    Logger.CooldownTrackerLog("FU: FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return TimeSpan.MaxValue;
                }
            }
            else
            {
                Logger.CooldownTrackerLog("FU: FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return TimeSpan.MaxValue;
        }
    }
}
