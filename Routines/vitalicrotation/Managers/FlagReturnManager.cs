using System;
using System.Linq;
using Styx.TreeSharp;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class FlagReturnManager
    {
        private const int ThrottleMs = 500;
        private const string ThrottleKey = "FlagReturn.Try";

        public static Composite Build()
        {
            return new Action(delegate
            {
                return Execute() ? RunStatus.Success : RunStatus.Failure;
            });
        }

        public static bool Execute()
        {
            bool acted = false;
            try
            {
                var S = VitalicSettings.Instance;
                if (!S.AutoFlagReturn) return false; // option disabled

                var me = StyxWoW.Me;
                if (me == null || !me.IsAlive) return false;

                if (!IsInBattleground()) return false; // BG only

                if (!Throttle.Check(ThrottleKey, ThrottleMs)) return false;

                var flag = ObjectManager.GetObjectsOfType<WoWGameObject>()
                    .Where(go => go != null && go.IsValid)
                    .Where(go => go.Name != null && go.Name.IndexOf("Flag", StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderBy(go => go.DistanceSqr)
                    .FirstOrDefault();

                if (flag == null) return false;
                if (flag.Distance > 5.0) return false; // short range safety
                if (me.IsCasting || me.IsChanneling) return false;

                flag.Interact();
                UiCompat.Notify("Return Flag");
                acted = true;
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "FlagReturnManager");
            }
            return acted;
        }

        private static bool IsInBattleground()
        {
            try
            {
                const string lua = "local i,t=IsInInstance(); if i and t=='pvp' then return 1 else return 0 end";
                return Lua.GetReturnVal<int>(lua, 0) == 1;
            }
            catch { return false; }
        }
    }
}
