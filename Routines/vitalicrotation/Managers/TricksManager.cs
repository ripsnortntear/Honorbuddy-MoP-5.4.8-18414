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
    internal static class TricksManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _lastCast = DateTime.MinValue;

        // Simple throttle keys
        private const int ThrottleMs = 500;

        // Refactored unified execute (E3)
        public static void Execute()
        {
            try
            {
                if (!PreChecks()) return;
                var target = ResolveTarget();
                if (target == null)
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Diag.Log("[Tricks] Skip: no valid target");
                    return;
                }

                if (!SpellBook.CanCast(SpellBook.TricksOfTheTrade, target))
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Diag.Log("[Tricks] Skip: cannot cast (LOS/range/CD)");
                    return;
                }

                // Avoid recasting if target already has buff alias and remaining >1.0s
                try
                {
                    if (HasTricksBuff(target))
                    {
                        if (VitalicSettings.Instance.DiagnosticMode)
                            Diag.Log("[Tricks] Skip: target already has buff");
                        return;
                    }
                }
                catch { }

                if (SpellBook.Cast(SpellBook.TricksOfTheTrade, target))
                {
                    _lastCast = DateTime.UtcNow;
                    Logger.Write("[Tricks] -> " + target.Name);
                    try { LuaHelper.SleepForLag(); } catch { }
                }
            }
            catch { }
        }

        // Return false if any hard gate fails
        private static bool PreChecks()
        {
            if (Me == null || !Me.IsAlive) return false;
            if (EventHandlers.ShouldPauseOffense()) return false;
            if (!ToggleState.Burst) { return false; }
            if (!CooldownManager.InPostHoldWindow()) { return false; }
            if ((DateTime.UtcNow - _lastCast).TotalMilliseconds < ThrottleMs) return false;
            return true;
        }

        // Resolution order:
        // 1) Explicit settings target name (exact match in raid/party)
        // 2) Focus (macro-based)
        // 3) First tank role
        // 4) Current friendly target player
        private static WoWPlayer ResolveTarget()
        {
            WoWPlayer p;
            try
            {
                string explicitName = null; try { explicitName = VitalicSettings.Instance.TricksTarget; } catch { }
                if (!string.IsNullOrWhiteSpace(explicitName))
                {
                    p = FindPlayerByName(explicitName.Trim());
                    if (IsValidTricksTarget(p)) return p;
                }
            }
            catch { }

            // Focus
            try
            {
                var focus = LuaHelpers.GetUnitById("focus") as WoWPlayer;
                if (IsValidTricksTarget(focus)) return focus;
            }
            catch { }

            // Tank role
            p = ResolveFirstTank();
            if (IsValidTricksTarget(p)) return p;

            // Current target if friendly player
            try
            {
                var ct = Me.CurrentTarget as WoWPlayer;
                if (IsValidTricksTarget(ct)) return ct;
            }
            catch { }

            return null;
        }

        private static WoWPlayer FindPlayerByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name)) return null;
                var objs = ObjectManager.GetObjectsOfType<WoWPlayer>();
                foreach (var o in objs)
                {
                    if (o == null || !o.IsValid || !o.IsAlive) continue;
                    if (string.Equals(o.Name, name, StringComparison.OrdinalIgnoreCase)) return o;
                }
            }
            catch { }
            return null;
        }

        private static bool IsValidTricksTarget(WoWUnit u)
        {
            if (u == null || !u.IsValid || !u.IsAlive) return false;
            try { if (!(u is WoWPlayer)) return false; } catch { return false; }
            try { if (u.Guid == Me.Guid) return false; } catch { }
            try { if (!u.IsFriendly) return false; } catch { return false; }
            try { if (u.IsHostile) return false; } catch { }
            try { if (!u.InLineOfSpellSight) return false; } catch { return false; }
            try { if (u.Distance > 30.0) return false; } catch { }
            return true;
        }

        private static WoWPlayer ResolveFirstTank()
        {
            try
            {
                int i;
                for (i = 1; i <= 5; i++) { var p = ResolveUnitByRole("party" + i, "TANK"); if (p != null) return p; }
                for (i = 1; i <= 40; i++) { var r = ResolveUnitByRole("raid" + i, "TANK"); if (r != null) return r; }
            }
            catch { }
            return null;
        }

        private static WoWPlayer ResolveUnitByRole(string token, string roleToken)
        {
            try
            {
                string role = Lua.GetReturnVal<string>("local r=UnitGroupRolesAssigned('" + token + "'); return r or ''", 0);
                if (!string.Equals(role, roleToken, StringComparison.OrdinalIgnoreCase)) return null;
                string g = Lua.GetReturnVal<string>("return UnitGUID('" + token + "') or ''", 0);
                if (string.IsNullOrEmpty(g)) return null;
                ulong guid;
                if (g.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ulong.TryParse(g.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out guid)) return null;
                }
                else if (!ulong.TryParse(g, out guid)) return null;
                var unit = ObjectManager.GetObjectByGuid<WoWPlayer>(guid);
                if (IsValidTricksTarget(unit)) return unit;
            }
            catch { }
            return null;
        }

        private static bool HasTricksBuff(WoWPlayer p)
        {
            if (p == null || !p.IsValid || !p.IsAlive) return false;
            try { return p.HasAura(SpellBook.TricksOfTheTradeBuffAlias) || p.HasAura("Tricks of the Trade"); } catch { return false; }
        }
    }
}
