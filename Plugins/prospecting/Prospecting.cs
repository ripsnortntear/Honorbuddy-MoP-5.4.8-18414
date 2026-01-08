using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.IO;
using System.Xml.Linq;
using Styx.Plugins;
using Styx;
using Styx.CommonBot;
using Styx.Common.Helpers;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Database;
using Styx.Common;
using Styx.CommonBot.Frames;
using System.Collections.Generic;
using System.Diagnostics;


namespace Prospecting
{
    public partial class Prospecting : HBPlugin
    {
        public override string Name { get { return "Prospecting"; } }
        public override string Author { get { return "Pasterke"; } }
        public override Version Version { get { return new Version(1, 1); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Prospecting Choice"; } }
        public override void OnButtonPress()
        {
            Form1 form = new Form1();
            form.ShowDialog();
        }
        public static void slog(string format, params object[] args)
        { Logging.Write("[Breakables]:" + format, args); }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override void Initialize()
        {
            Logging.Write(Colors.Lime, "Prospecting v1.1");
        }
        public override void Pulse()
        {
             
        }
         
    }
}
