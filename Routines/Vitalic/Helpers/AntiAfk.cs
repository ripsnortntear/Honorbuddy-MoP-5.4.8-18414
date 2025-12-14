using System;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using VitalicRotation.Settings;

namespace VitalicRotation.Helpers
{
    public static class AntiAfk
    {
        // Throttle de sécurité
        private static DateTime _nextPulseUtc = DateTime.UtcNow.AddMinutes(5);
        private static readonly Random _rng = new Random();

        public static void StartIf(bool enabled)
        {
            if (!enabled) return;
            // Réinitialise une fenêtre pseudo-aléatoire 4–8 min
            _nextPulseUtc = DateTime.UtcNow.AddMinutes(_rng.Next(4, 9));
            Logger.Write("[AntiAFK] Activé (prochain tick ~{0} min)", (_nextPulseUtc - DateTime.UtcNow).TotalMinutes.ToString("0"));
        }

        public static void Stop()
        {
            _nextPulseUtc = DateTime.MaxValue;
        }

        /// <summary>
        /// Appeler régulièrement hors combat (ex. dans le tick OOC).
        /// Envoie un "mini-jump" Lua qui n'interrompt pas l'activité.
        /// </summary>
        public static void Pulse()
        {
            var S = VitalicSettings.Instance; // Use single AntiAFK flag
            if (!S.AntiAFK) return;

            if (DateTime.UtcNow < _nextPulseUtc) return;

            var me = StyxWoW.Me;
            if (me == null || !me.IsAlive) { _nextPulseUtc = DateTime.UtcNow.AddMinutes(2); return; }

            // Conditions de sûreté : pas en combat, pas de cast, pas de taxi/chargement
            if (me.Combat || me.IsCasting || me.OnTaxi || !StyxWoW.IsInWorld)
            {
                _nextPulseUtc = DateTime.UtcNow.AddMinutes(1);
                return;
            }

            // "mini-jump" très court via Lua (MoP 5.4 dispose de C_Timer)
            Lua.DoString("JumpOrAscendStart(); C_Timer.After(0.10, function() AscendStop() end)");
            Logger.Write("[AntiAFK] Jump envoyé.");

            // Replanifie la prochaine fenêtre 4–8 min
            _nextPulseUtc = DateTime.UtcNow.AddMinutes(_rng.Next(4, 9));
        }
    }
}

