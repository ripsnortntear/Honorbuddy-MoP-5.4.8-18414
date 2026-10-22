--[[
************************************************************************
MountDB.lua
************************************************************************
File date: 2014-06-12T00:47:49Z
File hash: 65bbbac
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/collectinator/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]] --

-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub	= _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)

-------------------------------------------------------------------------------
-- Filter flags. Acquire types, and Reputation levels.
-------------------------------------------------------------------------------
local A = private.ACQUIRE_TYPES
local F = private.FILTER_IDS
local PROF = private.LOCALIZED_PROFESSION_NAMES
local Q = private.ITEM_QUALITIES
local REP = private.REP_LEVELS
local FAC = private.FACTION_IDS
local V = private.GAME_VERSIONS
local Z = private.ZONE_NAMES

--------------------------------------------------------------------------------------------------------------------
-- Initialize!
--------------------------------------------------------------------------------------------------------------------

function addon:InitMounts()
	local function AddMount(spell_id, genesis, quality)
		return addon:AddCollectable(spell_id, "MOUNT", genesis, quality)
	end

	local mount


-- Classic

	-- Brown Horse Bridle - 458
	mount = AddMount(458, V.ORIG, Q.RARE) -- Item: 5656
	mount:SetItemID(5656)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(384, 1460, 4885, 43694)

	-- Black Stallion Bridle - 470
	mount = AddMount(470, V.ORIG, Q.RARE) -- Item: 2411
	mount:SetItemID(2411)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1460, 43694)

	-- Pinto Bridle - 472
	mount = AddMount(472, V.ORIG, Q.RARE) -- Item: 2414
	mount:SetItemID(2414)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(384, 1460, 4885, 43694)

	-- Timber Wolf -- 580
	mount = AddMount(580, V.ORIG, Q.RARE) -- Item: 1132
	mount:SetItemID(1132)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Felsteed - 5784
	mount = AddMount(5784, V.ORIG, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TRAINER)
	mount:SetRequiredClass("WARLOCK")
	mount:AddTrainer(461, 906, 2127, 3172, 3324, 3325, 3326, 4565, 5171, 5172, 5495, 5496, 6251, 35778, 36519, 45720, 49998, 50028, 50142, 50156, 50502, 50729)

	-- Chestnut Mare Bridle - 6648
	mount = AddMount(6648, V.ORIG, Q.RARE) -- Item: 5655
	mount:SetItemID(5655)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(384, 1460, 4885, 43694)

	-- Dire Wolf -- 6653
	mount = AddMount(6653, V.ORIG, Q.RARE) -- Item: 5665
	mount:SetItemID(5665)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Brown Wolf -- 6654
	mount = AddMount(6654, V.ORIG, Q.RARE) -- Item: 5668
	mount:SetItemID(5668)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Gray Ram - 6777
	mount = AddMount(6777, V.ORIG, Q.RARE) -- Item: 5864
	mount:SetItemID(5864)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1261)

	-- White Ram - 6898
	mount = AddMount(6898, V.ORIG, Q.RARE) -- Item: 5873
	mount:SetItemID(5873)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1261)

	-- Brown Ram - 6899
	mount = AddMount(6899, V.ORIG, Q.RARE) -- Item: 5872
	mount:SetItemID(5872)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1261)

	-- Striped Frostsaber - 8394
	mount = AddMount(8394, V.ORIG, Q.RARE) -- Item: 8631
	mount:SetItemID(8631)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Emerald Raptor - 8395
	mount = AddMount(8395, V.ORIG, Q.RARE) -- Item: 8588
	mount:SetItemID(8588)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(7952)

	-- Spotted Frostsaber - 10789
	mount = AddMount(10789, V.ORIG, Q.RARE) -- Item: 8632
	mount:SetItemID(8632)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Striped Nightsaber - 10793
	mount = AddMount(10793, V.ORIG, Q.RARE) -- Item: 8629
	mount:SetItemID(8629)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Turquoise Raptor - 10796
	mount = AddMount(10796, V.ORIG, Q.RARE) -- Item: 8591
	mount:SetItemID(8591)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(7952)

	-- Violet Raptor - 10799
	mount = AddMount(10799, V.ORIG, Q.RARE) -- Item: 8592
	mount:SetItemID(8592)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(7952)

	-- Red Mechanostrider - 10873
	mount = AddMount(10873, V.ORIG, Q.RARE) -- Item: 8563
	mount:SetItemID(8563)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Blue Mechanostrider - 10969
	mount = AddMount(10969, V.ORIG, Q.RARE) -- Item: 8595
	mount:SetItemID(8595)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Warhorse (Alliance) -- 13819
	mount = AddMount(13819, V.ORIG, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("Human", "Dwarf")
	mount:SetRequiredFaction("Alliance")
	mount:AddTrainer(927, 928, 1232, 5148, 5149, 5491, 17844, 49793, 50023, 50160)

	-- White Mechanostrider Mod B - 15779
	mount = AddMount(15779, V.ORIG, Q.EPIC) -- Item: 13326
	mount:SetItemID(13326)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Black Nightsaber - 16055
	mount = AddMount(16055, V.ORIG, Q.EPIC) -- Item: 12303
	mount:SetItemID(12303)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Ancient Frostsaber - 16056
	mount = AddMount(16056, V.ORIG, Q.EPIC) -- Item: 12302
	mount:SetItemID(12302)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Horn of the Red Wolf - 16080
	mount = AddMount(16080, V.ORIG, Q.EPIC) -- Item: 12330
	mount:SetItemID(12330)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Horn of the Arctic Wolf - 16081
	mount = AddMount(16081, V.ORIG, Q.EPIC) -- Item: 12351
	mount:SetItemID(12351)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Palomino Bridle - 16082
	mount = AddMount(16082, V.ORIG, Q.EPIC) -- Item: 12354
	mount:SetItemID(12354)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- White Stallion Bridle - 16083
	mount = AddMount(16083, V.ORIG, Q.EPIC) -- Item: 12353
	mount:SetItemID(12353)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Mottled Red Raptor - 16084
	mount = AddMount(16084, V.ORIG, Q.EPIC) -- Item: 8586
	mount:SetItemID(8586)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Winterspring Frostsaber - 17229
	mount = AddMount(17229, V.ORIG, Q.EPIC) -- Item: 13086
	mount:SetItemID(13086)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.QUEST)
	mount:SetRequiredFaction("Alliance")
	mount:AddQuest(29034)

	-- Ivory Raptor - 17450
	mount = AddMount(17450, V.ORIG, Q.EPIC) -- Item: 13317
	mount:SetItemID(13317)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Green Mechanostrider - 17453
	mount = AddMount(17453, V.ORIG, Q.RARE) -- Item: 13321
	mount:SetItemID(13321)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Unpainted Mechanostrider - 17454
	mount = AddMount(17454, V.ORIG, Q.RARE) -- Item: 13322
	mount:SetItemID(13322)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Icy Blue Mechanostrider Mod A - 17459
	mount = AddMount(17459, V.ORIG, Q.EPIC) -- Item: 13327
	mount:SetItemID(13327)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Frost Ram - 17460
	mount = AddMount(17460, V.ORIG, Q.EPIC) -- Item: 13329
	mount:SetItemID(13329)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Black Ram - 17461
	mount = AddMount(17461, V.ORIG, Q.EPIC) -- Item: 13328
	mount:SetItemID(13328)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Red Skeletal Horse - 17462
	mount = AddMount(17462, V.ORIG, Q.RARE) -- Item: 13331
	mount:SetItemID(13331)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Blue Skeletal Horse - 17463
	mount = AddMount(17463, V.ORIG, Q.RARE) -- Item: 13332
	mount:SetItemID(1332)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Brown Skeletal Horse - 17464
	mount = AddMount(17464, V.ORIG, Q.RARE) -- Item: 13333
	mount:SetItemID(13333)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Green Skeletal Warhorse - 17465
	mount = AddMount(17465, V.ORIG, Q.EPIC) -- Item: 13334
	mount:SetItemID(13334)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Deathcharger's Reins - 17481
	mount = AddMount(17481, V.ORIG, Q.EPIC) -- Item: 13335
	mount:SetItemID(13335)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.INSTANCE, F.MOB_DROP)
	mount:AddMobDrop(45412)

	-- Gray Kodo - 18989
	mount = AddMount(18989, V.ORIG, Q.RARE) -- Item: 15277
	mount:SetItemID(15277)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3685)

	-- Brown Kodo - 18990
	mount = AddMount(18990, V.ORIG, Q.RARE) -- Item: 15290
	mount:SetItemID(15290)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3685)

	-- Green Kodo - 18991
	mount = AddMount(18991, V.ORIG, Q.EPIC) -- Item: 15292
	mount:SetItemID(15292)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Teal Kodo - 18992
	mount = AddMount(18992, V.ORIG, Q.EPIC) -- Item: 15293
	mount:SetItemID(15293)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Black War Steed Bridle - 22717
	mount = AddMount(22717, V.ORIG, Q.EPIC) -- Item: 29468
	mount:SetItemID(29468)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(12783)

	-- Black War Kodo - 22718
	mount = AddMount(22718, V.ORIG, Q.EPIC) -- Item: 29466
	mount:SetItemID(29466)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(12796)

	-- Black Battlestrider - 22719
	mount = AddMount(22719, V.ORIG, Q.EPIC) -- Item: 29465
	mount:SetItemID(29465)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(12783)

	-- Black War Ram - 22720
	mount = AddMount(22720, V.ORIG, Q.EPIC) -- Item: 29467
	mount:SetItemID(29467)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(12783)

	-- Black War Raptor - 22721
	mount = AddMount(22721, V.ORIG, Q.EPIC) -- Item: 29472
	mount:SetItemID(29472)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(12796)

	-- Red Skeletal Warhorse - 22722
	mount = AddMount(22722, V.ORIG, Q.EPIC) -- Item: 29470
	mount:SetItemID(29470)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(12796)

	-- Black War Tiger - 22723
	mount = AddMount(22723, V.ORIG, Q.EPIC) -- Item: 29471
	mount:SetItemID(29471)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(12783)

	-- Black War Wolf - 22724
	mount = AddMount(22724, V.ORIG, Q.EPIC) -- Item: 18245
	mount:SetItemID(18245)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(12796)

	-- Dreadsteed - 23161
	mount = AddMount(23161, V.ORIG, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TRAINER)
	mount:SetRequiredClass("WARLOCK")
	mount:AddTrainer(461, 906, 2127, 3172, 3324, 3325, 3326, 4565, 5171, 5172, 5495, 5496, 6251, 35778, 36519, 45720, 49998, 50028, 50142, 50156, 50502, 50729)

	-- Charger (Alliance) -- 23214
	mount = AddMount(23214, V.ORIG, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("Human", "Dwarf")
	mount:SetRequiredFaction("Alliance")
	mount:AddTrainer(927, 928, 1232, 5148, 5149, 5491, 17844, 49793, 50023, 50160)

	-- Swift Mistsaber - 23219
	mount = AddMount(23219, V.ORIG, Q.EPIC) -- Item: 18767
	mount:SetItemID(18767)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Swift Frostsaber - 23221
	mount = AddMount(23221, V.ORIG, Q.EPIC) -- Item: 18766
	mount:SetItemID(18766)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Swift Yellow Mechanostrider - 23222
	mount = AddMount(23222, V.ORIG, Q.EPIC) -- Item: 18774
	mount:SetItemID(18774)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Swift White Mechanostrider - 23223
	mount = AddMount(23223, V.ORIG, Q.EPIC) -- Item: 18773
	mount:SetItemID(18773)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Swift Green Mechanostrider - 23225
	mount = AddMount(23225, V.ORIG, Q.EPIC) -- Item: 18772
	mount:SetItemID(18772)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(7955)

	-- Swift Palomino - 23227
	mount = AddMount(23227, V.ORIG, Q.EPIC) -- Item: 18776
	mount:SetItemID(18776)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(384, 4885)

	-- Swift White Steed - 23228
	mount = AddMount(23228, V.ORIG, Q.EPIC) -- Item: 18778
	mount:SetItemID(18778)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(384, 4885)

	-- Swift Brown Steed - 23229
	mount = AddMount(23229, V.ORIG, Q.EPIC) -- Item: 18777
	mount:SetItemID(18777)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(384, 4885)

	-- Swift Brown Ram - 23238
	mount = AddMount(23238, V.ORIG, Q.EPIC) -- Item: 18786
	mount:SetItemID(18786)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1261)

	-- Swift Gray Ram - 23239
	mount = AddMount(23239, V.ORIG, Q.EPIC) -- Item: 18787
	mount:SetItemID(18787)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1261)

	-- Swift White Ram - 23240
	mount = AddMount(23240, V.ORIG, Q.EPIC) -- Item: 18785
	mount:SetItemID(18785)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(1261)

	-- Swift Blue Raptor - 23241
	mount = AddMount(23241, V.ORIG, Q.EPIC) -- Item: 18788
	mount:SetItemID(18788)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(7952)

	-- Swift Olive Raptor - 23242
	mount = AddMount(23242, V.ORIG, Q.EPIC) -- Item: 18789
	mount:SetItemID(18789)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(7952)

	-- Swift Orange Raptor - 23243
	mount = AddMount(23243, V.ORIG, Q.EPIC) -- Item: 18790
	mount:SetItemID(187902)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(7952)

	-- Purple Skeletal Warhorse - 23246
	mount = AddMount(23246, V.ORIG, Q.EPIC) -- Item: 18791
	mount:SetItemID(18791)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Great White Kodo - 23247
	mount = AddMount(23247, V.ORIG, Q.EPIC) -- Item: 18793
	mount:SetItemID(18793)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3685)

	-- Great Gray Kodo - 23248
	mount = AddMount(23248, V.ORIG, Q.EPIC) -- Item: 18795
	mount:SetItemID(18795)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3685)

	-- Great Brown Kodo - 23249
	mount = AddMount(23249, V.ORIG, Q.EPIC) -- Item: 18794
	mount:SetItemID(18794)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3685)

	-- Swift Brown Wolf -- 23250
	mount = AddMount(23250, V.ORIG, Q.EPIC) -- Item: 18796
	mount:SetItemID(18796)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Swift Timber Wolf -- 23251
	mount = AddMount(23251, V.ORIG, Q.EPIC) -- Item: 18797
	mount:SetItemID(18797)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Swift Gray Wolf -- 23252
	mount = AddMount(23252, V.ORIG, Q.EPIC) -- Item: 18798
	mount:SetItemID(18798)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Swift Stormsaber - 23338
	mount = AddMount(23338, V.ORIG, Q.EPIC) -- Item: 18902
	mount:SetItemID(18902)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Frostwolf Howler - 23509
	mount = AddMount(23509, V.ORIG, Q.EPIC) -- Item: 19029
	mount:SetItemID(34129)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(13218, 13219)

	-- Stormpike Battle Charger - 23510
	mount = AddMount(23510, V.ORIG, Q.EPIC) -- Item: 19030
	mount:SetItemID(19030)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(13216, 13217)

	-- Swift Zulian Tiger - 24252
	mount = AddMount(24252, V.ORIG, Q.EPIC) -- Item: 19902
	mount:SetItemID(19902)
	mount:Retire()

	-- Swift Razzashi Raptor - 24242
	mount = AddMount(24242, V.ORIG, Q.EPIC) -- Item: 19872
	mount:SetItemID(19872)
	mount:Retire()

	-- Blue Qiraji Battle Tank -- 25953
	mount = AddMount(25953, V.ORIG, Q.RARE) -- Item: 21218
	mount:SetItemID(21218)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.WORLD_DROP)
	mount:AddWorldDrop(Z.AHNQIRAJ_THE_FALLEN_KINGDOM)

	-- Red Qiraji Battle Tank -- 26054
	mount = AddMount(26054, V.ORIG, Q.RARE) -- Item: 21321
	mount:SetItemID(21321)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.WORLD_DROP)
	mount:AddWorldDrop(Z.AHNQIRAJ_THE_FALLEN_KINGDOM)

	-- Yellow Qiraji Battle Tank -- 26055
	mount = AddMount(26055, V.ORIG, Q.RARE) -- Item: 21324
	mount:SetItemID(21324)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.WORLD_DROP)
	mount:AddWorldDrop(Z.AHNQIRAJ_THE_FALLEN_KINGDOM)

	-- Green Qiraji Battle Tank -- 26056
	mount = AddMount(26056, V.ORIG, Q.RARE) -- Item: 21323
	mount:SetItemID(21323)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.WORLD_DROP)
	mount:AddWorldDrop(Z.AHNQIRAJ_THE_FALLEN_KINGDOM)

	-- Black Qiraji Resonating Crystal - 26656
	mount = AddMount(26656, V.ORIG, Q.LEGENDARY) -- Item: 21176
	mount:SetItemID(21176)
	mount:Retire()

	-- Riding Turtle - 30174
	mount = AddMount(30174, V.ORIG, Q.EPIC) -- Item: 23720
	mount:SetItemID(23720)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOP)
	mount:AddCustom("TCG", "UNCLAIMED_BM")

	-- Charger (Horde) -- 34767
	mount = AddMount(34767, V.ORIG, Q.COMMON)
	mount:AddFilters(F.HORDE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("BloodElf")
	mount:SetRequiredFaction("Horde")
	mount:AddTrainer(16681, 20406, 23128, 44725, 50012, 50150)

	-- Warhorse (Horde) -- 34769
	mount = AddMount(34769, V.ORIG, Q.COMMON)
	mount:AddFilters(F.HORDE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("BloodElf")
	mount:SetRequiredFaction("Horde")
	mount:AddTrainer(16681, 20406, 23128, 44725, 50012, 50150)

-- Burning Crusade

	-- Golden Gryphon - 32235
	mount = AddMount(32235, V.TBC, Q.RARE) -- Item: 25470
	mount:SetItemID(25470)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Ebon Gryphon - 32239
	mount = AddMount(32239, V.TBC, Q.RARE) -- Item: 25471
	mount:SetItemID(25471)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Snowy Gryphon - 32240
	mount = AddMount(32240, V.TBC, Q.RARE) -- Item: 25472
	mount:SetItemID(25472)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Swift Blue Gryphon - 32242
	mount = AddMount(32242, V.TBC, Q.EPIC) -- Item: 25473
	mount:SetItemID(25473)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Tawny Wind Rider - 32243
	mount = AddMount(32243, V.TBC, Q.RARE) -- Item: 25474
	mount:SetItemID(25474)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Blue Wind Rider - 32244
	mount = AddMount(32244, V.TBC, Q.RARE) -- Item: 25475
	mount:SetItemID(25475)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Green Wind Rider - 32245
	mount = AddMount(32245, V.TBC, Q.RARE) -- Item: 25476
	mount:SetItemID(25476)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Swift Red Wind Rider - 32246
	mount = AddMount(32246, V.TBC, Q.EPIC) -- Item: 25477
	mount:SetItemID(25477)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Swift Red Gryphon - 32289
	mount = AddMount(32289, V.TBC, Q.EPIC) -- Item: 25527
	mount:SetItemID(25527)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Swift Green Gryphon - 32290
	mount = AddMount(32290, V.TBC, Q.EPIC) -- Item: 25528
	mount:SetItemID(25528)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Swift Purple Gryphon - 32292
	mount = AddMount(32292, V.TBC, Q.EPIC) -- Item: 25529
	mount:SetItemID(25529)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(20510, 32216, 35101, 35131, 43768)

	-- Swift Green Wind Rider - 32295
	mount = AddMount(32295, V.TBC, Q.EPIC) -- Item: 25531
	mount:SetItemID(25531)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Swift Yellow Wind Rider - 32296
	mount = AddMount(32296, V.TBC, Q.EPIC) -- Item: 25532
	mount:SetItemID(25532)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Swift Purple Wind Rider - 32297
	mount = AddMount(32297, V.TBC, Q.EPIC) -- Item: 25533
	mount:SetItemID(25533)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(20494, 32216, 35099, 35132)

	-- Swift Pink Hawkstrider - 33660
	mount = AddMount(33660, V.TBC, Q.EPIC) -- Item: 28936
	mount:SetItemID(28936)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Brown Elekk - 34406
	mount = AddMount(34406, V.TBC, Q.RARE) -- Item: 28481
	mount:SetItemID(28481)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(17584)

	-- Dark War Talbuk - 34790
	mount = AddMount(34790, V.TBC, Q.EPIC) -- Item: 29228
	mount:SetItemID(29228)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:AddVendor(21474, 21485)

	-- Red Hawkstrider - 34795
	mount = AddMount(34795, V.TBC, Q.RARE) -- Item: 28927
	mount:SetItemID(28927)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Cobalt War Talbuk - 34896
	mount = AddMount(34896, V.TBC, Q.EPIC) -- Item: 29102
	mount:SetItemID(29102)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- White War Talbuk - 34897
	mount = AddMount(34897, V.TBC, Q.EPIC) -- Item: 29103
	mount:SetItemID(29103)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- Silver War Talbuk - 34898
	mount = AddMount(34898, V.TBC, Q.EPIC) -- Item: 29104
	mount:SetItemID(29104)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- Tan War Talbuk - 34899
	mount = AddMount(34899, V.TBC, Q.EPIC) -- Item: 29105
	mount:SetItemID(31829)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- Purple Hawkstrider - 35018
	mount = AddMount(35018, V.TBC, Q.RARE) -- Item: 29222
	mount:SetItemID(29222)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Blue Hawkstrider - 35020
	mount = AddMount(35020, V.TBC, Q.RARE) -- Item: 29220
	mount:SetItemID(29220)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Black Hawkstrider - 35022
	mount = AddMount(35022, V.TBC, Q.RARE) -- Item: 29221
	mount:SetItemID(29221)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Swift Green Hawkstrider - 35025
	mount = AddMount(35025, V.TBC, Q.EPIC) -- Item: 29223
	mount:SetItemID(29223)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Swift Purple Hawkstrider - 35027
	mount = AddMount(35027, V.TBC, Q.EPIC) -- Item: 29224
	mount:SetItemID(29224)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(16264)

	-- Swift Warstrider - 35028
	mount = AddMount(35028, V.TBC, Q.EPIC) -- Item: 34129
	mount:SetItemID(34129)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(12796)

	-- Gray Elekk - 35710
	mount = AddMount(35710, V.TBC, Q.RARE) -- Item: 29744
	mount:SetItemID(29744)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(17584)

	-- Purple Elekk - 35711
	mount = AddMount(35711, V.TBC, Q.RARE) -- Item: 29743
	mount:SetItemID(29743)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(17584)

	-- Great Green Elekk - 35712
	mount = AddMount(35712, V.TBC, Q.EPIC) -- Item: 29746
	mount:SetItemID(29746)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(17584)

	-- Great Blue Elekk - 35713
	mount = AddMount(35713, V.TBC, Q.EPIC) -- Item: 29745
	mount:SetItemID(29745)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(17584)

	-- Great Purple Elekk - 35714
	mount = AddMount(35714, V.TBC, Q.EPIC) -- Item: 29747
	mount:SetItemID(29747)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(17584)

	-- Fiery Warhorse's Reins - 36702
	mount = AddMount(36702, V.TBC, Q.EPIC) -- Item: 30480
	mount:SetItemID(30480)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(15550)

	-- Swift Nether Drake - 37015
	mount = AddMount(37015, V.TBC, Q.EPIC) -- Item: 30609
	mount:SetItemID(30609)
	mount:Retire()

	-- Cobalt Riding Talbuk - 39315
	mount = AddMount(39315, V.TBC, Q.EPIC) -- Item: 31829
	mount:SetItemID(31829)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- Dark Riding Talbuk - 39316
	mount = AddMount(39316, V.TBC, Q.EPIC) -- Item: 28915
	mount:SetItemID(28915)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:AddVendor(21474, 21485)

	-- Silver Riding Talbuk - 39317
	mount = AddMount(39317, V.TBC, Q.EPIC) -- Item: 31831
	mount:SetItemID(31831)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- Tan Riding Talbuk - 39318
	mount = AddMount(39318, V.TBC, Q.EPIC) -- Item: 31833
	mount:SetItemID(31833)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- White Riding Talbuk - 39319
	mount = AddMount(39319, V.TBC, Q.EPIC) -- Item: 31835
	mount:SetItemID(31835)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NAGRAND)
	mount:AddRepVendor(FAC.KURENAI, REP.EXALTED, 20240)
	mount:AddRepVendor(FAC.THE_MAGHAR, REP.EXALTED, 20241)

	-- Green Riding Nether Ray - 39798
	mount = AddMount(39798, V.TBC, Q.EPIC) -- Item: 32314
	mount:SetItemID(32314)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHATARI_SKYGUARD)
	mount:AddRepVendor(FAC.SHATARI_SKYGUARD, REP.EXALTED, 23367)

	-- Red Riding Nether Ray - 39800
	mount = AddMount(39800, V.TBC, Q.EPIC) -- Item: 32317
	mount:SetItemID(32317)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHATARI_SKYGUARD)
	mount:AddRepVendor(FAC.SHATARI_SKYGUARD, REP.EXALTED, 23367)

	-- Purple Riding Nether Ray - 39801
	mount = AddMount(39801, V.TBC, Q.EPIC) -- Item: 32316
	mount:SetItemID(32316)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHATARI_SKYGUARD)
	mount:AddRepVendor(FAC.SHATARI_SKYGUARD, REP.EXALTED, 23367)

	-- Silver Riding Nether Ray - 39802
	mount = AddMount(39802, V.TBC, Q.EPIC) -- Item: 32318
	mount:SetItemID(32318)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHATARI_SKYGUARD)
	mount:AddRepVendor(FAC.SHATARI_SKYGUARD, REP.EXALTED, 23367)

	-- Blue Riding Nether Ray - 39803
	mount = AddMount(39803, V.TBC, Q.EPIC) -- Item: 32319
	mount:SetItemID(32319)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHATARI_SKYGUARD)
	mount:AddRepVendor(FAC.SHATARI_SKYGUARD, REP.EXALTED, 23367)

	-- Ashes of Al'ar - 40192
	mount = AddMount(40192, V.TBC, Q.EPIC) -- Item: 32458
	mount:SetItemID(32458)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(19622)
	mount:AddCustom("UNCLAIMED_BM")

	-- Raven Lord - 41252
	mount = AddMount(41252, V.TBC, Q.EPIC) -- Item: 32768
	mount:SetItemID(32768)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.INSTANCE, F.MOB_DROP)
	mount:AddMobDrop(23035)
	mount:AddCustom("HEROIC")

	-- Onyx Netherwing Drake - 41513
	mount = AddMount(41513, V.TBC, Q.EPIC) -- Item: 32857
	mount:SetItemID(32319)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NETHERWING, F.QUEST)
	mount:AddRepVendor(FAC.NETHERWING, REP.EXALTED, 23489)
	mount:AddQuest(11111)

	-- Azure Netherwing Drake - 41514
	mount = AddMount(41514, V.TBC, Q.EPIC) -- Item: 32858
	mount:SetItemID(32858)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NETHERWING, F.QUEST)
	mount:AddRepVendor(FAC.NETHERWING, REP.EXALTED, 23489)
	mount:AddQuest(11112)

	-- Cobalt Netherwing Drake - 41515
	mount = AddMount(41515, V.TBC, Q.EPIC) -- Item: 32859
	mount:SetItemID(32859)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NETHERWING, F.QUEST)
	mount:AddRepVendor(FAC.NETHERWING, REP.EXALTED, 23489)
	mount:AddQuest(11109)

	-- Purple Netherwing Drake - 41516
	mount = AddMount(41516, V.TBC, Q.EPIC) -- Item: 32860
	mount:SetItemID(32860)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NETHERWING, F.QUEST)
	mount:AddRepVendor(FAC.NETHERWING, REP.EXALTED, 23489)
	mount:AddQuest(11110)

	-- Veridian Netherwing Drake - 41517
	mount = AddMount(41517, V.TBC, Q.EPIC) -- Item: 32861
	mount:SetItemID(32861)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NETHERWING, F.QUEST)
	mount:AddRepVendor(FAC.NETHERWING, REP.EXALTED, 23489)
	mount:AddQuest(11114)

	-- Violet Netherwing Drake - 41518
	mount = AddMount(41518, V.TBC, Q.EPIC) -- Item: 32862
	mount:SetItemID(32862)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.NETHERWING, F.QUEST)
	mount:AddRepVendor(FAC.NETHERWING, REP.EXALTED, 23489)
	mount:AddQuest(11113)

	-- Spectral Tiger - 42776
	mount = AddMount(42776, V.TBC, Q.RARE) -- Item: 49283
	mount:SetItemID(49283)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Swift Spectral Tiger - 42777
	mount = AddMount(42777, V.TBC, Q.EPIC) -- Item: 49284
	mount:SetItemID(49284)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Amani War Bear - 43688
	mount = AddMount(43688, V.TBC, Q.EPIC) -- Item: 33809
	mount:SetItemID(33809)
	mount:Retire()

	-- Brewfest Ram - 43899
	mount = AddMount(43899, V.TBC, Q.RARE) -- Item: 33976
	mount:SetItemID(33976)
	mount:Retire()

	-- Swift Brewfest Ram - 43900
	mount = AddMount(43900, V.TBC, Q.EPIC) -- Item: 33977
	mount:SetItemID(33977)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS, F.MOB_DROP)
	mount:AddMobDrop(23872)
	mount:AddWorldEvent("BREWFEST")

	-- Cenarion War Hippogryph - 43927
	mount = AddMount(43927, V.TBC, Q.EPIC) -- Item: 33999
	mount:SetItemID(33999)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.CENARION_EXPEDITION)
	mount:AddRepVendor(FAC.CENARION_EXPEDITION, REP.EXALTED, 17904)

	 -- Turbo-Charged Flying Machine Control - 44151
	mount = AddMount(44151, V.TBC, Q.EPIC)
	mount:SetItemID(34061)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.ENGINEERING)

	 -- Flying Machine Control - 44153
	mount = AddMount(44153, V.TBC, Q.RARE) -- Item: 34060
	mount:SetItemID(34060)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.ENGINEERING)

	-- Merciless Nether Drake - 44744
	mount = AddMount(44744, V.TBC, Q.EPIC) -- Item: 34092
	mount:SetItemID(34092)
	mount:Retire()

	-- X-51 Nether-Rocket - 46197
	mount = AddMount(46197, V.TBC, Q.RARE) -- Item: 49285
	mount:SetItemID(49285)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- X-51 Nether-Rocket X-TREME-- 46199
	mount = AddMount(46199, V.TBC, Q.EPIC) -- Item: 49286
	mount:SetItemID(49286)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")
	mount:AddCustom("UNCLAIMED_BM")

	-- Swift White Hawkstrider - 46628
	mount = AddMount(46628, V.TBC, Q.EPIC) -- Item: 35513
	mount:SetItemID(35513)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.INSTANCE, F.MOB_DROP)
	mount:AddMobDrop(24664)

	-- The Horseman's Reins - 48025
	mount = AddMount(48025, V.TBC, Q.EPIC) -- Item: 37012
	mount:SetItemID(37012)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS, F.MOB_DROP)
	mount:AddMobDrop(23682)
	mount:AddWorldEvent("HALLOWS_END")

	-- Black War Elekk - 48027
	mount = AddMount(48027, V.TBC, Q.EPIC) -- Item: 35906
	mount:SetItemID(35906)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(12783)

	-- Vengeful Nether Drake - 49193
	mount = AddMount(49193, V.TBC, Q.EPIC) -- Item: 37676
	mount:SetItemID(37676)
	mount:Retire()

	-- Swift Zhevra - 49322
	mount = AddMount(49322, V.TBC, Q.EPIC) -- Item: 37719
	mount:SetItemID(37719)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	mount:AddCustom("RECRUIT")

	-- Great Brewfest Kodo - 49379
	mount = AddMount(49379, V.TBC, Q.EPIC) -- Item: 37828
	mount:SetItemID(37828)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS, F.MOB_DROP)
	mount:AddMobDrop(23872)
	mount:AddWorldEvent("BREWFEST")

