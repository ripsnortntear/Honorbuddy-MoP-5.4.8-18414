#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-30 16:12:12 +0200 (Do, 30 Mai 2013) $
 * $ID$
 * $Revision: 1478 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Managers/HotKeyManager.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Settings.Settings;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Lua = Styx.WoWInternals.Lua;


namespace PureRotation.Managers
{
    public static class HotKeyManager
    {
        public delegate T Selection<out T>(object context);

        public static bool IsPaused { get; private set; }

        public static bool IsCooldown { get; private set; }

        public static bool IsAoe { get; private set; }

        public static bool IsSpecialKey { get; private set; }

        public static bool IsRotationKey { get; private set; }

        public static void BotEvents_OnBotStarted(EventArgs args)
        {
            if (RoutineManager.Current.Name != PureRotationRoutine._name) return;
            if (HotkeySettings.Instance.ModeChoice == Mode.Hotkey || HotkeySettings.Instance.ModeChoice == Mode.SemiAuto)
                RegisterKeys();
            Logger.InfoLog(string.Format("PureRotation Hotkeys registered!"));
            /*
            if (!DpsMeter.DpsMeterInitialized)
            {
                DpsMeter.Initialize();
                    if (DpsMeter.DpsMeterInitialized)
                    Logger.DebugLog(string.Format("DpsMeter initialized!"));
            }
                */
            Logger.InfoLog(string.Format("PureRotation started with {0}! Please Enjoy :)", BotManager.Current.Name));
        }

        private static void LogKey(string type, Keys kValue, ModifierKeys kModifier, bool kResult)
        {
            Logging.WriteDiagnostic("{0}-key ({1} + {2}) pressed, set to: {3}",
                type,
                kValue,
                kModifier,
                kResult);
        }

        public static void BotEvents_OnBotStopped(EventArgs args)
        {
            if (RoutineManager.Current.Name != PureRotationRoutine._name) return;
            RemoveAllKeys();
            
            if (DpsMeter.DpsMeterInitialized)
            {
                DpsMeter.Shutdown();
                if (!DpsMeter.DpsMeterInitialized)
                    Logger.DebugLog(string.Format("DpsMeter cleared!"));
            }
                
            if (DoTTracker.Initialized)
            {
                DoTTracker.Shutdown();
                if (!DoTTracker.Initialized)
                    Logger.DebugLog(string.Format("DoTTracker cleared!"));
            }
            Logging.WriteDiagnostic("PureRotation: stopped!");
        }

