using System;
using System.Collections.Generic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Singular.Managers
{
    static class SpellImmunityManager
    {
        // This dictionary uses Unit.Entry as key and WoWSpellSchool as value.
        static readonly Dictionary<uint, WoWSpellSchool> ImmuneNpcs = new Dictionary<uint, WoWSpellSchool>();

        public static void Add(uint mobId, WoWSpellSchool school)
        {
            if (!ImmuneNpcs.ContainsKey(mobId))
            {
                ImmuneNpcs.Add(mobId, school);
            }
        }

        public static bool IsImmune(this WoWUnit unit, WoWSpellSchool school)
        {
            return unit != null && ImmuneNpcs.ContainsKey(unit.Entry) && (ImmuneNpcs[unit.Entry] & school) > 0;
        }

        /// <summary>
        /// Checks if a unit currently has complete damage immunity (e.g. Divine Shield, Ice Block, Cyclone, Deterrence, Banish).
        /// </summary>
        public static bool HasTotalDamageImmunity(this WoWUnit unit)
        {
            if (unit == null || !unit.IsValid)
                return false;

            try
            {
                var auras = unit.ActiveAuras;
                if (auras == null || auras.Count == 0)
                    return false;

                return auras.ContainsKey("Divine Shield")
                    || auras.ContainsKey("Ice Block")
                    || auras.ContainsKey("Cyclone")
                    || auras.ContainsKey("Deterrence")
                    || auras.ContainsKey("Banish");
            }
            catch
            {
                return false;
            }
        }
    }
}
