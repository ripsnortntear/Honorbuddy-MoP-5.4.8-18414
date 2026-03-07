#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-22 17:17:33 +0200 (So, 22 Sep 2013) $
 * $Revision: 1734 $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: Added logging of Settings$
 */

#endregion Revision info

using System;
using System.Windows.Forms;
using PureRotation.Classes.DeathKnight;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.GUI;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

#region Changelog

/*
DEATHKNIGHT
--------------
FROST [X]
BLOOD [X]
UNHOLY [X]
DRUID
--------------
BALANCE [X]
FERAL [X]
GAURDIAN [ ]
RESTORATION [X]
HUNTER
--------------
MARKSMAN [ ]
BEASTMASTERY [X]
SURVIVAL [ ]
MAGE
--------------
ARCANE [X]
FIRE [X]
FROST [X]
MONK
--------------
BREWMASTER [X]
WINDWALKER [X]
MISTWEAVER [X]
PALADIN
--------------
HOLY [X]
PROTECTION [X]
RETRIBUTION [X]
PRIEST
--------------
DISCIPLINE [X]
HOLY [X]
SHADOW [X]
ROGUE
--------------
COMBAT [X]
ASSASINATION [X]
SUBTLETY [X]
SHAMAN
--------------
ELEMENTAL [ ]
RESTORATION [X]
ENHANCE [X]
WARLOCK
--------------
DESTRUCTION [X]
AFFLICTION [X]
DEMONOLOGY [X]
WARRIOR
--------------
ARMS [ ]
PROTECTION [X]
FURY [X]
*/

#endregion Changelog

namespace PureRotation
{
    public partial class PureRotationRoutine : CombatRoutine
    {
        internal static readonly Version Version = new Version(2, 1, 7);
        internal static readonly string _name = "PureRotation " + Version;

        public override string Name { get { return _name; } }

        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static PureRotationRoutine Instance { get; private set; }

        private Form _newtempui;

        internal delegate void OnPulseHandler();

        /// <summary>
        /// Occurs when Pulse is called by the bot.
        /// </summary>
        internal event OnPulseHandler PulseEvent;
        public PureRotationRoutine()
        {
            Instance = this;

            // Do this now, so we ensure we update our context when needed.
            BotEvents.Player.OnMapChanged += e =>
                {
                    // Don't run this handler if we're not the current routine!
                    if (RoutineManager.Current.Name != Name)
                        return;

                    // Only ever update the context. All our internal handlers will use the context changed event
                    // so we're not reliant on anything outside of ourselves for updates.
                    UpdateContext();
                };
            BotEvents.OnBotStopped += HotKeyManager.BotEvents_OnBotStopped;
            BotEvents.OnBotStarted += HotKeyManager.BotEvents_OnBotStarted;
        }

        public override bool WantButton
        {
            get { return true; }
        }

        public override void OnButtonPress()
        {
            if (_newtempui == null || _newtempui.IsDisposed || _newtempui.Disposing) _newtempui = new ConfigurationForm();
            if (_newtempui != null || _newtempui.IsDisposed) _newtempui.ShowDialog();
        }
        private static ulong _lastCheckCurrTargetGuid = 0;
        private static ulong _lastCheckPetsTargetGuid = 0;

        private void CheckCurrentTarget()
        {
            CheckTarget(Me.CurrentTarget, ref _lastCheckCurrTargetGuid, "YourCurrentTarget");
            if (Me.GotAlivePet) CheckTarget(Me.Pet.CurrentTarget, ref _lastCheckPetsTargetGuid, "PetsCurrentTarget");
        }

        private void CheckTarget(WoWUnit unit, ref ulong prevGuid, string description)
        {
            // there are moments where CurrentTargetGuid != 0 but CurrentTarget == null. following
            // .. tries to handle by only checking CurrentTarget reference and treating null as guid = 0
            if (unit == null)
            {
                if (prevGuid != 0)
                {
                    prevGuid = 0;
                    Logger.DebugLog(description + ": changed to: (null)");
                    HandleTrainingDummy(unit);
                }
            }
            else if (unit.Guid != prevGuid)
            {
                prevGuid = unit.Guid;

                HandleTrainingDummy(unit);

                string info = "";
                if (Styx.CommonBot.POI.BotPoi.Current.Guid == Me.CurrentTargetGuid)
                    info += string.Format(", IsBotPoi={0}", Styx.CommonBot.POI.BotPoi.Current.Type);

                if (Styx.CommonBot.Targeting.Instance.TargetList.Contains(Me.CurrentTarget))
                    info += string.Format(", TargetIndex={0}", Styx.CommonBot.Targeting.Instance.TargetList.IndexOf(Me.CurrentTarget) + 1);

                string playerInfo = "N";
                if (unit.IsPlayer)
                {
                    WoWPlayer p = unit.ToPlayer();
                    playerInfo = string.Format("Y, Friend={0}, IsPvp={1}, CtstPvp={2}, FfaPvp={3}", Me.IsHorde == p.IsHorde, p.IsPvPFlagged, p.ContestedPvPFlagged, p.IsFFAPvPFlagged);
                }

                Logger.DebugLog(description + ": changed to: {0} h={1:F1}%, maxh={2}, d={3:F1} yds, box={4:F1}, player={5}, attackable={6}, hostile={7}, entry={8}, faction={9}, loss={10}, facing={11}" + info,
                    unit.SafeName,
                    unit.HealthPercent,
                    unit.MaxHealth,
                    unit.Distance,
                    unit.CombatReach,
                    playerInfo,
                    unit.Attackable,
                    unit.IsHostile,
                    unit.Entry,
                    unit.FactionId,
                    unit.InLineOfSpellSight,
                    Me.IsSafelyFacing(unit)
                    );
            }
        }

