-- $Id: Atlas-zhTW.lua 2120 2013-09-16 16:12:39Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas", "zhTW", false);

if ( GetLocale() == "zhTW" ) then
-- Define the leading strings to be ignored while sorting
-- Ex: The Stockade
AtlasSortIgnore = {};

-- Syntax: ["real_zone_name"] = "localized map zone name"
AtlasZoneSubstitutions = {
	["Ahn'Qiraj"] = "安其拉神廟";
	["Karazhan"] = "卡拉贊 - 1.開始";
};
end


if AL then
--************************************************
-- UI terms and common strings
--************************************************
	AL["ATLAS_TITLE"] = "Atlas 地圖集";

	AL["BINDING_HEADER_ATLAS_TITLE"] = "Atlas 按鍵設定";
	AL["BINDING_NAME_ATLAS_TOGGLE"] = "開啟/關閉 Atlas";
	AL["BINDING_NAME_ATLAS_OPTIONS"] = "切換設定";
	AL["BINDING_NAME_ATLAS_AUTOSEL"] = "自動選擇";

	AL["ATLAS_SLASH"] = "/atlas";
	AL["ATLAS_SLASH_OPTIONS"] = "options";

	AL["ATLAS_STRING_LOCATION"] = "所在位置";
	AL["ATLAS_STRING_LEVELRANGE"] = "等級範圍";
	AL["ATLAS_STRING_RECLEVELRANGE"] = "建議等級"; -- abbrevation and shorten of "Recommended Level Range", the dungeon's recommended level range
	AL["ATLAS_STRING_PLAYERLIMIT"] = "人數上限";
	AL["ATLAS_STRING_SELECT_CAT"] = "選擇類別";
	AL["ATLAS_STRING_SELECT_MAP"] = "選擇地圖";
	AL["ATLAS_STRING_SEARCH"] = "搜尋";
	AL["ATLAS_STRING_CLEAR"] = "清除";
	AL["ATLAS_STRING_MINLEVEL"] = "最低等級";

	AL["ATLAS_OPTIONS_BUTTON"] = "選項";
	AL["ATLAS_OPTIONS_SHOWBUT"] = "在小地圖旁顯示 Atlas 按鈕";
	AL["ATLAS_OPTIONS_SHOWBUT_TIP"] = "在小地圖旁顯示 Atlas 按鈕";
	AL["ATLAS_OPTIONS_AUTOSEL"] = "自動選擇副本地圖";
	AL["ATLAS_OPTIONS_AUTOSEL_TIP"] = "Atlas 可偵測您目前所在的副區域以顯示一個最佳的副本地圖";
	AL["ATLAS_OPTIONS_BUTPOS"] = "按鈕位置";
	AL["ATLAS_OPTIONS_LOCK"] = "鎖定 Atlas 視窗位置";
	AL["ATLAS_OPTIONS_LOCK_TIP"] = "設定將 Atlas 視窗位置鎖定或不鎖定";
	AL["ATLAS_OPTIONS_TRANS"] = "透明度";
	AL["ATLAS_OPTIONS_RCLICK"] = "滑鼠右鍵開啟世界地圖";
	AL["ATLAS_OPTIONS_RCLICK_TIP"] = "啟用在 Atlas 視窗中按下滑鼠右鍵自動切換到魔獸的世界地圖";
	AL["ATLAS_OPTIONS_RESETPOS"] = "重設位置";
	AL["ATLAS_OPTIONS_ACRONYMS"] = "顯示副本縮寫";
	AL["ATLAS_OPTIONS_ACRONYMS_TIP"] = "在地圖的詳盡敘述中顯示副本的縮寫";
	AL["ATLAS_OPTIONS_SCALE"] = "Atlas 視窗大小比率";
	AL["ATLAS_OPTIONS_BOSS_DESC"] = "當首領資訊可獲取時, 顯示該資訊";
	AL["ATLAS_OPTIONS_BOSS_DESC_TIP"] = "當滑鼠游標移動到地圖上首領的標號時, 並且首領資訊可獲取時, 顯示該首領的相關資訊.";
	AL["ATLAS_OPTIONS_BOSS_DESC_SCALE"] = "首領資訊提示視窗大小比率";
	AL["ATLAS_OPTIONS_BUTRAD"] = "按鈕半徑範圍";
	AL["ATLAS_OPTIONS_CLAMPED"] = "使 Atlas 視窗不超出遊戲畫面";
	AL["ATLAS_OPTIONS_CLAMPED_TIP"] = "使 Atlas 視窗被拖曳時不會超出遊戲主畫面的邊界, 關閉此選項則可將 Atlas 視窗拖曳並超出遊戲畫面邊界";
	AL["ATLAS_OPTIONS_CTRL"] = "按住 Ctrl 鍵以顯示工具提示";
	AL["ATLAS_OPTIONS_CTRL_TIP"] = "勾選後, 當滑鼠移到地圖資訊欄位時, 按下 Ctrl 控制鍵, 則會將資訊的完整資訊以提示型態顯示. 當資訊過長而被截斷時很有用.";
	AL["ATLAS_OPTIONS_DONTSHOWAGAIN"] = "不再顯示相同訊息。";
	AL["ATLAS_OPTIONS_CHECKMODULE"] = "提醒我是否有遺失的模組或插件";
	AL["ATLAS_OPTIONS_CHECKMODULE_TIP"] = "勾選以在每次登入 WoW 時檢查是否有遺失的 Atlas 模組或插件。";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN"] = "副本清單以難易度色彩顯示";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN_TIP"] = "依據副本建議的最低進入等級、以及玩家現今等級的差異，將副本清單以難易度色彩顯示。";

	AL["ATLAS_BUTTON_CLOSE"] = "關閉";
	AL["ATLAS_BUTTON_TOOLTIP_TITLE"] = "Atlas 副本地圖";
	AL["ATLAS_BUTTON_TOOLTIP_HINT"] = "左鍵開啟 Atlas.\n中鍵開啟 Atlas 選項.\n右鍵並拖曳以移動圖示按鈕位置.";
	AL["ATLAS_LDB_HINT"] = "左鍵開啟 Atlas.\n中鍵開啟 Atlas 選項.\n右鍵打開顯示選單.";
	AL["ATLAS_MINIMAPLDB_HINT"] = "左鍵開啟 Atlas.\n右鍵開啟 Atlas 選項.\n左鍵並拖曳以移動圖示按鈕位置.";

	AL["ATLAS_OPTIONS_CATDD"] = "副本地圖分類方式:";
	AL["ATLAS_DDL_CONTINENT"] = "依不同大陸分類";
	AL["ATLAS_DDL_CONTINENT_EASTERN"] = "東部王國副本";
	AL["ATLAS_DDL_CONTINENT_KALIMDOR"] = "卡林多副本";
	AL["ATLAS_DDL_CONTINENT_OUTLAND"] = "外域副本";
	AL["ATLAS_DDL_CONTINENT_NORTHREND"] = "北裂境副本";
	AL["ATLAS_DDL_CONTINENT_DEEPHOLM"] = "地深之源副本";
	AL["ATLAS_DDL_CONTINENT_PANDARIA"] = "潘達利亞副本";
	AL["ATLAS_DDL_LEVEL"] = "依等級分類";
	AL["ATLAS_DDL_LEVEL_UNDER45"] = "副本等級低於 45";
	AL["ATLAS_DDL_LEVEL_45TO60"] = "副本等級介於 45-60";
	AL["ATLAS_DDL_LEVEL_60TO70"] = "副本等級介於 60-70";
	AL["ATLAS_DDL_LEVEL_70TO80"] = "副本等級介於 70-80";
	AL["ATLAS_DDL_LEVEL_80TO85"] = "副本等級介於 80-85";
	AL["ATLAS_DDL_LEVEL_85TO90"] = "副本等級介於 85-90";
	AL["ATLAS_DDL_LEVEL_90PLUS"] = "副本等級大於 90";
	AL["ATLAS_DDL_PARTYSIZE"] = "依隊伍人數分類";
	AL["ATLAS_DDL_PARTYSIZE_5_AE"] = "5 人副本 1/3";
	AL["ATLAS_DDL_PARTYSIZE_5_FS"] = "5 人副本 2/3";
	AL["ATLAS_DDL_PARTYSIZE_5_TZ"] = "5 人副本 3/3";
	AL["ATLAS_DDL_PARTYSIZE_10_AN"] = "10 人副本 1/2";
	AL["ATLAS_DDL_PARTYSIZE_10_OZ"] = "10 人副本 2/2";
	AL["ATLAS_DDL_PARTYSIZE_20TO40AH"] = "20-40 人副本 1/2";
	AL["ATLAS_DDL_PARTYSIZE_20TO40IZ"] = "20-40 人副本 2/2";
	AL["ATLAS_DDL_EXPANSION"] = "依資料片分類";
	AL["ATLAS_DDL_EXPANSION_OLD_AO"] = "舊世界副本 1/2";
	AL["ATLAS_DDL_EXPANSION_OLD_PZ"] = "舊世界副本 2/2";
	AL["ATLAS_DDL_EXPANSION_BC"] = "燃燒的遠征副本";
	AL["ATLAS_DDL_EXPANSION_WOTLK"] = "巫妖王之怒副本";
	AL["ATLAS_DDL_EXPANSION_CATA"] = "浩劫與重生副本";
	AL["ATLAS_DDL_EXPANSION_MOP"] = "潘達利亞之謎副本";
	AL["ATLAS_DDL_TYPE"] = "依地圖類型分類";
	AL["ATLAS_DDL_TYPE_INSTANCE_AB"] = "副本 1/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_CF"] = "副本 2/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_GM"] = "副本 3/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_NS"] = "副本 4/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_TZ"] = "副本 5/5";
	AL["ATLAS_DDL_TYPE_ENTRANCE"] = "副本入口";

	AL["ATLAS_INSTANCE_BUTTON"] = "副本";
	AL["ATLAS_ENTRANCE_BUTTON"] = "入口";
	AL["ATLAS_SEARCH_UNAVAIL"] = "搜尋功能停用";

	AL["ATLAS_DEP_MSG1"] = "Atlas 偵測到過期的模組";
	AL["ATLAS_DEP_MSG2"] = "這些模組已從這個角色被停用";
	AL["ATLAS_DEP_MSG3"] = "請將這些模組從 AddOns 目錄移除";
	AL["ATLAS_DEP_OK"] = "OK";

	AL["ATLAS_INFO"] = "Atlas 訊息";
	AL["ATLAS_INFO_12200"] = "重要提示：\n\n由於副本插件檔案大小日益增加，我們已獨立出部分副本地圖和內建插件\n到單獨的模組。\n\n您從各大遊戲插件網站所下載的 Atlas 插件，可能只包含了主要的核心功能\n與最新資料片裡的副本地圖。\n\n若您想要取得包含舊的資料片的所有地圖，以及 Atlas 團隊所開發的其他\n模組，您必須分別下載這些獨立模組的壓縮檔並分別進行安裝。\n\n請詳讀我們論壇的這個討論串以了解更多詳情：\n|cff6666ffhttp://www.atlasmod.com/phpBB3/viewtopic.php?t=1522|cffffffff\n或造訪我們的首頁：\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";
	AL["ATLAS_INFO_12201"] = "我們最近新增了一個新的 Atlas 插件 - |cff6666ffAtlas 情境地圖|cffffffff，用以提供 WoW 5.0 \n起新增的情境事件的地圖。\n\n請參見我們的網站以取得更詳細的資訊，並請記得分別下載並安裝此插件。\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";

	AL["ATLAS_MISSING_MODULE"] = "Atlas 已偵測到遺失的模組/插件：";

