-- $Id: Atlas_Battlegrounds.lua 2087 2013-09-09 14:50:57Z arithmandar $
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
local AL = LibStub("AceLocale-3.0"):GetLocale("Atlas_Battlegrounds");
local ALC = LibStub("AceLocale-3.0"):GetLocale("Atlas");
local ALIL = Atlas_IngameLocales;

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
local BULLET = " - ";

local myCategory = AL["Battleground Maps"];

local myData = {
	AlteracValleyNorth = {
		ZoneName = { BZ["Alterac Valley"].." ("..ALC["North"]..", "..FACTION_ALLIANCE..")" };
		Location = { BZ["Alterac Mountains"] };
		LevelRange = "45-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "45";
		PlayerLimit = "40";
		Acronym = AL["AV"];
		{ ORNG..ALC["Reputation"]..": "..ALIL["Stormpike Guard"] };
		{ BLUE.."A) "..ALC["Entrance"] };
		{ BLUE.."B) "..BZ["Dun Baldar"] };
		{ GREN..INDENT..AL["Vanndar Stormpike <Stormpike General>"] };
		{ GREN..INDENT..AL["Prospector Stonehewer"] };
		{ _RED.."1) "..AL["Dun Baldar North Bunker"] };
		{ GREN..INDENT..AL["Wing Commander Mulverick"].." ("..FACTION_HORDE..")" };
		{ _RED.."2) "..AL["Dun Baldar South Bunker"] };
		{ GREN..INDENT..AL["Gaelden Hammersmith <Stormpike Supply Officer>"] };
		{ _RED.."3) "..BZ["Icewing Cavern"] };
		{ GREN..INDENT..AL["Stormpike Banner"] };
		{ _RED.."4) "..AL["Stormpike Lumber Yard"] };
		{ GREN..INDENT..AL["Wing Commander Jeztor"].." ("..FACTION_HORDE..")" };
		{ _RED.."5) "..BZ["Icewing Bunker"] };
		{ GREN..INDENT..AL["Wing Commander Guse"].." ("..FACTION_HORDE..")" };
		{ _RED.."6) "..BZ["Stonehearth Outpost"] };
		{ GREN..INDENT..AL["Captain Balinda Stonehearth <Stormpike Captain>"] };
		{ _RED.."7) "..BZ["Stonehearth Bunker"] };
		{ _RED.."8) "..AL["Western Crater"] };
		{ GREN..INDENT..AL["Vipore's Beacon"] };
		{ GREN..INDENT..AL["Jeztor's Beacon"].." ("..FACTION_HORDE..")" };
		{ _RED.."9) "..AL["Eastern Crater"] };
		{ GREN..INDENT..AL["Slidore's Beacon"] };
		{ GREN..INDENT..AL["Guse's Beacon"].." ("..FACTION_HORDE..")" };
		{ GREN.."1) "..BZ["Irondeep Mine"] };
		{ GREN.."1') "..AL["Arch Druid Renferal"] };
		{ GREN.."2') "..AL["Murgot Deepforge"] };
		{ GREN..INDENT..AL["Lana Thunderbrew <Blacksmithing Supplies>"] };
		{ GREN.."3') "..AL["Stormpike Stable Master <Stable Master>"] };
		{ GREN..INDENT..AL["Stormpike Ram Rider Commander"] };
		{ GREN..INDENT..AL["Svalbrad Farmountain <Trade Goods>"] };
		{ GREN..INDENT..AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] };
		{ GREN.."4') "..AL["Stormpike Quartermaster"] };
		{ GREN..INDENT..AL["Jonivera Farmountain <General Goods>"] };
		{ GREN..INDENT..AL["Brogus Thunderbrew <Food & Drink>"] };
		{ GREN.."5') "..AL["Wing Commander Ichman"].." ("..AL["Rescued"]..")" };
		{ GREN..INDENT..AL["Wing Commander Slidore"].." ("..AL["Rescued"]..")" };
		{ GREN..INDENT..AL["Wing Commander Vipore"].." ("..AL["Rescued"]..")" };
		{ GREN.."6') "..AL["Stormpike Ram Rider Commander"] };
		{ GREN.."7') "..AL["Ivus the Forest Lord"].." ("..ALC["Summon"]..")" };
		{ ORNG.."1) "..AL["Stormpike Aid Station"] };
		{ ORNG.."2) "..BZ["Stormpike Graveyard"] };
		{ ORNG.."3) "..BZ["Stonehearth Graveyard"] };
		{ ORNG.."4) "..BZ["Snowfall Graveyard"] };
		{ GREN..INDENT..AL["Ichman's Beacon"] };
		{ GREN..INDENT..AL["Mulverick's Beacon"].." ("..FACTION_HORDE..")" };
	};
	AlteracValleySouth = {
		ZoneName = { BZ["Alterac Valley"].." ("..ALC["South"]..", "..FACTION_HORDE..")" };
		Location = { BZ["Hillsbrad Foothills"] };
		LevelRange = "45-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "45";
		PlayerLimit = "40";
		Acronym = AL["AV"];
		{ ORNG..ALC["Reputation"]..": "..ALIL["Frostwolf Clan"] };
		{ BLUE.."A) "..ALC["Entrance"].." ("..FACTION_HORDE..")" };
		{ BLUE.."B) "..BZ["Frostwolf Keep"] };
		{ GREN..INDENT..AL["Drek'Thar <Frostwolf General>"] };
		{ _RED.."1) "..BZ["Iceblood Garrison"] };
		{ GREN..INDENT..AL["Captain Galvangar <Frostwolf Captain>"] };
		{ _RED.."2) "..AL["Iceblood Tower"] };
		{ _RED.."3) "..AL["Tower Point"] };
		{ GREN..INDENT..AL["Wing Commander Slidore"].." ("..FACTION_ALLIANCE..")" };
		{ _RED.."4) "..AL["West Frostwolf Tower"] };
		{ GREN..INDENT..AL["Wing Commander Ichman"].." ("..FACTION_ALLIANCE..")" };
		{ _RED.."5) "..AL["East Frostwolf Tower"] };
		{ _RED.."6) "..BZ["Wildpaw Cavern"] };
		{ GREN..INDENT..AL["Frostwolf Banner"] };
		{ GREN.."1) "..BZ["Coldtooth Mine"] };
		{ GREN.."1') "..AL["Lokholar the Ice Lord"].." ("..ALC["Summon"]..")" };
		{ GREN.."2') "..AL["Wing Commander Vipore"].." ("..FACTION_ALLIANCE..")" };
		{ GREN..INDENT..AL["Jotek"] };
		{ GREN..INDENT..AL["Smith Regzar"] };
		{ GREN..INDENT..AL["Primalist Thurloga"] };
		{ GREN.."3') "..AL["Frostwolf Stable Master <Stable Master>"] };
		{ GREN..INDENT..AL["Frostwolf Wolf Rider Commander"] };
		{ GREN.."4') "..AL["Frostwolf Quartermaster"] };
		{ GREN.."5') "..AL["Wing Commander Guse"].." ("..AL["Rescued"]..")" };
		{ GREN..INDENT..AL["Wing Commander Jeztor"].." ("..AL["Rescued"]..")" };
		{ GREN..INDENT..AL["Wing Commander Mulverick"].." ("..AL["Rescued"]..")" };
		{ ORNG.."1) "..BZ["Iceblood Graveyard"] };
		{ ORNG.."2) "..BZ["Frostwolf Graveyard"] };
		{ ORNG.."3) "..AL["Frostwolf Relief Hut"] };
	};
	ArathiBasin = {
		ZoneName = { BZ["Arathi Basin"] };
		Location = { BZ["Arathi Highlands"] };
		LevelRange = "10-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "10";
		PlayerLimit = "15";
		Acronym = AL["AB"];
		{ ORNG..ALC["Reputation"]..": "..ALIL["The League of Arathor"].." ("..FACTION_ALLIANCE..")" };
		{ ORNG..ALC["Reputation"]..": "..ALIL["The Defilers"].." ("..FACTION_HORDE..")" };
		{ BLUE.."A) "..BZ["Trollbane Hall"].." ("..FACTION_ALLIANCE..")" };
		{ BLUE.."B) "..BZ["Defiler's Den"].." ("..FACTION_HORDE..")" };
		{ GREN.."1) "..BZ["Stables"] };
		{ GREN.."2) "..BZ["Gold Mine"] };
		{ GREN.."3) "..BZ["Blacksmith"] };
		{ GREN.."4) "..BZ["Lumber Mill"] };
		{ GREN.."5) "..BZ["Farm"] };
	};
	WarsongGulch = {
		ZoneName = { BZ["Warsong Gulch"] };
		Location = { BZ["Ashenvale"].." / "..BZ["Northern Barrens"] };
		LevelRange = "10-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "10";
		PlayerLimit = "10";
		Acronym = AL["WSG"];
		{ ORNG..ALC["Reputation"]..": "..ALIL["Silverwing Sentinels"].." ("..FACTION_ALLIANCE..")" };
		{ ORNG..ALC["Reputation"]..": "..ALIL["Warsong Outriders"].." ("..FACTION_HORDE..")" };
		{ BLUE.."A) "..BZ["Silverwing Hold"].." ("..FACTION_ALLIANCE..")" };
		{ BLUE.."B) "..BZ["Warsong Lumber Mill"].." ("..FACTION_HORDE..")" };
	};
	SilithystMustFlow = {
		ZoneName = { BZ["Silithus"].." - "..AL["The Silithyst Must Flow"] };
		--Location = { BZ["Silithus"] };
		LevelRange = "55-85";
		MinLevel = "55";
		{ ORNG.."PvP: "..AL["The Silithyst Must Flow"] };
		{ BLUE.."A) "..BZ["Cenarion Hold"] };
		{ BLUE.."B) "..AL["Alliance's Camp"] };
		{ BLUE.."C) "..AL["Horde's Camp"] };
	};
	EyeOfTheStorm = {
		ZoneName = { BZ["Eye of the Storm"] };
		Location = { BZ["Netherstorm"] };
		LevelRange = "35-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "35";
		PlayerLimit = "15";
		Acronym = AL["EotS"];
		{ BLUE.."A) "..ALC["Entrance"].." ("..FACTION_ALLIANCE..")" };
		{ BLUE.."B) "..ALC["Entrance"].." ("..FACTION_HORDE..")" };
		{ _RED.."1) "..AL["Flag"] };
		{ GREN.."1) "..BZ["Mage Tower"] };
		{ GREN.."2) "..BZ["Draenei Ruins"] };
		{ GREN.."3) "..BZ["Fel Reaver Ruins"] };
		{ GREN.."4) "..BZ["Blood Elf Tower"] };
		{ ORNG.."1) "..ALC["Graveyard"] };
	};
	HalaaPvP = {
		ZoneName = { BZ["Nagrand"].." - "..BZ["Halaa"] };
		Location = { BZ["Nagrand"] };
		LevelRange = "64-85";
		MinLevel = "64";
		{ ORNG.."PvP: "..BZ["Halaa"] };
		{ GREN.."1) "..BZ["Halaa"] };
		{ GREN..INDENT..FACTION_ALLIANCE };
		{ GREN..INDENT..BULLET..AL["Quartermaster Davian Vaclav"] };
		{ GREN..INDENT..BULLET..AL["Chief Researcher Kartos"] };
		{ GREN..INDENT..BULLET..AL["Aldraan <Blade Merchant>"] };
		{ GREN..INDENT..BULLET..AL["Cendrii <Food & Drink>"] };
		{ GREN..INDENT..FACTION_HORDE };
		{ GREN..INDENT..BULLET..AL["Quartermaster Jaffrey Noreliqe"] };
		{ GREN..INDENT..BULLET..AL["Chief Researcher Amereldine"] };
		{ GREN..INDENT..BULLET..AL["Coreiel <Blade Merchant>"] };
		{ GREN..INDENT..BULLET..AL["Embelar <Food & Drink>"] };
		{ GREN.."2) "..AL["Wyvern Camp"] };
	};
	HellfirePeninsulaPvP = {
		ZoneName = { BZ["Hellfire Peninsula"].." - "..AL["Hellfire Fortifications"] };
		Location = { BZ["Hellfire Peninsula"] };
		LevelRange = "58-85";
		MinLevel = "58";
		{ ORNG.."PvP: "..AL["Hellfire Fortifications"] };
		{ GREN.."1) "..BZ["The Stadium"] };
		{ GREN.."2) "..BZ["The Overlook"] };
		{ GREN.."3) "..BZ["Broken Hill"] };
	};
	TerokkarForestPvP = {
		ZoneName = { BZ["Terokkar Forest"].." - "..AL["Spirit Towers"] };
		Location = { BZ["The Bone Wastes"]..", "..BZ["Terokkar Forest"] };
		LevelRange = "62-85";
		MinLevel = "62";
		{ ORNG.."PvP: "..BZ["Auchindoun"].." "..AL["Spirit Towers"] };
		{ GREN.."1) "..AL["Spirit Towers"] };
	};
	ZangarmarshPvP = {
		ZoneName = { BZ["Zangarmarsh"].." - "..BZ["Twin Spire Ruins"] };
		Location = { BZ["Zangarmarsh"] };
		LevelRange = "60-85";
		MinLevel = "60";
		{ ORNG.."PvP: "..BZ["Twin Spire Ruins"] };
		{ GREN.."1) "..AL["West Beacon"] };
		{ GREN.."2) "..AL["East Beacon"] };
		{ GREN.."1') "..AL["Horde Field Scout"] };
		{ GREN.."2') "..AL["Alliance Field Scout"] };
		{ ORNG.."1) "..AL["Twinspire Graveyard"] };
	};
	IsleOfConquest = {
		ZoneName = { BZ["Isle of Conquest"] };
		Location = { BZ["Icecrown"] };
		LevelRange = "75-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "75";
		PlayerLimit = "40";
		Acronym = AL["IoC"];
		{ ORNG..AL["Gates are marked with red bars."] };
		{ BLUE.."A) "..ALC["Start"].." ("..FACTION_HORDE..")" };
		{ GREN..INDENT..AL["Overlord Agmar"] };
		{ BLUE.."B) "..ALC["Start"].." ("..FACTION_ALLIANCE..")" };
		{ GREN..INDENT..AL["High Commander Halford Wyrmbane <7th Legion>"] };
		{ GREN.."1) "..AL["The Refinery"] };
		{ GREN.."2) "..AL["The Docks"] };
		{ GREN.."3) "..AL["The Workshop"] };
		{ GREN.."4) "..AL["The Hangar"] };
		{ GREN.."5) "..AL["The Quarry"] };
		{ ORNG.."1) "..AL["Contested Graveyards"] };
		{ ORNG.."2) "..AL["Horde Graveyard"] };
		{ ORNG.."3) "..AL["Alliance Graveyard"] };
	};
	StrandOfTheAncients = {
		ZoneName = { BZ["Strand of the Ancients"] };
		Location = { BZ["Dragonblight"] };
		LevelRange = "65-84 ("..AL["Span of 5"]..") / 85";
		MinLevel = "65";
		PlayerLimit = "15";
		Acronym = AL["SotA"];
		{ ORNG..AL["Gates are marked with their colors."] };
		{ BLUE.."A) "..ALC["Start"].." ("..AL["Attacking Team"]..")" };
		{ BLUE.."B) "..ALC["Start"].." ("..AL["Defending Team"]..")" };
		{ _RED.."1) "..AL["Massive Seaforium Charge"] };
		{ _RED.."2) "..AL["Titan Relic"] };
		{ GREN.."1) "..AL["Battleground Demolisher"] };
		{ GREN.."2) "..AL["Graveyard Flag"] };
		{ ORNG.."1) "..AL["Resurrection Point"] };
	};
	WintergraspPvP = {
		ZoneName = { BZ["Wintergrasp"] };
		--Location = { BZ["Wintergrasp"] };
		LevelRange = "73-85";
		MinLevel = "73";
		{ ORNG.."PvP: "..BZ["Wintergrasp"] };
		{ BLUE.."A) "..BZ["Wintergrasp Fortress"] };
		{ BLUE..INDENT..BZ["Vault of Archavon"] };
		{ BLUE.."B) "..BZ["Valiance Landing Camp"] };
		{ BLUE.."C) "..BZ["Warsong Camp"] };
		{ GREN.."1) "..BZ["Wintergrasp Fortress"] };
		{ GREN..INDENT..AL["Fortress Vihecal Workshop (E)"] };
		{ GREN..INDENT..AL["Fortress Vihecal Workshop (W)"] };
		{ GREN.."2) "..BZ["The Sunken Ring"] };
		{ GREN..INDENT..AL["Sunken Ring Vihecal Workshop"] };
		{ GREN.."3) "..BZ["The Broken Temple"] };
		{ GREN..INDENT..AL["Broken Temple Vihecal Workshop"] };
		{ GREN.."4) "..BZ["Eastspark Workshop"] };
		{ GREN..INDENT..AL["Eastspark Vihecale Workshop"] };
		{ GREN.."5) "..BZ["Westspark Workshop"] };
		{ GREN..INDENT..AL["Westspark Vihecale Workshop"] };
		{ _RED.."1) "..BZ["Flamewatch Tower"] };
		{ _RED.."2) "..BZ["Winter's Edge Tower"] };
		{ _RED.."3) "..BZ["Shadowsight Tower"] };
		{ ORNG.."1) "..AL["Wintergrasp Graveyard"] };
		{ ORNG.."2) "..AL["Sunken Ring Graveyard"] };
		{ ORNG.."3) "..AL["Broken Temple Graveyard"] };
		{ ORNG.."4) "..AL["Southeast Graveyard"] };
		{ ORNG.."5) "..AL["Southwest Graveyard"] };
	};
	TheBattleForGilneas = {
		ZoneName = { BZ["Gilneas"].." - "..BZ["The Battle for Gilneas"] };
		--Location = { BZ["Gilneas"] };
		LevelRange = "85+";
		MinLevel = "85";
		{ BLUE.."A) "..BZ["Gilnean Stronghold"].." ("..FACTION_ALLIANCE..")" };
		{ ORNG..INDENT..AL["Alliance Graveyard"] };
		{ BLUE.."B) "..BZ["Horde Landing"].." ("..FACTION_HORDE..")" };
		{ GREN.."1) "..BZ["Mines"] };
		{ GREN.."2) "..BZ["Lighthouse"] };
		{ GREN.."3) "..BZ["Waterworks"] };
		{ ORNG.."1) "..AL["Horde Graveyard"] };
		{ ORNG.."2) "..AL["Contested Graveyards"] };
	};
	TolBarad = {
		ZoneName = { BZ["Tol Barad"] };
		--Location = { BZ["Tol Barad"] };
		LevelRange = "80-85";
		MinLevel = "80";
		{ ORNG..ALC["Reputation"]..": "..ALIL["Baradin's Wardens"].." ("..FACTION_ALLIANCE..")" };
		{ ORNG..ALC["Reputation"]..": "..ALIL["Hellscream's Reach"].." ("..FACTION_HORDE..")" };
		{ BLUE.."A) "..AL["Attackers"] };
		{ BLUE.."B) "..BZ["Baradin Hold"] };
		{ GREN..INDENT..FACTION_ALLIANCE };
		{ GREN..INDENT..BULLET..AL["Sergeant Parker <Baradin's Wardens>"]};
		{ GREN..INDENT..BULLET..AL["2nd Lieutenant Wansworth <Baradin's Wardens>"]};
		{ GREN..INDENT..BULLET..AL["Commander Stevens <Baradin's Wardens>"]};
		{ GREN..INDENT..BULLET..AL["Marshal Fallows <Baradin's Wardens>"]};
		{ GREN..INDENT..FACTION_HORDE };
		{ GREN..INDENT..BULLET..AL["Commander Zanoth <Hellscream's Reach>"] };
		{ GREN..INDENT..BULLET..AL["Drillmaster Razgoth <Hellscream's Reach>"] };
		{ GREN..INDENT..BULLET..AL["Private Garnoth <Hellscream's Reach>"] };
		{ GREN..INDENT..BULLET..AL["Staff Sergeant Lazgar <Hellscream's Reach>"] };
		{ GREN.."1) "..BZ["Ironclad Garrison"] };
		{ GREN.."2) "..BZ["Warden's Vigil"] };
		{ GREN.."3) "..BZ["Slagworks"] };
		{ GREN.."1') "..ALC["Meeting Stone"] };
		{ _RED.."1) "..BZ["West Spire"] };
		{ _RED.."2) "..BZ["East Spire"] };
		{ _RED.."3) "..BZ["South Spire"] };
		{ ORNG.."1) "..ALC["Graveyard"] };
		{ ORNG.."2) "..BZ["Cursed Depths"] };
		{ ORNG.."3) "..BZ["The Hole"] };
		{ ORNG.."4) "..BZ["D-Block"] };
	};
	TwinPeaks = {
		ZoneName = { BZ["Twin Peaks"] };
		Location = { BZ["Twilight Highlands"] };
		LevelRange = "85+";
		MinLevel = "85";
		{ ORNG..ALC["Reputation"]..": "..ALIL["Wildhammer Clan"].." ("..FACTION_ALLIANCE..")" };
		{ ORNG..ALC["Reputation"]..": "..ALIL["Dragonmaw Clan"].." ("..FACTION_HORDE..")" };
		{ BLUE.."A) "..AL["Wildhammer Longhouse"].." ("..FACTION_ALLIANCE..")" };
		{ BLUE.."B) "..AL["Dragonmaw Clan Compound"].." ("..FACTION_HORDE..")" };
		{ ORNG.."1) "..AL["Alliance Graveyard"] };
		{ ORNG.."2) "..AL["Horde Graveyard"] };
	};
	SilvershardMines = {
		ZoneName = { BZ["Silvershard Mines"] };
		Location = { BZ["Stranglethorn Vale"]..", "..BZ["Eastern Kingdoms"] };
		LevelRange = "90";
		MinLevel = "90";
		PlayerLimit = "10";
		{ BLUE.."A) "..ALC["Start"].." - "..FACTION_ALLIANCE };
		{ BLUE.."B) "..ALC["Start"].." - "..FACTION_HORDE };
		{ GREN.."1) "..AL["Mine Cart Spawn Point"] };
		{ GREN.."2) "..AL["Mine Cart Depot"] };
	};
	TempleofKotmogu = {
		ZoneName = { BZ["Temple of Kotmogu"] };
		Location = { BZ["Vale of Eternal Blossoms"]..", "..BZ["Pandaria"] };
		LevelRange = "90";
		MinLevel = "90";
		PlayerLimit = "10";
		{ BLUE.."A) "..ALC["Start"].." - "..FACTION_ALLIANCE };
		{ BLUE.."B) "..ALC["Start"].." - "..FACTION_HORDE };
		{ GREN.."1) "..AL["Orb of Power"] };
		{ GREN.."2) "..AL["Center Point (Maximum Points)"] };
	};
	DeepwindGorge = {
		ZoneName = { BZ["Deepwind Gorge"] };
		Location = { BZ["Valley of the Four Winds"]..", "..BZ["Pandaria"] };
		LevelRange = "90";
		MinLevel = "90";
		PlayerLimit = "15";
		{ BLUE.."A) "..ALC["Start"].." - "..FACTION_HORDE };
		{ BLUE.."B) "..ALC["Start"].." - "..FACTION_ALLIANCE };
		{ GREN.."1) "..BZ["Pandaren Mine"] };
		{ GREN.."2) "..AL["Center Mine"] };
		{ GREN.."3) "..BZ["Goblin Mine"] };
	};
};

Atlas_RegisterPlugin("Atlas_Battlegrounds", myCategory, myData);
