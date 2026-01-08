using System;
using VitalicRotation.Settings;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    public static class MechanicsGuard
    {
        // Simplified: only expose basic immunity queries retained by callers.

        public static bool IsImmuneNow(WoWUnit target)
        {
            if (target == null || !target.IsValid) return false;
            try
            {
                var auras = target.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null || !a.IsActive) continue;
                    int id = 0; try { id = a.SpellId; } catch { }
                    if (id == 45438 || id == 642 || id == 710 || id == 33786) return true; // Ice Block / Divine Shield / Banish / Cyclone
                }
            }
            catch { }
            return false;
        }

        public static bool ShouldHoldMajorCooldowns(WoWUnit target)
        {
            // Hold logic removed (settings removed); always false
            return false;
        }

        public static string GetHoldReason(WoWUnit target)
        {
            return "Hold disabled";
        }

        public static void Initialize() { }
    }
}