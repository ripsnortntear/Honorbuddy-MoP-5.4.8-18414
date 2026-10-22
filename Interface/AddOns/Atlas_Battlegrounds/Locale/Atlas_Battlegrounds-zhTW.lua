-- $Id: Atlas_Battlegrounds-zhTW.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "zhTW", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "戰場地圖";
	AL["Rescued"] = "營救";
	AL["Span of 5"] = "每五級一階"; -- Blizzard's span to put players with similar level range into a BG (10-14, 15-29)

	--Places
	AL["AV"] = "AV/奧山"; -- Alterac Valley
	AL["AB"] = "AB/阿拉希"; -- Arathi Basin
	AL["EotS"] = "EotS/暴風";
	AL["IoC"] = "IoC"; -- Isle of Conquest 征服之島
	AL["SotA"] = "SotA/遠祖"; -- Strand of the Ancients
	AL["WSG"] = "WSG/戰歌"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "范達爾·雷矛 <雷矛將軍>";
	AL["Prospector Stonehewer"] = "勘察員塔雷·石鎬";
	AL["Dun Baldar North Bunker"] = "丹巴達爾北部碉堡";
	AL["Wing Commander Mulverick"] = "空軍指揮官穆維里克";--omitted from AVS
	AL["Dun Baldar South Bunker"] = "丹巴達爾南部碉堡";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "蓋爾丁 <雷矛物資商人>";
	AL["Stormpike Banner"] = "雷矛軍旗";
	AL["Stormpike Lumber Yard"] = "雷矛林場";
	AL["Wing Commander Jeztor"] = "空軍指揮官傑斯托";--omitted from AVS
	AL["Wing Commander Guse"] = "空軍指揮官古斯";--omitted from AVS
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "巴琳達·石爐上尉 <雷矛上尉>";
	AL["Western Crater"] = "西部凹地";
	AL["Vipore's Beacon"] = "維波里的信號燈";
	AL["Jeztor's Beacon"] = "傑斯托的信號燈";
	AL["Eastern Crater"] = "東部凹地";
	AL["Slidore's Beacon"] = "斯里多爾的信號燈";
	AL["Guse's Beacon"] = "古斯的信號燈";
	AL["Arch Druid Renferal"] = "大德魯伊雷弗拉爾";
	AL["Murgot Deepforge"] = "莫高特·深爐";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "蘭納·雷酒 <鐵匠供應商>";
	AL["Stormpike Stable Master <Stable Master>"] = "雷矛獸欄管理員";
	AL["Stormpike Ram Rider Commander"] = "雷矛山羊騎兵指揮官";
	AL["Svalbrad Farmountain <Trade Goods>"] = "斯瓦爾布萊德·遠山 <商人>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "庫德拉姆·麥鬚 <材料與藥水供應商>";
	AL["Stormpike Quartermaster"] = "雷矛軍需官";
	AL["Jonivera Farmountain <General Goods>"] = "約尼維拉·遠山 <雜貨商>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "布羅古斯·雷酒 <食物和飲料>";
	AL["Wing Commander Ichman"] = "空軍指揮官艾克曼";--omitted from AVS
	AL["Wing Commander Slidore"] = "空軍指揮官斯里多爾";--omitted from AVS
	AL["Wing Commander Vipore"] = "空軍指揮官維波里";--omitted from AVS
	AL["Stormpike Ram Rider Commander"] = "雷矛山羊騎兵指揮官";
	AL["Ivus the Forest Lord"] = "『森林之王』伊弗斯";
	AL["Stormpike Aid Station"] = "雷矛急救站";
	AL["Ichman's Beacon"] = "艾克曼的信號燈";
	AL["Mulverick's Beacon"] = "穆維里克的信號燈";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "德雷克塔爾 <霜狼將軍>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "加爾凡加上尉 <霜狼上尉>";
	AL["Iceblood Tower"] = "冰血哨塔";
	AL["Tower Point"] = "哨塔高地";
	AL["West Frostwolf Tower"] = "西部霜狼哨塔";
	AL["East Frostwolf Tower"] = "東部霜狼哨塔";
	AL["Frostwolf Banner"] = "霜狼軍旗";
	AL["Lokholar the Ice Lord"] = "『冰雪之王』洛克霍拉";
	AL["Jotek"] = "喬泰克";
	AL["Smith Regzar"] = "鐵匠雷格薩";
	AL["Primalist Thurloga"] = "原獵者瑟魯加";
	AL["Frostwolf Stable Master <Stable Master>"] = "霜狼獸欄管理員";
	AL["Frostwolf Wolf Rider Commander"] = "霜狼騎兵指揮官";
	AL["Frostwolf Quartermaster"] = "霜狼軍需官";
	AL["Frostwolf Relief Hut"] = "霜狼急救站";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "收集希利塞斯";
	AL["Alliance's Camp"] = "聯盟營地";
	AL["Horde's Camp"] = "部落營地";

	--Eye of the Storm
	AL["Flag"] = "旗幟";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "軍需官戴夫恩·瓦克拉夫";
	AL["Chief Researcher Kartos"] = "首席調查員卡托斯";
	AL["Aldraan <Blade Merchant>"] = "阿爾德蘭 <劍刃武器商>";
	AL["Cendrii <Food & Drink>"] = "善德利 <食物和飲料>";
	AL["Quartermaster Jaffrey Noreliqe"] = "軍需官傑夫利·諾利克";
	AL["Chief Researcher Amereldine"] = "首席調查員阿莫瑞丹";
	AL["Coreiel <Blade Merchant>"] = "寇瑞歐 <劍刃武器商>";
	AL["Embelar <Food & Drink>"] = "安畢拉爾 <食物和飲料>";
	AL["Wyvern Camp"] = "雙足翼龍營地";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "地獄火防禦堡壘";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "精神哨塔";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "西部哨塔";
	AL["East Beacon"] = "東部哨塔";
	AL["Horde Field Scout"] = "部落戰場斥候";
	AL["Alliance Field Scout"] = "聯盟戰場斥候";
	AL["Twinspire Graveyard"] = "雙塔墓地";

	--Isle of Conquest
	AL["Gates are marked with red bars."] = "閘門以紅條標記.";
	AL["Overlord Agmar"] = "霸主阿格瑪";
	AL["High Commander Halford Wyrmbane <7th Legion>"] = "高階指揮官海弗德·龍禍 <第七軍團>";
	AL["The Refinery"] = "精煉廠";
	AL["The Docks"] = "碼頭";
	AL["The Workshop"] = "工坊";
	AL["The Hangar"] = "機棚";
	AL["The Quarry"] = "礦場";
	AL["Contested Graveyards"] = "爭奪中的墓地";
	AL["Horde Graveyard"] = "部落墓地";
	AL["Alliance Graveyard"] = "聯盟墓地";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "大門已被標記顏色";
	AL["Attacking Team"] = "攻擊隊伍";
	AL["Defending Team"] = "防禦隊伍";
	AL["Massive Seaforium Charge"] = "巨型爆鹽炸藥";
	AL["Titan Relic"] = "泰坦聖物";
	AL["Battleground Demolisher"] = "戰場石毀車";
	AL["Graveyard Flag"] = "墓地旗幟";
	AL["Resurrection Point"] = "復活點";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "堡壘載具工坊 (東)";
	AL["Fortress Vihecal Workshop (W)"] = "堡壘載具工坊 (西)";
	AL["Sunken Ring Vihecal Workshop"] = "沉沒之環載具工坊";
	AL["Broken Temple Vihecal Workshop"] = "破碎神殿載具工坊";
	AL["Eastspark Vihecale Workshop"] = "東炫載具工坊";
	AL["Westspark Vihecale Workshop"] = "西炫載具工坊";
	AL["Wintergrasp Graveyard"] = "堡壘墓地";
	AL["Sunken Ring Graveyard"] = "沉沒之環墓地";
	AL["Broken Temple Graveyard"] = "破碎神殿墓地";
	AL["Southeast Graveyard"] = "東南墓地";
	AL["Southwest Graveyard"] = "西南墓地";

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "攻擊方";
	AL["Sergeant Parker <Baradin's Wardens>"] = "派克中士 <巴拉丁鐵衛>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "第二中尉文斯沃斯 <巴拉丁鐵衛>";
	AL["Commander Stevens <Baradin's Wardens>"] = "指揮官史蒂文斯 <巴拉丁鐵衛>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "法洛斯元帥 <巴拉丁鐵衛>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "指揮官札諾斯 <地獄吼先鋒>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "訓練員拉茲苟斯 <地獄吼先鋒>";
	AL["Private Garnoth <Hellscream's Reach>"] = "士兵卡爾諾斯 <地獄吼先鋒>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "兵官中士拉茲加爾 <地獄吼先鋒>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "蠻錘長屋";
	AL["Dragonmaw Clan Compound"] = "龍喉氏族營地";

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Mine Cart Spawn Point";
	AL["Mine Cart Depot"] = "Mine Cart Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Orb of Power";
	AL["Center Point (Maximum Points)"] = "Center Point (Maximum Points)";

	-- Deepwind Gorge
	AL["Center Mine"] = "Center Mine";
end
