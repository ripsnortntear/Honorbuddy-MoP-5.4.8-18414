using System;
using System.Collections.Generic;

using Styx;
using Styx.Common;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace AeonaxxCatcher
{
    public class AeonaxxCatcher : HBPlugin
    {

        public override void Pulse()
        {
            if (StyxWoW.IsInGame)
            {
                if (StyxWoW.Me.IsAlive)
                {
                    _mainCampSpot();
                }
            }
        }

        public void _mainCampSpot()
        {
            List<WoWUnit> mobList = ObjectManager.GetObjectsOfType<WoWUnit>();

            uint Aeonaxx = 50062;
            uint AeonaxxMounted = 51236;
            uint YoungStoneDrake = 44038;

            var aeonaxx = mobList.Find(x => x.Entry == Aeonaxx);
            var youngStoneDrake = mobList.Find(x => x.Entry == YoungStoneDrake);
            var aeonaxxCombat = mobList.Find(x => x.Entry == AeonaxxMounted);

            /////                   //// X Waypoint Location Here (1120)
            const double targetXLocation = 1120;

            ////                   //// Y Waypoint Location Here (997)
            const double targetYLocation = 997;

            ////                  //// Z Waypoint Location Here (48)
            const double targetZLocation = 48;


            foreach (WoWUnit mob in mobList)
            {
                if (aeonaxx == null && aeonaxxCombat == null && youngStoneDrake == null)
                {
                    Flightor.MoveTo(new WoWPoint(targetXLocation, targetYLocation, targetZLocation));
                    Logging.Write("[AeonaxxCatcher] Flying to main camping location");
                }
                else if (aeonaxx != null || aeonaxxCombat != null || youngStoneDrake != null)
                {
                    _aeonaxx();
                }
            }
        }

        public void _aeonaxx()
        {
            List<WoWUnit> mobList = ObjectManager.GetObjectsOfType<WoWUnit>();
            const uint Aeonaxx = 50062;
            const uint AeonaxxMounted = 51236;
            const uint YoungStoneDrake = 44038;

            var aeonaxx = mobList.Find(x => x.Entry == Aeonaxx);
            var aeonaxxCombat = mobList.Find(x => x.Entry == AeonaxxMounted);
            var youngStoneDrake = mobList.Find(x => x.Entry == YoungStoneDrake);

            float myXLocation = StyxWoW.Me.Location.X;
            float myYLocation = StyxWoW.Me.Location.Y;
            float myZLocation = StyxWoW.Me.Location.Z;

            foreach (WoWUnit mob in mobList)
            {
                if (aeonaxx != null && youngStoneDrake == null && !aeonaxx.WithinInteractRange && aeonaxx.IsAlive && !StyxWoW.Me.Combat)
                {
                    float xLocation = aeonaxx.Location.X;
                    float yLocation = aeonaxx.Location.Y;
                    float zLocation = aeonaxx.Location.Z;

                    Flightor.MoveTo(new WoWPoint(xLocation, yLocation, zLocation));
                    Logging.Write("[AeonaxxCatcher] Aeonaxx is valid, we're not in combat and aeonaxx is alive...  moving to Aeonaxx at" + " X: " + aeonaxx.Location.X + " Y: " + aeonaxx.Location.Y + " Z: " + aeonaxx.Location.Z);
                }
                else if (aeonaxx != null && youngStoneDrake == null && aeonaxx.WithinInteractRange && !StyxWoW.Me.Combat) // no check for isAlive because we're also using this as a secondary Looting Method
                {
                    aeonaxx.Interact(); // Triple Interact attempt so it hopefully doesn't do the Interact and then stop (bottish and unreliable)
                    aeonaxx.Interact(); // Will also be used as a secondary Looting Method incase first one fails or they didn't enable Loot Mobs
                    aeonaxx.Interact(); // Since we parachute right next to Aeonaxx it shouldn't be an issue
                    Logging.Write("[AeonaxxCatcher] Aeonaxx is valid and within melee range, interacting...");
                }
                else if (aeonaxxCombat != null && youngStoneDrake == null && aeonaxxCombat.IsAlive && StyxWoW.Me.HealthPercent > 50)
                {
                    aeonaxxCombat.Target();
                    Logging.Write("[AeonaxxCatcher][Mounted] attacking Aeonaxx | [DEBUG] Mount Display ID: " + StyxWoW.Me.MountDisplayId);
                }
                else if (aeonaxxCombat != null && youngStoneDrake != null && youngStoneDrake.IsAlive && StyxWoW.Me.HealthPercent < 50)
                {
                    youngStoneDrake.Target();
                    Logging.Write("[AeonaxxCatcher][Mounted] attacking Young Stone Drakes until they are all slayed | [DEBUG] Mount Display ID: " + StyxWoW.Me.MountDisplayId);
                }
            }
        }

        public override void Initialize()
        {
            Logging.Write("AeonaxxCatcher - Loaded Version " + Version);
        }

        private static LocalPlayer intMe { get { return StyxWoW.Me; } }
        public override string Name { get { return "AeonaxxCatcher"; } }
        public override string Author { get { return "Giwin"; } }
        public override Version Version { get { return new Version(2, 7); } }
        public override bool WantButton { get { return false; } }
    }
}