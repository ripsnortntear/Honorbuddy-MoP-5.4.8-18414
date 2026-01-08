using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using Styx;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Diagnostics;
using Styx.Pathing;
using Styx.Common;



namespace UltimateEgger
{
    class UltimateEgger : HBPlugin
    {
        public override string Name { get { return "UltimateEgger v2.1"; } }
        public override string Author { get { return "BladingDancer"; } }
        public override Version Version { get { return new Version(1, 0); } }
        public LocalPlayer Me { get { return Styx.StyxWoW.Me; } }
        public override bool WantButton { get { return false; } }

        Random rnd = new Random();
        public int ranNum(int min, int max)
        {
            return rnd.Next(min, max);
        }

        private static void UseItem(uint id)
        {
            var item = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(it => it.Entry == id);
            if (item != null && CanUseItem(item))
                UseItem(item);
        }

        private static bool CanUseItem(WoWItem item)
        {
            return item != null && item.Usable && item.Cooldown <= 0;
        }

        private static void UseItem(WoWItem item)
        {
            if (item != null)
            {
                item.Use();
            }
        }
        public override void Pulse()
        {
            var egg = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(obj => (obj.Entry == 113768 || obj.Entry == 113769 || obj.Entry == 113770 || obj.Entry == 113771 || obj.Entry == 113772) && obj.Distance <= 30);
            if (egg != null && egg.IsValid)
            {
                while (egg.IsValid)
                {
                    if (egg.WithinInteractRange)
                    {
                        if (Me.IsMoving)
                        {
                            WoWMovement.MoveStop();
                        }
                        if (!Me.IsCasting)
                        {
                            egg.Interact();
                        }
                    }
                    else
                    {
                        Navigator.MoveTo(egg.Location);
                        if (!Me.HasAura("Egg Rush!"))
                        {
                            UseItem(45067);
                            return;
                        }
                    }
                    ObjectManager.Update();
                }
                if (!Me.IsCasting) UseItem(45072);
            }
        }
    }
}
