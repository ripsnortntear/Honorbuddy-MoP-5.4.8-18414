using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Styx;
using Styx.Helpers;
using Styx.Loaders;
using Styx.Patchables;
using Styx.Plugins;
using Styx.Common;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.CommonBot.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;

using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;

using BrodieHPlugin;
using BrodieHProfile;
using BrodieHQB;
using BarryDurex;
using KatzerleAvoid;
using BrodieMobList;

using ObjectManager = Styx.WoWInternals.ObjectManager;

namespace BrodiesPlugin
{
	public class BrodiesPlugin : HBPlugin
	{
		public bool hasBeenInitialized = false;
		public static bool hasBeenInitialized4 = false;
		public bool brodieupdated = false;
		
		#region Overrides except pulse
		public override string Author { get { return "TheBrodieMan"; } }
		public override Version Version { get { return new Version(2, 0, 4); } }
		public override string Name { get { return "TheBrodieMan's Compendium (Premium)"; } }
		public override bool WantButton { get { return true; } }
		public override string ButtonText { get { return "TBM Interface"; } }
		public override void OnButtonPress()
		{
			bool isRunning = TreeRoot.IsRunning;
			if (isRunning)
			{
				MessageBox.Show("Bot is running, stop bot before initiating Brodies Plugin", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else
				abreJanela();
		}
		private Brodie_Plugin_Updater UpdaterPlugin;
		private Brodie_QB_Updater UpdaterQB;
		private Brodie_Profile_Updater UpdaterProfile;
		
		private static LocalPlayer Me { get { return StyxWoW.Me; } }

		public override void Initialize()
		{
			if (!hasBeenInitialized)
			{
				Logging.Write(Colors.Teal, "Loaded TheBrodieMan's Compendium (Premium Version) v" + Version.ToString());
				Logging.Write(Colors.Teal, "Please Wait While [Brodies Plugin] Checks For Updates, This MAY Take Several Minutes");
				hasBeenInitialized = true;
				try
				{
					UpdaterPlugin = new Brodie_Plugin_Updater("https://tbmppluginplus.googlecode.com/svn/trunk/", "BrodiesPlugin");
					if (UpdaterPlugin.UpdateAvailable())
					{
						Logging.Write("[Brodies Plugin] Update to $" + UpdaterPlugin.GetNewestRev().ToString() + " is available! You are on $" + UpdaterPlugin.CurrentRev.ToString());
						Logging.Write("[Brodies Plugin] Starting update process...");
						if (UpdaterPlugin.Update())
						{
							Logging.Write("[Brodies Plugin] Plugin is now up to date! HB Reload REQUIRED");
							brodieupdated = true;
						}
						else
							Logging.Write("[Brodies Plugin] Error trying to auto-update. Please update manually!");
					}
					else
						Logging.Write("[Brodies Plugin] is at Rev $" + UpdaterPlugin.CurrentRev.ToString() + " and is up to date!");
				}
				catch (Exception ex)
				{
					Logging.Write(Colors.Teal, "Unable to run [Brodies Plugin] update process");
					Logging.Write(LogLevel.Diagnostic, "[Brodies Plugin]: Exception " + ex.Message);
				}
				try
				{
					UpdaterQB = new Brodie_QB_Updater("https://tbmpqb.googlecode.com/svn/trunk/", "");
					if (UpdaterQB.UpdateAvailable())
					{
						Logging.Write("[Brodies QB Updater] Update to $" + UpdaterQB.GetNewestRev().ToString() + " is available! You are on $" + UpdaterQB.CurrentRev.ToString());
						Logging.Write("[Brodies QB Updater] Starting update process...");
						if (UpdaterQB.Update())
						{
							Logging.Write("[Brodies QB Updater] QBs are now up to date! HB Reload REQUIRED");
							brodieupdated = true;
						}
						else
							Logging.Write("[Brodies QB Updater] Error trying to auto-update. Please update manually!");
					}
					else
						Logging.Write("[Brodies QB Updater] is at Rev $" + UpdaterQB.CurrentRev.ToString() + " and is up to date!");
				}
				catch (Exception ex)
				{
					Logging.Write(Colors.Teal, "Unable to run [Brodies QB Updater] update process");
					Logging.Write(LogLevel.Diagnostic, "[Brodies QB Updater]: Exception " + ex.Message);
				}
				try
				{
					UpdaterProfile = new Brodie_Profile_Updater("https://tbmpprofilesplus.googlecode.com/svn/trunk/", "");
					if (UpdaterProfile.UpdateAvailable())
					{
						Logging.Write("[Brodies Profile Updater] Update to $" + UpdaterProfile.GetNewestRev().ToString() + " is available! You are on $" + UpdaterProfile.CurrentRev.ToString());
						Logging.Write("[Brodies Profile Updater] Starting update process...");
						if (UpdaterProfile.Update())
							Logging.Write("[Brodies Profile Updater] Profiles are now up to date!");
						else
							Logging.Write("[Brodies Profile Updater] Error trying to auto-update. Please update manually!");
					}
					else
						Logging.Write("[Brodies Profile Updater] is at Rev $" + UpdaterProfile.CurrentRev.ToString() + " and is up to date!");
				}
				catch (Exception ex)
				{
					Logging.Write(Colors.Teal,"Unable to run [Brodies Profile Updater] update process");
					Logging.Write(LogLevel.Diagnostic, "[Brodies Profile Updater]: Exception " + ex.Message);
				}
				BrodiesPluginUI.BPGlobalSettings.Instance.Load();
				BrodiesPluginUI.BPSettings.Instance.Load();
			}
		}

		public override void Dispose()
		{
			Logging.Write(Colors.Teal, "[Brodies Plugin] Deactivation Complete");
		}
		
		#region Some Quest Helper Functions

		public bool IsOnQuest(uint questId)
		{
			return Me.QuestLog.GetQuestById(questId) != null && !Me.QuestLog.GetQuestById(questId).IsCompleted;
		}
		
		public bool HasQuest(uint questId)
		{
			return Me.QuestLog.GetQuestById(questId) != null;
		}

		public bool QuestComplete(uint questId)
		{
			return Me.QuestLog.GetQuestById(questId).IsCompleted;
		}

		public bool QuestFailed(uint questId)
		{
			return Me.QuestLog.GetQuestById(questId).IsFailed;
		}

		public bool QuestObjectiveComplete(uint questId, int objectiveNum)
		{
			return (Lua.GetReturnVal<int>("a,b,c=GetQuestLogLeaderBoard(" + objectiveNum + ",GetQuestLogIndexByID(" + questId + "));if c==1 then return 1 else return 0 end", 0) == 1);
		}

		private bool IsObjectiveComplete(int objectiveId, uint questId)
		{
			if (Me.QuestLog.GetQuestById(questId) == null)
				return false;
				
			int returnVal = Lua.GetReturnVal<int>("return GetQuestLogIndexByID(" + questId + ")", 0);
			return
				Lua.GetReturnVal<bool>(
					string.Concat(new object[] { "return GetQuestLogLeaderBoard(", objectiveId, ",", returnVal, ")" }), 2);
		}

		public bool ItemOnCooldown(uint ItemId)
		{
			return Lua.GetReturnVal<bool>("GetItemCooldown(" + ItemId + ")", 0);
		}

		public void UseIfNotOnCooldown(uint ItemId)
		{
			if (!ItemOnCooldown(ItemId) && !Me.IsFlying)
				Lua.DoString("UseItemByName(" + ItemId + ")");
		}

		public bool TargetingNpc(uint npcId)
		{
			return Me.CurrentTarget.Entry == npcId;
		}

		#endregion

		#endregion

		#region Privates/Publics
		private void abreJanela()
		{
			if (brodieupdated)
				MessageBox.Show("Brodies Plugin/Quest Behaviors has been updated a restart is required.", "RESTART REQUIRED", MessageBoxButtons.OK, MessageBoxIcon.Information);
				
			var mainBrodiePluginUI = new BrodiesPluginUI();
			mainBrodiePluginUI.ShowDialog();
		}

		#endregion

		#region Override Pulse
		public override void Pulse()
		{
			if (Me == null || !StyxWoW.IsInGame)
				return;
			if (Me.IsDead || Me.IsGhost)
				return;
			if (Battlegrounds.IsInsideBattleground || Me.IsInInstance || Me.IsOnTransport)
				return;
			
			#region PROFILE SWAPPER CODE
			if (BrodiesPluginUI.BPSettings.Instance.Active13)
			{				
				if (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 ||
											StyxWoW.Me.CastingSpellId == 94719 ||
											StyxWoW.Me.CastingSpellId == 136508 ||
											StyxWoW.Me.CastingSpellId == 75136 ||
											StyxWoW.Me.CastingSpellId == 82674))
				{
					Logging.Write(Colors.LightSkyBlue, "[Brodies Plugin] Profile Swapper: Checking to see if we should change profiles...");
					SpellManager.StopCasting();
					string useProfile = DetermineActiveProfile();
					if (useProfile != null)
						ChangeProfile(useProfile);
				}
			}
			#endregion
			
			#region Eastern Kingdoms Questing
			
			if (IsOnQuest(28538))
			{
				if (Me.Combat && BrodieMobList.BMobList.SIAgents != null && (TargetingNpc(48741) || TargetingNpc(48742)))
				{
					if (Me.CurrentTarget.HealthPercent <= 35)
					{
						SpellManager.StopCasting();
						UseIfNotOnCooldown(64445); // Banshee Mirror
					}
				}
			}
			
			if (BrodieMobList.BMobList.HiddenStash != null) // Use Hidden Stash, Eastern Plaguelands
				UseIfNotOnCooldown(61387);
				
			if (IsOnQuest(28138)) // Human Infestation - Hillsbrad Foothills
			{
				if (Me.Combat && BrodieMobList.BMobList.HillsbradHumans != null && (TargetingNpc(2503) || TargetingNpc(2270) || TargetingNpc(2269)))
				{
					if (Me.CurrentTarget.HealthPercent <= 35)
					{
						SpellManager.StopCasting();
						UseIfNotOnCooldown(63079); // Titanium Shackles
					}
				}
			}
			
			#endregion
			
			#region Kalimdor Questing
			
			if (IsOnQuest(25057))
			{
				if (Me.Combat && BrodieMobList.BMobList.TaurajoLooter != null && (TargetingNpc(37743)))
				{
					if (Me.CurrentTarget.HealthPercent <= 35)
						UseIfNotOnCooldown(52271); // Northwatch Manacles
				}
			}
			
			if (BrodieMobList.BMobList.OozingBag != null) // Use Oozing Bag, Kalimdor - Silithus
				UseIfNotOnCooldown(20768);
			
			#endregion

			#region [Operation Shieldwall/Dominance Offensive]

			#region Jungle Shredder
			//http://www.wowhead.com/quest=32446
			if (IsOnQuest(32446))
			{
				if (Me.Combat && BrodieMobList.BMobList.JungleShredder != null && !BrodieMobList.BMobList.JungleShredder.HasAura(135422) && BrodieMobList.BMobList.JungleShredder.Distance2D <= 10)
					UseIfNotOnCooldown(93180); //Re-Configured Remote
			}
			#endregion
			
			#region Mecha-Pounder
			//http://www.wowhead.com/quest=32238
			if (IsOnQuest(32238))
			{
				if (Me.Combat && BrodieMobList.BMobList.MechaPounder != null && !BrodieMobList.BMobList.MechaPounder.HasAura(133955) && BrodieMobList.BMobList.MechaPounder.Distance2D <= 10)
					UseIfNotOnCooldown(91902); //Universal remote
			}
			#endregion

			#region Shredmaster Packle
			//http://www.wowhead.com/quest=32158
			if (BrodieMobList.BMobList.ShredmasterP != null)
			{
				if (Me.Combat && BrodieMobList.BMobList.ShredmasterP.Distance2D <= 10 && BrodieMobList.BMobList.ShredmasterP.IsCasting && BrodieMobList.BMobList.ShredmasterP.CastingSpellId == 135865)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.ShredmasterP, 80, 10);
			}
			#endregion
			#endregion
			
			#region [Cloud Serpent]
			
			#region http://www.wowhead.com/quest=31717
			if (Me.HasAura("Hot Foot!"))
				BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getHotFootList, "Hot Foot!", 15);
			#endregion
			
