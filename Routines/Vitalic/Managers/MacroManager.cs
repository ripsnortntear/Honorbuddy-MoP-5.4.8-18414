using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using VitalicRotation.UI;

namespace VitalicRotation.Managers
{
    internal static class MacroManager
    {
        public enum MacroTargetKind { Unknown = 0, Target = 1, Focus = 2, Mouseover = 3 }

        private enum Macro
        {
            CheapShot,
            Garrote,
            Gouge,
            Blind,
            RedirectKidney,
            FastKick
        }

        private static readonly Dictionary<Macro, string> _labels = new Dictionary<Macro, string>
        {
            { Macro.CheapShot,      "Cheap Shot" },
            { Macro.Garrote,        "Garrote" },
            { Macro.Gouge,          "Gouge" },
            { Macro.Blind,          "Blind" },
            { Macro.RedirectKidney, "Redirect Kidney" },
            { Macro.FastKick,       "Fast Kick" },
        };

        private static readonly Dictionary<Macro, string> _luaVar = new Dictionary<Macro, string>();

        private static string _queuedSpell;
        private static DateTime _queuedAtUtc;
        private static string _sticky;
        private static DateTime _stickyUntilUtc;
        private static bool _bootstrapped;

    // Status helpers (for UI parity)
    private static MacroTargetKind _currentTargetKind = MacroTargetKind.Unknown;
    private static bool _garrotePoolingIndicator; // set by opener logic when pooling for garrote

