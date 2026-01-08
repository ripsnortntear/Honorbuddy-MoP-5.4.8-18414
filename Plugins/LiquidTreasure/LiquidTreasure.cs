/*
* LiquidTreasure v3.0.0.7 by LiquidAtoR
*
* This is a little addon that will approach world treasure chests.
* It will open them and loot the content (and confirm any BoP messages in the process).
* All currently knows treasure chests are supported.
* If any are missing report em in the release thread (No locked chests supported).
* There are no configuration settins in this code, so don't mess in it.
*
* Credits to HighVoltz for the codebase of this addon (The Egg Collector).
* This plugin was created on request of Fluffyhusky on the HB forums.
* Special thanks to Chinajade and Thephoenix25 for their help with the CanFly part.
* Special Thanks to Hazard for the idea and tiagofmcosta to improve on that idea.
*
* 2014/11/01 v3.0.0.7
* init change for new HB
*
* 2014/04/08 v3.0.0.6
* Small changes to account for API changes
*
* 2013/06/18 v3.0.0.5
* Small changes to make it run smoother.
*
* 2013/03/13 v3.0.0.4
* Added Trove of the Thunder King.
*
* 2012/12/30 v3.0.0.3
* Just cleaned up the code and added regions so it's easier to debug it in MVS.
* /AknA
*
* 2012/12/25 v3.0.0.2
* Added the changes in reputation checks.
*
* 2012/11/23 v3.0.0.1
* Added all Tiller friends faction ID's to the darksoil check.
* Let's hope the HB API supports the factions.
* Also added a BoP confirm lua string to the Dark Soil loot.
*
* 2012/11/18 v3.0.0.0
* Changed navigation codes per suggestion.
* Added reputation based gathers (thanks tiagofmcosta for the pasty).
* Added a levelcheck for Darksoil (have to be level 90 to collect em).
*
* 2012/10/08 v2.5.0.0
* Updated to the new HB API for MoP.
* Added loads of MoP BoP/BoE/BoA and valuable trash items from MoP expansion.
* Splitted the list in few parts, 1 for BoE items and 1 for confirmation on loot items.
*
* 2011/10/20 v2.0.0.0
* If you can fly, you will fly.
* Removed a chest which was a questreward in Booty Bay
*
* 2011/10/06 v1.3.1.0
* Changed the logging to show chest's Name + ID
*
* 2011/09/11 v1.3.0.0
* Added a check before mounting for travel to treasure chest location.
* Thanks to Tozedeado for reporting it.
*
* 2011/09/10 v1.2.0.0
* Added most Treasure Chest ID's.
* Thanks to Tozedeado for providing them.
*
* 2011/09/10 v1.1.0.0
* Added logging on request of Tozedeado.
*
* 2011/08/27 v1.0.0.0
* First release of the plugin, untested!
*
*/

namespace PluginLiquidTreasure3
{
    #region Styx Namespace
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
    #endregion Styx Namespace

    #region System Namespace
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
    #endregion System Namespace

    internal class LiquidTreasure3 : HBPlugin {
        public override string Name { get { return "LiquidTreasure 3.0"; } }
        public override string Author { get { return "LiquidAtoR"; } }
        public override Version Version { get { return new Version(3, 0, 0, 6); } }
        private bool _init;
        private const int MinimumReputationForExalted = 21000;
        private const int MinimumReputationForBestFriends = 42000;

        public override void Pulse() {
			if (!_init) {
				base.OnEnable();
				Logging.Write(LogLevel.Normal, Colors.DarkRed, "LiquidTreasure 3.0 ready for use...");
				_init = true;
				}

            if (_init) {
                try {
                    if (!StyxWoW.Me.IsActuallyInCombat || !StyxWoW.Me.IsDead) {
                        OnyxEgg();
                        NetherwingEgg();
                        DarkSoil();
                        AncientGuoLaiCache();
                        PickUpBOETreasure();
                        PickUpBOPTreasure();
                    }
                }
                catch (ThreadAbortException) {
                }
            }
        }

        #region AncientGuoLaiCacheKey
            private static bool AncientGuoLaiCacheKey() { 
                return StyxWoW.Me.BagItems.FirstOrDefault(o => o.Entry == 87779) != null; 
            }
        #endregion AncientGuoLaiCacheKey

