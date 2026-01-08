using System.Drawing;
using Styx;

namespace SlackMatto
{
    using System;
    using System.Linq;
    using Styx.Helpers;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using Styx.Plugins;
    using Styx.CommonBot.Routines;
    using Styx.CommonBot;
    using Styx.CommonBot.POI;
    using Styx.Pathing;
    using System.Diagnostics;
    using System.Threading;

    public class SlackMatto : HBPlugin
    {
#region PreStuff

        //Normal Stuff.
        public override string Name { get { return "SlackMatto"; } }
        public override string Author { get { return "Newk, Original by Ingrego(AssistFollow)"; } }
        public override Version Version { get { return new Version(1, 0); } }
        public override bool WantButton { get { return false; } }
        public override string ButtonText { get { return "None"; } }
        
        //Logging Class for your conviance
        public static void slog(string format, params object[] args)
        {
            bool Logging = true;
            if (Logging)
            {
                Styx.Common.Logging.Write("[SlackMatto]: " + format, args);
            }
            else
                return;

        }
        private WoWUnit AssistTarget
        {
            get 
            {
                WoWUnit assistTarget = Me;
                if (Me.FocusedUnitGuid != 18446744073709551614)
                {
                    return Me.FocusedUnit;
                }
                else
                {
                    return Me;
                }
            }
        }
        private static readonly LocalPlayer Me = StyxWoW.Me; 
        
        // Settings
        static int followCombatDist = 3;
        static int followNoCombatDist = 10;
        static int bufferDist = 5;
        
        //Uncomment if adding a UI for the plugin
        /*public override void OnButtonPress()
        {

        }*/

#endregion

