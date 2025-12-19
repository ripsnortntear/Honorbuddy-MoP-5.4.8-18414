using Styx;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    public static class ComboPointsEx
    {
        public static int EffectiveCP(WoWUnit me)
        {
            int cp = 0;
            try
            {
                var lp = StyxWoW.Me;
                if (lp != null) cp = lp.ComboPoints; else cp = 0;
            }
            catch { cp = 0; }

            int stacks = 0;
            try
            {
                var a = me.GetAllAuras().Find(x => x.SpellId == SpellBook.Anticipation && x.CreatorGuid == (StyxWoW.Me != null ? StyxWoW.Me.Guid : 0UL));
                if (a != null) stacks = (int)a.StackCount;
            }
            catch { /* noop */ }
            return cp + stacks; // parité v.zip: décisions sur CP “effectifs”
        }

        public static int Current
        {
            get
            {
                try
                {
                    var lp = StyxWoW.Me;
                    return lp != null ? lp.ComboPoints : 0;
                }
                catch { return 0; }
            }
        }

        public static bool HasAnticipation(WoWUnit me)
        {
            try { return me.HasAura(SpellBook.Anticipation); } catch { return false; }
        }
    }
}
