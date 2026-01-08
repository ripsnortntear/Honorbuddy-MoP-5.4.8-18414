using System;
using Styx;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    internal static class ShadowstepManager
    {
        private const int ThrottleMs = 200;
        private const string ThrottleKey = "Shadowstep.Try";

        // Legacy entry; delegate to MobilityManager safe method to centralize logic
        public static bool TryUse(WoWUnit target)
        {
            try
            {
                return MobilityManager.TryShadowstepSafe(target);
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "ShadowstepManager.TryUse");
                return false;
            }
        }
    }
}