        public override void Pulse()
        {
            if (preChecks())
            {
                //Mount Check
                if (NeedToMount())
                {
                    slog("Mounting");
                    WaitForMount();
                }
                //Dismount Check
                if (NeedToDismount())
                {
                    slog("DisMounting");
                    MoveToTarget(AssistTarget, 0);
                    if (AssistTarget.Distance < 5)
                    {
                        WaitForDismount();
                    }
                }
                
                //Role Check
                //slog("Current Spec Type: " + Me.SpecType.ToString());
                if (String.Compare(Me.SpecType.ToString(), "Healer") == 0)
                {
                    if (Me.GotTarget && Me.CurrentTarget.IsDead)
                    {
                        slog("Clearing Target and Stopping Movement");
                        Me.ClearTarget();
                        WaitForStop();
                        return;
                    }

                    if ((Me.Combat || AssistTarget.Combat))
                    {
                        if (!Me.IsCasting)
                        {
                            if (AssistTarget.Distance >= (followCombatDist + bufferDist))
                            {
                                MoveToTarget(AssistTarget, followCombatDist);
                            }
                        }
                    }

                    if (!Me.Combat && !AssistTarget.Combat)
                    {
                        if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                        {
                            if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                            {
                                MoveToTarget(AssistTarget, followNoCombatDist);
                            }
                            if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                            {
                                MoveToTarget(AssistTarget, 1);
                            }
                        }

                    }
                }
                if (String.Compare(Me.SpecType.ToString(), "MeleeDps") == 0)
                {
                    if (Me.GotTarget && Me.CurrentTarget.IsDead)
                    {
                        slog("Clearing Target and Stopping Movement");
                        Me.ClearTarget();
                        WaitForStop();
                        return;
                    }

                    if ((Me.Combat || AssistTarget.Combat))
                    {
                        if (!Me.GotTarget)
                        {
                            if (AssistTarget.GotTarget)
                            {
                                if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                                {
                                    AssistTarget.CurrentTarget.Target();
                                }
                            }
                            else if (!AssistTarget.GotTarget)
                            {
                                if (AssistTarget.Distance >= (followCombatDist + bufferDist))
                                {
                                    MoveToTarget(AssistTarget, followCombatDist);
                                }
                            }
                        }
                        else if (Me.GotTarget && Me.CurrentTarget.Attackable && !Me.CurrentTarget.IsFriendly)
                        {
                            if (Me.Mounted && Me.CurrentTarget.Distance <= 6)
                            {
                                WaitForDismount();
                            }
                            if (Me.CurrentTarget.Distance >= 5)
                            {
                                MoveToTarget(Me.CurrentTarget, 3);
                            }
                            else if (Me.CurrentTarget.Distance < 5)
                            {
                                WoWMovement.MoveStop();
                            }
                            if (!Me.IsFacing(Me.CurrentTarget))
                            {
                                WoWMovement.Face();
                            }

                        }
                        else if (Me.GotTarget && Me.CurrentTarget.IsFriendly)
                        {
                            if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                            {
                                AssistTarget.CurrentTarget.Target();
                            }
                        }
                    }

                    if (!Me.Combat && !AssistTarget.Combat)
                    {
                        if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                        {
                            if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                            {
                                MoveToTarget(AssistTarget, followNoCombatDist);
                            }
                            if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                            {
                                MoveToTarget(AssistTarget, 1);
                            }
                        }

                    }
                }
                if (String.Compare(Me.SpecType.ToString(), "RangedDps") == 0)
                {
                    if (Me.GotTarget && (Me.CurrentTarget.IsDead))
                    {
                        slog("Clearing Target and Stopping Movement");
                        Me.ClearTarget();
                        WaitForStop();
                        return;
                    }

                    if (Me.Combat || AssistTarget.Combat)
                    {
                        if (!Me.GotTarget)
                        {
                            if (AssistTarget.GotTarget)
                            {
                                if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                                {
                                    AssistTarget.CurrentTarget.Target();
                                }
                            }
                            else if (!AssistTarget.GotTarget)
                            {
                                if (AssistTarget.Distance >= (followCombatDist + bufferDist))
                                {
                                    MoveToTarget(AssistTarget, followCombatDist);
                                }
                            }
                        }
                        else if (Me.GotTarget && Me.CurrentTarget.Attackable && !Me.CurrentTarget.IsFriendly)
                        {
                            if (Me.CurrentTarget.InLineOfSight && Me.CurrentTarget.InLineOfSpellSight)
                            {
                                if (Me.Mounted && Me.CurrentTarget.Distance <= 40)
                                {
                                    WaitForDismount();
                                }
                                if (Me.CurrentTarget.Distance >= 40)
                                {
                                    MoveToTarget(Me.CurrentTarget, (int)Me.CurrentTarget.Distance - 10);
                                }
                                if (Me.Z - Me.CurrentTarget.Z <= -5)
                                {
                                    MoveToTarget(Me.CurrentTarget, (int)Me.CurrentTarget.Distance);
                                }

                                /*
                                if (Me.CurrentTarget.Distance <= 30)
                                {
                    kl                WoWMovement.MoveStop();
                                }
                                */

                                if (!Me.IsFacing(Me.CurrentTarget))
                                {
                                    WoWMovement.Face();
                                }
                            }
                            else if (!Me.CurrentTarget.InLineOfSight && !Me.CurrentTarget.InLineOfSpellSight)
                            {
                                while (!Me.CurrentTarget.InLineOfSight && !Me.CurrentTarget.InLineOfSpellSight)
                                {
                                    MoveToTarget(Me.CurrentTarget, 0);
                                }
                                MoveToTarget(Me.CurrentTarget, (int)Me.CurrentTarget.Distance - 4);
                                WoWMovement.MoveStop();
                            }
                        }
                        else if (Me.GotTarget && Me.CurrentTarget.IsFriendly)
                        {
                            if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                            {
                                AssistTarget.CurrentTarget.Target();
                            }
                        }
                    }

                    if (!Me.Combat && !AssistTarget.Combat)
                    {
                        if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                        {
                            if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                            {
                                MoveToTarget(AssistTarget, followNoCombatDist);
                            }
                            if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                            {
                                MoveToTarget(AssistTarget, 1);
                            }
                        }

                    }
                }
                if (String.Compare(Me.SpecType.ToString(), "Tank") == 0)
                {
                    if (Me.GotTarget && Me.CurrentTarget.IsDead)
                    {
                        slog("Clearing Target and Stopping Movement");
                        Me.ClearTarget();
                        WaitForStop();
                        return;
                    }

                    if ((Me.Combat || AssistTarget.Combat))
                    {
                        if (!Me.GotTarget)
                        {
                            if (AssistTarget.GotTarget)
                            {
                                if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                                {
                                    AssistTarget.CurrentTarget.Target();
                                }
                            }
                            else if (!AssistTarget.GotTarget)
                            {
                                if (AssistTarget.Distance >= (followCombatDist + bufferDist))
                                {
                                    MoveToTarget(AssistTarget, followCombatDist);
                                }
                            }
                        }
                        else if (Me.GotTarget && Me.CurrentTarget.Attackable && !Me.CurrentTarget.IsFriendly)
                        {
                            if (Me.Mounted && Me.CurrentTarget.Distance <= 6)
                            {
                                WaitForDismount();
                            }
                            if (Me.CurrentTarget.Distance >= 5)
                            {
                                MoveToTarget(Me.CurrentTarget, 3);
                            }
                            else if (Me.CurrentTarget.Distance < 5)
                            {
                                WoWMovement.MoveStop();
                            }
                            if (!Me.IsFacing(Me.CurrentTarget))
                            {
                                WoWMovement.Face();
                            }

                        }
                        else if (Me.GotTarget && Me.CurrentTarget.IsFriendly)
                        {
                            if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                            {
                                AssistTarget.CurrentTarget.Target();
                            }
                        }
                    }

                    if (!Me.Combat && !AssistTarget.Combat)
                    {
                        if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                        {
                            if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                            {
                                MoveToTarget(AssistTarget, followNoCombatDist);
                            }
                            if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                            {
                                MoveToTarget(AssistTarget, 1);
                            }
                        }

                    }
                }
            }
        }

#region Role Behavior Methods

