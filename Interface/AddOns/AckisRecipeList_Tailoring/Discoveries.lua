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
local L = _G.LibStub("AceLocale-3.0"):GetLocale(addon.constants.addon_name)

local BN = constants.BOSS_NAMES
local Z = constants.ZONE_NAMES

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeDiscoveries()
	local function AddDiscovery(identifier, location, coord_x, coord_y, faction)
		addon.AcquireTypes.Discovery:AddEntity(identifier, L[identifier], location, coord_x, coord_y, faction)
	end

	AddDiscovery("DISCOVERY_TAILOR_PANDARIA")
	AddDiscovery("DISCOVERY_TAILOR_CELESTIAL_PANDARIA")

	self.InitializeDiscoveries = nil
end
