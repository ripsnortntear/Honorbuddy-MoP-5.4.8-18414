#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Spells/Spell.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using Oracle.Classes;
using Oracle.Classes.Monk;
using Oracle.Classes.Paladin;
using Oracle.Core.Encounters;
using Oracle.Core.Spells.Auras;
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities;
using Oracle.UI.Settings;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Core.Spells
{
    internal static class Spell
    {
        #region Nested type: Selection

        internal delegate T Selection<out T>(object context);

        #endregion Nested type: Selection

        #region settings

        private static OracleSettings OSetting { get { return OracleSettings.Instance; } }

        private static bool EnableStopCasting { get { return OSetting.EnableStopCasting; } }

        private static int StopCastingPercent { get { return OSetting.StopCastingPercent; } }

        private static bool MalkorokEncounter { get { return OSetting.MalkorokEncounter; } }

        #endregion settings

        public static string LastspellcastString;
        public static int LastspellcastInt;
        public static WoWUnit LastTarget;

        private static bool CanClipChannel()
        {
            if (!StyxWoW.Me.IsChanneling) { return false; }
            return !(StyxWoW.Me.CurrentChannelTimeLeft.TotalMilliseconds < (StyxWoW.WoWClient.Latency << 1) + 150);
        }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite CreateWaitForLagDuration()
        {
            return new WaitContinue(TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency << 1) + 150), ret => false, new ActionAlwaysSucceed());
        }

        #region GCD

        public static double GetSpellCooldown_test(int spell)
        {
            SpellFindResults results;
            if (!SpellManager.FindSpell(spell, out results)) return TimeSpan.MaxValue.TotalSeconds;
            return results.Override != null
                ? Math.Max(results.Override.CooldownTimeLeft.TotalMilliseconds - SpellManager.GlobalCooldownLeft.TotalMilliseconds, 0)
                : Math.Max(results.Original.CooldownTimeLeft.TotalMilliseconds - SpellManager.GlobalCooldownLeft.TotalMilliseconds, 0);
        }

        public static double GetSpellCooldown_test(string spell)
        {
            SpellFindResults results;
            if (!SpellManager.FindSpell(spell, out results)) return TimeSpan.MaxValue.TotalSeconds;
            return results.Override != null
                ? Math.Max(results.Override.CooldownTimeLeft.TotalMilliseconds - SpellManager.GlobalCooldownLeft.TotalMilliseconds, 0)
                : Math.Max(results.Original.CooldownTimeLeft.TotalMilliseconds - SpellManager.GlobalCooldownLeft.TotalMilliseconds, 0);
        }

        public static bool GlobalCooldown()
        {
            using (new PerformanceLogger("Globalcooldown"))
            {
                var result1 = Math.Max(OracleGCD.GlobalCooldownLeft.TotalMilliseconds, 0); // - SpellManager.GlobalCooldownLeft.TotalMilliseconds
                return result1 > 0;
            }
        }

        #endregion GCD

        #region StopCasting

        public static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit; } }

        /// <summary> Stop casting healing spells if our heal target is above a certain health percent! </summary>
        public static Composite StopCasting()
        {
            try
            {
                return new Decorator(ret =>
                {
                    if (!EnableStopCasting)
                        return false;

                    if (!StyxWoW.Me.IsCasting)
                        return false;

                    if (MalkorokEncounter)
                        return false;

                    if (Thock.IsDeafeningScreech ||
                        Oondasta.IsPiercingRoar ||
                        DarkAnimus.IsInterruptingJolt)
                        return true;

                    if (!OracleRoutine.IsViable(HealTarget))
                        return false;

                    if (HealTarget.IsMe && StyxWoW.Me.HealthPercent > StopCastingPercent)
                        return false;

                    if ((StyxWoW.Me.CastingSpellId == RotationBase.Harmony &&
                         !Me.HasAura("Harmony", 0, true, 1200)))
                        return false;

                    if ((StyxWoW.Me.CastingSpellId == RotationBase.DivineLight &&
                         PaladinCommon.BuildingIlluminatedHealingBuffs))
                        return false;

                    if (StyxWoW.Me.CastingSpellId == RotationBase.ChainHeal)
                        return false;

                    if ((StyxWoW.Me.CastingSpellId == RotationBase.SoothingMist && MonkCommon.EnableSoothingSpam))
                        return false;

                    if (HealTarget.HealthPercent < StopCastingPercent)
                        return false;

                    if (!StyxWoW.Me.IsCastingHealingSpell())
                        return false;

                    if (StyxWoW.Me.ActiveAuras.ContainsKey("Spirit Shell") ||
                        StyxWoW.Me.ActiveAuras.ContainsKey("Clearcasting") ||
                        StyxWoW.Me.HasAnyAura(RotationBase.Lucidity))
                        return false;

                    return true;
                },
                new Action(delegate
                    {
                        Logger.Output(String.Format(" [DIAG] Stopped Casting {0} {2} @ {1:F1}%", WoWSpell.FromId(StyxWoW.Me.CastingSpellId).Name, (OracleRoutine.IsViable(HealTarget) ? HealTarget.HealthPercent : 0), (OracleRoutine.IsViable(HealTarget) ? HealTarget.SafeName : "Unknown")));
                        SpellManager.StopCasting();
                        return RunStatus.Success;
                    }));
            }

            catch (AccessViolationException)
            {
                // empty
                return new ActionAlwaysFail();
            }
            catch (InvalidObjectPointerException)
            {
                // empty
                return new ActionAlwaysFail();
            }
        }

        private static bool IsCastingHealingSpell(this WoWUnit unit)
        {
            if (!OracleRoutine.IsViable(unit)) return false;

            var castingSpellId = unit.CastingSpellId;

            if (castingSpellId != 0)
            {
                return HealingSpellIds.Contains(castingSpellId);
            }

            var nonChanneledCastingSpellId = unit.NonChanneledCastingSpellId;

            if (unit.NonChanneledCastingSpellId != 0)
            {
                return HealingSpellIds.Contains(nonChanneledCastingSpellId);
            }

            return false;
        }

        private static List<int> HealingSpellIds
        {
            get
            {
                var result = new List<int>();

                try
                {
                    switch (StyxWoW.Me.Specialization)
                    {
                        case WoWSpec.MonkMistweaver:
                            result.Add(RotationBase.SoothingMist);
                            result.Add(RotationBase.Uplift);
                            result.Add(RotationBase.SurgingMist);
                            result.Add(RotationBase.RenewingMist);
                            result.Add(RotationBase.Revival);
                            result.Add(RotationBase.EnvelopingMist);
                            result.Add(RotationBase.LifeCocoon);
                            result.Add(RotationBase.HealingSphere);
                            break;

                        case WoWSpec.DruidRestoration:
                            result.Add(RotationBase.Lifebloom);
                            result.Add(RotationBase.Regrowth);
                            result.Add(RotationBase.Rejuvenation);
                            result.Add(RotationBase.Nourish);
                            result.Add(RotationBase.HealingTouch);
                            result.Add(RotationBase.Swiftmend);
                            result.Add(RotationBase.WildGrowth);
                            result.Add(RotationBase.ForceOfNature);
                            result.Add(RotationBase.Incarnation);
                            result.Add(RotationBase.NaturesSwiftness);
                            break;

                        case WoWSpec.PriestHoly:
                        case WoWSpec.PriestDiscipline:
                            result.Add(RotationBase.Cascade);
                            result.Add(RotationBase.Halo);
                            result.Add(RotationBase.BindingHeal);
                            result.Add(RotationBase.FlashHeal);
                            result.Add(RotationBase.GreaterHeal);
                            result.Add(RotationBase.Heal);
                            result.Add(RotationBase.PowerWordBarrier);
                            result.Add(RotationBase.PowerWordShield);
                            result.Add(RotationBase.PrayerOfHealing);
                            result.Add(RotationBase.Renew);
                            result.Add(RotationBase.CircleOfHealing);
                            result.Add(RotationBase.GuardianSpirit);
                            result.Add(RotationBase.HolyWordSanctuary);
                            result.Add(RotationBase.HolyWordSanctuary);
                            result.Add(RotationBase.HolyWordSerenity);
                            result.Add(RotationBase.VoidShift);
                            break;

                        case WoWSpec.PaladinHoly:
                            result.Add(RotationBase.EternalFlame);
                            result.Add(RotationBase.SacredShield);
                            result.Add(RotationBase.HandOfPurity);
                            result.Add(RotationBase.HolyPrism);
                            result.Add(RotationBase.LightsHammer);
                            result.Add(RotationBase.ExecutionSentence);
                            result.Add(RotationBase.DivineLight);
                            result.Add(RotationBase.FlashofLight);
                            result.Add(RotationBase.SelflessHealer);
                            result.Add(RotationBase.HandofProtection);
                            result.Add(RotationBase.HolyLight);
                            result.Add(RotationBase.HolyRadiance);
                            result.Add(RotationBase.LayonHands);
                            result.Add(RotationBase.LightofDawn);
                            break;

                        case WoWSpec.ShamanRestoration:
                            result.Add(RotationBase.ChainHeal);
                            result.Add(RotationBase.GreaterHealingWave);
                            //result.Add(RotationBase.HealingRain);
                            result.Add(RotationBase.HealingSurge);
                            result.Add(RotationBase.HealingWave);
                            result.Add(RotationBase.Riptide);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Output(" Error in HealingSpellIds : {0}", ex.StackTrace);
                }

                return result;
            }
        }

        #endregion StopCasting

        #region Double Cast Shit

        private static readonly Dictionary<string, DoubleCastSpell> DoubleCastEntries = new Dictionary<string, DoubleCastSpell>();

        public static void OutputDoubleCastEntries()
        {
            foreach (var spell in DoubleCastEntries)
            {
                Logger.Output(spell.Key + " time: " + spell.Value.DoubleCastCurrentTime);
            }
        }

        internal static void PulseDoubleCastEntries()
        {
            DoubleCastEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.DoubleCastCurrentTime).TotalSeconds >= t.DoubleCastExpiryTime);
        }

        private static void UpdateDoubleCastEntries(string spellName, double expiryTime)
        {
            if (DoubleCastEntries.ContainsKey(spellName)) DoubleCastEntries[spellName] = new DoubleCastSpell(spellName, expiryTime, DateTime.UtcNow);
            if (!DoubleCastEntries.ContainsKey(spellName)) DoubleCastEntries.Add(spellName, new DoubleCastSpell(spellName, expiryTime, DateTime.UtcNow));
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

            public DateTime DoubleCastCurrentTime { get; private set; }

            internal double DoubleCastExpiryTime { get; set; }

            private string DoubleCastSpellName { get; set; }
        }

        #endregion Double Cast Shit

        #region BlanketSpell

        public static readonly Dictionary<string, BlanketSpell> BlanketSpellEntries = new Dictionary<string, BlanketSpell>();

        public static void OutputBlanketSpellEntries()
        {
            if (BlanketSpellEntries.Values.Count < 1) return;
            Logger.Output(" --> We have {0} targets blanketed for {1}", BlanketSpellEntries.Count, WoWSpell.FromId(BlanketSpellEntries.Values.FirstOrDefault().BlanketSpellName).Name);
        }

        internal static void PulseBlanketSpellEntries()
        {
            BlanketSpellEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.BlanketSpellCurrentTime).TotalSeconds >= t.BlanketSpellExpiryTime);
        }

        private static void UpdateBlanketSpellEntries(string key, int spellName, double expiryTime)
        {
            if (!BlanketSpellEntries.ContainsKey(key)) BlanketSpellEntries.Add(key, new BlanketSpell(key, spellName, expiryTime, DateTime.UtcNow));
        }

        internal struct BlanketSpell
        {
            public BlanketSpell(string key, int spellName, double expiryTime, DateTime currentTime)
                : this()
            {
                BlanketKey = key;
                BlanketSpellName = spellName;
                BlanketSpellExpiryTime = expiryTime;
                BlanketSpellCurrentTime = currentTime;
            }

            public DateTime BlanketSpellCurrentTime { get; private set; }

            public double BlanketSpellExpiryTime { get; set; }

            public int BlanketSpellName { get; set; }

            public string BlanketKey { get; set; }
        }

        #endregion BlanketSpell

        #region Cast

        public static Composite Cast(string spell, Selection<WoWUnit> onUnit = null)
        {
            return Cast(spell, onUnit, null, 0.5, true, false, false, true);
        }

        public static Composite Cast(string spell, Selection<WoWUnit> onUnit = null, Selection<bool> reqs = null)
        {
            return Cast(spell, onUnit, reqs, 0.5, true, false, false, true);
        }

        public static Composite Cast(string spell, Selection<WoWUnit> onUnit = null, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool clipChannel = false, bool checkrange = false)
        {
            return
                new Decorator(ret =>
                    {
                        if (reqs != null && !reqs(ret))
                            return false;

                        if (clipChannel && !CanClipChannel())
                            return false;

                        // To ignore CanCast stuff (CanCast returns false while channeling, useful to be ignored for dots)
                        if (!ignoreCanCast &&
                            !SpellManager.CanCast(spell, onUnit != null ? onUnit(ret) : StyxWoW.Me.CurrentTarget, checkrange))
                            return false;

                        // Check our doublecast entries for the spell
                        if (checkDoubleCast && onUnit != null && onUnit(ret) != null &&
                            DoubleCastEntries.ContainsKey(spell + onUnit(ret).GetHashCode()))
                            return false;

                        return true;
                    },
                    new Action(delegate(object ret)
                        {
                            WoWSpell castingSpell = StyxWoW.Me.CastingSpell;
                            // If we're casting something other than what we should be, stop casting it. (Channeling /stopcast stuff)
                            if (castingSpell != null && castingSpell.Name != spell)
                                Lua.DoString("SpellStopCasting()");

                            if (onUnit != null && onUnit(ret) != null)
                            {
                                LastTarget = onUnit(ret);
                                Logger.Output(String.Format(" [DIAG] Casting {0} on {1} at {2:F1} yds at {3:F1}%", spell, LastTarget.SafeName, LastTarget.Distance, LastTarget.HealthPercent(LastTarget.GetHPCheckType())));
                            }
                            

                            SpellManager.Cast(spell, ((onUnit != null && onUnit(ret) != null) ? onUnit(ret) : null));

                            if (onUnit != null && onUnit(ret) != null)
                                UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode(), expiryTime);

                            LastspellcastString = spell;
                        }));
        }

        public static Composite Cast(int spell, Selection<WoWUnit> onUnit = null, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool clipChannel = false)
        {
            return
                new Decorator(ret =>
                {
                    if (reqs != null && !reqs(ret))
                        return false;

                    if (clipChannel && !CanClipChannel())
                        return false;

                    // To ignore CanCast stuff (CanCast returns false while channeling, useful to be ignored for dots)
                    if (!ignoreCanCast &&
                        !SpellManager.CanCast(spell, onUnit != null ? onUnit(ret) : StyxWoW.Me.CurrentTarget))
                        return false;

                    // Check our doublecast entries for the spell
                    if (checkDoubleCast && onUnit != null && onUnit(ret) != null &&
                        DoubleCastEntries.ContainsKey(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode()))
                        return false;

                    return true;
                },
                    new Action(delegate(object ret)
                    {
                        WoWSpell castingSpell = StyxWoW.Me.CastingSpell;
                        // If we're casting something other than what we should be, stop casting it. (Channeling /stopcast stuff)
                        if (castingSpell != null && castingSpell.Id != spell)
                            Lua.DoString("SpellStopCasting()");

                        if (onUnit != null && onUnit(ret) != null)
                        {
                            LastTarget = onUnit(ret);
                            Logger.Output(String.Format(" [DIAG] Casting {0} on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, LastTarget.SafeName, LastTarget.Distance, LastTarget.HealthPercent(LastTarget.GetHPCheckType())));
                            UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + LastTarget.GetHashCode(), expiryTime);
                        }

                        SpellManager.Cast(spell, ((onUnit != null && onUnit(ret) != null) ? onUnit(ret) : null));

                        if (LastspellcastInt == RotationBase.ForceOfNature) ForceofNatureWaitTimer.Reset();

                        LastspellcastInt = spell;
                    }));
        }

        public static Composite CastBlanket(int spell, Selection<WoWUnit> onUnit = null, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool clipChannel = false)
        {
            return
                new Decorator(ret =>
                {
                    if (reqs != null && !reqs(ret))
                        return false;

                    if (clipChannel && !CanClipChannel())
                        return false;

                    // To ignore CanCast stuff (CanCast returns false while channeling, useful to be ignored for dots)
                    if (!ignoreCanCast &&
                        !SpellManager.CanCast(spell, onUnit != null ? onUnit(ret) : StyxWoW.Me.CurrentTarget))
                        return false;

                    // Check our doublecast entries for the spell
                    if (checkDoubleCast && onUnit != null && onUnit(ret) != null &&
                        DoubleCastEntries.ContainsKey(spell.ToString(CultureInfo.InvariantCulture) + onUnit(ret).GetHashCode()))
                        return false;

                    return true;
                },
                    new Action(delegate(object ret)
                    {
                        WoWSpell castingSpell = StyxWoW.Me.CastingSpell;
                        // If we're casting something other than what we should be, stop casting it. (Channeling /stopcast stuff)
                        if (castingSpell != null && castingSpell.Id != spell)
                            Lua.DoString("SpellStopCasting()");

                        if (onUnit != null && onUnit(ret) != null)
                        {
                            LastTarget = onUnit(ret);
                            Logger.Output(String.Format(" [DIAG] Casting {0} on {1} at {2:F1} yds at {3:F1}%", WoWSpell.FromId(spell).Name, LastTarget.SafeName, LastTarget.Distance, LastTarget.HealthPercent(LastTarget.GetHPCheckType())));
                            UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + LastTarget.GetHashCode(), expiryTime);
                        }

                        SpellManager.Cast(spell, ((onUnit != null && onUnit(ret) != null) ? onUnit(ret) : null));

                        if (spell == RotationBase.EternalFlame || spell == RotationBase.Rejuvenation) UpdateBlanketSpellEntries(spell.ToString(CultureInfo.InvariantCulture) + DateTime.UtcNow, spell, GetExpiryforBlanket(spell));

                        if (LastspellcastInt == RotationBase.ForceOfNature) ForceofNatureWaitTimer.Reset();

                        LastspellcastInt = spell;
                    }));
        }

        // returns the expiry time for each blanket spell.
        private static int GetExpiryforBlanket(int spell)
        {
            switch (spell)
            {
                case RotationBase.EternalFlame:
                    return 29;
                case RotationBase.Rejuvenation:
                    return 9;
            }
            return 999;
        }

        //// I think Efflorescence lasts around 6 seconds so wait for this time before we shoot out another.
        public static readonly WaitTimer ForceofNatureWaitTimer = new WaitTimer(TimeSpan.FromSeconds(OracleSettings.Instance.Druid.ForceOfNatureWaitTime));

        #endregion Cast

        #region Cast - Heal

        //public static Composite Heal(int spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        //{
        //    return new Decorator(ret => onUnit(ret).HealthPercent() <= HP, Cast(spell, onUnit, reqs));
        //}

        public static Composite Heal(string spell, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= HP, Cast(spell, on => HealTarget, reqs));
        }

        public static Composite Heal(string spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => (onUnit != null && onUnit(ret) != null && onUnit(ret).IsValid) && onUnit(ret).HealthPercent(onUnit(ret).GetHPCheckType()) <= HP, Cast(spell, onUnit, reqs));
        }

        public static Composite HoT(string spell, int HP = 100, Selection<bool> reqs = null, double expirytime = 0, bool checkDoubleCast = false)
        {
            return new Decorator(ret => OracleRoutine.IsViable(HealTarget) && !SpellAuras.HasMyAura(spell, HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= HP, Cast(spell, on => HealTarget, reqs, expirytime, checkDoubleCast));
        }

        public static Composite HoT(int spell, int HP = 100, Selection<bool> reqs = null, double expirytime = 0, bool checkDoubleCast = false)
        {
            return new Decorator(ret => OracleRoutine.IsViable(HealTarget) && !SpellAuras.HasMyAura(spell, HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= HP, Cast(spell, on => HealTarget, reqs, expirytime, checkDoubleCast));
        }

        public static Composite HoT(string spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => (onUnit != null && onUnit(ret) != null && onUnit(ret).IsValid) && !SpellAuras.HasMyAura(spell, onUnit(ret)) && onUnit(ret).HealthPercent(onUnit(ret).GetHPCheckType()) <= HP, Cast(spell, onUnit, reqs));
        }

        public static Composite HoT(int spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => (onUnit != null && onUnit(ret) != null && onUnit(ret).IsValid) && !SpellAuras.HasMyAura(spell, onUnit(ret)) && onUnit(ret).HealthPercent(onUnit(ret).GetHPCheckType()) <= HP, Cast(spell, onUnit, reqs));
        }

        public static Composite HoTBlanket(int spell, Selection<WoWUnit> onUnit, int HP = 100, Selection<bool> reqs = null)
        {
            return new Decorator(ret => (onUnit != null && onUnit(ret) != null && onUnit(ret).IsValid) && !SpellAuras.HasMyAura(spell, onUnit(ret)) && onUnit(ret).HealthPercent(onUnit(ret).GetHPCheckType()) <= HP, CastBlanket(spell, onUnit, reqs));
        }

        public static Composite HealGround(int spell, Selection<bool> reqs = null, bool ignoreCanCast = false, bool waitForSpell = false)
        {
            return new Decorator(ret => (!WoWSpell.FromId(spell).Cooldown), CastOnGround(spell, ret => Me.Location, reqs, 0, false, ignoreCanCast, waitForSpell));
        }

        #endregion Cast - Heal

        #region Cast on ground

        public static Composite CastOnGround(string spell, Selection<WoWPoint> onLocation, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool waitForSpell = false)
        {
            return
                new Decorator(ret =>
                    {
                        if (onLocation == null)
                            return false;

                        if (reqs != null && !reqs(ret))
                            return false;

                        // To ignore CanCast stuff (CanCast returns false while channeling, useful to be ignored for dots)
                        if (!ignoreCanCast &&
                            !SpellManager.CanCast(spell))
                            return false;

                        // Check our doublecast entries for the spell
                        if (checkDoubleCast &&
                            DoubleCastEntries.ContainsKey(spell + onLocation(ret)))
                            return false;

                        // Check Distance
                        //if (StyxWoW.Me.Location.Distance(onLocation(ret)) <= SpellManager.Spells[spell].MaxRange || SpellManager.Spells[spell].MaxRange == 0)
                        //  return false;

                        return true;
                    },
                new Sequence(
                    new Action(ret => Logger.Output(String.Format(" [DIAG] Casting on Ground {0} at {1:F1}", spell, onLocation(ret)))),
                    new Action(ret => SpellManager.Cast(spell)),
                    new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1,
                                ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Name == spell, new ActionAlwaysSucceed())),
                    new Action(ret => SpellManager.ClickRemoteLocation(onLocation(ret))),
                    new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onLocation(ret), expiryTime)),
                    new Action(ret => LastspellcastString = spell)
                    ));
        }

        public static Composite CastOnGround(int spell, Selection<WoWPoint> onLocation, Selection<bool> reqs = null, double expiryTime = 0, bool checkDoubleCast = false, bool ignoreCanCast = false, bool waitForSpell = false)
        {
            return
                new Decorator(ret =>
                {
                    if (onLocation == null)
                        return false;

                    if (reqs != null && !reqs(ret))
                        return false;

                    // To ignore CanCast stuff (CanCast returns false while channeling, useful to be ignored for dots)
                    if (!ignoreCanCast &&
                        !SpellManager.CanCast(spell))
                        return false;

                    // fix for HolyWordSanctuary/Lightwell
                    if (ignoreCanCast && WoWSpell.FromId(spell).Cooldown)
                        return false;

                    // Check our doublecast entries for the spell
                    if (checkDoubleCast &&
                        DoubleCastEntries.ContainsKey(spell.ToString(CultureInfo.InvariantCulture) + onLocation(ret)))
                        return false;

                    // Check Distance
                    // if (StyxWoW.Me.Location.Distance(onLocation(ret)) <= WoWSpell.FromId(spell).MaxRange || WoWSpell.FromId(spell).MaxRange == 0)
                    //return false;

                    return true;
                },
                new Sequence(
                    new Action(ret => Logger.Output(String.Format(" [DIAG] Casting on Ground {0} at {1:F1}", WoWSpell.FromId(spell).Name, onLocation(ret)))),
                    new Action(ret => SpellManager.Cast(spell)),
                //new Action(ret => Logger.Output(String.Format(" [DIAG] Current Pending spell is {0} ", StyxWoW.Me.CurrentPendingCursorSpell.Id))),
                    new DecoratorContinue(ctx => waitForSpell,
                            new WaitContinue(1,
                                ret =>
                                StyxWoW.Me.CurrentPendingCursorSpell != null &&
                                StyxWoW.Me.CurrentPendingCursorSpell.Id == spell, new ActionAlwaysSucceed())),
                    new Action(ret => SpellManager.ClickRemoteLocation(onLocation(ret))),
                //new Decorator(ret => StyxWoW.Me.HasPendingSpell("Wild Mushroom"), new Action(ret => Lua.DoString("SpellStopTargeting()"))),
                    new Action(ret => UpdateDoubleCastEntries(spell.ToString(CultureInfo.InvariantCulture) + onLocation(ret), expiryTime)),
                    new Action(ret => LastspellcastString = spell.ToString(CultureInfo.InvariantCulture))
                    ));
        }

        #endregion Cast on ground

        #region Spells - methods to handle Spells such as cooldowns

        // Not cached - Use CooldownTracker for performance.

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

        #endregion Spells - methods to handle Spells such as cooldowns
    }
}