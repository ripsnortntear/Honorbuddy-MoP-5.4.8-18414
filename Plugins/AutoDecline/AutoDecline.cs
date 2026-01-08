using System;
using System.Diagnostics;
using System.Linq;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.Common;


namespace AutoDecline
{
    public partial class AutoDecline : HBPlugin
    {
        //Normal Stuff.
        public override string Name { get { return "AutoDecline"; } }
        public override string Author { get { return "Bambam922"; } }
        public override Version Version { get { return new Version(1, 0 , 1); } }
        public override bool WantButton { get { return false; } }
        public override string ButtonText { get { return "AutoDecline"; } }

        public override void Pulse()
        {
            
        }
       
        public override void Initialize()
        {
            Lua.Events.AttachEvent("GUILD_INVITE_REQUEST", DeclineGuild);
            Lua.Events.AttachEvent("PARTY_INVITE_REQUEST", DeclineParty);
            Lua.Events.AttachEvent("ARENA_TEAM_INVITE_REQUEST", DeclineArena);
            Lua.Events.AttachEvent("TRADE_SHOW", DeclineTrade);
            Lua.Events.AttachEvent("DUEL_REQUESTED", DeclineDuel);
        }

        public static void DeclineGuild(object sender, LuaEventArgs args)
        {
            Logging.Write("Declining Guild Invitation.");
            Lua.DoString("DeclineGuild()");
        }

        public static void DeclineParty(object sender, LuaEventArgs args)
        {
            Logging.Write("Declining Party Invitation.");
            Lua.DoString("DeclineGroup()");
        }

        public static void DeclineArena(object sender, LuaEventArgs args)
        {
            Logging.Write("Declining Arena Team Invitation.");
            Lua.DoString("DeclineArenaTeam()");
        }

        public static void DeclineTrade(object sender, LuaEventArgs args)
        {
            Logging.Write("Cancelling Trade in Progress.");
            Lua.DoString("CancelTrade()");
        }

        public static void DeclineDuel(object sender, LuaEventArgs args)
        {
            Logging.Write("Declining Duel");
            Lua.DoString("CancelDuel()");
        }




        
    }
}
