/*
 * Tidy Bags v3.6.4.0 by LiquidAtoR
 *
 * This is a trivial little addon that will tidy up on-use items like Clams and
 * Borean Leather Scraps. It uses a stopwatch to stop it spamming Pulse() and
 * only runs when out of combat. Originally Created by Ryns.
 *
 * Credits to Ryns, MaiN, erenion, TIA, ShamWOW (Bobby53), Gilderoy, Samrick and Pasterke for their contributions
 * I would also like to thank everyone that has reported items that are added here in the list.
 *
 * 2014/04/08  v3.6.4.0
 *              Added a few items as requested on forums.
 *              
 * 2013/10/20  v3.6.3.9
 *				Added Handful of Treats and Loot-Filled Pumpkin (Hallow's End Event) (ShortRound).
 *				Added Darkmoon Game Prize (MaxMuster)
 *				Added several items from Pandaria, Timeless Isle and the SoO raids.
 *
 * 2013/06/18  v3.6.3.8
 *              Added Serpent's Heart (Daily Cooldown for Jewelcrafting - MoP Edition)
 *
 * 2013/06/02  v3.6.3.7
 *              Added 5.3 items to the on-use lists (request by Proto, Zamphire and Kavlantis).
 *              Added a UseOnThree part (suggestion by Proto).
 *              Added Cracked Primal Egg (request by Laria).
 *              Added Kyparite Fragment, a few new ID's for older items and a all new Heroic Cache (Heroic Scenario Reward).
 *              Added the Gold container rewards for the Brawler Guild Fights.
 *
 * 2013/05/05  v3.6.3.6
 *              Added Stormtouched Cache (Nalak World Boss Consolidation Loot)
 *
 * 2013/04/25  v3.6.3.5
 *				Cleaned up to much without able to test it, thanks ubergoober for pointing it out.
 *
 * 2013/04/14  v3.6.3.4
 *				Changed the ID's to match the new 5.2 ID's for LFR bags (Thanks Laria for reporting 1 change).
 *
 * 2013/04/05  v3.6.3.3
 *				Added Mage Hunter Personal Effects (Tumbum request).
 *
 * 2013/04/03  v3.6.3.2
 *				Added Pandaria Herbs (Laria request) and Brightly Colored Egg (flying and csl5707 request).
 *
 * 2013/03/24  v3.6.3.1
 *				Added Shiny Pile of Refuse (Oondasta's Consolidation Loot)
 *
 * 2013/03/24  v3.6.3.0
 *				Added Cache of Sha-Touched Gold (LFR Loot).
 *
 * 2013/03/21  v3.6.2.9
 *				Added Mantid Archaeology items for packaging.
 *
 * 2013/03/13  v3.6.2.8
 *				Added Arcane Trove, Fortuitous Coffer and Treasures of the Thunder King (LFR Loot).
 *
 * 2013/02/13  v3.6.2.7
 *				Moved lovely charm to use on 10 section (was in the general list).
 *
 * 2012/12/30  v3.6.2.6
 *				Added attachment to Mail Events for plugin to run (MaxMuster Request).
 *
 * 2012/12/25  v3.6.2.5
 *				Added a luastring to enable AutoLoot on every startup of TidyBags.
 *
 * 2012/12/21  v3.6.2.4
 *				Added Water Snail to the destroy items list.
 *
 * 2012/12/16  v3.6.2.3
 *				Added Dividends of the Everlasting Spring (LFR Loot).
 *
 * 2012/12/09  v3.6.2.2
 *				Added Amber Encased Treasure Pouch (LFR Loot).
 *
 * 2012/12/03  v3.6.2.1
 *				Added Silkworm Cocoon (Tailoring Imperial Silk).
 *
 * 2012/12/03  v3.6.2.0
 *				Added Greater Cache of Treasures (Scenario Reward).
 *
 * 2012/12/02  v3.6.1.9
 *				Added Cache of Mogu Riches (LFR Loot).
 *
 * 2012/11/23  v3.6.1.8
 *				Added Marauder's Gleaming Sack of Gold and Pandaren Tea Set (Archaeology).
 *				Running a test with attachment to Loot Events for plugin to run.
 *				Added destroying of grey items (credits to Pasterke).
 *
 * 2012/11/18  v3.6.1.7
 *				Added a different check to see if there's skinnables around.
 *				TidyBags will (hopefully) not run while skinning.
 *
 * 2012/11/16  v3.6.1.6
 *				Added MoP Archaeology items to be crated.
 *				Some other stuff I forgot to write down ^^
 *
 * 2012/10/05  v3.6.1.5
 *				Added Treasures of the Vale and Sparkling Shards.
 *
 * 2012/10/02  v3.6.1.4
 *				Added Sealed Crate (MoP version).
 *
 * 2012/09/29  v3.6.1.3
 *				Added few MoP items like Plump Intestines, Mote of Harmony,
 *				Cache of Treasures, Plundered Treasure, Good Fortune and Skyshards.
 *
 * 2012/09/24  v3.6.1.2
 *				Even more silly copy/paste faulty thingy.
 *
 * 2012/09/24  v3.6.1.1
 *				Re-added Strange Bloated Stomach (copy/paste faulty thingy).
 *
 * 2012/09/20  v3.6.1.0
 *				Separated items to each own list according to amounts (Samrick).
 *				Latest API changes for state of bot changed in the code.
 *
 * 2012/09/18  v3.6.0.0
 *              Applied suggested codechanges by Samrick.
 *              Temporary commented out the Styx.CommonBot.LootTargeting.LootMobs because it always returns true.
 *				This is a API flaw, since it should only return true on lootable mobs around.
 *
 * 2012/09/16  v3.5.5.1
 *              Removed references to Styx.Combat as they have been removed from the API and
 *              caused compiler errors on startup of HB.
 *
 * 2012/09/08  v3.5.5.0
 *              Adding and updating for the new API changes (Which are numerous).
 *              Also started to add requested items from the forum thread which are in-game already.
 *
 * 2012/08/28  v3.5.1.0
 *              Testing extra delays between looting corpses and activating TidyBags scripts to prevent lootbug.
 *
 * 2012/03/06  v3.5.0.8
 *              Added Sealed Crate (Darkmoon Faire Fishing) and Lovely Charm (Love is in the Air event).
 *
 * 2012/01/28  v3.5.0.7
 *              Added Canopic Jar on request of Laria.
 *              Sorted the list numerically (streamlining).
 *
 * 2011/12/26  v3.5.0.6
 *              Added Pouch and Sack o' Coins (Darkmoon Fair) on request of Laria.
 *              Added Lucky Red Envelopes (Lunar Festival Elder reward).
 *
 * 2011/12/26  v3.5.0.5
 *              Added the DragonSoul geodes on request of Laria.
 *              Added Jewelcrafting Daily Cooldowns (Brilliant Glass, Icy Prism and Fire Prism).
 *
 * 2011/11/22  v3.5.0.0
 *              Testing StyxWoW.SleepForLagDuration(); with Tidybags instead of hardcoded delays.
 *
 * 2011/10/20  v3.1.2.2
 *              Increased sleep time for special cases (Croga reported still lootbug).
 *              Added a break when loottime passes 6 seconds.
 *
 * 2011/10/19  v3.1.2.1
 *              Added Tiny Treasure Chest to the special handlers (SBS and HS) as it's soulbound too.
 *              (as reported by Inrego).
 *
 * 2011/10/19  v3.1.2.0
 *              Added accounting for player's lag while opening soulbound containers.
 *              Which could possibly prevent the lootbug appear for SBS and Hidden Stash.
 *
 * 2011/10/19  v3.1.1.3
 *              Added the CloseLoot command to the lua line for Soulbound items to use.
 *
 * 2011/10/19  v3.1.1.2
 *              Added the Hidden Stash to the same code as Strange Bloated Stomache (Lua wise).
 *              Seems that it's soulbound bags and items that are opened are the possible problem.
 *
 * 2011/10/18  v3.1.1.1
 *              Cleaned up the Strange Bloated Stomache lootcode.
 *
 * 2011/10/07  v3.1.1.0
 *              First attempt to a possible fix for the Bloated Stomach issue.
 *              This issue is reported by several people, but I can't reproduce it.
 *
 * 2011/08/30  v3.1.0.5
 *              Added Satchel of Exotic Mysteries (Level 85 LFD reward for performing missing roles in the group).
 *
 * 2011/08/27  v3.1.0.4
 *              Added Slimy Bag a per request Laria.
 *
 * 2011/06/02  v3.1.0.2
 *              Too hasty with adding stuff. Corrected as reported by Tomei.
 *
 * 2011/06/02  v3.1.0.1
 *              Added Satchels of Helpful Goods as reported by TheBuster.
 *              Added a extra satchel which was missing from the list.
 *              Added Sludge-Covered Object as reported by BlackBook.
 *              Added Hidden Stash as reported by tjhasty.
 *
 * 2011/02/02  v3.1.0.0
 *              Changed some behaviour regarding inventory checks (Only character inventory is checked now).
 *              Tested with latest test release (2.0.0.3895).
 *
 * 2010/12/28  v3.0.1.8
 *              Added the first Cataclysm Items (as reported and added by Maffyx).
 *              More items may possibly follow (when reported and tested).
 *
 * 2010/09/5   v3.0.0.0
 *              Reloaded Codebase and tested against HB 1.9.5.9 and AutoAngler9007. Success!!
 *              Added Oozing Bag and many Fished Items to the itemlist.
 *              More items may possibly follow (when reported and tested).
 *
 * 2010/06/11   v2.1.0.0 - Gilderoy update
 *              Removed the Lua code spam now the check for quantities is done internaly.
 *              Added a little log of the operation bone by the plugin.
 *
 * 2010/04/25   v2.0.0.0
 *              Adapted Code to HB2 Beta (LiquidAtoR).
 *
 * 2010/01/21   v1.3.1.0
 *              Added more items to the list, tried resolve problem with Jaggal Clam opening.
 *
 * 2009/12/26   v1.3.0.0
 *              Added more items to the list, added hack for motes & eternals.
 *
 * 2009/12/26   v1.2.0.0
 *              Added more items to the list.
 *
 * 2009/12/26   v1.1.0.0
 *              Fix for error "Missing reagent: Borean Leather Scraps" if you
 *              have >1 but <5 in your bags.
 *
 * 2009/12/26   v1.0.0.0
 *              First release of the plugin, seems to work for me!
 *
 */
