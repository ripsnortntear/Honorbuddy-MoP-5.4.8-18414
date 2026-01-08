using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Styx;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Managers;   // OpenerType / OpenerController / MacroManager
using VitalicRotation.UI;         // VitalicUi.ShowNotify / ShowMiniBanner / ShowBigBanner
using VitalicRotation.Settings;   // VitalicSettings
using Styx.WoWInternals;          // Lua

namespace VitalicRotation.Helpers
{
    internal static class InputPoller
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static readonly Dictionary<Keys, bool> _prev = new Dictionary<Keys, bool>();
        // D) Edge-based + debounce storage
        private static readonly Dictionary<Keys, int> _lastFire = new Dictionary<Keys, int>();
        private const int DebounceMs = 120;
    // Throttled diagnostics for skip reasons
    private static DateTime _lastFgSkipLogUtc = DateTime.MinValue;
    private static DateTime _lastChatSkipLogUtc = DateTime.MinValue;

        public static bool OpenerModHeld { get; private set; }   // <-- AJOUT
        public static bool FocusMacroHeld { get; private set; }  // Maintien pour macros focus (parité v)
        private static VitalicSettings S { get { return VitalicSettings.Instance; } }

        // Timestamp dernière entrée humaine (hotkey)
        private static DateTime _lastHumanInputUtc = DateTime.MinValue;
        public static bool WasRecentHumanInput(int ms)
        {
            if (ms <= 0) return false;
            return (DateTime.UtcNow - _lastHumanInputUtc).TotalMilliseconds <= ms;
        }

        // === Manual Cast Pause detection (Class80.smethod_5 like) ===
        private static readonly Keys[] _allKeys = (Keys[])Enum.GetValues(typeof(Keys));
        private static readonly Dictionary<Keys, bool> _prevScan = new Dictionary<Keys, bool>();
        private static readonly HashSet<Keys> _ignoredKeys = new HashSet<Keys>(new[] {
            Keys.LButton, Keys.RButton, Keys.MButton, Keys.XButton1, Keys.XButton2,
            Keys.LWin, Keys.RWin, Keys.ShiftKey, Keys.LShiftKey, Keys.RShiftKey,
            Keys.ControlKey, Keys.LControlKey, Keys.RControlKey,
            Keys.Menu, Keys.LMenu, Keys.RMenu,
            Keys.Tab, Keys.Capital
        });
        private static readonly HashSet<string> _movementActions = new HashSet<string>(new[] {
            "MOVEFORWARD","MOVEBACKWARD","TURNLEFT","TURNRIGHT","STRAFELEFT","STRAFERIGHT","JUMP","TURNORACTION","CAMERAORSELECTORMOVE"
        });

        private static bool IsMovementKey(Keys key)
        {
            try
            {
                // Convert Keys to WoW binding name is unreliable; use WoW API to fetch binding action from key text
                // Fallback minimal: check common movement keys by name with GetBindingAction
                string keyName = key.ToString();
                string lua = "return GetBindingAction('" + keyName + "') or ''";
                string action = string.Empty;
                try { action = Lua.GetReturnVal<string>(lua, 0); } catch { action = string.Empty; }
                if (!string.IsNullOrEmpty(action) && _movementActions.Contains(action))
                    return true;
            }
            catch { }
            return false;
        }

        private static bool IsOurHotkey(Keys k)
        {
            try
            {
                // Exclure tous les raccourcis de la config pour éviter faux positifs
                return k == S.GarroteKeyBind || k == S.CheapShotKeyBind ||
                       k == S.KidneyShotKeyBind || k == S.RedirectKidneyKeyBind ||
                       k == S.GougeKeyBind || k == S.BlindKeyBind ||
                       k == S.SmokeBombKeyBind || k == S.RestealthKeyBind ||
                       k == S.FastKickKeyBind || k == S.EventsKeyBind ||
                       k == S.PauseDamageKeyBind || k == S.AutoKidneyKeyBind ||
                       k == S.BurstKeyBind || k == S.LazyKeyBind ||
                       k == S.PauseKeyBind || k == S.BurstNoShadowBladesKeyBind ||
                       k == S.OpenerModifierKeyBind || k == S.FocusMacroKeyBind;
            }
            catch { return false; }
        }

