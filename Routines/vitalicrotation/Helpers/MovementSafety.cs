using Styx;
using Styx.WoWInternals.WoWObjects;
using System;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Helper class for movement and positioning safety checks
    /// </summary>
    public static class MovementSafety
    {
        /// <summary>
        /// Check if it's safe to use stationary burst abilities like Killing Spree
        /// </summary>
        public static bool IsSafeForStationaryBurst(float safetyRadius = 7.0f)
        {
            var me = StyxWoW.Me;
            if (me == null || !me.IsValid) return false;

            try
            {
                // Basic safety checks for stationary abilities
                
                // Check if we're not moving (or barely moving)
                if (me.IsMoving) return false;
                
                // Check if we're not in dangerous ground effects
                // (This would need more sophisticated detection in a real implementation)
                
                // Check if target is in range and not moving rapidly away
                var target = me.CurrentTarget as WoWUnit;
                if (target == null || !target.IsValid) return false;
                
                if (target.Distance > 5.0) return false;
                if (!target.InLineOfSpellSight) return false;
                
                // Check if we're not in a dangerous area (cliffs, etc.)
                // This is a simplified check - real implementation would be more sophisticated
                var myLocation = me.Location;
                if (myLocation.Z < -1000 || myLocation.Z > 1000) return false; // Basic sanity check
                
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Check if position is safe for channeled abilities
        /// </summary>
        public static bool IsSafeForChanneling()
        {
            var me = StyxWoW.Me;
            if (me == null || !me.IsValid) return false;

            try
            {
                // Don't channel if we're moving
                if (me.IsMoving) return false;
                
                // Don't channel if we're taking heavy damage
                if (me.HealthPercent < 30) return false;
                
                // Check for incoming dangerous casts on us
                // (This would need combat log analysis in real implementation)
                
                return true;
            }
            catch { return false; }
        }
    }
}