-- Wrath of the Lich King

	-- Acherus Deathcharger - 48778
	mount = AddMount(48778, V.WOTLK, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	mount:SetRequiredClass("DEATHKNIGHT")
	mount:AddQuest(12687)

	-- Big Battle Bear - 51412
	mount = AddMount(51412, V.WOTLK, Q.EPIC) -- Item: 49282
	mount:SetItemID(49282)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- White Polar Bear - 54753
	mount = AddMount(54753, V.WOTLK, Q.EPIC) -- Item: 43962
	mount:SetItemID(43962)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	mount:AddQuest(13422, 13423, 13424, 13425)
	mount:AddCustom("UNCLAIMED_BM")

	-- Winged Steed of the Ebon Blade - 54729
	mount = AddMount(54729, V.WOTLK, Q.EPIC) -- Item: 40775
	mount:SetItemID(40775)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.REPUTATION, F.IBOP, F.KNIGHTS_OF_THE_EBON_BLADE)
	mount:SetRequiredClass("DEATHKNIGHT")
	mount:AddRepVendor(FAC.KNIGHTS_OF_THE_EBON_BLADE, REP.EXALTED, 29587)

	-- Mechano-Hog - 55531
	mount = AddMount(55531, V.WOTLK, Q.EPIC) -- Item: 41508
	mount:SetItemID(41508)
	mount:AddFilters(F.HORDE, F.IBOE, F.PROFESSION)
	mount:SetRequiredFaction("Horde")
	mount:AddProfession(PROF.ENGINEERING)

	-- Brutal Nether Drake - 58615
	mount = AddMount(58615, V.WOTLK, Q.EPIC) -- Item: 43516
	mount:SetItemID(43516)
	mount:Retire()

	-- Big Blizzard Bear - 58983
	mount = AddMount(58983, V.WOTLK, Q.RARE) -- Item: 43599
	mount:SetItemID(43599)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOA, F.PROMO)
	mount:AddCustom("BLIZZCON")

	-- Azure Drake - 59567
	mount = AddMount(59567, V.WOTLK, Q.EPIC) -- Item: 43952
	mount:SetItemID(43592)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(28859)

	-- Blue Drake - 59568
	mount = AddMount(59568, V.WOTLK, Q.EPIC) -- Item: 43953
	mount:SetItemID(43953)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.INSTANCE, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(28859)
	mount:AddCustom("UNCLAIMED_BM")

	-- Bronze Drake - 59569
	mount = AddMount(59569, V.WOTLK, Q.EPIC) -- Item: 43951
	mount:SetItemID(43951)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.INSTANCE, F.MOB_DROP)
	mount:AddMobDrop(32273)

	-- Red Drake - 59570
	mount = AddMount(59570, V.WOTLK, Q.EPIC) -- Item: 43955
	mount:SetItemID(43955)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_WYRMREST_ACCORD)
	mount:AddRepVendor(FAC.THE_WYRMREST_ACCORD, REP.EXALTED, 32533)

	-- Twilight Drake -- 59571
	mount = AddMount(59571, V.WOTLK, Q.EPIC) -- Item: 43954
	mount:SetItemID(43954)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(28860)

	-- Black Drake -- 59650
	mount = AddMount(59650, V.WOTLK, Q.EPIC) -- Item: 43986
	mount:SetItemID(43986)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(28860)

	-- Black War Mammoth - 59785
	mount = AddMount(59785, V.WOTLK, Q.EPIC) -- Item: 43956
	mount:SetItemID(43956)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.PVP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(32294)

	-- Black War Mammoth - 59788
	mount = AddMount(59788, V.WOTLK, Q.EPIC) -- Item: 44077
	mount:SetItemID(44077)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.PVP)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(32296)

	-- Wooly Mammoth -- 59791
	mount = AddMount(59793, V.WOTLK, Q.EPIC) -- Item: 44231
	mount:SetItemID(44231)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(32216)

	-- Wooly Mammoth - 59793
	mount = AddMount(59791, V.WOTLK, Q.EPIC) -- Item: 44230
	mount:SetItemID(44230)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(32216)

	-- Ice Mammoth - 59797
	mount = AddMount(59797, V.WOTLK, Q.EPIC) -- Item: 44080
	mount:SetItemID(44080)
	mount:AddFilters(F.HORDE, F.IBOP, F.REPUTATION, F.VENDOR, F.THE_SONS_OF_HODIR)
	mount:SetRequiredFaction("Horde")
	mount:AddRepVendor(FAC.THE_SONS_OF_HODIR, REP.REVERED, 32540)

	-- Ice Mammoth - 59799
	mount = AddMount(59799, V.WOTLK, Q.EPIC) -- Item: 43958
	mount:SetItemID(43958)
	mount:AddFilters(F.ALLIANCE, F.REPUTATION, F.IBOP, F.THE_SONS_OF_HODIR)
	mount:SetRequiredFaction("Alliance")
	mount:AddRepVendor(FAC.THE_SONS_OF_HODIR, REP.REVERED, 32540)

	-- Red Proto-Drake - 59961
	mount = AddMount(59961, V.WOTLK, Q.EPIC)
	mount:SetItemID(44160)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(2136)

	 -- Black Proto-Drake - 59976
	mount = AddMount(59976, V.WOTLK, Q.EPIC) -- Item: 44164
	mount:SetItemID(44164)
	mount:Retire()

	-- Blue Proto-Drake - 59996
	mount = AddMount(59996, V.WOTLK, Q.EPIC) -- Item: 44151
	mount:SetItemID(44151)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.INSTANCE, F.MOB_DROP)
	mount:AddMobDrop(26693)
	mount:AddCustom("UNCLAIMED_BM")

	-- Time-Lost Proto-Drake - 60002
	mount = AddMount(60002, V.WOTLK, Q.EPIC) -- Item: 44168
	mount:SetItemID(44168)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(32491)

	 -- Plagued Proto-Drake - 60021
	mount = AddMount(60021, V.WOTLK, Q.EPIC) -- Item: 44175
	mount:SetItemID(44175)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP)
	mount:AddCustom("UNCLAIMED_BM")

	-- Violet Proto-Drake - 60024
	mount = AddMount(60024, V.WOTLK, Q.EPIC)
	mount:SetItemID(44177)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(2144)

	-- Albino Drake - 60025
	mount = AddMount(60025, V.WOTLK, Q.EPIC)
	mount:SetItemID(44178)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(2143)

	-- Armored Brown Bear - 60114
	mount = AddMount(60114, V.WOTLK, Q.EPIC) -- Item: 44225
	mount:SetItemID(44225)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(32216)

	-- Armored Brown Bear -- 60116
	mount = AddMount(60116, V.WOTLK, Q.EPIC) -- Item: 44226
	mount:SetItemID(44226)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(32216)

	-- Black War Bear - 60118
	mount = AddMount(60118, V.WOTLK, Q.EPIC) -- Item: 44223
	mount:SetItemID(44223)
	mount:AddFilters(F.ALLIANCE, F.ACHIEVEMENT, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddAchievement(614)

	-- Reins of the Black War Bear - Horde - 60119
	mount = AddMount(60119, V.WOTLK, Q.EPIC) -- Item: 44224
	mount:SetItemID(4424)
	mount:AddFilters(F.HORDE, F.IBOP, F.PVP, F.ACHIEVEMENT)
	mount:SetRequiredFaction("Horde")
	mount:AddAchievement(619)

	-- Mekgineer's Chopper - 60424
	mount = AddMount(60424, V.WOTLK, Q.EPIC) -- Item: 44413
	mount:SetItemID(73839)
	mount:AddFilters(F.ALLIANCE, F.IBOE, F.PROFESSION)
	mount:SetRequiredFaction("Alliance")
	mount:AddProfession(PROF.ENGINEERING)

	-- Armored Snowy Gryphon - 61229
	mount = AddMount(61229, V.WOTLK, Q.EPIC) -- Item: 44689
	mount:SetItemID(44689)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(32216)

	-- Armored Blue Wind Rider -- 61230
	mount = AddMount(61230, V.WOTLK, Q.EPIC) -- Item: 44690
	mount:SetItemID(44690)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(32216)

	 -- Green Proto-Drake - 61294
	mount = AddMount(61294, V.WOTLK, Q.EPIC) -- Item: 44707
	mount:SetItemID(44707)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	mount:AddCustom("MYSTERIOUS_EGG", "UNCLAIMED_BM")

	-- Magnificent Flying Carpet - 61309
	mount = AddMount(61309, V.WOTLK, Q.EPIC) -- Item: 44558
	mount:SetItemID(44558)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.TAILORING)

	-- Traveler's Tundra Mammoth - 61425
	mount = AddMount(61425, V.WOTLK, Q.EPIC) -- Item: 44235
	mount:SetItemID(44235)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(32216)

	-- Reins of the Traveler's Tundra Mammoth -- 61447
	mount = AddMount(61447, V.WOTLK, Q.EPIC) -- Item: 44234
	mount:SetItemID(44234)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(32216)

	-- Flying Carpet - 61451
	mount = AddMount(61451, V.WOTLK, Q.RARE) -- Item: 44554
	mount:SetItemID(44554)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.TAILORING)

	-- Grand Black War Mammoth - 61465
	mount = AddMount(61465, V.WOTLK, Q.EPIC) -- Item: 43959
	mount:SetItemID(43959)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP, F.RAID)
	mount:SetRequiredFaction("Alliance")
	mount:AddMobDrop(31125, 33993, 35013, 38433)

	-- Grand Black War Mammoth - Horde - 61467
	mount = AddMount(61467, V.WOTLK, Q.EPIC) -- Item: 44083
	mount:SetItemID(44083)
	mount:AddFilters(F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:SetRequiredFaction("Horde")
	mount:AddMobDrop(31125, 33993, 35013, 38433)

	-- Grand Ice Mammoth - 61469
	mount = AddMount(61469, V.WOTLK, Q.EPIC) -- Item: 44086
	mount:SetItemID(44086)
	mount:AddFilters(F.HORDE, F.IBOP, F.REPUTATION, F.VENDOR, F.THE_SONS_OF_HODIR)
	mount:SetRequiredFaction("Horde")
	mount:AddRepVendor(FAC.THE_SONS_OF_HODIR, REP.EXALTED, 32540)

	-- Grand Ice Mammoth - 61470
	mount = AddMount(61470, V.WOTLK, Q.EPIC) -- Item: 43961
	mount:SetItemID(43961)
	mount:AddFilters(F.ALLIANCE, F.REPUTATION, F.IBOP, F.THE_SONS_OF_HODIR)
	mount:SetRequiredFaction("Alliance")
	mount:AddRepVendor(FAC.THE_SONS_OF_HODIR, REP.EXALTED, 32540)

	-- Blue Dragonhawk - 61996
	mount = AddMount(61996, V.WOTLK, Q.EPIC) -- Item: 44843
	mount:SetItemID(44843)
	mount:AddFilters(F.ALLIANCE, F.ACHIEVEMENT, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddAchievement(2536)

	-- Red Dragonhawk -- 61997
	mount = AddMount(61997, V.WOTLK, Q.EPIC) -- Item: 44842
	mount:SetItemID(70909)
	mount:AddFilters(F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:SetRequiredFaction("Horde")
	mount:AddAchievement(2537)

	-- Stormwind Steed - 63232
	mount = AddMount(63232, V.WOTLK, Q.EPIC) -- Item: 45125
	mount:SetItemID(45125)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33307)
	mount:AddCustom("CHAMPION_SEALS")

	-- Darkspear Raptor -- 63635
	mount = AddMount(63635, V.WOTLK, Q.EPIC) -- Item: 45593
	mount:SetItemID(45593)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33554)
	mount:AddCustom("CHAMPION_SEALS")

	-- Ironforge Ram - 63636
	mount = AddMount(63636, V.WOTLK, Q.EPIC) -- Item: 45586
	mount:SetItemID(45586)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33310)
	mount:AddCustom("CHAMPION_SEALS")

	-- Darnassian Nightsaber - 63637
	mount = AddMount(63637, V.WOTLK, Q.EPIC) -- Item: 45591
	mount:SetItemID(45591)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33653)
	mount:AddCustom("CHAMPION_SEALS")

	-- Gnomeregan Mechanostrider - 63638
	mount = AddMount(63638, V.WOTLK, Q.EPIC) -- Item: 45589
	mount:SetItemID(45589)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33650)
	mount:AddCustom("CHAMPION_SEALS")

	-- Exodar Elekk - 63639
	mount = AddMount(63639, V.WOTLK, Q.EPIC) -- Item: 45590
	mount:SetItemID(45590)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33657)
	mount:AddCustom("CHAMPION_SEALS")

	-- Orgrimmar Wolf - 63640
	mount = AddMount(63640, V.WOTLK, Q.EPIC) -- Item: 45595
	mount:SetItemID(45595)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33553)
	mount:AddCustom("CHAMPION_SEALS")

	-- Thunder Bluff Kodo - 63641
	mount = AddMount(63641, V.WOTLK, Q.EPIC) -- Item: 45592
	mount:SetItemID(45592)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33556)
	mount:AddCustom("CHAMPION_SEALS")

	-- Silvermoon Hawkstrider - 63642
	mount = AddMount(63642, V.WOTLK, Q.EPIC) -- Item: 45596
	mount:SetItemID(45596)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33557)
	mount:AddCustom("CHAMPION_SEALS")

	-- Forsaken Warhorse -- 63643
	mount = AddMount(63643, V.WOTLK, Q.EPIC) -- Item: 45597
	mount:SetItemID(45597)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33555)
	mount:AddCustom("CHAMPION_SEALS")

	 -- Mimiron's Head - 63796
	mount = AddMount(63796, V.WOTLK, Q.EPIC) -- Item: 45693
	mount:SetItemID(49636)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(33288)

	 -- Argent Hippogryph - 63844
	mount = AddMount(63844, V.WOTLK, Q.EPIC) -- Item: 45725
	mount:SetItemID(45725)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(33307, 33310, 33553, 33554, 33555, 33556, 33557, 33650, 33653, 33657)

	-- Ironbound Proto-Drake - 63956
	mount = AddMount(63956, V.WOTLK, Q.EPIC)
	mount:SetItemID(45801)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(2958)

	-- Rusted Proto-Drake - 63963
	mount = AddMount(63963, V.WOTLK, Q.EPIC)
	mount:SetItemID(45802)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(2957)

	-- White Kodo -- 64657
	mount = AddMount(64657, V.WOTLK, Q.RARE) -- Item: 46100
	mount:SetItemID(46100)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3685)

	-- Black Wolf -- 64658
	mount = AddMount(64658, V.WOTLK, Q.RARE) -- Item: 46099
	mount:SetItemID(46099)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(3362)

	-- Venomhide Ravasaur -- 64659
	mount = AddMount(64659, V.WOTLK, Q.EPIC) -- Item: 46102
	mount:SetItemID(46102)
	mount:AddFilters(F.HORDE, F.IBOP, F.QUEST)
	mount:SetRequiredFaction("Horde")
	mount:AddQuest(13906)

	-- Sea Turtle -- 64731
	mount = AddMount(64731, V.WOTLK, Q.RARE) -- Item: 46109
	mount:SetItemID(46109)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP)
	mount:AddWorldDrop(Z.ULDUM, Z.TWILIGHT_HIGHLANDS, Z.TOL_BARAD_PENINSULA, Z.NORTHREND, Z.MOUNT_HYJAL, Z.DEEPHOLM, Z.THE_JADE_FOREST, Z.DARKMOON_ISLAND)

	-- Deadly Gladiator's Frostwyrm - 64927
	mount = AddMount(64927, V.WOTLK, Q.EPIC) -- Item: 46708
	mount:SetItemID(46708)
	mount:Retire()

	-- Black Skeletal Horse - 64977
	mount = AddMount(64977, V.WOTLK, Q.RARE) -- Item: 46308
	mount:SetItemID(46308)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Furious Gladiator's Frostwyrm - 65439
	mount = AddMount(65439, V.WOTLK, Q.EPIC) -- Item: 46171
	mount:SetItemID(19902)
	mount:Retire()

	-- Great Red Elekk -- 65637
	mount = AddMount(65637, V.WOTLK, Q.EPIC) -- Item: 46745
	mount:SetItemID(46745)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33657)
	mount:AddCustom("CHAMPION_SEALS")

	-- Swift Moonsaber - 65638
	mount = AddMount(65638, V.WOTLK, Q.EPIC) -- Item: 46744
	mount:SetItemID(46744)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33653)
	mount:AddCustom("CHAMPION_SEALS")

	-- Swift Red Hawkstrider - 65639
	mount = AddMount(65639, V.WOTLK, Q.EPIC) -- Item: 46751
	mount:SetItemID(46751)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33557)
	mount:AddCustom("CHAMPION_SEALS")

	-- Swift Gray Steed - 65640
	mount = AddMount(65640, V.WOTLK, Q.EPIC) -- Item: 46752
	mount:SetItemID(46752)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33307)
	mount:AddCustom("CHAMPION_SEALS")

	-- Great Golden Kodo -- 65641
	mount = AddMount(65641, V.WOTLK, Q.EPIC) -- Item: 46750
	mount:SetItemID(46750)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33556)
	mount:AddCustom("CHAMPION_SEALS")

	-- Turbostrider - 65642
	mount = AddMount(65642, V.WOTLK, Q.EPIC) -- Item: 46747
	mount:SetItemID(46747)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33650)
	mount:AddCustom("CHAMPION_SEALS")

	-- Swift Violet Ram - 65643
	mount = AddMount(65643, V.WOTLK, Q.EPIC) -- Item: 46748
	mount:SetItemID(46748)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(33310)
	mount:AddCustom("CHAMPION_SEALS")

	-- Swift Purple Raptor -- 65644
	mount = AddMount(65644, V.WOTLK, Q.EPIC) -- Item: 46743
	mount:SetItemID(45593)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33554)
	mount:AddCustom("CHAMPION_SEALS")

	-- White Skeletal Warhorse -- 65645
	mount = AddMount(65645, V.WOTLK, Q.EPIC) -- Item: 46746
	mount:SetItemID(46746)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33555)
	mount:AddCustom("CHAMPION_SEALS")

	-- Swift Burgundy Wolf -- 65646
	mount = AddMount(65646, V.WOTLK, Q.EPIC) -- Item: 46749
	mount:SetItemID(46749)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(33553)
	mount:AddCustom("CHAMPION_SEALS")

	-- Magic Rooster - 65917
	mount = AddMount(65917, V.WOTLK, Q.EPIC) -- Item: 49290
	mount:SetItemID(49290)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Silver Covenant Hippogryph - 66087
	mount = AddMount(66087, V.WOTLK, Q.EPIC) -- Item: 46813
	mount:SetItemID(46813)
	mount:AddFilters(F.ALLIANCE, F.REPUTATION, F.IBOP, F.WRATHCOMMON1)
	mount:SetRequiredFaction("Alliance")
	mount:AddRepVendor(FAC.THE_SILVER_COVENANT, REP.EXALTED, 34881)

	-- Sunreaver Dragonhawk -- 66088
	mount = AddMount(66088, V.WOTLK, Q.EPIC) -- Item: 46814
	mount:SetItemID(46814)
	mount:AddFilters(F.HORDE, F.IBOP, F.REPUTATION, F.VENDOR, F.WRATHCOMMON1)
	mount:SetRequiredFaction("Horde")
	mount:AddRepVendor(FAC.THE_SUNREAVERS, REP.EXALTED, 34772)

	-- Quel'dorei Steed - 66090
	mount = AddMount(66090, V.WOTLK, Q.EPIC) -- Item: 46815
	mount:SetItemID(46815)
	mount:AddFilters(F.ALLIANCE, F.REPUTATION, F.IBOP, F.WRATHCOMMON1)
	mount:SetRequiredFaction("Alliance")
	mount:AddRepVendor(FAC.THE_SILVER_COVENANT, REP.EXALTED, 34881)

	-- Sunreaver Hawkstrider -- 66091
	mount = AddMount(66091, V.WOTLK, Q.EPIC) -- Item: 46816
	mount:SetItemID(46816)
	mount:AddFilters(F.HORDE, F.IBOP, F.REPUTATION, F.VENDOR, F.WRATHCOMMON1)
	mount:SetRequiredFaction("Horde")
	mount:AddRepVendor(FAC.THE_SUNREAVERS, REP.EXALTED, 34772)

	-- Ochre Skeletal Warhorse - 66846
	mount = AddMount(66846, V.WOTLK, Q.EPIC) -- Item: 47101
	mount:SetItemID(47101)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(4731)

	-- Striped Dawnsaber - 66847
	mount = AddMount(66847, V.WOTLK, Q.RARE) -- Item: 47100
	mount:SetItemID(47100)
	mount:AddFilters(F.ALLIANCE, F.VENDOR, F.IBOP)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(4730)

	-- Argent Charger - 66906
	mount = AddMount(66906, V.WOTLK, Q.EPIC) -- Item: 47179
	mount:SetItemID(47179)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredClass("PALADIN")
	mount:AddVendor(34885)

	-- Relentless Gladiator's Frostwyrm - 67336
	mount = AddMount(67336, V.WOTLK, Q.EPIC)
	mount:SetItemID(19902)
	mount:Retire()

	-- Argent Warhorse - 67466
	mount = AddMount(67466, V.WOTLK, Q.EPIC) -- Item: 47180
	mount:SetItemID(47180)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(34885)

	-- Swift Horde Wolf - 68056
	mount = AddMount(68056, V.WOTLK, Q.EPIC) -- Item: 49046
	mount:SetItemID(49046)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	-- Swift Alliance Steed -- 68057
	mount = AddMount(68057, V.WOTLK, Q.EPIC) -- Item: 49044
	mount:SetItemID(49044)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Crusader's White Warhorse -- 68187
	mount = AddMount(68187, V.WOTLK, Q.EPIC)
	mount:SetItemID(49096)
	mount:Retire()
	mount:SetRequiredFaction("Alliance")

	-- Crusader's Black Warhorse - 68188
	mount = AddMount(68188, V.WOTLK, Q.EPIC) -- Item: 49098
	mount:SetItemID(49098)
	mount:Retire()
	mount:SetRequiredFaction("Horde")

	 -- Reins of the Onyxian Drake - 69395
	mount = AddMount(69395, V.WOTLK, Q.EPIC) -- Item: 49636
	mount:SetItemID(49636)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(10184)

	-- Sunwalker Kodo -- 69820
	mount = AddMount(69820, V.CATA, Q.COMMON)
	mount:AddFilters(F.HORDE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("Tauren")
	mount:SetRequiredFaction("Horde")
	mount:AddTrainer(8664, 43013, 50035, 50150)

	-- Great Sunwalker Kodo -- 69826
	mount = AddMount(69826, V.CATA, Q.COMMON)
	mount:AddFilters(F.HORDE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("Tauren")
	mount:SetRequiredFaction("Horde")
	mount:AddTrainer(8664, 43013, 50035, 50150)

	-- Big Love Rocket - 71342
	mount = AddMount(71342, V.WOTLK, Q.EPIC) -- Item: 50250
	mount:SetItemID(50250)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS, F.MOB_DROP)
	mount:AddMobDrop(36296)
	mount:AddWorldEvent("LOVE_IS_IN_THE_AIR")

	-- Wrathful Gladiator's Frostwyrm - 71810
	mount = AddMount(71810, V.WOTLK, Q.EPIC)
	mount:SetItemID(19902)
	mount:Retire()

	-- Icebound Frostbrood Vanquisher - 72807
	mount = AddMount(72807, V.WOTLK, Q.EPIC)
	mount:SetItemID(51955)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(4603)

	-- Bloodbathed Frostbrood Vanquisher - 72808
	mount = AddMount(72808, V.WOTLK, Q.EPIC)
	mount:SetItemID(51954)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(4602)

	-- Invincible's Reins - 72286
	mount = AddMount(72286, V.WOTLK, Q.EPIC) -- Item: 50818
	mount:SetItemID(50818)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.MOB_DROP)
	mount:AddMobDrop(36597)
	mount:AddCustom("UNCLAIMED_BM")

	-- Crimson Deathcharger - 73313
	mount = AddMount(73313, V.WOTLK, Q.EPIC) -- Item: 52200
	mount:SetItemID(52200)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOE)
	mount:AddQuest(24915)

	-- Exarch's Elekk -- 73629
	mount = AddMount(73629, V.CATA, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("Draenei")
	mount:SetRequiredFaction("Alliance")
	mount:AddTrainer(928, 5492, 17483, 17844, 50023)

	-- Great Exarch's Elekk -- 73630
	mount = AddMount(73630, V.CATA, Q.COMMON)
	mount:AddFilters(F.ALLIANCE, F.TRAINER)
	mount:SetRequiredClass("PALADIN")
	mount:SetRequiredRaces("Draenei")
	mount:SetRequiredFaction("Alliance")
	mount:AddTrainer(928, 5492, 17483, 17844, 50023)

	-- Blazing Hippogryph - 74856
	mount = AddMount(74856, V.WOTLK, Q.EPIC) -- Item: 54069
	mount:SetItemID(54069)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Wooly White Rhino -- 74918
	mount = AddMount(74918, V.WOTLK, Q.EPIC) -- Item: 54068
	mount:SetItemID(54068)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Frosty Flying Carpet - 75596
	mount = AddMount(75596, V.WOTLK, Q.EPIC) -- Item: 54797
	mount:SetItemID(54797)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.TAILORING)

	-- Celestial Steed - 75614
	mount = AddMount(75614, V.WOTLK, Q.EPIC) -- Item: 54811
	mount:SetItemID(54811)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOA)
	mount:AddCustom("STORE")

	-- X-53 Touring Rocket - 75973
	mount = AddMount(75973, V.WOTLK, Q.EPIC) -- Item: 54860
	mount:SetItemID(54860)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	mount:AddCustom("RECRUIT")

	-- Goblin Trike -- 87090
	mount = AddMount(87090, V.WOTLK, Q.RARE) -- Item: 62461
	mount:SetItemID(62461)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(48510)

	-- Goblin Turbo-Trike -- 87091
	mount = AddMount(87091, V.WOTLK, Q.EPIC) -- Item: 62462
	mount:SetItemID(62462)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(48510)

