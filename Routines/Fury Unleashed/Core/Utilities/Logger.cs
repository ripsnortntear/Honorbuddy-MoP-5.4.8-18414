using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Interfaces.Settings;
using FuryUnleashed.Rotations;
using Styx;
using Styx.Common;
using Styx.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using Timer = System.Timers.Timer;

namespace FuryUnleashed.Core.Utilities
{
    static class Logger
    {
        private static string _lastCombatmsg;

        // Combatlogging.
        public static void CombatLogFb(string message, params object[] args)
        {
            if (message == _lastCombatmsg && (!message.Contains("Cleave") || !message.Contains("Devastate") || !message.Contains("Execute") || !message.Contains("Heroic Strike") || !message.Contains("Overpower"))) return;
            Logging.Write(Colors.Firebrick, "{0}", String.Format(message, args));
            _lastCombatmsg = message;
        }

        public static void CombatLogLg(string message, params object[] args)
        {
            if (message == _lastCombatmsg && (!message.Contains("Cleave") || !message.Contains("Devastate") || !message.Contains("Execute") || !message.Contains("Heroic Strike") || !message.Contains("Overpower"))) return;
            Logging.Write(Colors.LimeGreen, "{0}", String.Format(message, args));
            _lastCombatmsg = message;
        }

        public static void CombatLogOr(string message, params object[] args)
        {
            if (message == _lastCombatmsg && (!message.Contains("Cleave") || !message.Contains("Devastate") || !message.Contains("Execute") || !message.Contains("Heroic Strike") || !message.Contains("Overpower"))) return;
            Logging.Write(Colors.Orange, "{0}", String.Format(message, args));
            _lastCombatmsg = message;
        }

        public static void CombatLogPu(string message, params object[] args)
        {
            if (message == _lastCombatmsg && (!message.Contains("Cleave") || !message.Contains("Devastate") || !message.Contains("Execute") || !message.Contains("Heroic Strike") || !message.Contains("Overpower"))) return;
            Logging.Write(Colors.MediumPurple, "{0}", String.Format(message, args));
            _lastCombatmsg = message;
        }

        public static void CombatLogWh(string message, params object[] args)
        {
            if (message == _lastCombatmsg && (!message.Contains("Cleave") || !message.Contains("Devastate") || !message.Contains("Execute") || !message.Contains("Heroic Strike") || !message.Contains("Overpower"))) return;
            Logging.Write(Colors.White, "{0}", String.Format(message, args));
            _lastCombatmsg = message;
        }

        // Debuglogging.
        public static void DiagLogFb(string message, params object[] args)
        {
            if (message == null) return;
            Logging.WriteDiagnostic(Colors.Firebrick, "{0}", String.Format(message, args));
        }

        public static void DiagLogLg(string message, params object[] args)
        {
            if (message == null) return;
            Logging.WriteDiagnostic(Colors.LimeGreen, "{0}", String.Format(message, args));
        }

        public static void DiagLogOr(string message, params object[] args)
        {
            if (message == null) return;
            Logging.WriteDiagnostic(Colors.Orange, "{0}", String.Format(message, args));
        }

        public static void DiagLogPu(string message, params object[] args)
        {
            if (message == null) return;
            Logging.WriteDiagnostic(Colors.MediumPurple, "{0}", String.Format(message, args));
        }

        public static void DiagLogWh(string message, params object[] args)
        {
            if (message == null) return;
            Logging.WriteDiagnostic(Colors.White, "{0}", String.Format(message, args));
        }

        // Specific Loggers
        public static void CooldownTrackerLog(string message, params object[] args)
        {
            if (message == null || InternalSettings.Instance.General.CheckCooldownTrackerLogging == false) return;
            Logging.WriteDiagnostic(Colors.Crimson, "{0}", String.Format(message, args));
        }

