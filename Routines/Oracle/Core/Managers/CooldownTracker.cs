#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using JetBrains.Annotations;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Shared.Logging;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Core.Managers
{
    // OK heres how it works. You cast the spell you want to track using
    //
    //  CooldownTracker.Cast("Outbreak")
    //
    // then to check if the outbreak is on cooldown
    //
    //  CooldownTracker.SpellOnCooldown("Outbreak")
    //
    // it is important to use the CooldownTrackers cast methods to track the cooldown of your spell.
    // GetSpellCooldown is the same (use the CooldownTracker.Cast("Outbreak") method) and then check it with
    //
    // CooldownTracker.GetSpellCooldown("Outbreak").TotalMilliseconds > 4
    //
    // you can also supply a buffer for the GetSpellCooldown which will begin to check honorbuddy for the Cooldowntimeleft at the set buffer time.
    // if the buffer was like this: CooldownTracker.GetSpellCooldown("Outbreak", 4) <-- 4 Seconds
    // it would query Honorbuddy for the cooldowntimeleft 4 seconds before the spell expired.
    // the default it 2 seconds. so if a spell has a cooldown of 8 seconds, for the first 6 seconds honorbuddy will not be checked
    // when there is 2 seconds remaining the honorbuddy cooldowntimeleft will be returned.
    // excample.
    // 00.00.08.000 <-- spell used
    // not checked
    // 00.00.02.000 <-- Honorbuddy Cooldowntimeleft is checked from here on.
    // 00.00.01.999
    // 00.00.01.750
    // 00.00.00.500
    // 00.00.00.000

    [UsedImplicitly]
    public static class CooldownTracker
    {
        public delegate T Selection<out T>(object context);

        private static readonly Dictionary<WoWSpell, DateTime> Cooldowns = new Dictionary<WoWSpell, DateTime>();

        private static bool IsOnCooldown(int spell, int buffer = 2)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (Cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            //Logger.Output("Checked cooldown for {0} at {1}", spell, DateTime.Now);
                            return result.Cooldown;
                        }

                        //Logger.Output("Found {0} but not ready to check at {1}", spell, DateTime.Now);
                        return true;
                    }
                }
                catch
                {
                    //Logger.Output("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return false;
                }
            }
            else
            {
                Logger.Output("FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return false;
        }

        private static bool IsOnCooldown(string spell, int buffer = 2)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (Cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            //Logger.Output("Checked cooldown for {0} at {1}", spell, DateTime.Now);
                            return result.Cooldown;
                        }

                        //Logger.Output("Found {0} but not ready to check at {1}", spell, DateTime.Now);
                        return true;
                    }
                }
                catch
                {
                    //Logger.Output("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return false;
                }
            }
            else
            {
                Logger.Output("FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return false;
        }

        private static TimeSpan CooldownTimeLeft(int spell, int buffer = 2)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (Cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            //Logger.Output("Checked CooldownTimeLeft for {0} at {1}", spell, DateTime.Now);
                            return result.CooldownTimeLeft;
                        }

                        //Logger.Output("Found {0} but not ready to check CooldownTimeLeft {1}", spell, DateTime.Now);
                        return TimeSpan.FromSeconds(10);
                    }
                }
                catch
                {
                    //Logger.Output("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return TimeSpan.MaxValue;
                }
            }
            else
            {
                Logger.Output("FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return TimeSpan.MaxValue;
        }

        private static TimeSpan CooldownTimeLeft(string spell, int buffer = 2)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                try
                {
                    DateTime lastUsed;
                    if (Cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            //Logger.Output("Checked CooldownTimeLeft for {0} at {1}", spell, DateTime.Now);
                            return result.CooldownTimeLeft;
                        }

                        //Logger.Output("Found {0} but not ready to check CooldownTimeLeft {1}", spell, DateTime.Now);
                        return TimeSpan.FromSeconds(10);
                    }
                }
                catch
                {
                    //Logger.Output("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return TimeSpan.MaxValue;
                }
            }
            else
            {
                Logger.Output("FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return TimeSpan.MaxValue;
        }

        private static void SpellUsed(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                Cooldowns[result] = DateTime.Now.Add(result.CooldownTimeLeft);
            }
        }

        private static void SpellUsed(int spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                Cooldowns[result] = DateTime.Now.Add(result.CooldownTimeLeft);
            }
        }

        #region LogSpellUsed

        private static Composite LogSpellUsed(int spell)
        {
            return new Sequence(
                // if spell was in progress before cast (we queued this one) then wait for the in progress one to finish
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => !(Spell.GlobalCooldown() || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null),
                                new ActionAlwaysSucceed()
                                ),
                // wait for this cast to appear on the GCD or Spell Casting indicators
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => Spell.GlobalCooldown() || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null,
                                new ActionAlwaysSucceed()
                                ),
                // Log the spell used in cooldowns dictionary
                           new Action(ret => SpellUsed(spell)));
        }

        private static Composite LogSpellUsed(string spell)
        {
            return new Sequence(
                // if spell was in progress before cast (we queued this one) then wait for the in progress one to finish
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => !(Spell.GlobalCooldown() || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null),
                                new ActionAlwaysSucceed()
                                ),
                // wait for this cast to appear on the GCD or Spell Casting indicators
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => Spell.GlobalCooldown() || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null,
                                new ActionAlwaysSucceed()
                                ),
                // Log the spell used in cooldowns dictionary
                           new Action(ret => SpellUsed(spell)));
        }

        #endregion LogSpellUsed

        #region Cast - by ID

        public static Composite Cast(int spell, Selection<WoWUnit> onUnit, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool clipChannel = false)
        {
            return new Sequence(
                // Call the Main cast method
                           Spell.Cast(spell, on => onUnit(on), ret => ((reqs != null && reqs(ret)) || (reqs == null)), expiryTime, checkDoubleCast, ignoreCanCast, clipChannel),
                           LogSpellUsed(spell));
        }

        #endregion Cast - by ID

        #region Cast - by string

        public static Composite Cast(String spell, Selection<WoWUnit> onUnit, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool clipChannel = false)
        {
            return new Sequence(
                // Call the Main cast method
                           Spell.Cast(spell, on => onUnit(on), ret => ((reqs != null && reqs(ret)) || (reqs == null)), expiryTime, checkDoubleCast, ignoreCanCast, clipChannel),
                           LogSpellUsed(spell));
        }

        #endregion Cast - by string

        #region CastOnGround - placeable spell casting

        public static Composite CastOnGround(string spell, Selection<WoWPoint> onLocation, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool waitForSpell = false)
        {
            return new Sequence(
                // Call the Main cast method
                           Spell.CastOnGround(spell, on => onLocation(on), ret => ((reqs != null && reqs(ret)) || (reqs == null)), expiryTime, checkDoubleCast, ignoreCanCast, waitForSpell),
                           LogSpellUsed(spell));
        }

        public static Composite CastOnGround(int spell, Selection<WoWPoint> onLocation, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool waitForSpell = false)
        {
            return new Sequence(
                // Call the Main cast method
                           Spell.CastOnGround(spell, on => onLocation(on), ret => ((reqs != null && reqs(ret)) || (reqs == null)), expiryTime, checkDoubleCast, ignoreCanCast, waitForSpell),
                           LogSpellUsed(spell));
        }

        #endregion CastOnGround - placeable spell casting

        #region Heal & HoT

        public static Composite Heal(string spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => (onUnit != null && onUnit(ret) != null && onUnit(ret).IsValid) && onUnit(ret).HealthPercent(onUnit(ret).GetHPCheckType()) <= HP, Cast(spell, onUnit, reqs));
        }

        public static Composite HoT(string spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => (onUnit != null && onUnit(ret) != null && onUnit(ret).IsValid) && !SpellAuras.HasMyAura(spell, onUnit(ret)) && onUnit(ret).HealthPercent(onUnit(ret).GetHPCheckType()) <= HP, Cast(spell, onUnit, reqs));
        }

        #endregion Heal & HoT

        #region Spells - methods to handle Spells such as cooldowns

        /// <summary>
        /// Get the CooldownTimeLeft on a spell, by default it will only return the true result at less than a second.
        /// </summary>
        /// <param name="spell">the spell to check for</param>
        /// <param name="buffer">the amount of time to left to begin checking HB for the cooldown</param>
        /// <returns>the time left or Maxtime</returns>
        public static TimeSpan GetSpellCooldown(string spell, int buffer = 1)
        {
            return CooldownTimeLeft(spell);
        }

        /// <summary>
        /// Get the CooldownTimeLeft on a spell, by default it will only return the true result at less than a second.
        /// </summary>
        /// <param name="spell">the spell to check for</param>
        /// <param name="buffer">the amount of time to left to begin checking HB for the cooldown</param>
        /// <returns>the time left or Maxtime</returns>
        public static TimeSpan GetSpellCooldown(int spell, int buffer = 1)
        {
            return CooldownTimeLeft(spell);
        }

        public static bool SpellOnCooldown(string spell)
        {
            return IsOnCooldown(spell);
        }

        public static bool SpellOnCooldown(int spell)
        {
            return IsOnCooldown(spell);
        }

        #endregion Spells - methods to handle Spells such as cooldowns
    }
}