using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace KingWoW
{
    public abstract class KingWoWAbstractBaseClass
    {
        protected static LocalPlayer Me { get { return StyxWoW.Me; } }
        public abstract bool Combat { get; }
        public abstract bool Pulse { get; }
        public abstract bool Initialize { get; }
        public abstract bool NeedRest { get; }
        public abstract bool NeedPullBuffs { get; }
        public abstract bool NeedCombatBuffs { get; }
        public abstract bool NeedPreCombatBuffs { get; }
        public abstract bool Pull { get; }
    }
}