        private static void DetectManualCastPause()
        {
            try
            {
                int ms = 0; try { ms = S.ManualCastPause; } catch { ms = 0; }
                // Special case: Shadowmeld (58984) forces 500ms pause regardless of setting
                try
                {
                    int lastSpell = Lua.GetReturnVal<int>("return tonumber(GetSpellInfo(58984) and 58984) or 0", 0);
                    // Simplified: if last ability polled equals Shadowmeld token (proxy), enforce 500ms
                    if (lastSpell == 58984)
                        ms = Math.Max(ms, 500);
                }
                catch { }

                if (ms <= 0) return;

                for (int i = 0; i < _allKeys.Length; i++)
                {
                    Keys key = _allKeys[i];
                    bool down = (GetAsyncKeyState(key) & 0x8000) != 0;
                    bool was;
                    _prevScan.TryGetValue(key, out was);

                    if (down && !was)
                    {
                        if (_ignoredKeys.Contains(key)) { _prevScan[key] = down; continue; }
                        if (IsOurHotkey(key)) { _prevScan[key] = down; continue; }
                        if (IsMovementKey(key)) { _prevScan[key] = down; continue; }

                        // Detected: user key press (not our hotkey, not movement) => pause offense
                        EventHandlers.ArmPauseOffense(ms);
                        if (S.DiagnosticMode)
                            Logger.Write("[Manual Cast Pause] Key press {0} -> {1} ms", key, ms);
                        _prevScan[key] = down;
                        continue;
                    }

                    _prevScan[key] = down;
                }
            }
            catch { }
        }

        public static void Initialize()
        {
            _prev.Clear();
            _lastFire.Clear();
            _prevScan.Clear();
        }

        public static void Shutdown()
        {
            _prev.Clear();
            _lastFire.Clear();
            _prevScan.Clear();
        }

        // v.zip-style : n'écouter que si WoW est en avant-plan et en jeu
        private static bool IsWowForeground()
        {
            try
            {
                if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                    return false;

                IntPtr fg = GetForegroundWindow();
                IntPtr wh = StyxWoW.Memory.Process.MainWindowHandle;
                return fg == wh && wh != IntPtr.Zero;
            }
            catch
            {
                return false;
            }
        }

        // Ignore hotkeys while typing in chat/edit boxes (mirror Vitalic behavior)
        private static bool IsTypingInChat()
        {
            try { return Lua.GetReturnVal<bool>("return GetCurrentKeyBoardFocus() ~= nil", 0); }
            catch { return false; }
        }

