//This BotBase was created by Apoc, I take no credit for anything within this code
//I just changed "!StyxWoW.Me.CurrentTarget.IsFriendly" to "!StyxWoW.Me.CurrentTarget.IsHostile"
//For the purpose of allowing RaidBot to work within Arenas

using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.WoWInternals;
using Styx.TreeSharp;

namespace RaidBot
{
	public class RaidBot : BotBase
	{
		private Composite _root;
		public override string Name { get { return "Raid Bot"; } }
		public override bool RequiresProfile { get { return false; } }
		public override Composite Root { get { return _root ?? (_root = CreateRootBehavior()); } }
		public override PulseFlags PulseFlags { get { return PulseFlags.All & ~(PulseFlags.Targeting | PulseFlags.Looting); } }
		public bool IsPaused { get; set; }

		public override void Initialize()
		{
			BotEvents.Player.OnMapChanged += HandleMapChanged;
		}

		private void HandleMapChanged(BotEvents.Player.MapChangedEventArgs args)
		{
			_root = null;
		}

		public override void Start()
		{
			_root = null;
			TreeRoot.TicksPerSecond = 30;
			ProfileManager.LoadEmpty();

			HotkeysManager.Unregister("RaidBot Pause");
			HotkeysManager.Register("RaidBot Pause",
				Keys.X,
				ModifierKeys.Alt,
				hk =>
					{
						IsPaused = !IsPaused;
						if (IsPaused)
						{
							Lua.DoString("print('RaidBot Paused!')");
							// Make the bot use less resources while paused.
							TreeRoot.TicksPerSecond = 5;
						}
						else
						{
							Lua.DoString("print('RaidBot Resumed!')");
							// Kick it back into overdrive!
							TreeRoot.TicksPerSecond = 30;
						}
					});
		}

		private Composite CreateRootBehavior()
		{
			var currentRoutine = RoutineManager.Current;
			Composite preCombat = (currentRoutine != null && currentRoutine.PreCombatBuffBehavior != null)
				? currentRoutine.PreCombatBuffBehavior
				: new Action(ret => RunStatus.Failure);

			Composite heal = (currentRoutine != null && currentRoutine.HealBehavior != null)
				? currentRoutine.HealBehavior
				: new Action(ret => RunStatus.Failure);

			Composite combatBuff = (currentRoutine != null && currentRoutine.CombatBuffBehavior != null)
				? currentRoutine.CombatBuffBehavior
				: new Action(ret => RunStatus.Failure);

			Composite combat = (currentRoutine != null && currentRoutine.CombatBehavior != null)
				? currentRoutine.CombatBehavior
				: new Action(ret => RunStatus.Failure);

			return
				new PrioritySelector(
					new Decorator(ret => IsPaused,
						new Action(ret => RunStatus.Success)),
					new Decorator(ret => !StyxWoW.Me.Combat,
						new PrioritySelector(preCombat)),
					new Decorator(ret => StyxWoW.Me.Combat,
						new LockSelector(
							heal,
							new Decorator(ret => StyxWoW.Me.GotTarget && !StyxWoW.Me.CurrentTarget.IsFriendly && !StyxWoW.Me.CurrentTarget.IsDead,
								new PrioritySelector(
									combatBuff,
									combat)))));
		}

		public override void Stop()
		{
			_root = null;
			TreeRoot.ResetTicksPerSecond();
			HotkeysManager.Unregister("RaidBot Pause");
		}

		#region Nested type: LockSelector

		private class LockSelector : PrioritySelector
		{
			public LockSelector(params Composite[] children) : base(children)
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
