using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieHolyPriestPvP
{
    public static class CombatLog
    {
        public static bool IsAttached { get; private set; }

        public static string FilterCriteria { get; set; }

        public static System.Action<ulong, SpellIDs, LuaEventArgs> OnSpellCastSuccess;
        public static System.Action<SpellIDs, LuaEventArgs> OnSpellCastFailed;
        public static System.Action<LuaEventArgs> OnUnhandledEvent;
        
        public static bool Attach()
        {
            if (string.IsNullOrEmpty(FilterCriteria))
                throw new ArgumentNullException("FilterCriteria is Empty!");

            if (IsAttached)
                return IsAttached;

            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);

            if (!Lua.Events.AddFilter("COMBAT_LOG_EVENT_UNFILTERED", FilterCriteria))
            {
                Logging.Write("ERROR: Could not add combat log event filter!");
                Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
                Logging.Write("Detached combat log");
            }
            else
            {
                Logging.Write("Attached combat log");
                IsAttached = true;
            }

            return IsAttached;
        }

        public static bool Detach()
        {
            if (!IsAttached)
                return false;

            IsAttached = false;

            Lua.Events.RemoveFilter("COMBAT_LOG_EVENT_UNFILTERED");
            Logging.Write("Removed combat log filter");
            Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
            Logging.Write("Detached combat log");

            Logging.Write("Detached combat log");

            return !IsAttached;
        }

        private static void HandleCombatLog(object sender, LuaEventArgs args)
        {
            switch (args.Args[1].ToString())
            {
                case "SPELL_CAST_SUCCESS":
                    SpellIDs id = (SpellIDs)(int)(double)args.Args[11];

                    string hexGUID = args.Args[3].ToString();
                    // .net can't parse hex strings that start with "0x".... :/
                    if (!string.IsNullOrEmpty(hexGUID) && hexGUID.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        hexGUID = hexGUID.Substring(2);
                    ulong guid = string.IsNullOrEmpty(hexGUID) ? 0 : ulong.Parse(hexGUID, NumberStyles.HexNumber);

                    if (OnSpellCastSuccess != null)
                        OnSpellCastSuccess(guid, id, args);
                    break;

                case "SPELL_CAST_FAILED":
                    id = (SpellIDs)(int)(double)args.Args[11];
                    if (OnSpellCastFailed != null)
                        OnSpellCastFailed(id, args);

                    Statistics.SpellFail(id);
                    break;

                default:
                    if (OnUnhandledEvent != null)
                        OnUnhandledEvent(args);
                    break;
            }
        }
    }
}