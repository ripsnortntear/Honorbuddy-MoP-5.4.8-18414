#region

using System.Drawing;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    internal class Trinket
    {
        public static bool UseTrinketOne()
        {
            if (!CheckTrinketOne() || StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown != 0) return false;
            StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
            Spell.LogAction(StyxWoW.Me.Inventory.Equipped.Trinket1.Name, Color.Yellow);
            return true;
        }

        private static bool CheckTrinketOne()
        {
            return StyxWoW.Me.Inventory.Equipped.Trinket1 != null &&
                   CanUseEquippedItem(StyxWoW.Me.Inventory.Equipped.Trinket1);
        }

        public static bool UseTrinketTwo()
        {
            if (!CheckTrinketTwo() || StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown != 0) return false;
            StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
            Spell.LogAction(StyxWoW.Me.Inventory.Equipped.Trinket2.Name, Color.Yellow);
            return true;
        }

        private static bool CheckTrinketTwo()
        {
            return StyxWoW.Me.Inventory.Equipped.Trinket2 != null &&
                   CanUseEquippedItem(StyxWoW.Me.Inventory.Equipped.Trinket2);
        }

        private static bool CanUseEquippedItem(WoWItem item)
        {
            var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;
            return item.Usable && item.Cooldown <= 0;
        }
    }
}