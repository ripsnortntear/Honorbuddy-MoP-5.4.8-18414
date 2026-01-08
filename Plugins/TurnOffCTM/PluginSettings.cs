using System.IO;
using Styx.Helpers;

namespace TurnOffCTM
{
    public class PluginSettings : Settings
    {
        /// <summary>
        /// The singleton of the plugin settings.
        /// </summary>
        public static readonly PluginSettings Instance = new PluginSettings();

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        public PluginSettings()
            : base(Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), @"Settings/TurnOffCTM-Settings.xml"))
        {
        }

        /// <summary>
        /// The type of the desactivation.
        /// </summary>
        [Setting, DefaultValue(TurnOffTypes.OnBotStopped)]
        public TurnOffTypes TurnOffType { get; set; }

        /// <summary>
        /// Disables the CTM if the bot exception is enabled.
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool BotException { get; set; }

        /// <summary>
        /// The name of the botbase for the CTM desactivation.
        /// </summary>
        [Setting, DefaultValue("")]
        public string BotName { get; set; }
    }
}
