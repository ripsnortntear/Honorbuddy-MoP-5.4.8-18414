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

namespace RepCollector
{
    class RepCollector : HBPlugin
    {
        public static string name { get { return "RepCollector"; } }
        public override string Name { get { return name; } }
        public override string Author { get { return "Hazard"; } }
        private readonly static Version _version = new Version(1, 1);
        public override Version Version { get { return _version; } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return false; } }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override void Pulse()
        {
            Thread.Sleep(1 / 30);
            try
            {
                if (!Me.Combat || !Me.IsDead)
                    OnyxEgg();
                    NetherwingEgg();
                    DarkSoil();
                    AncientGuoLaiCache();
            }
            catch (ThreadAbortException)
            {
            }
        }

        private static bool AncientGuoLaiCacheKey()
        {
            WoWItem key = Me.BagItems.FirstOrDefault(o => o.Entry == 87779);
            if (key == null)
            {
                return false;
            }
            else return true;
        }

        public static void MoveToLocation(WoWPoint loc)
        {
            while (loc.Distance(Me.Location) > 3)
            {
                Flightor.MountHelper.MountUp();
                Navigator.MoveTo(loc);
            }
        }

        static public void OnyxEgg()
        {
            ObjectManager.Update();
            List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(onyxegg => (onyxegg.Distance2D <= 40 && (onyxegg.Entry == 214945))).OrderBy(onyxegg => onyxegg.Distance).ToList();
            foreach (WoWGameObject onyxegg in objList)
            {
                if (!onyxegg.InLineOfSight) return;
                if (Me.Combat) return;
                MoveToLocation(WoWMovement.CalculatePointFrom(onyxegg.Location, -3));
                Flightor.MountHelper.Dismount();
                Thread.Sleep(1000);
                onyxegg.Interact();
                Thread.Sleep(2000);
                Logging.Write("1x Onyx Egg collected!");
                Flightor.MountHelper.MountUp();
                return;
            }
        }

        static public void NetherwingEgg()
        {
            ObjectManager.Update();
            List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(netherwingegg => (netherwingegg.Distance2D <= 40 && (netherwingegg.Entry == 214945))).OrderBy(netherwingegg => netherwingegg.Distance).ToList();
            foreach (WoWGameObject netherwingegg in objList)
            {
                if (!netherwingegg.InLineOfSight) return;
                if (Me.Combat) return;
                MoveToLocation(WoWMovement.CalculatePointFrom(netherwingegg.Location, -3));
                Flightor.MountHelper.Dismount();
                Thread.Sleep(1000);
                netherwingegg.Interact();
                Thread.Sleep(2000);
                Logging.Write("1x Netherwing Egg collected!");
                Flightor.MountHelper.MountUp();
                return;
            }
        }

        static public void DarkSoil()
        {
            ObjectManager.Update();
            List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(darksoil => (darksoil.Distance2D <= 40 && (darksoil.Entry == 210565))).OrderBy(darksoil => darksoil.Distance).ToList();
            foreach (WoWGameObject darksoil in objList)
            {
                if (!darksoil.InLineOfSight) return;
                if (Me.Combat) return;
                MoveToLocation(WoWMovement.CalculatePointFrom(darksoil.Location, -3));
                Flightor.MountHelper.Dismount();
                Thread.Sleep(2000);
                darksoil.Interact();
                Thread.Sleep(3000);
                Logging.Write("1x Dark Soil collected!");
                Flightor.MountHelper.MountUp();
                return;
            }
        }

        static public void AncientGuoLaiCache()
        {
            ObjectManager.Update();
            List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(ancientguolaicache => (ancientguolaicache.Distance2D <= 40 && (ancientguolaicache.Entry == 214388))).OrderBy(ancientguolaicache => ancientguolaicache.Distance).ToList();
            foreach (WoWGameObject ancientguolaicache in objList)
            {
                AncientGuoLaiCacheKey();
                if (!ancientguolaicache.InLineOfSight) return;
                if (Me.Combat) return;
                MoveToLocation(WoWMovement.CalculatePointFrom(ancientguolaicache.Location, -3));
                Thread.Sleep(1000);
                ancientguolaicache.Interact();
                Thread.Sleep(3000);
                Logging.Write("1x Ancient Guo-Lai Cache opened!");
            }
        }
    }
}