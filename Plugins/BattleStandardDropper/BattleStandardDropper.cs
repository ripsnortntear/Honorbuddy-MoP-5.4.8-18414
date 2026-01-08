using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;

using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.Common;


namespace BattleStandardDropper {
	public class Color {
		public static readonly System.Windows.Media.Color Red = System.Windows.Media.Color.FromRgb(255, 0, 0);
	}

	[Serializable]
	public class Settings {
		private static Settings instance;

		public static Settings Instance {
			get {
				if (null == instance) {
					instance = Load();
				}
				
				return instance;
			}
		}

		#region Settings Serialization

		public static string ConfigFileFormat = "BattleStandardDropper_{0}.config";

		public static string ConfigFile {
			get { return string.Format(ConfigFileFormat, StyxWoW.Me.Name); }
		}

		public static string SavePath {
			get {
				string path = Process.GetCurrentProcess().MainModule.FileName;
				path = Path.GetDirectoryName(path);
				path = Path.Combine(path, @"Plugins\BattleStandardDropper\settings");
				return path;
			}
		}

		private static XmlSerializer serializer;

		private static XmlSerializer Serializer {
			get {
				if (null == serializer) {
					serializer = new XmlSerializer(typeof(Settings));
				}

				return serializer;
			}
		}

		public static Settings Load() {
			string path = SavePath;
			string file = Path.Combine(path, ConfigFile);

			try {
				using (FileStream fStream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
					return (Settings)Serializer.Deserialize(fStream);
				}
			} catch {
				return new Settings();
			}
		}

		public void Save() {
			string path = SavePath;
			string file = Path.Combine(path, ConfigFile);

			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			try {
				using (FileStream fStream = new FileStream(file, FileMode.Create, FileAccess.Write)) {
					Serializer.Serialize(fStream, this);
				}
			} catch (Exception e) {
				Logging.Write(Color.Red, "Error saving BattleStandardDropper settings");
				Logging.WriteException(Color.Red, e);
			}
		}

		#endregion

		public uint DropZone = 0;
		public uint DropLocationRange = 30;
		public float DropX = 0;
		public float DropY = 0;
		public float DropZ = 0;
	}

	public class BattleStandardDropper : HBPlugin {
		private const string _revision = "$Revision: 386 $";
		private static readonly int revision;
		private static readonly Version version;

		private const long StandardCooldownMS = (long)(10.1 * 60 * 1000);

		private static readonly uint[] BattleStandards = {
			64402, // H 15%
			64399, // A 15%
			64401, // H 10%
			64398, // A 10%
			64400, // H 5%
			63359  // A 5%
		};

		private static readonly uint[] BattleStandardBuffs = {
			90633, // H 15%
			64399, // A 15%
			90632, // H 10%
			90626, // A 10%
			90631, // H 5%
			89479  // A 5%
		};

		private const uint GuildFaction = 1168;


		private static uint DropZone {
			get { return Settings.Instance.DropZone; }
			set { Settings.Instance.DropZone = value; }
		}

		// workaround
		private static WoWPoint dropLocation = WoWPoint.Empty;

		private static WoWPoint DropLocation {
			get {
				if (WoWPoint.Empty == dropLocation) {
					dropLocation = new WoWPoint(
						Settings.Instance.DropX,
						Settings.Instance.DropY,
						Settings.Instance.DropZ);
				}

				return dropLocation;
			}

			set {
				Settings.Instance.DropX = value.X;
				Settings.Instance.DropY = value.Y;
				Settings.Instance.DropZ = value.Z;
				dropLocation = value;
			}
		}

		private static uint DropLocationRange {
			get { return Settings.Instance.DropLocationRange; }
			set { Settings.Instance.DropLocationRange = value; }
		}

		private static uint DropLocationRangeSqr {
			get { return Settings.Instance.DropLocationRange * Settings.Instance.DropLocationRange; }
		}

		static BattleStandardDropper() {
			string str = string.Empty;

			try {
				str = _revision.Substring(11, _revision.Length - 2 - 11);
				revision = int.Parse(str);
			} catch {
				revision = 0;
			}

			version = new Version(0, 1, 1, revision);
		}

		public static LocalPlayer Me {
			get { return StyxWoW.Me; }
		}

