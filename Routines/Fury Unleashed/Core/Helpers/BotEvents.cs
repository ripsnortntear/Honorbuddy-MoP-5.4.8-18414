using System;
using FuryUnleashed.Core.Managers;
using FuryUnleashed.Core.Utilities;
using Styx.CommonBot.Routines;

namespace FuryUnleashed.Core.Helpers
{
    class BotEvents
    {
        public static void OnBotStarted(EventArgs args)
        {
            if (RoutineManager.Current.Name != Root.FuName) 
                return;
            HotKeyManager.RegisterKeys();
            LuaClass.DisableClickToMove();
            Logger.DiagLogPu("Fury Unleashed: Started! (OnBotStarted)");
        }

        public static void OnBotStopped(EventArgs args)
        {
            if (RoutineManager.Current.Name != Root.FuName) 
                return;
            HotKeyManager.RemoveAllKeys();
            LuaClass.EnableClickToMove();
            Logger.DiagLogPu("Fury Unleashed: Stopped! (OnBotStopped)");
        }

        public static void OnBotChanged(EventArgs args)
        {
            if (RoutineManager.Current.Name != Root.FuName)
                return;
            HotKeyManager.RemoveAllKeys();
            HotKeyManager.RegisterKeys();
            Logger.DiagLogPu("Fury Unleashed: Started! (OnBotChanged)");
        }
    }
}
