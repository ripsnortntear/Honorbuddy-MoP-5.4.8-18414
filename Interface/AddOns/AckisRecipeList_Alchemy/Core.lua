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
local MODULE_NAME = "Alchemy"
private.module_name = MODULE_NAME

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon("Ackis Recipe List")
private.addon = addon

local module = addon:NewModule(MODULE_NAME)

module.ITEM_FILTER_TYPES = {
	ALCHEMY_CAULDRON = true,
	ALCHEMY_ELIXIR = true,
	ALCHEMY_FLASK = true,
	ALCHEMY_MISC = true,
	ALCHEMY_OIL = true,
	ALCHEMY_POTION = true,
	ALCHEMY_TRANSMUTE = true,
	ALCHEMY_TRINKET = true,
	ALCHEMY_MOUNT = true,
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
	self:InitializeDiscoveries()
	self:InitializeMobDrops()
	self:InitializeQuests()
	self:InitializeTrainers()
	self:InitializeVendors()
	self:InitializeRecipes()
end