        private static void HandleTrainingDummy(WoWUnit unit)
        {
            bool trainingDummy = unit == null ? false : unit.IsTrainingDummy();

            if (trainingDummy && CurrentWoWContext == WoWContext.None)
            {
                  Logger.DebugLog("Detected Training Dummy -- forcing {0} behaviors", CurrentWoWContext.ToString());
            }
            else if (!trainingDummy && CurrentWoWContext != WoWContext.None)
            {
                Logger.DebugLog("Detected Training Dummy no longer target -- reverting to {0} behaviors", CurrentWoWContext.ToString());
            }
        }

        public override void Pulse()
        {
            try
            {
                // Double cast shit
                Spell.PulseDoubleCastEntries();

                // Calls any logic from the class rotations that need to be pulsed.
                OnPulseHandler handler = PulseEvent;

                if (handler != null)
                    handler();

                if (StyxWoW.Me.Specialization == WoWSpec.DeathKnightBlood)
                    DeathStrikeTracker.Pulse();
                // Output if Target changed 
                CheckCurrentTarget();
                //Spell.OutputDoubleCastDict();

                //// No pulsing if we're loading or out of the game.
                //if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                //    return;

                //// Update the current context, check if we need to rebuild any behaviors.
                UpdateContext();
                //Logger.LogStatistics();
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown in Pulse(): {0}", e);
            }
        }

        public override void Initialize()
        {
            Spell.mobcount = 0; // For Multi Dotting..didnt know where else to initiate it aahha. -wulf.

            //Log Settings
            PRSettings.Instance.LogSettings();
            Logger.InfoLog("Thank you for choosing {0} as your Combat Routine.", Name);
            Logger.InfoLog("Current BotBase is {0}", BotManager.Current.Name);
            Logger.InfoLog("Character level: {0}", Me.Level);
            Logger.InfoLog("Character Faction: {0}", Me.IsAlliance ? "Alliance" : "Horde");
            Logger.InfoLog("Character Race: {0}", Me.Race);
            Logger.InfoLog("Character Mapname: {0}", Me.MapName);
            Logger.InfoLog("GroupType: {0}", GroupType.ToString());
            Logger.InfoLog("LocationContext: {0}", CurrentWoWContext.ToString());
            Logger.InfoLog("Retrieving talent spec.");
            try
            {
                TalentManager.Update();
            }
            catch (Exception e)
            {
                StopBot(e.ToString());
            }

            Logger.InfoLog("Storing class for racial usage.");
            try
            {
                Racials.CurrentRace = StyxWoW.Me.Race;
            }
            catch (Exception e)
            {
                StopBot(e.ToString());
            }

            Logger.InfoLog("Current spec is " + TalentManager.CurrentSpec);

            Lua.PopulateSecondryStats();

            // Update the current WoWContext, and fire an event for the change.
            UpdateContext();

            // NOTE: Hook these events AFTER the context update.
            OnWoWContextChanged += (orig, ne) =>
            {
                Logger.InfoLog(" Context changed, re-creating behaviors");
                RebuildBehaviors();
                BossList.Init();
            };
            RoutineManager.Reloaded += (s, e) =>
            {
                Logger.InfoLog(" Routines were reloaded, re-creating behaviors");
                RebuildBehaviors();
            };

            // Intialize Behaviors....
            if (_combatBehavior == null)
                _combatBehavior = new PrioritySelector();

            if (_combatBuffBehavior == null)
                _combatBuffBehavior = new PrioritySelector();

            if (_preCombatBuffBehavior == null)
                _preCombatBuffBehavior = new PrioritySelector();

            // Behaviors
            if (!RebuildBehaviors())
            {
                return;
            }

            Logger.InfoLog(" Behaviors created!");
            Logger.InfoLog(" {0}", Me.IsInInstance ? "Character is currently in an Instance" : "Character seems to be outside an Instance");
            Logger.InfoLog(" {0}", StyxWoW.Me.CurrentMap.IsArena ? "Character is currently in an Arena" : "Character seems to be outside an Arena");
            Logger.InfoLog(" {0}", StyxWoW.Me.CurrentMap.IsBattleground ? "Character is currently in a Battleground  " : "Character seems to be outside a Battleground");
            Logger.InfoLog(" {0}", StyxWoW.Me.CurrentMap.IsDungeon ? "Character is currently in a Dungeon  " : "Character seems to be outside a Dungeon");
            Logger.InfoLog("Character HB Pull Range: {0}", Targeting.PullDistance);
            Logger.InitLog("Class SVN Revision: {0}", _currentRotation.Revision);

            //This stuff is Feral Druid only. We need to parse the Combat Log :X
            if (StyxWoW.Me.Specialization == WoWSpec.DruidFeral)
                Classes.Druid.Common.Initialize();

            //This stuff is Blood Death Knight only. We need to parse the Combat Log for accurate death strikes
            if (StyxWoW.Me.Specialization == WoWSpec.DeathKnightBlood)
                DeathStrikeTracker.Initialize();

            //need this for Arms PVP ownage.
            if (StyxWoW.Me.Specialization == WoWSpec.WarriorArms)
                Classes.Warrior.Common.Initialize();

            //Cached units to lesson HB checks.
            CachedUnits.Initialize();

            Logger.InfoLog(" Initialization Complete");
            Logger.InfoLog(" {0}", _currentRotation.Help);

            
        }

        private static void StopBot(string reason)
        {
            Logger.FailLog(reason);
            TreeRoot.Stop();
        }
    }
}