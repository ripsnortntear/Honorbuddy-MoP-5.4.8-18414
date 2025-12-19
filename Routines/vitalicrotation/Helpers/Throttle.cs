using System;
using System.Collections.Generic;

namespace VitalicRotation.Helpers
{
    internal static class Throttle
    {
        private static readonly Dictionary<string, DateTime> Throttles = new Dictionary<string, DateTime>();

        public static bool Check(string id, int ms)
        {
            DateTime now = DateTime.UtcNow;

            if (Throttles.ContainsKey(id))
            {
                double elapsed = (now - Throttles[id]).TotalMilliseconds;
                if (elapsed < ms)
                    return false;
            }

            Throttles[id] = now;
            return true;
        }

        /// <summary>
        /// Marque un identifiant comme utilisé "maintenant",
        /// sans vérification de délai (équivalent Vitalic).
        /// </summary>
        public static void Mark(string id)
        {
            Throttles[id] = DateTime.UtcNow;
        }
        public static bool Pass(string id, int ms)
        {
            // Alias de compatibilité vers Check(id, ms)
            return Check(id, ms);
        }


    }
}
