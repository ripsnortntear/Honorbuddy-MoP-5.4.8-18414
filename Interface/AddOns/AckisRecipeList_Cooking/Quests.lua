-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-------------------------------------------------------------------------------
-- Module namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local addon = private.addon
local constants = addon.constants
local module = addon:GetModule(private.module_name)

local Z = constants.ZONE_NAMES

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeQuests()
	local function AddQuest(quest_id, zone_name, coord_x, coord_y, faction)
		addon.AcquireTypes.Quest:AddEntity(quest_id, nil, zone_name, coord_x, coord_y, faction)
	end

	AddQuest(384,	Z.DUN_MOROGH,			46.8,	52.5,	"Alliance")
	AddQuest(6610,	Z.TANARIS,			52.6,	29.0,	"Neutral")
	AddQuest(8313,	Z.SILITHUS,			43.6,	42.0,	"Neutral")
	AddQuest(9171,	Z.GHOSTLANDS,			48.3,	30.9,	"Horde")
	AddQuest(9356,	Z.HELLFIRE_PENINSULA,		49.2,	74.8,	"Neutral")
	AddQuest(9454,	Z.AZUREMYST_ISLE,		49.8,	51.9,	"Alliance")
	AddQuest(10860,	Z.BLADES_EDGE_MOUNTAINS,	76.1,	60.3,	"Horde")
	AddQuest(11377,	Z.SHATTRATH_CITY,		61.6,	16.5,	"Neutral")
	AddQuest(11379,	Z.SHATTRATH_CITY,		61.6,	16.5,	"Neutral")
	AddQuest(11380,	Z.SHATTRATH_CITY,		61.6,	16.5,	"Neutral")
	AddQuest(11381,	Z.SHATTRATH_CITY,		61.6,	16.5,	"Neutral")
	AddQuest(11666,	Z.TEROKKAR_FOREST,		38.7,	12.8,	"Neutral")
	AddQuest(11667,	Z.TEROKKAR_FOREST,		38.7,	12.8,	"Neutral")
	AddQuest(11668,	Z.TEROKKAR_FOREST,		38.7,	12.8,	"Neutral")
	AddQuest(11669,	Z.TEROKKAR_FOREST,		38.7,	12.8,	"Neutral")
	AddQuest(13087,	Z.HOWLING_FJORD,		58.2,	62.1,	"Alliance")
	AddQuest(13088,	Z.BOREAN_TUNDRA,		57.9,	71.5,	"Alliance")
	AddQuest(13089,	Z.HOWLING_FJORD,		78.7,	29.5,	"Horde")
	AddQuest(13090,	Z.BOREAN_TUNDRA,		42.0,	54.2,	"Horde")
	AddQuest(13100,	Z.DALARAN,			40.5,	65.8,	"Alliance")
	AddQuest(13101,	Z.DALARAN,			40.5,	65.8,	"Alliance")
	AddQuest(13102,	Z.DALARAN,			40.5,	65.8,	"Alliance")
	AddQuest(13103,	Z.DALARAN,			40.5,	65.8,	"Alliance")
	AddQuest(13107,	Z.DALARAN,			40.5,	65.8,	"Alliance")
	AddQuest(13112,	Z.DALARAN,			70.0,	38.6,	"Horde")
	AddQuest(13113,	Z.DALARAN,			70.0,	38.6,	"Horde")
	AddQuest(13114,	Z.DALARAN,			70.0,	38.6,	"Horde")
	AddQuest(13115,	Z.DALARAN,			70.0,	38.6,	"Horde")
	AddQuest(13116,	Z.DALARAN,			70.0,	38.6,	"Horde")
	AddQuest(13571,	Z.DALARAN,			0,	0,	"Neutral")
	AddQuest(26620,	Z.DUSKWOOD,			73.8,	43.6,	"Alliance")
	AddQuest(26623,	Z.DUSKWOOD,			73.8,	43.6,	"Alliance")
	AddQuest(26860,	Z.LOCH_MODAN,			34.9,	49.1,	"Alliance")
	AddQuest(33022, Z.VALLEY_OF_THE_FOUR_WINDS,	53.6,	51.2,	"Neutral")
	AddQuest(33024, Z.VALLEY_OF_THE_FOUR_WINDS,	53.6,	51.2,	"Neutral")
	AddQuest(33027, Z.VALLEY_OF_THE_FOUR_WINDS,	53.6,	51.2,	"Neutral")

	self.InitializeQuests = nil
end