        private void RoleCheck()
        {
            //slog("Current Spec Type: " + Me.SpecType.ToString());
            if (String.Compare(Me.SpecType.ToString(), "Healer") == 0)
            {
                HealerBehavior();   
            }
            if (String.Compare(Me.SpecType.ToString(), "MeleeDps") == 0)
            {
                MeleeBehavior();
            }
            if (String.Compare(Me.SpecType.ToString(), "RangedDps") == 0)
            {
                RangedBehavior();
            }
            if (String.Compare(Me.SpecType.ToString(), "Tank") == 0)
            {
                MeleeBehavior();
            }   
        }

        private void TankBehavior()
        {
            //slog("Tank Behavior");
        }

        private void MeleeBehavior()
        {

            if (Me.GotTarget && Me.CurrentTarget.IsDead)
            {
                slog("Clearing Target and Stopping Movement");
                Me.ClearTarget();
                WaitForStop();
                return;
            }

            if ((Me.Combat || AssistTarget.Combat))
            {
                if (!Me.GotTarget)
                {
                    if (AssistTarget.GotTarget)
                    {
                        if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                        {
                            AssistTarget.CurrentTarget.Target();
                        }
                    }
                    else if (!AssistTarget.GotTarget)
                    {
                        if(AssistTarget.Distance >= (followCombatDist + bufferDist))
                        {
                            MoveToTarget(AssistTarget, followCombatDist);
                        }
                    }
                }
                else if (Me.GotTarget && Me.CurrentTarget.Attackable && !Me.CurrentTarget.IsFriendly)
                {
                    if (Me.Mounted && Me.CurrentTarget.Distance <= 6)
                    {
                        WaitForDismount();
                    }
                    if (Me.CurrentTarget.Distance >= 5)
                    {
                        MoveToTarget(Me.CurrentTarget, 3);
                    }
                    else if (Me.CurrentTarget.Distance < 5)
                    {
                        WoWMovement.MoveStop();
                    }
                    if (!Me.IsFacing(Me.CurrentTarget))
                    {
                        WoWMovement.Face();
                    }

                }
                else if (Me.GotTarget && Me.CurrentTarget.IsFriendly)
                {
                    if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                    {
                        AssistTarget.CurrentTarget.Target();
                    }
                }
            }

            if (!Me.Combat && !AssistTarget.Combat)
            {
                if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                {
                    if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                    {
                        MoveToTarget(AssistTarget, followNoCombatDist);
                    }
                    if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                    {
                        MoveToTarget(AssistTarget, 1);
                    }
                }

            }

        }