		private static uint GetItemCount(uint id) {
			return
				Lua.GetReturnVal<uint>("return GetItemCount(\"" + id + "\")", 0);
		}

		private static bool IsItemOnCooldown(uint id) {
			return
				0 != Lua.GetReturnVal<float>("return GetItemCooldown(" + id + ")", 0);
		}

		private static float GetItemCooldownTime(uint id) {
			List<string> values = Lua.GetReturnValues("return GetTime(), GetItemCooldown(" + id + ")");

			try {
				float now = float.Parse(values[0]);
				float cdStart = float.Parse(values[1]);
				float duration = float.Parse(values[2]);

				return (cdStart + duration) - now;
			} catch { }

			return 0;
		}


		public BattleStandardDropper() {

		}

		public override string Author {
			get { return "timglide"; }
		}

		public override string Name {
			get { return "BattleStandardDropper"; }
		}

		public override Version Version {
			get { return version; }
		}

		public override bool WantButton {
			get { return initialized; }
		}

		public override string ButtonText {
			get { return "Settings"; }
		}

		public override void OnButtonPress() {
			if (!initialized) {
				return;
			}

			DropZone = Me.ZoneId;
			DropLocation = Me.Location;
			Logging.Write("Will drop battle standard around {0}", DropLocation);
			Settings.Instance.Save();
		}

		private void FindBattleStandard() {
			battleStandardId = 0;
			battleStandardBuffId = 0;

			//using (new FrameLock()) { // FIXME framelock
				if ((int)Me.GetReputationLevelWith(GuildFaction) < (int)WoWUnitReaction.Friendly) {
					return;
				}

				for (int i = 0; i < BattleStandards.Length; i++) {
					if (GetItemCount(BattleStandards[i]) >= 1) {
						battleStandardId = BattleStandards[i];
						battleStandardBuffId = BattleStandardBuffs[i];
						return;
					}
				}
			//}
		}

		private Stopwatch useStandardSW = new Stopwatch();
		private bool initialized = false;
		private uint battleStandardId = 0;
		private uint battleStandardBuffId = 0;
		private long curStopwatchDurationMS = 0;

		public override void Initialize() {
			if (initialized) {
				return;
			}

			dropLocation = WoWPoint.Empty;
			FindBattleStandard();
			useStandardSW.Reset();
			initialized = true;
			
			if (0 != battleStandardId) {
				Logging.Write("BattleStandardDropper initialized with standard = {0}", battleStandardId);

				if (WoWPoint.Empty != DropLocation) {
					Logging.Write("Will drop battle standard around {0}", DropLocation);
				}
			} else {
				Logging.Write("BattleStandardDropper initialized but no standard found or you're not friendly with guild!");
			}
		}

		public override void Dispose() {
			// Settings were saved when the button was pressed, this would save an empty file
			// for chars that haven't even pressed the button
//			Settings.Instance.Save();
			initialized = false;
			useStandardSW.Reset();
		}

		public override void Pulse() {
			if (0 == battleStandardId) {
				return;
			}

			if (WoWPoint.Empty == DropLocation) {
				return;
			}

			if (useStandardSW.IsRunning && useStandardSW.ElapsedMilliseconds < curStopwatchDurationMS) {
				return;
			}

			if (DropZone != Me.ZoneId) {
				return;
			}

			if (IsItemOnCooldown(battleStandardId)) {
				int remaining = (int)GetItemCooldownTime(battleStandardId) + 1;
				Logging.Write("Dropping battle standard again in {0} second{1}.", remaining, 1 != remaining ? "s" : "");
				curStopwatchDurationMS = remaining * 1000;
				useStandardSW.Reset();
				useStandardSW.Start();
				return;
			}

			if (DropLocation.DistanceSqr(Me.Location) > DropLocationRangeSqr) {
				return;
			}

			if (0 == useStandardSW.ElapsedMilliseconds) {
				Logging.Write("Dropping battle standard now!");
			} else {
				Logging.Write("Dropping battle standard now ({0}s since last)!", useStandardSW.ElapsedMilliseconds / 1000);
			}

			useStandardSW.Reset();


			Thread.Sleep(250);
			Lua.DoString("UseItemByName(" + battleStandardId + ")");
			Thread.Sleep(250);
		}
	}
}