--************************************************
-- Zone Names, Acronyms, and Common Strings
--************************************************

	--Common strings
	AL["East"] = "東";
	AL["North"] = "北";
	AL["South"] = "南";
	AL["West"] = "西";

	--World Events, Festival
	AL["Brewfest"] = "啤酒節";
	AL["Hallow's End"] = "萬鬼節";
	AL["Love is in the Air"] = "愛就在身邊";
	AL["Lunar Festival"] = "新年慶典";
	AL["Midsummer Festival"] = "仲夏節慶";

	--Misc strings
	AL["Colon"] = ":"; -- The colon symbol to be used in string, ex: "Zone: Firelands
	AL["Adult"] = "成年";
	AL["AKA"] = "又稱";
	AL["Arcane Container"] = "秘法容器";
	AL["Arms Warrior"] = "武戰";
	AL["Attunement Required"] = "需完成傳送門/鑰匙前置任務";
	AL["Back"] = "後方";
	AL["Basement"] = "地下室";
	AL["Blacksmithing Plans"] = "黑鐵鍛造圖樣";
	AL["Chase Begins"] = "追逐開始";
	AL["Chase Ends"] = "追逐結束";
	AL["Child"] = "幼年";
	AL["Click to open Dungeon Journal window."] = "按下以開啟地城導覽視窗.";
	AL["Connection"] = "通道";
	AL["Elevator"] = "電梯";
	AL["End"] = "結束";
	AL["Engineer"] = "工程師";
	AL["Entrance"] = "入口";
	AL["Event"] = "事件";
	AL["Exalted"] = "崇拜";
	AL["Exit"] = "出口";
	AL["Fourth Stop"] = "第四停留點";
	AL["Front"] = "前方";
	AL["Ghost"] = "鬼魂";
	AL["Graveyard"] = "墓地";
	AL["Heroic"] = "英雄";
	AL["Holy Paladin"] = "神聖聖騎";
	AL["Holy Priest"] = "神聖牧師";
	AL["Hunter"] = "獵人";
	AL["Imp"] = "小鬼";
	AL["Key"] = "鑰匙";
	AL["Lower"] = "下層";
	AL["Mage"] = "法師";
	AL["Meeting Stone"] = "集合石";
	AL["Middle"] = "中間";
	AL["Monk"] = "僧侶";
	AL["Moonwell"] = "月井";
	AL["Optional"] = "可選擇";
	AL["Orange"] = "橙";
	AL["Outside"] = "戶外";
	AL["Paladin"] = "聖騎士";
	AL["Portal"] = "入口/傳送門";
	AL["Priest"] = "牧師";
	AL["Protection Warrior"] = "防戰";
	AL["Purple"] = "紫";
	AL["Random"] = "隨機";
	AL["Rare"] = "稀有";
	AL["Reputation"] = "聲望";
	AL["Repair"] = "修理";
	AL["Retribution Paladin"] = "懲戒聖騎";
	AL["Rewards"] = "獎勵";
	AL["Rogue"] = "盜賊";
	AL["Second Stop"] = "第二停留點";
	AL["Shadow Priest"] = "暗影牧師";
	AL["Shaman"] = "薩滿";
	AL["Spawn Point"] = "生成點";
	AL["Start"] = "開始";
	AL["Summon"] = "召喚";
	AL["Teleporter"] = "傳送";
	AL["Teleporter destination"] = "傳送目的地";
	AL["Third Stop"] = "第三停留點";
	AL["Top"] = "上方";
	AL["Tunnel"] = "通道";
	AL["Underwater"] = "水下";
	AL["Upper"] = "上層";
	AL["Varies"] = "多處";
	AL["Wanders"] = "徘徊";
	AL["Warlock"] = "術士";
	AL["Warrior"] = "戰士";
	AL["Wave 5"] = "第 5 波";
	AL["Wave 6"] = "第 6 波";
	AL["Wave 10"] = "第 10 波";
	AL["Wave 12"] = "第 12 波";
	AL["Wave 18"] = "第 18 波";	
	AL["MapsNotFound"] = "目前的副本找不到對應的地圖影像檔.\n\n請確認您是否有安裝 Atlas 相關的副本地圖模組.";

	--Classic Acronyms
	AL["AQ"] = "AQ"; -- Ahn'Qiraj 安其拉
	AL["AQ20"] = "AQ20"; -- Ruins of Ahn'Qiraj 安其拉廢墟
	AL["AQ40"] = "AQ40"; -- Temple of Ahn'Qiraj 安其拉神廟
	AL["BFD"] = "BFD/黑淵"; -- Blackfathom Deeps 黑暗深淵
	AL["BRD"] = "BRD/黑石淵"; -- Blackrock Depths 黑石深淵
	AL["BRM"] = "BRM/黑石山"; -- Blackrock Mountain 黑石山
	AL["BWL"] = "BWL/黑翼"; -- Blackwing Lair 黑翼之巢
	AL["DM"] = "DM/厄運"; -- Dire Maul 厄運之槌
	AL["Gnome"] = "Gnome/諾姆"; -- Gnomeregan 諾姆瑞根
	AL["Halls"] = "Halls/大廳"; -- Scarlet Halls
	AL["LBRS"] = "LBRS/黑下";  -- Lower Blackrock Spire 黑石塔下層
	AL["Mara"] = "Mara/瑪拉"; -- Maraudon 瑪拉頓
	AL["MC"] = "MC"; -- Molten Core 熔火之心
	AL["RFC"] = "RFC/怒焰"; -- Ragefire Chasm 怒焰裂谷
	AL["RFD"] = "RFD"; -- Razorfen Downs 剃刀高地
	AL["RFK"] = "RFK"; -- Razorfen Kraul 剃刀沼澤
	AL["Scholo"] = "Scholo/通靈"; -- Scholomance 通靈學院
	AL["SFK"] = "SFK/影牙"; -- Shadowfang Keep 影牙城堡
	AL["SM"] = "SM/血色"; -- Scarlet Monastery 血色修道院
	AL["ST"] = "ST/神廟"; -- Sunken Temple 沉沒的神廟
	AL["Strat"] = "Strat/斯坦"; -- Stratholme 斯坦索姆
	AL["Stocks"] = "監獄"; -- The Stockade 監獄
	AL["UBRS"] = "UBRS/黑上"; -- Upper Blackrock Spire 黑石塔上層
	AL["Ulda"] = "Ulda"; -- Uldaman 奧達曼
	AL["VC"] = "VC/死礦"; -- The Deadmines 死亡礦坑
	AL["WC"] = "WC/哀嚎"; -- Wailing Caverns 哀嚎洞穴
	AL["ZF"] = "ZF/祖法"; -- Zul'Farrak 祖爾法拉克

	--BC Acronyms
	AL["AC"] = "AC"; -- Auchenai Crypts 奧奇奈地穴
	AL["Arca"] = "Arca/亞克"; -- The Arcatraz 亞克崔茲
	AL["Auch"] = "Auch"; -- Auchindoun 奧齊頓
	AL["BF"] = "BF"; -- The Blood Furnace 血熔爐
	AL["BT"] = "BT/黑廟"; -- Black Temple 黑暗神廟
	AL["Bota"] = "Bota/波塔"; -- The Botanica 波塔尼卡
	AL["CoT"] = "CoT"; -- Caverns of Time 時光之穴
	AL["CoT1"] = "CoT1/舊址"; -- Old Hillsbrad Foothills 希爾斯布萊德丘陵舊址
	AL["CoT2"] = "CoT2/黑沼"; -- The Black Morass 黑色沼澤
	AL["CoT3"] = "CoT3/海山"; -- Hyjal Summit 海加爾山
	AL["CR"] = "CR/盤牙"; -- Coilfang Reservoir
	AL["GL"] = "GL/戈魯爾"; -- Gruul's Lair 戈魯爾之巢
	AL["HC"] = "HC/火堡"; -- Hellfire Citadel 地獄火堡壘
	AL["Kara"] = "Kara/卡拉"; -- Karazhan 卡拉贊
	AL["MaT"] = "MT/博學"; -- Magisters' Terrace 博學者殿堂
	AL["Mag"] = "Mag/瑪瑟"; -- Magtheridon's Lair 瑪瑟里頓的巢穴
	AL["Mech"] = "Mech/麥克"; -- The Mechanar 麥克納爾
	AL["MT"] = "MT/法力"; -- Mana-Tombs 法力墓地
	AL["Ramp"] = "Ramp"; -- Hellfire Ramparts 地獄火壁壘
	AL["SSC"] = "SSC/毒蛇"; -- Serpentshrine Cavern 毒蛇神殿洞穴
	AL["Seth"] = "Seth/塞司克"; -- Sethekk Halls 塞司克大廳
	AL["SH"] = "SH/破碎"; -- The Shattered Halls 破碎大廳
	AL["SL"] = "SL/迷宮"; -- Shadow Labyrinth 暗影迷宮
	AL["SP"] = "SP"; -- The Slave Pens 奴隸監獄
	AL["SuP"] = "SP/太陽井"; -- Sunwell Plateau 太陽之井高地
	AL["SV"] = "SV/蒸汽"; -- The Steamvault 蒸汽洞窟
	AL["TK"] = "TK/風暴"; -- Tempest Keep 風暴要塞
	AL["UB"] = "UB/深幽"; -- The Underbog 深幽泥沼

	--WotLK Acronyms
	AL["AK, Kahet"] = "AK/安卡"; -- Ahn'kahet -- 安卡罕特
	AL["AN, Nerub"] = "AN/奈幽"; -- Azjol-Nerub -- 阿茲歐-奈幽
	AL["Champ"] = "勇士"; -- Trial of the Champion -- 勇士試煉
	AL["CoT-Strat"] = "CoT斯坦"; -- Culling of Stratholme -- 斯坦索姆的抉擇
	AL["Crus"] = "十字軍"; -- Trial of the Crusader --十字軍試煉
	AL["DTK"] = "DTK/德拉克"; -- Drak'Tharon Keep -- 德拉克薩隆要塞
	AL["FoS"] = "FoS/熔爐"; 
	AL["FH1"] = "FH1"; -- The Forge of Souls -- 眾魂熔爐
	AL["Gun"] = "Gun/剛德"; -- Gundrak -- 剛德拉克
	AL["HoL"] = "HoL/雷光"; -- Halls of Lightning --雷光大廳
	AL["HoR"] = "HoR/倒影"; 
	AL["FH3"] = "FH3"; -- Halls of Reflection -- 倒影大廳
	AL["HoS"] = "HoS/石廳"; -- Halls of Stone -- 石之大廳
 	AL["IC"] = "ICC/冰冠"; -- Icecrown Citadel -- 冰冠城塞
	AL["Nax"] = "Nax/納克"; -- Naxxramas -- 納克薩瑪斯
	AL["Nex, Nexus"] = "Nex/奧心"; -- The Nexus -- 奧核之心
	AL["Ocu"] = "Ocu/奧眼"; -- The Oculus -- 奧核之眼
	AL["Ony"] = "Ony/黑龍"; -- Onyxia's Lair 奧妮克希亞的巢穴
	AL["OS"] = "OS/黑曜"; -- The Obsidian Sanctum -- 黑曜聖所
	AL["PoS"] = "PoS"; 
	AL["FH2"] = "FH2"; -- Pit of Saron -- 薩倫之淵
	AL["RS"] = "RS/晶紅"; -- The Ruby Sanctum
	AL["TEoE"] = "TEoE/永恆"; -- The Eye of Eternity--永恆之眼
	AL["UK, Keep"] = "UK/俄塞"; -- Utgarde Keep -- 俄特加德要塞
	AL["Uldu"] = "Uldu/奧杜亞"; -- Ulduar-- 奧杜亞
	AL["UP, Pinn"] = "UP/俄巔"; -- Utgarde Pinnacle -- 俄特加德之巔
	AL["VH"] = "VH/紫堡"; -- The Violet Hold -- 紫羅蘭堡
	AL["VoA"] = "VoA/亞夏"; -- Vault of Archavon--亞夏梵穹殿

	--Zones not included in LibBabble-Zone
	AL["Crusaders' Coliseum"] = "銀白大競技場";

	--Cataclysm Acronyms
	AL["BH"] = "BH"; --Baradin Hold 巴拉丁堡
	AL["BoT"] = "BoT"; --Bastion of Twilight 暮光堡壘
	AL["BRC"] = "BRC"; --Blackrock Caverns 黑石洞穴
	AL["BWD"] = "BWD"; --Blackwing Descent 黑翼陷窟
	AL["CoT-DS"] = "CoT-DS"; --Caverns of Time: Dragon Soul
	AL["CoT-ET"] = "CoT-ET"; --Caverns of Time: End Time
	AL["CoT-HoT"] = "CoT-HoT"; --Caverns of Time: Hour of Twilight
	AL["CoT-WoE"] = "CoT-WoE"; --Caverns of Time: Well of Eternity
	AL["FL"] = "FL"; --Firelands 火源之界
	AL["GB"] = "GB"; --Grim Batol 格瑞姆巴托
	AL["HoO"] = "HoO"; --Halls of Origination 起源大廳
	AL["LCoT"] = "LCoT"; --Lost City of the Tol'vir 托維爾的失落之城
	AL["TSC"] = "TSC"; --The Stonecore 石岩之心
	AL["TWT"] = "TWT"; --Throne of the Four Winds 四風王座
	AL["ToTT"] = "ToTT"; --Throne of the Tides 海潮王座
	AL["VP"] = "VP"; --The Vortex Pinnacle 漩渦尖塔
	AL["ZA"] = "ZA"; -- Zul'Aman 祖阿曼
	AL["ZG"] = "ZG"; --Zul'Gurub 祖爾格拉布

	--MoP Acronyms
	AL["GSS"] = "GSS"; --Gate of the Setting Sun
	AL["HoF"] = "HoF"; --Heart of Fear
	AL["MP"] = "MP"; --Mogu'shan Palace
	AL["MV"] = "MV"; --Mogu'shan Vaults
	AL["SPM"] = "SPM"; --Shado-Pan Monastery
	AL["SNT"] = "SNT"; --Siege of Niuzao Temple
	AL["SB"] = "SB"; --Stormstout Brewery
	AL["SoO"] = "SoO"; --Siege of Orgrimmar
	AL["TJS"] = "TJS"; --Temple of the Jade Serpent
	AL["TES"] = "TES"; --Terrace of Endless Spring
	AL["ToT"] = "ToT"; --Throne of Thunder
	
	--Map sections
	AL["MapA"] = " [1]"; -- For example: Shado-Pan Monastery [A]
	AL["MapB"] = " [2]"; -- 一, 二, 三...won't work as somehow it will be sorted as 一, 三, 二, 四. so need to change to digits
	AL["MapC"] = " [3]";
	AL["MapD"] = " [4]";
	AL["MapE"] = " [5]";
	AL["MapF"] = " [6]";

