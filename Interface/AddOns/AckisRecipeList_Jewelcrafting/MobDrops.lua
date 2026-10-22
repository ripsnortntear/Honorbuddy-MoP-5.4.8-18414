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

local BB = _G.LibStub("LibBabble-Boss-3.0"):GetLookupTable()
local BN = constants.BOSS_NAMES
local Z = constants.ZONE_NAMES

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeMobDrops()
	local function AddMob(mob_id, mob_name, zone_name, coord_x, coord_y)
		addon.AcquireTypes.MobDrop:AddEntity(mob_id, mob_name, zone_name, coord_x, coord_y, nil)
	end

	AddMob(8983,	BN.GOLEM_LORD_ARGELMACH,		Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(17722,	L["Coilfang Sorceress"],		Z.THE_STEAMVAULT,		0, 0)
	AddMob(18096,	BN.EPOCH_HUNTER,			Z.OLD_HILLSBRAD_FOOTHILLS,	0, 0)
	AddMob(18422,	L["Sunseeker Botanist"],		Z.THE_BOTANICA,			0, 0)
	AddMob(18472,	BN.DARKWEAVER_SYTH,			Z.SETHEKK_HALLS,		0, 0)
	AddMob(18866,	L["Mageslayer"],			Z.NETHERSTORM,			55.5, 85.5)
	AddMob(19768,	L["Coilskar Siren"],			Z.SHADOWMOON_VALLEY,		46.5, 30.0)
	AddMob(19826,	L["Dark Conclave Shadowmancer"],	Z.SHADOWMOON_VALLEY,		37.5, 29.0)
	AddMob(19984,	L["Vekh'nir Dreadhawk"],		Z.BLADES_EDGE_MOUNTAINS,	78.0, 74.3)
	AddMob(23954,	BN.INGVAR_THE_PLUNDERER,		Z.UTGARDE_KEEP,			0, 0)
	AddMob(26861,	BN.KING_YMIRON,			        Z.UTGARDE_PINNACLE,		0, 0)
	AddMob(27656,	BN.LEY_GUARDIAN_EREGOS,			Z.THE_OCULUS,			0, 0)
	AddMob(28379,	L["Shattertusk Mammoth"],		Z.SHOLAZAR_BASIN,		53.5, 24.4)
	AddMob(28851,	L["Enraged Mammoth"],			Z.ZULDRAK,			72.0, 41.1)
	AddMob(28923,	BN.LOKEN,				Z.HALLS_OF_LIGHTNING,		0, 0)
	AddMob(29311,	BN.HERALD_VOLAZJ,			Z.AHNKAHET_THE_OLD_KINGDOM,	0, 0)
	AddMob(29370,	L["Stormforged Champion"],		Z.THE_STORM_PEAKS,		26.1, 47.5)
	AddMob(29376,	L["Stormforged Artificer"],		Z.THE_STORM_PEAKS,		31.5, 44.2)
	AddMob(29402,	L["Ironwool Mammoth"],			Z.THE_STORM_PEAKS,		36.0, 83.5)
	AddMob(29792,	L["Frostfeather Screecher"],		Z.THE_STORM_PEAKS,		33.5, 65.5)
	AddMob(29793,	L["Frostfeather Witch"],		Z.THE_STORM_PEAKS,		33.0, 66.8)
	AddMob(30208,	L["Stormforged Ambusher"],		Z.THE_STORM_PEAKS,		70.3, 57.5)
	AddMob(30222,	L["Stormforged Infiltrator"],		Z.THE_STORM_PEAKS,		58.5, 63.2)
	AddMob(30260,	L["Stoic Mammoth"],			Z.THE_STORM_PEAKS,		54.8, 64.9)
	AddMob(30448,	L["Plains Mammoth"],			Z.THE_STORM_PEAKS,		66.1, 45.6)

	self.InitializeMobDrops = nil
end
