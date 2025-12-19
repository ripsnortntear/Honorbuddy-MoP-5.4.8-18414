using System;
using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Managers
{
    internal static class TotemRecentCache
    {
        private static readonly Dictionary<ulong, DateTime> _seen = new Dictionary<ulong, DateTime>();
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(5);

        public static bool IsRecent(WoWUnit u)
        {
            if (u == null || !u.IsValid) return false;
            DateTime t;
            if (_seen.TryGetValue(u.Guid, out t))
            {
                if ((DateTime.UtcNow - t) <= Ttl)
                    return true;
                _seen.Remove(u.Guid);
                return false;
            }
            return false;
        }

        public static void Mark(WoWUnit u)
        {
            if (u == null || !u.IsValid) return;
            _seen[u.Guid] = DateTime.UtcNow;
        }

        public static void Prune()
        {
            var now = DateTime.UtcNow;
            var keys = new List<ulong>(_seen.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                var k = keys[i];
                if ((now - _seen[k]) > Ttl) _seen.Remove(k);
            }
        }
    }
}
