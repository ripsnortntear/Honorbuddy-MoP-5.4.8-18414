using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Jumpy
{

	public class Routine
	{
		public string Name;
		public TimeSpan Length;
		public TimeSpan Cooldown;
		public int MinMod;
		public int MaxMod;
		public Stopwatch sw = new Stopwatch();
		public enum State { Running, CoolingDown, New }
		public State CurrentState = State.New;
		private Random rand = new Random();
		public JumpyWindow Settings;

		public static Routine Parse(string profile, string unparsed, JumpyWindow window)
		{
			Routine routine;
			TimeSpan length, cooldown;
			int minmod, maxmod;
			string name;

			string[] settings = unparsed.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			name = profile.Split('.')[0] + "." + settings[0];
			if (settings.Length != 5)
				throw new Exception("Routine must contain exactly 5 entries: Name, Length, Cooldown, MinMod, MaxMod");
			if (!TryParseTime(settings[1], out length) || !TryParseTime(settings[2], out cooldown))
				throw new Exception("Routine's length and cooldown values must be the format -> min[:sec]");
			if (!int.TryParse(settings[3], out minmod) || !int.TryParse(settings[4], out maxmod))
				throw new Exception("Routine's MinMod and MaxMod values must be integers only!");
			if (minmod <= 0 || maxmod <= 0)
				throw new Exception("Routine's MinMod and MaxMod values must both be > 0");
			if (cooldown.Ticks < 0 || length.Ticks <= 0)
				throw new Exception("Routine's length value must be > 0:0");

			routine = new Routine(name, length, cooldown, minmod, maxmod);
			routine.Settings = window;
			return routine;
		}

		private static bool TryParseTime(string p, out TimeSpan length)
		{
			length = new TimeSpan();
			List<string> elements = p.Split(':').ToList();
            if (elements.Count == 1) elements.Add("0");
            if (elements[0].Trim() == "") elements[0] = "0";

			int min, sec;
			if (elements.Count == 2 && int.TryParse(elements[0], out min) && int.TryParse(elements[1], out sec))
			{
				length = new TimeSpan(0, min, sec);
				return true;
			}
			return false;
		}

		private Routine(string name, TimeSpan length, TimeSpan cooldown, int minmod, int maxmod)
		{
			Name = name; Length = length; Cooldown = cooldown; MinMod = Math.Min(minmod, maxmod); MaxMod = Math.Max(minmod, maxmod);
		}

		public bool IsReady
		{

			get { return !IsPaused && (CurrentState == State.New || sw.Elapsed > Cooldown && CurrentState == State.CoolingDown); }
		}

		public bool IsDone
		{
			get { return sw.Elapsed > Length && CurrentState == State.Running; }
		}


		public void Start()
		{
			CurrentState = State.Running;
			sw.Reset();
			sw.Start();
			Settings.Log("Started ", System.Drawing.FontStyle.Bold, System.Drawing.Color.DarkGreen);
			Settings.Log(Name + "  (Length " + Length.CustomFormat() + ")\r\n", System.Drawing.Color.DarkGreen);
		}

		public void Stop()
		{
			CurrentState = State.CoolingDown;
			sw.Reset();
			sw.Start();
			Settings.Log("Finished ", System.Drawing.FontStyle.Bold, System.Drawing.Color.DarkRed);
			Settings.Log(Name + "  (Cooldown " + Cooldown.CustomFormat() + ")\r\n", System.Drawing.Color.DarkRed);
		}

		public void Pause()
		{
			if (!sw.IsRunning)
				return;
			sw.Stop();
			if (CurrentState == State.Running)
			{
				Settings.Log("Paused ", System.Drawing.FontStyle.Bold, System.Drawing.Color.DarkGray);
				Settings.Log(Name + "\r\n", System.Drawing.Color.DarkGray);
			}
		}

		public bool IsPaused { get { return !sw.IsRunning; } }

		public void Resume()
		{
			if (sw.IsRunning)
				return;
			sw.Start();
			if (CurrentState == State.Running)
			{
				Settings.Log("Resumed ", System.Drawing.FontStyle.Bold, System.Drawing.Color.DarkBlue);
				Settings.Log(Name + "\r\n", System.Drawing.Color.DarkBlue);
			}
		}

		public void Do()
		{
            Settings.tbStatus.Text = Settings.jumpy.CurrentState.ToString() + ": " + Name + " [" + MinMod + "," + MaxMod + "] " + sw.Elapsed.Minutes + ":" + sw.Elapsed.Seconds + "/" + Length.Minutes + ":" + Length.Seconds + " " + Math.Round(sw.Elapsed.TotalMilliseconds / Length.TotalMilliseconds * 100) + "%";
			int mod = rand.Next(MinMod, MaxMod + 1);
			int num = rand.Next();
			Settings.Log(Math.Round(sw.Elapsed.TotalMilliseconds / Length.TotalMilliseconds * 100) + "%\tRand % " + mod + " = " + num%mod + (num%mod==0 ? "   J U M P " : "") + "\r\n");
			if (num % mod == 0)
				Styx.Helpers.KeyboardManager.KeyUpDown((char)Keys.Space);
		}

		public override string ToString()
		{
			return  Name + ", len: " + Length + ", cool: " + Cooldown + ", mod: [" + MinMod + "," + MaxMod +"]"; 
		}

		public override bool Equals(object obj)
		{
			return this.Name.ToLower() == ((Routine)obj).Name.ToLower();
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
