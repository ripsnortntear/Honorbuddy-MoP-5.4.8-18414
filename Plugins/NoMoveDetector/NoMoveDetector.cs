using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace HB.Plugins.NoMoveDetector
{
    class NoMoveDetector : HBPlugin
    {
        #region Overrides of HBPlugin

        public override void Initialize()
        {
            Styx.CommonBot.BotEvents.OnBotStart += _OnBotStart;
            _init();
        }
        public override void Dispose()
        {
            Styx.CommonBot.BotEvents.OnBotStart -= _OnBotStart;
        }
        public override string ButtonText { get { return "---"; } }
        public override bool WantButton { get { return false; } }
        public override void OnButtonPress(){}
        public override void Pulse(){_MainPulseProc();}
        public override string Name { get { return "No Move Detector"; } }
        public override string Author { get { return "cls15"; } }
        public override Version Version { get { return new Version(1, 2, 0); } }

        #endregion

        private Stopwatch LastOK ;
        private WoWPoint LastLoc;
        private int nBotRestart;
        private static Thread RestartThread;

        private static void _RestartThread()
        {
            TreeRoot.Stop();
            Thread.Sleep(2000);
            TreeRoot.Start();
        }

        private void _OnBotStart(EventArgs args)
        {
            Logging.Write(@"[NoMoveDetector] Bot started");
            LastLoc = StyxWoW.Me.Location;
            LastOK.Restart();
            RestartThread = new Thread(new ThreadStart(_RestartThread));
        }

        private void _init()
        {
            Logging.Write(@"[NoMoveDetector] init");
            LastOK = new Stopwatch();
            nBotRestart = 0;
        }

        private void _MainPulseProc()
        {
            // Must we go futher anyway?
            if (!TreeRoot.IsRunning)
            {
                if (LastOK.ElapsedMilliseconds > 1000 * 30)
                {
                    Logging.Write(@"[NoMoveDetector] LastPosition reseted, bot is not running (but pulse is called ???)");
                    LastOK.Restart();
                }
                return;
            } 
            
            WoWPlayer Me = StyxWoW.Me;
            //Cancel timer if move > 10 yards is detected
            if (LastLoc.Distance(Me.Location) > 10f)
            {
                if (LastOK.ElapsedMilliseconds > 1000 * 30)  Logging.Write(@"[NoMoveDetector] Move detected. LastPosition reseted");
                LastOK.Restart();
                LastLoc = Me.Location;
                return;
            }
           /* if (LastOK.ElapsedMilliseconds > 1000 * 5 && !StyxWoW.Me.HasAura("Food"))
            { 
                // BestFood detection correct
                ulong nCurrentFood = CharacterSettings.Instance.DrinkName.ToUInt32();
                WoWItem tp = ObjectManager.GetObjectByGuid<WoWItem>(nCurrentFood);
                if (nCurrentFood == 0)
                {
                    
                }
            } */
            // Have we moved whithin last 5 mins
            if (LastOK.ElapsedMilliseconds > 1000*60*5)
            {
                if (Styx.CommonBot.Frames.AuctionFrame.Instance.IsVisible || Styx.CommonBot.Frames.MailFrame.Instance.IsVisible)
                {
                    Logging.Write(@"[NoMoveDetector] not mooving last {0} min but has open frame.  LastPosition reseted", nBotRestart * 5);
                    LastOK.Restart();
                    LastLoc = Me.Location;
                    return;
                }
                if (Me.HasAura("Resurrection Sickness"))
                {
                    LastOK.Restart();
                    return;
                }
                if (nBotRestart > 1) // Not mooving for 15 min, hope you have a reloger...
                {
                    Logging.Write(@"[NoMoveDetector] not mooving last 15 min : Stopping Wow...");
                    Lua.DoString(@"ForceQuit()");
                }
                else
                {
                    nBotRestart++;
                    Logging.Write(@"[NoMoveDetector] not mooving last {0} min : Restarting bot...",nBotRestart*5);
                    LastOK.Restart();
                    RestartThread.Start();
                }
                
            }
        }
    }
}
