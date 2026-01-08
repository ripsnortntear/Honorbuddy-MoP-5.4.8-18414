/*----------------------------------------------------
 * TakeControl! - Developped by ZenLulz (on thebuddyforum.com)
 * Supported WoW Version : 5.1.0
 * SVN : https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/Plugins/TakeControl/
 * Note : This is a free plugin, and could not be sold, or repackaged.
 * Version : 1.0.0 (Release)
 ----------------------------------------------------*/


using System;
using Styx.Plugins;

namespace TakeControl
{
    public class TakeControl : HBPlugin
    {
        #region Fields
        /// <summary>
        /// State if the plugin is initialized.
        /// </summary>
        private bool _isInitialized;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public override string Name
        {
            get { return "TakeControl!"; }
        }
        /// <summary>
        /// The author.
        /// </summary>
        public override string Author
        {
            get { return "ZenLulz"; }
        }
        public static Version StaticVersion
        {
            get { return new Version(1, 0, 0, 0); }
        }
        /// <summary>
        /// The version of the plugin.
        /// </summary>
        public override Version Version
        {
            get { return StaticVersion; }
        }
        /// <summary>
        /// We want a settings button.
        /// </summary>
        public override bool WantButton
        {
            get { return true; }
        }
        /// <summary>
        /// The text written on the settings button
        /// </summary>
        public override string ButtonText
        {
            get { return "Settings"; }
        }
        #endregion

        #region Methods
        #region Dispose (override)
        public override void Dispose()
        {
            // Unbind the key
            Logic.RemoveBinding();
        }
        #endregion
        #region Initialize (override)
        public override void Initialize()
        {
            
            if (!_isInitialized)
            {
                _isInitialized = true;
                Logic.WriteLog("Loaded (Ver.{0}.{1})", Version.Major, Version.Minor);
                Logic.WriteLog("Bindings List:");
                Logic.WriteLog("  - Blacklist All Objects: {0} (Radius: {1}y, Time: {2}s)", 
                    TakeControlSettings.Get(s => s.KeyBlObjects), 
                    TakeControlSettings.Get(s => s.BlAllObjectsRadius),
                    TakeControlSettings.Get(s => s.BlAllObjectsTime));
                Logic.WriteLog("  - Blacklist Target: {0} (Time: {1}s)", 
                    TakeControlSettings.Get(s => s.KeyBlTarget),
                    TakeControlSettings.Get(s => s.BlTargetTime));
                Logic.WriteLog("  - HonorBuddy Suspend/Resume: {0}", TakeControlSettings.Get(s => s.KeyPause));
                Logic.WriteLog("  - HonorBuddy Stop/Start: {0}", TakeControlSettings.Get(s => s.KeyStartOrStop));
                Logic.WriteLog("  - HonorBuddy Restart: {0}", TakeControlSettings.Get(s => s.KeyRestart));
                // Apply bindings
                Logic.ApplyBinding();
                // Check for update
                if (!Logic.IsUpToDate)
                {
                    Logic.WriteLog("New version available ! Current: {0} Available: {1}. Check thebuddyforum or the ZenLulz's SVN.", Version, Logic.GetLatestVersion());
                }
            }
        }
        #endregion
        #region OnButtonPress (override)
        /// <summary>
        /// When the settings button is pressed.
        /// </summary>
        public override void OnButtonPress()
        {
            (new Gui.Settings()).Show();
        }
        #endregion
        #region Pulse (override)
        /// <summary>
        /// Nothing to pulse.
        /// </summary>
        public override void Pulse()
        {}
        #endregion
        #endregion
    }
}