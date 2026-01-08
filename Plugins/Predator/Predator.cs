// Plugin Developed by Naut
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Net;
using System.Globalization;
using System.Windows.Media;
using System.Media;
using System.Linq;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.Pathing;
using Styx.WoWInternals.World;

namespace Predator
{
    class Predator : HBPlugin
    {
		
		#region Default Overrides
        public override string Name { get { return "Predator"; } }
        public override string Author { get { return "Naut"; } }
        public override Version Version { get { return new Version(1, 0, 0, 1); } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return true; } }
		
		public static Settings Settings = new Settings();
		public static LocalPlayer Me = StyxWoW.Me;
		
		private List<WoWUnit> unitsToKill;
        private List<WoWUnit> KillableUnits;
		private List<WoWUnit> humansToKill;
        private List<WoWUnit> KillableHumanoid;
		private List<WoWUnit> pocketsToPlunder;
        private List<WoWUnit> PickpocketableHumanoid;
		private List<WoWUnit> beastsToKill;
        private List<WoWUnit> KillableBeasts;
        private WoWUnit SelectedAliveTarget { get; set; }
        public int MobCount { get; private set; }
		
		public int MaxHPPercent = 99; // Max HP % -- Used for blacklist check.
		public int DamagedHPPercent = 95; // Max HP % -- Used for blacklist check.
		
		int BlacklistAfterMilliseconds=Convert.ToInt32(Settings.TimeToBlacklist);
		
        private static Stopwatch blacklisttimer = new Stopwatch();
		#endregion Default Overrides
		
		public Predator()
		{
			Logging.Write("Predator Loaded.");
			Settings.Load();
		}

		public override void OnButtonPress()
		{
			Settings.Load();
			ConfigForm.ShowDialog();
		}


		private Form MyForm;
		public Form ConfigForm
		{
			get
			{
				if (MyForm == null)
					MyForm = new Config();
				return MyForm;
			}
		}
		
		public override void Pulse()
        {	
			//                     Death Handling Start
			if (!Me.IsAlive && !Me.IsGhost)
			{
				Logging.Write("[Predator]: 'Ressurect Request' menu detected, clicking Accept in 2 seconds.");
				Thread.Sleep(2000);
				Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");
			}
			if (!Me.IsAlive && Me.IsGhost)
			{
				WoWPoint moveToHere = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 4);
				Navigator.MoveTo(moveToHere);
				Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");
				Logging.Write("[Predator]: 'Ressurecting.");
			}
			//                     Death Handling End
			
			//                     Find Target Start
			if (!Me.GotTarget && Settings.JustFarmCloth == true && Settings.JustFarmLeather == false)
			{
				if (!Me.Combat && Me.IsAlive && !Me.IsGhost && !Me.IsOnTransport && !Me.OnTaxi && !Me.Stunned && !Me.IsCasting) 
				{
					HuntHumanoid();
				}
			}
			if (!Me.GotTarget && Settings.PickpocketOnly == true)
			{
				if (!Me.Combat && Me.IsAlive && !Me.IsGhost && !Me.IsOnTransport && !Me.OnTaxi && !Me.Stunned && !Me.IsCasting) 
				{
					HuntPockets();
				}
			}
			if (!Me.GotTarget && Settings.JustFarmLeather == true && Settings.JustFarmCloth == false)
			{
				if (!Me.Combat && Me.IsAlive && !Me.IsGhost && !Me.IsOnTransport && !Me.OnTaxi && !Me.Stunned && !Me.IsCasting) 
				{
					HuntBeast();
				}
			}
			if (!Me.GotTarget && Settings.JustFarmLeather == false && Settings.JustFarmCloth == false)
			{
				if (!Me.Combat && Me.IsAlive && !Me.IsGhost && !Me.IsOnTransport && !Me.OnTaxi && !Me.Stunned && !Me.IsCasting) 
				{
					HuntAll();
				}
			}
			//                     Find Target End
			
			//                     Blacklist Start
			if (!Me.GotTarget && blacklisttimer.IsRunning)
			{
                blacklisttimer.Reset();
                blacklisttimer.Stop();
            }
			if (Me.GotTarget && Me.CurrentTarget.HealthPercent > MaxHPPercent && !blacklisttimer.IsRunning)
			{
				blacklisttimer.Reset();
                blacklisttimer.Start();
			}
			if (Me.GotTarget && Me.CurrentTarget.HealthPercent < DamagedHPPercent && blacklisttimer.IsRunning)
			{
				blacklisttimer.Reset();
                blacklisttimer.Stop();
			}
            if (Me.GotTarget && blacklisttimer.ElapsedMilliseconds >= BlacklistAfterMilliseconds && Me.CurrentTarget.HealthPercent < MaxHPPercent)
            {
				 Blacklist.Add(Me.CurrentTarget, BlacklistFlags.Pull, TimeSpan.FromSeconds(180));
				 Logging.Write("Detected current targets HP has not dropped below 100% for length of time chosen in settings, blacklist current target for 3minutes.");
				 Thread.Sleep(1000);
				 StyxWoW.Me.ClearTarget();
			}
			//                     Blacklist End
			
