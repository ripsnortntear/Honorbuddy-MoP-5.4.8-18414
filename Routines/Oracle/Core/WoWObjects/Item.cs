#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/WoWObjects/Item.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx;
using Styx.Common.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Core.WoWObjects
{
    public enum PotionForRole
    {
        None,
        IntelligencePotion,
        SpiritPotion
    }

    [UsedImplicitly]
    internal class Item
    {
        private static readonly int _potionManaPct = OracleSettings.Instance.ManaPotionPct; // Percent to use Mana Potion.

        private static readonly WaitTimer HealthStoneCooldown = new WaitTimer(TimeSpan.FromMinutes(2));
        private static readonly WaitTimer PotionCooldown = new WaitTimer(TimeSpan.FromMinutes(1));

        public static bool CooldownFinishsWhenWeLeaveCombat { get; set; }

        private static readonly HashSet<uint> IntelligencePotions = new HashSet<uint>
        {
            76093, // Potion of the Jade Serpent -  Increases Intellect by 4000 for 25 sec.
        };

        private static readonly HashSet<uint> SpiritPotions = new HashSet<uint>
        {
            76098, // MasterManaPotion - Restores 28501 to 31500 mana.
            76092, // PotionofFocus -  Puts the imbiber in an elevated state of focus where they can restore up to [4500 * 10] mana over 10 sec
            76093, // PotionoftheJadeSerpent - Increases Intellect by 4000 for 25 sec.
            76094, // AlchemistsRejuvenation - Restores 114001 to 126000 health and 28501 to 31500 mana.
            89641, // WaterSpirit - Release the spirit, restoring 30000 mana over 6 sec.
        };

        public static Composite UsePotion()
        {
            return new PrioritySelector(
                new Action(delegate
                    {
                        UseAppropriatePotion(OracleSettings.Instance.PotionSelection);
                        return RunStatus.Failure;
                    }));
        }

        // new Action(ret => Items.UseAppropriatePotion(PotionForRole.SpiritPotion)),
        private static void UseAppropriatePotion(PotionForRole usagePotionForRole)
        {
            switch (usagePotionForRole)
            {
                case PotionForRole.None:
                    return;

                case PotionForRole.IntelligencePotion:
                    if (!CooldownFinishsWhenWeLeaveCombat && PotionCooldown.IsFinished)
                    {
                        foreach (var potion in IntelligencePotions)
                        {
                            UseItem(potion);
                            PotionCooldown.Reset();
                            CooldownFinishsWhenWeLeaveCombat = StyxWoW.Me.Combat;
                        }
                    }
                    break;

                case PotionForRole.SpiritPotion:
                    if (!CooldownFinishsWhenWeLeaveCombat && PotionCooldown.IsFinished && StyxWoW.Me.ManaPercent < _potionManaPct)
                    {
                        foreach (var potion in SpiritPotions)
                        {
                            UseItem(potion);
                            PotionCooldown.Reset();
                            CooldownFinishsWhenWeLeaveCombat = StyxWoW.Me.Combat;
                        }
                    }
                    break;
            }
        }

        // Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone")
        public static Composite UseBagItem(string name, CanRunDecoratorDelegate cond, string label)
        {
            WoWItem item = null;
            return new Decorator(
                delegate(object a)
                {
                    if (!cond(a))
                        return false;

                    if (!HealthStoneCooldown.IsFinished)
                        return false;

                    item = StyxWoW.Me.BagItems.FirstOrDefault(x => x.Name == name && x.Usable && x.Cooldown <= 0);
                    return item != null;
                },
                new Sequence(
                    new Action(delegate
                        {
                            item.UseContainerItem();

                            if (name == "Healthstone")
                                HealthStoneCooldown.Reset();

                            Logger.Output("Used {0} @ {1}", name, StyxWoW.Me.HealthPercent);

                            return RunStatus.Success;
                        })));
        }

        private static bool CanUseItem(WoWItem item)
        {
            return item != null && item.Usable && item.Cooldown <= 0;
        }

        private static WoWItem Itm(uint id)
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWItem>().FirstOrDefault(item => item.Entry == id);
        }

        private static void UseItem(uint id)
        {
            var canUseItm = CanUseItem(Itm(id));
            if (canUseItm) UseItem(Itm(id));
        }

        private static void UseItem(WoWItem item)
        {
            if (item != null)
            {
                item.Use();
            }
        }
    }
}