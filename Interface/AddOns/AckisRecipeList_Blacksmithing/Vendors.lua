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
local Z = constants.ZONE_NAMES

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeVendors()
	local function AddVendor(id_num, name, zone_name, x, y, faction)
		addon.AcquireTypes.Vendor:AddEntity(id_num, name, zone_name, x, y, faction)
	end

	AddVendor(1146,		L["Vharr"],				Z.NORTHERN_STRANGLETHORN,	38.7,	49.2,	"Horde")
	AddVendor(1471,		L["Jannos Ironwill"],			Z.ARATHI_HIGHLANDS,		40.8,	48.0,	"Alliance")
	AddVendor(2482,		L["Zarena Cromwind"],			Z.THE_CAPE_OF_STRANGLETHORN,	43.0,	70.7,	"Neutral")
	AddVendor(2483,		L["Jaquilina Dramet"],			Z.NORTHERN_STRANGLETHORN,	43.7,	23.1,	"Neutral")
	AddVendor(2843,		L["Jutak"],				Z.THE_CAPE_OF_STRANGLETHORN,	41.6,	74.1,	"Neutral")
	AddVendor(2999,		L["Taur Stonehoof"],			Z.THUNDER_BLUFF,		39.8,	55.7,	"Horde")
	AddVendor(3356,		L["Sumi"],				Z.ORGRIMMAR,			75.8,	35.2,	"Horde")
	AddVendor(4259,		L["Thurgrum Deepforge"],		Z.IRONFORGE,			51.5,	42.7,	"Alliance")
	AddVendor(4597,		L["Samuel Van Brunt"],			Z.UNDERCITY,			61.4,	30.1,	"Horde")
	AddVendor(5411,		L["Krinkle Goodsteel"],			Z.TANARIS,			51.2,	30.4,	"Neutral")
	AddVendor(5512,		L["Kaita Deepforge"],			Z.STORMWIND_CITY,		63.5,	37.6,	"Alliance")
	AddVendor(8161,		L["Harggan"],				Z.THE_HINTERLANDS,		13.4,	44.0,	"Alliance")
	AddVendor(8176,		L["Gharash"],				Z.SWAMP_OF_SORROWS,		47.2,	52.1,	"Horde")
	AddVendor(8878,		L["Muuran"],				Z.DESOLACE,			55.6,	56.5,	"Horde")
	AddVendor(15176,	L["Vargus"],				Z.SILITHUS,			54.8,	36.6,	"Neutral")
	AddVendor(15471,	BB["Lieutenant General Andorov"],	Z.RUINS_OF_AHNQIRAJ,		0,	0,	"Neutral")
	AddVendor(16388,	L["Koren"],				Z.KARAZHAN,			0,	0,	"Neutral")
	AddVendor(16583,	L["Rohok"],				Z.HELLFIRE_PENINSULA,		53.2,	38.2,	"Horde")
	AddVendor(16670,	L["Eriden"],				Z.SILVERMOON_CITY,		80.3,	36.1,	"Horde")
	AddVendor(16713,	L["Arras"],				Z.THE_EXODAR,			60.0,	89.5,	"Alliance")
	AddVendor(19342,	L["Krek Cragcrush"],			Z.SHADOWMOON_VALLEY,		28.9,	30.8,	"Horde")
	AddVendor(19373,	L["Mari Stonehand"],			Z.SHADOWMOON_VALLEY,		36.8,	55.1,	"Alliance")
	AddVendor(19662,	L["Aaron Hollman"],			Z.SHATTRATH_CITY,		63.1,	71.1,	"Neutral")
	AddVendor(19694,	L["Loolruna"],				Z.ZANGARMARSH,			68.5,	50.1,	"Alliance")
	AddVendor(26081, 	L["High Admiral \"Shelly\" Jorrik"],	Z.DUN_MOROGH, 			17.8,	74.6,	"Neutral")
	AddVendor(38561,	L["Dramm Riverhorn"],			Z.UNGORO_CRATER,		43.46,	41.60,	"Neutral")
	AddVendor(45549,	L["Zido Helmbreaker"],			Z.ORGRIMMAR,			36.4,	83.0,	"Horde")
	AddVendor(46359,	L["Punra"],				Z.ORGRIMMAR,			45.0,	76.8,	"Horde")
	AddVendor(50129,	L["Daleohm"],				Z.WINTERSPRING,			58.0,	63.8,	"Neutral")
	AddVendor(50375,	L["Kuldar Steeltooth"],			Z.TWILIGHT_HIGHLANDS,		77.3,	53.1,	"Horde")
	AddVendor(50382,	L["Brundall Chiselgut"],		Z.TWILIGHT_HIGHLANDS,		79.1,	76.5,	"Alliance")
	AddVendor(52641,	L["Layna Karner"],			Z.DARNASSUS,			56.8,	52.5,	"Alliance")
	AddVendor(64058,	L["Jorunga Stonehoof"],			Z.SHRINE_OF_TWO_MOONS,		29.4,	43.4,	"Horde")
	AddVendor(64085,	L["Cullen Hammerbrow"],			Z.SHRINE_OF_SEVEN_STARS,	74.4,	51.0,	"Alliance")
	AddVendor(64599,	L["Ambersmith Zikk"],			Z.DREAD_WASTES,			55.0,	35.6,	"Neutral")

	self.InitializeVendors = nil
end
