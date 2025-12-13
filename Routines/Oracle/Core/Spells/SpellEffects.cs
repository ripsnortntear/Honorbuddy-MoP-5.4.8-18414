#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Spells/SpellEffects.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Linq;

namespace Oracle.Core.Spells
{
    [UsedImplicitly]
    internal class SpellEffects
    {
        #region Ground Effects

        public static WoWDynamicObject GetGroundEffectBySpellId(int spellId)
        {
            return ObjectManager.GetObjectsOfType<WoWDynamicObject>().FirstOrDefault(o => o.SpellId == spellId);
        }

        public static bool IsStandingInGroundEffect(bool harmful = true)
        {
            foreach (var obj in ObjectManager.GetObjectsOfType<WoWDynamicObject>().Where(obj => obj.Distance <= obj.Radius))
            {
                // We're standing in this.
                if (obj.Caster.IsFriendly && !harmful)
                    return true;
                if (obj.Caster.IsHostile && harmful)
                    return true;
            }
            return false;
        }

        #endregion Ground Effects
    }
}