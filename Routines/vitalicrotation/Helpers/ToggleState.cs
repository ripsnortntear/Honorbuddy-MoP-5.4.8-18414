using System;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Central runtime toggle flags. (E11 naming unification)
    /// Legacy public fields retained; new preferred properties use IsX naming for clarity.
    /// </summary>
    internal static class ToggleState
    {
        // Core flags (legacy names kept)
        public static bool Burst;
        public static bool Lazy;
        public static bool Pause;
        public static bool PauseDamage;      // damage-only pause
        public static bool NoShadowBlades;   // disable Shadow Blades during Burst

        // Preferred unified property naming (use these moving forward)
        public static bool IsBurstOn { get { return Burst; } set { Burst = value; } }
        public static bool IsLazyOn { get { return Lazy; } set { Lazy = value; } }
        public static bool IsPaused { get { return Pause; } set { Pause = value; } }
        public static bool IsDamagePaused { get { return PauseDamage; } set { PauseDamage = value; } }
        public static bool IsNoShadowBlades { get { return NoShadowBlades; } set { NoShadowBlades = value; } }

        // Legacy property aliases (kept for compatibility with existing calls) – mark obsolete to encourage migration
        [Obsolete("Use IsBurstOn instead.")]
        public static bool BurstEnabled { get { return Burst; } set { Burst = value; } }
        [Obsolete("Use IsLazyOn instead.")]
        public static bool LazyMode { get { return Lazy; } set { Lazy = value; } }

        /// <summary>
        /// Initialize default toggle states (mirrors original defaults).
        /// </summary>
        public static void InitializeDefaults()
        {
            Burst = true;
            Lazy = true;       // ON at boot
            Pause = false;
            PauseDamage = false;
            NoShadowBlades = false;
        }

        /// <summary>
        /// Generic toggle by string key (lower-case). Accepts both new and legacy keys.
        /// </summary>
        public static bool Toggle(string which)
        {
            if (string.IsNullOrEmpty(which)) return false;
            string w = which.ToLowerInvariant();
            if (w == "burst") { Burst = !Burst; return Burst; }
            if (w == "lazy") { Lazy = !Lazy; return Lazy; }
            if (w == "pause") { Pause = !Pause; return Pause; }
            if (w == "pausedamage" || w == "damagepause") { PauseDamage = !PauseDamage; return PauseDamage; }
            if (w == "noshadowblades" || w == "noblades") { NoShadowBlades = !NoShadowBlades; return NoShadowBlades; }
            return false;
        }

        /// <summary>Explicit setter by key (lower-case).</summary>
        public static void Set(string which, bool state)
        {
            if (string.IsNullOrEmpty(which)) return;
            string w = which.ToLowerInvariant();
            if (w == "burst") Burst = state;
            else if (w == "lazy") Lazy = state;
            else if (w == "pause") Pause = state;
            else if (w == "pausedamage" || w == "damagepause") PauseDamage = state;
            else if (w == "noshadowblades" || w == "noblades") NoShadowBlades = state;
        }
    }
}
