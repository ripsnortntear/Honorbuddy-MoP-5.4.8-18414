using System;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using VitalicRotation.Settings;

namespace VitalicRotation.Helpers
{
    public static class AntiAfk
    {
        // Safety throttle
        private static DateTime _nextPulseUtc = DateTime.UtcNow.AddMinutes(5);
        private static readonly Random _rng = new Random();

        public static void StartIf(bool enabled)
        {
            if (!enabled) return;
            // Resets a pseudo-random window of 4–8 minutes.
            _nextPulseUtc = DateTime.UtcNow.AddMinutes(_rng.Next(4, 9));
            Logger.Write("[AntiAFK] Activé (prochain tick ~{0} min)", (_nextPulseUtc - DateTime.UtcNow).TotalMinutes.ToString("0"));
        }

        public static void Stop()
        {
            _nextPulseUtc = DateTime.MaxValue;
        }

        /// <summary>
        /// Call regularly outside of combat (e.g., in the OOC tick).
        /// Sends a "mini-jump" Lua command that does not interrupt activity.
        /// </summary>
        public static void Pulse()
        {
            var S = VitalicSettings.Instance; // Use single AntiAFK flag
            if (!S.AntiAFK) return;

            if (DateTime.UtcNow < _nextPulseUtc) return;

            var me = StyxWoW.Me;
            if (me == null || !me.IsAlive) { _nextPulseUtc = DateTime.UtcNow.AddMinutes(2); return; }

            // Safety conditions: not in combat, no spellcasting, no taxi/loading.
            if (me.Combat || me.IsCasting || me.OnTaxi || !StyxWoW.IsInWorld)
            {
                _nextPulseUtc = DateTime.UtcNow.AddMinutes(1);
                return;
            }

            // A very short "mini-jump" via Lua (MoP 5.4 has C_Timer)
            Lua.DoString("JumpOrAscendStart(); C_Timer.After(0.10, function() AscendStop() end)");
            Logger.Write("[AntiAFK] Jump command sent.");

            // Reschedule the next window to 4–8 minutes.
            _nextPulseUtc = DateTime.UtcNow.AddMinutes(_rng.Next(4, 9));
        }
    }
}

