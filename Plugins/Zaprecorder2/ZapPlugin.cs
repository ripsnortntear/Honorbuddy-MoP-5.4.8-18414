using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx;
using Styx.Plugins;
using Styx.Common;
using Styx.WoWInternals;

using BadWolf;

using System.Windows.Media;

namespace ZapRecorder2
{
    public class ZapPlugin : HBPlugin
    {
        public override string Author { get { return "BadWolf (originally by Zapman)"; } }
        public override Version Version { get { return new Version(1, 3, 0); } }
        public override string Name { get { return "ZapRecorder2"; } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Show Recorder"; } }

        public bool hasBeenInitialized = false;
        public bool mainFormLoaded = false;
        public ZapMainForm mainForm;
        public static bool isHidden = false;

        private BadWolf_Updater Updater;


        
        public override void Initialize()
        {
            if (hasBeenInitialized)
            {
                return;
            }

            hasBeenInitialized = true;
            
            
            Logging.Write(Colors.Teal, "Loaded ZapRecorder2 by BadWolf v" + Version.ToString());

    
            try
            {
                Updater = new BadWolf_Updater("https://zaprecorder2.googlecode.com/svn/trunk/","ZapRecorder");

                if (Updater.UpdateAvailable())
                {
                    Logging.Write("[ZapRecorder2] Update to $" + Updater.GetNewestRev().ToString() + " is available! You are on $" + Updater.CurrentRev.ToString());
                    Logging.Write("[Zaprecorder2] Starting update process");
                    if (Updater.Update())
                    {
                        Logging.Write("[ZapRecorder2] is now up to date! Please reload HB");
                    }
                    else
                    {
                        Logging.Write("[ZapRecorder2] Encountered an error trying to auto-update. Please update manually");
                    }
                }
                else
                {
                    Logging.Write("ZapRecorder2 is at Rev $" + Updater.CurrentRev.ToString() + " and up to date!");

                    
                }
            } catch (Exception ex) {
                Logging.Write(Colors.Teal,"Unable to run ZapRecorder2 update process");
                Logging.Write(LogLevel.Diagnostic, "[ZapRecorder2]: Exception " + ex.Message);
            }
       }

        public static void HandleHotkey()
        {

            Logging.Write("Hotkey received");
        }

        public override void Pulse()
        {

        }


        public override void Dispose()
        {
            base.Dispose();
        }

        public override void OnButtonPress()
        {
            if (!ZapPlugin.isHidden)
            {
                // First time button press, create form.
                if (!mainFormLoaded)
                {
                    //We need to create a new form
                    mainForm = new ZapMainForm();
                    mainForm.Show();
                    mainFormLoaded = true;

                    
                }
            }
            else
            {
                // Reload existing form
                mainForm.Show();
            }
        }
    }
}
