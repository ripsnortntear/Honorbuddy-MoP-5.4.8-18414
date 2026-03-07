using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Utilities;
using FuryUnleashed.Interfaces.Settings;
using Styx.Common;
using Styx.WoWInternals;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace FuryUnleashed.Core.Managers
{
    internal static class HotKeyManager
    {
        public static bool IsPaused { get; private set; }
        public static bool IsCooldown { get; private set; }
        public static bool IsAoe { get; private set; }

        public delegate T Selection<out T>(object context);

        private static void LogKey(string kType, Keys kValue, ModifierKeys kModifier, bool kResult)
        {
            Logger.DiagLogPu("{0}-key ({1} + {2}) pressed, set to: {3}", kType, kValue, kModifier, kResult);
        }

        /* Keystates - One press with Spell Queueing */
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        public static bool IsKeyAsyncDown(Keys key)
        {
            return (GetAsyncKeyState(key)) != 0;
        }

        /* Keystates - One press without Spell Queueing */
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int keyCode);
        private static Enum.KeyStates GetKeyState(Keys key)
        {
            Enum.KeyStates state = Enum.KeyStates.None;

            short retVal = GetKeyState((int)key);
            if ((retVal & 0x8000) == 0x8000)
                state |= Enum.KeyStates.Down;

            return state;
        }

        public static bool IsKeyDown(Keys key)
        {
            return Enum.KeyStates.Down == (GetKeyState(key) & Enum.KeyStates.Down);
        }

        public static void RegisterKeys()
        {
            if (SettingsH.Instance.ModeSelection == Enum.Mode.Hotkey || SettingsH.Instance.ModeSelection == Enum.Mode.SemiHotkey)
            {
                if (SettingsH.Instance.PauseKeyChoice != Keys.None)
                {
                    HotkeysManager.Register("Pause", SettingsH.Instance.PauseKeyChoice, SettingsH.Instance.ModKeyChoice, hk =>
                    {
                        IsPaused = !IsPaused;
                        LogKey("Pause", SettingsH.Instance.PauseKeyChoice, SettingsH.Instance.ModKeyChoice, IsPaused);
                        if (InternalSettings.Instance.General.CheckHotkeyChatOutput)
                            Lua.DoString(IsPaused
                                ? @"print('Rotation \124cFFE61515 Paused!')"
                                : @"print('Rotation \124cFF15E61C Resumed!')");
                    });
                }

                if (SettingsH.Instance.CooldownKeyChoice != Keys.None)
                {
                    HotkeysManager.Register("Cooldown", SettingsH.Instance.CooldownKeyChoice, SettingsH.Instance.ModKeyChoice, hk =>
                    {
                        IsCooldown = !IsCooldown;
                        LogKey("Cooldown", SettingsH.Instance.CooldownKeyChoice, SettingsH.Instance.ModKeyChoice, IsCooldown);
                        if (InternalSettings.Instance.General.CheckHotkeyChatOutput && !IsPaused)
                            Lua.DoString(IsCooldown
                                ? @"print('Cooldowns \124cFF15E61C Enabled!')"
                                : @"print('Cooldowns \124cFFE61515 Disabled!')");
                    });
                }

                if (SettingsH.Instance.MultiTgtKeyChoice != Keys.None)
                {
                    HotkeysManager.Register("AoE", SettingsH.Instance.MultiTgtKeyChoice, SettingsH.Instance.ModKeyChoice, hk =>
                    {
                        IsAoe = !IsAoe;
                        LogKey("AoE", SettingsH.Instance.MultiTgtKeyChoice, SettingsH.Instance.ModKeyChoice, IsAoe);
                        if (InternalSettings.Instance.General.CheckHotkeyChatOutput && !IsPaused)
                            Lua.DoString(IsAoe
                                ? @"print('Aoe \124cFF15E61C Enabled!')"
                                : @"print('Aoe \124cFFE61515 Disabled!')");
                    });
                }

                Logger.DiagLogPu("Fury Unleashed: Hotkeys registered with the following values: {0} as Pause Key, {1} as Cooldown Key, {2} as AoE Key and {3} as Modifier Key.",
                    SettingsH.Instance.PauseKeyChoice,
                    SettingsH.Instance.CooldownKeyChoice,
                    SettingsH.Instance.MultiTgtKeyChoice,
                    SettingsH.Instance.ModKeyChoice);
            }
        }

        public static void RemoveAllKeys()
        {
            HotkeysManager.Unregister("Pause");
            HotkeysManager.Unregister("Cooldown");
            HotkeysManager.Unregister("AoE");
            Logger.DiagLogPu("Fury Unleashed: Hotkeys removed!");
        }

        #region ManualCastPause
        private static string KeySystemtoKeyBind(string key)
        {
            switch (key)
            {
                case "D1":
                    return "1";
                case "D2":
                    return "2";
                case "D3":
                    return "3";
                case "D4":
                    return "4";
                case "D5":
                    return "5";
                case "D6":
                    return "6";
                case "D7":
                    return "7";
                case "D8":
                    return "8";
                case "D9":
                    return "9";
                case "D0":
                    return "0";
                case "OemMinus":
                    return "-";
                case "Oemplus":
                    return "=";
                default:
                    return key;
            }
        }

        private static System.Timers.Timer _hotkeyTimer;

        public static void HotkeyTimer(int tickingtime)
        {
            _hotkeyTimer = new System.Timers.Timer(tickingtime);
            _hotkeyTimer.Elapsed += OnTimedEvent;
            _hotkeyTimer.AutoReset = false;
            _hotkeyTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            InitializeBindings();
        }

        internal static void InitializeBindings()
        {
            if (InternalSettings.Instance.General.AutoDetectManualCast)
            {
                foreach (Keys key in System.Enum.GetValues(typeof(Keys)))
                {
                    var blackListKey = Lua.GetReturnVal<string>("return GetBindingAction('" + KeySystemtoKeyBind(key.ToString()) + "')", 0);

                    if (HashSets.MovementKeysHash.Contains(blackListKey))
                    {
                        HashSets.MovementKey.Add(key);
                    }
                }
            }
        }

        internal static bool AnyKeyPressed()
        {
            foreach (Keys key in System.Enum.GetValues(typeof(Keys)))
            {
                if (GetAsyncKeyState(key) != 0 &&
                    key != Keys.LButton &&
                    key != Keys.RButton &&
                    key != Keys.LWin &&
                    key != Keys.RWin &&
                    key != Keys.ShiftKey &&
                    key != Keys.LShiftKey &&
                    key != Keys.RShiftKey &&
                    key != Keys.ControlKey &&
                    key != Keys.LControlKey &&
                    key != Keys.RControlKey &&
                    key != Keys.Menu &&
                    key != Keys.LMenu &&
                    key != Keys.RMenu &&
                    key != Keys.Tab &&
                    key != Keys.CapsLock &&
                    !HashSets.MovementKey.Contains(key))
                {
                    Logger.CombatLogWh("FU: Auto Pause on Manual Cast: Key press detected - Combat Routine Pause for {0} ms", InternalSettings.Instance.General.ResumeTime);
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}