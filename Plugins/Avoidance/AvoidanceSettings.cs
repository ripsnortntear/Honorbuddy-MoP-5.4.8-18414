using System.IO;
using Styx.Common;
using Styx.Helpers;

namespace Avoidance
{
    public class AvoidanceSettings : Settings
    {
        private static AvoidanceSettings _instance;
        public static AvoidanceSettings Instance
        {
            get { return _instance ?? (_instance = new AvoidanceSettings()); }
        }

        public AvoidanceSettings()
            : base(Path.Combine(Settings.SettingsDirectory, "Avoidance-Settings.xml"))
        {
        }

        [Setting, DefaultValue(true)]
        public bool Enabled { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseClickToMove { get; set; }

        [Setting, DefaultValue(true)]
        public bool RestoreClickToMove { get; set; }

        [Setting, DefaultValue(2.5f)]
        public float SafetyBuffer { get; set; }

        [Setting, DefaultValue(true)]
        public bool AvoidDynamicObjects { get; set; }

        [Setting, DefaultValue(true)]
        public bool AvoidAreaTriggers { get; set; }

        [Setting, DefaultValue(true)]
        public bool AvoidGroundAuras { get; set; }

        [Setting, DefaultValue(false)]
        public bool OnlyInCombat { get; set; }

        [Setting, DefaultValue(false)]
        public bool DebugLogging { get; set; }

        [Setting, DefaultValue(150)]
        public int ThrottleMs { get; set; }

        [Setting, DefaultValue("26573, 81297, 110701, 118283, Consecration")]
        public string CustomWhitelist { get; set; }
    }
}
