-- $Id: Atlas-ruRU.lua 2124 2013-09-16 23:24:27Z StingerSoft@gmail.com $
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
local AL = AceLocale:NewLocale("Atlas", "ruRU", false);

-- Atlas Russian Localization
-- Compiled by Eugene Filatov, bigoblin, StingerSoft
-- Last Update: 23.01.2011
-- $Date: 2013-09-17 07:24:27 +0800 (週二, 17 九月 2013) $
-- $Revision: 2124 $

if ( GetLocale() == "ruRU" ) then
-- Define the leading strings to be ignored while sorting
-- Ex: The Stockade
AtlasSortIgnore = {"(.+)"};

-- Syntax: ["real_zone_name"] = "localized map zone name"
AtlasZoneSubstitutions = {
	["Ahn'Qiraj"] = "Ан'Кираж";
	["The Temple of Atal'Hakkar"] = "Храм Атал'Хаккара";
--	["Throne of Tides"] = "Бездонная пучина: Трон Приливов";
};
end


if AL then
--************************************************
-- UI terms and common strings
--************************************************
	AL["ATLAS_TITLE"] = "Атлас";

	AL["BINDING_HEADER_ATLAS_TITLE"] = "Сопоставления кнопок";
	AL["BINDING_NAME_ATLAS_TOGGLE"] = "Атлас";
	AL["BINDING_NAME_ATLAS_OPTIONS"] = "Настройки Атласа";
	AL["BINDING_NAME_ATLAS_AUTOSEL"] = "Авто-выбор поздемелья";

	AL["ATLAS_SLASH"] = "/atlas";
	AL["ATLAS_SLASH_OPTIONS"] = "options";

	AL["ATLAS_STRING_LOCATION"] = "Расположение";
	AL["ATLAS_STRING_LEVELRANGE"] = "Уровень"; -- shorten from "Level Range" as we are running out of space
	AL["ATLAS_STRING_RECLEVELRANGE"] = "Реком. уровень"; -- abbrevation and shorten of "Recommended Level Range", the dungeon's recommended level range
	AL["ATLAS_STRING_PLAYERLIMIT"] = "Лимит игроков";
	AL["ATLAS_STRING_SELECT_CAT"] = "Выбор категории";
	AL["ATLAS_STRING_SELECT_MAP"] = "Выбор карты";
	AL["ATLAS_STRING_SEARCH"] = "Поиск";
	AL["ATLAS_STRING_CLEAR"] = "Сбросить";
	AL["ATLAS_STRING_MINLEVEL"] = "Минимальный уровень";

	AL["ATLAS_OPTIONS_BUTTON"] = "Настройки";
	AL["ATLAS_OPTIONS_SHOWBUT"] = "Показывать кнопку у мини-карты";
	AL["ATLAS_OPTIONS_SHOWBUT_TIP"] = "Отображать кнопку Атласа у мини-карты.";
	AL["ATLAS_OPTIONS_AUTOSEL"] = "Автоматический выбор поздемелья";
	AL["ATLAS_OPTIONS_AUTOSEL_TIP"] = "Автоматический выбор карты поздемелья, Атлас будет определить ваше местоположение, чтобы выбрать лучшую карту подземелья для вас.";
	AL["ATLAS_OPTIONS_BUTPOS"] = "Расположение кнопки";
	AL["ATLAS_OPTIONS_LOCK"] = "Закрепить окно Атласа";
	AL["ATLAS_OPTIONS_LOCK_TIP"] = "Закрепить / освободить окно Атласа.";
	AL["ATLAS_OPTIONS_TRANS"] = "Прозрачность";
	AL["ATLAS_OPTIONS_RCLICK"] = "[ПКМ] для карты мира";
	AL["ATLAS_OPTIONS_RCLICK_TIP"] = "Включает отображение мировой карты при нажатии ПКМ в окне Атласа.";
	AL["ATLAS_OPTIONS_RESETPOS"] = "Сбросить позиции";
	AL["ATLAS_OPTIONS_ACRONYMS"] = "Короткие названия";
	AL["ATLAS_OPTIONS_ACRONYMS_TIP"] = "Будут отображаться сокрощенные названия подземелий в информации о карте.";
	AL["ATLAS_OPTIONS_SCALE"] = "Размер";
	AL["ATLAS_OPTIONS_BOSS_DESC"] = "Показать описание босса (если доступно)";
	AL["ATLAS_OPTIONS_BOSS_DESC_TIP"] = "При наведении курсора мышки над номером босса, будет показано описание босса, если такая информация доступна.";
	AL["ATLAS_OPTIONS_BOSS_DESC_SCALE"] = "Размер подсказки описания босса на карте";
	AL["ATLAS_OPTIONS_BUTRAD"] = "Радиус расположения кнопки";
	AL["ATLAS_OPTIONS_CLAMPED"] = "Не заходить за размеры экрана";
	AL["ATLAS_OPTIONS_CLAMPED_TIP"] = "Фиксировать окно Атласа на экране, отключение позволит перемещать окно Атласа за пределы игрового экрана.";
	AL["ATLAS_OPTIONS_CTRL"] = "Удерживайте клавишу [CTRL] для сравнений";
	AL["ATLAS_OPTIONS_CTRL_TIP"] = "Включить/отключить отображение подсказки при удерживании клавиши CTRL и наведении курсора мышки на информационной карте. Полезно тогда, когда текст слишком длинный, для отображения в окне.";
	AL["ATLAS_OPTIONS_DONTSHOWAGAIN"] = "Не отображать одинаковую информацию снова.";
	AL["ATLAS_OPTIONS_CHECKMODULE"] = "Напоминать о недостоющих модулях / плагинах.";
	AL["ATLAS_OPTIONS_CHECKMODULE_TIP"] = "Выполнение проверки после загрузки WoW, на наличие недостающих модулей / плагинов Atlas'а.";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN"] = "Цвет ур. сложности подземелья";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN_TIP"] = "Полагаясь на предложенный минимальный уровень подземелья и уровень игрока, окрашивать названия подземелий с учетом их уровня сложности.";

	AL["ATLAS_BUTTON_CLOSE"] = "Закрыть";	
	AL["ATLAS_BUTTON_TOOLTIP_TITLE"] = "Атлас";
	AL["ATLAS_BUTTON_TOOLTIP_HINT"] = "[ЛКМ] - открывает Атлас.\n[СКМ] - открывает настройки Атласа.\n[ПКМ] + [перемещение] - изменяет позицию кнопки Атласа.";
	AL["ATLAS_LDB_HINT"] = "[ЛКМ] - открывает Атлас.\n[ПКМ] - открывает настройки Атласа.";
	AL["ATLAS_MINIMAPLDB_HINT"] = "[ЛКМ] - открывает Атлас.\n[ПКМ] + открывает настройки Атласа.\n[ЛКМ] + [перемещение] - изменяет позицию кнопки.";

	AL["ATLAS_OPTIONS_CATDD"] = "Сортировать подземелья по:";
	AL["ATLAS_DDL_CONTINENT"] = "Континенту";
	AL["ATLAS_DDL_CONTINENT_EASTERN"] = "Подземелья Восточных королевств";
	AL["ATLAS_DDL_CONTINENT_KALIMDOR"] = "Подземелья Калимдора";
	AL["ATLAS_DDL_CONTINENT_OUTLAND"] = "Подземелья Запределья";
	AL["ATLAS_DDL_CONTINENT_NORTHREND"] = "Подземелья Нордскола";
	AL["ATLAS_DDL_CONTINENT_DEEPHOLM"] = "Подземелья Подземья";
	AL["ATLAS_DDL_CONTINENT_PANDARIA"] = "Подземелья Пандории";
	AL["ATLAS_DDL_LEVEL"] = "Уровню";
	AL["ATLAS_DDL_LEVEL_UNDER45"] = "Подземелья уровня ниже 45";
	AL["ATLAS_DDL_LEVEL_45TO60"] = "Подземелья уровня 45-60";
	AL["ATLAS_DDL_LEVEL_60TO70"] = "Подземелья уровня 60-70";
	AL["ATLAS_DDL_LEVEL_70TO80"] = "Подземелья уровня 70-80";
	AL["ATLAS_DDL_LEVEL_80TO85"] = "Подземелья уровня 80-85";
	AL["ATLAS_DDL_LEVEL_85TO90"] = "Подземелья уровня 85-90";
	AL["ATLAS_DDL_LEVEL_90PLUS"] = "Подземелья уровня 90+";
	AL["ATLAS_DDL_PARTYSIZE"] = "Размеру группы";
	AL["ATLAS_DDL_PARTYSIZE_5_AE"] = "Подземелья на 5 игроков A-E";
	AL["ATLAS_DDL_PARTYSIZE_5_FS"] = "Подземелья на 5 игроков F-S";
	AL["ATLAS_DDL_PARTYSIZE_5_TZ"] = "Подземелья на 5 игроков T-Z";
	AL["ATLAS_DDL_PARTYSIZE_10_AN"] = "Подземелья на 10 игроков A-N";
	AL["ATLAS_DDL_PARTYSIZE_10_OZ"] = "Подземелья на 10 игроков O-Z";
	AL["ATLAS_DDL_PARTYSIZE_20TO40AH"] = "Подземелья на 20-40 игроков A-H";
	AL["ATLAS_DDL_PARTYSIZE_20TO40IZ"] = "Подземелья на 20-40 игроков I-Z";
	AL["ATLAS_DDL_EXPANSION"] = "Дополнению";
	AL["ATLAS_DDL_EXPANSION_OLD_AO"] = "Подземелья Старого Мира A-O";
	AL["ATLAS_DDL_EXPANSION_OLD_PZ"] = "Подземелья Старого Мира P-Z";
	AL["ATLAS_DDL_EXPANSION_BC"] = "Подземелья Пылающего Крестового Похода";
	AL["ATLAS_DDL_EXPANSION_WOTLK"] = "Подземелья Wrath of the Lich King";
	AL["ATLAS_DDL_EXPANSION_CATA"] = "Подземелья Cataclysm";
	AL["ATLAS_DDL_EXPANSION_MOP"] = "Подземелья Mists of Pandaria";
	AL["ATLAS_DDL_TYPE"] = "Типу";
	AL["ATLAS_DDL_TYPE_INSTANCE_AB"] = "Подземелья A-B";
	AL["ATLAS_DDL_TYPE_INSTANCE_CF"] = "Подземелья C-F";
	AL["ATLAS_DDL_TYPE_INSTANCE_GM"] = "Подземелья G-M";
	AL["ATLAS_DDL_TYPE_INSTANCE_NS"] = "Подземелья N-S";
	AL["ATLAS_DDL_TYPE_INSTANCE_TZ"] = "Подземелья T-Z";
	AL["ATLAS_DDL_TYPE_ENTRANCE"] = "Входы";

	AL["ATLAS_INSTANCE_BUTTON"] = "Подземелье";
	AL["ATLAS_ENTRANCE_BUTTON"] = "Вход";
	AL["ATLAS_SEARCH_UNAVAIL"] = "Поиск недоступен";

	AL["ATLAS_DEP_MSG1"] = "Атлас обнаружил устаревший(е) модуль(и).";
	AL["ATLAS_DEP_MSG2"] = "Они будут отключены для данного персонажа.";
	AL["ATLAS_DEP_MSG3"] = "Удалите их из вашей папки аддонов.";
	AL["ATLAS_DEP_OK"] = "Ok";

	AL["ATLAS_INFO"] = "Atlas информация";
	AL["ATLAS_INFO_12200"] = "Важное уведомление:\n\nВ связи с проблемой увеличение размеров файлов модификации, мы разделили\n модификацию на отдельные модули подземелий.\n\nПользователи, которые скачивают нашу модификацию с известных веб-сайтов,\n в основном получают только основное ядро, которое включает в себя все\n функции ядра Атласа и карты подземелий Cataclysm'а.\n\nПользователи, которые хотят загрузить все старые карты подземелий и все\n модули Атласа, сделанные нами, могут их скачать по отдельности.\n\nДля получения дополнительной информации прочтите следующий топик:\n|cff6666ffhttp://www.atlasmod.com/phpBB3/viewtopic.php?t=1522|cffffffff\n\nИли посетите наш сайт, чтобы узнать где скачать:\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";
	AL["ATLAS_INFO_12201"] = "Обратите внимание, что мы создали новый плагин - |cff6666ffAtlas Сценарии|cffffffff, который \nобеспечивает совершенно новыми картыми сценарий, введенных в 5.0. \n\nДля более подробной информации посетите наш веб-сайт, и не забудьте скачать / \nустановить его отдельно.\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";

	AL["ATLAS_MISSING_MODULE"] = "Atlas обнаружил недостоющие модули / плагины: ";

