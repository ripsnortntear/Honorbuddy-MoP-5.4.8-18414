using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.AreaManagement;
using Styx.Pathing;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FpsRefresh
{
    public class Main : HBPlugin
    {
        public static DateTime _fpsTimer;

        public override string Name { get { return "FpsRefresh"; } }
        public override string Author { get { return "Pasterke"; } }
        public override Version Version { get { return new Version(1, 0); } }

        public override void OnEnable()
        {
            Logging.Write(Colors.Yellow, "FPS Refresher Active !");
            _fpsTimer = DateTime.Now + new TimeSpan(0, 0, 30);
        }

        public override void OnDisable()
        {
            Logging.Write(Colors.Yellow, "FPS Refresher Deactivated. !");
        }

        public override void Pulse()
        {
            if (DateTime.Now >= _fpsTimer)
            {
                Lua.DoString("ForceRefresh()"); // Refresh the FPS
                _fpsTimer = DateTime.Now + new TimeSpan(0, 0, 30);
            }
        }

    }
}
