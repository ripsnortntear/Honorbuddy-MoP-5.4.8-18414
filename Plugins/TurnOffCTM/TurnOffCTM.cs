/*----------------------------------------------------
 * TurnOffCTM - Developped by ZenLulz (on thebuddyforum.com)
 * SVN : https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/Plugins/TurnOffCTM/
 * Note : This is a free plugin, and could not be sold, or repackaged.
 * Version : 1.1
 ----------------------------------------------------*/


using System;
using System.Net;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;

namespace TurnOffCTM
{

    public class TurnOffCtm : HBPlugin
    {
        public override string Name { get { return "TurnOffCTM"; } }
        public override string Author { get { return "ZenLulz"; } }
        public override Version Version { get { return new Version(1, 1); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText{ get { return "Settings"; } }
        private readonly PluginSettings _settings = PluginSettings.Instance;
        /// <summary>
        /// Returns the current type of turn off.
        /// </summary>
        private TurnOffTypes CurrentType
        {
            get
            {
                _settings.Load();
                return _settings.TurnOffType;
            }
        }

        /// <summary>
        /// Returns if the plugin is up to date.
        /// </summary>
        private bool IsUpToDate
        {
            get
            {
                try
                {
                    var remoteVersion = new Version((new WebClient()).DownloadString("https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/Plugins/TurnOffCTM/VERSION"));
                    return (Version >= remoteVersion);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// No pulse.
        /// </summary>
        public override void Pulse()
        {
        }

        /// <summary>
        /// When the button of settings is pressed.
        /// </summary>
        public override void OnButtonPress()
        {
            (new Gui.Settings()).ShowDialog();
        }

        /// <summary>
        /// The plugin initialization procedure.
        /// </summary>
        public override void Initialize()
        {
            BotEvents.OnBotStarted += BotEventsOnOnBotStarted;
            BotEvents.OnBotStopped += BotEventsOnBotStopped;
            WriteLog("Plugin enabled (Ver. {0})", Version);
            if(!IsUpToDate)
                WriteLog("An update is available, check thebuddyforum.com to get the latest version");
            
        }

        /// <summary>
        /// When the bot is started.
        /// </summary>
        private void BotEventsOnOnBotStarted(EventArgs args)
        {
            _settings.Load();
            if(_settings.BotException && _settings.BotName.Equals(TreeRoot.Current.Name))
                DisableCtm();
        }

        /// <summary>
        /// When the bot is stopped.
        /// </summary>
        void BotEventsOnBotStopped(EventArgs args)
        {
            if(CurrentType == TurnOffTypes.OnBotStopped)
                DisableCtm();
        }

        /// <summary>
        /// The plugin dispose procedure.
        /// </summary>
        public override void Dispose()
        {
            if (CurrentType == TurnOffTypes.OnBotExited)
                DisableCtm();
            BotEvents.OnBotStarted -= BotEventsOnOnBotStarted;
            BotEvents.OnBotStopped -= BotEventsOnBotStopped;
            WriteLog("Plugin disabled");
        }

        /// <summary>
        /// Removes the Click-To-Move flag.
        /// </summary>
        private void DisableCtm()
        {
            WriteLog("CTM disabled");
            Lua.DoString("SetCVar('autoInteract', '0')");
        }

        /// <summary>
        /// Writes a message in the console log.
        /// </summary>
        private void WriteLog(string text, params object[] args)
        {
            Logging.Write("[TurnOffCTM]: " + text, args);
        }
   }

    /// <summary>
    /// The list of possible desactivation of the CTM.
    /// </summary>
    public enum TurnOffTypes
    {
        OnBotStopped,
        OnBotExited
    }
}

