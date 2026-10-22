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

local Z = constants.ZONE_NAMES

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeVendors()
	local function AddVendor(id_num, name, zone_name, x, y, faction)
		addon.AcquireTypes.Vendor:AddEntity(id_num, name, zone_name, x, y, faction)
	end

	AddVendor(28723,	L["Larana Drome"],			Z.DALARAN,			42.3,	37.5,	"Neutral")
	AddVendor(30734,	L["Jezebel Bican"],			Z.HELLFIRE_PENINSULA,		53.9,	65.5,	"Alliance")
	AddVendor(30735,	L["Kul Inkspiller"],			Z.HELLFIRE_PENINSULA,		52.5,	36.0,	"Horde")
	AddVendor(49703,	L["Casandra Downs"],			Z.TWILIGHT_HIGHLANDS,		78.6,	76.2,	"Alliance")
	AddVendor(50248,	L["Una Kobuna"],			Z.TWILIGHT_HIGHLANDS,		76.6,	49.4,	"Horde")
	AddVendor(73293,	L["Whizzig"],				Z.TIMELESS_ISLE,		42.8,	59.2,	"Neutral")

	self.InitializeVendors = nil
end
