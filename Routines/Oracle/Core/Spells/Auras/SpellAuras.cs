#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Spells/Auras/SpellAuras.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Logging;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

//using Logger = Oracle.Shared.Logging.Logger;

namespace Oracle.Core.Spells.Auras
{
    internal static class SpellAuras
    {
        private static ulong MyGuid { get; set; }

        static SpellAuras()
        {
            // caching..cos we can.
            MyGuid = StyxWoW.Me.Guid;
        }

        #region Auras - methods to handle Auras/Buffs/Debuffs

        public static void CancelAura(this WoWUnit unit, string aura)
        {
            if (!OracleRoutine.IsViable(unit)) return;
            WoWAura a = unit.GetAuraFromName(aura);
            if (a != null && a.Cancellable)
                a.TryCancelAura();
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
            var result = StyxWoW.Me.GetAuraFromId(spellId);
            if (result != null)
            {
                if (result.StackCount > 0)
                    return result.StackCount;
            }

            return 0;
        }

        public static bool HasMyAura(string aura, WoWUnit u)
        {
            return OracleRoutine.IsViable(u) && u.GetAllAuras().Any(a => a.Name == aura && a.CreatorGuid == MyGuid);
        }

        public static bool HasMyAura(int aura, WoWUnit u)
        {
            return OracleRoutine.IsViable(u) && u.GetAllAuras().Any(a => a.SpellId == aura && a.CreatorGuid == MyGuid);
        }

        public static uint StackCount(WoWUnit unit, string aura)
        {
            {
                if (OracleRoutine.IsViable(unit))
                {
                    WoWAura result = unit.GetAllAuras().FirstOrDefault(a => a.Name == aura && a.StackCount > 0);
                    if (result != null) return result.StackCount;
                }
                return 0;
            }
        }

        internal static float AuraAmount(this WoWUnit unit, int aura, int index, bool isMyAura = false)
        {
            if (!OracleRoutine.IsViable(unit)) return 0;

            var result = unit.GetAuraFromId(aura, isMyAura);

            if (result == null)
                return 0;

            return result.VariableEffects[index];
        }

        #endregion Auras - methods to handle Auras/Buffs/Debuffs

        #region HasAura - Internal Extenstions

        /// <summary>
        /// Gets an Aura by Name. Note: this is a fix for the HB API wraper GetAuraByName
        /// </summary>
        public static WoWAura GetAuraFromName(this WoWUnit unit, string aura, bool isMyAura = false)
        {
            return isMyAura ? unit.GetAllAuras().FirstOrDefault(a => a.Name == aura && a.CreatorGuid == MyGuid && a.TimeLeft > TimeSpan.Zero) : unit.GetAllAuras().FirstOrDefault(a => a.Name == aura && a.TimeLeft > TimeSpan.Zero);
        }

        /// <summary>
        /// Gets an Aura by ID. Note: this is a fix for the HB API wraper GetAuraById
        /// </summary>
        public static WoWAura GetAuraFromId(this WoWUnit unit, int aura, bool isMyAura = false)
        {
            return isMyAura ? unit.GetAllAuras().FirstOrDefault(a => a.SpellId == aura && a.CreatorGuid == MyGuid && a.TimeLeft > TimeSpan.Zero) : unit.GetAllAuras().FirstOrDefault(a => a.SpellId == aura && a.TimeLeft > TimeSpan.Zero);
        }

        public static bool HasAura(this WoWUnit unit, string aura, int stacks)
        {
            return HasAura(unit, aura, stacks, null);
        }

        public static bool HasAura(this WoWUnit unit, string aura, int stacks, WoWUnit creator)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            return unit.GetAllAuras().Any(a => a.Name == aura && a.StackCount >= stacks && (creator == null || a.CreatorGuid == creator.Guid));
        }

