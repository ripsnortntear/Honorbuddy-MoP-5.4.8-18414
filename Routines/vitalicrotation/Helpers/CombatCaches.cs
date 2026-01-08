using System;
using System.Collections.Generic;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    // Class63 parity: simple caches with TTL to reduce hot-path allocations/queries
    internal static class CombatCaches
    {
        // attackable_units TTL 3000ms
        private static DateTime _nextAttackableUpdate = DateTime.MinValue;
        private static readonly List<WoWUnit> _attackableUnits = new List<WoWUnit>(64);

        // units_in_melee TTL 1250ms
        private static DateTime _nextMeleeUpdate = DateTime.MinValue;
        private static int _unitsInMelee;

        // enemy_healers TTL 2500ms (parity hashSet_0 = {4,11})
        private static DateTime _nextHealersUpdate = DateTime.MinValue;
        private static readonly List<WoWUnit> _enemyHealers = new List<WoWUnit>(16);

        public static IList<WoWUnit> AttackableUnits()
        {
            var now = DateTime.UtcNow;
            if (now >= _nextAttackableUpdate)
            {
                RebuildAttackableUnits();
                _nextAttackableUpdate = now.AddMilliseconds(3000);
            }
            return _attackableUnits;
        }

        public static int UnitsInMelee()
        {
            var now = DateTime.UtcNow;
            if (now >= _nextMeleeUpdate)
            {
                RebuildUnitsInMelee();
                _nextMeleeUpdate = now.AddMilliseconds(1250);
            }
            return _unitsInMelee;
        }

        public static IList<WoWUnit> EnemyHealers()
        {
            var now = DateTime.UtcNow;
            if (now >= _nextHealersUpdate)
            {
                RebuildEnemyHealers();
                _nextHealersUpdate = now.AddMilliseconds(2500);
            }
            return _enemyHealers;
        }

        private static void RebuildAttackableUnits()
        {
            _attackableUnits.Clear();
            try
            {
                var me = StyxWoW.Me;
                if (me == null || !me.IsValid) return;
                var list = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < list.Count; i++)
                {
                    var u = list[i];
                    if (u == null || !u.IsValid || u.IsDead) continue;
                    if (!u.Attackable || u.IsFriendly) continue;
                    _attackableUnits.Add(u);
                }
            }
            catch { }
        }

        private static void RebuildUnitsInMelee()
        {
            _unitsInMelee = 0;
            try
            {
                var me = StyxWoW.Me;
                if (me == null || !me.IsValid) return;
                var list = AttackableUnits(); // reuse cache
                for (int i = 0; i < list.Count; i++)
                {
                    var u = list[i];
                    if (u == null || !u.IsValid) continue;
                    if (u.Distance <= 5.0) _unitsInMelee++;
                }
            }
            catch { }
        }

        private static void RebuildEnemyHealers()
        {
            _enemyHealers.Clear();
            try
            {
                var me = StyxWoW.Me;
                if (me == null || !me.IsValid) return;
                var list = AttackableUnits();
                for (int i = 0; i < list.Count; i++)
                {
                    var u = list[i];
                    if (u == null || !u.IsValid) continue;
                    // Parity: hashSet_0 = {4,11} Warrior,Druid (do not correct)
                    if (u.Class == WoWClass.Warrior || u.Class == WoWClass.Druid)
                        _enemyHealers.Add(u);
                }
            }
            catch { }
        }
    }

    internal static class LosFacingCache
    {
        private class Entry
        {
            public bool Result;
            public DateTime Expire;
        }

        private static readonly Dictionary<ulong, Entry> _los = new Dictionary<ulong, Entry>(256);
        private static readonly Dictionary<ulong, Entry> _facing = new Dictionary<ulong, Entry>(256);
        private static readonly Dictionary<ulong, Entry> _melee = new Dictionary<ulong, Entry>(256);

        /// <summary>
        /// Class63.smethod_6 mirror: melee range 3.5y with TTL caching
        /// </summary>
        public static bool InMeleeRangeCached(WoWUnit u)
        {
            if (u == null || !u.IsValid || u.IsDead) return false;
            var guid = u.Guid;
            var now = DateTime.UtcNow;
            Entry e;
            if (_melee.TryGetValue(guid, out e) && e.Expire > now)
                return e.Result;

            bool val = u.Distance <= 3.5; // Class63 smethod_6 range

            // TTL: 50ms current target, 250ms others (Class63 behavior)
            var me = StyxWoW.Me;
            bool isCurrentTarget = me != null && me.CurrentTargetGuid == guid;
            int ttlMs = isCurrentTarget ? 50 : 250;

            _melee[guid] = new Entry { Result = val, Expire = now.AddMilliseconds(ttlMs) };
            return val;
        }

        public static bool InLineOfSpellSightCached(WoWUnit u, int ttlMs)
        {
            if (u == null || !u.IsValid) return false;
            var guid = u.Guid;
            var now = DateTime.UtcNow;
            Entry e;
            if (_los.TryGetValue(guid, out e) && e.Expire > now)
                return e.Result;

            bool val = false;
            try { val = u.InLineOfSpellSight; } catch { val = false; }
            _los[guid] = new Entry { Result = val, Expire = now.AddMilliseconds(ttlMs > 0 ? ttlMs : 2000) };
            return val;
        }

        public static bool IsSafelyFacingCached(WoWUnit u, int ttlMs)
        {
            var me = StyxWoW.Me;
            if (me == null || u == null || !u.IsValid) return false;
            var guid = u.Guid;
            var now = DateTime.UtcNow;
            Entry e;
            if (_facing.TryGetValue(guid, out e) && e.Expire > now)
                return e.Result;

            bool val = false;
            try { val = me.IsSafelyFacing(u); } catch { val = false; }
            _facing[guid] = new Entry { Result = val, Expire = now.AddMilliseconds(ttlMs > 0 ? ttlMs : 2000) };
            return val;
        }

        public static void PruneExpired()
        {
            var now = DateTime.UtcNow;
            try
            {
                var keys = new List<ulong>(_los.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    Entry e;
                    if (_los.TryGetValue(key, out e) && e.Expire <= now)
                        _los.Remove(key);
                }

                keys.Clear();
                keys.AddRange(_facing.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    Entry e;
                    if (_facing.TryGetValue(key, out e) && e.Expire <= now)
                        _facing.Remove(key);
                }

                keys.Clear();
                keys.AddRange(_melee.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    Entry e;
                    if (_melee.TryGetValue(key, out e) && e.Expire <= now)
                        _melee.Remove(key);
                }
            }
            catch { }
        }

        public static void Clear()
        {
            _los.Clear();
            _facing.Clear();
            _melee.Clear();
        }
    }
}
