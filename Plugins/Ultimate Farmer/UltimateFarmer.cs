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
using Styx.Common;
using Styx.Plugins;
using System.Linq;
using System.Windows.Media;
using System.Windows.Forms;
using Styx.CommonBot.POI; 
using Bots.ArchaeologyBuddy;
using System.Text.RegularExpressions;
using System.Drawing;
using Styx.CommonBot;
using GreyMagic;
using Styx.Common.Helpers;
using Styx.CommonBot.Inventory;
using Styx.Localization;
using System.Media;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.Globalization;
using Styx.CommonBot.Frames;
using Styx.WoWInternals.DBC;
using UltimateFarmer.Settings;
using Styx.CommonBot.Profiles;

namespace UltimateFarmer
{
   public class Main : HBPlugin

    {
		private Form _gui;
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _targetLast;
		private static DateTime _targetLastT;  		
        public override string Author { get { return "directed by bCrazy [moded by PePPeRmix]"; }}
        public override string ButtonText { get { return "Setup"; } }
        public override string Name { get { return "- ULTIMATE FARMER (PePPeRmix mod)"; }}
        public override Version Version { get { return new Version(0,0,6); }}
		private Stopwatch throttle = new Stopwatch();
		private Stopwatch loottimer = new Stopwatch();
		private Stopwatch runtime = new Stopwatch();
		private static string SvnRevision { get { return "$Rev: 06 $"; } }
		private static DateTime _movementSuspendedTime;
        private static bool _movementSuspended;
		private static bool _initialized;
		private static bool looting = false;
		private static bool loadprofile;
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetAsyncKeyState(int vkey);
		public override bool WantButton { get { return true; } }
		private static ulong guid;
		private static int blid1;
		private static int blid2;
		private static int blid3;
		private static bool _okpulling = false;
		private static bool _KillAll = false;
		private static DateTime _PullSuspendedTime;
		public WoWUnit debugUnit;
		public int stepToEnd = 0;
		private static DateTime _PullSuspendedTime_2;
		public int totalpulls = 0;
		public List<WoWUnit> pulledList = new List<WoWUnit>();
		public WoWUnit unit;
		public WoWUnit unitL;
		//public var XMLFilepath = Path.Combine(Utilities.AssemblyDirectory, "Plugins/Ultimate Farmer/blank.xml");
		