-- Cataclysm

	-- Abyssal Seahorse -- 75207
	mount = AddMount(75207, V.CATA, Q.RARE) -- Item: 54465
	mount:SetItemID(54465)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	mount:AddQuest(25371)

	-- Fossilized Raptor -- 84751
	mount = AddMount(84751, V.CATA, Q.EPIC) -- Item: 60954
	mount:SetItemID(60954)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	mount:AddProfession(PROF.ARCHAEOLOGY)

	-- Volcanic Stone Drake -- 88331
	mount = AddMount(88331, V.CATA, Q.EPIC) -- Item: 62900
	mount:SetItemID(62900)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(4845)

	-- Drake of the East Wind -- 88335
	mount = AddMount(88335, V.CATA, Q.EPIC) -- Item: 62901
	mount:SetItemID(62901)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(4853)

	-- Drake of the West Wind -- 88741
	mount = AddMount(88741, V.CATA, Q.EPIC) -- Item: 65356
	mount:SetItemID(35356)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.REPUTATION, F.IBOP, F.CATACOMMON2)
	mount:AddRepVendor(FAC.BARADINS_WARDENS, REP.EXALTED, 47328)
	mount:AddRepVendor(FAC.HELLSCREAMS_REACH, REP.EXALTED, 48531)

	-- Tan Riding Camel -- 88749
	mount = AddMount(88749, V.CATA, Q.EPIC) -- Item: 63045
	mount:SetItemID(63045)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.RAMKAHEN)
	mount:AddRepVendor(FAC.RAMKAHEN, REP.EXALTED, 48617)

	-- Grey Riding Camel -- 88750
	mount = AddMount(88750, V.CATA, Q.EPIC) -- Item: 63046
	mount:SetItemID(63046)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	mount:AddMobDrop(50245)

	-- Drake of the South Wind -- 88744
	mount = AddMount(88744, V.CATA, Q.EPIC) -- Item: 63041
	mount:SetItemID(63041)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(46753)

	-- Drake of the North Wind -- 88742
	mount = AddMount(88742, V.CATA, Q.EPIC) -- Item: 63040
	mount:SetItemID(63040)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(43873)

	-- Vitreous Stone Drake -- 88746
	mount = AddMount(88746, V.CATA, Q.EPIC) -- Item: 63043
	mount:SetItemID(63043)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.INSTANCE, F.IBOP)
	mount:AddMobDrop(43214)
	mount:AddCustom("UNCLAIMED_BM")

	-- Brown Riding Camel -- 88748
	mount = AddMount(88748, V.CATA, Q.EPIC) -- Item: 63044
	mount:SetItemID(63044)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.RAMKAHEN)
	mount:AddRepVendor(FAC.RAMKAHEN, REP.EXALTED, 48617)

	-- Phosphorescent Stone Drake -- 88718
	mount = AddMount(88718, V.CATA, Q.EPIC) -- Item: 63042
	mount:SetItemID(63042)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	mount:AddMobDrop(50062)

	-- Dark Phoenix -- 88990
	mount = AddMount(88990, V.CATA, Q.EPIC) -- Item: 63125
	mount:SetItemID(63125)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(4988)
	mount:AddVendor(46572, 46602, 51495, 51496, 51503, 51504, 51512, 52268)

	-- Golden King -- 90621
	mount = AddMount(90621, V.CATA, Q.EPIC) -- Item: 62298
	mount:SetItemID(62298)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.ACHIEVEMENT, F.GUILD)
	mount:SetRequiredFaction("Alliance")
	mount:AddAchievement(4912)
	mount:AddRepVendor(FAC.GUILD, REP.EXALTED, 51512, 52268, 46602, 51495, 51504)

	-- Ultramarine Qiraji Battle Tank -- 92155
	mount = AddMount(92155, V.CATA, Q.EPIC) -- Item: 64883
	mount:SetItemID(64883)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.PROFESSION, F.IBOP)
	mount:AddProfession(PROF.ARCHAEOLOGY)

	-- Spectral Steed -- 92231
	mount = AddMount(92231, V.CATA, Q.EPIC) -- Item: 64998
	mount:SetItemID(64998)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.VENDOR, F.PVP, F.REPUTATION, F.CATACOMMON2)
	mount:SetRequiredFaction("Alliance")
	mount:AddRepVendor(FAC.BARADINS_WARDENS, REP.EXALTED, 47328)

	-- Spectral Wolf -- 92232
	mount = AddMount(92232, V.CATA, Q.EPIC) -- Item: 64999
	mount:SetItemID(64999)
	mount:AddFilters(F.HORDE, F.IBOP, F.REPUTATION, F.CATACOMMON2)
	mount:SetRequiredFaction("Horde")
	mount:AddRepVendor(FAC.HELLSCREAMS_REACH, REP.EXALTED, 48531)

	-- Sandstone Drake -- 93326
	mount = AddMount(93326, V.CATA, Q.EPIC) -- Item: 65891
	mount:SetItemID(65891)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.PROFESSION, F.IBOP)
	mount:AddProfession(PROF.ALCHEMY)

	-- Mottled Drake -- 93623
	mount = AddMount(93623, V.CATA, Q.EPIC) -- Item: 68008
	mount:SetItemID(68008)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Kor'kron Annihilator -- 93644
	mount = AddMount(93644, V.CATA, Q.EPIC) -- Item: 67107
	mount:SetItemID(67107)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.ACHIEVEMENT, F.REPUTATION, F.GUILD)
	mount:SetRequiredFaction("Horde")
	mount:AddAchievement(5492)
	mount:AddRepVendor(FAC.GUILD, REP.EXALTED, 46572, 51496, 51503, 51512, 52268)

	-- Armored Razzashi Raptor -- 96491
	mount = AddMount(96491, V.CATA, Q.EPIC) -- Item: 68823
	mount:SetItemID(68823)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.INSTANCE, F.IBOP)
	mount:AddMobDrop(52151)

	-- Swift Zulian Panther -- 96499
	mount = AddMount(96499, V.CATA, Q.EPIC) -- Item: 68824
	mount:SetItemID(68824)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.INSTANCE, F.IBOP)
	mount:AddMobDrop(52059)

	-- Amani Dragonhawk -- 96503
	mount = AddMount(96503, V.CATA, Q.EPIC) -- Item: 68825
	mount:SetItemID(68825)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Flameward Hippogryph -- 97359
	mount = AddMount(97359, V.CATA, Q.EPIC) -- Item: 69213
	mount:SetItemID(69213)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(5866)

	-- Pureblood Fire Hawk -- 97493
	mount = AddMount(97493, V.CATA, Q.EPIC) -- Item: 69224
	mount:SetItemID(69224)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(52409)

	-- Corrupted Fire Hawk -- 97560
	mount = AddMount(97560, V.CATA, Q.EPIC) -- Item: 69230
	mount:SetItemID(69230)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(5828)

	-- Savage Raptor -- 97581
	mount = AddMount(97581, V.CATA, Q.EPIC) -- Item: 69228
	mount:SetItemID(69228)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOP)
	mount:AddCustom("TCG")

	-- Amani Battle Bear -- 98204
	mount = AddMount(98204, V.CATA, Q.EPIC) -- Item: 69747
	mount:SetItemID(69747)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.INSTANCE, F.IBOP)
	mount:AddMobDrop(23577)

	-- Subdued Seahorse (Reins of Posedius) -- 98718
	mount = AddMount(98718, V.CATA, Q.EPIC) -- Item: 67151
	mount:SetItemID(67151)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	mount:AddMobDrop(50005)

	-- Winged Guardian -- 98727
	mount = AddMount(98727, V.CATA, Q.EPIC) -- Item: 69846
	mount:SetItemID(69846)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOA)
	mount:AddCustom("STORE")

	-- Vicious War Steed -- 100332
	mount = AddMount(100332, V.CATA, Q.EPIC) -- Item: 70909
	mount:SetItemID(70909)
	mount:AddFilters(F.ALLIANCE, F.IBOA, F.PVP, F.ACHIEVEMENT, F.VENDOR)
	mount:SetRequiredFaction("Alliance")
	mount:AddAchievement(5328, 5329, 5823)
	mount:AddVendor(73190)

	-- Vicious War Wolf -- 100333
	mount = AddMount(100333, V.CATA, Q.EPIC) -- Item: 70910
	mount:SetItemID(70910)
	mount:AddFilters(F.HORDE, F.IBOP, F.ACHIEVEMENT, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddAchievement(5325, 5326, 5824)
	mount:AddVendor(73151)

	-- Ruthless Gladiator's Twilight Drake -- 101821
	mount = AddMount(101821, V.CATA, Q.EPIC) -- Item: 71954
	mount:SetItemID(71954)
	mount:Retire()

	-- Vicious Gladiator's Twilight Drake -- 101282
	mount = AddMount(101282, V.CATA, Q.EPIC) -- Item: 71339
	mount:SetItemID(71339)
	mount:Retire()

	-- Flametalon of Alysrazor -- 101542
	mount = AddMount(101542, V.CATA, Q.EPIC) -- Item: 71665
	mount:SetItemID(71665)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(52530)

	-- Swift Shorestrider -- 101573
	mount = AddMount(101573, V.CATA, Q.EPIC) -- Item: 71718
	mount:SetItemID(71718)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Swift Forest Strider -- 102346
	mount = AddMount(102346, V.CATA, Q.EPIC) -- Item: 72140
	mount:SetItemID(72140)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.WORLD_EVENTS, F.IBOP)
	mount:AddVendor(14846)
	mount:AddWorldEvent("DARKMOON_FAIRE")

	-- Swift Springstrider -- 102349
	mount = AddMount(102349, V.CATA, Q.EPIC) -- Item: 72145
	mount:SetItemID(72145)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.VENDOR, F.IBOE)
	mount:AddVendor(32836, 32837)
	mount:AddWorldEvent("NOBLEGARDEN")
	mount:AddCustom("NOBLE_VENDOR")

	-- Swift Lovebird -- 102350
	mount = AddMount(102350, V.CATA, Q.EPIC) -- Item: 72146
	mount:SetItemID(72146)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.IBOE)
	mount:AddWorldEvent("LOVE_IS_IN_THE_AIR")
	mount:AddCustom("CITY")

	-- White Riding Camel -- 102488
	mount = AddMount(102488, V.CATA, Q.EPIC) -- Item: 72575
	mount:SetItemID(72575)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Corrupted Hippogryph -- 102514
	mount = AddMount(102514, V.CATA, Q.EPIC) -- Item: 72582
	mount:SetItemID(72582)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Darkmoon Dancing Bear -- 103081
	mount = AddMount(103081, V.CATA, Q.EPIC) -- Item: 73766
	mount:SetItemID(73766)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.WORLD_EVENTS, F.IBOP)
	mount:AddVendor(14846)
	mount:AddWorldEvent("DARKMOON_FAIRE")

	-- Mountain Horse -- 103195
	mount = AddMount(103195, V.CATA, Q.RARE) -- Item: 73838
	mount:SetItemID(73838)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(55285)

	-- Swift Mountain Horse -- 103196
	mount = AddMount(103196, V.CATA, Q.EPIC) -- Item: 73839
	mount:SetItemID(73839)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(55285)

	-- Tyrael's Charger -- 107203
	mount = AddMount(107203, V.CATA, Q.EPIC) -- Item: 76755
	mount:SetItemID(76755)
	mount:Retire()

	-- Spectral Gryphon -- 107516
	mount = AddMount(107516, V.CATA, Q.EPIC) -- Item: 76889
	mount:SetItemID(76889)
	mount:SetRequiredFaction("Alliance")
	mount:Retire()

	-- Spectral Wind Rider -- 107517
	mount = AddMount(107517, V.CATA, Q.EPIC) -- Item: 76902
	mount:SetItemID(76902)
	mount:SetRequiredFaction("Horde")
	mount:Retire()

	-- Blazing Drake -- 107842
	mount = AddMount(107842, V.CATA, Q.EPIC) -- Item: 77067
	mount:SetItemID(77067)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(56173)

	-- Twilight Harbinger -- 107844
	mount = AddMount(107844, V.CATA, Q.EPIC) -- Item: 77068
	mount:SetItemID(77068)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	mount:AddAchievement(6169)

	-- Life-Binder's Handmaiden -- 107845
	mount = AddMount(107845, V.CATA, Q.EPIC) -- Item: 77069
	mount:SetItemID(77069)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(56173)

	-- Experiment 12-B -- 110039
	mount = AddMount(110039, V.CATA, Q.EPIC) -- Item: 78919
	mount:SetItemID(78919)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	mount:AddMobDrop(55294)

	-- Heart of the Aspects -- 110051
	mount = AddMount(110051, V.CATA, Q.EPIC) -- Item: 78924
	mount:SetItemID(78924)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	mount:AddCustom("STORE")

	-- Obsidian Nightwing (Heart of the Nightwing) - 121820
	mount = AddMount(121820, V.CATA, Q.EPIC) -- Item: 83086
	mount:SetItemID(83086)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	mount:AddCustom("RECRUIT")

	-- Imperial Quilen -- 124659
	mount = AddMount(124659, V.CATA, Q.EPIC) -- Item: 85870
	mount:SetItemID(85870)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.COLLECTORS_EDITION, F.IBOP)
	mount:AddCustom("CE")

