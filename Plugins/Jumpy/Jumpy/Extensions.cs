using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumpy
{
	public static class Extensions
	{
		static Random rand = new Random();
		public static void PauseAll(this List<Routine> routines)
		{
			foreach (Routine routine in routines)
				routine.Pause();
		}

		public static void ResumeAll(this List<Routine> routines)
		{
			foreach (Routine routine in routines)
				routine.Resume();
		}

		/// <summary>
		/// Finds all routines that are done cooling down, aren't currently running, and are not paused.
		/// </summary>
		/// <param name="routines"></param>
		/// <returns></returns>
		public static List<Routine> ReadyRoutines(this List<Routine> routines)
		{
			return routines.Where(routine => routine.IsReady).ToList();
		}

		/// <summary>
		/// Finds the routine that is currently running and not paused! (Must only be up to 1 running at a time). 
		/// </summary>
		public static Routine RunningRoutine(this List<Routine> routines)
		{
			return routines.Where(routine => routine.CurrentState == Routine.State.Running && !routine.IsPaused).SingleOrDefault(); 
		}

		public static Routine BestRoutine(this List<Routine> routines)
		{
			Routine runningRoutine = routines.RunningRoutine();
			List<Routine> readyRoutines = routines.ReadyRoutines();
			Routine routine = null;
			if (runningRoutine != null)
				routine = runningRoutine;
			else if (readyRoutines.Count > 0)
				routine = readyRoutines[rand.Next(readyRoutines.Count)];
			return routine;
		}

		public static Routine ManageRoutine(this List<Routine> routines)
		{
			Routine routine = routines.BestRoutine();
			if (routine == null) return routine;
			if (routine.CurrentState == Routine.State.Running && routine.IsDone)
			{
				routine.Stop();
				routine = routines.BestRoutine();
			}
			if (routine == null) return routine;
			if (routine.CurrentState != Routine.State.Running)
				routine.Start();
			return routine;
		}

		public static void Add(this List<Routine> routines, string profile, string line, JumpyWindow window)
		{
			Routine routine;
			try
			{
				routine = Routine.Parse(profile, line, window);
				if (routines.Any(r => r == routine))
					throw new Exception("Cannot add duplicate routines! (All routines within the same profile must have unique names, no matter the casing");
				routines.Add(routine);
			}
			catch (Exception e)
			{
				window.Log(profile.Split('.')[0] + "." + line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0] + ":  " + e.Message);
			}
		}

		public static string CustomFormat(this TimeSpan timespan)
		{
			return timespan.Minutes + ":" + (timespan.Seconds == 0 ? "00" : timespan.Seconds.ToString());
		}
	}
}
