using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;
using VitalicRotation.Helpers;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Helpers
{
    internal static class RestealthManager
    {
        private static bool _armed; // mode actif

        public static bool Toggle() { _armed = !_armed; return _armed; }
        public static void Disarm() { _armed = false; }
        public static bool IsArmed { get { return _armed; } }

        // Minimal v.zip style: attempt Stealth OOC only, no extra safety checks/delays
        public static Composite Build()
        {
            return new Action(delegate
            {
                try
                {
                    var me = StyxWoW.Me;
                    if (!_armed || me == null || !me.IsAlive) return RunStatus.Failure;
                    if (me.Combat) return RunStatus.Failure; // strictly OOC
                    if (me.HasAura("Stealth")) { _armed = false; return RunStatus.Failure; }

                    // Stealth safety gating
                    if (!IsStealthSafe())
                    {
                        if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                            Logger.Write("[Restealth] Safety gate blocked restealth attempt");
                        return RunStatus.Failure;
                    }

                    if (SpellBook.CanCast(SpellBook.Stealth))
                    {
                        if (SpellBook.Cast(SpellBook.Stealth))
                        {
                            UiCompat.Notify("Re-stealth");
                            _armed = false; // disarm after success
                            return RunStatus.Success;
                        }
                    }
                }
                catch { }
                return RunStatus.Failure;
            });
        }

        // Safety: prevent restealth if nearby hostile reveal / flare / bleed dots active (E18)
        private static bool IsStealthSafe()
        {
            try
            {
                var me = StyxWoW.Me; if (me == null || !me.IsAlive) return false;

                // 0) Recent damage check (simple heuristic: health lost in last 2s? use combat log surrogate via HealthPercent delta cache)
                double hpNow = 0; try { hpNow = me.HealthPercent; } catch { }
                if (_lastHpSnapshot > 0 && (DateTime.UtcNow - _lastHpStamp).TotalMilliseconds <= 2000 && hpNow < _lastHpSnapshot - 2.0)
                {
                    return false; // took noticeable damage recently
                }
                if ((DateTime.UtcNow - _lastHpStamp).TotalMilliseconds > 500) { _lastHpSnapshot = hpNow; _lastHpStamp = DateTime.UtcNow; }

                // 1) Hard reveal auras (cannot stealth) – Faerie Fire, Hunter's Mark variants (MoP IDs)
                try
                {
                    foreach (var a in me.GetAllAuras())
                    {
                        if (a == null || !a.IsActive || !a.IsHarmful) continue;
                        int id = a.SpellId;
                        if (id == 770       // Faerie Fire
                            || id == 102355 // Faerie Swarm (slow but treat cautiously)
                            || id == 1130   // Hunter's Mark
                            || id == 113344 // Shadowy retouch (placeholder)
                            ) return false;
                    }
                }
                catch { }

                // 2) Active flare / reveal game objects (scan within 15y for names containing 'Flare')
                try
                {
                    var objs = Styx.WoWInternals.ObjectManager.GetObjectsOfType<Styx.WoWInternals.WoWObjects.WoWGameObject>();
                    foreach (var go in objs)
                    {
                        if (go == null || !go.IsValid) continue;
                        if (go.Distance > 15.0) continue;
                        string name = go.Name ?? string.Empty;
                        if (name.IndexOf("Flare", StringComparison.OrdinalIgnoreCase) >= 0)
                            return false;
                    }
                }
                catch { }

                // 3) Hostile players in combat very near (<=12y)
                var units = Styx.WoWInternals.ObjectManager.GetObjectsOfType<Styx.WoWInternals.WoWObjects.WoWUnit>();
                foreach (var u in units)
                {
                    if (u == null || !u.IsValid || !u.IsAlive) continue;
                    if (!u.IsPlayer || !u.Attackable || u.IsFriendly) continue;
                    double dist = 0; try { dist = u.Distance; } catch { dist = 99; }
                    if (dist <= 12.0 && u.Combat)
                    {
                        if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                            Logger.Write("[Restealth] Unsafe: hostile combatant {0} at {1:0.0}y", u.Name, dist);
                        return false;
                    }
                }

                // 4) Harmful DoT / Bleed with >3s remaining
                try
                {
                    var auras = me.GetAllAuras();
                    for (int i = 0; i < auras.Count; i++)
                    {
                        var a = auras[i]; if (a == null || !a.IsActive || !a.IsHarmful) continue;
                        double rem = 0; try { rem = a.TimeLeft.TotalSeconds; } catch { rem = 0; }
                        if (rem <= 0) continue;
                        string name = a.Name ?? string.Empty;
                        if (rem > 3.0 && (name.IndexOf("Bleed", System.StringComparison.OrdinalIgnoreCase) >= 0
                                           || name.IndexOf("Rend", System.StringComparison.OrdinalIgnoreCase) >= 0
                                           || name.IndexOf("Rip", System.StringComparison.OrdinalIgnoreCase) >= 0
                                           || name.IndexOf("Garrote", System.StringComparison.OrdinalIgnoreCase) >= 0
                                           || name.IndexOf("Deep Wounds", System.StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                                Logger.Write("[Restealth] Unsafe: harmful dot {0} rem={1:0.0}s", name, rem);
                            return false;
                        }
                    }
                }
                catch { }

                return true; // safe
            }
            catch { return false; }
        }

        private static double _lastHpSnapshot = 0; private static DateTime _lastHpStamp = DateTime.MinValue;

        public static void NotifyCombatEnded() { /* no-op in minimal version */ }
    }
}
