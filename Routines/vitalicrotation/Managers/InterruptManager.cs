using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Diagnostics;
using System.Linq;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using VitalicRotation.UI;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    public static class InterruptManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _lastInterruptGlobal = DateTime.MinValue;
        private static readonly Stopwatch _kickThrottle = new Stopwatch();
        private static readonly Stopwatch _gougeThrottle = new Stopwatch();
        private static int _lastInterruptedSpellId;

        // ======== Public (tree) ========
        public static Composite Build() { return new Action(ret => Execute() ? RunStatus.Success : RunStatus.Failure); }

        // Main tick: attempt interrupt on current target or focus (simple path)
        public static bool Execute()
        {
            try
            {
                if (Me == null || !Me.IsAlive) return false;

                // Target first
                var unit = Me.CurrentTarget as WoWUnit;
                if (!IsValidInterruptTarget(unit)) unit = GetFocusUnit();
                if (!IsValidInterruptTarget(unit)) return false;

                if (!unit.CanInterruptCurrentSpellCast) return false; // nothing to do

                var S = VitalicSettings.Instance;

                // Prefer Gouge when HP above threshold (original simple behavior), else Kick first
                bool preferGouge = unit.HealthPercent > S.GougeNoKickHP;

                if (preferGouge && TryGouge(unit, S)) return true;
                if (TryKick(unit, S)) return true;
                if (!preferGouge && TryGouge(unit, S)) return true;
            }
            catch { }
            return false;
        }

        // Fast hotkey helper (pipeline) – mirror simple logic (Kick only, fallback Gouge by HP)
        public static bool TryFastKickWithFallback()
        {
            var me = Me; if (me == null || !me.IsAlive) return false;
            var S = VitalicSettings.Instance;
            var targets = new WoWUnit[] { GetFocusUnit(), me.CurrentTarget as WoWUnit }
                .Where(IsValidInterruptTarget).ToArray();
            foreach (var u in targets)
            {
                bool preferGouge = u.HealthPercent > S.GougeNoKickHP;
                if (preferGouge && TryGouge(u, S)) return true;
                if (TryKick(u, S)) return true;
                if (!preferGouge && TryGouge(u, S)) return true;
            }
            return false;
        }

        public static void OnFocusChanged(string unitId) { /* stub retained for compatibility */ }
        // Deadly Throw interrupt / snare disabled in mirror mode
        public static bool TryDeadlyThrowInterrupt(WoWUnit explicitTarget) { return false; }
        public static bool ShouldDeadlyThrowSnare(WoWUnit u) { return false; }

        // ======== Helpers ========
        private static bool IsValidInterruptTarget(WoWUnit u)
        {
            if (u == null || !u.IsValid || !u.IsAlive || u.IsFriendly) return false;
            if (!(u.IsCasting || u.IsChanneling)) return false;
            return true;
        }

        private static bool GlobalThrottled()
        {
            // Simple fixed throttle (~200ms legacy)
            return (DateTime.UtcNow - _lastInterruptGlobal).TotalMilliseconds < 200;
        }
        private static void StampGlobal() { _lastInterruptGlobal = DateTime.UtcNow; }

        private static bool TryKick(WoWUnit unit, VitalicSettings S)
        {
            if (unit == null) return false;
            if (!unit.CanInterruptCurrentSpellCast) return false;
            if (GlobalThrottled()) return false;
            if (!IsKickInRange(unit)) return false;
            if (!InWindow(unit, S)) return false;
            double latencySec = 0; try { latencySec = StyxWoW.WoWClient != null ? StyxWoW.WoWClient.Latency / 1000.0 : 0.0; } catch { }
            if (_kickThrottle.IsRunning && _kickThrottle.Elapsed.TotalSeconds < (latencySec + 0.05)) return false;
            if (SpellBook.CanCast(SpellBook.Kick, unit) && SpellBook.Cast(SpellBook.Kick, unit))
            {
                _kickThrottle.Restart();
                StampGlobal();
                CaptureInterruptedSpell(unit);
                try { if (S.SoundAlertsEnabled) AudioBus.PlayInterrupt(); } catch { }
                if (_lastInterruptedSpellId != 0) try { VitalicUi.ShowNotify(_lastInterruptedSpellId, null); } catch { }
                Logger.Write("[Interrupt] Kick -> " + unit.SafeName);
                return true;
            }
            return false;
        }

        private static bool TryGouge(WoWUnit unit, VitalicSettings S)
        {
            if (unit == null) return false;
            if (!unit.CanInterruptCurrentSpellCast) return false;
            if (unit.HealthPercent < S.GougeNoKickHP) return false; // below threshold prefer Kick path
            if (!IsKickInRange(unit)) return false; // need melee zone
            try { if (!DRTracker.CanApplyIncap(unit, 0.5)) return false; } catch { }
            double gd = Math.Max(0.2, S.GougeDelay);
            if (_gougeThrottle.IsRunning && _gougeThrottle.Elapsed.TotalSeconds < gd) return false;
            if (SpellBook.CanCast(SpellBook.Gouge, unit))
            {
                if (!Me.IsSafelyFacing(unit)) try { Me.SetFacing(unit); } catch { }
                if (SpellBook.Cast(SpellBook.Gouge, unit))
                {
                    _gougeThrottle.Restart();
                    Logger.Write("[Interrupt] Gouge -> " + unit.SafeName);
                    return true;
                }
            }
            return false;
        }

        private static bool InWindow(WoWUnit u, VitalicSettings S)
        {
            // Simple seconds window: InterruptMinimum .. InterruptDelay (+ latency)
            double leftSec = 0.0; try { leftSec = u.IsChanneling ? u.CurrentChannelTimeLeft.TotalSeconds : u.CurrentCastTimeLeft.TotalSeconds; } catch { }
            double minW = 0.0, maxW = 0.0; double latency = 0.0;
            try { minW = S.InterruptMinimum; } catch { }
            try { maxW = S.InterruptDelay; } catch { }
            try { latency = StyxWoW.WoWClient != null ? (double)StyxWoW.WoWClient.Latency / 1000.0 : 0.0; } catch { }
            if (maxW > 0) maxW += latency;
            if (minW > 0 && leftSec < minW) return false;
            if (maxW > 0 && leftSec > maxW) return false;
            return true;
        }

        private static void CaptureInterruptedSpell(WoWUnit unit)
        {
            try
            {
                int spellId; double ms; double pct;
                if (IsInterruptibleNow(unit, out spellId, out ms, out pct) && spellId != 0)
                    _lastInterruptedSpellId = spellId;
            }
            catch { }
        }

        private static bool IsKickInRange(WoWUnit unit) { return unit != null && LosFacingCache.InLineOfSpellSightCached(unit, 2000) && unit.Distance <= 5.0; }

        // Minimal spell info helper
        private static bool IsInterruptibleNow(WoWUnit u, out int spellId, out double remainingMs, out double pctLeft)
        {
            spellId = 0; remainingMs = 0; pctLeft = 0;
            if (u == null || !u.IsValid || u.IsFriendly) return false;
            try
            {
                if (u.IsCasting && u.CastingSpell != null)
                {
                    spellId = (int)u.CastingSpell.Id;
                    remainingMs = u.CurrentCastTimeLeft.TotalMilliseconds;
                    return u.CanInterruptCurrentSpellCast;
                }
                if (u.IsChanneling && u.ChanneledSpell != null)
                {
                    spellId = (int)u.ChanneledSpell.Id;
                    remainingMs = u.CurrentChannelTimeLeft.TotalMilliseconds;
                    return u.CanInterruptCurrentSpellCast;
                }
            }
            catch { }
            return false;
        }

        private static WoWUnit GetFocusUnit()
        {
            try
            {
                string s = Lua.GetReturnVal<string>("local g = UnitGUID('focus'); if g then return g else return '' end", 0);
                if (string.IsNullOrEmpty(s)) return null;
                ulong focusGuid = 0UL;
                if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    ulong.TryParse(s.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out focusGuid);
                else
                    ulong.TryParse(s, out focusGuid);
                if (focusGuid == 0UL) return null;
                return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Guid == focusGuid);
            }
            catch { return null; }
        }
    }
}
