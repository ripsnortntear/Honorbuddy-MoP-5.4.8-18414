#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    internal static class Spell
    {
        public static WoWAuraCollection CurrentTargetAuras;
        public static WoWAuraCollection MyAuras;

        public static readonly Dictionary<string, DoubleCastSpell> DoubleCastEntries =
            new Dictionary<string, DoubleCastSpell>();

        public static readonly Dictionary<string, LogEntry> LogEntries = new Dictionary<string, LogEntry>();

        public static readonly Dictionary<ulong, double> RakeTargets = new Dictionary<ulong, double>();


        public static readonly Dictionary<double, CoolDownEntry> ItemCooldowns = new Dictionary<double, CoolDownEntry>();

        public static void UpdateItemEntries(double spellName, double expiryTime)
        {
            if (ItemCooldowns.ContainsKey(spellName))
                ItemCooldowns[spellName] = new CoolDownEntry(spellName, expiryTime, DateTime.Now);
            if (!ItemCooldowns.ContainsKey(spellName))
                ItemCooldowns.Add(spellName, new CoolDownEntry(spellName, expiryTime, DateTime.Now));
        }

        internal static void PulseItemEntries()
        {
            ItemCooldowns.RemoveAll(
                t => DateTime.Now.Subtract(t.DoubleCastCurrentTime).TotalSeconds >= t.DoubleCastExpiryTime);
        }


        public static void UpdateRakeTargets(ulong guid, double strength)
        {
            if (RakeTargets.ContainsKey(guid))
                RakeTargets[guid] = strength;
            if (!RakeTargets.ContainsKey(guid))
                RakeTargets.Add(guid, strength);
        }

        internal static void PulseRakeTargets()
        {
            foreach (var rakeTarget in RakeTargets.Where(rakeTarget => rakeTarget.Value == 0))
            {
                RakeTargets.Remove(rakeTarget.Key);
            }
        }

        internal static double GetRakeStrength(ulong guid)
        {
            return !RakeTargets.ContainsKey(guid) ? 0 : RakeTargets.FirstOrDefault(t => t.Key == guid).Value;
        }

        public static void UpdateLogEntries(string spellName, double expiryTime)
        {
            if (LogEntries.ContainsKey(spellName))
                LogEntries[spellName] = new LogEntry(spellName, expiryTime, DateTime.Now);
            if (!LogEntries.ContainsKey(spellName))
                LogEntries.Add(spellName, new LogEntry(spellName, expiryTime, DateTime.Now));
        }

        internal static void PulseLogEntries()
        {
            LogEntries.RemoveAll(
                t => DateTime.Now.Subtract(t.DoubleCastCurrentTime).TotalSeconds >= t.DoubleCastExpiryTime);
        }

        public static void UpdateDoubleCastEntries(string spellName, double expiryTime)
        {
            if (DoubleCastEntries.ContainsKey(spellName))
                DoubleCastEntries[spellName] = new DoubleCastSpell(spellName, expiryTime, DateTime.Now);
            if (!DoubleCastEntries.ContainsKey(spellName))
                DoubleCastEntries.Add(spellName, new DoubleCastSpell(spellName, expiryTime, DateTime.Now));
        }

        internal static void PulseDoubleCastEntries()
        {
            DoubleCastEntries.RemoveAll(
                t => DateTime.Now.Subtract(t.DoubleCastCurrentTime).TotalSeconds >= t.DoubleCastExpiryTime);
        }

        public static bool SpellOnCooldownOffGCD(int id)
        {
            return GetSpellCooldown(id) > 0;
        }

        public static bool SpellOnCooldown(int id, bool cd = false)
        {
            if (Superbad._gcdTimeLeftTotalSeconds > 0)
                return true;
            if (cd)
                return GetSpellCooldown(id) > 0;
            return false;
        }


        public static double GetSpellCooldownONGCD(int id, bool cd = false)
        {
            if (Superbad._gcdTimeLeftTotalSeconds > 0)
                return Superbad._gcdTimeLeftTotalSeconds;
            if (cd)
                return GetSpellCooldown(id);
            return TimeSpan.MaxValue.TotalSeconds;
        }

        public static double GetSpellCooldown(int id)
        {
            return id == 33878 ? GetSpellCooldownOLD(id) : CooldownTracker.GetSpellCooldownTimeLeft(id);
        }

        public static double GetItemCooldown(int id)
        {
            if (!ItemCooldowns.ContainsKey(id))
            {
                UpdateItemEntries(id, Superbad.Gloves.CooldownTimeLeft.TotalSeconds);
                return Superbad.Gloves.Cooldown;
            }
            KeyValuePair<double, CoolDownEntry> cd = ItemCooldowns.FirstOrDefault(t => t.Key == id);
            return cd.Value.DoubleCastExpiryTime - (DateTime.Now - cd.Value.DoubleCastCurrentTime).TotalSeconds;
        }

        public static double GetSpellCooldownOLD(int spell)
        {
            SpellFindResults results;
            if (!SpellManager.FindSpell(spell, out results)) return TimeSpan.MaxValue.TotalSeconds;
            return results.Override != null
                ? results.Override.CooldownTimeLeft.TotalSeconds
                : results.Original.CooldownTimeLeft.TotalSeconds;
        }


        public static bool SpellOnCooldown(string spell, bool cd = false)
        {
            if (Superbad._gcdTimeLeftTotalSeconds > 0)
                return true;
            if (cd)
                return GetSpellCooldown(spell) > 0;
            return false;
        }

        private static double GetSpellCooldown(string spell)
        {
            SpellFindResults results;
            if (!SpellManager.FindSpell(spell, out results)) return TimeSpan.MaxValue.TotalSeconds;
            return results.Override != null
                ? results.Override.CooldownTimeLeft.TotalSeconds
                : results.Original.CooldownTimeLeft.TotalSeconds;
        }

        public static double MyAuraRemains(this WoWUnit unit, int aura)
        {
            if (CurrentTargetAuras == null)
                return 0;
            WoWAura cachedResult =
                CurrentTargetAuras
                    .FirstOrDefault(
                        a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid);
            return cachedResult != null ? cachedResult.TimeLeft.TotalSeconds : 0;
        }

        public static double MyAuraRemains(this WoWUnit unit, string aura)
        {
            if (CurrentTargetAuras == null)
                return 0;
            WoWAura cachedResult =
                CurrentTargetAuras
                    .FirstOrDefault(
                        a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid);
            return cachedResult != null ? cachedResult.TimeLeft.TotalSeconds : 0;
        }

        public static double MyBuffRemains(this WoWUnit unit, int aura)
        {
            WoWAura cachedResult =
                MyAuras
                    .FirstOrDefault(
                        a => a.SpellId == aura);
            return cachedResult != null ? cachedResult.TimeLeft.TotalSeconds : 0;
        }

        public static void GatherAuras()
        {
            if (StyxWoW.Me.CurrentTarget != null)
                CurrentTargetAuras = StyxWoW.Me.CurrentTarget.GetAllAuras();
            MyAuras = StyxWoW.Me.GetAllAuras();
        }

        public static bool TargetHasAura(this WoWUnit unit, int id)
        {
            if (CurrentTargetAuras == null)
                return false;
            WoWAura aura = CurrentTargetAuras.FirstOrDefault(
                a => a.SpellId == id);
            return aura != null;
        }

        public static bool HasBuff(this WoWUnit unit, int id)
        {
            WoWAura aura = MyAuras.FirstOrDefault(
                a => a.SpellId == id);
            return aura != null;
        }

        public static bool HasMyDebuff(this WoWUnit unit, int id)
        {
            if (CurrentTargetAuras == null)
                return false;
            WoWAura aura = CurrentTargetAuras.FirstOrDefault(
                a => a.SpellId == id && a.CreatorGuid == StyxWoW.Me.Guid);
            return aura != null;
        }

        public static bool HasMyDebuffCycle(this WoWUnit unit, int id)
        {
            WoWAura aura = unit.GetAllAuras().FirstOrDefault(
                a => a.SpellId == id && a.CreatorGuid == StyxWoW.Me.Guid);
            return aura != null;
        }

        public static bool HasBuff(this WoWUnit unit, String spell)
        {
            WoWAura aura = MyAuras.FirstOrDefault(
                a => a.Name == spell);
            return aura != null;
        }

        public static bool HasMyDebuff(this WoWUnit unit, String spell)
        {
            if (CurrentTargetAuras == null)
                return false;
            WoWAura aura = CurrentTargetAuras.FirstOrDefault(
                a => a.Name == spell && a.CreatorGuid == StyxWoW.Me.Guid);
            return aura != null;
        }

        public static bool HasActiveBuff(this WoWUnit unit, int id)
        {
            KeyValuePair<string, WoWAura> aura = StyxWoW.Me.ActiveAuras.FirstOrDefault(
                a => a.Value.SpellId == id);
            return aura.Value != null;
        }

        public static bool HasAnyBuff(this WoWUnit unit, HashSet<int> id)
        {
            WoWAuraCollection auras = unit.GetAllAuras();
            return auras.Any(a => id.Contains(a.SpellId));
        }

        public static bool HasAnyBuff(this WoWUnit unit, String[] spell)
        {
            WoWAuraCollection auras = unit.GetAllAuras();
            var hashes = new HashSet<string>(spell);
            return auras.Any(a => hashes.Contains(a.Name));
        }

        public static double StackCountOnMe(this WoWUnit unit, string aura)
        {
            WoWAura cachedResult =
                MyAuras
                    .FirstOrDefault(
                        a => a.Name == aura);
            return cachedResult != null ? cachedResult.StackCount : 0;
        }

        public static double StackCount(this WoWUnit unit, string aura)
        {
            if (CurrentTargetAuras == null)
                return 0;
            WoWAura cachedResult =
                CurrentTargetAuras
                    .FirstOrDefault(
                        a => a.Name == aura);
            if (cachedResult != null)
                return cachedResult.StackCount;
            return 0;
        }

        public static bool Buff(int spell, bool cd = false)
        {
            return !SpellOnCooldown(spell, cd) &&
                   Buff(spell, StyxWoW.Me);
        }

        public static bool BuffHackInca(int spell, bool cd = false)
        {
            if (!SpellOnCooldown(spell, cd))
            {
                SpellManager.Cast(spell, StyxWoW.Me);
                Color clr = Color.Yellow;
                LogAction(WoWSpell.FromId(spell).Name, clr);
                return true;
            }
            return false;
        }

        public static bool BuffOFFGCD(int spell)
        {
            return !SpellOnCooldownOffGCD(spell) &&
                   Buff(spell, StyxWoW.Me);
        }

        public static bool Buff(string spell, bool cd = false)
        {
            return !SpellOnCooldown(spell, cd) &&
                   Buff(spell, StyxWoW.Me);
        }

        public static bool CastHack(int spell)
        {
            if (SpellOnCooldown(spell)) return false;
            SpellManager.Cast(spell, StyxWoW.Me);
            Color clr = Color.Yellow;
            LogAction(WoWSpell.FromId(spell).Name, clr);
            return true;
        }

        public static bool Buff(int spell, WoWUnit onUnit)
        {
            if (SpellManager.CanCast(spell, onUnit) || Superbad.BotbaseManualMode())
            {
                SpellManager.Cast(spell, onUnit);
                Color clr = Color.Yellow;
                LogAction(WoWSpell.FromId(spell).Name, clr);
                return true;
            }
            return false;
        }

        public static bool Buff(string spell, WoWUnit onUnit)
        {
            if (SpellManager.CanCast(spell, onUnit) || Superbad.BotbaseManualMode())
            {
                SpellManager.Cast(spell, onUnit);
                Color clr = Color.Yellow;
                LogAction(spell, clr);
                return true;
            }
            return false;
        }

        public static bool CastSymbSpell(int spell)
        {
            if (SpellOnCooldown(spell, true)) return false;
            SpellManager.Cast(spell);
            Color clr = Color.Yellow;
            LogAction(WoWSpell.FromId(spell).Name, clr);
            return true;
        }

        public static bool CastSymbSpell(int spell, WoWUnit unit)
        {
            if (SpellOnCooldown(spell, true)) return false;
            if (unit == null) return false;
            SpellManager.Cast(spell, unit);
            Color clr = Color.Red;
            LogAction(WoWSpell.FromId(spell).Name, clr);
            return true;
        }

        public static bool Cast(int spell, bool cd = false)
        {
            return !SpellOnCooldown(spell, cd) && Superbad.Facing &&
                   Cast(spell, StyxWoW.Me.CurrentTarget);
        }

        public static bool CastOFFGCD(int spell, WoWUnit target = null)
        {
            if (target != null)
                return !SpellOnCooldownOffGCD(spell) && Superbad.Facing &&
                       Cast(spell, target);
            return !SpellOnCooldownOffGCD(spell) && Superbad.Facing &&
                   Cast(spell, StyxWoW.Me.CurrentTarget);
        }

        public static bool Cast(string spell)
        {
            return !SpellOnCooldown(spell) && Superbad.Facing &&
                   Cast(spell, StyxWoW.Me.CurrentTarget);
        }

        public static bool CastHackFON(int spell)
        {
            if (!SpellOnCooldownOffGCD(spell) && StyxWoW.Me.CurrentTarget != null)
            {
                SpellManager.Cast(spell, StyxWoW.Me.CurrentTarget);
                Color clr = Color.Red;
                LogAction(WoWSpell.FromId(spell).Name, clr);
                return true;
            }
            return false;
        }

        public static bool Cast(int spell, WoWUnit onUnit)
        {
            if (SpellManager.CanCast(spell, onUnit) || Superbad.BotbaseManualMode())
            {
                SpellManager.Cast(spell, onUnit);
                Color clr = Color.Red;
                LogAction(WoWSpell.FromId(spell).Name, clr);
                return true;
            }
            return false;
        }

        public static bool Cast(string spell, WoWUnit onUnit)
        {
            if (SpellManager.CanCast(spell, onUnit) || Superbad.BotbaseManualMode())
            {
                SpellManager.Cast(spell, onUnit);
                Color clr = Color.Red;
                LogAction(spell, clr);
                return true;
            }
            return false;
        }

        public static bool CastnoFace(int spell, bool cd = false)
        {
            return !SpellOnCooldown(spell, cd) && Cast(spell, StyxWoW.Me.CurrentTarget);
        }

        public static bool Heal(int spell, bool cd = false)
        {
            return !SpellOnCooldown(spell, cd) &&
                   Heal(spell, StyxWoW.Me);
        }

        public static bool HealOFFGCD(int spell)
        {
            return !SpellOnCooldownOffGCD(spell) &&
                   Heal(spell, StyxWoW.Me);
        }

        public static bool Heal(int spell, WoWUnit onUnit)
        {
            if (SpellManager.CanCast(spell, onUnit) || Superbad.BotbaseManualMode())
            {
                SpellManager.Cast(spell, onUnit);
                Color clr = Color.Green;
                LogAction(WoWSpell.FromId(spell).Name, clr);
                return true;
            }
            return false;
        }

        public static void LogAction(string log, Color colour)
        {
            Color clr = colour;
            if (LogEntries.ContainsKey(log))
                return;
            UpdateLogEntries(log, 1.5);
            var newColor = System.Windows.Media.Color.FromArgb(clr.A, clr.R, clr.G, clr.B);
            Logging.Write(LogLevel.Normal, newColor, log);
        }

        public static float SpellDistance(this WoWUnit unit, WoWUnit other = null)
        {
            // abort if mob null
            if (unit == null)
                return 0;

            // optional arg implying Me, then make sure not Mob also
            if (other == null)
                other = StyxWoW.Me;

            // pvp, then keep it close
            float dist = other.Location.Distance(unit.Location);
            dist -= other.CombatReach + unit.CombatReach;
            return Math.Max(0, dist);
        }

        public struct CoolDownEntry
        {
            public CoolDownEntry(double spellName, double expiryTime, DateTime currentTime)
                : this()
            {
                DoubleCastSpellName = spellName;
                DoubleCastExpiryTime = expiryTime;
                DoubleCastCurrentTime = currentTime;
            }

            private double DoubleCastSpellName { [UsedImplicitly] get; set; }

            public double DoubleCastExpiryTime { get; set; }

            public DateTime DoubleCastCurrentTime { get; set; }
        }

        public struct DoubleCastSpell
        {
            public DoubleCastSpell(string spellName, double expiryTime, DateTime currentTime)
                : this()
            {
                DoubleCastSpellName = spellName;
                DoubleCastExpiryTime = expiryTime;
                DoubleCastCurrentTime = currentTime;
            }

            private string DoubleCastSpellName { [UsedImplicitly] get; set; }

            public double DoubleCastExpiryTime { get; set; }

            public DateTime DoubleCastCurrentTime { get; set; }
        }

        public struct LogEntry
        {
            public LogEntry(string spellName, double expiryTime, DateTime currentTime)
                : this()
            {
                DoubleCastSpellName = spellName;
                DoubleCastExpiryTime = expiryTime;
                DoubleCastCurrentTime = currentTime;
            }

            private string DoubleCastSpellName { [UsedImplicitly] get; set; }

            public double DoubleCastExpiryTime { get; set; }

            public DateTime DoubleCastCurrentTime { get; set; }
        }
    }
}