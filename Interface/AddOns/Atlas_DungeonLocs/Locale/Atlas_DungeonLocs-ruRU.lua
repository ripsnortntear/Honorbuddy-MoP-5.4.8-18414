-- $Id: Atlas_DungeonLocs-ruRU.lua 1963 2013-03-30 19:04:14Z Dynaletik $
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

--[[

-- Atlas Data  (Russian)
-- Compiled by StingerSoft
-- stingersoft@gmail.com
-- Last Update: $Date: 2013-03-31 03:04:14 +0800 (週日, 31 三月 2013) $

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_DungeonLocs", "ruRU", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_DungeonLocs", "deDE", false);
if AL then
	--Common
	AL["Battlegrounds"] = "Поля сражений";
	AL["Blue"] = "Синий";
	AL["Dungeon Locations"] = "Расположение подземелий";
	AL["Green"] = "Зеленый";
	AL["Instances"] = "Инстансы";
	AL["White"] = "Белый";
end
