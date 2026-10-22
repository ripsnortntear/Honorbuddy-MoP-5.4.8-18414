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
	addon:AddTrainer(1385, "Brawn", Z.NORTHERN_STRANGLETHORN, 37.8, 50.4, "Horde")
	addon:AddTrainer(1632, "Adele Fielder", Z.ELWYNN_FOREST, 46.4, 62.1, "Alliance")
	addon:AddTrainer(3007, "Una", Z.THUNDER_BLUFF, 41.5, 42.6, "Horde")
	addon:AddTrainer(3069, "Chaw Stronghide", Z.MULGORE, 45.5, 57.9, "Horde")
	addon:AddTrainer(3365, "Karolek", Z.ORGRIMMAR, 62.8, 44.5, "Horde")
	addon:AddTrainer(3549, "Shelene Rhobart", Z.TIRISFAL_GLADES, 65.5, 61, "Horde")
	addon:AddTrainer(3605, "Nadyia Maneweaver", Z.TELDRASSIL, 41.8, 49.5, "Alliance")
	addon:AddTrainer(3967, "Aayndia Floralwind", Z.ASHENVALE, 35.9, 52.1, "Alliance")
	addon:AddTrainer(4212, "Telonis", Z.DARNASSUS, 60.5, 36.8, "Alliance")
	addon:AddTrainer(4588, "Arthur Moore", Z.UNDERCITY, 70.3, 58.5, "Horde")
	addon:AddTrainer(5127, "Fimble Finespindle", Z.IRONFORGE, 39.8, 33.5, "Alliance")
	addon:AddTrainer(5564, "Simon Tanner", Z.STORMWIND_CITY, 71.8, 62.9, "Alliance")
	addon:AddTrainer(5784, "Waldor", Z.WAILING_CAVERNS, 32.6, 28.5, "Neutral")
	addon:AddTrainer(7868, "Sarah Tanner", Z.SEARING_GORGE, 63.7, 75.7, "Alliance")
	addon:AddTrainer(7869, "Brumn Winterhoof", Z.ARATHI_HIGHLANDS, 28.2, 45, "Horde")
	addon:AddTrainer(7871, "Se'Jib", Z.NORTHERN_STRANGLETHORN, 45.3, 58.7, "Horde")
	addon:AddTrainer(8153, "Narv Hidecrafter", Z.DESOLACE, 55.3, 56.3, "Horde")
	addon:AddTrainer(11097, "Drakk Stonehand", Z.THE_HINTERLANDS, 13.4, 43.4, "Alliance")
	addon:AddTrainer(11098, "Hahrana Ironhide", Z.FERALAS, 74.4, 43.1, "Horde")
	addon:AddTrainer(16278, "Sathein", Z.EVERSONG_WOODS, 53.5, 51, "Horde")
	addon:AddTrainer(16688, "Lynalis", Z.SILVERMOON_CITY, 84, 80.2, "Horde")
	addon:AddTrainer(16728, "Akham", Z.THE_EXODAR, 66, 74.6, "Alliance")
	addon:AddTrainer(17442, "Moordo", Z.AZUREMYST_ISLE, 44.8, 23.8, "Alliance")
	addon:AddTrainer(18754, "Barim Spilthoof", Z.HELLFIRE_PENINSULA, 56.2, 38.6, "Horde")
	addon:AddTrainer(18771, "Brumman", Z.HELLFIRE_PENINSULA, 54.1, 64, "Alliance")
	addon:AddTrainer(19187, "Darmari", Z.SHATTRATH_CITY, 66.8, 67.1, "Neutral")
	addon:AddTrainer(21087, "Grikka", Z.BLADES_EDGE_MOUNTAINS, 76.8, 65.5, "Horde")
	addon:AddTrainer(26911, "Bernadette Dexter", Z.HOWLING_FJORD, 59.9, 63.6, "Alliance")
	addon:AddTrainer(26961, "Gunter Hansen", Z.HOWLING_FJORD, 78.3, 28.2, "Horde")
	addon:AddTrainer(26996, "Awan Iceborn", Z.BOREAN_TUNDRA, 76.3, 37, "Horde")
	addon:AddTrainer(26998, "Rosemary Bovard", Z.BOREAN_TUNDRA, 57.6, 71.9, "Alliance")
	addon:AddTrainer(28700, "Diane Cannings", Z.DALARAN, 35.7, 28.8, "Neutral")
	addon:AddTrainer(29507, "Manfred Staller", Z.DALARAN, 34.2, 29.5, "Neutral")
	addon:AddTrainer(29508, "Andellion", Z.DALARAN, 34.5, 27.1, "Neutral")
	addon:AddTrainer(29509, "Namha Moonwater", Z.DALARAN, 36.3, 29.4, "Neutral")
	addon:AddTrainer(33581, "Kul'de", Z.ICECROWN, 71.8, 20.8, "Neutral")
	addon:AddTrainer(33612, 51302, Z.SHATTRATH_CITY, 43.8, 90.9, "Neutral")
	addon:AddTrainer(33635, "Daenril", Z.SHATTRATH_CITY, 41.9, 63.4, "Neutral")
	addon:AddTrainer(33681, "Korim", Z.SHATTRATH_CITY, 37.6, 28, "Neutral")
	addon:AddTrainer(53436, "Eustace Tanwell", Z.DUSTWALLOW_MARSH, 66.4, 45.1, "Alliance")
	addon:AddTrainer(65121, "Clean Pelt", Z.KUN_LAI_SUMMIT, 64.6, 60.9, "Neutral")
	addon:AddTrainer(66354, "Master Cannon", Z.KUN_LAI_SUMMIT, 50.6, 42.0, "Neutral")

	self.InitializeTrainers = nil
end