        private void RangedBehavior()
        {

            if (Me.GotTarget && (Me.CurrentTarget.IsDead))
            {
                slog("Clearing Target and Stopping Movement");
                Me.ClearTarget();
                WaitForStop();
                return;
            }

            if ((Me.Combat || AssistTarget.Combat))
            {
                if (!Me.GotTarget)
                {
                    if (AssistTarget.GotTarget)
                    {
                        if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                        {
                            AssistTarget.CurrentTarget.Target();
                        }
                    }
                    else if (!AssistTarget.GotTarget)
                    {
                        if (AssistTarget.Distance >= (followCombatDist + bufferDist))
                        {
                            MoveToTarget(AssistTarget, followCombatDist);
                        }
                    }
                }
                else if (Me.GotTarget && Me.CurrentTarget.Attackable && !Me.CurrentTarget.IsFriendly)
                {
                    if (Me.CurrentTarget.InLineOfSight && Me.CurrentTarget.InLineOfSpellSight)
                    {
                        if (Me.Mounted && Me.CurrentTarget.Distance <= 40)
                        {
                            WaitForDismount();
                        }
                        if (Me.CurrentTarget.Distance >= 40)
                        {
                            MoveToTarget(Me.CurrentTarget, (int)Me.CurrentTarget.Distance - 10);
                        }
                        if (Me.Z - Me.CurrentTarget.Z <= -5)
                        {
                            MoveToTarget(Me.CurrentTarget, (int)Me.CurrentTarget.Distance);
                        }

                        /*
                        if (Me.CurrentTarget.Distance <= 30)
                        {
            kl                WoWMovement.MoveStop();
                        }
                        */

                        if (!Me.IsFacing(Me.CurrentTarget))
                        {
                            WoWMovement.Face();
                        }
                    }
                    else if (!Me.CurrentTarget.InLineOfSight && !Me.CurrentTarget.InLineOfSpellSight)
                    {
                        while (!Me.CurrentTarget.InLineOfSight && !Me.CurrentTarget.InLineOfSpellSight)
                        {
                            MoveToTarget(Me.CurrentTarget, 0);
                        }
                        MoveToTarget(Me.CurrentTarget, (int)Me.CurrentTarget.Distance - 4);
                        WoWMovement.MoveStop();
                    }
                }
                else if (Me.GotTarget && Me.CurrentTarget.IsFriendly)
                {
                    if (AssistTarget.CurrentTarget.Attackable && !AssistTarget.CurrentTarget.IsFriendly)
                    {
                        AssistTarget.CurrentTarget.Target();
                    }
                }
            }

            if (!Me.Combat && !AssistTarget.Combat)
            {
                if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                {
                    if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                    {
                        MoveToTarget(AssistTarget, followNoCombatDist);
                    }
                    if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                    {
                        MoveToTarget(AssistTarget, 1);
                    }
                }

            }

        }

        private void HealerBehavior()
        {

            if (Me.GotTarget && Me.CurrentTarget.IsDead)
            {
                slog("Clearing Target and Stopping Movement");
                Me.ClearTarget();
                WaitForStop();
                return;
            }

            if ((Me.Combat || AssistTarget.Combat))
            {
                if (!Me.IsCasting)
                {
                    if (AssistTarget.Distance >= (followCombatDist + bufferDist))
                    {
                        MoveToTarget(AssistTarget, followCombatDist);
                    }
                }
            }

            if (!Me.Combat && !AssistTarget.Combat)
            {
                if (AssistTarget != Me && AssistTarget.IsValid && AssistTarget != null)
                {
                    if (AssistTarget.Distance >= (followNoCombatDist + bufferDist))
                    {
                        MoveToTarget(AssistTarget, followNoCombatDist);
                    }
                    if (AssistTarget.Distance < (followNoCombatDist + bufferDist) && AssistTarget.ZDiff >= 3)
                    {
                        MoveToTarget(AssistTarget, 1);
                    }
                }

            }

        }

#endregion

#region ToolBox Methods

        private bool preChecks()
        {
            //slog("-----------------");
            
            /*
            if (AssistTarget == null)
            {
                slog("No AssistTarget");
                return false;
            }
             */

            if (Me.CurrentHealth <= 1)
            {
                slog("You're currently Dead or Ghost");
                return false; // dead or ghost
            }

            /*
            if (AssistTarget.CurrentHealth <= 1 && AssistTarget != null)
            {
                slog("Focus currently Dead or Ghost");
                return false; // dead or ghost
            }
            */

            if (Me.IsResting && Me.ManaPercent < 0.90)
            {
                slog("Resting");
                return false;
            }

            if (RoutineManager.Current.NeedRest || RoutineManager.Current.NeedPreCombatBuffs || RoutineManager.Current.NeedPullBuffs /*|| RoutineManager.Current.NeedHeal*/)
            {
                slog("Need to Rest/Buff");
                return false;
            }
            else
                return true;

        }

