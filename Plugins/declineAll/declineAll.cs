
namespace declineAll
{
    using System.Drawing;

    using System;
 

    using System.Diagnostics;
    using Styx.WoWInternals;

    using Styx;
    using Styx.Plugins;
    using Styx.Helpers;
    using Styx.Common;
    using System.Windows.Media;

    public class declineAll : HBPlugin
    {
		public int _MinTime = 2000; //minimum and
		public int _MaxTime = 5000; //maximum time before decline

		private Stopwatch _Timer1 = new Stopwatch();
		private Stopwatch _Timer2 = new Stopwatch();
		private Stopwatch _Timer3 = new Stopwatch();
		private Stopwatch _Timer4 = new Stopwatch();
		public bool _GuildInvited = false;
		public bool _PartyInvited = false;
		public bool _TradeRequest = false;
		public bool _DuelRequest = false;
		public int _RandomTime = 0;
		private bool _FirstStart = true;

        public override void Pulse()
        {
			if (_FirstStart){
				Lua.Events.AttachEvent("GUILD_INVITE_REQUEST", _CheckGuild);
				Lua.Events.AttachEvent("PARTY_INVITE_REQUEST", _CheckParty);
				Lua.Events.AttachEvent("TRADE_SHOW", _CheckTrade);
				Lua.Events.AttachEvent("DUEL_REQUESTED", _CheckDuel);
				_FirstStart = false;
			}

			if (_GuildInvited && _Timer1.ElapsedMilliseconds >= _RandomTime ){
				Lua.DoString("DeclineGuild()");
				Lua.DoString("StaticPopup_Hide(\"GUILD_INVITE\")");
				_GuildInvited = false;
			}

			if (_PartyInvited && _Timer2.ElapsedMilliseconds >= _RandomTime ){
				Lua.DoString("DeclineGroup()");
				Lua.DoString("StaticPopup_Hide(\"PARTY_INVITE\")");
				_PartyInvited = false;
			}

			if (_TradeRequest && _Timer3.ElapsedMilliseconds >= _RandomTime ){
				Lua.DoString("CancelTrade()");
				_PartyInvited = false;
			}

			if (_DuelRequest && _Timer4.ElapsedMilliseconds >= _RandomTime ){
				Lua.DoString("CancelDuel()");
				Lua.DoString("StaticPopup_Hide(\"DUEL_REQUESTED\")");
				_DuelRequest = false;
			}
        }

		public void _CheckGuild(object sender, LuaEventArgs args)
		{
			_Timer1.Reset();
			_Timer1.Start();
			_RandomTime = RandomNumber(_MinTime, _MaxTime);
			_GuildInvited = true;
		
                        Logging.Write(Colors.Crimson, "[DeclineAll]: Guild invite detected, decline in: "+_RandomTime/1000+" seconds");

		}

		public void _CheckParty(object sender, LuaEventArgs args)

		{
			_Timer2.Reset();
			_Timer2.Start();
			_RandomTime = RandomNumber(_MinTime, _MaxTime);
			_PartyInvited = true;
		
   			Logging.Write(Colors.Crimson, "[DeclineAll]: Group invite detected detected, decline in: "+_RandomTime/1000+" seconds");
		}

		public void _CheckTrade(object sender, LuaEventArgs args)
		{
			_Timer3.Reset();
			_Timer3.Start();
			_RandomTime = RandomNumber(_MinTime, _MaxTime);
			_TradeRequest = true;
		
			Logging.Write(Colors.Crimson, "[DeclineAll]: Trade request detected detected, decline in: "+_RandomTime/1000+" seconds");
		}

		public void _CheckDuel(object sender, LuaEventArgs args)
		{
			_Timer4.Reset();
			_Timer4.Start();
			_RandomTime = RandomNumber(_MinTime, _MaxTime);
			_DuelRequest = true;
			Logging.Write(Colors.Crimson, "[DeclineAll]: Trade request detected detected, decline in: "+_RandomTime/1000+" seconds");
		}

		private int RandomNumber(int min, int max)
		{
			Random random = new Random();
			return random.Next(min, max);
		}

        public override string Name { get { return "declineAll"; } }

        public override string Author { get { return "Bryt"; } }

        public override Version Version { get { return new Version(1,0);} }
    }
}