--************************************************
-- Zone Names, Acronyms, and Common Strings
--************************************************

	--Common strings
	AL["East"] = "Восток";
	AL["North"] = "Север";
	AL["South"] = "Юг";
	AL["West"] = "Запад";

	--World Events, Festival
	AL["Brewfest"] = "Праздника пива";
	AL["Hallow's End"] = "Тыква";
	AL["Love is in the Air"] = "Любовная лихорадка";
	AL["Lunar Festival"] = "Лунный фестиваль";
	AL["Midsummer Festival"] = "Огненный солнцеворот";

	--Misc strings
	AL["Colon"] = ": "; -- The colon symbol to be used in string, ex: "Zone: Firelands
	AL["Adult"] = "Взрослый";
	AL["AKA"] = "AKA";
	AL["Arcane Container"] = "Волшебный контейнер";
	AL["Arms Warrior"] = "Воин-Оружия";
	AL["Attunement Required"] = "Необходима подготавка";
	AL["Back"] = "Назад";
	AL["Basement"] = "Подвал";
	AL["Blacksmithing Plans"] = "Чертежи кузнечного дела";
	AL["Chase Begins"] = "Начало охоты";
	AL["Chase Ends"] = "Конец охоты";
	AL["Child"] = "Ребенок";
	AL["Click to open Dungeon Journal window."] = "[ЛКМ] - открывает окно журнала подземелий.";
	AL["Connection"] = "Связан";
	AL["Elevator"] = "Лифт";
	AL["End"] = "Конец";
	AL["Engineer"] = "Инженер";
	AL["Entrance"] = "Вход";
	AL["Event"] = "Событие";
	AL["Exalted"] = "Превознесение";
	AL["Exit"] = "Выход";
	AL["Fourth Stop"] = "Четвертая остановка";
	AL["Front"] = "Передний";
	AL["Ghost"] = "Призрак";
	AL["Graveyard"] = "Кладбище";
	AL["Heroic"] = "Героический";
	AL["Holy Paladin"] = "Паладин-Света";
	AL["Holy Priest"] = "Жрец-Света";
	AL["Hunter"] = "Охотник";
	AL["Imp"] = "Бесс";
	AL["Key"] = "Ключ";
	AL["Lower"] = "Нижний";
	AL["Mage"] = "Маг";
	AL["Meeting Stone"] = "Камень встреч";
	AL["Middle"] = "Центр"; --???
	AL["Monk"] = "Монах";
	AL["Moonwell"] = "Лунный колодец";
	AL["Optional"] = "Необяз.";
	AL["Orange"] = "Оранжевый";
	AL["Outside"] = "Снаружи";
	AL["Paladin"] = "Паладин";
	AL["Portal"] = "Портал";
	AL["Priest"] = "Жрец";
	AL["Protection Warrior"] = "Воин-Защиты";
	AL["Purple"] = "Пурпурный";
	AL["Random"] = "Случайный";
	AL["Rare"] = "Редкий";
	AL["Reputation"] = "Репа";
	AL["Repair"] = "Починка";
	AL["Retribution Paladin"] = "Паладин-Возмездия";
	AL["Rewards"] = "Награды";
	AL["Rogue"] = "Разбойник";
	AL["Second Stop"] = "Вторая остановка";
	AL["Shadow Priest"] = "Жрец-Темной магии";
	AL["Shaman"] = "Шаман";
	AL["Spawn Point"] = "Точка рождения";
	AL["Start"] = "Начало";
	AL["Summon"] = "Вызов";
	AL["Teleporter"] = "Телепорт";
	AL["Teleporter destination"] = "Назначение телепорта";
	AL["Third Stop"] = "Третья остановка";
	AL["Top"] = "Верхний";
	AL["Tunnel"] = "Туннель";
	AL["Underwater"] = "Подводный";
	AL["Upper"] = "Верхний";
	AL["Varies"] = "Изменяется";
	AL["Wanders"] = "Странник";
	AL["Warlock"] = "Чернокнижник";
	AL["Warrior"] = "Воин";
	AL["Wave 5"] = "5-ая волна";
	AL["Wave 6"] = "6-ая волна";
	AL["Wave 10"] = "10-ая волна";
	AL["Wave 12"] = "12-ая волна";
	AL["Wave 18"] = "18-ая волна";
	AL["MapsNotFound"] = "Текущее выбранное подземелье не имеет \nсоответствующего изображения карты. \n\nПожалуйста, убедитесь, что вы установили \nсоответствующий модуль(и) карт Atlas'а.";

	--Classic Acronyms
	AL["AQ"] = "АКУ"; -- Ан'Кираж
	AL["AQ20"] = "АКУ20"; -- Руины Ан'Киража
	AL["AQ40"] = "АКУ40"; -- Храм Ан'Киража
	AL["BFD"] = "НП"; -- Непроглядная Пучина
	AL["BRD"] = "ГЧГ"; -- Глубины Черной горы
	AL["BRM"] = "ЧГ"; -- Черная гора
	AL["BWL"] = "ЛКТ"; -- Логово Крыла Тьмы
	AL["DM"] = "ЗГ"; -- Забытый Город
	AL["Gnome"] = "Гном"; -- Гномреган
	AL["Halls"] = "ЗАо"; -- Кладбище
	AL["LBRS"] = "НЧГ"; -- Нижняя часть Вершины Черной горы
	AL["Mara"] = "Маро"; -- Мародон
	AL["MC"] = "ОН"; -- Огненные Недра
	AL["RFC"] = "ОгП"; -- Огненная пропасть
	AL["RFD"] = "Курганы"; -- Курганы Иглошкурых
	AL["RFK"] = "ЛабИ"; -- Лабиринты Иглошкурых
	AL["Scholo"] = "Некро"; -- Некроситет
	AL["SFK"] = "КТК"; -- Крепость Темного Клыка
	AL["SM"] = "МАО"; -- Монастырь Алого ордена
	AL["ST"] = "ЗХ"; -- Затонувший храм
	AL["Strat"] = "Страт"; -- Стратхольм
	AL["Stocks"] = "Тюрьма"; -- Тюрьма
	AL["UBRS"] = "ВЧГ"; -- Вершина Черной горы
	AL["Ulda"] = "Ульд"; -- Ульдаман
	AL["VC"] = "МК"; -- Мертвые копи
	AL["WC"] = "ПС"; -- Пещеры Стенаний
	AL["ZF"] = "ЗФ"; -- Зул'Фаррак

	--BC Acronyms
	AL["AC"] = "АГ"; -- Аукенайские гробницы
	AL["Arca"] = "Арка"; -- Аркатрац
	AL["Auch"] = "Аук"; -- Аукиндон
	AL["BF"] = "КК"; -- Кузня Крови
	AL["BT"] = "ЧХ"; -- Черный Храм
	AL["Bota"] = "Бота"; -- Ботаника
	AL["CoT"] = "ПВ"; -- Пещеры Времени
	AL["CoT1"] = "ПВ1"; -- Старые предгорья Хилсбрада
	AL["CoT2"] = "ПВ2"; -- Черные топи
	AL["CoT3"] = "ПВ3"; -- Вершина Хиджала
	AL["CR"] = "РКК"; -- Резервуар Кривого Клыка
	AL["GL"] = "Груль"; -- Логово Груула
	AL["HC"] = "ЦАП"; -- Цитадель Адского Пламени
	AL["Kara"] = "Кара"; -- Каражан
	AL["MaT"] = "ТМ"; -- Терраса Магистров
	AL["Mag"] = "Маги"; -- Логово Магтеридона
	AL["Mech"] = "Мех"; -- Механар
	AL["MT"] = "ГМ"; -- Гробницы Маны
	AL["Ramp"] = "Баст"; -- Бастионы Адского Пламени
	AL["SSC"] = "ЗС"; -- Змеиное святилище
	AL["Seth"] = "Сетекк"; -- Сетеккские залы
	AL["SH"] = "РЗ"; -- Разрушенные залы
	AL["SL"] = "ТЛ"; -- Темный Лабиринт
	AL["SP"] = "Узи"; -- Узилище
	AL["SuP"] = "СК"; -- Солнечный Колодец
	AL["SV"] = "ПП"; -- Паровое Подземелье
	AL["TK"] = "КБ"; -- Крепость Бурь
	AL["UB"] = "НТ"; -- Нижетопь

	--WotLK Acronyms
	AL["AK, Kahet"] = "АК, Кахет"; -- Ан'кахет
	AL["AN, Nerub"] = "АЖ, Неруб"; -- Азжол-Неруб
	AL["Champ"] = "ИЧ"; -- Испытание чемпиона
	AL["CoT-Strat"] = "ПВ-Страт"; -- Очищение Стратхольма
	AL["Crus"] = "Crus"; -- Испытание крестоносца
	AL["DTK"] = "КДТ"; -- Крепость Драк'Тарон
	AL["FoS"] = "Кузня Душ"; 
	AL["FH1"] = "ЛЗ1"; -- Кузня Душ
	AL["Gun"] = "Гун"; -- Гундрак
	AL["HoL"] = "ЧМ"; -- Чертоги Молний
	AL["HoR"] = "ЗО"; 
	AL["FH3"] = "ЛЗ3"; -- Залы Отражения
	AL["HoS"] = "ЧК"; -- Чертоги Камня
	AL["IC"] = "ЦЛК"; -- Цитадель Ледяной Короны
	AL["Nax"] = "Накс"; -- Наксрамас
	AL["Nex, Nexus"] = "Некс, Нексус"; -- Нексус
	AL["Ocu"] = "Оку"; -- Окулус
	AL["Ony"] = "Ony"; -- Onyxia's Lair
	AL["OS"] = "OS"; -- The Obsidian Sanctum
	AL["PoS"] = "Яма"; 
	AL["FH2"] = "ЛЗ2"; -- Яма Сарона
	AL["RS"] = "PC"; -- Рубиновое святилище
	AL["TEoE"] = "ОВ"; -- Око Вечности
	AL["UK, Keep"] = "УК, Крепость"; -- Крепость Утгард
	AL["Uldu"] = "Ульда"; -- Ульдуар
	AL["UP, Pinn"] = "УВ, Вершина"; -- Вершина Утгард
	AL["VH"] = "АМК"; -- Аметистовая крепость
	AL["VoA"] = "Склеп"; -- Склеп Аркавона

	--Zones not included in LibBabble-Zone
	AL["Crusaders' Coliseum"] = "Колизей Авангарда"; 

	--Cataclysm Acronyms
	AL["BH"] = "КБ"; --Крепость Барадин
	AL["BoT"] = "СБ"; --Сумеречный бастион
	AL["BRC"] = "ПСГ"; --Пещеры Черной горы
	AL["BWD"] = "ТКТ"; --Твердыня Крыла Тьмы
	AL["CoT-DS"] = "ПВ-ДД"; --Caverns of Time: Dragon Soul
	AL["CoT-ET"] = "ПВ-КВ"; --Caverns of Time: End Time
	AL["CoT-HoT"] = "ПВ-ЧС"; --Caverns of Time: Hour of Twilight
	AL["CoT-WoE"] = "ПВ-ИВ"; --Caverns of Time: Well of Eternity
	AL["FL"] = "ОП"; --Firelands
	AL["GB"] = "ГБ"; --Грим Батол
	AL["HoO"] = "ЧТГС"; --Чертоги Созидания
	AL["LCoT"] = "ЗГТВ"; --Затерянный город Тол'вир
	AL["TSC"] = "КН"; --Каменные Недра
	AL["TWT"] = "ТЧВ"; --Трон Четырех Ветров
	AL["ToTT"] = "ТП"; --Трон Приливов
	AL["VP"] = "ВС"; --Вершина смерча
	AL["ZA"] = "ЗА"; -- Зул'Аман
	AL["ZG"] = "ЗГ"; --Зул'Гуруб

	--MoP Acronyms
	AL["GSS"] = "ВЗС"; --Врата Заходящего Солнца
	AL["HoF"] = "СС"; --Сердце Страха
	AL["MP"] = "ДМОГ"; --Дворец Могу'шан
	AL["MV"] = "ПМ"; --Подземелья Могу'шан
	AL["SPM"] = "МШадо"; --Монастырь Шадо-Пан
	AL["SNT"] = "ОХН"; --Осада храма Нюцзао
	AL["SB"] = "ХБП"; --Хмелеварня Буйных Портеров
	AL["SoO"] = "Оо"; --Осада Оргриммара
	AL["TJS"] = "ХНЗ"; --Храм Нефритовой Змеи
	AL["TES"] = "ТВВ"; --Терраса Вечной Весны
	AL["ToT"] = "ЗМ"; --Throne of Thunder

