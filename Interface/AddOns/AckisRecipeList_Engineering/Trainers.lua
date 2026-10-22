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

local Z = constants.ZONE_NAMES

-----------------------------------------------------------------------
-- What we _really_ came here to see...
-----------------------------------------------------------------------
function module:InitializeTrainers()
	addon:AddTrainer(1676, "Finbus Geargrind", Z.DUSKWOOD, 77.4, 48.6, "Alliance")
	addon:AddTrainer(1702, "Bronk Guzzlegear", Z.DUN_MOROGH, 50.2, 50.4, "Alliance")
	addon:AddTrainer(3290, "Deek Fizzlebizz", Z.LOCH_MODAN, 45.9, 13.6, "Alliance")
	addon:AddTrainer(3494, "Tinkerwiz", Z.NORTHERN_BARRENS, 68.5, 69.2, "Neutral")
	addon:AddTrainer(4941, "Caz Twosprocket", Z.DUSTWALLOW_MARSH, 64.7, 50.4, "Alliance")
	addon:AddTrainer(5174, "Springspindle Fizzlegear", Z.IRONFORGE, 68.4, 44, "Alliance")
	addon:AddTrainer(5518, "Lilliam Sparkspindle", Z.STORMWIND_CITY, 62.8, 32, "Alliance")
	addon:AddTrainer(7406, "Oglethorpe Obnoticus", Z.THE_CAPE_OF_STRANGLETHORN, 43, 72.1, "Neutral")
	addon:AddTrainer(7944, "Tinkmaster Overspark", Z.IRONFORGE, 69.8, 50, "Alliance")
	addon:AddTrainer(8126, "Nixx Sprocketspring", Z.TANARIS, 52.4, 28.3, "Neutral")
	addon:AddTrainer(8736, "Buzzek Bracketswing", Z.TANARIS, 51.7, 30.4, "Neutral")
	addon:AddTrainer(8738, "Vazario Linkgrease", Z.NORTHERN_BARRENS, 68.5, 69.2, "Neutral")
	addon:AddTrainer(11017, "Roxxik", Z.ORGRIMMAR, 56.85, 56.54, "Horde")
	addon:AddTrainer(11025, "Mukdrak", Z.DUROTAR, 52.2, 40.8, "Horde")
	addon:AddTrainer(11031, "Franklin Lloyd", Z.UNDERCITY, 75.9, 73.7, "Horde")
	addon:AddTrainer(11037, "Jenna Lemkenilli", Z.DARKSHORE, 50.8, 20.7, "Alliance")
	addon:AddTrainer(14742, "Zap Farflinger", Z.WINTERSPRING, 59.7, 49.8, "Neutral")
	addon:AddTrainer(14743, "Jhordy Lapforge", Z.TANARIS, 52.2, 27.9, "Neutral")
	addon:AddTrainer(16667, "Danwe", Z.SILVERMOON_CITY, 76.5, 40.9, "Horde")
	addon:AddTrainer(16726, "Ockil", Z.THE_EXODAR, 54, 92.1, "Alliance")
	addon:AddTrainer(17222, "Artificer Daelo", Z.AZUREMYST_ISLE, 48, 51, "Alliance")
	addon:AddTrainer(17634, "K. Lee Smallfry", Z.ZANGARMARSH, 68.6, 50.2, "Alliance")
	addon:AddTrainer(17637, "Mack Diver", Z.ZANGARMARSH, 34, 50.8, "Horde")
	addon:AddTrainer(18752, "Zebig", Z.HELLFIRE_PENINSULA, 54.8, 38.5, "Horde")
	addon:AddTrainer(18775, "Lebowski", Z.HELLFIRE_PENINSULA, 55.7, 65.5, "Alliance")
	addon:AddTrainer(19576, "Xyrol", Z.NETHERSTORM, 32.5, 66.7, "Neutral")
	addon:AddTrainer(21493, "Kablamm Farflinger", Z.NETHERSTORM, 32.9, 63.7, "Neutral")
	addon:AddTrainer(21494, "Smiles O'Byron", Z.BLADES_EDGE_MOUNTAINS, 60.3, 65.2, "Neutral")
	addon:AddTrainer(24868, "Niobe Whizzlespark", Z.SHADOWMOON_VALLEY, 36.7, 54.8, "Alliance")
	addon:AddTrainer(25099, "Jonathan Garrett", Z.SHADOWMOON_VALLEY, 29.2, 28.5, "Horde")
	addon:AddTrainer(25277, "Chief Engineer Leveny", Z.BOREAN_TUNDRA, 42.6, 53.7, "Horde")
	addon:AddTrainer(26907, "Tisha Longbridge", Z.HOWLING_FJORD, 59.7, 64, "Alliance")
	addon:AddTrainer(26955, "Jamesina Watterly", Z.HOWLING_FJORD, 78.5, 30, "Horde")
	addon:AddTrainer(26991, "Sock Brightbolt", Z.BOREAN_TUNDRA, 57.7, 72.2, "Alliance")
	addon:AddTrainer(28697, "Timofey Oshenko", Z.DALARAN, 39, 27.5, "Neutral")
	addon:AddTrainer(29513, "Didi the Wrench", Z.DALARAN, 39.5, 25.5, "Neutral")
	addon:AddTrainer(29514, "Findle Whistlesteam", Z.DALARAN, 39.5, 25.2, "Neutral")
	addon:AddTrainer(33586, "Binkie Brightgear", Z.ICECROWN, 72.1, 20.9, "Neutral")
	addon:AddTrainer(33611, 51306, Z.SHATTRATH_CITY, 43.7, 90.1, "Neutral")
	addon:AddTrainer(33634, "Engineer Sinbei", Z.SHATTRATH_CITY, 43.1, 64.9, "Neutral")
	addon:AddTrainer(45545, "\"Jack\" Pisarek Slamfix", Z.ORGRIMMAR, 36.34, 86.74, "Horde")
	addon:AddTrainer(52636, "Tana Lentner", Z.DARNASSUS, 49.6, 32.3, "Alliance")
	addon:AddTrainer(52651, "Engineer Palehoof", Z.THUNDER_BLUFF, 36.1, 59.6, "Horde")
	addon:AddTrainer(55143, "Sally Fizzlefury", Z.VALLEY_OF_THE_FOUR_WINDS, 16.1, 83.1, "Neutral")

	self.InitializeTrainers = nil
end