        // Debug Logging
        public static string PrintBarrierSize
        {
            get
            {
                var shieldbarriersize = DamageTracker.CalculateEstimatedAbsorbValue();

                if (StyxWoW.Me.Specialization != WoWSpec.WarriorProtection)
                {
                    return "Use Protection Spec!";
                }

                CombatLogLg("FU: Shield Barrier size is {0} with Spell ID {1}", shieldbarriersize, DamageTracker.ShieldBarrierSpellId);
                return shieldbarriersize.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string PrintBlockSize
        {
            get
            {
                var shieldblocksize = DamageTracker.CalculateEstimatedBlockValue();

                if (StyxWoW.Me.Specialization != WoWSpec.WarriorProtection)
                {
                    return "Use Protection Spec!";
                }

                CombatLogLg("FU: Shield Block size is {0} with Spell ID {1}", shieldblocksize, DamageTracker.ShieldBlockSpellId);
                return shieldblocksize.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string PrintDamageTaken
        {
            get
            {
                var damagetaken = DamageTracker.GetDamageTaken(DateTime.Now);

                if (StyxWoW.Me.Specialization != WoWSpec.WarriorProtection)
                {
                    return "Use Protection Spec!";
                }

                CombatLogLg("FU: Damage taken is {0}", damagetaken);
                return damagetaken.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string PrintVigilanceTarget
        {
            get
            {
                if (StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid)
                {
                    Unit.GetVigilanceTarget();

                    var vigilancetarget = Unit.VigilanceTarget;

                    if (vigilancetarget == null)
                    {
                        return "No Suitable Target";
                    }

                    CombatLogLg("FU: Vigilance target is {0}", vigilancetarget);
                    return vigilancetarget.ToString();
                }
                return "No Suitable Target";
            }
        }

        public static void WriteFileLog(string message, params object[] args)
        {
            if (message == null) return;
            WriteFile("{0}", String.Format(message, args));
        }

        public static void WriteInfoToLogFile()
        {
            Item.RefreshSecondaryStats();

            WriteFile("Fury Unleashed: Diagnostic Logging");
            WriteFile("");
            WriteFile("{0} is the used revision.", Root.Revision);
            WriteFile("Current race {0} with {1} as spec and level {2}.", StyxWoW.Me.Race, StyxWoW.Me.Specialization.ToString().CamelToSpaced(), StyxWoW.Me.Level);
            WriteFile("{0} is your faction", StyxWoW.Me.IsAlliance ? "Alliance" : "Horde");
            WriteFile("");
            WriteFile("{0:F1} days since Windows was started.", TimeSpan.FromMilliseconds(Environment.TickCount).TotalHours / 24.0);
            WriteFile("{0} FPS currently in WoW.", LuaClass.GetFps());
            WriteFile("{0} ms of Latency in WoW.", StyxWoW.WoWClient.Latency);
            WriteFile("");
            WriteFile("{0} is the HB path.", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            WriteFile("");
            LogSettings("General Settings (SettingsG)", InternalSettings.Instance.General);
            LogSettings("Hotkey Settings (SettingsH)", SettingsH.Instance);
            if (Global.IsArmsSpec)
                LogSettings("Arms Settings (SettingsA)", InternalSettings.Instance.Arms);
            if (Global.IsFurySpec)
                LogSettings("Fury Settings (SettingsF)", InternalSettings.Instance.Fury);
            if (Global.IsProtSpec)
                LogSettings("Protection Settings (SettingsP)", InternalSettings.Instance.Protection);
            WriteFile("");
            WriteFile("====== Talents ======");
            WriteFile("Juggernaut Talent: {0}", Global.JuggernautTalent);
            WriteFile("Double Time Talent: {0}", Global.DoubleTimeTalent);
            WriteFile("Warbringer Talent: {0}", Global.WarbringerTalent);
            WriteFile("Enraged Regeneration Talent: {0}", Global.EnragedRegenerationTalent);
            WriteFile("Second Wind Talent: {0}", Global.SecondWindTalent);
            WriteFile("Impending Victory Talent: {0}", Global.ImpendingVictoryTalent);
            WriteFile("Staggering Shout Talent: {0}", Global.StaggeringShoutTalent);
            WriteFile("Piercing Howl Talent: {0}", Global.PiercingHowlTalent);
            WriteFile("Disrupting Shout Talent: {0}", Global.DisruptingShoutTalent);
            WriteFile("Bladestorm Talent: {0}", Global.BladestormTalent);
            WriteFile("Shockwave Talent: {0}", Global.ShockwaveTalent);
            WriteFile("Dragon Roar Talent: {0}", Global.DragonRoarTalent);
            WriteFile("Mass Spell Reflection Talent: {0}", Global.MassSpellReflectionTalent);
            WriteFile("Safeguard Talent: {0}", Global.SafeguardTalent);
            WriteFile("Vigilance Talent: {0}", Global.VigilanceTalent);
            WriteFile("Avatar Talent: {0}", Global.AvatarTalent);
            WriteFile("Bloodbath Talent: {0}", Global.BloodbathTalent);
            WriteFile("Storm Bolt Talent: {0}", Global.StormBoltTalent);
            WriteFile("");
            WriteFile("======= Item Info =======");
            WriteFile("AttackSpeed: {0}", Item.AttackSpeed);
            WriteFile("Crit Chance: {0}", Item.CritChance);
            WriteFile("Crit Rating: {0}", Item.CritRating);
            WriteFile("Expertise: {0}", Item.Expertise);
            WriteFile("Mastery: {0}", Item.Mastery);
            WriteFile("Haste: {0}", Item.MeleeHaste);
            WriteFile("Hit: {0}", Item.MeleeHit);
            WriteFile("2H Weapons: {0}", Global.WieldsTwoHandedWeapons);
            WriteFile("Tier 16 DPS 2P: {0}", Global.Tier16TwoPieceBonus);
            WriteFile("Tier 16 DPS 4P: {0}", Global.Tier16FourPieceBonus);
            WriteFile("Tier 16 Prot 2P: {0}", Global.Tier16TwoPieceBonusT);
            WriteFile("Tier 16 Prot 4P: {0}", Global.Tier16FourPieceBonusT);
        }

        private static Timer _fuTimer;

        public static void LogTimer(int tickingtime)
        {
            _fuTimer = new Timer(tickingtime);
            _fuTimer.Elapsed += OnTimedEvent;
            _fuTimer.AutoReset = false;
            _fuTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            WriteInfoToLogFile();
        }

        internal static void StatCounter()
        {
            try
            {
                var statcounterDate = DateTime.Now.DayOfYear.ToString(CultureInfo.InvariantCulture);
                if (!statcounterDate.Equals(InternalSettings.Instance.General.LastStatCounted))
                {
                    Parallel.Invoke(
                        () => new WebClient().DownloadData("http://c.statcounter.com/9163286/0/396c7d29/1/"),
                        () => DiagLogWh("FU: StatCounter has been updated!"));
                    InternalSettings.Instance.General.LastStatCounted = statcounterDate;
                    InternalSettings.Instance.Save();
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { /* Catch all errors */ }
        }


        public static void WriteFile(string message)
        {
            WriteFile(LogLevel.Verbose, message);
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static void WriteFile(string message, params object[] args)
        {
            WriteFile(LogLevel.Verbose, message, args);
        }

        public static void WriteFile(LogLevel ll, string message, params object[] args)
        {
            if (GlobalSettings.Instance.LogLevel >= LogLevel.Quiet)
                Logging.WriteToFileSync(ll, "Fury Unleashed: " + message, args);
        }

        public static void LogSettings(string desc, Settings set)
        {
            if (set == null)
                return;

            WriteFile("====== {0} ======", desc);
            foreach (var kvp in set.GetSettings())
            {
                WriteFile("  {0}: {1}", kvp.Key, kvp.Value.ToString());
            }
            WriteFile("");
        }

        public static string CamelToSpaced(this string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsUpper(c))
                {
                    sb.Append(' ');
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