			//                     Handle Target Start
			if (Me.GotTarget && Settings.PickpocketOnly == false)
			{
				if (Me.CurrentTarget.Distance > 5 && !Me.IsCasting) 
				{
					WoWPoint moveToTarget = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, 4);
					Navigator.MoveTo(moveToTarget);
				}
				if (Me.CurrentTarget.Distance < 5 && Me.CurrentTarget.IsAlive && (Me.CurrentTarget.IsHumanoid || Me.CurrentTarget.IsUndead) && !Blacklist.Contains(Me.CurrentTarget, BlacklistFlags.Node) && Settings.Pickpocket == true) 
				{
					StyxWoW.Me.CurrentTarget.Face();
					SpellManager.Cast("Sap");
					Thread.Sleep(500);
					SpellManager.Cast("Pick Pocket");
					Thread.Sleep(1200);
					Blacklist.Add(Me.CurrentTarget, BlacklistFlags.Node, TimeSpan.FromSeconds(60));
				}
				if (Me.CurrentTarget.Distance < 5 && Me.CurrentTarget.IsAlive && Settings.Pickpocket == false) 
				{
					StyxWoW.Me.ToggleAttack();
				}
				if (Me.CurrentTarget.Distance < 5 && Me.CurrentTarget.IsAlive && Settings.Pickpocket == true && (!Me.CurrentTarget.IsHumanoid || !Me.CurrentTarget.IsUndead || Blacklist.Contains(Me.CurrentTarget.Guid, BlacklistFlags.Node))) 
				{
					StyxWoW.Me.ToggleAttack();
				}
				if (Me.CurrentTarget.Distance < 5 && !Me.CurrentTarget.IsAlive && Me.CurrentTarget.CanLoot) 
				{
					Me.CurrentTarget.Interact();
					Thread.Sleep(3000);
				}
				if (Me.CurrentTarget.Distance < 5 && !Me.CurrentTarget.IsAlive && Me.CurrentTarget.CanSkin && Settings.SkinMobs == true) 
				{
					Me.CurrentTarget.Interact();
					Thread.Sleep(3000);
				}
				if (!Me.CurrentTarget.IsAlive && !Me.CurrentTarget.CanLoot && (!Me.CurrentTarget.CanSkin || (Me.CurrentTarget.CanSkin && Settings.SkinMobs == false)))
				{
					StyxWoW.Me.ClearTarget();
				}
			}
			if (Me.GotTarget && Settings.PickpocketOnly == true)
			{
				if (Me.CurrentTarget.Distance > 5 && !Me.IsCasting) 
				{
					WoWPoint moveToTarget = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, 4);
					Navigator.MoveTo(moveToTarget);
				}
				if (Me.CurrentTarget.Distance < 5 && Me.CurrentTarget.IsAlive && (Me.CurrentTarget.IsHumanoid || Me.CurrentTarget.IsUndead) && !Blacklist.Contains(Me.CurrentTarget, BlacklistFlags.Node) && Settings.Pickpocket == true) 
				{
					StyxWoW.Me.CurrentTarget.Face();
					SpellManager.Cast("Sap");
					Thread.Sleep(500);
					SpellManager.Cast("Pick Pocket");
					Thread.Sleep(1200);
					Blacklist.Add(Me.CurrentTarget, BlacklistFlags.Node, TimeSpan.FromSeconds(60));
				}
				if (Blacklist.Contains(Me.CurrentTarget, BlacklistFlags.Node))
				{
					StyxWoW.Me.ClearTarget();
				}
			}
			//                     Handle Target End
        }
		
		private void HuntAll()
        {
            unitsToKill = new List<WoWUnit>();
            unitsToKill = GetKillableUnits();
            for (int i = 0; i < unitsToKill.Count(); i++)
            {
                if ((unitsToKill[i].Distance < 1000))
                {
                    unitsToKill[i].Target();
					StyxWoW.SleepForLagDuration();
                }
            }
        }
		
		private void HuntHumanoid()
        {
            humansToKill = new List<WoWUnit>();
            humansToKill = GetKillableHumanoid();
            for (int i = 0; i < humansToKill.Count(); i++)
            {
                if ((humansToKill[i].Distance < 1000))
                {
                    humansToKill[i].Target();
					StyxWoW.SleepForLagDuration();
                }
            }
        }
		
		private void HuntPockets()
        {
            pocketsToPlunder = new List<WoWUnit>();
            pocketsToPlunder = GetKillableHumanoid();
            for (int i = 0; i < humansToKill.Count(); i++)
            {
                if ((pocketsToPlunder[i].Distance < 1000))
                {
                    pocketsToPlunder[i].Target();
					StyxWoW.SleepForLagDuration();
                }
            }
        }
		
		private void HuntBeast()
        {
            beastsToKill = new List<WoWUnit>();
            beastsToKill = GetKillableBeasts();
            for (int i = 0; i < beastsToKill.Count(); i++)
            {
                if ((beastsToKill[i].Distance < 1000))
                {
                    beastsToKill[i].Target();
					StyxWoW.SleepForLagDuration();
                }
            }
        }
		
		private List<WoWUnit> GetKillableHumanoid()
        {
            KillableHumanoid = (ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                        .Where(unit =>
                        (!unit.IsDead || (unit.IsDead && unit.CanLoot)) &&
                        !unit.IsCritter &&
                        unit.IsHumanoid &&
						!unit.Elite &&
						!unit.IsNeutral &&
						!unit.TaggedByOther &&
                        !unit.IsFriendly &&
						!Blacklist.Contains(unit, BlacklistFlags.Pull))
                        .OrderBy(unit => unit.Distance)).ToList<WoWUnit>();
            List<WoWUnit> units = new List<WoWUnit>();

            if (KillableHumanoid != null && KillableHumanoid.Count >= 1)
            {
                for (int index = 0; index < 1; index++)
                {
                    units.Add(KillableHumanoid[index]);
                }
            }
            else if (KillableHumanoid != null)
            {
                for (int index = 0; index < KillableHumanoid.Count; index++)
                {
                    units.Add(KillableHumanoid[index]);
                }
            }
            return units;
        } 
		
		private List<WoWUnit> GetPickpocketableHumanoid()
        {
            PickpocketableHumanoid = (ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                        .Where(unit =>
                        (!unit.IsDead || (unit.IsDead && unit.CanLoot)) &&
                        !unit.IsCritter &&
                        unit.IsHumanoid &&
						!unit.Elite &&
						!unit.IsNeutral &&
						!unit.TaggedByOther &&
                        !unit.IsFriendly &&
						!Blacklist.Contains(unit, BlacklistFlags.Node))
                        .OrderBy(unit => unit.Distance)).ToList<WoWUnit>();
            List<WoWUnit> units = new List<WoWUnit>();

            if (PickpocketableHumanoid != null && PickpocketableHumanoid.Count >= 1)
            {
                for (int index = 0; index < 1; index++)
                {
                    units.Add(PickpocketableHumanoid[index]);
                }
            }
            else if (PickpocketableHumanoid != null)
            {
                for (int index = 0; index < PickpocketableHumanoid.Count; index++)
                {
                    units.Add(PickpocketableHumanoid[index]);
                }
            }
            return units;
        } 
		
		private List<WoWUnit> GetKillableBeasts()
        {
            KillableBeasts = (ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                        .Where(unit =>
                        (!unit.IsDead || (unit.IsDead && (unit.CanLoot || unit.CanSkin))) &&
                        !unit.IsCritter &&
                        unit.IsBeast &&
						unit.SkinType == WoWCreatureSkinType.Leather &&
						!unit.Elite &&
						!unit.TaggedByOther &&
                        !unit.IsFriendly &&
						!Blacklist.Contains(unit, BlacklistFlags.Pull))
                        .OrderBy(unit => unit.Distance)).ToList<WoWUnit>();
            List<WoWUnit> units = new List<WoWUnit>();

            if (KillableBeasts != null && KillableBeasts.Count >= 1)
            {
                for (int index = 0; index < 1; index++)
                {
                    units.Add(KillableBeasts[index]);
                }
            }
            else if (KillableBeasts != null)
            {
                for (int index = 0; index < KillableBeasts.Count; index++)
                {
                    units.Add(KillableBeasts[index]);
                }
            }
            return units;
        }

        private List<WoWUnit> GetKillableUnits()
        {
            KillableUnits = (ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                        .Where(unit =>
                        (!unit.IsDead || (unit.IsDead && (unit.CanLoot || unit.CanSkin))) &&
                        !unit.IsCritter &&
                        (unit.IsHumanoid || (unit.IsBeast && unit.SkinType == WoWCreatureSkinType.Leather)) &&
						!unit.Elite &&
						!unit.IsNeutral &&
						!unit.TaggedByOther &&
                        !unit.IsFriendly &&
						!Blacklist.Contains(unit, BlacklistFlags.Pull))
                        .OrderBy(unit => unit.Distance)).ToList<WoWUnit>();
            List<WoWUnit> units = new List<WoWUnit>();

            if (KillableUnits != null && KillableUnits.Count >= 1)
            {
                for (int index = 0; index < 1; index++)
                {
                    units.Add(KillableUnits[index]);
                }
            }
            else if (KillableUnits != null)
            {
                for (int index = 0; index < KillableUnits.Count; index++)
                {
                    units.Add(KillableUnits[index]);
                }
            }
            return units;
        }  
    }
}