--************************************************
-- Instance Entrance Maps
--************************************************

	--Auchindoun (Entrance)
	AL["Clarissa"] = "Кларисса";
	AL["Greatfather Aldrimus"] = "Великий Отец Алдримус";
	AL["Ha'Lei"] = "Ха'лей";
	AL["Horvon the Armorer <Armorsmith>"] = "Хорвон Бронник <Бронник>";
	AL["Ramdor the Mad"] = "Рамдор Безумный";
	AL["Nexus-Prince Haramad"] = "Принц Харамад";
	AL["\"Slim\" <Shady Dealer>"] = "Тип <Сомнительный делец>";
	AL["\"Captain\" Kaftiz"] = "Капитан Кафтиц";
	AL["Dealer Tariq <Shady Dealer>"] = "Делец Тариг <Сомнительный делец>";
	AL["Provisioner Tsaalt"] = "Поставщик Тсаальт";

	--Blackfathom Deeps (Entrance)

	--Blackrock Mountain (Entrance)
	AL["Bodley"] = "Бодли";
	AL["Lothos Riftwaker"] = "Лотос Хранитель Портала";
	AL["Orb of Command"] = "Сфера Приказа";
	AL["Scarshield Quartermaster <Scarshield Legion>"] = "Интендант из легиона Изрубленного Щита";
	AL["The Behemoth"] = "Чудище";

	--Caverns of Time (Entrance)
	AL["Steward of Time <Keepers of Time>"] = "Распорядитель времени <Хранители Времени>";
	AL["Alexston Chrome <Tavern of Time>"] = "Алекстон Хром <Таверна Времени>";
	AL["Yarley <Armorer>"] = "Ярли <Бронник>";
	AL["Bortega <Reagents & Poison Supplies>"] = "Бортега <Реагенты и яды>";
	AL["Alurmi <Keepers of Time Quartermaster>"] = "Алурми <Начальник снабжения Хранителей Времени>";
	AL["Galgrom <Provisioner>"] = "Гальгром <Поставщик>";
	AL["Zaladormu"] = "Заладорму";
	AL["Soridormi <The Scale of Sands>"] = "Соридорми <Песчаная Чешуя>";
	AL["Arazmodu <The Scale of Sands>"] = "Аразмоду <Песчаная Чешуя>";
	AL["Andormu <Keepers of Time>"] = "Андорму <Хранители Времени>";
	AL["Nozari <Keepers of Time>"] = "Нозари <Хранители Времени>";
	AL["Anachronos <Keepers of Time>"] = "Анахронос <Хранители Времени>";

	--Caverns of Time: Hyjal (Entrance)
	AL["Indormi <Keeper of Ancient Gem Lore>"] = "Индорми <Хранитель знаний о древних самоцветах>";
	AL["Tydormu <Keeper of Lost Artifacts>"] = "Тайдорму <Хранитель утраченных артефактов>";

	--Coilfang Reservoir (Entrance)
	AL["Mortog Steamhead"] = "Мортог Горячая Голова";

	--Dire Maul (Entrance)
	AL["Dire Pool"] = "Забытый остров";
	AL["Dire Maul Arena"] = "Арена забытого города";
	AL["Elder Mistwalker"] = "Старейшина Странник Туманов ";

	--Gnomeregan (Entrance)
	AL["Torben Zapblast <Teleportation Specialist>"] = "Торбен Запрыгуль <Мастер телепортации>";

	--Hellfire Citadel (Entrance)
	AL["Steps and path to the Blood Furnace"] = "Подъем и путь к Кузне Крови";
	AL["Path to the Hellfire Ramparts and Shattered Halls"] = "Путь к Бастионам и Разрушенным залам";
	AL["Meeting Stone of Magtheridon's Lair"] = "Камень встреч Логова Магтеридона";
	AL["Meeting Stone of Hellfire Citadel"] = "Камень встреч Цитадели Адского Пламени";

	--Icecrown Citadel (Entrance)

	--Karazhan (Entrance)
	AL["Archmage Leryda"] = "Верховный маг Лерида";
	AL["Archmage Alturus"] = "Верховный маг Альтур";
	AL["Apprentice Darius"] = "Ученик Дариус";
	AL["Stairs to Underground Pond"] = "Лестница к Подземному пруду";
	AL["Stairs to Underground Well"] = "Лестница к Подземному колодцу";
	AL["Charred Bone Fragment"] = "Фрагмент обугленной кости";

	--Maraudon (Entrance)
	AL["The Nameless Prophet"] = "Безымянный пророк";
	AL["Cursed Centaur"] = "Проклятый кентавр";
	AL["Kherrah"] = "Керра";

	--Scarlet Monastery (Entrance)

	--The Deadmines (Entrance)

	--Sunken Temple (Entrance)
	AL["Priestess Udum'bra"] = "Жрица Удум'бра";
	AL["Gomora the Bloodletter"] = "Гомора Кровопускатель";
	AL["Captain Wyrmak"] = "Капитан Змеюк";

	--Uldaman (Entrance)

	--Ulduar (Entrance)
	AL["Shavalius the Fancy <Flight Master>"] = "Шавалий Модник <Распорядитель полетов>";
	AL["Chester Copperpot <General & Trade Supplies>"] = "Честер Медноковш <Потребительские и хозяйственные товары>";
	AL["Slosh <Food & Drink>"] = "Хлюп <Еда и напитки>";

	--Wailing Caverns (Entrance)