        public static bool HasAura(this WoWUnit unit, string aura, int stacks = 0, bool isMyAura = false, int msLeft = 0)
        {
            if (!OracleRoutine.IsViable(unit)) return false;

            var result = unit.GetAuraFromName(aura, isMyAura);

            if (result == null)
                return false;

            //Logger.Output("{0} -- {1} -- {2}ms -- {3}stacks", result.SpellId, result.Name, result.TimeLeft.TotalMilliseconds, result.StackCount);

            if (result.TimeLeft.TotalMilliseconds > msLeft)
                return result.StackCount >= stacks;

            return false;
        }

        /// <summary> Here until HB fixs Aura detection for 5.4 </summary>
        public static bool HasAuraOveride(this WoWUnit unit, int id)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var result = unit.GetAllAuras().FirstOrDefault(a => a.SpellId == id);

            return result != null;
        }

        /// <summary> Here until HB fixs Aura detection for 5.4 </summary>
        public static bool HasAuraOveride(this WoWUnit unit, string name)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var result = unit.GetAllAuras().FirstOrDefault(a => a.Name == name);

            return result != null;
        }

        public static bool HasAura(this WoWUnit unit, int aura, int stacks = 0, bool isMyAura = false, int msLeft = 0)
        {
            if (!OracleRoutine.IsViable(unit)) return false;

            var result = unit.GetAuraFromId(aura, isMyAura);

            if (result == null)
                return false;

            //Logger.Output("{0} -- {1} -- {2}ms -- {3}stacks", result.SpellId, result.Name, result.TimeLeft.TotalMilliseconds, result.StackCount);

            if (result.TimeLeft.TotalMilliseconds > msLeft)
                return result.StackCount >= stacks;

            return false;
        }

        public static bool HasMyAura(this WoWUnit unit, string aura)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            return unit.GetAllAuras().Any(a => a.Name == aura && a.CreatorGuid == MyGuid);
        }

        public static bool HasMyAura(this WoWUnit unit, int spellId)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            return unit.GetAllAuras().Any(a => a.SpellId == spellId && a.CreatorGuid == MyGuid);
        }

        public static bool HasAnyAura(this WoWUnit unit, params string[] auraNames)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var auras = unit.GetAllAuras();
            var hashes = new HashSet<string>(auraNames);
            return auras.Any(a => hashes.Contains(a.Name));
        }

        public static bool HasAnyAura(this WoWUnit unit, params int[] auraIDs)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            return auraIDs.Any(unit.HasAura);
        }

        public static bool HasAnyAura(this WoWUnit unit, HashSet<int> auraIDs)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var auras = unit.GetAllAuras();
            return auras.Any(a => auraIDs.Contains(a.SpellId));
        }

        public static bool HasAnyAura(this WoWUnit unit, HashSet<string> auraIDs)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var auras = unit.GetAllAuras();
            return auras.Any(a => auraIDs.Contains(a.Name));
        }

        public static IEnumerable<SpellEffect> GetSpellEffects(this WoWSpell spell)
        {
            var effects = new SpellEffect[3];
            effects[0] = spell.GetSpellEffect(0);
            effects[1] = spell.GetSpellEffect(1);
            effects[2] = spell.GetSpellEffect(2);
            return effects;
        }

        public static bool HasAuraWithEffect(this WoWUnit unit, WoWApplyAuraType applyType)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            return unit.Auras.Values.Any(a => a.Spell != null && a.Spell.SpellEffects.Any(se => applyType == se.AuraType));
        }

        public static bool HasAuraWithEffect(this WoWUnit unit, WoWApplyAuraType auraType, int miscValue, int basePointsMin, int basePointsMax)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
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

        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWSpellMechanic[] mechanics)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var auras = unit.GetAllAuras();
            return auras.Any(a => mechanics.Contains(a.Spell.Mechanic));
        }

        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWApplyAuraType[] applyType)
        {
            if (!OracleRoutine.IsViable(unit)) return false;
            var auras = unit.GetAllAuras();
            return auras.Any(a => a.Spell.SpellEffects.Any(se => applyType.Contains(se.AuraType)));
        }

        #endregion HasAura - Internal Extenstions
    }
}