        private static VitalicSettings S { get { return VitalicSettings.Instance; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static string CurrentSticky { get { return _sticky; } }
        public static string CurrentQueuedSpell { get { return _queuedSpell; } }
    public static MacroTargetKind CurrentMacroTargetKind { get { return _currentTargetKind; } }
    public static bool GarrotePoolingIndicator { get { return _garrotePoolingIndicator; } set { _garrotePoolingIndicator = value; } }

        private static DateTime _lastManualMacroUtc = DateTime.MinValue;
        public static bool WasManualMacroRecently(int ms)
        {
            if (ms <= 0) return false;
            return (DateTime.UtcNow - _lastManualMacroUtc).TotalMilliseconds <= ms;
        }
        internal static void NotifyManualMacro() { _lastManualMacroUtc = DateTime.UtcNow; }

        /// <summary>
        /// Vide la file d'attente des macros (utilisé lors des resets).
        /// </summary>
        public static void ResetQueue()
        {
            _queuedSpell = null;
            _queuedAtUtc = DateTime.MinValue;
            _sticky = null;
            _stickyUntilUtc = DateTime.MinValue;
            _currentTargetKind = MacroTargetKind.Unknown;
        }

        // Wrapper public pour satisfaire la réflexion dans SmokeBombManager
        public static bool Consume(string actionName)
        {
            WoWUnit dummy;
            return TryDequeue(actionName, out dummy);
        }

        // Surchage pour compatibilité supplémentaire
        public static bool TryDequeue(string actionName)
        {
            WoWUnit dummy;
            return TryDequeue(actionName, out dummy);
        }

        public static void Initialize()
        {
            if (_bootstrapped) return;
            _bootstrapped = true;

            _luaVar.Clear();
            foreach (Macro m in Enum.GetValues(typeof(Macro)))
                _luaVar[m] = MakeUniqueLuaVar();

            // Déclare les slashs comme v.Zip (un par macro)
            foreach (var kv in _luaVar)
            {
                var macro = kv.Key;
                string luaName = kv.Value;
                string upper = macro.ToString().ToUpper(CultureInfo.InvariantCulture);
                string lower = macro.ToString().ToLower(CultureInfo.InvariantCulture);
                Lua.DoString(string.Format(@"SLASH_{0}1='/ {1}'", upper, lower).Replace("/ ", "/")); // fallback minimal
                Lua.DoString(string.Format(@"
SLASH_{0}1='/{1}'
SlashCmdList['{0}']=function(msg)
  if msg and string.len(msg)>0 then {2}=msg else {2}=true end
end", upper, lower, luaName));
            }

            // function gmv() -> retourne chaque var macro:valeur
            List<string> parts = new List<string>();
            foreach (var kv in _luaVar)
            {
                string v = kv.Value;
                parts.Add(string.Format("'{0}:'..((type({1})=='string') and {1} or ({1} and '1' or 'false'))", v, v));
            }
            Lua.DoString("function gmv() return " + string.Join(", ", parts.ToArray()) + " end");
            ResetQueue();
        }

        private static string MakeUniqueLuaVar() { return "V" + Guid.NewGuid().ToString("N").Substring(0, 8); }

        public static void Poll()
        {
            try
            {
                // Skip processing while typing in chat/edit boxes; process after Enter
                try { if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld) return; } catch { }
                try { if (Lua.GetReturnVal<bool>("return GetCurrentKeyBoardFocus() ~= nil", 0)) return; } catch { }

                var ret = Lua.GetReturnValues("return gmv()");
                if (ret == null || ret.Count == 0) return;

                Dictionary<string,string> map = new Dictionary<string,string>();
                foreach (var s in ret)
                {
                    int idx = s.IndexOf(':');
                    if (idx <= 0) continue;
                    map[s.Substring(0, idx)] = s.Substring(idx + 1);
                }

                foreach (var kv in _luaVar)
                {
                    string luaName = kv.Value;
                    string value;
                    if (!map.TryGetValue(luaName, out value) || value == "false") continue;
                    Lua.DoString(luaName + " = nil");

                    _queuedSpell = _labels[kv.Key];
                    _queuedAtUtc = DateTime.UtcNow;
                    NotifyManualMacro();

                    _sticky = _queuedSpell;
                    _stickyUntilUtc = _queuedAtUtc.AddSeconds(S.StickyDelay);

                    // Determine preferred target kind at queue time (for status display)
                    try
                    {
                        if (InputPoller.FocusMacroHeld)
                            _currentTargetKind = MacroTargetKind.Focus;
                        else
                        {
                            var mo = GetMouseover();
                            _currentTargetKind = IsAttackableUnit(mo) ? MacroTargetKind.Mouseover : MacroTargetKind.Target;
                        }
                    }
                    catch { _currentTargetKind = MacroTargetKind.Unknown; }
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(_sticky) && DateTime.UtcNow > _stickyUntilUtc)
                _sticky = null;
        }

        // Hotkeys
        public static void Enqueue(string spellName)
        {
            if (string.IsNullOrEmpty(spellName)) return;
            _queuedSpell = spellName;
            _queuedAtUtc = DateTime.UtcNow;
            _sticky = spellName;
            _stickyUntilUtc = _queuedAtUtc.AddSeconds(S.StickyDelay);
        }
        public static void Enqueue(string spellName, WoWUnit target) { Enqueue(spellName); }

        // Consommation côté rotation
        public static bool TryDequeue(string spellName, out WoWUnit resolvedTarget)
        {
            resolvedTarget = null;

            if (!S.MacrosEnabled) return false;
            if (string.IsNullOrEmpty(spellName)) return false;
            if (!string.Equals(spellName, _queuedSpell, StringComparison.Ordinal)) return false;

            var elapsed = (DateTime.UtcNow - _queuedAtUtc).TotalSeconds;

            // 0) anti-rebond ~0.25s (miroir v.zip)
            if (elapsed < 0.25)
            {
                return false; // wait for anti-bounce
            }

            // 1) fenêtre de validité = MacroDelay (défaut 2.0s comme l'original)
            double macroDelay = 0.0;
            try { macroDelay = System.Math.Max(0.0, S.MacroDelay); } catch { }

            if (macroDelay > 0.0 && elapsed > macroDelay)
            {
                // expire: on DROP l'entrée (ne pas ré-enfiler)
                _queuedSpell = null;
                return false;
            }

            // Focus modifier (maintien) sinon mouseover si pertinent, sinon current target
            try
            {
                if (InputPoller.FocusMacroHeld)
                    resolvedTarget = GetFocus();
                if (resolvedTarget == null)
                {
                    var mo = GetMouseover();
                    if (IsAttackableUnit(mo)) resolvedTarget = mo;
                }
            }
            catch { }
            if (resolvedTarget == null) resolvedTarget = Me != null ? Me.CurrentTarget as WoWUnit : null;

            _queuedSpell = null; // consommé
            _currentTargetKind = MacroTargetKind.Unknown; // reset after consumption
            return true;
        }

        // --- Sticky helpers (exposés) ---
        public static bool IsWithinStickyWindow(string spellName)
        {
            try
            {
                if (string.IsNullOrEmpty(spellName)) return false;
                if (string.IsNullOrEmpty(_sticky)) return false;
                if (!string.Equals(_sticky, spellName, StringComparison.Ordinal)) return false;
                return DateTime.UtcNow <= _stickyUntilUtc;
            }
            catch { return false; }
        }

        public static void ExtendSticky(string spellName, double additionalSeconds)
        {
            try
            {
                if (string.IsNullOrEmpty(spellName)) return;
                if (!string.Equals(_sticky, spellName, StringComparison.Ordinal)) return;
                if (additionalSeconds <= 0) return;
                var until = _stickyUntilUtc;
                var add = TimeSpan.FromSeconds(additionalSeconds);
                _stickyUntilUtc = until > DateTime.UtcNow ? until.Add(add) : DateTime.UtcNow.Add(add);
            }
            catch { }
        }

        // Re-enfile la macro si on est toujours dans la fenêtre sticky (pour retry auto sur tick suivant)
        public static bool RequeueIfSticky(string spellName)
        {
            try
            {
                if (!IsWithinStickyWindow(spellName)) return false;
                _queuedSpell = spellName;
                // Anti-bounce: artificiellement reculer l'horloge pour passer la barrière 0.25s
                _queuedAtUtc = DateTime.UtcNow.AddSeconds(-0.3);
                return true;
            }
            catch { return false; }
        }

        // Anti-conflits: signale si un CC macro est en file sur un GUID donné
        public static bool HasPendingCCOn(ulong guid)
        {
            try
            {
                if (guid == 0UL) return false;
                if (string.IsNullOrEmpty(_queuedSpell)) return false;
                if (!(_queuedSpell == "Gouge" || _queuedSpell == "Blind" || _queuedSpell == "Redirect Kidney")) return false;
                double macroDelay = 0.0; try { macroDelay = Math.Max(0.0, S.MacroDelay); } catch { }
                if ((DateTime.UtcNow - _queuedAtUtc).TotalSeconds > (macroDelay > 0 ? macroDelay : 2.0)) return false;

                // Simplifié: compare juste current target ou focus
                WoWUnit target = null; try { if (InputPoller.FocusMacroHeld) target = GetFocus(); } catch { }
                if (target == null) target = Me != null ? Me.CurrentTarget as WoWUnit : null;
                return target != null && target.Guid == guid;
            }
            catch { return false; }
        }

        private static WoWUnit GetFocus() { return GetUnitByLua("focus"); }
    private static WoWUnit GetMouseover() { return GetUnitByLua("mouseover"); }
        private static WoWUnit GetUnitByLua(string unitId)
        {
            try
            {
                string guid = Lua.GetReturnVal<string>(string.Format("if UnitExists('{0}') then return UnitGUID('{0}') else return '' end", unitId), 0);
                if (string.IsNullOrEmpty(guid)) return null;
                ulong g;
                if (guid.StartsWith("0x", StringComparison.OrdinalIgnoreCase) && ulong.TryParse(guid.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out g))
                    return Styx.WoWInternals.ObjectManager.GetObjectByGuid<WoWUnit>(g);
                if (ulong.TryParse(guid, out g)) return Styx.WoWInternals.ObjectManager.GetObjectByGuid<WoWUnit>(g);
            }
            catch { }
            return null;
        }

        private static bool IsAttackableUnit(WoWUnit u)
        {
            try { return u != null && u.IsValid && u.IsAlive && u.Attackable && !u.IsFriendly; } catch { return false; }
        }

        private static string _focusUnitId = "focus"; // compatibility storage
        public static string FocusUnitId { get { return _focusUnitId; } }
        public static void SetFocusUnitId(string unitId)
        {
            if (string.IsNullOrEmpty(unitId)) unitId = "focus";
            _focusUnitId = unitId;
        }

        // Public helper for consumers: resolve target by focus/mouseover/target
        public static WoWUnit ResolveMacroUnit()
        {
            try
            {
                if (InputPoller.FocusMacroHeld)
                {
                    var f = GetFocus();
                    if (IsAttackableUnit(f)) return f;
                }
            }
            catch { }
            try
            {
                var mo = GetMouseover();
                if (IsAttackableUnit(mo)) return mo;
            }
            catch { }
            try
            {
                var t = Me != null ? Me.CurrentTarget as WoWUnit : null;
                if (IsAttackableUnit(t)) return t;
            }
            catch { }
            return null;
        }
    }
}
