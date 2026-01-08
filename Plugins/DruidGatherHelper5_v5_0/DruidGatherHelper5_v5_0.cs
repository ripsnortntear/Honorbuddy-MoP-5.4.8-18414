/*
 * Druid Harvest Helper
 *      Will lock HB if we are within 10 yards of herb,
 *      in Flight Form and under fire (in combat) and
 *      attempt to harvest herb itself.
 *      
 *      As GB2's harvesting is not accessible from outside
 *      i had to write my own harvesting, 
 *      here are "implications":
 *      - GB2 might add those nodes to blacklist even if DHH
 *        successfully harvest them,
 *      - every 2 minutes DHH will verify GB2 blacklist
 *        removing those actually harvested by DHH,
 *        
 * thanks to bobby53 that i even started writing this :)
 *        
 * Author:  strix
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Bots.Gatherbuddy;
using Styx;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;

namespace DruidHarvestHelper
{
    public class DruidHarvestHelper : HBPlugin
    {
        public override string Name
        {
            get { return "Druid's Harvest Helper"; }
        }

        public override string Author
        {
            get { return "strix and the community"; }
        }

        public override Version Version
        {
            get { return new Version(5, 0, 1); }
        }

        public override bool WantButton
        {
            get { return false; }
        }

        public LocalPlayer Me
        {
            get { return Styx.StyxWoW.Me; }
        }

        private static bool _isHarvesting;
        private static bool _haveInitialized;

        private readonly HashSet<WoWPoint> _latestHarvests = new HashSet<WoWPoint>();
        private WoWGameObject _currentHerb;

        private readonly WaitTimer _wtVerifyGb2Blacklist = new WaitTimer(new TimeSpan(0, 2, 0));
        private readonly WaitTimer _wtHelper = new WaitTimer(new TimeSpan(0, 0, 3));
        private readonly WaitTimer _wtInteractHelper = StyxWoW.Me.Race == WoWRace.Tauren ?
            new WaitTimer(new TimeSpan(0, 0, 0, 1, 300))
            : new WaitTimer(new TimeSpan(0, 0, 0, 2, 0));

        private bool _interactedFlag;
        private bool _shouldAbandonHerb;
        private int _failCounter;

        private const int MaxFailedAttempts = 5;

        public override void Pulse()
        {
            if (!_haveInitialized)
            {
                _isHarvesting = TreeRoot.Current != null &&
                    "GATHERBUDDY2" == Left(TreeRoot.Current.Name, 12).ToUpper() &&
                    Me.Class == WoWClass.Druid;
                if (_isHarvesting)
                {
                    slog(
                         "Druid using Gatherbuddy 2 detected... locking HB and launching Druid Harvest Helper harvest if following conditions are met:");
                    slog(" - we are in Flight Form,");
                    slog(" - we are under fire,");
                    slog(" - there is herb withing 10 yards range,");
                }
                _haveInitialized = true;
            }

            ChooseForm();

            if (_isHarvesting &&
                (_interactedFlag || HerbWithinTen2DRange != null) )//&&
                //(Me.Shapeshift == ShapeshiftForm.FlightForm || Me.Shapeshift == ShapeshiftForm.EpicFlightForm))
            {

                //Cast("Barkskin");
                HarvestHerb();
            }

            Reset();

            if (!Me.Combat && _wtVerifyGb2Blacklist.IsFinished)
                VerifyGB2BlacklistAndClearLatestHarvests();
        }

        private static string Left(string s, int c)
        {
            return String.IsNullOrEmpty(s) ? s : s.Substring(0, Math.Min(c, s.Length));
        }

        private void slog(string format, params object[] args)
        {
            Logging.Write("[Harvest Helper] " + format, args);
        }

        private void elog(string format, params object[] args)
        {
            Logging.Write("[Harvest Helper] " + format, args);
        }

        private void dlog(string format, params object[] args)
        {
           Logging.Write("<Harvest Helper> " + format, args);
        }

        public static WoWGameObject HerbWithinInteractRange
        {
            get
            {
                ObjectManager.Update();
                IOrderedEnumerable<WoWGameObject> tars =
                    (from o in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                     where
                         o.IsHerb && o.WithinInteractRange &&
                         o.RequiredSkill <= StyxWoW.Me.GetSkill("Herbalism").CurrentValue
                     orderby o.DistanceSqr ascending
                     select o);
                return tars.Count() > 0 ? tars.First() : null;
            }
        }

        public static WoWGameObject HerbWithinTen2DRange
        {
            get
            {
                ObjectManager.Update();
                IOrderedEnumerable<WoWGameObject> tars =
                    (from o in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                     where
                         o.IsHerb && o.Distance2D < 10 &&
                         o.RequiredSkill <= StyxWoW.Me.GetSkill("Herbalism").CurrentValue
                     orderby o.DistanceSqr ascending
                     select o);
                return tars.Count() > 0 ? tars.First() : null;
            }
        }

        public static WoWGameObject Herb
        {
            get
            {
                ObjectManager.Update();
                IOrderedEnumerable<WoWGameObject> tars =
                    (from o in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                     where o.IsHerb && o.RequiredSkill <= StyxWoW.Me.GetSkill("Herbalism").CurrentValue
                     orderby o.DistanceSqr ascending
                     select o);
                return tars.Count() > 0 ? tars.First() : null;
            }
        }

        public void VerifyGB2BlacklistAndClearLatestHarvests()
{
    slog("Verifying GB2 blacklist.");
    
    var itemsToRemove = new List<WoWPoint>();
    
    foreach (WoWPoint point in _latestHarvests)
    {
        itemsToRemove.AddRange(GatherbuddyBot.BlacklistNodes.Keys.Where(key => key == point));
    }

    foreach (var pointToRemove in itemsToRemove)
    {
        GatherbuddyBot.BlacklistNodes.Remove(pointToRemove);
    }

    _latestHarvests.Clear();
    _wtVerifyGb2Blacklist.Reset();
}


        public void HarvestHerb()
        {
            if (!_interactedFlag && Me.Combat)
            {
                slog("Starting harvesting...");
                dlog("HarvestHerb(): starting...");
                if (_currentHerb == null)
                {
                    dlog("HarvestHerb(): Assigning currentHerb...");
                    _currentHerb = Herb;
                }

                if (_currentHerb.Distance2D > 5)
                {
                    WoWMovement.ClickToMove(WoWMathHelper.CalculatePointFrom(Me.Location, Herb.Location, 1));
                    dlog("HarvestHerb(): Getting within interaction range of herb...");
                    _wtHelper.Reset();
                    while (_currentHerb.Distance2D > 5)
                    {
                        if (_wtHelper.IsFinished)
                        {
                            dlog("HarvestHerb(): currentHerb.Distance2d < 5 timed out...");
                            break;
                        }
                    }
                }
                if (Me.IsFlying)
                {
                    dlog("HarvestHerb(): Getting on the ground...");
                    ActuallyStopMoving();
                    Navigator.PlayerMover.Move(WoWMovement.MovementDirection.Descend);
                    _wtHelper.Reset();
                    while (Me.IsFlying && !_wtHelper.IsFinished) ;
                }
                dlog("HarvestHerb(): Interacting with currentHerb started..");
                _failCounter = 0;
                if (_currentHerb.IsValid)
                    dlog("is valid");
                if (!_currentHerb.IsDisabled)
                    dlog("is not disabled");
                if (_currentHerb.CanUseNow())
                    dlog("can use now");

                //while (_currentHerb.IsValid && !_currentHerb.IsDisabled && _currentHerb.CanUseNow())
                while (_currentHerb.IsValid && _currentHerb.CanUseNow())
                {
                    dlog("in while loop");

                    ObjectManager.Update();
                    _wtInteractHelper.Reset();
                    _currentHerb.Interact();
                    if (_failCounter > MaxFailedAttempts)
                    {
                        dlog("HarvestHerb(): Harvesting herb failed for " + _failCounter + " times, abandoning it...");
                        _shouldAbandonHerb = true;
                        break;
                    }
                    while (!Me.IsCasting)
                    {
                        if (_wtInteractHelper.IsFinished)
                        {
                            _failCounter++;
                            dlog("HarvestHerb(): failed for " + _failCounter + " time...");
                            break;
                        }
                    }
                }
                if (!_shouldAbandonHerb)
                    dlog("HarvestHerb(): Interacting with currentHerb finished..");

                _interactedFlag = true;
            }
            if (_interactedFlag)
            {
                if (!Me.IsFlying)
                {
                    dlog("HarvestHerb(): JumpAscend to exit combat...");
                    _wtHelper.Reset();
                    while (Me.Combat && !_wtHelper.IsFinished)
                    {
                        //Navigator.PlayerMover.Move(
                        //    WoWMovement.MovementDirection.JumpAscend);
                        if (!Me.IsMoving) WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(200)); System.Threading.Thread.Sleep(200);
                    }
                    //while (Me.Combat && !_wtHelper.IsFinished) ;
                    ActuallyStopMoving();
                }
                if (_shouldAbandonHerb)
                    _currentHerb = null;
                if (_currentHerb != null)
                {
                    if (_latestHarvests.Add(_currentHerb.Location))
                        dlog("Added currentHerb to latestHarvests");
                    _currentHerb = null;
                    _interactedFlag = false;
                }
                dlog("HarvestHerb() finished.");
                slog("Harvesting finished.");
                return;
            }
        }

        public void ActuallyStopMoving()
        {
            var timeout = new WaitTimer(new TimeSpan(0, 0, 3));
            timeout.Reset();
            dlog("Stopping movement...");
            while (Me.IsMoving || timeout.IsFinished)
            {
                Navigator.PlayerMover.MoveStop();
                WoWMovement.MoveStop();
            }
            if (!Me.IsMoving)
                dlog("Movement stopped");
        }

        private void ChooseForm()
        {
            if (_isHarvesting &&
                !(Me.GotTarget &&
                  !Me.CurrentTarget.IsFriendly &&
                  !Me.CurrentTarget.IsDead
                 ) &&
                !Me.Combat &&
                !Me.Mounted &&
                !Me.HasAura("Aquatic Form") &&
                !Me.HasAura("Travel Form") &&
                Me.IsMoving)
            {
                if (Me.IsSwimming)
                    Cast("Aquatic Form");

                if (!Me.IsSwimming)
                    if (!Cast("Swift Flight Form"))
                        Cast("Flight Form");
            }
        }

        private void Reset()
        {
            _interactedFlag = false;
            _currentHerb = null;
            _failCounter = 0;
            _shouldAbandonHerb = false;
        }

        private bool Cast(string spellName)
        {
            return
                SpellManager.HasSpell(spellName) &&
                SpellManager.CanCast(spellName) &&
                SpellManager.Cast(spellName);
        }
    }
}