using System;
using Styx;
using Styx.CommonBot.Routines;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace HighVoltz.Composites
{
    public class FollowPathAction : Action
    {

        private readonly LocalPlayer _me = StyxWoW.Me;
        private readonly AutoAnglerSettings _settings = AutoAngler.Instance.MySettings;

        protected override RunStatus Run(object context)
        {
            if (LootAction.GetLoot())
                return RunStatus.Success;
            //  dks can refresh water walking while flying around.
            if (AutoAngler.Instance.MySettings.UseWaterWalking &&
                StyxWoW.Me.Class == WoWClass.DeathKnight && !WaterWalking.IsActive)
            {
                WaterWalking.Cast();
            }
            if (AutoAngler.CurrentPoint == WoWPoint.Zero )
                return RunStatus.Failure;
            if (AutoAngler.FishAtHotspot && StyxWoW.Me.Location.Distance(AutoAngler.CurrentPoint) <= 3)
            {
                return RunStatus.Failure;
            }
            //float speed = StyxWoW.Me.MovementInfo.CurrentSpeed;
            //float modifier = _settings.Fly ? 5f : 2f;
            //float precision = speed > 7 ? (modifier*speed)/7f : modifier;
            float precision = StyxWoW.Me.IsFlying ? AutoAnglerSettings.Instance.PathPrecision : 3; 
            if (StyxWoW.Me.Location.Distance(AutoAngler.CurrentPoint) <= precision)
                AutoAngler.CycleToNextPoint();
            if (_settings.Fly)
            {
                if (_me.IsSwimming)
                {
                    if (_me.GetMirrorTimerInfo(MirrorTimerType.Breath).CurrentTime > 0)
                        WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                    else if (_me.MovementInfo.IsAscending || _me.MovementInfo.JumpingOrShortFalling)
                        WoWMovement.MoveStop(WoWMovement.MovementDirection.JumpAscend);
                }
                if (!StyxWoW.Me.Mounted)
                    Flightor.MountHelper.MountUp();
                Flightor.MoveTo(AutoAngler.CurrentPoint);
            }
            else
            {
                if (!StyxWoW.Me.Mounted && Mount.ShouldMount(AutoAngler.CurrentPoint) && Mount.CanMount())
                    Mount.MountUp(() => AutoAngler.CurrentPoint);
                Navigator.MoveTo(AutoAngler.CurrentPoint);
            }
            return RunStatus.Success;
        }

 
    }
}