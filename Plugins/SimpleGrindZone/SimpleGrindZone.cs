//!CompilerOption:Optimize:On

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

using Styx;
using Styx.Helpers;
//using Styx.Logic;
//using Styx.Logic.AreaManagement;
//using Styx.Logic.BehaviorTree;
//using Styx.Logic.Combat;
//using Styx.Logic.Inventory.Frames.Gossip;
//using Styx.Logic.Inventory.Frames.LootFrame;
//using Styx.Logic.Pathing;
//using Styx.Logic.Profiles;
using Styx.Plugins;
//using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;


namespace SimpleGrindZone
{


    class SimpleGrindZone : HBPlugin
    {



        
        //Plugin Name
        static public string name = "SimpleGrindZone";
        public override string Name { get { return name + " " + Version.ToString(); } }
        //Author
        public override string Author { get { return "blw"; } }
        //Version number
        private readonly Version _version = new Version(1,4 );
        public override Version Version { get { return _version; } }
        //Button name
        public override string ButtonText { get { return "SimpleGrindZone"; } }
        //Button required
        public override bool WantButton { get { return true; } }
        //        private static readonly configForm Gui = new configForm();

        public override void OnButtonPress()
        {
            Form1 _MyForm = new Form1();
            _MyForm.ShowDialog();
         
        }
           
       

        public override void Pulse()
        {
        }

    }
}






