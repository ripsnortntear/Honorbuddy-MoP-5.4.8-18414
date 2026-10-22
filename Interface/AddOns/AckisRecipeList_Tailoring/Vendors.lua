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

	AddVendor(1250,		L["Drake Lindgren"],			Z.ELWYNN_FOREST,		83.3,	66.7,	"Alliance")
	AddVendor(1347,		L["Alexandra Bolero"],			Z.STORMWIND_CITY,		53.3,	81.7,	"Alliance")
	AddVendor(1454,		L["Jennabink Powerseam"],		Z.WETLANDS,			 8.1,	55.9,	"Alliance")
	AddVendor(1474,		L["Rann Flamespinner"],			Z.LOCH_MODAN,			36.0,	46.0,	"Alliance")
	AddVendor(2394,		L["Mallen Swain"],			Z.HILLSBRAD_FOOTHILLS,		58.1,	47.9,	"Horde")
	AddVendor(2663,		L["Narkk"],				Z.THE_CAPE_OF_STRANGLETHORN,	42.7,	69.2,	"Neutral")
	AddVendor(2668,		L["Danielle Zipstitch"],		Z.DUSKWOOD,			75.8,	45.5,	"Alliance")
	AddVendor(2669,		L["Sheri Zipstitch"],			Z.DUSKWOOD,			75.7,	45.5,	"Alliance")
	AddVendor(2670,		L["Xizk Goodstitch"],			Z.THE_CAPE_OF_STRANGLETHORN,	43.6,	73.0,	"Neutral")
	AddVendor(2672,		L["Cowardly Crosby"],			Z.THE_CAPE_OF_STRANGLETHORN,	40.9,	82.5,	"Neutral")
	AddVendor(3005,		L["Mahu"],				Z.THUNDER_BLUFF,		43.8,	45.1,	"Horde")
	AddVendor(3364,		L["Borya"],				Z.ORGRIMMAR,			60.7,	58.6,	"Horde")
	AddVendor(3485,		L["Wrahk"],				Z.NORTHERN_BARRENS,		50.0,	61.1,	"Horde")
	AddVendor(3522,		L["Constance Brisboise"],		Z.TIRISFAL_GLADES,		52.6,	55.7,	"Horde")
	AddVendor(4168,		L["Elynna"],				Z.DARNASSUS,			60.6,	36.9,	"Alliance")
	AddVendor(4577,		L["Millie Gregorian"],			Z.UNDERCITY,			70.6,	30.1,	"Horde")
	AddVendor(5154,		L["Poranna Snowbraid"],			Z.IRONFORGE,			43.0,	28.3,	"Alliance")
	AddVendor(6567,		L["Ghok'kah"],				Z.DUSTWALLOW_MARSH,		35.2,	30.8,	"Horde")
	AddVendor(6568,		L["Vizzklick"],				Z.TANARIS,			50.7,	28.7,	"Neutral")
	AddVendor(6574,		L["Jun'ha"],				Z.ARATHI_HIGHLANDS,		72.7,	36.5,	"Horde")
	AddVendor(7940,		L["Darnall"],				Z.MOONGLADE,			51.6,	33.3,	"Neutral")
	AddVendor(8681,		L["Outfitter Eric"],			Z.IRONFORGE,			43.2,	29.2,	"Alliance")
	AddVendor(14371,	L["Shen'dralar Provisioner"],		Z.DIRE_MAUL,			0,	0,	"Neutral")
	AddVendor(16224,	L["Rathis Tomber"],			Z.GHOSTLANDS,			47.2,	28.7,	"Horde")
	AddVendor(16638,	L["Deynna"],				Z.SILVERMOON_CITY,		55.6,	51.0,	"Horde")
	AddVendor(16767,	L["Neii"],				Z.THE_EXODAR,			64.5,	68.5,	"Alliance")
	AddVendor(19015,	L["Mathar G'ochar"],			Z.NAGRAND,			57.0,	39.6,	"Horde")
	AddVendor(19017,	L["Borto"],				Z.NAGRAND,			53.3,	71.9,	"Alliance")
	AddVendor(19213,	L["Eiin"],				Z.SHATTRATH_CITY,		66.2,	68.7,	"Neutral")
	AddVendor(19521,	L["Arrond"],				Z.SHADOWMOON_VALLEY,		55.9,	58.2,	"Neutral")
	AddVendor(19722,	L["Muheru the Weaver"],			Z.ZANGARMARSH,			40.6,	28.2,	"Alliance")
	AddVendor(22208,	L["Nasmara Moonsong"],			Z.SHATTRATH_CITY,		66.0,	69.0,	"Neutral")
	AddVendor(22212,	L["Andrion Darkspinner"],		Z.SHATTRATH_CITY,		66.0,	67.8,	"Neutral")
	AddVendor(22213,	L["Gidge Spellweaver"],			Z.SHATTRATH_CITY,		66.0,	67.9,	"Neutral")
	AddVendor(29510,	L["Linna Bruder"],			Z.DALARAN,			34.6,	34.5,	"Neutral")
	AddVendor(29511,	L["Lalla Brightweave"],			Z.DALARAN,			36.5,	33.5,	"Neutral")
	AddVendor(29512,	L["Ainderu Summerleaf"],		Z.DALARAN,			36.5,	34.0,	"Neutral")
	AddVendor(40160,	L["Frozo the Renowned"],		Z.DALARAN,			41.0,	28.5,	"Neutral")
	AddVendor(40572,	L["Haughty Modiste"],			Z.TANARIS,			50.7,	28.6,	"Neutral")
	AddVendor(45558,	L["Lizna Goldweaver"],			Z.ORGRIMMAR,			41.3,	79.2,	"Horde")
	AddVendor(50386,	L["Sal Ferraga"],			Z.TWILIGHT_HIGHLANDS,		78.6,	76.9,	"Alliance")
	AddVendor(50433,	L["Aristaleon Sunweaver"],		Z.TWILIGHT_HIGHLANDS,		75.2,	50.1,	"Horde")
	AddVendor(64051,	L["Esha the Loommaiden"],		Z.SHRINE_OF_TWO_MOONS,		29.8,	54.0,	"Horde")
	AddVendor(64052,	L["Raishen the Needle"],		Z.SHRINE_OF_SEVEN_STARS,	67.6,	46.2,	"Alliance")

	self.InitializeVendors = nil
end
