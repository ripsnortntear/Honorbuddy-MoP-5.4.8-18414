using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals.WoWObjects;

namespace Bots.FishingBuddy
{
	static partial class Coroutines
	{
		private static readonly Styx.Common.Helpers.WaitTimer EquipThrottleTimer = new Styx.Common.Helpers.WaitTimer(System.TimeSpan.FromMilliseconds(1500));

		static void Gear_OnStart()
		{
		}

		static void Gear_OnStop()
		{
			if (Utility.EquipWeapons())
				FishingBuddyBot.Log("Equipping weapons");

			if (Utility.EquipMainHat())
				FishingBuddyBot.Log("Switched to my normal hat");
		}

		public async static Task<bool> EquipGear()
		{
			if (Me.Combat)
				return false;

			return await EquipPole() || await EquipHat();
		}

		public async static Task<bool> EquipPole()
		{
			var mainHand = StyxWoW.Me.Inventory.Equipped.MainHand;

			WoWItem pole = Me.CarriedItems
				.Where(i => i != null && i.IsValid
				   && i.ItemInfo != null
				   && i.ItemInfo.WeaponClass == WoWItemWeaponClass.FishingPole && Utility.HasRequiredSkillLevel(i.ItemInfo))
				.OrderByDescending(Utility.GetBonusFishingSkillOnEquip)
				.ThenByDescending(i => i.ItemInfo != null ? i.ItemInfo.Level : 0)
				.FirstOrDefault();

			// We are comparing Entry rather then the WoWItems instance since there can be multiple of the same item
			if (pole == null || (mainHand != null && pole.Entry == mainHand.Entry))
				return false;

			if (!EquipThrottleTimer.IsFinished)
				return false;

			EquipThrottleTimer.Reset();
			return await EquipItem(pole, WoWInventorySlot.MainHand);
		}

		public async static Task<bool> EquipHat()
		{
			var hat = Utility.GetFishingHat();

			var equippedHat = StyxWoW.Me.Inventory.Equipped.Head;

			// We are comparing Entry rather then the WoWItems instance since there can be multiple of the same item
			if (hat == null || (equippedHat != null && equippedHat.Entry == hat.Entry))
				return false;

			return Utility.EquipItem(hat, WoWInventorySlot.Head);
		}

		public async static Task<bool> EquipItem(WoWItem item, WoWInventorySlot slot)
		{
			if (!Utility.EquipItem(item, slot))
				return false;
			await CommonCoroutines.SleepForLagDuration();
			await CommonCoroutines.SleepForLagDuration();
			return true;
		}
	}
}
