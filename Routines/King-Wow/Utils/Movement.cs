#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author$
// $Date$
// $HeadURL$
// $LastChangedBy$
// $LastChangedDate$
// $LastChangedRevision$
// $Revision$

#endregion

using Styx;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using System;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals.DBC;
using Styx.Common;
using System.Diagnostics;
using System.Timers;

namespace KingWoW
{
    public class Movement
    {
        public static bool DisableAllMovement = true;
        private const float meeleRange = 2f;
        public float range = meeleRange;
        public bool stopAtRange = true;
        public bool isRangedTypeMovement = false;
        public WoWPoint destination;
        private Timer updateMovementTimer;

        public Movement()
        {
            updateMovementTimer = new Timer(100);
            updateMovementTimer.Elapsed +=new ElapsedEventHandler(updateMovementTimer_Elapsed);
     
        }

        void  updateMovementTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isRangedTypeMovement)
            {
                //Logging.Write("moving: target distance=" + StyxWoW.Me.Location.Distance(destination) + " range is " + range);
                if (StyxWoW.Me.Location.Distance(destination) <= range)
                {
                    //Logging.Write("Stop moving");
                    Navigator.PlayerMover.MoveStop();
                    range = meeleRange;
                    isRangedTypeMovement = false;
                    updateMovementTimer.Interval = 100;
                    updateMovementTimer.Stop();
                }
                else
                {
                    updateMovementTimer.Interval = 100;
                    updateMovementTimer.Start();
                }
            }

        }

        public void KingHealMove(WoWPoint destination, float range, bool stopAtRange, bool isRangedTypeMovement, WoWUnit target=null)
        {
            if ((StyxWoW.Me.Location.Distance2DSqr(destination) > range*range) || (target!=null && !target.IsDead && !target.InLineOfSight))
            {
                this.range = range;
                this.stopAtRange = stopAtRange;
                this.isRangedTypeMovement = isRangedTypeMovement;
                this.destination = destination;
                Navigator.MoveTo(destination);
                updateMovementTimer.Interval = 100;
                updateMovementTimer.Start();
            }
            
        }
    }
}