--************************************************
-- Instance Entrance Maps
--************************************************

	--Auchindoun (Entrance)
	AL["Clarissa"] = "克萊瑞莎";
	AL["Greatfather Aldrimus"] = "大祖父阿爾崔瑪斯";
	AL["Ha'lei"] = "哈勒";
	AL["Horvon the Armorer <Armorsmith>"] = "護甲匠霍沃 <護甲鍛造師>";
	AL["Ramdor the Mad"] = "瘋狂者藍姆多";
	AL["Nexus-Prince Haramad"] = "奈薩斯王子哈拉瑪德";
	AL["\"Slim\" <Shady Dealer>"] = "『瘦子』 <黑市商人>";
	AL["\"Captain\" Kaftiz"] = "隊長卡夫提茲";
	AL["Dealer Tariq <Shady Dealer>"] = "商人塔爾利奎 <黑市商人>";
	AL["Provisioner Tsaalt"] = "糧食供應者·茲索特";

	--Blackfathom Deeps (Entrance)

	--Blackrock Mountain (Entrance)
	AL["Bodley"] = "布德利";
	AL["Lothos Riftwaker"] = "洛索斯·天痕";
	AL["Orb of Command"] = "命令寶珠";
	AL["Scarshield Quartermaster <Scarshield Legion>"] = "裂盾軍需官 <裂盾軍團>";
	AL["The Behemoth"] = "貝希摩斯";

	--Caverns of Time (Entrance)
	AL["Steward of Time <Keepers of Time>"] = "時間服務員 <時光守望者>";
	AL["Alexston Chrome <Tavern of Time>"] = "艾力克斯頓·科洛米 <時間酒館>";
	AL["Yarley <Armorer>"] = "亞利 <護甲商>";
	AL["Bortega <Reagents & Poison Supplies>"] = "伯特卡 <施法材料和毒藥供應商>";
	AL["Alurmi <Keepers of Time Quartermaster>"] = "阿勒米 <時光守望者軍需官>";
	AL["Galgrom <Provisioner>"] = "卡葛隆姆 <物資供應者>";
	AL["Zaladormu"] = "薩拉多姆";
	AL["Soridormi <The Scale of Sands>"] = "索芮朵蜜 <流沙之鱗>";
	AL["Arazmodu <The Scale of Sands>"] = "阿拉斯莫杜 <流沙之鱗>";
	AL["Andormu <Keepers of Time>"] = "安杜姆 <時光守望者>";
	AL["Nozari <Keepers of Time>"] = "諾札瑞 <時光守望者>";
	AL["Anachronos <Keepers of Time>"] = "安納克羅斯 <時光守望者>";

	--Caverns of Time: Hyjal (Entrance)
	AL["Indormi <Keeper of Ancient Gem Lore>"] = "隱多米 <寶石傳說的守護者>";
	AL["Tydormu <Keeper of Lost Artifacts>"] = "提多姆 <失落的神器看管者>";

	--Coilfang Reservoir (Entrance)
	AL["Mortog Steamhead"] = "莫塔格·史提海德";

	--Dire Maul (Entrance)
	AL["Dire Pool"] = "厄運之池";
	AL["Dire Maul Arena"] = "厄運競技場";
	AL["Elder Mistwalker"] = "霧行長者";

	--Gnomeregan (Entrance)
	AL["Torben Zapblast <Teleportation Specialist>"] = "托爾班·速轟 <傳送專家>";

	--Hellfire Citadel (Entrance)
	AL["Steps and path to the Blood Furnace"] = "通往血熔爐的階梯與通道";
	AL["Path to the Hellfire Ramparts and Shattered Halls"] = "通往地獄火壁壘與破碎大廳的通道";
	AL["Meeting Stone of Magtheridon's Lair"] = "集合石 - 瑪瑟里頓的巢穴";
	AL["Meeting Stone of Hellfire Citadel"] = "集合石 - 地獄火堡壘";

	--Icecrown Citadel (Entrance)

	--Karazhan (Entrance)
	AL["Archmage Leryda"] = "大法師利瑞達";
	AL["Archmage Alturus"] = "大法師艾特羅斯";
	AL["Apprentice Darius"] = "學徒達瑞爾斯";
	AL["Stairs to Underground Pond"] = "通往地底池塘的階梯";
	AL["Stairs to Underground Well"] = "通往地底水井的階梯";
	AL["Charred Bone Fragment"] = "燒焦的白骨碎片";

	--Maraudon (Entrance)
	AL["The Nameless Prophet"] = "無名預言者";
	AL["Cursed Centaur"] = "被詛咒的半人馬";
	AL["Kherrah"] = "凱拉";

	--Scarlet Monastery (Entrance)

	--The Deadmines (Entrance)

	--Sunken Temple (Entrance)
	AL["Priestess Udum'bra"] = "女祭師烏丹姆布拉";
	AL["Gomora the Bloodletter"] = "『放血者』高摩拉";
	AL["Captain Wyrmak"] = "維爾瑪克隊長";

	--Uldaman (Entrance)

	--Ulduar (Entrance)
	AL["Shavalius the Fancy <Flight Master>"] = "『狂想』夏瓦利厄斯 <飛行管理員>";
	AL["Chester Copperpot <General & Trade Supplies>"] = "查斯特·銅壺 <一般與貿易供應商>";
	AL["Slosh <Food & Drink>"] = "斯洛許 <食物和飲料>";

	--Wailing Caverns (Entrance)

