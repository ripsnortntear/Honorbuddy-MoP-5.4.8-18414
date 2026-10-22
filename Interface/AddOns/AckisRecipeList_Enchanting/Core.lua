-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-- Functions
local pairs = _G.pairs

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...
local MODULE_NAME = "Enchanting"
private.module_name = MODULE_NAME

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon("Ackis Recipe List")
private.addon = addon

local module = addon:NewModule(MODULE_NAME)

module.ITEM_FILTER_TYPES = {
	ENCHANTING_BOOTS = true,
	ENCHANTING_BRACER = true,
	ENCHANTING_CHEST = true,
	ENCHANTING_CLOAK = true,
	ENCHANTING_GLOVES = true,
	ENCHANTING_RING = true,
	ENCHANTING_SHIELD = true,
	ENCHANTING_WEAPON = true,
	ENCHANTING_2H_WEAPON = true,
	ENCHANTING_STAFF = true,
	ENCHANTING_OIL = true,
	ENCHANTING_ROD = true,
	ENCHANTING_WAND = true,
	ENCHANTING_MISC = true,
}

function module:OnInitialize()
	local defaults = {
		profile = {
			filters = {
				item = {} -- Populated below.
			}
		}
	}

	for filter_name in pairs(self.ITEM_FILTER_TYPES) do
		defaults.profile.filters.item[filter_name:lower()] = true
		addon.constants.ITEM_FILTER_TYPES[filter_name] = true
	end

	self.db = addon.db:RegisterNamespace(MODULE_NAME, defaults)
end

function module:OnEnable()
	self:InitializeMobDrops()
	self:InitializeTrainers()
	self:InitializeVendors()
	self:InitializeRecipes()
end
