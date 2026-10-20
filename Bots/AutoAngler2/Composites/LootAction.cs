using System.Diagnostics;
using Styx.CommonBot.Frames;
using Styx.TreeSharp;

namespace HighVoltz.AutoAngler.Composites
{
    public class LootAction : Action
    {
		private static readonly Stopwatch _lootSw = new Stopwatch();

		public static Stopwatch WaitingForLootSW
		{
			get { return _lootSw; }
		}

        protected override RunStatus Run(object context)
        {
	        if (GetLoot())
                return RunStatus.Success;
	        return RunStatus.Failure;
        }

	    /// <summary>
        /// returns true if waiting for loot or if successfully looted.
        /// </summary>
        /// <returns></returns>
        public static bool GetLoot()
        {
            if (_lootSw.IsRunning && _lootSw.ElapsedMilliseconds < 5000)
            {
                // loot everything.
				if (AutoAnglerBot.LootFrameIsOpen)
                {
                    if (AutoAnglerBot.FishCaught == null)
                        AutoAnglerBot.FishCaught = new System.Collections.Generic.Dictionary<string, uint>();

                    if (LootFrame.Instance != null && LootFrame.Instance.IsVisible)
                    {
                        for (int i = 0; i < LootFrame.Instance.LootItems; i++)
                        {
                            LootSlotInfo lootInfo = LootFrame.Instance.LootInfo(i);
                            if (lootInfo != null && !string.IsNullOrEmpty(lootInfo.LootName))
                            {
                                if (AutoAnglerBot.FishCaught.ContainsKey(lootInfo.LootName))
                                    AutoAnglerBot.FishCaught[lootInfo.LootName] += (uint)lootInfo.LootQuantity;
                                else
                                    AutoAnglerBot.FishCaught.Add(lootInfo.LootName, (uint)lootInfo.LootQuantity);
                            }
                        }
                        LootFrame.Instance.LootAll();
                    }
                    _lootSw.Reset();
                }
                return true;
            }
            return false;
        }
    }
}