namespace PluginTidyBags3
{
    using Styx;
    using Styx.Common;
    using Styx.Common.Helpers;
	using Styx.CommonBot;
    using Styx.CommonBot.Frames;
    using Styx.CommonBot.Inventory;
    using Styx.CommonBot.Profiles;
    using Styx.Helpers;
    using Styx.Pathing;
    using Styx.Plugins;
    using Styx.WoWInternals;
	using Styx.WoWInternals.Misc;
	using Styx.WoWInternals.World;
    using Styx.WoWInternals.WoWObjects;

    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
	using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Xml.Linq;

    public class TidyBags3 : HBPlugin
    {
        public override string Name { get { return "Tidy Bags 3.6 Reloaded"; } }
        public override string Author { get { return "LiquidAtoR"; } }
        public override Version Version { get { return new Version(3,6,4,0); } }
		public bool InventoryCheck = false;
		private bool _init;
		
		
        private void LootFinished(object sender, LuaEventArgs args)
        {
            if (InventoryCheck == false)
            {
                InventoryCheck = true;
            }
        }
		
		private void MailboxFinished(object sender, LuaEventArgs args)
        {
            if (InventoryCheck == false)
            {
                InventoryCheck = true;
            }
        }

        private HashSet<uint> _itemUseOnOne = new HashSet<uint>() {
            3352,  // Ooze-covered Bag
            6351,  // Dented Crate
            6352,  // Waterlogged Crate
            6353,  // Small Chest
            6356,  // Battered Chest
            6357,  // Sealed Crate
            5523,  // Small Barnacled Clam
            5524,  // Thick-shelled Clam
            7973,  // Big-mouth Clam
            13874, // Heavy Crate
            20708, // Tightly Sealed Trunk
            20766, // Slimy Bag
            20767, // Scum Covered Bag
            20768, // Oozing Bag
            21113, // Watertight Trunk
            21150, // Iron Bound Trunk
            21228, // Mithril Bound Trunk
            21746, // Lucky Red Envelope (Lunar Festival item)
            24476, // Jaggal Clam
            24881, // Satchel of Helpful Goods (5-15 1st)
            24889, // Satchel of Helpful Goods (5-15 others)
            24882, // Satchel of Helpful Goods (15-25 1st)
            24890, // Satchel of Helpful Goods (15-25 others)
            27481, // Heavy Supply Crate
            27511, // Inscribed Scrollcase
            27513, // Curious Crate
            32724, // Sludge Covered Object
			35792, // Mage Hunter Personal Effects
            35945, // Brilliant Glass (Daily Cooldown for Jewelcrafting - The Burning Crusade Edition)
            36781, // Darkwater Clam
			37586, // Handful of Treats (Hallow's End Event)
            44475, // Reinforced Crate
            44663, // Abandoned Adventurer's Satchel
            44700, // Brooding Darkwater Clam
			45072, // Brightly Colored Egg (Noble Garden Event)
            45909, // Giant Darkwater Clam
            51999, // Satchel of Helpful Goods (iLevel 25)
            52000, // Satchel of Helpful Goods (31)
            52001, // Satchel of Helpful Goods (41)
            52002, // Satchel of Helpful Goods (50)
            52003, // Satchel of Helpful Goods (57)
            52004, // Satchel of Helpful Goods (62)
            52005, // Satchel of Helpful Goods (66)
            52340, // Abyssal Clam
			54516, // Loot-Filled Pumpkin (Hallow's End Event)
            57542, // Coldridge Mountaineer's Pouch 
            61387, // Hidden Stash
            62242, // Icy Prism (Daily Cooldown for Jewelcrafting - Wrath Edition)
            64657, // Canopic Jar (Archaeology Tol'vir relic)
            67248, // Satchel of Helpful Goods (39)
            67250, // Satchel of Helpful Goods (85)
            67495, // Strange Bloated Stomach (Cataclysm Skinning)
            67539, // Tiny Treasure Chest
            67597, // Sealed Crate (level 85 version)
            69903, // Satchel of Exotic Mysteries (LFD - Extra Reward)
			72201, // Plump Intestines (MoP Skinning)
            73478, // Fire Prism (Daily Cooldown for Jewelcrafting - Cataclysm Edition)
            78890, // Crystalline Geode (Dragon Soul Raid - Normal 10/25 every bossloot)
            78891, // Elementium-Coated Geode (Dragon Soul Raid - Normal 10/25 Deathwing Kill)
            78892, // Perfect Geode (Dragon Soul Raid - Heroic Hardmode 10/25 Deathwing Kill)
            78897, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78898, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78899, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78900, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78901, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78902, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78903, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78905, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78906, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78907, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78908, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78909, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78930, // Sealed Crate (around the Darkmoon Faire Island)
			79896, // Pandaren Tea Set (Archaeology)
			79897, // Pandaren Game Board (Archaeology)
			79898, // Twin Stein Set (Archaeology)
			79899, // Walking Cane (Archaeology)
			79900, // Empty Keg (Archaeology)
			79901, // Carved Bronze Mirror (Archaeology)
			79902, // Gold-Inlaid Figurine (Archaeology)
			79903, // Apothecary Tins (Archaeology)
			79904, // Pearl of Yu'lon (Archaeology)
			79905, // Standard of Niuzao (Archaeology)
			79908, // Manacles of Rebellion (Archaeology)
			79909, // Cracked Mogu Runestone (Archaeology)
			79910, // Terracotta Arm (Archaeology)
			79911, // Petrified Bone Whip (Archaeology)
			79912, // Thunder King Insignia (Archaeology)
			79913, // Edicts of the Thunder King (Archaeology)
			79914, // Iron Amulet (Archaeology)
			79915, // Warlord's Branding Iron (Archaeology)
			79916, // Mogu Coin (Archaeology)
			79917, // Worn Monument Ledger (Archaeology)
            85224, // Basic Seed Pack
            85225, // Basic Seed Pack
            85226, // Basic Seed Pack
			87391, // Plundered Treasure (Luck of the Lotus Buff)
			88496, // Sealed Crate (MoP version)
			89610, // Pandaria Herbs (Trade for Spirit of Harmony)
			89613, // Cache of Treasures (Scenario Reward)
			89810, // Bounty of a Sundered Land (LFR Bonus Roll Gold Reward)
			90625, // Treasures of the Vale (Daily Quest Reward)
			90716, // Good Fortune (When using a Lucky Charm on a boss for loot)
			90839, // Cache of Sha-Touched Gold (World Boss gold drop)
			90840, // Marauder's Gleaming Sack of Gold (World Boss gold drop)
			92813, // Greater Cache of Treasures (Scenario Reward)
			92960, // Silkworm Cocoon (Tailoring Imperial Silk)
			93724, // Darkmoon Game Prize
			94219, // Arcane Trove (Daily Quest Reward IoTK Alliance)
			94220, // Sunreaver Bounty (Daily Quest Reward IoTK Horde)
			94296, // Cracked Primal Egg
			94566, // Fortuitous Coffer (Loot Item IoTK)
			95343, // Treasures of the Thunder King (LFR Loot)
			95469, // Serpent's Heart (Daily Cooldown for Jewelcrafting - MoP Edition)
			95601, // Shiny Pile of Refuse (World Boss drop)
			95602, // Stormtouched Cache (World Boss drop)
			95617, // Dividends of the Everlasting Spring (LFR Loot)
			95618, // Cache of Mogu Riches (LFR Loot)
			95619, // Amber Encased Treasure Pouch (LFR Loot)
			98096, // Large Sack of Coins (Brawler Fight Reward)
			98097, // Huge Sack of Coins (Brawler Fight Reward)
			98098, // Bulging Sack of Coins (Brawler Fight Reward)
			98099, // Giant Sack of Coins (Brawler Fight Reward)
			98100, // Humongous Sack of Coins (Brawler Fight Reward)
			98101, // Enormous Sack of Coins (Brawler Fight Reward)
			98102, // Overflowing Sack of Coins (Brawler Fight Reward)
			98103, // Gigantic Sack of Coins (Brawler Fight Reward)
			98133, // Greater Cache of Treasures (Scenario Reward)
			98134, // Heroic Cache of Treasures (Heroic Scenario Reward)
			98546, // Bulging Heroic Cache of Treasures (First Heroic Scenario Reward)
			98560, // Arcane Trove (Vendor Version Alliance)
			98562, // Sunreaver Bounty (Vendor Version Horde)
			103624,// Treasures of the Vale (Zone Loot)
			104034,// Purse of Timeless Coins (Timeless Isle)
			104035,// Giant Purse of Timeless Coins (Timeless Isle)
			104271,// Coalesced Turmoil (SoO LFR Loot)
			104272,// Celestial Treasure Box (Timeless Isle Loot)
			104273,// Flame-Scarred Cache of Offerings (Timeless Isle Loot)
			104275,// Twisted Treasures of the Vale (SoO LFR Loot)
			105713,// Twisted Treasures of the Vale (SoO Flex Loot)
			105714,// Coalesced Turmoil (SoO Flex Loot)
			139776,// Banner of the Mantid Empire (Archaeology)
			139779,// Ancient Sap Feeder (Archaeology)
			139780,// The Praying Mantid (Archaeology)
			139781,// Inert Sound Beacon (Archaeology)
			139782,// Remains of a Paragon (Archaeology)
			139783,// Mantid Lamp (Archaeology)
			139784,// Pollen Collector (Archaeology)
			139785 // Kypari sap Container (Archaeology)
        };

