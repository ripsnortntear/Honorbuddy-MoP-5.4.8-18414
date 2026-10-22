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

	AddVendor(1286,		L["Edna Mullby"],			Z.STORMWIND_CITY,		64.7,	71.2,	"Alliance")
	AddVendor(1448,		L["Neal Allen"],			Z.WETLANDS,			11.8,	52.6,	"Alliance")
	AddVendor(3367,		L["Felika"],				Z.ORGRIMMAR,			60.5,	50.7,	"Horde")
	AddVendor(4775,		L["Felicia Doan"],			Z.UNDERCITY,			64.1,	50.6,	"Horde")
	AddVendor(5163,		L["Burbik Gearspanner"],		Z.IRONFORGE,			46.5,	27.1,	"Alliance")
	AddVendor(8363,		L["Shadi Mistrunner"],			Z.THUNDER_BLUFF,		40.6,	64.0,	"Horde")
	AddVendor(12941,	L["Jase Farlane"],			Z.EASTERN_PLAGUELANDS,		74.8,	51.8,	"Neutral")
	AddVendor(16624,	L["Gelanthis"],				Z.SILVERMOON_CITY,		90.9,	73.3,	"Horde")
	AddVendor(17512,	L["Arred"],				Z.THE_EXODAR,			45.9,	24.9,	"Alliance")
	AddVendor(19065,	L["Inessera"],				Z.SHATTRATH_CITY,		34.5,	20.2,	"Neutral")
	AddVendor(21474,	L["Coreiel"],				Z.NAGRAND,			42.8,	42.6,	"Horde")
	AddVendor(21485,	L["Aldraan"],				Z.NAGRAND,			42.9,	42.5,	"Alliance")
	AddVendor(23437,	L["Indormi"],				Z.HYJAL_SUMMIT,			0,	0,	"Neutral")
	AddVendor(25950,	L["Shaani"],				Z.ISLE_OF_QUELDANAS,		51.5,	32.6,	"Neutral")
	AddVendor(27666,	L["Ontuvo"],				Z.SHATTRATH_CITY,		48.7,	41.3,	"Neutral")
	AddVendor(28701,	L["Timothy Jones"],			Z.DALARAN,			40.5,	35.2,	"Neutral")
	AddVendor(28721,	L["Tiffany Cartier"],			Z.DALARAN,			40.5,	34.4,	"Neutral")
	AddVendor(30489,	L["Morgan Day"],			Z.WINTERGRASP,			49.0,	17.1,	"Alliance")
	AddVendor(31910,	L["Geen"],				Z.SHOLAZAR_BASIN,		54.5,	56.2,	"Neutral")
	AddVendor(31911,	L["Tanak"],				Z.SHOLAZAR_BASIN,		55.1,	69.1,	"Neutral")
	AddVendor(32294,	L["Knight Dameron"],			Z.WINTERGRASP,			51.7,	17.5,	"Alliance")
	AddVendor(32296,	L["Stone Guard Mukar"],			Z.WINTERGRASP,			51.7,	17.5,	"Horde")
	AddVendor(33602,	L["Anuur"],				Z.ICECROWN,			71.4,	20.8,	"Neutral")
	AddVendor(33637,	L["Kirembri Silvermane"],		Z.SHATTRATH_CITY,		58.1,	75.0,	"Neutral")
	AddVendor(33680,	L["Nemiha"],				Z.SHATTRATH_CITY,		36.1,	47.7,	"Neutral")
	AddVendor(44583,	L["Terrance Denman"],			Z.STORMWIND_CITY, 		63.2,	61.7,	"Alliance")
	AddVendor(50480,	L["Isabel Jones"],			Z.STORMWIND_CITY,		63.7,	61.3,	"Alliance")
	AddVendor(50482,	L["Marith Lazuria"],			Z.ORGRIMMAR,			72.5,	34.4,	"Horde")
	AddVendor(52584,	L["Laida Gembold"],			Z.IRONFORGE,			50.6,	27.5,	"Alliance")
	AddVendor(52588,	L["Sara Lanner"],			Z.UNDERCITY,			56.7,	36.9,	"Horde")
	AddVendor(52644,	L["Tarien Silverdew"],			Z.DARNASSUS,			54.4,	30.7,	"Alliance")
	AddVendor(52658,	L["Paku Cloudchaser"],			Z.THUNDER_BLUFF,		34.7,	53.5,	"Horde")
	AddVendor(56925,	L["Farrah Facet"],			Z.STORMWIND_CITY,		63.8,	61.6,	"Alliance")
	AddVendor(57922,	L["Taryssa Lazuria"],			Z.ORGRIMMAR,			72.4,	34.6,	"Horde")
	AddVendor(58414,	L["San Redscale"],			Z.THE_JADE_FOREST,		56.6,	44.4,	"Neutral")

	self.InitializeVendors = nil
end
