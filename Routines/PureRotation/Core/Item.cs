#region Revision info

/*
 * $Author: alexsteh $
 * $Date: 2013-09-05 19:36:03 +0200 (Do, 05 Sep 2013) $
 * $ID$
 * $Revision: 1715 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Core/Item.cs $
 * $LastChangedBy: alexsteh $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Linq;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using PureRotation.Helpers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Core
{
    internal static class Item
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Items - General

        /// <summary>Checks the item cooldown is 0 and its usable</summary>
        private static bool CanUseItem(WoWItem item)
        {
            return item != null && item.Usable && item.Cooldown <= 0;
        }


        /// <summary>Uses the item specified (simulated click) </summary>
        private static void UseItem(WoWItem item)
        {
            Logger.InfoLog(" [UseItem] {0} ", item.Name);
            item.Use();
        }

        #endregion Items - General

        #region Equiped Items

        /// <summary>Checks the equiped item cooldown is 0 and its usable</summary>
        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            var itemSpell = Styx.WoWInternals.Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;

            return item.Usable && item.Cooldown <= 0;
        }

        /// <summary>Creates a behavior to use an equipped item.</summary>
        private static Composite UseEquippedItem(uint slot)
        {
            return new PrioritySelector(
                ctx => StyxWoW.Me.Inventory.GetItemBySlot(slot),
                new Decorator(
                    ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                    new Action(ctx => UseItem((WoWItem)ctx))));
        }

        public static WoWItem FindFirstUsableItemById(uint ItemID)
        {
            List<WoWItem> carried = StyxWoW.Me.CarriedItems;
            return (from i in carried
                    where i != null && 
                          i.ItemInfo != null && 
                          i.Usable && 
                          i.Cooldown == 0 && 
                          i.ItemInfo.RequiredLevel <= StyxWoW.Me.Level &&
                          i.Entry==ItemID
                    orderby i.ItemInfo.Level descending
                    select i).FirstOrDefault();
        }
        public static Composite UseContainerItem(uint id)
        {
            return new PrioritySelector(
                ctx => FindFirstUsableItemById(id),
                new Decorator(
                    ret => ret != null,
                    new Sequence(
                        new Action(ret => Logger.InfoLog(String.Format("Using {0} @ {1:F1}% Health", ((WoWItem)ret).Name, StyxWoW.Me.HealthPercent ))),
                        new Action(ret => ((WoWItem)ret).UseContainerItem()),
                        Spell.CreateWaitForLagDuration()))
                        );
        }
        /// <summary>Creates a behavior to use an item, in your bags or paperdoll.</summary>
        public static Composite UseItem(uint id)
        {
            return new PrioritySelector(
                ctx => ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == id),
                new Decorator(
                    ctx => ctx != null && CanUseItem((WoWItem)ctx),
                    new Action(ctx => UseItem((WoWItem)ctx))));
        }

        public static WoWItem FindFirstUsableItemBySpell(params string[] spellNames)
        {
            List<WoWItem> carried = StyxWoW.Me.CarriedItems;
            var spellNameHashes = new HashSet<string>(spellNames);

            return (from i in carried
                    let spells = i.ItemSpells
                    where i.ItemInfo != null && spells != null && spells.Count != 0 &&
                          i.Usable &&
                        // ReSharper disable CompareOfFloatsByEqualityOperator
                          i.Cooldown == 0 &&
                        // ReSharper restore CompareOfFloatsByEqualityOperator
                          i.ItemInfo.RequiredLevel <= StyxWoW.Me.Level &&
                          spells.Any(s => s.IsValid && s.ActualSpell != null && spellNameHashes.Contains(s.ActualSpell.Name))
                    orderby i.ItemInfo.Level descending
                    select i).FirstOrDefault();
        }

        /// <summary>Uses the players engineering gloves provided the player has engineering gloves
        /// and the engineering gloves are not on cooldown and the engineering gloves are usable.</summary>
        public static Composite UseEngineerGloves()
        {
            return new PrioritySelector(
                ctx => StyxWoW.Me.Inventory.GetItemBySlot((uint)WoWInventorySlot.Hands),
                new Decorator(
                    ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                    new Action(ctx => UseItem((WoWItem)ctx))
                    )
               );
        }


        private static Composite UseEquippedTrinket2(TrinketUsage usage)
        {
            try
            {
                return new PrioritySelector(
                    new Decorator(
                        ret => usage == PRSettings.Instance.Trinket1Choice,
                        new PrioritySelector(
                            ctx => StyxWoW.Me.Inventory.GetItemBySlot((uint)WoWInventorySlot.Trinket1),
                            new Decorator(
                                ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                                new Action(ctx => UseItem((WoWItem)ctx))
                                )
                                )
                                )
                    );
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown in UseEquippedTrinket: {0}", e);
                return new PrioritySelector();
            }
        }

        #endregion Equiped Items

        #region Bag Items

        /// <summary>Returns true if the player has a mana gem in his bag.</summary>
        public static bool HaveManaGem()
        {
            return ObjectManager.GetObjectsOfType<WoWItem>(false).Any(item => item.Entry == 36799 || item.Entry == 81901);
        }

        /// <summary>Returns true if the player has the item in his bag.</summary>
        public static bool HasItemInBag(WoWItem item)
        {
            return item != null && Me.BagItems.Any(x => x.ItemInfo.Id == item.ItemInfo.Id);
        }

        /// <summary>Returns true if the player has the item in his bag.</summary>
        public static bool HasCarriedItem(WoWItem item)
        {
            return item != null && Me.CarriedItems.Any(x => x.ItemInfo.Id == item.ItemInfo.Id);
        }

        /// <summary>Attempts to use the bag item provided it is usable and its not on cooldown</summary>
        /// <param name="name">the name of the bag item to use</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The use bag item.</returns>
        public static Composite UseBagItem(string name, CanRunDecoratorDelegate cond, string label)
        {
            WoWItem item = null;
            return new Decorator(
                delegate(object a)
                {
                    if (!cond(a))
                        return false;
                    item = Me.BagItems.FirstOrDefault(x => x.Name == name && x.Usable && x.Cooldown <= 0);
                    return item != null;
                },
                new Sequence(
                    new Action(a => Logger.InfoLog(" [BagItem] {0} ", label)),
                    new Action(a => item.UseContainerItem())));
        }

        /// <summary>Attempts to use the bag item provided it is usable and its not on cooldown</summary>
        /// <param name="name">the name of the bag item to use</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The use bag item.</returns>
        public static Composite UseBagItem(int id, CanRunDecoratorDelegate cond, string label)
        {
            WoWItem item = null;
            return new Decorator(
                delegate(object a)
                {
                    if (!cond(a))
                        return false;
                    item = Me.BagItems.FirstOrDefault(x => x.Entry == id && x.Usable && x.Cooldown <= 0);
                    return item != null;
                },
                new Sequence(
                    new Action(a => Logger.InfoLog(" [BagItem] {0} ", label)),
                    new Action(a => item.UseContainerItem())));
        }

        #endregion Bag Items

        #region Trinkets

        /// <summary>Creates a composite to handle Trinkets, Potions and Tinkers.</summary>  new Action(ret => RunStatus.Failure)
        public static Composite HandleItems()
        {
            return new PrioritySelector(
                UseTrinkets(),
                new Decorator(ret => PRSettings.Instance.UseGloves && CanUseItem(StyxWoW.Me.Inventory.GetItemBySlot((uint)WoWInventorySlot.Hands)), UseEngineerGloves()));
        }

        #region PotionTestShit

        public static Composite HandlePotion()
        {
            return new PrioritySelector(CreateUsePotion());
        }

        private static Composite CreateWaitForLagDuration()
        {
            return new WaitContinue(TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150), ret => false, new ActionAlwaysSucceed());
        }

        private static Composite CreateUsePotion()
        {
            return new PrioritySelector(
                new Decorator(
                    ret =>
                    StyxWoW.Me.HasAnyAura(SpellLists.BurstHaste) && PRSettings.Instance.UsePotion, // only use them when we are hasted (bloodlust/heroism)
                    new PrioritySelector(
                        ctx => FindFirstUsableItemById(EnsureAppropriatePotion(StyxWoW.Me.HasAnyAura(SpellLists.BurstHaste))),
                // Flask of Enhancement / Flask of the North
                        new Decorator(
                            ret => ret != null,
                            new Sequence(
                                new Action(ret => Logger.InfoLog(String.Format("Using {0}", ((WoWItem)ret).Name))),
                                new Action(ret => ((WoWItem)ret).UseContainerItem()),
                                CreateWaitForLagDuration()
                                )
                            )
                        )
                    )
                );
        }

        #endregion PotionTestShit

        /// <summary>
        ///  Blows your wad all over the floor
        /// </summary>
        /// <returns>Nothing but win</returns> // && (TrinketUsageSatisfied(StyxWoW.Me.Inventory.Equipped.Trinket1) || TrinketUsageSatisfied(StyxWoW.Me.Inventory.Equipped.Trinket2))
        private static Composite UseTrinkets()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.CurrentTarget != null && Me.HealthPercent < 40, UseEquippedTrinket(TrinketUsage.LowHealth)),
                new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.IsBoss(), UseEquippedTrinket(TrinketUsage.OnBoss)),
                new Decorator(ret => true, UseEquippedTrinket(TrinketUsage.OnCooldownInCombat)));
        }

        private static Composite UseEquippedTrinket(TrinketUsage usage)
        {
            try
            {
                return new PrioritySelector(
                    new Decorator(
                        ret => usage == PRSettings.Instance.Trinket1Choice,
                        new PrioritySelector(
                            ctx => StyxWoW.Me.Inventory.GetItemBySlot((uint)WoWInventorySlot.Trinket1),
                            new Decorator(
                                ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                                new Action(ctx => UseItem((WoWItem)ctx))
                                )
                                )
                                ),
                    new Decorator(
                        ret => usage == PRSettings.Instance.Trinket2Choice,
                        new PrioritySelector(
                            ctx => StyxWoW.Me.Inventory.GetItemBySlot((uint)WoWInventorySlot.Trinket2),
                            new Decorator(
                                ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                                new Action(ctx => UseItem((WoWItem)ctx))
                                )
                                )
                                )
                    );
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown in UseEquippedTrinket: {0}", e);
                return new PrioritySelector();
            }
        }

        /// <summary> Returns true if the trinkets conditions are met</summary>
        private static bool TrinketUsageSatisfied(WoWItem trinket)
        {
            return true; // TODO: this needs to be here..there is going to be trinkets that require a condition to fire..and some will just halt the rotation like trinkets that summon minions and shit...
        }

        #endregion Trinkets

        #region Potions

        public static uint EnsureAppropriatePotion(bool cond)
        {
            if (!cond) return 0;
            uint PotionId = 0;
            SpecType s = Me.SpecType;
            WoWSpec t = Me.Specialization;
            WoWClass c = Me.Class;
            double m = Me.ManaPercent;
            if (s == SpecType.Tank)
            {
                if (c == WoWClass.Monk)
                {
                    PotionId = 76089;
                }
                else
                {
                    PotionId = 76090;
                }
            }
            if (s == SpecType.MeleeDps)
            {
                if (c==WoWClass.DeathKnight || c == WoWClass.Paladin || c == WoWClass.Warrior)
                {
                    PotionId = 76090;
                }
                else
                {
                    PotionId = 76089;
                }
            }
       
            if (s == SpecType.RangedDps)
            {
                if (c == WoWClass.Hunter)
                {
                    PotionId = 76089;
                }
                else
                {
                    PotionId = 76093;
                }
            }
            if (s == SpecType.Healer)
            {
                if (m<30)
                {
                    PotionId = 76098;
                }
                else
                {
                    PotionId = 76093;
                }
            }
            return PotionId;
        }

        #endregion Potions

        #region Tier Piece Armor Detection

        /// <summary>Checks the number of Tier pieces a player is wearing </summary>
        private static int NumTierPieces(int txxItemSetId)
        {
            {
                var
                count = Me.Inventory.Equipped.Hands != null && Me.Inventory.Equipped.Hands.ItemInfo.ItemSetId != 0 && Me.Inventory.Equipped.Hands.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Legs != null && Me.Inventory.Equipped.Legs.ItemInfo.ItemSetId != 0 && Me.Inventory.Equipped.Legs.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Chest != null && Me.Inventory.Equipped.Chest.ItemInfo.ItemSetId != 0 && Me.Inventory.Equipped.Chest.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Shoulder != null && Me.Inventory.Equipped.Shoulder.ItemInfo.ItemSetId != 0 && Me.Inventory.Equipped.Shoulder.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Head != null && Me.Inventory.Equipped.Head.ItemInfo.ItemSetId != 0 && Me.Inventory.Equipped.Head.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                return count;
            }
        }

        /// <summary>Returns true if player has 2pc Tier set Bonus</summary>
        public static bool Has2PcTeirBonus(int txxItemSetId)
        {
            {
                return NumTierPieces(txxItemSetId) >= 2;
            }
        }

        /// <summary>Returns true if player has 4pc Tier set Bonus</summary>
        public static bool Has4PcTeirBonus(int txxItemSetId)
        {
            {
                return NumTierPieces(txxItemSetId) == 4;
            }
        }

        #endregion Tier Piece Armor Detection
    }
}