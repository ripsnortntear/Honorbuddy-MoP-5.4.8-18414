using System;

namespace VitalicRotation.Managers
{
    // Mince wrapper d’enregistrement (parité v.zip)
    // Dans cette implémentation, le routage CombatLog -> fenêtres défensives
    // est déjà géré par Helpers.EventHandlers. Ce registre évite tout double-hook.
    public static class ThreatEvents
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered) return;
            // Rien à faire ici: EventHandlers.Initialize() attache déjà COMBAT_LOG_EVENT_UNFILTERED
            // et arme DefensivesManager via ArmDefensiveThreat.
            _registered = true;
        }

        public static void Unregister()
        {
            _registered = false;
        }
    }
}
