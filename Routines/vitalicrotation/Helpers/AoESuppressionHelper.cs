using System;
using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Simple helper to manage AoE suppression for breakable CC protection
    /// </summary>
    public static class AoESuppressionHelper
    {
        private static bool _suppressAoeThisTick = false;
        private static DateTime _lastResetUtc = DateTime.MinValue;

        /// <summary>
        /// Decide if AoE should be suppressed this tick based on nearby friendly fragile CC.
        /// </summary>
        public static bool ShouldSuppressAoeThisTick()
        {
            try
            {
                // CC allié fragile proche ? (Incap/Disorient/Fear)
                bool ccNear = DRTracker.HasFriendlyHardCcNearby(AlliesProvider.GetAllies, 12f);
                return ccNear;
            }
            catch { return false; }
        }

        /// <summary>
        /// Mark that AoE abilities should be suppressed this tick
        /// </summary>
        public static void SuppressAoeThisTick()
        {
            _suppressAoeThisTick = true;
        }

        /// <summary>
        /// Check if AoE abilities should be suppressed this tick
        /// </summary>
        public static bool IsAoeSuppressed()
        {
            return _suppressAoeThisTick;
        }

        /// <summary>
        /// Reset the suppression flag (call at start of each tick)
        /// </summary>
        public static void ResetSuppressionFlag()
        {
            _suppressAoeThisTick = false;
            _lastResetUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Optional: milliseconds since the last reset (for diagnostics)
        /// </summary>
        public static double MsSinceReset
        {
            get { return (DateTime.UtcNow - _lastResetUtc).TotalMilliseconds; }
        }
    }

    /// <summary>
    /// Fournisseur d’alliés (adapter à la stack HB): yield WoWUnitLike for group/raid allies
    /// </summary>
    public static class AlliesProvider
    {
        public static IEnumerable<WoWUnitLike> GetAllies()
        {
            var result = new List<WoWUnitLike>();
            try
            {
                var me = StyxWoW.Me;
                if (me != null && me.GroupInfo != null && me.GroupInfo.RaidMembers != null)
                {
                    foreach (var m in me.GroupInfo.RaidMembers)
                    {
                        WoWPlayer p = null;
                        try { p = m.ToPlayer(); } catch { p = null; }
                        if (p == null || !p.IsValid || p.IsDead) continue;
                        string guidHex = string.Empty;
                        try { guidHex = p.Guid.ToString("X"); } catch { guidHex = string.Empty; }
                        float dist = 0f; try { dist = (float)p.Distance; } catch { dist = 0f; }
                        result.Add(new WoWUnitLike { Guid = guidHex, Distance = dist, IsFriendly = true });
                    }
                }
            }
            catch { }

            if (result.Count == 0)
            {
                try
                {
                    var me = StyxWoW.Me;
                    if (me != null)
                    {
                        string guidHex = string.Empty; try { guidHex = me.Guid.ToString("X"); } catch { }
                        float dist = 0f;
                        result.Add(new WoWUnitLike { Guid = guidHex, Distance = dist, IsFriendly = true });
                    }
                }
                catch { }
            }

            for (int i = 0; i < result.Count; i++)
                yield return result[i];
        }
    }
}