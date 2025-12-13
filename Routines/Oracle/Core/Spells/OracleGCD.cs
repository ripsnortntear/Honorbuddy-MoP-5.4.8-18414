using JetBrains.Annotations;
using Oracle.Classes;
using Oracle.Shared.Logging;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.Spells
{
    [UsedImplicitly]
    internal class OracleGCD
    {
        private static readonly HashSet<int> Gcdspells = new HashSet<int>
            {
                RotationBase.MassResurrection,
                RotationBase.InnerFire,
                RotationBase.StanceoftheFierceTiger,
                RotationBase.CatForm,
                RotationBase.LightningShield,
                RotationBase.RighteousFury,
                RotationBase.Wrath,
                RotationBase.SealofCommand,
                RotationBase.Smite,
                RotationBase.LightningBolt,
            };

        public static WoWSpell GcdSpell { get; set; }

        public static WoWSpell GetGlobalCooldownSpell
        {
            get
            {
                return SpellManager.Spells.FirstOrDefault(s => Gcdspells.Contains(s.Value.Id)).Value;
            }
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
                        var gcd = new TimeSpan(0, 0, 0, 0, (int)guidresults.ExpiryTime);
                        //Logger.Output(" [GCD] Cached Expiry {0}", gcd);
                        return gcd;
                    }
                    else
                    {
                        var gcd = GcdSpell.CooldownTimeLeft;
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

        public static void OutputOracleBlackListEntries()
        {
            if (GCDEntries.Values.Count < 1) return;
            Logger.Output(" --> We have {0} GCD Check {1}", GCDEntries.Count, GCDEntries.Values.FirstOrDefault().ExpiryTime);
        }

        internal static void PulseGCDCache()
        {
            GCDEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.CurrentTime).TotalMilliseconds >= t.ExpiryTime);
        }

        private static void UpdateGCDCache(int key, double expiryTime)
        {
            if (!GCDEntries.ContainsKey(key)) GCDEntries.Add(key, new GCDCache(key, expiryTime, DateTime.UtcNow));
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

            public int Key { get; set; }

            public double ExpiryTime { get; set; }
        }

        #endregion GCD Entries
    }
}