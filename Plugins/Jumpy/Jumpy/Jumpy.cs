namespace Jumpy
{
    using Styx;
    using System;
    using Styx.Helpers;
    using Styx.Pathing;
    using System.Threading;
    using System.Diagnostics;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;
    using System.Linq;
    using System.Net;
    using System.Windows.Forms;
    using System.Drawing;
    using Styx.Plugins;
    using Styx;

	public class Jumpy : HBPlugin
	{
		Random rand = new Random();
		public override string Author { get { return "verythaiguy"; } }
		public override string Name { get { return "Jumpy"; } }
		public override Version Version { get { return new System.Version(1, 8); } }
		public override bool WantButton { get { return true; } }
		public override string ButtonText { get { return "Settings"; } }
		public Thread WorkerThread;
		public readonly int THREAD_SLEEP = 50;
		public readonly int JUMP_TO_FLY_SPAN = 1500;
		public readonly int JUMP_TO_FLY_HOLD = 200;
		public DateTime LastJumpToFly = DateTime.MinValue;
		public override void OnButtonPress()
		{
            if (Window == null) Window = new JumpyWindow(this);
			Window.Show();
		}

		public Jumpy()
		{
			ApplicationStartupPath = Application.StartupPath;
		}

		public static string ApplicationStartupPath = String.Empty;

		public enum State { Combat, Mounted, Walking, Flying };
		JumpyWindow Window;

		Dictionary<State, List<Routine>> Routines
		{
			get
			{
				Dictionary<State, List<Routine>> routines = new Dictionary<State, List<Routine>>();
				routines[State.Combat] = Window.CombatRoutines.Values.Aggregate(new List<Routine>(), (result, next) => { result.AddRange(next); return result; });
				routines[State.Walking] = Window.WalkingRoutines.Values.Aggregate(new List<Routine>(), (result, next) => { result.AddRange(next); return result; });
				routines[State.Mounted] = Window.MountedRoutines.Values.Aggregate(new List<Routine>(), (result, next) => { result.AddRange(next); return result; });
				return routines;
			}
		}

		LocalPlayer Me { get { return Styx.StyxWoW.Me; } }

		public State CurrentState
		{
			get
			{
				if (Me.Combat)
					return State.Combat;
				else if (Me.Mounted && !IsOnFlyingMount)
					return State.Mounted;
				else if (IsOnFlyingMount)
					return State.Flying;
				else
					return State.Walking;
			}
		}

		void OnBotStopped(EventArgs args)
		{
			WorkerThread.Abort();
		}



		public override void Pulse()
		{
            if (Window == null) Window = new JumpyWindow(this);
			if (WorkerThread == null || !WorkerThread.IsAlive)
			{
				WorkerThread = new Thread(new ThreadStart(Loop));
				WorkerThread.IsBackground = true;
				WorkerThread.Start();
			}
		}

        bool IsPlayerInRange 
        {
            get { return ObjectManager.GetObjectsOfType<WoWPlayer>().Any(player => player.Distance <= Window.PlayerRange); }
        }

		bool SafeToJump
		{
			get
			{
                try { return Styx.CommonBot.TreeRoot.IsRunning && Me.IsAlive && !Me.Looting && !Me.IsResting && !Me.IsCasting && (Me.IsMoving || Window.StandJumpInCombat && CurrentState == State.Combat || Window.StandJumpOutCombat && CurrentState != State.Combat) && (!Window.UsePlayerRange || IsPlayerInRange || CurrentState == State.Flying); }
				catch { return false; }
			}
		}

		Stopwatch sw = new Stopwatch();
		void WaitNextJump()
		{
			if (CurrentState == State.Combat || CurrentState == State.Walking)
				Thread.Sleep(Math.Max(Window.BaseWalkJumpSpan + rand.Next(Window.RandWalkJumpSpan) - THREAD_SLEEP, 0));
			else
				Thread.Sleep(Math.Max(Window.BaseMountJumpSpan + rand.Next(Window.RandMountJumpSpan) - THREAD_SLEEP,0));
		}

		void Loop()
		{
			var combat = Routines[State.Combat];
			var mounted = Routines[State.Mounted];
			var walking = Routines[State.Walking];
			var all = combat.Union(mounted).Union(walking).ToList();

			while (true)
			{
				try
				{
					Thread.Sleep(THREAD_SLEEP);
					if (!SafeToJump)
					{
                        Window.tbStatus.Text = "Stopped:  " + (!Styx.CommonBot.TreeRoot.IsRunning ? "Bot Stopped" : !Me.IsAlive ? "Dead" : Me.IsCasting ? "Casting" : Me.Looting ? "Looting" : Me.IsResting ? " Resting" : Window.UsePlayerRange && !IsPlayerInRange ? "Player not within " + Window.PlayerRange + " yards" : !Me.IsMoving ? "Motionless" : "None");
						all.PauseAll();
						continue;
					}

					if (CurrentState == State.Combat)
					{
						all.Except(combat).ToList().PauseAll();
						combat.ResumeAll();
						Routine routine = Routines[State.Combat].ManageRoutine();
						if (routine == null)
							continue; // Didn't find suitable routine.
						routine.Do();
					}

					else if (CurrentState == State.Mounted)
					{
						all.Except(mounted).ToList().PauseAll();
						mounted.ResumeAll();
						Routine routine = Routines[State.Mounted].ManageRoutine();
						if (routine == null)
							continue; // Didn't find suitable routine.
						routine.Do();
					}

					else if (CurrentState == State.Walking)
					{
						all.Except(walking).ToList().PauseAll();
						walking.ResumeAll();
						Routine routine = Routines[State.Walking].ManageRoutine();
						if (routine == null)
							continue; // Didn't find suitable routine.
						routine.Do();
					}

					else if (CurrentState == State.Flying)
					{
						Window.tbStatus.Text = CurrentState.ToString() + ": staying in the air";
						if (!Me.IsFlying && DateTime.Now.Subtract(LastJumpToFly) > TimeSpan.FromMilliseconds(JUMP_TO_FLY_SPAN))
						{
							Thread.Sleep(100);
							//Styx.Helpers.KeyboardManager.PressKey((char)Keys.Space);
							//Thread.Sleep(JUMP_TO_FLY_HOLD);
							//Styx.Helpers.KeyboardManager.ReleaseKey((char)Keys.Space);							
							Styx.Helpers.KeyboardManager.KeyUpDown((Char)Keys.Space);
							LastJumpToFly = DateTime.Now;
						}
					}


					if (CurrentState != State.Flying)
						WaitNextJump();
				}
				catch { }
			}

		}

		bool _isOnFlyingMount = false;
		bool flag = true;
		bool IsOnFlyingMount
		{ get {
			return Me.Mounted && Me.Auras.Keys.Any(s=>s.ToLower().Contains("flying mount"));
		}
		}

	}

		



}
