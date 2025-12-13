#region

using System;
using System.Collections.Generic;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;

#endregion

namespace Superbad
{
    internal static class CooldownTracker
    {
        public static readonly Dictionary<ulong, SpellCooldown> SpellCooldownEntries =
            new Dictionary<ulong, SpellCooldown>();

        static CooldownTracker()
        {
            MeGuid = StyxWoW.Me.Guid;
            SpellCooldownEntries = new Dictionary<ulong, SpellCooldown>();
        }

        private static ulong MeGuid { get; set; }

        internal static double GetSpellCooldownTimeLeft(int spell)
        {
            ulong guid = MeGuid + (ulong) spell;

            SpellCooldown results;
            string spellName = WoWSpell.FromId(spell).Name;
            if (!SpellCooldownEntries.TryGetValue(guid, out results))
            {
                double left = BaseCooldown(spell);
                if (left > 0)
                {
                    UpdateSpellCooldownEntries(guid, spellName, left);
                }
                return left;
            }

            double msleft = 0;
            if (DateTime.Now.Subtract(results.SpellCooldownCurrentTime).TotalSeconds >=
                results.SpellCooldownExpiryTime)
                msleft = 0;
            if (DateTime.Now.Subtract(results.SpellCooldownCurrentTime).TotalSeconds <
                results.SpellCooldownExpiryTime)
                msleft = results.SpellCooldownExpiryTime -
                         (DateTime.Now.Subtract(results.SpellCooldownCurrentTime).TotalSeconds);

            return msleft;
        }

        private static double BaseCooldown(int spell)
        {
            SpellFindResults results;
            if (!SpellManager.FindSpell(spell, out results)) return Double.MaxValue;

            return results.Override != null
                ? results.Override.CooldownTimeLeft.TotalSeconds
                : results.Original.CooldownTimeLeft.TotalSeconds;
        }

        internal static void PulseSpellCooldownEntries()
        {
            SpellCooldownEntries.RemoveAll(
                t => DateTime.Now.Subtract(t.SpellCooldownCurrentTime).TotalSeconds >= t.SpellCooldownExpiryTime);

            SpellCooldownEntries.RemoveAll(
                t => DateTime.Now.Subtract(t.SpellCooldownCurrentTime).TotalSeconds >= t.SpellCooldownExpiryTime);
        }

        public static void UpdateSpellCooldownEntries(ulong key, string name, double expiryTime)
        {
            if (!SpellCooldownEntries.ContainsKey(key))
                SpellCooldownEntries.Add(key, new SpellCooldown(key, name, expiryTime, DateTime.Now));
        }

        public struct SpellCooldown
        {
            public SpellCooldown(ulong key, string name, double expiryTime, DateTime currentTime)
                : this()
            {
                SpellCooldownKey = key;
                SpellCooldownName = name;
                SpellCooldownExpiryTime = expiryTime;
                SpellCooldownCurrentTime = currentTime;
            }

            public DateTime SpellCooldownCurrentTime { get; private set; }

            public double SpellCooldownExpiryTime { get; set; }

            public ulong SpellCooldownKey { get; set; }

            public string SpellCooldownName { get; set; }
        }
    }
}