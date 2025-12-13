#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Managers/TrinketManager.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info


using Oracle.Core.DataStores;
using Oracle.Shared.Utilities;
using Oracle.UI.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Core.Managers
{
    public enum TrinketUsage
    {
        Never,
        Always,
        OnBoss,
        LowHealth,
        LowMana
    }

    internal static class TrinketManager
    {
        #region Item Wrappers

        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;

            return item.Usable && item.Cooldown <= 0;
        }

        private static void UseTrinkets()
        {
            if (OracleSettings.Instance.FirstTrinketUsage == TrinketUsage.Never &&
                OracleSettings.Instance.SecondTrinketUsage == TrinketUsage.Never) return;

            var firstTrinket = StyxWoW.Me.Inventory.Equipped.Trinket1;
            var secondTrinket = StyxWoW.Me.Inventory.Equipped.Trinket2;

            if (CanUseTrinket(OracleSettings.Instance.FirstTrinketUsage, firstTrinket))
                firstTrinket.Use();

            if (CanUseTrinket(OracleSettings.Instance.SecondTrinketUsage, secondTrinket))
                secondTrinket.Use();
        }

        #endregion Item Wrappers

        private static bool CanUseTrinket(TrinketUsage usage, WoWItem trinket)
        {
            var canUseTrinket = trinket != null && CanUseEquippedItem(trinket);

            switch (usage)
            {
                case TrinketUsage.OnBoss:
                    return canUseTrinket && BossList.IsBossNearby;
                case TrinketUsage.LowHealth:
                    return canUseTrinket && StyxWoW.Me.HealthPercent <= OracleSettings.Instance.TrinketHealthPct;
                case TrinketUsage.LowMana:
                    return canUseTrinket && StyxWoW.Me.ManaPercent <= OracleSettings.Instance.TrinketManaPct;
                case TrinketUsage.Always:
                    return canUseTrinket;
            }
            return false;
        }

        public static Composite CreateTrinketBehaviour()
        {
            return new Action(ret =>
                {
                    using (new PerformanceLogger("CreateTrinketBehaviour"))
                    {
                        UseTrinkets();
                    }
                    return RunStatus.Failure;
                });
        }
    }
}