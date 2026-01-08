using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Styx;
using Styx.CommonBot.Database;
using Styx.Helpers;
using Styx.Loaders;
using Styx.CommonBot.AreaManagement;
using Styx.CommonBot.AreaManagement.Triangulation;
using Styx.TreeSharp;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.Frames;
using Styx.Pathing;
using Styx.Pathing.OnDemandDownloading;
using Styx.Patchables;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using Styx.Common;

using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Forms;

namespace BrodieMobList
{
	public class BMobList
	{
		private static LocalPlayer Me { get { return StyxWoW.Me; } }
		
		#region Mobs for Spell Avoidance or Special Interactions
		// Eastern Kingdoms
		public static WoWUnit SIAgents { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 48742 || u.Entry == 48741) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit HillsbradHumans { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 2270 || u.Entry == 2503 || u.Entry == 2269) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		
		// Kalimdor
		public static WoWUnit TaurajoLooter { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 37743) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		
		// Pandaria Mobs
		public static WoWUnit JungleShredder { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 67285 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit MasterCaller { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 69286 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit MechaPounder { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 67967 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit ShredmasterP { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 67371 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit DreadKunchong { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 64717 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit Amberhusk { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 64982 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit AScorpion { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 63728 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit ShaoTienSorcerer { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 65133 && u.Distance < 5 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit Tormentor { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 59238 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit Krichon { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 63978 && u.Distance < 30).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit Cracklefang { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 58768 && u.IsAlive).FirstOrDefault();}}
		public static WoWUnit Sydow { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 63240 && u.IsAlive && u.Distance < 30).FirstOrDefault();}}
		public static List<WoWUnit> MantidNiuzao { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 61502 || u.Entry == 61508 || u.Entry == 61509 && u.Distance < 50 && u.IsAlive).OrderBy(u => u.Distance).ToList();}}
		public static WoWUnit WarbringerScout { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.Entry == 69768 || u.Entry == 69769 || u.Entry == 69841 || u.Entry == 69842) && u.IsAlive).FirstOrDefault();}}
		public static WoWUnit VengefulSpirit { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.Entry == 69806) && u.IsAlive).FirstOrDefault();}}
		
		// Isle of Thunder Mobs
		public static WoWUnit PrimalDirehorn { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.Entry == 70016 || u.Entry == 69983 || u.Entry == 69142 || u.Entry == 70017 || u.Entry == 70018 || u.Entry == 70019 || u.Entry == 69983 ) && u.IsAlive && u.Distance < 30).FirstOrDefault();}}
		public static WoWUnit ZColossus { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69405 && u.IsAlive && u.Distance < 30).FirstOrDefault();}}
		public static WoWUnit MDevilsaur { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69406 && u.IsAlive && u.Distance < 30).FirstOrDefault();}}
		public static WoWUnit MLMonoHan { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69326 && u.IsAlive && u.Distance < 50).FirstOrDefault();}}
		public static WoWUnit Itoka { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69461 && u.IsAlive && u.Distance < 50).FirstOrDefault();}}
		public static WoWUnit FCHoku { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69435 && u.IsAlive && u.Distance < 50).FirstOrDefault();}}
		public static List<WoWUnit> VileSpit { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 70571 && u.Distance < 100).ToList();}}
		public static List<WoWUnit> EnergizedMetal { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69637 && u.Distance < 20).ToList();}}
		public static List<WoWUnit> BallLightning { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 69462 && u.Distance < 20).ToList();}}
		
		// Timeless Isle Mobs
		public static WoWUnit TimelessBirds { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72762 || u.Entry == 73158) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit TimelessTurtle { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72765 || u.Entry == 72764 || u.Entry == 72045 || u.Entry == 73161) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit TimelessOxen { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72843 || u.Entry == 73160 || u.Entry == 72844) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit TimelessTiger { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72807 || u.Entry == 72805) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit TimelessRock { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72809) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit Ordon { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72898 || u.Entry == 72896 || u.Entry == 72875 || u.Entry == 72892 || u.Entry == 72895 || u.Entry == 72894) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit Spineclaw { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 72766) && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();}}
		public static WoWUnit SpectralPanda { get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && (u.Entry == 73018 || u.Entry == 73025 || u.Entry == 73021) && u.IsAlive && u.ControllingPlayer == null).OrderBy(u => u.Distance).FirstOrDefault();}}
		#endregion
		
		#region Items to check for
		
		public static WoWItem HiddenStash { get { return Me.BagItems.FirstOrDefault(r => r.Entry == 61387); }} // Eastern Plaguelands Drop
		public static WoWItem OozingBag { get { return Me.BagItems.FirstOrDefault(r => r.Entry == 20768); }} // Silithus Drop
		
		public static WoWItem SingingCrystal { get { return Me.BagItems.FirstOrDefault(r => r.Entry == 103641); }}
		public static WoWItem BookOfTheAges { get { return Me.BagItems.FirstOrDefault(r => r.Entry == 103642); }}
		public static WoWItem DewOfEternalMorning { get { return Me.BagItems.FirstOrDefault(r => r.Entry == 103643); }}
		public static WoWItem CrystalOfInsanity { get { return Me.BagItems.FirstOrDefault(r => r.Entry == 86569); }}
		
		#endregion
	}
}