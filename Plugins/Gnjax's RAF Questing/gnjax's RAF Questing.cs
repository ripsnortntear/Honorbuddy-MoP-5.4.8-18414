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

namespace RAFQuesting
{
	public class RAFQuesting : HBPlugin
	{
		public static LocalPlayer Me { get { return StyxWoW.Me; } }
		public static bool Partied { get { return Me.GroupInfo.IsInParty; } }
		public uint partySize = 0;
		public string currentProfile;
		public bool   waiting;
		public WoWUnit lastQuestGiver = null;

		public override string Name{ get { return "Gnjax's RAF Questing"; } }
		public override string Author{ get { return "Gnjax"; } }
		public override Version Version{ get { return new Version(1, 0, 0); } }
		public override bool WantButton{ get { return false; } }
	
		public void Initialize()
		{
			Logging.Write(Colors.Green, Name + "Loaded");
			partySize = Me.GroupInfo.PartySize;
		}

		public override void Pulse()
		{
			try
			{
				if(!Partied)
				{
					Logging.Write(Colors.Orange, "[RAF Questing] - You need to be in a party to run this plugin. Stopping bot.");
					TreeRoot.Stop(Name);
				}
				ShouldIWaitOrShouldIGo();
			}
			catch (Exception ex)
			{
				Logging.WriteException(ex);
			}
		}

		private async void ShouldIWaitOrShouldIGo()
		{
			if(partySize != Me.GroupInfo.PartySize)
			{
				partySize = Me.GroupInfo.PartySize;
				Logging.Write(Colors.Green, "[RAF Questing] - Party size changed to " + partySize.ToString());
			}
			
			if(Me.CurrentTarget != null && Me.CurrentTarget.QuestGiverStatus == QuestGiverStatus.TurnIn)
			{
				
				if(Me.CurrentTarget.WithinInteractRange)
				{
					WoWMovement.MoveStop();
					if (PartyMemberNear(20))
					{
						if (waiting && PartyMemberNear(4))
						{
							ProfileManager.LoadNew(currentProfile);
							waiting = false;
							Logging.Write(Colors.Green, "[RAF Questing] - Everyone's there, resuming questing");
						}
						lastQuestGiver = Me.CurrentTarget;
					}
					else if (!waiting)
					{
						currentProfile = ProfileManager.XmlLocation;
						ProfileManager.LoadEmpty();
						waiting = true;
						Logging.Write(Colors.Green, "[RAF Questing] - Waiting for other members");
					}
				}
			}

			if (lastQuestGiver != null)
			{
				if (lastQuestGiver.QuestGiverStatus == QuestGiverStatus.Unavailable)
				{
					currentProfile = ProfileManager.XmlLocation;
					ProfileManager.LoadEmpty();
					waiting = true;
					Logging.Write(Colors.Green, "[RAF Questing] - Quest giver is unavailable, pausing");
				}
				else
				{
					if (waiting)
					{
						ProfileManager.LoadNew(currentProfile);
						waiting = false;
						Logging.Write(Colors.Green, "[RAF Questing] - Quest giver is available agin, resuming questing");
					}
					lastQuestGiver = null;
				}
			}
		}
		
		private bool PartyMemberNear(int distance)
		{
			if (!Partied) 
				return false;
			int memberswithinRange = Me.PartyMembers.Count(player=> player != null && Me.Location.Distance(player.Location) <= distance);
			if(memberswithinRange == partySize && memberswithinRange != 0)
				return true;
			return false;
		}

	}
}