--************************************************
-- Kalimdor Instances (Classic)
--************************************************

	--Blackfathom Deeps
	AL["Shrine of Gelihast"] = "Святыня Гелихастa";
	AL["Fathom Stone"] = "Глубинный Камень";
	AL["Lorgalis Manuscript"] = "Манускрипт Лоргалиса";
	AL["Scout Thaelrid"] = "Страж Талрид";
	AL["Flaming Eradicator"] = "Пылающий искоренитель";
	AL["Altar of the Deeps"] = "Алтарь пучины";
	AL["Ashelan Northwood"] = "Эшлан Северный Лес";
	AL["Relwyn Shadestar"] = "Релвин Тень Звезды";
	AL["Sentinel Aluwyn"] = "Часовой Алувин";
	AL["Sentinel-trainee Issara"] = "Курсант-часовой Иссара";
	AL["Je'neu Sancrea <The Earthen Ring>"] = "Дже'неу Санкри <Служители Земли>";
	AL["Zeya"] = "Зейя";

	--Dire Maul (East)
	AL["\"Ambassador\" Dagg'thol"] = "\"Посланник\"Дагг'тол";
	AL["Furgus Warpwood"] = "Фургус Криводрев";
	AL["Old Ironbark"] = "Старик Железной Коры";
	AL["Ironbark the Redeemed"] = "Железная Кора - отмщенный";

	--Dire Maul (North)
	AL["Druid of the Talon"] = "Друид-ворон";
	AL["Stonemaul Ogre"] = "Огр из клана Каменного Молота";
	AL["Knot Thimblejack"] = "Уззл Наперстяк";

	--Dire Maul (West)
	AL["Ferra"] = "Ферра";
	AL["Estulan <The Highborne>"] = "Эстулан <Высокорожденный>";
	AL["Shen'dralar Watcher"] = "Шен'драларский дозорный";
	AL["Pylons"] = "Опоры";
	AL["Ancient Equine Spirit"] = "Дух древнего коня";
	AL["Shen'dralar Ancient"] = "Шен'драларский поставщик";
	AL["Falrin Treeshaper"] = "Фалрин Садовник";
	AL["Lorekeeper Lydros"] = "Сказитель Лидрос";
	AL["Lorekeeper Javon"] = " Сказитель Явон";
	AL["Lorekeeper Kildrath"] = "Сказитель Килдрат";
	AL["Lorekeeper Mykos"] = "Сказительница Микос";
	AL["Shen'dralar Provisioner"] = "Шен'драларский поставщик";

	--Maraudon	
	AL["Elder Splitrock"] = "Старейшина Камнепад";
	AL["Celebras the Redeemed"] = "Келебрас Освобожденный";

	--Ragefire Chasm
	AL["Commander Bagran"] = "Командир Багран";
	AL["Invoker Xorenth"] = "Заклинатель Ксорент";
	AL["Scout Cage"] = "Scout Cage"; --need check

	--Razorfen Downs
	AL["Koristrasza"] = "Користраза";
	AL["Belnistrasz"] = "Белнистраз";

	--Razorfen Kraul
	AL["Auld Stonespire"] = "Ольд Каменное Копье";
	AL["Spirit of Agamaggan <Ancient>"] = "Дух Агамаггана <Древний>";
	AL["Willix the Importer"] = "Импортер Вилликс";

	--Ruins of Ahn'Qiraj
	AL["Four Kaldorei Elites"] = "4 Кальдорайских гвардейцев";
	AL["Captain Qeez"] = "Капитан Квиз";
	AL["Captain Tuubid"] = "Капитан Туубид";
	AL["Captain Drenn"] = "Капитан Дренн";
	AL["Captain Xurrem"] = "Капитан Ксуррем";
	AL["Major Yeggeth"] = "Майор Йеггет";
	AL["Major Pakkon"] = "Майор Паккон";
	AL["Colonel Zerran"] = "Полковник Зерран";
	AL["Safe Room"] = "Безопасная Комната";

	--Temple of Ahn'Qiraj
	AL["Andorgos <Brood of Malygos>"] = "Андоргос <Род Малигоса>";
	AL["Vethsera <Brood of Ysera>"] = "Ветсера <Род Изеры >";
	AL["Kandrostrasz <Brood of Alexstrasza>"] = "Кандострас <Племя Алекстразы>";
	AL["Arygos"] = "Аригос";
	AL["Caelestrasz"] = "Келестраз";
	AL["Merithra of the Dream"] = "Меритра из Сна";

	--Wailing Caverns
	AL["Ebru <Disciple of Naralex>"] = "Эбру <Ученица Наралекса>"; -- 5768
	AL["Nalpak <Disciple of Naralex>"] = "Налпак <Ученик Наралекса>"; -- 5767
	AL["Muyoh <Disciple of Naralex>"] = "Муйон <Ученик Наралекса>";  -- 3678
	AL["Naralex"] = "Наралекс"; -- 3679

	--Zul'Farrak
	AL["Chief Engineer Bilgewhizzle <Gadgetzan Water Co.>"] = "Главный инженер Чепухастер <Компания \"Воды Прибамбасска\">";
	AL["Mazoga's Spirit"] = "Дух Мазоги";
	AL["Tran'rek"] = "Тран'рек";
	AL["Weegli Blastfuse"] = "Вигиль Фитиль";
	AL["Raven"] = "Ворон";
	AL["Elder Wildmane"] = "Старейшина Дикая Грива ";

