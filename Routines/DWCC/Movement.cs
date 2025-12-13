using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Styx.Plugins;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Pathing;

namespace DWCC
{
    public static class Movement
    {
        
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        private static WoWPlayer Me = StyxWoW.Me;
        private static WoWUnit Target; 
        private static int Cone = 40;  

        internal static void PulseMovement()
        {
            if (!DunatanksSettings.Instance.DisableMovement)
            {
                try
                {
                    if (StyxWoW.Me.CurrentTarget == null)
                    {
                        WoWMovement.StopFace();
                        StopMovement();
                    }

                    if (StyxWoW.IsInGame == false) return;
                    if (Me.IsValid == false) return;
                    if (Me.CurrentTarget == null) return;
                    if (Me.GotTarget == false) return;
                    if (Me.Mounted) return;
                    if (Me.IsDead) return;
                    if (Me.CurrentTarget.IsPlayer == false) return;

                    Target = Me.CurrentTarget;
                    if (Target.Distance > 12) return;
                    if (Target.IsDead) return;
                    if (!Target.IsHostile) return;
                    if (!Target.Attackable) return;

                    CheckFace();
                    if (CheckMoving()) return;
                    if (CheckStop()) return;
                    CheckStrafe();
                }
                catch (System.Exception) { } 
            }
        }

        private static void CheckFace()
        {
            if (!WoWMovement.IsFacing)
            {
                WoWMovement.Face(Target.Guid);
            }
        }

        private static bool CheckMoving()
        {
            if (Target.Distance >= 1.5 && Target.IsMoving && !Me.MovementInfo.MovingForward)
            {
                WoWMovement.Move(WoWMovement.MovementDirection.Forward);
                return true;
            }


            if (Target.Distance < 1.5 && Target.IsMoving && Me.MovementInfo.MovingForward)
            {
                WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward);
                return true;
            }

            return false;
        }

        private static bool CheckStop()
        {

                if (Target.IsMoving) return false;
                float Distance = 3.2f;

                if (Target.Distance >= Distance && !Me.MovementInfo.MovingForward)
                {
                    WoWMovement.Move(WoWMovement.MovementDirection.Forward, new TimeSpan(99, 99, 99));
                    return true;
                }

                if (Target.Distance < 2 && Me.IsMoving && Me.MovementInfo.MovingForward)
                {
                    WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward);
                }

                return false;
        }

        private static void StopMovement()
        {
            if (Me.MovementInfo.MovingStrafeRight && !KeyDown(Keys.D))
                WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeRight);

            if (Me.MovementInfo.MovingStrafeLeft && !KeyDown(Keys.A))
                WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeLeft);

            if (Me.MovementInfo.MovingForward && !KeyDown(Keys.W))
                WoWMovement.MoveStop(WoWMovement.MovementDirection.Forward);
        }

        private static void CheckStrafe()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                if (Me.Stunned) return;

                if (Me.MovementInfo.MovingStrafeRight && Target.Distance >= 2.5)
                {
                    WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeRight);
                    return;
                }

                if (Me.MovementInfo.MovingStrafeLeft && Target.Distance >= 2.5)
                {
                    WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeLeft);
                    return;
                }

                if (Me.MovementInfo.MovingStrafeRight && GetDegree <= 180 && GetDegree >= Cone)
                {
                    WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeRight);
                    return;
                }
                if (Me.MovementInfo.MovingStrafeLeft && GetDegree >= 180 && GetDegree <= (360 - Cone))
                {
                    WoWMovement.MoveStop(WoWMovement.MovementDirection.StrafeLeft);
                    return;
                }

                if (!Target.IsWithinMeleeRange) return;


                if (GetDegree >= 180 && GetDegree <= (360 - Cone) && !Me.MovementInfo.MovingStrafeRight)
                {
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeRight, new TimeSpan(99,99,99));
                    return;
                }

                if (GetDegree <= 180 && GetDegree >= Cone && !Me.MovementInfo.MovingStrafeLeft)
                {
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, new TimeSpan(99, 99, 99));
                    return;
                }
            }
        }

        internal static double GetDegree
        {
            get
            {
                double d = Math.Atan2((Target.Y - Me.Y), (Target.X - Me.X));

                double r = d - Target.Rotation;
                if (r < 0)
                    r += (Math.PI * 2);

                return WoWMathHelper.RadiansToDegrees((float)r);
            }            
        }

        internal static bool KeyDown(Keys VKey)
        {
            if (GetAsyncKeyState(VKey) != 0) return true;
            return false;
        }
    }
}
