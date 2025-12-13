using Styx;
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
using System.Windows.Media;

namespace RichieHolyPriestPvP
{
    public static class HolyCoolDown
    {
        private static Dictionary<int, DateTime> LastCastTimes = new Dictionary<int, DateTime>();
        private static HashSet<int> Casting = new HashSet<int>();
        private static DateTime GCDReady;
        private static TimeSpan? innerFireCD = null;

        public static bool SkipNextPoMCD { get; set; }

        public static uint Latency { get; private set; }

        static HolyCoolDown()
        {
            CombatLog.OnSpellCastSuccess += OnSpellCastSuccess;
            CombatLog.OnSpellCastFailed += OnSpellCastFailed;
        }

        #region Public interface

        public static TimeSpan GetSpellCooldown(SpellIDs spellId)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell((int)spellId, out results))
                return (results.Override ?? results.Original).CooldownTimeLeft;

            return TimeSpan.MaxValue;
        }

        public static void SetCasting(SpellIDs id)
        {
            Casting.Add((int)id);
        }

        public static void Fake(SpellIDs spellId)
        {
            DateTime lastCastTime;

            if (!LastCastTimes.TryGetValue((int)spellId, out lastCastTime))
                LastCastTimes.Add((int)spellId, DateTime.Now);
        }

        public static bool IsOff(SpellIDs spellId, double cooldownSecs)
        {
#if TRACE_MODE
            Logging.WriteDiagnostic("spellId: " + spellId + "(" + ((int)spellId)  + "), cooldown: " + cooldownSecs);
            LastCastTimes.All(ret => {

                Logging.WriteDiagnostic("   spell: " + (SpellIDs)ret.Key + "(" + ret.Key + ") - lastcast: " + ret.Value);

                return true;
            });
#endif
            int id = (int)spellId;
            if (Casting.Contains(id))
                return false;

            DateTime lastCastTime;

            if (LastCastTimes.TryGetValue(id, out lastCastTime))
                return lastCastTime.AddMilliseconds((cooldownSecs * 1000) - Latency) <= DateTime.Now;

            TimeSpan cooldownTimeSpan = GetSpellCooldown(spellId);

            if (cooldownTimeSpan == TimeSpan.MaxValue)
                return false;

            double cooldownMillisecs = cooldownTimeSpan.TotalMilliseconds;

            if (cooldownMillisecs == 0)
                return true;

            LastCastTimes[id] = DateTime.Now.Subtract(TimeSpan.FromMilliseconds((cooldownSecs * 1000) - cooldownMillisecs));

            return cooldownMillisecs <= Latency;
        }

        public static void UpdateGCD()
        {
            if (innerFireCD == null)
            {
                SpellFindResults results;
                if (SpellManager.FindSpell((int)SpellIDs.InnerFire, out results))
                    innerFireCD = (results.Override ?? results.Original).CooldownTimeLeft;
            }

            GCDReady = DateTime.Now + (innerFireCD ?? TimeSpan.FromMilliseconds(1500));
        }

        public static bool IsGlobalCD()
        {
            return DateTime.Now + TimeSpan.FromMilliseconds(Latency) <= GCDReady;
        }

        public static void UpdateLatency()
        {
            Latency = StyxWoW.WoWClient.Latency;

            if (HolySettings.IsDebugMode)
            {
                Logging.Write("----------------------------------");
                Logging.Write("MyLatency: " + Latency);
                Logging.Write("----------------------------------");
            }

            //Lag Tolerance cap at 400
            if (Latency > 400)
                Latency = 400;
        }

        public static bool IsCasting()
        {
            if (UnitManager.Me.HasPendingSpell((int)SpellIDs.MassDispel) || UnitManager.Me.HasPendingSpell((int)SpellIDs.Psyfiend))
                return true;

            if ((UnitManager.Me.IsCasting && UnitManager.Me.CurrentCastTimeLeft.TotalMilliseconds > Latency) ||
                (UnitManager.Me.IsChanneling && UnitManager.Me.CurrentChannelTimeLeft.TotalMilliseconds > Latency))
                return true;

            return false;
        }

        public static bool IsCastingOrGlobalCD()
        {
            return IsCasting() || IsGlobalCD();
        }

        #endregion

        #region Event Handlers

        private static void OnSpellCastSuccess(ulong casterGUID, SpellIDs id, LuaEventArgs args)
        {
            if (casterGUID != StyxWoW.Me.Guid)
                return;
            
            switch (id)
            {
                //The chakras are on shared cooldown
                case SpellIDs.ChakraChastise:
                case SpellIDs.ChakraSanctuary:
                case SpellIDs.ChakraSerenity:
                    LastCastTimes[(int)SpellIDs.ChakraChastise] = DateTime.Now;
                    LastCastTimes[(int)SpellIDs.ChakraSanctuary] = DateTime.Now;
                    LastCastTimes[(int)SpellIDs.ChakraSerenity] = DateTime.Now;

                    Casting.Remove((int)SpellIDs.ChakraChastise);
                    Casting.Remove((int)SpellIDs.ChakraSanctuary);
                    Casting.Remove((int)SpellIDs.ChakraSerenity);
                    break;

                case SpellIDs.PrayerOfMending:
                    if (SkipNextPoMCD)
                    {
                        Logging.Write(Colors.Gold, "Skipped Prayer Of Mending CD.");
                        SkipNextPoMCD = false;
                        break;
                    }
                    goto default;

                default:
                    LastCastTimes[(int)id] = DateTime.Now;
                    break;
            }

            Casting.Remove((int)id);

            Statistics.SpellCast(id);

            if (HolySettings.IsDebugMode)
                    Logging.WriteDiagnostic("Reset last cast time of " + args.Args[12] + " (id: " + id + ")");
        }

        private static void OnSpellCastFailed(SpellIDs spellId, LuaEventArgs args)
        {
            switch (spellId)
            {
                //The chakras are on shared cooldown
                case SpellIDs.ChakraChastise:
                case SpellIDs.ChakraSanctuary:
                case SpellIDs.ChakraSerenity:
                    Casting.Remove((int)SpellIDs.ChakraChastise);
                    Casting.Remove((int)SpellIDs.ChakraSanctuary);
                    Casting.Remove((int)SpellIDs.ChakraSerenity);
                    break;
                default:
                    Casting.Remove((int)spellId);
                    break;
            }
        }

        #endregion
    }
}