#region Revision info
/*
 * $Author: millz $
 * $Date: 2013-04-27 09:32:20 +0100 (Sat, 27 Apr 2013) $
 * $ID: $
 * $Revision: 23 $
 * $URL: https://subversion.assembla.com/svn/iwantmovement/trunk/IWantMovement/Managers/Rest.cs $
 * $LastChangedBy: millz $
 * $ChangesMade: $
 */
#endregion

using System;
using CommonBehaviors.Actions;
using IWantMovement.Helper;
using IWantMovement.Settings;
using Styx;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.POI;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

/*
 * 
 * A lot of code in here was taken from CLU's Movement.cs with permission from Wulf.
 * Big credits/thanks go to the CLU/PureRotation team (past and present) for code in here.
 * 
 * -- Millz
 * 
 */

namespace IWantMovement.Managers
{
    class Rest
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static IWMSettings Settings { get { return IWMSettings.Instance; } }

        public static Composite DefaultRestBehaviour()
        {
            return

                // Don't fucking run the rest behavior (or any other) if we're dead or a ghost. Thats all.
                new Decorator(
                    ret => !Me.IsDead && !Me.IsGhost && !Me.IsCasting && Settings.EnableRest && BotPoi.Current.Type != PoiType.Loot && !Me.IsActuallyInCombat,
                    new PrioritySelector(

                // Make sure we wait out res sickness. Fuck the classes that can deal with it. :O
                        new Decorator(ret => StyxWoW.Me.HasAura("Resurrection Sickness"), new Action(ret => { })),

                // Check if we're allowed to eat (and make sure we have some food. Don't bother going further if we have none.
                        new Decorator(
                            ret =>
                            !StyxWoW.Me.IsSwimming && StyxWoW.Me.HealthPercent <= IWMSettings.Instance.EatPercent && !StyxWoW.Me.HasAura("Food") &&
                            Consumable.GetBestFood(true) != null && !StyxWoW.Me.IsCasting,
                            new PrioritySelector(
                                new Decorator(
                                    ret => StyxWoW.Me.IsMoving,
                                    new Action(ret => Navigator.PlayerMover.MoveStop())),
                                new Sequence(
                                    new Action(ret => Log.Info("[Rest] [Eating]")),
                                    new Action(ret => Styx.CommonBot.Rest.FeedImmediate()),
                                    CreateWaitForLagDuration()))),

                // Make sure we're a class with mana, if not, just ignore drinking all together! Other than that... same for food.
                        new Decorator(
                            ret =>
                            !StyxWoW.Me.IsSwimming && (StyxWoW.Me.PowerType == WoWPowerType.Mana) &&
                            StyxWoW.Me.ManaPercent <= IWMSettings.Instance.DrinkPercent &&
                            !StyxWoW.Me.HasAura("Drink") && Consumable.GetBestDrink(true) != null && !StyxWoW.Me.IsCasting,
                            new PrioritySelector(
                                new Decorator(
                                    ret => StyxWoW.Me.IsMoving,
                                    new Action(ret => Navigator.PlayerMover.MoveStop())),
                                new Sequence(
                                    new Action(ret => Log.Info("[Rest] [Drinking]")),
                                    new Action(ret => Styx.CommonBot.Rest.DrinkImmediate()),
                                    CreateWaitForLagDuration()))),

                // This is to ensure we STAY SEATED while eating/drinking. No reason for us to get up before we have to.
                        new Decorator(
                            ret =>
                            (StyxWoW.Me.HasAura("Food") && StyxWoW.Me.HealthPercent < 98) ||
                            (StyxWoW.Me.HasAura("Drink") && StyxWoW.Me.PowerType == WoWPowerType.Mana && StyxWoW.Me.ManaPercent < 98),
                            new ActionAlwaysSucceed()),
                        new Decorator(
                            ret =>
                            ((StyxWoW.Me.PowerType == WoWPowerType.Mana && StyxWoW.Me.ManaPercent <= 60) ||
                             StyxWoW.Me.HealthPercent <= 60) && !StyxWoW.Me.CurrentMap.IsBattleground,
                            new Action(ret => Log.Warning("We have no food/drink. Waiting to recover our health/mana back")))
                    ));
        }

        public static Composite CreateWaitForLagDuration()
        {
            return new WaitContinue(TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150), ret => false, new ActionAlwaysSucceed());
        }
    }
}
