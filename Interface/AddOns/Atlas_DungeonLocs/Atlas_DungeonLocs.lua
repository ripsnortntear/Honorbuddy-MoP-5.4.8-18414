-- $Id: Atlas_DungeonLocs.lua 2087 2013-09-09 14:50:57Z arithmandar $
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

local BZ = Atlas_GetLocaleLibBabble("LibBabble-SubZone-3.0");
local AL = LibStub("AceLocale-3.0"):GetLocale("Atlas_DungeonLocs");
local ALC = LibStub("AceLocale-3.0"):GetLocale("Atlas");

local BLUE = "|cff6666ff";
local GREN = "|cff66cc33";
local LBLU = "|cff33cccc";
local _RED = "|cffcc3333";
local ORNG = "|cffcc9933";
local PINK = "|ccfcc33cc";
local PURP = "|cff9900ff";
local WHIT = "|cffffffff";
local YLOW = "|cffcccc33";
local INDENT = "      ";

local myCategory = AL["Dungeon Locations"];

local myData = {
	DLEast = {
		ZoneName = { BZ["Eastern Kingdoms"] };
		{ WHIT.." 1) "..BZ["Sunwell Plateau"]..", ".._RED..BZ["Isle of Quel'Danas"] };
		{ WHIT.." 2) "..BZ["Magisters' Terrace"]..", ".._RED..BZ["Isle of Quel'Danas"] };
		{ WHIT.." 3) "..BZ["Zul'Aman"]..", ".._RED..BZ["Ghostlands"] };
		{ WHIT.." 4) "..BZ["Stratholme"]..", ".._RED..BZ["Eastern Plaguelands"] };
		{ WHIT..INDENT..BZ["Stratholme"].." - "..BZ["Crusaders' Square"] };
		{ WHIT..INDENT..BZ["Stratholme"].." - "..BZ["The Gauntlet"] };
		{ WHIT.." 5) "..BZ["Scarlet Monastery"]..", ".._RED..BZ["Tirisfal Glades"] };
		{ WHIT..INDENT..BZ["Scarlet Halls"] };
		{ WHIT..INDENT..BZ["Scarlet Monastery"] };
		{ WHIT.." 6) "..BZ["Scholomance"]..", ".._RED..BZ["Western Plaguelands"] };
		{ WHIT.." 7) "..BZ["Shadowfang Keep"]..", ".._RED..BZ["Silverpine Forest"] };
		{ WHIT.." 8) "..BZ["Baradin Hold"]..", ".._RED..BZ["Tol Barad"] };
		{ WHIT.." 9) "..BZ["Grim Batol"]..", ".._RED..BZ["Twilight Highlands"] };
		{ WHIT.."10) "..BZ["The Bastion of Twilight"]..", ".._RED..BZ["Twilight Highlands"] };
		{ WHIT.."11) "..BZ["Gnomeregan"]..", ".._RED..BZ["Dun Morogh"] };
		{ WHIT.."12) "..BZ["The Abyssal Maw"]..", ".._RED..BZ["Abyssal Depths"] };
		{ WHIT.."13) "..BZ["Uldaman"]..", ".._RED..BZ["Badlands"] };
		{ WHIT.."14) "..BZ["Blackrock Mountain"]..", ".._RED..BZ["Searing Gorge"].." / "..BZ["Burning Steppes"] };
		{ WHIT..INDENT..BZ["Blackrock Depths"] };
		{ WHIT..INDENT..BZ["Blackrock Spire"] };
		{ WHIT..INDENT..BZ["The Molten Core"]..", ".._RED..BZ["Blackrock Depths"] };
		{ WHIT..INDENT..BZ["Blackwing Lair"]..", ".._RED..BZ["Blackrock Spire"] };
		{ WHIT..INDENT..BZ["Blackrock Caverns"] };
		{ WHIT..INDENT..BZ["Blackwing Descent"] };
		{ WHIT.."15) "..BZ["The Stockade"]..", ".._RED..BZ["Stormwind City"] };
		{ WHIT.."16) "..BZ["Sunken Temple"]..", ".._RED..BZ["Swamp of Sorrows"] };
		{ WHIT.."17) "..BZ["The Deadmines"]..", ".._RED..BZ["Westfall"] };
		{ WHIT.."18) "..BZ["Karazhan"]..", ".._RED..BZ["Deadwind Pass"] };
		{ WHIT.."19) "..BZ["Zul'Gurub"]..", ".._RED..BZ["Northern Stranglethorn"] };
		{ GREN.." 1') "..BZ["Alterac Valley"]..", ".._RED..BZ["Hillsbrad Foothills"] };
		{ GREN.." 2') "..BZ["Arathi Basin"]..", ".._RED..BZ["Arathi Highlands"] };
		{ GREN.." 3') "..BZ["Tol Barad"]..", ".._RED..BZ["Tol Barad"] };
		{ "" };
		{ WHIT..AL["White"]..": "..ORNG..AL["Instances"] };
		{ GREN..AL["Green"]..": "..ORNG..AL["Battlegrounds"] };
	};
	DLWest = {
		ZoneName = { BZ["Kalimdor"] };
		{ WHIT.." 1) "..BZ["Firelands"]..", ".._RED..BZ["Mount Hyjal"] };
		{ WHIT.." 2) "..BZ["Blackfathom Deeps"]..", ".._RED..BZ["Ashenvale"] };
		{ WHIT.." 3) "..BZ["Ragefire Chasm"]..", ".._RED..BZ["Orgrimmar"] };
		{ WHIT.." 4) "..BZ["Wailing Caverns"]..", ".._RED..BZ["Northern Barrens"] };
		{ WHIT.." 5) "..BZ["Maraudon"]..", ".._RED..BZ["Desolace"] };
		{ WHIT.." 6) "..BZ["Dire Maul"]..", ".._RED..BZ["Feralas"] };
		{ WHIT.." 7) "..BZ["Razorfen Kraul"]..", ".._RED..BZ["Southern Barrens"] };
		{ WHIT.." 8) "..BZ["Razorfen Downs"]..", ".._RED..BZ["Thousand Needles"] };
		{ WHIT.." 9) "..BZ["Onyxia's Lair"]..", ".._RED..BZ["Dustwallow Marsh"] };
		{ WHIT.."10) "..BZ["Zul'Farrak"]..", ".._RED..BZ["Tanaris"] };
		{ WHIT.."11) "..BZ["Caverns of Time"]..", ".._RED..BZ["Tanaris"] };
		{ WHIT..INDENT..BZ["Old Hillsbrad Foothills"] };
		{ WHIT..INDENT..BZ["The Black Morass"] };
		{ WHIT..INDENT..BZ["Hyjal Summit"] };
		{ WHIT..INDENT..BZ["The Culling of Stratholme"] };
		{ WHIT..INDENT..BZ["End Time"] };
		{ WHIT..INDENT..BZ["Well of Eternity"] };
		{ WHIT..INDENT..BZ["Hour of Twilight"] };
		{ WHIT..INDENT..BZ["Dragon Soul"] };
		{ WHIT.."12) "..BZ["Ahn'Qiraj"]..", ".._RED..BZ["Ahn'Qiraj: The Fallen Kingdom"] };
		{ WHIT..INDENT..BZ["Ruins of Ahn'Qiraj"] };
		{ WHIT..INDENT..BZ["Temple of Ahn'Qiraj"] };
		{ WHIT.."13) "..BZ["Halls of Origination"]..", ".._RED..BZ["Uldum"] };
		{ WHIT.."14) "..BZ["Lost City of the Tol'vir"]..", ".._RED..BZ["Uldum"] };
		{ WHIT.."15) "..BZ["Throne of the Four Winds"]..", ".._RED..BZ["Uldum"] };
		{ WHIT.."16) "..BZ["The Vortex Pinnacle"]..", ".._RED..BZ["Uldum"] };
		{ GREN.." 1') "..BZ["Warsong Gulch"]..", ".._RED..BZ["Ashenvale"] };
		{ "" };
		{ WHIT..AL["White"]..": "..ORNG..AL["Instances"] };
		{ GREN..AL["Green"]..": "..ORNG..AL["Battlegrounds"] };
	};
	DLOutland = {
		ZoneName = { BZ["Outland"] };
		{ WHIT.." 1) "..BZ["Gruul's Lair"]..", ".._RED..BZ["Blade's Edge Mountains"] };
		{ WHIT.." 2) "..BZ["Tempest Keep"]..", ".._RED..BZ["Netherstorm"] };
		{ WHIT..INDENT..BZ["The Mechanar"] };
		{ WHIT..INDENT..BZ["The Botanica"] };
		{ WHIT..INDENT..BZ["The Arcatraz"] };
		{ WHIT..INDENT..BZ["Tempest Keep"] };
		{ WHIT.." 3) "..BZ["Coilfang Reservoir"]..", ".._RED..BZ["Zangarmarsh"] };
		{ WHIT..INDENT..BZ["The Slave Pens"] };
		{ WHIT..INDENT..BZ["The Underbog"] };
		{ WHIT..INDENT..BZ["The Steamvault"] };
		{ WHIT..INDENT..BZ["Serpentshrine Cavern"] };
		{ WHIT.." 4) "..BZ["Hellfire Citadel"]..", ".._RED..BZ["Hellfire Peninsula"] };
		{ WHIT..INDENT..BZ["Hellfire Ramparts"] };
		{ WHIT..INDENT..BZ["The Blood Furnace"] };
		{ WHIT..INDENT..BZ["The Shattered Halls"] };
		{ WHIT..INDENT..BZ["Magtheridon's Lair"] };
		{ WHIT.." 5) "..BZ["Auchindoun"]..", ".._RED..BZ["Terokkar Forest"] };
		{ WHIT..INDENT..BZ["Mana-Tombs"] };
		{ WHIT..INDENT..BZ["Auchenai Crypts"] };
		{ WHIT..INDENT..BZ["Sethekk Halls"] };
		{ WHIT..INDENT..BZ["Shadow Labyrinth"] };
		{ WHIT.." 6) "..BZ["Black Temple"]..", ".._RED..BZ["Shadowmoon Valley"] };
	};
	DLNorthrend = {
		ZoneName = { BZ["Northrend"] };
		{ WHIT.." 1) "..BZ["Ulduar"]..", ".._RED..BZ["The Storm Peaks"] };
		{ WHIT..INDENT..BZ["Ulduar"] };
		{ WHIT..INDENT..BZ["Halls of Stone"] };
		{ WHIT..INDENT..BZ["Halls of Lightning"] };
		{ WHIT.." 2) "..ALC["Crusaders' Coliseum"]..", ".._RED..BZ["Icecrown"] };
		{ WHIT..INDENT..BZ["Trial of the Crusader"] };
		{ WHIT..INDENT..BZ["Trial of the Champion"] };
		{ WHIT.." 3) "..BZ["Gundrak"]..", ".._RED..BZ["Zul'Drak"] };
		{ WHIT.." 4) "..BZ["Icecrown Citadel"]..", ".._RED..BZ["Icecrown"] };
		{ WHIT..INDENT..BZ["Icecrown Citadel"] };
		{ WHIT..INDENT..BZ["The Frozen Halls"] };		
		{ WHIT..INDENT..INDENT..BZ["The Forge of Souls"] };
		{ WHIT..INDENT..INDENT..BZ["Pit of Saron"] };
		{ WHIT..INDENT..INDENT..BZ["Halls of Reflection"] };
		{ WHIT.." 5) "..BZ["The Violet Hold"]..", ".._RED..BZ["Dalaran"] };
		{ WHIT.." 6) "..BZ["Vault of Archavon"]..", ".._RED..BZ["Wintergrasp"] };
		{ WHIT.." 7) "..BZ["Drak'Tharon Keep"]..", ".._RED..BZ["Grizzly Hills"] };
		{ WHIT.." 8) "..BZ["The Nexus"]..", ".._RED..BZ["Coldarra"] };
		{ WHIT..INDENT..BZ["The Nexus"] };
		{ WHIT..INDENT..BZ["The Oculus"] };
		{ WHIT..INDENT..BZ["The Eye of Eternity"] };
		{ WHIT.." 9) "..BZ["Azjol-Nerub"]..", ".._RED..BZ["Dragonblight"] };
		{ WHIT..INDENT..BZ["Azjol-Nerub"] };
		{ WHIT..INDENT..BZ["Ahn'kahet: The Old Kingdom"] };
		{ WHIT.."10) "..BZ["Wyrmrest Temple"]..", ".._RED..BZ["Dragonblight"] };
		{ WHIT..INDENT..BZ["The Obsidian Sanctum"] };
		{ WHIT..INDENT..BZ["The Ruby Sanctum"] };
		{ WHIT.."11) "..BZ["Naxxramas"]..", ".._RED..BZ["Dragonblight"] };
		{ WHIT.."12) "..BZ["Utgarde Keep"]..", ".._RED..BZ["Howling Fjord"] };
		{ WHIT..INDENT..BZ["Utgarde Keep"] };
		{ WHIT..INDENT..BZ["Utgarde Pinnacle"] };
		{ GREN.." 1') "..BZ["Wintergrasp"]..", ".._RED..BZ["Wintergrasp"] };
		{ "" };
		{ WHIT..AL["White"]..": "..ORNG..AL["Instances"] };
		{ GREN..AL["Green"]..": "..ORNG..AL["Battlegrounds"] };
	};
	DLDeepholm = {
		ZoneName = { BZ["Deepholm"] };
		{ WHIT.." 1) "..BZ["The Stonecore"] };
	};
	DLPandaria = {
		ZoneName = { BZ["Pandaria"] };
		{ WHIT.." 1) "..BZ["Throne of Thunder"]..", ".._RED..BZ["Isle of Thunder"] };
		{ WHIT.." 2) "..BZ["Shado-Pan Monastery"]..", ".._RED..BZ["Kun-Lai Summit"] };
		{ WHIT.." 3) "..BZ["Mogu'shan Vaults"]..", ".._RED..BZ["Kun-Lai Summit"] };
		{ WHIT.." 4) "..BZ["Siege of Niuzao Temple"]..", ".._RED..BZ["Townlong Steppes"] };
		{ WHIT.." 5) "..BZ["Gate of the Setting Sun"]..", ".._RED..BZ["Dread Wastes"].." / ".._RED..BZ["Vale of Eternal Blossoms"] };
		{ WHIT.." 6) "..BZ["Siege of Orgrimmar"]..", ".._RED..BZ["Vale of Eternal Blossoms"] };
		{ WHIT.." 7) "..BZ["Mogu'shan Palace"]..", ".._RED..BZ["Vale of Eternal Blossoms"] };
		{ WHIT.." 8) "..BZ["Terrace of Endless Spring"]..", ".._RED..BZ["The Veiled Stair"] };
		{ WHIT.." 9) "..BZ["Temple of the Jade Serpent"]..", ".._RED..BZ["The Jade Forest"] };
		{ WHIT.."10) "..BZ["Heart of Fear"]..", ".._RED..BZ["Dread Wastes"] };
		{ WHIT.."11) "..BZ["Stormstout Brewery"]..", ".._RED..BZ["Valley of the Four Winds"] };
		{ GREN.." 1') "..BZ["Deepwind Gorge"]..", ".._RED..BZ["Valley of the Four Winds"] };
		{ "" };
		{ WHIT..AL["White"]..": "..ORNG..AL["Instances"] };
		{ GREN..AL["Green"]..": "..ORNG..AL["Battlegrounds"] };
	};
};

Atlas_RegisterPlugin("Atlas_DungeonLocs", myCategory, myData);