--************************************************
-- Kalimdor Instances (Classic)
--************************************************

	--Blackfathom Deeps
	AL["Shrine of Gelihast"] = "格里哈斯特神殿";
	AL["Fathom Stone"] = "深淵之石";
	AL["Lorgalis Manuscript"] = "洛迦里斯手稿";
	AL["Scout Thaelrid"] = "斥候塞爾瑞德";
	AL["Flaming Eradicator"] = "火焰根除者";
	AL["Altar of the Deeps"] = "瑪塞斯特拉祭壇";
	AL["Ashelan Northwood"] = "阿謝蘭·北木";
	AL["Relwyn Shadestar"] = "芮爾溫·影星";
	AL["Sentinel Aluwyn"] = "哨兵阿露溫";
	AL["Sentinel-trainee Issara"] = "哨兵受訓員伊薩拉";
	AL["Je'neu Sancrea <The Earthen Ring>"] = "耶努薩克雷 <陶土議會>";
	AL["Zeya"] = "仄亞";

	--Dire Maul (East)
	AL["\"Ambassador\" Dagg'thol"] = "達格索大使";
	AL["Furgus Warpwood"] = "佛格斯·扭木";
	AL["Old Ironbark"] = "埃隆巴克";
	AL["Ironbark the Redeemed"] = "贖罪的鐵朴";

	--Dire Maul (North)
	AL["Druid of the Talon"] = "猛禽德魯伊";
	AL["Stonemaul Ogre"] = "石槌巨魔";
	AL["Knot Thimblejack"] = "諾特·希姆加克";

	--Dire Maul (West)
	AL["Ferra"] = "費拉";
	AL["Estulan <The Highborne>"] = "艾斯圖蘭";
	AL["Shen'dralar Watcher"] = "辛德拉看守者";
	AL["Pylons"] = "水晶塔";
	AL["Ancient Equine Spirit"] = "上古聖馬之魂";
	AL["Shen'dralar Ancient"] = "辛德拉古靈";
	AL["Falrin Treeshaper"] = "法琳·樹形者";
	AL["Lorekeeper Lydros"] = "博學者萊德羅斯";
	AL["Lorekeeper Javon"] = "博學者亞沃";
	AL["Lorekeeper Kildrath"] = "博學者基爾達斯";
	AL["Lorekeeper Mykos"] = "博學者麥庫斯";
	AL["Shen'dralar Provisioner"] = "辛德拉聖職者";

	--Maraudon	
	AL["Elder Splitrock"] = "劈石長者";
	AL["Celebras the Redeemed"] = "贖罪的塞雷布拉斯";

	--Ragefire Chasm
	AL["Commander Bagran"] = "指揮官巴格仁";
	AL["Invoker Xorenth"] = "塑能師索倫斯";
	AL["Scout Cage"] = "斥侯牢籠";

	--Razorfen Downs
	AL["Koristrasza"] = "柯莉史卓莎";
	AL["Belnistrasz"] = "貝尼斯特拉茲";

	--Razorfen Kraul
	AL["Auld Stonespire"] = "奧爾德·石塔";
	AL["Spirit of Agamaggan <Ancient>"] = "阿迦瑪甘之靈 <先祖>";
	AL["Willix the Importer"] = "進口商威利克斯";

	--Ruins of Ahn'Qiraj
	AL["Four Kaldorei Elites"] = "四個卡多雷精英";
	AL["Captain Qeez"] = "奎茲上尉";
	AL["Captain Tuubid"] = "圖畢德上尉";
	AL["Captain Drenn"] = "德蘭上尉";
	AL["Captain Xurrem"] = "瑟瑞姆上尉";
	AL["Major Yeggeth"] = "葉吉斯少校";
	AL["Major Pakkon"] = "帕康少校";
	AL["Colonel Zerran"] = "澤朗上校";
	AL["Safe Room"] = "安全的空間";

	--Temple of Ahn'Qiraj
	AL["Andorgos <Brood of Malygos>"] = "安多葛斯 <瑪里苟斯的後裔>";
	AL["Vethsera <Brood of Ysera>"] = "溫瑟拉 <伊瑟拉的後裔>";
	AL["Kandrostrasz <Brood of Alexstrasza>"] = "坎多斯塔茲 <雅立史卓莎的後裔>";
	AL["Arygos"] = "亞雷戈斯";
	AL["Caelestrasz"] = "凱雷斯特拉茲";
	AL["Merithra of the Dream"] = "夢境之龍麥琳瑟拉";

	--Wailing Caverns
	AL["Ebru <Disciple of Naralex>"] = "厄布魯 <納拉雷克斯的侍徒>"; -- 5768
	AL["Nalpak <Disciple of Naralex>"] = "納爾派克 <納拉雷克斯的侍徒>"; -- 5767
	AL["Muyoh <Disciple of Naralex>"] = "繆幽 <納拉雷克斯的侍徒>";  -- 3678
	AL["Naralex"] = "納拉雷克斯"; -- 3679

	--Zul'Farrak
	AL["Chief Engineer Bilgewhizzle <Gadgetzan Water Co.>"] = "首席工程師膨嘯 <加基森水業公司>";
	AL["Mazoga's Spirit"] = "瑪柔伽的靈魂";
	AL["Tran'rek"] = "特蘭雷克";
	AL["Weegli Blastfuse"] = "維格利";
	AL["Raven"] = "拉文";
	AL["Elder Wildmane"] = "蠻鬃長者";

