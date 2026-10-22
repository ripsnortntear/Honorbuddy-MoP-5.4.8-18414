--[[
************************************************************************
Scanner.lua
Database scanning for Collectinator
************************************************************************
File date: 2013-03-09T18:32:14Z
File hash: 2547f1c
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/collectinator/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]] --

-------------------------------------------------------------------------------
-- Upvalued Lua API.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-- Functions
local ipairs, pairs = _G.ipairs, _G.pairs
local tonumber, tostring = _G.tonumber, _G.tostring
local type = _G.type

-- Libraries
local bit = _G.bit
local string = _G.string
local table = _G.table

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub

local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)
local LPJ = LibStub("LibPetJournal-2.0")

-------------------------------------------------------------------------------
-- Constants
-------------------------------------------------------------------------------
local F = private.FILTER_IDS
local A = private.ACQUIRE_TYPES
local FS = private.LOCALIZED_FACTION_STRINGS
local REP = private.REP_LEVELS
local Q = private.ITEM_QUALITY_NAMES
local PROF = private.LOCALIZED_PROFESSION_NAMES
local FAC = private.FACTION_IDS
local Z = private.ZONE_NAMES

-------------------------------------------------------------------------------
-- Functions/methods
-------------------------------------------------------------------------------
function private.LoadAllCollectables()
	local collectable_list = private.collectable_list

	if addon.db.profile.autoloaddb then
		for identifier, name in pairs(private.ORDERED_COLLECTIONS) do
			addon:InitializeCollection(name)
		end
	end
	return collectable_list
end

