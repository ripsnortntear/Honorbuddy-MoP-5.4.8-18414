#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-30 20:20:43 +0200 (Fr, 30 Aug 2013) $
 * $ID$
 * $Revision: 1705 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Helpers/CooldownTracker.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using PureRotation.Core;
using PureRotation.Managers;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Helpers
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

    public static class CooldownTracker
    {
        public delegate WoWUnit UnitSelectionDelegate(object context);

        public delegate WoWPoint LocationRetriever(object context);

        public delegate T Selection<out T>(object context);

        private static Dictionary<WoWSpell, DateTime> cooldowns = new Dictionary<WoWSpell, DateTime>();

        private static bool IsOnCooldown(int spell, int buffer = 2)
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
                            Logger.DebugLog("Checked cooldown for {0} at {1}", spell, DateTime.Now);
                            return result.Cooldown;
                        }

                        Logger.DebugLog("Found {0} but not ready to check at {1}", spell, DateTime.Now);
                        return true;
                    }
                }
                catch
                {
                    Logger.DebugLog("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return false;
                }
            }
            else
            {
                Logger.DebugLog("FindSpell {0} failed at {1}", spell, DateTime.Now);
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
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.DebugLog("Checked cooldown for {0} at {1}", spell, DateTime.Now);
                            return result.Cooldown;
                        }

                        Logger.DebugLog("Found {0} but not ready to check at {1}", spell, DateTime.Now);
                        return true;
                    }
                }
                catch
                {
                    Logger.DebugLog("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return false;
                }
            }
            else
            {
                Logger.DebugLog("FindSpell {0} failed at {1}", spell, DateTime.Now);
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
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.DebugLog("Checked CooldownTimeLeft for {0} at {1}", spell, DateTime.Now);
                            return result.CooldownTimeLeft;
                        }

                        Logger.DebugLog("Found {0} but not ready to check CooldownTimeLeft {1}", spell, DateTime.Now);
                        return TimeSpan.FromSeconds(10);
                    }
                }
                catch
                {
                    Logger.DebugLog("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return TimeSpan.MaxValue;
                }
            }
            else
            {
                Logger.DebugLog("FindSpell {0} failed at {1}", spell, DateTime.Now);
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
                    if (cooldowns.TryGetValue(result, out lastUsed))
                    {
                        var lastUsedminusBuffer = lastUsed - TimeSpan.FromSeconds(buffer);
                        if (DateTime.Compare(DateTime.Now, lastUsedminusBuffer) > 0)
                        {
                            Logger.DebugLog("Checked CooldownTimeLeft for {0} at {1}", spell, DateTime.Now);
                            return result.CooldownTimeLeft;
                        }

                        Logger.DebugLog("Found {0} but not ready to check CooldownTimeLeft {1}", spell, DateTime.Now);
                        return TimeSpan.FromSeconds(10);
                    }
                }
                catch
                {
                    Logger.DebugLog("FindSpell Found {0} but currently not in our cooldownlist, first usage?", spell);
                    return TimeSpan.MaxValue;
                }
            }
            else
            {
                Logger.DebugLog("FindSpell {0} failed at {1}", spell, DateTime.Now);
            }
            return TimeSpan.MaxValue;
        }

        public static void SpellUsed(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                cooldowns[result] = DateTime.Now.Add(result.CooldownTimeLeft);
            }
        }

        public static void SpellUsed(int spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                WoWSpell result = results.Override ?? results.Original;
                cooldowns[result] = DateTime.Now.Add(result.CooldownTimeLeft);
            }
        }

        #region LogSpellUsed

        private static Composite LogSpellUsed(int spell)
        {
            return new Sequence(

                // if spell was in progress before cast (we queued this one) then wait for the in progress one to finish
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => !(SpellManager.GlobalCooldown || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null),
                                new ActionAlwaysSucceed()
                                ),

                // wait for this cast to appear on the GCD or Spell Casting indicators
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => SpellManager.GlobalCooldown || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null,
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
                                ret => !(SpellManager.GlobalCooldown || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null),
                                new ActionAlwaysSucceed()
                                ),

                // wait for this cast to appear on the GCD or Spell Casting indicators
                            new WaitContinue(
                                new TimeSpan(0, 0, 0, 0, (int)StyxWoW.WoWClient.Latency << 1),
                                ret => SpellManager.GlobalCooldown || StyxWoW.Me.IsCasting || StyxWoW.Me.ChanneledSpell != null,
                                new ActionAlwaysSucceed()
                                ),

                // Log the spell used in cooldowns dictionary
                           new Action(ret => SpellUsed(spell)));
        }

        #endregion LogSpellUsed

        #region Double Cast Shit

        public static Composite PreventDoubleCast(string spell, double expiryTime, Selection<bool> reqs = null)
        {
            return PreventDoubleCast(spell, expiryTime, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite PreventDoubleCast(int spell, double expiryTime, Selection<bool> reqs = null)
        {
            return PreventDoubleCast(spell, expiryTime, target => StyxWoW.Me.CurrentTarget, reqs);
        }

        /// <summary>
        /// This Method can enable / disable the Moving Check from CanCast and should fix Pauls issues
        /// </summary>
        public static Composite PreventDoubleCast(string spell, double expiryTime, UnitSelectionDelegate onUnit, Selection<bool> reqs = null, bool ignoreMoving = false)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.PreventDoubleCast(spell, expiryTime, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null)), ignoreMoving),
                            LogSpellUsed(spell));
        }

        public static Composite PreventDoubleCast(int spell, double expiryTime, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.PreventDoubleCast(spell, expiryTime, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        public static Composite PreventDoubleCastNoCanCast(string spell, double expiryTime, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.PreventDoubleCastNoCanCast(spell, expiryTime, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        public static Composite PreventDoubleChannel(string spell, double expiryTime, bool checkCancast, Selection<bool> reqs = null)
        {
            return PreventDoubleChannel(spell, expiryTime, checkCancast, onUnit => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite PreventDoubleChannel(string spell, double expiryTime, bool checkCancast, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return PreventDoubleChannel(spell, expiryTime, checkCancast, onUnit, false, reqs);
        }

        /// <summary>
        /// Prevent Double Channel - Allows Clipping of channeling (i.e. for priest mind flay).
        /// </summary>
        /// <param name="spell">spell name</param>
        /// <param name="expiryTime">expiry time</param>
        /// <param name="checkCancast">check CanCast (false if you want to clip)</param>
        /// <param name="onUnit">unit</param>
        /// <param name="reqs">requirements</param>
        /// <param name="ignoreMoving">check moving</param>
        /// <returns></returns>
        public static Composite PreventDoubleChannel(string spell, double expiryTime, bool checkCancast, UnitSelectionDelegate onUnit, bool ignoreMoving, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.PreventDoubleChannel(spell, expiryTime, checkCancast, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null)), ignoreMoving),
                            LogSpellUsed(spell));
        }

        public static Composite PreventDoubleCastOnGround(string spell, double expiryTime, LocationRetriever onLocation)
        {
            return PreventDoubleCastOnGround(spell, expiryTime, onLocation, ret => true);
        }

        public static Composite PreventDoubleCastOnGround(string spell, double expiryTime, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return new Sequence(

                            // Call the Main cast method
                             Spell.PreventDoubleCastOnGround(spell, expiryTime, ret => onLocation(ret), requirements, waitForSpell),
                             LogSpellUsed(spell));
        }
        
        #endregion Double Cast Shit

        #region Cast - by ID

        public static Composite Cast(int spell, Selection<bool> reqs = null)
        {
            return Cast(spell, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite Cast(int spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.Cast(spell, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        public static Composite ForceCast(int spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.ForceCast(spell, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        #endregion Cast - by ID

        #region Cast - by string

        public static Composite Cast(string spell, Selection<bool> reqs = null)
        {
            return Cast(spell, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite Cast(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.Cast(spell, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        #endregion Cast - by string

        #region Cast - Multi DoT

        public static Composite MultiDoT(string spellName, WoWUnit unit, Selection<bool> reqs = null)
        {
            return MultiDoT(spellName, unit, 15, 1, reqs);
        }

        public static Composite MultiDoT(string spellName, WoWUnit unit, double radius, double refreshDurationRemaining, Selection<bool> reqs = null)
        {
            WoWUnit dotTarget = null;
            return
                new Decorator(ret => unit != null && ((reqs != null && reqs(ret)) || (reqs == null)),
                    new PrioritySelector(ctx => dotTarget = Unit.GetMultiDoTTarget(unit, spellName, radius, refreshDurationRemaining),
                        PreventDoubleCast(spellName, Spell.GetSpellCastTime(spellName) + 0.5, on => dotTarget, ret => dotTarget != null)
                ));
        }

        #endregion Cast - Multi DoT

        #region ChanneledSpell - by name

        public static Composite ChannelSpell(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.ChannelSpell(spell, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        #endregion ChanneledSpell - by name

        #region ChanneledSpell - by ID

        public static Composite ChannelSpell(int spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.ChannelSpell(spell, ret => onUnit(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                            LogSpellUsed(spell));
        }

        #endregion ChanneledSpell - by ID

        #region CastOnGround - placeable spell casting

        public static Composite CastOnUnitLocation(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return CastOnGround(spell, loc => onUnit(loc).Location, ret => (reqs == null || reqs(ret)));
        }

        public static Composite CastOnUnitLocation(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null, bool waitForSpell = false)
        {
            return CastOnGround(spell, loc => onUnit(loc).Location, ret => ((reqs != null && reqs(ret)) || (reqs == null)), waitForSpell);
        }

        public static Composite CastOnGround(string spell, LocationRetriever onLocation)
        {
            return CastOnGround(spell, onLocation, ret => true);
        }

        public static Composite CastOnGround(string spell, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.CastOnGround(spell, ret => onLocation(ret), requirements, waitForSpell),
                            LogSpellUsed(spell));
        }

        public static Composite CastOnGround(int spell, LocationRetriever onLocation)
        {
            return CastOnGround(spell, onLocation, ret => true);
        }

        public static Composite CastOnGround(int spell, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return new Sequence(

                // Call the Main cast method
                            Spell.CastOnGround(spell, ret => onLocation(ret), requirements, waitForSpell),
                            LogSpellUsed(spell));
        }

        public static Composite CastHunterTrap(string trapName, LocationRetriever onLocation, CanRunDecoratorDelegate reqs = null)
        {
            return new Sequence(

                // Call the Main cast method
                        Spell.CastHunterTrap(trapName, ret => onLocation(ret), ret => ((reqs != null && reqs(ret)) || (reqs == null))),
                        LogSpellUsed(trapName));
        }

        #endregion CastOnGround - placeable spell casting

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