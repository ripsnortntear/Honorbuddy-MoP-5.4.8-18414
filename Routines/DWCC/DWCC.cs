using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;


using Styx;
using Styx.TreeSharp;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Common;
using Styx.Resources;
using Styx.Loaders;
using Styx.Common.Helpers;
using Styx.Pathing;
using Styx.XmlEngine;
using Styx.CommonBot.POI;
using Styx.WoWInternals.World;
using CommonBehaviors.Actions;

using Action = Styx.TreeSharp.Action;


namespace DWCC
{
    public partial class Warrior : CombatRoutine
    {
        public override sealed string Name { get { return "Dunatank's Warrior CC v" + ver + "  " + Tag; } }
        public override WoWClass Class { get { return WoWClass.Warrior; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static Version ver { get { return new Version(5, 7); } }
        public static string Tag { get { return "PvP+"; } }

        #region Initialize
        public override void Initialize()
        {
            Logging.Write("[DWCC]: Dunatank's Warrior CC v" + ver + " BT will do the job for you!");
            Logging.Write("[DWCC]: created by Wownerds Dev Team!");
            CharacterSettings.Instance.PullDistance = 24;
            Logging.WriteDiagnostic("[DWCC]: ###Character Information###");
            Logging.WriteDiagnostic("[DWCC]: Level: " + Me.Level);
            Logging.WriteDiagnostic("[DWCC]: Alliance/Horde: " + Me.IsAlliance + " || " + Me.IsHorde);
            Logging.WriteDiagnostic("[DWCC]: Health: " + Me.MaxHealth);
            Logging.WriteDiagnostic("[DWCC]: Movement Disabled: " + DunatanksSettings.Instance.DisableMovement);
            Logging.WriteDiagnostic("[DWCC]: Pull: " + !DunatanksSettings.Instance.DisableMovement);
            Logging.WriteDiagnostic("[DWCC]: CC Specc: " + DunatanksSettings.Instance.useArms + " || " + DunatanksSettings.Instance.useFury + " || " + DunatanksSettings.Instance.useProt);
            Logging.WriteDiagnostic("[DWCC]: ###Character Information###");
            if (Me.Specialization == WoWSpec.WarriorArms)
            {
                DunatanksSettings.Instance.useArms = true;
                DunatanksSettings.Instance.useFury = false;
                DunatanksSettings.Instance.useProt = false;
            }
            else if (Me.Specialization == WoWSpec.WarriorFury)
            {
                DunatanksSettings.Instance.useArms = false;
                DunatanksSettings.Instance.useFury = true;
                DunatanksSettings.Instance.useProt = false;
            }
            else if (Me.Specialization == WoWSpec.WarriorProtection)
            {
                DunatanksSettings.Instance.useArms = false;
                DunatanksSettings.Instance.useFury = false;
                DunatanksSettings.Instance.useProt = true;
            }
            Logging.Write("[DWCC]: Spec set.");
            TreeRoot.TicksPerSecond = (byte)30;
            HSTimer.Interval = 3000;
            HSTimer.Elapsed +=  HSTimer_Elapsed;
            HSTimer.Start();
            LastRage[0] = 0;
            LastRage[1] = 0;
            ReadXML();
            Logging.Write("[DWCC]: XML files read.");
            HaveOneHander();
            HaveShield();
            StoreTwoHander();
            Logging.Write("[DWCC]: Weapons found.");
            WriteSettingsToLog();
            Logging.Write("[DWCC]: Initialization succeeded.");
        }
        #endregion

        #region Pulse
        public override void Pulse()
        {
           if (!DunatanksSettings.Instance.pureDPS) {
                if (Me.Mounted && Me.Combat && !DunatanksSettings.Instance.DisableMovement) { Mount.Dismount("Combat"); }
                FearBreak();
                MoveOutOfAoE();
                LifeSpiritRegen();
            }
           Movement.PulseMovement();
           if ((Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid) && !Battlegrounds.IsInsideBattleground && DunatanksSettings.Instance.useAutoTargetProt && DunatanksSettings.Instance.useProt)
           {
               TankManager.Instance.Pulse();
               if (TankManager.Instance.FirstUnit != null) TankManager.Instance.FirstUnit.Target();
           }
           if (Me.CurrentTarget != null)
               if (!Me.CurrentTarget.IsAlive || Me.CurrentTarget.IsFriendly && !DunatanksSettings.Instance.DisableMovement) Me.ClearTarget();
        }
        #endregion

        #region Buttons
        public override bool WantButton
        {
            get
            {
                return true;
            }
        }
        public override void OnButtonPress()
        {
            DWCC.cfg cfg = new DWCC.cfg();
            cfg.ShowDialog();
        }
        #endregion

        #region Rest
        public override bool NeedRest
        {
            get
            {
                if (Me.HealthPercent <= DunatanksSettings.Instance.RestPercent && DunatanksSettings.Instance.UseRest)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public override void Rest()
        {
            if (Me.HealthPercent <= DunatanksSettings.Instance.RestPercent && DunatanksSettings.Instance.UseRest && !Me.IsFlying && !Me.IsSwimming && !Me.Combat && !Me.IsDead && !Me.IsGhost)
            {
                Styx.CommonBot.Rest.Feed();
            }

        }
        #endregion

        #region Combat
        private Composite _combatBehavior;

        public override Composite CombatBehavior
        {
            get { if (_combatBehavior == null) { Logging.Write("[DWCC]: Creating DWCC MoP Combat Behavior."); _combatBehavior = CreateCombatBehavior(); } return _combatBehavior; }
        }

        private Composite CreateCombatBehavior()
        {
            return CreateDWCCBehavior();
        }
        #endregion

        #region Pull
        private Composite _pullBehavior;
        public override Composite PullBehavior
        {
            get
            {
                if (_pullBehavior == null)
                {
                    Logging.Write("[DWCC]: Creating 'Pull' behavior");
                    _pullBehavior = CreatePullBehavior();
                }
                _pullBehavior = CreatePullBehavior();
                return _pullBehavior;
            }
        }
        private PrioritySelector CreatePullBehavior()
        {
            return new PrioritySelector(
                //new Action(a => { Logging.Write("Pull"); return RunStatus.Failure;}),
                new Decorator(ret => Me.Mounted && Me.CurrentTarget.Distance < 5, new Action(a => { Mount.Dismount("Pull"); return RunStatus.Failure; })),
                FaceTarget(),
                CastCharge(),
                HeroicLeap(),
                CreateSpellCheckAndCast("Heroic Throw", ret => true),
                /*new Action(a => { Logging.Write("Hostile: " + Me.CurrentTarget.IsHostile);
                                    Logging.Write("Disable Movement: " + !DunatanksSettings.Instance.DisableMovement);
                                    Logging.Write("Target out of Melee: " + Me.CurrentTarget.Distance + "/" + MeleeDistance(Me.CurrentTarget));
                                    Logging.Write("Last Movement: " + (DateTime.Now - LastMovement));
                                    Logging.Write("Distance from last MovePoint: " + LastMovementPoint.Distance(Me.CurrentTarget.Location));
                                    return RunStatus.Failure; }),*/
                MoveToTarget(),
                CreateAutoAttack());
        }
        #endregion

        #region Resting
        public override Composite RestBehavior
        {
            get
            {
                return new Decorator(ret => !Me.IsDead && !Me.Combat && !Me.IsMoving && !Me.IsGhost && !Me.IsSwimming && Me.HealthPercent < 40,
                    new Action(ret =>
                        {
                            if (!Me.HasAura("Food")) Styx.CommonBot.Rest.FeedImmediate();
                            if (Me.HasAura("Food") && Me.HealthPercent < 90) return RunStatus.Running;
                            else return RunStatus.Success;
                        }));
            } 
        }
        #endregion
    }
}