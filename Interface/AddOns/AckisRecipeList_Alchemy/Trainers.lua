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
	addon:AddTrainer(1215, "Alchemist Mallory", Z.ELWYNN_FOREST, 39.8, 48.3, "Alliance")
	addon:AddTrainer(1246, "Vosur Brakthel", Z.IRONFORGE, 66.5, 55.2, "Alliance")
	addon:AddTrainer(1386, "Rogvar", Z.SWAMP_OF_SORROWS, 50, 56.2, "Horde")
	addon:AddTrainer(1470, "Ghak Healtouch", Z.LOCH_MODAN, 37, 49.2, "Alliance")
	addon:AddTrainer(2132, "Carolai Anise", Z.TIRISFAL_GLADES, 59.5, 52.2, "Horde")
	addon:AddTrainer(2391, "Serge Hinott", Z.HILLSBRAD_FOOTHILLS, 49.1, 66.4, "Horde")
	addon:AddTrainer(2837, "Jaxin Chong", Z.THE_CAPE_OF_STRANGLETHORN, 42.6, 74.8, "Neutral")
	addon:AddTrainer(3009, "Bena Winterhoof", Z.THUNDER_BLUFF, 46.6, 33.2, "Horde")
	addon:AddTrainer(3184, "Miao'zan", Z.DUROTAR, 55.5, 74, "Horde")
	addon:AddTrainer(3347, "Yelmak", Z.ORGRIMMAR, 55.68, 44.77, "Horde")
	addon:AddTrainer(3603, "Cyndra Kindwhisper", Z.TELDRASSIL, 57.6, 60.7, "Alliance")
	addon:AddTrainer(3964, "Kylanna", Z.ASHENVALE, 50.8, 67.1, "Alliance")
	addon:AddTrainer(4160, "Ainethil", Z.DARNASSUS, 53.9, 38.5, "Alliance")
	addon:AddTrainer(4611, "Doctor Herbert Halsey", Z.UNDERCITY, 47.7, 73, "Horde")
	addon:AddTrainer(4900, "Alchemist Narett", Z.DUSTWALLOW_MARSH, 64, 47.7, "Alliance")
	addon:AddTrainer(5177, "Tally Berryfizz", Z.IRONFORGE, 66.6, 55.2, "Alliance")
	addon:AddTrainer(5499, "Lilyssia Nightbreeze", Z.STORMWIND_CITY, 55.6, 85.9, "Alliance")
	addon:AddTrainer(7948, "Kylanna Windwhisper", Z.FERALAS, 32.6, 43.8, "Alliance")
	addon:AddTrainer(16161, "Arcanist Sheynathren", Z.EVERSONG_WOODS, 38.2, 72.5, "Horde")
	addon:AddTrainer(16588, "Apothecary Antonivich", Z.HELLFIRE_PENINSULA, 52.4, 36.5, "Horde")
	addon:AddTrainer(16642, "Camberon", Z.SILVERMOON_CITY, 66.1, 17.4, "Horde")
	addon:AddTrainer(16723, "Lucc", Z.THE_EXODAR, 27.5, 60.9, "Alliance")
	addon:AddTrainer(17215, "Daedal", Z.AZUREMYST_ISLE, 48.5, 51.5, "Alliance")
	addon:AddTrainer(18802, "Alchemist Gribble", Z.HELLFIRE_PENINSULA, 53.8, 65.8, "Alliance")
	addon:AddTrainer(19052, "Lorokeem", Z.SHATTRATH_CITY, 45.4, 19.5, "Neutral")
	addon:AddTrainer(26903, "Lanolis Dewdrop", Z.HOWLING_FJORD, 58.4, 62.3, "Alliance")
	addon:AddTrainer(26951, "Wilhelmina Renel", Z.HOWLING_FJORD, 78.7, 28.5, "Horde")
	addon:AddTrainer(26975, "Arthur Henslowe", Z.BOREAN_TUNDRA, 41.8, 54.3, "Horde")
	addon:AddTrainer(26987, "Falorn Nightwhisper", Z.BOREAN_TUNDRA, 57.8, 71.9, "Alliance")
	addon:AddTrainer(27023, "Apothecary Bressa", Z.DRAGONBLIGHT, 36.2, 48.7, "Horde")
	addon:AddTrainer(27029, "Apothecary Wormwick", Z.DRAGONBLIGHT, 76.9, 62.2, "Horde")
	addon:AddTrainer(28703, "Linzy Blackbolt", Z.DALARAN, 42.5, 32.1, "Neutral")
	addon:AddTrainer(33588, "Crystal Brightspark", Z.ICECROWN, 71.6, 21, "Neutral")
	addon:AddTrainer(33608, 51304, Z.SHATTRATH_CITY, 44.3, 90.4, "Neutral")
	addon:AddTrainer(33630, "Aelthin", Z.SHATTRATH_CITY, 38.6, 70.8, "Neutral")
	addon:AddTrainer(33674, "Alchemist Kanhu", Z.SHATTRATH_CITY, 38.6, 30, "Neutral")
	addon:AddTrainer(56777, "Ni Gentlepaw", Z.THE_JADE_FOREST, 46.6, 46, "Neutral")
	addon:AddTrainer(65186, "Poisoncrafter Kil'zit", Z.DREAD_WASTES, 55.6, 32.3, "Neutral")

	self.InitializeTrainers = nil
end
