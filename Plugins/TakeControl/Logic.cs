using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;

namespace TakeControl
{
    public static class Logic
    {
        #region Fields
        /// <summary>
        /// Locker to avoid error when spamming objects blacklist.
        /// </summary>
        private static readonly object _blObjectsLocker = new object();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the behavior tree thread.
        /// </summary>
        public static Thread BehaviorTreeThread
        {
            get
            {
                return (Thread) typeof(TreeRoot).GetFields(BindingFlags.NonPublic | BindingFlags.Static).First(f => f.FieldType == typeof(Thread)).GetValue(typeof(TreeRoot));
            }
        }
        /// <summary>
        /// State if the bot is paused.
        /// </summary>
        public static bool IsBotPaused { get; private set; }
        /// <summary>
        /// State if the specified key is pressed.
        /// </summary>
        private static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState(key)) != 0;
        }
        /// <summary>
        /// All pressed keys.
        /// </summary>
        public static Keys[] PressedKeys
        {
            get
            {
                return Enum.GetValues(typeof(Keys)).Cast<Keys>().Where(IsKeyDown).ToArray();
            }
        }
        #endregion

        #region Methods
        #region ApplyBinding
        /// <summary>
        /// Applies a binding key.
        /// </summary>
        public static void ApplyBinding()
        {
            // Remove all bindings
            RemoveBinding();
            

            // Blacklist all objects
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyBlObjects)))
            {
                var key = (Keys) Enum.Parse(typeof (Keys), TakeControlSettings.Get(s => s.KeyBlObjects));
                HotkeysManager.Register("TakeControl!_BlObjects",
                                    key,
                                    0,
                                    hotkey => BlacklistAllObjects());
            }
            Thread.Sleep(10);

            // Blacklist current target
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyBlTarget)))
            {
                var key = (Keys)Enum.Parse(typeof(Keys), TakeControlSettings.Get(s => s.KeyBlTarget));
                HotkeysManager.Register("TakeControl!_BlObjects",
                                    key,
                                    0,
                                    hotkey => BlacklistCurrentTarget());
            }
            Thread.Sleep(10);

            // HonorBuddy Pause
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyPause)))
            {
                var key = (Keys)Enum.Parse(typeof(Keys), TakeControlSettings.Get(s => s.KeyPause));
                HotkeysManager.Register("TakeControl!_Pause",
                                    key,
                                    0,
                                    hotkey => { if (IsBotPaused) HbResume(); else HbSuspend(); });
            }
            Thread.Sleep(10);

            // HonorBuddy Start
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyStartOrStop)))
            {
                var key = (Keys)Enum.Parse(typeof(Keys), TakeControlSettings.Get(s => s.KeyStartOrStop));
                HotkeysManager.Register("TakeControl!_StartOrStop",
                                    key,
                                    0,
                                    hotkey => { if(TreeRoot.IsRunning) HbStop(); else HbStart(); });
            }
            Thread.Sleep(10);

            // HonorBuddy Restart
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyRestart)))
            {
                var key = (Keys)Enum.Parse(typeof(Keys), TakeControlSettings.Get(s => s.KeyRestart));
                HotkeysManager.Register("TakeControl!_Restart",
                                    key,
                                    0,
                                    hotkey => { HbStop(); HbStart(); });
            }
        }
        #endregion
        #region BlacklistAllObjects
        /// <summary>
        /// Blacklist all objects in radius.
        /// </summary>
        public static void BlacklistAllObjects()
        {
            lock (_blObjectsLocker)
            {
                var time = TimeSpan.FromSeconds(TakeControlSettings.Get(s => s.BlAllObjectsTime));
                var count = 0;
                // Using a for loop to avoid memory read errors
                for (var i = 0; i < ObjectManager.ObjectList.Count - 1; i++)
                {
                    try
                    {
                        // Check the validity
                        if (ObjectManager.ObjectList[i].IsValid)
                        {
                            // Check the range
                            if(ObjectManager.ObjectList[i].Distance <= TakeControlSettings.Get(s => s.BlAllObjectsRadius))
                            {
                                Blacklist.Add(ObjectManager.ObjectList[i].Guid, BlacklistFlags.All, time);
                                count++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteDiagnostic(Colors.Tomato, "[TakeControl!]: Error during blacklisting objects: {0}", ex.Message);
                    }
                }
                WriteLog("{0} objects blacklisted in a radius of {1} during {2} seconds", 
                    count, 
                    TakeControlSettings.Get(s => s.BlAllObjectsRadius), 
                    TakeControlSettings.Get(s => s.BlAllObjectsTime));
            }
        }
        #endregion
        #region BlacklistCurrentTarget
        /// <summary>
        /// Blacklist the current target.
        /// </summary>
        public static void BlacklistCurrentTarget()
        {
            if (StyxWoW.Me.CurrentTarget != null)
            {
                WriteLog("{0} blacklisted during {1} seconds", StyxWoW.Me.CurrentTarget.Name,
                         TakeControlSettings.Get(s => s.BlTargetTime));
                Blacklist.Add(StyxWoW.Me.CurrentTargetGuid, BlacklistFlags.All, TimeSpan.FromSeconds(0));
            }
            else
            {
                WriteLog("You don't have target; cannot blacklist it");
            }
        }
        #endregion
        #region CheckForUpdate
        /// <summary>
        /// Gets the latest version from the SVN.
        /// </summary>
        public static Version GetLatestVersion()
        {
            return Version.Parse((new WebClient()).DownloadString("https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/Plugins/TakeControl/Version"));
        }
        #endregion
        #region HbResume
        /// <summary>
        /// Resumes the Behavior tree.
        /// </summary>
        public static void HbResume()
        {
            if (TreeRoot.IsRunning)
            {
                BehaviorTreeThread.Resume();
                IsBotPaused = false;
                TreeRoot.StatusText = "[TakeControl!] Bot resumed";
                TreeRoot.StatusText = "[TakeControl!] Bot resumed";
                WriteLog("Honorbuddy: Resumed");
            }
            else
            {
                WriteLog("The bot is stopped; cannot resume it");
            }
        }
        #endregion
        #region HbSuspend
        /// <summary>
        /// Suspends the Behavior tree.
        /// </summary>
        public static void HbSuspend()
        {
            if (TreeRoot.IsRunning)
            {
                BehaviorTreeThread.Suspend();
                IsBotPaused = true;
                TreeRoot.StatusText = "[TakeControl!] Bot suspended";
                TreeRoot.StatusText = "[TakeControl!] Bot suspended";
                WriteLog("Honorbuddy: Suspended");
            }
            else
            {
                WriteLog("The bot is stopped; cannot resume it");
            }
        }
        #endregion
        #region HbStart
        /// <summary>
        /// Starts HonorBuddy.
        /// </summary>
        public static void HbStart()
        {
            TreeRoot.Start();
            WriteLog("Honorbuddy: Started");
        }
        #endregion
        #region HbStop
        /// <summary>
        /// Stops HonorBuddy.
        /// </summary>
        public static void HbStop()
        {
            TreeRoot.Stop();
            WriteLog("Honorbuddy: Stopped");
        }
        #endregion
        #region IsUpToDate
        /// <summary>
        /// State if the current version is up to date.
        /// </summary>
        public static bool IsUpToDate
        {
            get { return TakeControl.StaticVersion >= GetLatestVersion(); }
        }
        #endregion
        #region RemoveBinding
        /// <summary>
        /// Removes the binding key.
        /// </summary>
        public static void RemoveBinding()
        {
            HotkeysManager.Unregister("TakeControl!");
            for (var i = HotkeysManager.Hotkeys.Count() - 1; i >= 0; i--)
            {
                if(HotkeysManager.Hotkeys.ElementAt(i).Name.Contains("TakeControl!"))
                    HotkeysManager.Unregister(HotkeysManager.Hotkeys.ElementAt(i));
            }
        }

        #endregion
        #region WriteLog
        /// <summary>
        /// Write a line in the HB log.
        /// </summary>
        public static void WriteLog(string text, params object[] args)
        {
            Logging.Write(Colors.Yellow, "[TakeControl!]: " + text, args);
        }
        #endregion
        #endregion

        #region Interop
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        #endregion
    }
}