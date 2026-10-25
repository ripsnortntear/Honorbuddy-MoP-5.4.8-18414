//!CompilerOption:AddRef:System.Xaml.dll
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.Plugins;

namespace WindowAndBotPersistence
{
    public class WindowAndBotPersistencePlugin : HBPlugin
    {
        public override string Name { get { return "Window & Bot Persistence"; } }
        public override string Author { get { return "Antigravity"; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Settings"; } }

        private static bool _initialized;
        private static Window _mainWindow;
        private static ComboBox _botSelector;
        private static DispatcherTimer _debounceTimer;
        private static DispatcherTimer _retryTimer;
        private static int _retryCount;

        private static double _lastNormalLeft = -1;
        private static double _lastNormalTop = -1;
        private static double _lastNormalWidth = -1;
        private static double _lastNormalHeight = -1;

        static WindowAndBotPersistencePlugin()
        {
            HookGui();
        }

        public WindowAndBotPersistencePlugin()
        {
            HookGui();
        }

#pragma warning disable 672
        public override void Initialize()
        {
            EnsurePluginEnabled();
            HookGui();
        }
#pragma warning restore 672

        public override void Pulse()
        {
            // Passive monitoring - event driven
        }

        public override void OnButtonPress()
        {
            try
            {
                var form = new SettingsForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }

        public override void OnEnable()
        {
            HookGui();
        }

        public override void OnDisable()
        {
        }

#pragma warning disable 672
        public override void Dispose()
        {
            SaveWindowPosition();
        }
#pragma warning restore 672

        private static void EnsurePluginEnabled()
        {
            try
            {
                if (CharacterSettings.Instance.EnabledPlugins != null)
                {
                    if (!CharacterSettings.Instance.EnabledPlugins.Contains("Window & Bot Persistence"))
                    {
                        var list = CharacterSettings.Instance.EnabledPlugins.ToList();
                        list.Add("Window & Bot Persistence");
                        CharacterSettings.Instance.EnabledPlugins = list.ToArray();
                    }
                }
            }
            catch
            {
            }
        }

        public static void HookGui()
        {
            if (_initialized)
                return;

            var app = Application.Current;
            if (app == null)
                return;

            app.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                try
                {
                    _mainWindow = app.MainWindow;
                    if (_mainWindow == null)
                        return;

                    _initialized = true;

                    // Locate cmbBotSelector
                    _botSelector = _mainWindow.FindName("cmbBotSelector") as ComboBox;
                    if (_botSelector == null)
                    {
                        var field = _mainWindow.GetType().GetField("cmbBotSelector", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (field != null)
                        {
                            _botSelector = field.GetValue(_mainWindow) as ComboBox;
                        }
                    }

                    // Reload settings for current character
                    WindowAndBotPersistenceSettings.ReloadInstance();
                    var settings = WindowAndBotPersistenceSettings.Instance;

                    // Restore Window Position & Size
                    RestoreWindowLocation(_mainWindow, settings);

                    // Restore BotBase
                    TryRestoreBotBase();

                    // Hook window events for position tracking
                    _mainWindow.LocationChanged += MainWindow_LocationChanged;
                    _mainWindow.SizeChanged += MainWindow_SizeChanged;
                    _mainWindow.StateChanged += MainWindow_StateChanged;
                    _mainWindow.Closing += MainWindow_Closing;

                    // Hook bot selector
                    if (_botSelector != null)
                    {
                        _botSelector.SelectionChanged += BotSelector_SelectionChanged;
                    }

                    Logging.Write("[Window & Bot Persistence] Initialized and active.");
                }
                catch (Exception ex)
                {
                    Logging.WriteException(ex);
                }
            }));
        }

        private static void RestoreWindowLocation(Window mw, WindowAndBotPersistenceSettings settings)
        {
            if (!settings.RememberWindowPosition)
                return;

            try
            {
                double x = settings.WindowX;
                double y = settings.WindowY;
                double w = settings.WindowWidth;
                double h = settings.WindowHeight;
                WindowState state = settings.WindowState;

                if (w >= 200 && h >= 150 && x > -5000 && y > -5000)
                {
                    bool onScreen = IsOnScreen(x, y, w, h);
                    if (!onScreen)
                    {
                        var primary = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                        x = primary.Left + (primary.Width - w) / 2;
                        y = primary.Top + (primary.Height - h) / 2;
                    }

                    mw.WindowStartupLocation = WindowStartupLocation.Manual;
                    mw.Left = x;
                    mw.Top = y;
                    mw.Width = w;
                    mw.Height = h;

                    _lastNormalLeft = x;
                    _lastNormalTop = y;
                    _lastNormalWidth = w;
                    _lastNormalHeight = h;

                    if (state == WindowState.Maximized)
                    {
                        mw.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        mw.WindowState = WindowState.Normal;
                    }

                    Logging.Write("[Window & Bot Persistence] Restored window: ({0}, {1}) {2}x{3} [{4}]",
                        (int)x, (int)y, (int)w, (int)h, state);
                }
                else
                {
                    // No valid character settings saved yet; if currently maximized, revert to clean normal bounds
                    if (mw.WindowState == WindowState.Maximized)
                    {
                        mw.WindowState = WindowState.Normal;
                    }
                    if (mw.Width > 1200 || mw.Height > 900)
                    {
                        mw.Width = 550;
                        mw.Height = 450;
                    }
                    _lastNormalLeft = mw.Left;
                    _lastNormalTop = mw.Top;
                    _lastNormalWidth = mw.Width;
                    _lastNormalHeight = mw.Height;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }

        public static void ResetMainWindowBounds(double x, double y, double w, double h)
        {
            if (_mainWindow == null)
                return;

            _mainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    _mainWindow.WindowState = WindowState.Normal;
                    _mainWindow.Left = x;
                    _mainWindow.Top = y;
                    _mainWindow.Width = w;
                    _mainWindow.Height = h;
                    _lastNormalLeft = x;
                    _lastNormalTop = y;
                    _lastNormalWidth = w;
                    _lastNormalHeight = h;
                    SaveWindowPosition();
                }
                catch (Exception ex)
                {
                    Logging.WriteException(ex);
                }
            }));
        }