        private HashSet<uint> _itemUseOnThree = new HashSet<uint>() {
			10938, // Lesser Magic Essence
			10998, // Lesser Astral Essence
			11134, // Lesser Mystic Essence
			11174, // Lesser Nether Essence
			16202, // Lesser Eternal Essence
			22447, // Lesser Planar Essence
			34053, // Small Dream Shard
			34056, // Lesser Cosmic Essence
			52718, // Lesser Celestial Essence
            74252, // Small Ethereal Shard
            52720  // Small Heavenly Shard
        };
		
        private HashSet<uint> _itemUseOnFive = new HashSet<uint>() {
            33567 // Borean Leather Scraps
        };

        private HashSet<uint> _itemUseOnTen = new HashSet<uint>() {
            22572, // Mote of Air
            22573, // Mote of Earth
            22574, // Mote of Fire
            22575, // Mote of Life
            22576, // Mote of Mana
            22577, // Mote of Shadow
            22578, // Mote of Water
            37700, // Crystallized Air
            37701, // Crystallized Earth
            37702, // Crystallized Fire
            37703, // Crystallized Shadow
            37704, // Crystallized Life
            37705, // Crystallized Water
            49655, // Lovely Charm (Love is in the Air item)
			86547, // Skyshard
			89112, // Mote of Harmony
			90407, // Sparkling Shard (from Prospecting ores)
			97512, // Ghost Iron Nugget
			97546, // Kyparite Fragment
			97619, // Torn Green Tea Leaf
            97620, // Rain Poppy Petal
            97621, // Silkweed Stem
            97622, // Snow Lily Petal
            97623, // Fool's Cap Spores
            97624  // Desecrated Herb Pod
        };

