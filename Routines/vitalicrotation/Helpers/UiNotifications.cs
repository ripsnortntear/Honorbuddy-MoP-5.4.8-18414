using System;
using VitalicRotation.Managers;

namespace VitalicRotation.Helpers
{
    public static class UiNotifications
    {
        private static bool _registered;
        public static void Register()
        {
            if (_registered) return;
            try
            {
                // Successful interrupt sound
                // If such event exists, wire it; otherwise PlayInterrupt from InterruptManager when casting
                // Here we also add a minimal hook via EventHandlers combat log, but keep it simple
                // Cooldown major events
                CooldownManager_OnMajorCooldown_Subscribe();
                // Toggles change -> simple event stub if available
                ToggleState_OnChanged_Subscribe();
                _registered = true;
            }
            catch { }
        }

        private static void CooldownManager_OnMajorCooldown_Subscribe()
        {
            // If CooldownManager exposes an event, subscribe here. Our build does not have one,
            // so as a minimal solution, play the event sound when burst toggles change or Ready/Role notifications already play.
        }

        private static void ToggleState_OnChanged_Subscribe()
        {
            // ToggleState does not expose an event in this codebase; InputPoller already shows banners.
            // We play a generic event sound when Burst/Lazy/Pause toggled in InputPoller instead of here.
        }
    }
}