        #region MoveToLocation
            public static void MoveToLocation(WoWPoint loc) {
                while (loc.Distance(StyxWoW.Me.Location) > 3) {
                    if (!Flightor.MountHelper.Mounted) {
                        Flightor.MountHelper.MountUp();
                    }
                    if (!StyxWoW.Me.IsMoving) {
                        Flightor.MoveTo(loc);
                    }
                }
            }
        #endregion MoveToLocation

        #region OnyxEgg
            public static void OnyxEgg() {
                if (StyxWoW.Me.GetReputationWith(1271) > MinimumReputationForExalted) {
                    return;
                }
                ObjectManager.Update();
                var objList = ObjectManager.GetObjectsOfType<WoWGameObject>().Where(onyxegg => (onyxegg.Distance2D 
                    <= Styx.CommonBot.LootTargeting.LootRadius && (onyxegg.Entry == 214945) && onyxegg.CanUse()))
                    .OrderBy(onyxegg => onyxegg.Distance).ToList();
                foreach (var onyxegg in objList) {
                    if (!onyxegg.InLineOfSight) {
                        return;
                    }
                    if (StyxWoW.Me.Combat) {
                        return;
                    }
                    WoWMovement.MoveStop();
                    MoveToLocation(WoWMovement.CalculatePointFrom(onyxegg.Location, 3));
                    if (!StyxWoW.Me.HasAura(40120) && !StyxWoW.Me.HasAura(33943)) {
                        Flightor.MountHelper.Dismount();
                    }
                    Thread.Sleep(1000);
                    onyxegg.Interact();
                    Thread.Sleep(2000);
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[LiquidTreasure 2]: Opened a {0} with ID {1}", onyxegg.Name, onyxegg.Entry);
                    if (!Flightor.MountHelper.Mounted) {
                        Flightor.MountHelper.MountUp();
                    }
                    return;
                }
            }
        #endregion OnyxEgg

        #region NetherwingEgg
            public static void NetherwingEgg()
            {
                if (StyxWoW.Me.GetReputationWith(1015) > MinimumReputationForExalted)
                {
                    return;
                }
                ObjectManager.Update();
                var objList = ObjectManager.GetObjectsOfType<WoWGameObject>().Where(netherwingegg => (netherwingegg.Distance2D
                    <= Styx.CommonBot.LootTargeting.LootRadius && (netherwingegg.Entry == 185915) && netherwingegg.CanUse()))
                    .OrderBy(netherwingegg => netherwingegg.Distance).ToList();
                foreach (var netherwingegg in objList)
                {
                    if (!netherwingegg.InLineOfSight)
                    {
                        return;
                    }
                    if (StyxWoW.Me.Combat)
                    {
                        return;
                    }
                    WoWMovement.MoveStop();
                    MoveToLocation(WoWMovement.CalculatePointFrom(netherwingegg.Location, 3));
                    if (!StyxWoW.Me.HasAura(40120) && !StyxWoW.Me.HasAura(33943))
                    {
                        Flightor.MountHelper.Dismount();
                    }
                    Thread.Sleep(1000);
                    netherwingegg.Interact();
                    Thread.Sleep(2000);
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[LiquidTreasure 2]: Opened a {0} with ID {1}", netherwingegg.Name, netherwingegg.Entry);
                    if (!Flightor.MountHelper.Mounted)
                    {
                        Flightor.MountHelper.MountUp();
                    }
                    return;
                }
            }
            #endregion NetherwingEgg

