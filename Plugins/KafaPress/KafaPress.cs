/*
Kafa Press - Increases your Haste by 4000 for 25 sec!
Created by Cisem
http://www.cisem.se
*/
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Styx.Plugins;
using Styx.Common;
using Styx.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace  CrystalofInsanity
{
    public class CrystalofInsanity : HBPlugin
    {
        public override Version Version { get { return new Version(1, 0, 0); } }

        public override string Author
        {
            get { return "Cisem"; }
        }

        public override string Name
        {
            get { return " Kafa Press "; }
        }

        public override string ButtonText
        {
            get
            {
                return "Kafa Press";
            }
        }
        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
        }

        public override void Pulse()
        {
            foreach (WoWItem Crystal in StyxWoW.Me.BagItems)
            {
										if (StyxWoW.Me.Combat)
                if (Crystal.Name.Contains("Kafa Press"))
                {
                    if (Crystal.Cooldown == 0)
                    {
                        Logging.Write("Using Kafa Press. Increases your Haste by 4000 for 25 sec!");
                        Crystal.Use();
                    }
                }
            }
        }

        public override void OnEnable()
        {
        }
    }
}