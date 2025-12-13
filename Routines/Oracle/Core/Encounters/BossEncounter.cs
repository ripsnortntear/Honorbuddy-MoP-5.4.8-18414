using System.Linq;
using Oracle.Core.CombatLog;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Core.Encounters
{
    internal abstract class BossEncounter
    {
        public bool IsCurrentBoss(int bossId)
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Any(u => u.Entry == bossId);
        }

        public abstract int BossId { get; }

        public abstract string Name { get; }

        public abstract void Shutdown();

        public abstract void Initialize();

        public abstract void HandleCombatLogEvent(CombatLogEventArgs args);
    }
}