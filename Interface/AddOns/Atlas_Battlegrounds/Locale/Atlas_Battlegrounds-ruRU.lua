-- $Id: Atlas_Battlegrounds-ruRU.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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
-- Last Update: 27.09.2008

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "ruRU", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "Карты Полей Сражений";
	AL["Rescued"] = "Спасенный";
	AL["Span of 5"] = "Диапазон: 5"; -- Blizzard's span to put players with similar level range into a BG (10-14, 15-29)

	--Places
	AL["AV"] = "АД"; -- Alterac Valley
	AL["AB"] = "НА"; -- Arathi Basin
	AL["EotS"] = "Око";
	AL["IoC"] = "ОЗ"; -- Isle of Conquest
	AL["SotA"] = "Берег"; -- Strand of the Ancients
	AL["WSG"] = "УПВ"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "Вандар Грозовая Вершина <Генерал клана Грозовой Вершины>";
	AL["Prospector Stonehewer"] = "Геолог Камнетерка";
	AL["Dun Baldar North Bunker"] = "Северный Оплот Дун Болдара";
	AL["Wing Commander Mulverick"] = "Командир звена Маэстр";--omitted from AVS
	AL["Dun Baldar South Bunker"] = "Южный Оплот Дун Болдара";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "Гаелден Кузнечный Молот <Снабженец клана Грозовой Вершины>";
	AL["Stormpike Banner"] = "Знамя Грозовой Вершины";
	AL["Stormpike Lumber Yard"] = "Лесопилка Грозовой Вершины";
	AL["Wing Commander Jeztor"] = "Командир звена Мааша";--omitted from AVS
	AL["Wing Commander Guse"] = "Командир звена Смуггл";--omitted from AVS
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "Капитан Балинда Каменный Очаг <Капитан клана Грозовой Вершины>";
	AL["Western Crater"] = "Западный Кратер";
	AL["Vipore's Beacon"] = "Маяк Сквороца";
	AL["Jeztor's Beacon"] = "Маяк Мааша";
	AL["Eastern Crater"] = "Восточный Кратер";
	AL["Slidore's Beacon"] = "Маяк Макарча";
	AL["Guse's Beacon"] = "Маяк Смуггла";
	AL["Arch Druid Renferal"] = "Верховный друид Дикая Лань";
	AL["Murgot Deepforge"] = "Мургот Подземная Кузня";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "Лана Грозовар <Товары для кузнецов>";
	AL["Stormpike Stable Master <Stable Master>"] = "Смотритель стойл из клана Грозовой Вершины <Смотритель стойл>";
	AL["Stormpike Ram Rider Commander"] = "Командир наездников на баранах из клана Грозовой Вершины";
	AL["Svalbrad Farmountain <Trade Goods>"] = "Свальбрад Дальногор <Хозяйственные товары>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "Курдрум Ячменобород <Реагенты и яды>";
	AL["Stormpike Quartermaster"] = "Интендант клана Грозовой Вершины";
	AL["Jonivera Farmountain <General Goods>"] = "Джонивера Дальняя Гора <Потребительские товары>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "Брогус Грозовар <Еда и напитки>";
	AL["Wing Commander Ichman"] = "Командир звена Ичман";--omitted from AVS
	AL["Wing Commander Slidore"] = "Командир звена Макарч";--omitted from AVS
	AL["Wing Commander Vipore"] = "Командир звена Сквороц";--omitted from AVS
	AL["Stormpike Ram Rider Commander"] = "Командир наездников на баранах из клана Грозовой Вершины";
	AL["Ivus the Forest Lord"] = "Ивус Лесной Властелин";
	AL["Stormpike Aid Station"] = "Лазарет Грозовой Вершины";
	AL["Ichman's Beacon"] = "Маяк Ичмена";
	AL["Mulverick's Beacon"] = "Маяк Малверика";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "Дрек'Тар <Генерал клана Северного Волка>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "Капитан Гальвангар <Капитан клана Северного Волка>";
	AL["Iceblood Tower"] = "Башня Стылой Крови";
	AL["Tower Point"] = "Смотровая башня";
	AL["West Frostwolf Tower"] = "Западная башня Северного Волка";
	AL["East Frostwolf Tower"] = "Восточная башня Северного Волка";
	AL["Frostwolf Banner"] = "Знамя Северного Волка";
	AL["Lokholar the Ice Lord"] = "Локолар Владыка Льда";
	AL["Jotek"] = "Джотек";
	AL["Smith Regzar"] = "Кузнец Регзар";
	AL["Primalist Thurloga"] = "Старейшина Турлога";
	AL["Frostwolf Stable Master <Stable Master>"] = "Смотритель стойл из клана Северного Волка <Смотритель стойл>";
	AL["Frostwolf Wolf Rider Commander"] = "Командир наездников на волках клана Северного Волка";
	AL["Frostwolf Quartermaster"] = "Интендант клана Северного Волка";
	AL["Frostwolf Relief Hut"] = "Приют Северного Волка";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "The Silithyst Must Flow";
	AL["Alliance's Camp"] = "Лагерь альянса";
	AL["Horde's Camp"] = "лагерь орды";

	--Eye of the Storm
	AL["Flag"] = "Флаг";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "Интендант Дэвиан Ваклав";
	AL["Chief Researcher Kartos"] = "Старший ученый Картос";
	AL["Aldraan <Blade Merchant>"] = "Алдраан <Торговец клинками>";
	AL["Cendrii <Food & Drink>"] = "Сендри <Еда и напитки>";
	AL["Quartermaster Jaffrey Noreliqe"] = "Интендант Джеффри Норелик";
	AL["Chief Researcher Amereldine"] = "Старший ученый Амерельдина";
	AL["Coreiel <Blade Merchant>"] = "Кориэль <Торговец клинками>";
	AL["Embelar <Food & Drink>"] = "Янталар <Еда и напитки>";
	AL["Wyvern Camp"] = "Гнездо виверн";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "Штурмовые укрепления";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "Башни Духов";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "Западный Маяк";
	AL["East Beacon"] = "Восточный Маяк";
	AL["Horde Field Scout"] = "Боевой разведчик Орды";
	AL["Alliance Field Scout"] = "Боевой разведчик Альянса";
	AL["Twinspire Graveyard"] = "Кладбище Двух Башен";

	--Isle of Conquest
	AL["Gates are marked with red bars."] = "Ворота помечены красным.";
	AL["Overlord Agmar"] = "Командир Агмар";
	AL["High Commander Halford Wyrmbane <7th Legion>"] = "Главнокомандующий Халфорд Змеевержец <7-й легион>";
	AL["The Refinery"] = "Нефтезавод";
	AL["The Docks"] = "Причал";
	AL["The Workshop"] = "Мастерская";
	AL["The Hangar"] = "Ангар	";
	AL["The Quarry"] = "Каменоломня";
	AL["Contested Graveyards"] = "Спорные Кладбища";
	AL["Horde Graveyard"] = "Кладбище Орды";
	AL["Alliance Graveyard"] = "Кладбище Альянса";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "Ворота, отмечены их цветами.";
	AL["Attacking Team"] = "Группа штурма";
	AL["Defending Team"] = "Группа защиты";
	AL["Massive Seaforium Charge"] = "Сверхмощный сефориевый заряд";
	AL["Titan Relic"] = "Реликвия титанов";
	AL["Battleground Demolisher"] = "Разрушитель";
	AL["Graveyard Flag"] = "Кладбище";
	AL["Resurrection Point"] = "Точки воскрешения";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "Восточная мастерской в крепости";
	AL["Fortress Vihecal Workshop (W)"] = "Западная мастерская в крепости";
	AL["Sunken Ring Vihecal Workshop"] = "Мастерской в Затопленном Круге";
	AL["Broken Temple Vihecal Workshop"] = "Мастерской в Павшем храме";
	AL["Eastspark Vihecale Workshop"] = "Мастерской в Восточном парке";
	AL["Westspark Vihecale Workshop"] = "Мастерская в Западном парке";
	AL["Wintergrasp Graveyard"] = "Кладбище Озера Ледяных Оков";
	AL["Sunken Ring Graveyard"] = "Кладбище Затопленного Круга";
	AL["Broken Temple Graveyard"] = "Кладбище Павшего храма";
	AL["Southeast Graveyard"] = "Юго-Восточное кладбище";
	AL["Southwest Graveyard"] = "Юго-Западное кладбище";

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "Attackers";
	AL["Sergeant Parker <Baradin's Wardens>"] = "Сержант Паркер <Защитники Тол Барада>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "Второй лейтенант Вансворт <Защитники Тол Барада>";
	AL["Commander Stevens <Baradin's Wardens>"] = "Командир Стивенс <Защитники Тол Барада>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "Маршал Фэллоус <Защитники Тол Барада>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "Командир Занот <Батальон Адского Крика>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "Военный инструктор Разгот <Батальон Адского Крика>";
	AL["Private Garnoth <Hellscream's Reach>"] = "Рядовой Гарнот <Батальон Адского Крика>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "Штаб-сержант Лазгар <Батальон Адского Крика>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "Клан Громового Молота";
	AL["Dragonmaw Clan Compound"] = "Клан Драконьей Пасти";

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Mine Cart Spawn Point";
	AL["Mine Cart Depot"] = "Mine Cart Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Orb of Power";
	AL["Center Point (Maximum Points)"] = "Center Point (Maximum Points)";

	-- Deepwind Gorge
	AL["Center Mine"] = "Центральная шахта"; --
end