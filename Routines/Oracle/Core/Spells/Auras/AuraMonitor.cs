using Oracle.Core.CombatLog;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.Spells.Auras
{
    public class AuraMonitor
    {
        /*
         *
         * Not yet implemented..Something i put together for Weischbier, leaving it here for later use.
         *
         * AuraMonitor.GetAllAuras
         *
         * The Key is the players GUID.
         *
         */

        public static Dictionary<ulong, List<WoWAura>> GetAllAuras = new Dictionary<ulong, List<WoWAura>>();

        private static IEnumerable<WoWPlayer> SearchAreaPlayers()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>();
        }

        public AuraMonitor()
        {
            GetAllAuras = new Dictionary<ulong, List<WoWAura>>();
        }

        public void Initialize()
        {
            CombatLogHandler.Register("SPELL_AURA_APPLIED", HandleAuraUpdate);
            CombatLogHandler.Register("SPELL_AURA_REMOVED", HandleAuraUpdate);
        }

        public static IEnumerable<WoWAura> AuraMonitorAuras(WoWUnit unit)
        {
            return GetAllAuras.Where(u => u.Key == unit.Guid).SelectMany(a => a.Value).ToList();
        }

        public void ClearAll()
        {
            GetAllAuras.Clear();
        }

        public int Count
        {
            get { return GetAllAuras.Count; }
        }

        public bool ContainsPlayer(ulong key)
        {
            return GetAllAuras.ContainsKey(key);
        }

        public void Shutdown()
        {
            // this will shutdown all combat log checks ;)
            CombatLogHandler.Shutdown();
        }

        private void HandleAuraUpdate(CombatLogEventArgs args)
        {
            var guid = args.SourceGuid;

            var unit = SearchAreaPlayers().FirstOrDefault(p => p.Guid == guid);

            if (OracleRoutine.IsViable(unit))
                GetAllAuras[guid] = unit.GetAllAuras(); // UPDATE the Dictionary or add the record if its not there.
        }

        #region Aura Methods

        public static WoWAura GetAuraFromName(WoWUnit unit, string aura, bool isMyAura = false)
        {
            return isMyAura ? AuraMonitorAuras(unit).FirstOrDefault(a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : AuraMonitorAuras(unit).FirstOrDefault(a => a.Name == aura && a.TimeLeft > TimeSpan.Zero);
        }

        public static WoWAura GetAuraFromId(WoWUnit unit, int aura, bool isMyAura = false)
        {
            return isMyAura ? AuraMonitorAuras(unit).FirstOrDefault(a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid && a.TimeLeft > TimeSpan.Zero) : AuraMonitorAuras(unit).FirstOrDefault(a => a.SpellId == aura && a.TimeLeft > TimeSpan.Zero);
        }

        public static bool HasAura(WoWUnit unit, string aura, int stacks = 0, bool isMyAura = false, int msLeft = 0)
        {
            if (!OracleRoutine.IsViable(unit)) return false;

            var result = GetAuraFromName(unit, aura, isMyAura);

            if (result == null)
                return false;

            if (result.TimeLeft.TotalMilliseconds > msLeft)
                return result.StackCount >= stacks;

            return false;
        }

        public static bool HasAura(WoWUnit unit, int aura, int stacks = 0, bool isMyAura = false, int msLeft = 0)
        {
            if (!OracleRoutine.IsViable(unit)) return false;

            var result = GetAuraFromId(unit, aura, isMyAura);

            if (result == null)
                return false;

            if (result.TimeLeft.TotalMilliseconds > msLeft)
                return result.StackCount >= stacks;

            return false;
        }

        public static bool HasMyAura(string aura, WoWUnit u)
        {
            if (!OracleRoutine.IsViable(u)) return false;
            return AuraMonitorAuras(u).Any(a => a.Name == aura && a.CreatorGuid == StyxWoW.Me.Guid);
        }

        public static bool HasMyAura(int aura, WoWUnit u)
        {
            if (!OracleRoutine.IsViable(u)) return false;
            return AuraMonitorAuras(u).Any(a => a.SpellId == aura && a.CreatorGuid == StyxWoW.Me.Guid);
        }

        #endregion Aura Methods
    }
}