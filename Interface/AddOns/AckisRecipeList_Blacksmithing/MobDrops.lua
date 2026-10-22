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

	AddMob(1844,	L["Foreman Marcrid"],			Z.EASTERN_PLAGUELANDS,		54.0, 68.0)
	AddMob(9028,	BB["Grizzle"],				Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(9543,	BN.RIBBLY_SCREWSPIGOT,			Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(9554,	L["Hammered Patron"],			Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(9596,	BB["Bannok Grimaxe"],			Z.BLACKROCK_SPIRE,		0, 0)
	AddMob(10043,	L["Ribbly's Crony"],			Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(10119,	L["Volchan"],				Z.BURNING_STEPPES,		22.0, 41.0)
	AddMob(10438,	BN.MALEKI_THE_PALLID,			Z.STRATHOLME,			0, 0)
	AddMob(10997,	BB["Willey Hopebreaker"],		Z.STRATHOLME,			0, 0)
	AddMob(10899,	BB["Goraluk Anvilcrack"],		Z.BLACKROCK_SPIRE,		0, 0)
	AddMob(15263,	BB["The Prophet Skeram"],		Z.AHNQIRAJ_THE_FALLEN_KINGDOM,	0, 0)
	AddMob(15340,	BB["Moam"],				Z.RUINS_OF_AHNQIRAJ,		0, 0)
	AddMob(16952,	L["Anger Guard"],			Z.BLADES_EDGE_MOUNTAINS,	72.0, 40.5)
	AddMob(17136,	L["Boulderfist Warrior"],		Z.NAGRAND,			51.0, 57.0)
	AddMob(17975,	BN.HIGH_BOTANIST_FREYWINN,		Z.THE_BOTANICA,			0, 0)
	AddMob(18203,	L["Murkblood Raider"],			Z.NAGRAND,			31.5, 43.5)
	AddMob(18314,	L["Nexus Stalker"],			Z.MANA_TOMBS,			0, 0)
	AddMob(18497,	L["Auchenai Monk"],			Z.AUCHENAI_CRYPTS,		0, 0)
	AddMob(18830,	L["Cabal Fanatic"],			Z.SHADOW_LABYRINTH,		0, 0)
	AddMob(18853,	L["Sunfury Bloodwarder"],		Z.NETHERSTORM,			27.0, 72.0)
	AddMob(18873,	L["Disembodied Protector"],		Z.NETHERSTORM,			31.8, 52.7)
	AddMob(20900,	L["Unchained Doombringer"],		Z.THE_ARCATRAZ,			0, 0)
	AddMob(21050,	L["Enraged Earth Spirit"],		Z.SHADOWMOON_VALLEY,		46.5, 45.0)
	AddMob(21059,	L["Enraged Water Spirit"],		Z.SHADOWMOON_VALLEY,		51.0, 25.5)
	AddMob(21060,	L["Enraged Air Spirit"],		Z.SHADOWMOON_VALLEY,		70.5, 28.5)
	AddMob(21061,	L["Enraged Fire Spirit"],		Z.SHADOWMOON_VALLEY,		48.0, 43.5)
	AddMob(21454,	L["Ashtongue Warrior"],			Z.SHADOWMOON_VALLEY,		57.0, 36.0)
	AddMob(23305,	L["Crazed Murkblood Foreman"],		Z.SHADOWMOON_VALLEY,		72.3, 90.0)
	AddMob(23324,	L["Crazed Murkblood Miner"],		Z.SHADOWMOON_VALLEY,		73.5, 88.5)
	AddMob(26270,	L["Iron Rune-Shaper"],			Z.GRIZZLY_HILLS,		67.8, 16.3)
	AddMob(27333,	L["Onslaught Mason"],			Z.DRAGONBLIGHT,			85.8, 36.0)
	AddMob(28123,	L["Venture Co. Excavator"],		Z.SHOLAZAR_BASIN,		35.8, 45.5)
	AddMob(29235,	L["Gundrak Savage"],			Z.ZULDRAK,			66.8, 42.4)
	AddMob(69461,	L["Itoka"],				Z.ISLE_OF_THUNDER,		0, 0)

	self.InitializeMobDrops = nil
end
