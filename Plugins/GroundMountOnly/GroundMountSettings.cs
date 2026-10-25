using System;
using System.IO;
using System.Reflection;
using Styx;
using Styx.Common;
using Styx.Helpers;

namespace Plugins.GroundMountOnly
{
    public class GroundMountSettings : Settings
    {
        private static GroundMountSettings _instance;

        public static GroundMountSettings Instance
        {
            get { return _instance ?? (_instance = new GroundMountSettings()); }
        }

        public GroundMountSettings()
            : base(GetSettingsPath())
        {
        }

        private static string GetSettingsPath()
        {
            string baseDir = Utilities.AssemblyDirectory;
            string settingsDir = Path.Combine(baseDir, "Settings");

            try
            {
                if (StyxWoW.Me != null && StyxWoW.Me.IsValid && !string.IsNullOrEmpty(StyxWoW.Me.Name))
                {
                    string charDir = Path.Combine(settingsDir, StyxWoW.Me.RealmName, StyxWoW.Me.Name);
                    if (!Directory.Exists(charDir))
                    {
                        Directory.CreateDirectory(charDir);
                    }
                    return Path.Combine(charDir, "GroundMountSettings.xml");
                }
            }
            catch
            {
            }

            if (!Directory.Exists(settingsDir))
            {
                Directory.CreateDirectory(settingsDir);
            }
            return Path.Combine(settingsDir, "GroundMountSettings.xml");
        }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        public bool UseGroundMountsOnly { get; set; }
    }
}
