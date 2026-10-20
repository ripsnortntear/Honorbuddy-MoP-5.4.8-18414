using System.Linq;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace HighVoltz.AutoAngler.Composites
{
    public class EquipPoleAction : Action
    {
        private readonly System.Diagnostics.Stopwatch _equipThrottle = new System.Diagnostics.Stopwatch();

        protected override RunStatus Run(object context)
        {
            // equip fishing pole if there's none equipped
			if (StyxWoW.Me.Inventory.Equipped.MainHand == null ||
				StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo == null ||
				StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole)
            {
                if (_equipThrottle.IsRunning && _equipThrottle.ElapsedMilliseconds < 1500)
                    return RunStatus.Success;

                if (EquipPole())
                    return RunStatus.Success;
            }
            else
            {
                if (_equipThrottle.IsRunning)
                    _equipThrottle.Reset();
            }
            return RunStatus.Failure;
        }

        private bool EquipPole()
        {
			WoWItem pole = StyxWoW.Me.BagItems.FirstOrDefault(i => i != null && i.ItemInfo != null && i.ItemInfo.WeaponClass == WoWItemWeaponClass.FishingPole);
            if (pole != null)
            {
				AutoAnglerBot.Instance.Log("Equipping " + pole.Name);
                _equipThrottle.Restart();
                Utils.EquipItemByID(pole.Entry);
                return true;
            }
            return false;
        }
    }
}