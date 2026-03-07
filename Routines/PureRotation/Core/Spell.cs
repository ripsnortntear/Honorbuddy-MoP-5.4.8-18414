#region Revision info

/*
 * $Author: treek $
 * $Date: 2013-09-16 03:06:16 +0200 (Mo, 16 Sep 2013) $
 * $ID$
 * $Revision: 1728 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Core/Spell.cs $
 * $LastChangedBy: treek $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using PureRotation.Classes;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.Patchables;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.DBC;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Action = Styx.TreeSharp.Action;
using Lua = Styx.WoWInternals.Lua;

namespace PureRotation.Core
{
    //-- Dont Put Lua in here -- Put it in the Lua Class -- wulf.

    internal static class Spell
    {
        #region Delegates

        public delegate WoWUnit UnitSelectionDelegate(object context);

        internal static WoWPoint _loc;

        public delegate WoWPoint LocationRetriever(object context);

        #endregion Delegates

        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        public static string Lastspellcast;

        private static readonly uint ClientLagMs = StyxWoW.WoWClient.Latency;

        public static bool IsGlobalCooldown(bool allowLagTollerance = true)
        {
            var latency = allowLagTollerance ? ClientLagMs << 1 : 0;
            var gcdTimeLeft = SpellManager.GlobalCooldownLeft.TotalMilliseconds;
            using (StyxWoW.Memory.AcquireFrame())
                return gcdTimeLeft > latency;
        }

        public static HashSet<int> HeroismBuff = new HashSet<int>
            {
                32182, //Bloodlust
                2825, // Heroism
                80353, // Timewarp
                90355, // Ancient Hysteria
            };

        public static Composite CreateWaitForLagDuration()
        {
            return new WaitContinue(TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency << 1) + 150), ret => false, new ActionAlwaysSucceed());
        }

        #region Cancast and Range checks

        /// <summary>
        /// checked if the spell has an instant cast, the spell is one which can be cast while moving, or we have an aura active which allows moving without interrupting casting.
        /// does not check whether you are presently moving, only whether you could cast if you are moving
        /// </summary>
        private static bool AllowMovingWhileCasting(WoWSpell _spell)
        {
            // quick return for instant spells
            if (_spell.CastTime == 0 && !IsFunnel(_spell))
                return true;

            // assume we cant do that, but then check for class specific buffs which allow movement while casting
            bool allowMovingWhileCasting = false;
            if (Me.Class == WoWClass.Shaman)
                allowMovingWhileCasting = (_spell.Name == "Lightning Bolt" && TalentManager.HasGlyph("Unleashed Lightning"));
            else if (Me.Specialization == WoWSpec.MageFire)
                allowMovingWhileCasting = _spell.Name == "Scorch";
            else if (Me.Class == WoWClass.Hunter)
                allowMovingWhileCasting = _spell.Name == "Steady Shot" || (_spell.Name == "Aimed Shot" && TalentManager.HasGlyph("Aimed Shot")) || _spell.Name == "Cobra Shot";
            else if (Me.Class == WoWClass.Warlock)
                allowMovingWhileCasting = TalentManager.HasTalent(17);
            if (!allowMovingWhileCasting)
                allowMovingWhileCasting = HaveAllowMovingWhileCastingAura();

            return allowMovingWhileCasting;
        }

        public static float ActualMaxRange(this WoWSpell spell, WoWUnit unit)
        {
            if (spell.MaxRange == 0)
                return 0;

            // 0.1 margin for error
            return unit != null ? spell.MaxRange + unit.CombatReach + StyxWoW.Me.CombatReach - 0.1f : spell.MaxRange;
        }

        public static float ActualMaxRange(string name, WoWUnit unit)
        {
            SpellFindResults sfr;
            if (!SpellManager.FindSpell(name, out sfr))
                return 0f;

            WoWSpell spell = sfr.Override ?? sfr.Original;
            return spell.ActualMaxRange(unit);
        }

        public static float ActualMinRange(this WoWSpell spell, WoWUnit unit)
        {
            if (spell.MinRange == 0)
                return 0;

            // some code was using 1.66666675f instead of Me.CombatReach ?
            return unit != null ? spell.MinRange + unit.CombatReach + StyxWoW.Me.CombatReach + 0.1f : spell.MinRange;
        }

        /// <summary>
        /// CastHack following done because CanCast() wants spell as "Metamorphosis: Doom" while Cast() and aura name are "Doom"
        /// </summary>
        public static bool CanCastHack(string castName, WoWUnit unit)
        {
            SpellFindResults sfr;
            if (!SpellManager.FindSpell(castName, out sfr))
            {
                return false;
            }

            WoWSpell spell = sfr.Override ?? sfr.Original;

            // check range
            if (unit != null && !spell.IsSelfOnlySpell && !unit.IsMe)
            {
                if (spell.IsMeleeSpell && !unit.IsWithinMeleeRange)
                {
                    Logger.DebugLog("CanCastHack[{0}]: not in melee range", castName);
                    return false;
                }
                if (spell.HasRange)
                {
                    if (unit.Distance > spell.ActualMaxRange(unit))
                    {
                        Logger.DebugLog("CanCastHack[{0}]: out of range - further than {1:F1}", castName, spell.ActualMaxRange(unit));
                        return false;
                    }
                    if (unit.Distance < spell.ActualMinRange(unit))
                    {
                        Logger.DebugLog("CanCastHack[{0}]: out of range - closer than {1:F1}", castName, spell.ActualMinRange(unit));
                        return false;
                    }
                }

                if (!unit.InLineOfSpellSight)
                {
                    Logger.DebugLog("CanCastHack[{0}]: not in spell line of {1}", castName, unit.SafeName);
                    return false;
                }
            }

            if ((spell.CastTime != 0u || IsFunnel(spell)) && Me.IsMoving && !AllowMovingWhileCasting(spell))
            {
                Logger.DebugLog("CanCastHack[{0}]: cannot cast while moving", castName);
                return false;
            }

            if (Me.ChanneledCastingSpellId == 0)
            {
                uint num = StyxWoW.WoWClient.Latency * 2u;
                if (StyxWoW.Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds > num)
                {
                    Logger.DebugLog("CanCastHack[{0}]: current cast of [1] has {2:F0} ms left", castName, Me.CurrentCastId, Me.CurrentCastTimeLeft.TotalMilliseconds - num);
                    return false;
                }

                if (spell.CooldownTimeLeft.TotalMilliseconds > num)
                {
                    Logger.DebugLog("CanCastHack[{0}]: still on cooldown for {1:F0} ms", castName, spell.CooldownTimeLeft.TotalMilliseconds - num);
                    return false;
                }
            }

            if (Me.CurrentPower < spell.PowerCost)
            {
                Logger.DebugLog("CanCastHack[{0}]: insufficient power (needs {1:F0} but have {2:F0})", castName, spell.PowerCost, Me.CurrentPower);
                return false;
            }

            return true;
        }

        #endregion Cancast and Range checks

        #region Double Cast Shit

        // PRSettings.ThrottleTime is used throughout the rotationbases to enable a setable expiryTime for the methods below.

        private static readonly Dictionary<string, DoubleCastSpell> DoubleCastEntries = new Dictionary<string, DoubleCastSpell>();

        private static void UpdateDoubleCastEntries(string spellName, double expiryTime)
        {
            if (DoubleCastEntries.ContainsKey(spellName)) DoubleCastEntries[spellName] = new DoubleCastSpell(spellName, expiryTime, DateTime.UtcNow);
            if (!DoubleCastEntries.ContainsKey(spellName)) DoubleCastEntries.Add(spellName, new DoubleCastSpell(spellName, expiryTime, DateTime.UtcNow));
        }

        public static void OutputDoubleCastEntries()
        {
            foreach (var spell in DoubleCastEntries)
            {
                Logger.InfoLog(spell.Key + " time: " + spell.Value.DoubleCastCurrentTime);
            }
        }

        internal static void PulseDoubleCastEntries()
        {
            DoubleCastEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.DoubleCastCurrentTime).TotalSeconds >= t.DoubleCastExpiryTime);
        }

        public static Composite PreventDoubleCast(string spell, double expiryTime, Selection<bool> reqs = null)
        {
            return PreventDoubleCast(spell, expiryTime, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite PreventDoubleCast(int spell, double expiryTime, Selection<bool> reqs = null)
        {
            return PreventDoubleCast(spell, expiryTime, target => Me.CurrentTarget, reqs);
        }

        /// <summary> This Method can enable / disable the Moving Check from CanCast and should fix Pauls issues </summary>
        public static Composite PreventDoubleCast(string spell, double expiryTime, UnitSelectionDelegate onUnit, Selection<bool> reqs = null, bool ignoreMoving = false)
        {
            try{
            return
                new Decorator(
                    ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && onUnit != null
                        && onUnit(ret) != null
                        && SpellManager.CanCast(spell, onUnit(ret), true, !ignoreMoving)
                        && !DoubleCastEntries.ContainsKey(spell + onUnit(ret).GetHashCode()),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                        new Action(ret => Lastspellcast = spell),
                        new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode(), expiryTime))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception PreventDoubleCast by string {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        public static Composite PreventDoubleCast(int spell, double expiryTime, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try{
            return
                new Decorator(
                    ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && onUnit != null
                        && onUnit(ret) != null
                        && SpellManager.CanCast(spell, onUnit(ret))
                        && !DoubleCastEntries.ContainsKey(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode()),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                        new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode(), expiryTime))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception PreventDoubleCast by id {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        public static Composite PreventDoubleChannel(string spell, double expiryTime, bool checkCancast, Selection<bool> reqs = null)
        {
            return PreventDoubleChannel(spell, expiryTime, checkCancast, onUnit => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite PreventDoubleChannel(string spell, double expiryTime, bool checkCancast, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return PreventDoubleChannel(spell, expiryTime, checkCancast, onUnit, reqs, false);
        }

        public static Composite PreventDoubleCastNoCanCast(string spell, double expiryTime, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try{
            return
                new Decorator(
                    ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && onUnit != null
                        && onUnit(ret) != null
                        && SpellManager.CanCast(spell, onUnit(ret))
                        && !DoubleCastEntries.ContainsKey(spell + onUnit(ret).GetHashCode()),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                        new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode(), expiryTime))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception PreventDoubleCastNoCanCast {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        /// <summary> Prevent Double Channel - Allows Clipping of channeling (i.e. for priest mind flay).</summary>
        public static Composite PreventDoubleChannel(string spell, double expiryTime, bool checkCancast, UnitSelectionDelegate onUnit, Selection<bool> reqs, bool ignoreMoving)
        {
            try{
            return new Decorator(
                delegate(object a)
                {
                    if (CanClipChannel())
                    {
                        return false;
                    }

                    if (!reqs(a))
                    {
                        return false;
                    }

                    if (onUnit != null && DoubleCastEntries.ContainsKey(spell + onUnit(a).GetHashCode()))
                    {
                        return false;
                    }

                    if (onUnit != null && (checkCancast && !SpellManager.CanCast(spell, onUnit(a), true, !ignoreMoving)))
                    {
                        return false;
                    }

                    return true;
                },
                new Sequence(
                    new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                    new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                    new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode(), expiryTime))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception PreventDoubleChannel {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        public static Composite PreventDoubleCastOnGround(string spell, double expiryTime, LocationRetriever onLocation)
        {
            return PreventDoubleCastOnGround(spell, expiryTime, onLocation, ret => true);
        }

        private static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private static WoWPoint GetRealLocation(WoWPoint loc)
        {
            if (PRSettings.Instance.EnableExperimentalPlacement)
            {
                _loc = CalculateNewPoint(loc, PRSettings.Instance.ExperimentalPlacementDistance);
                Logger.DebugLog("Experimental Placement enabled! old Location {0}, new Location {1}", loc, _loc);
            }
            else
            {
                _loc = loc;
            }
            return _loc;
        }

        private static WoWPoint CalculateNewPoint(WoWPoint currentLocation, int distanceFromCurrentLocation = 0, int headingFromCurrentLocation = 0)
        {
            int defaultdistance = 8;
            Random random = new Random();
            double randomHeading = GetRandomNumber(0, headingFromCurrentLocation == 0 ? 360 : headingFromCurrentLocation);
            double randomDistance = GetRandomNumber(0, distanceFromCurrentLocation == 0 ? defaultdistance : distanceFromCurrentLocation);
            WoWPoint _t = currentLocation.RayCast((float)randomHeading, (float)randomDistance);
            return _t;
        }

        public static Composite PreventDoubleCastOnGround(string spell, double expiryTime, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            try{
            return new Decorator(
                    ret =>
                    onLocation != null && requirements(ret) && SpellManager.CanCast(spell) &&
                        //StyxWoW.Me.CurrentTarget != null && !BossList.IgnoreAoE.Contains(StyxWoW.Me.CurrentTarget.Entry)
                    (StyxWoW.Me.Location.Distance(onLocation(ret)) <= SpellManager.Spells[spell].MaxRange ||
                     SpellManager.Spells[spell].MaxRange == 0) && !DoubleCastEntries.ContainsKey(spell.ToString(CultureInfo.InvariantCulture) + onLocation(ret)
                    ),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell)),

                        new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1,
                                ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Name == spell, new ActionAlwaysSucceed())),
                        new Action(ret => { _loc = GetRealLocation(onLocation(ret)); SpellManager.ClickRemoteLocation(_loc); }),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} at {1:F1}", spell, _loc))),
                        new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + _loc, expiryTime))));
            }
            catch (Exception ex)
        {
            return new PrioritySelector(
                new Action(ret =>
                {
                    Logger.DebugLog("Exception PreventDoubleCastOnGround {0}", ex);

                    var hasOnLocation = (onLocation != null);
                    Styx.WoWPoint loc = default(Styx.WoWPoint);

                    if (hasOnLocation)
                        loc = onLocation(ret);

                    Logger.DebugLog(
                        "We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},location={3}",
                        spell,
                        hasOnLocation,
                        hasOnLocation,
                        hasOnLocation ? (object)loc : null);
                }));
               
	        	}
  	    }


        private struct DoubleCastSpell
        {
            public DoubleCastSpell(string spellName, double expiryTime, DateTime currentTime)
                : this()
            {
                DoubleCastSpellName = spellName;
                DoubleCastExpiryTime = expiryTime;
                DoubleCastCurrentTime = currentTime;
            }

            private string DoubleCastSpellName { get; set; }

            public double DoubleCastExpiryTime { get; set; }

            public DateTime DoubleCastCurrentTime { get; set; }
        }

        #endregion Double Cast Shit

        #region Cast - by name

        public static Composite Cast(string spell, Selection<bool> reqs = null)
        {
            return Cast(spell, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite Cast(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try
            {
                return
                    new Decorator(
                        ret => (onUnit != null && onUnit(ret) != null &&
                            ((reqs != null && reqs(ret)) || (reqs == null)) &&
                            SpellManager.CanCast(spell, onUnit(ret))),
                        new Sequence(
                            new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                            new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                            new Action(ret => Lastspellcast = spell))); // this is here so i can track stuff like pyroblast. --wulf
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                        {
                            Logger.DebugLog("Exception Cast by string {0}", ex);
                            Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                        }));
            }
        }

        public static Composite CastIgnoreMovement(string spell, Selection<bool> reqs = null)
        {
            return CastIgnoreMovement(spell, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite CastIgnoreMovement(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try
            {
            return
                new Decorator(
                    ret => (onUnit != null && onUnit(ret) != null &&
                        ((reqs != null && reqs(ret)) || (reqs == null)) &&
                        SpellManager.CanCast(spell, onUnit(ret), true, false)),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                        new Action(ret => Lastspellcast = spell))); // this is here so i can track stuff like pyroblast. --wulf
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                        {
                            Logger.DebugLog("Exception CastIgnoreMovement {0}", ex);
                            Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                        }));
            }
        }

        /// <summary>
        /// used for warlocks and shit like this Spell.CastHack("Metamorphosis: Doom", "Doom", on => Me.CurrentTarget, ret =>  NeedDoom));
        /// </summary>
        /// <param name="canCastName">1st is the spell name SpellManager expects</param>
        /// <param name="castName">2nd is the spell the game expects</param>
        /// <returns></returns>
        public static Composite CastHack(string canCastName, string castName, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try
            {
            return
                new Decorator(ret => castName != null && canCastName != null && ((reqs != null && reqs(ret)) || (reqs == null)) && onUnit != null && onUnit(ret) != null
                    && SpellManager.CanCast(canCastName, onUnit(ret), true, false),
                new Sequence(
                    new Action(ret => SpellManager.Cast(castName, onUnit(ret))),
                    new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", castName, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                    new Action(ret => Lastspellcast = castName)));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                        {
                            Logger.DebugLog("Exception CastHack {0}", ex);
                            Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", canCastName, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                        }));
            }
        }

        /// <summary>
        /// Face the target before casting spell
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="onUnit"></param>
        /// <param name="reqs"></param>
        /// <returns></returns>
        public static Composite FaceAndCast(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try{
            return
                new Decorator(
                    ret => (onUnit != null && onUnit(ret) != null && Me.CurrentTarget != null &&
                        ((reqs != null && reqs(ret)) || (reqs == null)) &&
                        SpellManager.CanCast(spell, onUnit(ret))),
                    new Sequence(
                // Face Target
                        new Action(ret => onUnit(ret).Face()),
                // Wait until we're facing.
                        new WaitContinue(TimeSpan.FromMilliseconds(500), ret => Me.IsSafelyFacing(onUnit(ret)), new ActionAlwaysSucceed()),
                // Cast Spell
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent))),
                        new Action(ret => Lastspellcast = spell)
                        ));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception FaceAndCast {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        #endregion Cast - by name

        #region Cast - by ID

        public static Composite Cast(int spell, Selection<bool> reqs = null)
        {
            return Cast(spell, ret => StyxWoW.Me.CurrentTarget, reqs);
        }

        public static Composite Cast(int spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try{
            return
                new Decorator(
                    ret => ((reqs != null && reqs(ret)) || (reqs == null)) && onUnit != null && onUnit(ret) != null && SpellManager.CanCast(spell, onUnit(ret)),
                     new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent)))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception Cast by Id {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        public static Composite ForceCast(int spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            try{
            return
                new Decorator(
                    ret => ((reqs != null && reqs(ret)) || (reqs == null)) && onUnit != null && onUnit(ret) != null && !WoWSpell.FromId(spell).Cooldown
                           && onUnit(ret).Distance < 40 && onUnit(ret).InLineOfSpellSight,
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent)))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception ForceCast {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, onUnit={1}, onUnit(ret)={2},name={3},health={4}", spell, onUnit != null, onUnit(ret) != null, onUnit(ret) != null ? onUnit(ret).SafeName : "<none>", onUnit(ret) != null ? onUnit(ret).HealthPercent : 0);
                    }));
            }
        }

        #endregion Cast - by ID

        #region Cast - by Specific Requirements and Functionality

        private static WoWUnit _unitInterrupt = null;

        /// <summary>Creates an interrupt spell cast composite. It will attempt to stun if possible!</summary>
        public static Composite InterruptSpellCasts(UnitSelectionDelegate onUnit)
        {
            if (!PRSettings.Instance.EnableInterupts) return new ActionAlwaysFail();
            Composite actionSelectTarget;

            actionSelectTarget = new Action(
                ret =>
                {
                    WoWUnit u = onUnit(ret);
                    _unitInterrupt = u != null && u.IsCasting && u.CanInterruptCurrentSpellCast ? u : null;
                    //if (_unitInterrupt != null) Logger.DebugLog("Possible Interrupt Target: {0} @ {1:F1} yds casting {2} #{3} for {4} ms", _unitInterrupt.SafeName, _unitInterrupt.Distance, _unitInterrupt.CastingSpell.Name, _unitInterrupt.CastingSpell.Id, _unitInterrupt.CurrentCastTimeLeft.TotalMilliseconds);
                    if (_unitInterrupt != null && _unitInterrupt.CastingSpell != null) Logger.DebugLog("Possible Interrupt Target: {0} @ {1:F1} yds casting {2} #{3} for {4} ms", _unitInterrupt.SafeName, _unitInterrupt.Distance, _unitInterrupt.CastingSpell.Name, _unitInterrupt.CastingSpell.Id, _unitInterrupt.CurrentCastTimeLeft.TotalMilliseconds);
                }
                );

            PrioritySelector prioSpell = new PrioritySelector();

            #region Pet Spells First!

            if (Me.Class == WoWClass.Warlock)
            {
                // this will be either a Optical Blast or Spell Lock
                prioSpell.AddChild(
                    Spell.Cast(
                        "Command Demon",
                        on => _unitInterrupt,
                        ret => _unitInterrupt != null
                            && _unitInterrupt.Distance < 40
                            && Me.Pet != null
                            && Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Felhunter
                        )
                    );
            }

            #endregion Pet Spells First!

            #region Melee Range

            if (Me.Class == WoWClass.Paladin)
                prioSpell.AddChild(Spell.Cast("Rebuke", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Rogue)
            {
                prioSpell.AddChild(Spell.Cast("Kick", ctx => _unitInterrupt));
                prioSpell.AddChild(Spell.Cast("Gouge", ctx => _unitInterrupt, ret => !_unitInterrupt.IsBoss() && Me.IsSafelyFacing(_unitInterrupt)));
            }

            if (Me.Class == WoWClass.Warrior)
                prioSpell.AddChild(Spell.Cast("Pummel", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Monk)
                prioSpell.AddChild(Spell.Cast("Spear Hand Strike", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Druid)
            {
                // Spell.Cast("Skull Bash (Cat)", ctx => _unitInterrupt, ret => StyxWoW.Me.Shapeshift == ShapeshiftForm.Cat));
                // Spell.Cast("Skull Bash (Bear)", ctx => _unitInterrupt, ret => StyxWoW.Me.Shapeshift == ShapeshiftForm.Bear));
                prioSpell.AddChild(Spell.Cast("Skull Bash", ctx => _unitInterrupt, ret => StyxWoW.Me.Shapeshift == ShapeshiftForm.Bear || StyxWoW.Me.Shapeshift == ShapeshiftForm.Cat));
                prioSpell.AddChild(Spell.Cast("Mighty Bash", ctx => _unitInterrupt, ret => !_unitInterrupt.IsBoss() && _unitInterrupt.IsWithinMeleeRange));
            }

            if (Me.Class == WoWClass.DeathKnight)
                prioSpell.AddChild(Spell.Cast("Mind Freeze", ctx => _unitInterrupt));

            if (Me.Race == WoWRace.Pandaren)
                prioSpell.AddChild(Spell.Cast("Quaking Palm", ctx => _unitInterrupt));

            #endregion Melee Range

            #region 8 Yard Range

            if (Me.Race == WoWRace.BloodElf)
                prioSpell.AddChild(Spell.Cast("Arcane Torrent", ctx => _unitInterrupt, req => _unitInterrupt.Distance < 8 && !Unit.AttackableUnits.Any(u => u.IsSensitiveDamage(8f))));

            if (Me.Race == WoWRace.Tauren)
                prioSpell.AddChild(Spell.Cast("War Stomp", ctx => _unitInterrupt, ret => _unitInterrupt.Distance < 8 && !_unitInterrupt.IsBoss() && !Unit.AttackableUnits.Any(u => u.IsSensitiveDamage(8f))));

            #endregion 8 Yard Range

            #region 10 Yards

            if (Me.Class == WoWClass.Paladin)
                prioSpell.AddChild(Spell.Cast("Hammer of Justice", ctx => _unitInterrupt));

            if (Me.Specialization == WoWSpec.DruidBalance)
                prioSpell.AddChild(Spell.Cast("Hammer of Justice", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Warrior)
                prioSpell.AddChild(Spell.Cast("Disrupting Shout", ctx => _unitInterrupt));

            #endregion 10 Yards

            #region 25 yards

            if (Me.Class == WoWClass.Shaman)
                prioSpell.AddChild(Spell.Cast("Wind Shear", ctx => _unitInterrupt, req => Me.IsSafelyFacing(_unitInterrupt)));

            #endregion 25 yards

            #region 30 yards

            if (Me.Specialization == WoWSpec.PaladinProtection)
                prioSpell.AddChild(Spell.Cast("Avenger's Shield", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Shaman)
                // Gag Order only works on non-bosses due to it being a silence, not an interrupt!
                prioSpell.AddChild(Spell.Cast("Heroic Throw", ctx => _unitInterrupt, ret => TalentManager.HasGlyph("Gag Order") && !_unitInterrupt.IsBoss()));

            if (Me.Class == WoWClass.Priest)
                prioSpell.AddChild(Spell.Cast("Silence", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.DeathKnight)
                prioSpell.AddChild(Spell.Cast("Strangulate", ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Mage)
                prioSpell.AddChild(Spell.Cast("Frostjaw", ctx => _unitInterrupt));

            #endregion 30 yards

            #region 40 yards

            if (Me.Class == WoWClass.Mage)
                prioSpell.AddChild(Spell.Cast("Counterspell", ctx => _unitInterrupt));

			if (Me.Specialization == WoWSpec.HunterMarksmanship)
				prioSpell.AddChild(Spell.Cast("Silencing Shot", ctx => _unitInterrupt));

            if (Me.Specialization == WoWSpec.HunterSurvival)
                prioSpell.AddChild(Spell.Cast(147362, ctx => _unitInterrupt));
				
			if (Me.Specialization == WoWSpec.HunterBeastMastery)
                prioSpell.AddChild(Spell.Cast(147362, ctx => _unitInterrupt));

            if (Me.Class == WoWClass.Druid)
                prioSpell.AddChild(Spell.Cast("Solar Beam", ctx => _unitInterrupt, ret => StyxWoW.Me.Shapeshift == ShapeshiftForm.Moonkin));

            if (Me.Specialization == WoWSpec.ShamanElemental || Me.Specialization == WoWSpec.ShamanEnhancement)
                prioSpell.AddChild(Spell.Cast("Solar Beam", ctx => _unitInterrupt, ret => true));

            #endregion 40 yards

            return new Sequence(
                actionSelectTarget,
                new Decorator(
                    ret => PRSettings.Instance.EnableInterupts && _unitInterrupt != null,
                // majority of these are off GCD, so throttle all to avoid most fail messages
                    new Throttle(TimeSpan.FromMilliseconds(500), prioSpell)
                    )
                );
        }

        private static bool IsShortDruidInterruptOnCooldown
        {
            get
            {
                if (Me.Class == WoWClass.Druid)
                {
                    if (StyxWoW.Me.Shapeshift == ShapeshiftForm.Cat) return SpellOnCooldown("Skull Bash (Cat)");
                    if (StyxWoW.Me.Shapeshift == ShapeshiftForm.Bear) return SpellOnCooldown("Skull Bash (Bear)");
                }
                return true;
            }
        }

        /// <summary> Casts a raid buff if the buff is not currently applied on the player or the raid</summary>
        public static Composite CastRaidBuff(string name, CanRunDecoratorDelegate cond)
        {
            return new Decorator(
                delegate(object a)
                {
                    if (!PRSettings.Instance.EnableRaidPartyBuffing)
                        return false;

                    if (!cond(a))
                        return false;

                    if (!SpellManager.CanCast(name, Me))
                    {
                        return false;
                    }

                    // If we are solo then return true if the name of the requested buff matchs the users UI setting and the player does not have the buff..
                    //if (!Unit.IsInGroup && !Me.IsDead && !Me.IsGhost && Me.IsAlive)
                    //{
                    //    if (name.Contains(PRSettings.Instance.Warrior.ShoutSelection.ToString()) && !PlayerHasBuff(name)) return true;
                    //    if (name.Contains(PRSettings.Instance.Monk.LegacySelection.ToString()) && !PlayerHasBuff(name)) return true;
                    //    if (name.Contains(PRSettings.Instance.Paladin.BlessingSelection.ToString()) && !PlayerHasBuff(name)) return true;
                    //}

                    // Continue on if we are in a raid group and check all raid members for the buffs we can provide and cast them if ok.
                    var players = new List<WoWPlayer> { Me };
                    if (Me.GroupInfo.IsInRaid)
                    {
                        players.Remove(Me);
                        players.AddRange(Me.RaidMembers);
                    }
                    else if (Me.GroupInfo.IsInParty)
                    {
                        players.AddRange(Me.PartyMembers);
                    }

                    var ProvidablePlayerBuffs = new HashSet<int>();
                    switch (StyxWoW.Me.Class)
                    {
                        case WoWClass.Warrior:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Stamina);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.AttackPower);

                            break;

                        case WoWClass.Paladin:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Stats);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Mastery);

                            break;

                        case WoWClass.Hunter:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.AttackPower);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.CriticalStrike);

                            break;

                        case WoWClass.Rogue:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.AttackSpeed);

                            break;

                        case WoWClass.Priest:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Stamina);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.SpellHaste);

                            break;

                        case WoWClass.DeathKnight:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.AttackPower);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.AttackSpeed);

                            break;

                        case WoWClass.Shaman:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.AttackSpeed);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.SpellPower);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.SpellHaste);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Mastery);

                            break;

                        case WoWClass.Mage:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.SpellPower);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.CriticalStrike);

                            break;

                        case WoWClass.Warlock:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Stamina);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.SpellPower);

                            break;

                        case WoWClass.Druid:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Stats);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.CriticalStrike);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.SpellHaste);
                            break;

                        case WoWClass.Monk:
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Stats);
                            ProvidablePlayerBuffs.UnionWith(SpellLists.Mastery);
                            break;
                    }

                    return players.Any(x => x.Distance2DSqr < 40 * 40 && !x.HasAnyAura(ProvidablePlayerBuffs) && !x.IsDead && !x.IsGhost && x.IsAlive);
                },
                new Sequence(
                    new Action(a => SpellManager.Cast(name))));
        }

        #endregion Cast - by Specific Requirements and Functionality

        #region Cast - Multi DoT

        /// <summary> Multi-DoT targets within range of target</summary>
        public static Composite MultiDoT(string spellName, WoWUnit unit, Selection<bool> reqs = null)
        {
            return MultiDoT(spellName, unit, 15, 1, reqs);
        }

        /// <summary> Multi-DoT targets within range of target</summary>
        public static Composite MultiDoT(string spellName, WoWUnit unit, double radius, double refreshDurationRemaining, Selection<bool> reqs = null)
        {
            WoWUnit dotTarget = null;
            try{
            return new PrioritySelector(
                        CachedUnits.Pulse,
                        new Decorator(ret => unit != null && ((reqs != null && reqs(ret)) || reqs == null),
                              new PrioritySelector(ctx => dotTarget = Unit.GetMultiDoTTarget(unit, spellName, radius, refreshDurationRemaining),
                                  PreventDoubleCast(spellName, GetSpellCastTime(spellName) + 0.5, on => dotTarget, ret => dotTarget != null))));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception MultiDot {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, dotTarget={1},name={3},health={4}", spellName, dotTarget != null, dotTarget != null ? dotTarget.SafeName : "<none>", dotTarget != null ? dotTarget.HealthPercent : 0);
                    }));
            }
        }

        public static int mobcount { get; set; }
   

        /// <summary> Multi-DoT targets within range of target</summary>
        public static Composite PreventDoubleMultiDoT(string spellName, double expiryTime, WoWUnit unit, double radius, double refreshDurationRemaining, Selection<bool> reqs = null)
        {
            WoWUnit dotTarget = null;

            try{
            return new PrioritySelector(
                        CachedUnits.Pulse,
                        new Decorator(ret => unit != null && ((reqs != null && reqs(ret)) || reqs == null),
                              new Sequence(ctx => dotTarget = Unit.GetMultiDoTTarget(unit, spellName, radius, refreshDurationRemaining),
                                
                                  PreventDoubleCast(spellName, expiryTime, on => dotTarget, ret => dotTarget != null),
                                    new Action(delegate
                                       {
                                           //Logger.InfoLog("Mobcount: {0} ", mobcount);
                                           mobcount = mobcount +1;
                                           if (mobcount > 4)
                                           {
                                               mobcount = 0;
                                                return RunStatus.Success;
                                           }

                                            return RunStatus.Failure;
                                            })
                                   
                                  )));
            }
            catch (Exception ex)
            {

                return new PrioritySelector(
                    new Action(ret =>
                    {
                        Logger.DebugLog("Exception MultiDot {0}", ex);
                        Logger.DebugLog("We tried to cast spell={0}, dotTarget={1},name={3},health={4}", spellName, dotTarget != null, dotTarget != null ? dotTarget.SafeName : "<none>", dotTarget != null ? dotTarget.HealthPercent : 0);
                    }));
            }
        }

        /// <summary>
        /// Multi-DoT targets within range of unit - allow different "can cast name" to "cast name"
        /// </summary>
        /// <param name="canCastName">The spell value HB expects</param>
        /// <param name="spellName">The spell value the game expects</param>
        /// <param name="unit">DoT units around which unit?</param>
        /// <param name="radius">Radius</param>
        /// <param name="refreshDurationRemaining">Duration remaining to refresh dot</param>
        /// <param name="reqs">requirements</param>
        /// <returns></returns>
        public static Composite MultiDoTCastHack(string canCastName, string spellName, WoWUnit unit, double radius, double refreshDurationRemaining, Selection<bool> reqs = null)
        {
            WoWUnit dotTarget = null;
            return new PrioritySelector(
                        CachedUnits.Pulse,
                        new Decorator(ret => unit != null && ((reqs != null && reqs(ret)) || reqs == null),
                              new PrioritySelector(ctx => dotTarget = Unit.GetMultiDoTTarget(unit, spellName, radius, refreshDurationRemaining),
                                  CastHack(canCastName, spellName, on => dotTarget, ret => dotTarget != null))));
        }

        #endregion Cast - Multi DoT

        #region Cast With Lua

        public static string RealLuaEscape(string luastring)
        {
            //luastring = Lua.LocalizeSpellName(luastring);
            var bytes = Encoding.UTF8.GetBytes(luastring);
            return bytes.Aggregate(String.Empty, (current, b) => current + ("\\" + b));
        }

        public static Composite CastWithLua(string spellName, CanRunDecoratorDelegate cond)
        {
            return new Decorator(
                       cond,

                //new PrioritySelector(
                       new Sequence(
                           new Action(a => Lua.DoString("RunMacroText(\"" + RealLuaEscape("/cast " + spellName) + "\")"))

                //new Action(a => Logger.DebugLog("Running Macro Text: {0}", macro))
                               )
                           );
        }

        public static Composite PreventDoubleCastWithLua(string spellName, double expiryTime, CanRunDecoratorDelegate reqs)
        {
            return new Decorator(ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && !DoubleCastEntries.ContainsKey(spellName.ToString(CultureInfo.InvariantCulture) + Me.GetHashCode()),
                       new Sequence(
                           new Action(a => Lua.DoString("RunMacroText(\"" + RealLuaEscape("/cast " + spellName) + "\")")),
                           new Action(a => UpdateDoubleCastEntries(spellName.ToString(CultureInfo.InvariantCulture) + Me.GetHashCode(), expiryTime))
                           )
                           );
        }

        /// <summary> Designed for the warlock green fire quest, allows using spells on a fixed cooldown</summary>
        public static Composite PreventDoubleCastWithLua(string spellName, double expiryTime, UnitSelectionDelegate onUnit, UnitSelectionDelegate afterUnit, CanRunDecoratorDelegate reqs)
        {
            return new Decorator(ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && onUnit != null
                        && onUnit(ret) != null
                        && afterUnit != null
                        && afterUnit(ret) != null
                        && !DoubleCastEntries.ContainsKey(spellName.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode()),
                       new Sequence(
                           new Action(ret => onUnit(ret).Target()),
                           new WaitContinue(1, ret => Me.CurrentTarget == onUnit(ret), new ActionAlwaysSucceed()),
                           new Action(a => Lua.DoString("RunMacroText(\"" + RealLuaEscape("/cast " + spellName) + "\")")),
                           new Action(a => UpdateDoubleCastEntries(spellName.ToString(CultureInfo.InvariantCulture) + onUnit(a).GetHashCode(), expiryTime)),
                           new Action(ret => afterUnit(ret).Target()),
                           new WaitContinue(1, ret => Me.CurrentTarget == afterUnit(ret), new ActionAlwaysSucceed())

                           )
                           );
        }

        /// <summary> Created to run pet abilities on a timed interval</summary>
        public static Composite PreventDoubleRunMacro(string macroText, double expiryTime, CanRunDecoratorDelegate reqs)
        {
            return new Decorator(ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && !DoubleCastEntries.ContainsKey(macroText.ToString(CultureInfo.InvariantCulture) + Me.GetHashCode()),
                       new Sequence(
                        new Action(a => Lua.DoString("RunMacroText(\"" + RealLuaEscape(macroText) + "\")")),
                            new WaitContinue(2, ret => false, new ActionAlwaysSucceed()),
                            new Action(a => UpdateDoubleCastEntries(macroText.ToString(CultureInfo.InvariantCulture) + Me.GetHashCode(), expiryTime))
                           )
                           );
        }

        /// <summary> Created to run pet abilities on a timed interval, on a specific unit</summary>
        public static Composite PreventDoubleRunMacro(string macroText, double expiryTime, UnitSelectionDelegate onUnit, UnitSelectionDelegate afterUnit, CanRunDecoratorDelegate reqs)
        {
            return new Decorator(ret =>
                        ((reqs != null && reqs(ret)) || (reqs == null))
                        && onUnit != null
                        && onUnit(ret) != null
                        && afterUnit != null
                        && afterUnit(ret) != null
                        && !DoubleCastEntries.ContainsKey(macroText.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode()),
                       new Sequence(
                           new Action(ret => onUnit(ret).Target()),
                           new WaitContinue(1, ret => Me.CurrentTarget == onUnit(ret), new ActionAlwaysSucceed()),
                           new Action(a => Lua.DoString("RunMacroText(\"" + RealLuaEscape(macroText) + "\")")),
                           new Action(a => UpdateDoubleCastEntries(macroText.ToString(CultureInfo.InvariantCulture) + onUnit(a).GetHashCode(), expiryTime)),
                           new Action(ret => afterUnit(ret).Target()),
                           new WaitContinue(1, ret => Me.CurrentTarget == afterUnit(ret), new ActionAlwaysSucceed())

                           )
                           );
        }

        #endregion Cast With Lua

        #region Cast - Pet Spells

        /// <summary> From Singular: Checks if we can cast a pet spell</summary>
        private static bool CanCastPetAction(string action)
        {
            WoWPetSpell petAction = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (petAction == null || petAction.Spell == null)
            {
                Logger.DebugLog("Can't Cast Pet Cast: {0}", action);
                return false;
            }

            Logger.DebugLog("Can Cast Pet Cast: {0}", action);
            return !petAction.Spell.Cooldown;
        }

        private static void CastPetAction(string spellName)
        {
            WoWPetSpell spell = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == spellName);
            if (spell == null)
                return;

            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);

            Logger.DebugLog("Pet Cast: {0}", spellName);
        }

        public static Composite CastPetAction(string spellName, CanRunDecoratorDelegate conditions)
        {
            return new Decorator(
                ret => conditions(ret),
                new Sequence(
                    new Action(ret => Logger.DebugLog("Attempting to cast pet action: {0}", spellName)),
                    new Action(ret => CastPetAction(spellName))
                    ));
        }

        public static Composite CastPetActionOnLocation(string spellName, LocationRetriever location, CanRunDecoratorDelegate conditions)
        {
            return new Decorator(
                ret => conditions(ret) && CanCastPetAction(spellName),
                new Sequence(
                    new Action(ret => CastPetAction(spellName)),
                    new WaitContinue(TimeSpan.FromMilliseconds(250), ret => false, new ActionAlwaysSucceed()),
                    new Action(ret => { _loc = GetRealLocation(location(ret)); SpellManager.ClickRemoteLocation(_loc); }),
                    new Action(ret => CooldownTracker.SpellUsed(spellName)),
                    new Action(ret => Logger.DebugLog("Pet Cast at Location: {0}", spellName))
                    ));
        }

        public static Composite PreventDoubleCastPetActionOnLocation(string spellName, double expiryTime, LocationRetriever location, CanRunDecoratorDelegate conditions)
        {
            return new Decorator(ret =>
                        ((conditions != null && conditions(ret)) || (conditions == null))
                        && CanCastPetAction(spellName)
                        && !DoubleCastEntries.ContainsKey(spellName.ToString(CultureInfo.InvariantCulture) + Me.GetHashCode()),
                       new Sequence(
                        new Action(a => UpdateDoubleCastEntries(spellName.ToString(CultureInfo.InvariantCulture) + Me.GetHashCode(), expiryTime)),
                        new Action(ret => CastPetAction(spellName)),
                        new WaitContinue(TimeSpan.FromMilliseconds(250), ret => false, new ActionAlwaysSucceed()),
                        new Action(ret => { _loc = GetRealLocation(location(ret)); SpellManager.ClickRemoteLocation(_loc); }),
                        new Action(ret => CooldownTracker.SpellUsed(spellName)),
                        new Action(ret => Logger.DebugLog("Pet Cast at Location: {0}", spellName))
                           )
                           );
        }

        #endregion Cast - Pet Spells

        #region Channeling bools

        public static bool IsChanneling { get { return StyxWoW.Me.ChanneledCastingSpellId != 0 && StyxWoW.Me.IsChanneling; } }

        public static bool CanClipChannel()
        {
            if (!StyxWoW.Me.IsChanneling)
            {
                // Allow Cast - Not channeling
                return false;
            }

            if (StyxWoW.Me.CurrentChannelTimeLeft.TotalMilliseconds < (ClientLagMs << 1) + 150)
            {
                // Allow Cast - Clip the lag duration
                return false;
            }

            // Prevent Cast - Channeling spell
            return true;
        }

        public static bool IsCastingOrChannelling()
        {
            return CanClipChannel();
        }

        #endregion Channeling bools

        #region ChanneledSpell - by name

        public static Composite ChannelSpell(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return
                new Decorator(
                    ret => ((reqs != null && reqs(ret)) || (reqs == null)) && onUnit != null && onUnit(ret) != null && SpellManager.CanCast(spell, onUnit(ret)) && !IsChanneling,
                    new PrioritySelector(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", spell, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent)))));
        }

        #endregion ChanneledSpell - by name

        #region ChanneledSpell - by ID

        public static Composite ChannelSpell(int spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return
                new Decorator(
                    ret => ((reqs != null && reqs(ret)) || (reqs == null)) && onUnit != null && onUnit(ret) != null && SpellManager.CanCast(spell, onUnit(ret)) && !IsChanneling,
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Logger.InfoLog(String.Format("*{0} on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, onUnit(ret).SafeName, onUnit(ret).Distance, onUnit(ret).HealthPercent)))));
        }

        #endregion ChanneledSpell - by ID

        #region CastOnGround - placeable spell casting

        private static WoWSpell _spell;

        public static bool IsFunnel(string name)
        {
            SpellFindResults sfr;
            SpellManager.FindSpell(name, out sfr);
            WoWSpell spell = sfr.Override ?? sfr.Original;
            if (spell == null)
                return false;
            return IsFunnel(spell);
        }

        public static bool IsFunnel(WoWSpell spell)
        {
#if IS_FUNNEL_IS_FIXED
            return spell.IsFunnel;
#elif LUA_CALL_ACTUALLY_WORKED
            bool channeled = Styx.WoWInternals.Lua.GetReturnVal<bool>( "return GetSpellInfo("+ spell.Id.ToString() + ")", 4 );
            return channeled;
#else   // HV has the answers... ty m8
            bool IsChanneled = false;
            var row = StyxWoW.Db[ClientDb.Spell].GetRow((uint)spell.Id);
            if (row.IsValid)
            {
                var spellMiscIdx = row.GetField<uint>(24);
                row = StyxWoW.Db[ClientDb.SpellMisc].GetRow(spellMiscIdx);
                var flags = row.GetField<uint>(4);
                IsChanneled = (flags & 68) != 0;
            }

            return IsChanneled;
#endif
        }

        /// <summary> check for aura which allows moving without interrupting spell casting</summary>
        public static bool HaveAllowMovingWhileCastingAura()
        {
            try
            {
                //throws exception when aura not present!
                return Me.GetAllAuras().Any(a => a.ApplyAuraType == (WoWApplyAuraType)330 && _spell.CastTime < (uint)a.TimeLeft.TotalMilliseconds);
            }
            catch
            {
                return false;
            }
        }

        public static int PendingSpell()
        {
            var pendingSpellPtr = StyxWoW.Memory.Read<IntPtr>((IntPtr)0xC124C4, true);
            if (pendingSpellPtr != IntPtr.Zero)
            {
                var pendingSpellId = StyxWoW.Memory.Read<int>(pendingSpellPtr + 32);
                return pendingSpellId;
            }

            return 0;
        }

        private static bool LocationInRange(string spellName, WoWPoint loc)
        {
            SpellFindResults sfr;
            if (SpellManager.FindSpell(spellName, out sfr))
            {
                WoWSpell spell = sfr.Override ?? sfr.Original;
                if (spell.HasRange)
                {
                    return spell.MinRange <= Me.Location.Distance(loc) && Me.Location.Distance(loc) < spell.MaxRange;
                }
            }

            return false;
        }

        public static Composite CastOnUnitLocation(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return CastOnGround(spell, loc => onUnit(loc).Location, ret => ((reqs != null && reqs(ret)) || (reqs == null)));
        }

        public static Composite CastOnUnitLocation(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null, bool waitForSpell = false)
        {
            return CastOnGround(spell, loc => onUnit(loc).Location, ret => ((reqs != null && reqs(ret)) || (reqs == null)), waitForSpell);
        }

        public static Composite CastOnGround(string spell, LocationRetriever onLocation)
        {
            return CastOnGround(spell, onLocation, ret => true);
        }

        /// <summary>Creates a behavior to cast a spell by name, on the ground at the specified location. Returns RunStatus.Success if successful, RunStatus.Failure otherwise.</summary>
        public static Composite CastOnGround(string spell, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return
                new Decorator(
                    ret => requirements(ret)
                        && onLocation != null
                        && SpellManager.CanCast(spell) //CanCastHack(spell, null)
                        && LocationInRange(spell, onLocation(ret))
                        && GameWorld.IsInLineOfSpellSight(StyxWoW.Me.GetTraceLinePos(), onLocation(ret)),
                    new Sequence(
                        new Action(ret => Logger.InfoLog(String.Format("Casting {0} at location {1} at {2:F1} yds", spell, onLocation(ret), onLocation(ret).Distance(StyxWoW.Me.Location)))),

                        new Action(ret => { return SpellManager.Cast(spell) ? RunStatus.Success : RunStatus.Failure; }),

                        new DecoratorContinue(
                            ctx => waitForSpell,
                            new PrioritySelector(
                                new WaitContinue(1,
                                    ret => StyxWoW.Me.HasPendingSpell(spell), // && StyxWoW.Me.CurrentPendingCursorSpell.Name == spell,
                                    new ActionAlwaysSucceed()
                                    ),
                                new Action(r =>
                                {
                                    Logger.DebugLog("error: spell {0} not seen as pending on cursor after 1 second", spell);
                                    return RunStatus.Failure;
                                })
                                )
                            ),
                        new Action(ret => { _loc = GetRealLocation(onLocation(ret)); SpellManager.ClickRemoteLocation(_loc); }),
                // check for we are done status
                        new PrioritySelector(

                // done if cursor doesn't have spell anymore
                            new Decorator(
                                ret => !waitForSpell,
                                new Action(r => Lua.DoString("SpellStopTargeting()"))   //just in case
                                ),

                            new Wait(TimeSpan.FromMilliseconds(750),
                                ret => !StyxWoW.Me.HasPendingSpell(spell) || Me.IsCasting || Me.IsChanneling,
                                new ActionAlwaysSucceed()
                                ),

                            // otherwise cancel
                            new Action(ret =>
                            {
                                Logger.DebugLog("/cancel {0} - click {1} failed -OR- Pending Cursor Spell API is still broken [{5}={6}] -- distance={2:F1} yds, loss={3}, face={4}",
                                    spell,
                                    onLocation(ret),
                                    StyxWoW.Me.Location.Distance(onLocation(ret)),
                                    GameWorld.IsInLineOfSpellSight(StyxWoW.Me.GetTraceLinePos(), onLocation(ret)),
                                    StyxWoW.Me.IsSafelyFacing(onLocation(ret)),
                                    Me.HasPendingSpell(spell),
                                    PendingSpell()
                                    );

                                // Pending Spell Cursor API is broken... seems like we can't really check at this point, so assume it failed and worked... uggghhh
                                Lua.DoString("SpellStopTargeting()");
                                return RunStatus.Failure;
                            })
                            )
                        )
                    );
        }

        public static Composite CastOnGround(int spell, LocationRetriever onLocation)
        {
            return CastOnGround(spell, onLocation, ret => true);
        }

        public static Composite CastOnGround(int spell, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return
                new Decorator(
                    ret => onLocation != null && requirements(ret) && !SpellOnCooldown(spell),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell)),
                        new Action(ret => Logger.DebugLog(String.Format("Casting {0} at location {1} at {2:F1} yds", WoWSpell.FromId(spell).Name, onLocation(ret), onLocation(ret).Distance(StyxWoW.Me.Location)))),
                        new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1,
                                ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Id == spell, new ActionAlwaysSucceed())),
                        new Action(ret => { _loc = GetRealLocation(onLocation(ret)); SpellManager.ClickRemoteLocation(_loc); })));
        }

        public static Composite CastHunterTrap(string trapName, LocationRetriever onLocation, CanRunDecoratorDelegate req = null)
        {
            const bool useLauncher = true;
            return new PrioritySelector(
                new Decorator(
                    ret => onLocation != null
                        && (req == null || req(ret))
                        && StyxWoW.Me.Location.Distance(onLocation(ret)) < (40 * 40)
                        && SpellManager.HasSpell(trapName) && GetSpellCooldown(trapName) == TimeSpan.Zero,
                    new Sequence(
                        new Action(ret => Logger.DebugLog("Trap: use trap launcher requested: {0}", useLauncher)),
                        new PrioritySelector(
                            new Decorator(ret => useLauncher && Me.HasAura("Trap Launcher"), new ActionAlwaysSucceed()),
                            Cast("Trap Launcher", ret => useLauncher && !Me.HasAura("Trap Launcher")),
                            new Decorator(ret => !useLauncher, new Action(ret => Me.CancelAura("Trap Launcher")))
                            ),
                        new Wait(TimeSpan.FromMilliseconds(500),
                            ret => (useLauncher && Me.HasAura("Trap Launcher")),
                            new ActionAlwaysSucceed()),
                        new Action(ret => Logger.DebugLog("Trap: launcher aura present = {0}", Me.HasAura("Trap Launcher"))),
                        new Action(ret => Logger.DebugLog("^{0} trap: {1}", useLauncher ? "Launch" : "Set", trapName)),
                        new Action(ret => SpellManager.Cast(trapName)),
                        new Action(ret => { _loc = GetRealLocation(onLocation(ret)); SpellManager.ClickRemoteLocation(_loc); }),
                        new Action(ret => Logger.DebugLog("Trap: Complete!"))
                        )
                    )
                );
        }

        #endregion CastOnGround - placeable spell casting

        #region Spells - methods to handle Spells such as cooldowns

        // Not cached - Use CooldownTracker for performance.

        public static TimeSpan GetSpellCooldown(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft;
            }

            return TimeSpan.MaxValue;
        }

        public static TimeSpan GetSpellCooldown(int spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft;
            }

            return TimeSpan.MaxValue;
        }

        public static bool SpellOnCooldown(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            }

            return false;
        }

        public static bool SpellOnCooldown(int spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            }

            return false;
        }

        public static double GetSpellCastTime(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.CastTime / 1000.0 : results.Original.CastTime / 1000.0;
            }

            return 99999.9;
        }

        public static double GetSpellCastTime(int spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.CastTime / 1000.0 : results.Original.CastTime / 1000.0;
            }
            return 99999.9;
        }

        #endregion Spells - methods to handle Spells such as cooldowns

        #region Auras - methods to handle Auras/Buffs/Debuffs

        /// <summary>
        /// Gets an Aura by Name. Note: this is a fix for the HB API wraper GetAuraByName
        /// </summary>
        public static WoWAura GetAuraFromName(this WoWUnit unit, string aura, bool isMyAura = false)
        {
            return isMyAura ? unit.GetAllAuras().FirstOrDefault(a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.GetAllAuras().FirstOrDefault(a => a.Name == aura && a.TimeLeft > TimeSpan.Zero);
        }

        /// <summary>
        /// Gets an Aura by ID. Note: this is a fix for the HB API wraper GetAuraById
        /// </summary>
        public static WoWAura GetAuraFromID(this WoWUnit unit, int aura, bool isMyAura = false)
        {
            return isMyAura ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == aura && a.TimeLeft > TimeSpan.Zero);
        }

        public static uint StackCount(WoWUnit unit, string aura)
        {
            {
                if (unit != null)
                {
                    WoWAura result = unit.GetAllAuras().FirstOrDefault(a => a.Name == aura && a.StackCount > 0);
                    if (result != null) return result.StackCount;
                }
                return 0;
            }
        }

        public static uint GetAuraStackCount(string aura)
        {
            var result = StyxWoW.Me.GetAuraFromName(aura);

            if (result != null)
            {
                if (result.StackCount > 0)
                    return result.StackCount;
            }

            return 0;
        }

        public static uint GetAuraStackCount(int spellId)
        {
            var result = StyxWoW.Me.GetAuraFromID(spellId);

            if (result != null)
            {
                if (result.StackCount > 0)
                    return result.StackCount;
            }

            return 0;
        }

        public static double GetAuraTimeLeft(string aura)
        {
            return GetAuraTimeLeft(aura, StyxWoW.Me);
        }

        public static double GetAuraTimeLeft(string aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var result = onUnit.GetAuraFromName(aura);
                if (result != null)
                {
                    if (result.TimeLeft.TotalSeconds > 0)
                        return result.TimeLeft.TotalSeconds;
                }
            }
            return 0;
        }

        public static double GetAuraTimeLeftMilli(int aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var result = onUnit.GetAuraFromID(aura);
                if (result != null)
                {
                 //   if (result.TimeLeft.TotalMilliseconds > 0)
                        return result.TimeLeft.TotalMilliseconds;
                }
            }
            return 0;
        }

        public static double GetAuraTimeLeftMilli(int aura)
        {
            return GetAuraTimeLeftMilli(aura, StyxWoW.Me);
        }



        public static double GetAuraTimeLeft(int aura)
        {
            return GetAuraTimeLeft(aura, StyxWoW.Me);
        }

        public static double GetAuraTimeLeft(int aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var result = onUnit.GetAuraFromID(aura);
                if (result != null)
                {
                    if (result.TimeLeft.TotalSeconds > 0)
                        return result.TimeLeft.TotalSeconds;
                }
            }
            return 0;
        }

        public static double GetMyAuraTimeLeft(string aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var result = onUnit.GetAuraFromName(aura, true);
                if (result != null && result.TimeLeft.TotalSeconds > 0)
                    return result.TimeLeft.TotalSeconds;
            }
            return 0;
        }

        public static double GetMyAuraTimeLeft(int aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var result = onUnit.GetAuraFromID(aura, true);
                if (result != null && result.TimeLeft.TotalSeconds > 0)
                    return result.TimeLeft.TotalSeconds;
            }
            return 0;
        }

        public static double GetMyAuraTimeLeft(string[] aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var auras = onUnit.GetAllAuras();
                var hashes = new HashSet<string>(aura);
                var result = auras.FirstOrDefault(a => hashes.Contains(a.Name) && a.CreatorGuid == Me.Guid);
                if (result != null && result.TimeLeft.TotalSeconds > 0)
                    return result.TimeLeft.TotalSeconds;
            }
            return 0;
        }

        public static double GetMyAuraTimeLeft(HashSet<int> aura, WoWUnit onUnit)
        {
            if (onUnit != null)
            {
                var auras = onUnit.GetAllAuras();
                var result = auras.FirstOrDefault(a => aura.Contains(a.SpellId) && a.CreatorGuid == Me.Guid);
                if (result != null && result.TimeLeft.TotalSeconds > 0)
                    return result.TimeLeft.TotalSeconds;
            }
            return 0;
        }

        public static bool HasMyAura(string aura, WoWUnit u)
        {
            return u.GetAllAuras().Any(a => a.Name == aura && a.CreatorGuid == Me.Guid);
        }

        public static void CancelAura(this WoWUnit unit, string aura)
        {
            WoWAura a = unit.GetAuraFromName(aura);
            if (a != null && a.Cancellable)
                a.TryCancelAura();
        }

        #endregion Auras - methods to handle Auras/Buffs/Debuffs

        #region HasAura - Internal Extenstions

        public static bool HasAura(this WoWUnit unit, string aura, int stacks)
        {
            return HasAura(unit, aura, stacks, null);
        }

        public static bool HasAura(this WoWUnit unit, string aura, int stacks, WoWUnit creator)
        {
            return unit.GetAllAuras().Any(a => a.Name == aura && a.StackCount >= stacks && (creator == null || a.CreatorGuid == creator.Guid));
        }

        /// <summary>
        /// Checks for aura on unit
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="aura">Aura Name (i.e. "Corruption")</param>
        /// <param name="stacks">Set to 0 if aura doesn't stack</param>
        /// <param name="isMyAura">Checks the aura was applied by your (your GUID)</param>
        /// <param name="msLeft">Time remaining on aura</param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, string aura, int stacks = 0, bool isMyAura = false, int msLeft = 0)
        {
            WoWAura result = unit.GetAuraFromName(aura, isMyAura);

            if (result == null)
                return false;

            if (result.TimeLeft.TotalMilliseconds > msLeft)
                return result.StackCount >= stacks;

            return false;
        }

        /// <summary>
        /// Checks for aura on unit
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="aura">Aura Name (i.e. "Corruption")</param>
        /// <param name="stacks">Set to 0 if aura doesn't stack</param>
        /// <param name="isMyAura">Checks the aura was applied by your (your GUID)</param>
        /// <param name="msLeft">Time remaining on aura</param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, int aura, int stacks = 0, bool isMyAura = false, int msLeft = 0)
        {
            WoWAura result = unit.GetAuraFromID(aura, isMyAura);

            if (result == null)
                return false;

            if (result.TimeLeft.TotalMilliseconds > msLeft)
                return result.StackCount >= stacks;

            return false;
        }

        public static bool HasRaidDebuff(this WoWUnit unit)
        {
            return unit != null && unit.HasAnyAura(SpellLists.BuffRaidValid);
        }

        public static bool HasMyAura(this WoWUnit unit, string aura)
        {
            return unit.GetAllAuras().Any(a => a.Name == aura && a.CreatorGuid == Me.Guid);
        }

        public static bool HasMyAura(this WoWUnit unit, int spellId)
        {
            return unit.GetAllAuras().Any(a => a.SpellId == spellId && a.CreatorGuid == Me.Guid);
        }

        public static bool HasAnyAura(this WoWUnit unit, params string[] auraNames)
        {
            var auras = unit.GetAllAuras();
            var hashes = new HashSet<string>(auraNames);
            return auras.Any(a => hashes.Contains(a.Name));
        }

        public static bool HasAnyAura(this WoWUnit unit, params int[] auraIDs)
        {
            return auraIDs.Any(unit.HasAura);
        }

        public static bool HasAnyAura(this WoWUnit unit, HashSet<int> auraIDs)
        {
            var auras = unit.GetAllAuras();
            return auras.Any(a => auraIDs.Contains(a.SpellId));
        }

        public static bool HasAllAuras(this WoWUnit unit, params string[] auraNames)
        {
            return auraNames.All(unit.HasAura);
        }

        public static bool HasAllAuras(this WoWUnit unit, params int[] auraIDs)
        {
            return auraIDs.All(unit.HasAura);
        }

        public static bool HasAllMyAuras(this WoWUnit unit, params string[] auraNames)
        {
            return auraNames.All(unit.HasMyAura);
        }

        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWSpellMechanic[] mechanics)
        {
            var auras = unit.GetAllAuras();
            return auras.Any(a => mechanics.Contains(a.Spell.Mechanic));
        }

        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWApplyAuraType[] applyType)
        {
            var auras = unit.GetAllAuras();
            return auras.Any(a => a.Spell.SpellEffects.Any(se => applyType.Contains(se.AuraType)));
        }

        public static bool HasAuraWithEffect(this WoWUnit unit, WoWApplyAuraType applyType)
        {
            return unit.Auras.Values.Any(a => a.Spell != null && a.Spell.SpellEffects.Any(se => applyType == se.AuraType));
        }

        public static bool HasAuraWithEffect(this WoWUnit unit, WoWApplyAuraType auraType, int miscValue, int basePointsMin, int basePointsMax)
        {
            var auras = unit.Auras.Values;
            return (from a in auras
                    where a.Spell != null
                    let spell = a.Spell
                    from e in spell.GetSpellEffects()

                    // First check: Ensure the effect is... well... valid
                    where e != null &&

                        // Ensure the aura type is correct.
                    e.AuraType == auraType &&

                        // Check for a misc value. (Resistance types, etc)
                    (miscValue == -1 || e.MiscValueA == miscValue) &&

                        // Check for the base points value. (Usually %s for most debuffs)
                    e.BasePoints >= basePointsMin && e.BasePoints <= basePointsMax
                    select a).Any();
        }

        private static IEnumerable<SpellEffect> GetSpellEffects(this WoWSpell spell)
        {
            var effects = new SpellEffect[3];
            effects[0] = spell.GetSpellEffect(0);
            effects[1] = spell.GetSpellEffect(1);
            effects[2] = spell.GetSpellEffect(2);
            return effects;
        }

        #endregion HasAura - Internal Extenstions

        #region Moving - Internal Extentions

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool IsMoving(this WoWUnit unit)
        {
            return unit.MovementInfo.MovingBackward || unit.MovementInfo.MovingForward || unit.MovementInfo.MovingStrafeLeft || unit.MovementInfo.MovingStrafeRight;
        }

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot, and allows specifying how long you've been moving for before accepting it as actually moving.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="movingDuration">Duration in MS how long the unit has been moving before accepting it as a moving unit</param>
        /// <returns></returns>
        public static bool IsMoving(this WoWUnit unit, int movingDuration)
        {
            return unit.IsMoving() && unit.MovementInfo.TimeMoved >= movingDuration;
        }

        #endregion Moving - Internal Extentions

        #region Cached methods

        /*
         * Note: I attempted to build a method that could be re-used like so
         * CacheManager.QueryCachedObjects(unit.GetAllAuras(), "GetAllAuras_" + unit.Guid, expiry)
         * CacheManager.QueryCachedObjects(unit.GetAllAuras(), "CachedStackCountString_" + unit.Guid, expiry)
         * however when you pass the object to the method its giving a copy of itself 'New' thus defeating the purpose of caching
         * if anyone can think of a better way so we dont have to create 10,000 huge arse methods of repeating code let me know. -- wulf
         *
         * Default times are 1000ms
         */

        public static bool CachedHasAura(this WoWUnit unit, string aura, int stacks = 0, bool fromMyAura = false, int msLeft = 0, int expiry = 1000)
        {
            if (unit == null) return false;

            var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.Name == aura && a.TimeLeft > TimeSpan.Zero);
                
            if (cachedResult == null)
                return false;

            // debugging put your spell in here
            //if (cachedResult.Name == "Unstable Affliction")
            //Logger.InfoLog("[TimeLeft] {0} : {1} msLeft {2} msSetpoint {3}", aura, unit, cachedResult.TimeLeft.TotalMilliseconds, msLeft);

            if (cachedResult.TimeLeft.TotalMilliseconds > msLeft)
                return cachedResult.StackCount >= stacks;

            return false;
        }

        public static bool CachedHasAura(this WoWUnit unit, int aura, int stacks = 0, bool fromMyAura = false, int msLeft = 0, int expiry = 1000)
        {
            if (unit == null) return false;
           
            var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura && a.TimeLeft > TimeSpan.Zero);

            if (cachedResult == null)
                return false;

            // debugging put your spell in here
            //if (cachedResult.Name == "Unstable Affliction")
            //Logger.InfoLog("[TimeLeft] {0} : {1} msLeft {2} msSetpoint {3}", aura, unit, cachedResult.TimeLeft.TotalMilliseconds, msLeft);

            if (cachedResult.TimeLeft.TotalMilliseconds > msLeft)
                return cachedResult.StackCount >= stacks;

            return false;
        }

        public static bool CachedHasAuraDown(this WoWUnit unit, int aura, int stacks = 0, bool fromMyAura = false, int msLeft = 0, int expiry = 1000)
        {
            var cachedResult = unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura);

            if (cachedResult == null)
                return false;

            if (fromMyAura && cachedResult.CreatorGuid != StyxWoW.Me.Guid)
                return false;

            // debugging put your spell in here
            //if (cachedResult.Name == "Unstable Affliction")
            //Logger.InfoLog("[TimeLeft] {0} : {1} msLeft {2} msSetpoint {3}", aura, unit, cachedResult.TimeLeft.TotalMilliseconds, msLeft);

            if (cachedResult.TimeLeft.TotalMilliseconds < msLeft)
                return cachedResult.StackCount >= stacks;

            return false;
        }

        public static bool CachedHasAnyAura(this WoWUnit unit, HashSet<int> auraIDs, int expiry = 1000)
        {
            if (unit == null) return false;
            var cachedauras = unit.CachedGetAllAuras(expiry);
            return cachedauras.Any(a => auraIDs.Contains(a.SpellId));
        }

        public static bool CachedHasAnyAura(this WoWUnit unit, HashSet<string> auras, int expiry = 1000)
        {
            if (unit == null) return false;
            var cachedauras = unit.CachedGetAllAuras(expiry);
            return cachedauras.Any(a => auras.Contains(a.Name));
        }

        public static bool CachedAnyFriendlyHasMyAura(string aura, int expiry = 1000)
        {
            return CachedUnits.HealList.Any(u => u.ToUnit().CachedGetAllAuras(expiry).Any(a => a.Name == aura && a.CreatorGuid == Me.Guid));
        }

        public static bool CachedAnyEnemeyHasMyAura(string aura, int expiry = 1000)
        {
            return CachedUnits.AttackableUnits.Any(u => u.CachedGetAllAuras(expiry).Any(a => a.Name == aura && a.CreatorGuid == Me.Guid));
        }

        public static uint CachedStackCount(this WoWUnit unit, string aura, bool fromMyAura = false, int expiry = 1000)
        {
            {
                if (unit != null)
                {
                    var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.Name == aura && a.TimeLeft > TimeSpan.Zero);

                    if (cachedResult == null)
                        return 0;

                    return cachedResult.StackCount;
                }
                return 0;
            }
        }

        public static uint CachedStackCount(this WoWUnit unit, int aura, bool fromMyAura = false, int expiry = 1000)
        {
            {
                if (unit != null)
                {
                    var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura && a.TimeLeft > TimeSpan.Zero);

                    if (cachedResult == null)
                        return 0;

                    return cachedResult.StackCount;
                }
                return 0;
            }
        }

        public static double CachedGetAuraTimeLeft(this WoWUnit unit, int aura, bool fromMyAura = false, int expiry = 1000)
        {
            if (unit != null)
            {
                var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.SpellId == aura && a.TimeLeft > TimeSpan.Zero);

                if (cachedResult == null)
                    return 0;

                if (cachedResult.TimeLeft.TotalMilliseconds > 0)
                    return cachedResult.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        public static double CachedGetAuraTimeLeft(this WoWUnit unit, string aura, bool fromMyAura = false, int expiry = 1000)
        {
            if (unit != null)
            {
                var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => a.Name == aura && a.TimeLeft > TimeSpan.Zero);

                if (cachedResult == null)
                    return 0;

                if (cachedResult.TimeLeft.TotalMilliseconds > 0)
                    return cachedResult.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        public static double CachedGetAuraTimeLeft(this WoWUnit unit, HashSet<int> aura, bool fromMyAura = false, int expiry = 1000)
        {
            if (unit != null)
            {
                var cachedResult = fromMyAura ? unit.CachedGetAllAuras(expiry).FirstOrDefault(a => aura.Contains(a.SpellId) && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : unit.CachedGetAllAuras(expiry).FirstOrDefault(a => aura.Contains(a.SpellId) && a.TimeLeft > TimeSpan.Zero);
                   
                if (cachedResult == null)
                    return 0;

                if (cachedResult.TimeLeft.TotalMilliseconds > 0)
                    return cachedResult.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        // Counts (cached) to blanket check the AttackableUnits for aura(s)
        internal static int CountUnitAura(HashSet<int> aura)
        {
            return CachedUnits.AttackableUnits.Count(unit => !unit.CachedHasAnyAura(aura));
        }

        internal static int CountUnitAura(int aura)
        {
            return CachedUnits.AttackableUnits.Count(unit => !unit.ToPlayer().CachedHasAura(aura));
        }

        internal static int CountUnitAura(string aura)
        {
            return CachedUnits.AttackableUnits.Count(unit => !unit.CachedHasAura(aura));
        }

        // Gets (cached) target to apply an aura too
        internal static WoWUnit GetUnbuffedTarget(int withoutBuff)
        {
            return CachedUnits.AttackableUnits.FirstOrDefault(unit => unit != null && !unit.CachedHasAura(withoutBuff));
        }

        internal static WoWUnit GetUnbuffedTarget(HashSet<int> withoutBuff)
        {
            return CachedUnits.AttackableUnits.FirstOrDefault(unit => unit != null && !unit.CachedHasAnyAura(withoutBuff));
        }

        public static bool CachedHasAuraWithEffect(this WoWUnit unit, WoWApplyAuraType auraType, int miscValue, int basePointsMin, int basePointsMax)
        {
            var auras = unit.CachedGetAllAuras();
            return (from a in auras
                    where a.Spell != null
                    let spell = a.Spell
                    from e in spell.GetSpellEffects()

                    // First check: Ensure the effect is... well... valid
                    where e != null &&

                        // Ensure the aura type is correct.
                    e.AuraType == auraType &&

                        // Check for a misc value. (Resistance types, etc)
                    (miscValue == -1 || e.MiscValueA == miscValue) &&

                        // Check for the base points value. (Usually %s for most debuffs)
                    e.BasePoints >= basePointsMin && e.BasePoints <= basePointsMax
                    select a).Any();
        }

        internal static WoWAuraCollection CachedGetAllAuras(this WoWUnit unit, int expiry = 1000)
        {
            string cacheKey = "GetAllAuras_" + unit.Guid;

            // Check the cache
            var getAllAuras = CacheManager.Get<WoWAuraCollection>(cacheKey);

            if (getAllAuras == null)
            {
                // Go and retrieve the data from the objectManager
                getAllAuras = unit.GetAllAuras();

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(getAllAuras, cacheKey, expiry);
            }
            return getAllAuras;
        }

        #endregion Cached methods

        #region Ground Affects

        public static WoWDynamicObject GetGroundEffectBySpellId(int spellId)
        {
            return ObjectManager.GetObjectsOfType<WoWDynamicObject>().FirstOrDefault(o => o.SpellId == spellId);
        }

        public static bool IsStandingInGroundEffect(bool harmful = true)
        {
            foreach (var obj in ObjectManager.GetObjectsOfType<WoWDynamicObject>().Where(obj => obj.Distance <= obj.Radius))
            {
                // We're standing in this.
                if (obj.Caster.IsFriendly && !harmful)
                    return true;
                if (obj.Caster.IsHostile && harmful)
                    return true;
            }
            return false;
        }

        #endregion Ground Affects

        #region Nested type: Selection

        internal delegate T Selection<out T>(object context);

        #endregion Nested type: Selection

        #region GetTreePerformance

        private static readonly Stopwatch TreePerformanceTimer = new Stopwatch();
        private static readonly Stopwatch CompositePerformanceTimer = new Stopwatch();

        /// <summary>  Usage: Spell.TreePerformance(true) within a composite. </summary>
        internal static Composite TreePerformance(bool enable)
        {
            return new Action(ret =>
            {
                if (!enable)
                {
                    return RunStatus.Failure;
                }

                if (TreePerformanceTimer.ElapsedMilliseconds > 0)
                {
                    // NOTE: This dosnt account for Spell casts (meaning the total time is not the time to traverse the tree plus the current cast time of the spell)..this is actual time to traverse the tree.
                    Logger.InfoLog("[TreePerformance] Elapsed Time to traverse Tree: {0} ms", TreePerformanceTimer.ElapsedMilliseconds);
                    TreePerformanceTimer.Stop();
                    TreePerformanceTimer.Reset();
                }
                TreePerformanceTimer.Start();

                return RunStatus.Failure;
            });
        }

        /// <summary>  Usage: Spell.CompositePerformance(Composite, "SomeComposite") within a composite. </summary>
        internal static Composite CompositePerformance(Composite child, string name = "SomeComposite")
        {
            return new Sequence(
                new Action(delegate
                    {
                        CompositePerformanceTimer.Reset();
                        CompositePerformanceTimer.Start();
                        return RunStatus.Success;
                    }),
                child,
                new Action(delegate
                    {
                        CompositePerformanceTimer.Stop();
                        Logger.InfoLog("[CompositePerformance] {0} took {1} ms", name,
                                       CompositePerformanceTimer.ElapsedMilliseconds);
                        return RunStatus.Success;
                    })
                );
        }

        #endregion GetTreePerformance

        //-- Dont Put Lua in here -- Put it in the Lua Class -- wulf.
    }
}