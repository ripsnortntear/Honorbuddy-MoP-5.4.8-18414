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

	AddQuest(6032,	Z.FELWOOD,			65.7,	2.9,	"Neutral")

	self.InitializeQuests = nil
end
