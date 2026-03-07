using FuryUnleashed.Core;
using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Managers;
using FuryUnleashed.Core.Utilities;
using FuryUnleashed.Interfaces.GUI;
using JetBrains.Annotations;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System;
using A = FuryUnleashed.Rotations.Arms.ArmsGlobal;
using BotEvents = Styx.CommonBot.BotEvents;
using F = FuryUnleashed.Rotations.Fury.FuryGlobal;
using P = FuryUnleashed.Rotations.Protection.ProtGlobal;

// HB API Documentation: http://docs.honorbuddy.com/

namespace FuryUnleashed
{
    public class Root : CombatRoutine
    {
        [UsedImplicitly]
        public static Root Instance { get; private set; }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly Version Revision = new Version(1, 5, 6, 8);
        public static readonly string FuName = "Fury Unleashed - IR " + Revision;
        public static readonly string WoWVersion = "5.4.7";

        public override string Name { get { return FuName; } }
        public override bool WantButton { get { return true; } }
        
        public override Composite CombatBehavior { get { return _combatBehavior ?? (_combatBehavior = CombatSelector()); } }
        public override Composite PreCombatBuffBehavior { get { return _preCombatBehavior ?? (_preCombatBehavior = PreCombatSelector()); } }

        private Composite _combatBehavior, _preCombatBehavior;

        internal static ulong MyToonGuid = 0;

        #region Publics
        public override WoWClass Class
        {
            get { return Me.Class == WoWClass.Warrior ? WoWClass.Warrior : WoWClass.None; }
        }

        public Root()
        {
            Instance = this;
            BotEvents.OnBotStopped += Core.Helpers.BotEvents.OnBotStopped;
            BotEvents.OnBotStarted += Core.Helpers.BotEvents.OnBotStarted;
            BotEvents.OnBotChanged += Core.Helpers.BotEvents.OnBotChanged;
        }

        public override void Initialize()
        {
            try
            {
                TreeHooks.Instance.ClearAll();
                Updater.CheckForUpdate();

                Unleash();
            }
            catch (Exception exception)
            {
                Logger.DiagLogPu("FU: Exception thrown at Initialize - {0}", new object[] { exception.ToString() });
            }
        }

        public override void Pulse()
        {
			if (!StyxWoW.IsInWorld || Me == null || !Me.IsValid || Me.IsDead)
            {
                return;
            }

            if (TalentManager.Pulse())
			{
                // ReSharper disable once RedundantJumpStatement
				return;
			}

            DamageTracker.Pulse();   
        }

        public override void OnButtonPress()
        {
            new Interface().ShowDialog();
        }

        public override void ShutDown()
        {
            StopBot("FU: Shutting down HonorBuddy and all attached addons.");
        }
        #endregion

        #region Internals
        internal void Unleash()
        {
            Logger.CombatLogWh("------------------------------------------");
            Logger.CombatLogFb(Name + " by nomnomnom & alxaw.");
            Logger.CombatLogOr("Internal Revision: " + Revision + ".");
            Logger.CombatLogOr("Supported World of Warcraft version: " + WoWVersion + ".");
            Logger.CombatLogOr("Support will be handled via the HB Forums.");
            Logger.CombatLogOr("Thanks list is available in the topic!");
            Logger.CombatLogOr("Special thanks to: Stormchasing, Wulf, Mirabis, Alxaw, Weischbier & Millz!\r\n");
            Logger.CombatLogOr("Your specialization is " + Me.Specialization.ToString().CamelToSpaced() + " and your race is " + Me.Race + ".\r\n");
            Logger.CombatLogFb("Recommended rotations are (Selectable in the GUI):");
            Logger.CombatLogOr("Arms: Release");
            Logger.CombatLogOr("Fury: Release");
            Logger.CombatLogOr("Protection: Release");
            Logger.CombatLogWh("-------------------------------------------\r\n");

            /* Set Characters GUID */
            MyToonGuid = Me.Guid;

            /* Update TalentManager */
            try { TalentManager.Update(); }
            catch (Exception e) { StopBot(e.ToString()); }

            /* Initialize Various Functions */
            DamageTracker.Initialize();
            HotKeyManager.InitializeBindings();
            Item.RefreshSecondaryStats();

            /* Gather required information */
            Logger.StatCounter();
            Logger.LogTimer(500);

            /* Start Combat */
            Spell.InitGcdSpell();
            PreCombatSelector();
            CombatSelector();

            Logger.CombatLogOr("Routine initialized with " + Me.Specialization.ToString().CamelToSpaced() + " as rotation. \r\n");
        }

        internal Composite PreCombatSelector()
        {
            return new Switch<WoWSpec>(ret => Me.Specialization,
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorArms, A.InitializeArmsPreCombat),
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorFury, F.InitializeFuryPreCombat),
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorProtection, P.InitializeProtPreCombat));
        }

        internal Composite CombatSelector()
        {
            return new Switch<WoWSpec>(ret => Me.Specialization,
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorArms, A.InitializeArmsCombat),
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorFury, F.InitializeFuryCombat),
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorProtection, P.InitializeProtCombat));
        }

        internal static void StopBot(string reason)
        {
            DamageTracker.Stop();
            TreeRoot.Stop();
            Logger.CombatLogWh(reason);
        }
        #endregion

        #region LockSelector - FrameLock - AcquireFrame
        [UsedImplicitly]
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children)
                : base(children)
            {
            }
            public override RunStatus Tick(object context)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    return base.Tick(context);
                }
            }
        }
        #endregion
    }
}