        #region DarkSoil
            public static void DarkSoil() {
                if ((StyxWoW.Me.GetReputationWith(1273) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1275) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1276) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1277) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1278) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1279) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1280) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1281) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1282) > MinimumReputationForBestFriends) &&
					(StyxWoW.Me.GetReputationWith(1283) > MinimumReputationForBestFriends) ||
					(StyxWoW.Me.LevelFraction < 90)) {
                    return;
                }
                ObjectManager.Update();
                var objList = ObjectManager.GetObjectsOfType<WoWGameObject>().Where(darksoil => (darksoil.Distance2D 
                    <= Styx.CommonBot.LootTargeting.LootRadius && (darksoil.Entry == 210565) && darksoil.CanUse()))
                    .OrderBy(darksoil => darksoil.Distance).ToList();
                foreach (var darksoil in objList) {
                    if (!darksoil.InLineOfSight) {
                        return;
                    }
                    if (StyxWoW.Me.Combat) {
                        return;
                    }
                    WoWMovement.MoveStop();
                    MoveToLocation(WoWMovement.CalculatePointFrom(darksoil.Location, 3));
                    if (!StyxWoW.Me.HasAura(40120) && !StyxWoW.Me.HasAura(33943)) {
                        Flightor.MountHelper.Dismount();
                    }
                    Thread.Sleep(2000);
                    darksoil.Interact();
                    Thread.Sleep(3000);
                    Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[LiquidTreasure 2]: Opened a {0} with ID {1}", darksoil.Name, darksoil.Entry);
                    if (!Flightor.MountHelper.Mounted) {
                        Flightor.MountHelper.MountUp();
                    }
                    return;
                }
            }
        #endregion DarkSoil

        #region AncientGuoLaiCache
            public static void AncientGuoLaiCache() {
                if (StyxWoW.Me.GetReputationWith(1269) > MinimumReputationForExalted){
                    return;
                }
                ObjectManager.Update();
                var objList = ObjectManager.GetObjectsOfType<WoWGameObject>().Where(ancientguolaicache => (ancientguolaicache.Distance2D 
                    <= Styx.CommonBot.LootTargeting.LootRadius && (ancientguolaicache.Entry == 214388)))
                    .OrderBy(ancientguolaicache => ancientguolaicache.Distance).ToList();
                foreach (var ancientguolaicache in objList) {
                    if (!AncientGuoLaiCacheKey()) {
                        return;
                    }
                    if (!ancientguolaicache.InLineOfSight) {
                        return;
                    }
                    if (StyxWoW.Me.Combat) {
                        return;
                    }
                    WoWMovement.MoveStop();
                    MoveToLocation(WoWMovement.CalculatePointFrom(ancientguolaicache.Location, 3));
                    if (!StyxWoW.Me.HasAura(40120) && !StyxWoW.Me.HasAura(33943)) {
                        Flightor.MountHelper.Dismount();
                    }
                    Thread.Sleep(1000);
                    ancientguolaicache.Interact();
                    Thread.Sleep(3000);
                    Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[LiquidTreasure 2]: Opened a {0} with ID {1}", ancientguolaicache.Name, ancientguolaicache.Entry);
                    if (!Flightor.MountHelper.Mounted) {
                        Flightor.MountHelper.MountUp();
                    }
                    return;
                }
            }
        #endregion AncientGuoLaiCache

        #region PickUpBOETreasure
            public static void PickUpBOETreasure() {
                ObjectManager.Update();
                List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(boe => (boe.Distance2D <= Styx.CommonBot.LootTargeting.LootRadius &&
                (boe.Entry == 176944) || // Old Treasure Chest (Scholomance Instance)
                (boe.Entry == 179697) || // Arena Treasure Chest (STV Arena)
                (boe.Entry == 203090) || // Sunken Treaure Chest
                (boe.Entry == 207472) || // Silverbound Treasure Chest (Zone 1)
                (boe.Entry == 207473) || // Silverbound Treasure Chest (Zone 2)
                (boe.Entry == 207474) || // Silverbound Treasure Chest (Zone 3)
                (boe.Entry == 207475) || // Silverbound Treasure Chest (Zone 4)
                (boe.Entry == 207476) || // Silverbound Treasure Chest (Zone 5)
                (boe.Entry == 207477) || // Silverbound Treasure Chest (Zone 6)
                (boe.Entry == 207478) || // Silverbound Treasure Chest (Zone 7)
                (boe.Entry == 207479) || // Silverbound Treasure Chest (Zone 8)
                (boe.Entry == 207480) || // Silverbound Treasure Chest (Zone 9)
                (boe.Entry == 207484) || // Sturdy Treasure Chest (Zone 1)
                (boe.Entry == 207485) || // Sturdy Treasure Chest (Zone 2)
                (boe.Entry == 207486) || // Sturdy Treasure Chest (Zone 3)
                (boe.Entry == 207487) || // Sturdy Treasure Chest (Zone 4)
                (boe.Entry == 207488) || // Sturdy Treasure Chest (Zone 5)
                (boe.Entry == 207489) || // Sturdy Treasure Chest (Zone 6)
                (boe.Entry == 207492) || // Sturdy Treasure Chest (Zone 7)
                (boe.Entry == 207493) || // Sturdy Treasure Chest (Zone 8)
                (boe.Entry == 207494) || // Sturdy Treasure Chest (Zone 9)
                (boe.Entry == 207495) || // Sturdy Treasure Chest (Zone 10)
                (boe.Entry == 207496) || // Dark Iron Treasure Chest (Zone 1)
                (boe.Entry == 207497) || // Dark Iron Treasure Chest (Zone 2)
                (boe.Entry == 207498) || // Dark Iron Treasure Chest (Zone 3)
                (boe.Entry == 207500) || // Dark Iron Treasure Chest (Zone 4)
                (boe.Entry == 207507) || // Dark Iron Treasure Chest (Zone 5)
                (boe.Entry == 207512) || // Silken Treasure Chest (Zone 1)
                (boe.Entry == 207513) || // Silken Treasure Chest (Zone 2)
                (boe.Entry == 207517) || // Silken Treasure Chest (Zone 3)
                (boe.Entry == 207518) || // Silken Treasure Chest (Zone 4)
                (boe.Entry == 207519) || // Silken Treasure Chest (Zone 5)
                (boe.Entry == 207520) || // Maplewood Treasure Chest (Zone 1)
                (boe.Entry == 207521) || // Maplewood Treasure Chest (Zone 2)
                (boe.Entry == 207522) || // Maplewood Treasure Chest (Zone 3)
                (boe.Entry == 207523) || // Maplewood Treasure Chest (Zone 4)
                (boe.Entry == 207524) || // Maplewood Treasure Chest (Zone 5)
                (boe.Entry == 207528) || // Maplewood Treasure Chest (Zone 6)
                (boe.Entry == 207529) || // Maplewood Treasure Chest (Zone 7)
                (boe.Entry == 207533) || // Runestone Treasure Chest (Zone 1)
                (boe.Entry == 207534) || // Runestone Treasure Chest (Zone 2)
                (boe.Entry == 207535) || // Runestone Treasure Chest (Zone 3)
                (boe.Entry == 207540) || // Runestone Treasure Chest (Zone 4)
                (boe.Entry == 207542) || // Runestone Treasure Chest (Zone 5)
                (boe.Entry == 213362) || // Ship's Locker (Contains ~ 96G)
                (boe.Entry == 213650) || // Virmen Treasure Cache (Contains ~ 100G)
                (boe.Entry == 213769) || // Hozen Treasure Cache (Contains ~ 100G)
                (boe.Entry == 213770) || // Stolen Sprite Treasure (Contains ~ 105G)
                (boe.Entry == 213774) || // Lost Adventurer's Belongings (Contains ~ 100G)
                (boe.Entry == 213961) || // Abandoned Crate of Goods (Contains ~ 100G)
                (boe.Entry == 214325) || // Forgotten Lockbox (Contains ~ 10G)
                (boe.Entry == 214407) || // Mo-Mo's Treasure Chest (Contains ~ 9G)
                (boe.Entry == 214337) || // Stash of Gems (few green uncut MoP gems and ~ 7G)
                (boe.Entry == 214337)))  // Offering of Rememberance (Contains ~ 30G and debuff turns you grey)
                .OrderBy(boe => boe.Distance).ToList();
                foreach (WoWGameObject boe in objList) {
                    if (!boe.InLineOfSight) {
                        return;
                    }
                    if (StyxWoW.Me.Combat) {
                        return;
                    }
                    WoWMovement.MoveStop();
                    MoveToLocation(WoWMovement.CalculatePointFrom(boe.Location, 3));
                    if (!StyxWoW.Me.HasAura(40120) && !StyxWoW.Me.HasAura(33943)) {
                        Flightor.MountHelper.Dismount();
                    }
                    Thread.Sleep(1000);
                    boe.Interact();
                    Thread.Sleep(3000);
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[LiquidTreasure 2]: Opened a {0} with ID {1}", boe.Name, boe.Entry);
                    if (!Flightor.MountHelper.Mounted) {
                        Flightor.MountHelper.MountUp();
                    }
                    return;
                }
            }
        #endregion PickUpBOETreasure

        #region PickUpBOPTreasure
            public static void PickUpBOPTreasure() {
                ObjectManager.Update();
                List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(bop => (bop.Distance2D <= Styx.CommonBot.LootTargeting.LootRadius &&
                (bop.Entry == 213363) || // Wodin's Mantid Shanker
                (bop.Entry == 213364) || // Ancient Pandaren Mining Pick
                (bop.Entry == 213366) || // Ancient Pandaren Tea Pot (Grey trash worth 100G)
                (bop.Entry == 213368) || // Lucky Pandaren Coin (Grey trash worth 95G)
                (bop.Entry == 213649) || // Cache of Pilfered Goods
                (bop.Entry == 213653) || // Pandaren Fishing Spear
                (bop.Entry == 213741) || // Ancient Jinyu Staff
                (bop.Entry == 213742) || // Hammer of Ten Thunders
                (bop.Entry == 213748) || // Pandaren Ritual Stone (Grey trash worth 105G)
                (bop.Entry == 213749) || // Staff of the Hidden Master
                (bop.Entry == 213750) || // Saurok Stone Tablet (Grey trash worth 100G)
                (bop.Entry == 213751) || // Sprite's Cloth Chest
                (bop.Entry == 213765) || // Tablet of Ren Yun (Cooking Recipy)
                (bop.Entry == 213768) || // Hozen Warrior Spear
                (bop.Entry == 213771) || // Statue of Xuen (Grey trash worth 100G)
                (bop.Entry == 213782) || // Terracotta Head (Grey trash worth 100G)
                (bop.Entry == 213793) || // Riktik's Tiny Chest (Grey trash worth 105G)
                (bop.Entry == 213842) || // Stash of Yaungol Weapons
                (bop.Entry == 213844) || // Amber Encased Moth (Grey trash worth 105G)
                (bop.Entry == 213845) || // The Hammer of Folly (Grey trash worth 100G)
                (bop.Entry == 213956) || // Fragment of Dread (Grey trash worth 90G)
                (bop.Entry == 213959) || // Hardened Sap of Kri'vess (Grey trash worth 110G)
                (bop.Entry == 213960) || // Yaungol Fire Carrier
                (bop.Entry == 213962) || // Wind-Reaver's Dagger of Quick Strikes
                (bop.Entry == 213964) || // Malik's Stalwart Spear
                (bop.Entry == 213966) || // Amber Encased Necklace
                (bop.Entry == 213967) || // Blade of the Prime
                (bop.Entry == 213968) || // Swarming Cleaver of Ka'roz
                (bop.Entry == 213969) || // Dissector's Staff of Mutilation
                (bop.Entry == 213970) || // Bloodsoaked Chitin Fragment
                (bop.Entry == 213972) || // Blade of the Poisoned Mind
                (bop.Entry == 214340) || // Boat-Building Instructions (Grey trash worth 10G)
                (bop.Entry == 214438) || // Ancient Mogu Tablet (Grey trash worth 95G)
                (bop.Entry == 214439) || // Barrel of Banana Infused Rum (Cooking Recipy and Rum)
				(bop.Entry == 218593)))  // Trove of the Thunder King (IoTK chest containing a BoP item)
                .OrderBy(bop => bop.Distance).ToList();
                foreach (WoWGameObject bop in objList) {
                    if (!bop.InLineOfSight) {
                        return;
                    }
                    if (StyxWoW.Me.Combat) {
                        return;
                    }
                    WoWMovement.MoveStop();
                    MoveToLocation(WoWMovement.CalculatePointFrom(bop.Location, 3));
                    if (!StyxWoW.Me.HasAura(40120) && !StyxWoW.Me.HasAura(33943)) {
                        Flightor.MountHelper.Dismount();
                    }
                    Thread.Sleep(1000);
                    bop.Interact();
                    Thread.Sleep(3000);
                    Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");
                    Logging.Write(LogLevel.Normal, Colors.DarkRed, "[LiquidTreasure 2]: Opened a {0} with ID {1}", bop.Name, bop.Entry);
                    if (!Flightor.MountHelper.Mounted) {
                        Flightor.MountHelper.MountUp();
                    }
                    return;
                }
            }
        #endregion PickUpBOPTreasure
    }
}