-- Mists of Pandaria

	-- Feldrake -- 113120
	mount = AddMount(113120, V.MOP, Q.EPIC) -- Item: 79771
	mount:SetItemID(79771)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Jade Cloud Serpent -- 113199
	mount = AddMount(113199, V.MOP, Q.EPIC)
	mount:SetItemID(79802)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.REPUTATION, F.ORDER_OF_THE_CLOUD_SERPENT)
	mount:AddRepVendor(FAC.ORDER_OF_THE_CLOUD_SERPENT, REP.EXALTED, 58414)
	mount:AddQuest(30188)

	-- Azure Water Strider -- 118089
	mount = AddMount(118089, V.MOP, Q.EPIC) -- Item: 81354
	mount:SetItemID(81354)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_ANGLERS)
	mount:AddRepVendor(FAC.THE_ANGLERS, REP.EXALTED, 63721)

	-- Pandaren Kite -- 118737
	mount = AddMount(118737, V.MOP, Q.EPIC)
	mount:SetItemID(81559)
	mount:AddFilters(F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:SetRequiredFaction("Horde")
	mount:AddAchievement(6827)

	-- Jeweled Onyx Panther -- 120043
	mount = AddMount(120043, V.MOP, Q.EPIC)
	mount:SetItemID(82453)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.JEWELCRAFTING)

	-- Green Dragon Turtle -- 120395
	mount = AddMount(120395, V.MOP, Q.EPIC)
	mount:SetItemID(82765)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Great Red Dragon Turtle -- 120822
	mount = AddMount(120822, V.MOP, Q.EPIC)
	mount:SetItemID(91010)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Sapphire Panther -- 121836
	mount = AddMount(121836, V.MOP, Q.RARE)
	mount:SetItemID(83090)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.JEWELCRAFTING)

	-- Jade Panther -- 121837
	mount = AddMount(121837, V.MOP, Q.RARE)
	mount:SetItemID(83088)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.JEWELCRAFTING)

	-- Ruby Panther -- 121838
	mount = AddMount(121838, V.MOP, Q.RARE)
	mount:SetItemID(83087)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.JEWELCRAFTING)

	-- Sunstone Panther -- 121839
	mount = AddMount(121839, V.MOP, Q.RARE)
	mount:SetItemID(83089)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.JEWELCRAFTING)

	-- Grand Expedition Yak -- 122708
	mount = AddMount(122708, V.MOP, Q.EPIC)
	mount:SetItemID(84101)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP)
	mount:AddVendor(64518)

	-- Amber Scorpion -- 123886
	mount = AddMount(123886, V.MOP, Q.EPIC) -- Item: 85262
	mount:SetItemID(85262)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_KLAXXI)
	mount:AddRepVendor(FAC.THE_KLAXXI, REP.EXALTED, 64599)

	-- Azure Cloud Serpent -- 123992
	mount = AddMount(123992, V.MOP, Q.EPIC) -- Item: 85430
	mount:SetItemID(85430)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.REPUTATION, F.ORDER_OF_THE_CLOUD_SERPENT)
	mount:AddRepVendor(FAC.ORDER_OF_THE_CLOUD_SERPENT, REP.EXALTED, 58414)
	mount:AddQuest(31810)

	-- Golden Cloud Serpent -- 123993
	mount = AddMount(123993, V.MOP, Q.EPIC)
	mount:SetItemID(85429)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.REPUTATION, F.ORDER_OF_THE_CLOUD_SERPENT)
	mount:AddRepVendor(FAC.ORDER_OF_THE_CLOUD_SERPENT, REP.EXALTED, 58414)
	mount:AddQuest(31811)

	-- Thundering Jade Cloud Serpent -- 124408
	mount = AddMount(124408, V.MOP, Q.EPIC)
	mount:SetItemID(85666)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:AddAchievement(6682)

	-- Cataclysmic Gladiator's Twilight Drake -- 124550
	mount = AddMount(124550, V.MOP, Q.EPIC)
	mount:SetItemID(85785)
	mount:Retire()

	-- Depleted-Kyparium Rocket -- 126507
	mount = AddMount(126507, V.MOP, Q.RARE)
	mount:SetItemID(87250)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.ENGINEERING)

	-- Geosynchronous World Spinner -- 126508
	mount = AddMount(126508, V.MOP, Q.RARE)
	mount:SetItemID(87251)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	mount:AddProfession(PROF.ENGINEERING)

	-- Onyx Cloud Serpent -- 127154
	mount = AddMount(127154, V.MOP, Q.EPIC)
	mount:SetItemID(87768)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	mount:AddQuest(31277)

	-- Crimson Cloud Serpent -- 127156
	mount = AddMount(127156, V.MOP, Q.EPIC)
	mount:SetItemID(87769)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:AddAchievement(6927)

	-- Heavenly Onyx Cloud Serpent -- 127158
	mount = AddMount(127158, V.MOP, Q.EPIC)
	mount:SetItemID(87771)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(60491)

	-- Heavenly Crimson Cloud Serpent -- 127161
	mount = AddMount(127161, V.MOP, Q.EPIC)
	mount:SetItemID(87773)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:AddAchievement(6932)

	-- Heavenly Golden Cloud Serpent -- 127164
	mount = AddMount(127164, V.MOP, Q.EPIC)
	mount:SetItemID(87774)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.EMPEROR_SHAOHAO)
	mount:AddRepVendor(FAC.EMPEROR_SHAOHAO, REP.EXALTED, 73306)

	-- Astral Cloud Serpent -- 127170
	mount = AddMount(127170, V.MOP, Q.EPIC) -- Item: 87777
	mount:SetItemID(87777)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(60410)

	-- Azure Riding Crane -- 127174
	mount = AddMount(127174, V.MOP, Q.EPIC) -- Item: 87781
	mount:SetItemID(87781)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.GOLDEN_LOTUS)
	mount:AddRepVendor(FAC.GOLDEN_LOTUS, REP.EXALTED, 59908)

	-- Golden Riding Crane -- 127176
	mount = AddMount(127176, V.MOP, Q.EPIC)
	mount:SetItemID(87782)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.GOLDEN_LOTUS)
	mount:AddRepVendor(FAC.GOLDEN_LOTUS, REP.EXALTED, 59908)

	-- Regal Riding Crane -- 127177
	mount = AddMount(127177, V.MOP, Q.EPIC)
	mount:SetItemID(87783)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.GOLDEN_LOTUS)
	mount:AddRepVendor(FAC.GOLDEN_LOTUS, REP.EXALTED, 59908)

	-- Grey Riding Yak -- 127216
	mount = AddMount(127216, V.MOP, Q.EPIC)
	mount:SetItemID(87788)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP)
	mount:AddVendor(64518)

	-- Blonde Riding Yak -- 127220
	mount = AddMount(127220, V.MOP, Q.EPIC)
	mount:SetItemID(87789)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP)
	mount:AddVendor(64518)

	-- Black Dragon Turtle -- 127286
	mount = AddMount(127286, V.MOP, Q.EPIC) -- Item: 91008
	mount:SetItemID(91008)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Blue Dragon Turtle -- 127287
	mount = AddMount(127287, V.MOP, Q.EPIC)
	mount:SetItemID(87796)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Brown Dragon Turtle -- 127288
	mount = AddMount(127288, V.MOP, Q.EPIC)
	mount:SetItemID(87797)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Purple Dragon Turtle -- 127289
	mount = AddMount(127289, V.MOP, Q.EPIC)
	mount:SetItemID(87799)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Red Dragon Turtle -- 127290
	mount = AddMount(127290, V.MOP, Q.EPIC)
	mount:SetItemID(87800)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Great Green Dragon Turtle -- 127293
	mount = AddMount(127293, V.MOP, Q.EPIC)
	mount:SetItemID(87801)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Great Black Dragon Turtle -- 127295
	mount = AddMount(127295, V.MOP, Q.EPIC)
	mount:SetItemID(91011)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Great Blue Dragon Turtle -- 127302
	mount = AddMount(127302, V.MOP, Q.EPIC)
	mount:SetItemID(87803)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Great Brown Dragon Turtle -- 127308
	mount = AddMount(127308, V.MOP, Q.EPIC)
	mount:SetItemID(87804)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Great Purple Dragon Turtle -- 127310
	mount = AddMount(127310, V.MOP, Q.EPIC)
	mount:SetItemID(87805)
	mount:AddFilters(F.ALLIANCE, F.HORDE,  F.VENDOR, F.IBOP, F.PANDACOMMON1)
	mount:AddRepVendor(FAC.HUOJIN_PANDAREN, REP.EXALTED, 66022)
	mount:AddRepVendor(FAC.TUSHUI_PANDAREN, REP.EXALTED, 65068)

	-- Crimson Pandaren Phoenix -- 129552
	mount = AddMount(129552, V.MOP, Q.EPIC)
	mount:SetItemID(89154)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(63994, 64028, 66973)

	-- Thundering August Cloud Serpent -- 129918
	mount = AddMount(129918, V.MOP, Q.EPIC)
	mount:SetItemID(89304)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.THE_AUGUST_CELESTIALS)
	mount:AddRepVendor(FAC.THE_AUGUST_CELESTIALS, REP.EXALTED, 64001, 64032)

	-- Green Shado-Pan Riding Tiger -- 129932
	mount = AddMount(129932, V.MOP, Q.EPIC)
	mount:SetItemID(89305)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHADO_PAN)
	mount:AddRepVendor(FAC.SHADO_PAN, REP.EXALTED, 64595)

	-- Blue Shado-Pan Riding Tiger -- 129934
	mount = AddMount(129934, V.MOP, Q.EPIC)
	mount:SetItemID(89307)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHADO_PAN)
	mount:AddRepVendor(FAC.SHADO_PAN, REP.EXALTED, 64595)

	-- Red Shado-Pan Riding Tiger -- 129935
	mount = AddMount(129935, V.MOP, Q.EPIC)
	mount:SetItemID(89306)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.SHADO_PAN)
	mount:AddRepVendor(FAC.SHADO_PAN, REP.EXALTED, 64595)

	-- Brown Riding Goat -- 130086
	mount = AddMount(130086, V.MOP, Q.EPIC)
	mount:SetItemID(89362)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_TILLERS)
	mount:AddRepVendor(FAC.THE_TILLERS, REP.EXALTED, 58706)

	-- Red Flying Cloud -- 130092
	mount = AddMount(130092, V.MOP, Q.RARE)
	mount:SetItemID(89363)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.REPUTATION, F.IBOP, F.THE_LOREWALKERS)
	mount:AddRepVendor(FAC.THE_LOREWALKERS, REP.EXALTED, 64605)

	-- White Riding Goat -- 130137
	mount = AddMount(130137, V.MOP, Q.EPIC)
	mount:SetItemID(89390)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_TILLERS)
	mount:AddRepVendor(FAC.THE_TILLERS, REP.EXALTED, 58706)

	-- Black Riding Goat -- 130138
	mount = AddMount(130138, V.MOP, Q.EPIC) -- Item: 89391
	mount:SetItemID(89391)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_TILLERS)
	mount:AddRepVendor(FAC.THE_TILLERS, REP.EXALTED, 58706)

	-- Son of Galleon -- 130965
	mount = AddMount(130965, V.MOP, Q.EPIC)
	mount:SetItemID(89783)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(62346)

	-- Pandaren Kite -- 130985
	mount = AddMount(130985, V.MOP, Q.EPIC)
	mount:SetItemID(89785)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.ACHIEVEMENT)
	mount:SetRequiredFaction("Alliance")
	mount:AddAchievement(6828)

	-- Thundering Ruby Cloud Serpent -- 132036
	mount = AddMount(132036, V.MOP, Q.EPIC)
	mount:SetItemID(90655)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.THE_AUGUST_CELESTIALS, F.IBOP)
	mount:AddMobDrop(64403)

	-- Ashen Pandaren Phoenix -- 132117
	mount = AddMount(132117, V.MOP, Q.EPIC) -- Item: 90710
	mount:SetItemID(90710)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(63994, 64028, 66973)

	-- Emerald Pandaren Phoenix -- 132118
	mount = AddMount(132118, V.MOP, Q.EPIC)
	mount:SetItemID(90711)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(63994, 64028, 66973)

	-- Violet Pandaren Phoenix -- 132119
	mount = AddMount(132119, V.MOP, Q.EPIC)
	mount:SetItemID(90712)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(63994, 64028, 66973)

	-- Jade Pandaren Kite String -- 133023
	mount = AddMount(133023, V.MOP, Q.EPIC)
	mount:SetItemID(91802)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:AddAchievement(7860, 7862)

	-- Sky Golem -- 134359
	mount = AddMount(134359, V.MOP, Q.EPIC)
	mount:SetItemID(95416)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	mount:AddProfession(PROF.ENGINEERING)

	-- Swift Windsteed -- 134573
	mount = AddMount(134573, V.MOP, Q.EPIC)
	mount:SetItemID(92724)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	mount:AddCustom("STORE")

	-- Grand Armored Gryphon -- 135416
	mount = AddMount(135416, V.MOP, Q.EPIC)
	mount:SetItemID(93168)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.VENDOR)
	mount:AddVendor(69059)
	mount:SetRequiredFaction("Alliance")

	-- Grand Armored Wyvern -- 135418
	mount = AddMount(135418, V.MOP, Q.EPIC)
	mount:SetItemID(93169)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(69060)
	mount:SetRequiredFaction("Horde")

	-- Grand Gryphon -- 136163
	mount = AddMount(136163, V.MOP, Q.EPIC)
	mount:SetItemID(93385)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.QUEST)
	mount:AddQuest(32455)
	mount:SetRequiredFaction("Alliance")

	-- Grand Wyvern -- 136164
	mount = AddMount(136164, V.MOP, Q.EPIC)
	mount:SetItemID(93386)
	mount:AddFilters(F.HORDE, F.IBOP, F.QUEST)
	mount:AddQuest(32399)
	mount:SetRequiredFaction("Horde")

	-- Armored Skyscreamer -- 136400
	mount = AddMount(136400, V.MOP, Q.EPIC)
	mount:SetItemID(93662)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	mount:AddAchievement(8124)

	-- Spawn of Horridon -- 136471
	mount = AddMount(136471, V.MOP, Q.EPIC)
	mount:SetItemID(93666)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(68476)

	-- Ghastly Charger's Skull -- 136505
	mount = AddMount(136505, V.MOP, Q.EPIC)
	mount:SetItemID(93671)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.TCG, F.IBOE)
	mount:AddCustom("TCG")

	-- Cobalt Primordial Direhorn -- 138423
	mount = AddMount(138423, V.MOP, Q.EPIC)
	mount:SetItemID(94228)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(69161)

	-- Amber Primordial Direhorn -- 138424
	mount = AddMount(138424, V.MOP, Q.EPIC)
	mount:SetItemID(94230)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(69841)
	mount:AddCustom("DIREHORN")

	-- Slate Primordial Direhorn -- 138425
	mount = AddMount(138425, V.MOP, Q.EPIC)
	mount:SetItemID(94229)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(69769)
	mount:AddCustom("DIREHORN")

	-- Jade Primordial Direhorn -- 138426
	mount = AddMount(138426, V.MOP, Q.EPIC)
	mount:SetItemID(94231)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(69842)
	mount:AddCustom("DIREHORN")

	-- Bone-White Primal Raptor -- 138640
	mount = AddMount(138640, V.MOP, Q.EPIC)
	mount:SetItemID(94290)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	mount:AddQuest(32617)

	-- Red Primal Raptor -- 138641
	mount = AddMount(138641, V.MOP, Q.EPIC)
	mount:SetItemID(94291)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	mount:AddCustom("PRIMAL_EGG")

	-- Black Primal Raptor -- 138642
	mount = AddMount(138642, V.MOP, Q.EPIC)
	mount:SetItemID(94292)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	mount:AddCustom("PRIMAL_EGG")

	-- Green Primal Raptor -- 138643
	mount = AddMount(138643, V.MOP, Q.EPIC)
	mount:SetItemID(94293)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	mount:AddCustom("PRIMAL_EGG")

	-- Thundering Cobalt Cloud Serpent -- 139442
	mount = AddMount(139442, V.MOP, Q.EPIC)
	mount:SetItemID(95057)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(69099)

	-- Clutch of Ji-Kun -- 139448
	mount = AddMount(139448, V.MOP, Q.EPIC)
	mount:SetItemID(95059)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(69712)

	-- Armored Bloodwing -- 139595
	mount = AddMount(139595, V.MOP, Q.EPIC)
	mount:SetItemID(95341)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	mount:AddCustom("STORE")

	-- Crimson Golden Direhorn -- 140249
	mount = AddMount(140249, V.MOP, Q.EPIC)
	mount:SetItemID(95564)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.REPUTATION, F.PANDACOMMON3)
	mount:AddRepVendor(FAC.KIRIN_TOR_OFFENSIVE, REP.EXALTED, 68000)

	-- Crimson Primal Direhorn -- 140250
	mount = AddMount(140250, V.MOP, Q.EPIC)
	mount:SetItemID(95565)
	mount:AddFilters(F.HORDE, F.IBOP, F.REPUTATION, F.PANDACOMMON3)
	mount:AddRepVendor(FAC.SUNREAVER_ONSLAUGHT, REP.EXALTED, 67672)

	-- Hearthsteed -- 142073
	mount = AddMount(142073, V.MOP, Q.EPIC)
	mount:SetItemID(98618)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP)
	mount:AddAchievement(8345)

	-- Armored Red Dragonhawk -- 142266
	mount = AddMount(142266, V.MOP, Q.EPIC)
	mount:SetItemID(98104)
	mount:AddFilters(F.HORDE, F.IBOP)
	mount:AddAchievement(8302)

	-- Armored Blue Dragonhawk -- 142478
	mount = AddMount(142478, V.MOP, Q.EPIC)
	mount:SetItemID(98259)
	mount:AddFilters(F.ALLIANCE, F.IBOP)
	mount:AddAchievement(8304)

	-- Brawler's Burly Mushan Beast -- 142641
	mount = AddMount(142641, V.MOP, Q.EPIC)
	mount:SetItemID(98405)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP)
	mount:AddVendor(68363, 68364)

	-- Enchanted Fey Dragon -- 142878
	mount = AddMount(142878, V.MOP, Q.EPIC)
	mount:SetItemID(97989)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	mount:AddCustom("STORE")

	-- Vicious Warsaber -- 146615
	mount = AddMount(146615, V.MOP, Q.EPIC)
	mount:SetItemID(102514)
	mount:AddFilters(F.ALLIANCE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Alliance")
	mount:AddVendor(73190)

	-- Vicious Skeletal Warhorse -- 146622
	mount = AddMount(146622, V.MOP, Q.EPIC)
	mount:SetItemID(102533)
	mount:AddFilters(F.HORDE, F.IBOP, F.VENDOR)
	mount:SetRequiredFaction("Horde")
	mount:AddVendor(73151)

	-- Spawn of Galakras -- 148392
	mount = AddMount(148392, V.MOP, Q.EPIC)
	mount:SetItemID(104208)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.ACHIEVEMENT)
	mount:AddAchievement(8454)

	-- Kor'kron War Wolf -- 148396
	mount = AddMount(148396, V.MOP, Q.EPIC)
	mount:SetItemID(104246)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID, F.ACHIEVEMENT)
	mount:AddAchievement(8398, 8399)

	-- Kor'kron Juggernaut -- 148417
	mount = AddMount(148417, V.MOP, Q.EPIC)
	mount:SetItemID(104253)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.RAID)
	mount:AddMobDrop(71865)
	mount:AddCustom("HEROIC")

	-- Ashhide Mushan Beast -- 148428
	mount = AddMount(148428, V.MOP, Q.EPIC)
	mount:SetItemID(103638)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	mount:AddVendor(73307)

	-- Thundering Onyx Cloud Serpent -- 148476
	mount = AddMount(148476, V.MOP, Q.EPIC)
	mount:SetItemID(104269)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	mount:AddMobDrop(73167)

	-- Tyrannical Gladiator's Cloud Serpent -- 148618
	mount = AddMount(148618, V.MOP, Q.EPIC)
	mount:SetItemID(104208)
	mount:Retire()

	-- Grievous Gladiator's Cloud Serpent -- 148619  -- Season 14
	mount = AddMount(148619, V.MOP, Q.EPIC)
	mount:SetItemID(104326)
	mount:Retire()

	-- Emerald Hippogryph -- 149801
	mount = AddMount(149801, V.MOP, Q.EPIC)
	mount:SetItemID(106246)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	mount:AddCustom("RECRUIT")

	-- Iron Skyreaver -- 153489
	mount = AddMount(153489, V.MOP, Q.EPIC)
	mount:SetItemID(107951)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	mount:AddCustom("STORE")

	-- Reins of the Dread Raven -- 155741
	mount = AddMount(155741, V.MOP, Q.EPIC)
	mount:SetItemID(109013)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	mount:AddCustom("CE")

	-- Warforged Nightmare -- 163024
	mount = AddMount(163024, V.MOP, Q.EPIC)
	mount:SetItemID(112326)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	mount:AddCustom("STORE")

	self.InitMounts = nil
