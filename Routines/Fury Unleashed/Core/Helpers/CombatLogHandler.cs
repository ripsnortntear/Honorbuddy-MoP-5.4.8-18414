using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Styx;
using Styx.Common;
using Styx.WoWInternals;

namespace FuryUnleashed.Core.Helpers
{
    [UsedImplicitly]
    internal class CombatLogHandler
    {
        //Credits to Apoc and Wulf for this class!!!

        internal static void Initialize()
        {
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLogEvent);
        }

        public delegate void CombatLogEventHandler(CombatLogEventArgs args);

        private static readonly Dictionary<string, List<CombatLogEventHandler>> EventHandlers =
            new Dictionary<string, List<CombatLogEventHandler>>();

        internal static void Shutdown()
        {
            Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLogEvent);
        }

        private static void HandleCombatLogEvent(object sender, LuaEventArgs args)
        {
            int index = 0;
            try
            {
                // ReSharper disable UseObjectOrCollectionInitializer
                var a = new CombatLogEventArgs();
                // ReSharper restore UseObjectOrCollectionInitializer

                // These are common to EVERY combat log event.
                if (index < args.Args.Length) a.TimeStamp = args.FireTimeStamp;
                if (index < args.Args.Length) a.Event = args.Args[1].ToString();

                //Logger.Output(a.Event);

                // Kinda unused. No real reason to bother with it.
                if (index < args.Args.Length) a.HideCaster = args.Args[2].ToString() == "true";

                //if (args.Args[3] != null && !string.IsNullOrEmpty(args.Args[3].ToString()))
                if (args.Args[3] != null && !string.IsNullOrEmpty(args.Args[3].ToString()) && args.Args[3].ToString().Length > 4)
                {
                    if (index < args.Args.Length) a.SourceGuid = ulong.Parse(args.Args[3].ToString().Remove(0, 2), NumberStyles.HexNumber);
                }

                if (args.Args[4] != null && index < args.Args.Length)
                {
                    a.SourceName = args.Args[4].ToString();
                }

                // Double cast is required here!
                if (index < args.Args.Length) a.SourceFlags = (SourceFlags)(double)args.Args[5];
                if (index < args.Args.Length) a.SourceRaidFlags = (SourceFlags)(double)args.Args[6];

                //if (args.Args[7] != null && !string.IsNullOrEmpty(args.Args[7].ToString()) && index < args.Args.Length)
                if (args.Args[7] != null && !string.IsNullOrEmpty(args.Args[7].ToString()) && index < args.Args.Length && args.Args[7].ToString().Length > 4)
                {
                    a.DestGuid = ulong.Parse(args.Args[7].ToString().Remove(0, 2), NumberStyles.HexNumber);
                }

                if (index < args.Args.Length) a.DestName = args.Args[8].ToString();
                // Double cast is required here!
                if (index < args.Args.Length) a.DestFlags = (SourceFlags)(double)args.Args[9];
                if (index < args.Args.Length) a.DestRaidFlags = (SourceFlags)(double)args.Args[10];

                // Do some stuff to fill the rest of the args...
                string prefix = a.Event.Split('_').FirstOrDefault();

                // We'll use an incrementing index here, so we don't need to keep track of what index we're at,
                // since the different prefixes cause different argument indexes.
                index = 11;

                // Now we parse in the prefix data.
                switch (prefix)
                {
                    case "SWING":
                        // ... yep... swing has no extra params.
                        break;
                    // DAMAGE prefix is handled the same as SPELL.
                    // This is just melee damage I believe?
                    case "DAMAGE":
                    case "SPELL":
                        // All SPELL prefixes have the same 3 args to start with.
                        // The suffixes change though!
                        if (index < args.Args.Length) a.SpellId = (int)(double)args.Args[index++];
                        if (index < args.Args.Length) a.SpellName = args.Args[index++].ToString();
                        if (index < args.Args.Length) a.SpellSchool = (WoWSpellSchool)(double)args.Args[index++];
                        break;

                    case "ENVIRONMENTAL":
                        if (index < args.Args.Length) a.EnvironmentalType = (EnvironmentalType)System.Enum.Parse(typeof(EnvironmentalType), args.Args[index++].ToString(), true);
                        break;
                }

                try
                {
                    // Suffixes are a little weird.
                    switch (GetSuffix(a.Event))
                    {
                        case "DAMAGE":
                        case "SHIELD": // DAMAGE_SHIELD
                        case "SPLIT": // DAMAGE_SPLIT
                            if (index < args.Args.Length) a.Amount = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Overhealing = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.School = (WoWSpellSchool)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Resisted = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Blocked = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Absorbed = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Critical = Convert.ToBoolean(args.Args[index++]);
                            if (index < args.Args.Length) a.Glancing = Convert.ToBoolean(args.Args[index++]);
                            if (index < args.Args.Length) a.Crushing = Convert.ToBoolean(args.Args[index++]);
                            break;

                        case "SHIELD_MISSED": // DAMAGE_SHIELD_MISSED
                        case "MISSED":
                            if (index < args.Args.Length) a.MissType = (MissType)System.Enum.Parse(typeof(MissType), args.Args[index++].ToString(), true);
                            if (index < args.Args.Length) a.IsOffHand = Convert.ToBoolean(args.Args[index++]);
                            // AmountMissed (probably not there)
                            break;

                        case "HEAL":
                            if (index < args.Args.Length) a.Amount = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Overhealing = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Absorbed = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.Critical = Convert.ToBoolean(args.Args[index++]);
                            break;

                        // +mana/energy type gains.
                        case "ENERGIZE":
                            if (index < args.Args.Length) a.Amount = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.PowerType = (WoWPowerType)(double)args.Args[index++];
                            break;

                        case "DRAIN":
                        case "LEECH":
                            if (index < args.Args.Length) a.Amount = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.PowerType = (WoWPowerType)(double)args.Args[index++];
                            if (index < args.Args.Length) a.ExtraAmount = (int)(double)args.Args[index++];
                            break;

                        case "INTERRUPT":
                        case "DISPEL_FAILED":
                            if (index < args.Args.Length) a.ExtraSpellId = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.ExtraSpellName = args.Args[index++].ToString();
                            if (index < args.Args.Length) a.ExtraSchool = (WoWSpellSchool)(double)args.Args[index++];
                            break;

                        case "DISPEL":
                        case "STOLEN":
                        case "AURA_BROKEN_SPELL":
                            if (index < args.Args.Length) a.ExtraSpellId = (int)(double)args.Args[index++];
                            if (index < args.Args.Length) a.ExtraSpellName = args.Args[index++].ToString();
                            if (index < args.Args.Length) a.ExtraSchool = (WoWSpellSchool)(double)args.Args[index++];
                            if (index < args.Args.Length) a.AuraType = (AuraType)System.Enum.Parse(typeof(AuraType), args.Args[index++].ToString(), true);
                            break;

                        case "EXTRA_ATTACKS":
                            if (index < args.Args.Length) a.Amount = (int)(double)args.Args[index++];
                            break;

                        case "AURA_APPLIED":
                        case "AURA_REMOVED":
                        case "AURA_REFRESH":
                        case "AURA_BROKEN":
                            if (index < args.Args.Length) a.AuraType = (AuraType)System.Enum.Parse(typeof(AuraType), args.Args[index++].ToString(), true);
                            break;

                        case "AURA_APPLIED_DOSE":
                        case "AURA_REMOVED_DOSE":
                            if (index < args.Args.Length) a.AuraType = (AuraType)System.Enum.Parse(typeof(AuraType), args.Args[index++].ToString(), true);
                            if (index < args.Args.Length) a.Amount = (int)(double)args.Args[index++];
                            break;

                        case "CAST_FAILED":
                            if (index < args.Args.Length) a.FailedType = args.Args[index++].ToString();
                            break;

                        case "CAST_START":
                        case "CAST_SUCCESS":
                        case "INSTAKILL":
                        case "DURABILITY_DAMAGE":
                        case "DURABILITY_DAMAGE_ALL":
                        case "CREATE":
                        case "SUMMON":
                        case "RESURRECT":
                            break;

                        default:

                            // These are the "special" events.
                            // They don't have normal prefix/suffix stuff
                            // So we'll handle them here.
                            switch (a.Event)
                            {
                                case "ENCHANT_APPLIED":
                                case "ENCHANT_REMOVED":
                                    if (index < args.Args.Length) a.SpellName = args.Args[index++].ToString();
                                    if (index < args.Args.Length) a.ItemId = (int)(double)args.Args[index++];
                                    if (index < args.Args.Length) a.ItemName = args.Args[index++].ToString();
                                    break;

                                case "PARTY_KILL":
                                case "UNIT_DIED":
                                case "UNIT_DESTROYED":
                                    break;

                                // Something we don't know about! Duh oh!
                                default:
                                    Logging.WriteDiagnostic("Unknown combat log event: " + a.Event);
                                    break;
                            }
                            break;
                    }
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                    // This is easier to just catch the index exceptions than to do all the extra checks
                    // to ensure the optional params are there.
                    // So just leave this at an empty catch. Unavailable params will just have default values. :)
                }

                //if (CurrentBossEncounter != null) CurrentBossEncounter.HandleCombatLogEvent(a);

                //Logger.Output(a.ToString());

                List<CombatLogEventHandler> handlers;
                if (EventHandlers.TryGetValue(a.Event, out handlers))
                {
                    foreach (CombatLogEventHandler handler in handlers)
                    {
                        try
                        {
                            handler(a);
                        }
                        catch (Exception ex)
                        {
                            Utilities.Logger.DiagLogWh("{0}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.Logger.DiagLogWh(args.Args[1] + ", Index: " + index);
                Utilities.Logger.DiagLogWh("{0}", ex);
            }
        }

        private static string GetSuffix(string s)
        {
            string[] split = s.Split('_');
            switch (split[0])
            {
                case "SWING":
                case "RANGE":
                case "ENVIRONMENTAL":
                case "DAMAGE":
                    return s.Replace(split[0] + "_", "");
                case "SPELL":
                    // 3 main prefixes for SPELL events.
                    // We want the suffix so we can parse in the data.
                    // Simple 'nuff

                    if (s.StartsWith("SPELL_PERIODIC"))
                        return s.Replace("SPELL_PERIODIC_", "");

                    if (s.StartsWith("SPELL_BUILDING"))
                        return s.Replace("SPELL_BUILDING_", "");

                    return s.Replace("SPELL_", "");
                default:
                    return s;
            }
        }

        public static void Register(string combatLogEventName, CombatLogEventHandler handler)
        {
            List<CombatLogEventHandler> handlers;

            if (!EventHandlers.TryGetValue(combatLogEventName, out handlers))
            {
                EventHandlers.Add(combatLogEventName, new List<CombatLogEventHandler> { handler });

                // Remove the old filter.
                try
                {
                    Lua.Events.RemoveFilter("COMBAT_LOG_EVENT_UNFILTERED");
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                { /* Catch all errors */ }

                var sb = new StringBuilder();
                sb.Append("return ");
                foreach (string eh in EventHandlers.Keys)
                {
                    sb.Append("args[2] == '" + eh + "' or ");
                }
                // Trim the " or" at the end. Cuz we're lazu! The evolved form of lazy!

                // Pop in our new one. This avoids a shitload of events being thrown at us, that we really don't care about.
                Lua.Events.AddFilter("COMBAT_LOG_EVENT_UNFILTERED", sb.ToString().TrimEnd(" or ".ToArray()));
            }
            else
            {
                handlers.Add(handler);
            }
        }

        public static void Remove(string combatLogEventName)
        {
            List<CombatLogEventHandler> handlers;

            if (EventHandlers.TryGetValue(combatLogEventName, out handlers))
            {
                EventHandlers.Remove(combatLogEventName);

                // Remove the old filter.
                try
                {
                    Lua.Events.RemoveFilter("COMBAT_LOG_EVENT_UNFILTERED");
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                { /* Catch all errors */ }

                var sb = new StringBuilder();
                sb.Append("return ");
                foreach (string eh in EventHandlers.Keys)
                {
                    sb.Append("args[2] == '" + eh + "' or ");
                }
                // Trim the " or" at the end. Cuz we're lazu! The evolved form of lazy!

                // Pop in our new one. This avoids a shitload of events being thrown at us, that we really don't care about.
                Lua.Events.AddFilter("COMBAT_LOG_EVENT_UNFILTERED", sb.ToString().TrimEnd(" or ".ToArray()));

                Utilities.Logger.DiagLogWh(" Removed combat filter:  {0}", combatLogEventName);
            }
        }
    }

    [Flags]
    public enum SourceFlags
    {
        TypeObject = 0x4000,
        TypeGuardian = 0x2000,
        TypePet = 0x1000,
        TypeNpc = 0x800,
        TypePlayer = 0x400,

        ControlNpc = 0x200,
        ControlPlayer = 0x100,

        ReactionHostile = 0x40,
        ReactionNeutral = 0x20,
        ReactionFriendly = 0x10,

        AffiliationOutsider = 0x8,
        AffiliationRaid = 0x4,
        AffiliationParty = 0x2,
        AffiliationMine = 0x1,

        RaidTarget8 = 0x8000000,
        RaidTarget7 = 0x4000000,
        RaidTarget6 = 0x2000000,
        RaidTarget5 = 0x1000000,
        RaidTarget4 = 0x800000,
        RaidTarget3 = 0x400000,
        RaidTarget2 = 0x200000,
        RaidTarget1 = 0x100000,
        MainAssist = 0x80000,
        MainTank = 0x40000,
        Focus = 0x20000,
        Target = 0x10000,
    }

    public enum MissType
    {
        Absorb,
        Block,
        Deflect,
        Dodge,
        Evade,
        Immune,
        Miss,
        Parry,
        Reflect,
        Resist
    }

    public enum AuraType
    {
        Buff,
        Debuff
    }

    public enum EnvironmentalType
    {
        Drowning,
        Falling,
        Fatigue,
        Fire,
        Lava,
        Slime
    }

    public class CombatLogEventArgs
    {
        public uint TimeStamp { get; set; }

        public string Event { get; set; }

        public bool HideCaster { get; set; }

        public ulong SourceGuid { get; set; }

        public string SourceName { get; set; }

        public SourceFlags SourceFlags { get; set; }

        public SourceFlags SourceRaidFlags { get; set; }

        public ulong DestGuid { get; set; }

        public string DestName { get; set; }

        public SourceFlags DestFlags { get; set; }

        public SourceFlags DestRaidFlags { get; set; }

        // Spell/Range stuff
        public int SpellId { get; set; }

        public string SpellName { get; set; }

        public WoWSpellSchool SpellSchool { get; set; }

        // Environmental Type
        public EnvironmentalType EnvironmentalType { get; set; }

        // _DAMAGE suffix
        public int Amount { get; set; }

        public int Overkill { get; set; }

        public WoWSpellSchool School { get; set; }

        public int Resisted { get; set; }

        public int Blocked { get; set; }

        public int Absorbed { get; set; }

        public bool Critical { get; set; }

        public bool Glancing { get; set; }

        public bool Crushing { get; set; }

        // _MISSED suffix
        public MissType MissType { get; set; }

        public bool IsOffHand { get; set; }

        public int AmountMissed { get; set; }

        // _HEAL suffix
        public int Overhealing { get; set; }

        // _ENERGIZE/DRAIN/LEECH
        public WoWPowerType PowerType { get; set; }

        public int ExtraAmount { get; set; }

        // _INTERRUPT/DISPEL/DISPEL_FAILED/STOLEN
        public int ExtraSpellId { get; set; }

        public string ExtraSpellName { get; set; }

        public WoWSpellSchool ExtraSchool { get; set; }

        public AuraType AuraType { get; set; }

        // _CAST_FAILED
        public string FailedType { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "TimeStamp: {0}, Event: {1}, HideCaster: {2}, SourceGuid: {3}, SourceName: {4}, SourceFlags: {5}, SourceRaidFlags: {6}, DestGuid: {7}, DestName: {8}, DestFlags: {9}, DestRaidFlags: {10}, SpellId: {11}, SpellName: {12}, SpellSchool: {13}, EnvironmentalType: {14}, Amount: {15}, Overkill: {16}, School: {17}, Resisted: {18}, Blocked: {19}, Absorbed: {20}, Critical: {21}, Glancing: {22}, Crushing: {23}, MissType: {24}, IsOffHand: {25}, AmountMissed: {26}, Overhealing: {27}, PowerType: {28}, ExtraAmount: {29}, ExtraSpellId: {30}, ExtraSpellName: {31}, ExtraSchool: {32}, AuraType: {33}, FailedType: {34}, ItemId: {35}, ItemName: {36}",
                    TimeStamp,
                    Event,
                    HideCaster,
                    SourceGuid,
                    SourceName,
                    SourceFlags,
                    SourceRaidFlags,
                    DestGuid,
                    DestName,
                    DestFlags,
                    DestRaidFlags,
                    SpellId,
                    SpellName,
                    SpellSchool,
                    EnvironmentalType,
                    Amount,
                    Overkill,
                    School,
                    Resisted,
                    Blocked,
                    Absorbed,
                    Critical,
                    Glancing,
                    Crushing,
                    MissType,
                    IsOffHand,
                    AmountMissed,
                    Overhealing,
                    PowerType,
                    ExtraAmount,
                    ExtraSpellId,
                    ExtraSpellName,
                    ExtraSchool,
                    AuraType,
                    FailedType,
                    ItemId,
                    ItemName);
        }
    }
}