        private HashSet<uint> _itemRequiresSleep = new HashSet<uint>() {
            61387, // Hidden Stash
            67495, // Strange Bloated Stomach (Cataclysm Skinning)
            67539, // Tiny Treasure Chest
			72201, // Plump Intestines (MoP Skinning)
			87391, // Plundered Treasure (Luck of the Lotus Buff)
			88496, // Sealed Crate (MoP version)
			89610, // Pandaria Herbs (Trade for Spirit of Harmony)
			89613, // Cache of Treasures (Scenario Reward)
			89810, // Bounty of a Sundered Land (LFR Bonus Roll Gold Reward)
			90625, // Treasures of the Vale (Daily Quest Reward)
			90716, // Good Fortune (When using a Lucky Charm on a boss for loot)
			90839, // Cache of Sha-Touched Gold (World Boss gold drop)
			90840, // Marauder's Gleaming Sack of Gold (World Boss gold drop)
			92813, // Greater Cache of Treasures (Scenario Reward)
			92960, // Silkworm Cocoon (Tailoring Imperial Silk)
			94219, // Arcane Trove (Daily Quest Reward IoTK Alliance)
			94220, // Sunreaver Bounty (Daily Quest Reward IoTK Horde)
			94296, // Cracked Primal Egg
			94566, // Fortuitous Coffer (Loot Item IoTK)
			95343, // Treasures of the Thunder King (LFR Loot)
			95601, // Shiny Pile of Refuse (World Boss drop)
			95602, // Stormtouched Cache (World Boss drop)
			95617, // Dividends of the Everlasting Spring (LFR Loot)
			95618, // Cache of Mogu Riches (LFR Loot)
			95619, // Amber Encased Treasure Pouch (LFR Loot)
			98096, // Large Sack of Coins (Brawler Fight Reward)
			98097, // Huge Sack of Coins (Brawler Fight Reward)
			98098, // Bulging Sack of Coins (Brawler Fight Reward)
			98099, // Giant Sack of Coins (Brawler Fight Reward)
			98100, // Humongous Sack of Coins (Brawler Fight Reward)
			98101, // Enormous Sack of Coins (Brawler Fight Reward)
			98102, // Overflowing Sack of Coins (Brawler Fight Reward)
			98103, // Gigantic Sack of Coins (Brawler Fight Reward)
			98133, // Greater Cache of Treasures (Scenario Reward)
			98134, // Heroic Cache of Treasures (Heroic Scenario Reward)
			98546, // Bulging Heroic Cache of Treasures (First Heroic Scenario Reward)
			98560, // Arcane Trove (Vendor Version Alliance)
			98562  // Sunreaver Bounty (Vendor Version Horde)
        };
		
