-- $Id: Atlas_Battlegrounds-zhCN.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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
-- Atlas Localization Data (Simplified Chinese)
-- Initial translation by DiabloHu
-- Maintained by DiabloHu, arith, Ananhaid

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "zhCN", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "战场地图";
	AL["Rescued"] = "被营救";
	AL["Span of 5"] = "每5级一阶"; -- Blizzard's span to put players with similar level range into a BG (10-14, 15-29)

	--Places
	AL["AV"] = "AV"; -- Alterac Valley
	AL["AB"] = "AB"; -- Arathi Basin
	AL["EotS"] = "EotS";
	AL["IoC"] = "IoC"; -- Isle of Conquest
	AL["SotA"] = "SotA"; -- Strand of the Ancients
	AL["WSG"] = "WSG"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "范达尔·雷矛 <雷矛将军>";
	AL["Prospector Stonehewer"] = "勘查员塔雷·石镐";
	AL["Dun Baldar North Bunker"] = "丹巴达尔北部碉堡";
	AL["Wing Commander Mulverick"] = "空军指挥官穆维里克";--omitted from AVS
	AL["Dun Baldar South Bunker"] = "丹巴达尔南部碉堡";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "盖尔丁 <雷矛军需官>";
	AL["Stormpike Banner"] = "雷矛军旗";
	AL["Stormpike Lumber Yard"] = "雷矛伐木场";
	AL["Wing Commander Jeztor"] = "空军指挥官杰斯托";--omitted from AVS
	AL["Wing Commander Guse"] = "空军指挥官古斯";--omitted from AVS
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "巴琳达·斯通赫尔斯 <雷矛上尉>";
	AL["Western Crater"] = "西部平原";
	AL["Vipore's Beacon"] = "维波里的信号灯";
	AL["Jeztor's Beacon"] = "杰斯托的信号灯";
	AL["Eastern Crater"] = "东部平原";
	AL["Slidore's Beacon"] = "斯里多尔的信号灯";
	AL["Guse's Beacon"] = "古斯的信号灯";
	AL["Arch Druid Renferal"] = "大德鲁伊雷弗拉尔";
	AL["Murgot Deepforge"] = "莫高特·深炉";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "兰纳·雷酒 <锻造供应商>";
	AL["Stormpike Stable Master <Stable Master>"] = "雷矛兽栏管理员 <兽栏管理员>";
	AL["Stormpike Ram Rider Commander"] = "雷矛山羊骑兵指挥官";
	AL["Svalbrad Farmountain <Trade Goods>"] = "斯瓦尔布莱德·远山 <商人>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "库德拉姆·麦须 <毒药和材料>";
	AL["Stormpike Quartermaster"] = "雷矛军需官";
	AL["Jonivera Farmountain <General Goods>"] = "约尼维拉·远山 <杂货商>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "布罗古斯·雷酒 <食物和饮料>";
	AL["Wing Commander Ichman"] = "空军指挥官艾克曼";--omitted from AVS
	AL["Wing Commander Slidore"] = "空军指挥官斯里多尔";--omitted from AVS
	AL["Wing Commander Vipore"] = "空军指挥官维波里";--omitted from AVS
	AL["Stormpike Ram Rider Commander"] = "雷矛山羊骑兵指挥官";
	AL["Ivus the Forest Lord"] = "森林之王伊弗斯";
	AL["Stormpike Aid Station"] = "雷矛急救站";
	AL["Ichman's Beacon"] = "艾克曼的信号灯";
	AL["Mulverick's Beacon"] = "穆维里克的信号灯";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "德雷克塔尔 <霜狼将军>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "加尔范上尉 <霜狼上尉>";
	AL["Iceblood Tower"] = "冰血哨塔";
	AL["Tower Point"] = "哨塔高地";
	AL["West Frostwolf Tower"] = "西部霜狼哨塔";
	AL["East Frostwolf Tower"] = "东部霜狼哨塔";
	AL["Frostwolf Banner"] = "霜狼军旗";
	AL["Lokholar the Ice Lord"] = "冰雪之王洛克霍拉";
	AL["Jotek"] = "乔泰克";
	AL["Smith Regzar"] = "铁匠雷格萨";
	AL["Primalist Thurloga"] = "指挥官瑟鲁加";
	AL["Frostwolf Stable Master <Stable Master>"] = "霜狼兽栏管理员 <兽栏管理员>";
	AL["Frostwolf Wolf Rider Commander"] = "霜狼骑兵指挥官";
	AL["Frostwolf Quartermaster"] = "霜狼军需官";
	AL["Frostwolf Relief Hut"] = "霜狼急救站";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "横扫沙漠水晶尘";
	AL["Alliance's Camp"] = "联盟营地";
	AL["Horde's Camp"] = "部落营地";

	--Eye of the Storm
	AL["Flag"] = "旗帜";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "军需官达维安·瓦克拉弗";
	AL["Chief Researcher Kartos"] = "主研究员卡托斯";
	AL["Aldraan <Blade Merchant>"] = "阿尔德兰 <铸剑商>";
	AL["Cendrii <Food & Drink>"] = "森德里 <餐饮供应商>";
	AL["Quartermaster Jaffrey Noreliqe"] = "军需官亚弗雷·诺雷里克";
	AL["Chief Researcher Amereldine"] = "主研究员阿米蒂恩";
	AL["Coreiel <Blade Merchant>"] = "克蕾伊尔 <铸剑商>";
	AL["Embelar <Food & Drink>"] = "艾比拉尔 <餐饮供应商>";
	AL["Wyvern Camp"] = "双足飞龙营地";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "地狱火半岛的工事";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "灵魂之塔";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "西部灯塔";
	AL["East Beacon"] = "东部灯塔";
	AL["Horde Field Scout"] = "部落斥候";
	AL["Alliance Field Scout"] = "联盟斥候";
	AL["Twinspire Graveyard"] = "双塔墓地";

	--Isle of Conquest
	AL["Gates are marked with red bars."] = "大门以红色进行了标记。";
	AL["Overlord Agmar"] = "阿格玛大王";
	AL["High Commander Halford Wyrmbane <7th Legion>"] = "高级指挥官哈尔弗·维姆班恩 <第七军团>";
	AL["The Refinery"] = "油料精炼厂";
	AL["The Docks"] = "码头";
	AL["The Workshop"] = "车间";
	AL["The Hangar"] = "飞艇基地";
	AL["The Quarry"] = "采石场";
	AL["Contested Graveyards"] = "争夺中的墓地";
	AL["Horde Graveyard"] = "部落墓地";
	AL["Alliance Graveyard"] = "联盟墓地";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "大门以其颜色进行了标记。";
	AL["Attacking Team"] = "进攻方";
	AL["Defending Team"] = "防守方";
	AL["Massive Seaforium Charge"] = "大型爆盐炸弹";
	AL["Titan Relic"] = "泰坦圣物";
	AL["Battleground Demolisher"] = "战场攻城车";
	AL["Graveyard Flag"] = "墓地旗帜";
	AL["Resurrection Point"] = "复活点";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "堡垒车间（东部）";
	AL["Fortress Vihecal Workshop (W)"] = "堡垒车间（西部）";
	AL["Sunken Ring Vihecal Workshop"] = "沉降之环车间";
	AL["Broken Temple Vihecal Workshop"] = "破碎神殿车间";
	AL["Eastspark Vihecale Workshop"] = "东部火花车间";
	AL["Westspark Vihecale Workshop"] = "西部火花车间";
	AL["Wintergrasp Graveyard"] = "冬拥湖墓地";
	AL["Sunken Ring Graveyard"] = "沉降之环墓地";
	AL["Broken Temple Graveyard"] = "破碎神殿墓地";
	AL["Southeast Graveyard"] = "东南墓地";
	AL["Southwest Graveyard"] = "西南墓地";

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "进攻者";
	AL["Sergeant Parker <Baradin's Wardens>"] = "帕克中士 <巴拉丁典狱官>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "万斯沃斯少尉 <巴拉丁典狱官>";
	AL["Commander Stevens <Baradin's Wardens>"] = "指挥官斯蒂文斯 <巴拉丁典狱官>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "法洛斯元帅 <巴拉丁典狱官>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "指挥官扎诺斯 <地狱咆哮近卫军>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "拉斯格斯教官 <地狱咆哮近卫军>";
	AL["Private Garnoth <Hellscream's Reach>"] = "列兵加尔诺斯 <地狱咆哮近卫军>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "拉兹加尔上士 <地狱咆哮近卫军>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "蛮锤要塞";
	AL["Dragonmaw Clan Compound"] = "龙喉要塞";

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Mine Cart Spawn Point";
	AL["Mine Cart Depot"] = "Mine Cart Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Orb of Power";
	AL["Center Point (Maximum Points)"] = "Center Point (Maximum Points)";

	-- Deepwind Gorge
	AL["Center Mine"] = "Center Mine";
end
