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
local MODULE_NAME = "Jewelcrafting"
private.module_name = MODULE_NAME

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon("Ackis Recipe List")
private.addon = addon

local module = addon:NewModule(MODULE_NAME)

module.ITEM_FILTER_TYPES = {
	JEWELCRAFTING_CREATED_ITEM = true,
	JEWELCRAFTING_FIST_WEAPON = true,
	JEWELCRAFTING_HEAD = true,
	JEWELCRAFTING_MATERIALS = true,
	JEWELCRAFTING_NECK = true,
	JEWELCRAFTING_RING = true,
	JEWELCRAFTING_TRINKET = true,
	JEWELCRAFTING_GEM_BLUE = true,
	JEWELCRAFTING_GEM_GREEN = true,
	JEWELCRAFTING_GEM_META = true,
	JEWELCRAFTING_GEM_ORANGE = true,
	JEWELCRAFTING_GEM_PRISMATIC = true,
	JEWELCRAFTING_GEM_PURPLE = true,
	JEWELCRAFTING_GEM_RED = true,
	JEWELCRAFTING_GEM_YELLOW = true,
	JEWELCRAFTING_MOUNT = true,
	JEWELCRAFTING_PET = true,
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
	self:InitializeTrainers()
	self:InitializeVendors()
	self:InitializeRecipes()
end