        public void MountCheck()
        {
            if (NeedToMount())
            {
                slog("Mounting");
                WaitForMount();
            }

            if (NeedToDismount())
            {
                slog("DisMounting");
                MoveToTarget(AssistTarget, 0);
                if (AssistTarget.Distance < 5)
                {
                    WaitForDismount();
                }
            }
        }

        private void MoveToTarget(WoWUnit target, int distance)
        {
            var location = WoWMovement.CalculatePointFrom(target.Location, distance);
            if (Me.Combat &&
                ((Me.Location.Distance(location) <= 10 && Me.Z - location.Z >= 3) || (Me.Location.Distance(location) <= 10 && target.InLineOfSight)))
            {
                slog("Clicking to Move to : " + target);
                WoWMovement.ClickToMove(location);
            }
            else
            {
                if (target != null &&  
                    (target.IsFlying || target.IsSwimming || Me.IsFlying))
                {
                    slog("Moving using Flightor to target : " + target);
                    Flightor.MoveTo(location);
                }
                else
                {
                    slog("Moving using Navigator to target : " + target);
                    Navigator.MoveTo(location);
                }
            }
        }

        private bool NeedToMount()
        {
            return !Me.Mounted
                && CharacterSettings.Instance.UseMount
                && AssistTarget != null
                && (AssistTarget.Distance > NeedToMountDistance || AssistTarget.Mounted)
                && Me.IsOutdoors
                && Mount.CanMount();
        }

        public static int NeedToMountDistance
        {
            get
            {
                return Math.Max(CharacterSettings.Instance.MountDistance, 40);
            }
        }

        public bool NeedToDismount()
        {
            return Me.Mounted
                && CharacterSettings.Instance.UseMount
                && AssistTarget != null
                && AssistTarget.Distance <= 15
                && (!AssistTarget.Mounted && AssistTarget.IsPlayer);
        }

        public static void WaitForMount()
        {
            if (Me.Combat || Me.IsIndoors || !CharacterSettings.Instance.UseMount)
                return;

            WaitForStop();
            WoWPoint ptStop = Me.Location;

            var timeOut = new Stopwatch();
            timeOut.Start();

            if (Mount.Mounts.Count() == 0 || !Mount.CanMount())
                return;

            slog("Attempting to mount via HB...");
            Mount.MountUp(LazyLocationRetriever);
            StyxWoW.SleepForLagDuration();

            while (IsGameStable() && Me.CurrentHealth > 1 && Me.IsCasting)
            {
                Thread.Sleep(75);
            }

            if (!Me.Mounted)
            {
                slog("unable to mount after {0} ms", timeOut.ElapsedMilliseconds);
                if (ptStop.Distance(Me.Location) != 0)
                    slog("character was stopped but somehow moved {0:F3} yds while trying to mount", ptStop.Distance(Me.Location));
            }
            else
            {
                slog("Mounted");
            }
        }

        public static void WaitForDismount()
        {
            while (IsGameStable() && Me.CurrentHealth > 1 && Me.Mounted)
            {
                Lua.DoString("Dismount()");
                // Mount.Dismount();  // HB API forces Stop also, so use LUA to keep running and let Squire or CC stop if needed
                StyxWoW.SleepForLagDuration();
            }
        }

        public static void WaitForStop()
        {
            // excessive attempt to make sure HB doesn't have any cached movement
#if DO_CRAZY_EXCESSIVE_STOP_EVERY_MOVEMENT
            WoWMovement.MoveStop(WoWMovement.MovementDirection.AutoRun);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.ClickToMove);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.Descend);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.ForwardBackMovement);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.JumpAscend);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.PitchDown);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.PitchUp);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeLeft);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeRight);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.TurnLeft);
            WoWMovement.MoveStop(WoWMovement.MovementDirection.TurnRight);
#endif
            WoWMovement.MoveStop(WoWMovement.MovementDirection.All);
            WoWMovement.MoveStop();
            Navigator.PlayerMover.MoveStop();

            do
            {
                StyxWoW.SleepForLagDuration();
            } while (IsGameStable() && Me.CurrentHealth > 1 && Me.IsMoving);
        }

        public static bool IsMelee(WoWPlayer p)
        {
            return p.Intellect < (p.Strength + p.Agility);
        }

        public static WoWPoint LazyLocationRetriever()
        {
            return WoWPoint.Empty;
        }

        public static bool IsGameStable()
        {
            return StyxWoW.IsInGame && Me != null && Me.IsValid;
        }

#endregion
    }
}