        public static void RegisterKeys()
        {

            if (HotkeySettings.Instance.ModeChoice == Mode.SemiAuto || HotkeySettings.Instance.ModeChoice == Mode.Hotkey)
            {

                #region Pause
                HotkeysManager.Register("PRPause", HotkeySettings.Instance.PauseKeyChoice,
                                                HotkeySettings.Instance.ModKeyChoice, hk =>
                                                    {
                                                        IsPaused = !IsPaused;
                                                        LogKey("Pause", HotkeySettings.Instance.PauseKeyChoice,
                                                               HotkeySettings.Instance.ModKeyChoice, IsPaused);
                                                        if (PRSettings.Instance.EnableWoWChatOutput)
                                                            Lua.DoString(IsPaused
                                                                             ? @"print('Rotation \124cFFE61515 Paused!')"
                                                                             : @"print('Rotation \124cFF15E61C Resumed!')");
                                                    });
                #endregion

                #region Cooldown
                HotkeysManager.Register("PRCooldown", HotkeySettings.Instance.CooldownKeyChoice,
                                                HotkeySettings.Instance.ModKeyChoice, hk =>
                                                    {
                                                        IsCooldown = !IsCooldown;
                                                        LogKey("Cooldown", HotkeySettings.Instance.CooldownKeyChoice,
                                                               HotkeySettings.Instance.ModKeyChoice, IsCooldown);
                                                        if (PRSettings.Instance.EnableWoWChatOutput && !IsPaused)
                                                            Lua.DoString(IsCooldown
                                                                             ? @"print('Cooldowns \124cFF15E61C Enabled!')"
                                                                             : @"print('Cooldowns \124cFFE61515 Disabled!')");
                                                    });

                #endregion

                #region AoE
                if (HotkeySettings.Instance.ModeChoice == Mode.Hotkey)
                {
                    HotkeysManager.Register("PRAoe", HotkeySettings.Instance.SwitchKeyChoice,
                                            HotkeySettings.Instance.ModKeyChoice, hk =>
                                                {
                                                    IsAoe = !IsAoe;
                                                    LogKey("AOE", HotkeySettings.Instance.SwitchKeyChoice,
                                                           HotkeySettings.Instance.ModKeyChoice, IsAoe);
                                                    if (PRSettings.Instance.EnableWoWChatOutput && !IsPaused)
                                                        Lua.DoString(IsAoe
                                                                         ? @"print('AoE \124cFF15E61C Enabled!')"
                                                                         : @"print('AoE \124cFFE61515 Disabled!')");
                                                });
                }
                #endregion

                #region Special Key
                HotkeysManager.Register("PRSpecial", HotkeySettings.Instance.SpecialKeyChoice,
                                                HotkeySettings.Instance.ModKeyChoice, hk =>
                                                    {
                                                        IsSpecialKey = !IsSpecialKey;
                                                        LogKey("Special", HotkeySettings.Instance.SpecialKeyChoice,
                                                               HotkeySettings.Instance.ModKeyChoice, IsSpecialKey);
                                                        if (PRSettings.Instance.EnableWoWChatOutput && !IsPaused)
                                                            Lua.DoString(IsSpecialKey
                                                                             ? @"print('Special \124cFF15E61C Enabled!')"
                                                                             : @"print('Special \124cFFE61515 Disabled!')");
                                                    });
                #endregion

                #region Rotation Selector
                HotkeysManager.Register("PRRotation", HotkeySettings.Instance.RotationKeyChoice,
                                                HotkeySettings.Instance.ModKeyChoice, hk =>
                                                    {
                                                        IsRotationKey = !IsRotationKey;
                                                        LogKey("Rotation", HotkeySettings.Instance.RotationKeyChoice,
                                                               HotkeySettings.Instance.ModKeyChoice, IsRotationKey);
                                                        if (PRSettings.Instance.EnableWoWChatOutput && !IsPaused)
                                                            Lua.DoString(IsRotationKey
                                                                             ? @"print('Custom Rotation \124cFF15E61C Enabled!')"
                                                                             : @"print('Custom Rotation \124cFFE61515 Disabled!')");
                                                    });
                #endregion

                Logging.WriteDiagnostic(
                            "PureRotation: Hotkeys registered. Configured ModifierKey: {0}, PauseKey: {1}, CooldownKey: {2}, AOEKey: {3}, SpecialKey: {4} , RotationKeyChoice  {5}",
                            HotkeySettings.Instance.ModKeyChoice,
                            HotkeySettings.Instance.PauseKeyChoice,
                            HotkeySettings.Instance.CooldownKeyChoice,
                            HotkeySettings.Instance.SwitchKeyChoice,
                            HotkeySettings.Instance.SpecialKeyChoice,
                            HotkeySettings.Instance.RotationKeyChoice);
                       
                }

            //Logging.WriteDiagnostic("PureRotation: Hotkeys registered!");
        }

        public static void RemoveAllKeys()
        {
            HotkeysManager.Unregister("PRPause");
            HotkeysManager.Unregister("PRCooldown");
            HotkeysManager.Unregister("PRAoe");
            HotkeysManager.Unregister("PRSpecial");
            HotkeysManager.Unregister("PRRotation");
            Logging.WriteDiagnostic("PureRotation: Hotkeys Removed!");
        }

        public static Composite DeactivateSpecialKey(Selection<bool> reqs = null)
        {
            return new Decorator(ret => IsSpecialKey && (reqs != null && reqs(ret)) || (reqs == null),
                new Styx.TreeSharp.Action(delegate
                    {
                            IsSpecialKey = false;
                            LogKey("Special", HotkeySettings.Instance.SpecialKeyChoice, HotkeySettings.Instance.ModKeyChoice, IsSpecialKey);
                            if (PRSettings.Instance.EnableWoWChatOutput && !IsPaused)
                                    Lua.DoString(@"print('Special \124cFFE61515 Disabled!')");
                        return RunStatus.Failure;
                    }));
        }


        internal class KeyboardPolling
        {

            /* Keystates - One press with Spell Queueing */
            [DllImport("user32.dll")]
            private static extern short GetAsyncKeyState(Keys vKey);

            public static bool IsKeyAsyncDown(Keys vKey)
            {
                return (GetAsyncKeyState(vKey)) != 0;
            }

            /* Keystates - One press without Spell Queueing */
            [DllImport("user32.dll")]
            private static extern short GetKeyState(int keyCode);

            private static KeyStates GetKeyState(Keys key)
            {
                KeyStates state = KeyStates.None;

              short retVal = GetKeyState((int)key);

              //If the high-order bit is 1, the key is down otherwise, it is up.
              if ((retVal & 0x8000) == 0x8000)
                  state |= KeyStates.Down;

              return state;
            }

            //Currently unable to get this to work with modifiers.  -worklifebalance
            //Let me know if you have a fix.  ie: [Styx.Helpers.DefaultValue(Keys.Shift | Keys.C)]

            public static bool IsKeyDown(Keys key)
            {
                return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
            }

            public static bool IsKeyToggled(Keys key)
            {
                return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
            }
        }
    }
}