--****************************
-- Eastern Kingdoms Instances (Classic)
--****************************

	--Blackrock Depths
	AL["The Black Anvil"] = "Черная наковальня";
	AL["The Vault"] = "Подземелье";
	AL["Watchman Doomgrip"] = "Сторож Хватка Смерти";
	AL["Elder Morndeep"] = "Старейшина Рассветень";
	AL["Schematic: Field Repair Bot 74A"] = "Схема: полевой ремонтный робот 74A";
	AL["Private Rocknot"] = "Рядовой Камнеузл";
	AL["Mistress Nagmara"] = "Госпожа Нагмара";
	AL["Jalinda Sprig <Morgan's Militia>"] = "Джалинда Тирлипунька";
	AL["Oralius <Morgan's Militia>"] = "Орелий";
	AL["Thal'trak Proudtusk <Kargath Expeditionary Force>"] = "Тал'трак Гордый Клык <Каргатский экспедиционный корпус>";
	AL["Galamav the Marksman <Kargath Expeditionary Force>"] = "Галамав Стрелок <Каргатский экспедиционный корпус>";
	AL["Maxwort Uberglint"] = "Максворт Суперблеск";
	AL["Tinkee Steamboil"] = "Тинки Кипеллер";
	AL["Yuka Screwspigot <Engineering Supplies>"] = "Юка Крутипроб";
	AL["Abandonded Mole Machine"] = "Брошенная буровая установка";
	AL["Kevin Dawson <Morgan's Militia>"] = "Кевин Доусон <Отряд Морганы>";
	AL["Lexlort <Kargath Expeditionary Force>"] = "Лекслорт <Каргатский экспедиционный корпус>";
	AL["Prospector Seymour <Morgan's Militia>"] = "Геолог Сеймур <Отряд Морганы>";
	AL["Razal'blade <Kargath Expeditionary Force>"] = "Разал'меч <Каргатский экспедиционный корпус>";
	AL["The Shadowforge Lock"] = "Замок Тенегорна";
	AL["Mayara Brightwing <Morgan's Militia>"] = "Майра Светлое Крыло <Отряд Морганы>";
	AL["Hierophant Theodora Mulvadania <Kargath Expeditionary Force>"] = "Верховная Жрица Теодора Мальвадания";
	AL["Lokhtos Darkbargainer <The Thorium Brotherhood>"] = "Локтос Зловещий Торговец";
	AL["Mountaineer Orfus <Morgan's Militia>"] = "Горный пехотинец Орфус <Отряд Морганы>";
	AL["Thunderheart <Kargath Expeditionary Force>"] = "Громосерд <Каргатский экспедиционный корпус>";
	AL["Marshal Maxwell <Morgan's Militia>"] = "Маршал Максвелл <Отряд Морганы>";
	AL["Warlord Goretooth <Kargath Expeditionary Force>"] = "Полководец Клинозуб <Каргатский экспедиционный корпус>";
	AL["The Black Forge"] = "Черная Кузня";
	AL["Core Fragment"] = "Осколок из Огненных Недр";
	AL["Shadowforge Brazier"] = "Жаровня Тенегорна";

	--Blackrock Spire (Lower)
	AL["Urok's Tribute Pile"] = "Груда приношений Арроку";
	AL["Acride <Scarshield Legion>"] = "Секретный агент <Легион Изрубленного Щита>";
	AL["Elder Stonefort"] = "Старейшина Камнеград";
	AL["Roughshod Pike"] = "Наконечник Грубой силы ";

	--Blackrock Spire (Upper)
	AL["Finkle Einhorn"] = "Айс Вентурон";
	AL["Drakkisath's Brand"] = "Клеймо Драккисата";
	AL["Father Flame"] = "Огонь отцов";

	--Blackwing Lair
	AL["Orb of Domination"] = "Сфера Приказа";
	AL["Master Elemental Shaper Krixix"] = "Ваятель стихий Криксикс";

	--Gnomeregan
	AL["Chomper"] = "Чавккер";
	AL["Blastmaster Emi Shortfuse"] = "Взрывник Ими Фитилюшка";
	AL["Murd Doc <S.A.F.E.>"] = "Мерд-Док <С.П.А.С.>";
	AL["Tink Sprocketwhistle <Engineering Supplies>"] = "Звяк Пружиносвист <Инженерные материалы>";
	AL["The Sparklematic 5200"] = "Чистер 5200!";
	AL["Mail Box"] = "Почтовый яшик";
	AL["B.E Barechus <S.A.F.E.>"] = "Б.Е. Барекус <С.П.А.С.>";
	AL["Face <S.A.F.E.>"] = "Физий <С.П.А.С.>";
	AL["Hann Ibal <S.A.F.E.>"] = "Ганни Бал <С.П.А.С.>";

	--Molten Core

	--Scarlet Halls
	AL["Commander Lindon"] = "Командир Линдон";
	AL["Hooded Crusader"] = "Рыцарь в капюшоне";
	AL["Bucket of Meaty Dog Food"] = "Ведро с собачьим кормом";
	AL["Reinforced Archery Target"] = "Укрепленная мишень";

	--Scarlet Monastery

	--Scholomance
	AL["Instructor Chillheart's Phylactery"] = "Филактерия инструктора Ледяное Сердце";
	AL["Professor Slate"] = "Профессор Слейт";
	AL["Polyformic Acid Potion"] = "Наука о кислоте";
	AL["Talking Skull"] = "Говорящий череп";
	AL["In the Shadow of the Light"] = "В тени света";
	AL["Kel'Thuzad's Deep Knowledge"] = "Сокровенное знание Кел'Тузада";
	AL["Forbidden Rites and other Rituals Necromantic"] = "Запретные обряды и иные ритуалы некромантов";
	AL["Coffer of Forgotten Souls"] = "Сундук забытых душ";
	AL["The Dark Grimoire"] = "Гримуар Тьмы";

	--Shadowfang Keep
	AL["Apothecary Trio"] = "Трио аптекарей"; --
	AL["Apothecary Hummel <Crown Chemical Co.>"] = "Аптекарь Хаммел <Королевская химическая компания>";
	AL["Apothecary Baxter <Crown Chemical Co.>"] = "Аптекарь Бакстер <Королевская химическая компания>";
	AL["Apothecary Frye <Crown Chemical Co.>"] = "Аптекарь Фрай <Королевская химическая компания>";
	AL["Packleader Ivar Bloodfang"] = "Вожак стаи Ивар Кровавый Клык";
	AL["Deathstalker Commander Belmont"] = "Командир стражей смерти Бельмонт";
	AL["Haunted Stable Hand"] = "Дух помощника смотрителя стойл";
	AL["Investigator Fezzen Brasstacks"] = "Испытатель Феззен Клейстр";

	--Stratholme - Crusader's Square
	AL["Crusade Commander Eligor Dawnbringer <Brotherhood of the Light>"] = "Командир Элигор Вестник Рассвета <Братство Света>";
	AL["Master Craftsman Wilhelm <Brotherhood of the Light>"] = "Мастер-ремесленник Вильгельм <Братство Света>";
	AL["Packmaster Stonebruiser <Brotherhood of the Light>"] = "Караванщик Камнетес <Братство Света>";
	AL["Stratholme Courier"] = "Стратхольмский курьер";
	AL["Fras Siabi's Postbox"] = "Ключ от почтового ящика Фраса Сиаби";
	AL["King's Square Postbox"] = "Ключ от почтового ящика на Королевской площали";
	AL["Festival Lane Postbox"] = "Ключ от почтового ящика на Праздничной улице";
	AL["Elder Farwhisper"] = "Старейшина Тихий Шепот";
	AL["Market Row Postbox"] = "Ключ от почтового ящика в торговом ряду";

	--Stratholme - The Gauntlet
	AL["Elders' Square Postbox"] = "Ключ от почтового ящика на Площади старейшины";
	AL["Archmage Angela Dosantos <Brotherhood of the Light>"] = "Верховный маг Анджела Досантос <Братство Света>";
	AL["Crusade Commander Korfax <Brotherhood of the Light>"] = "Командир рыцарей Корфакс <Братство Света>";

	--The Deadmines
	AL["Lieutenant Horatio Laine"] = "Лейтенант Горацио Лейн";
	AL["Kagtha"] = "Кагта";
	AL["Slinky Sharpshiv"] = "Крадли Дротик";
	AL["Quartermaster Lewis <Quartermaster>"] = "Интендант Льюис <Интендант>";
	AL["Miss Mayhem"] = "Мисс Кавардак";
	AL["Vend-O-Tron D-Luxe"] = "Торг-о-трон делюкс";

	--The Stockade
	AL["Rifle Commander Coe"] = "Командир стрелков Коу";
	AL["Warden Thelwater"] = "Тюремщик Телвотер";
	AL["Nurse Lillian"] = "Медсестра Лилиан";

	--The Sunken Temple
	AL["Lord Itharius"] = "Лорд Итар";
	AL["Elder Starsong"] = "Старейшина Звездная Песня";

	--Uldaman
	AL["Baelog's Chest"] = "Сундук Бейлога";
	AL["Kand Sandseeker <Explorer's League>"] = "Канд Искатель Песков <Лига исследователей>";
	AL["Lead Prospector Durdin <Explorer's League>"] = "Старший геолог Дардин <Лига исследователей>";
	AL["Olga Runesworn <Explorer's League>"] = "Ольга Преданная Рунам <Лига исследователей>";
	AL["Aoren Sunglow <The Reliquary>"] = "Аорен Солнечное Сияние <Реликварий>";
	AL["High Examiner Tae'thelan Bloodwatcher <The Reliquary>"] = "Главный дознаватель Тей'телан Кровавый Взор <Реликварий>";
	AL["Lidia Sunglow <The Reliquary>"] = "Лидия Солнечное Сияние <Реликварий>";
	AL["Ancient Treasure"] = "Древнее сокровище";
	AL["The Discs of Norgannon"] = "Диски Норганнона";

