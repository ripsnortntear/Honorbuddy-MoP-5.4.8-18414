using System;
using System.Text;
using System.Windows.Media;

using Eclipse.EclipsePlugins;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
using Styx;

namespace Eclipse
{
    class EclipseProfileBuilder : HBPlugin
    {
        public override string Name { get { return "Eclipse Profile Creator"; } }
        public override string Author { get { return "Twist"; } }
        public override Version Version { get { return new Version(1, 0, 2, 0); } }

        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Build Profiles!"; } }

        private static LocalPlayer intMe = StyxWoW.Me;
        private ProfileTypeSelector mainForm;


        public EclipseProfileBuilder()
        {
            mainForm = new ProfileTypeSelector();
            Logging.Write("Loaded " + Name + " v" + Version.ToString() + " by " + Author);
        }

        public override void Pulse()
        {
            if (!StyxWoW.IsInGame)
            {
                return;
            }

            /* PULSE ACTION HERE */
           //if (mainForm != null) mainForm.Pulse();
        }
        
        
        public override void OnButtonPress()
        {
            if (mainForm == null || mainForm.IsDisposed)
                mainForm = new ProfileTypeSelector();
            try
            {
                mainForm.Show();
                mainForm.Activate();
            }
            catch (ArgumentOutOfRangeException ee)
            {

            }
        }
       
    }
}