			#region http://www.wowhead.com/quest=32158
			if (Me.HasAura("Ignite Fuel"))
				BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getIgniteFuelList, "Ignite Fuel", 15);			
			#endregion
			
			#region http://www.wowhead.com/quest=31717
			if (Me.HasAura("Solar Beam"))
				BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getSolarBeamList, "Solar Beam", 15);
			#endregion
			#endregion

			#region [Golden Lotus]

			if (Me.HasAura("Lightning Pool"))
				BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getLightningPoolList, "Lightning Pool", 15);
			
			if (Me.HasAura("Caustic Pitch"))
				BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getCausticPitchList, "Caustic Pitch", 15);

			if (Me.HasAura("Venom Splash") && BarryDurex.QuestHelper.getVenomSplashList != null && BarryDurex.QuestHelper.getVenomSplashList[0].Distance < (BarryDurex.QuestHelper.getVenomSplashList[0].Radius * 1.6f))
				BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getVenomSplashList, "Venom Splash", 15);

			if (BrodieMobList.BMobList.Sydow != null && BrodieMobList.BMobList.Sydow.CastingSpellId == 126347)
				BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Sydow, 80, 15);

			if (BrodieMobList.BMobList.Cracklefang != null && BrodieMobList.BMobList.Cracklefang.Distance2D <= 20 && BrodieMobList.BMobList.Cracklefang.IsCasting)
				BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Cracklefang, 90, 20);

			if (BrodieMobList.BMobList.Krichon != null && BrodieMobList.BMobList.Krichon.IsCasting && !Me.IsBehind(BrodieMobList.BMobList.Krichon))
				BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Krichon, 80, 15);
			
			if (BrodieMobList.BMobList.ShaoTienSorcerer != null)
			{
				if (Me.Combat && BrodieMobList.BMobList.ShaoTienSorcerer.Distance2D <= 10 && BrodieMobList.BMobList.ShaoTienSorcerer.IsCasting && BrodieMobList.BMobList.ShaoTienSorcerer.CastingSpellId == 127552)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.ShaoTienSorcerer, 80, 15);
			}
			
			if ((IsOnQuest(30242) || IsOnQuest(30240)) && Me.Location.Distance(new WoWPoint(802.4619, 2000.587, 318.2415)) <= 50)
			{
				Navigator.NavigationProvider.StuckHandler.Reset();
			}
			
			#endregion

			#region [The August Celestials]
			// http://www.wowhead.com/quest=30952 http://www.wowhead.com/quest=30953 http://www.wowhead.com/quest=30954 http://www.wowhead.com/quest=30955
			// http://www.wowhead.com/quest=30956 http://www.wowhead.com/quest=30957 http://www.wowhead.com/quest=30958 http://www.wowhead.com/quest=30959
			if (IsOnQuest(30952) || IsOnQuest(30953) || IsOnQuest(30954) || IsOnQuest(30955) || IsOnQuest(30956) || IsOnQuest(30957) || IsOnQuest(30958) || IsOnQuest(30959))
			{
				if (Me.Combat && Me.IsMoving)
				{
					if (BrodieMobList.BMobList.MantidNiuzao != null)
					{
						BrodieMobList.BMobList.MantidNiuzao[0].Face();
						BrodieMobList.BMobList.MantidNiuzao[0].Target();
						WoWMovement.MoveStop();
					}
				}
			}
			#endregion

			#region [Klaxxi]
			// http://www.wowhead.com/quest=31487
			if (IsOnQuest(31487))
			{
				if (Me.Combat && BrodieMobList.BMobList.DreadKunchong != null && BrodieMobList.BMobList.DreadKunchong.Distance2D <= 10)
					UseIfNotOnCooldown(87394); //Sonic Disruption Fork
			}
			if (BrodieMobList.BMobList.DreadKunchong != null)
			{
				if (Me.Combat && BrodieMobList.BMobList.DreadKunchong.Distance2D <= 15 && BrodieMobList.BMobList.DreadKunchong.IsCasting && BrodieMobList.BMobList.DreadKunchong.CastingSpellId == 128022)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.DreadKunchong, 80, 15);
			}
			
			// http://www.wowhead.com/quest=31268
			if (IsOnQuest(31268))
			{
				if (Me.Combat && BrodieMobList.BMobList.AScorpion != null && BrodieMobList.BMobList.AScorpion.Distance2D <= 10)
					UseIfNotOnCooldown(85884); //Sonic Emitter
			}
			
			// http://www.wowhead.com/quest=31507
			if (IsOnQuest(31507))
			{
				if (Me.Combat && BrodieMobList.BMobList.Amberhusk != null && BrodieMobList.BMobList.Amberhusk.Distance2D <= 15)
				{
					UseIfNotOnCooldown(87841);
					SpellManager.ClickRemoteLocation(BrodieMobList.BMobList.Amberhusk.Location);
				}
			}
			#endregion
			
			#region Warbringer and Scouts
			
			if (BrodieMobList.BMobList.WarbringerScout != null)
			{
				if (Me.Combat && BrodieMobList.BMobList.WarbringerScout.Distance2D <= 20 && BrodieMobList.BMobList.WarbringerScout.IsCasting && BrodieMobList.BMobList.WarbringerScout.CastingSpellId == 138044)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.WarbringerScout, 120, 20);
			}	

			if (BrodieMobList.BMobList.VengefulSpirit != null)
			{
				if (Me.Combat && BrodieMobList.BMobList.VengefulSpirit.Distance2D <= 15)
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.VengefulSpirit, 0, 20, 10, 5);
			}
			
			#endregion

			#region Isle of Thunder
			
			if (Me.Combat && BrodieMobList.BMobList.ZColossus != null)
			{
				if (BrodieMobList.BMobList.ZColossus.Distance2D <= 15 && BrodieMobList.BMobList.ZColossus.IsCasting && BrodieMobList.BMobList.ZColossus.CastingSpellId == 140239)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.ZColossus, 80, 15);
				if (BrodieMobList.BMobList.ZColossus.Distance2D <= 15 && BrodieMobList.BMobList.ZColossus.IsCasting && BrodieMobList.BMobList.ZColossus.CastingSpellId == 140254)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.ZColossus, 80, 15);
			}
			
			if (BrodieMobList.BMobList.EnergizedMetal.Count > 0)
			{
				if (BrodieMobList.BMobList.EnergizedMetal[0].Location.Distance(Me.Location) <= 15)
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.EnergizedMetal[0], 0, 15, 10, 5);
			}
			
			if (BrodieMobList.BMobList.BallLightning.Count > 0)
			{
				if (BrodieMobList.BMobList.BallLightning[0].Location.Distance(Me.Location) <= 15)
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.BallLightning[0], 0, 15, 10, 5);
			}
				
			if (BrodieMobList.BMobList.MLMonoHan != null && Me.Combat)
			{
				if (BrodieMobList.BMobList.MLMonoHan.Distance2D <= 50 && ((BrodieMobList.BMobList.MLMonoHan.IsCasting && BrodieMobList.BMobList.MLMonoHan.CastingSpellId == 136906) || BrodieMobList.BMobList.MLMonoHan.HasAura(136906)))
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.MLMonoHan, 0, 30, 10, 5);
			}
			
			if (BrodieMobList.BMobList.Itoka != null && Me.Combat)
			{
				if (BrodieMobList.BMobList.Itoka.Distance2D <= 50 && BrodieMobList.BMobList.Itoka.IsCasting && BrodieMobList.BMobList.Itoka.CastingSpellId == 137142)
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.Itoka, 0, 25, 10, 5);
				if (BrodieMobList.BMobList.Itoka.Distance2D <= 30 && BrodieMobList.BMobList.Itoka.IsCasting && BrodieMobList.BMobList.Itoka.CastingSpellId == 137132)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Itoka, 150, 30);
			}
			
			if (BrodieMobList.BMobList.FCHoku != null && Me.Combat)
			{
				if (BrodieMobList.BMobList.FCHoku.Distance2D <= 30 && BrodieMobList.BMobList.FCHoku.IsCasting && BrodieMobList.BMobList.FCHoku.CastingSpellId == 140526)
					using (StyxWoW.Memory.ReleaseFrame(true))
					{
						while (!Me.IsSafelyBehind(Me.CurrentTarget))
						{
							WoWMovement.Move(WoWMovement.MovementDirection.StrafeRight);
							Me.CurrentTarget.Face();
						}
					}
					WoWMovement.MoveStop();
			}
			
			if (BrodieMobList.BMobList.MDevilsaur != null && Me.Combat)
			{
				if (BrodieMobList.BMobList.VileSpit[0].Location.Distance(Me.Location) <= 15)
					WoWMovement.Move(WoWMovement.MovementDirection.StrafeRight, TimeSpan.FromSeconds(1));
					
				if (BrodieMobList.BMobList.MDevilsaur.Distance2D <= 50 && BrodieMobList.BMobList.MDevilsaur.IsCasting && BrodieMobList.BMobList.MDevilsaur.CastingSpellId == 140424)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.MDevilsaur, 150, 50);
				if (BrodieMobList.BMobList.MDevilsaur.Distance2D <= 50 && BrodieMobList.BMobList.MDevilsaur.IsCasting && BrodieMobList.BMobList.MDevilsaur.CastingSpellId == 140427)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.MDevilsaur, 150, 50);
				if (BrodieMobList.BMobList.MDevilsaur.Distance2D <= 100 && BrodieMobList.BMobList.MDevilsaur.IsCasting && (BrodieMobList.BMobList.MDevilsaur.CastingSpellId == 140397 || BrodieMobList.BMobList.MDevilsaur.CastingSpellId == 140407 || BrodieMobList.BMobList.MDevilsaur.CastingSpellId == 140406 || BrodieMobList.BMobList.MDevilsaur.CastingSpellId == 140405))
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.MDevilsaur, 120, 100);
			}
			
			#endregion
			
			#region Isle of Giants
			if (BrodieMobList.BMobList.PrimalDirehorn != null)
			{
				if (Me.Combat && BrodieMobList.BMobList.PrimalDirehorn.Distance2D <= 15 && BrodieMobList.BMobList.PrimalDirehorn.IsCasting && BrodieMobList.BMobList.PrimalDirehorn.CastingSpellId == 138772)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.PrimalDirehorn, 90, 15);
			}			
			#endregion
			
			#region Timeless Isle
			
			if (BrodieMobList.BMobList.CrystalOfInsanity != null && !Me.HasAura(127230))
				UseIfNotOnCooldown(86569);
			
			if (Me.ZoneId == 6757)
			{
				if (BrodieMobList.BMobList.SingingCrystal != null && !Me.HasAura(147055))
					UseIfNotOnCooldown(103641);
				
				if (BrodieMobList.BMobList.BookOfTheAges != null && !Me.HasAura(147226))
					UseIfNotOnCooldown(103642);
				
				if (BrodieMobList.BMobList.DewOfEternalMorning != null && !Me.HasAura(147476))
					UseIfNotOnCooldown(103643);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.TimelessBirds != null)
			{
				// Gust of Wind
				if (BrodieMobList.BMobList.TimelessBirds.Distance2D <= 15 && BrodieMobList.BMobList.TimelessBirds.IsCasting && BrodieMobList.BMobList.TimelessBirds.CastingSpellId == 147310)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessBirds, 120, 15);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.TimelessTurtle != null)
			{
				// Geyser (Elder Great Turtle only)
				if (BrodieMobList.BMobList.TimelessTurtle.Distance2D <= 15 && BrodieMobList.BMobList.TimelessTurtle.IsCasting && BrodieMobList.BMobList.TimelessTurtle.CastingSpellId == 147573)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessTurtle, 90, 15);
					
				// Shell Spin (version 1)
				if (BrodieMobList.BMobList.TimelessTurtle.Location.Distance(Me.Location) <= 15 && (BrodieMobList.BMobList.TimelessTurtle.CastingSpellId == 147590 || BrodieMobList.BMobList.TimelessTurtle.HasAura(147590)))
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.TimelessTurtle, 147590, 20, 10, 5);
				
				// Shell Spin (version 2)
				if (BrodieMobList.BMobList.TimelessTurtle.Location.Distance(Me.Location) <= 15 && (BrodieMobList.BMobList.TimelessTurtle.CastingSpellId == 147571 || BrodieMobList.BMobList.TimelessTurtle.HasAura(147571)))
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.TimelessTurtle, 147571, 20, 10, 5);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.TimelessOxen != null)
			{
				// Headbutt (Version 1)
				if (BrodieMobList.BMobList.TimelessOxen.Distance2D <= 20 && BrodieMobList.BMobList.TimelessOxen.IsCasting && BrodieMobList.BMobList.TimelessOxen.CastingSpellId == 147382)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessOxen, 120, 20);

				// Headbutt (Version 2)
				if (BrodieMobList.BMobList.TimelessOxen.Distance2D <= 20 && BrodieMobList.BMobList.TimelessOxen.IsCasting && BrodieMobList.BMobList.TimelessOxen.CastingSpellId == 147384)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessOxen, 120, 20);
					
				// Ox Charge
				if (BrodieMobList.BMobList.TimelessOxen.Distance2D <= 30 && BrodieMobList.BMobList.TimelessOxen.IsCasting && (BrodieMobList.BMobList.TimelessOxen.CastingSpellId == 147386 || BrodieMobList.BMobList.TimelessOxen.CastingSpellId == 147385))
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessOxen, 120, 30);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.TimelessTiger != null)
			{
				// Rending Swipe
				if (BrodieMobList.BMobList.TimelessTiger.Distance2D <= 15 && BrodieMobList.BMobList.TimelessTiger.IsCasting && BrodieMobList.BMobList.TimelessTiger.CastingSpellId == 147646)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessTiger, 120, 15);
					
				// Greater Swipe
				if (BrodieMobList.BMobList.TimelessTiger.Distance2D <= 15 && BrodieMobList.BMobList.TimelessTiger.IsCasting && BrodieMobList.BMobList.TimelessTiger.CastingSpellId == 147652)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.TimelessTiger, 150, 15);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.TimelessRock != null)
			{
				// Stomp (Version 1)
				if (BrodieMobList.BMobList.TimelessRock.Location.Distance(Me.Location) <= 15 && (BrodieMobList.BMobList.TimelessRock.CastingSpellId == 147500 || BrodieMobList.BMobList.TimelessRock.HasAura(147500)))
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.TimelessRock, 147500, 40, 10, 5);
				
				// Stomp (Version 2)
				if (BrodieMobList.BMobList.TimelessRock.Location.Distance(Me.Location) <= 15 && (BrodieMobList.BMobList.TimelessRock.CastingSpellId == 147512 || BrodieMobList.BMobList.TimelessRock.HasAura(147512)))
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.TimelessRock, 147512, 40, 10, 5);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.SpectralPanda != null)
			{
				// Spinning Crane Kick
				if (BrodieMobList.BMobList.SpectralPanda.Location.Distance(Me.Location) <= 15 && (BrodieMobList.BMobList.SpectralPanda.CastingSpellId == 148730 || BrodieMobList.BMobList.SpectralPanda.HasAura(148730)))
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.SpectralPanda, 148730, 30, 10, 5);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.Spineclaw != null)
			{
				// Claw Flurry
				if (BrodieMobList.BMobList.Spineclaw.Distance2D <= 15 && BrodieMobList.BMobList.Spineclaw.IsCasting && BrodieMobList.BMobList.Spineclaw.CastingSpellId == 147557)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Spineclaw, 120, 15);
			}
			
			if (Me.Combat && BrodieMobList.BMobList.Ordon != null)
			{
				// Breath of Fire
				if (BrodieMobList.BMobList.Ordon.Distance2D <= 15 && BrodieMobList.BMobList.Ordon.IsCasting && BrodieMobList.BMobList.Ordon.CastingSpellId == 147416)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Ordon, 120, 15);

				// Crackling Blow
				if (BrodieMobList.BMobList.Ordon.Distance2D <= 15 && BrodieMobList.BMobList.Ordon.IsCasting && BrodieMobList.BMobList.Ordon.CastingSpellId == 147674)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Ordon, 120, 15);
				
				// Blazing Blow
				if (BrodieMobList.BMobList.Ordon.Distance2D <= 25 && BrodieMobList.BMobList.Ordon.IsCasting && BrodieMobList.BMobList.Ordon.CastingSpellId == 148003)
					BarryDurex.QuestHelper.AvoidEnemyCast(BrodieMobList.BMobList.Ordon, 120, 25);

				// Burning Sacrifice
				if (BrodieMobList.BMobList.Ordon.Distance2D <= 15 && BrodieMobList.BMobList.Ordon.IsCasting && BrodieMobList.BMobList.Ordon.CastingSpellId == 147422)
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.Ordon, 147422, 30, 10, 5);
				
				// Blazing Cleave
				if (BrodieMobList.BMobList.Ordon.Distance2D <= 15 && BrodieMobList.BMobList.Ordon.IsCasting && BrodieMobList.BMobList.Ordon.CastingSpellId == 147702)
					KatzerleAvoid.RunAwayCode.FleeingFromEnemy(BrodieMobList.BMobList.Ordon, 147702, 30, 10, 5);
					
				// Defensive Shield
				if (Me.CurrentTarget != null && Me.CurrentTarget.HasAura(147689) && !Me.IsSafelyBehind(Me.CurrentTarget) && Me.Location.Distance(Me.CurrentTarget.Location) <= 10)
				{
					using (StyxWoW.Memory.ReleaseFrame(true))
					{
						while (!Me.IsSafelyBehind(Me.CurrentTarget))
						{
							WoWMovement.Move(WoWMovement.MovementDirection.StrafeRight);
							Me.CurrentTarget.Face();
						}
					}
					WoWMovement.MoveStop();
				}
			}
			#endregion
		}
		#endregion
		
		public static void ChangeProfile(string Profile)
		{
			Logging.Write(Colors.LightSkyBlue, "[Brodies Plugin] Profile Swapper: Load Profile: {0}", Profile);
			WoWMovement.MoveStop();
			ProfileManager.LoadNew(Profile);
			Logging.Write(Colors.LightSkyBlue, "[Brodies Plugin] Profile Swapper: Load Profile: {0} done", Profile);
		}
		
		public static string DetermineActiveProfile()
		{
			if (BrodiesPluginUI.BPSettings.Instance.Active1)
			{
				BrodiesPluginUI.BPSettings.Instance.Active1 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile1;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active2)
			{
				BrodiesPluginUI.BPSettings.Instance.Active2 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile2;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active3)
			{
				BrodiesPluginUI.BPSettings.Instance.Active3 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile3;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active4)
			{
				BrodiesPluginUI.BPSettings.Instance.Active4 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile4;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active5)
			{
				BrodiesPluginUI.BPSettings.Instance.Active5 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile5;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active6)
			{
				BrodiesPluginUI.BPSettings.Instance.Active6 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile6;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active7)
			{
				BrodiesPluginUI.BPSettings.Instance.Active7 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile7;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active8)
			{
				BrodiesPluginUI.BPSettings.Instance.Active8 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile8;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active9)
			{
				BrodiesPluginUI.BPSettings.Instance.Active9 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile9;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active10)
			{
				BrodiesPluginUI.BPSettings.Instance.Active10 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile10;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active11)
			{
				BrodiesPluginUI.BPSettings.Instance.Active11 = false;
				return BrodiesPluginUI.BPSettings.Instance.Profile11;
			}
			else if (BrodiesPluginUI.BPSettings.Instance.Active12)
			{
				BrodiesPluginUI.BPSettings.Instance.Active12 = false;
				Logging.WriteDiagnostic(Colors.LightSkyBlue, "[Brodies Plugin] Profile Swapper has run out of profiles to use. Thank you come again!");
				return BrodiesPluginUI.BPSettings.Instance.Profile12;
			}
			else
			{
				SpellManager.StopCasting();
				Logging.Write(Colors.LightSkyBlue, "[Brodies Plugin] Profile Swapper: No profiles left by selection. Halting bot.");
				TreeRoot.Stop("Bot stop requested by Brodies Plugin: Lack of continuing profile selection.");
				return null;
			}
		}
		
		static public string FolderPath
		{
			get
			{
				string sPath = Process.GetCurrentProcess().MainModule.FileName;
				sPath = Path.GetDirectoryName(sPath);
				sPath = Path.Combine(sPath, "Plugins\\BrodiesPlugin\\");
				return sPath;
			}
		}
		
		static public bool InPetCombat()
		{
			List<string> cnt = Lua.GetReturnValues("dummy,reason=C_PetBattles.IsTrapAvailable() return dummy,reason");

			if (cnt != null) { if (cnt[1] != "0") return true; }
			return false;
		}
	}
}