--*******************
-- Burning Crusade Instances
--*******************

	--Auch: Auchenai Crypts
	AL["Draenei Spirit"] = "Дух дренея";
	AL["Avatar of the Martyred"] = "Аватара Мученика";
	AL["D'ore"] = "Д'оре";
	AL["Tormented Soulpriest"] = "Измученный жрец душ";

	--Auch: Mana-Tombs
	AL["Artificer Morphalius"] = "Работник Морфалиус";
	AL["Mamdy the \"Ologist\""] = "Мамди по кличке \"Олог\"";
	AL["Shadow Lord Xiraxis"] = "Владыка теней Зираксис";
	AL["Ambassador Pax'ivi"] = "Посол Пакс'иви";
	AL["Cryo-Engineer Sha'heen"] = "Крио-инженер Ша'хин";
	AL["Ethereal Transporter Control Panel"] = "Пульт управления астрального телепорта";

	--Auch: Sethekk Halls
	AL["Isfar"] = "Исфар";
	AL["Dealer Vijaad"] = "Делец Виджад";
	AL["Lakka"] = "Лакка";
	AL["The Saga of Terokk"] = "Сага о Терокке";

	--Auch: Shadow Labyrinth
	AL["Field Commander Mahfuun"] = "Боевой командир Мафуун";
	AL["Spy Grik'tha"] = "Шпион Грик'та";
	AL["The Codex of Blood"] = "Кодекс Крови";
	AL["First Fragment Guardian"] = "Страж первого фрагмента";
	AL["Spy To'gun"] = "Шпион То'гун";

	--Black Temple (Start)
	AL["Towards Reliquary of Souls"] = "К Гробнице Душ";
	AL["Towards Teron Gorefiend"] = "К Терону Кровожадному";
	AL["Towards Illidan Stormrage"] = "К Иллидану Ярости Бури";
	AL["Spirit of Olum"] = "Олумов дух";
	AL["Spirit of Udalo"] = "Дух Адало";
	AL["Aluyen <Reagents>"] = "Алуйен <Реагенты>";
	AL["Okuno <Ashtongue Deathsworn Quartermaster>"] = "Окуно <Начальник снабжения Пеплоустов>";
	AL["Seer Kanai"] = "Провидец Канеи";

	--Black Temple (Basement)

	--Black Temple (Top)

	--CFR: Serpentshrine Cavern
	AL["Seer Olum"] = "Провидец Олум";

	--CFR: The Slave Pens
	AL["Nahuud"] = "Нахууд";
	AL["Watcher Jhang"] = "Дозорный Джанг";
	AL["Weeder Greenthumb"] = "Культиватор Зеленопал";
	AL["Skar'this the Heretic"] = "Скартис Еретик";
	AL["Naturalist Bite"] = "Натуралист Кус";

	--CFR: The Steamvault
	AL["Windcaller Claw"] = "Призыватель ветров Коготь";
	AL["Main Chambers Access Panel"] = "Главная камера сгорания - Панель доступа";
	AL["Second Fragment Guardian"] = "Страж второго фрагмента";

	--CFR: The Underbog
	AL["T'shu"] = "Тшу";
	AL["The Underspore"] = "Подспорник";
	AL["Earthbinder Rayge"] = "Землепряд Гневвс";

	--CoT: The Black Morass
	AL["Sa'at <Keepers of Time>"] = "Са'ат <Хранители Времени>";

	--CoT: Hyjal Summit
	AL["Lady Jaina Proudmoore"] = "Леди Джайна Праудмур";
	AL["Thrall <Warchief>"] = "Тралл <Вождь>";
	AL["Tyrande Whisperwind <High Priestess of Elune>"] = "Тиранда Шелест Ветра";

	--CoT: Old Hillsbrad Foothills
	AL["Erozion"] = "Эрозион";
	AL["Brazen"] = "Бронзень";
	AL["Landing Spot"] = "Место высадки";
	AL["Thrall"] = "Раб";
	AL["Taretha"] = "Тарета";
	AL["Don Carlos"] = "Дон Карлос";
	AL["Guerrero"] = "Герреро";
	AL["Thomas Yance <Travelling Salesman>"] = "Томас Янс <Странствующий торговец>";
	AL["Aged Dalaran Wizard"] = "Даларанский старый волшебник";
	AL["Jonathan Revah"] = "Джонатан Рева";
	AL["Jerry Carter"] = "Джерри Картер";
	AL["Helcular"] = "Гелькулар";
	AL["Farmer Kent"] = "Фермер Кент";
	AL["Sally Whitemane"] = "Сэлли Белогрив";
	AL["Renault Mograine"] = "Рено Могрейн";
	AL["Little Jimmy Vishas"] = "Малыш Джимми Вишас";
	AL["Herod the Bully"] = "Герод Забияка";
	AL["Nat Pagle"] = "Нат Пэгл";
	AL["Hal McAllister"] = "Хал Макаллистер";
	AL["Zixil <Aspiring Merchant>"] = "Зиксель <Знаменитый купец>";
	AL["Overwatch Mark 0 <Protector>"] = "Суперсторож, модель 0 <Заступник>";
	AL["Southshore Inn"] = "Дома южнобережья";
	AL["Captain Edward Hanes"] = "Капитан Эдвард Хейнс";
	AL["Captain Sanders"] = "Капитан Сандерс";
	AL["Commander Mograine"] = "Командир Могрейн";
	AL["Isillien"] = "Изиллиен";
	AL["Abbendis"] = "Аббендис";
	AL["Fairbanks"] = "Фэйрбанкс";
	AL["Taelan"] = "Таэлан";
	AL["Barkeep Kelly <Bartender>"] = "Кабатчик Келли <Бармен>";
	AL["Frances Lin <Barmaid>"] = "Франс Лин <Официантка>";
	AL["Chef Jessen <Speciality Meat & Slop>"] = "Шеф-повар Джессен <Деликатесное мясо и похлебки>";
	AL["Stalvan Mistmantle"] = "Сталван Мистмантл";
	AL["Phin Odelic <The Kirin Tor>"] = "Фин Оделик <Кирин Тор>";
	AL["Magistrate Henry Maleb"] = "Мировой судья Генри Малеб";
	AL["Raleigh the True"] = "Роли Истинный";
	AL["Nathanos Marris"] = "Натанос Маррис";
	AL["Bilger the Straight-laced"] = "Бочкопуз Крепкосбитый";
	AL["Innkeeper Monica"] = "Хозяйка таверны Моника";
	AL["Julie Honeywell"] = "Джули Медовушка";
	AL["Jay Lemieux"] = "Джей Лемье";
	AL["Young Blanchy"] = "Молодая Савраска";

	--Gruul's Lair

	--HFC: The Blood Furnace
	AL["Gunny"] = "Пушкаренок";
	AL["Caza'rez"] = "Каса'рес";

	--HFC: Hellfire Ramparts
	AL["Advance Scout Chadwick"] = "Главный разведчик Чадвик";
	AL["Stone Guard Stok'ton"] = "Каменный страж Сток'тон";
	AL["Reinforced Fel Iron Chest"] = "Укрепленный сундук из оскверненного железа";

	--HFC: Magtheridon's Lair

	--HFC: The Shattered Halls
	AL["Shattered Hand Executioner"] = "Палач из клана Изувеченной Длани";
	AL["Private Jacint"] = "Рядовой Джасинт";
	AL["Rifleman Brownbeard"] = "Ружейник Буробород";
	AL["Captain Alina"] = "Капитан Алина";
	AL["Scout Orgarr"] = "Разведчик Оргарр";
	AL["Korag Proudmane"] = "Кораг Гордая Грива";
	AL["Captain Boneshatter"] = "Капитан Костолом";
	AL["Randy Whizzlesprocket"] = "Рэнди Свистельник";
	AL["Drisella"] = "Дризелла";

	--Karazhan Start
	AL["Baroness Dorothea Millstipe"] = "Баронесса Дороти Милстип";
	AL["Lady Catriona Von'Indi"] = "Леди Катриона Фон'Инди";
	AL["Lady Keira Berrybuck"] = "Леди Кейра Ягодная Корзина";
	AL["Baron Rafe Dreuger"] = "Барон Раф Дреугер";
	AL["Lord Robin Daris"] = "Лорд Робин Дэрис";
	AL["Lord Crispin Ference"] = "Лорд Криспин Ференс";
	AL["Red Riding Hood"] = "Красная Шапочка";
	AL["Wizard of Oz"] = "Волшебник страны Oз";
	AL["The Master's Terrace"] = "Терраса Мастера";
	AL["Servant Quarters"] = "Жильё прислуги";
	AL["Hastings <The Caretaker>"] = "Гастингс <Управляющий>";
	AL["Berthold <The Doorman>"] = "Бертольд <Привратник>";
	AL["Calliard <The Nightman>"] = "Кальярд <Ночной страж>";
	AL["Koren <The Blacksmith>"] = "Корен <Кузнец>";
	AL["Bennett <The Sergeant at Arms>"] = "Беннет <Начальник охраны>";
	AL["Keanna's Log"] = "Записи Кеанны";
	AL["Ebonlocke <The Noble>"] = "Чернодрев <Аристократ>";
	AL["Sebastian <The Organist>"] = "Себастиан <Органист>";
	AL["Barnes <The Stage Manager>"] = "Барнс <Конферансье>";

	--Karazhan End
	AL["Path to the Broken Stairs"] = "Путь к разрушенным лестницам";
	AL["Broken Stairs"] = "Сломаная лесница";
	AL["Ramp to Guardian's Library"] = "Рампа к библиотеку стражи";
	AL["Mysterious Bookshelf"] = "Подозрительные книжные полки";
	AL["Ramp up to the Celestial Watch"] = "Подъём к небесному надзору";
	AL["Ramp down to the Gamesman's Hall"] = "Спуск в игровой зал";
	AL["Ramp to Medivh's Chamber"] = "Рампа в комнату Медива";
	AL["Spiral Stairs to Netherspace"] = "Спиральные лестницы к Пустомари";
	AL["Wravien <The Mage>"] = "Вравьен <Маг>";
	AL["Gradav <The Warlock>"] = "Градав <Чернокнижник>";
	AL["Kamsis <The Conjurer>"] = "Камсис <Кудесник>";
	AL["Ythyar"] = "Айтар";
	AL["Echo of Medivh"] = "Эхо Медива";

	--Magisters Terrace
	AL["Exarch Larethor"] = "Экзарх Ларетор";
	AL["Fel Crystals"] = "Кристалл Скверны";
	AL["Apoko"] = "Апоко";
	AL["Eramas Brightblaze"] = "Эрамас Сияющее Пламя";
	AL["Ellrys Duskhallow"] = "Эллриса Почитательница Тени";
	AL["Fizzle"] = "Пшикс";
	AL["Garaxxas"] = "Гараксас";
	AL["Sliver <Garaxxas' Pet>"] = "Лыббс <Питомец Гараксаса>";
	AL["Kagani Nightstrike"] = "Кагани Ночной Удар";
	AL["Warlord Salaris"] = "Полководец Саларис";
	AL["Yazzai"] = "Яззай";
	AL["Zelfan"] = "Зелфан";
	AL["Tyrith"] = "Тирит";
	AL["Scrying Orb"] = "Гадательный шар Соланиана";

	--Sunwell Plateau
	AL["Madrigosa"] = "Мадригоса";

	--TK: The Arcatraz
	AL["Millhouse Manastorm"] = "Милхаус Манашторм";
	AL["Third Fragment Guardian"] = "Страж третьего фрагмента";
	AL["Udalo"] = "Адало";

	--TK: The Botanica

	--TK: The Mechanar
	AL["Overcharged Manacell"] = "Переполненный зарядом контейнер с маной";

	--TK: The Eye

