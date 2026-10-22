-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-------------------------------------------------------------------------------
-- Module namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local addon = private.addon
local module = addon:GetModule(private.module_name)
local L = _G.LibStub("AceLocale-3.0"):GetLocale(addon.constants.addon_name)

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeDiscoveries()
	local function AddDiscovery(identifier, location, coord_x, coord_y, faction)
		addon.AcquireTypes.Discovery:AddEntity(identifier, L[identifier], location, coord_x, coord_y, faction)
	end

	AddDiscovery("DISCOVERY_INSC_BOOK")
	AddDiscovery("DISCOVERY_INSC_MINOR")
	AddDiscovery("DISCOVERY_INSC_NORTHREND")
	AddDiscovery("DISCOVERY_INSC_PANDARIA")

	self.InitializeDiscoveries = nil
end
