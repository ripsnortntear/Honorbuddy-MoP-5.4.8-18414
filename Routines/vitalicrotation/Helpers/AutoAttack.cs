using Styx;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Utility for managing auto-attack state like original Class77.smethod_8/smethod_9
    /// </summary>
    internal static class AutoAttack
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        /// <summary>
        /// Start auto-attack if not already attacking (Class77.smethod_8)
        /// </summary>
        public static void Start()
        {
            try
            {
                if (Me == null || !Me.IsValid) return;
                if (Me.IsAutoAttacking) return;
                LuaHelper.Do("StartAttack()");
            }
            catch { }
        }

        /// <summary>
        /// Stop auto-attack and cancel queued spells (Class77.smethod_9)
        /// </summary>
        public static void Stop()
        {
            try
            {
                if (Me == null || !Me.IsValid) return;
                if (!Me.IsAutoAttacking) return;
                LuaHelper.Do("SpellCancelQueuedSpell(); StopAttack();");
            }
            catch { }
        }
    }
}