--*****************
-- Wrath of the Lich King Instances
--*****************

	--Azjol-Nerub: Ahn'kahet: The Old Kingdom
	AL["Seer Ixit"] = "Провидец Изит";
	AL["Ahn'kahet Brazier"] = "Ан'кахетская жаровня";

	--Azjol-Nerub: Azjol-Nerub
	AL["Reclaimer A'zak"] = "Завоеватель А'зак";
	AL["Watcher Gashra"] = "Дозорный Гашра";
	AL["Watcher Narjil"] = "Дозорный Нарджил";
	AL["Watcher Silthik"] = "Дозорный Силтик";
	AL["Elder Nurgen"] = "Предок Нурген";

	--Caverns of Time: The Culling of Stratholme
	AL["The Culling of Stratholme"] = "Очищение Стратхольма";
	AL["Scourge Invasion Points"] = "Точки вторжения Плети";
	AL["Guardian of Time"] = "Хранитель Времени";
	AL["Chromie"] = "Хроми";

	--Drak'Tharon Keep
	AL["Image of Drakuru"] = "Проекция Дракуру";
	AL["Kurzel"] = "Курцель";
	AL["Elder Kilias"] = "Предок Килиас";
	AL["Drakuru's Brazier"] = "Жаровня Дракуру";

	--The Frozen Halls: Halls of Reflection
	--3 beginning NPCs omitted, see The Forge of Souls
	AL["The Captain's Chest"] = "Сундук капитана";

	--The Frozen Halls: Pit of Saron
	--6 beginning NPCs omitted, see The Forge of Souls
	AL["Martin Victus"] = "Мартин Викт";
	AL["Gorkun Ironskull"] = "Горкун Железный Череп";
	AL["Rimefang"] = "Иний";

	--The Frozen Halls: The Forge of Souls
	--Lady Jaina Proudmoore omitted, in Hyjal Summit
	AL["Archmage Koreln <Kirin Tor>"] = "Верховный маг Корелн <Кирин-Тор>";
	AL["Archmage Elandra <Kirin Tor>"] = "Верховный маг Эландра <Кирин-Тор>";
	AL["Lady Sylvanas Windrunner <Banshee Queen>"] = "Леди Сильвана Ветрокрылая";
	AL["Dark Ranger Loralen"] = "Темный следопыт Лорален";
	AL["Dark Ranger Kalira"] = "Темный следопыт Калира";

	--Gundrak
	AL["Chronicler Bah'Kini"] = "Летописец Ба'кини";
	AL["Tol'mar"] = "Тол'мар";
	AL["Elder Ohanzee"] = "Предок Оханзи";

	--Icecrown Citadel
	AL["To next map"] = "На следеющею карту";
	AL["From previous map"] = "На предыдущую карту";
	AL["Upper Spire"] = "Верхний ярус";
	AL["Sindragosa's Lair"] = "Логово Синдрагосы";
	AL["Stinky"] = "Вонючка";
	AL["Precious"] = "Прелесть";
	AL["Rimefang"] = "Иней";	-- NPC: 37533
	AL["Spinestalker"] = "Хребтохруст";	-- NPC: 37534
	AL["Sister Svalna"] = "Сестра Свална";	-- NPC: 37126

	--Naxxramas
	AL["Mr. Bigglesworth"] = "Мистер Бигглсуорт";
	AL["Frostwyrm Lair"] = "Логово Ледяного Змея";
	AL["Teleporter to Middle"] = "Телепорт в центр";

	--The Obsidian Sanctum
	AL["Black Dragonflight Chamber"] = "Комната черных драконов";

	--Onyxia's Lair

	--The Ruby Sanctum
	AL["Red Dragonflight Chamber"] = "Комната красных драконов";

	--The Nexus: The Eye of Eternity

	--The Nexus: The Nexus
	AL["Warmage Kaitlyn"] = "Боевой маг Кейтлин";
	AL["Berinand's Research"] = "Исследования Беринарда";
	AL["Elder Igasho"] = "Предок Игашо";

	--The Nexus: The Oculus
	AL["Belgaristrasz"] = "Белгаристраз";
	AL["Eternos"] = "Этернос";
	AL["Verdisa"] = "Вердиса";
	AL["Centrifuge Construct"] = "Центрифужное создание";
	AL["Cache of Eregos"] = "Тайник Эрегоса";

	--Trial of the Champion
	AL["Marshal Jacob Alerius"] = "Маршал Якоб Алерий";
	AL["Ambrose Boltspark"] = "Амброз Искрокрут";
	AL["Colosos"] = "Колосус";
	AL["Jaelyne Evensong"] = "Джейлин Закатная Песня";
	AL["Lana Stouthammer"] = "Лана Твердомолот";

	--Trial of the Crusader
	AL["Heroic: Trial of the Grand Crusader"] = "Героик: Испытание великого крестоносца";
	AL["Cavern Entrance"] = "Вход";

	--Ulduar General
	AL["The Siege"] = "Осада"; --A
	AL["The Keepers"] = "Хранители"; --C

	--Ulduar A
	AL["Tower of Life"] = "Башня Жизни";
	AL["Tower of Flame"] = "Башня Пламени";
	AL["Tower of Frost"] = "Башня Холода";
	AL["Tower of Storms"] = "Башня Гроз";

	--Ulduar B
	AL["Prospector Doren"] = "Геолог Дорен";
	AL["Archivum Console"] = "Панель управления Архивом";

	--Ulduar C
	AL["Sif"] = "Сиф";

	--Ulduar D

	--Ulduar E

	--Ulduar: Halls of Lightning
	AL["Stormherald Eljrrin"] = "Штормовестник Элдррин";

	--Ulduar: Halls of Stone
	AL["Kaldir Ironbane"] = "Калдир Железоруб";
	AL["Tribunal Chest"] = "Сундук Трибунала";
	AL["Elder Yurauk"] = "Предок Яруак";
	AL["Brann Bronzebeard"] = "Бранн Бронзобород";

	--Utgarde Keep: Utgarde Keep
	AL["Defender Mordun"] = "Защитник Мордун";
	AL["Dark Ranger Marrah"] = "Темный следопыт Марра";
	AL["Elder Jarten"] = "Предок Яртен";

	--Utgarde Keep: Utgarde Pinnacle
	AL["Brigg Smallshanks"] = "Бригг Мелкотруб";
	AL["Image of Argent Confessor Paletress"] = "Проекция исповедницы Серебряного Авангарда Пейлтресс";
	AL["Elder Chogan'gada"] = "Предок Чоган'гада";

	--Vault of Archavon

	--The Violet Hold
	AL["Lieutenant Sinclari"] = "Лейтенант Синклари";

