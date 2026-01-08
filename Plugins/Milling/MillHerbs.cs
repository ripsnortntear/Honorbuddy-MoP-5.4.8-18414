using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.Profiles;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;

namespace Milling
{
    public class MillHerbs : HBPlugin
    {
        public override string Name { get { return "【PN】Automatic herb grinding insert"; } }
        public override string Author { get { return "Pasterke"; } }
        public override Version Version { get { return new Version(1, 0, 0, 0); } }

        public override string ButtonText { get { return "Show Window"; } }
        public override bool WantButton { get { return true; } }

        public bool hasBeenInitialized = false;

        public Form1 myForm;
        public override void OnButtonPress()
        {
            myForm = new Form1();
            myForm.Show();
        }

        public void Initialize()
        {
            Logging.Write(Colors.Lime, "Herb Milling Beta");
        }

        public override void Pulse()
        {
            if (!hasBeenInitialized)
            {
                hasBeenInitialized = true;
                Initialize();
            }
        }










    }
}
