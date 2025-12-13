#region

using System;
using Bots.BGBuddy.Helpers;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.WoWInternals;

#endregion

namespace Superbad
{
    public static class EventHandlers
    {
        public static DateTime RipAppliedDateTime;
        public static double RealRipTimeLeft;
        public static double Ripextends;
        public static double ClipTime;

        private static bool _combatLogAttached;

        public static void Init()
        {
            AttachCombatLogEvent();
        }

        private static void AttachCombatLogEvent()
        {
            if (_combatLogAttached)
                return;

            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
            if (
                !Lua.Events.AddFilter(
                    "COMBAT_LOG_EVENT_UNFILTERED",
                    "return args[2] == 'SPELL_AURA_REMOVED' or args[2] == 'SPELL_DAMAGE' or args[2] == 'SPELL_AURA_APPLIED' or args[2] =='SPELL_AURA_REFRESH'"))
            {
                Logger.Write(
                    "ERROR: Could not add combat log event filter! - Performance may be horrible, and things may not work properly!");
            }

            Logger.WriteDebug("Attached combat log");
            _combatLogAttached = true;
        }

        private static void HandleCombatLog(object sender, LuaEventArgs args)
        {
            var e = new CombatLogEventArgs(args.EventName, args.FireTimeStamp, args.Args);
            switch (e.Event)
            {
                case "SPELL_AURA_APPLIED":
                    if (e.SourceGuid != 1 && e.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (e.SpellId == 1822)
                        {
                            Spell.UpdateRakeTargets(e.DestGuid, Superbad.Rake_sDamage);
                        }
                        if (e.SpellId == 1079)
                        {
                            RipAppliedDateTime = DateTime.Now;
                            Superbad._dot_rip_multiplier = Superbad.Rip_sDamage;
                            Ripextends = 0;
                        }
                    }
                    break;
                case "SPELL_AURA_REFRESH":
                    if (e.SourceGuid != 1 && e.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (e.SpellId == 1822)
                        {
                            Spell.UpdateRakeTargets(e.DestGuid, Superbad.Rake_sDamage);
                        }
                        if (e.SpellId == 1079)
                        {
                            ClipTime =
                                CalcClipTime((16 + CalcExtensionsTime() + ClipTime) -
                                             (-1*(RipAppliedDateTime - DateTime.Now).TotalSeconds));
                            RipAppliedDateTime = DateTime.Now;
                            Superbad._dot_rip_multiplier = Superbad.Rip_sDamage;
                            Ripextends = 0;
                        }
                    }
                    break;

                case "SPELL_AURA_REMOVED":
                    if (e.SourceGuid != 1 && e.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (e.SpellId == 1822)
                        {
                            Spell.UpdateRakeTargets(e.DestGuid, 0);
                        }
                        if (e.SpellId == 1079)
                        {
                            Superbad._dot_rip_multiplier = 0;
                            Ripextends = 0;
                            ClipTime = 0;
                        }
                    }
                    break;

                case "SPELL_DAMAGE":
                    if (e.SourceGuid != 1 && e.SourceGuid == StyxWoW.Me.Guid)
                    {
                        if (Superbad.dot.rip.ticking)
                        {
                            if (e.SpellId == 5221 || e.SpellId == 114236 || e.SpellId == 102545 || e.SpellId == 6785 ||
                                e.SpellId == 33876)
                                //Normal Shred, Glyphed Shred, Ravage, other Ravage, Mangle 		
                                if (Ripextends < 3)
                                    Ripextends ++;
                            if (e.SpellId == 22568 && StyxWoW.Me.CurrentTarget != null &&
                                StyxWoW.Me.CurrentTarget.HealthPercent < 25)
                            {
                                ClipTime =
                                    CalcClipTime((16 + CalcExtensionsTime() + ClipTime) -
                                                 (-1*(RipAppliedDateTime - DateTime.Now).TotalSeconds));
                                RipAppliedDateTime = DateTime.Now;
                                Ripextends = 0;
                            }
                        }
                    }
                    break;

                case "SWING_MISSED":
                    if (e.Args[11].ToString() == "EVADE")
                    {
                        Logger.Write("Mob is evading swing. Blacklisting it!");
                        Blacklist.Add(e.DestGuid, BlacklistFlags.Combat, TimeSpan.FromMinutes(30));
                        if (StyxWoW.Me.CurrentTargetGuid == e.DestGuid)
                        {
                            StyxWoW.Me.ClearTarget();
                        }

                        BotPoi.Clear("Blacklisting evading mob");
                        StyxWoW.SleepForLagDuration();
                    }
                    break;
            }
        }

        private static double CalcClipTime(double time)
        {
            if (time < 2)
                return time;
            if (time < 4)
                return time - 2;
            if (time < 6)
                return time - 4;
            if (time < 8)
                return time - 6;
            if (time < 10)
                return time - 8;
            if (time < 12)
                return time - 10;
            if (time < 14)
                return time - 12;
            if (time < 16)
                return time - 14;
            if (time < 18)
                return time - 16;
            if (time < 20)
                return time - 18;
            if (time < 22)
                return time - 20;
            if (time < 24)
                return time - 22;
            if (time < 26)
                return time - 24;
            if (time < 28)
                return time - 26;
            return 0;
        }

        public static double CalcExtensionsTime()
        {
            if (Ripextends == 3)
                return 8;
            if (Ripextends == 2)
                return 6;
            if (Ripextends == 1)
                return 2;
            return 0;
        }

        public static void CalcRealTimeOfRip()
        {
            RealRipTimeLeft = !Superbad.dot.rip.ticking
                ? 0
                : (16 + CalcExtensionsTime() + ClipTime) - (-1*(RipAppliedDateTime - DateTime.Now).TotalSeconds);
        }
    }
}