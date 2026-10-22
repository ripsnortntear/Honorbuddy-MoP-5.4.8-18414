-- $Id: Atlas_Transportation-ruRU.lua 2136 2013-12-16 21:25:55Z StingerSoft@gmail.com $
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
-- Atlas Localization Data (Russian)
-- Translated by Nitrogen (Exorsus Guild)
-- Свежеватель Душ
-- Last Update : $Date: 2013-12-17 05:25:55 +0800 (週二, 17 十二月 2013) $
--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Transportation", "ruRU", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	AL["The Bogpaddle Bullet"] = "Болотный экспресс";
	AL["Death Knight Only"] = "Только Рыцарю Смерти";
	AL["Druid-only"] = "Только Друиду";
	AL["Legend"] = "Легенда";	-- The chart's legend, for example, the purple line means the portal's path
	AL["Gryphon"] = "Gryphon";
	AL["Only available after winning the PvP battle"] = "Доступно только после победы в PvP сражении";
	AL["Orb of Translocation"] = "Шар Транслокации";
	AL["Portals"] = "Порталы";	
	AL["Portal / Waygate Path to the destination"] = "Портал / путь по Связующей спирали";
	AL["Ship / Zeppelin sailing path to destination"] = "Путь Корабля / Цеппелина";
	AL["Requires honored faction with Sha'tari Skyguard"] = "Требуется Уважение с фракцией Стражи Небес Ша'тар";
	AL["Seahorse"] = "Морской конек"; -- Need check
	AL["South of the path along Lake Elune'ara"] = "Южный путь вдоль Озера Алуне'ара";
	AL["Special transportation"] = "Особый транспорт";
	AL["Taxi Nodes"] = "Мастера полетов";
	AL["Transportation Maps"] = "Маршруты полётов";
	AL["Transporter"] = "Транспорт";			-- The NPC who can transport you to other place
	AL["Transporters by the sea and on the cliff"] = "Транспорт у моря и утесов"; -- The transporters (machine) can be found at Fuselight-by-the-Sea
	AL["West of the path to Timbermaw Hold"] = "Западный путь в Крепость Древобрюхов";
	AL["Wind Rider"] = "Укротитель ветрокрылов";
	AL["Won't be available once the Battle for Andorhal chain is finished."] = "Не будет доступна пока не завершена серия Битвы за Андорал."; -- After quest "Alas, Andorhal" (27206) is completed.
	AL["Zeppelin Towers"] = "Башни Цеппелинов";
	AL["Climbing Rope"] = "Канат";
	AL["Rappelling Rope"] = "Канат для спуска";
	AL["Abandoned Kite"] = "Брошенный воздушный змей";
	AL["From sea level to ground level"] = "От уровня моря к земле";
	AL["Whispercloud's Balloon"] = "Воздушный шар Шепота Облака";
	AL["Shado-Pan Rope"] = "Шадопанская веревка"; -- 66390
	AL["Require to complete \"Meet the Scout\" quest line first."] = "Необходимо сначала выполнить серию заданий \"Встреча с разведчицей\".";
end