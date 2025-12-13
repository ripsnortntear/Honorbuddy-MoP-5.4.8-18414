#region

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;

#endregion

namespace Superbad
{
    internal class GCD
    {
        private static readonly HashSet<int> Gcdspells = new HashSet<int>
        {
            768
        };

        public static WoWSpell GcdSpell { get; set; }

        public static WoWSpell GetGlobalCooldownSpell
        {
            get { return SpellManager.Spells.FirstOrDefault(s => Gcdspells.Contains(s.Value.Id)).Value; }
        }

        public static TimeSpan GlobalCooldownLeft
        {
            get
            {
                if (GcdSpell != null)
                {
                    GCDCache guidresults;
                    if (GCDEntries.TryGetValue(123456789, out guidresults))
                    {
                        var gcd = new TimeSpan(0, 0, 0, 0, (int) guidresults.ExpiryTime);
                        //Logger.Output(" [GCD] Cached Expiry {0}", gcd);
                        return gcd;
                    }
                    else
                    {
                        TimeSpan gcd = GcdSpell.CooldownTimeLeft;
                        //Logger.Output(" [GCD] ::REAL:: Expiry {0}", gcd.TotalMilliseconds);
                        UpdateGCDCache(123456789, gcd.TotalMilliseconds);
                        return gcd;
                    }
                }
                return SpellManager.GlobalCooldownLeft;
            }
        }

        #region GCD Entries

        private static readonly Dictionary<int, GCDCache> GCDEntries = new Dictionary<int, GCDCache>();

        internal static void PulseGCDCache()
        {
            GCDEntries.RemoveAll(t => DateTime.Now.Subtract(t.CurrentTime).TotalMilliseconds >= t.ExpiryTime);
        }

        private static void UpdateGCDCache(int key, double expiryTime)
        {
            if (!GCDEntries.ContainsKey(key)) GCDEntries.Add(key, new GCDCache(key, expiryTime, DateTime.Now));
        }

        private struct GCDCache
        {
            public GCDCache(int key, double expiryTime, DateTime currentTime)
                : this()
            {
                Key = key;
                ExpiryTime = expiryTime;
                CurrentTime = currentTime;
            }

            public DateTime CurrentTime { get; private set; }

            private int Key { [UsedImplicitly] get; set; }

            public double ExpiryTime { get; private set; }
        }

        #endregion GCD Entries
    }
}