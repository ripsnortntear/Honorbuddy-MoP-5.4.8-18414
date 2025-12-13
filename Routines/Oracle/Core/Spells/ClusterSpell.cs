using Oracle.Shared.Utilities.Clusters.Data;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;

namespace Oracle.Core.Spells
{
    public class ClusterSpell
    {
        public ClusterSpell(int id, SpellType type, int cSize, int gSize, int avgHp = 0, CanRunDecoratorDelegate reqs = null)
        {
            SpellId = id;
            SpellType = type;
            AvgHealthPct = avgHp;
            ClusterSize = cSize;
            GrpSize = gSize;
            SpellName = WoWSpell.FromId(SpellId).Name;
            Requirements = reqs;
        }

        public ClusterSpell(string name, SpellType type, int cSize, int gSize, int avgHp = 0, CanRunDecoratorDelegate reqs = null)
        {
            SpellName = name;
            SpellType = type;
            AvgHealthPct = avgHp;
            ClusterSize = cSize;
            GrpSize = gSize;
            SpellId = SpellManager.Spells[name].Id;
            Requirements = reqs;
        }

        public int SpellId { get; private set; }

        public int AvgHealthPct { get; private set; }

        public int ClusterSize { get; private set; }

        public int GrpSize { get; private set; }

        public SpellType SpellType { get; private set; }

        public string SpellName { get; private set; }

        public CanRunDecoratorDelegate Requirements { get; private set; }
    }
}