#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Managers/DispelManager.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using Oracle.Core.DataStores;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Core.Spells.Debuffs;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Core.Managers
{
    //Credit to Singular developers for 95% of this class -- wulf.


    /// <summary>Bitfield of flags for specifying DispelCapabilities.</summary>
    /// <remarks>Created 5/3/2011.</remarks>
    [Flags]
    public enum DispelCapabilities
    {
        None = 0,
        Curse = 1,
        Disease = 2,
        Poison = 4,
        Magic = 8,
        All = Curse | Disease | Poison | Magic
    }

    public enum DispelType
    {
        None = 0,
        Priority,
        Delay,
        Stack,
        Range,
        BlackList
    }

    public enum DispelDelayType
    {
        None = 0,
        CountingUp,
        CountingDown
    }

    internal static class DispelManager
    {
        #region settings

        private static OracleSettings Setting { get { return OracleSettings.Instance; } }

        private static bool DispelDebuffs { get { return Setting.DispelDebuffs; } }

        private static double DispelManaPct { get { return Setting.DispelManaPct; } }

        

        #endregion settings


        #region Helpers

        // SetField Mask = flg;
        // SetOn Mask |= flg;
        // SetOff Mask &= ~flg;
        // SetToggle Mask &= ~flg;
        // AnyOn (Mask & flg) != 0;
        // AllOn (Mask & flg) == flg;
        // IsEqual Mask == flg;

        #endregion Helpers

        public static DispelCapabilities _cachedCapabilities = DispelCapabilities.None;

        public static void Initialize()
        {
            _cachedCapabilities = Capabilities;
            Logger.Output(" DispelManager DispelCapabilities: {0}", _cachedCapabilities);
        }

        /// <summary>Gets the dispel capabilities of the current player.</summary>
        /// <value>The capabilities.</value>
        public static DispelCapabilities Capabilities
        {
            get
            {
                var ret = DispelCapabilities.None;
                if (CanDispelCurse)
                {
                    ret |= DispelCapabilities.Curse;
                }
                if (CanDispelMagic)
                {
                    ret |= DispelCapabilities.Magic;
                }
                if (CanDispelPoison)
                {
                    ret |= DispelCapabilities.Poison;
                }
                if (CanDispelDisease)
                {
                    ret |= DispelCapabilities.Disease;
                }

                return ret;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel diseases.</summary>
        /// <value>true if we can dispel diseases, false if not.</value>
        public static bool CanDispelDisease
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Paladin:
                        return true;
                    case WoWClass.Monk:
                        return true;
                    case WoWClass.Priest:
                        return true;
                }
                return false;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel poison.</summary>
        /// <value>true if we can dispel poison, false if not.</value>
        public static bool CanDispelPoison
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Druid:
                        return true;
                    case WoWClass.Paladin:
                        return true;
                    case WoWClass.Monk:
                        return true;
                }
                return false;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel curses.</summary>
        /// <value>true if we can dispel curses, false if not.</value>
        public static bool CanDispelCurse
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Druid:
                        return true;
                    case WoWClass.Shaman:
                        return true;
                    case WoWClass.Mage:
                        return true;
                }
                return false;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel magic.</summary>
        /// <value>true if we can dispel magic, false if not.</value>
        public static bool CanDispelMagic
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Druid:
                        return StyxWoW.Me.Specialization == WoWSpec.DruidRestoration;
                    case WoWClass.Paladin:
                        return StyxWoW.Me.Specialization == WoWSpec.PaladinHoly;
                    case WoWClass.Shaman:
                        return true;
                    case WoWClass.Priest:
                        return true;
                    case WoWClass.Monk:
                        return StyxWoW.Me.Specialization == WoWSpec.MonkMistweaver;
                }
                return false;
            }
        }

        /// <summary>Gets a dispellable types on unit. </summary>
        /// <remarks>Created 5/3/2011.</remarks>
        /// <param name="unit">The unit.</param>
        /// <param name="msLeft"></param>
        /// <param name="stackcount"></param>
        /// <returns>The dispellable types on unit.</returns>
        public static DispelCapabilities GetDispellableTypesOnUnit(WoWUnit unit, int msLeft = 3000, int stackcount = 0)
        {
            int count = 0;

            var ret = DispelCapabilities.None;
            var dispelDelayType = DispelDelayType.None;
            var debuffName = "Nothing";
            var debuffid = 0;
            if (OracleRoutine.IsViable(unit))
                foreach (var debuff in unit.GetAllAuras().Where(a => a.IsHarmful && (a.Duration > 0 || a.SpellId == 144351)))
                {
                    count++;
                    switch (debuff.Spell.DispelType)
                    {
                        case WoWDispelType.Magic:
                            ret |= DispelCapabilities.Magic;
                            debuffName = debuff.Name;
                            debuffid = debuff.SpellId;
                            break;

                        case WoWDispelType.Curse:
                            ret |= DispelCapabilities.Curse;
                            debuffName = debuff.Name;
                            debuffid = debuff.SpellId;
                            break;

                        case WoWDispelType.Disease:
                            ret |= DispelCapabilities.Disease;
                            debuffName = debuff.Name;
                            debuffid = debuff.SpellId;
                            break;

                        case WoWDispelType.Poison:
                            ret |= DispelCapabilities.Poison;
                            debuffName = debuff.Name;
                            debuffid = debuff.SpellId;
                            break;
                    }

                    if (ret != DispelCapabilities.None)
                        Logger.Dispel(String.Format("[Dispel] Found {1} with {0} debuff called {2} (Spell ID: {3})", ret, unit.SafeName, debuffName, debuffid));

                    // Mark of Arrogance and Sha of Doubt -  check we have Gift of the Titans
                    if (debuffid == 144351 && !StyxWoW.Me.HasAnyAura(HashSets.GiftoftheTitansBuff)) //Mark of Arrogance(144351) - Gift of the Titans(144363)
                    {
                        ret = DispelCapabilities.None;
                        return ret;
                    }

                    var result = DispelableSpell.Instance.SpellList.Spells.FirstOrDefault(d => d.Id == debuff.SpellId);

                    if (result != null)
                    {
                        dispelDelayType = result.DisDelayType;

                        switch (result.DisType)
                        {
                            case DispelType.Priority:
                                Logger.Dispel("[Dispel - Priority] {0}", result.Name);
                                return ret;

                            case DispelType.Delay:
                                Logger.Dispel("[Dispel - Delay ({1})] {0}", result.Name, result.DisDelayType);
                                msLeft = result.Delay;
                                break;

                            case DispelType.Stack:
                                Logger.Dispel("[Dispel - Stack] {0}", result.Name);
                                stackcount = result.StackCount;
                                break;

                            case DispelType.Range:
                                Logger.Dispel("[Dispel - Range] {0}", result.Name);
                                if (unit.SafeRange(result.Range))
                                {
                                    return ret;
                                }
                                break;

                            case DispelType.BlackList:
                                Logger.Dispel("[BlackList Dispel] {0}", result.Name);
                                ret = DispelCapabilities.None;
                                return ret;
                        }
                    }

                    if (StyxWoW.Me.ManaPercent <= DispelManaPct) // lets not go OOM
                    {
                        ret = DispelCapabilities.None;
                        return ret;
                    }

                    if (ret != DispelCapabilities.None)
                        Logger.Dispel(String.Format("[Dispel] Found {1} with {0} debuff called {2} (Spell ID: {3}) [STACK: {4:f1} [{5}]]", ret, unit.SafeName, debuff.Name, debuffid, debuff.StackCount, stackcount));

                    if (stackcount != 0 && debuff.StackCount <= stackcount)
                        ret = DispelCapabilities.None;

                    if (ret != DispelCapabilities.None)
                        Logger.Dispel(String.Format("[Dispel] Found {1} with {0} debuff called {2} (Spell ID: {3}) [TIMELEFT: {4:f1} ms [{5}]]", ret, unit.SafeName, debuff.Name, debuffid, debuff.TimeLeft.TotalMilliseconds, msLeft));

                    switch (dispelDelayType)
                    {
                        case DispelDelayType.None:
                        case DispelDelayType.CountingDown:
                            if (debuff.Duration > 0 && debuff.TimeLeft.TotalMilliseconds > msLeft)
                                ret = DispelCapabilities.None;
                            break;

                        case DispelDelayType.CountingUp:
                            if (debuff.Duration > 0 && debuff.TimeLeft.TotalMilliseconds <= msLeft)
                                ret = DispelCapabilities.None;
                            break;
                    }
                }
            return ret;
        }

        /// <summary>Queries if we can dispel unit 'unit'. </summary>
        /// <remarks>Created 5/3/2011.</remarks>
        /// <param name="unit">The unit.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool CanDispel(WoWUnit unit)
        {
            return CanDispel(unit, _cachedCapabilities);
        }

        public static bool CanDispel(WoWUnit unit, DispelCapabilities chk)
        {
            return OracleRoutine.IsViable(unit) && (chk & GetDispellableTypesOnUnit(unit)) != 0;
        }

        private static WoWUnit _unitDispel;

        public static Composite CreateDispelBehavior()
        {
            var prio = new PrioritySelector();

           switch (StyxWoW.Me.Class)
            {
                case WoWClass.Paladin:
                    prio.AddChild(Spell.Cast("Cleanse", on => _unitDispel, ret => OracleRoutine.IsViable(_unitDispel), 0, false, false, false));
                    break;

                case WoWClass.Monk:
                    prio.AddChild(Spell.Cast("Detox", on => _unitDispel, ret => OracleRoutine.IsViable(_unitDispel), 0, false, false, false));
                    break;

                case WoWClass.Priest:
                    if (StyxWoW.Me.Specialization == WoWSpec.PriestHoly ||
                        StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                        prio.AddChild(Spell.Cast("Purify", on => _unitDispel, ret => OracleRoutine.IsViable(_unitDispel), 0, false, false, false));

                    break;

                case WoWClass.Druid:
                    prio.AddChild(Spell.Cast("Nature's Cure", on => _unitDispel, ret => OracleRoutine.IsViable(_unitDispel), 0, false, false, false));
                    break;

                case WoWClass.Shaman:
                    prio.AddChild(StyxWoW.Me.Specialization == WoWSpec.ShamanRestoration
                                      ? Spell.Cast("Purify Spirit", on => _unitDispel, ret => OracleRoutine.IsViable(_unitDispel), 0, false, false, false)
                                      : Spell.Cast("Cleanse Spirit", on => _unitDispel, ret => OracleRoutine.IsViable(_unitDispel), 0, false, false, false));
                    break;
            }

            return new Sequence(
                new DecoratorContinue(ret => !DispelDebuffs, new ActionAlwaysFail()),
                // Just grab the HealingPriorities list its going to have pruned out the shit players and prioritised the list for us already, now its just Dispel time, after that we ActionAlwaysFail to hit them with a heal if needed!
                new Action(ret => _unitDispel = Unit.FriendlyPriorities.Count > 0 ? Unit.FriendlyPriorities.FirstOrDefault(CanDispel) : null),
                new PrioritySelector(new Decorator(ret => _unitDispel == null, new ActionAlwaysSucceed()), prio), // succeed if that shit be null..why pass null to the cast method ??
                new ActionAlwaysFail()); // Continue on Guv..lika baws
        }

        private static bool SafeRange(this WoWUnit unit, float range)
        {
            if (OracleRoutine.IsViable(unit))
            {
                var centerpoint = unit.Location;
                if (OracleHealTargeting.HealingPriorities.Count(u => u.Location.DistanceSqr(centerpoint) < range * range) <= 0)
                {
                    Logger.Dispel(String.Format("--: Player {0} at {1:F1} yds with {2:F1}% is ok to Dispel", unit.SafeName, unit.Distance, unit.HealthPercent(unit.GetHPCheckType())));
                    return true;
                }
            }
            return false;
        }
    }
}