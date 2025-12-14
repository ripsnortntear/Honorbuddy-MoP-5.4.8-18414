using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    internal static class GlueManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // Track recent target GUIDs and last positions
        private static readonly Dictionary<ulong, DateTime> _recentTargets = new Dictionary<ulong, DateTime>();
        private static readonly Dictionary<ulong, WoWPoint> _lastKnown = new Dictionary<ulong, WoWPoint>();
        private static DateTime _lastTick = DateTime.MinValue;
        private static DateTime _lastReacquire = DateTime.MinValue;
        private static DateTime _lastAutorun = DateTime.MinValue;
        private static bool _autorunActive;

        // Tunables (conservative, MoP/HB safe)
        private const int TrackWindowMs = 5000;     // keep targets for 5s after loss
        private const int TickIntervalMs = 100;     // periodic check
        private const int ReacquireIntervalMs = 350; // how often to attempt a reacquire
        private const double NearbyRadius = 15.0;   // proximity radius for last-known match

        public static void Execute()
        {
            try
            {
                var S = VitalicSettings.Instance;
                if (S == null || !S.EnableGluePlugin) return;

                var now = DateTime.UtcNow;
                if ((now - _lastTick).TotalMilliseconds < TickIntervalMs) return;
                _lastTick = now;

                var me = Me;
                if (me == null || !me.IsValid || me.IsDead) return;

                var target = me.CurrentTarget as WoWUnit;
                if (target != null && target.IsValid && target.Attackable && !target.IsDead)
                {
                    _recentTargets[target.Guid] = now;
                    _lastKnown[target.Guid] = target.Location;

                    // Stop autorun once melee achieved
                    if (_autorunActive && target.Distance < 5.0)
                    {
                        try { Lua.DoString("MoveForwardStop()"); } catch { }
                        _autorunActive = false;
                        if (S.DiagnosticMode) Logger.Write("[Glue] Autorun stop (melee)");
                    }

                    return; // nothing else if we have a valid target
                }

                // No valid target: try to reacquire something recent
                if ((now - _lastReacquire).TotalMilliseconds < ReacquireIntervalMs) return;
                _lastReacquire = now;

                // prune old entries
                var guids = _recentTargets.Keys.ToArray();
                for (int i = 0; i < guids.Length; i++)
                {
                    var g = guids[i];
                    DateTime ts;
                    if (_recentTargets.TryGetValue(g, out ts))
                    {
                        if ((now - ts).TotalMilliseconds > TrackWindowMs)
                        {
                            _recentTargets.Remove(g); _lastKnown.Remove(g);
                        }
                    }
                }

                // attempt reacquire by GUID proximity
                foreach (var kv in _recentTargets)
                {
                    WoWPoint last;
                    if (!_lastKnown.TryGetValue(kv.Key, out last)) continue;

                    double distToLast = 9999;
                    try { distToLast = me.Location.Distance(last); } catch { }
                    if (distToLast > 45.0) continue;

                    var units = ObjectManager.GetObjectsOfType<WoWUnit>();
                    for (int i = 0; i < units.Count; i++)
                    {
                        var u = units[i];
                        if (u == null || !u.IsValid || u.IsDead) continue;
                        try { if (!u.Attackable || u.IsFriendly) continue; } catch { }

                        // if GUID exact match in range, pick it
                        if (u.Guid == kv.Key)
                        {
                            TryTarget(u, distToLast, S);
                            return;
                        }

                        // otherwise positional proximity near last-known
                        double near = 9999; try { near = u.Location.Distance(last); } catch { }
                        if (near <= NearbyRadius)
                        {
                            TryTarget(u, distToLast, S);
                            return;
                        }
                    }

                    // move toward last-known as fallback
                    if (S.GlueAutoRun && !_autorunActive && distToLast > 6.0 && distToLast < 30.0)
                    {
                        try
                        {
                            me.SetFacing(last);
                            Lua.DoString("MoveForwardStart()");
                            _autorunActive = true;
                            _lastAutorun = now;
                            if (S.DiagnosticMode) Logger.Write(string.Format("[Glue] Autorun toward last pos ({0:0.0}y)", distToLast));
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        private static void TryTarget(WoWUnit u, double distToLast, VitalicSettings S)
        {
            try
            {
                u.Target();
                if (S.DiagnosticMode) Logger.Write("[Glue] Reacquired -> " + u.SafeName);

                // optional autorun toward target if far
                if (S.GlueAutoRun && !_autorunActive && distToLast > 8.0)
                {
                    try { Lua.DoString("MoveForwardStart()"); _autorunActive = true; } catch { }
                }

                // Try Shadowstep via MobilityManager when within 25y
                try
                {
                    if (u.Distance <= 25.0)
                    {
                        MobilityManager.TryShadowstepSafe(u);
                    }
                }
                catch { }
            }
            catch { }
        }

        public static void HandleCombatLog(string subEvent, int spellId, ulong srcGuid, ulong dstGuid)
        {
            try
            {
                var S = VitalicSettings.Instance; if (S == null || !S.EnableGluePlugin) return;
                if (!string.Equals(subEvent, "SPELL_CAST_SUCCESS", StringComparison.Ordinal)) return;

                bool isEscape = (spellId == 1953)    // Blink
                             || (spellId == 36554)   // Shadowstep
                             || (spellId == 1856)    // Vanish
                             || (spellId == 100)     // Charge
                             || (spellId == 20253)   // Intercept
                             || (spellId == 6544)    // Heroic Leap
                             || (spellId == 51490)   // Thunderstorm
                             || (spellId == 109132)  // Roll
                             || (spellId == 108843)  // Blazing Speed
                             || (spellId == 68992);  // Darkflight

                if (!isEscape) return;

                try
                {
                    var u = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid);
                    if (u != null && u.IsValid && !u.IsFriendly)
                    {
                        _recentTargets[srcGuid] = DateTime.UtcNow;
                        _lastKnown[srcGuid] = u.Location;
                        if (S.DiagnosticMode) Logger.Write("[Glue] Escape spell " + spellId + " by " + u.SafeName);
                    }
                }
                catch { }
            }
            catch { }
        }
    }
}
