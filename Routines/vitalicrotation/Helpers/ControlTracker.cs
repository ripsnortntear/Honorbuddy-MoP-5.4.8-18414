using System;
using Styx;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Settings;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Tracks the last time we APPLIED a short hard control (Cheap Shot, Kidney Shot, Paralytic stun) - parity with original Class32.
    /// Used for potential burst gating or spacing logic. (Implementation phase: tracker only; usage gating TBD.)
    /// </summary>
    internal static class ControlTracker
    {
        // MoP spellIds (original Class32): Cheap Shot (1833), Kidney Shot (408), Paralytic Poison stun (113953)
        private static readonly int[] ControlIds = { 1833, 408, 113953 };

        private static DateTime _lastAppliedUtc = DateTime.MinValue;
        private static int _lastSpellId;
        private static ulong _lastTargetGuid;

        /// <summary>UTC timestamp of last controlled stun we applied.</summary>
        public static DateTime LastAppliedUtc { get { return _lastAppliedUtc; } }
        /// <summary>SpellId of last control (0 if none yet).</summary>
        public static int LastSpellId { get { return _lastSpellId; } }
        /// <summary>Target GUID of last control (0 if unknown).</summary>
        public static ulong LastTargetGuid { get { return _lastTargetGuid; } }

        /// <summary>Seconds since last control (returns large value if never).</summary>
        public static double SecondsSinceLastControl()
        {
            if (_lastAppliedUtc == DateTime.MinValue) return 9999.0;
            return (DateTime.UtcNow - _lastAppliedUtc).TotalSeconds;
        }

        /// <summary>True if we applied any tracked control within the given milliseconds.</summary>
        public static bool WasRecent(int ms)
        {
            if (ms <= 0) ms = 1000;
            if (_lastAppliedUtc == DateTime.MinValue) return false;
            return (DateTime.UtcNow - _lastAppliedUtc).TotalMilliseconds <= ms;
        }

        /// <summary>Called from combat log when we successfully apply an aura.</summary>
        public static void MarkIfTracked(int spellId, ulong targetGuid)
        {
            // Fast path
            bool match = false;
            for (int i = 0; i < ControlIds.Length; i++)
                if (ControlIds[i] == spellId) { match = true; break; }
            if (!match) return;

            _lastAppliedUtc = DateTime.UtcNow;
            _lastSpellId = spellId;
            _lastTargetGuid = targetGuid;

            if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode)
            {
                try { Logger.Write("[Diag][Control] Applied {0} on {1:X} secsSincePrev={2:0.00}", spellId, targetGuid, SecondsSinceLastControl()); } catch { }
            }
        }

        /// <summary>
        /// Reset the tracker state (Phase 6 pruning on combat boundary).
        /// </summary>
        public static void Reset()
        {
            _lastAppliedUtc = DateTime.MinValue;
            _lastSpellId = 0;
            _lastTargetGuid = 0UL;
            if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode)
            {
                try { Logger.Write("[Diag][Control] Reset"); } catch { }
            }
        }
    }
}