--*********************
-- Cataclysm Instances
--*********************

	--Baradin Hold

	--Blackrock Caverns

	--Blackwing Descent

	--Caverns of Time: Dragon Soul
	AL["Dasnurimi <Geologist & Conservator>"] = "Даснурими <Геолог>";
	AL["Lord Afrasastrasz"] = "Лорд Афрасастраз";

	--Caverns of Time: End Time
	AL["Alurmi"] = "Алурми";
	AL["Nozdormu"] = "Ноздорму";

	--Caverns of Time: Hour of Twilight

	--Caverns of Time: Well of Eternity

	--Firelands
	AL["Lurah Wrathvine <Crystallized Firestone Collector>"] = "Лура Гневная Лоза <Собиратель кристаллизованного кремня>";
	AL["Naresir Stormfury <Avengers of Hyjal Quartermaster>"] = "Наресир Штормовая Ярость <Интендант Хиджальских мстителей>";

	--Grim Batol
	AL["Baleflame"] = "Пламегон";
	AL["Farseer Tooranu <The Earthen Ring>"] = "Предсказатель Тоорану <Служители Земли>";
	AL["Velastrasza"] = "Веластраза";

	--Halls of Origination
	AL["Large Stone Obelisk"] = "Большой каменный обелиск";

	--Lost City of the Tol'vir
	AL["Captain Hadan"] = "Капитан Хадан";
	AL["Tol'vir Grave"] = "Тол'вирская могила";

	--The Bastion of Twilight

	--The Stonecore
	AL["Earthwarden Yrsa <The Earthen Ring>"] = "Хранитель земли Изра <Служители Земли>";

	--The Vortex Pinnacle
	AL["Itesh"] = "Итеш";
	AL["Magical Brazier"] = "Магическая жаровня";

	--Throne of the Four Winds

	--Throne of the Tides
	AL["Captain Taylor"] = "Капитан Тейлор";
	AL["Legionnaire Nazgrim"] = "Легионер Назгрим";
	AL["Neptulon"] = "Нептулон";

	--Zul'Aman
	AL["Vol'jin"] = "Вол'джин";
	AL["Witch Doctor T'wansi"] = "Знахарь Т'ванши";
	AL["Blood Guard Hakkuz <Darkspear Elite>"] = "Кровавый страж Хаккуз <Элита племени Черного Копья>";
	AL["Voodoo Pile"] = "Куча черепов вуду";
	AL["Bakkalzu"] = "Баккальзу";
	AL["Hazlek"] = "Хазлек";
	AL["The Map of Zul'Aman"] = "Карта Зул'Амана";
	AL["Norkani"] = "Норкани";
	AL["Kasha"] = "Кайша";
	AL["Thurg"] = "Тург";
	AL["Gazakroth"] = "Газакрот";
	AL["Lord Raadan"] = "Лорд Раадан";
	AL["Darkheart"] = "Черносерд";
	AL["Alyson Antille"] = "Алисон Антиль";
	AL["Slither"] = "Скользь";
	AL["Fenstalker"] = "Болотный ловец";
	AL["Koragg"] = "Корагг";
	AL["Zungam"] = "Зангам";
	AL["Forest Frogs"] = "Лесная лягушка";
	AL["Eulinda <Reagents>"] = "Эулинда <Реагенты>";
	AL["Harald <Food Vendor>"] = "Гаральд <Продавец еды>";
	AL["Arinoth"] = "Аринот";
	AL["Kaldrick"] = "Кальдрик";
	AL["Lenzo"] = "Ленцо";
	AL["Mawago"] = "Маваго";
	AL["Melasong"] = "Меласонг";
	AL["Melissa"] = "Мелисса";
	AL["Micah"] = "Мика";
	AL["Relissa"] = "Релисса";
	AL["Rosa"] = "Роза";
	AL["Tyllan"] = "Тиллан";

	--Zul'Gurub
	AL["Briney Boltcutter <Blackwater Financial Interests>"] = "Брини Болторез <Финансовый воротила пиратов Черноводья>";
	AL["Vehini <Assault Provisions>"] = "Вехини <Поставщик провизии для армии>";
	AL["Overseer Blingbang"] = "Инспектор Бадабум";
	AL["Bloodslayer T'ara <Darkspear Veteran>"] = "Умертвительница Т'ара <Ветеран Черного Копья>";
	AL["Bloodslayer Vaena <Darkspear Veteran>"] = "Умертвительница Ваэна <Ветеран Черного Копья>";
	AL["Bloodslayer Zala <Darkspear Veteran>"] = "Умертвительница Залла <Ветеран Черного Копья>";
	AL["Helpful Jungle Monkey"] = "Услужливая мартышка";
	AL["Venomancer Mauri <The Snake's Whisper>"] = "Ядомант Маури <Змееязыкая>";
	AL["Zanzil's Cauldron of Toxic Torment"] = "Котел Занзила с едкой щелочью";
	AL["Tiki Lord Mu'Loa"] = "Вождь тики Му'Лоа";
	AL["Gub <Destroyer of Fish>"] = "Габ <Гроза рыб>";
	AL["Venomancer T'Kulu <The Toxic Bite>"] = "Ядомант Т'Кулу <Ядовитый укус>";
	AL["Tor-Tun <The Slumberer>"] = "Тор-Тун <Спящий>";
	AL["Kaulema the Mover"] = "Каулема Толкатель";
	AL["Berserking Boulder Roller"] = "Яростный выворачиватель валунов";
	AL["Zanzil's Cauldron of Frostburn Formula"] = "Котел Занзила с раствором обжигающего холода";
	AL["Mor'Lek the Dismantler"] = "Мор'Лек Расчленитель";
	AL["Witch Doctor Qu'in <Medicine Woman>"] = "Доктор Ку'ин <Тролль-знахарка>";
	AL["Zanza the Restless"] = "Занза Неупокоенный";
	AL["Mortaxx <The Tolling Bell>"] = "Мортакс <Предвестник смерти>";
	AL["Tiki Lord Zim'wae"] = "Вождь тики Зим'вэ";
	AL["Zanzil's Cauldron of Burning Blood"] = "Котел Занзила с пылающей кровью";

--*********************
-- Mists of Pandaria Instances
--*********************

	--Gate of the Setting Sun
	AL["Bowmistress Li <Guard Captain>"] = "Лучница Ли <Капитан стражи>";

	--Heart of Fear

	--Mogu'shan Palace
	AL["Sinan the Dreamer"] = "Сынань Мечтательница";

	--Mogu'shan Vaults

	--Shado-Pan Monastery
	AL["Ban Bearheart"] = "Бань Медвежье Сердце";

	--Siege of Niuzao Temple
	AL["Shado-Master Chum Kiu"] = "Шадо-мастер Чум Киу";

	--Siege of Orgrimmar

	--Stormstout Brewery
	AL["Auntie Stormstout"] = "Тетушка Буйный Портер";
	AL["Chen Stormstout"] = "Чэнь Буйный Портер";

	--Temple of the Jade Serpent
	AL["Master Windstrong"] = "Мастер Порывистый Ветер";
	AL["Priestess Summerpetal"] = "Жрица Летний Лепесток";

	--Terrace of Endless Spring

	--Throne of Thunder
	AL["Monara <The Last Queen>"] = "Монара <Последняя королева>";
	AL["No'ku Stormsayer <Lord of Tempest>"] = "Но'ку Буревестник <Владыка бури>";
	AL["Rocky Horror"] = "Скальный ужас";
	AL["Focused Eye"] = "Сосредоточенный глаз";
	AL["Unblinking Eye"] = "Немигающий глаз";
	AL["Archritualist Kelada"] = "Предводитель ритуалистов Келад";
	AL["Flesh'rok the Diseased <Primordial Saurok Horror>"] = "Мясо'рок Прокаженный <Древний ужас>";
	AL["Zao'cho <The Emperor's Shield>"] = "Зао'чо <Щит императора>";

end