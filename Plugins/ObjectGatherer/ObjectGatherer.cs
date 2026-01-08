#region Information
/* 
 * ObjectGatherer by AknA
 * 
 * Original idea by LiquidAtoR but I decided to recode what he had done and make
 * a whole new plugin.
 * 
 * Special thanks to chinajade and Inrego for your endless help with my silly questions.
 * And to BarryDurex for the avoid logics.
 * 
 * Ver 0.53 Beta
 * First official Beta testing stage.
 * 
 * Ver 0.57 Beta
 * Fixed a spamming issues with Logging.Write if you're exalted with a faction.
 * Fixed the interact timer.
 * 
 * Ver 0.58 Beta
 * Tweaked the timer logic abit more.
 * 
 * Ver 0.60 Beta
 * Just found out that HB had a Stopwatch() function (yes, I'm a retard).
 * Added the NPC id's for Lost And Found achievement (not working yet tho)
 * 
 * Ver 0.63 Beta
 * Reworked the Interact behavior after some testing, it should work now (I hope).
 * Fixed some Logging.Write spamming issues.
 * 
 * Ver 0.69 Beta
 * Implemented the blacklist again with a new logic, hopefully it should work.
 * Removed some Logging.Write spam as that part of the plugin is working.
 * Removed alot of not needed code to optimate plugin some more :)
 * 
 * Ver 1.00
 * No more Beta :)
 * Figued out a way to check if the object we found can be looted (no need for faction check).
 * So don't need to wait for HonorBuddy to be updated before taking this out of Beta.
 * Everything should work now after my own testing. (Still report if it doesn't.)
 * So now I can concentrate on adding new features instead.
 * 
 * Ver 1.10
 * Added logic for WoWUnit as well.
 * Should be able to interact with the npc's for the Lost and Found achievement now.
 * Added all 5 npc id's for the Lost and Found achievement.
 * Allso improved the interact logic to not clear the waypoint until we're sure we have looted/interacted.
 * 
 * Ver 1.20
 * Fixed the issues that I broke the plugin with last update (thanks chinajade)
 * Improved the code alot.
 * Now everything should work (I hope), npc interact is still untested.
 * Added Gold Coins by request.
 * 
 * Ver 1.21
 * Finaly found a NPC, it interacted but didn't select gossip option correctly, testing a new method.
 * Select Gossip option (1) is still untested, please report if it works or not for you.
 * Allso recoded the interact logic abit.
 * 
 * Ver 1.25
 * NPC interact is now working, had to do a lua script for it to work correct but atleast it's working now.
 * Updated MoveTo to include both Navigator and Flightor to improve the logic abit more.
 * Added aditional checks to clear the waypoint if we are dead, ghost or in combat, hopefully it works now.
 * 
 * Ver 1.30
 * GUI has been added. (However the settings doesn't change anything yet :P)
 * Added a new cathegory for Quests and added a few items (if it's any quest items you want me to add please comment)
 * Tweaks in the move to logic and more checks to clear waypoint if it takes too long etc.
 * 
 * Ver 1.31
 * All the settings in the GUI is finaly working, now you can choose if you want the plugin to not search for a itemgroup.
 * 
 * Ver 1.35
 * Added the option to Skin / Herb / Mine corpses if you have the ability to do so (BETA!!!) as requested.
 * 
 * Ver 1.40
 * Now the option to Skin / Herb / Mine corpses if you have the profession should work.
 * 
 * Ver 1.41
 * Tweaked the moveto logic (forgot to add the check for SpecialToFind).
 * 
 * Ver 1.43
 * Fixed the annoying hb error.
 * Allso made the logic for SpecialToFind ALOT better :)
 * 
 * Ver 1.45
 * Tweaked the skinning/mining/herbing part to only use navigator and not flightor.
 * Looks more human this way.
 * 
 * Ver 1.50
 * Recoded the interact logic completly to make it act more human.
 * Added a Lua Event to confirm BOP items so it won't be any issues with it anymore.
 * Added the ID's for Mysterious Camel Figurine (However, there is no logic to attack the mob 
 * in case you find the correct one, so you need to monitor the bot)
 * 
 * Ver 1.53
 * Made a extra check in MoveToObject to see that we really can reach the object.
 * 
 * Ver 1.54
 * Rewrote the MoveTo logic.
 * Allso readded the check if you're a ghost to not search for stuff (accidently removed it)
 * 
 * Ver 1.55
 * Rewrote the loot logic for ObjectToFind (This should resolv the issues with BOP items)
 * Fixed a issue that if you where in combat for too long then it wouldn't loot the item.
 * 
 * Ver 1.56
 * Changed the loot logic abit more to make sure we have looted the item before moving again.
 * 
 * Ver 1.57
 * Updated the GUI, move the buttons around abit and catogarized it.
 * Allso added new things to the GUI (not working yet) but for future updates.
 * 
 * Ver 2.00
 * Completly recoded plugin to use Behavior Tree.
 * Added Local Blacklist (thanks Chinajade for the code).
 * 
 * Ver 2.01
 * Fixed the bugs I created when I made some methods private when they should have been public.
 * I made a few methods none static when they should have been static.
 * Hopefully it should be fixed now.
 * 
 * Ver 2.02
 * Fixed so that we don't have a null reference when we search for objects.
 * 
 * Ver 2.03
 * Fixed some issues with BT, it will now correctly hook into Loot_Main
 * 
 * Ver 2.04
 * Added Kor'kron items under quests as requested.
 */
