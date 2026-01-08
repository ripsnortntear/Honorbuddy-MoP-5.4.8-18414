using System;
using System.Text;
using System.Windows.Media;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Common.Helpers;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Eclipse.EclipsePlugins;

namespace Eclipse
{
    class EclipseProfileBuilder : HBPlugin
    {
        public override string Name { get { return "Eclipse Profile Plugin"; } }
        public override string Author { get { return "Twist"; } }
        public override Version Version { get { return new Version(1, 0, 0, 8); } }

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
            mainForm.Show();
        }


        public override void Dispose()
        {
            base.Dispose();
        }

        
    }
}
