using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    public static class DefensivesManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static DateTime _lastDamagedAt = DateTime.MinValue;
        private static readonly Stopwatch _recupThrottle = new Stopwatch();
        private static readonly Stopwatch _defThrottle = new Stopwatch();
        private static DateTime _dangerUntilUtc = DateTime.MinValue;
        private static readonly Dictionary<int, int> _activeDangerAuras = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> _dangerResponsesByAura = new Dictionary<int, int>();
        private static int _lastDangerResponses;

        private sealed class Window
        {
            public DateTime Until; public DefensiveFlags Flags; public ulong Source; public int SpellId; public double MaxRange;
        }
        private static readonly Dictionary<ulong, Window> _windows = new Dictionary<ulong, Window>();
        private static DateTime _lastCast = DateTime.MinValue;
        private static DateTime _lastDefUse = DateTime.MinValue;
        private const int DefThrottleMs = 400;
        private static bool ThrottleDef()
        {
            if ((DateTime.UtcNow - _lastDefUse).TotalMilliseconds < DefThrottleMs) return true;
            _lastDefUse = DateTime.UtcNow; return false;
        }
        private static bool TryFeint() { if (SpellBook.CanCast(SpellBook.Feint)) return SpellBook.Cast(SpellBook.Feint); return false; }
        private static void TrySprintOut()
        {
            if (StyxWoW.Me != null && !StyxWoW.Me.Combat) return;
            if (SpellBook.CanCast(SpellBook.Sprint)) SpellBook.Cast(SpellBook.Sprint);
        }

        public static void OpenAuraWindow(ulong src, int spellId, DefensiveFlags flags) { _windows[src] = new Window { Until = DateTime.UtcNow.AddSeconds(6), Flags = flags, Source = src, SpellId = spellId, MaxRange = 0 }; }
        public static void KeepAuraWindow(ulong src, int spellId, DefensiveFlags flags) { _windows[src] = new Window { Until = DateTime.UtcNow.AddSeconds(60), Flags = flags, Source = src, SpellId = spellId, MaxRange = 0 }; }
        public static void CloseAuraWindow(ulong src) { if (_windows.ContainsKey(src)) _windows.Remove(src); }
        public static void OpenCastWindow(ulong src, int spellId, DefensiveFlags flags, double seconds, double maxRange) { _windows[src] = new Window { Until = DateTime.UtcNow.AddSeconds(seconds), Flags = flags, Source = src, SpellId = spellId, MaxRange = maxRange }; }

        public static void EvaluateAndReact()
        {
            PurgeExpired();
            if ((DateTime.UtcNow - _lastCast).TotalMilliseconds < 700) return;
            var me = StyxWoW.Me; if (me == null || me.IsDead) return;
            WoWUnit caster; ThreatTable.DangerRule castRule;
            if (ThreatTable.AnyDangerCastOnMe(out caster, out castRule))
            {
                if (!ThrottleDef())
                {
                    if ((object)castRule.Type == (object)ThreatTable.DangerType.MagicNuke)
                    {
                        if (TryCloak()) return; if (TryFeint()) return;
                    }
                    if (TryFeint()) return;
                    // Emergency Vanish as last resort during a danger cast
                    if (TryEmergencyVanish()) { _lastCast = DateTime.UtcNow; return; }
                }
            }
            WoWUnit source; ThreatTable.DangerRule auraRule;
            if (ThreatTable.AnyDangerAuraNearMe(out source, out auraRule))
            {
                if (!ThrottleDef())
                {
                    if ((object)auraRule.Type == (object)ThreatTable.DangerType.PhysicalWhirl)
                    {
                        try { if (source.Distance <= 8f) { if (TryEvasion()) return; if (TryFeint()) return; } } catch { }
                        TrySprintOut(); return;
                    }
                    if ((object)auraRule.Type == (object)ThreatTable.DangerType.TeleportBurst)
                    {
                        if (TryEvasion()) return; if (TryCloak()) return; if (TryFeint()) return; return;
                    }
                    // Emergency Vanish when a dangerous aura is near (after other defensives)
                    if (TryEmergencyVanish()) { _lastCast = DateTime.UtcNow; return; }
                }
            }
            Window danger = null;
            try { danger = _windows.Values.OrderByDescending(w => Priority(w.Flags)).FirstOrDefault(w => InRangeOf(w)); } catch { }
            if (danger != null && VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode)
            {
                try
                {
                    Logger.Write("[Diag][Def] Window hit: spell={0} flags={1} until={2:HH:mm:ss.fff} inRange={3} maxRange={4}", danger.SpellId, danger.Flags, danger.Until, InRangeOf(danger), danger.MaxRange);
                }
                catch { }
            }
            if (danger == null) return;
            if ((danger.Flags & DefensiveFlags.Cloak) != 0)
            {
                if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode)
                {
                    bool dangerActive = false, can = false, gcd = false; bool resp = false;
                    try { dangerActive = IsDangerWindowActive(); } catch { }
                    try { resp = HasResponseFlag(DefFlag_Cloak); } catch { }
                    try { can = SpellBook.CanCast(SpellBook.CloakOfShadows); } catch { }
                    try { gcd = Styx.CommonBot.SpellManager.GlobalCooldown; } catch { }
                    Logger.Write("[Diag][Def] Cloak via window: dangerActive={0} respFlag={1} gcd={2} canCast={3}", dangerActive, resp, gcd, can);
                }
                if (TryCloak()) { _lastCast = DateTime.UtcNow; return; }
            }
            if ((danger.Flags & DefensiveFlags.Evasion) != 0 && TryEvasion()) { _lastCast = DateTime.UtcNow; return; }
            if ((danger.Flags & DefensiveFlags.Feint) != 0 && TryAutoFeint()) { _lastCast = DateTime.UtcNow; return; }
            // Fallback: emergency Vanish if window still active and no other defensive used
            if (TryEmergencyVanish()) { _lastCast = DateTime.UtcNow; return; }
        }

        private static void PurgeExpired()
        {
            var now = DateTime.UtcNow;
            try
            {
                var keys = new List<ulong>(_windows.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    var k = keys[i]; Window w; if (_windows.TryGetValue(k, out w)) { if (w == null || w.Until <= now) _windows.Remove(k); }
                }
            }
            catch { }
        }
        private static int Priority(DefensiveFlags f)
        {
            if ((f & DefensiveFlags.Cloak) != 0) return 3; if ((f & DefensiveFlags.Evasion) != 0) return 2; if ((f & DefensiveFlags.Feint) != 0) return 1; return 0;
        }
        private static bool InRangeOf(Window w)
        {
            if (w == null) return false; if (w.MaxRange <= 0) return true;
            try { var src = ObjectManager.GetObjectByGuid<WoWUnit>(w.Source); return src != null && src.IsAlive && src.Distance <= w.MaxRange; } catch { return true; }
        }

        public static bool Execute()
        {
            bool acted = false;
            if (Me == null || !Me.IsAlive) return false;
            EvaluateAndReact(); // may cast inside
            if (_defThrottle.IsRunning && _defThrottle.ElapsedMilliseconds < 700) { /* skip heavy checks */ }
            else
            {
                if (TryCloak()) { _defThrottle.Restart(); return true; }
                if (TryEvasion()) { _defThrottle.Restart(); return true; }
                if (TryAutoFeint()) { _defThrottle.Restart(); return true; }
                // Emergency Vanish as last defensive in sequence
                if (TryEmergencyVanish()) { _defThrottle.Restart(); return true; }
            }
            if (TryCombatReadiness()) return true;
            if (TryUseHealthstone()) return true;
            if (TryRecuperate()) return true;
            return acted; // false if nothing
        }

        public static Composite Build()
        {
            return new Action(ret => { try { Execute(); } catch { } return RunStatus.Failure; });
        }

        public static void NotifyPlayerDamaged() { _lastDamagedAt = DateTime.UtcNow; }
        public static void ActivateDangerWindow(double seconds) { ActivateDangerWindow(seconds, 0UL, 0, 0); }
        public static void ActivateDangerWindow(double seconds, ulong sourceGuid, int spellId, int responsesFlags)
        {
            if (seconds <= 0) return; var until = DateTime.UtcNow.AddSeconds(seconds); if (until > _dangerUntilUtc) { _dangerUntilUtc = until; _lastDangerResponses = responsesFlags; }
        }
        public static void DangerAuraApplied(int spellId) { DangerAuraApplied(0UL, spellId, 0); }
        public static void DangerAuraRemoved(int spellId) { DangerAuraRemoved(0UL, spellId); }
        public static void DangerAuraApplied(ulong sourceGuid, int spellId, int responsesFlags)
        {
            if (spellId <= 0) return; int c; if (_activeDangerAuras.TryGetValue(spellId, out c)) _activeDangerAuras[spellId] = c + 1; else _activeDangerAuras[spellId] = 1; _dangerResponsesByAura[spellId] = responsesFlags;
        }
        public static void DangerAuraRemoved(ulong sourceGuid, int spellId)
        {
            if (spellId <= 0) return; int c; if (_activeDangerAuras.TryGetValue(spellId, out c)) { c--; if (c <= 0) { _activeDangerAuras.Remove(spellId); _dangerResponsesByAura.Remove(spellId); } else _activeDangerAuras[spellId] = c; }
        }
        private static bool IsDangerWindowActive() { if (_activeDangerAuras.Count > 0) return true; return DateTime.UtcNow <= _dangerUntilUtc; }
        private const int DefFlag_Cloak = 1; private const int DefFlag_Evasion = 2; private const int DefFlag_Feint = 4;
        private static bool HasResponseFlag(int flagBit)
        {
            try { if ((_lastDangerResponses & flagBit) != 0) return true; } catch { }
            try { foreach (var kv in _dangerResponsesByAura) if ((kv.Value & flagBit) != 0) return true; } catch { }
            return false;
        }

        private static bool TryCloak()
        {
            if (IsDangerWindowActive() && HasResponseFlag(DefFlag_Cloak))
            {
                if (SpellBook.CanCast(SpellBook.CloakOfShadows)) { Logger.Write("[Def] Cloak of Shadows (danger)"); return SpellBook.Cast(SpellBook.CloakOfShadows, Me); }
                else if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode)
                {
                    try
                    {
                        bool gcd = false; try { gcd = Styx.CommonBot.SpellManager.GlobalCooldown; } catch { }
                        Logger.Write("[Diag][Def] Cloak danger-window gate: cannot cast (gcd={0})", gcd);
                    }
                    catch { }
                }
            }
            // Fallback now uses CloakHealth slider (exact control like Vitalic)
            var S = VitalicSettings.Instance;
            if (S == null || S.CloakHealth <= 0) return false; // disabled if not configured
            if (Me.HealthPercent > S.CloakHealth) return false;
            if (!HasHarmfulMagicDebuff()) return false;
            if (!SpellBook.CanCast(SpellBook.CloakOfShadows)) return false;
            if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode)
            {
                try
                {
                    Logger.Write("[Diag][Def] Cloak by HP/Magic: hp={0}% thr={1}% magic={2}", (int)Me.HealthPercent, S.CloakHealth, true);
                }
                catch { }
            }
            Logger.Write("[Def] Cloak of Shadows");
            return SpellBook.Cast(SpellBook.CloakOfShadows, Me);
        }
        private static bool TryEvasion()
        {
            if (IsDangerWindowActive() && HasResponseFlag(DefFlag_Evasion) && !HasAura(Me, SpellBook.Evasion))
            {
                if (SpellBook.CanCast(SpellBook.Evasion)) { Logger.Write("[Def] Evasion (danger)"); return SpellBook.Cast(SpellBook.Evasion); }
            }
            if (Me.HealthPercent > VitalicSettings.Instance.LowHealthWarning) return false;
            if (HasAura(Me, SpellBook.Evasion)) return false;
            if (CountEnemiesInMeleeRange(8.0) < 2) return false;
            if (!SpellBook.CanCast(SpellBook.Evasion)) return false;
            Logger.Write("[Def] Evasion");
            return SpellBook.Cast(SpellBook.Evasion);
        }
        private static bool TryAutoFeint()
        {
            // Class141 gate (1966): Check GCD and Feint buff BEFORE any decision or logging
            if (Styx.CommonBot.SpellManager.GlobalCooldown) return false;
            if (HasAura(Me, SpellBook.Feint)) return false; // Already have Feint buff
            
            if (VitalicSettings.Instance.FeintInMeleeRange)
            {
                var t = Me.CurrentTarget as WoWUnit; 
                if (t == null || !t.IsValid || !t.IsAlive || !t.Attackable || !t.IsWithinMeleeRange) return false;
            }
            
            if (IsDangerWindowActive() && HasResponseFlag(DefFlag_Feint))
            {
                if (SpellBook.CanCast(SpellBook.Feint)) 
                { 
                    // Only log when actually eligible (post-gating)
                    if (VitalicSettings.Instance.LogMessagesEnabled)
                        Logger.Write("[Def] Feint (danger)"); 
                    return SpellBook.Cast(SpellBook.Feint); 
                }
            }
            
            int hpThreshold = VitalicSettings.Instance.AutoFeint; 
            bool hpOk = (hpThreshold > 0 && Me.HealthPercent <= hpThreshold);
            int window = VitalicSettings.Instance.FeintLastDamage; 
            bool recentDamageOk = (window > 0) && ((DateTime.UtcNow - _lastDamagedAt).TotalSeconds <= window);
            bool danger = IsDangerWindowActive();
            
            if (!(hpOk || recentDamageOk || danger)) return false;
            if (!SpellBook.CanCast(SpellBook.Feint)) return false;
            
            bool cast = SpellBook.Cast(SpellBook.Feint);
            if (cast && VitalicSettings.Instance.LogMessagesEnabled)
            {
                string extra = string.Empty; 
                if (VitalicSettings.Instance.FeintInMeleeRange) extra += ", melee"; 
                if (hpOk) extra += ", hp"; 
                if (recentDamageOk) extra += ", dmg≤" + window + "s"; 
                if (danger) extra += ", danger";
                Logger.Write("[Def] Feint (HP {0}%{1})", (int)Me.HealthPercent, extra);
            }
            return cast;
        }
        private static bool TryRecuperate()
        {
            if (Me.HealthPercent > VitalicSettings.Instance.RecuperateHP) return false;
            if (Me.ComboPoints < 1) return false;
            if (!SpellBook.CanCast(SpellBook.Recuperate)) return false;
            if (_recupThrottle.IsRunning && _recupThrottle.ElapsedMilliseconds < 400) return false;
            bool cast = SpellBook.Cast(SpellBook.Recuperate, Me);
            if (cast)
            {
                _recupThrottle.Restart(); if (VitalicSettings.Instance.LogMessagesEnabled) Logger.Write("[Def] Recuperate (HP {0}%, CP {1})", (int)Me.HealthPercent, Me.ComboPoints);
            }
            return cast;
        }
        private static bool TryCombatReadiness()
        {
            var me = Me; if (me == null || !me.IsAlive) return false;
            if (HasAura(me, SpellBook.CombatReadiness)) return false;
            if (!SpellBook.CanCast(SpellBook.CombatReadiness)) return false;
            int meleeThreat = CountEnemiesInMeleeRange(8.0);
            if (meleeThreat >= 2 && me.HealthPercent <= VitalicSettings.Instance.LowHealthWarning)
            {
                Logger.Write("[Def] Combat Readiness"); return SpellBook.Cast(SpellBook.CombatReadiness);
            }
            return false;
        }
        private static bool TryUseHealthstone()
        {
            int hp = VitalicSettings.Instance.HealthstoneHP; if (hp <= 0) return false; if (Me.HealthPercent > hp) return false;
            WoWItem hs = null;
            try
            {
                var items = Me.BagItems; if (items != null)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        var it = items[i]; if (it == null || !it.IsValid) continue; if (!it.Usable) continue; bool isHS = (it.Entry == 5512);
                        try { if (!isHS && it.Name != null && it.Name.IndexOf("Healthstone", StringComparison.OrdinalIgnoreCase) >= 0) isHS = true; } catch { }
                        if (isHS) { hs = it; break; }
                    }
                }
            }
            catch { hs = null; }
            if (hs == null) return false; if (hs.Cooldown > 0f) return false;
            try { hs.Use(); if (VitalicSettings.Instance.LogMessagesEnabled) Logger.Write("[Def] Healthstone"); return true; } catch { return false; }
        }

        // === Emergency Vanish (no new settings; relies on current danger window) ===
        private static bool TryEmergencyVanish()
        {
            try
            {
                if (!IsDangerWindowActive()) return false;
                // Do not try if already stealthed/subterfuge
                bool stealthed = false;
                try { stealthed = Me.HasAura("Stealth") || Me.HasAura(115192) || Me.HasAura(115193) || Me.HasAura(112942); } catch { }
                if (stealthed) return false;
                if (!SpellBook.CanCast(SpellBook.Vanish)) return false;
                Logger.Write("[Def] Vanish (danger)");
                return SpellBook.Cast(SpellBook.Vanish);
            }
            catch { return false; }
        }

        private static bool HasHarmfulMagicDebuff()
        {
            try
            {
                for (int i = 1; i <= 40; i++)
                {
                    string dtype = Lua.GetReturnVal<string>("local _,_,_,dt=UnitDebuff('player'," + i + "); return dt or ''", 0);
                    if (dtype == "Magic") return true;
                }
            }
            catch { }
            return false;
        }
        private static int CountEnemiesInMeleeRange(double yards)
        {
            int count = 0;
            try
            {
                // P3.1 - Performance: foreach manual au lieu de LINQ
                var units = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(true, true);
                foreach (var u in units)
                {
                    if (u == null || !u.IsValid) continue;
                    if (!u.IsAlive || !u.Attackable || u.IsFriendly) continue;
                    if (u.Distance <= yards) 
                    {
                        count++;
                    }
                }
            }
            catch { }
            return count;
        }
        private static bool HasAura(WoWUnit unit, int spellId)
        {
            if (unit == null || !unit.IsValid) return false;
            try
            {
                var auras = unit.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a != null && a.SpellId == spellId) return true;
                }
            }
            catch { }
            return false;
        }

        // Ajout ShouldExecute pour module PvERotation (parité plan P1.1)
        public static bool ShouldExecute()
        {
            try
            {
                if (Me == null || !Me.IsAlive) return false;
                var S = VitalicSettings.Instance;
                if (S == null) return IsDangerWindowActive();
                bool danger = IsDangerWindowActive();
                if (danger) return true;

                // Cloak condition : HP <= CloakHealth & magic debuff
                if (S.CloakHealth > 0 && Me.HealthPercent <= S.CloakHealth && HasHarmfulMagicDebuff() && SpellBook.CanCast(SpellBook.CloakOfShadows)) return true;
                // Evasion condition : HP <= EvasionHealth & multiple melee
                if (S.EvasionHealth > 0 && Me.HealthPercent <= S.EvasionHealth && CountEnemiesInMeleeRange(8.0) >= 2 && SpellBook.CanCast(SpellBook.Evasion)) return true;

                // Feint condition: new FeintHealth OR legacy AutoFeint / recent damage window
                bool feintByNew = (S.FeintHealth > 0 && Me.HealthPercent <= S.FeintHealth);
                bool feintByOld = (S.AutoFeint > 0 && Me.HealthPercent <= S.AutoFeint);
                bool recentDamage = (S.FeintLastDamage > 0 && (DateTime.UtcNow - _lastDamagedAt).TotalSeconds <= S.FeintLastDamage);
                if ((feintByNew || feintByOld || recentDamage) && SpellBook.CanCast(SpellBook.Feint)) return true;

                // Recuperate defensive heal
                if (Me.HealthPercent <= S.RecuperateHP && Me.ComboPoints >= 1 && SpellBook.CanCast(SpellBook.Recuperate)) return true;
                // Healthstone
                if (S.HealthstoneHP > 0 && Me.HealthPercent <= S.HealthstoneHP) return true;
                // Combat Readiness
                if (S.EvasionHealth > 0 && Me.HealthPercent <= S.EvasionHealth && CountEnemiesInMeleeRange(8.0) >= 2 && SpellBook.CanCast(SpellBook.CombatReadiness)) return true;
                return false;
            }
            catch { return false; }
        }
    }
}