#endregion

#region Using
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.Plugins;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

using CommonBehaviors.Actions;
using Action = Styx.TreeSharp.Action;
#endregion System Namespace

namespace ObjectGatherer {
    public class ObjectGatherer : HBPlugin {
        #region Variables
        public override string Name { get { return "ObjectGatherer"; } }
        public override string Author { get { return "AknA"; } }
        public override Version Version { get { return new Version(2, 0, 4); } }
        public static void OGlog(string message, params object[] args) { Logging.Write(Colors.DeepSkyBlue, "[ObjectGatherer]: " + message, args); }
        public static void OGdiag(string message, params object[] args) { Logging.WriteDiagnostic(Colors.DeepSkyBlue, "[ObjectGatherer]: " + message, args); }
        public static uint[] Filterlist;
        private LocalPlayer Me { get { return StyxWoW.Me; } }
        private WoWPoint _LocationId = WoWPoint.Empty;
        private WoWGameObject _ObjectToFind;
        private WoWUnit _NPCToFind;
        private WoWUnit _SHMToFind;
        private WoWGameObject _ObjNext;
        private WoWUnit _NPCNext;
        private WoWUnit _SHMNext;
        private bool _Miner;
        private bool _Skinner;
        private bool _Herber;
        private int _Interactway;
        private bool _Initialized;
        private readonly Stopwatch _CheckPointTimer = new Stopwatch();
        private Composite _BehaviorTreeHookObjectGatherer;
        private readonly LocalBlacklist _ObjectBlacklist = new LocalBlacklist(TimeSpan.FromSeconds(30));
        public delegate string ProvideStringDelegate(object context);
        #endregion

        #region Initialize
        public override void Initialize() {
            _LocationId = WoWPoint.Empty;
            _BehaviorTreeHookObjectGatherer = null;
            if (_Initialized) return;
            _Initialized = true;
            OGlog("Version " + Version + " Loaded.");
            Filterlist = UpdateFilterList();
            BotEvents.OnBotStarted += BotEvent_OnBotStarted;
            BotEvents.OnBotStopped += BotEvent_OnBotStopped;
            if (Me.GetSkill(SkillLine.Mining).CurrentValue != 0) { _Miner = true; }
            if (Me.GetSkill(SkillLine.Skinning).CurrentValue != 0) { _Skinner = true; }
            if (Me.GetSkill(SkillLine.Herbalism).CurrentValue != 0) { _Herber = true; }
        }
        #endregion

        #region Dispose
        public override void Dispose() {
            _LocationId = WoWPoint.Empty;
            TreeHooks.Instance.RemoveHook("Loot_Main", _BehaviorTreeHookObjectGatherer);
            _BehaviorTreeHookObjectGatherer = null;
        }
        #endregion

        #region Load GUI
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress() { var GUI = new ObjectGatherer_Gui(); GUI.ShowDialog(); }
        #endregion

