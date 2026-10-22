-----------------------------------------------------------------------
-- Upvalued Lua API.
-----------------------------------------------------------------------
local _G = getfenv(0)

-----------------------------------------------------------------------
-- AddOn namespace.
-----------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name, true)

local Z = private.ZONE_NAMES

private.trainer_list = {}

function private:AddTrainer(id_num, trainer_name, zone_name, coord_x, coord_y, faction)
	if _G.type(trainer_name) == "number" then
		local entry = self:AddListEntry(self.trainer_list, id_num, _G.GetSpellInfo(trainer_name), zone_name, coord_x, coord_y, faction)
		entry.spell_id = trainer_name
	else
		self:AddListEntry(self.trainer_list, id_num, L[trainer_name], zone_name, coord_x, coord_y, faction)
	end
end

function addon:InitTrainer()

	private:AddTrainer(461,		L["Demisette Cloyce"],		Z.STORMWIND_CITY,	39.6,	84.4, "Alliance")
	private:AddTrainer(906,		L["Maximillian Crowe"],		Z.ELWYNN_FOREST,	44.4,	66.0, "Alliance")
	private:AddTrainer(927,		L["Brother Wilhelm"],		Z.ELWYNN_FOREST,	41.0,	65.8, "Alliance")
	private:AddTrainer(928,		L["Lord Grayson Shadowbreaker"],Z.STORMWIND_CITY,	52.6,	45.0, "Alliance")
	private:AddTrainer(3172,	L["Dhugru Gorelust"],		Z.DUROTAR,		54.2,	41.2, "Horde")
	private:AddTrainer(1232,	L["Azar Stronghammer"],		Z.DUN_MOROGH,		54.6,	50.4, "Alliance")
	private:AddTrainer(2127,	L["Rupert Boch"],		Z.TIRISFAL_GLADES,	61.0,	50.4, "Horde")
	private:AddTrainer(3324,	L["Grol'dar"],			Z.ORGRIMMAR,		52.8,	38.6, "Horde")
	private:AddTrainer(3325,	L["Mirket"],			Z.ORGRIMMAR,		53.6,	40.6, "Horde")
	private:AddTrainer(3326,	L["Zevrost"],			Z.ORGRIMMAR,		49.0,	55.2, "Horde")
	private:AddTrainer(4565,	L["Richard Kerwin"],		Z.UNDERCITY,		88.6,	16.0, "Horde")
	private:AddTrainer(5148,	L["Beldruk Doombrow"],		Z.IRONFORGE,		24.6,	 5.6, "Alliance")
	private:AddTrainer(5149,	L["Brandur Ironhammer"],	Z.IRONFORGE,		24.0,	 6.8, "Alliance")
	private:AddTrainer(5171,	L["Thistleheart"],		Z.IRONFORGE,		50.6,	 6.6, "Alliance")
	private:AddTrainer(5172,	L["Briarthorn"],		Z.IRONFORGE,		51.0,	 5.2, "Alliance")
	private:AddTrainer(5491,	L["Arthur the Faithful"],	Z.STORMWIND_CITY,	52.6,	45.4, "Alliance")
	private:AddTrainer(5492,	L["Katherine the Pure"],	Z.STORMWIND_CITY,	52.6,	45.0, "Alliance")
	private:AddTrainer(5495,	L["Ursula Deline"],		Z.STORMWIND_CITY,	40.0,	84.2, "Alliance")
	private:AddTrainer(5496,	L["Sandahl"],			Z.STORMWIND_CITY,	38.9,	85.4, "Alliance")
	private:AddTrainer(6251,	L["Strahad Farsan"],		Z.NORTHERN_BARRENS,	68.2,	67.8, "Neutral")
	private:AddTrainer(8664,	L["Sunwalker Saern"],		Z.THUNDER_BLUFF,	63.0,	79.8, "Horde")
	private:AddTrainer(16681,	L["Champion Bachi"], 		Z.SILVERMOON_CITY, 	92.0,	37.6, "Horde")
	private:AddTrainer(17483,	L["Tullas"], 			Z.AZUREMYST_ISLE, 	48.4,	49.6, "Alliance")
	private:AddTrainer(17844,	L["Vindicator Aesom"], 		Z.BLOODMYST_ISLE, 	55.6,	55.6, "Alliance")
	private:AddTrainer(20406,	L["Champion Cyssa Dawnrose"], 	Z.UNDERCITY,	 	57.8,	90.8, "Horde")
	private:AddTrainer(23128,	L["Master Pyreanor"], 		Z.ORGRIMMAR,	 	49.2,	71.2, "Horde")
	private:AddTrainer(35778,	L["Evol Fingers"], 		Z.THE_LOST_ISLES, 	28.4,	75.6, "Horde")
	private:AddTrainer(36519,	L["Evol Fingers"], 		Z.THE_LOST_ISLES, 	36.0,	67.2, "Horde")
	private:AddTrainer(43013,	L["Sunwalker Iopi"], 		Z.MULGORE,	 	48.6,	58.8, "Horde")
	private:AddTrainer(44725,	L["Sunwalker Atohmo"], 		Z.ORGRIMMAR,	 	45.2,	53.6, "Horde")
	private:AddTrainer(45720,	L["Kazrali the Witch"], 	Z.ORGRIMMAR,	 	40.6,	80.2, "Horde")
	private:AddTrainer(49793,	L["Faldoc Stonefaith"],		Z.LOCH_MODAN,		35.4,	48.4, "Alliance")
	private:AddTrainer(49998,	L["Gusini"],			Z.DUROTAR, 		56.2,	73.6, "Horde")
	private:AddTrainer(50012,	L["Blood Knight Argaron"],	Z.SHATTRATH_CITY, 	52.8,	52.6, "Horde")
	private:AddTrainer(50023,	L["Ordo"], 			Z.SHATTRATH_CITY, 	55.8,	37.2, "Alliance")
	private:AddTrainer(50028,	L["Chintoka"], 			Z.NORTHERN_BARRENS, 	49.8,	58.6, "Horde")
	private:AddTrainer(50035,	L["Sunwalker Lonaki"],		Z.NORTHERN_BARRENS, 	49.6,	61.2, "Horde")
	private:AddTrainer(50142,	L["Summoner Calwen"], 		Z.DALARAN,	 	57.0,	27.8, "Horde")
	private:AddTrainer(50156,	L["Summoner Nolric"], 		Z.DALARAN,	 	38.6,	64.6, "Alliance")
	private:AddTrainer(50150,	L["Nelur Lightsown"], 		Z.DALARAN,	 	60.2,	25.8, "Horde")
	private:AddTrainer(50160,	L["Rulen Lightsreap"], 		Z.DALARAN,	 	39.6,	62.6, "Alliance")
	private:AddTrainer(50502,	L["Vitus Darkwalker"], 		Z.DARNASSUS,	 	49.8,	16.6, "Alliance")
	private:AddTrainer(50729,	L["Darba the Crone"],		Z.IRONFORGE,		52.0,	 7.0, "Alliance")
	private:AddTrainer(63014,	L["Marcus Jensen"],		Z.ELWYNN_FOREST,	40.2,	66.0, "Alliance")
	private:AddTrainer(63061,	L["Narzak"],			Z.DUROTAR,		52.6,	41.4, "Horde")
	private:AddTrainer(63067,	L["Naleen"],			Z.MULGORE,		49.2,	56.0, "Horde")
	private:AddTrainer(63070,	L["Valeena"],			Z.TELDRASSIL,		55.2,	51.2, "Alliance")
	private:AddTrainer(63083,	L["Will Larsons"],		Z.DARKSHORE,		50.2,	20.0, "Alliance")
	private:AddTrainer(63086,	L["Matty"],			Z.ORGRIMMAR,		37.6,	79.0, "Horde")
	private:AddTrainer(63596,	L["Audrey Burnhep"],		Z.STORMWIND_CITY,	69.6,	25.8, "Alliance")
	private:AddTrainer(63626,	L["Varzok"],			Z.ORGRIMMAR,		52.6,	58.8, "Horde")  -- Blizz thinks this is Varzog

	self.InitTrainer = nil
end
