-- $Id: Atlas-enUS.lua 2111 2013-09-15 18:16:38Z arithmandar $
--[[

	Atlas, a World of Warcraft instance map browser
	Copyright 2005 ~ 2010 - Dan Gilbert <dan.b.gilbert@gmail.com>
	Copyright 2010 - Lothaer <lothayer@gmail.com>, Atlas Team
	Copyright 2011 ~ 2013 - Arith Hsu, Atlas Team <atlas.addon@gmail.com>

	This file is part of Atlas.

	Atlas is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Atlas is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Atlas; if not, write to the Free Software
	Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas", "enUS", true, true);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

-- Atlas English Localization
--if ( GetLocale() ==	"enUS" ) then
-- Define the leading strings to be ignored while sorting
-- Ex: The Stockade
AtlasSortIgnore = {"the (.+)"};

-- Syntax: ["real_zone_name"] = "localized map zone name"
AtlasZoneSubstitutions = {
	["Ahn'Qiraj"] = "Temple of Ahn'Qiraj";
	["The Temple of Atal'Hakkar"] = "Sunken Temple";
--	["Throne of Tides"] = "The Abyssal Maw: Throne of the Tides";
};
--end


if AL then
--************************************************
-- UI terms and common strings
--************************************************
	AL["ATLAS_TITLE"] = "Atlas";

	AL["BINDING_HEADER_ATLAS_TITLE"] = "Atlas Bindings";
	AL["BINDING_NAME_ATLAS_TOGGLE"] = "Toggle Atlas";
	AL["BINDING_NAME_ATLAS_OPTIONS"] = "Toggle Options";
	AL["BINDING_NAME_ATLAS_AUTOSEL"] = "Auto-Select";

	AL["ATLAS_SLASH"] = "/atlas";
	AL["ATLAS_SLASH_OPTIONS"] = "options";

	AL["ATLAS_STRING_LOCATION"] = "Location";
	AL["ATLAS_STRING_LEVELRANGE"] = "Level"; -- shorten from "Level Range" as we are running out of space
	AL["ATLAS_STRING_RECLEVELRANGE"] = "Rec. Level"; -- abbrevation and shorten of "Recommended Level Range", the dungeon's recommended level range
	AL["ATLAS_STRING_PLAYERLIMIT"] = "Player Limit";
	AL["ATLAS_STRING_SELECT_CAT"] = "Select Category";
	AL["ATLAS_STRING_SELECT_MAP"] = "Select Map";
	AL["ATLAS_STRING_SEARCH"] = "Search";
	AL["ATLAS_STRING_CLEAR"] = "Clear";
	AL["ATLAS_STRING_MINLEVEL"] = "Minimum Level";

	AL["ATLAS_OPTIONS_BUTTON"] = "Options";
	AL["ATLAS_OPTIONS_SHOWBUT"] = "Show Button on Minimap";
	AL["ATLAS_OPTIONS_SHOWBUT_TIP"] = "Show Atlas button around the minimap.";
	AL["ATLAS_OPTIONS_AUTOSEL"] = "Auto-Select Instance Map";
	AL["ATLAS_OPTIONS_AUTOSEL_TIP"] = "Auto-select instance map, Atlas will detect your location to choose the best instance map for you.";
	AL["ATLAS_OPTIONS_BUTPOS"] = "Button Position";
	AL["ATLAS_OPTIONS_LOCK"] = "Lock Atlas window";
	AL["ATLAS_OPTIONS_LOCK_TIP"] = "Toggle lock / unlock of Atlas window.";
	AL["ATLAS_OPTIONS_TRANS"] = "Transparency";
	AL["ATLAS_OPTIONS_RCLICK"] = "Right-Click for World Map";
	AL["ATLAS_OPTIONS_RCLICK_TIP"] = "Enable the Right-Click in Atlas window to switch to WoW World Map.";
	AL["ATLAS_OPTIONS_RESETPOS"] = "Reset Position";
	AL["ATLAS_OPTIONS_ACRONYMS"] = "Display Acronyms";
	AL["ATLAS_OPTIONS_ACRONYMS_TIP"] = "Display the instance's acronym in the map details.";
	AL["ATLAS_OPTIONS_SCALE"] = "Atlas Frame Scale";
	AL["ATLAS_OPTIONS_BOSS_DESC"] = "Show boss description when available";
	AL["ATLAS_OPTIONS_BOSS_DESC_TIP"] = "When mouse hover the boss number, display the boss description when relative information is available.";
	AL["ATLAS_OPTIONS_BOSS_DESC_SCALE"] = "Boss Description Map ToolTip Scale";
	AL["ATLAS_OPTIONS_BUTRAD"] = "Button Radius";
	AL["ATLAS_OPTIONS_CLAMPED"] = "Clamp window to screen";
	AL["ATLAS_OPTIONS_CLAMPED_TIP"] = "Clamp Atlas window to screen, disable to allow Atlas window can be dragged outside the game screen.";
	AL["ATLAS_OPTIONS_CTRL"] = "Hold down Control for tooltips";
	AL["ATLAS_OPTIONS_CTRL_TIP"] = "Enable to show tooltips text while hold down control key and mouse over the map info. Useful when the text is too long to be displayed in the window.";
	AL["ATLAS_OPTIONS_DONTSHOWAGAIN"] = "Don't show the same information again.";
	AL["ATLAS_OPTIONS_CHECKMODULE"] = "Remind me for missing module(s) / plug-in(s).";
	AL["ATLAS_OPTIONS_CHECKMODULE_TIP"] = "Enable to perform checking if any missing Atlas module / plug-in after WoW loaded.";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN"] = "Show dungeon dropdown in colors";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN_TIP"] = "Based on the dungeon's recommended minimul level and player's level, to show the dungeon with difficulty colors. ";

	AL["ATLAS_BUTTON_CLOSE"] = "Close";
	AL["ATLAS_BUTTON_TOOLTIP_TITLE"] = "Atlas";
	AL["ATLAS_BUTTON_TOOLTIP_HINT"] = "Left-click to open Atlas.\nMiddle-click for Atlas options.\nRight-click and drag to move this button.";
	AL["ATLAS_LDB_HINT"] = "Left-Click to open Atlas.\nRight-Click for Atlas options.";
	AL["ATLAS_MINIMAPLDB_HINT"] = "Left-Click to open Atlas.\nRight-Click for Atlas options.\nLeft-click and drag to move this button.";

	AL["ATLAS_OPTIONS_CATDD"] = "Sort Instance Maps by:";
	AL["ATLAS_DDL_CONTINENT"] = "Continent";	-- Sort Instance Maps by: Continent
	AL["ATLAS_DDL_CONTINENT_EASTERN"] = "Eastern Kingdoms Instances";
	AL["ATLAS_DDL_CONTINENT_KALIMDOR"] = "Kalimdor Instances";
	AL["ATLAS_DDL_CONTINENT_OUTLAND"] = "Outland Instances";
	AL["ATLAS_DDL_CONTINENT_NORTHREND"] = "Northrend Instances";
	AL["ATLAS_DDL_CONTINENT_DEEPHOLM"] = "Deepholm Instances";
	AL["ATLAS_DDL_CONTINENT_PANDARIA"] = "Pandaria Instances";
	AL["ATLAS_DDL_LEVEL"] = "Level";		-- Sort Instance Maps by: Level
	AL["ATLAS_DDL_LEVEL_UNDER45"] = "Instances Under Level 45";
	AL["ATLAS_DDL_LEVEL_45TO60"] = "Instances Level 45-60";
	AL["ATLAS_DDL_LEVEL_60TO70"] = "Instances Level 60-70";
	AL["ATLAS_DDL_LEVEL_70TO80"] = "Instances Level 70-80";
	AL["ATLAS_DDL_LEVEL_80TO85"] = "Instances Level 80-85";
	AL["ATLAS_DDL_LEVEL_85TO90"] = "Instances Level 85-90";
	AL["ATLAS_DDL_LEVEL_90PLUS"] = "Instances Level 90+";
	AL["ATLAS_DDL_PARTYSIZE"] = "Party Size";	-- Sort Instance Maps by: Party Size
	AL["ATLAS_DDL_PARTYSIZE_5_AE"] = "Instances for 5 Players A-E";
	AL["ATLAS_DDL_PARTYSIZE_5_FS"] = "Instances for 5 Players F-S";
	AL["ATLAS_DDL_PARTYSIZE_5_TZ"] = "Instances for 5 Players T-Z";
	AL["ATLAS_DDL_PARTYSIZE_10_AN"] = "Instances for 10 Players A-N";
	AL["ATLAS_DDL_PARTYSIZE_10_OZ"] = "Instances for 10 Players O-Z";
	AL["ATLAS_DDL_PARTYSIZE_20TO40AH"] = "Instances for 20-40 Players A-H";
	AL["ATLAS_DDL_PARTYSIZE_20TO40IZ"] = "Instances for 20-40 Players I-Z";
	AL["ATLAS_DDL_EXPANSION"] = "Expansion";	-- Sort Instance Maps by: Expansion
	AL["ATLAS_DDL_EXPANSION_OLD_AO"] = "Old World Instances A-O";
	AL["ATLAS_DDL_EXPANSION_OLD_PZ"] = "Old World Instances P-Z";
	AL["ATLAS_DDL_EXPANSION_BC"] = "Burning Crusade Instances";
	AL["ATLAS_DDL_EXPANSION_WOTLK"] = "Wrath of the Lich King Instances";
	AL["ATLAS_DDL_EXPANSION_CATA"] = "Cataclysm Instances";
	AL["ATLAS_DDL_EXPANSION_MOP"] = "Mists of Pandaria Instances";
	AL["ATLAS_DDL_TYPE"] = "Type";			-- -- Sort Instance Maps by: Map Type
	AL["ATLAS_DDL_TYPE_INSTANCE_AB"] = "Instances A-B";
	AL["ATLAS_DDL_TYPE_INSTANCE_CF"] = "Instances C-F";
	AL["ATLAS_DDL_TYPE_INSTANCE_GM"] = "Instances G-M";
	AL["ATLAS_DDL_TYPE_INSTANCE_NS"] = "Instances N-S";
	AL["ATLAS_DDL_TYPE_INSTANCE_TZ"] = "Instances T-Z";
	AL["ATLAS_DDL_TYPE_ENTRANCE"] = "Entrances";

	AL["ATLAS_INSTANCE_BUTTON"] = "Instance";
	AL["ATLAS_ENTRANCE_BUTTON"] = "Entrance";
	AL["ATLAS_SEARCH_UNAVAIL"] = "Search Unavailable";

	AL["ATLAS_DEP_MSG1"] = "Atlas has detected outdated module(s).";
	AL["ATLAS_DEP_MSG2"] = "They have been disabled for this character.";
	AL["ATLAS_DEP_MSG3"] = "Delete them from your AddOns folder.";
	AL["ATLAS_DEP_OK"] = "Ok";

	AL["ATLAS_INFO"] = "Atlas Information";
	AL["ATLAS_INFO_12200"] = "Important Notice:\n\nDue to the concern of increasing addon file size, we have moved out \npart of our dungeon maps and built-in plug-ins into separated addon package.\n\nUsers who download our addons from some of the famous game web sites \nmay only get our core addon which only include the Atlas core function \nand the latest WoW expansion maps.\n\nIf you also want to see all the old expansions' maps, and also want all those \nAtlas plug-ins made by us, you have to download and install them separately.\n\nRead below forum topic for more information:\n|cff6666ffhttp://www.atlasmod.com/phpBB3/viewtopic.php?t=1522|cffffffff\n\nOr visit our website to see where to download:\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";
	AL["ATLAS_INFO_12201"] = "Please be advised that we have created a new plug-in - |cff6666ffAtlas Scenarios|cffffffff, to \nprovide the brand-new Scenarios maps introduced in WoW 5.0. \n\nCheck out our web site for more details, and don't forget to download / \ninstall it separately.\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";

	AL["ATLAS_MISSING_MODULE"] = "Atlas has detected missing module(s) / plugin(s): ";

--************************************************
-- Zone Names, Acronyms, and Common Strings
--************************************************

	--Common strings
	AL["East"] = "East";
	AL["North"] = "North";
	AL["South"] = "South";
	AL["West"] = "West";

	--World Events, Festival
	AL["Brewfest"] = "Brewfest";
	AL["Hallow's End"] = "Hallow's End";
	AL["Love is in the Air"] = "Love is in the Air";
	AL["Lunar Festival"] = "Lunar Festival";
	AL["Midsummer Festival"] = "Midsummer Festival";

	--Misc strings
	AL["Colon"] = ": "; -- The colon symbol to be used in string, ex: "Zone: Firelands
	AL["Adult"] = "Adult";
	AL["AKA"] = "AKA"; -- As Known As
	AL["Arcane Container"] = "Arcane Container";
	AL["Arms Warrior"] = "Arms Warrior";
	AL["Attunement Required"] = "Attunement Required";
	AL["Back"] = "Back";
	AL["Basement"] = "Basement";
	AL["Blacksmithing Plans"] = "Blacksmithing Plans";
	AL["Chase Begins"] = "Chase Begins";
	AL["Chase Ends"] = "Chase Ends";
	AL["Child"] = "Child";
	AL["Click to open Dungeon Journal window."] = "Click to open Dungeon Journal window.";
	AL["Connection"] = "Connection";
	AL["Elevator"] = "Elevator";
	AL["End"] = "End";
	AL["Engineer"] = "Engineer";
	AL["Entrance"] = "Entrance";
	AL["Event"] = "Event";
	AL["Exalted"] = "Exalted";
	AL["Exit"] = "Exit";
	AL["Fourth Stop"] = "Fourth Stop";
	AL["Front"] = "Front";
	AL["Ghost"] = "Ghost";
	AL["Graveyard"] = "Graveyard";
	AL["Heroic"] = "Heroic";
	AL["Holy Paladin"] = "Holy Paladin";
	AL["Holy Priest"] = "Holy Priest";
	AL["Hunter"] = "Hunter";
	AL["Imp"] = "Imp";
	AL["Key"] = "Key";
	AL["Lower"] = "Lower";
	AL["Mage"] = "Mage";
	AL["Meeting Stone"] = "Meeting Stone";
	AL["Middle"] = "Middle";
	AL["Monk"] = "Monk";
	AL["Moonwell"] = "Moonwell";
	AL["Optional"] = "Optional";
	AL["Orange"] = "Orange";
	AL["Outside"] = "Outside";
	AL["Paladin"] = "Paladin";
	AL["Portal"] = "Portal";
	AL["Priest"] = "Priest";
	AL["Protection Warrior"] = "Protection Warrior";
	AL["Purple"] = "Purple";
	AL["Random"] = "Random";
	AL["Rare"] = "Rare";
	AL["Reputation"] = "Reputation";
	AL["Repair"] = "Repair";
	AL["Retribution Paladin"] = "Retribution Paladin";
	AL["Rewards"] = "Rewards";
	AL["Rogue"] = "Rogue";
	AL["Second Stop"] = "Second Stop";
	AL["Shadow Priest"] = "Shadow Priest";
	AL["Shaman"] = "Shaman";
	AL["Spawn Point"] = "Spawn Point";
	AL["Start"] = "Start";
	AL["Summon"] = "Summon";
	AL["Teleporter"] = "Teleporter";
	AL["Teleporter destination"] = "Teleporter destination";
	AL["Third Stop"] = "Third Stop";
	AL["Top"] = "Top";
	AL["Tunnel"] = "Tunnel";
	AL["Underwater"] = "Underwater";
	AL["Upper"] = "Upper";
	AL["Varies"] = "Varies";
	AL["Wanders"] = "Wanders";
	AL["Warlock"] = "Warlock";
	AL["Warrior"] = "Warrior";
	AL["Wave 5"] = "Wave 5";
	AL["Wave 6"] = "Wave 6";
	AL["Wave 10"] = "Wave 10";
	AL["Wave 12"] = "Wave 12";
	AL["Wave 18"] = "Wave 18";
	AL["MapsNotFound"] = "Current selected dungeon does not have a \ncorresponding map image associated with. \n\nPlease make sure you have installed \nthe corresponding Atlas map module(s).";

	--Classic Acronyms
	AL["AQ"] = "AQ"; -- Ahn'Qiraj
	AL["AQ20"] = "AQ20"; -- Ruins of Ahn'Qiraj
	AL["AQ40"] = "AQ40"; -- Temple of Ahn'Qiraj
	AL["BFD"] = "BFD"; -- Blackfathom Deeps
	AL["BRD"] = "BRD"; -- Blackrock Depths
	AL["BRM"] = "BRM"; -- Blackrock Mountain
	AL["BWL"] = "BWL"; -- Blackwing Lair
	AL["DM"] = "DM"; -- Dire Maul
	AL["Gnome"] = "Gnome"; -- Gnomeregan
	AL["Halls"] = "Halls"; -- Scarlet Halls
	AL["LBRS"] = "LBRS"; -- Lower Blackrock Spire
	AL["Mara"] = "Mara"; -- Maraudon
	AL["MC"] = "MC"; -- Molten Core
	AL["RFC"] = "RFC"; -- Ragefire Chasm
	AL["RFD"] = "RFD"; -- Razorfen Downs
	AL["RFK"] = "RFK"; -- Razorfen Kraul
	AL["Scholo"] = "Scholo"; -- Scholomance
	AL["SFK"] = "SFK"; -- Shadowfang Keep
	AL["SM"] = "SM"; -- Scarlet Monastery
	AL["ST"] = "ST"; -- Sunken Temple
	AL["Strat"] = "Strat"; -- Stratholme
	AL["Stocks"] = "Stocks"; -- The Stockade
	AL["UBRS"] = "UBRS"; -- Upper Blackrock Spire
	AL["Ulda"] = "Ulda"; -- Uldaman
	AL["VC"] = "VC"; -- The Deadmines
	AL["WC"] = "WC"; -- Wailing Caverns
	AL["ZF"] = "ZF"; -- Zul'Farrak

	--BC Acronyms
	AL["AC"] = "AC"; -- Auchenai Crypts
	AL["Arca"] = "Arca"; -- The Arcatraz
	AL["Auch"] = "Auch"; -- Auchindoun
	AL["BF"] = "BF"; -- The Blood Furnace
	AL["BT"] = "BT"; -- Black Temple
	AL["Bota"] = "Bota"; -- The Botanica
	AL["CoT"] = "CoT"; -- Caverns of Time
	AL["CoT1"] = "CoT1"; -- Old Hillsbrad Foothills
	AL["CoT2"] = "CoT2"; -- The Black Morass
	AL["CoT3"] = "CoT3"; -- Hyjal Summit
	AL["CR"] = "CR"; -- Coilfang Reservoir
	AL["GL"] = "GL"; -- Gruul's Lair
	AL["HC"] = "HC"; -- Hellfire Citadel
	AL["Kara"] = "Kara"; -- Karazhan
	AL["MaT"] = "MT"; -- Magisters' Terrace
	AL["Mag"] = "Mag"; -- Magtheridon's Lair
	AL["Mech"] = "Mech"; -- The Mechanar
	AL["MT"] = "MT"; -- Mana-Tombs
	AL["Ramp"] = "Ramp"; -- Hellfire Ramparts
	AL["SSC"] = "SSC"; -- Serpentshrine Cavern
	AL["Seth"] = "Seth"; -- Sethekk Halls
	AL["SH"] = "SH"; -- The Shattered Halls
	AL["SL"] = "SL"; -- Shadow Labyrinth
	AL["SP"] = "SP"; -- The Slave Pens
	AL["SuP"] = "SP"; -- Sunwell Plateau
	AL["SV"] = "SV"; -- The Steamvault
	AL["TK"] = "TK"; -- Tempest Keep
	AL["UB"] = "UB"; -- The Underbog

	--WotLK Acronyms
	AL["AK, Kahet"] = "AK, Kahet"; -- Ahn'kahet
	AL["AN, Nerub"] = "AN, Nerub"; -- Azjol-Nerub
	AL["Champ"] = "Champ"; -- Trial of the Champion
	AL["CoT-Strat"] = "CoT-Strat"; -- Culling of Stratholme
	AL["Crus"] = "Crus"; -- Trial of the Crusader
	AL["DTK"] = "DTK"; -- Drak'Tharon Keep
	AL["FoS"] = "FoS"; -- The Forge of Souls
	AL["FH1"] = "FH1"; -- The Forge of Souls
	AL["Gun"] = "Gun"; -- Gundrak
	AL["HoL"] = "HoL"; -- Halls of Lightning
	AL["HoR"] = "HoR"; -- Halls of Reflection
	AL["FH3"] = "FH3"; -- Halls of Reflection
	AL["HoS"] = "HoS"; -- Halls of Stone
	AL["IC"] = "IC"; -- Icecrown Citadel
	AL["Nax"] = "Nax"; -- Naxxramas
	AL["Nex, Nexus"] = "Nex, Nexus"; -- The Nexus
	AL["Ocu"] = "Ocu"; -- The Oculus
	AL["Ony"] = "Ony"; -- Onyxia's Lair
	AL["OS"] = "OS"; -- The Obsidian Sanctum
	AL["PoS"] = "PoS"; -- Pit of Saron
	AL["FH2"] = "FH2"; -- Pit of Saron
	AL["RS"] = "RS"; -- The Ruby Sanctum
	AL["TEoE"] = "TEoE"; -- The Eye of Eternity
	AL["UK, Keep"] = "UK, Keep"; -- Utgarde Keep
	AL["Uldu"] = "Uldu"; -- Ulduar
	AL["UP, Pinn"] = "UP, Pinn"; -- Utgarde Pinnacle
	AL["VH"] = "VH"; -- The Violet Hold
	AL["VoA"] = "VoA"; -- Vault of Archavon

	--Zones not included in LibBabble-Zone
	AL["Crusaders' Coliseum"] = "Crusaders' Coliseum"; 

	--Cataclysm Acronyms
	AL["BH"] = "BH"; --Baradin Hold
	AL["BoT"] = "BoT"; --Bastion of Twilight
	AL["BRC"] = "BRC"; --Blackrock Caverns
	AL["BWD"] = "BWD"; --Blackwing Descent
	AL["CoT-DS"] = "CoT-DS"; --Caverns of Time: Dragon Soul
	AL["CoT-ET"] = "CoT-ET"; --Caverns of Time: End Time
	AL["CoT-HoT"] = "CoT-HoT"; --Caverns of Time: Hour of Twilight
	AL["CoT-WoE"] = "CoT-WoE"; --Caverns of Time: Well of Eternity
	AL["FL"] = "FL"; --Firelands
	AL["GB"] = "GB"; --Grim Batol
	AL["HoO"] = "HoO"; --Halls of Origination
	AL["LCoT"] = "LCoT"; --Lost City of the Tol'vir 
	AL["TSC"] = "TSC"; --The Stonecore
	AL["TWT"] = "TWT"; --Throne of the Four Winds
	AL["ToTT"] = "ToTT"; --Throne of the Tides
	AL["VP"] = "VP"; --The Vortex Pinnacle
	AL["ZA"] = "ZA"; -- Zul'Aman
	AL["ZG"] = "ZG"; --Zul'Gurub

	--MoP Acronyms
	AL["GSS"] = "GSS"; --Gate of the Setting Sun
	AL["HoF"] = "HoF"; --Heart of Fear
	AL["MP"] = "MP"; --Mogu'shan Palace
	AL["MV"] = "MV"; --Mogu'shan Vaults
	AL["SPM"] = "SPM"; --Shado-Pan Monastery
	AL["SNT"] = "SNT"; --Siege of Niuzao Temple
	AL["SB"] = "SB"; --Stormstout Brewery
	AL["SoO"] = "SoO"; --Siege of Orgrimmar
	AL["TJS"] = "TJS"; --Temple of the Jade Serpent
	AL["TES"] = "TES"; --Terrace of Endless Spring
	AL["ToT"] = "ToT"; --Throne of Thunder

	--Map sections
	AL["MapA"] = " [A]"; -- For example: Shado-Pan Monastery [A]
	AL["MapB"] = " [B]";
	AL["MapC"] = " [C]";
	AL["MapD"] = " [D]";
	AL["MapE"] = " [E]";
	AL["MapF"] = " [F]";

--************************************************
-- Instance Entrance Maps
--************************************************

	--Auchindoun (Entrance)
	AL["Clarissa"] = "Clarissa";
	AL["Greatfather Aldrimus"] = "Greatfather Aldrimus";
	AL["Ha'lei"] = "Ha'lei";
	AL["Horvon the Armorer <Armorsmith>"] = "Horvon the Armorer <Armorsmith>";
	AL["Ramdor the Mad"] = "Ramdor the Mad";
	AL["Nexus-Prince Haramad"] = "Nexus-Prince Haramad";
	AL["\"Slim\" <Shady Dealer>"] = "\"Slim\" <Shady Dealer>";
	AL["\"Captain\" Kaftiz"] = "\"Captain\" Kaftiz";
	AL["Dealer Tariq <Shady Dealer>"] = "Dealer Tariq <Shady Dealer>";
	AL["Provisioner Tsaalt"] = "Provisioner Tsaalt";

	--Blackfathom Deeps (Entrance)

	--Blackrock Mountain (Entrance)
	AL["Bodley"] = "Bodley";
	AL["Lothos Riftwaker"] = "Lothos Riftwaker";
	AL["Orb of Command"] = "Orb of Command";
	AL["Scarshield Quartermaster <Scarshield Legion>"] = "Scarshield Quartermaster <Scarshield Legion>";
	AL["The Behemoth"] = "The Behemoth";

	--Caverns of Time (Entrance)
	AL["Steward of Time <Keepers of Time>"] = "Steward of Time <Keepers of Time>";
	AL["Alexston Chrome <Tavern of Time>"] = "Alexston Chrome <Tavern of Time>";
	AL["Yarley <Armorer>"] = "Yarley <Armorer>";
	AL["Bortega <Reagents & Poison Supplies>"] = "Bortega <Reagents & Poison Supplies>";
	AL["Alurmi <Keepers of Time Quartermaster>"] = "Alurmi <Keepers of Time Quartermaster>";
	AL["Galgrom <Provisioner>"] = "Galgrom <Provisioner>";
	AL["Zaladormu"] = "Zaladormu";
	AL["Soridormi <The Scale of Sands>"] = "Soridormi <The Scale of Sands>";
	AL["Arazmodu <The Scale of Sands>"] = "Arazmodu <The Scale of Sands>";
	AL["Andormu <Keepers of Time>"] = "Andormu <Keepers of Time>";
	AL["Nozari <Keepers of Time>"] = "Nozari <Keepers of Time>";
	AL["Anachronos <Keepers of Time>"] = "Anachronos <Keepers of Time>";

	--Caverns of Time: Hyjal (Entrance)
	AL["Indormi <Keeper of Ancient Gem Lore>"] = "Indormi <Keeper of Ancient Gem Lore>";
	AL["Tydormu <Keeper of Lost Artifacts>"] = "Tydormu <Keeper of Lost Artifacts>";

	--Coilfang Reservoir (Entrance)
	AL["Mortog Steamhead"] = "Mortog Steamhead";

	--Dire Maul (Entrance)
	AL["Dire Pool"] = "Dire Pool";
	AL["Dire Maul Arena"] = "Dire Maul Arena";
	AL["Elder Mistwalker"] = "Elder Mistwalker";

	--Gnomeregan (Entrance)
	AL["Torben Zapblast <Teleportation Specialist>"] = "Torben Zapblast <Teleportation Specialist>";

	--Hellfire Citadel (Entrance)
	AL["Steps and path to the Blood Furnace"] = "Steps and path to the Blood Furnace";
	AL["Path to the Hellfire Ramparts and Shattered Halls"] = "Path to the Hellfire Ramparts and Shattered Halls";
	AL["Meeting Stone of Magtheridon's Lair"] = "Meeting Stone of Magtheridon's Lair";
	AL["Meeting Stone of Hellfire Citadel"] = "Meeting Stone of Hellfire Citadel";

	--Icecrown Citadel (Entrance)

	--Karazhan (Entrance)
	AL["Archmage Leryda"] = "Archmage Leryda";
	AL["Archmage Alturus"] = "Archmage Alturus";
	AL["Apprentice Darius"] = "Apprentice Darius";
	AL["Stairs to Underground Pond"] = "Stairs to Underground Pond";
	AL["Stairs to Underground Well"] = "Stairs to Underground Well";
	AL["Charred Bone Fragment"] = "Charred Bone Fragment";

	--Maraudon (Entrance)
	AL["The Nameless Prophet"] = "The Nameless Prophet";
	AL["Cursed Centaur"] = "Cursed Centaur";
	AL["Kherrah"] = "Kherrah";

	--Scarlet Monastery (Entrance)

	--The Deadmines (Entrance)

	--Sunken Temple (Entrance)
	AL["Priestess Udum'bra"] = "Priestess Udum'bra";
	AL["Gomora the Bloodletter"] = "Gomora the Bloodletter";
	AL["Captain Wyrmak"] = "Captain Wyrmak";

	--Uldaman (Entrance)

	--Ulduar (Entrance)
	AL["Shavalius the Fancy <Flight Master>"] = "Shavalius the Fancy <Flight Master>";
	AL["Chester Copperpot <General & Trade Supplies>"] = "Chester Copperpot <General & Trade Supplies>";
	AL["Slosh <Food & Drink>"] = "Slosh <Food & Drink>";

	--Wailing Caverns (Entrance)

--************************************************
-- Kalimdor Instances (Classic)
--************************************************

	--Blackfathom Deeps
	AL["Shrine of Gelihast"] = "Shrine of Gelihast";
	AL["Fathom Stone"] = "Fathom Stone";
	AL["Lorgalis Manuscript"] = "Lorgalis Manuscript";
	AL["Scout Thaelrid"] = "Scout Thaelrid";
	AL["Flaming Eradicator"] = "Flaming Eradicator";
	AL["Altar of the Deeps"] = "Altar of the Deeps";
	AL["Ashelan Northwood"] = "Ashelan Northwood";
	AL["Relwyn Shadestar"] = "Relwyn Shadestar";
	AL["Sentinel Aluwyn"] = "Sentinel Aluwyn";
	AL["Sentinel-trainee Issara"] = "Sentinel-trainee Issara";
	AL["Je'neu Sancrea <The Earthen Ring>"] = "Je'neu Sancrea <The Earthen Ring>";
	AL["Zeya"] = "Zeya";

	--Dire Maul (East)
	AL["\"Ambassador\" Dagg'thol"] = "\"Ambassador\" Dagg'thol";
	AL["Furgus Warpwood"] = "Furgus Warpwood";
	AL["Old Ironbark"] = "Old Ironbark";
	AL["Ironbark the Redeemed"] = "Ironbark the Redeemed";

	--Dire Maul (North)
	AL["Druid of the Talon"] = "Druid of the Talon";
	AL["Stonemaul Ogre"] = "Stonemaul Ogre";
	AL["Knot Thimblejack"] = "Knot Thimblejack";

	--Dire Maul (West)
	AL["Ferra"] = "Ferra";
	AL["Estulan <The Highborne>"] = "Estulan <The Highborne>";
	AL["Shen'dralar Watcher"] = "Shen'dralar Watcher";
	AL["Pylons"] = "Pylons";
	AL["Ancient Equine Spirit"] = "Ancient Equine Spirit";
	AL["Shen'dralar Ancient"] = "Shen'dralar Ancient";
	AL["Falrin Treeshaper"] = "Falrin Treeshaper";
	AL["Lorekeeper Lydros"] = "Lorekeeper Lydros";
	AL["Lorekeeper Javon"] = "Lorekeeper Javon";
	AL["Lorekeeper Kildrath"] = "Lorekeeper Kildrath";
	AL["Lorekeeper Mykos"] = "Lorekeeper Mykos";
	AL["Shen'dralar Provisioner"] = "Shen'dralar Provisioner";

	--Maraudon	
	AL["Elder Splitrock"] = "Elder Splitrock";
	AL["Celebras the Redeemed"] = "Celebras the Redeemed";

	--Ragefire Chasm
	AL["Commander Bagran"] = "Commander Bagran";
	AL["Invoker Xorenth"] = "Invoker Xorenth";
	AL["Scout Cage"] = "Scout Cage";

	--Razorfen Downs
	AL["Koristrasza"] = "Koristrasza";
	AL["Belnistrasz"] = "Belnistrasz";

	--Razorfen Kraul
	AL["Auld Stonespire"] = "Auld Stonespire";
	AL["Spirit of Agamaggan <Ancient>"] = "Spirit of Agamaggan <Ancient>";
	AL["Willix the Importer"] = "Willix the Importer";

	--Ruins of Ahn'Qiraj
	AL["Four Kaldorei Elites"] = "Four Kaldorei Elites";
	AL["Captain Qeez"] = "Captain Qeez";
	AL["Captain Tuubid"] = "Captain Tuubid";
	AL["Captain Drenn"] = "Captain Drenn";
	AL["Captain Xurrem"] = "Captain Xurrem";
	AL["Major Yeggeth"] = "Major Yeggeth";
	AL["Major Pakkon"] = "Major Pakkon";
	AL["Colonel Zerran"] = "Colonel Zerran";
	AL["Safe Room"] = "Safe Room";

	--Temple of Ahn'Qiraj
	AL["Andorgos <Brood of Malygos>"] = "Andorgos <Brood of Malygos>";
	AL["Vethsera <Brood of Ysera>"] = "Vethsera <Brood of Ysera>";
	AL["Kandrostrasz <Brood of Alexstrasza>"] = "Kandrostrasz <Brood of Alexstrasza>";
	AL["Arygos"] = "Arygos";
	AL["Caelestrasz"] = "Caelestrasz";
	AL["Merithra of the Dream"] = "Merithra of the Dream";

	--Wailing Caverns
	AL["Ebru <Disciple of Naralex>"] = "Ebru <Disciple of Naralex>"; -- 5768
	AL["Nalpak <Disciple of Naralex>"] = "Nalpak <Disciple of Naralex>"; -- 5767
	AL["Muyoh <Disciple of Naralex>"] = "Muyoh <Disciple of Naralex>";  -- 3678
	AL["Naralex"] = "Naralex"; -- 3679

	--Zul'Farrak
	AL["Chief Engineer Bilgewhizzle <Gadgetzan Water Co.>"] = "Chief Engineer Bilgewhizzle <Gadgetzan Water Co.>";
	AL["Mazoga's Spirit"] = "Mazoga's Spirit";
	AL["Tran'rek"] = "Tran'rek";
	AL["Weegli Blastfuse"] = "Weegli Blastfuse";
	AL["Raven"] = "Raven";
	AL["Elder Wildmane"] = "Elder Wildmane";

--****************************
-- Eastern Kingdoms Instances (Classic)
--****************************

	--Blackrock Depths
	AL["The Black Anvil"] = "The Black Anvil";
	AL["The Vault"] = "The Vault";
	AL["Watchman Doomgrip"] = "Watchman Doomgrip";
	AL["Elder Morndeep"] = "Elder Morndeep";
	AL["Schematic: Field Repair Bot 74A"] = "Schematic: Field Repair Bot 74A";
	AL["Private Rocknot"] = "Private Rocknot";
	AL["Mistress Nagmara"] = "Mistress Nagmara";
	AL["Jalinda Sprig <Morgan's Militia>"] = "Jalinda Sprig <Morgan's Militia>";
	AL["Oralius <Morgan's Militia>"] = "Oralius <Morgan's Militia>";
	AL["Thal'trak Proudtusk <Kargath Expeditionary Force>"] = "Thal'trak Proudtusk <Kargath Expeditionary Force>";
	AL["Galamav the Marksman <Kargath Expeditionary Force>"] = "Galamav the Marksman <Kargath Expeditionary Force>";
	AL["Maxwort Uberglint"] = "Maxwort Uberglint";
	AL["Tinkee Steamboil"] = "Tinkee Steamboil";
	AL["Yuka Screwspigot <Engineering Supplies>"] = "Yuka Screwspigot <Engineering Supplies>";
	AL["Abandonded Mole Machine"] = "Abandonded Mole Machine";
	AL["Kevin Dawson <Morgan's Militia>"] = "Kevin Dawson <Morgan's Militia>";
	AL["Lexlort <Kargath Expeditionary Force>"] = "Lexlort <Kargath Expeditionary Force>";
	AL["Prospector Seymour <Morgan's Militia>"] = "Prospector Seymour <Morgan's Militia>";
	AL["Razal'blade <Kargath Expeditionary Force>"] = "Razal'blade <Kargath Expeditionary Force>";
	AL["The Shadowforge Lock"] = "The Shadowforge Lock";
	AL["Mayara Brightwing <Morgan's Militia>"] = "Mayara Brightwing <Morgan's Militia>";
	AL["Hierophant Theodora Mulvadania <Kargath Expeditionary Force>"] = "Hierophant Theodora Mulvadania <Kargath Expeditionary Force>";
	AL["Lokhtos Darkbargainer <The Thorium Brotherhood>"] = "Lokhtos Darkbargainer <The Thorium Brotherhood>";
	AL["Mountaineer Orfus <Morgan's Militia>"] = "Mountaineer Orfus <Morgan's Militia>";
	AL["Thunderheart <Kargath Expeditionary Force>"] = "Thunderheart <Kargath Expeditionary Force>";
	AL["Marshal Maxwell <Morgan's Militia>"] = "Marshal Maxwell <Morgan's Militia>";
	AL["Warlord Goretooth <Kargath Expeditionary Force>"] = "Warlord Goretooth <Kargath Expeditionary Force>";
	AL["The Black Forge"] = "The Black Forge";
	AL["Core Fragment"] = "Core Fragment";
	AL["Shadowforge Brazier"] = "Shadowforge Brazier";

	--Blackrock Spire (Lower)
	AL["Urok's Tribute Pile"] = "Urok's Tribute Pile";
	AL["Acride <Scarshield Legion>"] = "Acride <Scarshield Legion>";
	AL["Elder Stonefort"] = "Elder Stonefort";
	AL["Roughshod Pike"] = "Roughshod Pike";

	--Blackrock Spire (Upper)
	AL["Finkle Einhorn"] = "Finkle Einhorn";
	AL["Drakkisath's Brand"] = "Drakkisath's Brand";
	AL["Father Flame"] = "Father Flame";

	--Blackwing Lair
	AL["Orb of Domination"] = "Orb of Domination";
	AL["Master Elemental Shaper Krixix"] = "Master Elemental Shaper Krixix";

	--Gnomeregan
	AL["Chomper"] = "Chomper";
	AL["Blastmaster Emi Shortfuse"] = "Blastmaster Emi Shortfuse";
	AL["Murd Doc <S.A.F.E.>"] = "Murd Doc <S.A.F.E.>";
	AL["Tink Sprocketwhistle <Engineering Supplies>"] = "Tink Sprocketwhistle <Engineering Supplies>";
	AL["The Sparklematic 5200"] = "The Sparklematic 5200";
	AL["Mail Box"] = "Mail Box";
	AL["B.E Barechus <S.A.F.E.>"] = "B.E Barechus <S.A.F.E.>";
	AL["Face <S.A.F.E.>"] = "Face <S.A.F.E.>";
	AL["Hann Ibal <S.A.F.E.>"] = "Hann Ibal <S.A.F.E.>";

	--Molten Core

	--Scarlet Halls
	AL["Commander Lindon"] = "Commander Lindon";
	AL["Hooded Crusader"] = "Hooded Crusader";
	AL["Bucket of Meaty Dog Food"] = "Bucket of Meaty Dog Food";
	AL["Reinforced Archery Target"] = "Reinforced Archery Target";

	--Scarlet Monastery

	--Scholomance
	AL["Instructor Chillheart's Phylactery"] = "Instructor Chillheart's Phylactery";
	AL["Professor Slate"] = "Professor Slate";
	AL["Polyformic Acid Potion"] = "Polyformic Acid Potion";
	AL["Talking Skull"] = "Talking Skull";
	AL["In the Shadow of the Light"] = "In the Shadow of the Light";
	AL["Kel'Thuzad's Deep Knowledge"] = "Kel'Thuzad's Deep Knowledge";
	AL["Forbidden Rites and other Rituals Necromantic"] = "Forbidden Rites and other Rituals Necromantic";
	AL["Coffer of Forgotten Souls"] = "Coffer of Forgotten Souls";
	AL["The Dark Grimoire"] = "The Dark Grimoire";

	--Shadowfang Keep
	AL["Apothecary Trio"] = "Apothecary Trio";
	AL["Apothecary Hummel <Crown Chemical Co.>"] = "Apothecary Hummel <Crown Chemical Co.>";
	AL["Apothecary Baxter <Crown Chemical Co.>"] = "Apothecary Baxter <Crown Chemical Co.>";
	AL["Apothecary Frye <Crown Chemical Co.>"] = "Apothecary Frye <Crown Chemical Co.>";
	AL["Packleader Ivar Bloodfang"] = "Packleader Ivar Bloodfang";
	AL["Deathstalker Commander Belmont"] = "Deathstalker Commander Belmont";
	AL["Haunted Stable Hand"] = "Haunted Stable Hand";
	AL["Investigator Fezzen Brasstacks"] = "Investigator Fezzen Brasstacks";

	--Stratholme - Crusader's Square
	AL["Crusade Commander Eligor Dawnbringer <Brotherhood of the Light>"] = "Crusade Commander Eligor Dawnbringer <Brotherhood of the Light>";
	AL["Master Craftsman Wilhelm <Brotherhood of the Light>"] = "Master Craftsman Wilhelm <Brotherhood of the Light>";
	AL["Packmaster Stonebruiser <Brotherhood of the Light>"] = "Packmaster Stonebruiser <Brotherhood of the Light>";
	AL["Stratholme Courier"] = "Stratholme Courier";
	AL["Fras Siabi's Postbox"] = "Fras Siabi's Postbox";
	AL["King's Square Postbox"] = "King's Square Postbox";
	AL["Festival Lane Postbox"] = "Festival Lane Postbox";
	AL["Elder Farwhisper"] = "Elder Farwhisper";
	AL["Market Row Postbox"] = "Market Row Postbox";

	--Stratholme - The Gauntlet
	AL["Elders' Square Postbox"] = "Elders' Square Postbox";
	AL["Archmage Angela Dosantos <Brotherhood of the Light>"] = "Archmage Angela Dosantos <Brotherhood of the Light>";
	AL["Crusade Commander Korfax <Brotherhood of the Light>"] = "Crusade Commander Korfax <Brotherhood of the Light>";

	--The Deadmines
	AL["Lieutenant Horatio Laine"] = "Lieutenant Horatio Laine";
	AL["Kagtha"] = "Kagtha";
	AL["Slinky Sharpshiv"] = "Slinky Sharpshiv";
	AL["Quartermaster Lewis <Quartermaster>"] = "Quartermaster Lewis <Quartermaster>";
	AL["Miss Mayhem"] = "Miss Mayhem";
	AL["Vend-O-Tron D-Luxe"] = "Vend-O-Tron D-Luxe";

	--The Stockade
	AL["Rifle Commander Coe"] = "Rifle Commander Coe";
	AL["Warden Thelwater"] = "Warden Thelwater";
	AL["Nurse Lillian"] = "Nurse Lillian";

	--The Sunken Temple
	AL["Lord Itharius"] = "Lord Itharius";
	AL["Elder Starsong"] = "Elder Starsong";

	--Uldaman
	AL["Baelog's Chest"] = "Baelog's Chest";
	AL["Kand Sandseeker <Explorer's League>"] = "Kand Sandseeker <Explorer's League>";
	AL["Lead Prospector Durdin <Explorer's League>"] = "Lead Prospector Durdin <Explorer's League>";
	AL["Olga Runesworn <Explorer's League>"] = "Olga Runesworn <Explorer's League>";
	AL["Aoren Sunglow <The Reliquary>"] = "Aoren Sunglow <The Reliquary>";
	AL["High Examiner Tae'thelan Bloodwatcher <The Reliquary>"] = "High Examiner Tae'thelan Bloodwatcher <The Reliquary>";
	AL["Lidia Sunglow <The Reliquary>"] = "Lidia Sunglow <The Reliquary>";
	AL["Ancient Treasure"] = "Ancient Treasure";
	AL["The Discs of Norgannon"] = "The Discs of Norgannon";

--*******************
-- Burning Crusade Instances
--*******************

	--Auch: Auchenai Crypts
	AL["Draenei Spirit"] = "Draenei Spirit";
	AL["Avatar of the Martyred"] = "Avatar of the Martyred";
	AL["D'ore"] = "D'ore";
	AL["Tormented Soulpriest"] = "Tormented Soulpriest";

	--Auch: Mana-Tombs
	AL["Artificer Morphalius"] = "Artificer Morphalius";
	AL["Mamdy the \"Ologist\""] = "Mamdy the \"Ologist\"";
	AL["Shadow Lord Xiraxis"] = "Shadow Lord Xiraxis";
	AL["Ambassador Pax'ivi"] = "Ambassador Pax'ivi";
	AL["Cryo-Engineer Sha'heen"] = "Cryo-Engineer Sha'heen";
	AL["Ethereal Transporter Control Panel"] = "Ethereal Transporter Control Panel";

	--Auch: Sethekk Halls
	AL["Isfar"] = "Isfar";
	AL["Dealer Vijaad"] = "Dealer Vijaad";
	AL["Lakka"] = "Lakka";
	AL["The Saga of Terokk"] = "The Saga of Terokk";

	--Auch: Shadow Labyrinth
	AL["Field Commander Mahfuun"] = "Field Commander Mahfuun";
	AL["Spy Grik'tha"] = "Spy Grik'tha";
	AL["The Codex of Blood"] = "The Codex of Blood";
	AL["First Fragment Guardian"] = "First Fragment Guardian";
	AL["Spy To'gun"] = "Spy To'gun";

	--Black Temple (Start)
	AL["Towards Reliquary of Souls"] = "Towards Reliquary of Souls";
	AL["Towards Teron Gorefiend"] = "Towards Teron Gorefiend";
	AL["Towards Illidan Stormrage"] = "Towards Illidan Stormrage";
	AL["Spirit of Olum"] = "Spirit of Olum";
	AL["Spirit of Udalo"] = "Spirit of Udalo";
	AL["Aluyen <Reagents>"] = "Aluyen <Reagents>";
	AL["Okuno <Ashtongue Deathsworn Quartermaster>"] = "Okuno <Ashtongue Deathsworn Quartermaster>";
	AL["Seer Kanai"] = "Seer Kanai";

	--Black Temple (Basement)

	--Black Temple (Top)

	--CFR: Serpentshrine Cavern
	AL["Seer Olum"] = "Seer Olum";

	--CFR: The Slave Pens
	AL["Nahuud"] = "Nahuud";
	AL["Watcher Jhang"] = "Watcher Jhang";
	AL["Weeder Greenthumb"] = "Weeder Greenthumb";
	AL["Skar'this the Heretic"] = "Skar'this the Heretic";
	AL["Naturalist Bite"] = "Naturalist Bite";

	--CFR: The Steamvault
	AL["Windcaller Claw"] = "Windcaller Claw";
	AL["Main Chambers Access Panel"] = "Main Chambers Access Panel";
	AL["Second Fragment Guardian"] = "Second Fragment Guardian";

	--CFR: The Underbog
	AL["T'shu"] = "T'shu";
	AL["The Underspore"] = "The Underspore";
	AL["Earthbinder Rayge"] = "Earthbinder Rayge";

	--CoT: The Black Morass
	AL["Sa'at <Keepers of Time>"] = "Sa'at <Keepers of Time>";

	--CoT: Hyjal Summit
	AL["Lady Jaina Proudmoore"] = "Lady Jaina Proudmoore";
	AL["Thrall <Warchief>"] = "Thrall <Warchief>";
	AL["Tyrande Whisperwind <High Priestess of Elune>"] = "Tyrande Whisperwind <High Priestess of Elune>";

	--CoT: Old Hillsbrad Foothills
	AL["Erozion"] = "Erozion";
	AL["Brazen"] = "Brazen";
	AL["Landing Spot"] = "Landing Spot";
	AL["Thrall"] = "Thrall";
	AL["Taretha"] = "Taretha";
	AL["Don Carlos"] = "Don Carlos";
	AL["Guerrero"] = "Guerrero";
	AL["Thomas Yance <Travelling Salesman>"] = "Thomas Yance <Travelling Salesman>";
	AL["Aged Dalaran Wizard"] = "Aged Dalaran Wizard";
	AL["Jonathan Revah"] = "Jonathan Revah";
	AL["Jerry Carter"] = "Jerry Carter";
	AL["Helcular"] = "Helcular";
	AL["Farmer Kent"] = "Farmer Kent";
	AL["Sally Whitemane"] = "Sally Whitemane";
	AL["Renault Mograine"] = "Renault Mograine";
	AL["Little Jimmy Vishas"] = "Little Jimmy Vishas";
	AL["Herod the Bully"] = "Herod the Bully";
	AL["Nat Pagle"] = "Nat Pagle";
	AL["Hal McAllister"] = "Hal McAllister";
	AL["Zixil <Aspiring Merchant>"] = "Zixil <Aspiring Merchant>";
	AL["Overwatch Mark 0 <Protector>"] = "Overwatch Mark 0 <Protector>";
	AL["Southshore Inn"] = "Southshore Inn";
	AL["Captain Edward Hanes"] = "Captain Edward Hanes";
	AL["Captain Sanders"] = "Captain Sanders";
	AL["Commander Mograine"] = "Commander Mograine";
	AL["Isillien"] = "Isillien";
	AL["Abbendis"] = "Abbendis";
	AL["Fairbanks"] = "Fairbanks";
	AL["Taelan"] = "Taelan";
	AL["Barkeep Kelly <Bartender>"] = "Barkeep Kelly <Bartender>";
	AL["Frances Lin <Barmaid>"] = "Frances Lin <Barmaid>";
	AL["Chef Jessen <Speciality Meat & Slop>"] = "Chef Jessen <Speciality Meat & Slop>";
	AL["Stalvan Mistmantle"] = "Stalvan Mistmantle";
	AL["Phin Odelic <The Kirin Tor>"] = "Phin Odelic <The Kirin Tor>";
	AL["Magistrate Henry Maleb"] = "Magistrate Henry Maleb";
	AL["Raleigh the True"] = "Raleigh the True";
	AL["Nathanos Marris"] = "Nathanos Marris";
	AL["Bilger the Straight-laced"] = "Bilger the Straight-laced";
	AL["Innkeeper Monica"] = "Innkeeper Monica";
	AL["Julie Honeywell"] = "Julie Honeywell";
	AL["Jay Lemieux"] = "Jay Lemieux";
	AL["Young Blanchy"] = "Young Blanchy";

	--Gruul's Lair

	--HFC: The Blood Furnace
	AL["Gunny"] = "Gunny";
	AL["Caza'rez"] = "Caza'rez";

	--HFC: Hellfire Ramparts
	AL["Advance Scout Chadwick"] = "Advance Scout Chadwick";
	AL["Stone Guard Stok'ton"] = "Stone Guard Stok'ton";
	AL["Reinforced Fel Iron Chest"] = "Reinforced Fel Iron Chest";

	--HFC: Magtheridon's Lair

	--HFC: The Shattered Halls
	AL["Shattered Hand Executioner"] = "Shattered Hand Executioner";
	AL["Private Jacint"] = "Private Jacint";
	AL["Rifleman Brownbeard"] = "Rifleman Brownbeard";
	AL["Captain Alina"] = "Captain Alina";
	AL["Scout Orgarr"] = "Scout Orgarr";
	AL["Korag Proudmane"] = "Korag Proudmane";
	AL["Captain Boneshatter"] = "Captain Boneshatter";
	AL["Randy Whizzlesprocket"] = "Randy Whizzlesprocket";
	AL["Drisella"] = "Drisella";

	--Karazhan Start
	AL["Baroness Dorothea Millstipe"] = "Baroness Dorothea Millstipe";
	AL["Lady Catriona Von'Indi"] = "Lady Catriona Von'Indi";
	AL["Lady Keira Berrybuck"] = "Lady Keira Berrybuck";
	AL["Baron Rafe Dreuger"] = "Baron Rafe Dreuger";
	AL["Lord Robin Daris"] = "Lord Robin Daris";
	AL["Lord Crispin Ference"] = "Lord Crispin Ference";
	AL["Red Riding Hood"] = "Red Riding Hood";
	AL["Wizard of Oz"] = "Wizard of Oz";
	AL["The Master's Terrace"] = "The Master's Terrace";
	AL["Servant Quarters"] = "Servant Quarters";
	AL["Hastings <The Caretaker>"] = "Hastings <The Caretaker>";
	AL["Berthold <The Doorman>"] = "Berthold <The Doorman>";
	AL["Calliard <The Nightman>"] = "Calliard <The Nightman>";
	AL["Koren <The Blacksmith>"] = "Koren <The Blacksmith>";
	AL["Bennett <The Sergeant at Arms>"] = "Bennett <The Sergeant at Arms>";
	AL["Keanna's Log"] = "Keanna's Log";
	AL["Ebonlocke <The Noble>"] = "Ebonlocke <The Noble>";
	AL["Sebastian <The Organist>"] = "Sebastian <The Organist>";
	AL["Barnes <The Stage Manager>"] = "Barnes <The Stage Manager>";

	--Karazhan End
	AL["Path to the Broken Stairs"] = "Path to the Broken Stairs";
	AL["Broken Stairs"] = "Broken Stairs";
	AL["Ramp to Guardian's Library"] = "Ramp to Guardian's Library";
	AL["Mysterious Bookshelf"] = "Mysterious Bookshelf";
	AL["Ramp up to the Celestial Watch"] = "Ramp up to the Celestial Watch";
	AL["Ramp down to the Gamesman's Hall"] = "Ramp down to the Gamesman's Hall";
	AL["Ramp to Medivh's Chamber"] = "Ramp to Medivh's Chamber";
	AL["Spiral Stairs to Netherspace"] = "Spiral Stairs to Netherspace";
	AL["Wravien <The Mage>"] = "Wravien <The Mage>";
	AL["Gradav <The Warlock>"] = "Gradav <The Warlock>";
	AL["Kamsis <The Conjurer>"] = "Kamsis <The Conjurer>";
	AL["Ythyar"] = "Ythyar";
	AL["Echo of Medivh"] = "Echo of Medivh";

	--Magisters Terrace
	AL["Exarch Larethor"] = "Exarch Larethor";
	AL["Fel Crystals"] = "Fel Crystals";
	AL["Apoko"] = "Apoko";
	AL["Eramas Brightblaze"] = "Eramas Brightblaze";
	AL["Ellrys Duskhallow"] = "Ellrys Duskhallow";
	AL["Fizzle"] = "Fizzle";
	AL["Garaxxas"] = "Garaxxas";
	AL["Sliver <Garaxxas' Pet>"] = "Sliver <Garaxxas' Pet>";
	AL["Kagani Nightstrike"] = "Kagani Nightstrike";
	AL["Warlord Salaris"] = "Warlord Salaris";
	AL["Yazzai"] = "Yazzai";
	AL["Zelfan"] = "Zelfan";
	AL["Tyrith"] = "Tyrith";
	AL["Scrying Orb"] = "Scrying Orb";

	--Sunwell Plateau
	AL["Madrigosa"] = "Madrigosa";

	--TK: The Arcatraz
	AL["Millhouse Manastorm"] = "Millhouse Manastorm";
	AL["Third Fragment Guardian"] = "Third Fragment Guardian";
	AL["Udalo"] = "Udalo";

	--TK: The Botanica

	--TK: The Mechanar
	AL["Overcharged Manacell"] = "Overcharged Manacell";

	--TK: The Eye

--*****************
-- Wrath of the Lich King Instances
--*****************

	--Azjol-Nerub: Ahn'kahet: The Old Kingdom
	AL["Seer Ixit"] = "Seer Ixit";
	AL["Ahn'kahet Brazier"] = "Ahn'kahet Brazier";

	--Azjol-Nerub: Azjol-Nerub
	AL["Reclaimer A'zak"] = "Reclaimer A'zak";
	AL["Watcher Gashra"] = "Watcher Gashra";
	AL["Watcher Narjil"] = "Watcher Narjil";
	AL["Watcher Silthik"] = "Watcher Silthik";
	AL["Elder Nurgen"] = "Elder Nurgen";	

	--Caverns of Time: The Culling of Stratholme
	AL["The Culling of Stratholme"] = "The Culling of Stratholme";
	AL["Scourge Invasion Points"] = "Scourge Invasion Points";
	AL["Guardian of Time"] = "Guardian of Time";
	AL["Chromie"] = "Chromie";

	--Drak'Tharon Keep
	AL["Image of Drakuru"] = "Image of Drakuru";
	AL["Kurzel"] = "Kurzel";
	AL["Elder Kilias"] = "Elder Kilias";
	AL["Drakuru's Brazier"] = "Drakuru's Brazier";

	--The Frozen Halls: Halls of Reflection
	--3 beginning NPCs omitted, see The Forge of Souls
	AL["The Captain's Chest"] = "The Captain's Chest";

	--The Frozen Halls: Pit of Saron
	--6 beginning NPCs omitted, see The Forge of Souls
	AL["Martin Victus"] = "Martin Victus";
	AL["Gorkun Ironskull"] = "Gorkun Ironskull";
	AL["Rimefang"] = "Rimefang";

	--The Frozen Halls: The Forge of Souls
	--Lady Jaina Proudmoore omitted, in Hyjal Summit
	AL["Archmage Koreln <Kirin Tor>"] = "Archmage Koreln <Kirin Tor>";
	AL["Archmage Elandra <Kirin Tor>"] = "Archmage Elandra <Kirin Tor>";
	AL["Lady Sylvanas Windrunner <Banshee Queen>"] = "Lady Sylvanas Windrunner <Banshee Queen>";
	AL["Dark Ranger Loralen"] = "Dark Ranger Loralen";
	AL["Dark Ranger Kalira"] = "Dark Ranger Kalira";

	--Gundrak
	AL["Chronicler Bah'Kini"] = "Chronicler Bah'Kini";
	AL["Tol'mar"] = "Tol'mar";
	AL["Elder Ohanzee"] = "Elder Ohanzee";

	--Icecrown Citadel
	AL["To next map"] = "To next map";
	AL["From previous map"] = "From previous map";
	AL["Upper Spire"] = "Upper Spire";
	AL["Sindragosa's Lair"] = "Sindragosa's Lair";
	AL["Stinky"] = "Stinky";
	AL["Precious"] = "Precious";
	AL["Rimefang"] = "Rimefang";	-- NPC: 37533
	AL["Spinestalker"] = "Spinestalker";	-- NPC: 37534
	AL["Sister Svalna"] = "Sister Svalna";	-- NPC: 37126

	--Naxxramas
	AL["Mr. Bigglesworth"] = "Mr. Bigglesworth";
	AL["Frostwyrm Lair"] = "Frostwyrm Lair";
	AL["Teleporter to Middle"] = "Teleporter to Middle";

	--The Obsidian Sanctum
	AL["Black Dragonflight Chamber"] = "Black Dragonflight Chamber";

	--Onyxia's Lair

	--The Ruby Sanctum
	AL["Red Dragonflight Chamber"] = "Red Dragonflight Chamber";

	--The Nexus: The Eye of Eternity

	--The Nexus: The Nexus
	AL["Warmage Kaitlyn"] = "Warmage Kaitlyn";
	AL["Berinand's Research"] = "Berinand's Research";
	AL["Elder Igasho"] = "Elder Igasho";

	--The Nexus: The Oculus
	AL["Belgaristrasz"] = "Belgaristrasz";
	AL["Eternos"] = "Eternos";
	AL["Verdisa"] = "Verdisa";
	AL["Centrifuge Construct"] = "Centrifuge Construct";
	AL["Cache of Eregos"] = "Cache of Eregos";

	--Trial of the Champion
	AL["Marshal Jacob Alerius"] = "Marshal Jacob Alerius";
	AL["Ambrose Boltspark"] = "Ambrose Boltspark";
	AL["Colosos"] = "Colosos";
	AL["Jaelyne Evensong"] = "Jaelyne Evensong";
	AL["Lana Stouthammer"] = "Lana Stouthammer";

	--Trial of the Crusader
	AL["Heroic: Trial of the Grand Crusader"] = "Heroic: Trial of the Grand Crusader";
	AL["Cavern Entrance"] = "Cavern Entrance";

	--Ulduar General
	AL["The Siege"] = "The Siege";
	AL["The Keepers"] = "The Keepers";

	--Ulduar A
	AL["Tower of Life"] = "Tower of Life";
	AL["Tower of Flame"] = "Tower of Flame";
	AL["Tower of Frost"] = "Tower of Frost";
	AL["Tower of Storms"] = "Tower of Storms";

	--Ulduar B
	AL["Prospector Doren"] = "Prospector Doren";
	AL["Archivum Console"] = "Archivum Console";

	--Ulduar C
	AL["Sif"] = "Sif";

	--Ulduar D

	--Ulduar E

	--Ulduar: Halls of Lightning
	AL["Stormherald Eljrrin"] = "Stormherald Eljrrin";

	--Ulduar: Halls of Stone
	AL["Kaldir Ironbane"] = "Kaldir Ironbane";
	AL["Tribunal Chest"] = "Tribunal Chest";
	AL["Elder Yurauk"] = "Elder Yurauk";
	AL["Brann Bronzebeard"] = "Brann Bronzebeard";

	--Utgarde Keep: Utgarde Keep
	AL["Defender Mordun"] = "Defender Mordun";
	AL["Dark Ranger Marrah"] = "Dark Ranger Marrah";
	AL["Elder Jarten"] = "Elder Jarten";

	--Utgarde Keep: Utgarde Pinnacle
	AL["Brigg Smallshanks"] = "Brigg Smallshanks";
	AL["Image of Argent Confessor Paletress"] = "Image of Argent Confessor Paletress";
	AL["Elder Chogan'gada"] = "Elder Chogan'gada";

	--Vault of Archavon

	--The Violet Hold
	AL["Lieutenant Sinclari"] = "Lieutenant Sinclari";

--*********************
-- Cataclysm Instances
--*********************

	--Baradin Hold

	--Blackrock Caverns

	--Blackwing Descent

	--Caverns of Time: Dragon Soul
	AL["Dasnurimi <Geologist & Conservator>"] = "Dasnurimi <Geologist & Conservator>";
	AL["Lord Afrasastrasz"] = "Lord Afrasastrasz";

	--Caverns of Time: End Time
	AL["Alurmi"] = "Alurmi";
	AL["Nozdormu"] = "Nozdormu";

	--Caverns of Time: Hour of Twilight

	--Caverns of Time: Well of Eternity

	--Firelands
	AL["Lurah Wrathvine <Crystallized Firestone Collector>"] = "Lurah Wrathvine <Crystallized Firestone Collector>"; -- 54402
	AL["Naresir Stormfury <Avengers of Hyjal Quartermaster>"] = "Naresir Stormfury <Avengers of Hyjal Quartermaster>"; -- 54401

	--Grim Batol
	AL["Baleflame"] = "Baleflame";
	AL["Farseer Tooranu <The Earthen Ring>"] = "Farseer Tooranu <The Earthen Ring>";
	AL["Velastrasza"] = "Velastrasza";

	--Halls of Origination
	AL["Large Stone Obelisk"] = "Large Stone Obelisk";

	--Lost City of the Tol'vir
	AL["Captain Hadan"] = "Captain Hadan";
	AL["Tol'vir Grave"] = "Tol'vir Grave";

	--The Bastion of Twilight

	--The Stonecore
	AL["Earthwarden Yrsa <The Earthen Ring>"] = "Earthwarden Yrsa <The Earthen Ring>";

	--The Vortex Pinnacle
	AL["Itesh"] = "Itesh";
	AL["Magical Brazier"] = "Magical Brazier";

	--Throne of the Four Winds

	--Throne of the Tides
	AL["Captain Taylor"] = "Captain Taylor";
	AL["Legionnaire Nazgrim"] = "Legionnaire Nazgrim";
	AL["Neptulon"] = "Neptulon";

	--Zul'Aman
	AL["Vol'jin"] = "Vol'jin";
	AL["Witch Doctor T'wansi"] = "Witch Doctor T'wansi";
	AL["Blood Guard Hakkuz <Darkspear Elite>"] = "Blood Guard Hakkuz <Darkspear Elite>";
	AL["Voodoo Pile"] = "Voodoo Pile";
	AL["Bakkalzu"] = "Bakkalzu";
	AL["Hazlek"] = "Hazlek";
	AL["The Map of Zul'Aman"] = "The Map of Zul'Aman";
	AL["Norkani"] = "Norkani";
	AL["Kasha"] = "Kasha";
	AL["Thurg"] = "Thurg";
	AL["Gazakroth"] = "Gazakroth";
	AL["Lord Raadan"] = "Lord Raadan";
	AL["Darkheart"] = "Darkheart";
	AL["Alyson Antille"] = "Alyson Antille";
	AL["Slither"] = "Slither";
	AL["Fenstalker"] = "Fenstalker";
	AL["Koragg"] = "Koragg";
	AL["Zungam"] = "Zungam";
	AL["Forest Frogs"] = "Forest Frogs";
	AL["Eulinda <Reagents>"] = "Eulinda <Reagents>";
	AL["Harald <Food Vendor>"] = "Harald <Food Vendor>";
	AL["Arinoth"] = "Arinoth";
	AL["Kaldrick"] = "Kaldrick";
	AL["Lenzo"] = "Lenzo";
	AL["Mawago"] = "Mawago";
	AL["Melasong"] = "Melasong";
	AL["Melissa"] = "Melissa";
	AL["Micah"] = "Micah";
	AL["Relissa"] = "Relissa";
	AL["Rosa"] = "Rosa";
	AL["Tyllan"] = "Tyllan";

	--Zul'Gurub
	AL["Briney Boltcutter <Blackwater Financial Interests>"] = "Briney Boltcutter <Blackwater Financial Interests>";
	AL["Vehini <Assault Provisions>"] = "Vehini <Assault Provisions>";
	AL["Overseer Blingbang"] = "Overseer Blingbang";
	AL["Bloodslayer T'ara <Darkspear Veteran>"] = "Bloodslayer T'ara <Darkspear Veteran>";
	AL["Bloodslayer Vaena <Darkspear Veteran>"] = "Bloodslayer Vaena <Darkspear Veteran>";
	AL["Bloodslayer Zala <Darkspear Veteran>"] = "Bloodslayer Zala <Darkspear Veteran>";
	AL["Helpful Jungle Monkey"] = "Helpful Jungle Monkey";
	AL["Venomancer Mauri <The Snake's Whisper>"] = "Venomancer Mauri <The Snake's Whisper>";
	AL["Zanzil's Cauldron of Toxic Torment"] = "Zanzil's Cauldron of Toxic Torment";
	AL["Tiki Lord Mu'Loa"] = "Tiki Lord Mu'Loa";
	AL["Gub <Destroyer of Fish>"] = "Gub <Destroyer of Fish>";
	AL["Venomancer T'Kulu <The Toxic Bite>"] = "Venomancer T'Kulu <The Toxic Bite>";
	AL["Tor-Tun <The Slumberer>"] = "Tor-Tun <The Slumberer>";
	AL["Kaulema the Mover"] = "Kaulema the Mover";
	AL["Berserking Boulder Roller"] = "Berserking Boulder Roller";
	AL["Zanzil's Cauldron of Frostburn Formula"] = "Zanzil's Cauldron of Frostburn Formula";
	AL["Mor'Lek the Dismantler"] = "Mor'Lek the Dismantler";
	AL["Witch Doctor Qu'in <Medicine Woman>"] = "Witch Doctor Qu'in <Medicine Woman>";
	AL["Zanza the Restless"] = "Zanza the Restless";
	AL["Mortaxx <The Tolling Bell>"] = "Mortaxx <The Tolling Bell>";
	AL["Tiki Lord Zim'wae"] = "Tiki Lord Zim'wae";
	AL["Zanzil's Cauldron of Burning Blood"] = "Zanzil's Cauldron of Burning Blood";

--*********************
-- Mists of Pandaria Instances
--*********************

	--Gate of the Setting Sun
	AL["Bowmistress Li <Guard Captain>"] = "Bowmistress Li <Guard Captain>";

	--Heart of Fear

	--Mogu'shan Palace
	AL["Sinan the Dreamer"] = "Sinan the Dreamer";

	--Mogu'shan Vaults

	--Shado-Pan Monastery
	AL["Ban Bearheart"] = "Ban Bearheart";

	--Siege of Niuzao Temple
	AL["Shado-Master Chum Kiu"] = "Shado-Master Chum Kiu";

	--Siege of Orgrimmar

	--Stormstout Brewery
	AL["Auntie Stormstout"] = "Auntie Stormstout";
	AL["Chen Stormstout"] = "Chen Stormstout";

	--Temple of the Jade Serpent
	AL["Master Windstrong"] = "Master Windstrong";
	AL["Priestess Summerpetal"] = "Priestess Summerpetal";

	--Terrace of Endless Spring

	--Throne of Thunder
	AL["Monara <The Last Queen>"] = "Monara <The Last Queen>";
	AL["No'ku Stormsayer <Lord of Tempest>"] = "No'ku Stormsayer <Lord of Tempest>";
	AL["Rocky Horror"] = "Rocky Horror";
	AL["Focused Eye"] = "Focused Eye";
	AL["Unblinking Eye"] = "Unblinking Eye";
	AL["Archritualist Kelada"] = "Archritualist Kelada";
	AL["Flesh'rok the Diseased <Primordial Saurok Horror>"] = "Flesh'rok the Diseased <Primordial Saurok Horror>";
	AL["Zao'cho <The Emperor's Shield>"] = "Zao'cho <The Emperor's Shield>";

end