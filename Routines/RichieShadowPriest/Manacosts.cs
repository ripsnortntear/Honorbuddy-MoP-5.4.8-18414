using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;

namespace RichieShadowPriestPvP
{
    public class ManaCosts
    {
        private Dictionary<int, int> ManaConsts = new Dictionary<int, int>();

        public int this[SpellIDs id]
        {
            get
            { 
                int manacost = int.MaxValue;
                if (!this.ManaConsts.TryGetValue((int)id, out manacost))
                {
                    SpellFindResults spell;
                    if (SpellManager.FindSpell((int)id, out spell))
                    {
                        if (SpellManager.HasSpell((int)id))
                            this.ManaConsts.Add((int)id, manacost = (spell.Override ?? spell.Original).PowerCost);
                        else
                            this.ManaConsts.Add((int)id, manacost = int.MaxValue);
                    }
                }

                return manacost;
            }
        }
    }
}