end

--[[
Apparentely not in game yet!

	-- Prideful Gladiator's Cloud Serpent -- 148620  -- Season 15
	mount = AddMount(148620, V.MOP, Q.EPIC)
	mount:SetItemID(104327)
	mount:Retire()

	-- Grinning Reaver -- 163025
	mount = AddMount(163025, V.WOD, Q.EPIC)
	mount:SetItemID(112327)
	mount:AddFilters(F.ALLIANCE, F.HORDE)

-- Albino Riding Crane
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Black Polar Bear -- 59572
-- mount = AddMount(62048, V.TBC,) -- Item: 43964
-- mount:SetItemID(43964)
-- mount:AddFilters(F.ALLIANCE, F.HORDE)

-- Black Dragonhawk Mount -- 62048
-- mount = AddMount(62048, V.WRATH,) -- Item:
-- mount:SetItemID()
-- mount:AddFilters(F.ALLIANCE, F.HORDE)

-- Black Riding Yak
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Black Warp Stalker
-- mount = AddMount(, V.TBC, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Blue Ram
-- mount = AddMount(, V.CATA, )
-- mount:AddFilters(, F.ALLIANCE, )
-- self:AddCompanionAcquire(DB,)

-- Brewfest Kodo  -- Only learn this mount if you don't have 100% riding, it is in-game however.
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Brown Riding Yak
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Brown Polar Bear
-- mount = AddMount(, V.WRATH, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Crimson Riding Crane
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Crimson Water Strider
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Fluorescent Green Mechanostrider -- Only 1, on EU servers accidentally given to a player.
-- mount = AddMount(, V.TBC, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Golden Sabercat
-- mount = AddMount(16060, V.ORIG, )
-- mount:SetItemID(12327)
-- mount:AddFilters(, F.ALLIANCE, )
-- self:AddCompanionAcquire(DB,)

-- Golden Water Strider
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Green Fire Hawk
-- mount = AddMount(97501, V.CATA, Q.EPIC)
-- mount:SetItemID(69226)
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Heavenly Azure Cloud Serpent
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Heavenly Jade Cloud Serpent
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Jade Water Strider
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Jungle Riding Crane
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

	-- Lucky Riding Turtle  -- 30174
	-- mount = AddMount(30174, V.MOP, Q.EPIC)
	-- mount:SetItemID(103630)
	-- mount:AddFilters()

-- Orange Water Strider
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

-- Peep's Whistle -- 32345
-- mount = AddMount(32345, V.TBC, Q.LEGENDARY) -- Item: 25596
-- mount:SetItemID(25596)
-- mount:AddFilters(F.ALLIANCE, F.HORDE)

-- Primal Leopard
-- mount = AddMount(16058, V.ORIG, )
-- mount:AddFilters(12325, F.ALLIANCE, )
-- self:AddCompanionAcquire(DB,)

-- Purple Mechanostrider
-- mount = AddMount(17455, V.ORIG, )
-- mount:AddFilters(13323, F.ALLIANCE, )
-- self:AddCompanionAcquire(DB,)

	-- Stormcrow -- 147595
	mount = AddMount(147595, V.MOP, Q.EPIC)
	mount:SetItemID(104011)
	mount:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)

-- White Riding Yak
-- mount = AddMount(, V.MOP, )
-- mount:AddFilters(, F.ALLIANCE, F.HORDE, )
-- self:AddCompanionAcquire(DB,)

]]--