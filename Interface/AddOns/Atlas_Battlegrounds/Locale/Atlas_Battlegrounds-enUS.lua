-- $Id: Atlas_Battlegrounds-enUS.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "enUS", true);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "Battleground Maps";
	AL["Rescued"] = "Rescued";
	AL["Span of 5"] = "Span of 5"; -- Blizzard's span to put players with similar level range into a BG (10-14, 15-29)

	--Places
	AL["AV"] = "AV"; -- Alterac Valley
	AL["AB"] = "AB"; -- Arathi Basin
	AL["EotS"] = "EotS";
	AL["IoC"] = "IoC"; -- Isle of Conquest
	AL["SotA"] = "SotA"; -- Strand of the Ancients
	AL["WSG"] = "WSG"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "Vanndar Stormpike <Stormpike General>";
	AL["Prospector Stonehewer"] = "Prospector Stonehewer";
	AL["Dun Baldar North Bunker"] = "Dun Baldar North Bunker";
	AL["Wing Commander Mulverick"] = "Wing Commander Mulverick";--omitted from AVS
	AL["Dun Baldar South Bunker"] = "Dun Baldar South Bunker";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "Gaelden Hammersmith <Stormpike Supply Officer>";
	AL["Stormpike Banner"] = "Stormpike Banner";
	AL["Stormpike Lumber Yard"] = "Stormpike Lumber Yard";
	AL["Wing Commander Jeztor"] = "Wing Commander Jeztor";--omitted from AVS
	AL["Wing Commander Guse"] = "Wing Commander Guse";--omitted from AVS
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "Captain Balinda Stonehearth <Stormpike Captain>";
	AL["Western Crater"] = "Western Crater";
	AL["Vipore's Beacon"] = "Vipore's Beacon";
	AL["Jeztor's Beacon"] = "Jeztor's Beacon";
	AL["Eastern Crater"] = "Eastern Crater";
	AL["Slidore's Beacon"] = "Slidore's Beacon";
	AL["Guse's Beacon"] = "Guse's Beacon";
	AL["Arch Druid Renferal"] = "Arch Druid Renferal";
	AL["Murgot Deepforge"] = "Murgot Deepforge";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "Lana Thunderbrew <Blacksmithing Supplies>";
	AL["Stormpike Stable Master <Stable Master>"] = "Stormpike Stable Master <Stable Master>";
	AL["Stormpike Ram Rider Commander"] = "Stormpike Ram Rider Commander";
	AL["Svalbrad Farmountain <Trade Goods>"] = "Svalbrad Farmountain <Trade Goods>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "Kurdrum Barleybeard <Reagents & Poison Supplies>";
	AL["Stormpike Quartermaster"] = "Stormpike Quartermaster";
	AL["Jonivera Farmountain <General Goods>"] = "Jonivera Farmountain <General Goods>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "Brogus Thunderbrew <Food & Drink>";
	AL["Wing Commander Ichman"] = "Wing Commander Ichman";--omitted from AVS
	AL["Wing Commander Slidore"] = "Wing Commander Slidore";--omitted from AVS
	AL["Wing Commander Vipore"] = "Wing Commander Vipore";--omitted from AVS
	AL["Stormpike Ram Rider Commander"] = "Stormpike Ram Rider Commander";
	AL["Ivus the Forest Lord"] = "Ivus the Forest Lord";
	AL["Stormpike Aid Station"] = "Stormpike Aid Station";
	AL["Ichman's Beacon"] = "Ichman's Beacon";
	AL["Mulverick's Beacon"] = "Mulverick's Beacon";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "Drek'Thar <Frostwolf General>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "Captain Galvangar <Frostwolf Captain>";
	AL["Iceblood Tower"] = "Iceblood Tower";
	AL["Tower Point"] = "Tower Point";
	AL["West Frostwolf Tower"] = "West Frostwolf Tower";
	AL["East Frostwolf Tower"] = "East Frostwolf Tower";
	AL["Frostwolf Banner"] = "Frostwolf Banner";
	AL["Lokholar the Ice Lord"] = "Lokholar the Ice Lord";
	AL["Jotek"] = "Jotek";
	AL["Smith Regzar"] = "Smith Regzar";
	AL["Primalist Thurloga"] = "Primalist Thurloga";
	AL["Frostwolf Stable Master <Stable Master>"] = "Frostwolf Stable Master <Stable Master>";
	AL["Frostwolf Wolf Rider Commander"] = "Frostwolf Wolf Rider Commander";
	AL["Frostwolf Quartermaster"] = "Frostwolf Quartermaster";
	AL["Frostwolf Relief Hut"] = "Frostwolf Relief Hut";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "The Silithyst Must Flow";
	AL["Alliance's Camp"] = "Alliance's Camp";
	AL["Horde's Camp"] = "Horde's Camp";

	--Eye of the Storm
	AL["Flag"] = "Flag";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "Quartermaster Davian Vaclav";
	AL["Chief Researcher Kartos"] = "Chief Researcher Kartos";
	AL["Aldraan <Blade Merchant>"] = "Aldraan <Blade Merchant>";
	AL["Cendrii <Food & Drink>"] = "Cendrii <Food & Drink>";
	AL["Quartermaster Jaffrey Noreliqe"] = "Quartermaster Jaffrey Noreliqe";
	AL["Chief Researcher Amereldine"] = "Chief Researcher Amereldine";
	AL["Coreiel <Blade Merchant>"] = "Coreiel <Blade Merchant>";
	AL["Embelar <Food & Drink>"] = "Embelar <Food & Drink>";
	AL["Wyvern Camp"] = "Wyvern Camp";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "Hellfire Fortifications";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "Spirit Towers";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "West Beacon";
	AL["East Beacon"] = "East Beacon";
	AL["Horde Field Scout"] = "Horde Field Scout";
	AL["Alliance Field Scout"] = "Alliance Field Scout";
	AL["Twinspire Graveyard"] = "Twinspire Graveyard";

	--Isle of Conquest
	AL["Gates are marked with red bars."] = "Gates are marked with red bars.";
	AL["Overlord Agmar"] = "Overlord Agmar";
	AL["High Commander Halford Wyrmbane <7th Legion>"] = "High Commander Halford Wyrmbane <7th Legion>";
	AL["The Refinery"] = "The Refinery";
	AL["The Docks"] = "The Docks";
	AL["The Workshop"] = "The Workshop";
	AL["The Hangar"] = "The Hangar";
	AL["The Quarry"] = "The Quarry";
	AL["Contested Graveyards"] = "Contested Graveyards";--omitted from Gilneas
	AL["Horde Graveyard"] = "Horde Graveyard";--omitted from Gilneas
	AL["Alliance Graveyard"] = "Alliance Graveyard";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "Gates are marked with their colors.";
	AL["Attacking Team"] = "Attacking Team";
	AL["Defending Team"] = "Defending Team";
	AL["Massive Seaforium Charge"] = "Massive Seaforium Charge";
	AL["Titan Relic"] = "Titan Relic";
	AL["Battleground Demolisher"] = "Battleground Demolisher";
	AL["Graveyard Flag"] = "Graveyard Flag";
	AL["Resurrection Point"] = "Resurrection Point";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "Fortress Vihecal Workshop (E)";
	AL["Fortress Vihecal Workshop (W)"] = "Fortress Vihecal Workshop (W)";
	AL["Sunken Ring Vihecal Workshop"] = "Sunken Ring Vihecal Workshop";
	AL["Broken Temple Vihecal Workshop"] = "Broken Temple Vihecal Workshop";
	AL["Eastspark Vihecale Workshop"] = "Eastspark Vihecale Workshop";
	AL["Westspark Vihecale Workshop"] = "Westspark Vihecale Workshop";
	AL["Wintergrasp Graveyard"] = "Wintergrasp Graveyard";
	AL["Sunken Ring Graveyard"] = "Sunken Ring Graveyard";
	AL["Broken Temple Graveyard"] = "Broken Temple Graveyard";
	AL["Southeast Graveyard"] = "Southeast Graveyard";
	AL["Southwest Graveyard"] = "Southwest Graveyard";

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "Attackers";
	AL["Sergeant Parker <Baradin's Wardens>"] = "Sergeant Parker <Baradin's Wardens>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "2nd Lieutenant Wansworth <Baradin's Wardens>";
	AL["Commander Stevens <Baradin's Wardens>"] = "Commander Stevens <Baradin's Wardens>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "Marshal Fallows <Baradin's Wardens>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "Commander Zanoth <Hellscream's Reach>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "Drillmaster Razgoth <Hellscream's Reach>";
	AL["Private Garnoth <Hellscream's Reach>"] = "Private Garnoth <Hellscream's Reach>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "Staff Sergeant Lazgar <Hellscream's Reach>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "Wildhammer Longhouse";
	AL["Dragonmaw Clan Compound"] = "Dragonmaw Clan Compound";

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Mine Cart Spawn Point";
	AL["Mine Cart Depot"] = "Mine Cart Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Orb of Power";
	AL["Center Point (Maximum Points)"] = "Center Point (Maximum Points)";

	-- Deepwind Gorge
	AL["Center Mine"] = "Center Mine";
end