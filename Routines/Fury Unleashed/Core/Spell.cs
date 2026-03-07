// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable CompareOfFloatsByEqualityOperator
using CommonBehaviors.Actions;
using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Utilities;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace FuryUnleashed.Core
{
    internal static class Spell
    {
        #region Delegates
        // Public delegates
        public delegate WoWUnit UnitSelectionDelegate(object context);
        public delegate WoWPoint LocationRetriever(object context);
        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static WoWSpell GcdSpell { get; set; }

        // Internal delegates
        internal delegate T Selection<out T>(object context);
        internal static readonly uint ClientLatency = StyxWoW.WoWClient.Latency;
        #endregion

        #region Casting Methods
        /// <summary>
        /// Casting method for regular casting - string
        /// </summary>
        /// <param name="spellname">String SpellName to Cast</param>
        /// <param name="reqs">-</param>
        /// <param name="failThrough">This at true continues the tree as it returns a runstatus.failure - Required for chain-casting like popping multiple CD's!</param>
        /// <returns></returns>
        public static Composite Cast(string spellname, Selection<bool> reqs = null, bool failThrough = false)
        {
            return Cast(spellname, ret => StyxWoW.Me.CurrentTarget, reqs, failThrough);
        }

        public static Composite Cast(string spellname, UnitSelectionDelegate onUnit, Selection<bool> reqs = null, bool failThrough = false)
        {
            return
                new Decorator(ret => (onUnit != null && onUnit(ret) != null && (reqs == null || reqs(ret)) && SpellManager.CanCast(spellname, onUnit(ret))),
                    new Action(ret =>
                    {
                        if (SpellManager.Cast(spellname, onUnit(ret)))
                        {
                            CooldownTracker.SpellUsed(spellname);
                            Logger.CombatLogOr("Casting: " + spellname + " on " + onUnit(ret).SafeName);
                            if (!failThrough)
                                return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    }));
        }

        /// <summary>
        /// Casting method for regular casting - int
        /// </summary>
        /// <param name="spellid">Integer Spell to Cast</param>
        /// <param name="reqs">-</param>
        /// <param name="failThrough">This at true continues the tree as it returns a runstatus.failure - Required for chain-casting like popping multiple CD's!</param>
        /// <returns></returns>
        public static Composite Cast(int spellid, Selection<bool> reqs = null, bool failThrough = false)
        {
            return Cast(spellid, ret => StyxWoW.Me.CurrentTarget, reqs, failThrough);
        }

        public static Composite Cast(int spellid, UnitSelectionDelegate onUnit, Selection<bool> reqs = null, bool failThrough = false)
        {
            return
                new Decorator(ret => ((reqs != null && reqs(ret)) || (reqs == null)) && onUnit != null && onUnit(ret) != null && SpellManager.CanCast(spellid, onUnit(ret)),
                    new Action(ret =>
                    {
                        if (SpellManager.Cast(spellid, onUnit(ret)))
                        {
                            CooldownTracker.SpellUsed(spellid);
                            Logger.CombatLogOr("Casting: " + WoWSpell.FromId(spellid).Name + " on " + onUnit(ret).SafeName);
                            if (!failThrough)
                                return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    }));
        }

        /// <summary>
        /// Casting method used for MultiDotting. Not using any caching, so might decrease performance!
        /// </summary>
        /// <param name="spellid">ID of the spell to be cast - eg SpellBook.Bloodthirst</param>
        /// <param name="debuffid">ID of the debuff to find on a possible multidot target - eg AuraBook.DeepWounds</param>
        /// <param name="auratimeleft">Timeleft on the aura in order to refresh it if no unit without the aura is found - Default 1500ms</param>
        /// <param name="radius">Radius - Default 5 yards</param>
        /// <param name="conedegrees">Degrees for the Facing Cone (Me.IsSafelyFacing() check) - Default 160</param>
        /// <param name="reqs">-</param>
        /// <param name="failThrough">This at true continues the tree as it returns a runstatus.failure - Required for chain-casting like popping multiple CD's!</param>
        /// <returns>-</returns>
        public static Composite MultiDoT(int spellid, int debuffid, int auratimeleft = 1500, double radius = 5, int conedegrees = 160, Selection<bool> reqs = null, bool failThrough = false)
        {
            using (new PerformanceLogger("MultiDoT"))
            {
                WoWUnit target = null;
                try
                {
                    return new Decorator(ret => ((reqs != null && reqs(ret)) || reqs == null) && Unit.NearbyAttackableUnitsCount > 1,
                        new PrioritySelector(dot => target = Unit.MultiDotUnit(debuffid, auratimeleft, radius, conedegrees),
                            new Decorator(ctx => Unit.IsViable(target),
                                Cast(spellid, on => target, ret => Unit.IsViable(target)))));
                }
                catch (Exception ex)
                {
                    return new Action(ret =>
                        {
                            Logger.DiagLogPu("Exception MultiDot {0}", ex);
                            Logger.DiagLogPu("Tried to cast spell (MultiDot)={0}, dotTarget={1},name={3},health={4}", spellid, target != null, target != null ? target.SafeName : "<none>", target != null ? target.HealthPercent : 0);
                        });
                }

            }
        }

        /// <summary>
        /// Casting method for casting on Ground location - string
        /// </summary>
        /// <param name="spellname">String Spellname</param>
        /// <param name="onLocation">Location where to cast - Like Me.Location</param>
        /// <returns></returns>
        public static Composite CastOnGround(string spellname, LocationRetriever onLocation)
        {
            return CastOnGround(spellname, onLocation, ret => true);
        }

        public static Composite CastOnGround(string spellname, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return
                new Decorator(ret => onLocation != null && requirements(ret) && SpellManager.CanCast(spellname) && (StyxWoW.Me.Location.Distance(onLocation(ret)) <= SpellManager.Spells[spellname].MaxRange || SpellManager.Spells[spellname].MaxRange == 0),
                    new Sequence(
                       new Action(ret => SpellManager.Cast(spellname)),
                        new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1, ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Name == spellname, new ActionAlwaysSucceed())),
                        new Action(ret =>
                            {
                                SpellManager.ClickRemoteLocation(onLocation(ret));
                                CooldownTracker.SpellUsed(spellname);
                                Logger.CombatLogOr("Casting: " + spellname);
                            }
                        )));
        }

        /// <summary>
        /// Casting method for casting on Ground location - int
        /// </summary>
        /// <param name="spellid">Integer Spell</param>
        /// <param name="onLocation">Location where to cast - Like Me.Location</param>
        /// <returns></returns>
        public static Composite CastOnGround(int spellid, LocationRetriever onLocation)
        {
            return CastOnGround(spellid, onLocation, ret => true);
        }

        public static Composite CastOnGround(int spellid, LocationRetriever onLocation, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return
                new Decorator(ret => onLocation != null && requirements(ret) && SpellManager.CanCast(spellid) && (StyxWoW.Me.Location.Distance(onLocation(ret)) <= WoWSpell.FromId(spellid).MaxRange || WoWSpell.FromId(spellid).MaxRange == 0),
                    new Sequence(
                       new Action(ret => SpellManager.Cast(spellid)),
                        new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1, ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Id == spellid, new ActionAlwaysSucceed())),
                        new Action(ret =>
                        {
                            SpellManager.ClickRemoteLocation(onLocation(ret));
                            CooldownTracker.SpellUsed(spellid);
                            Logger.CombatLogOr("Casting: " + spellid);
                        }
                        )));
        }

        /// <summary>
        /// Casting method for casting on Ground location - string
        /// </summary>
        /// <param name="spellname">String Spellname</param>
        /// <param name="onUnit">Casting on unit ground location</param>
        /// <returns></returns>
        public static Composite CastOnGround(string spellname, UnitSelectionDelegate onUnit)
        {
            return CastOnGround(spellname, onUnit, ret => true);
        }

        public static Composite CastOnGround(string spellname, UnitSelectionDelegate onUnit, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return
                new Decorator(ret => onUnit != null && requirements(ret) && SpellManager.CanCast(spellname) && (StyxWoW.Me.Location.Distance(onUnit(ret).Location) <= SpellManager.Spells[spellname].MaxRange || SpellManager.Spells[spellname].MaxRange == 0),
                    new Sequence(
                       new Action(ret => SpellManager.Cast(spellname)),
                        new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1, ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Name == spellname, new ActionAlwaysSucceed())),
                        new Action(ret =>
                        {
                            SpellManager.ClickRemoteLocation(onUnit(ret).Location);
                            CooldownTracker.SpellUsed(spellname);
                            Logger.CombatLogOr("Casting: " + spellname);
                        }
                        )));
        }

        /// <summary>
        /// Casting method for casting on Ground location - int
        /// </summary>
        /// <param name="spellid">Integer Spell ID</param>
        /// <param name="onUnit">Casting on unit ground location</param>
        /// <returns></returns>
        public static Composite CastOnGround(int spellid, UnitSelectionDelegate onUnit)
        {
            return CastOnGround(spellid, onUnit, ret => true);
        }

        public static Composite CastOnGround(int spellid, UnitSelectionDelegate onUnit, CanRunDecoratorDelegate requirements, bool waitForSpell = false)
        {
            return
                new Decorator(ret => onUnit != null && requirements(ret) && SpellManager.CanCast(spellid) && (StyxWoW.Me.Location.Distance(onUnit(ret).Location) <= WoWSpell.FromId(spellid).MaxRange || WoWSpell.FromId(spellid).MaxRange == 0),
                    new Sequence(
                       new Action(ret => SpellManager.Cast(spellid)),
                        new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1, ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Id == spellid, new ActionAlwaysSucceed())),
                        new Action(ret =>
                        {
                            SpellManager.ClickRemoteLocation(onUnit(ret).Location);
                            CooldownTracker.SpellUsed(spellid);
                            Logger.CombatLogOr("Casting: " + spellid);
                        }
                        )));
        }
        #endregion

        #region GCD Detection
        /// <summary>
        /// This section is used to determine if we're on GlobalCooldown and not continueing treetraverse if we are.
        /// </summary>
        /// <returns></returns>
        public static bool IsGlobalCooldown()
        {
            using (new PerformanceLogger("IsGlobalCooldown"))
            {
                return (GlobalCooldownLeft.TotalMilliseconds - 10) > StyxWoW.WoWClient.Latency << 1;
            }
        }

        public static void InitGcdSpell()
        {
            if (GetGlobalCooldownSpell != null)
            {
                Logger.DiagLogWh("FU: GCD Spell is set to {0}", GetGlobalCooldownSpell);
                GcdSpell = GetGlobalCooldownSpell;
            }
        }

        private static TimeSpan GlobalCooldownLeft
        {
            get { return GcdSpell != null ? GcdSpell.CooldownTimeLeft : SpellManager.GlobalCooldownLeft; }
        }

        public static WoWSpell GetGlobalCooldownSpell
        {
            get { return SpellManager.Spells.FirstOrDefault(s => GcdSpells.Contains(s.Value.Id)).Value; }
        }

        private static readonly HashSet<int> GcdSpells = new HashSet<int>
        {
            7386, // Sunder Armor
            78 // Heroic Strike
        };
        #endregion GCD

        #region Cached & Non-Cached Aura Functions
        /// <summary>
        /// The lists required for cached auras.
        /// </summary>
        public static IEnumerable<WoWAura> CachedAuras = new List<WoWAura>();
        public static IEnumerable<WoWAura> CachedTargetAuras = new List<WoWAura>();

        /// <summary>
        /// Gets aura's on StyxWoW.Me and StyxWoW.Me.CurrentTarget and adds them to CachedAuras & CachedTargetAuras lists for one traverse.
        /// </summary>
        public static void GetCachedAuras()
        {
            using (new PerformanceLogger("GetCachedAuras"))
            {
                try
                {
                    if (Unit.IsViable(Me)) { CachedAuras = Me.GetAllAuras(); }
                    if (Unit.IsViable(Me.CurrentTarget)) { CachedTargetAuras = Me.CurrentTarget.GetAllAuras(); }
                }
                catch (Exception getcachedauraException)
                {
                    Logger.DiagLogFb("FU: Failed to retrieve aura's - {0}", getcachedauraException);
                }
            }
        }

        /// <summary>
        /// Gets fading aura timeleft  - String
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, Etc)</param>
        /// <param name="auraname">Full Auraname</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <returns></returns>
        public static double AuraTimeLeft(this WoWUnit unit, string auraname, bool isFromMe = true, bool cached = true)
        {
            using (new PerformanceLogger("AuraTimeLeft-String"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return 0;

                    if (!cached)
                    {
                        WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname);
                        return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        WoWAura aura = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.Name == auraname);
                        return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.Name == auraname);
                        return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;
                    }
                    return 0;
                }
                catch (Exception) { return 0; }
            }
        }

        /// <summary>
        /// Gets fading aura timeleft  - ID
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, Etc)</param>
        /// <param name="auraId">AuraBook.Auraname or Aura ID</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <returns></returns>
        public static double AuraTimeLeft(this WoWUnit unit, int auraId, bool isFromMe = true, bool cached = true)
        {
            using (new PerformanceLogger("AuraTimeLeft-Int"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return 0;

                    if (!cached)
                    {
                        WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        WoWAura aura = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;
                    }
                    return 0;
                }
                catch (Exception) { return 0; }
            }
        }

        /// <summary>
        /// Used for cancelling aura's.
        /// </summary>
        /// <param name="unit">Unit to cancel aura's on --> Always Me?</param>
        /// <param name="auraname">AuraBook.Auraname or Aura ID</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        public static void CancelAura(this WoWUnit unit, string auraname, bool isFromMe = false, bool cached = true)
        {
            using (new PerformanceLogger("CancelAura-String"))
            {
                if (!cached && unit == Me)
                {
                    WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname);
                    if (aura != null)
                    {
                        Logger.CombatLogWh("Cancelling Aura: {0}", auraname);
                        aura.TryCancelAura();
                    }
                }

                if (cached && CachedAuras != null && unit == Me)
                {
                    WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.Name == auraname);
                    if (aura != null)
                    {
                        Logger.CombatLogWh("Cancelling Aura: {0}", auraname);
                        aura.TryCancelAura();
                    }
                }
            }
        }

        /// <summary>
        /// Used for cancelling aura's.
        /// </summary>
        /// <param name="unit">Unit to cancel aura's on --> Always Me?</param>
        /// <param name="auraId">AuraBook.Auraname or Aura ID</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        public static void CancelAura(this WoWUnit unit, int auraId, bool isFromMe = false, bool cached = true)
        {
            using (new PerformanceLogger("CancelAura-Int"))
            {
                if (!cached && unit == Me)
                {
                    WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId);
                    if (aura != null)
                    {
                        Logger.CombatLogWh("Cancelling Aura: {0}", WoWSpell.FromId(auraId).Name);
                        aura.TryCancelAura();
                    }
                }

                if (cached && CachedAuras != null && unit == Me)
                {
                    WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.SpellId == auraId);
                    if (aura != null)
                    {
                        Logger.CombatLogWh("Cancelling Aura: {0}", WoWSpell.FromId(auraId).Name);
                        aura.TryCancelAura();
                    }
                }
            }
        }

        /// <summary>
        /// Gets fading aura timeleft - String
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, Etc)</param>
        /// <param name="auraname">Full Auraname</param>
        /// <param name="fadingtime">Timeleft on the Aura</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <returns></returns>
        public static bool FadingAura(this WoWUnit unit, string auraname, int fadingtime, bool isFromMe = true, bool cached = true)
        {
            using (new PerformanceLogger("FadingAura-String"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return false;

                    if (!cached)
                    {
                        WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname);
                        return aura != null && aura.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        WoWAura aura = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.Name == auraname);
                        return aura != null && aura.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.Name == auraname);
                        return aura != null && aura.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
                    }
                }
                catch (Exception) { return false; }
            }
            return false;
        }

        /// <summary>
        /// Gets fading aura timeleft - ID
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, Etc)</param>
        /// <param name="auraId">AuraBook.Auraname or Aura ID</param>
        /// <param name="fadingtime">Timeleft on the Aura</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <returns></returns>
        public static bool FadingAura(this WoWUnit unit, int auraId, int fadingtime, bool isFromMe = true, bool cached = true)
        {
            using (new PerformanceLogger("FadingAura-Int"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return false;

                    if (!cached)
                    {
                        WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null && aura.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        WoWAura aura = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null && aura.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null && aura.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
                    }
                }
                catch (Exception) { return false; }
            }
            return false;
        }

        /// <summary>
        /// HasAura extension - String
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, etc)</param>
        /// <param name="auraname">Full Auraname</param>
        /// <param name="stacks">Amount of Stacks on the Aura</param>
        /// <param name="msuLeft">Timeleft on the Aura</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <param name="nonlinq">Avoids using LINQ query to retrieve aura, use this is aura retrieval fails for whatever reason - !!!Not possible to check anything but aura!!!</param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, string auraname, int stacks = 0, int msuLeft = 0, bool isFromMe = true, bool cached = true, bool nonlinq = false)
        {
            using (new PerformanceLogger("HasAura-String"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return false;

                    WoWAura auraresult = null;

                    if (nonlinq)
                    {
                        if (unit.HasAura(auraname)) return true;
                    }

                    if (!cached)
                    {
                        auraresult = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname);
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        auraresult = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.Name == auraname);
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        auraresult = isFromMe ? CachedAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.Name == auraname);
                    }

                    if (auraresult == null) return false;

                    if (auraresult.TimeLeft.TotalMilliseconds > msuLeft)
                        return auraresult.StackCount >= stacks;
                }
                catch (Exception) { return false; }
            }
            return false;
        }

        /// <summary>
        /// HasAura extension - ID
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, etc)</param>
        /// <param name="auraId">AuraBook.Auraname or Aura ID</param>
        /// <param name="stacks">Amount of Stacks on the Aura</param>
        /// <param name="msuLeft">Timeleft on the Aura</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <param name="nonlinq">Avoids using LINQ query to retrieve aura, use this is aura retrieval fails for whatever reason - !!!Not possible to check anything but aura!!!</param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, int auraId, int stacks = 0, int msuLeft = 0, bool isFromMe = true, bool cached = true, bool nonlinq = false)
        {
            using (new PerformanceLogger("HasAura-Int"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return false;

                    WoWAura auraresult = null;

                    if (nonlinq)
                    {
                        if (unit.HasAura(auraId)) return true;
                    }

                    if (!cached)
                    {
                        auraresult = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId);
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        auraresult = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId);
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        auraresult = isFromMe ? CachedAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.SpellId == auraId);
                    }

                    if (auraresult == null) return false;

                    if (auraresult.TimeLeft.TotalMilliseconds > msuLeft)
                        return auraresult.StackCount >= stacks;
                }
                catch (Exception) { return false; }
            }
            return false;
        }

        /// <summary>
        /// Gets remaining aura timeleft - String
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, Etc)</param>
        /// <param name="auraname">Full Auraname</param>
        /// <param name="remainingtime">Timeleft on the Aura</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <returns></returns>
        public static bool RemainingAura(this WoWUnit unit, string auraname, int remainingtime, bool isFromMe = true, bool cached = true)
        {
            using (new PerformanceLogger("RemainingAura-String"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return false;

                    if (!cached)
                    {
                        WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.Name == auraname);
                        return aura != null && aura.TimeLeft >= TimeSpan.FromMilliseconds(remainingtime);
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        WoWAura aura = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.Name == auraname);
                        return aura != null && aura.TimeLeft >= TimeSpan.FromMilliseconds(remainingtime);
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.Name == auraname && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.Name == auraname);
                        return aura != null && aura.TimeLeft >= TimeSpan.FromMilliseconds(remainingtime);
                    }
                }
                catch (Exception) { return false; }
            }
            return false;
        }

        /// <summary>
        /// Gets remaining aura timeleft - ID
        /// </summary>
        /// <param name="unit">Unit (Me, Me.CurrentTarget, Etc)</param>
        /// <param name="auraId">AuraBook.Auraname or Aura ID</param>
        /// <param name="remainingtime">Timeleft on the Aura</param>
        /// <param name="isFromMe">True, False - Is aura made by Me</param>
        /// <param name="cached">True, False - Use cached aura's</param>
        /// <returns></returns>
        public static bool RemainingAura(this WoWUnit unit, int auraId, int remainingtime, bool isFromMe = true, bool cached = true)
        {
            using (new PerformanceLogger("RemainingAura-Int"))
            {
                try
                {
                    if (!Unit.IsViable(unit)) return false;

                    if (!cached)
                    {
                        WoWAura aura = isFromMe ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null && aura.TimeLeft >= TimeSpan.FromMilliseconds(remainingtime);
                    }

                    if (CachedTargetAuras != null && unit == Me.CurrentTarget)
                    {
                        WoWAura aura = isFromMe ? CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedTargetAuras.FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null && aura.TimeLeft >= TimeSpan.FromMilliseconds(remainingtime);
                    }

                    if (CachedAuras != null && unit == Me)
                    {
                        WoWAura aura = isFromMe ? CachedAuras.FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Root.MyToonGuid) : CachedAuras.FirstOrDefault(a => a.SpellId == auraId);
                        return aura != null && aura.TimeLeft >= TimeSpan.FromMilliseconds(remainingtime);
                    }
                }
                catch (Exception) { return false; }
            }
            return false;
        }
        #endregion
    }
}
