using System;
using System.IO;
using System.Windows;
using Styx;
using Styx.Helpers;

namespace WindowAndBotPersistence
{
    public class WindowAndBotPersistenceSettings : Settings
    {
        private static WindowAndBotPersistenceSettings _instance;

        public static WindowAndBotPersistenceSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WindowAndBotPersistenceSettings();
                }
                return _instance;
            }
        }

        public WindowAndBotPersistenceSettings()
            : base(GetSettingsPath())
        {
            Load();
        }

        public static void ReloadInstance()
        {
            _instance = new WindowAndBotPersistenceSettings();
        }

        private static string GetSettingsPath()
        {
            string charDir = null;
            try
            {
                charDir = Settings.CharacterSettingsDirectory;
            }
            catch
            {
                // Ignored if Assembly.GetEntryAssembly() is null or StyxWoW.Me is not initialized yet
            }

            if (string.IsNullOrEmpty(charDir) || !Directory.Exists(charDir))
            {
                try
                {
                    string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
                    if (StyxWoW.Me != null && StyxWoW.Me.IsValid)
                    {
                        charDir = Path.Combine(Path.Combine(baseDir, StyxWoW.Me.RealmName), StyxWoW.Me.Name);
                        if (!Directory.Exists(charDir))
                        {
                            Directory.CreateDirectory(charDir);
                        }
                    }
                    else
                    {
                        charDir = baseDir;
                    }
                }
                catch
                {
                    charDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
                }
            }

            return Path.Combine(charDir, "WindowAndBotPersistence.xml");
        }

        [Setting]
        [DefaultValue("")]
        public string LastBotBase { get; set; }

        [Setting]
        [DefaultValue(-1.0)]
        public double WindowX { get; set; }

        [Setting]
        [DefaultValue(-1.0)]
        public double WindowY { get; set; }

        [Setting]
        [DefaultValue(-1.0)]
        public double WindowWidth { get; set; }

        [Setting]
        [DefaultValue(-1.0)]
        public double WindowHeight { get; set; }

        [Setting]
        [DefaultValue(WindowState.Normal)]
        public WindowState WindowState { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool RememberBotBase { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool RememberWindowPosition { get; set; }
    }
}
