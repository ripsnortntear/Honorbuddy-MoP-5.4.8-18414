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

	AddQuest(1578,	Z.IRONFORGE,			48.5,	43.0,	"Alliance")
	AddQuest(1618,	Z.IRONFORGE,			48.5,	43.0,	"Alliance")
	AddQuest(2751,	Z.ORGRIMMAR,			78.0,	21.4,	"Horde")
	AddQuest(2752,	Z.ORGRIMMAR,			78.0,	21.4,	"Horde")
	AddQuest(2753,	Z.ORGRIMMAR,			78.0,	21.4,	"Horde")
	AddQuest(2754,	Z.ORGRIMMAR,			78.0,	21.4,	"Horde")
	AddQuest(2755,	Z.ORGRIMMAR,			78.0,	21.4,	"Horde")
	AddQuest(7604,	Z.BLACKROCK_DEPTHS,		0,	0,	"Neutral")

	self.InitializeQuests = nil
end
