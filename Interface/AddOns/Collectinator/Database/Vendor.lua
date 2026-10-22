--[[
************************************************************************
Vendor.lua
************************************************************************
File date: 2013-09-07T03:15:28Z
File hash: 1892ad7
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/arl/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]]--

-----------------------------------------------------------------------
-- Upvalued Lua API.
-----------------------------------------------------------------------
local _G = getfenv(0)

-----------------------------------------------------------------------
-- AddOn namespace.
-----------------------------------------------------------------------
local FOLDER_NAME, private	= ...

local LibStub = _G.LibStub

local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)
local BB = LibStub("LibBabble-Boss-3.0"):GetLookupTable()

local BN = private.BOSS_NAMES
local FN = private.LOCALIZED_FACTION_STRINGS
local Z = private.ZONE_NAMES

private.vendor_list = {}

function addon:InitVendor()
	local function AddVendor(id_num, name, zone_name, x, y, faction)
		private:AddListEntry(private.vendor_list, id_num, name, zone_name, x, y, faction)
	end

	AddVendor(384,		L["Katie Hunter"],		Z.ELWYNN_FOREST,		84.0,	65.4,	"Alliance")
	AddVendor(1261,		L["Veron Amberstill"],		Z.DUN_MOROGH,			70.6,	48.8,	"Alliance")
	AddVendor(1263,		L["Yarlyn Amberstill"],		Z.DUN_MOROGH,			70.6,	49.0,	"Alliance")
	AddVendor(1460,		L["Unger Statforth"],		Z.WETLANDS,			9.2,	56.6,	"Alliance")
	AddVendor(2663,		L["Narkk"],			Z.THE_CAPE_OF_STRANGLETHORN,	42.6,	69.2,	"Neutral")
	AddVendor(3362,		L["Ogunaro Wolfrunner"],	Z.ORGRIMMAR,			61.0,	35.2,	"Horde")
	AddVendor(3685,		L["Harb Clawhoof"],		Z.MULGORE,			47.6,	58.0,	"Horde")
	AddVendor(4730,		L["Lelanai"],			Z.DARNASSUS,			42.6,	32.8,	"Alliance")
	AddVendor(4731,		L["Zachariah Post"],		Z.TIRISFAL_GLADES,		61.8,	51.8,	"Horde")
	AddVendor(4885,		L["Gregor MacVince"],		Z.DUSTWALLOW_MARSH,		65.2,	51.4,	"Alliance")
	AddVendor(6367,		L["Donni Anthania"],		Z.ELWYNN_FOREST,		44.2,	53.2,	"Alliance")
	AddVendor(7952,		L["Zjolnir"],			Z.DUROTAR,			55.2,	75.6,	"Horde")
	AddVendor(7955,		L["Milli Featherwhistle"],	Z.DUN_MOROGH,			56.2,	46.2,	"Alliance")
	AddVendor(8401,		L["Halpa"],			Z.THUNDER_BLUFF,		62.2,	58.6,	"Horde")
	AddVendor(8403,		L["Jeremiah Payson"],		Z.UNDERCITY,			68.0,	44.0,	"Horde")
	AddVendor(8404,		L["Xan'tish"],			Z.ORGRIMMAR,			34.8,	65.2,	"Horde")
	AddVendor(8665,		L["Shylenai"],			Z.DARNASSUS,			64.0,	53.6,	"Alliance")
	AddVendor(8666,		L["Lil Timmy"],			Z.STORMWIND_CITY,		69.4,	62.6,	"Alliance")
	AddVendor(12783,	L["Lieutenant Karter"],		Z.STORMWIND_CITY,		76.2,	65.6,	"Alliance")
	AddVendor(12796,	L["Raider Bork"],		Z.ORGRIMMAR,			47.8,	73.6,	"Horde")
	AddVendor(13216,	L["Gaelden Hammersmith"],	Z.ALTERAC_VALLEY,		44.2,	18.2,	"Alliance")
	AddVendor(13217,	L["Thanthaldis Snowgleam"],	Z.HILLSBRAD_FOOTHILLS,		44.6,	46.6,	"Alliance")
	AddVendor(13218,	L["Grunnda Wolfheart"],		Z.ALTERAC_VALLEY,		49.4,	82.4,	"Horde")
	AddVendor(13219,	L["Jorek Ironside"],		Z.HILLSBRAD_FOOTHILLS,		58.0,	33.6,	"Horde")
	AddVendor(14828,	L["Gelvas Grimegate"],		Z.DARKMOON_ISLAND,		48.0,	64.8,	"Neutral")
	AddVendor(14846,	L["Lhara"],			Z.DARKMOON_ISLAND,		48.6,	69.8,	"Neutral")
	AddVendor(14860,	L["Flik"],			Z.DARKMOON_ISLAND,		59.6,	68.0,	"Neutral")
	AddVendor(15864,	L["Valadar Starsong"],		Z.MOONGLADE,			54.0,	35.0,	"Neutral")
	AddVendor(16264,	L["Winaestra"],			Z.EVERSONG_WOODS,		61.0,	54.6,	"Horde")
	AddVendor(16860,	L["Jilanne"],			Z.EVERSONG_WOODS,		44.8,	71.6,	"Horde")
	AddVendor(17584,	L["Torallius the Pack Handler"],Z.THE_EXODAR,			81.6,	52.6,	"Alliance")
	AddVendor(17904,	L["Fedryen Swiftspear"],	Z.ZANGARMARSH,			79.2,	63.8,	"Neutral")
	AddVendor(18382,	L["Mycah"],			Z.ZANGARMARSH,			17.8,	51.2,	"Neutral")
	AddVendor(20240,	L["Trader Narasu"],		Z.NAGRAND,			54.6,	75.0,	"Alliance")
	AddVendor(20241,	L["Provisioner Nasela"],	Z.NAGRAND,			53.4,	36.8,	"Horde")
	AddVendor(20494,	L["Dama Wildmane"],		Z.SHADOWMOON_VALLEY,		29.0,	29.4,	"Horde")
	AddVendor(20510,	L["Brunn Flamebeard"],		Z.SHADOWMOON_VALLEY,		37.6,	56.0,	"Alliance")
	AddVendor(20980,	L["Dealer Rashaad"],		Z.NETHERSTORM,			43.4,	35.2,	"Neutral")
	AddVendor(21019,	L["Sixx"],			Z.THE_EXODAR,			30.8,	34.6,	"Alliance")
	AddVendor(21474,	L["Coreiel"],			Z.NAGRAND,			42.8,	42.6,	"Horde")
	AddVendor(21485,	L["Aldraan"],			Z.NAGRAND,			42.8,	42.6,	"Alliance")
	AddVendor(23367,	L["Grella"],			Z.TEROKKAR_FOREST,		64.2,	66.2,	"Neutral")
	AddVendor(23489,	L["Drake Dealer Hurlunk"],	Z.SHADOWMOON_VALLEY,		65.6,	86.0,	"Neutral")
	AddVendor(23710,	L["Belbi Quikswitch"],		Z.DUN_MOROGH,			56.2,	37.8,	"Alliance")
	AddVendor(24468,	L["Pol Amberstill"],		Z.DUN_MOROGH,			53.6,	38.6,	"Alliance")
	AddVendor(24495,	L["Blix Fixwidget"],		Z.DUROTAR,			40.4,	17.8,	"Horde")  -- Blizz thinks this is "Bliz Fixwidget"
	AddVendor(24510,	L["Driz Tumblequick"],		Z.DUROTAR,			42.6,	17.6,	"Horde")
	AddVendor(26123,	L["Midsummer Supplier"],	Z.STORMWIND_CITY,		49.2,	71.8,	"Alliance")
	AddVendor(26124,	L["Midsummer Merchant"],	Z.ORGRIMMAR,			47.6,	38.6,	"Horde")
	AddVendor(27478,	L["Larkin Thunderbrew"],	Z.IRONFORGE,			19.8,	53.2,	"Alliance")
	AddVendor(27489,	L["Ray'ma"],			Z.ORGRIMMAR,			50.6,	73.6,	"Horde")
	AddVendor(28951,	L["Breanni"],			Z.DALARAN,			40.5,	35.2,	"Neutral")
	AddVendor(29478,	L["Jepetto Joybuzz"],		Z.DALARAN,			44.6,	46.0,	"Neutral")
	AddVendor(29537,	L["Darahir"],			Z.DALARAN,			63.8,	16.6,	"Neutral")
	AddVendor(29587,	L["Dread Commander Thalanor"],	Z.EASTERN_PLAGUELANDS,		84.0,	49.8,	"Neutral")
	AddVendor(29716,	L["Clockwork Assistant"],	Z.DALARAN,			44.8,	46.2,	"Neutral")
	AddVendor(31910,	L["Geen"],			Z.SHOLAZAR_BASIN,		54.6, 	56.2,	"Neutral")
	AddVendor(31916,	L["Tanaika"],			Z.HOWLING_FJORD,		25.4,	58.6,	"Neutral")
	AddVendor(32216,	L["Mei Francis"],		Z.DALARAN,			58.6,	43.2,	"Neutral")
	AddVendor(32294,	L["Knight Dameron"],		Z.WINTERGRASP,			51.6,	17.6,	"Alliance")
	AddVendor(32533,	L["Cielstrasza"],		Z.DRAGONBLIGHT,			59.8,	53.0,	"Neutral")
	AddVendor(32296,	L["Stone Guard Mukar"],		Z.WINTERGRASP,			51.6,	17.6,	"Horde")
	AddVendor(32540,	L["Lillehoff"],			Z.THE_STORM_PEAKS,		66.0,	61.4,	"Neutral")
	AddVendor(32763,	L["Sairuk"],			Z.DRAGONBLIGHT,			48.6,	75.6,	"Neutral")
	AddVendor(32836,	L["Noblegarden Vendor"],	Z.ELWYNN_FOREST,		43.0,	65.3,	"Alliance")
	AddVendor(32837,	L["Noblegarden Merchant"],	Z.DUROTAR,			51.0,	41.0,	"Horde")
	AddVendor(33307,	L["Corporal Arthur Flew"],	Z.ICECROWN,			76.4,	19.2,	"Alliance")
	AddVendor(33310,	L["Derrick Brindlebeard"],	Z.ICECROWN,			76.4,	19.4,	"Alliance")
	AddVendor(33553,	L["Freka Bloodaxe"],		Z.ICECROWN,			76.4,	24.2,	"Horde")
	AddVendor(33554,	L["Samamba"],			Z.ICECROWN,			76.0,	24.4,	"Horde")
	AddVendor(33555,	L["Eliza Killian"],		Z.ICECROWN,			76.4,	24.0,	"Horde")
	AddVendor(33556,	L["Doru Thunderhorn"],		Z.ICECROWN,			76.2,	24.4,	"Horde")
	AddVendor(33557,	L["Trellis Morningsun"],	Z.ICECROWN,			76.2,	23.8,	"Horde")
	AddVendor(33650,	L["Rillie Spindlenut"],		Z.ICECROWN,			76.4,	19.6,	"Alliance")
	AddVendor(33653,	L["Rook Hawkfist"],		Z.ICECROWN,			76.2,	19.2,	"Alliance")
	AddVendor(33657,	L["Irisee"],			Z.ICECROWN,			76.2,	19.2,	"Alliance")
	AddVendor(33980,	L["Apothecary Furrows"],	Z.DARKSHORE,			57.2,	33.8,	"Horde")
	AddVendor(34772,	L["Vasarin Redmorn"],		Z.ICECROWN,			76.2,	24.0,	"Horde")
	AddVendor(34881,	L["Hiren Loresong"],		Z.ICECROWN,			76.2,	19.6,	"Alliance")
	AddVendor(34882,	L["Vasarin Redmorn"],		Z.ICECROWN,			76.2,	24.0,	"Horde")
	AddVendor(34885,	L["Dame Evniki Kapsalis"],	Z.ICECROWN,			69.4,	23.2,	"Neutral")
	AddVendor(35099,	L["Bana Wildmane"],		Z.HELLFIRE_PENINSULA,		54.2,	41.6,	"Horde")
	AddVendor(35101,	L["Grunda Bronzewing"],		Z.HELLFIRE_PENINSULA,		54.2,	62.6,	"Alliance")
	AddVendor(35131,	L["Durgan Thunderbeak"],	Z.BOREAN_TUNDRA,		58.8,	68.2,	"Alliance")
	AddVendor(35132,	L["Tohfo Skyhoof"],		Z.BOREAN_TUNDRA,		42.2,	55.4,	"Horde")
	AddVendor(41135,	L["\"Plucky\" Johnson"],	Z.THOUSAND_NEEDLES,		85.6,	91.6,	"Neutral")
	AddVendor(43694,	L["Katie Stokx"],		Z.STORMWIND_CITY,		77.0,	67.8,	"Alliance")
	AddVendor(43768,	L["Tannec Stonebeak"],		Z.STORMWIND_CITY,		71.4,	72.2,	"Alliance")
	AddVendor(44179,	L["Harry No-Hooks"],		Z.THE_CAPE_OF_STRANGLETHORN,	46.6,	93.6,	"Alliance")
	AddVendor(46572,	L["Goram"],			Z.ORGRIMMAR,			48.2,	75.6,	"Horde")
	AddVendor(46602,	L["Shay Pressler"],		Z.STORMWIND_CITY,		64.6,	76.8,	"Alliance")
	AddVendor(47328,	L["Quartermaster Brazie"],	Z.TOL_BARAD_PENINSULA,		72.6,	62.6,	"Alliance")
	AddVendor(48510,	L["Kall Worthaton"],		Z.ORGRIMMAR,			36.0,	86.4,	"Horde")
	AddVendor(48531,	L["Pogg"],			Z.TOL_BARAD_PENINSULA,		54.6,	81.0,	"Horde")
	AddVendor(48617,	L["Blacksmith Abasi"],		Z.ULDUM,			54.0,	33.2,	"Neutral")
	AddVendor(51495,	L["Steeg Haskell"],		Z.IRONFORGE,			36.6,	84.6,	"Alliance")
	AddVendor(51496,	L["Kim Horn"],			Z.UNDERCITY,			69.6,	43.8,	"Horde")
	AddVendor(51501,	L["Nuri"],			Z.THE_EXODAR,			53.6,	70.6,	"Alliance")
	AddVendor(51502,	L["Larissia"],			Z.SILVERMOON_CITY,		78.2,	84.8,	"Horde")
	AddVendor(51503,	L["Randah Songhorn"],		Z.THUNDER_BLUFF,		37.6,	62.8,	"Horde")
	AddVendor(51504,	L["Velia Moonbow"],		Z.DARNASSUS,			64.6,	37.6,	"Alliance")
	AddVendor(51512,	L["Mirla Silverblaze"],		Z.DALARAN,			52.6,	56.6,	"Neutral")
	AddVendor(52268,	L["Riha"],			Z.SHATTRATH_CITY,		58.6,	46.6,	"Neutral")
	AddVendor(52358,	L["Craggle Wobbletop"],		Z.STORMWIND_CITY,		57.6,	73.4,	"Alliance")
	AddVendor(52809,	L["Blax Bottlerocket"],		Z.ORGRIMMAR,			58.8,	59.6,	"Horde")
	AddVendor(52822,	L["Zen'Vorka"],			Z.MOLTEN_FRONT,			47.0,	90.6,	"Neutral")
	AddVendor(52830,	L["Michelle De Rum"],		Z.WINTERSPRING,			59.8,	51.6,	"Neutral")
	AddVendor(53728,	L["Dorothy"],			Z.ELWYNN_FOREST,		31.8,	50.0,	"Alliance")
	AddVendor(53757,	L["Chub"],			Z.TIRISFAL_GLADES,		67.8,	7.6,	"Horde")
	AddVendor(53881,	L["Ayla Shadowstorm"],		Z.MOLTEN_FRONT,			44.8,	86.6,	"Neutral")
	AddVendor(53882,	L["Varlan Highbough"],		Z.MOLTEN_FRONT,			44.6,	88.6,	"Neutral")
	AddVendor(55285,	L["Astrid Langstrump"],		Z.DARNASSUS,			48.6,	22.2,	"Neutral")
	AddVendor(55305,	L["Carl Goodup"],		Z.DARKMOON_ISLAND,		49.8,	85.6,	"Neutral")
	AddVendor(58414,	L["San Redscale"],		Z.THE_JADE_FOREST,		56.6,	44.4,	"Neutral")
	AddVendor(58706,	L["Gina Mudclaw"],		Z.VALLEY_OF_THE_FOUR_WINDS,	53.2,	51.6,	"Neutral")
	AddVendor(59908,	L["Jaluu the Generous"],	Z.VALE_OF_ETERNAL_BLOSSOMS,	74.2,	42.6,	"Neutral")
	AddVendor(63194,	L["Steven Lisbane"],		Z.NORTHERN_STRANGLETHORN,	46.0,	40.6,	"Alliance")
	AddVendor(63596,	L["Audrey Burnhep"],		Z.STORMWIND_CITY,		69.6,	25.8,	"Alliance")
	AddVendor(63721,	FN.NAT_PAGLE,			Z.KRASARANG_WILDS,		68.4,	43.4,	"Neutral")
	AddVendor(63994,	L["Challenger Wuli"],		Z.SHRINE_OF_TWO_MOONS,		61.0,	21.0,	"Horde")
	AddVendor(64001,	L["Sage Lotusbloom"],		Z.SHRINE_OF_TWO_MOONS,		62.6,	23.2,	"Horde")
	AddVendor(64028,	L["Challenger Soong"],		Z.SHRINE_OF_SEVEN_STARS,	86.2,	61.6,	"Alliance")
	AddVendor(64032,	L["Sage Whiteheart"],		Z.SHRINE_OF_SEVEN_STARS,	84.6,	63.6,	"Alliance")
	AddVendor(64518,	L["Uncle Bigpocket"],		Z.KUN_LAI_SUMMIT,		65.4,	61.6,	"Neutral")
	AddVendor(64595,	L["Rushi the Fox"],		Z.TOWNLONG_STEPPES,		48.8,	70.6,	"Neutral")
	AddVendor(64599,	L["Ambersmith Zikk"],		Z.DREAD_WASTES,			55.0,	35.6,	"Neutral")
	AddVendor(64605,	L["Tan Shin Tiao"],		Z.VALE_OF_ETERNAL_BLOSSOMS,	82.2,	29.4,	"Neutral")
	AddVendor(65068,	L["Old Whitenose"],		Z.STORMWIND_CITY,		67.8,	18.6,	"Alliance")
	AddVendor(66022,	L["Turtlemaster Odai"],		Z.ORGRIMMAR,			69.8,	41.0,	"Horde")
	AddVendor(66973,	L["Kay Featherfall"],		Z.VALE_OF_ETERNAL_BLOSSOMS,	82.2,	34.0,	"Neutral")
	AddVendor(67672,	L["Vasarin Redmorn"],		Z.ISLE_OF_THUNDER,		33.4,	32.4,	"Horde")
	AddVendor(68000,	L["Hiren Loresong"],		Z.ISLE_OF_THUNDER,		64.8,	74.4,	"Alliance")
	AddVendor(68363,	L["Quackenbush"],		Z.DEEPRUN_TRAM,			53.9,	26.3,	"Alliance")
	AddVendor(68364,	L["Paul North"],		Z.BRAWLGAR_ARENA,		50.8,	31.8,	"Horde")
	AddVendor(69059,	L["Agent Malley"],		Z.KRASARANG_WILDS,		89.6,	33.4,	"Alliance")
	AddVendor(69060,	L["Tuskripper Grukna"],		Z.KRASARANG_WILDS,		10.8,	53.4,	"Horde")
	AddVendor(71226,	L["Ravika"],			Z.DUROTAR,			49.4,	40.2,	"Neutral")
	AddVendor(73082,	L["Master Li"],			Z.TIMELESS_ISLE,		34.7,	59.7,	"Neutral")
	AddVendor(73151,	L["Deathguard Netharian"],	Z.ORGRIMMAR,			42.2,	72.8,	"Horde")
	AddVendor(73190,	L["Necrolord Sipe"],		Z.STORMWIND_CITY,		77.6,	66.2,	"Alliance")
	AddVendor(73306,	L["Mistweaver Ku"],		Z.TIMELESS_ISLE,		42.7,	54.7,	"Neutral")
	AddVendor(73307,	L["Speaker Gulan"],		Z.TIMELESS_ISLE,		74.9,	44.9,	"Neutral")
	AddVendor(73819,	L["Ku-Mo"],			Z.TIMELESS_ISLE,		41.4,	63.6,	"Neutral")

	self.InitVendor = nil
end