--****************************
-- Eastern Kingdoms Instances (Classic)
--****************************

	--Blackrock Depths
	AL["The Black Anvil"] = "黑鐵砧";
	AL["The Vault"] = "地窖";
	AL["Watchman Doomgrip"] = "衛兵杜格瑞普";
	AL["Elder Morndeep"] = "深晨長者";
	AL["Schematic: Field Repair Bot 74A"] = "結構圖:戰地修理機器人74A型";
	AL["Private Rocknot"] = "羅克諾特下士";
	AL["Mistress Nagmara"] = "娜瑪拉小姐";
	AL["Jalinda Sprig <Morgan's Militia>"] = "加琳達 <摩根的民兵>";
	AL["Oralius <Morgan's Militia>"] = "奧拉留斯 <摩根的民兵>";
	AL["Thal'trak Proudtusk <Kargath Expeditionary Force>"] = "薩特拉克·長齒 <卡加斯遠征軍>";
	AL["Galamav the Marksman <Kargath Expeditionary Force>"] = "『神射手』賈拉瑪弗 <卡加斯遠征軍>";
	AL["Maxwort Uberglint"] = "麥克斯沃特·尤柏格林";
	AL["Tinkee Steamboil"] = "丁奇·斯迪波爾";
	AL["Yuka Screwspigot <Engineering Supplies>"] = "尤卡·斯庫比格特 <工程學供應商>";
	AL["Abandonded Mole Machine"] = "棄置的鑽地機";
	AL["Kevin Dawson <Morgan's Militia>"] = "凱文·多森 <摩根的民兵>";
	AL["Lexlort <Kargath Expeditionary Force>"] = "雷克斯洛特 <卡加斯遠征軍>";
	AL["Prospector Seymour <Morgan's Militia>"] = "勘查員希摩爾 <摩根的民兵>";
	AL["Razal'blade <Kargath Expeditionary Force>"] = "拉札布雷德 <卡加斯遠征軍>";
	AL["The Shadowforge Lock"] = "暗爐之鎖";
	AL["Mayara Brightwing <Morgan's Militia>"] = "瑪亞拉·亮翼 <摩根的民兵>";
	AL["Hierophant Theodora Mulvadania <Kargath Expeditionary Force>"] = "祭師塞朵拉·穆瓦丹尼 <卡加斯遠征軍>";
	AL["Lokhtos Darkbargainer <The Thorium Brotherhood>"] = "羅克圖斯·暗契 <瑟銀兄弟會>";
	AL["Mountaineer Orfus <Morgan's Militia>"] = "巡山人歐弗斯 <摩根的民兵>";
	AL["Thunderheart <Kargath Expeditionary Force>"] = "桑德哈特 <卡加斯遠征軍>";
	AL["Marshal Maxwell <Morgan's Militia>"] = "麥斯威爾元帥 <摩根的民兵>";
	AL["Warlord Goretooth <Kargath Expeditionary Force>"] = "督軍高圖斯 <卡加斯遠征軍>";
	AL["The Black Forge"] = "黑熔爐";
	AL["Core Fragment"] = "熔核碎片";
	AL["Shadowforge Brazier"] = "暗爐火盆";

	--Blackrock Spire (Lower)
	AL["Urok's Tribute Pile"] = "烏洛克的貢品堆";
	AL["Acride <Scarshield Legion>"] = "裂盾滲透者 <裂盾軍團>";
	AL["Elder Stonefort"] = "石壘長者";
	AL["Roughshod Pike"] = "尖銳長矛";

	--Blackrock Spire (Upper)
	AL["Finkle Einhorn"] = "芬克·恩霍爾";
	AL["Drakkisath's Brand"] = "達基薩斯徽記";
	AL["Father Flame"] = "烈焰之父";

	--Blackwing Lair
	AL["Orb of Domination"] = "統禦寶珠";
	AL["Master Elemental Shaper Krixix"] = "大元素師克里希克";

	--Gnomeregan
	AL["Chomper"] = "咀嚼者";
	AL["Blastmaster Emi Shortfuse"] = "爆破專家艾米·短線";
	AL["Murd Doc <S.A.F.E.>"] = "哮·狼的護腿 <S.A.F.E.>";
	AL["Tink Sprocketwhistle <Engineering Supplies>"] = "丁克·鐵哨 <工程學供應商>";
	AL["The Sparklematic 5200"] = "超級清潔器5200型！";
	AL["Mail Box"] = "鎖甲箱";
	AL["B.E Barechus <S.A.F.E.>"] = "怪怪頭 <S.A.F.E.>";
	AL["Face <S.A.F.E.>"] = "小白臉 <S.A.F.E.>";
	AL["Hann Ibal <S.A.F.E.>"] = "漢·泥巴 <S.A.F.E.>";

	--Molten Core

	--Scarlet Halls
	AL["Commander Lindon"] = "指揮官林敦";
	AL["Hooded Crusader"] = "戴頭罩的十字軍";
	AL["Bucket of Meaty Dog Food"] = "一桶豐盛狗食";
	AL["Reinforced Archery Target"] = "強化箭靶";

	--Scarlet Monastery

	--Scholomance
	AL["Instructor Chillheart's Phylactery"] = "講師冷心的骨匣";
	AL["Professor Slate"] = "史雷特教授";
	AL["Polyformic Acid Potion"] = "變體蟻酸藥水";
	AL["Talking Skull"] = "說話的骨頭";
	AL["In the Shadow of the Light"] = "在聖光的陰影之中";
	AL["Kel'Thuzad's Deep Knowledge"] = "科爾蘇加德的深層知識";
	AL["Forbidden Rites and other Rituals Necromantic"] = "禁忌儀式與其他死靈儀式";
	AL["Coffer of Forgotten Souls"] = "遺忘靈魂的法櫃";
	AL["The Dark Grimoire"] = "闇黑魔典";

	--Shadowfang Keep 影牙城堡
	AL["Apothecary Trio"] = "藥劑師三人組";
	AL["Apothecary Hummel <Crown Chemical Co.>"] = "藥劑師胡默爾 <王冠化學製藥公司>";
	AL["Apothecary Baxter <Crown Chemical Co.>"] = "藥劑師巴克斯特 <王冠化學製藥公司>";
	AL["Apothecary Frye <Crown Chemical Co.>"] = "藥劑師弗萊伊 <王冠化學製藥公司>";
	AL["Packleader Ivar Bloodfang"] = "狼群首領伊瓦·血牙";
	AL["Deathstalker Commander Belmont"] = "亡靈哨兵指揮官貝爾蒙特";
	AL["Haunted Stable Hand"] = "鬼怪獸欄僕人";
	AL["Investigator Fezzen Brasstacks"] = "調查員菲贊·銅釘";

	--Stratholme - Crusader's Square
	AL["Crusade Commander Eligor Dawnbringer <Brotherhood of the Light>"] = "指揮官艾利格·黎明使者 <聖光兄弟會>";
	AL["Master Craftsman Wilhelm <Brotherhood of the Light>"] = "工匠大師維爾海姆 <聖光兄弟會>";
	AL["Packmaster Stonebruiser <Brotherhood of the Light>"] = "軍需籌備官石漢 <聖光兄弟會>";
	AL["Stratholme Courier"] = "斯坦索姆信差";
	AL["Fras Siabi's Postbox"] = "弗拉斯·希亞比的郵箱";
	AL["King's Square Postbox"] = "國王廣場郵箱";
	AL["Festival Lane Postbox"] = "節日小道郵箱";
	AL["Elder Farwhisper"] = "遙語長者";
	AL["Market Row Postbox"] = "市場郵箱";

	--Stratholme - The Gauntlet
	AL["Elders' Square Postbox"] = "長者廣場郵箱";
	AL["Archmage Angela Dosantos <Brotherhood of the Light>"] = "大法師安琪拉·多桑杜 <聖光兄弟會>";
	AL["Crusade Commander Korfax <Brotherhood of the Light>"] = "『聖光勇士』柯菲斯 <聖光兄弟會>";

	--The Deadmines
	AL["Lieutenant Horatio Laine"] = "何瑞修·萊恩中尉";
	AL["Kagtha"] = "卡格薩";
	AL["Slinky Sharpshiv"] = "史琳琪·利刀";
	AL["Quartermaster Lewis <Quartermaster>"] = "軍需官路易斯 <軍需官>";
	AL["Miss Mayhem"] = "暴行小姐";
	AL["Vend-O-Tron D-Luxe"] = "高級自動販賣機";

	--The Stockade
	AL["Rifle Commander Coe"] = "步槍指揮官寇伊";
	AL["Warden Thelwater"] = "典獄官塞爾沃特";
	AL["Nurse Lillian"] = "護士莉蓮";

	--The Sunken Temple
	AL["Lord Itharius"] = "伊薩里奧斯領主";
	AL["Elder Starsong"] = "星歌長者";

	--Uldaman
	AL["Baelog's Chest"] = "巴爾洛戈的箱子";
	AL["Kand Sandseeker <Explorer's League>"] = "坎德·覓沙 <探險者協會>";
	AL["Lead Prospector Durdin <Explorer's League>"] = "首席勘察員杜爾丁 <探險者協會>";
	AL["Olga Runesworn <Explorer's League>"] = "歐嘉·符誓 <探險者協會>";
	AL["Aoren Sunglow <The Reliquary>"] = "安歐連·日耀";
	AL["High Examiner Tae'thelan Bloodwatcher <The Reliquary>"] = "高階審查員泰瑟連·血腥看守者 <聖匣守護者>";
	AL["Lidia Sunglow <The Reliquary>"] = "莉蒂雅·日耀";
	AL["Ancient Treasure"] = "古代寶藏";
	AL["The Discs of Norgannon"] = "諾甘農圓盤";

