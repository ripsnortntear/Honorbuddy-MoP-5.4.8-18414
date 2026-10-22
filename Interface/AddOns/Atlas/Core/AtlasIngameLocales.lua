-- $Id: AtlasIngameLocales.lua 2087 2013-09-09 14:50:57Z arithmandar $
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


Atlas_IngameLocales = {
	-- ######################################################################
	--- Items
	-- ######################################################################
	-- Keys
	["Burial Trove Key"] = GetItemInfo(95497),
	["Dark Keeper Key"] = GetItemInfo(11197),
	["J'eevee's Jar"] = GetItemInfo(18663),
	["Key to the Palace of Lei Shen"] = GetItemInfo(94222),
	["Relic Coffer Key"] = GetItemInfo(11078),
	["The Eye of Haramad"] = GetItemInfo(32092),

	-- ######################################################################
	-- Factions
	-- ######################################################################
	-- Mists of Pandaria
	["Shado-Pan Assault"] = GetFactionInfoByID(1435),
	["The August Celestials"] = GetFactionInfoByID(1341),

	-- Cataclysm
	["Avengers of Hyjal"] = GetFactionInfoByID(1204),
	["Baradin's Wardens"] = GetFactionInfoByID(1177),
	["Dragonmaw Clan"] = GetFactionInfoByID(1172),
	["Hellscream's Reach"] = GetFactionInfoByID(1178),
	["Wildhammer Clan"] = GetFactionInfoByID(1174),

	-- Wrath of the Lich King
	["The Ashen Verdict"] = GetFactionInfoByID(1156),

	-- Burning Crusade
	["Ashtongue Deathsworn"] = GetFactionInfoByID(1012),
	["Cenarion Expedition"] = GetFactionInfoByID(942),
	["Honor Hold"] = GetFactionInfoByID(946),
	["Keepers of Time"] = GetFactionInfoByID(989),
	["Lower City"] = GetFactionInfoByID(1011),
	["Shattered Sun Offensive"] = GetFactionInfoByID(1077),
	["The Aldor"] = GetFactionInfoByID(932),
	["The Consortium"] = GetFactionInfoByID(933),
	["The Scale of the Sands"] = GetFactionInfoByID(990),
	["The Scryers"] = GetFactionInfoByID(934),
	["The Sha'tar"] = GetFactionInfoByID(935),
	["The Violet Eye"] = GetFactionInfoByID(967),
	["Thrallmar"] = GetFactionInfoByID(947),

	-- Classic
	["Brood of Nozdormu"] = GetFactionInfoByID(910),
	["Cenarion Circle"] = GetFactionInfoByID(609),
	["Frostwolf Clan"] = GetFactionInfoByID(729),
	["Hydraxian Waterlords"] = GetFactionInfoByID(749),
	["Silverwing Sentinels"] = GetFactionInfoByID(890),
	["Stormpike Guard"] = GetFactionInfoByID(730),
	["The Defilers"] = GetFactionInfoByID(510),
	["The League of Arathor"] = GetFactionInfoByID(509),
	["Warsong Outriders"] = GetFactionInfoByID(889),
}

do
	setmetatable(Atlas_IngameLocales, {__index = function(tab, key)
		return rawget(tab, key) or key end
	})
end
