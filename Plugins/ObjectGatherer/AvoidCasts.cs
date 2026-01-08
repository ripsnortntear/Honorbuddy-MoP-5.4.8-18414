#region Styx Namespace
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
#endregion

namespace ObjectGatherer {
    public class AvoidCasts {

        #region Lightning Pool Behaviors
        /*
        public static void AvoidEnemyAOE(WoWPoint location, List<WoWDynamicObject> Objects, string Aura, int TraceStep)
        {
            if (Objects == null)
            { Logging.Write("no Lightning Pools found .."); return; }
            Logging.Write("found {0} {1}! start RayCast ..", Objects.Count, Aura);

            int MinDistToPools = (int)(Objects[0].Radius * 1.6f);
            int MaxDistToMove = MinDistToPools * 2;

            // get save location
            WoWPoint newP = getSaveLocation(location, Objects, MinDistToPools, MaxDistToMove, TraceStep);

            if (newP == WoWPoint.Empty)
            {
                // no save location found, move 2sec Strafe Left
                WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(2));
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            else
            {
                // move to save location
                while (StyxWoW.Me.HasAura(Aura) && StyxWoW.Me.Location.Distance(newP) > 0.2f)
                {
                    Navigator.MoveTo(newP);
                    Thread.Sleep(80);
                }
            }

            WoWMovement.MoveStop();
            if (StyxWoW.Me.CurrentTargetGuid != 0)
                StyxWoW.Me.CurrentTarget.Face();

            //Styx.CommonBot.Blacklist.Add(
            //Styx.CommonBot.Profiles.Blackspot vv = new Styx.CommonBot.Profiles.Blackspot(
        }

        private static bool wlog(WoWDynamicObject obj)
        { Logging.Write("add pool - dis2D: {0}", obj.Distance2D); return true; }
        public static List<WoWDynamicObject> getLightningPoolList
        {
            get
            {
                ObjectManager.Update();
                return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
                        orderby lp.Distance2D ascending
                        where lp.Entry == 129657
                        where wlog(lp)
                        select lp).ToList();
            }
        }

        public static List<WoWDynamicObject> getCausticPitchList
        {
            get
            {
                ObjectManager.Update();
                return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
                        orderby lp.Distance2D ascending
                        where lp.Entry == 126336
                        where wlog(lp)
                        select lp).ToList();
            }
        }

        public static List<WoWDynamicObject> getVenomSplashList
        {
            get
            {
                ObjectManager.Update();
                return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
                        orderby lp.Distance2D ascending
                        where lp.Entry == 79607
                        where wlog(lp)
                        select lp).ToList();
            }
        }

        #region RayCast

        private static WoWPoint getSaveLocation(WoWPoint Location, List<WoWDynamicObject> badObjects, int minDist, int maxDist, int traceStep)
        {
            Logging.Write("Navigation: Looking for save Location around {0}.", Location);

            try
            {
                //float _PIx2 = 3.14159f * 2f;
                float _PIx2 = ((float)new Random().Next(1, 80) * (1.248349f + (float)new Random().NextDouble()));

                for (int i = 0, x = minDist; i < traceStep && x < maxDist; i++)
                {
                    WoWPoint p = Location.RayCast((i * _PIx2) / traceStep, x);

                    p.Z = getGroundZ(p);

                    if (p.Z != float.MinValue && StyxWoW.Me.Location.Distance2D(p) > 1 &&
                        (badObjects.FirstOrDefault(_obj => _obj.Location.Distance2D(p) <= minDist) == null) &&
                        //(ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Location.Distance2D(p) < 20 && u.IsAlive && !u.Combat) == null) &&
                        Navigator.GeneratePath(StyxWoW.Me.Location, p).Length != 0)
                    {
                        if (getHighestSurroundingSlope(p) < 1.2f)
                        {
                            Logging.Write("Navigation: Moving to {0}. Distance: {1}", p, Location.Distance(p));
                            return p;
                        }
                    }

                    if (i == (traceStep - 1))
                    {
                        i = 0;
                        x++;
                    }
                }
            }
            catch (Exception ex)
            { Logging.WriteException(ex); }


            Logging.Write(" - No valid points returned by RayCast ...");
            return WoWPoint.Empty;

        }

        /// <summary>
        /// Credits to exemplar.
        /// </summary>
        /// <returns>Z-Coordinates for PoolPoints so we don't jump into the water.</returns>
        private static float getGroundZ(WoWPoint p)
        {
            WoWPoint ground = WoWPoint.Empty;

            GameWorld.TraceLine(new WoWPoint(p.X, p.Y, (p.Z + 100)), new WoWPoint(p.X, p.Y, (p.Z - 5)), GameWorld.CGWorldFrameHitFlags.HitTestGroundAndStructures/* | GameWorld.CGWorldFrameHitFlags.HitTestBoundingModels | GameWorld.CGWorldFrameHitFlags.HitTestWMO*/ /*, out ground);

            if (ground != WoWPoint.Empty)
            {
                Logging.Write(" - Ground Z: {0}.", ground.Z);
                return ground.Z;
            }
            Logging.Write(" - Ground Z returned float.MinValue.");
            return float.MinValue;
        }

        /// <summary>
        /// Credits to funkescott.
        /// </summary>
        /// <returns>Highest slope of surrounding terrain, returns 100 if the slope can't be determined</returns>
        private static float getHighestSurroundingSlope(WoWPoint p)
        {
            Logging.Write("Navigation: Sloapcheck on Point: {0}", p);
            float _PIx2 = 3.14159f * 2f;
            float highestSlope = -100;
            float slope = 0;
            int traceStep = 15;
            float range = 0.5f;
            WoWPoint p2;
            for (int i = 0; i < traceStep; i++)
            {
                p2 = p.RayCast((i * _PIx2) / traceStep, range);
                p2.Z = getGroundZ(p2);
                slope = Math.Abs(getSlope(p, p2));
                if (slope > highestSlope)
                {
                    highestSlope = (float)slope;
                }
            }
            Logging.Write(" - Highslope {0}", highestSlope);
            return Math.Abs(highestSlope);
        }

        /// <summary>
        /// Credits to funkescott.
        /// </summary>
        /// <param name="p1">from WoWPoint</param>
        /// <param name="p2">to WoWPoint</param>
        /// <returns>Return slope from WoWPoint to WoWPoint.</returns>
        private static float getSlope(WoWPoint p1, WoWPoint p2)
        {
            float rise = p2.Z - p1.Z;
            float run = (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

            return rise / run;
        }
        #endregion
*/
        #endregion

        #region AvoidFrontal
        public static void AvoidFrontal(WoWUnit npcID, int radius, int range) { // npcName: NPC ID number, radius: (how wide is the cone in deg) 0 is safely behind, range: Range of the spell
            if (!StyxWoW.Me.IsFacing(npcID)) { npcID.Face(); }

            var rotation = GetPositive(npcID.RotationDegrees);
            var invertRotation = GetInvert(rotation);
            var move = GetPositive(StyxWoW.Me.RotationDegrees) > invertRotation ? WoWMovement.MovementDirection.StrafeRight : WoWMovement.MovementDirection.StrafeLeft;

            while (npcID.Distance2D <= range && npcID.IsCasting && ((radius == 0 && !StyxWoW.Me.IsSafelyBehind(npcID)) || 
                  (radius != 0 && npcID.IsSafelyFacing(StyxWoW.Me, radius)) || npcID.Distance2D <= 2)) {
                WoWMovement.Move(move);
                npcID.Face();
            }
            WoWMovement.MoveStop();
        }

        private static float GetInvert(float f) {
            if (f < 180) { return (f + 180); }
            return (f - 180);
        }

        private static float GetPositive(float f) {
            if (f < 0) { return (f + 360); }
            return f;
        }
        #endregion
    }
}