do
	local FACTION_TEXT = {
		["thorium brotherhood"] = 98,
		["zandalar tribe"] = 100,
		["argent dawn"] = 96,
		["timbermaw hold"] = 99,
		["cenarion circle"] = 97,
		["the aldor"] = 101,
		["the consortium"] = 105,
		["the scryers"] = 110,
		["the sha'tar"] = 111,
		["the mag'har"] = 108,
		["cenarion expedition"] = 103,
		["honor hold"] = 104,
		["thrallmar"] = 104,
		["the violet eye"] = 114,
		["sporeggar"] = 113,
		["kurenai"] = 108,
		["keepers of time"] = 106,
		["the scale of the sands"] = 109,
		["lower city"] = 107,
		["ashtongue deathsworn"] = 102,
		["alliance vanguard"] = 131,
		["valiance expedition"] = 126,
		["horde expedition"] = 130,
		["the taunka"] = 128,
		["the hand of vengeance"] = 127,
		["explorers' league"] = 125,
		["the kalu'ak"] = 120,
		["shattered sun offensive"] = 112,
		["warsong offensive"] = 129,
		["kirin tor"] = 118,
		["the wyrmrest accord"] = 122,
		["knights of the ebon blade"] = 117,
		["frenzyheart tribe"] = 116,
		["the oracles"] = 121,
		["argent crusade"] = 115,
		["the sons of hodir"] = 119,
	}

	local FACTION_LEVELS = {
		neutral = 0,
		friendly = 1,
		honored = 2,
		revered = 3,
		exalted = 4,
	}

	local CLASS_TYPES = {
		["Death Knight"] = "DK",
		["Druid"] = "DRUID",
		["Hunter"] = "HUNTER",
		["Mage"] = "MAGE",
		["Paladin"] = "PALADIN",
		["Priest"] = "PRIEST",
		["Shaman"] = "SHAMAN",
		["Rogue"] = "ROGUE",
		["Warlock"] = "WARLOCK",
		["Warrior"] = "WARRIOR",
		["Monk"] = "MONK",
	}

	local ORDERED_CLASS_TYPES = {
		"Death Knight",
		"Druid",
		"Hunter",
		"Mage",
		"Paladin",
		"Priest",
		"Shaman",
		"Rogue",
		"Warlock",
		"Warrior",
		"Monk",
	}

	local FS = private.FILTER_STRINGS

	-- Flag data for printing. Wiped and re-used.
	local missing_flags = {}
	local extra_flags = {}
	local general_issues = {}

	local ACHIEVEMENT_LOOK_UP = {
		["Menagerie"] = 5877,
		["Petting Zoo"] = 5876,
		["WoW's 4th Anniversary"] = 2398,
		["Littlest Pet Shop"] = 5875,
		["Going to Need More Leashes"] = 7500,
		["Higher Learning"] = 1956,
		["Lil' Game Hunter"] = 2516,
		["WoW's 5th Anniversary"] = 4400,
		["Rock Lover"] = 5449,
		["Looking For Multitudes"] = 4478,
		["Shop Smart, Shop Pet...Smart"] = 1250,
		["Ling-Ting's Herbal Journey"] = 6402,
		["Time To Open a Pet Store"] = 7521,
		["Jade Tiger"] = 3636,
		["Pro Pet Mob"] = 6582,
		["That's a Lot of Pet Food"] = 7501,
	}

	-- Copy/pasta from Utilities.lua
	local function TableKeyFormat(input)
		if not input then
			return ""
		end
		return input:upper():gsub(" ", "_"):gsub("'", ""):gsub(":", ""):gsub("-", "_"):gsub("%(", ""):gsub("%)", "")
	end

	local output = private.TextDump

	function addon:UpdatePet(c_id)
		addon:ScanCompanionCreature(c_id)
		addon:DumpPet(c_id)
	end

	function addon:ScanCompanionCreature(c_id)

		-- Clear all the filters showing all pets known/unknown
		C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true)
		C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false)
		C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true)
		C_PetJournal.AddAllPetTypesFilter()
		C_PetJournal.AddAllPetSourcesFilter()

		local num_pets = _G.C_PetJournal.GetNumPets(_G.PetJournal.isWild)

		for pet_index = 1, num_pets do
			local pet_id, _, _, _, _, _, _, name, icon, pet_type, creature_id, source_text, description, is_wild, can_battle = _G.C_PetJournal.GetPetInfoByIndex(pet_index, false)
			if c_id == creature_id then
				addon:ScanSpecificCompanion(pet_index)
				break
			end
		end
	end

	function addon:DumpPet(creature_id)
		addon:InitializeCollection("CRITTER")

		local output = private.TextDump

		local pet_list = private.collectable_list["CRITTER"]
		local pet = pet_list[creature_id]

		if pet then
			output:Clear()
			pet:Dump()
			output:Display()
		end
	end

	local ZM = private.ZONE_NAME_MAP

	local function PetConditions(collectable, source_text)
		local time_of_day = source_text:match("Time:%s+(.+)")

		if time_of_day then
			collectable:SetTimeOfDay(private.TIME_OF_DAY[TableKeyFormat(time_of_day)])
		end
		local weather = source_text:match("Weather:%s+(.+)")

		if weather then
			collectable:SetWeather(private.WEATHER[TableKeyFormat(weather)])
		end
		local season = source_text:match("Season:%s+(.+)")

		if season then
			collectable:SetSeason(private.SEASON[TableKeyFormat(season)])
		end
	end

	local function PetWorldDrops(collectable, source_text)
		collectable:AddFilters(F.WORLD_DROP)
		for token in source_text:gmatch("([^,]+)[,%s]*") do
			-- Deal with Blizzard
			if ZM[token] then
				token = ZM[token]
			end

			-- TODO: Deal with weather/time of day/time of year
			token = token:gsub("Season: (%a+)", ""):gsub("Weather: (.+)", ""):gsub("Time: (.+)", ""):gsub("Morning(%a+)", ""):trim()
			token = TableKeyFormat(token)
			if (Z[token]) then
				collectable:AddWorldDrop(Z[token])
			else
				addon:Print("Zone: " .. token .. " not found.")
				print(source_text)
			end
		end
	end

	function addon:ScanSpecificCompanion(pet_index)
		addon:InitializeCollection("CRITTER")

		local vendor_list = private.vendor_list

		local pet_list = private.collectable_list["CRITTER"]
		local pet_id, _, _, _, _, _, _, name, icon, pet_type, creature_id, source_text, description, is_wild, can_battle = _G.C_PetJournal.GetPetInfoByIndex(pet_index, false)
		local flag_list = {}

		local pet = pet_list[creature_id]
		--print("Scanning: " .. name .. " " .. creature_id)
		if not pet then
			addon:Print("Found CRITTER not in database: " .. name .. " (" .. creature_id .. ") -- You will manually have to add this collectable into the database.")
			return
		end

		local quality = "Q." .. Q[pet.quality]

		-- Strip formatting
		source_text = source_text:gsub("%|c%x%x%x%x%x%x%x%x", ""):gsub("%|[r|t|T]", ""):gsub("%|n", "")

		PetConditions(pet, source_text)
		if source_text:match("Pet Battle:") then
			pet:AddFilters(F.ALLIANCE, F.HORDE)
			pet:SetMiscFilterType("PET_CAPTURABLE")

			if source_text:match("Faction:") then
				local faction, rep_level = source_text:match("Faction: ([%a ]+)[( -]+(%a+)[ )]?")
				source_text = source_text:gsub("Faction: ([%a ]+)[( -]+(%a+)[ )]?", "")
			end

			if source_text:match("Event") then
				pet:AddFilters(F.WORLD_EVENTS)
				local event = TableKeyFormat(source_text:match("Event:%s+(.+)"))
				pet:AddWorldEvent(event)
				source_text = source_text:gsub("Event:%s+(.+)", "")
			end
			source_text = source_text:gsub("Pet Battle:", "", 1):gsub("Pet Battle:", ","):trim() -- Blizzard uses different formats for Pet Battles, some are just listed others have Pet Battle before each zone
			PetWorldDrops(pet, source_text)
		elseif source_text:match("Achievement:") then
			pet:AddFilters(F.ACHIEVEMENT)

			source_text = source_text:gsub("Achievement: ", ""):gsub("Category: (.+)", ""):trim()
			if ACHIEVEMENT_LOOK_UP[source_text] then
				pet:AddAchievement(ACHIEVEMENT_LOOK_UP[source_text])
			else
				addon:Print("Unknown achievement found: " .. source_text)
			end
		elseif source_text:match("Profession:") then
			source_text = source_text:gsub("Profession: ", ""):trim()
			pet:AddFilters(F.PROFESSION)
			if source_text:match("Zone") then
				pet:AddProfession(PROF.FISHING)
			elseif source_text:match("Formula") then
				pet:AddProfession(PROF.ENCHANTING)
			elseif source_text:match("Imperial Silk") then

			else
				pet:AddProfession(PROF[string.upper(source_text:gsub(" ", "_"))])
			end
		elseif source_text:match("Fishing:") then -- Fuck blizzard
			pet:AddProfession(PROF.FISHING)
		elseif source_text:match("World Event:") then
			pet:AddFilters(F.WORLD_EVENTS)
			source_text = source_text:gsub("World Event: ", "")
			if source_text:match("Vendor") then
				local vendor_name_list
				if source_text:match("Vendor: (.-)Zone") then
					vendor_name_list = ""
					local index = 1
					for vendor_name, i in source_text:gmatch("Vendor: (.-)Zone") do
						if index == 1 then
							vendor_name_list = vendor_name
						else
							vendor_name_list = vendor_name_list .. ", " .. vendor_name
						end
						index = index + 1
					end
					for vendor_name in vendor_name_list:gmatch("([^,]+)[,%s]*") do
						local vendor_id
						for i, k in pairs(vendor_list) do
							local vendor = vendor_list[i]
							if vendor.name == vendor_name then
								vendor_id = i
								break
							end
						end
						if vendor_id then
							if vendor_list[vendor_id].faction == "Alliance" then
								pet:AddFilters(F.ALLIANCE)
							elseif vendor_list[vendor_id].faction == "Horde" then
								pet:AddFilters(F.HORDE)
							elseif vendor_list[vendor_id].faction == "Neutral" then
								pet:AddFilters(F.ALLIANCE, F.HORDE)
							end
							pet:AddVendor(vendor_id)
						elseif vendor_name then
							addon:Print("Vendor: " .. vendor_name .. " not in database.")
						end
					end
				elseif source_text:match("Vendor: (.+)Cost:") then
					vendor_name_list = ""
					local index = 1
					for vendor_name, i in source_text:gmatch("Vendor: (.-)Cost") do
						if index == 1 then
							vendor_name_list = vendor_name
						else
							vendor_name_list = vendor_name_list .. ", " .. vendor_name
						end
						index = index + 1
					end
					for vendor_name in vendor_name_list:gmatch("([^,]+)[,%s]*") do
						local vendor_id
						for i, k in pairs(vendor_list) do
							local vendor = vendor_list[i]
							if vendor.name == vendor_name then
								vendor_id = i
								break
							end
						end
						if vendor_id then
							if vendor_list[vendor_id].faction == "Alliance" then
								pet:AddFilters(F.ALLIANCE)
							elseif vendor_list[vendor_id].faction == "Horde" then
								pet:AddFilters(F.HORDE)
							elseif vendor_list[vendor_id].faction == "Neutral" then
								pet:AddFilters(F.ALLIANCE, F.HORDE)
							end
							pet:AddVendor(vendor_id)
						elseif vendor_name then
							addon:Print("Vendor: " .. vendor_name .. " not in database.")
						end
					end
				end
			end
			source_text = TableKeyFormat(source_text:gsub("Vendor: (.+)", ""):trim())
			pet:AddWorldEvent(source_text)
		elseif source_text:match("Quest:") then
			source_text = source_text:gsub("Quest: ", ""):trim()
			pet:AddFilters(F.QUEST)
			local quest_name, quest_zone = source_text:match("(%a+%s*%a*)Zone: (%a+%s*%a*)")
			-- TODO: How do I get the quest ID from a quest name?
			--output:AddLine("--pet:AddQuest()")
		elseif source_text:match("Vendor:") then -- Blizzard has no space after the : here
			if source_text:match("Faction:") then
				-- TODO: Fix pattern so I don't have to trim it.
				local vendor_name, faction, rep_level = source_text:match("Vendor: ([%a ]+)Zone: .+Faction: ([%a ]+)[ -]+(%a+)Cost")
				local vendor_id

				for i, k in pairs(vendor_list) do
					local vendor = vendor_list[i]
					if vendor.name == vendor_name then
						vendor_id = i
						break
					end
				end

				if vendor_id then
					if vendor_list[vendor_id].faction == "Alliance" then
						pet:AddFilters(F.ALLIANCE)
					elseif vendor_list[vendor_id].faction == "Horde" then
						pet:AddFilters(F.HORDE)
					elseif vendor_list[vendor_id].faction == "Neutral" then
						pet:AddFilters(F.ALLIANCE, F.HORDE)
					end
				elseif vendor_name then
					addon:Print("Vendor: " .. vendor_name .. " not in database. Manually add it to the Vendor.lua file and rescan this collectable.")
				end

				if faction then
					local temp_id = vendor_id or 0
					local formatted_faction = TableKeyFormat(faction:trim())
					local formatted_rep_level = string.upper(rep_level)
					pet:AddFilters(F[formatted_faction])
					pet:AddRepVendor(FAC[formatted_faction], REP[formatted_rep_level], temp_id)
				end
				pet:AddFilters(F.REPUTATION)
			else
				-- World Vendors are special names for the guild vendors.
				-- TODO: Deal with these special vendors.
				if source_text:match("World Vendors") then
					-- pet:AddRepVendor(FAC.GUILD, REP.EXALTED, 51512, 52268, 46602, 51495, 51504) -- ally
					-- pet:AddRepVendor(FAC.GUILD, REP.EXALTED, 46572, 51496, 51503, 51512, 52268) -- horde
				elseif source_text:match("Lovely Merchant") then
					pet:AddCustom("CITY")
				else
					local vendor_name_list
					-- Blizzard has two formats for vendors.  I have not seen a case where they intermingle yet.
					-- Format 1: Vendor: XZone: YCost: Z
					if source_text:match("Vendor: (.-)Zone") then
						vendor_name_list = ""
						local index = 1
						for vendor_name, i in source_text:gmatch("Vendor: (.-)Zone") do
							if index == 1 then
								vendor_name_list = vendor_name
							else
								vendor_name_list = vendor_name_list .. ", " .. vendor_name
							end
							index = index + 1
						end
						for vendor_name in vendor_name_list:gmatch("([^,]+)[,%s]*") do
							local vendor_id
							for i, k in pairs(vendor_list) do
								local vendor = vendor_list[i]
								if vendor.name == vendor_name then
									vendor_id = i
									break
								end
							end
							if vendor_id then
								if vendor_list[vendor_id].faction == "Alliance" then
									pet:AddFilters(F.ALLIANCE)
								elseif vendor_list[vendor_id].faction == "Horde" then
									pet:AddFilters(F.HORDE)
								elseif vendor_list[vendor_id].faction == "Neutral" then
									pet:AddFilters(F.ALLIANCE, F.HORDE)
								end
								pet:AddVendor(vendor_id)
							elseif vendor_name then
								addon:Print("Vendor: " .. vendor_name .. " not in database.")
							end
						end
						-- Format 2: Vendor: XCost: Z
					elseif source_text:match("Vendor: (.+)Cost:") then
						print(pet.name)
						vendor_name_list = ""
						local index = 1
						for vendor_name, i in source_text:gmatch("Vendor: (.-)Cost") do
							if index == 1 then
								vendor_name_list = vendor_name
							else
								vendor_name_list = vendor_name_list .. ", " .. vendor_name
							end
							index = index + 1
						end
						for vendor_name in vendor_name_list:gmatch("([^,]+)[,%s]*") do
							local vendor_id
							for i, k in pairs(vendor_list) do
								local vendor = vendor_list[i]
								if vendor.name == vendor_name then
									vendor_id = i
									break
								end
							end
							if vendor_id then
								if vendor_list[vendor_id].faction == "Alliance" then
									pet:AddFilters(F.ALLIANCE)
								elseif vendor_list[vendor_id].faction == "Horde" then
									pet:AddFilters(F.HORDE)
								elseif vendor_list[vendor_id].faction == "Neutral" then
									pet:AddFilters(F.ALLIANCE, F.HORDE)
								end
								pet:AddVendor(vendor_id)
							elseif vendor_name then
								addon:Print("Vendor: " .. vendor_name .. " not in database.")
							end
						end
					end
				end
				pet:AddFilters(F.VENDOR)
			end
		elseif source_text:match("Drop:") then -- Blizzard has no space after the : here
			local mob_name, mob_zone = source_text:match("Drop:*%s+(.+)Zone:%s+(.+)")
			if mob_name == "World Drop" then
				PetWorldDrops(pet, mob_zone)
			else
				-- TODO: How do we get the id from a name?
				pet:AddFilters(F.MOB_DROP)
				--output:AddLine("--pet:AddMobDrop() -- " .. (mob_name or source_text) .. ": " .. (mob_zone or "Unknown"))
			end
		elseif source_text:match("Promotion:") then
			source_text = source_text:gsub("Promotion:", ""):trim()
			if source_text:match("World of Warcraft Collectors Edition") or source_text:match("World of Warcraft Collector's Edition") or source_text:match("Wrath of the Lich King Collector's Edition") or source_text:match("Cataclysm Collector's Edition") or source_text:match("Mists of Pandaria Collector's Edition") or source_text:match("Burning Crusade Collector's Edition") then -- Seriously fuck blizzard
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.COLLECTORS_EDITION, F.IBOP)
				pet:AddCustom("CE")
			elseif source_text:match("BlizzCon") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
				pet:AddCustom("BLIZZCON")
			elseif source_text:match("Starcraft") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
				pet:AddCustom("STARCRAFTCE")
			elseif source_text:match("Diablo") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
				pet:AddCustom("DIABLOCE")
			elseif source_text:match("Authenticator Account Link") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
				pet:AddCustom("AUTH")
			elseif source_text:match("World Wide Invitational") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
				pet:AddCustom("WWI")
			elseif source_text:match("China New Year's Celebration") or source_text:match("Chinese New Years") or source_text:match("China Billing Promotion") or source_text:match("iCoke") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP)
				pet:AddCustom("CHINA")
			elseif source_text:match("PVP") or source_text:match("Arena") then
				pet:AddFilters(F.ALLIANCE, F.HORDE, F.PROMO, F.IBOP, F.PVP)
				--- -output:AddLine("pet:AddCustom()")
			else
				addon:Print("Unknown Promotion: " .. source_text)
				--pet:AddCustom(TableKeyFormat(source_text))
			end
		elseif source_text:match("Pet Store") then
			source_text = source_text:gsub("Pet Store", ""):trim()
			pet:AddFilters(F.STORE)
			--print(source_text)
		elseif source_text:match("Trading Card Game:") then
			pet:AddFilters(F.TCG)
			pet:AddCustom("TCG")
		else
			addon:Print("Unknown acquire method; " .. source_text)
		end
	end

	function addon:ScanCompanions()

		-- Clear all the filters showing all pets known/unknown
		C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true)
		C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false)
		C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true)
		C_PetJournal.AddAllPetTypesFilter()
		C_PetJournal.AddAllPetSourcesFilter()

		addon:InitializeCollection("CRITTER")

		output:Clear()

		local num_pets = _G.C_PetJournal.GetNumPets(_G.PetJournal.isWild)

		for pet_index = 1, num_pets do
			addon:ScanSpecificCompanion(pet_index, true)
		end

		local output = private.TextDump

		local pet_list = private.collectable_list["CRITTER"]

		output:Clear()

		for pet in pairs(pet_list) do
			if pet_list[pet] then
				pet_list[pet]:Dump()
			end
		end
		output:Display()
	end
end