        private static bool IsOnScreen(double x, double y, double w, double h)
        {
            try
            {
                foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                {
                    var area = screen.WorkingArea;
                    if (x < area.Right - 50 && x + w > area.Left + 50 &&
                        y < area.Bottom - 50 && y + h > area.Top + 50)
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        private static void TryRestoreBotBase()
        {
            var settings = WindowAndBotPersistenceSettings.Instance;
            if (!settings.RememberBotBase)
                return;

            if (_botSelector == null)
                return;

            string targetName = settings.LastBotBase;
            if (string.IsNullOrEmpty(targetName))
            {
                targetName = CharacterSettings.Instance.SelectedBotName;
            }

            if (string.IsNullOrEmpty(targetName))
                return;

            if (_botSelector.Items.Count == 0)
            {
                ScheduleBotRestoreRetry();
                return;
            }

            bool matched = false;
            foreach (var item in _botSelector.Items)
            {
                BotBase bot = item as BotBase;
                if (bot != null && string.Equals(bot.Name, targetName, StringComparison.OrdinalIgnoreCase))
                {
                    if (_botSelector.SelectedItem != bot)
                    {
                        _botSelector.SelectedItem = bot;
                        try
                        {
                            BotManager.Instance.SetCurrent(bot);
                        }
                        catch
                        {
                        }
                    }
                    CharacterSettings.Instance.SelectedBotName = bot.Name;
                    CharacterSettings.Instance.SelectedBotIndex = -1;
                    CharacterSettings.Instance.Save();
                    matched = true;
                    Logging.Write("[Window & Bot Persistence] Restored BotBase to '{0}'.", bot.Name);
                    break;
                }
            }

            if (!matched && _retryCount < 6)
            {
                ScheduleBotRestoreRetry();
            }
        }

        private static void ScheduleBotRestoreRetry()
        {
            if (_retryTimer == null)
            {
                _retryTimer = new DispatcherTimer();
                _retryTimer.Interval = TimeSpan.FromMilliseconds(500);
                _retryTimer.Tick += (s, e) =>
                {
                    _retryCount++;
                    if (_retryCount > 6)
                    {
                        _retryTimer.Stop();
                        return;
                    }
                    if (_botSelector != null && _botSelector.Items.Count > 0)
                    {
                        _retryTimer.Stop();
                        TryRestoreBotBase();
                    }
                };
            }
            _retryTimer.Start();
        }

        private static void BotSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_botSelector != null)
                {
                    BotBase bot = _botSelector.SelectedItem as BotBase;
                    if (bot != null)
                    {
                        var settings = WindowAndBotPersistenceSettings.Instance;
                        if (settings.RememberBotBase)
                        {
                            settings.LastBotBase = bot.Name;
                            settings.Save();
                        }
                        CharacterSettings.Instance.SelectedBotName = bot.Name;
                        CharacterSettings.Instance.SelectedBotIndex = -1;
                        CharacterSettings.Instance.Save();
                        Logging.WriteDiagnostic("[Window & Bot Persistence] Saved active BotBase: {0}", bot.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }

        private static void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            UpdateNormalBounds();
            TriggerDebouncedSave();
        }

        private static void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateNormalBounds();
            TriggerDebouncedSave();
        }

        private static void MainWindow_StateChanged(object sender, EventArgs e)
        {
            TriggerDebouncedSave();
        }

        private static void UpdateNormalBounds()
        {
            if (_mainWindow != null && _mainWindow.WindowState == WindowState.Normal)
            {
                _lastNormalLeft = _mainWindow.Left;
                _lastNormalTop = _mainWindow.Top;
                _lastNormalWidth = _mainWindow.Width;
                _lastNormalHeight = _mainWindow.Height;
            }
        }

        private static void TriggerDebouncedSave()
        {
            if (_debounceTimer == null)
            {
                _debounceTimer = new DispatcherTimer();
                _debounceTimer.Interval = TimeSpan.FromSeconds(1);
                _debounceTimer.Tick += (s, e) =>
                {
                    _debounceTimer.Stop();
                    SaveWindowPosition();
                };
            }
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private static void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowPosition();
        }

        private static void SaveWindowPosition()
        {
            if (_mainWindow == null)
                return;

            try
            {
                var settings = WindowAndBotPersistenceSettings.Instance;
                if (!settings.RememberWindowPosition)
                    return;

                WindowState state = _mainWindow.WindowState;
                if (state == WindowState.Minimized)
                {
                    state = WindowState.Normal;
                }

                settings.WindowState = state;

                if (_mainWindow.WindowState == WindowState.Normal)
                {
                    settings.WindowX = _mainWindow.Left;
                    settings.WindowY = _mainWindow.Top;
                    settings.WindowWidth = _mainWindow.Width;
                    settings.WindowHeight = _mainWindow.Height;
                }
                else if (_lastNormalWidth > 0 && _lastNormalHeight > 0)
                {
                    settings.WindowX = _lastNormalLeft;
                    settings.WindowY = _lastNormalTop;
                    settings.WindowWidth = _lastNormalWidth;
                    settings.WindowHeight = _lastNormalHeight;
                }

                settings.Save();

                // Sync to UISettings so Honorbuddy's internal SaveLocation doesn't re-maximize
                UISettings.Instance.MainWindowLocationX = (int)settings.WindowX;
                UISettings.Instance.MainWindowLocationY = (int)settings.WindowY;
                UISettings.Instance.MainWindowWidth = (int)settings.WindowWidth;
                UISettings.Instance.MainWindowHeight = (int)settings.WindowHeight;
                UISettings.Instance.MainWindowState = settings.WindowState;
                UISettings.Instance.Save();
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }
    }
}
