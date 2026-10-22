--[[
************************************************************************
Mob.lua
************************************************************************
File date: 2014-02-14T00:49:48Z
File hash: 16804c9
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/arl/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]]--

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
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)
local BB = LibStub("LibBabble-Boss-3.0"):GetLookupTable()

local BN = private.BOSS_NAMES
local Z = private.ZONE_NAMES

private.mob_list = {}

function addon:InitMob()
	local function AddMob(mob_id, mob_name, zone_name, coord_x, coord_y)
		private:AddListEntry(private.mob_list, mob_id, mob_name, zone_name, coord_x, coord_y, nil)
	end

	AddMob(2725,	L["Scalding Whelp"],			Z.BADLANDS,			69.8, 47.0)
	AddMob(3636,	L["Deviate Ravager"],			Z.WAILING_CAVERNS,		 0.0,  0.0)
	AddMob(3637,	L["Deviate Guardian"],			Z.WAILING_CAVERNS,		 0.0,  0.0)
	AddMob(4323,	L["Searing Hatchling"],			Z.DUSTWALLOW_MARSH,		46.0, 69.4)
	AddMob(4324,	L["Searing Whelp"],			Z.DUSTWALLOW_MARSH,		46.0, 69.4)
	AddMob(7049,	L["Flamescale Broodling"],		Z.BURNING_STEPPES,		15.4, 66.2)
	AddMob(9736,	BN.QUARTERMASTER_ZIGRIS,		Z.BLACKROCK_SPIRE,		 0.0,  0.0)
	AddMob(10184,	BB["Onyxia"],				Z.ONYXIAS_LAIR,			68.0, 35.2)
	AddMob(10596,	BN.MOTHER_SMOLDERWEB,			Z.BLACKROCK_SPIRE,		 0.0,  0.0)
	AddMob(11982,	BB["Magmadar"],				Z.MOLTEN_CORE,			71.0, 19.4)
	AddMob(11988,	BB["Golemagg the Incinerator"],		Z.MOLTEN_CORE,			68.6, 61.6)
	AddMob(12017,	BB["Broodlord Lashlayer"],		Z.BLACKWING_LAIR,		 0.0,  0.0)
	AddMob(12098,	BB["Sulfuron Harbinger"],		Z.MOLTEN_CORE,			82.6, 83.2)
	AddMob(12435,	BB["Razorgore the Untamed"],		Z.BLACKWING_LAIR,		 0.0,  0.0)
	AddMob(14020,	BB["Chromaggus"],			Z.BLACKWING_LAIR,		 0.0,  0.0)
	AddMob(15276,	BB["Emperor Vek'lor"],			Z.AHNQIRAJ_THE_FALLEN_KINGDOM,	61.4, 65.6)
	AddMob(15299,	BB["Viscidus"],				Z.AHNQIRAJ_THE_FALLEN_KINGDOM,	71.6, 18.4)
	AddMob(15550,	BB["Attumen the Huntsman"],		Z.KARAZHAN,			 0.0,  0.0)
	AddMob(15690,	BB["Prince Malchezaar"],		Z.KARAZHAN,			 0.0,  0.0)
	AddMob(15691,	BB["The Curator"],			Z.KARAZHAN,			 0.0,  0.0)
	AddMob(15932,	BB["Gluth"],				Z.NAXXRAMAS,			 0.0,  0.0)
	AddMob(15952,	BB["Maexxna"],				Z.NAXXRAMAS,			 0.0,  0.0)
	AddMob(15263,	BB["The Prophet Skeram"],		Z.AHNQIRAJ_THE_FALLEN_KINGDOM,	44.6, 45.6)
	AddMob(15688,	BB["Terestian Illhoof"],		Z.KARAZHAN,			 0.0,  0.0)
	AddMob(16011,	BB["Loatheb"],				Z.NAXXRAMAS,			 0.0,  0.0)
	AddMob(17521,	BB["The Big Bad Wolf"],			Z.KARAZHAN,			 0.0,  0.0)
	AddMob(18805,	BB["High Astromancer Solarian"],	Z.TEMPEST_KEEP,			73.6, 50.0)
	AddMob(19514,	BB["Al'ar"],				Z.TEMPEST_KEEP,			46.0, 58.4)
	AddMob(19516,	BB["Void Reaver"],			Z.TEMPEST_KEEP,			27.2, 47.8)
	AddMob(19622,   BN.KAELTHAS_SUNSTRIDER,			Z.TEMPEST_KEEP,			50.0, 15.6)
	AddMob(20197,	L["Bogflare Needler"],			Z.ZANGARMARSH,			53.6, 33.4)
	AddMob(21212,	BB["Lady Vashj"],			Z.SERPENTSHRINE_CAVERN,		71.6, 59.0)
	AddMob(21213,	BB["Morogrim Tidewalker"],		Z.SERPENTSHRINE_CAVERN,		59.6, 25.8)
	AddMob(21216,	BB["Hydross the Unstable"],		Z.SERPENTSHRINE_CAVERN,		36.8, 84.8)
	AddMob(23035,	BN.ANZU,				Z.SETHEKK_HALLS,		32.6, 58.0)
	AddMob(23577,	BN.HALAZZI,				Z.ZULAMAN,			57.6, 23.6)
	AddMob(23682,	BB["Headless Horseman"],		Z.SCARLET_MONASTERY,		 0.0,  0.0)
	AddMob(23872,	BB["Coren Direbrew"],			Z.BLACKROCK_DEPTHS,		 0.0,  0.0)
	AddMob(24664,   BN.KAELTHAS_SUNSTRIDER,			Z.MAGISTERS_TERRACE,		 8.6, 50.6)
	AddMob(25740,	BB["Ahune"],				Z.THE_SLAVE_PENS,		32.2, 50.8)
	AddMob(26693,	BN.SKADI_THE_RUTHLESS,			Z.UTGARDE_PINNACLE,		69.8, 36.4)
	AddMob(28859,	BB["Malygos"],				Z.THE_EYE_OF_ETERNITY,		 0.0,  0.0)
	AddMob(28860,	BB["Sartharion"],			Z.THE_OBSIDIAN_SANCTUM,		 0.0,  0.0)
	AddMob(29334,	L["Gundrak Raptor"],			Z.ZULDRAK,			73.8, 17.6)
	AddMob(31125,	BB["Archavon the Stone Watcher"],	Z.VAULT_OF_ARCHAVON,		49.2, 17.0)
	AddMob(32273,	BB["Infinite Corruptor"],		Z.THE_CULLING_OF_STRATHOLME,	 0.0,  0.0)
	AddMob(32491,	L["Time-Lost Proto Drake"],		Z.THE_STORM_PEAKS,		 0.0,  0.0)
	AddMob(33288,	BB["Yogg-Saron"],			Z.ULDUAR,			 0.0,  0.0)
	AddMob(33993,	BB["Emalon the Storm Watcher"],		Z.VAULT_OF_ARCHAVON,		62.6, 55.4)
	AddMob(35013,	BB["Koralon the Flame Watcher"],	Z.VAULT_OF_ARCHAVON,		36.0, 55.4)
	AddMob(36296,	L["Apothecary Hummel"],			Z.SHADOWFANG_KEEP,		41.8, 53.6)
	AddMob(36597,	BB["The Lich King"],			Z.ICECROWN_CITADEL,		 0.0,  0.0)
	AddMob(38433,	BB["Toravon the Ice Watcher"],		Z.VAULT_OF_ARCHAVON,		62.6, 36.8)
	AddMob(34564,	BN.ANUBARAK,				Z.TRIAL_OF_THE_CRUSADER,	 0.0,  0.0)
	AddMob(39384,	L["Noxious Whelp"],			Z.FERALAS,			48.2, 10.6)
	AddMob(42042,	L["Ebon Whelp"],			Z.WETLANDS,			62.0, 49.8)
	AddMob(43214,	BN.SLABHIDE,				Z.THE_STONECORE,		36.2, 45.8)
	AddMob(43873,	BN.ALTAIRUS,				Z.THE_VORTEX_PINNACLE,		52.0, 80.6)
	AddMob(45412,	BN.LORD_AURIUS_RIVENDARE,		Z.STRATHOLME,			38.6, 20.2)
	AddMob(46753,	BN.ALAKIR,				Z.THRONE_OF_THE_FOUR_WINDS,	46.6, 50.2)
	AddMob(46914,	L["Raging Whelp"],			Z.BADLANDS,			29.6, 44.2)
	AddMob(46916,	L["Nyxondra's Broodling"],		Z.BADLANDS,			10.2, 41.6)
	AddMob(47676,	L["Baradin Fox"],			Z.TOL_BARAD_PENINSULA,		52.6, 59.0)
	AddMob(48522,	L["Defias Pirate"],			Z.THE_DEADMINES,		 0.0,  0.0)
	AddMob(49822,	L["Jadefang"],				Z.DEEPHOLM,			61.2, 22.6)
	AddMob(50005,	L["Poseidus"],				Z.VASHJIR,			 0.0,  0.0)
	AddMob(50062,	L["Aeonaxx"],				Z.DEEPHOLM,			 0.0,  0.0)
	AddMob(50245,	L["Dormus the Camel-Hoarder"],		Z.FERALAS,			69.6, 73.2)
	AddMob(50347,	L["Karr the Darkener"],			Z.DREAD_WASTES,			71.8, 37.6)
	AddMob(50358,	L["Haywire Sunreaver Construct"],	Z.ISLE_OF_THUNDER,		49.6, 89.2)
	AddMob(50776,	L["Nalash Verdantis"],			Z.DREAD_WASTES,			64.2, 58.6)
	AddMob(52059,	BN.HIGH_PRIESTESS_KILNARA,		Z.ZULGURUB,			47.8, 20.6)
	AddMob(52151,	BN.BLOODLORD_MANDOKIR,			Z.ZULGURUB,			60.6, 80.6)
	AddMob(52409,	BN.RAGNAROS,				Z.FIRELANDS,			 0.0,  0.0)
	AddMob(52530,	BN.ALYSRAZOR,				Z.FIRELANDS,			 0.0,  0.0)
	AddMob(55294,	BN.ULTRAXION,				Z.DRAGON_SOUL,			 0.0,  0.0)
	AddMob(56173,	BN.MADNESS_OF_DEATHWING,		Z.DRAGON_SOUL,			 0.0,  0.0)
	AddMob(58336,	L["Darkmoon Rabbit"],			Z.DARKMOON_ISLAND,		77.4, 80.2)
	AddMob(60410,	BN.ELEGON,				Z.MOGUSHAN_VAULTS,		20.6, 51.5)
	AddMob(60491,	BN.SHA_OF_ANGER, 			Z.KUN_LAI_SUMMIT,		67.8, 77.8)
	AddMob(62346,	L["Galleon"], 				Z.VALLEY_OF_THE_FOUR_WINDS,	70.8, 63.4)
	AddMob(64403,	L["Alani"], 				Z.VALE_OF_ETERNAL_BLOSSOMS,	39.4, 30.8)
	AddMob(66467,	L["G'nathus"], 				Z.TOWNLONG_STEPPES,		33.4,  6.4)
	AddMob(68476,	BN.HORRIDON,				Z.THRONE_OF_THUNDER,		 0.0,  0.0)
	AddMob(69017,	BN.PRIMORDIUS,				Z.THRONE_OF_THUNDER,		 0.0,  0.0)
	AddMob(69099,	L["Nalak"],				Z.ISLE_OF_THUNDER,		59.8, 39.4)
	AddMob(69161,	BN.OONDASTA,				Z.ISLE_OF_GIANTS,		50.6, 54.4)
	AddMob(69251,	L["Quivering Filth"],			Z.ISLE_OF_THUNDER,		52.0, 80.6)
	AddMob(69427,	BN.DARK_ANIMUS,				Z.THRONE_OF_THUNDER,		 0.0,  0.0)
	AddMob(69712,	BN.JI_KUN,				Z.THRONE_OF_THUNDER,		 0.0,  0.0)
	AddMob(69769,	L["Zandalari Warbringer"],		Z.PANDARIA,			 0.0,  0.0)  -- Slate
	AddMob(69808,	L["Sunreaver Construct"],		Z.ISLE_OF_THUNDER,		 0.0,  0.0)
	AddMob(69841,	L["Zandalari Warbringer"],		Z.PANDARIA,			 0.0,  0.0)  -- Amber
	AddMob(69842,	L["Zandalari Warbringer"],		Z.PANDARIA,			 0.0,  0.0)  -- Jade
	AddMob(69925,	L["Zandalari Dinomancer"],		Z.ISLE_OF_GIANTS,		 0.0,  0.0)
	AddMob(69983,	L["Primal Direhorn"],			Z.ISLE_OF_GIANTS,		 0.0,  0.0)
	AddMob(70013,	L["Primal Direhorn Hatchling"],		Z.ISLE_OF_GIANTS,		 0.0,  0.0)
	AddMob(71152,	BN.PARAGONS_OF_THE_KLAXXI,		Z.SIEGE_OF_ORGRIMMAR,		 0.0,  0.0)
	AddMob(71504,	BN.SIEGECRAFTER_BLACKFUSE,		Z.SIEGE_OF_ORGRIMMAR,		 0.0,  0.0)
	AddMob(71734,	BN.SHA_OF_PRIDE,			Z.SIEGE_OF_ORGRIMMAR,		 0.0,  0.0)
	AddMob(71826,	L["Scary Sprite"],			Z.TIMELESS_ISLE,		 0.0,  0.0)
	AddMob(71865,	BN.GARROSH_HELLSCREAM,			Z.SIEGE_OF_ORGRIMMAR,		 0.0,  0.0)
	AddMob(71919,	L["Zhu-Gon the Sour"],			Z.TIMELESS_ISLE,		 0.0,  0.0)
	AddMob(71992,	L["Moonfang"],				Z.DARKMOON_ISLAND,		39.6, 46.0)
	AddMob(72767,	L["Jademist Dancer"],			Z.TIMELESS_ISLE,		27.2, 31.5)
	AddMob(72775,	L["Bufo"],				Z.TIMELESS_ISLE,		 0.0,  0.0)
	AddMob(72909,	L["Gu'chi the Swarmling"],		Z.TIMELESS_ISLE,		41.6, 78.4)
	AddMob(73162,	L["Foreboding Flame"],			Z.TIMELESS_ISLE,		43.4, 40.2)
	AddMob(73163,	L["Imperial Python"],			Z.TIMELESS_ISLE,		44.4, 65.5)
	AddMob(73166,	L["Monstrous Spineclaw"],		Z.TIMELESS_ISLE,		 0.0,  0.0)
	AddMob(73167,	L["Huolon"],				Z.TIMELESS_ISLE,		72.6, 48.8)
	AddMob(73277,	L["Leafmender"],			Z.TIMELESS_ISLE,		67.3, 44.1)
	AddMob(73282,	L["Garnia"],				Z.TIMELESS_ISLE,		 0.0,  0.0)

	self.InitMob = nil
end
