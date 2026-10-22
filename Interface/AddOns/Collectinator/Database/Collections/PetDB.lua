--[[
************************************************************************
PetDB.lua
************************************************************************
File date: 2014-06-20T01:20:32Z
File hash: 7dcdac2
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

-- Libraries
local table = _G.table

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub	= _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)
local LPJ = LibStub("LibPetJournal-2.0")

-------------------------------------------------------------------------------
-- Filter flags. Acquire types, and Reputation levels.
-------------------------------------------------------------------------------
local F = private.FILTER_IDS
local A = private.ACQUIRE_TYPES
local Q = private.ITEM_QUALITIES
local REP = private.REP_LEVELS
local FAC = private.FACTION_IDS
local V = private.GAME_VERSIONS
local Z = private.ZONE_NAMES
local PROF = private.LOCALIZED_PROFESSION_NAMES
local TOD = private.TIME_OF_DAY
local WEATHER = private.WEATHER
local SEASON = private.SEASON

--------------------------------------------------------------------------------------------------------------------
-- Initialize!
--------------------------------------------------------------------------------------------------------------------

local UpdatePetList
do
	local known_pets = {}

	function UpdatePetList()
		local pet_journal = _G.C_PetJournal

		-- Reset all flags so the scan will actually work
		pet_journal.SetFlagFilter(_G.LE_PET_JOURNAL_FLAG_COLLECTED, true)
		pet_journal.SetFlagFilter(_G.LE_PET_JOURNAL_FLAG_FAVORITES, false)
		pet_journal.SetFlagFilter(_G.LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true)
		pet_journal.AddAllPetTypesFilter()
		pet_journal.AddAllPetSourcesFilter()

		local search_box = _G.PetJournalSearchBox
		search_box:ClearFocus()
		search_box:SetText(_G.SEARCH)
		_G.PetJournal_OnSearchTextChanged(search_box)

		local pet_list = private.collectable_list["CRITTER"]

		if not pet_list then
			return
		end
		table.wipe(known_pets)

		for index, pet_id in LPJ:IteratePetIDs() do
			local _, _, _, _, _, _, _, _, _, _, creature_id = _G.C_PetJournal.GetPetInfoByPetID(pet_id)
			local pet = pet_list[creature_id]

			if pet then
				pet:AddState("KNOWN")
				known_pets[creature_id] = true
			end
		end
		local num_pets = _G.C_PetJournal.GetNumPets(_G.PetJournal.isWild)

		for pet_index = 1, num_pets do
			local pet_id, _, _, _, _, _, _, name, icon, pet_type, creature_id, source_text, description, is_wild, can_battle = _G.C_PetJournal.GetPetInfoByIndex(pet_index, false)
			local pet = pet_list[creature_id]

			if pet then
				if not known_pets[creature_id] then
					pet:RemoveState("KNOWN")
				end
				pet:SetName(name)
				pet:SetIcon(icon)
				pet:SetDescription(description)
			end
		end
	end
end


function addon:InitCritters()
	local function AddPet(spell_id, genesis, quality)
		return addon:AddCollectable(spell_id, "CRITTER", genesis, quality)
	end
	local pet

	-- Albino Snake -- 7561
	pet = AddPet(7561, V.WOTLK, Q.COMMON)
	pet:SetItemID(44822)
	pet:SetSpellID(10713)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(28951)

	-- Alliance Balloon -- 54539
	pet = AddPet(54539, V.CATA, Q.COMMON)
	pet:SetItemID(72042)
	pet:SetSpellID(101986)
	pet:SetRequiredFaction("Alliance")
	pet:AddFilters(F.ALLIANCE, F.QUEST, F.IBOP)
	pet:AddQuest(29412)

	-- Alpine Foxling -- 63550
	pet = AddPet(63550, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-24", false, "51.0:59.4", "39.8:68.0", "57.6:56.8", "64.8:45.8")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24-25", false, "64.2:34.4", "62.0:33.6")

	-- Alpine Foxling Kit -- 63551
	pet = AddPet(63551, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "43.6:65.6", "52.6:62.6", "32.6:50.6", "54.6:41.8")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "23-25", false, "64.2:34.2")

	-- Alterac Brew-Pup -- 74402
	pet = AddPet(74402, V.MOP, Q.RARE)
	pet:SetItemID(106240)
	pet:SetSpellID(149787)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	pet:AddCustom("STORE")

	-- Amber Moth -- 65187
	pet = AddPet(65187, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "24", false, "30.6:78.6", "55.0:33.6", "45.0:18.6", "47.4:16.8", "49.6:13.6")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24", false, "47.2:77.2", "48.2:78.2", "50.0:80.2", "66.8:80.6", "64.6:83.0")

	-- Amethyst Spiderling -- 63288
	pet = AddPet(63288, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "31.8:75.6", "37.0:58.2", "78.6:19.4", "26.6:54.4", "38.4:34.6")
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "12.6:85.6", "16.2:33.0")

	-- Ammen Vale Lashling -- 33205
	pet = AddPet(33205, V.WOTLK, Q.RARE)
	pet:SetItemID(44984)
	pet:SetSpellID(62562)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(33657)
	pet:AddCustom("CHAMPION_SEALS")

	-- Ancona Chicken -- 7394
	pet = AddPet(7394, V.ORIG, Q.COMMON)
	pet:SetItemID(11023)
	pet:SetSpellID(10685)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(41135)

	-- Anodized Robo Cub -- 68839
	pet = AddPet(68839, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.WINTERSPRING, "17-18", false, "61.6:51.8", "59.0:52.8", "58.2:47.4", "60.6:45.6", "60.2:50.6")

	-- Anubisath Idol -- 68659
	pet = AddPet(68659, V.MOP, Q.RARE)
	pet:SetItemID(93040)
	pet:SetSpellID(135267)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(15276)

	-- Arcane Eye -- 68819
	pet = AddPet(68819, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DEADWIND_PASS, "17-18", false, "42.6:75.8", "48.2:76.6", "50.0:69.6", "52.6:69.8", "42.2:74.6", "40.0:76.6")

	-- Argent Gruntling -- 33239
	pet = AddPet(33239, V.WOTLK, Q.RARE)
	pet:SetItemID(45022)
	pet:SetSpellID(62746)
	pet:SetRequiredFaction("Horde")
	pet:AddFilters(F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(13736)

	-- Argent Squire -- 33238
	pet = AddPet(33238, V.WOTLK, Q.RARE)
	pet:SetItemID(44998)
	pet:SetSpellID(62609)
	pet:SetRequiredFaction("Alliance")
	pet:AddFilters(F.ALLIANCE, F.QUEST, F.IBOP)
	pet:AddQuest(13702)

	-- Armadillo Pup -- 48242
	pet = AddPet(48242, V.CATA, Q.RARE)
	pet:SetItemID(63398)
	pet:SetSpellID(89670)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.GUILD, F.IBOP, F.ACHIEVEMENT, F.REPUTATION)
	pet:AddRepVendor(FAC.GUILD, REP.EXALTED, 46572, 51496, 51502, 51512, 51501, 51503, 52268, 46602, 51495, 51504)

	-- Ashleaf Spriteling -- 73533
	pet = AddPet(73533, V.MOP, Q.RARE)
	pet:SetItemID(104156)
	pet:SetSpellID(148046)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(73277)

	-- Ashstone Core -- 68666
	pet = AddPet(68666, V.MOP, Q.RARE)
	pet:SetItemID(93035)
	pet:SetSpellID(135261)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	pet:AddMobDrop(11988)

	-- Ashwing Moth -- 73542
	pet = AddPet(73542, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TIMELESS_ISLE, "25", false, "47.2:46.6", "41.6:64.8", "30.6:66.5")

	-- Aqua Strider -- 64633
	pet = AddPet(64633, V.MOP, Q.RARE)
	pet:SetItemID(86563)
	pet:SetSpellID(126249)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(50776)

	-- Azure Crane Chick -- 73534
	pet = AddPet(73534, V.MOP, Q.RARE)
	pet:SetItemID(104157)
	pet:SetSpellID(148047)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	pet:AddCustom("CRANENEST")

	-- Azure Whelpling -- 7547
	pet = AddPet(7547, V.ORIG, Q.COMMON)
	pet:SetItemID(34535)
	pet:SetSpellID(10696)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_DROP)
	pet:AddWorldDrop(Z.WINTERSPRING)

	-- Baby Blizzard Bear -- 32841
	pet = AddPet(32841, V.WOTLK, Q.COMMON)
	pet:SetItemID(44819)
	pet:SetSpellID(61855)
	pet:Retire()

	-- Bananas -- 23234
	pet = AddPet(23234, V.TBC, Q.RARE)
	pet:SetItemID(32588)
	pet:SetSpellID(40549)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Bandicoon -- 63062
	pet = AddPet(63062, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "74.8:38.2", "39.8:43.8", "47.6:35.2", "23.6:45.4", "36.0:34.4")
	pet:AddZoneLocations(Z.THE_VEILED_STAIR, "23-24", false, "63.6:82.6", "68.4:81.0", "68.4:66.8", "57.6:63.6")
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "29.4:61.2", "28.0:59.2")

	-- Bandicoon Kit -- 63064
	pet = AddPet(63064, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "55.2:42.0", "79.6:29.6", "50.8:44.6", "73.6:25.0")
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "28.0:59.4")

	-- Baneling -- 66984
	pet = AddPet(66984, V.MOP, Q.RARE)
	pet:SetItemID(90177)
	pet:SetSpellID(131650)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("STARCRAFTCE")

	-- Black Kingsnake -- 7565
	pet = AddPet(7565, V.ORIG, Q.COMMON)
	pet:SetItemID(10360)
	pet:SetSpellID(10714)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(8404)

	-- Black Rat -- 61257
	pet = AddPet(61257, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BADLANDS, "13-14", false, "22.2:58.6", "17.2:62.8", "32.8:40.6", "49.0:29.6")
	pet:AddZoneLocations(Z.DUN_MOROGH, "1-2", true)
	pet:AddZoneLocations(Z.DUSKWOOD, "5-7", false, "78.6:33.8", "43.6:73.6", "41.8:64.6", "21.8:53.6")
	pet:AddZoneLocations(Z.DUSTWALLOW_MARSH, "12-13", false, "51.4:71.6", "49.6:75.0", "49.0:81.2")
	pet:AddZoneLocations(Z.EASTERN_PLAGUELANDS, "12-13", false, "74.4:58.4", "35.0:85.2", "21.0:64.8", "54.6:67.6")
	pet:AddZoneLocations(Z.KEZAN, "1", true)
	pet:AddZoneLocations(Z.THOUSAND_NEEDLES, "13-14", false, "94.0:76.6", "92.6:75.8", "91.6:78.8")
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "50.8:33.6", "46.2:56.0", "23.0:22.4", "19.8:15.2")
	pet:AddZoneLocations(Z.WESTERN_PLAGUELANDS, "10-11", false, "63.6:54.2", "50.8:76.6", "69.0:77.0")
	pet:AddZoneLocations(Z.WETLANDS, "6-7", false, "63.4:41.2", "58.4:46.6", "54.6:27.8", "58.6:24.6")

	-- Black Tabby Cat -- 7383
	pet = AddPet(7383, V.ORIG, Q.COMMON)
	pet:SetItemID(8491)
	pet:SetSpellID(10675)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.IBOE)
	pet:AddWorldDrop(Z.HILLSBRAD_FOOTHILLS)

	-- Blackfuse Bombling -- 73352
	pet = AddPet(73352, V.MOP, Q.RARE)
	pet:SetItemID(104158)
	pet:SetSpellID(148049)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(71504)

	-- Blossoming Ancient -- 71488
	pet = AddPet(71488, V.MOP, Q.COMMON)
	pet:SetItemID(98550)
	pet:SetSpellID(142880)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.STORE)
	pet:AddCustom("STORE")

	-- Blue Clockwork Rocket Bot -- 40295
	pet = AddPet(40295, V.WOTLK, Q.RARE)
	pet:SetItemID(54436)
	pet:SetSpellID(75134)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.QUEST, F.WORLD_EVENTS)
	pet:AddVendor(52809, 29716, 52358, 29478)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")
	pet:AddQuest(6983, 7043)

	-- Blue Dragonhawk Hatchling -- 21056
	pet = AddPet(21056, V.TBC, Q.COMMON)
	pet:SetItemID(29958)
	pet:SetSpellID(36031)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(20980)

	-- Blue Mini Jouster -- 42177
	pet = AddPet(42177, V.CATA, Q.COMMON)
	pet:SetItemID(65661)
	pet:SetSpellID(78683)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(25560)

	-- Blue Moth -- 21010
	pet = AddPet(21010, V.TBC, Q.COMMON)
	pet:SetItemID(29901)
	pet:SetSpellID(35907)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(21019)

	-- Bombay Cat -- 7385
	pet = AddPet(7385, V.ORIG, Q.COMMON)
	pet:SetItemID(8485)
	pet:SetSpellID(10673)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(6367)

	-- Bonkers -- 73668
	pet = AddPet(73668, V.MOP, Q.RARE)
	pet:SetItemID(104202)
	pet:SetSpellID(148373)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	pet:AddCustom("TIMELESS_CHEST")

	-- Brilliant Kaliri -- 54374
	pet = AddPet(54374, V.CATA, Q.COMMON)
	pet:SetItemID(71387)
	pet:SetSpellID(101424)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(5877)

	-- Brown Marmot -- 61752
	pet = AddPet(61752, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BLADES_EDGE_MOUNTAINS,  "18-20", true)
	pet:AddZoneLocations(Z.THE_HINTERLANDS, "11-12" , false, "51.2:53.6", "63.0:43.2", "52.2:20.4", "36.6:48.8")

	-- Brown Prairie Dog -- 14421
	pet = AddPet(14421, V.ORIG, Q.COMMON)
	pet:SetItemID(10394)
	pet:SetSpellID(10709)
	pet:AddFilters(F.HORDE, F.VENDOR, F.TRAINER, F.IBOE)
	pet:AddTrainer(63067, 63086, 63061, 63626)

	-- Brown Rabbit -- 20472
	pet = AddPet(20472, V.ORIG, Q.COMMON)
	pet:SetItemID(29364)
	pet:SetSpellID(35239)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(20980)

	-- Brown Snake -- 7562
	pet = AddPet(7562, V.ORIG, Q.COMMON)
	pet:SetItemID(10361)
	pet:SetSpellID(10716)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(8404)

	-- Bucktooth Flapper -- 62992
	pet = AddPet(62992, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "48.2:26.4", "46.2:36.6", "26.4:33.0", "36.6:49.4", "41.6:90.6", "56.0:80.4")

	-- Calico Cat -- 34364
	pet = AddPet(34364, V.WOTLK, Q.COMMON)
	pet:SetItemID(46398)
	pet:SetSpellID(65358)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(28951)

	-- Carrion Rat -- 62885
	pet = AddPet(62885, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.MOUNT_HYJAL, "22-23", false, "78.6:50.2", "30.4:75.6", "30.6:79.6", "58.8:78.2", "54.2:86.2")

	-- Celestial Dragon -- 40624
	pet = AddPet(40624, V.CATA, Q.RARE)
	pet:SetItemID(54810)
	pet:SetSpellID(75613)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(5875)

	-- Cenarion Hatchling -- 53623
	pet = AddPet(53623, V.CATA, Q.EPIC)
	pet:SetItemID(70099)
	pet:SetSpellID(99578)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Chi-Chi, Hatchling of Chi-Ji -- 72462
	pet = AddPet(72462, V.MOP, Q.RARE)
	pet:SetItemID(102145)
	pet:SetSpellID(145697)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(73082)

	-- Chi-ji Kite -- 66104
	pet = AddPet(66104, V.MOP, Q.COMMON)
	pet:SetItemID(89368)
	pet:SetSpellID(127008)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.INSCRIPTION)

	-- Chrominius -- 68662
	pet = AddPet(68662, V.MOP, Q.RARE)
	pet:SetItemID(93038)
	pet:SetSpellID(135264)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(14020)

	-- Chuck -- 26056
	pet = AddPet(26056, V.TBC, Q.RARE)
	pet:SetItemID(35350)
	pet:SetSpellID(46426)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.PROFESSION)
	pet:AddProfession(PROF.FISHING)
	pet:AddQuest(11665)

	-- Cinder Kitten -- 68267
	pet = AddPet(68267, V.MOP, Q.RARE)
	pet:SetItemID(92707)
	pet:SetSpellID(134538)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Clock'em -- 68601
	pet = AddPet(68601, V.MOP, Q.COMMON)
	pet:SetItemID(93025)
	pet:SetSpellID(135156)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.REPUTATION, F.BRAWLERS)
	pet:AddRepVendor(FAC.BRAWLGAR_ARENA, REP.REVERED, 68364)
	pet:AddRepVendor(FAC.BIZMOS_BRAWLPUB, REP.REVERED, 68363)

	-- Clockwork Gnome -- 48609
	pet = AddPet(48609, V.CATA, Q.RARE)
	pet:SetItemID(64372)
	pet:SetSpellID(90521)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ARCHAEOLOGY)

	-- Clockwork Rocket Bot -- 24968
	pet = AddPet(24968, V.TBC, Q.RARE)
	pet:SetItemID(34424)
	pet:SetSpellID(54187)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.WORLD_EVENTS)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")
	pet:AddQuest(6983, 7043)

	-- Clouded Hedgehog -- 64242
	pet = AddPet(64242, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "23-25", false, "31.2:82.0", "50.6:70.6", "66.6:70.0", "66.0:24.0")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "23-25", false, "71.6:91.4", "53.0:90.2", "78.0:89.6")

	-- Cobra Hatchling -- 32591
	pet = AddPet(32591, V.WOTLK, Q.COMMON)
	pet:SetItemID(39898)
	pet:SetSpellID(61351)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.REPUTATION, F.THE_ORACLES)
	pet:AddRepVendor(FAC.THE_ORACLES, REP.REVERED, 31910)
	pet:AddCustom("MYSTERIOUS_EGG")

	-- Cockatiel -- 7390
	pet = AddPet(7390, V.ORIG, Q.COMMON)
	pet:SetItemID(8496)
	pet:SetSpellID(10680)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(2663, 44179)

	-- Cogblade Raptor -- 68841
	pet = AddPet(68841, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BLADES_EDGE_MOUNTAINS, "18-20", false, "69.0:71.4", "68.8:66.2", "68.0:58.6", "60.0:65.0", "59.0:68.0", "59.6:73.2")

	-- Coilfang Stalker -- 71019
	pet = AddPet(71019, V.MOP, Q.COMMON)
	pet:SetItemID(97554)
	pet:SetSpellID(141446)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(21212)

	-- Coral Adder -- 62991
	pet = AddPet(62991, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "66.6:86.0", "64.4:86.2", "63.6:82.0", "65.8:81.0")

	-- Coral Snake -- 62190
	pet = AddPet(62190, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.STONETALON_MOUNTAINS, "5-7", false, "73.0:55.2", "64.4:52.4", "62.8:64.8", "48.2:47.6", "39.8:42.6")

	-- Core Hound Pup -- 36871
	pet = AddPet(36871, V.WOTLK, Q.RARE)
	pet:SetItemID(49646)
	pet:SetSpellID(69452)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("AUTH")

	-- Corefire Imp -- 68664
	pet = AddPet(68664, V.MOP, Q.RARE)
	pet:SetItemID(93034)
	pet:SetSpellID(135259)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	pet:AddMobDrop(11982)

	-- Cornish Rex Cat -- 7384
	pet = AddPet(7384, V.ORIG, Q.COMMON)
	pet:SetItemID(8486)
	pet:SetSpellID(10674)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(6367)

	-- Crawling Claw -- 45128
	pet = AddPet(45128, V.CATA, Q.EPIC)
	pet:SetItemID(60847)
	pet:SetSpellID(84263)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	pet:AddProfession(PROF.ARCHAEOLOGY)

	-- Creepy Crate -- 54128
	pet = AddPet(54128, V.CATA, Q.COMMON)
	pet:SetItemID(71076)
	pet:SetSpellID(100684)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.WORLD_EVENTS, F.IBOE)
	pet:AddQuest(29413, 29429)
	pet:AddWorldEvent("HALLOWS_END")

	-- Crimson Geode -- 62927
	pet = AddPet(62927, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DEEPHOLM, "22-23", false, "64.4:42.6", "76.4:52.2", "72.0:26.6", "62.4:16.2", "72.6:41.2")

	-- Crimson Lasher -- 53661
	pet = AddPet(53661, V.CATA, Q.RARE)
	pet:SetItemID(70160)
	pet:SetSpellID(99668)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP)
	pet:AddVendor(53881)

	-- Crimson Snake -- 7567
	pet = AddPet(7567, V.ORIG, Q.COMMON)
	pet:SetItemID(10392)
	pet:SetSpellID(10717)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(8404, 20980)

	-- Crimson Whelpling -- 7544
	pet = AddPet(7544, V.ORIG, Q.COMMON)
	pet:SetItemID(8499)
	pet:SetSpellID(10697)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.IBOE)
	pet:AddWorldDrop(Z.WETLANDS)
	pet:AddCustom("UNCLAIMED_BM")

	-- Crow -- 67443
	pet = AddPet(67443, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET, F.WORLD_EVENTS)
	pet:AddZoneLocations(Z.DARKMOON_ISLAND, "1-10", true)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Crunchy Scorpion -- 63548
	pet = AddPet(63548, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "24-25", false, "53.8:27.0", "51.6:71.6", "30.6:90.6", "59.4:40.0")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24-25", false, "77.0:88.8", "82.2:89.6")

	-- Curious Oracle Hatchling -- 33530
	pet = AddPet(33530, V.WOTLK, Q.RARE)
	pet:SetItemID(46545)
	pet:SetSpellID(65381)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP, F.WORLD_EVENTS)
	pet:AddQuest(13959)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- Curious Wolvar Pup -- 33529
	pet = AddPet(33529, V.WOTLK, Q.RARE)
	pet:SetItemID(46544)
	pet:SetSpellID(65382)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.QUEST, F.IBOP)
	pet:AddQuest(13960)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- Dancing Water Skimmer -- 63847
	pet = AddPet(63847, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALE_OF_ETERNAL_BLOSSOMS, "24-25", false, "41.4:48.6", "36.2:60.6", "75.8:59.6", "72.2:44.0", "63.2:54.0")

	-- Dandelion Frolicker -- 73532
	pet = AddPet(73532, V.MOP, Q.RARE)
	pet:SetItemID(104160)
	pet:SetSpellID(148051)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(71826)

	-- Dark Phoenix Hatchling -- 47944
	pet = AddPet(47944, V.CATA, Q.RARE)
	pet:SetItemID(63138)
	pet:SetSpellID(89039)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(5812, 5892)
	pet:AddRepVendor(FAC.GUILD, REP.EXALTED, 46572, 51496, 51512, 51503, 52268, 46602, 51495, 51504)

	-- Dark Whelpling -- 7543
	pet = AddPet(7543, V.ORIG, Q.COMMON)
	pet:SetItemID(10822)
	pet:SetSpellID(10695)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP)
	pet:AddMobDrop(2725, 4323, 4324, 7049, 42042, 46914, 46916)
	pet:AddCustom("UNCLAIMED_BM")

	-- Darkmoon Balloon -- 55187
	pet = AddPet(55187, V.CATA, Q.RARE)
	pet:SetItemID(73762)
	pet:SetSpellID(103076)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.WORLD_EVENTS)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddVendor(14846, 14828, 55305)

	-- Darkmoon Cub -- 56031
	pet = AddPet(56031, V.CATA, Q.RARE)
	pet:SetItemID(74981)
	pet:SetSpellID(105122)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.WORLD_EVENTS)
	pet:AddVendor(14846)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Darkmoon Eye -- 67332
	pet = AddPet(67332, V.MOP, Q.RARE)
	pet:SetItemID(91040)
	pet:SetSpellID(132789)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.WORLD_EVENTS, F.IBOP)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddQuest(32175)

	-- Darkmoon Glowfly -- 67329
	pet = AddPet(67329, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET, F.WORLD_EVENTS)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddZoneLocations(Z.DARKMOON_ISLAND, "1-10", false, "53.0:23.6", "44.6:59.8", "59.2:65.6", "62.6:77.6")

	-- Darkmoon Hatchling -- 67319
	pet = AddPet(67319, V.MOP, Q.RARE)
	pet:SetItemID(91003)
	pet:SetSpellID(132762)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.WORLD_EVENTS)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddVendor(14846)

	-- Darkmoon Monkey -- 54491
	pet = AddPet(54491, V.CATA, Q.RARE)
	pet:SetItemID(73764)
	pet:SetSpellID(101733)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddVendor(14846)

	-- Darkmoon Rabbit -- 59358
	pet = AddPet(59358, V.MOP, Q.EPIC)
	pet:SetItemID(80008)
	pet:SetSpellID(114090)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP, F.WORLD_EVENTS, F.RAID)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddMobDrop(58336)

	-- Darkmoon Tonk -- 55356
	pet = AddPet(55356, V.CATA, Q.RARE)
	pet:SetItemID(73903)
	pet:SetSpellID(103544)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.WORLD_EVENTS)
	pet:AddVendor(14846)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Darkmoon Turtle -- 54487
	pet = AddPet(54487, V.CATA, Q.RARE)
	pet:SetItemID(73765)
	pet:SetSpellID(103074)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.WORLD_EVENTS)
	pet:AddVendor(14846)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Darkmoon Zeppelin -- 55367
	pet = AddPet(55367, V.CATA, Q.RARE)
	pet:SetItemID(73905)
	pet:SetSpellID(103549)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddVendor(14846)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Darkshore Cub -- 62250
	pet = AddPet(62250, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DARKSHORE, "3-6", "44.6:80.8", "44.6:76.8", "40.6:73.8", "39.2:82.0", "42.6:77.6")

	-- Darting Hatchling -- 35396
	pet = AddPet(35396, V.WOTLK, Q.COMMON)
	pet:SetItemID(48112)
	pet:SetSpellID(67413)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_DROP)
	pet:AddWorldDrop(Z.DUSTWALLOW_MARSH)
	pet:AddCustom("DARTSNEST", "UNCLAIMED_BM")

	-- De-Weaponized Mechanical Companion -- 43916
	pet = AddPet(43916, V.CATA, Q.RARE)
	pet:SetItemID(60216)
	pet:SetSpellID(82173)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Death's Head Cockroach -- 62887
	pet = AddPet(62887, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.MOUNT_HYJAL, "22-23", false, "43.8:20.8", "28.6:36.0", "59.0:80.6", "58.0:68.6")

	-- Death Adder Hatchling -- 73364
	pet = AddPet(73364, V.MOP, Q.RARE)
	pet:SetItemID(104161)
	pet:SetSpellID(148052)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(73163)

	-- Death Talon Whelpguard -- 68663
	pet = AddPet(68663, V.MOP, Q.RARE)
	pet:SetItemID(93037)
	pet:SetSpellID(135265)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(12017)

	-- Deathy -- 51122
	pet = AddPet(51122, V.WOTLK, Q.RARE)
	pet:SetItemID(67418)
	pet:SetSpellID(94070)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("BLIZZCON")

	-- Desert Spider -- 62186
	pet = AddPet(62186, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DESOLACE, "7-9", false, "69.4:43.2", "29.2:79.0", "25.4:70.2", "55.0:33.2", "57.2:15.4")
	pet:AddZoneLocations(Z.SILITHUS, "16-17", false, "29.2:76.4", "32.0:71.8", "40.2:78.0", "34.4:67.0", "47.6:71.4")
	pet:AddZoneLocations(Z.TANARIS, "13-14", false, "54.4:56.8", "48.6:27.2", "52.8:26.0", "51.2:38.2", "48.8:32.8")
	pet:AddZoneLocations(Z.ULDUM, "23-24", false, "30.0:9.0", "25.8:7.0", "64.2:22.6", "53.2:29.6", "70.6:40.0", "80.2:50.6")

	-- Deviate Hatchling -- 35395
	pet = AddPet(35395, V.WOTLK, Q.COMMON)
	pet:SetItemID(48114)
	pet:SetSpellID(67414)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP)
	pet:AddMobDrop(3636, 3637)

	-- Diemetradon Hatchling -- 62375
	pet = AddPet(62375, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.UNGORO_CRATER, "15-16", false, "40.0:81.8", "40.6:50.6", "25.0:61.6", "29.8:73.2")

	-- Direhorn Runt -- 70154
	pet = AddPet(70154, V.MOP, Q.COMMON)
	pet:SetItemID(94573)
	pet:SetSpellID(139153)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP)
	pet:AddMobDrop(69983, 70013)

	-- Disgusting Oozeling -- 15429
	pet = AddPet(15429, V.ORIG, Q.COMMON)
	pet:SetItemID(20769)
	pet:SetSpellID(25162)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP)
	pet:AddCustom("OOZING_BAG")

	-- Dragon Kite -- 25110
	pet = AddPet(25110, V.TBC, Q.EPIC)
	pet:SetItemID(34493)
	pet:SetSpellID(45127)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Dread Hatchling -- 77137  -- Warlords CE pet
	pet = AddPet(77137, V.WOD, Q.RARE)
	pet:SetItemID(109014)
	pet:SetSpellID()
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Droplet of Y'Shaarj -- 73350
	pet = AddPet(73350, V.MOP, Q.RARE)
	pet:SetItemID(104162)
	pet:SetSpellID(148058)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(71734)
	pet:AddCustom("NORMAL", "HEROIC")

	-- Dun Morogh Cub -- 33194
	pet = AddPet(33194, V.WOTLK, Q.RARE)
	pet:SetItemID(44970)
	pet:SetSpellID(62508)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(33310)
	pet:AddCustom("CHAMPION_SEALS", "UNCLAIMED_BM")

	-- Durotar Scorpion -- 33198
	pet = AddPet(33198, V.WOTLK, Q.RARE)
	pet:SetItemID(44973)
	pet:SetSpellID(62513)
	pet:AddFilters(F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(33553)
	pet:AddCustom("CHAMPION_SEALS")

	-- Effervescent Glowfly -- 63850
	pet = AddPet(63850, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "54.0:88.2", "55.8:88.6")
	pet:AddZoneLocations(Z.VALE_OF_ETERNAL_BLOSSOMS, "24-25", false, "85.0:48.4", "69.4:44.8", "27.2:28.4", "25.6:62.0")

	-- Egbert -- 23258
	pet = AddPet(23258, V.TBC, Q.RARE)
	pet:SetItemID(32616)
	pet:SetSpellID(40614)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.WORLD_EVENTS)
	pet:AddQuest(10966, 10967)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- Elder Python -- 69818
	pet = AddPet(69818, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ISLE_OF_THUNDER, "25", false, "57:71", "41.6:64.6", "40.6:57.4", "35.8:52.6")

	-- Electrified Razortooth -- 69794
	pet = AddPet(69794, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ISLE_OF_THUNDER, "22-23", false, "40:58", "48:65")

	-- Elementium Geode -- 50722
	pet = AddPet(50722, V.CATA, Q.RARE)
	pet:SetItemID(67282)
	pet:SetSpellID(93838)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.MINING)

	-- Elwynn Lamb -- 33200
	pet = AddPet(33200, V.WOTLK, Q.RARE)
	pet:SetItemID(44974)
	pet:SetSpellID(62516)
	pet:AddFilters(F.ALLIANCE, F.IBOE, F.VENDOR, F.IBOE)
	pet:AddVendor(33307)
	pet:AddCustom("CHAMPION_SEALS", "UNCLAIMED_BM")

	-- Emerald Proto-Whelp -- 68850
	pet = AddPet(68850, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.SHOLAZAR_BASIN, "21-22", false, "45.6:24.6", "49.6:23.8", "48.6:30.2", "44.4:30.6")

	-- Emerald Shale Hatchling -- 62915
	pet = AddPet(62915, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DEEPHOLM, "22-23", false, "63:24", "49:53", "45:85", "30:47", "36:77")

	-- Emerald Turtle -- 62994
	pet = AddPet(62994, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "42.6:63.4", "51.6:79.2", "59.2:83.0")
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "90.2:20.0")
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "87.2:20.6")

	-- Emerald Whelpling -- 7545
	pet = AddPet(7545, V.ORIG, Q.COMMON)
	pet:SetItemID(8498)
	pet:SetSpellID(10698)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP)
	pet:AddMobDrop(39384)

	-- Emperor Crab -- 65203
	pet = AddPet(65203, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "24-25", false, "25.5:65.6", "41.4:70.2", "44.6:63.0", "30.6:55.8")

	-- Enchanted Broom -- 33227
	pet = AddPet(33227, V.WOTLK, Q.RARE)
	pet:SetItemID(44982)
	pet:SetSpellID(62564)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(33557)
	pet:AddCustom("CHAMPION_SEALS", "UNCLAIMED_BM")

	-- Enchanted Lantern -- 46898
	pet = AddPet(46898, V.CATA, Q.RARE)
	pet:SetItemID(67274)
	pet:SetSpellID(93836)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENCHANTING)

	-- Essence of Competition -- 27346
	pet = AddPet(27346, V.WOTLK, Q.UNCOMMON)
	pet:SetItemID(37298)
	pet:SetSpellID(48408)
	pet:Retire()

	-- Ethereal Soul-Trader -- 27914
	pet = AddPet(27914, V.TBC, Q.RARE)
	pet:SetItemID(38050)
	pet:SetSpellID(49964)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Eternal Strider -- 61088
	pet = AddPet(61088, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALE_OF_ETERNAL_BLOSSOMS, "24-25", false, "87.6:44.8", "69.0:44.6", "72.6:27.6", "43.4:48.6")

	-- Eye of the Legion -- 59020
	pet = AddPet(59020, V.MOP, Q.RARE)
	pet:SetItemID(79744)
	pet:SetSpellID(112994)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Father Winter's Helper -- 15698
	pet = AddPet(15698, V.ORIG, Q.COMMON)
	pet:SetItemID(21301)
	pet:SetSpellID(26533)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.IBOE)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")

	-- Feline Familiar -- 53884
	pet = AddPet(53884, V.CATA, Q.COMMON)
	pet:SetItemID(70908)
	pet:SetSpellID(100330)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.IBOE)
	pet:AddVendor(53728, 53757)
	pet:AddWorldEvent("HALLOWS_END")

	-- Feral Vermling -- 63621
	pet = AddPet(63621, V.MOP, Q.COMMON)
	pet:SetItemID(85578)
	pet:SetSpellID(124152)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.ACHIEVEMENT)
	pet:AddAchievement(7500)

	-- Festival Lantern -- 55574
	pet = AddPet(55574, V.CATA, Q.RARE)
	pet:SetItemID(74611)
	pet:SetSpellID(104049)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(15864)

	-- Fetish Shaman -- 56266
	pet = AddPet(56266, V.CATA, Q.RARE)
	pet:SetItemID(76062)
	pet:SetSpellID(105633)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("DIABLOCE")

	-- Fiendish Imp -- 71033
	pet = AddPet(71033, V.MOP, Q.COMMON)
	pet:SetItemID(97551)
	pet:SetSpellID(141451)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(15688)

	-- Filthling -- 71201
	pet = AddPet(71201, V.MOP, Q.COMMON)
	pet:SetItemID(97961)
	pet:SetSpellID(142030)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP)
	pet:AddMobDrop(69251)

	-- Firefly -- 21076
	pet = AddPet(21076, V.TBC, Q.COMMON)
	pet:SetItemID(29960)
	pet:SetSpellID(36034)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(20197)

	-- Fishy -- 62829
	pet = AddPet(62829, V.MOP, Q.COMMON)
	pet:SetItemID(84105)
	pet:SetSpellID(122748)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(29905, 31239)

	-- Fjord Rat -- 62641
	pet = AddPet(62641, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.HOWLING_FJORD, "20-22", false, "57.0:57.0", "56.6:50.2", "59.8:50.0", "58.8:55.8", "68.2:29.6", "74.6:34.4", "30.4:44.0")

	-- Flamering Moth -- 73543
	pet = AddPet(73543, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TIMELESS_ISLE, "25", false, "55.0:57.6")

	-- Fluxfire Feline -- 68838
	pet = AddPet(68838, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DUN_MOROGH, "1-2", false, "32.6:69.8", "48.0:63.0", "45.6:62.2", "34.2:64.0")
	pet:AddZoneLocations(Z.NEW_TINKERTOWN, "1-2", false, "39.4:65.8", "47.6:57.8", "42.6:33.6", "35.4:40.2")

	-- Fossilized Hatchling -- 45340
	pet = AddPet(45340, V.CATA, Q.RARE)
	pet:SetItemID(60955)
	pet:SetSpellID(84752)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	pet:AddProfession(PROF.ARCHAEOLOGY)

	-- Fox Kit -- 48641
	pet = AddPet(48641, V.CATA, Q.RARE)
	pet:SetItemID(64403)
	pet:SetSpellID(90637)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.VENDOR)
	pet:AddMobDrop(47676)
	pet:AddVendor(47328, 48531)

	-- Frigid Frostling -- 40198
	pet = AddPet(40198, V.WOTLK, Q.RARE)
	pet:SetItemID(53641)
	pet:SetSpellID(74932)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.WORLD_EVENTS)
	pet:AddWorldEvent("MIDSUMMER_FIRE_FESTIVAL")
	pet:AddMobDrop(25740)

	-- Frosty -- 28883
	pet = AddPet(28883, V.WOTLK, Q.RARE)
	pet:SetItemID(39286)
	pet:SetSpellID(52615)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOA, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Fungal Abomination -- 68657
	pet = AddPet(68657, V.MOP, Q.RARE)
	pet:SetItemID(93032)
	pet:SetSpellID(135255)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(16011)

	-- Fungal Moth -- 62916
	pet = AddPet(62916, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DEEPHOLM, "22-23", false, "69.4:69.8", "23.4:44.6", "73.8:27.6", "69.4:24.2", "73.6:62.6")

	-- Garden Moth -- 65215
	pet = AddPet(65215, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "22-24", false, "56.1:54.6", "56:42", "51.4:46.2", "54.4:48.2")

	-- Gahz'rooki -- 71159
	pet = AddPet(71159, V.MOP, Q.COMMON)
	pet:SetItemID(97821)
	pet:SetSpellID(141789)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP)
	pet:AddWorldDrop(Z.NORTHERN_BARRENS)

	-- Ghostly Skull -- 29147
	pet = AddPet(29147, V.WOTLK, Q.RARE)
	pet:SetItemID(39973)
	pet:SetSpellID(53316)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(29537)

	-- Giant Bone Spider -- 68656
	pet = AddPet(68656, V.MOP, Q.RARE)
	pet:SetItemID(93030)
	pet:SetSpellID(135254)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(15952)

	-- Giant Sewer Rat -- 31575
	pet = AddPet(31575, V.WOTLK, Q.RARE)
	pet:SetItemID(43698)
	pet:SetSpellID(59250)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	pet:AddWorldDrop(Z.DALARAN)
	pet:AddProfession(PROF.FISHING)

	-- Gilded Moth -- 63838
	pet = AddPet(63838, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALE_OF_ETERNAL_BLOSSOMS, "24-25", false, "68.4:28.8", "63.0:41.4", "50.8:67.0", "45.2:43.0")

	-- Gilnean Raven -- 63098
	pet = AddPet(63098, V.MOP, Q.COMMON)
	pet:SetSpellID(123214)
	pet:AddFilters(F.ALLIANCE, F.TRAINER)
	pet:AddTrainer(63083, 63596, 63014, 63070)
	--pet:SetRequiredFaction("Alliance")

	-- Gold Mini Jouster -- 42183
	pet = AddPet(42183, V.CATA, Q.COMMON)
	pet:SetItemID(65662)
	pet:SetSpellID(78685)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(25560)

	-- Gold Beetle -- 61438
	pet = AddPet(61438, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BADLANDS, "13-14", false, "52.6:46.0", "42.0:68.6", "15.8:67.0", "45.0:10.6")
	pet:AddZoneLocations(Z.TANARIS, "13-14", false, "46.6:32.8", "37.6:75.0", "40.0:56.8", "60.8:57.4")

	-- Golden Civet -- 63841
	pet = AddPet(63841, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALE_OF_ETERNAL_BLOSSOMS, "24-25", false, "80.2:49.4", "65.0:57.4", "68.6:30.4", "49.6:52.0", "54.0:39.6")

	-- Golden Civet Kitten -- 63842
	pet = AddPet(63842, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALE_OF_ETERNAL_BLOSSOMS, "24-25", false, "52.6:41.0", "78.6:49.2", "65.6:37.0", "68.6:53.8")

	-- Golden Dragonhawk Hatchling -- 21055
	pet = AddPet(21055, V.TBC, Q.COMMON)
	pet:SetItemID(29953)
	pet:SetSpellID(36027)
	pet:AddFilters(F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(16860)

	-- Golden Pig -- 25146
	pet = AddPet(25146, V.WOTLK, Q.RARE)
	pet:SetItemID(34518)
	pet:SetSpellID(45174)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("RECRUIT")

	-- Gooey Sha-ling -- 73351
	pet = AddPet(73351, V.MOP, Q.RARE)
	pet:SetItemID(104163)
	pet:SetSpellID(148059)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(71734)
	pet:AddCustom("LFR", "FLEX")

	-- Grasslands Cottontail -- 61704
	pet = AddPet(61704, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ARATHI_HIGHLANDS, "7-8", false, "50.6:53.6", "38.0:43.6", "66.6:48.2", "18.8:37.8", "47.6:43.6")

	-- Great Horned Owl -- 7553
	pet = AddPet(7553, V.ORIG, Q.COMMON)
	pet:SetItemID(8500)
	pet:SetSpellID(10707)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(8665)

	-- Green Wing Macaw -- 7387
	pet = AddPet(7387, V.ORIG, Q.COMMON)
	pet:SetItemID(8492)
	pet:SetSpellID(10683)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP)
	pet:AddMobDrop(48522)
	pet:AddCustom("UNCLAIMED_BM")

	-- Gregarious Grell -- 54730
	pet = AddPet(54730, V.CATA, Q.RARE)
	pet:SetItemID(72134)
	pet:SetSpellID(102317)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Grey Moth -- 62050
	pet = AddPet(62050, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.AZUREMYST_ISLE, "1-2", false, "15.2:86.2")

	-- Grinder -- 64634
	pet = AddPet(64634, V.MOP, Q.COMMON)
	pet:SetItemID(86564)
	pet:SetSpellID(126251)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(50347)

	-- Grunty -- 34694
	pet = AddPet(34694, V.WOTLK, Q.RARE)
	pet:SetItemID(46802)
	pet:SetSpellID(66030)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("BLIZZCON")

	-- Gryphon Hatchling -- 36908
	pet = AddPet(36908, V.WOTLK, Q.RARE)
	pet:SetItemID(49662)
	pet:SetSpellID(69535)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("PLUSHIE")

	-- Gu'chi Swarmling -- 73730
	pet = AddPet(73730, V.MOP, Q.RARE)
	pet:SetItemID(104291)
	pet:SetSpellID(148527)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(72909)

	-- Guardian Cub -- 53283
	pet = AddPet(53283, V.CATA, Q.RARE)
	pet:SetItemID(69847)
	pet:SetSpellID(98736)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOE)
	pet:Retire()

	-- Guild Herald -- 49587
	pet = AddPet(49587, V.CATA, Q.COMMON)
	pet:SetItemID(65363)
	pet:SetSpellID(92397)
	pet:AddFilters(F.ALLIANCE, F.ACHIEVEMENT, F.VENDOR, F.GUILD, F.IBOP)
	pet:AddAchievement(5201)
	pet:AddRepVendor(FAC.GUILD, REP.REVERED, 51512, 51501, 52268, 46602, 51495, 51504)
	pet:SetRequiredFaction("Alliance")

	-- Guild Herald -- 49590
	pet = AddPet(49590, V.CATA, Q.COMMON)
	pet:SetItemID(65364)
	pet:SetSpellID(92398)
	pet:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.GUILD, F.ACHIEVEMENT)
	pet:AddAchievement(5201)
	pet:AddRepVendor(FAC.GUILD, REP.REVERED, 46572, 51496, 51502, 51512, 51503, 52268)
	pet:SetRequiredFaction("Horde")

	-- Guild Page -- 49586
	pet = AddPet(49586, V.CATA, Q.COMMON)
	pet:SetItemID(65361)
	pet:SetSpellID(92395)
	pet:AddFilters(F.ALLIANCE, F.ACHIEVEMENT, F.VENDOR, F.GUILD, F.IBOP)
	pet:AddAchievement(5031)
	pet:AddRepVendor(FAC.GUILD, REP.HONORED, 51512, 51501, 52268, 46602, 51495, 51504)
	pet:SetRequiredFaction("Alliance")

	-- Guild Page -- 49588
	pet = AddPet(49588, V.CATA, Q.COMMON)
	pet:SetItemID(65362)
	pet:SetSpellID(92396)
	pet:AddFilters(F.HORDE, F.IBOP, F.VENDOR, F.ACHIEVEMENT, F.GUILD)
	pet:AddAchievement(5179)
	pet:AddRepVendor(FAC.GUILD, REP.HONORED, 46572, 51496, 51502, 51512, 51503, 52268)
	pet:SetRequiredFaction("Horde")

	-- Gulp Froglet -- 73359
	pet = AddPet(73359, V.MOP, Q.RARE)
	pet:SetItemID(104169)
	pet:SetSpellID(148067)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(72775)

	-- Gundrak Hatchling -- 35400
	pet = AddPet(35400, V.WOTLK, Q.COMMON)
	pet:SetItemID(48116)
	pet:SetSpellID(67415)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP)
	pet:AddMobDrop(29334)

	-- Gurky -- 16069
	pet = AddPet(16069, V.ORIG, Q.RARE)
	pet:SetItemID(22114)
	pet:SetSpellID(27241)
	pet:Retire()

	-- Gusting Grimoire -- 69208
	pet = AddPet(69208, V.ORIG, Q.RARE)
	pet:SetItemID(93669)
	pet:SetSpellID(136484)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Harbinger of Flame -- 68665
	pet = AddPet(68665, V.MOP, Q.RARE)
	pet:SetItemID(93033)
	pet:SetSpellID(135258)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(12098)

	-- Hare -- 61751
	pet = AddPet(61751, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DUROTAR, "1-2", false, "54.0:18.4", "40.6:17.6", "54.4:33.8")
	pet:AddZoneLocations(Z.ARATHI_HIGHLANDS, "7-8", true)
	pet:AddZoneLocations(Z.THE_HINTERLANDS, "11-12", false, "68.2:26.8", "59.0:49.2", "36.0:64.4", "25.2:53.6")

	-- Harmonious Porcupette -- 73732
	pet = AddPet(73732, V.MOP, Q.RARE)
	pet:SetItemID(104295)
	pet:SetSpellID(148530)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.EMPEROR_SHAOHAO)
	pet:AddRepVendor(FAC.EMPEROR_SHAOHAO, REP.REVERED, 73306)

	-- Harpy Youngling -- 68804
	pet = AddPet(68804, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.NORTHERN_BARRENS, "3-4", false, "31.6:40.8", "32.0:31.6", "26.2:25.4", "24.2:34.6")

	-- Hawk Owl -- 7555
	pet = AddPet(7555, V.ORIG, Q.COMMON)
	pet:SetItemID(8501)
	pet:SetSpellID(10706)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(8665)

	-- Highlands Mouse -- 62905
	pet = AddPet(62905, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.WETLANDS, "6-7", false, "78.6:47.6", "70.4:31.4", "67.6:29.6", "69.2:28.8")
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "47.8:88.6", "28.2:39.8", "35.4:33.2", "49.6:36.6", "22.8:20.6")

	-- Highlands Skunk -- 62907
	pet = AddPet(62907, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "46.6:30.2", "51.0:30.4", "66.4:56.6", "72.2:65.6")

	-- Highlands Turkey -- 62906
	pet = AddPet(62906, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "29.6:31.2", "50.6:35.4", "56.6:31.4", "28.2:19.6", "47.8:24.6")

	-- Hippogryph Hatchling -- 17255
	pet = AddPet(17255, V.TBC, Q.EPIC)
	pet:SetItemID(23713)
	pet:SetSpellID(30156)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG", "UNCLAIMED_BM")

	-- Hopling -- 64632
	pet = AddPet(64632, V.MOP, Q.COMMON)
	pet:SetItemID(86562)
	pet:SetSpellID(126247)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(6402)

	-- Horde Balloon -- 54541
	pet = AddPet(54541, V.CATA, Q.COMMON)
	pet:SetItemID(72045)
	pet:SetSpellID(101989)
	pet:AddFilters(F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(29401)
	pet:SetRequiredFaction("Horde")

	-- Horned Lizard -- 62894
	pet = AddPet(62894, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ULDUM, "23-24", false, "44.8:67.8", "27.6:13.2", "32.6:37.0", "28.6:24.6", "64.6:27.6", "75.2:43.8")

	-- Horny Toad -- 62185
	pet = AddPet(62185, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DESOLACE, "7-9", false, "71.4:47.0", "29.2:79.2", "68.6:26.6", "55.8:11.0", "73.6:35.0")

	-- Huge Toad -- 61368
	pet = AddPet(61368, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.HILLSBRAD_FOOTHILLS, "6-7", false, "61.2:51.4", "65.0:37.4", "61.2:55.6", "54.0:63.8")
	pet:AddZoneLocations(Z.SWAMP_OF_SORROWS, "14-15", false, "49.0:42.6", "52.2:40.8", "57.6:55.8", "63.2:32.4")
	pet:AddZoneLocations(Z.ZULDRAK, "22-23", false, "44.2:60.6", "21.6:62.4", "26.2:58.8", "25.2:74.0", "35.6:82.0")
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "24", false, "57.6:13.0", "21.8:17.8")

	-- Hyacinth Macaw -- 7391
	pet = AddPet(7391, V.ORIG, Q.EPIC)
	pet:SetItemID(8494)
	pet:SetSpellID(10682)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.IBOE)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN)

	-- Hyjal Bear Cub -- 53658
	pet = AddPet(53658, V.CATA, Q.RARE)
	pet:SetItemID(70140)
	pet:SetSpellID(99663)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP)
	pet:AddVendor(53882)

	-- Imperial Silkworm -- 67233
	pet = AddPet(67233, V.MOP, Q.UNCOMMON)
	pet:SetItemID(90902)
	pet:SetSpellID(132580)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.TAILORING)

	-- Imperial Moth -- 67230
	pet = AddPet(67230, V.MOP, Q.RARE)
	pet:SetItemID(90900)
	pet:SetSpellID(132574)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.TAILORING)

	-- Infinite Whelpling -- 68820
	pet = AddPet(68820, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TANARIS, "13-14", false, "67.2:49.0", "64.2:51.4", "63.6:57.0", "61.0:50.2")
	pet:AddCustom("COT")

	-- Jade Crane Chick -- 65314
	pet = AddPet(65314, V.MOP, Q.COMMON)
	pet:SetItemID(88148)
	pet:SetSpellID(127816)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.TRAINER)
	pet:AddTrainer(63596)
	pet:SetRequiredRaces("Pandaren")

	-- Jade Oozeling -- 61718
	pet = AddPet(61718, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_HINTERLANDS, "11", false, "55.6:43.6", "60.0:42.8", "57.4:38.4")

	-- Jade Owl -- 61877
	pet = AddPet(61877, V.MOP, Q.RARE)
	pet:SetItemID(82774)
	pet:SetSpellID(120501)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.JEWELCRAFTING)

	-- Jade Tentacle -- 66450
	pet = AddPet(66450, V.MOP, Q.COMMON)
	pet:SetItemID(89686)
	pet:SetSpellID(130726)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(7521)

	-- Jade Tiger -- 34930
	pet = AddPet(34930, V.WOTLK, Q.RARE)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("RECRUIT")

	-- Jadefire Spirit -- 73738
	pet = AddPet(73738, V.MOP, Q.RARE)
	pet:SetItemID(104307)
	pet:SetSpellID(148552)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(73162)

	-- Jademist Dancer -- 73355
	pet = AddPet(73355, V.MOP, Q.RARE)
	pet:SetItemID(104164)
	pet:SetSpellID(148060)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(72767)

	-- Ji-Kun Hatchling -- 70144
	pet = AddPet(70144, V.MOP, Q.RARE)
	pet:SetItemID(94835)
	pet:SetSpellID(139148)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(69712)

	-- Jubling -- 14878
	pet = AddPet(14878, V.ORIG, Q.COMMON)
	pet:SetItemID(19450)
	pet:SetSpellID(23811)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS, F.QUEST)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddQuest(7946)

	-- Jumping Spider -- 63715
	pet = AddPet(63715, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "63.6:77.6", "54.0:79.6", "47.2:54.0", "32.4:44", "46.8:37.4")

	-- Jungle Darter -- 62997
	pet = AddPet(62997, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "44.6:91.6", "56.2:84.2", "56.8:71.8", "47.2:76.8")

	-- Jungle Grub -- 63304
	pet = AddPet(63304, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", "19.6:45.6", "23.8:47.6", "45.0:42.8", "55.0:32.0", "73.6:20.2", "67.6:33.0")

	-- Kirin Tor Familiar -- 32643
	pet = AddPet(32643, V.WOTLK, Q.RARE)
	pet:SetItemID(44738)
	pet:SetSpellID(61472)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(1956)

	-- Kovok -- 73354
	pet = AddPet(73354, V.MOP, Q.RARE)
	pet:SetItemID(104165)
	pet:SetSpellID(148061)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(71152)

	-- Kuitan Mongoose -- 63953
	pet = AddPet(63953, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24-25", false, "43.0:65.6", "57.0:80.2", "79.6:85.8", "48.6:88.6", "36.0:51.4")

	-- Kun-Lai Runt -- 68846
	pet = AddPet(68846, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "52.2:50.4", "43.2:48.6", "33.4:53.4", "58.4:50.2")

	-- Landro's Lichling -- 52343
	pet = AddPet(52343, V.CATA, Q.RARE)
	pet:SetItemID(68840)
	pet:SetSpellID(96817)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Landro's Lil' XT -- 50468
	pet = AddPet(50468, V.CATA, Q.RARE)
	pet:SetItemID(67128)
	pet:SetSpellID(93624)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Larva -- 62022
	pet = AddPet(62022, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.GHOSTLANDS, "3-6", false, "47.6:53.6", "29.6:47.0", "47.6:35.2", "31.2:61.6")

	-- Lashtail Hatchling -- 52894
	pet = AddPet(52894, V.CATA, Q.COMMON)
	pet:SetItemID(69251)
	pet:SetSpellID(97779)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(29208)

	-- Lava Beetle -- 61386
	pet = AddPet(61386, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BURNING_STEPPES, "15-16", false, "68.8:36.4", "68.4:47.0", "34.6:43.6", "56.0:31.2", "26.8:56.6")

	-- Lava Crab -- 61383
	pet = AddPet(61383, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.SEARING_GORGE, "13", false, "38.8:73.6", "38.6:76.8", "35.6:77.6", "35.6:74.8", "45.8:47.7", "48.6:50.2", "53.6:55.0")
	pet:AddZoneLocations(Z.BURNING_STEPPES, "15", false, "67.6:45.0", "54.8:26.6", "54.6:31.6", "51.6:31.8", "14.6:44.6", "20.6:60.6", "26.8:59.0")

	-- Leaping Hatchling -- 35387
	pet = AddPet(35387, V.WOTLK, Q.COMMON)
	pet:SetItemID(48118)
	pet:SetSpellID(67416)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MISC)
	pet:AddWorldDrop(Z.NORTHERN_BARRENS)
	pet:AddCustom("TAKKSNEST")

	-- Legs -- 53048
	pet = AddPet(53048, V.CATA, Q.COMMON)
	pet:SetItemID(69648)
	pet:SetSpellID(98079)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS)
	pet:AddQuest(10966, 10967)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- Lesser Voidcaller -- 71021
	pet = AddPet(71021, V.MOP, Q.COMMON)
	pet:SetItemID(97556)
	pet:SetSpellID(141448)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(18805)

	-- Lifelike Toad -- 12419
	pet = AddPet(12419, V.ORIG, Q.COMMON)
	pet:SetItemID(15996)
	pet:SetSpellID(19772)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Lil' Bad Wolf -- 71014
	pet = AddPet(71014, V.MOP, Q.COMMON)
	pet:SetItemID(141433)
	pet:SetSpellID(97548)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(17521)

	-- Lil' Bling -- 73011
	pet = AddPet(73011, V.MOP, Q.RARE)
	pet:SetItemID(103670)
	pet:SetSpellID(147124)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MISC)
	pet:AddCustom("BLINGTRON")

	-- Lil' Deathwing -- 46896
	pet = AddPet(46896, V.CATA, Q.RARE)
	pet:SetItemID(62540)
	pet:SetSpellID(87344)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOA, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Lil' K.T. -- 36979
	pet = AddPet(36979, V.WOTLK, Q.RARE)
	pet:SetItemID(49693)
	pet:SetSpellID(69677)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Lil' Ragnaros -- 51600
	pet = AddPet(51600, V.WOTLK, Q.RARE)
	pet:SetItemID(68385)
	pet:SetSpellID(95787)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Lil' Smoky -- 9657
	pet = AddPet(9657, V.ORIG, Q.COMMON)
	pet:SetItemID(11826)
	pet:SetSpellID(15049)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Lil' Tarecgosa -- 54027
	pet = AddPet(54027, V.CATA, Q.COMMON)
	pet:SetItemID(71033)
	pet:SetSpellID(100576)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(5840)
	pet:AddVendor(46572, 51496, 51512, 51503, 52268, 46602, 51495)

	-- Lil' XT -- 40703
	pet = AddPet(40703, V.WOTLK, Q.RARE)
	pet:SetItemID(54847)
	pet:SetSpellID(75906)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Little Fawn -- 32939
	pet = AddPet(32939, V.WOTLK, Q.RARE)
	pet:SetItemID(44841)
	pet:SetSpellID(61991)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(2516)

	-- Living Fluid -- 71199
	pet = AddPet(71199, V.MOP, Q.COMMON)
	pet:SetItemID(97959)
	pet:SetSpellID(142028)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(69017)
	pet:AddCustom("LFR")

	-- Living Sandling -- 69748
	pet = AddPet(69748, V.MOP, Q.RARE)
	pet:SetItemID(94125)
	pet:SetSpellID(137977)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP, F.RAID)
	pet:AddWorldDrop(Z.THRONE_OF_THUNDER)

	-- Locust -- 62893
	pet = AddPet(62893, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ULDUM, "23-24", false, "69.0:75.8", "83.0:82.0", "58.4:49.2", "54.2:50.6")

	-- Lofty Libram -- 68806
	pet = AddPet(68806, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.HILLSBRAD_FOOTHILLS, "6-7", false, "31.6:42.2", "35.2:33.6", "31.0:31.8", "30.4:44.0")

	-- Lucky -- 23198
	pet = AddPet(23198, V.TBC, Q.RARE)
	pet:SetItemID(32498)
	pet:SetSpellID(40405)
	pet:Retire()

	-- Lucky Quilen Cub -- 63832
	pet = AddPet(63832, V.MOP, Q.RARE)
	pet:SetItemID(85871)
	pet:SetSpellID(124660)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Lumpy -- 55215
	pet = AddPet(55215, V.CATA, Q.RARE)
	pet:SetItemID(73797)
	pet:SetSpellID(103125)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.WORLD_EVENTS)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")
	pet:AddQuest(6983, 7043)

	-- Lunar Lantern -- 55571
	pet = AddPet(55571, V.CATA, Q.RARE)
	pet:SetItemID(74610)
	pet:SetSpellID(104047)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddWorldEvent("LUNAR_FESTIVAL")
	pet:AddVendor(15864)

	-- Lurky -- 15358
	pet = AddPet(15358, V.ORIG, Q.RARE)
	pet:SetItemID(30360)
	pet:SetSpellID(24988)
	pet:Retire()

	-- Luyu Moth -- 65124
	pet = AddPet(65124, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "56.0:38.6", "24.2:37.6", "46.0:34.6", "71.4:24.6", "81.8:19.2")

	-- Macabre Marionette -- 34770
	pet = AddPet(34770, V.MOP, Q.RARE)
	pet:SetItemID(46831)
	pet:SetSpellID(66175)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddWorldEvent("DAY_OF_THE_DEAD")

	-- Magic Lamp -- 50545
	pet = AddPet(50545, V.CATA, Q.RARE)
	pet:SetItemID(67275)
	pet:SetSpellID(93837)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENCHANTING)

	-- Magical Crawdad -- 18839
	pet = AddPet(18839, V.TBC, Q.RARE)
	pet:SetItemID(27445)
	pet:SetSpellID(33050)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	pet:AddProfession(PROF.FISHING)
	pet:AddWorldDrop(Z.TEROKKAR_FOREST)

	-- Malayan Quillrat -- 63094
	pet = AddPet(63094, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_VEILED_STAIR, "23-24", false, "67.0:75.6", "61.8:88.2", "57.8:73.8", "54.0:47.0")
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "26.2:59.4", "25.2:4.6", "55.8:44.0", "61.6:45.4")
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "29.6:61.2", "23.0:54.2")
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "74.6:12.2", "84.6:23.6", "79.2:7.2", "74.6:5.6")

	-- Malayan Quillrat Pup -- 63095
	pet = AddPet(63095, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "61.6:45.6", "67.6:50.6", "77.6:30.8", "49.6:49.6", "50.6:39.8", "31.4:63.8")
	pet:AddZoneLocations(Z.THE_VEILED_STAIR, "23-24", false, "57.6:78.6")

	-- Mana Wyrmling -- 20408
	pet = AddPet(20408, V.TBC, Q.COMMON)
	pet:SetItemID(29363)
	pet:SetSpellID(35156)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(20980)

	-- Marsh Fiddler -- 63096
	pet = AddPet(63096, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "35.8:45.6", "46.6:37.8", "55.0:52.4", "62.8:50.8", "38.0:52.2")

	-- Masked Tanuki -- 63003
	pet = AddPet(63003, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "29.6:26.6", "29.6:39.4", "54.4:48.4", "50.0:53.6", "41.8:72.8", "46.2:77.4")

	-- Masked Tanuki Pup -- 63716
	pet = AddPet(63716, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "55.0:34.0", "36.0:20.8", "46.8:23.6", "45.0:78.6", "50.6:70.8")

	-- Mechanical Chicken -- 8376
	pet = AddPet(8376, V.ORIG, Q.COMMON)
	pet:SetItemID(10398)
	pet:SetSpellID(12243)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOE)
	pet:AddQuest(3721)

	-- Mechanical Pandaren Dragonling -- 64899
	pet = AddPet(64899, V.MOP, Q.UNCOMMON)
	pet:SetItemID(87526)
	pet:SetSpellID(126885)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Mechanical Squirrel -- 2671
	pet = AddPet(2671, V.ORIG, Q.COMMON)
	pet:SetItemID(4401)
	pet:SetSpellID(4055)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Mechanopeep -- 33274
	pet = AddPet(33274, V.WOTLK, Q.RARE)
	pet:SetItemID(45002)
	pet:SetSpellID(62674)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(33650)
	pet:AddCustom("CHAMPION_SEALS")

	-- Mei Li Sparkler -- 65185
	pet = AddPet(65185, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "44.6:54.2", "35.6:54.2", "33.6:42.2", "19.0:44.0")

	-- Menagerie Custodian -- 71015
	pet = AddPet(71015, V.MOP, Q.COMMON)
	pet:SetItemID(97549)
	pet:SetSpellID(141434)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(15691)

	-- Mini Diablo -- 11326
	pet = AddPet(11326, V.ORIG, Q.RARE)
	pet:SetItemID(13584)
	pet:SetSpellID(17708)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Mini Mindslayer -- 68658
	pet = AddPet(68658, V.MOP, Q.RARE)
	pet:SetItemID(93041)
	pet:SetSpellID(135268)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(15263)

	-- Mini Thor -- 42078
	pet = AddPet(42078, V.WOTLK, Q.RARE)
	pet:SetItemID(56806)
	pet:SetSpellID(78381)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOA, F.COLLECTORS_EDITION)
	pet:AddCustom("STARCRAFTCE")

	-- Mini Tyrael -- 29089
	pet = AddPet(29089, V.TBC, Q.RARE)
	pet:SetItemID(39656)
	pet:SetSpellID(53082)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("WWI")

	-- Miniwing -- 22445
	pet = AddPet(22445, V.TBC, Q.COMMON)
	pet:SetItemID(31760)
	pet:SetSpellID(39181)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(10898)

	-- Mirror Strider -- 62998
	pet = AddPet(62998, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "31.8:46.4", "40.6:54.8", "56.0:78.8", "63.0:87.6", "57.0:85.6")

	-- Mojo -- 24480
	pet = AddPet(24480, V.TBC, Q.RARE)
	pet:SetItemID(33993)
	pet:SetSpellID(43918)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP)
	pet:AddWorldDrop(Z.ZULAMAN)

	-- Mongoose Pup -- 63954
	pet = AddPet(63954, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24-25", false, "61.0:75.6", "78.6:64.2", "37.8:55.6", "42.4:89.6", "35.2:61.6")

	-- Moon Moon -- 72160
	pet = AddPet(72160, V.MOP, Q.RARE)
	pet:SetItemID(101570)
	pet:SetSpellID(144761)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.MOB_DROP, F.WORLD_EVENTS, F.RAID)
	pet:AddWorldEvent("DARKMOON_FAIRE")
	pet:AddMobDrop(71992)

	-- Moonkin Hatchling -- 51601
	pet = AddPet(51601, V.MOP, Q.RARE)
	pet:SetItemID(68618)
	pet:SetSpellID(95786)
	pet:AddFilters(F.ALLIANCE, F.STORE, F.IBOP)
	pet:AddCustom("WEBSTORE")
	pet:SetRequiredFaction("Alliance")

	-- Moonkin Hatchling -- 51649
	pet = AddPet(51649, V.MOP, Q.RARE)
	pet:SetItemID(68619)
	pet:SetSpellID(95909)
	pet:AddFilters(F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("WEBSTORE")
	pet:SetRequiredFaction("Horde")

	-- Mountain Panda -- 69892
	pet = AddPet(69892, V.MOP, Q.RARE)
	pet:SetItemID(94210)
	pet:SetSpellID(138382)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32604)

	-- Mr. Bigglesworth -- 68655
	pet = AddPet(68655, V.MOP, Q.RARE)
	pet:SetItemID(93031)
	pet:SetSpellID(135256)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT, F.IBOP)
	pet:AddAchievement(7934)

	-- Mr. Chilly -- 29726
	pet = AddPet(29726, V.WOTLK, Q.RARE)
	pet:SetItemID(41133)
	pet:SetSpellID(55068)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("BATTLEMERGE")

	-- Mr. Grubbs -- 50586
	pet = AddPet(50586, V.CATA, Q.COMMON)
	pet:SetItemID(66076)
	pet:SetSpellID(93739)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP)
	pet:AddCustom("FIONA'S_CHARM")
	pet:AddWorldDrop(Z.EASTERN_PLAGUELANDS)

	-- Mr. Wiggles -- 16548
	pet = AddPet(16548, V.ORIG, Q.COMMON)
	pet:SetItemID(23007)
	pet:SetSpellID(28739)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.WORLD_EVENTS, F.IBOP)
	pet:AddQuest(171, 5502)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- Muckbreath -- 24389
	pet = AddPet(24389, V.TBC, Q.RARE)
	pet:SetItemID(33818)
	pet:SetSpellID(43698)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.QUEST, F.IBOP)
	pet:AddQuest(11665)
	pet:AddProfession(PROF.FISHING)

	-- Mulgore Hatchling -- 33219
	pet = AddPet(33219, V.WOTLK, Q.RARE)
	pet:SetItemID(44980)
	pet:SetSpellID(62542)
	pet:AddFilters(F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(33556)
	pet:AddCustom("CHAMPION_SEALS")

	-- Murkablo -- 54438
	pet = AddPet(54438, V.CATA, Q.RARE)
	pet:SetItemID(71726)
	pet:SetSpellID(101606)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("BLIZZCON")

	-- Murkalot -- 74405
	pet = AddPet(74405, V.MOP, Q.RARE)
	pet:SetItemID(106244)
	pet:SetSpellID(149792)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("BLIZZCON")

	-- Murki -- 15361
	pet = AddPet(15361, V.ORIG, Q.RARE)
	pet:Retire()

	-- Murkimus the Gladiator -- 33578
	pet = AddPet(33578, V.WOTLK, Q.RARE)
	pet:SetItemID(46892)
	pet:SetSpellID(63318)
	pet:Retire()

	-- Murky -- 15186
	pet = AddPet(15186, V.ORIG, Q.RARE)
	pet:SetItemID(20371)
	pet:SetSpellID(24696)
	pet:Retire()

	-- Nether Ray Fry -- 28470
	pet = AddPet(28470, V.TBC, Q.RARE)
	pet:SetItemID(38628)
	pet:SetSpellID(51716)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.REPUTATION, F.SHATARI_SKYGUARD)
	pet:AddRepVendor(FAC.SHATARI_SKYGUARD, REP.EXALTED, 23367)

	-- Nether Roach -- 62625
	pet = AddPet(62625, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.NETHERSTORM, "20", false, "29.0:78.4", "58.0:80.8", "57.0:48.2", "67.6:45.6", "40.6:29.4", "35.6:39.8", "28.6:44.0")

	-- Netherspace Abyssal -- 71016
	pet = AddPet(71016, V.MOP, Q.COMMON)
	pet:SetItemID(97550)
	pet:SetSpellID(141435)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(15690)

	-- Netherwhelp -- 18381
	pet = AddPet(18381, V.TBC, Q.RARE)
	pet:SetItemID(25535)
	pet:SetSpellID(32298)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Nexus Whelpling -- 68845
	pet = AddPet(68845, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BOREAN_TUNDRA, "20-22", false, "33.2:31.4", "25.6:34.2", "23.0:26.2", "26.6:19.2")

	-- Nightsaber Cub -- 52344
	pet = AddPet(52344, V.CATA, Q.COMMON)
	pet:SetItemID(68841)
	pet:SetSpellID(96819)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Nordrassil Wisp -- 62888
	pet = AddPet(62888, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.MOUNT_HYJAL, "22-23", false, "58.6:31.2", "63.8:27.4", "63.0:20.6", "58.2:22.6", "61.6:23.2")

	-- Nuts -- 54227
	pet = AddPet(54227, V.CATA, Q.RARE)
	pet:SetItemID(71140)
	pet:SetSpellID(100970)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(5876)

	-- Obsidian Hatchling -- 35399
	pet = AddPet(35399, V.WOTLK, Q.COMMON)
	pet:SetItemID(48120)
	pet:SetSpellID(67417)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(28951)

	-- Ominous Flame -- 73357
	pet = AddPet(73357, V.MOP, Q.RARE)
	pet:SetItemID(104166)
	pet:SetSpellID(148062)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(73162)

	-- Onyx Panther -- 35468
	pet = AddPet(35468, V.WOTLK, Q.RARE)
	pet:Retire()

	-- Onyxian Whelpling -- 36607
	pet = AddPet(36607, V.WOTLK, Q.COMMON)
	pet:SetItemID(49362)
	pet:SetSpellID(49362)
	pet:Retire()

	-- Orange Tabby Cat -- 7382
	pet = AddPet(7382, V.ORIG, Q.COMMON)
	pet:SetItemID(8487)
	pet:SetSpellID(10676)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(6367)

	-- Panda Cub -- 11325
	pet = AddPet(11325, V.ORIG, Q.RARE)
	pet:SetItemID(13583)
	pet:SetSpellID(17707)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Pandaren Air Spirit -- 68467
	pet = AddPet(68467, V.MOP, Q.RARE)
	pet:SetItemID(92799)
	pet:SetSpellID(134894)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32428, 32440)
	pet:AddCustom("PETTAMER")

	-- Pandaren Earth Spirit -- 68468
	pet = AddPet(68468, V.MOP, Q.RARE)
	pet:SetItemID(92800)
	pet:SetSpellID(134895)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32428, 32441)
	pet:AddCustom("PETTAMER")

	-- Pandaren Fire Spirit -- 68466
	pet = AddPet(68466, V.MOP, Q.RARE)
	pet:SetItemID(92798)
	pet:SetSpellID(134892)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32428, 32434)
	pet:AddCustom("PETTAMER")

	-- Pandaren Monk -- 36911
	pet = AddPet(36911, V.WOTLK, Q.RARE)
	pet:SetItemID(49665)
	pet:SetSpellID(69541)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Pandaren Water Spirit -- 66950
	pet = AddPet(66950, V.MOP, Q.RARE)
	pet:SetItemID(90173)
	pet:SetSpellID(131590)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32428, 32439)
	pet:AddCustom("PETTAMER")

	-- Panther Cub -- 52226
	pet = AddPet(52226, V.CATA, Q.COMMON)
	pet:SetItemID(68833)
	pet:SetSpellID(96571)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(29267, 29268)

	-- Peanut -- 23266
	pet = AddPet(23266, V.TBC, Q.RARE)
	pet:SetItemID(32622)
	pet:SetSpellID(40634)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.WORLD_EVENTS)
	pet:AddWorldEvent("CHILDRENS_WEEK")
	pet:AddQuest(10966, 10967)

	-- Pebble -- 45247
	pet = AddPet(45247, V.CATA, Q.COMMON)
	pet:SetItemID(60869)
	pet:SetSpellID(84492)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(5449)

	-- Peddlefeet -- 16085
	pet = AddPet(16085, V.ORIG, Q.COMMON)
	pet:SetItemID(22235)
	pet:SetSpellID(27570)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddWorldEvent("LOVE_IS_IN_THE_AIR")
	pet:AddCustom("CITY")

	-- Pengu -- 32595
	pet = AddPet(32595, V.WOTLK, Q.RARE)
	pet:SetItemID(44723)
	pet:SetSpellID(61357)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.REPUTATION, F.THE_KALUAK)
	pet:AddRepVendor(FAC.THE_KALUAK, REP.EXALTED, 32763, 31916)

	-- Perky Pug -- 37865
	pet = AddPet(37865, V.WOTLK, Q.RARE)
	pet:SetItemID(49912)
	pet:SetSpellID(70613)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(4478)

	-- Personal World Destroyer -- 43800
	pet = AddPet(43800, V.CATA, Q.RARE)
	pet:SetItemID(59597)
	pet:SetSpellID(84412)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Pet Bombling -- 9656
	pet = AddPet(9656, V.ORIG, Q.COMMON)
	pet:SetItemID(11825)
	pet:SetSpellID(15048)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Phoenix Hatchling -- 26119
	pet = AddPet(26119, V.TBC, Q.RARE)
	pet:SetItemID(35504)
	pet:SetSpellID(46599)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(24664)
	pet:AddCustom("UNCLAIMED_BM")

	-- Phoenix Hawk Hatchling -- 71022
	pet = AddPet(71022, V.MOP, Q.COMMON)
	pet:SetItemID(97557)
	pet:SetSpellID(141449)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(19514)

	-- Pierre -- 70082
	pet = AddPet(70082, V.MOP, Q.RARE)
	pet:SetItemID(94903)
	pet:SetSpellID(138824)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)

	-- Pint-Sized Pink Pachyderm -- 24753
	pet = AddPet(24753, V.TBC, Q.RARE)
	pet:SetItemID(46707)
	pet:SetSpellID(44369)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddVendor(23710, 24495, 27478, 27489)
	pet:AddWorldEvent("BREWFEST")

	-- Plump Turkey -- 32818
	pet = AddPet(32818, V.WOTLK, Q.RARE)
	pet:SetItemID(44810)
	pet:SetSpellID(61773)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(3478)
	pet:AddWorldEvent("PILGRIMS_BOUNTY")

	-- Pocket Reaver -- 71020
	pet = AddPet(71020, V.MOP, Q.COMMON)
	pet:SetItemID(97555)
	pet:SetSpellID(141447)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(19516)

	-- Poley -- 16456
	pet = AddPet(16456, V.ORIG, Q.COMMON)
	pet:SetItemID(22781)
	pet:SetSpellID(28505)
	pet:Retire()

	-- Porcupette -- 61086
	pet = AddPet(61086, V.MOP, Q.COMMON)
	pet:SetItemID(89587)
	pet:SetSpellID(118414)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.QUEST)
	pet:AddCustom("PETBATTLEDAILY")

	-- Proto-Drake Whelp -- 32592
	pet = AddPet(32592, V.WOTLK, Q.COMMON)
	pet:SetItemID(44721)
	pet:SetSpellID(61350)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.REPUTATION, F.THE_ORACLES)
	pet:AddRepVendor(FAC.THE_ORACLES, REP.REVERED, 31910)
	pet:AddCustom("MYSTERIOUS_EGG")

	-- Pterrordax Hatchling -- 53225
	pet = AddPet(53225, V.CATA, Q.RARE)
	pet:SetItemID(69821)
	pet:SetSpellID(98571)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ARCHAEOLOGY)

	-- Purple Puffer -- 54383
	pet = AddPet(54383, V.CATA, Q.RARE)
	pet:SetItemID(71624)
	pet:SetSpellID(101493)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Pygmy Direhorn -- 70083
	pet = AddPet(70083, V.MOP, Q.COMMON)
	pet:SetItemID(94574)
	pet:SetSpellID(138825)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(68476)

	-- Rabbit -- 61080
	pet = AddPet(61080, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.STORMWIND_CITY, "1", false, "79.8:64.2")
	pet:AddZoneLocations(Z.AZUREMYST_ISLE, "1-2", false, "30.8:56.2")
	pet:AddZoneLocations(Z.DUN_MOROGH, "1-2", false, "44.6:51.0")
	pet:AddZoneLocations(Z.ELWYNN_FOREST, "1-2", false, "34.0:51.8")
	pet:AddZoneLocations(Z.EVERSONG_WOODS, "1-2", false, "43.6:41.4")
	pet:AddZoneLocations(Z.MULGORE, "1-2", false, "59.0:58.0")
	pet:AddZoneLocations(Z.TELDRASSIL, "1-2", true)
	pet:AddZoneLocations(Z.TIRISFAL_GLADES, "1-2", false, "69.6:55.0")
	pet:AddZoneLocations(Z.SILVERMOON_CITY, "1-3", false, "60.2:72.2")
	pet:AddZoneLocations(Z.WESTFALL, "3-4", false, "53.6:47.4")
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "33.2:70.2")
	pet:AddZoneLocations(Z.DARKSHORE, "3-6", false, "49.0:27.8")
	pet:AddZoneLocations(Z.SILVERPINE_FOREST, "3-6", true)
	pet:AddZoneLocations(Z.ASHENVALE, "4-6", true)
	pet:AddZoneLocations(Z.REDRIDGE_MOUNTAINS, "4-6", false, "32.4:56.4")
	pet:AddZoneLocations(Z.DUSKWOOD, "5-7", false, "9.0:30.0")
	pet:AddZoneLocations(Z.STONETALON_MOUNTAINS, "5-7", false, "65.4:85.2")
	pet:AddZoneLocations(Z.HILLSBRAD_FOOTHILLS, "5-7", false, "54.8:60.4")
	pet:AddZoneLocations(Z.WETLANDS, "6-7", true)
	pet:AddZoneLocations(Z.WESTERN_PLAGUELANDS, "10-11", false, "36.2:66.8")
	pet:AddZoneLocations(Z.FERALAS, "11-12", false, "59.4:51.6")
	pet:AddZoneLocations(Z.MOONGLADE, "15-16", false, "66.2:62.8")
	pet:AddZoneLocations(Z.NAGRAND, "18-19", false, "59.2:72.0")
	pet:AddZoneLocations(Z.BLADES_EDGE_MOUNTAINS, "18-20", false, "41.0:64.8")
	pet:AddZoneLocations(Z.HOWLING_FJORD, "20-22", false, "63.4:36.6")
	pet:AddZoneLocations(Z.CRYSTALSONG_FOREST, "22-23", false, "23.4:32.0")
	pet:AddZoneLocations(Z.MOUNT_HYJAL, "22-23", true)

	-- Rabid Nut Varmint 5000 -- 62120
	pet = AddPet(62120, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "29.0:57.8", "29.0:66.4", "42.0:76.8")
	pet:AddZoneLocations(Z.STONETALON_MOUNTAINS, "5-7", false, "70.4:59.2", "64.8:60.6", "66.0:59.0", "61.0:57.2", "63.6:53.6")
	pet:AddZoneLocations(Z.WINTERSPRING, "17-18", false, "59.4:51.6", "60.6:51.0", "60.6:48.4", "59.2:48.2")

	-- Rapana Whelk -- 64352
	pet = AddPet(64352, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "24-25", false, "54.6:73.8", "30.0:65.2", "40.2:71.6", "47.0:61.6", "29.6:54.6")

	-- Rascal-Bot -- 71693
	pet = AddPet(71693, V.MOP, Q.COMMON)
	pet:SetItemID(100905)
	pet:SetSpellID(143703)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)

	-- Rat Snake -- 61258
	pet = AddPet(61258, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DUSKWOOD, "5-7", false, "59.6:60.6", "63.0:80.2", "34.6:72.2", "72.8:68.0", "86.8:50.2")

	-- Rattlesnake -- 61439
	pet = AddPet(61439, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BADLANDS, "13-14", false, "14.2:57.2", "39.2:56.4", "10.8:48.8", "19.0:56.2")
	pet:AddZoneLocations(Z.TANARIS, "13-14", false, "50.4:30.6", "51.2:51.6", "39.4:71.2", "40.0:57.0")
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "67.6:44.8", "56.0:37.8", "42.4:56.0", "51.2:64.6")

	-- Ravasaur Hatchling -- 35397
	pet = AddPet(35397, V.WOTLK, Q.COMMON)
	pet:SetItemID(48122)
	pet:SetSpellID(67418)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_DROP)
	pet:AddWorldDrop(Z.UNGORO_CRATER)
	pet:AddCustom("MATRIARCHNEST")

	-- Razormaw Hatchling -- 35398
	pet = AddPet(35398, V.WOTLK, Q.COMMON)
	pet:SetItemID(48124)
	pet:SetSpellID(67419)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_DROP)
	pet:AddWorldDrop(Z.WETLANDS)
	pet:AddCustom("RAZORMAWNEST")

	-- Razzashi Hatchling -- 35394
	pet = AddPet(35394, V.WOTLK, Q.COMMON)
	pet:SetItemID(48126)
	pet:SetSpellID(67420)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.IBOE)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN)

	-- Red Cricket -- 63370
	pet = AddPet(63370, V.MOP, Q.COMMON)
	pet:SetItemID(85222)
	pet:SetSpellID(123784)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.REPUTATION)
	pet:AddCustom("SHOEXALTED")

	-- Red Dragonhawk Hatchling -- 21064
	pet = AddPet(21064, V.TBC, Q.COMMON)
	pet:SetItemID(29956)
	pet:SetSpellID(36028)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(16860)

	-- Resilient Roach -- 64238
	pet = AddPet(64238, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "24-25", false, "46.2:43.0", "59.0:73.6", "44.8:27.2", "38.6:45.6", "32.0:46.6")

	-- Red Moth -- 21009
	pet = AddPet(21009, V.TBC, Q.COMMON)
	pet:SetItemID(29902)
	pet:SetSpellID(35909)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(20980)

	-- Red Panda -- 69649
	pet = AddPet(69649, V.MOP, Q.RARE)
	pet:SetItemID(94025)
	pet:SetSpellID(137568)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32603)

	-- Robo-Chick -- 62119
	pet = AddPet(62119, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ORGRIMMAR, "1", false, "38.0:85.8", "40.0:80.6", "39.6:83.0")
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "66.8:21.2", "26.0:50.4", "29.2:65.2", "41.4:76.8")
	pet:AddZoneLocations(Z.WINTERSPRING, "17-18", false, "61.2:51.2", "59.4:48.0", "59.6:50.0")

	-- Rocket Chicken -- 25109
	pet = AddPet(25109, V.TBC, Q.RARE)
	pet:SetItemID(34492)
	pet:SetSpellID(45125)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG", "UNCLAIMED_BM")

	-- Rotten Little Helper -- 73741
	pet = AddPet(73741, V.MOP, Q.RARE)
	pet:SetItemID(104317)
	pet:SetSpellID(148567)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.WORLD_EVENTS)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")
	pet:AddQuest(6983, 7043)

	-- Ruby Droplet -- 73356
	pet = AddPet(73356, V.MOP, Q.RARE)
	pet:SetItemID(104159)
	pet:SetSpellID(148050)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(73282)

	-- Rustberg Gull -- 48107
	pet = AddPet(48107, V.CATA, Q.RARE)
	pet:SetItemID(63355)
	pet:SetSpellID(89472)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.REPUTATION, F.IBOP, F.CATACOMMON2)
	pet:AddRepVendor(FAC.BARADINS_WARDENS, REP.HONORED, 47328)
	pet:AddRepVendor(FAC.HELLSCREAMS_REACH, REP.HONORED, 48531)

	-- Rusty Snail -- 62313
	pet = AddPet(62313, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ASHENVALE, "4-6", false, "5.6:12.6", "15.2:15.2", "15.6:28.6", "6.2:28.6")

	-- Sand Scarab -- 54745
	pet = AddPet(54745, V.CATA, Q.RARE)
	pet:SetItemID(72153)
	pet:SetSpellID(102353)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Sapphire Cub -- 61883
	pet = AddPet(61883, V.MOP, Q.RARE)
	pet:SetItemID(82775)
	pet:SetSpellID(120507)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.JEWELCRAFTING)

	-- Savory Beetle -- 63291
	pet = AddPet(63291, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "31.6:75.2", "42.2:89.6", "76.8:18.6", "58.6:38.6", "47.8:48.4")
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "21.6:86.2", "20.0:85.6", "14.0:83.4", "12.6:85.6")

	-- Scooter the Snail -- 51635
	pet = AddPet(51635, V.CATA, Q.COMMON)
	pet:SetItemID(66073)
	pet:SetSpellID(93817)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP, F.WORLD_EVENTS)
	pet:AddWorldEvent("CHILDRENS_WEEK")
	pet:AddQuest(171, 5502)

	-- Sea Gull -- 62953
	pet = AddPet(62953, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DARKMOON_ISLAND, "1-10", true)
	pet:AddZoneLocations(Z.TANARIS, "13-14", false, "69.8:55.2")
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", true)

	-- Sea Pony -- 55386
	pet = AddPet(55386, V.CATA, Q.RARE)
	pet:SetItemID(73953)
	pet:SetSpellID(103588)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP, F.PROFESSION, F.WORLD_EVENTS)
	pet:AddProfession(PROF.FISHING)
	pet:AddWorldDrop(Z.DARKMOON_ISLAND)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Searing Scorchling -- 25706
	pet = AddPet(25706, V.TBC, Q.RARE)
	pet:SetItemID(34955)
	pet:SetSpellID(45890)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOP)
	pet:AddVendor(52822)

	-- Senegal -- 7389
	pet = AddPet(7389, V.ORIG, Q.COMMON)
	pet:SetItemID(8495)
	pet:SetSpellID(10684)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(2663, 20980, 44179)

	-- Sen'jin Fetish -- 33810
	pet = AddPet(33810, V.WOTLK, Q.RARE)
	pet:SetItemID(45606)
	pet:SetSpellID(63712)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(33554)
	pet:AddCustom("CHAMPION_SEALS", "UNCLAIMED_BM")

	-- Shimmering Wyrmling -- 34724
	pet = AddPet(34724, V.WOTLK, Q.COMMON)
	pet:SetSpellID(66096)
	pet:SetItemID(69992)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.REPUTATION, F.VENDOR, F.WRATHCOMMON2)
	pet:AddRepVendor(FAC.THE_SILVER_COVENANT, REP.EXALTED, 34881)
	pet:AddRepVendor(FAC.THE_SUNREAVERS, REP.EXALTED, 34772)
	pet:AddCustom("CHAMPION_SEALS", "UNCLAIMED_BM")

	-- Shore Crawler -- 63097
	pet = AddPet(63097, V.MOP, Q.COMMON)
	pet:AddFilters(F.HORDE, F.TRAINER, F.IBOP)
	pet:AddTrainer(63086, 63626)

	-- Shrine Fly -- 65216
	pet = AddPet(65216, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "36.6:58.6", "33.4:50.6")

	-- Shy Bandicoon -- 64246
	pet = AddPet(64246, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "26.8:53.8", "21.6:45.4", "18.6:50.0", "17.8:34.8")

	-- Siamese Cat -- 7380
	pet = AddPet(7380, V.ORIG, Q.COMMON)
	pet:SetItemID(8490)
	pet:SetSpellID(10677)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(20980)
	pet:AddCustom("UNCLAIMED_BM")

	-- Sifang Otter -- 63057
	pet = AddPet(63057, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "71.0:54.6", "61.6:72.6", "55.2:35.4", "45.6:71.8")

	-- Sifang Otter Pup -- 63358
	pet = AddPet(63358, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "38.8:87.4", "38.2:92.6", "39.8:98.0")

	-- Silent Hedgehog -- 64804
	pet = AddPet(64804, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DREAD_WASTES, "24-25", false, "48.6:12.2", "43.6:21.2", "27.8:50.6", "63.8:69.6", "58.6:70.6", "33.0:50.6")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24-25", false, "52.6:93.6", "53.8:86.6", "71.6:91.2", "77.6:90.2")

	-- Silky Moth -- 62373
	pet = AddPet(62373, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.MOONGLADE, "15-16", false, "63.2:44.4", "62.2:66.6")
	pet:AddZoneLocations(Z.UNGORO_CRATER, "15-16", false, "43.8:80.0")
	pet:AddZoneLocations(Z.MOUNT_HYJAL, "22-23", false, "49.2:23.6", "40.2:26.6", "64.0:22.8", "60.6:29.6")

	-- Silver Dragonhawk Hatchling -- 21063
	pet = AddPet(21063, V.TBC, Q.COMMON)
	pet:SetItemID(29957)
	pet:SetSpellID(36029)
	pet:AddFilters(F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(16860)

	-- Silver Pig -- 25147
	pet = AddPet(25147, V.MOP, Q.COMMON)
	pet:SetItemID(34519)
	pet:SetSpellID(45175)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("RECRUIT")

	-- Silver Tabby Cat -- 7381
	pet = AddPet(7381, V.ORIG, Q.COMMON)
	pet:SetItemID(8488)
	pet:SetSpellID(10678)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(6367)

	-- Singing Cricket -- 64232
	pet = AddPet(64232, V.MOP, Q.COMMON)
	pet:SetItemID(88147)
	pet:SetSpellID(127815)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(6582)

	-- Singing Sunflower -- 51090
	pet = AddPet(51090, V.CATA, Q.COMMON)
	pet:SetItemID(66067)
	pet:SetSpellID(93823)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(28748)

	-- Sinister Squashling -- 23909
	pet = AddPet(23909, V.TBC, Q.RARE)
	pet:SetItemID(33154)
	pet:SetSpellID(42609)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddWorldEvent("HALLOWS_END")
	pet:AddVendor(53728, 53757)

	-- Skittering Cavern Crawler -- 62638
	pet = AddPet(62638, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.BLADES_EDGE_MOUNTAINS, "18-20", false, "52.2:92.2", "52.6:88.8", "51.0:82.2", "36.8:84.0", "34.6:88.2")

	-- Skunk -- 61255
	pet = AddPet(61255, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.AZUREMYST_ISLE, "1-2", false, "32.2:71.2", "35.0:73.0", "50.0:22.8", "38.8:32.0")
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "42.2:67.6", "27.2:46.8", "26.6:63.8", "31.2:77.6", "39.6:77.0", "52.6:75.2")
	pet:AddZoneLocations(Z.BLOODMYST_ISLE, "3-6", false, "60.8:86.8", "39.6:50.8", "25.6:60.4")
	pet:AddZoneLocations(Z.DUSKWOOD, "5-7", false, "65.6:35.8", "64.6:45.8", "70.8:23.6", "32.6:34.0", "13.0:44.4")
	pet:AddZoneLocations(Z.TEROKKAR_FOREST, "18-19", false, "54.8:37.4", "68.4:38.6", "47.6:22.6", "40.4:31.8")
	pet:AddZoneLocations(Z.HOWLING_FJORD, "20-22", false, "50.6:41.6", "43.8:45.2", "41.2:28.4", "72.2:27.0", "72.8:45.6")

	-- Skunky Alemental -- 73367
	pet = AddPet(73367, V.MOP, Q.RARE)
	pet:SetItemID(104167)
	pet:SetSpellID(148063)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(71919)

	-- Sky Lantern -- 73809
	pet = AddPet(73809, V.MOP, Q.RARE)
	pet:SetItemID(104332)
	pet:SetSpellID(148684)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(73819)

	-- Skywisp Moth -- 73368
	pet = AddPet(73368, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TIMELESS_ISLE, "25", false, "47.2:67.7", "28.4:34.9", "27.2:65.7", "25.4:43.1")

	-- Smolderweb Hatchling -- 10598
	pet = AddPet(10598, V.CATA, Q.RARE)
	pet:SetItemID(68673)
	pet:SetSpellID(16450)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(10596)

	-- Snarly -- 26050
	pet = AddPet(26050, V.TBC, Q.RARE)
	pet:SetItemID(35349)
	pet:SetSpellID(46425)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.QUEST)
	pet:AddQuest(11665)
	pet:AddProfession(PROF.FISHING)

	-- Snowshoe Hare -- 61755
	pet = AddPet(61755, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.HILLSBRAD_FOOTHILLS, "6-7", false, "46.6:35.2", "41.2:27.6", "47.6:21.0", "51.0:22.4", "53.6:24.8")

	-- Snowshoe Rabbit -- 7560
	pet = AddPet(7560, V.ORIG, Q.COMMON)
	pet:SetItemID(8497)
	pet:SetSpellID(10711)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(1263)

	-- Snowy Owl -- 7554
	pet = AddPet(7554, V.MOP, Q.COMMON)
	pet:SetSeason(SEASON.WINTER)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.WINTERSPRING, "17-18", false, "63.6:49.4", "57.6:45.8", "51.6:32.6", "57.6:58.8")

	-- Snowy Panda -- 69893
	pet = AddPet(69893, V.MOP, Q.RARE)
	pet:SetItemID(94209)
	pet:SetSpellID(138381)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32604)

	-- Softshell Snapling -- 63060
	pet = AddPet(63060, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.VALLEY_OF_THE_FOUR_WINDS, "23-25", false, "88.4:30.6", "57.8:37.0", "70.2:57.4", "45.0:72.0")

	-- Son of Animus -- 69820
	pet = AddPet(69820, V.MOP, Q.RARE)
	pet:SetItemID(94152)
	pet:SetSpellID(138161)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(69427)

	-- Soul of the Aspects -- 58163
	pet = AddPet(58163, V.CATA, Q.RARE)
	pet:SetItemID(78916)
	pet:SetSpellID(110029)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("STORE")

	-- Spawn of G'nathus -- 70098
	pet = AddPet(70098, V.MOP, Q.RARE)
	pet:SetItemID(94595)
	pet:SetSpellID(138913)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	pet:AddMobDrop(66467)

	-- Spectral Cub -- 68502
	pet = AddPet(68502, V.MOP, Q.RARE)
	pet:SetItemID(90953)
	pet:SetSpellID(132759)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
	pet:AddCustom("CHINA")

	-- Spectral Porcupette -- 69848
	pet = AddPet(69848, V.MOP, Q.RARE)
	pet:SetItemID(94190)
	pet:SetSpellID(138285)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32616)

	-- Spectral Tiger Cub -- 36511
	pet = AddPet(36511, V.WOTLK, Q.RARE)
	pet:SetItemID(49343)
	pet:SetSpellID(68810)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Speedy -- 16547
	pet = AddPet(16547, V.ORIG, Q.COMMON)
	pet:SetItemID(23002)
	pet:SetSpellID(28738)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_EVENTS, F.QUEST)
	pet:AddWorldEvent("CHILDRENS_WEEK")
	pet:AddQuest(171, 5502)

	-- Spineclaw Crab -- 73366
	pet = AddPet(73366, V.MOP, Q.RARE)
	pet:SetItemID(104168)
	pet:SetSpellID(148066)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(73166)

	-- Spirebound Crab -- 63005
	pet = AddPet(63005, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "66.6:27.0", "68.6:25.0", "68.4:31.2", "68.8:28.8")

	-- Spirit of Competition -- 27217
	pet = AddPet(27217, V.TBC, Q.RARE)
	pet:SetItemID(37297)
	pet:SetSpellID(48406)
	pet:Retire()

	-- Spirit of Summer -- 16701
	pet = AddPet(16701, V.ORIG, Q.RARE)
	pet:SetItemID(23083)
	pet:SetSpellID(28871)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE, F.WORLD_EVENTS)
	pet:AddVendor(26123, 26124)
	pet:AddWorldEvent("MIDSUMMER_FIRE_FESTIVAL")
	pet:AddCustom("CITY")

	-- Spring Rabbit -- 32791
	pet = AddPet(32791, V.WOTLK, Q.RARE)
	pet:SetItemID(44794)
	pet:SetSpellID(61725)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE)
	pet:AddVendor(32836, 32837)
	pet:AddWorldEvent("NOBLEGARDEN")
	pet:AddCustom("NOBLE_VENDOR")

	-- Sprite Darter Hatchling -- 9662
	pet = AddPet(9662, V.ORIG, Q.COMMON)
	pet:SetItemID(11474)
	pet:SetSpellID(15067)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.WORLD_DROP)
	pet:AddWorldDrop(Z.FERALAS)

	-- Stinker -- 23274
	pet = AddPet(23274, V.TBC, Q.RARE)
	pet:SetItemID(40653)
	pet:SetSpellID(40990)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(1250)

	-- Stitched Pup -- 68654
	pet = AddPet(68654, V.MOP, Q.RARE)
	pet:SetItemID(93029)
	pet:SetSpellID(135257)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(15932)

	-- Stone Armadillo -- 62187
	pet = AddPet(62187, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DESOLACE, "7-9", false, "71.6:62.6", "59.6:78.8", "67.0:29.8", "43.6:39.2", "60.6:29.6")
	pet:SetTimeOfDay(TOD.NIGHT)

	-- Stowaway Rat -- 62921
	pet = AddPet(62921, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DEEPHOLM, "22-23", false, "54.4:72.6", "56.4:75.4", "57.8:76.4")

	-- Strand Crawler -- 33226
	pet = AddPet(33226, V.WOTLK, Q.RARE)
	pet:SetItemID(44983)
	pet:SetSpellID(62561)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION)
	pet:AddProfession(PROF.FISHING)
	pet:AddCustom("FISHING_DAILIES")

	-- Stunted Direhorn -- 69849
	pet = AddPet(69849, V.MOP, Q.COMMON)
	pet:SetItemID(94191)
	pet:SetSpellID(138287)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.ACHIEVEMENT)
	pet:AddAchievement(8300)

	-- Summit Kid -- 64248
	pet = AddPet(64248, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "41.6:71.2", "39.8:82.8", "36.4:74.0", "55.0:56.2", "59.8:47.6")

	-- Sumprush Rodent -- 68506
	pet = AddPet(68506, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KRASARANG_WILDS, "23-25", false, "89.0:32.0", "10.0:54.2")

	-- Sunfur Panda -- 69891
	pet = AddPet(69891, V.MOP, Q.RARE)
	pet:SetItemID(94208)
	pet:SetSpellID(138380)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.IBOP)
	pet:AddQuest(32603)

	-- Sunreaver Micro-Sentry -- 69778
	pet = AddPet(69778, V.MOP, Q.RARE)
	pet:SetItemID(94124)
	pet:SetSpellID(138082)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	pet:AddMobDrop(50358)

	-- Stunted Yeti -- 68805
	pet = AddPet(68805, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.FERALAS, "11-12", false, "53.0:59.6", "51.4:58.0", "56.4:57.2", "53.6:55.6")

	-- Swamp Croaker -- 69819
	pet = AddPet(69819, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ISLE_OF_THUNDER, "25", false, "50.8:48.2", "41.8:64.6", "38.4:56.2")

	-- Szechuan Chicken -- 63585
	pet = AddPet(63585, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "61.2:88.0", "49.2:46.2")

	-- Tainted Cockroach -- 62314
	pet = AddPet(62314, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.FELWOOD, "14-15", false, "61.8:9.8", "54.2:18.8", "42.8:50.6", "48.8:90.2", "45.2:66.4", "39.0:68.6")
	pet:AddZoneLocations(Z.SHADOWMOON_VALLEY, "20-21", false, "51.8:31.6", "42.4:53.4", "37.4:41.8", "37.4:32.2", "64.2:35.2")

	-- Tainted Waveling -- 71018
	pet = AddPet(71018, V.MOP, Q.COMMON)
	pet:SetItemID(97553)
	pet:SetSpellID(141437)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(21216)

	-- Teldrassil Sproutling -- 33188
	pet = AddPet(33188, V.WOTLK, Q.RARE)
	pet:SetItemID(44965)
	pet:SetSpellID(62491)
	pet:AddFilters(F.ALLIANCE, F.IBOE, F.VENDOR)
	pet:AddVendor(33653)
	pet:AddCustom("CHAMPION_SEALS")

	-- Temple Snake -- 62999
	pet = AddPet(62999, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.THE_JADE_FOREST, "23-25", false, "57.6:62.2", "55.2:63.0", "52.8:55.8", "56.8:51.0")

	-- Terky -- 16445
	pet = AddPet(16445, V.MOP, Q.COMMON)
	pet:SetItemID(22780)
	pet:Retire()

	-- Terrible Turnip -- 63365
	pet = AddPet(63365, V.MOP, Q.COMMON)
	pet:SetItemID(85220)
	pet:SetSpellID(123778)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_DROP)
	pet:AddWorldDrop(Z.VALLEY_OF_THE_FOUR_WINDS)

	-- Thundering Serpent Hatchling -- 65313
	pet = AddPet(65313, V.MOP, Q.COMMON)
	pet:SetItemID(85513)
	pet:SetSpellID(127813)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(6634)

	-- Thundertail Flapper -- 69648
	pet = AddPet(69648, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ISLE_OF_THUNDER, "25", false, "47.8:47.4", "54.4:49.6", "53.2:55.0", "55.6:40.6")

	-- Tickbird Hatchling -- 32589
	pet = AddPet(32589, V.WOTLK, Q.COMMON)
	pet:SetItemID(39896)
	pet:SetSpellID(61348)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.REPUTATION, F.THE_ORACLES)
	pet:AddRepVendor(FAC.THE_ORACLES, REP.REVERED, 31910)
	pet:AddCustom("MYSTERIOUS_EGG")

	-- Tideskipper -- 71017
	pet = AddPet(71017, V.MOP, Q.COMMON)
	pet:SetItemID(97552)
	pet:SetSpellID(141436)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(21213)

	-- Tiny Blue Carp -- 70258
	pet = AddPet(70258, V.MOP, Q.COMMON)
	pet:SetItemID(94933)
	pet:SetSpellID(139362)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.WORLD_DROP)
	pet:AddProfession(PROF.FISHING)
	pet:AddWorldDrop(Z.VALE_OF_ETERNAL_BLOSSOMS)

	-- Tiny Bog Beast -- 61686
	pet = AddPet(61686, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.WETLANDS, "6-7", false, "55.6:34.6", "52.8:38.0", "50.4:30.6", "55.6:24.2")

	-- Tiny Flamefly -- 51632
	pet = AddPet(51632, V.CATA, Q.COMMON)
	pet:SetItemID(66080)
	pet:SetSpellID(93813)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(28491, 28415)

	-- Tiny Goldfish -- 63559
	pet = AddPet(63559, V.MOP, Q.COMMON)
	pet:SetItemID(85447)
	pet:SetSpellID(124000)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.REPUTATION, F.THE_ANGLERS)
	pet:AddRepVendor(FAC.THE_ANGLERS, REP.HONORED, 63721)

	-- Tiny Green Carp -- 70259
	pet = AddPet(70259, V.MOP, Q.COMMON)
	pet:SetItemID(94934)
	pet:SetSpellID(139363)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.WORLD_DROP)
	pet:AddProfession(PROF.FISHING)
	pet:AddWorldDrop(Z.VALLEY_OF_THE_FOUR_WINDS, Z.KRASARANG_WILDS, Z.THE_JADE_FOREST)

	-- Tiny Green Dragon -- 14755
	pet = AddPet(14755, V.WOTLK, Q.RARE)
	pet:SetItemID(19055)
	pet:SetSpellID(23531)
	pet:Retire()

	-- Tiny Red Carp -- 70257
	pet = AddPet(70257, V.MOP, Q.COMMON)
	pet:SetItemID(94932)
	pet:SetSpellID(139361)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.WORLD_DROP)
	pet:AddProfession(PROF.FISHING)
	pet:AddWorldDrop(Z.TOWNLONG_STEPPES, Z.DREAD_WASTES)

	-- Tiny Red Dragon -- 14756
	pet = AddPet(14756, V.WOTLK, Q.RARE)
	pet:SetItemID(19054)
	pet:SetSpellID(23530)
	pet:Retire()

	-- Tiny Shale Spider -- 48982
	pet = AddPet(48982, V.CATA, Q.COMMON)
	pet:SetItemID(64494)
	pet:SetSpellID(91343)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP)
	pet:AddMobDrop(49822)

	-- Tiny Snowman -- 15710
	pet = AddPet(15710, V.ORIG, Q.COMMON)
	pet:SetItemID(21309)
	pet:SetSpellID(26045)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_EVENTS)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")

	-- Tiny Sporebat -- 25062
	pet = AddPet(25062, V.TBC, Q.RARE)
	pet:SetItemID(34478)
	pet:SetSpellID(45082)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.REPUTATION, F.SPOREGGAR, F.IBOP)
	pet:AddRepVendor(FAC.SPOREGGAR, REP.EXALTED, 18382)
	pet:AddCustom("UNCLAIMED_BM")

	-- Tiny White Carp -- 70260
	pet = AddPet(70260, V.MOP, Q.COMMON)
	pet:SetItemID(94935)
	pet:SetSpellID(139365)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.WORLD_DROP)
	pet:AddProfession(PROF.FISHING)
	pet:AddWorldDrop(Z.KUN_LAI_SUMMIT)

	-- Tirisfal Batling -- 33197
	pet = AddPet(33197, V.WOTLK, Q.RARE)
	pet:SetItemID(44971)
	pet:SetSpellID(62510)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(33555)
	pet:AddCustom("CHAMPION_SEALS")

	-- Tito -- 71023
	pet = AddPet(71023, V.MOP, Q.COMMON)
	pet:SetItemID(97558)
	pet:SetSpellID(141450)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.ACHIEVEMENT)
	pet:AddAchievement(8293)

	-- Tolai Hare -- 63557
	pet = AddPet(63557, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "59.6:82.4", "54.8:66.8", "67.0:33.6", "39.6:77.6", "69.6:91.0")
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "23-25", false, "61.6:35.2", "64.6:37.4")

	-- Tolai Hare Pup -- 63558
	pet = AddPet(63558, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-25", false, "59.4:82.8", "66.6:79.2", "73.8:64.4", "44.0:70.2", "55.8:63.8")

	-- Tol'vir Scarab -- 62899
	pet = AddPet(62899, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ULDUM, "23-24", false, "39.2:43.8", "43.0:39.8", "46.0:16.0", "53.2:28.8", "63.2:23.8", "63.6:30.2")

	-- Toothy -- 24388
	pet = AddPet(24388, V.TBC, Q.RARE)
	pet:SetItemID(33816)
	pet:SetSpellID(43697)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROFESSION, F.QUEST)
	pet:AddProfession(PROF.FISHING)
	pet:AddQuest(11665)

	-- Toxic Wasteling -- 38374
	pet = AddPet(38374, V.WOTLK, Q.RARE)
	pet:SetItemID(50446)
	pet:SetSpellID(71840)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.WORLD_EVENTS)
	pet:AddMobDrop(36296)
	pet:AddWorldEvent("LOVE_IS_IN_THE_AIR")

	-- Tranquil Mechanical Yeti -- 15699
	pet = AddPet(15699, V.ORIG, Q.COMMON)
	pet:SetItemID(21277)
	pet:SetSpellID(26010)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ENGINEERING)
	pet:AddCustom("BLINGTRON")

	-- Treasure Goblin -- 74413
	pet = AddPet(74413, V.MOP, Q.RARE)
	pet:SetItemID(106256)
	pet:SetSpellID(149810)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.COLLECTORS_EDITION)
	pet:AddCustom("DIABLOCE")

	-- Tree Frog -- 7549
	pet = AddPet(7549, V.ORIG, Q.COMMON)
	pet:SetItemID(11026)
	pet:SetSpellID(10704)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR, F.WORLD_EVENTS)
	pet:AddVendor(14860)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Turquoise Turtle -- 62121
	pet = AddPet(62121, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "66.8:69.8", "43.0:60.6", "40.6:56.0", "42.6:49.9", "43.2:41.8", "75.8:36.0")

	-- Tuskarr Kite -- 36482
	pet = AddPet(36482, V.WOTLK, Q.EPIC)
	pet:SetItemID(49287)
	pet:SetSpellID(68767)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.TCG)
	pet:AddCustom("TCG")

	-- Twilight Beetle -- 62118
	pet = AddPet(62118, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "69.2:16.6")
	pet:AddZoneLocations(Z.DEEPHOLM, "22-23", false, "61.6:34.0")
	pet:AddZoneLocations(Z.MOUNT_HYJAL, "22-23", false, "42.6:23.2")

	-- Twilight Spider -- 62117
	pet = AddPet(62117, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.AZSHARA, "3-6", false, "64.6:15.2", "71.6:16.0", "69.4:16.4", "67.6:17.4", "64.0:17.8")
	pet:AddZoneLocations(Z.DEEPHOLM, "22-23", false, "62.4:54.4", "35.6:67.4", "41.6:71.8", "43.2:76.4", "60.0:42.2", "63.2:34.4")
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "58.0:32.0", "40.0:50.2", "55.6:47.0", "61.6:47.6", "62.8:54.8", "57.0:57.6", "33.2:67.8", "59.8:83.4", "70.6:74.8", "63.6:67.4")

	-- Unborn Val'kyr -- 71163
	pet = AddPet(71163, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.BATTLE_PET, F.WORLD_DROP)
	pet:AddZoneLocations(Z.SHOLAZAR_BASIN, "22-23", false, "58.5:22.0")
	pet:AddZoneLocations(Z.ICECROWN, "22-23", false, "47.9:86.6", "73.8:64.5")
	pet:AddZoneLocations(Z.THE_STORM_PEAKS, "22-23", false, "42.5:78.5")
	pet:AddZoneLocations(Z.ZULDRAK, "22-23", false, "57.0:39.8")

	-- Undercity Cockroach -- 7395
	pet = AddPet(7395, V.ORIG, Q.COMMON)
	pet:SetItemID(10393)
	pet:SetSpellID(10688)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(8403, 20980)

	-- Untamed Hatchling -- 68661
	pet = AddPet(68661, V.MOP, Q.RARE)
	pet:SetItemID(93036)
	pet:SetSpellID(135263)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(12435)

	-- Vampiric Batling -- 28513
	pet = AddPet(28513, V.TBC, Q.RARE)
	pet:SetItemID(38658)
	pet:SetSpellID(51851)
	pet:Retire()

	-- Vengeful Porcupette -- 73688
	pet = AddPet(73688, V.MOP, Q.RARE)
	pet:SetItemID(103637)
	pet:SetSpellID(148427)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(73307)

	-- Venus -- 66491
	pet = AddPet(66491, V.MOP, Q.COMMON)
	pet:SetItemID(89736)
	pet:SetSpellID(130759)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.ACHIEVEMENT)
	pet:AddAchievement(7501)

	-- Viscidus Globule -- 68660
	pet = AddPet(68660, V.MOP, Q.RARE)
	pet:SetItemID(93039)
	pet:SetSpellID(135266)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID, F.IBOP)
	pet:AddMobDrop(15299)

	-- Viscous Horror -- 71200
	pet = AddPet(71200, V.MOP, Q.RARE)
	pet:SetItemID(97960)
	pet:SetSpellID(142029)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.RAID)
	pet:AddMobDrop(69017)
	pet:AddCustom("NORMAL")
	pet:AddCustom("HEROIC")

	-- Voodoo Figurine -- 53232
	pet = AddPet(53232, V.CATA, Q.RARE)
	pet:SetItemID(69824)
	pet:SetSpellID(98587)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.ARCHAEOLOGY)

	-- Warbot -- 34587
	pet = AddPet(34587, V.WOTLK, Q.RARE)
	pet:SetItemID(46767)
	pet:SetSpellID(65682)
	pet:Retire()

	-- Warpstalker Hatchling -- 62583
	pet = AddPet(62583, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TEROKKAR_FOREST, "18-19", false, "46.8:25.2", "36.6:37.6", "66.8:54.6", "68.4:43.2", "57.6:38.4")
	pet:AddZoneLocations(Z.SHADOWMOON_VALLEY, "18-19", false, "18.0:22.4")

	-- Water Waveling -- 62820
	pet = AddPet(62820, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.ZULDRAK, "22-23", false, "38.8:75.6", "40.8:71.4", "46.0:78.2", "42.6:83.6")

	-- Westfall Chicken -- 30379
	pet = AddPet(30379, V.ORIG, Q.COMMON)
	pet:SetItemID(11110)
	pet:SetSpellID(13548)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST)
	pet:AddQuest(3861)

	-- Whiskers the Rat -- 16549
	pet = AddPet(16549, V.ORIG, Q.COMMON)
	pet:SetItemID(23015)
	pet:SetSpellID(28740)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.IBOP, F.QUEST)
	pet:AddQuest(171, 5502)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- White Kitten -- 7386
	pet = AddPet(7386, V.ORIG, Q.COMMON)
	pet:SetItemID(8489)
	pet:SetSpellID(10679)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.VENDOR)
	pet:AddVendor(8666)

	-- White Moth -- 21018
	pet = AddPet(21018, V.TBC, Q.COMMON)
	pet:SetItemID(29904)
	pet:SetSpellID(35911)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(21019)

	-- White Tickbird Hatchling -- 32590
	pet = AddPet(32590, V.WOTLK, Q.COMMON)
	pet:SetItemID(39899)
	pet:SetSpellID(61349)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.REPUTATION, F.THE_ORACLES)
	pet:AddRepVendor(FAC.THE_ORACLES, REP.REVERED, 31910)
	pet:AddCustom("MYSTERIOUS_EGG")

	-- Widow Spiderling -- 61259
	pet = AddPet(61259, V.MOP, Q.COMMON)
	pet:SetTimeOfDay(TOD.NIGHT)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.DUSKWOOD, "5-7", false, "29.4:62.4", "34.6:54.6", "30.6:46.2", "33.8:39.6", "64.8:26.6", "60.0:32.0", "65.6:39.6")

	-- Wildhammer Gryphon Hatchling -- 62900
	pet = AddPet(62900, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TWILIGHT_HIGHLANDS, "23-24", false, "56.6:15.8", "48.4:28.2")

	-- Willy -- 23231
	pet = AddPet(23231, V.TBC, Q.RARE)
	pet:SetItemID(32617)
	pet:SetSpellID(40613)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.QUEST, F.WORLD_EVENTS, F.IBOP)
	pet:AddQuest(10966, 10967)
	pet:AddWorldEvent("CHILDRENS_WEEK")

	-- Wind Rider Cub -- 36909
	pet = AddPet(36909, V.WOTLK, Q.RARE)
	pet:SetItemID(49663)
	pet:SetSpellID(69536)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.STORE, F.IBOP)
	pet:AddCustom("PLUSHIE")

	-- Winter's Little Helper -- 15705
	pet = AddPet(15705, V.ORIG, Q.COMMON)
	pet:SetItemID(21395)
	pet:SetSpellID(26541)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.IBOP)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")

	-- Winter Reindeer -- 15706
	pet = AddPet(15706, V.ORIG, Q.RARE)
	pet:SetItemID(21308)
	pet:SetSpellID(26529)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.WORLD_EVENTS)
	pet:AddWorldEvent("FEAST_OF_WINTER_VEIL")

	-- Winterspring Cub -- 52831
	pet = AddPet(52831, V.CATA, Q.COMMON)
	pet:SetItemID(69239)
	pet:SetSpellID(97638)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE)
	pet:AddVendor(52830)

	-- Withers -- 34278
	pet = AddPet(34278, V.CATA, Q.COMMON)
	pet:SetItemID(46325)
	pet:SetSpellID(65046)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.QUEST, F.VENDOR)
	pet:AddQuest(13570)
	pet:AddVendor(33980)

	-- Wolpertinger -- 22943
	pet = AddPet(22943, V.TBC, Q.RARE)
	pet:SetItemID(32233)
	pet:SetSpellID(39709)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR, F.WORLD_EVENTS, F.QUEST)
	pet:AddWorldEvent("BREWFEST")
	pet:AddVendor(23710, 24495, 27478, 27489)
	pet:AddQuest(11117, 11431)

	-- Wood Frog -- 7550
	pet = AddPet(7550, V.ORIG, Q.COMMON)
	pet:SetItemID(11027)
	pet:SetSpellID(10703)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.VENDOR, F.IBOE, F.WORLD_EVENTS)
	pet:AddVendor(14860)
	pet:AddWorldEvent("DARKMOON_FAIRE")

	-- Worg Pup -- 10259
	pet = AddPet(10259, V.ORIG, Q.RARE)
	pet:SetItemID(12264)
	pet:SetSpellID(15999)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.MOB_DROP, F.IBOP, F.RAID)
	pet:AddMobDrop(9736)

	-- Xu-Fu, Cub of Xuen -- 71942
	pet = AddPet(71942, V.MOP, Q.RARE)
	pet:SetItemID(101771)
	pet:SetSpellID(145696)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(73082)

	-- Yakrat -- 63957
	pet = AddPet(63957, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.IBOP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.TOWNLONG_STEPPES, "24-25", false, "17.6:58.0", "75.8:82.6", "82.0:73.6", "83.6:79.0", "71.6:56.6", "67.2:49.2", "67.6:44.6")

	-- Yellow Moth -- 21008
	pet = AddPet(21008, V.TBC, Q.COMMON)
	pet:SetItemID(29903)
	pet:SetSpellID(35910)
	pet:AddFilters(F.ALLIANCE, F.VENDOR)
	pet:AddVendor(21019)

	-- Yu'la, Broodling of Yu'lon -- 72463
	pet = AddPet(72463, V.MOP, Q.RARE)
	pet:SetItemID(102147)
	pet:SetSpellID(145698)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(73082)

	-- Yu'lon Kite -- 66105
	pet = AddPet(66105, V.MOP, Q.COMMON)
	pet:SetItemID(89367)
	pet:SetSpellID(127006)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOE, F.PROFESSION)
	pet:AddProfession(PROF.INSCRIPTION)

	-- Zandalari Anklerender -- 70451
	pet = AddPet(70451, V.MOP, Q.RARE)
	pet:SetItemID(95422)
	pet:SetSpellID(139932)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(69925)

	-- Zandalari Footslasher -- 70452
	pet = AddPet(70452, V.MOP, Q.RARE)
	pet:SetItemID(95423)
	pet:SetSpellID(139933)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(69925)

	-- Zandalari Kneebiter -- 69796
	pet = AddPet(69796, V.MOP, Q.RARE)
	pet:SetItemID(94126)
	pet:SetSpellID(138087)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(69925)

	-- Zandalari Toenibbler -- 70453
	pet = AddPet(70453, V.MOP, Q.RARE)
	pet:SetItemID(95424)
	pet:SetSpellID(139934)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.MOB_DROP)
	pet:AddMobDrop(69925)

	-- Zao, Calfling of Niuzao -- 72464
	pet = AddPet(72464, V.MOP, Q.RARE)
	pet:SetItemID(102146)
	pet:SetSpellID(145699)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.VENDOR)
	pet:AddVendor(73082)

	-- Zergling -- 11327
	pet = AddPet(11327, V.ORIG, Q.COMMON)
	pet:SetItemID(13582)
	pet:SetSpellID(17709)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOA, F.COLLECTORS_EDITION)
	pet:AddCustom("CE")

	-- Zipao Tiger -- 36910
	pet = AddPet(36910, V.WOTLK, Q.RARE)
	pet:SetItemID(49664)
	pet:SetSpellID(69539)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.IBOP, F.PROMO)
	pet:AddCustom("RECRUIT")

	-- Zooey Snake -- 63555
	pet = AddPet(63555, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddZoneLocations(Z.KUN_LAI_SUMMIT, "23-24", false, "71.6:90.2", "68.6:87.4", "61.2:69.8", "53.6:78.6", "73.2:65.6")



-- TO DO STILL

	-- Rock Viper -- 62184
	pet = AddPet(62184, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLADES_EDGE_MOUNTAINS, Z.DESOLACE, Z.MOUNT_HYJAL, Z.SILITHUS)

	-- Maggot -- 61753
	pet = AddPet(61753, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.DUN_MOROGH, Z.GHOSTLANDS, Z.HILLSBRAD_FOOTHILLS, Z.HOWLING_FJORD, Z.THE_HINTERLANDS, Z.TIRISFAL_GLADES)

	-- Water Snake -- 61367
	pet = AddPet(61367, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUROTAR, Z.NORTHERN_STRANGLETHORN, Z.ORGRIMMAR, Z.SWAMP_OF_SORROWS, Z.TWILIGHT_HIGHLANDS, Z.WETLANDS)

	-- Long-tailed Mole -- 61317
	pet = AddPet(61317, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUN_MOROGH, Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN, Z.UNGORO_CRATER)

	-- Ash Spiderling -- 61420
	pet = AddPet(61420, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SEARING_GORGE)

	-- Tundra Penguin -- 62835
	pet = AddPet(62835, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BOREAN_TUNDRA, Z.DRAGONBLIGHT)

	-- Silkbead Snail -- 63001
	pet = AddPet(63001, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Spawn of Onyxia -- 62201
	pet = AddPet(62201, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUSTWALLOW_MARSH)

	-- Fire-Proof Roach -- 62886
	pet = AddPet(62886, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.MOUNT_HYJAL)

	-- Grotto Vole -- 62884
	pet = AddPet(62884, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.MOUNT_HYJAL)

	-- Arctic Fox Kit -- 62864
	pet = AddPet(62864, V.MOP, Q.COMMON)
	pet:SetWeather(WEATHER.SNOW)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_STORM_PEAKS)

	-- Scourged Whelpling -- 62854
	pet = AddPet(62854, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ICECROWN)

	-- Dragonbone Hatchling -- 62852
	pet = AddPet(62852, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DRAGONBLIGHT)

	-- Fawn -- 61165
	pet = AddPet(61165, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ELWYNN_FOREST, Z.GRIZZLY_HILLS, Z.TELDRASSIL)

	-- Biletoad -- 62815
	pet = AddPet(62815, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_BARRENS, Z.SHOLAZAR_BASIN, Z.WAILING_CAVERNS)

	-- Oily Slimeling -- 62697
	pet = AddPet(62697, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BOREAN_TUNDRA)

	-- Borean Marmot -- 62695
	pet = AddPet(62695, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BOREAN_TUNDRA)

	-- Adder -- 61325
	pet = AddPet(61325, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLASTED_LANDS, Z.DUN_MOROGH, Z.DUROTAR, Z.HELLFIRE_PENINSULA, Z.NAGRAND, Z.NORTHERN_BARRENS, Z.NORTHERN_STRANGLETHORN)

	-- Arctic Hare -- 62693
	pet = AddPet(62693, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BOREAN_TUNDRA, Z.DRAGONBLIGHT, Z.THE_STORM_PEAKS, Z.ZULDRAK)

	-- Crimson Shale Hatchling -- 62922
	pet = AddPet(62922, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DEEPHOLM, Z.THE_STONECORE)

	-- Chicken -- 62664
	pet = AddPet(62664, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ARATHI_BASIN, Z.AZUREMYST_ISLE, Z.BLOODMYST_ISLE, Z.DUSKWOOD, Z.DUSTWALLOW_MARSH, Z.ELWYNN_FOREST, Z.GILNEAS, Z.HILLSBRAD_FOOTHILLS, Z.HOWLING_FJORD, Z.NORTHERN_BARRENS, Z.REDRIDGE_MOUNTAINS, Z.SHATTRATH_CITY, Z.TIRISFAL_GLADES, Z.WESTFALL, Z.WETLANDS)

	-- Stormwind Rat -- 62954
	pet = AddPet(62954, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ELWYNN_FOREST)

	-- Garden Frog -- 63002
	pet = AddPet(63002, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Scalded Basilisk Hatchling -- 62628
	pet = AddPet(62628, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLADES_EDGE_MOUNTAINS)

	-- Sidewinder -- 62523
	pet = AddPet(62523, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SILITHUS, Z.ULDUM)

	-- Fledgling Nether Ray -- 62627
	pet = AddPet(62627, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NETHERSTORM)

	-- Flayer Youngling -- 62555
	pet = AddPet(62555, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.HELLFIRE_PENINSULA)

	-- Mongoose -- 65190
	pet = AddPet(65190, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TOWNLONG_STEPPES)

	-- Fel Flame -- 62621
	pet = AddPet(62621, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SHADOWMOON_VALLEY)

	-- Mountain Skunk -- 61677
	pet = AddPet(61677, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.GRIZZLY_HILLS, Z.STONETALON_MOUNTAINS, Z.THE_STORM_PEAKS, Z.WETLANDS, Z.WINTERSPRING)

	-- Shimmershell Snail -- 62246
	pet = AddPet(62246, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DARKSHORE)

	-- Sporeling Sprout -- 62564
	pet = AddPet(62564, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ZANGARMARSH)

	-- Spirit Crab -- 62034
	pet = AddPet(62034, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.GHOSTLANDS)

	-- Yellow-Bellied Bullfrog -- 63849
	pet = AddPet(63849, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.VALE_OF_ETERNAL_BLOSSOMS)

	-- Scarab Hatchling -- 62524
	pet = AddPet(62524, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SILITHUS)

	-- Nether Faerie Dragon -- 62395
	pet = AddPet(62395, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.FERALAS)

	-- Spotted Bell Frog -- 62370
	pet = AddPet(62370, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.UNGORO_CRATER)

	-- Ash Lizard -- 62364
	pet = AddPet(62364, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.MOUNT_HYJAL, Z.UNGORO_CRATER)

	-- Small Frog -- 61071
	pet = AddPet(61071, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ARATHI_HIGHLANDS, Z.DARNASSUS, Z.DESOLACE, Z.DUN_MOROGH, Z.ELWYNN_FOREST, Z.EVERSONG_WOODS, Z.GHOSTLANDS, Z.GILNEAS, Z.LOCH_MODAN, Z.NORTHERN_BARRENS, Z.SOUTHERN_BARRENS, Z.SWAMP_OF_SORROWS, Z.TELDRASSIL, Z.THE_LOST_ISLES, Z.ZANGARMARSH)

	-- Clefthoof Runt -- 62620
	pet = AddPet(62620, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NAGRAND)

	-- Tainted Rat -- 62316
	pet = AddPet(62316, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.FELWOOD)

	-- Tainted Moth -- 62315
	pet = AddPet(62315, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.FELWOOD)

	-- Mountain Cottontail -- 61167
	pet = AddPet(61167, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.MULGORE, Z.REDRIDGE_MOUNTAINS)

	-- Frog -- 62312
	pet = AddPet(62312, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.THE_LOST_ISLES)

	-- Silithid Hatchling -- 62258
	pet = AddPet(62258, V.MOP, Q.COMMON)
	pet:SetWeather(WEATHER.SANDSTORM)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TANARIS)

	-- Sand Kitten -- 62257
	pet = AddPet(62257, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TANARIS)

	-- Infested Bear Cub -- 61758
	pet = AddPet(61758, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.HILLSBRAD_FOOTHILLS)

	-- Stinkbug -- 62256
	pet = AddPet(62256, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TANARIS)

	-- Minfernal -- 62317
	pet = AddPet(62317, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.FELWOOD)

	-- Spider -- 61327
	pet = AddPet(61327, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.AZSHARA, Z.BLASTED_LANDS, Z.DUN_MOROGH, Z.DUSTWALLOW_MARSH, Z.EASTERN_PLAGUELANDS, Z.ELWYNN_FOREST, Z.GHOSTLANDS, Z.GILNEAS, Z.GILNEAS_CITY, Z.HILLSBRAD_FOOTHILLS, Z.HOWLING_FJORD, Z.ORGRIMMAR, Z.RUINS_OF_GILNEAS, Z.STONETALON_MOUNTAINS, Z.SWAMP_OF_SORROWS, Z.TELDRASSIL, Z.THE_HINTERLANDS, Z.THE_STORM_PEAKS, Z.TIRISFAL_GLADES, Z.WINTERSPRING)

	-- Mac Frog -- 62892
	pet = AddPet(62892, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ULDUM)

	-- Restless Shadeling -- 61375
	pet = AddPet(61375, V.MOP, Q.COMMON)
	pet:SetTimeOfDay(TOD.EARLY_MORNING)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DEADWIND_PASS)

	-- Deepholm Cockroach -- 62924
	pet = AddPet(62924, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DEEPHOLM)

	-- Alpine Chipmunk -- 62189
	pet = AddPet(62189, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.MOUNT_HYJAL, Z.STONETALON_MOUNTAINS, Z.WINTERSPRING)

	-- Grove Viper -- 63004
	pet = AddPet(63004, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Plains Monitor -- 63547
	pet = AddPet(63547, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.KUN_LAI_SUMMIT)

	-- Amethyst Shale Hatchling -- 62182
	pet = AddPet(62182, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DEEPHOLM, Z.DESOLACE)

	-- Topaz Shale Hatchling -- 62181
	pet = AddPet(62181, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DEEPHOLM, Z.DESOLACE)

	-- Fjord Worg Pup -- 62669
	pet = AddPet(62669, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.HOWLING_FJORD)

	-- Redridge Rat -- 61168
	pet = AddPet(61168, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.REDRIDGE_MOUNTAINS)

	-- Cheetah Cub -- 62129
	pet = AddPet(62129, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_BARRENS)

	-- Bat -- 61829
	pet = AddPet(61829, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.EASTERN_PLAGUELANDS, Z.MOUNT_HYJAL, Z.TIRISFAL_GLADES)

	-- Strand Crab -- 61312
	pet = AddPet(61312, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.DRAGONBLIGHT, Z.DUN_MOROGH, Z.GILNEAS, Z.KEZAN, Z.NORTHERN_STRANGLETHORN, Z.SWAMP_OF_SORROWS, Z.THE_CAPE_OF_STRANGLETHORN, Z.THE_LOST_ISLES)

	-- Fire Beetle -- 61328
	pet = AddPet(61328, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLASTED_LANDS, Z.BURNING_STEPPES, Z.MOUNT_HYJAL, Z.SEARING_GORGE, Z.UNGORO_CRATER)



	-- Crystal Beetle -- 62925
	pet = AddPet(62925, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DEEPHOLM)

	-- Stripe-Tailed Scorpid -- 61440
	pet = AddPet(61440, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BADLANDS, Z.TANARIS, Z.TEROKKAR_FOREST)

	-- Qiraji Guardling -- 62526
	pet = AddPet(62526, V.MOP, Q.COMMON)
	pet:SetSeason(SEASON.SUMMER)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SILITHUS)

	-- Wharf Rat -- 61323
	pet = AddPet(61323, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_CAPE_OF_STRANGLETHORN, Z.TOL_BARAD_PENINSULA)

	-- Blighted Squirrel -- 61890
	pet = AddPet(61890, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SILVERPINE_FOREST)

	-- Festering Maggot -- 61830
	pet = AddPet(61830, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.EASTERN_PLAGUELANDS)

	-- Emerald Boa -- 62127
	pet = AddPet(62127, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_BARRENS, Z.SOUTHERN_BARRENS, Z.ULDUM, Z.UNGORO_CRATER)

	-- Infected Squirrel -- 61828
	pet = AddPet(61828, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLOODMYST_ISLE, Z.EASTERN_PLAGUELANDS, Z.SILVERPINE_FOREST)

	-- Venomspitter Hatchling -- 62191
	pet = AddPet(62191, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.STONETALON_MOUNTAINS)

	-- Wanderer's Festival Hatchling -- 67022
	pet = AddPet(67022, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_EVENTS, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldEvent("WANDERERS_FESTIVAL")
	pet:AddWorldDrop(Z.KRASARANG_WILDS)

	-- Red-Tailed Chipmunk -- 61757
	pet = AddPet(61757, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DARNASSUS, Z.DESOLACE, Z.HILLSBRAD_FOOTHILLS, Z.TELDRASSIL)

	-- Twilight Iguana -- 62255
	pet = AddPet(62255, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THOUSAND_NEEDLES)

	-- Spiny Terrapin -- 63293
	pet = AddPet(63293, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.KRASARANG_WILDS)

	-- Tiny Twister -- 61703
	pet = AddPet(61703, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ARATHI_HIGHLANDS)

	-- Irradiated Roach -- 61691
	pet = AddPet(61691, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NEW_TINKERTOWN)

	-- Alpine Hare -- 61690
	pet = AddPet(61690, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUN_MOROGH, Z.WINTERSPRING, Z.NEW_TINKERTOWN)

	-- Parrot -- 61313
	pet = AddPet(61313, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.SWAMP_OF_SORROWS, Z.THE_CAPE_OF_STRANGLETHORN, Z.UNGORO_CRATER)

	-- Scorpling -- 61329
	pet = AddPet(61329, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLASTED_LANDS)

	-- Snow Cub -- 61689
	pet = AddPet(61689, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUN_MOROGH)

	-- Little Black Ram -- 61459
	pet = AddPet(61459, V.MOP, Q.COMMON)
	pet:SetTimeOfDay(TOD.DAY)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.LOCH_MODAN)

	-- King Snake -- 61443
	pet = AddPet(61443, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BADLANDS)

	-- Molten Hatchling -- 61425
	pet = AddPet(61425, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SEARING_GORGE)

	-- Spiky Lizard -- 61441
	pet = AddPet(61441, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BADLANDS, Z.SILITHUS)

	-- Sandy Petrel -- 63006
	pet = AddPet(63006, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Grassland Hopper -- 63549
	pet = AddPet(63549, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TOWNLONG_STEPPES)

	-- Ash Viper -- 61385
	pet = AddPet(61385, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BURNING_STEPPES, Z.SHADOWMOON_VALLEY)

	-- Cockroach -- 61384
	pet = AddPet(61384, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BURNING_STEPPES, Z.EASTERN_PLAGUELANDS, Z.ICECROWN, Z.THE_HINTERLANDS, Z.TWILIGHT_HIGHLANDS, Z.UNGORO_CRATER)

	-- Moccasin -- 61372
	pet = AddPet(61372, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SWAMP_OF_SORROWS)

	-- Swamp Moth -- 61370
	pet = AddPet(61370, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SWAMP_OF_SORROWS)

	-- Toad -- 61369
	pet = AddPet(61369, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.DUN_MOROGH, Z.DUROTAR, Z.DUSTWALLOW_MARSH, Z.EVERSONG_WOODS, Z.FELWOOD, Z.GHOSTLANDS, Z.GILNEAS, Z.HILLSBRAD_FOOTHILLS, Z.HOWLING_FJORD, Z.NAGRAND, Z.ORGRIMMAR, Z.RUINS_OF_GILNEAS, Z.SILVERPINE_FOREST, Z.SWAMP_OF_SORROWS, Z.TELDRASSIL, Z.TWILIGHT_HIGHLANDS, Z.WETLANDS)

	-- Gazelle Fawn -- 62176
	pet = AddPet(62176, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.MULGORE)

	-- Rat -- 61366
	pet = AddPet(61366, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.AMMEN_VALE, Z.ARATHI_BASIN, Z.ARATHI_HIGHLANDS, Z.ASHENVALE, Z.AZSHARA, Z.AZUREMYST_ISLE, Z.BLOODMYST_ISLE, Z.CRYSTALSONG_FOREST, Z.DARKSHORE, Z.DESOLACE, Z.DUN_MOROGH, Z.GHOSTLANDS, Z.GILNEAS, Z.GILNEAS_CITY, Z.HILLSBRAD_FOOTHILLS, Z.HOWLING_FJORD, Z.LOCH_MODAN, Z.NAGRAND, Z.RUINS_OF_GILNEAS, Z.SHATTRATH_CITY, Z.SILVERPINE_FOREST, Z.STONETALON_MOUNTAINS, Z.STORMWIND_CITY, Z.SWAMP_OF_SORROWS, Z.TEROKKAR_FOREST, Z.THE_CAPE_OF_STRANGLETHORN, Z.THE_HINTERLANDS, Z.THE_LOST_ISLES, Z.TIRISFAL_GLADES)
	pet:AddZoneLocations(Z.TIMELESS_ISLE, "25", false, "37.8:46.5")

	-- Scorpid -- 61326
	pet = AddPet(61326, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLADES_EDGE_MOUNTAINS, Z.BLASTED_LANDS, Z.BURNING_STEPPES, Z.EASTERN_PLAGUELANDS, Z.HELLFIRE_PENINSULA, Z.ORGRIMMAR, Z.SHADOWMOON_VALLEY, Z.SILITHUS, Z.THOUSAND_NEEDLES, Z.TWILIGHT_HIGHLANDS)

	-- Baby Ape -- 61324
	pet = AddPet(61324, V.MOP, Q.COMMON)
	pet:SetWeather(WEATHER.RAIN)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_CAPE_OF_STRANGLETHORN)

	-- Polly -- 61322
	pet = AddPet(61322, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN)

	-- Lizard Hatchling -- 61321
	pet = AddPet(61321, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN)

	-- Forest Spiderling -- 61320
	pet = AddPet(61320, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN)

	-- Crimson Moth -- 61314
	pet = AddPet(61314, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN)

	-- Beetle -- 61319
	pet = AddPet(61319, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.BADLANDS, Z.DARKSHORE, Z.EASTERN_PLAGUELANDS, Z.FELWOOD, Z.NORTHERN_STRANGLETHORN, Z.SILITHUS, Z.THE_CAPE_OF_STRANGLETHORN, Z.UNGORO_CRATER)

	-- Tree Python -- 61318
	pet = AddPet(61318, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.NORTHERN_STRANGLETHORN, Z.THE_CAPE_OF_STRANGLETHORN, Z.UNGORO_CRATER)

	-- Oasis Moth -- 62895
	pet = AddPet(62895, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ULDUM)

	-- Undercity Rat -- 61889
	pet = AddPet(61889, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TIRISFAL_GLADES)

	-- Lost of Lordaeron -- 61905
	pet = AddPet(61905, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TIRISFAL_GLADES)

	-- Fledgling Buzzard -- 61171
	pet = AddPet(61171, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.REDRIDGE_MOUNTAINS)

	-- Roach -- 61169
	pet = AddPet(61169, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.AZSHARA, Z.DESOLACE, Z.DUN_MOROGH, Z.DUSKWOOD, Z.GILNEAS, Z.GILNEAS_CITY, Z.HOWLING_FJORD, Z.KEZAN, Z.LOCH_MODAN, Z.NORTHERN_STRANGLETHORN, Z.REDRIDGE_MOUNTAINS, Z.STONETALON_MOUNTAINS, Z.THE_CAPE_OF_STRANGLETHORN, Z.THOUSAND_NEEDLES, Z.TIRISFAL_GLADES)

	-- Tiny Harvester -- 61160
	pet = AddPet(61160, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.WESTFALL)

	-- Mouse -- 61143
	pet = AddPet(61143, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUSKWOOD, Z.DUSTWALLOW_MARSH, Z.GRIZZLY_HILLS, Z.MULGORE, Z.NETHERSTORM, Z.WESTFALL, Z.WETLANDS)

	-- Prairie Dog -- 61141
	pet = AddPet(61141, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ARATHI_HIGHLANDS, Z.MULGORE, Z.NAGRAND, Z.NORTHERN_BARRENS, Z.STORMWIND_CITY, Z.WESTFALL)

	-- Squirrel -- 61081
	pet = AddPet(61081, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.AMMEN_VALE, Z.ASHENVALE, Z.AZSHARA, Z.AZUREMYST_ISLE, Z.BLADES_EDGE_MOUNTAINS, Z.CRYSTALSONG_FOREST, Z.DARKSHORE, Z.DUN_MOROGH, Z.DUSKWOOD, Z.DUSTWALLOW_MARSH, Z.ELWYNN_FOREST, Z.FERALAS, Z.GILNEAS, Z.HILLSBRAD_FOOTHILLS, Z.HOWLING_FJORD, Z.LOCH_MODAN, Z.MOONGLADE, Z.MOUNT_HYJAL, Z.NAGRAND, Z.RUINS_OF_GILNEAS, Z.SHOLAZAR_BASIN, Z.SILVERPINE_FOREST, Z.STORMWIND_CITY, Z.TELDRASSIL, Z.TEROKKAR_FOREST, Z.TOL_BARAD_PENINSULA, Z.TWILIGHT_HIGHLANDS, Z.WESTERN_PLAGUELANDS, Z.WETLANDS)

	-- Black Lamb -- 60649
	pet = AddPet(60649, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ELWYNN_FOREST)

	-- Prairie Mouse -- 59702
	pet = AddPet(59702, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.KUN_LAI_SUMMIT)

	-- Devouring Maggot -- 62640
	pet = AddPet(62640, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.HOWLING_FJORD)

	-- Forest Moth -- 62177
	pet = AddPet(62177, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ASHENVALE, Z.DARNASSUS, Z.DESOLACE, Z.MOONGLADE, Z.MOUNT_HYJAL, Z.TELDRASSIL)

	-- Wild Jade Hatchling -- 65323
	pet = AddPet(65323, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Stunted Shardhorn -- 62816
	pet = AddPet(62816, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SHOLAZAR_BASIN)

	-- Blighthawk -- 61826
	pet = AddPet(61826, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.WESTERN_PLAGUELANDS)

	-- Leopard Scorpid -- 62896
	pet = AddPet(62896, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ULDUM)

	-- Spiny Lizard -- 62114
	pet = AddPet(62114, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUROTAR, Z.ORGRIMMAR)

	-- Giraffe Calf -- 62130
	pet = AddPet(62130, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.SOUTHERN_BARRENS)

	-- Elfin Rabbit -- 62178
	pet = AddPet(62178, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DARNASSUS, Z.DESOLACE, Z.MOUNT_HYJAL, Z.TELDRASSIL)

	-- Crested Owl -- 62242
	pet = AddPet(62242, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TELDRASSIL)

	-- Wild Golden Hatchling -- 65324
	pet = AddPet(65324, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Infected Fawn -- 61827
	pet = AddPet(61827, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLOODMYST_ISLE, Z.EASTERN_PLAGUELANDS, Z.SILVERPINE_FOREST)

	-- Leopard Tree Frog -- 63919
	pet = AddPet(63919, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Wild Crimson Hatchling -- 65321
	pet = AddPet(65321, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.THE_JADE_FOREST)

	-- Cat -- 62019
	pet = AddPet(62019, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.ARATHI_HIGHLANDS, Z.ELWYNN_FOREST, Z.EVERSONG_WOODS, Z.GILNEAS_CITY, Z.NETHERSTORM, Z.SILVERMOON_CITY, Z.SUNSTRIDER_ISLE)

	-- Ravager Hatchling -- 62051
	pet = AddPet(62051, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.BLOODMYST_ISLE)

	-- Dung Beetle -- 62115
	pet = AddPet(62115, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUROTAR, Z.ORGRIMMAR, Z.ULDUM)

	-- Dusk Spiderling -- 61253
	pet = AddPet(61253, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUSKWOOD)

	-- Grizzly Squirrel -- 62818
	pet = AddPet(62818, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.GRIZZLY_HILLS, Z.TWILIGHT_HIGHLANDS)

	-- Twilight Fiendling -- 62914
	pet = AddPet(62914, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TWILIGHT_HIGHLANDS)

	-- Crystal Spider -- 62435
	pet = AddPet(62435, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.WINTERSPRING)

	-- Feverbite Hatchling -- 65054
	pet = AddPet(65054, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.KRASARANG_WILDS)

	-- Ruby Sapling -- 62020
	pet = AddPet(62020, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.EVERSONG_WOODS)

	-- Turkey -- 62648
	pet = AddPet(62648, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.HOWLING_FJORD)

	-- Creepy Crawly -- 62116
	pet = AddPet(62116, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUROTAR)

	-- Yellow-Bellied Marmot -- 62904
	pet = AddPet(62904, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.TWILIGHT_HIGHLANDS)

	-- Snake -- 61142
	pet = AddPet(61142, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.DUN_MOROGH, Z.DUSTWALLOW_MARSH, Z.EVERSONG_WOODS, Z.FERALAS, Z.GHOSTLANDS, Z.GILNEAS, Z.HOWLING_FJORD, Z.LOCH_MODAN, Z.NAGRAND, Z.NORTHERN_STRANGLETHORN, Z.SHOLAZAR_BASIN, Z.SILVERPINE_FOREST, Z.TEROKKAR_FOREST, Z.TOL_BARAD, Z.WAILING_CAVERNS, Z.WESTFALL, Z.ZANGARMARSH, Z.ZULDRAK)

	-- Shore Crab -- 61158
	pet = AddPet(61158, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.AZSHARA, Z.BOREAN_TUNDRA, Z.HOWLING_FJORD, Z.KRASARANG_WILDS, Z.TWILIGHT_HIGHLANDS, Z.WESTFALL)

	-- Imperial Eagle Chick -- 62819
	pet = AddPet(62819, V.MOP, Q.COMMON)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.WORLD_DROP, F.BATTLE_PET)
	pet:AddWorldDrop(Z.GRIZZLY_HILLS)

	self.InitCritters = nil

	LPJ:RegisterCallback("PetListUpdated", UpdatePetList)
	UpdatePetList()
end


--[[ unknown when these pets will be added

	-- Murkimus Tyrannicus -- 71655
	pet = AddPet(71655, V.MOP, Q.RARE)
	pet:SetItemID(100870)
	pet:SetSpellID(143637)
	pet:AddFilters(F.ALLIANCE, F.HORDE, F.PVP, F.IBOP, F.PROMO)
	pet:AddCustom("ARENA")



]] --
