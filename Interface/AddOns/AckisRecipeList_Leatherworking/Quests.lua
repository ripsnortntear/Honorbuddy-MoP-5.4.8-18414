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

	AddQuest(769,	Z.THUNDER_BLUFF,		44.1,	44.6,	"Horde")
	AddQuest(1582,	Z.DARNASSUS,			60.5,	37.3,	"Alliance")
	AddQuest(7493,	Z.ORGRIMMAR,			51.0,	76.5,	"Horde")
	AddQuest(7497,	Z.STORMWIND_CITY,		67.2,	85.5,	"Alliance")

	self.InitializeQuests = nil
end
