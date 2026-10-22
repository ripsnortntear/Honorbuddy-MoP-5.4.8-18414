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

	AddQuest(9635,	Z.ZANGARMARSH,			34.0,	50.8,	"Horde")
	AddQuest(9636,	Z.ZANGARMARSH,			68.6,	50.2,	"Alliance")
	AddQuest(12889,	Z.THE_STORM_PEAKS,		37.7,	46.5,	"Neutral")

	self.InitializeQuests = nil
end
