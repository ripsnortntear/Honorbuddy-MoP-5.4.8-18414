//Mr.PuginTemplate - Created by CodenameGamma - 4-12-11 - For WoW Version 4.0.3+
//www.honorbuddy.de
//this is a free plugin, and should not be sold, or repackaged.
//Donations Accepted. 
//Version 1.0

using System;
using Styx.Common;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace MrPluginTemplate
{

    public class MrPluginTemplate : HBPlugin
    {
        //Normal Stuff.
        public override string Name { get { return "Mr.LootBind"; } }
        public override string Author { get { return "CnG"; } }
        public override Version Version { get { return new Version(1, 0); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Mr.LootBind"; } }

        //Logging Class for your conviance
        public static void slog(string format, params object[] args)
        { Logging.Write("[Mr.LootBind]:" + format, args); }

        public override void Initialize()
        {
            slog("Loot Bind Confirm Attached!");
            Lua.Events.AttachEvent("LOOT_BIND_CONFIRM", BindItemConfirmPopup);

        }
        public override void  Dispose()
        {
            slog("Loot Bind Confirm Detached!");
            Lua.Events.DetachEvent("LOOT_BIND_CONFIRM", BindItemConfirmPopup);

        }
        //Uncomment if adding a UI for the plugin
        /*public override void OnButtonPress()
        {

        }*/
        private static void BindItemConfirmPopup(object sender, LuaEventArgs args)
        {

           slog("Clicking Yes to Comfirm An Item being SoulBound to you.");
           Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");

        }
        public override void Pulse()
        {
         
        }
    }
}
