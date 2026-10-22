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

	AddMob(7800,	BN.MEKGINEER_THERMAPLUGG,		Z.GNOMEREGAN,			0, 0)
	AddMob(8897,	L["Doomforge Craftsman"],		Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(8920,	L["Weapon Technician"],			Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(9499,	BN.PLUGGER_SPAZZRING,			Z.BLACKROCK_DEPTHS,		0, 0)
	AddMob(10264,	BN.SOLAKAR_FLAMEWREATH,			Z.BLACKROCK_SPIRE,		0, 0)
	AddMob(10426,	L["Risen Inquisitor"],			Z.STRATHOLME,			0, 0)
	AddMob(16152,	BB["Attumen the Huntsman"],		Z.KARAZHAN,			0, 0)
	AddMob(17796,	BN.MEKGINEER_STEAMRIGGER,		Z.THE_STEAMVAULT,		0, 0)
	AddMob(19219,	BN.MECHANO_LORD_CAPACITUS,		Z.THE_MECHANAR,			0, 0)
	AddMob(19960,	L["Doomforge Engineer"],		Z.BLADES_EDGE_MOUNTAINS,	75.1, 39.8)
	AddMob(20207,	L["Sunfury Bowman"],			Z.NETHERSTORM,			56.8, 64.6)
	AddMob(23386,	L["Gan'arg Analyzer"],			Z.BLADES_EDGE_MOUNTAINS,	33.0, 52.5)

	self.InitializeMobDrops = nil
end