--*******************
-- Burning Crusade Instances
--*******************

	--Auch: Auchenai Crypts
	AL["Draenei Spirit"] = "德萊尼靈魂";
	AL["Avatar of the Martyred"] = "馬丁瑞德的化身";
	AL["D'ore"] = "迪歐瑞";
	AL["Tormented Soulpriest"] = "受折磨的靈魂牧師";

	--Auch: Mana-Tombs
	AL["Artificer Morphalius"] = "工匠莫法利厄司";
	AL["Mamdy the \"Ologist\""] = "『學家』瑪姆迪";
	AL["Shadow Lord Xiraxis"] = "暗影領主希瑞西斯";
	AL["Ambassador Pax'ivi"] = "帕克西維大使";
	AL["Cryo-Engineer Sha'heen"] = "工程師薩希恩";
	AL["Ethereal Transporter Control Panel"] = "虛空傳送者控制面板";

	--Auch: Sethekk Halls
	AL["Isfar"] = "伊斯法";
	AL["Dealer Vijaad"] = "商人維傑";
	AL["Lakka"] = "拉卡";
	AL["The Saga of Terokk"] = "泰洛克的傳說";

	--Auch: Shadow Labyrinth
	AL["Field Commander Mahfuun"] = "戰場元帥瑪赫范";
	AL["Spy Grik'tha"] = "間諜葛瑞克莎";
	AL["The Codex of Blood"] = "血之聖典";
	AL["First Fragment Guardian"] = "第一碎片守衛者";
	AL["Spy To'gun"] = "間諜·吐剛";

	--Black Temple (Start)
	AL["Towards Reliquary of Souls"] = "通往靈魂聖盒";
	AL["Towards Teron Gorefiend"] = "通往泰朗·血魔";
	AL["Towards Illidan Stormrage"] = "通往伊利丹";
	AL["Spirit of Olum"] = "歐蘭的靈魂";
	AL["Spirit of Udalo"] = "烏達羅之靈";
	AL["Aluyen <Reagents>"] = "阿魯焰 <施法材料>";
	AL["Okuno <Ashtongue Deathsworn Quartermaster>"] = "歐庫諾 <灰舌死亡誓言者軍需官>";
	AL["Seer Kanai"] = "先知卡奈";

	--Black Temple (Basement)

	--Black Temple (Top)

	--CFR: Serpentshrine Cavern
	AL["Seer Olum"] = "先知歐蘭";

	--CFR: The Slave Pens
	AL["Nahuud"] = "納霍德";
	AL["Watcher Jhang"] = "看守者詹汗格";
	AL["Weeder Greenthumb"] = "威德·綠指";
	AL["Skar'this the Heretic"] = "異教徒司卡利斯";
	AL["Naturalist Bite"] = "自然學家拜特";

	--CFR: The Steamvault
	AL["Windcaller Claw"] = "喚風者卡勞";
	AL["Main Chambers Access Panel"] = "主房間通道面板";
	AL["Second Fragment Guardian"] = "第二碎片守衛者";

	--CFR: The Underbog
	AL["T'shu"] = "塔蘇";
	AL["The Underspore"] = "地孢";
	AL["Earthbinder Rayge"] = "縛地者瑞吉";

	--CoT: The Black Morass
	AL["Sa'at <Keepers of Time>"] = "塞特 <時光守望者>";

	--CoT: Hyjal Summit
	AL["Lady Jaina Proudmoore"] = "珍娜·普勞德摩爾女士";
	AL["Thrall <Warchief>"] = "索爾 <首領>";
	AL["Tyrande Whisperwind <High Priestess of Elune>"] = "泰蘭妲·語風 <伊露恩的高階女祭司>";

	--CoT: Old Hillsbrad Foothills
	AL["Erozion"] = "伊洛森";
	AL["Brazen"] = "布瑞茲恩";
	AL["Landing Spot"] = "降落點";
	AL["Thrall"] = "索爾";
	AL["Taretha"] = "塔蕾莎";
	AL["Don Carlos"] = "卡洛斯大爺";
	AL["Guerrero"] = "葛雷洛";
	AL["Thomas Yance <Travelling Salesman>"] = "湯瑪斯·陽斯 <旅行商人>";
	AL["Aged Dalaran Wizard"] = "年邁的達拉然法師";
	AL["Jonathan Revah"] = "強納森·瑞瓦";
	AL["Jerry Carter"] = "傑瑞·卡特";
	AL["Helcular"] = "赫爾庫拉";
	AL["Farmer Kent"] = "農夫肯特";
	AL["Sally Whitemane"] = "莎麗·白鬃";
	AL["Renault Mograine"] = "雷諾·莫根尼";
	AL["Little Jimmy Vishas"] = "小吉米·維希斯";
	AL["Herod the Bully"] = "流氓希洛特";
	AL["Nat Pagle"] = "納特·帕格";
	AL["Hal McAllister"] = "哈爾·馬克奧里斯特";
	AL["Zixil <Aspiring Merchant>"] = "吉克希爾 <高級商人>";
	AL["Overwatch Mark 0 <Protector>"] = "守候者零型 <保衛者>";
	AL["Southshore Inn"] = "南海鎮旅館";
	AL["Captain Edward Hanes"] = "隊長艾德華·漢尼斯";
	AL["Captain Sanders"] = "桑德斯船長";
	AL["Commander Mograine"] = "指揮官莫格萊尼";
	AL["Isillien"] = "伊斯利恩";
	AL["Abbendis"] = "阿比迪斯";
	AL["Fairbanks"] = "費爾班克";
	AL["Taelan"] = "泰蘭";
	AL["Barkeep Kelly <Bartender>"] = "酒吧老闆凱利 <酒保>";
	AL["Frances Lin <Barmaid>"] = "法蘭斯·林 <酒吧女服務員>";
	AL["Chef Jessen <Speciality Meat & Slop>"] = "廚師傑森 <特殊肉品和食物>";
	AL["Stalvan Mistmantle"] = "斯塔文·密斯特曼托";
	AL["Phin Odelic <The Kirin Tor>"] = "費恩·奧德利克 <祈倫托>";
	AL["Magistrate Henry Maleb"] = "赫尼·馬雷布鎮長";
	AL["Raleigh the True"] = "純真者洛歐欸";
	AL["Nathanos Marris"] = "納薩諾斯·瑪瑞斯";
	AL["Bilger the Straight-laced"] = "嚴厲者畢歐吉";
	AL["Innkeeper Monica"] = "旅店老闆莫妮卡";
	AL["Julie Honeywell"] = "喬莉·哈妮威爾";
	AL["Jay Lemieux"] = "杰·黎米厄斯";
	AL["Young Blanchy"] = "小馬布蘭契";

	--Gruul's Lair

	--HFC: The Blood Furnace
	AL["Gunny"] = "甘尼";
	AL["Caza'rez"] = "卡沙瑞斯";

	--HFC: Hellfire Ramparts
	AL["Advance Scout Chadwick"] = "先遣斥候查德威克";
	AL["Stone Guard Stok'ton"] = "石衛士史托克頓";
	AL["Reinforced Fel Iron Chest"] = "強化惡魔鐵箱";

	--HFC: Magtheridon's Lair

	--HFC: The Shattered Halls
	AL["Shattered Hand Executioner"] = "破碎之手劊子手";
	AL["Private Jacint"] = "士兵賈辛特";
	AL["Rifleman Brownbeard"] = "槍兵伯朗畢爾";
	AL["Captain Alina"] = "隊長阿蓮娜";
	AL["Scout Orgarr"] = "斥候歐卡爾";
	AL["Korag Proudmane"] = "科洛特·波特曼";
	AL["Captain Boneshatter"] = "隊長碎骨";
	AL["Randy Whizzlesprocket"] = "藍迪·威索洛克";
	AL["Drisella"] = "崔賽拉";

	--Karazhan Start
	AL["Baroness Dorothea Millstipe"] = "女爵朵洛希·米爾斯泰普";
	AL["Lady Catriona Von'Indi"] = "凱崔娜·瓦映迪女士";
	AL["Lady Keira Berrybuck"] = "凱伊拉·拜瑞巴克女士";
	AL["Baron Rafe Dreuger"] = "男爵洛夫·崔克爾";
	AL["Lord Robin Daris"] = "貴族羅賓·達利斯";
	AL["Lord Crispin Ference"] = "貴族克利斯平·費蘭斯";
	AL["Red Riding Hood"] = "小紅帽";
	AL["Wizard of Oz"] = "綠野仙蹤";
	AL["The Master's Terrace"] = "大師的露臺";
	AL["Servant Quarters"] = "伺從區";
	AL["Hastings <The Caretaker>"] = "哈斯丁 <照料者>";
	AL["Berthold <The Doorman>"] = "勃特霍德 <看門人>";
	AL["Calliard <The Nightman>"] = "卡利卡 <夜間工作者>";
	AL["Koren <The Blacksmith>"] = "卡爾侖 <鐵匠>";
	AL["Bennett <The Sergeant at Arms>"] = "班尼特 <待命中的中士>";
	AL["Keanna's Log"] = "琪安娜的日誌";
	AL["Ebonlocke <The Noble>"] = "埃伯洛克 <貴族>";
	AL["Sebastian <The Organist>"] = "塞巴斯汀 <風琴演奏家>";
	AL["Barnes <The Stage Manager>"] = "巴奈斯 <舞台管理員>";

	--Karazhan End
	AL["Path to the Broken Stairs"] = "通往損壞的階梯的通道";
	AL["Broken Stairs"] = "損壞的階梯";
	AL["Ramp to Guardian's Library"] = "通往管理員圖書館的斜坡";
	AL["Mysterious Bookshelf"] = "神秘的書架";
	AL["Ramp up to the Celestial Watch"] = "通往天文觀測台的斜坡";
	AL["Ramp down to the Gamesman's Hall"] = "通往投機者大廳的斜坡";
	AL["Ramp to Medivh's Chamber"] = "通往麥迪文房間的斜坡";
	AL["Spiral Stairs to Netherspace"] = "通往虛空空間的螺旋梯";
	AL["Wravien <The Mage>"] = "瑞依恩 <法師>";
	AL["Gradav <The Warlock>"] = "葛瑞戴 <術士>";
	AL["Kamsis <The Conjurer>"] = "康席斯 <咒術師>";
	AL["Ythyar"] = "伊斯亞爾";
	AL["Echo of Medivh"] = "麥迪文的回音";

	--Magisters Terrace
	AL["Exarch Larethor"] = "主教雷索爾";
	AL["Fel Crystals"] = "惡魔水晶";
	AL["Apoko"] = "阿波考";
	AL["Eramas Brightblaze"] = "依拉瑪·火光";
	AL["Ellrys Duskhallow"] = "艾爾里斯·聖暮";
	AL["Fizzle"] = "費索";
	AL["Garaxxas"] = "卡拉克薩斯";
	AL["Sliver <Garaxxas' Pet>"] = "割裂者 <卡拉克薩斯的寵物>";
	AL["Kagani Nightstrike"] = "卡嘉尼·夜擊";
	AL["Warlord Salaris"] = "督軍沙拉利思";
	AL["Yazzai"] = "耶賽";
	AL["Zelfan"] = "塞爾汎";
	AL["Tyrith"] = "提里斯";
	AL["Scrying Orb"] = "索蘭尼亞的占卜寶珠";

	--Sunwell Plateau
	AL["Madrigosa"] = "瑪德里茍沙";

	--TK: The Arcatraz
	AL["Millhouse Manastorm"] = "米歐浩斯·曼納斯頓";
	AL["Third Fragment Guardian"] = "第三碎片守衛者";
	AL["Udalo"] = "先知烏達羅";

	--TK: The Botanica

	--TK: The Mechanar
	AL["Overcharged Manacell"] = "滿溢的法力容器";

	--TK: The Eye