        #region Behavior Tree
        private Composite CreateBehaviorObjectGatherer() { 
            return new PrioritySelector(
                new Decorator(context => CanSaflyLootCheck(),
                    new Sequence(
                        new DecoratorContinue(context => _LocationId == WoWPoint.Empty,
                            #region UpdateObjectList
                            new Sequence(
                                new Action(context => ObjectManager.Update()),
                                new Action(context => _NPCNext = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => Filterlist.Contains(u.Entry) && u.CanSelect && !_ObjectBlacklist.Contains(u)).OrderBy(u => u.Distance).FirstOrDefault()),
                                new Action(context => _ObjNext = ObjectManager.GetObjectsOfType<WoWGameObject>().Where(o => (o.Distance2D <= LootTargeting.LootRadius) && Filterlist.Contains(o.Entry) && o.CanUse() && !_ObjectBlacklist.Contains(o)).OrderBy(o => o.Distance).FirstOrDefault()),
                                new Action(context => _SHMNext = ObjectManager.GetObjectsOfType<WoWUnit>().Where(s => s.CanSkin && s.Distance < 20 && !_ObjectBlacklist.Contains(s)).OrderBy(s => s.Distance).FirstOrDefault()),
                                
                                #region Search for Skinn-/Herb-/Minable NPC's
                                new DecoratorContinue(context => ObjectGatherer_Settings.Instance.ShmcCb && _SHMNext != null && _LocationId == WoWPoint.Empty,
                                    new Sequence(
                                        #region Mineing
                                        new DecoratorContinue(context => _SHMNext.SkinType == WoWCreatureSkinType.Rock && _Miner && _LocationId == WoWPoint.Empty,
                                            new Sequence(
                                                new DecoratorContinue(context => (!Navigator.CanNavigateFully(Me.Location, _SHMNext.Location)),
                                                    new Sequence(
                                                        new Action(context => _ObjectBlacklist.Add(_SHMNext, TimeSpan.FromSeconds(300))),
                                                        new Action(context => _LocationId = WoWPoint.Empty)
                                                    )
                                                ),
                                                new DecoratorContinue(context => (_SHMToFind != _SHMNext),
                                                    new Sequence(
                                                        new Action(context => OGlog("Moveing to Mine {0}", _SHMNext.Name)),
                                                        new Action(context => _SHMToFind = _SHMNext),
                                                        new Action(context => _Interactway = 3),
                                                        new Action(context => WoWMovement.MoveStop()),
                                                        new Action(context => _CheckPointTimer.Restart()),
                                                        new Action(context => _LocationId = WoWMovement.CalculatePointFrom(_SHMNext.Location, 3))
                                                    )
                                                )
                                            )
                                        ),
                                        #endregion

                                        #region Herbing
                                        new DecoratorContinue(context => _SHMNext.SkinType == WoWCreatureSkinType.Herb && _Herber && _LocationId == WoWPoint.Empty,
                                            new Sequence(
                                                new DecoratorContinue(context => (!Navigator.CanNavigateFully(Me.Location, _SHMNext.Location)),
                                                    new Sequence(
                                                        new Action(context => _ObjectBlacklist.Add(_SHMNext, TimeSpan.FromSeconds(300))),
                                                        new Action(context => _LocationId = WoWPoint.Empty)
                                                    )
                                                ),
                                                new DecoratorContinue(context => (_SHMToFind != _SHMNext),
                                                    new Sequence(
                                                        new Action(context => OGlog("Moveing to Herb {0}", _SHMNext.Name)),
                                                        new Action(context => _SHMToFind = _SHMNext),
                                                        new Action(context => _Interactway = 3),
                                                        new Action(context => WoWMovement.MoveStop()),
                                                        new Action(context => _CheckPointTimer.Restart()),
                                                        new Action(context => _LocationId = WoWMovement.CalculatePointFrom(_SHMNext.Location, 3))
                                                    )
                                                )
                                            )
                                        ),
                                        #endregion

                                        #region Skinning
                                        new DecoratorContinue(context => _SHMNext.SkinType == WoWCreatureSkinType.Leather && _Skinner && _LocationId == WoWPoint.Empty,
                                            new Sequence(
                                                new DecoratorContinue(context => (!Navigator.CanNavigateFully(Me.Location, _SHMNext.Location)),
                                                    new Sequence(
                                                        new Action(context => _ObjectBlacklist.Add(_SHMNext, TimeSpan.FromSeconds(300))),
                                                        new Action(context => _LocationId = WoWPoint.Empty)
                                                    )
                                                ),
                                                new DecoratorContinue(context => (_SHMToFind != _SHMNext),
                                                    new Sequence(
                                                        new Action(context => OGlog("Moveing to Skin {0}", _SHMNext.Name)),
                                                        new Action(context => _SHMToFind = _SHMNext),
                                                        new Action(context => _Interactway = 3),
                                                        new Action(context => WoWMovement.MoveStop()),
                                                        new Action(context => _CheckPointTimer.Restart()),
                                                        new Action(context => _LocationId = WoWMovement.CalculatePointFrom(_SHMNext.Location, 3))
                                                    )
                                                )
                                            )
                                        )
                                        #endregion
                                    )
                                ),
                                #endregion

                                #region Search for NPC
                                new DecoratorContinue(context => _LocationId == WoWPoint.Empty && _NPCNext != null,
                                    new Sequence(
                                        new DecoratorContinue(context => ((!Navigator.CanNavigateFully(Me.Location, _NPCNext.Location)) && (!_NPCNext.InLineOfSight)),
                                            new Sequence(
                                                new DecoratorContinue(context => _NPCToFind != _NPCNext,
                                                    new Sequence(
                                                        new Action(context => _ObjectBlacklist.Add(_NPCNext, TimeSpan.FromSeconds(300))),
                                                        new Action(context => _NPCToFind = _NPCNext)
                                                    )
                                                ),
                                                new Action(context => _LocationId = WoWPoint.Empty)
                                            )
                                        ),
                                        new DecoratorContinue(context => _NPCToFind != _NPCNext,
                                            new Sequence(
                                                new Action(context => OGlog("Moving to {0}, to interact with {1}.", _NPCNext.Location, _NPCNext.Name)),
                                                new Action(context => _NPCToFind = _NPCNext),
                                                new Action(context => _Interactway = 1),
                                                new Action(context => WoWMovement.MoveStop()),
                                                new Action(context => _CheckPointTimer.Restart()),
                                                new Action(context => _LocationId = WoWMovement.CalculatePointFrom(_NPCNext.Location, 3))
                                            )
                                        )
                                    )
                                ),
                                #endregion

                                #region Search for Object
                                new DecoratorContinue(context => _ObjNext != null && _ObjNext.Entry == 214388 && !AncientGuoLaiCacheKey(),
                                    new Action(context => _LocationId = WoWPoint.Empty)
                                ),
                                new DecoratorContinue(context => _LocationId == WoWPoint.Empty && _ObjNext != null,
                                    new Sequence(
                                        new DecoratorContinue(context => !Navigator.CanNavigateFully(Me.Location, _ObjNext.Location) && !_ObjNext.InLineOfSight,
                                            new Sequence(
                                                new DecoratorContinue(context => _ObjectToFind != _ObjNext,
                                                    new Sequence(
                                                        new Action(context => OGlog("Blacklisting Object : " + _ObjNext)),
                                                        new Action(context => _ObjectBlacklist.Add(_ObjNext, TimeSpan.FromSeconds(300))),
                                                        new Action(context => _ObjectToFind = _ObjNext)
                                                    )
                                                ),
                                                new Action(context => _LocationId = WoWPoint.Empty)
                                            )
                                        ),
                                        new DecoratorContinue(context => _ObjectToFind != _ObjNext,
                                            new Sequence(
                                                new Action(context => OGlog("Moving to {0}, to pickup {1}.", _ObjNext.Location, _ObjNext.Name)),
                                                new Action(context => _ObjectToFind = _ObjNext),
                                                new Action(context => _Interactway = 2),
                                                new Action(context => WoWMovement.MoveStop()),
                                                new Action(context => _CheckPointTimer.Restart()),
                                                new Action(context => _LocationId = WoWMovement.CalculatePointFrom(_ObjNext.Location, 3))
                                            )
                                        )
                                    )
                                )
                                #endregion
                            )
                            #endregion
                        ),
                        new Decorator(context => _LocationId != WoWPoint.Empty,
                            new Sequence(
                                new DecoratorContinue(context => _CheckPointTimer.ElapsedMilliseconds < 30000,
                                    new Sequence(
                                        new DecoratorContinue(context => _LocationId.Distance(Me.Location) > 3,
                                            #region MoveToObject
                                            new Sequence(
                                                new DecoratorContinue(context => !Me.IsMoving,
                                                    new Sequence(
                                                        new DecoratorContinue(context => _Interactway == 1,
                                                            new Sequence(
                                                                new DecoratorContinue(context => _NPCToFind.InLineOfSight,
                                                                    new Action(context => Flightor.MoveTo(_LocationId))
                                                                ),
                                                                new DecoratorContinue(context => Navigator.CanNavigateFully(Me.Location, _LocationId),
                                                                    new Action(context => Navigator.MoveTo(_LocationId))
                                                                )
                                                            )
                                                        ),
                                                        new DecoratorContinue(context => _Interactway == 2,
                                                            new Sequence(
                                                                new DecoratorContinue(context => _ObjectToFind.InLineOfSight,
                                                                    new Action(context => Flightor.MoveTo(_LocationId))
                                                                ),
                                                                new DecoratorContinue(context => (Navigator.CanNavigateFully(Me.Location, _LocationId)),
                                                                    new Action(context => Navigator.MoveTo(_LocationId))
                                                                )
                                                            )
                                                        ),
                                                        new DecoratorContinue(context => _Interactway == 3,
                                                            new Sequence(
                                                                new DecoratorContinue(context => _SHMToFind.InLineOfSight,
                                                                    new Action(context => Flightor.MoveTo(_LocationId))
                                                                ),
                                                                new DecoratorContinue(context => (Navigator.CanNavigateFully(Me.Location, _LocationId)),
                                                                    new Action(context => Navigator.MoveTo(_LocationId))
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                            #endregion
                                        ),
                                        new DecoratorContinue(context => _LocationId.Distance(Me.Location) <= 3,
                                            #region InteractWithObject
                                            new Sequence(
                                                new DecoratorContinue(context => _LocationId != WoWPoint.Empty,
                                                    new Sequence(
                                                        // If we are Mounted (and not in flightform) we need to Dismount.
                                                        new DecoratorContinue(context => (!Me.HasAura(40120) && !Me.HasAura(33943) && Flightor.MountHelper.Mounted),
                                                            new Sequence(
                                                                new Action(context => WoWMovement.MoveStop()),
                                                                new Action(context => Flightor.MountHelper.Dismount())
                                                            )
                                                        ),
                                                        new Action(context => WoWMovement.MoveStop()),
                                                        new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed()),
                                                        // Interact with NPC
                                                        new DecoratorContinue(context => _Interactway == 1,
                                                            new Sequence(
                                                                new Action(context => _NPCToFind.Interact()),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed()),
                                                                new Action(context => Lua.DoString("SelectGossipOption(1)")),
                                                                new Action(context => _Interactway = 0),
                                                                new Action(context => _LocationId = WoWPoint.Empty)
                                                            )
                                                        ),
                                                        // Interact with Object
                                                        new DecoratorContinue(context => _Interactway == 2,
                                                            new Sequence(
                                                                new Action(context => WoWMovement.MoveStop()),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed()),
                                                                new DecoratorContinue(context => !_ObjectToFind.InUse,
                                                                    new Sequence(
                                                                        new Action(context => _ObjectToFind.Interact()),
                                                                        new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed())
                                                                    )
                                                                ),
                                                                new Action(context => Lua.DoString("LootSlot(1)")),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(200), context => false, new ActionAlwaysSucceed()),
                                                                new Action(context => Lua.DoString("ConfirmLootSlot(1)")),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed()),
                                                                new Action(context => Styx.CommonBot.Frames.LootFrame.Instance.Close()),
                                                                new Action(context => _Interactway = 0),
                                                                new Action(context => _LocationId = WoWPoint.Empty)
                                                            )
                                                        ),
                                                        // Herb/Mine/Skin Corpses
                                                        new DecoratorContinue(context => _Interactway == 3,
                                                            new Sequence(
                                                                new Action(context => WoWMovement.MoveStop()),
                                                                new Action(context => _SHMToFind.Interact()),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed()),
                                                                new Action(context => Lua.DoString("LootSlot(1)")),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(500), context => false, new ActionAlwaysSucceed()),
                                                                new Action(context => Styx.CommonBot.Frames.LootFrame.Instance.Close()),
                                                                new Action(context => _Interactway = 0),
                                                                new Action(context => _LocationId = WoWPoint.Empty)
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                            #endregion
                                        )
                                    )
                                ),
                                new DecoratorContinue(context => _CheckPointTimer.ElapsedMilliseconds > 30000,
                                    new Sequence(
                                        new Action(context => _LocationId = WoWPoint.Empty)
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }
        #endregion

        #region Pulse
        public override void Pulse() {
            if (_Initialized && _BehaviorTreeHookObjectGatherer == null) {
                _BehaviorTreeHookObjectGatherer = CreateBehaviorObjectGatherer();
                TreeHooks.Instance.AddHook("Loot_Main", _BehaviorTreeHookObjectGatherer);
            }
        }
        #endregion

        #region Ancient Guo-Lai Cache Key
        private bool AncientGuoLaiCacheKey() { return (Me.BagItems.FirstOrDefault(i => i.Entry == 87779) != null); }
        #endregion

        #region LogMarker
        // Use this in PrioritySelector containers...
        public static Composite LogMarkerPs(ProvideStringDelegate messageDelegate) {
            return new Action(context => {
                Logging.Write(Colors.Fuchsia, messageDelegate(context));
                return RunStatus.Failure;
            });
        }

        // Use this in Sequence containers...
        public static Composite LogMarkerSeq(ProvideStringDelegate messageDelegate) {
            return new Action(context => {
                Logging.Write(Colors.Fuchsia, messageDelegate(context));
            });
        }
        #endregion

        #region BotEvent_OnBotStart
        private void BotEvent_OnBotStarted(EventArgs args) { Initialize(); }
        #endregion

        #region BotEvent_OnBotStop
        private void BotEvent_OnBotStopped(EventArgs args) { Dispose(); }
        #endregion

        #region CanSaflyLootCheck
        public bool CanSaflyLootCheck() {
            if (Me.Combat || Me.IsActuallyInCombat) {
                _CheckPointTimer.Restart();
                return false;
            }
            if (ObjectManager.GetObjectsOfType<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro)) {
                _CheckPointTimer.Restart();
                return false;
            }
            if (Me.IsOnTransport || Me.IsGhost || Me.IsDead) {
                _LocationId = WoWPoint.Empty;
                return false;
            }
            return true;
        }
        #endregion

        #region FilterList

        #region Ancient Guo-Lai Cache
        public static readonly uint[] FilterAglc = {
            214388, // Ancient Guo-Lai Cache
        };
        #endregion

        #region Dark Soil
        public static readonly uint[] FilterDs = {
            210565, // Dark Soil
        };
        #endregion

        #region Gold Coins
        public static readonly uint[] FilterGc = {
            186633, // Gold Coins
            186634, // Gold Coins
            210894, // Gold Coins
            214458, // Gold Coins
            214985, // Gold Coins
        };
        #endregion

        #region Is Another Man's Treasure
        public static readonly uint[] FilterIamt = {
            213363, // Wodin's Mantid Shanker
            213364, // Ancient Pandaren Mining Pick
            213366, // Ancient Pandaren Tea Pot (Grey trash worth 100G)
            213368, // Lucky Pandaren Coin (Grey trash worth 95G)
            213649, // Cache of Pilfered Goods
            213653, // Pandaren Fishing Spear
            213741, // Ancient Jinyu Staff
            213742, // Hammer of Ten Thunders
            213748, // Pandaren Ritual Stone (Grey trash worth 105G)
            213749, // Staff of the Hidden Master
            213750, // Saurok Stone Tablet (Grey trash worth 100G)
            213751, // Sprite's Cloth Chest
            213765, // Tablet of Ren Yun (Cooking Recipy)
            213768, // Hozen Warrior Spear
            213771, // Statue of Xuen (Grey trash worth 100G)
            213782, // Terracotta Head (Grey trash worth 100G)
            213793, // Riktik's Tiny Chest (Grey trash worth 105G)
            213842, // Stash of Yaungol Weapons
            213844, // Amber Encased Moth (Grey trash worth 105G)
            213845, // The Hammer of Folly (Grey trash worth 100G)
            213956, // Fragment of Dread (Grey trash worth 90G)
            213959, // Hardened Sap of Kri'vess (Grey trash worth 110G)
            213960, // Yaungol Fire Carrier
            213962, // Wind-Reaver's Dagger of Quick Strikes
            213964, // Malik's Stalwart Spear
            213966, // Amber Encased Necklace
            213967, // Blade of the Prime
            213968, // Swarming Cleaver of Ka'roz
            213969, // Dissector's Staff of Mutilation
            213970, // Bloodsoaked Chitin Fragment
            213972, // Blade of the Poisoned Mind
            214340, // Boat-Building Instructions (Grey trash worth 10G)
            214438, // Ancient Mogu Tablet (Grey trash worth 95G)
            214439, // Barrel of Banana Infused Rum (Cooking Recipy and Rum)
        };
        #endregion

        #region Is Another Man's Treasure NPC
        public static readonly uint[] FilterIamtn = {
            65552, // Glinting Rapana Whelk
            64272, // Jade Warrior Statue
            64004, // Ghostly Pandaren Fisherman
            64191, // Ghostly Pandaren Craftsman
            64227, // Frozen Trail Packer
        };
        #endregion

        #region Netherwing Egg
        public static readonly uint[] FilterNe = {
            185915, // Netherwing Egg
        };
        #endregion

        #region Onyx Egg
        public static readonly uint[] FilterOe = {
            214945, // Onyx Egg
        };
        #endregion

        #region Treasure Chests
        public static readonly uint[] FilterTc = {
            176944, // Old Treasure Chest (Scholomance Instance)
            179697, // Arena Treasure Chest (STV Arena)
            203090, // Sunken Treaure Chest
            207472, // Silverbound Treasure Chest (Zone 1)
            207473, // Silverbound Treasure Chest (Zone 2)
            207474, // Silverbound Treasure Chest (Zone 3)
            207475, // Silverbound Treasure Chest (Zone 4)
            207476, // Silverbound Treasure Chest (Zone 5)
            207477, // Silverbound Treasure Chest (Zone 6)
            207478, // Silverbound Treasure Chest (Zone 7)
            207479, // Silverbound Treasure Chest (Zone 8)
            207480, // Silverbound Treasure Chest (Zone 9)
            207484, // Sturdy Treasure Chest (Zone 1)
            207485, // Sturdy Treasure Chest (Zone 2)
            207486, // Sturdy Treasure Chest (Zone 3)
            207487, // Sturdy Treasure Chest (Zone 4)
            207488, // Sturdy Treasure Chest (Zone 5)
            207489, // Sturdy Treasure Chest (Zone 6)
            207492, // Sturdy Treasure Chest (Zone 7)
            207493, // Sturdy Treasure Chest (Zone 8)
            207494, // Sturdy Treasure Chest (Zone 9)
            207495, // Sturdy Treasure Chest (Zone 10)
            207496, // Dark Iron Treasure Chest (Zone 1)
            207497, // Dark Iron Treasure Chest (Zone 2)
            207498, // Dark Iron Treasure Chest (Zone 3)
            207500, // Dark Iron Treasure Chest (Zone 4)
            207507, // Dark Iron Treasure Chest (Zone 5)
            207512, // Silken Treasure Chest (Zone 1)
            207513, // Silken Treasure Chest (Zone 2)
            207517, // Silken Treasure Chest (Zone 3)
            207518, // Silken Treasure Chest (Zone 4)
            207519, // Silken Treasure Chest (Zone 5)
            207520, // Maplewood Treasure Chest (Zone 1)
            207521, // Maplewood Treasure Chest (Zone 2)
            207522, // Maplewood Treasure Chest (Zone 3)
            207523, // Maplewood Treasure Chest (Zone 4)
            207524, // Maplewood Treasure Chest (Zone 5)
            207528, // Maplewood Treasure Chest (Zone 6)
            207529, // Maplewood Treasure Chest (Zone 7)
            207533, // Runestone Treasure Chest (Zone 1)
            207534, // Runestone Treasure Chest (Zone 2)
            207535, // Runestone Treasure Chest (Zone 3)
            207540, // Runestone Treasure Chest (Zone 4)
            207542, // Runestone Treasure Chest (Zone 5)
            213362, // Ship's Locker (Contains ~ 96G)
            213650, // Virmen Treasure Cache (Contains ~ 100G)
            213769, // Hozen Treasure Cache (Contains ~ 100G)
            213770, // Stolen Sprite Treasure (Contains ~ 105G)
            213774, // Lost Adventurer's Belongings (Contains ~ 100G)
            213961, // Abandoned Crate of Goods (Contains ~ 100G)
            214325, // Forgotten Lockbox (Contains ~ 10G)
            214407, // Mo-Mo's Treasure Chest (Contains ~ 9G)
            214337, // Stash of Gems (few green uncut MoP gems and ~ 7G)
            214337, // Offering of Rememberance (Contains ~ 30G and debuff turns you grey)
            218593, // Trove of the Thunder King
        };
        #endregion

        #region Quests
        public static readonly uint[] FilterQ = {
            #region Kor'kron items
            97530, // Kor'kron Lumber
            97543, // Kor'kron Stone
            97544, // Kor'kron Oil
            97545, // Kor'kron Meat
            #endregion

            #region Paying Tribute (Niuzao Food Supply)
            212131, // Niuzao Food Supply
            212132, // Niuzao Food Supply
            212133, // Niuzao Food Supply
            #endregion

            #region Mysterious Camel Figurine
            50409, //Mysterious Camel Figurine
            50410, //Mysterious Camel Figurine
            #endregion
        };
        #endregion

        #region UpdateFilterList()
        public static uint[] UpdateFilterList() {
            var tmpList = new List<uint>();
            if (ObjectGatherer_Settings.Instance.AglcCb) { tmpList.AddRange(FilterAglc); }
            if (ObjectGatherer_Settings.Instance.DsCb) { tmpList.AddRange(FilterDs); }
            if (ObjectGatherer_Settings.Instance.GcCb) { tmpList.AddRange(FilterGc); }
            if (ObjectGatherer_Settings.Instance.IamtCb) { tmpList.AddRange(FilterIamt); }
            if (ObjectGatherer_Settings.Instance.IamtnCb) { tmpList.AddRange(FilterIamtn); }
            if (ObjectGatherer_Settings.Instance.NeCb) { tmpList.AddRange(FilterNe); }
            if (ObjectGatherer_Settings.Instance.OeCb) { tmpList.AddRange(FilterOe); }
            if (ObjectGatherer_Settings.Instance.TcCb) { tmpList.AddRange(FilterTc); }
            if (ObjectGatherer_Settings.Instance.QCb) { tmpList.AddRange(FilterQ); }
            return (tmpList.ToArray());
        }
        #endregion

        #endregion
    }
    #region LocalBlacklist
    //
    // This class is coded by Chinajade. I've just made some small modifications to suit my need.
    //
    public class LocalBlacklist {
        public LocalBlacklist(TimeSpan maxSweepTime) { _SweepTimer = new WaitTimer(maxSweepTime) { WaitTime = maxSweepTime }; }

        private readonly Dictionary<ulong, DateTime> _BlackList = new Dictionary<ulong, DateTime>();
        private readonly WaitTimer _SweepTimer;

        public void Add(ulong guid, TimeSpan timeSpan) {
            RemoveExpired();
            _BlackList[guid] = DateTime.Now.Add(timeSpan);
        }

        public void Add(WoWObject wowObject, TimeSpan timeSpan) { if (wowObject != null) { Add(wowObject.Guid, timeSpan); } }

        public bool Contains(ulong guid) {
            DateTime expiry;
            if (_BlackList.TryGetValue(guid, out expiry)) { return (expiry > DateTime.Now); }
            return false;
        }

        public bool Contains(WoWObject wowObject) { return (wowObject != null) && Contains(wowObject.Guid); }

        public void RemoveExpired() {
            if (_SweepTimer.IsFinished) {
                DateTime now = DateTime.Now;
                List<ulong> expiredEntries = (from key in _BlackList.Keys
                                              where (_BlackList[key] < now)
                                              select key).ToList();
                foreach (var entry in expiredEntries) { _BlackList.Remove(entry); }
                _SweepTimer.Reset();
            }
        }
    }
    #endregion
}
