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

namespace RichieShadowPriestPvP
{
    public static class SPCoolDown
    {
        private static Dictionary<int, DateTime> LastCastTimes = new Dictionary<int, DateTime>();
        private static DateTime GCDReady;
        private static TimeSpan? innerFireCD = null;
        private static Dictionary<SpellIDs, DateTime> SpellJustCasted = new Dictionary<SpellIDs, DateTime>();
        private static bool SkipNextSWDCD = false;
        public static uint Latency { get; private set; }

        static SPCoolDown()
        {
            CombatLog.OnSpellCastSuccess += OnSpellCastSuccess;
            CombatLog.OnSpellEnergize += OnSpellEnergize;
            
        }

        #region Public interface

        public static TimeSpan GetSpellCooldown(SpellIDs spellId)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell((int)spellId, out results))
                return (results.Override ?? results.Original).CooldownTimeLeft;
            
            return TimeSpan.MaxValue;
        }

        public static void JustCasted(SpellIDs id) {
            SpellJustCasted[id] = DateTime.Now;
        }       

        public static void ConfirmSpellCast(SpellIDs id) {
            SpellJustCasted.Remove(id);
        }               

        public static void SetLastCastTime(SpellIDs id)
        {
            LastCastTimes[(int)id] = DateTime.Now;            
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

            DateTime lastCastTime;
            //to prevent double casts, but still remove spells from the list after some time
            if (SpellJustCasted.TryGetValue(spellId, out lastCastTime))
                if (lastCastTime.AddMilliseconds(5000) < DateTime.Now) {
                    ConfirmSpellCast(spellId);
                    Logging.Write("Spell(" + spellId + ") wasn't cleared properly.");
                } else
                    return false;       

            if (LastCastTimes.TryGetValue((int)spellId, out lastCastTime))
                return lastCastTime.AddMilliseconds((cooldownSecs * 1000) - Latency) <= DateTime.Now;

            TimeSpan cooldownTimeSpan = GetSpellCooldown(spellId);

            if (cooldownTimeSpan == TimeSpan.MaxValue)
                return false;

            double cooldownMillisecs = cooldownTimeSpan.TotalMilliseconds;

            if (cooldownMillisecs == 0)
                return true;

            LastCastTimes[(int)spellId] = DateTime.Now.Subtract(TimeSpan.FromMilliseconds((cooldownSecs * 1000) - cooldownMillisecs));

            return cooldownMillisecs <= Latency;
        }

        public static void UpdateGCD()
        {
            /*if (innerFireCD == null) {
                SpellFindResults results;
                if (SpellManager.FindSpell((int)SpellIDs.InnerFire, out results))
                    innerFireCD = (results.Override ?? results.Original).CooldownTimeLeft;
            }

            GCDReady = DateTime.Now + (innerFireCD ?? TimeSpan.FromMilliseconds(1500));*/
            //GCDReady = DateTime.Now + SpellManager.Spells["Inner Fire"].CooldownTimeLeft;
        }

        //Thanks for this code to the Pure Rotation team
        public static bool IsGlobalCD() {
            var latency = Latency << 1;
            var gcdTimeLeft = SpellManager.GlobalCooldownLeft.TotalMilliseconds;
            using (StyxWoW.Memory.AcquireFrame())
                return gcdTimeLeft > latency;
            //return DateTime.Now + TimeSpan.FromMilliseconds(Latency) <= GCDReady;
        }

        public static void UpdateLatency()
        {
            try {
                Latency = StyxWoW.WoWClient.Latency;
            } catch {
                return;
            }

            if (SPSettings.IsDebugMode)
            {
                Logging.Write("----------------------------------");
                Logging.Write("MyLatency: " + Latency);
                Logging.Write("----------------------------------");
            }

            //Lag Tolerance cap at 400
            if (Latency > 400)
                Latency = 400;
        }

        public static bool IsCasting() {

            //Mind Flay
            if (UnitManager.Me.IsChanneling && UnitManager.Me.ChanneledCastingSpellId == (int)SpellIDs.MindFlay) {
                return false;
            }

            if (UnitManager.Me.HasPendingSpell((int)SpellIDs.MassDispel) || UnitManager.Me.HasPendingSpell((int)SpellIDs.Psyfiend)
                || UnitManager.Me.HasPendingSpell((int)SpellIDs.AngelicFeather))
                return true;

            if (UnitManager.Me.IsCasting && UnitManager.Me.CurrentCastTimeLeft.TotalMilliseconds > Latency ||
                UnitManager.Me.IsChanneling && UnitManager.Me.CurrentChannelTimeLeft.TotalMilliseconds > Latency)
            {
                return true;
            }

            return false;
        }

        public static bool IsCastingOrGlobalCD()
        {
            return IsCasting() || IsGlobalCD();
        }

        #endregion

        #region Event Handlers

        private static void OnSpellCastSuccess(ulong casterGUID, SpellIDs id, LuaEventArgs args) {

            if (casterGUID != StyxWoW.Me.Guid)
                return;
            
            switch (id)
            {
                case SpellIDs.AngelicFeather:
                        double cooldownMillisecsAF = GetSpellCooldown(id).TotalMilliseconds;
                        LastCastTimes[(int)id] = DateTime.Now.Subtract(TimeSpan.FromMilliseconds((10000) - cooldownMillisecsAF));
                        SPCoolDown.ConfirmSpellCast(SpellIDs.AngelicFeatherAura);
                        SPCoolDown.ConfirmSpellCast(SpellIDs.AngelicFeather);
                    break;
                case SpellIDs.ShadowWordDeath:
                case SpellIDs.ShadowWordDeath_:
                        if (!SkipNextSWDCD) {
                            double cooldownMillisecsSWD = GetSpellCooldown(SpellIDs.ShadowWordDeath).TotalMilliseconds;
                            LastCastTimes[(int)SpellIDs.ShadowWordDeath] = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(8000 - cooldownMillisecsSWD));
                        }
                        SkipNextSWDCD = false;
                        SPCoolDown.ConfirmSpellCast(SpellIDs.ShadowWordDeath);
                        SPCoolDown.ConfirmSpellCast(SpellIDs.ShadowWordDeath_);
                    break;
                default:
                    LastCastTimes[(int)id] = DateTime.Now;
                    break;
            }
            
            Statistics.SpellCast(id);

            if (SPSettings.IsDebugMode)
                    Logging.WriteDiagnostic("Reset last cast time of " + args.Args[12] + " (id: " + (int)id + ")");
        }


        private static void OnSpellEnergize(SpellIDs id, LuaEventArgs args) {

            switch (id)
            {
                case SpellIDs.ShadowWordDeathCDResetProc:
                        LastCastTimes[(int)SpellIDs.ShadowWordDeath] = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(8000));
                        SkipNextSWDCD = true;  
                        SPCoolDown.ConfirmSpellCast(SpellIDs.ShadowWordDeath);
                        SPCoolDown.ConfirmSpellCast(SpellIDs.ShadowWordDeath_);
                        if (SPSettings.IsDebugMode)
                            Logging.WriteDiagnostic("Reset last cast time of Shadow Word: Death(target is not killed)");

                    break;
            }

        }
        #endregion
    }
}