--*****************
-- Wrath of the Lich King Instances
--*****************

	--Azjol-Nerub: Ahn'kahet: The Old Kingdom
	AL["Seer Ixit"] = "先知伊須特";
	AL["Ahn'kahet Brazier"] = "安卡罕特火盆";

	--Azjol-Nerub: Azjol-Nerub
	AL["Reclaimer A'zak"] = "回收者阿札克";
	AL["Watcher Gashra"] = "看守者賈西拉";
	AL["Watcher Narjil"] = "看守者納吉爾";
	AL["Watcher Silthik"] = "看守者席爾希克";
	AL["Elder Nurgen"] = "訥金長者";

	--Caverns of Time: The Culling of Stratholme
	AL["The Culling of Stratholme"] = "斯坦索姆的抉擇";
	AL["Scourge Invasion Points"] = "天譴軍團地點";
	AL["Guardian of Time"] = "時光守護者";
	AL["Chromie"] = "克羅米";

	--Drak'Tharon Keep
	AL["Image of Drakuru"] = "德拉庫魯的影像";
	AL["Kurzel"] = "庫賽爾";
	AL["Elder Kilias"] = "奇里亞斯長者";
	AL["Drakuru's Brazier"] = "德拉庫魯的火盆";

	--The Frozen Halls: Halls of Reflection
	--3 beginning NPCs omitted, see The Forge of Souls
	AL["The Captain's Chest"] = "船長的箱子";

	--The Frozen Halls: Pit of Saron
	--6 beginning NPCs omitted, see The Forge of Souls
	AL["Martin Victus"] = "馬汀·維特斯";
	AL["Gorkun Ironskull"] = "葛剛·鐵顱";
	AL["Rimefang"] = "霜牙";

	--The Frozen Halls: The Forge of Souls
	--Lady Jaina Proudmoore omitted, in Hyjal Summit
	AL["Archmage Koreln <Kirin Tor>"] = "大法師寇瑞倫 <祈倫托>";
	AL["Archmage Elandra <Kirin Tor>"] = "大法師伊蘭卓 <祈倫托>";
	AL["Lady Sylvanas Windrunner <Banshee Queen>"] = "希瓦娜斯·風行者女士 <女妖之王>";
	AL["Dark Ranger Loralen"] = "黑暗遊俠洛拉倫";
	AL["Dark Ranger Kalira"] = "黑暗遊俠卡麗菈";

	--Gundrak
	AL["Chronicler Bah'Kini"] = "撰史者巴琪妮";
	AL["Tol'mar"] = "托爾瑪";
	AL["Elder Ohanzee"] = "歐漢茲長者";

	--Icecrown Citadel
	AL["To next map"] = "到下一個地圖";
	AL["From previous map"] = "到前一個地圖";
	AL["Upper Spire"] = "冰冠尖塔";
	AL["Sindragosa's Lair"] = "辛德拉苟莎之巢";
	AL["Stinky"] = "臭皮";
	AL["Precious"] = "普萊瑟斯";
	AL["Rimefang"] = "霜牙";	-- NPC: 37533
	AL["Spinestalker"] = "骨脊潛獵者";	-- NPC: 37534
	AL["Sister Svalna"] = "絲瓦娜姐妹";	-- NPC: 37126

	--Naxxramas
	AL["Mr. Bigglesworth"] = "畢勾沃斯先生";
	AL["Frostwyrm Lair"] = "冰霜巨龍的巢穴";
	AL["Teleporter to Middle"] = "傳送到中間";

	--The Obsidian Sanctum
	AL["Black Dragonflight Chamber"] = "黑龍軍團密室";

	--Onyxia's Lair

	--The Ruby Sanctum
	AL["Red Dragonflight Chamber"] = "紅龍軍團密室";

	--The Nexus: The Eye of Eternity

	--The Nexus: The Nexus
	AL["Warmage Kaitlyn"] = "戰爭法師凱特林";
	AL["Berinand's Research"] = "貝瑞那德的研究";
	AL["Elder Igasho"] = "伊加修長者";

	--The Nexus: The Oculus
	AL["Belgaristrasz"] = "貝加瑞斯塔茲";
	AL["Eternos"] = "伊特諾斯";
	AL["Verdisa"] = "薇爾迪莎";
	AL["Centrifuge Construct"] = "離心傀儡";
	AL["Cache of Eregos"] = "伊瑞茍斯的貯藏箱";	

	--Trial of the Champion
	AL["Marshal Jacob Alerius"] = "傑科布·亞雷瑞斯元帥";
	AL["Ambrose Boltspark"] = "安布羅斯·拴炫";
	AL["Colosos"] = "克羅索斯";
	AL["Jaelyne Evensong"] = "潔琳·晚歌";
	AL["Lana Stouthammer"] = "菈娜·頑錘";

	--Trial of the Crusader
	AL["Heroic: Trial of the Grand Crusader"] = "英雄: 大十字軍試煉";
	AL["Cavern Entrance"] = "洞穴入口";

	--Ulduar General
	AL["The Siege"] = "攻城區";
	AL["The Keepers"] = "守護者";

	--Ulduar A
	AL["Tower of Life"] = "生命之塔";
	AL["Tower of Flame"] = "烈焰之塔";
	AL["Tower of Frost"] = "冰霜之塔";
	AL["Tower of Storms"] = "風暴之塔";

	--Ulduar B
	AL["Prospector Doren"] = "勘察員多倫";
	AL["Archivum Console"] = "大資料庫控制臺";

	--Ulduar C
	AL["Sif"] = "希芙";

	--Ulduar D

	--Ulduar E

	--Ulduar: Halls of Lightning
	AL["Stormherald Eljrrin"] = "風暴信使埃利林";

	--Ulduar: Halls of Stone
	AL["Kaldir Ironbane"] = "卡迪爾·鐵禍";
	AL["Tribunal Chest"] = "議庭之箱";
	AL["Elder Yurauk"] = "由羅克長者";	
	AL["Brann Bronzebeard"] = "布萊恩·銅鬚";

	--Utgarde Keep: Utgarde Keep
	AL["Defender Mordun"] = "防衛者摩丹";
	AL["Dark Ranger Marrah"] = "黑暗遊俠瑪拉";
	AL["Elder Jarten"] = "加坦長者";

	--Utgarde Keep: Utgarde Pinnacle
	AL["Brigg Smallshanks"] = "布里格·細柄";
	AL["Image of Argent Confessor Paletress"] = "銀白告解者帕爾璀絲的影像";
	AL["Elder Chogan'gada"] = "修干加達長者";

	--Vault of Archavon

	--The Violet Hold
	AL["Lieutenant Sinclari"] = "辛克拉麗中尉";

