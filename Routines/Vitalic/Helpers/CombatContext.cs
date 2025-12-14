using System;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    public static class CombatContext
    {
        private static bool _inCombat;
        private static DateTime _lastStateChange = DateTime.MinValue;

        public static bool InCombat { get { return _inCombat; } }
        public static DateTime LastStateChangeUtc { get { return _lastStateChange; } }

        public static void Update()
        {
            try
            {
                if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                    return;

                var me = StyxWoW.Me;
                bool now = (me != null && me.Combat);
                if (now != _inCombat)
                {
                    _inCombat = now;
                    _lastStateChange = DateTime.UtcNow;

                    // Pruning / resets at boundaries (Phase 6)
                    try { DRTracker.PruneExpired(); } catch { }
                    try { DangerTracker.Reset(); } catch { }
                    try { ControlTracker.Reset(); } catch { }
                    try { LosFacingCache.PruneExpired(); } catch { } // Prune caches on combat state change
                    if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    {
                        try { Logger.Write("[Diag][Context] Combat state -> {0}", now ? "ENTER" : "EXIT"); } catch { }
                    }
                }
            }
            catch { }
        }
    }
}