		public static string XMLFilepath = @"Plugins\Ultimate Farmer\" + "blank.xml";
		public override void OnButtonPress()
        {
            if (_gui == null || _gui.IsDisposed || _gui.Disposing) _gui = new GUI();
            if (_gui != null || _gui.IsDisposed) _gui.ShowDialog();
        }
		
		
		
		
        static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState((int)key) & 0x8000) != 0;
        }
        
		public override void Initialize()
        {
			
				//loadprofile = true;
			//ProfileManager.LoadNew(XMLFilepath);
		//while (ProfileManager.CurrentOuterProfile.Equals(null) || ProfileManager.CurrentProfile.Equals(null)) { Thread.Sleep(500); }
			
                 blid1 = Settings.UFSettings.Instance.Bl1;
				 blid2 = Settings.UFSettings.Instance.Bl2;
				 blid3 = Settings.UFSettings.Instance.Bl3;
                 base.Initialize();
				Logging.Write(Colors.Fuchsia, "ULTIMATE FARMER Initialized [ {0}]", SvnRevision.Replace("$", "")); // Will print as [ Rev: 1 ]
				Logging.Write(Colors.Fuchsia, "SETTINGS: [Max Distance: {0} yds] [Max Mobs: {1}]  [Max HP: {2}]", Settings.UFSettings.Instance.PullRange, 
				Settings.UFSettings.Instance.MobMax, Settings.UFSettings.Instance.MaxHealth); 
				Logging.Write(Colors.Fuchsia, "FACTIONS: [{0}] [{1}] [{2}]", Settings.UFSettings.Instance.FactionId1, Settings.UFSettings.Instance.FactionId2, Settings.UFSettings.Instance.FactionId3);
				Logging.Write(Colors.Fuchsia, "Blacklist: [{0}] [{1}] [{2}]", Settings.UFSettings.Instance.Bl1, Settings.UFSettings.Instance.Bl2, Settings.UFSettings.Instance.Bl3);
				if (_KillAll)
				{
					loadprofile = true;
					
					if (Settings.UFSettings.Instance.RunTime != 0)
					{
					Logging.Write(Colors.Gold, "[UF] KILL ALL is Active and Running for {0} Min.]", Settings.UFSettings.Instance.RunTime);
					}
					if (Settings.UFSettings.Instance.RunTime == 0)
					{
					Logging.Write(Colors.Gold, "[UF] KILL ALL is Active and Running Until Stopped");
					}
				}
				_PullSuspendedTime = DateTime.UtcNow;
		}
		 
		/////////////////// PULSE ///////////////////////////
        
	public override void Pulse() 
    {
		bool stopRT = false;
		if 	(Settings.UFSettings.Instance.RunTime != 0)
		{
			if (!runtime.IsRunning) runtime.Start();
			if (runtime.Elapsed.TotalMinutes > Settings.UFSettings.Instance.RunTime) 
			{
				stopRT = true;
				if (runtime.IsRunning) runtime.Stop();
			}
		}
			
		if 	(!stopRT)
		{
				if 	(!_KillAll)
				{
					//SuspendMovement();
					//if (UserIsMoving()) return;
					
					if (Settings.UFSettings.Instance.LootInCombat) lootNow();
					
					AquireTarget();
					
					if (!Settings.UFSettings.Instance.LootInCombat && Lootables.Count() > Settings.UFSettings.Instance.MaxCorpse)
					{
						_okpulling = false;
						looting = true;
					}
					
					if (Me.Combat && 
						(totalpulls > Settings.UFSettings.Instance.MobMax || Me.HealthPercent < Settings.UFSettings.Instance.MaxHealth)
						|| NearbyUnfriendlyUnits.Count() > Settings.UFSettings.Instance.MobMax)
						{
						_okpulling = false;
						}
					
					if (!Me.Combat && (Lootables.Count() < Settings.UFSettings.Instance.MaxCorpse || Settings.UFSettings.Instance.LootInCombat))
						{
						_okpulling = true;
						looting = false;
						totalpulls = 0;
						}
					
					/////////////// PULL /////////////////////////////
					if (_okpulling)
					{
						if (totalpulls > Settings.UFSettings.Instance.MobMax || Me.HealthPercent < Settings.UFSettings.Instance.MaxHealth)
						{
						_okpulling = false;
						} else
						{
							Pull();
						}
					}else if(looting)
					{
						lootNow();
					}
					
				}

				if (_KillAll)
				{	
				if (loadprofile)
				{
					if (!ProfileManager.CurrentProfile.Name.Equals("UF Kill All"))
					{
					ProfileManager.LoadNew(XMLFilepath);
					}
				}
					
					AquireTarget();
					
					//CheckClass();
					//SuspendMovement();
					//if (UserIsMoving()) return;
					if (Lootables.Count() < Settings.UFSettings.Instance.MaxCorpse)
					{
					_okpulling = true;
					looting = false;
					}
								
					if (Lootables.Count() > Settings.UFSettings.Instance.MaxCorpse)
					{
						_okpulling = false;
						looting = true;
						//loottimer.Start();
						
						//if (BotPoi.Current.Type != PoiType.Loot && loottimer.Elapsed.TotalSeconds > 3)
						//{
						//loottimer.Reset();
						//unitL = Lootable;
						//_okpulling = false;
						//BotPoi.Current = new BotPoi(unitL, PoiType.Loot);
						//Logging.Write(Colors.Fuchsia, "[Forcing Looting {0}]", unitL);
						//}
						
					}
					
					if  ((totalpulls > Settings.UFSettings.Instance.MobMax || Me.HealthPercent < Settings.UFSettings.Instance.MaxHealth)
						|| (NearbyUnfriendlyUnits.Count() > Settings.UFSettings.Instance.MobMax))
						{
						_okpulling = false;
						}
					
					if (!Me.Combat && Lootables.Count() < Settings.UFSettings.Instance.MaxCorpse && !looting)
						{
						_okpulling = true;
						totalpulls = 0;
						}
					
					/////////////// PULL /////////////////////////////
					if (_okpulling)
					{
						if (totalpulls > Settings.UFSettings.Instance.MobMax || Me.HealthPercent < Settings.UFSettings.Instance.MaxHealth)
						{
						_okpulling = false;
						}
					
						if (!UserIsMoving() && 
						totalpulls < Settings.UFSettings.Instance.MobMax && Me.HealthPercent > Settings.UFSettings.Instance.MaxHealth)
						{
						PullAll();
						}
					}
				}
				////////////////// FACE TARGET ///////////////////		
				//	throttle.Start();
				//	
				//	if(UserIsMoving())
				//		{
				//		throttle.Reset();
				//		}
				//	
				//	if (throttle.Elapsed.TotalSeconds > 1.5 && !UserIsMoving())
				//		{
				//		Face();
				//		}
		}
		
		
		////////////////////////// END PULSE //////////////////////////////////
		}
		///////////////// PULL /////////////////////////
		public void Pull()
        {
          
           
		   if (Me.Combat || (Me.GotAlivePet && Me.Pet.Combat))
            {
					if (Settings.UFSettings.Instance.AllFactions)
					{
					unit =FarmAll;
					}
					 
					if (!Settings.UFSettings.Instance.AllFactions)
					{
					unit = Farm;
					}
					
					
					if (unit != null && !unit.IsDead && unit.Guid != guid && !unit.TaggedByOther && DateTime.UtcNow > _PullSuspendedTime.AddMilliseconds(2000))
                    {
						guid = unit.Guid;
						if (!Settings.UFSettings.Instance.PullInFight)
						{
							totalpulls = totalpulls + 1;
						}
						
						Logging.Write(Colors.Fuchsia, "[ PULL ] [ {0} ]    [{1}] [Distance: {2}]", totalpulls, unit.SafeName, unit.Distance);
						unit.Target();
						BotPoi.Current = new BotPoi(unit, PoiType.Kill);
						
						_PullSuspendedTime = DateTime.UtcNow;
						
						return;
						
                    }
			}
        }
		
				
		public void PullAll()
        {
			unit = FarmAll;
                    
			if (unit != null && !unit.IsDead && unit.Guid != guid && !unit.TaggedByOther && DateTime.UtcNow > _PullSuspendedTime.AddMilliseconds(2000))
            {
				guid = unit.Guid;
				if (!Settings.UFSettings.Instance.PullInFight)
				{
					totalpulls = totalpulls + 1;
				}
				
				Logging.Write(Colors.Fuchsia, "[ PULL ] [ {0} ]    [{1}] [Distance: {2}]", totalpulls, unit.SafeName, unit.Distance);
				unit.Target();
				Navigator.MoveTo(unit.Location);
				BotPoi.Current = new BotPoi(unit, PoiType.Kill);
				
				_PullSuspendedTime = DateTime.UtcNow;
				
				return;
				
            }
        }
		
		public void lootNow()
		{
			if (Me.IsCasting || (Lootables.Count() < Settings.UFSettings.Instance.MaxCorpse  && !Settings.UFSettings.Instance.LootInCombat) || Me.IsMoving)
                return;
            if (LootFrame.Instance != null && LootFrame.Instance.IsVisible)
            {
                LootFrame.Instance.LootAll();
                return;
            }
            var lootTarget = Lootables.FirstOrDefault();
            if (lootTarget != null && lootTarget.Distance > lootTarget.InteractRange)
            {
				Logging.Write(Colors.Gold, "[UF] [Moving to loot {0}.]", lootTarget.Name);
                Navigator.MoveTo(lootTarget.Location);
            }
            else if (lootTarget != null)
            {
                lootTarget.Interact();
                return;
            }
		}
		
		private static bool UserIsMoving()
        {
            return  false;
			//IsKeyDown(Keys.A) || 
             //       IsKeyDown(Keys.S) || 
             //       IsKeyDown(Keys.D) || 
             //       IsKeyDown(Keys.W) || 
             //       IsKeyDown(Keys.Q) ||
             //       IsKeyDown(Keys.E);
        }

		/// TARGET ///////////////////////////////////
		
       public void AquireTarget()
        {
			if (Me.GotTarget)
			{
				WoWUnit _unitT;
				_unitT = Me.CurrentTarget as WoWUnit;
				if (!_unitT.IsAlive)
				{
					Me.ClearTarget();
				}
				
				
				
				if (_unitT.IsAlive && _unitT.Distance > 40 && DateTime.UtcNow > _PullSuspendedTime_2.AddMilliseconds(1000))
				{
					if (debugUnit != null && _unitT != null && debugUnit == _unitT && stepToEnd > 2)
					{
						Me.ClearTarget();
						stepToEnd = 0;
						Logging.Write(Colors.Red, "[UF][Target unreacheble, clear target]");
					}else if (_unitT != null && (debugUnit == null || debugUnit != _unitT))
					{
						debugUnit = _unitT;
						stepToEnd = 0;
					}
					
					stepToEnd++;
					_PullSuspendedTime_2 = DateTime.UtcNow;
				}
			}
			
			if (Settings.UFSettings.Instance.PullInFight && (Me.Combat || Me.PetInCombat))
			{
				List<WoWUnit> tempUList = takeUnits;
				
				for (int i=0; i<tempUList.Count; i++)
				{
					if (tempUList[i].IsTargetingMeOrPet && !pulledList.Contains(tempUList[i]))
					{
						pulledList.Add(tempUList[i]);
					}
				}
				
				for (int j=pulledList.Count-1; j>=0; j--)
				{
					if ((pulledList[j] != null && (pulledList[j].IsDead || !pulledList[j].IsTargetingMeOrPet || !pulledList[j].IsAlive)) || pulledList[j] == null)
					{
						pulledList.RemoveAt(j);
					}
				}
				totalpulls = pulledList.Count;
			}
			
			if ((Me.Combat || Me.PetInCombat) && !UserIsMoving() &&
			(!Me.GotTarget || Me.CurrentTarget.IsFriendly || Me.CurrentTarget.IsDead))
			{
            
				WoWUnit unitT;
				unitT = AqTarget;
            
				if (unitT != null && !unitT.IsDead)
				{
					unitT.Target();
					Logging.Write(Colors.Gold, "[ UF ] [ TARGET: {0}] [Distance: {1}]", unitT.SafeName, unitT.Distance);
					
				}
			}
			
        }
		
		
		private static void SuspendMovement()
        {
            
            if (UserIsMoving())
            {
                if (!_movementSuspended)
                {
                    Logging.Write(Colors.Fuchsia, "ULTIMATE FARMER Off");
                }
                _movementSuspended = true;
                _movementSuspendedTime = DateTime.UtcNow;
            }
            
            if (_movementSuspended && !UserIsMoving() && DateTime.UtcNow > _movementSuspendedTime.AddMilliseconds(1500))
            {
                if (_movementSuspended)
                {
                    Logging.Write(Colors.Fuchsia, "ULTIMATE FARMER Active");
                }
                _movementSuspended = false;

            }

        }
		
		public WoWUnit getByGUID(ulong _GUID_)
		{
			
			return ObjectManager.GetObjectByGuid<WoWUnit>(_GUID_);
			
		}
		
		public WoWUnit AqTarget
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o != null && !o.IsDead && o.IsTargetingMeOrPet).FirstOrDefault();
            }
		}
		
		public List<WoWUnit> takeUnits
		{
			get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => (
                    o.FactionId == Settings.UFSettings.Instance.FactionId1 || o.FactionId == Settings.UFSettings.Instance.FactionId2 || o.FactionId == Settings.UFSettings.Instance.FactionId3) 
					&& o.Distance < Settings.UFSettings.Instance.PullRange && !o.IsDead && !o.IsTargetingMeOrPet && !o.TaggedByOther && 
					o.CanSelect && !o.IsFriendly && !o.IsDead && !o.IsNonCombatPet && !o.IsCritter &&
					(o.Entry != blid1 && o.Entry != blid2 && o.Entry != blid3)).OrderBy(o => o.Distance).ToList();
            }
		}
		
		public WoWUnit Farm
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => (
                    o.FactionId == Settings.UFSettings.Instance.FactionId1 || o.FactionId == Settings.UFSettings.Instance.FactionId2 || o.FactionId == Settings.UFSettings.Instance.FactionId3) 
					&& o.Distance < Settings.UFSettings.Instance.PullRange && !o.IsDead && !o.IsTargetingMeOrPet && !o.TaggedByOther && 
					o.CanSelect && !o.IsFriendly && !o.IsDead && !o.IsNonCombatPet && !o.IsCritter &&
					(o.Entry != blid1 && o.Entry != blid2 && o.Entry != blid3)).OrderBy(o => o.Distance).FirstOrDefault();
            }
		}	
		
		public WoWUnit FarmAll
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o != null && o.Distance < Settings.UFSettings.Instance.PullRange && !o.IsDead && !o.IsTargetingMeOrPet && !o.TaggedByOther &&
					o.Attackable && o.IsValid && o.CanSelect && !o.IsFriendly && !o.IsDead && !o.IsNonCombatPet && !o.IsCritter && 
					(o.Entry != blid1 && o.Entry != blid2 && o.Entry != blid3)).OrderBy(o => o.Distance).FirstOrDefault();
            }
		}	
		
		public WoWUnit Lootable
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o!= null && o.IsDead && o.Lootable).OrderBy(o => o.Distance).FirstOrDefault();
            }
		}	
		
		public void Face()
		{
			if (!UserIsMoving() && !Me.HasAura("Food") && !Me.HasAura("Drink") && (Me.Combat || Me.PetInCombat))
            {
                if (!Me.CurrentTarget.IsDead && !Me.IsFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 50)
                {
                    Logging.Write(Colors.Gold, "[UF][Facing: {0}]", Me.CurrentTarget.Name);
                    Me.CurrentTarget.Face();
					throttle.Reset();
                }
            }
		}
		
		public static bool IsViable(WoWObject wowObject)
        {
            return (wowObject != null) && wowObject.IsValid;
        }
		
			
				
		public static IEnumerable<WoWUnit> UnfriendlyUnits(int maxSpellDist)
        {
            
            Type typeWoWUnit = typeof(WoWUnit);
            Type typeWoWPlayer = typeof(WoWPlayer);
            List<WoWUnit> list = new List<WoWUnit>();
            List<WoWObject> objectList = ObjectManager.ObjectList;
            for (int i = 0; i < objectList.Count; i++)
            {
                Type type = objectList[i].GetType();
                if (type == typeWoWUnit || type == typeWoWPlayer)
                {
                    WoWUnit t = objectList[i] as WoWUnit;
                    if (t != null && t.IsTargetingMeOrPet)
                    {
                        list.Add(t);
                    }
                }
            }
            return list;
        }
		public static IEnumerable<WoWUnit> NearbyUnfriendlyUnits
        {
            get
            {
                return UnfriendlyUnits(100);
            }
        }
		
		private double LootRange
        {
            get { return LootTargeting.LootRadius; }
        }
        private IOrderedEnumerable<WoWUnit> Lootables
        {
            get
            {
                var targetsList = ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(
                        p => (p.Lootable || p.CanSkin) && p.Distance <= LootRange).OrderBy(l => l.Distance);
                return targetsList;
            }
        }
		////////////////// BLACKLIST //////////////////////
	}	
}