        public static void Poll()
        {
            // IMPORTANT : pas de hotkeys quand la fenêtre WoW n'est pas active (évite les déclenchements en Alt-Tab)
            if (!IsWowForeground())
            {
                if (S.DiagnosticMode)
                {
                    var now = DateTime.UtcNow;
                    if ((now - _lastFgSkipLogUtc).TotalMilliseconds > 1000)
                    {
                        _lastFgSkipLogUtc = now;
                        try { Logger.Write("[Input] Skipping hotkeys: WoW not in foreground"); } catch { }
                    }
                }
                return;
            }

            // Do not process hotkeys while typing in chat (edit box focused)
            if (IsTypingInChat())
            {
                if (S.DiagnosticMode)
                {
                    var now = DateTime.UtcNow;
                    if ((now - _lastChatSkipLogUtc).TotalMilliseconds > 1000)
                    {
                        _lastChatSkipLogUtc = now;
                        try { Logger.Write("[Input] Skipping hotkeys: typing in chat/UI edit box"); } catch { }
                    }
                }
                return;
            }

            // Détection globale d'input humain pour Manual Cast Pause (hors nos hotkeys & mouvements)
            DetectManualCastPause();

            // état "opener modifier" (maintenu)
            OpenerModHeld = (GetAsyncKeyState(S.OpenerModifierKeyBind) & 0x8000) != 0;
            // état "focus macro" (maintenu) — utilisé par MacroManager lors de la consommation d'une macro sans token
            FocusMacroHeld = (GetAsyncKeyState(S.FocusMacroKeyBind) & 0x8000) != 0;

            // --- Openers en Stealth (miroir v.zip) ---
            CheckEdge(S.GarroteKeyBind, delegate { MacroManager.Enqueue("Garrote", StyxWoW.Me.CurrentTarget as WoWUnit); });
            CheckEdge(S.CheapShotKeyBind, delegate { MacroManager.Enqueue("Cheap Shot", StyxWoW.Me.CurrentTarget as WoWUnit); });
            // (Ambush non piloté ici dans v.zip)

            // --- CC / actions via MacroManager (noms tels qu'utilisés dans v.zip) ---
            // v.zip enfile "Kidney Shot + Shuriken" (label UI)
            // Kidney Shot + Shuriken (file -> Routine.HandleQueuedMacros)
            CheckEdge(S.KidneyShotKeyBind, delegate {
                MacroManager.Enqueue("Kidney Shot + Shuriken", StyxWoW.Me.CurrentTarget as WoWUnit);
            });
            CheckEdge(S.RedirectKidneyKeyBind, delegate { MacroManager.Enqueue("Redirect Kidney", StyxWoW.Me.CurrentTarget as WoWUnit); });
            CheckEdge(S.GougeKeyBind, delegate { MacroManager.Enqueue("Gouge", StyxWoW.Me.CurrentTarget as WoWUnit); });
            CheckEdge(S.BlindKeyBind, delegate { MacroManager.Enqueue("Blind", StyxWoW.Me.CurrentTarget as WoWUnit); });
            CheckEdge(S.SmokeBombKeyBind, delegate { MacroManager.Enqueue("Smoke Bomb", StyxWoW.Me.CurrentTarget as WoWUnit); });
            CheckEdge(S.RestealthKeyBind, delegate { MacroManager.Enqueue("Restealth", StyxWoW.Me); });

            // Fast Kick (exécution immédiate du pipeline au lieu d'enfiler une macro)
            CheckEdge(S.FastKickKeyBind, delegate {
                try
                {
                    var t = StyxWoW.Me.CurrentTarget as WoWUnit;
                    if (t != null) try { t.Target(); } catch { }
                    if (!InterruptManager.TryFastKickWithFallback())
                    {
                        // Feedback similaire à la consommation macro si l'interrupt échoue
                        VitalicUi.ShowMiniBanner("Kick indisponible", 1f, 0.4f, 0.4f, 1f);
                    }
                }
                catch { }
            });

            // Toggle UI Events (affichage notify on/off)
            CheckEdge(S.EventsKeyBind, delegate {
                EventHandlers.Enabled = !EventHandlers.Enabled;
                VitalicUi.ShowMiniBanner(EventHandlers.Enabled ? "Events: ON" : "Events: OFF", 
                    EventHandlers.Enabled ? 0.7f : 1f, 
                    EventHandlers.Enabled ? 1f : 0.4f, 
                    EventHandlers.Enabled ? 0.7f : 0.4f);
                VitalicUi.UpdateStatusAuto();
            });

            // Pause Damage (n'arrête pas les défensifs/interrupts)
            CheckEdge(S.PauseDamageKeyBind, delegate {
                ToggleState.PauseDamage = !ToggleState.PauseDamage;
                Logger.Write("[Input] Toggle PauseDamage -> {0}", ToggleState.PauseDamage ? "ON" : "OFF");
                VitalicUi.ShowMiniBanner(ToggleState.PauseDamage ? "Damage Paused" : "Damage Resumed",
                    ToggleState.PauseDamage ? 1f : 0.7f,
                    ToggleState.PauseDamage ? 0.4f : 1f,
                    ToggleState.PauseDamage ? 0.4f : 0.7f);
                VitalicUi.UpdateStatusAuto();
            });

            // Toggle AutoKidney (option settings)
            CheckEdge(S.AutoKidneyKeyBind, delegate {
                var v = !VitalicSettings.Instance.AutoKidney;
                VitalicSettings.Instance.AutoKidney = v;
                VitalicSettings.Instance.Save();
                VitalicUi.ShowMiniBanner(v ? "Auto Kidney: ON" : "Auto Kidney: OFF",
                    v ? 0.7f : 1f,
                    v ? 1f : 0.4f,
                    v ? 0.7f : 0.4f);
            });

            // --- Toggles (fidèle à v.zip) : bascule C# + slash Lua + toast ---
            CheckEdge(S.BurstKeyBind, ToggleBurst);
            CheckEdge(S.LazyKeyBind, ToggleLazy);
            CheckEdge(S.PauseKeyBind, TogglePause);

            // "Burst sans Shadow Blades" : v.zip passe par /vnoblades puis /toggle burst.
            CheckEdge(S.BurstNoShadowBladesKeyBind, ToggleBurstNoBlades);
        }

        // ===================== Helpers =====================

