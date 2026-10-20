using CommonBehaviors.Actions;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Windows.Forms;
using Tyrael.Shared;

namespace Tyrael
{
    public class Tyrael : BotBase
    {
		public static bool IsPaused;
		
        internal static readonly Version Revision = new Version(5, 6, 4);
        
        private static Composite _root;
        private static PulseFlags _pulseFlags;
        private static bool _oldLogoutForInactivity;

        /// <summary>
        /// Returns Me as StyxWoW.Me.
        /// </summary>
        internal static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        #region Overrides
        /// <summary>
        /// Returns the Name of the BotBase.
        /// </summary>
        public override string Name
        {
            get { return "Tyrael"; }
        }

        /// <summary>
        /// Returns the requires PulseFlags for the BotBase.
        /// </summary>
        public override PulseFlags PulseFlags
        {
            get { return !TyraelUtilities.IsTyraelPaused ? _pulseFlags : PulseFlags.Objects | PulseFlags.Lua | PulseFlags.InfoPanel; }
        }

        /// <summary>
        /// Returns the GUI of the BotBase on button press.
        /// </summary>
        public override Form ConfigurationForm
        {
            get { return new TyraelInterface(); }
        }
		
        /// <summary>
        /// Forcefully disable the profile requirements.
        /// </summary>
		public override bool RequiresProfile
        {
            get { return false; }
        }

        /// <summary>
        /// Creating the Root Composite - The magic happens within this composite.
        /// </summary>
        public override Composite Root
        {
            get { return _root ?? (_root = CreateRoot()); }
        }

        /// <summary>
        /// Runs when the BotBase is started - Initializes several basic functions.
        /// </summary>
        public override void Start()
        {
            try
            {
                InitializeComponents();
                InitializePlugins();

                if (!GlobalSettings.Instance.UseFrameLock && !TyraelSettings.Instance.UseSoftLock)
                {
                    TyraelUtilities.PrintLog("------------------------------------------");
                    TyraelUtilities.PrintLog("HardLock and SoftLock are both disabled - For optimal DPS/HPS I suggest enabling ONE of them.");
                }

                TyraelUtilities.PrintLog("------------------------------------------");
                TyraelUtilities.PrintLog("{0} {1} has been started.", Name, Revision);
                TyraelUtilities.PrintLog("{0} is loaded.\r\n", RoutineManager.Current != null ? RoutineManager.Current.Name : "No Routine");
                TyraelUtilities.PrintLog("Special thanks to the following persons:");
                TyraelUtilities.PrintLog("PureRotation Team");
                TyraelUtilities.PrintLog("-------------------------------------------\r\n");
            }
            catch (Exception startexception)
            {
                TyraelUtilities.DiagnosticLog("Start() Error: {0}", startexception);
            }
        }

        /// <summary>
        /// Runs when the BotBase is stopped - Stops several basic functions.
        /// </summary>
        public override void Stop()
        {
            try
            {
                TyraelUtilities.PrintLog("------------------------------------------");
                TyraelUtilities.PrintLog("Shutdown - Performing required actions.");
                StopComponents();
                _root = null;
                TyraelUtilities.PrintLog("-------------------------------------------\r\n");
            }
            catch (Exception stopexception)
            {
                TyraelUtilities.DiagnosticLog("Stop() Error: {0}", stopexception);
            }
        }

        public override void Initialize()
        {
            BotEvents.Player.OnMapChanged += Player_OnMapChanged;
        }

        private void Player_OnMapChanged(BotEvents.Player.MapChangedEventArgs args)
        {
            _root = null;
        }
        #endregion

        #region Privates & Internals
        /// <summary>
        /// SanityCheck - Checks if we are actually ingame and able to control the character.
        /// </summary>
        private static bool SanityCheckCombat()
        {
            try
            {
                return TyraelUtilities.IsViable(Me) && (StyxWoW.Me.Combat || TyraelSettings.Instance.ContinuesHealingMode) && !Me.IsDead;
            }
            catch (Exception sanitycheckexception)
            {
                TyraelUtilities.DiagnosticLog("SanityCheckCombat() Error: {0}", sanitycheckexception); return false;
            }
        }

