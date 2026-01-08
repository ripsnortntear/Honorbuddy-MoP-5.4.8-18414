using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Drawing;

using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Pathing;
using Styx.CommonBot.Frames;
using Styx.CommonBot;

namespace PandarenPick
{
    class PandarenPick : HBPlugin
    {
        public static string name { get { return "Pandaren Pick Farm 1.22 "; } }
        public override string Name { get { return name; } }
        public override string Author { get { return "Dannyfresh"; } }
        private readonly static Version _version = new Version(1, 0);
        public override Version Version { get { return _version; } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return false; } }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override void Pulse()
        {
            Thread.Sleep(1 / 30);
            try
            {
                {
                    if (Battlegrounds.IsInsideBattleground ||
                            StyxWoW.Me.IsActuallyInCombat ||
                                StyxWoW.Me.Mounted ||
                                    StyxWoW.Me.IsDead ||
                                        StyxWoW.Me.IsGhost)
                    {
                        return;
                    }
                    MiningPick();
                }
            }
            catch (ThreadAbortException)
            {
            }
        }



        public static void MoveToLocation(WoWPoint loc)
        {
            while (loc.Distance(Me.Location) > 3)
            {
                Flightor.MountHelper.MountUp();
                Navigator.MoveTo(loc);
            }
        }

        static public void MiningPick()
        {
            ObjectManager.Update();
            List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(miningpick => (miningpick.Distance2D <= 40 && (miningpick.Entry == 213364))).OrderBy(miningpick => miningpick.Distance).ToList();
            foreach (WoWGameObject miningpick in objList)
            {
                if (!miningpick.InLineOfSight) return;
                if (Me.Combat) return;
                MoveToLocation(WoWMovement.CalculatePointFrom(miningpick.Location, -3));
                Flightor.MountHelper.Dismount();
                Thread.Sleep(1000);
                miningpick.Interact();
                Thread.Sleep(2000);
                Logging.Write("Ancient Pandaren Mining pick found, Gratz bitches. Thanks Dannyfresh!");
                Flightor.MountHelper.MountUp();
                return;
            }
        }
    }
}