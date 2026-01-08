
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.AreaManagement;
using Styx.CommonBot.Database;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;

using Sequence = Styx.TreeSharp.Sequence;
using Action = Styx.TreeSharp.Action;
using CommonBehaviors.Actions;
using Levelbot.Actions.Combat;

namespace RAFPlugin
{
	public class RAFPlugin : HBPlugin
	{
		public static LocalPlayer Me { get { return StyxWoW.Me; } }
		public static bool Partied { get { return Me.GroupInfo.IsInParty; } }
		
		public uint partySize = 0;
		public int myLevel = 0;
		public int resetQuest = 0;
		public long withinRange = 0;
		public bool checkLevel = false;
		public Random rng = new Random();
		
		public WoWUnit LastQuestGiver = null;
		
		public override string Name{ get { return "RAF Plugin"; } }
		public override string Author{ get { return "Celisuis"; } }
		public override Version Version{ get { return new Version(1, 0, 0); } }
		public override bool WantButton{ get { return false; } }
		
		public void Initialize()
		{
			Logging.Write(Colors.Green, Name + "Loaded" + Version + "Enjoy");
			partySize = Me.GroupInfo.PartySize;
			myLevel = Me.Level;
			Logging.Write(Colors.Green, "Pary Size is" + partySize.ToString());
		}
		
		public override void Pulse()
		{
			try{
				if(!Partied)
				{
					Logging.Write(Colors.Orange, "We are not partied. Exiting Bot");
					TreeRoot.Stop(Name);
				}
				CheckPartyMemberLevel();
				QuestGiverTarget();
			}
			
			catch (Exception ex){
				Logging.WriteException(ex);
			}
		}
		
		private async void QuestGiverTarget()
		{
			if(partySize !=Me.GroupInfo.PartySize)
			{
				partySize = Me.GroupInfo.PartySize;
				Logging.Write(Colors.Green, "Pary Size is" + partySize.ToString());
			}
			
			if(Me.CurrentTarget !=null)
			{
				if (Me.CurrentTarget.QuestGiverStatus == QuestGiverStatus.TurnIn)
				{
					if(Me.CurrentTarget.WithinInteractRange)
					   {
					   	WoWMovement.MoveStop();
					   	if(PartyMemberNear())
					   	{
					   		if(LastQuestGiver !=Me.CurrentTarget || Me.CurrentTarget == null)
					   		{
					   			Logging.Write(Colors.Green, "All Party Members are within range.");
					   			Logging.Write(Colors.Blue, "Handing in Quest in 6 Seconds.");
					   			withinRange = 0;
					   			await Task.Delay(6000);
					   		}
					   		
					   		if(Me.CurrentTarget != null)
					   			LastQuestGiver = Me.CurrentTarget;
					   		return;
					   	}
					   	else
					   	{
					   		if(Styx.CommonBot.Frames.QuestFrame.Instance.IsVisible || Styx.CommonBot.Frames.GossipFrame.Instance.IsVisible){Styx.CommonBot.Frames.QuestFrame.Instance.Close();Styx.CommonBot.Frames.GossipFrame.Instance.Close();}
							return;
					   	}
					   }
				}
			}
			
			if(LastQuestGiver != null)
			{
				if(LastQuestGiver.WithinInteractRange)
				{
					if(LastQuestGiver.QuestGiverStatus == QuestGiverStatus.TurnIn)
					{
						LastQuestGiver.Target();
						QuestGiverTarget();
					}
					
					if(LastQuestGiver.QuestGiverStatus ==  QuestGiverStatus.Available || LastQuestGiver.QuestGiverStatus == QuestGiverStatus.Available || LastQuestGiver.QuestGiverStatus == QuestGiverStatus.AvailableRepeatable || LastQuestGiver.QuestGiverStatus == QuestGiverStatus.LowLevelAvailable || LastQuestGiver.QuestGiverStatus == QuestGiverStatus.None)
					{
						int waitTime = rng.Next(5, 9);
						Logging.Write(Colors.Green, "Waiting for:" + waitTime.ToString() + "seconds.");
						WoWMovement.MoveStop();
						Me.ClearTarget();
						await Task.Delay(waitTime*1000);
						
						if(Me.PartyMembers.Count(player=> player != null && player.IsValid && player.CurrentTarget == LastQuestGiver && LastQuestGiver != null) > 0 )
						{
							Logging.Write(Colors.Green, "Awaiting all party members to hand in quest.");
								resetQuest++;
							if(resetQuest == 2)
							{
								Logging.Write(Colors.Orange, "We have waited for 2 turns now, assuming loop. Taking Action");
								resetQuest = 1;
								LastQuestGiver = null;
								return;
							}
							QuestGiverTarget();
						}
					}
				}
				LastQuestGiver = null;
			}
			
			return;
		}
		
		private bool PartyMemberNear()
		{
			if (!Partied) return false;
			{
			int memberswithinRange = Me.PartyMembers.Count(player=> player != null && Me.Location.Distance(player.Location) <= 10);
			
			if(memberswithinRange == partySize && memberswithinRange != 0){
					return true;
			}
			else{
				if(withinRange != (partySize-memberswithinRange)){
					withinRange = (partySize-memberswithinRange);
					Logging.Write(Colors.Orange, Name + " - " + (partySize-memberswithinRange).ToString() + " player(s) are out of range.");
				}
				return false;
			}

		}
		}
		
		private void CheckPartyMemberLevel()
		{
			if(LastQuestGiver == null)
			{
				if(Me.Level == (myLevel+1))
				   {
					Logging.Write(Colors.Green, "We have leveled up. We are now level:" + myLevel + ", Checking if party is the same level.");
					myLevel = Me.Level;
					checkLevel = true;
				   }
				
				if(checkLevel)
				{
					if(Me.PartyMembers.Count(player=> player != null && player.IsValid && player.Level == Me.Level) == partySize && Me.Level >= 10)
					{
						Logging.Write(Colors.Green, "We are all the same level. Reloading the questing profile now.");
						ProfileManager.LoadNew(ProfileManager.XmlLocation,true);
						checkLevel = false;
					}
				}
			}
			return;
		}
		
		
		
	}
}