        /// <summary>
        /// Actual Root Composite - Within this the RoutineManager runs the routines behaviors.
        /// </summary>
        private static Composite CreateRoot()
        {
            System.Collections.Generic.List<Composite> combatBehaviors = new System.Collections.Generic.List<Composite>();
            if (RoutineManager.Current != null)
            {
                if (RoutineManager.Current.HealBehavior != null)
                    combatBehaviors.Add(RoutineManager.Current.HealBehavior);
                if (RoutineManager.Current.CombatBuffBehavior != null)
                    combatBehaviors.Add(RoutineManager.Current.CombatBuffBehavior);
                if (RoutineManager.Current.CombatBehavior != null)
                    combatBehaviors.Add(RoutineManager.Current.CombatBehavior);
            }

            System.Collections.Generic.List<Composite> rootChildren = new System.Collections.Generic.List<Composite>();
            rootChildren.Add(new Decorator(ret => IsPaused, new ActionAlwaysSucceed()));
            rootChildren.Add(new Decorator(ret => SanityCheckCombat(), SelectLockMethod(combatBehaviors.ToArray())));

            if (RoutineManager.Current != null)
            {
                if (RoutineManager.Current.PreCombatBuffBehavior != null)
                {
                    rootChildren.Add(new Decorator(
                        ret => RoutineManager.Current.NeedPreCombatBuffs,
                        RoutineManager.Current.PreCombatBuffBehavior));
                }

                if (RoutineManager.Current.RestBehavior != null)
                {
                    rootChildren.Add(new Decorator(
                        ret => RoutineManager.Current.NeedRest,
                        RoutineManager.Current.RestBehavior));
                }
            }

            return new PrioritySelector(rootChildren.ToArray());
        }

        /// <summary>
        /// Initializes all the required components for Tyrael to run.
        /// </summary>
        private static void InitializeComponents()
        {
            try
            {
                TyraelSettings.Instance.Load();
                TyraelUpdater.CheckForUpdate();
                TyraelUtilities.StatCounter();
                TyraelUtilities.RegisterHotkeys();
                TyraelUtilities.PrintInformation();
                TyraelUtilities.ClickToMove(1000);

                ProfileManager.LoadEmpty();

                _oldLogoutForInactivity = GlobalSettings.Instance.LogoutForInactivity;
                GlobalSettings.Instance.LogoutForInactivity = false;
                TreeRoot.TicksPerSecond = CharacterSettings.Instance.TicksPerSecond;
            }
            catch (Exception initializecomponentsexception)
            {
                TyraelUtilities.DiagnosticLog("InitializeComponents() Error: {0}", initializecomponentsexception);
            }
        }

        /// <summary>
        /// Stops components for Tyrael.
        /// </summary>
        private static void StopComponents()
        {
            try
            {
                TyraelUtilities.RemoveHotkeys();
                GlobalSettings.Instance.LogoutForInactivity = _oldLogoutForInactivity;
                TreeRoot.TicksPerSecond = CharacterSettings.Instance.TicksPerSecond;
            }
            catch (Exception stopcomponentsexception)
            {
                TyraelUtilities.DiagnosticLog("StopComponents() Error: {0}", stopcomponentsexception);
            }
        }

        /// <summary>
        /// Initializes the proper plugins based on the Setting CheckPluginPulsing.
        /// </summary>
        internal static void InitializePlugins()
        {
            try
            {
                if (TyraelSettings.Instance.PluginPulsing)
                {
                    _pulseFlags = PulseFlags.CharacterManager | PulseFlags.InfoPanel | PulseFlags.Lua | PulseFlags.Objects | PulseFlags.Plugins;
                }
                else
                {
                    _pulseFlags = PulseFlags.InfoPanel | PulseFlags.Lua | PulseFlags.Objects;
                }
            }
            catch (Exception initializepluginsexception)
            {
                TyraelUtilities.DiagnosticLog("InitializePlugins() Error: {0}", initializepluginsexception);
            }
        }
        #endregion

        #region Softlock
        /// <summary>
        /// Used for Locking only the BotBase and Routine - Not HonorBuddy itself (Softlock). Only kicks in when HardLock isn't enabled.
        /// </summary>
        private static Composite SelectLockMethod(params Composite[] children)
        {
            return TyraelSettings.Instance.UseSoftLock && !GlobalSettings.Instance.UseFrameLock ? new FrameLockSelector(children) : new PrioritySelector(children);
        }

        private class FrameLockSelector : PrioritySelector
        {
            public FrameLockSelector(params Composite[] children)
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
