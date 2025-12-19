using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;
using System.Threading;

namespace VitalicRotation.Helpers
{
    public static class Cast
    {
        /// <summary>
        /// Simple cast helper that uses SpellManager.Cast with basic validation.
        /// Returns true if the cast was successful.
        /// </summary>
        public static bool TryCast(string spellName)
        {
            try
            {
                if (string.IsNullOrEmpty(spellName)) return false;
                if (!SpellManager.CanCast(spellName)) return false;
                return SpellManager.Cast(spellName);
            }
            catch { return false; }
        }

        /// <summary>
        /// Cast spell on target with basic validation.
        /// </summary>
        public static bool TryCast(string spellName, WoWUnit target)
        {
            try
            {
                if (string.IsNullOrEmpty(spellName)) return false;
                if (target == null || !target.IsValid) return false;
                if (!SpellManager.CanCast(spellName, target)) return false;
                return SpellManager.Cast(spellName, target);
            }
            catch { return false; }
        }

        /// <summary>
        /// Simple Thread.Sleep wrapper for interrupt jitter (.NET 4.5.1 compatible).
        /// </summary>
        public static void Sleep(int milliseconds)
        {
            if (milliseconds > 0 && milliseconds < 5000) // sanity check
            {
                Thread.Sleep(milliseconds);
            }
        }
    }
}