--*********************
-- Cataclysm Instances
--*********************

	--Baradin Hold

	--Blackrock Caverns

	--Blackwing Descent

	--Caverns of Time: Dragon Soul
	AL["Dasnurimi <Geologist & Conservator>"] = "達斯魯黎米 <地理學家與護存者>";
	AL["Lord Afrasastrasz"] = "艾弗薩斯塔茲領主";

	--Caverns of Time: End Time
	AL["Alurmi"] = "阿勒米";
	AL["Nozdormu"] = "諾茲多姆";

	--Caverns of Time: Hour of Twilight

	--Caverns of Time: Well of Eternity

	--Firelands
	AL["Lurah Wrathvine <Crystallized Firestone Collector>"] = "盧拉·怒藤 <晶化火石收集者>";
	AL["Naresir Stormfury <Avengers of Hyjal Quartermaster>"] = "那瑞希爾·風暴之怒 <海加爾復仇者軍需官>";

	--Grim Batol
	AL["Baleflame"] = "罪火";
	AL["Farseer Tooranu <The Earthen Ring>"] = "先知圖拉奴 <陶土議會>";
	AL["Velastrasza"] = "維菈史卓莎";

	--Halls of Origination
	AL["Large Stone Obelisk"] = "大型石板";

	--Lost City of the Tol'vir
	AL["Captain Hadan"] = "哈丹隊長";
	AL["Tol'vir Grave"] = "托維爾墓地";

	--The Bastion of Twilight

	--The Stonecore
	AL["Earthwarden Yrsa <The Earthen Ring>"] = "大地守望者伊爾薩 <陶土議會>";

	--The Vortex Pinnacle
	AL["Itesh"] = "伊塔許";
	AL["Magical Brazier"] = "魔法火盆";

	--Throne of the Four Winds

	--Throne of the Tides
	AL["Captain Taylor"] = "泰勒隊長";
	AL["Legionnaire Nazgrim"] = "軍團士兵納茲格寧姆";
	AL["Neptulon"] = "奈普圖隆";

	--Zul'Aman
	AL["Vol'jin"] = "沃金";
	AL["Witch Doctor T'wansi"] = "巫醫塔灣西";
	AL["Blood Guard Hakkuz <Darkspear Elite>"] = "血衛士哈庫茲 <暗矛精英>";
	AL["Voodoo Pile"] = "巫毒堆";
	AL["Bakkalzu"] = "巴卡祖";
	AL["Hazlek"] = "哈茲雷克";
	AL["The Map of Zul'Aman"] = "祖阿曼地圖";
	AL["Norkani"] = "諾卡尼";
	AL["Kasha"] = "卡沙";
	AL["Thurg"] = "瑟吉";
	AL["Gazakroth"] = "葛薩克羅司";
	AL["Lord Raadan"] = "領主雷阿登";
	AL["Darkheart"] = "黑心";
	AL["Alyson Antille"] = "艾利森·安第列";
	AL["Slither"] = "史立塞";
	AL["Fenstalker"] = "沼群巡者";
	AL["Koragg"] = "可拉格";
	AL["Zungam"] = "祖剛";
	AL["Forest Frogs"] = "森林樹蛙";
	AL["Eulinda <Reagents>"] = "尤琳達 <施法材料>";
	AL["Harald <Food Vendor>"] = "哈拉德 <食物商人>";
	AL["Arinoth"] = "阿瑞諾斯";
	AL["Kaldrick"] = "卡爾崔克";
	AL["Lenzo"] = "蘭佐";
	AL["Mawago"] = "瑪哇苟";
	AL["Melasong"] = "馬拉頌";
	AL["Melissa"] = "梅麗莎";
	AL["Micah"] = "米迦";
	AL["Relissa"] = "瑞麗莎";
	AL["Rosa"] = "羅莎";
	AL["Tyllan"] = "泰倫";

	--Zul'Gurub
	AL["Briney Boltcutter <Blackwater Financial Interests>"] = "布蘭尼·破壞剪 <黑水金融>";
	AL["Vehini <Assault Provisions>"] = "維希尼 <突襲物資供應者>";
	AL["Overseer Blingbang"] = "監督者閃砰";
	AL["Bloodslayer T'ara <Darkspear Veteran>"] = "血腥殺戮者特亞拉 <暗矛精兵>";
	AL["Bloodslayer Vaena <Darkspear Veteran>"] = "血腥殺戮者瓦那 <暗矛精兵>";
	AL["Bloodslayer Zala <Darkspear Veteran>"] = "血腥殺戮者札拉 <暗矛精兵>";
	AL["Helpful Jungle Monkey"] = "好幫手叢林猴";
	AL["Venomancer Mauri <The Snake's Whisper>"] = "怨毒法師莫里 <蛇之耳語>";
	AL["Zanzil's Cauldron of Toxic Torment"] = "贊吉爾的毒物折磨大鍋";
	AL["Tiki Lord Mu'Loa"] = "提基王穆羅亞";
	AL["Gub <Destroyer of Fish>"] = "古布 <魚類滅殺者>";
	AL["Venomancer T'Kulu <The Toxic Bite>"] = "怨毒法師堤庫魯 <毒咬>";
	AL["Tor-Tun <The Slumberer>"] = "托通 <沉睡者>";
	AL["Kaulema the Mover"] = "移石者考勒瑪";
	AL["Berserking Boulder Roller"] = "狂暴巨礫滾動者";
	AL["Zanzil's Cauldron of Frostburn Formula"] = "贊吉爾的霜燃配方";
	AL["Mor'Lek the Dismantler"] = "拆卸人摩勒克";
	AL["Witch Doctor Qu'in <Medicine Woman>"] = "巫醫枯因 <女巫醫>";
	AL["Zanza the Restless"] = "『無眠者』贊札";
	AL["Mortaxx <The Tolling Bell>"] = "莫爾塔克斯 <鐘鳴者>";
	AL["Tiki Lord Zim'wae"] = "提基王辛瓦";
	AL["Zanzil's Cauldron of Burning Blood"] = "贊吉爾的燃燒之血";

--*********************
-- Mists of Pandaria Instances
--*********************

	--Gate of the Setting Sun
	AL["Bowmistress Li <Guard Captain>"] = "女弓手李 <守衛隊長>";

	--Heart of Fear

	--Mogu'shan Palace
	AL["Sinan the Dreamer"] = "『夢旅者』司南";

	--Mogu'shan Vaults

	--Shado-Pan Monastery
	AL["Ban Bearheart"] = "班·熊心";

	--Siege of Niuzao Temple
	AL["Shado-Master Chum Kiu"] = "影潘宗師成樵";

	--Siege of Orgrimmar

	--Stormstout Brewery
	AL["Auntie Stormstout"] = "風暴烈酒姑媽";
	AL["Chen Stormstout"] = "老陳·風暴烈酒";

	--Temple of the Jade Serpent
	AL["Master Windstrong"] = "風強大師";
	AL["Priestess Summerpetal"] = "女司祭夏瓣";

	--Terrace of Endless Spring

	--Throne of Thunder
	AL["Monara <The Last Queen>"] = "魔娜菈 <魔古的末代皇后>";
	AL["No'ku Stormsayer <Lord of Tempest>"] = "諾庫·風暴預言者 <風暴之王>";
	AL["Rocky Horror"] = "磐石駭獸";
	AL["Focused Eye"] = "集束之眼";
	AL["Unblinking Eye"] = "堅定無畏之眼";
	AL["Archritualist Kelada"] = "大祭儀師凱烈德";
	AL["Flesh'rok the Diseased <Primordial Saurok Horror>"] = "『瘟疫獸』血腐洛克 <原生薩烏洛克的恐怖>";
	AL["Zao'cho <The Emperor's Shield>"] = "趙仇 <帝王之盾>";

end