        private static void CheckEdge(Keys key, Action onPressed)
        {
            if (key == Keys.None || onPressed == null)
                return;

            short state = 0;
            try { state = GetAsyncKeyState(key); } catch { }
            bool down = (state & 0x8000) != 0;               // currently down
            bool pressedSinceLast = (state & 0x0001) != 0;   // pressed at least once since last call
            bool was;
            _prev.TryGetValue(key, out was);

            // Edge conditions:
            // 1) Normal down edge: down && !was
            // 2) Missed quick tap between polls: pressedSinceLast && !down (we observed a tap but it's already up)
            bool isEdge = (down && !was) || (pressedSinceLast && !down);

            if (isEdge)
            {
                int now = Environment.TickCount;
                int last;
                _lastFire.TryGetValue(key, out last);
                if (now - last >= DebounceMs)
                {
                    _lastFire[key] = now;
                    try
                    {
                        _lastHumanInputUtc = DateTime.UtcNow;
                        if (S.DiagnosticMode)
                        {
                            try { Logger.Write("[Input][Edge] {0} via {1}", key, down && !was ? "down-edge" : "pressed-since-last"); } catch { }
                        }
                        onPressed();
                    }
                    catch { }
                }
            }

            _prev[key] = down;
        }

        // Expose simple utility to query current hotkey state from elsewhere (e.g., rotations)
        public static bool IsHotkeyActive(Keys key)
        {
            try
            {
                if (key == Keys.None)
                    return false;

                // Consistent with Poll(): only consider keys while WoW window is foreground and not typing
                if (!IsWowForeground())
                    return false;
                if (IsTypingInChat())
                    return false;

                return (GetAsyncKeyState(key) & 0x8000) != 0;
            }
            catch { return false; }
        }

        // ---- Toggles : logique v.zip (état C# pour l'overlay + slash Lua si utilisé ailleurs) ----

        private static void ToggleBurst()
        {
            try
            {
                ToggleState.IsBurstOn = !ToggleState.IsBurstOn;
                Logger.Write("[Input] Toggle Burst -> {0}", ToggleState.IsBurstOn ? "ON" : "OFF");
                _lastHumanInputUtc = DateTime.UtcNow;
                VitalicUi.ShowMiniBanner(ToggleState.IsBurstOn ? "Burst Enabled" : "Burst Disabled",
                    ToggleState.IsBurstOn ? 0.7f : 1f,
                    ToggleState.IsBurstOn ? 1f : 0.4f,
                    ToggleState.IsBurstOn ? 0.7f : 0.4f);
                VitalicUi.UpdateStatusAuto();
                AudioBus.PlayEvent();
            }
            catch { }
        }

        private static void ToggleLazy()
        {
            try
            {
                ToggleState.IsLazyOn = !ToggleState.IsLazyOn;
                Logger.Write("[Input] Toggle Lazy -> {0}", ToggleState.IsLazyOn ? "ON" : "OFF");
                _lastHumanInputUtc = DateTime.UtcNow;
                VitalicUi.ShowMiniBanner(ToggleState.IsLazyOn ? "Lazy Enabled" : "Lazy Disabled",
                    ToggleState.IsLazyOn ? 0.7f : 1f,
                    ToggleState.IsLazyOn ? 1f : 0.4f,
                    ToggleState.IsLazyOn ? 0.7f : 0.4f);
                VitalicUi.UpdateStatusAuto();
                AudioBus.PlayEvent();
            }
            catch { }
        }

        private static void TogglePause()
        {
            try
            {
                ToggleState.Pause = !ToggleState.Pause;
                Logger.Write("[Input] Toggle Pause -> {0}", ToggleState.Pause ? "ON" : "OFF");
                _lastHumanInputUtc = DateTime.UtcNow;
                VitalicUi.ShowMiniBanner(ToggleState.Pause ? "Pause ON" : "Pause OFF",
                    ToggleState.Pause ? 1f : 0.7f,
                    ToggleState.Pause ? 0.4f : 1f,
                    ToggleState.Pause ? 0.4f : 0.7f);
                VitalicUi.UpdateStatusAuto();
                AudioBus.PlayEvent();
            }
            catch { }
        }

        private static void ToggleBurstNoBlades()
        {
            try
            {
                // v.zip : toggle du mode Burst sans Shadow Blades (flag interne)
                ToggleState.NoShadowBlades = !ToggleState.NoShadowBlades;
                Logger.Write("[Input] Toggle BurstNoBlades -> {0}", ToggleState.NoShadowBlades ? "ON" : "OFF");
                _lastHumanInputUtc = DateTime.UtcNow;

                // Mini banner (uniform with other toggles)
                VitalicUi.ShowMiniBanner(ToggleState.NoShadowBlades ? "No Blades: Enabled" : "No Blades: Disabled",
                    ToggleState.NoShadowBlades ? 0.7f : 1f,
                    ToggleState.NoShadowBlades ? 1f : 0.4f,
                    ToggleState.NoShadowBlades ? 0.7f : 0.4f);
                VitalicUi.UpdateStatusAuto(); // refresh instant to show/hide NoBlades
                AudioBus.PlayEvent();
            }
            catch { }
        }
    }
}