		private HashSet<uint> _destroyItems = new HashSet<uint>() {
			19221, // Darkmoon Special Reserve
			19222, // Cheap Beer
			19223, // Darkmoon Dog
			19224, // Red Hot Wings
			19225, // Deep Fried Candybar
			19299, // Fizzy Faire Drink
			19300, // Bottled Winterspring Water
			19304, // Spiced Beef Jerky
			19305, // Pickled Kodo Foot
			19306, // Crunchy Frog
			21151, // Rumsey Rum Black Label
			44940, // Corn-Breaded Sausage
			44941, // Fresh-Squeezed Limeade
			45188, // Whitered Kelp
			45189, // Torn Sail
			45190, // Driftwood
			45191, // Empty Clam
			45194, // Tangled Fishing Line
			45195, // Empty Rum Bottle
			45196, // Tattered Cloth
			45197, // Tree Branch
			45198, // Weeds
			45199, // Old Boot
			45200, // Sickly Fish
			45201, // Rock
			45202, // Water Snail
			73260, // Salty Sea Dog
			74822  // Sasparilla Sinker
		};

        public override void Pulse()
        {
            if (!_init) {
            base.OnEnable();
			Lua.DoString("SetCVar('AutoLootDefault','1')");
			Lua.Events.AttachEvent("LOOT_CLOSED", LootFinished);
			Lua.Events.AttachEvent("MAIL_CLOSED", MailboxFinished);
            Logging.Write(LogLevel.Normal, Colors.DarkRed, "TidyBags 3.6 ready for use...");
            _init = true;
            }

		if (_init)
            if (StyxWoW.Me.IsActuallyInCombat
                || StyxWoW.Me.Mounted
                || StyxWoW.Me.IsDead
                || StyxWoW.Me.IsGhost
                ) {
                return;
            }

            if (InventoryCheck) { // Loot Event has Finished
                foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>()) { // iterate over every item
                    if (item != null && item.BagSlot != -1 && StyxWoW.Me.FreeNormalBagSlots >= 2) { // check if item exists and is in bag and we have space
                        if (_itemUseOnOne.Contains(item.Entry)) { // stacks of 1
                            if (item.StackCount >= 1) {
                                this.useItem(item);
                            }
						} else if (_itemUseOnThree.Contains(item.Entry)) { // stacks of 3
                            if (item.StackCount >= 3) {
                                this.useItem(item);
                            }
						} else if (_itemUseOnFive.Contains(item.Entry)) { // stacks of 5
                            if (item.StackCount >= 5) {
                                this.useItem(item);
                            }
                        } else if (_itemUseOnTen.Contains(item.Entry)) { // stacks of 10
                            if (item.StackCount >= 10) {
                                this.useItem(item);
                            }
						} else if (_destroyItems.Contains(item.Entry)) {
								this.destroyItem(item);
						}
                    }
                }
				InventoryCheck = false;
                StyxWoW.SleepForLagDuration();
            }
        }

        private void useItem(WoWItem item)
        {
            Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Using {2} we have {3}", this.Name, this.Version, item.Name, item.StackCount);
			
            if (_itemRequiresSleep.Contains(item.Entry)) {
                // some (soulbound) items require an additional sleep to prevent a loot bug
                StyxWoW.SleepForLagDuration();
            }
            Lua.DoString("UseItemByName(\"" + item.Name + "\")");
            StyxWoW.SleepForLagDuration();
        }

		private void destroyItem(WoWItem item) 
		{
			Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Destroying {2} we have {3}", this.Name, this.Version, item.Name, item.StackCount);
			item.PickUp();
			Lua.DoString("DeleteCursorItem()");
			StyxWoW.SleepForLagDuration();
		}
    }
}
