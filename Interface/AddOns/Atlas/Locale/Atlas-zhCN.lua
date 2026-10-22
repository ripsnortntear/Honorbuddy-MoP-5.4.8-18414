-- $Id: Atlas-zhCN.lua 2120 2013-09-16 16:12:39Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas", "zhCN", false);

if ( GetLocale() == "zhCN" ) then
-- Define the leading strings to be ignored while sorting
-- Ex: The Stockade
AtlasSortIgnore = {};

-- Syntax: ["real_zone_name"] = "localized map zone name"
AtlasZoneSubstitutions = {
	["Ahn'Qiraj"] = "安其拉神殿";
	["The Temple of Atal'Hakkar"] = "阿塔哈卡神庙";
};
end


if AL then
--************************************************
-- UI terms and common strings
--************************************************
	AL["ATLAS_TITLE"] = "Atlas";

	AL["BINDING_HEADER_ATLAS_TITLE"] = "Atlas 绑定";
	AL["BINDING_NAME_ATLAS_TOGGLE"] = "切换 Atlas";
	AL["BINDING_NAME_ATLAS_OPTIONS"] = "切换选项";
	AL["BINDING_NAME_ATLAS_AUTOSEL"] = "自动选择";

	AL["ATLAS_SLASH"] = "/atlas";
	AL["ATLAS_SLASH_OPTIONS"] = "options";

	AL["ATLAS_STRING_LOCATION"] = "区域";
	AL["ATLAS_STRING_LEVELRANGE"] = "等级跨度";
	AL["ATLAS_STRING_RECLEVELRANGE"] = "建议等级"; -- abbrevation and shorten of "Recommended Level Range", the dungeon's recommended level range
	AL["ATLAS_STRING_PLAYERLIMIT"] = "人数上限";
	AL["ATLAS_STRING_SELECT_CAT"] = "选择分类";
	AL["ATLAS_STRING_SELECT_MAP"] = "选择地图";
	AL["ATLAS_STRING_SEARCH"] = "搜索";
	AL["ATLAS_STRING_CLEAR"] = "重置";
	AL["ATLAS_STRING_MINLEVEL"] = "需要等级";

	AL["ATLAS_OPTIONS_BUTTON"] = "选项";
	AL["ATLAS_OPTIONS_SHOWBUT"] = "在小地图周围显示图标";
	AL["ATLAS_OPTIONS_SHOWBUT_TIP"] = "在小地图旁显示 Atlas 按钮。";
	AL["ATLAS_OPTIONS_AUTOSEL"] = "自动选择副本地图";
	AL["ATLAS_OPTIONS_AUTOSEL_TIP"] = "Atlas 可侦测您目前所在的副区域以判定您所在的副本，开启 Atlas 时将会自动选择到该副本地图。";
	AL["ATLAS_OPTIONS_BUTPOS"] = "图标位置";
	AL["ATLAS_OPTIONS_LOCK"] = "锁定 Atlas 窗口";
	AL["ATLAS_OPTIONS_LOCK_TIP"] = "切换锁定/解锁 Atlas 窗口。";
	AL["ATLAS_OPTIONS_TRANS"] = "透明度";
	AL["ATLAS_OPTIONS_RCLICK"] = "右击打开世界地图";
	AL["ATLAS_OPTIONS_RCLICK_TIP"] = "在 Atlas 窗口中右击自动切换到魔兽的世界地图。";
	AL["ATLAS_OPTIONS_RESETPOS"] = "重置位置";
	AL["ATLAS_OPTIONS_ACRONYMS"] = "显示简称";
	AL["ATLAS_OPTIONS_ACRONYMS_TIP"] = "在地图的详尽叙述中显示副本的缩写。";
	AL["ATLAS_OPTIONS_SCALE"] = "缩放";
	AL["ATLAS_OPTIONS_BOSS_DESC"] = "当首领信息可获取时, 显示该信息";
	AL["ATLAS_OPTIONS_BOSS_DESC_TIP"] = "当鼠标光标移动到地图上首领的标号时, 并且首领信息可获取时, 显示该首领的相关信息.";
	AL["ATLAS_OPTIONS_BOSS_DESC_SCALE"] = "首领信息提示窗口大小比率";
	AL["ATLAS_OPTIONS_BUTRAD"] = "图标半径";
	AL["ATLAS_OPTIONS_CLAMPED"] = "不超出游戏画面";
	AL["ATLAS_OPTIONS_CLAMPED_TIP"] = "使 Atlas 窗口被拖曳时不会超出游戏主画面的边界, 关闭此选项则可将 Atlas 窗口拖曳并超出游戏画面边界。";
	AL["ATLAS_OPTIONS_CTRL"] = "按下 Ctrl 显示弹出工具说明";
	AL["ATLAS_OPTIONS_CTRL_TIP"] = "勾选后当鼠标移到地图信息字段时，按下 Ctrl 控制键，则会将信息的完整信息以提示型态显示。当信息过长而被截断时很有用。";
	AL["ATLAS_OPTIONS_DONTSHOWAGAIN"] = "不再显示相同信息。";
	AL["ATLAS_OPTIONS_CHECKMODULE"] = "提醒我是否有遗失的模块或插件";
	AL["ATLAS_OPTIONS_CHECKMODULE_TIP"] = "勾选以在每次登入 WoW 时检查是否有遗失的 Atlas 模块或插件。";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN"] = "副本列表以难易度色彩显示";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN_TIP"] = "依据副本建议的最低进入等级、以及玩家现今等级的差异，将副本列表以难易度色彩显示。";

	AL["ATLAS_BUTTON_CLOSE"] = "关闭";
	AL["ATLAS_BUTTON_TOOLTIP_TITLE"] = "Atlas";
	AL["ATLAS_BUTTON_TOOLTIP_HINT"] = "单击打开 Atlas。\n中键单击打开 Atlas 选项。\n右击可移动这个按钮。";
	AL["ATLAS_LDB_HINT"] = "单击打开 Atlas。\n右击打开 Atlas 选项。";
	AL["ATLAS_MINIMAPLDB_HINT"] = "单击打开 Atlas。\n右击打开 Atlas 选项。\n单击並拖拉可移动这个按钮。";

	AL["ATLAS_OPTIONS_CATDD"] = "副本地图排序方式：";
	AL["ATLAS_DDL_CONTINENT"] = "所属大陆";
	AL["ATLAS_DDL_CONTINENT_EASTERN"] = "东部王国副本";
	AL["ATLAS_DDL_CONTINENT_KALIMDOR"] = "卡利姆多副本";
	AL["ATLAS_DDL_CONTINENT_OUTLAND"] = "外域副本";
	AL["ATLAS_DDL_CONTINENT_NORTHREND"] = "诺森德副本";
	AL["ATLAS_DDL_CONTINENT_DEEPHOLM"] = "深岩之洲副本";
	AL["ATLAS_DDL_CONTINENT_PANDARIA"] = "潘达利亚副本";
	AL["ATLAS_DDL_LEVEL"] = "等级";
	AL["ATLAS_DDL_LEVEL_UNDER45"] = "45 级以下副本";
	AL["ATLAS_DDL_LEVEL_45TO60"] = "45-60 级副本";
	AL["ATLAS_DDL_LEVEL_60TO70"] = "60-70 级副本";
	AL["ATLAS_DDL_LEVEL_70TO80"] = "70-80 级副本";
	AL["ATLAS_DDL_LEVEL_80TO85"] = "80-85 级副本";
	AL["ATLAS_DDL_LEVEL_85TO90"] = "85-90 级以副本";
	AL["ATLAS_DDL_LEVEL_90PLUS"] = "90 级以上副本";
	AL["ATLAS_DDL_PARTYSIZE"] = "副本规模";
	AL["ATLAS_DDL_PARTYSIZE_5_AE"] = "5 人副本 1/3";
	AL["ATLAS_DDL_PARTYSIZE_5_FS"] = "5 人副本 2/3";
	AL["ATLAS_DDL_PARTYSIZE_5_TZ"] = "5 人副本 3/3";
	AL["ATLAS_DDL_PARTYSIZE_10_AN"] = "10 人副本 1/2";
	AL["ATLAS_DDL_PARTYSIZE_10_OZ"] = "10 人副本 2/2";
	AL["ATLAS_DDL_PARTYSIZE_20TO40"] = "20-40 人副本 1/2";
	AL["ATLAS_DDL_PARTYSIZE_20TO40IZ"] = "20-40 人副本 2/2";
	AL["ATLAS_DDL_EXPANSION"] = "资料片";	-- Sort Instance Maps by: Expansion
	AL["ATLAS_DDL_EXPANSION_OLD_AO"] = "旧世界副本 (第一页)";
	AL["ATLAS_DDL_EXPANSION_OLD_PZ"] = "旧世界副本 (第二页)";
	AL["ATLAS_DDL_EXPANSION_BC"] = "《燃烧的远征》副本";
	AL["ATLAS_DDL_EXPANSION_WOTLK"] = "《巫妖王之怒》副本";
	AL["ATLAS_DDL_EXPANSION_CATA"] = "《大地的裂变》副本";
	AL["ATLAS_DDL_EXPANSION_MOP"] = "《熊猫人之谜》副本";
	AL["ATLAS_DDL_TYPE"] = "类型";
	AL["ATLAS_DDL_TYPE_INSTANCE_AB"] = "副本 1/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_CF"] = "副本 2/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_GM"] = "副本 3/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_NS"] = "副本 4/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_TZ"] = "副本 5/5";
	AL["ATLAS_DDL_TYPE_ENTRANCE"] = "入口";

	AL["ATLAS_INSTANCE_BUTTON"] = "副本";
	AL["ATLAS_ENTRANCE_BUTTON"] = "入口";
	AL["ATLAS_SEARCH_UNAVAIL"] = "搜索不可用";

	AL["ATLAS_DEP_MSG1"] = "检测到过期的 Atlas 扩展插件。";
	AL["ATLAS_DEP_MSG2"] = "这些插件已经被禁用。";
	AL["ATLAS_DEP_MSG3"] = "请从插件目录（AddOns）中将其删除。";
	AL["ATLAS_DEP_OK"] = "确定";

	AL["ATLAS_INFO"] = "Atlas 信息";
	AL["ATLAS_INFO_12200"] = "重要提示：\n\n由于增加插件文件大小的关注，我们移出\n部分地下城地图和内置插件到单独模块。\n\n用户可以从各大知名的游戏站点下载我们\n的插件可能只包含了 Atlas 核心功能以及\n大灾变地图。\n\n用户如果想下载全部旧地下城地图和全部\n我们制作的 Atlas 其他模块需要单独下载。\n\n阅读更多信息在下面的论坛主题：\nhttp://www.atlasmod.com/phpBB3/viewtopic.php?t=1522";
	AL["ATLAS_INFO_12201"] = "我们最近新增了一个新的 Atlas 插件 - |cff6666ffAtlas 情景战役|cffffffff，用以提供 WoW 5.0 \n起新增的情景战役的地图。\n\n请详见我们的网站以取得更详细的信息，并请记得分别下载并安装此插件。\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";

	AL["ATLAS_MISSING_MODULE"] = "Atlas 已侦测到遗失的模块/插件：";

--************************************************
-- Zone Names, Acronyms, and Common Strings
--************************************************

	--Common strings
	AL["East"] = "东";
	AL["North"] = "北";
	AL["South"] = "南";
	AL["West"] = "西";

	--World Events, Festival
	AL["Brewfest"] = "美酒节";
	AL["Hallow's End"] = "万圣节";
	AL["Love is in the Air"] = "情人节";
	AL["Lunar Festival"] = "春节庆典";
	AL["Midsummer Festival"] = "仲夏火焰节";

	--Misc strings
	AL["Colon"] = "："; -- The colon symbol to be used in string, ex: "Zone: Firelands
	AL["Adult"] = "成年";
	AL["AKA"] = "亦作";
	AL["Arcane Container"] = "奥术容器";
	AL["Arms Warrior"] = "武器战士";
	AL["Attunement Required"] = "需要完成入口任务";
	AL["Back"] = "后门";
	AL["Basement"] = "底层";
	AL["Blacksmithing Plans"] = "锻造设计图";
	AL["Chase Begins"] = "追捕开始";
	AL["Chase Ends"] = "追捕结束";
	AL["Child"] = "幼年";
	AL["Click to open Dungeon Journal window."] = "单击打开地下城手册窗口。";
	AL["Connection"] = "通道";
	AL["Elevator"] = "升降梯";
	AL["End"] = "尾部";
	AL["Engineer"] = "工程师";
	AL["Entrance"] = "入口";
	AL["Event"] = "事件";
	AL["Exalted"] = "崇拜";
	AL["Exit"] = "出口";
	AL["Fourth Stop"] = "第四次止步";
	AL["Front"] = "前门";
	AL["Ghost"] = "幽灵";
	AL["Graveyard"] = "墓地";
	AL["Heroic"] = "英雄模式";
	AL["Holy Paladin"] = "神圣圣骑士";
	AL["Holy Priest"] = "神圣牧师";
	AL["Hunter"] = "猎人";
	AL["Imp"] = "小鬼";
	AL["Key"] = "钥匙";
	AL["Lower"] = "下层";
	AL["Mage"] = "法师";
	AL["Meeting Stone"] = "集合石";
	AL["Middle"] = "中间";
	AL["Monk"] = "僧侣";
	AL["Moonwell"] = "月亮井";
	AL["Optional"] = "可跳过";
	AL["Orange"] = "橙色";
	AL["Outside"] = "室外";
	AL["Paladin"] = "圣骑士";
	AL["Portal"] = "传送";
	AL["Priest"] = "牧师";
	AL["Protection Warrior"] = "防护战士";
	AL["Purple"] = "紫色";
	AL["Random"] = "随机";
	AL["Rare"] = "稀有";
	AL["Reputation"] = "阵营";
	AL["Repair"] = "修理";
	AL["Retribution Paladin"] = "惩戒圣骑士";
	AL["Rewards"] = "奖励";
	AL["Rogue"] = "潜行者";
	AL["Second Stop"] = "第二次止步";
	AL["Shadow Priest"] = "暗影牧师";
	AL["Shaman"] = "萨满祭司";
	AL["Spawn Point"] = "刷新点";
	AL["Start"] = "起始";
	AL["Summon"] = "召唤";
	AL["Teleporter"] = "传送";
	AL["Teleporter destination"] = "传送目的地";
	AL["Third Stop"] = "第三次止步";
	AL["Top"] = "顶层";
	AL["Tunnel"] = "通道";
	AL["Underwater"] = "水下";
	AL["Upper"] = "上层";
	AL["Varies"] = "多个位置";
	AL["Wanders"] = "游荡";
	AL["Warlock"] = "术士";
	AL["Warrior"] = "战士";
	AL["Wave 5"] = "第5波";
	AL["Wave 6"] = "第6波";
	AL["Wave 10"] = "第10波";
	AL["Wave 12"] = "第12波";
	AL["Wave 18"] = "第18波";
	AL["MapsNotFound"] = "当前选定的地下城没有 \n与之相对应的地图图像。 \n\n请确认已经安装 \n相应的 Atlas 地图模块。";

	--Classic Acronyms
	AL["AQ"] = "AQ"; -- Ahn'Qiraj
	AL["AQ20"] = "AQ20"; -- Ruins of Ahn'Qiraj
	AL["AQ40"] = "AQ40"; -- Temple of Ahn'Qiraj
	AL["BFD"] = "BFD"; -- Blackfathom Deeps
	AL["BRD"] = "BRD"; -- Blackrock Depths
	AL["BRM"] = "BRM"; -- Blackrock Mountain
	AL["BWL"] = "BWL"; -- Blackwing Lair
	AL["DM"] = "DM"; -- Dire Maul
	AL["Gnome"] = "Gnome"; -- Gnomeregan
	AL["Halls"] = "Halls"; -- Scarlet Halls
	AL["LBRS"] = "LBRS"; -- Lower Blackrock Spire
	AL["Mara"] = "Mara"; -- Maraudon
	AL["MC"] = "MC"; -- Molten Core
	AL["RFC"] = "RFC"; -- Ragefire Chasm
	AL["RFD"] = "RFD"; -- Razorfen Downs
	AL["RFK"] = "RFK"; -- Razorfen Kraul
	AL["Scholo"] = "Scholo"; -- Scholomance
	AL["SFK"] = "SFK"; -- Shadowfang Keep
	AL["SM"] = "SM"; -- Scarlet Monastery
	AL["ST"] = "ST"; -- Sunken Temple
	AL["Strat"] = "Strat"; -- Stratholme
	AL["Stocks"] = "Stocks"; -- The Stockade
	AL["UBRS"] = "UBRS"; -- Upper Blackrock Spire
	AL["Ulda"] = "Ulda"; -- Uldaman
	AL["VC"] = "VC"; -- The Deadmines
	AL["WC"] = "WC"; -- Wailing Caverns
	AL["ZF"] = "ZF"; -- Zul'Farrak

	--BC Acronyms
	AL["AC"] = "AC"; -- Auchenai Crypts
	AL["Arca"] = "Arca"; -- The Arcatraz
	AL["Auch"] = "Auch"; -- Auchindoun
	AL["BF"] = "BF"; -- The Blood Furnace
	AL["BT"] = "BT"; -- Black Temple
	AL["Bota"] = "Bota"; -- The Botanica
	AL["CoT"] = "CoT"; -- Caverns of Time
	AL["CoT1"] = "CoT1"; -- Old Hillsbrad Foothills
	AL["CoT2"] = "CoT2"; -- The Black Morass
	AL["CoT3"] = "CoT3"; -- Hyjal Summit
	AL["CR"] = "CR"; -- Coilfang Reservoir
	AL["GL"] = "GL"; -- Gruul's Lair
	AL["HC"] = "HC"; -- Hellfire Citadel
	AL["Kara"] = "Kara"; -- Karazhan
	AL["MaT"] = "MT"; -- Magisters' Terrace
	AL["Mag"] = "Mag"; -- Magtheridon's Lair
	AL["Mech"] = "Mech"; -- The Mechanar
	AL["MT"] = "MT"; -- Mana-Tombs
	AL["Ramp"] = "Ramp"; -- Hellfire Ramparts
	AL["SSC"] = "SSC"; -- Serpentshrine Cavern
	AL["Seth"] = "Seth"; -- Sethekk Halls
	AL["SH"] = "SH"; -- The Shattered Halls
	AL["SL"] = "SL"; -- Shadow Labyrinth
	AL["SP"] = "SP"; -- The Slave Pens
	AL["SuP"] = "SP"; -- Sunwell Plateau
	AL["SV"] = "SV"; -- The Steamvault
	AL["TK"] = "TK"; -- Tempest Keep
	AL["UB"] = "UB"; -- The Underbog

	--WotLK Acronyms
	AL["AK, Kahet"] = "AK, 安卡"; -- Ahn'kahet
	AL["AN, Nerub"] = "AN, 艾卓"; -- Azjol-Nerub
	AL["Champ"] = "Champ, 试炼"; -- Trial of the Champion
	AL["CoT-Strat"] = "CoT-Strat"; -- Culling of Stratholme
	AL["Crus"] = "Crus"; -- Trial of the Crusader
	AL["DTK"] = "DTK"; -- Drak'Tharon Keep
	AL["FoS"] = "FoS"; -- The Forge of Souls
	AL["FH1"] = "FH1"; -- The Forge of Souls
	AL["Gun"] = "Gun"; -- Gundrak
	AL["HoL"] = "HoL"; -- Halls of Lightning
	AL["HoR"] = "HoR"; -- Halls of Reflection
	AL["FH3"] = "FH3"; -- Halls of Reflection
	AL["HoS"] = "HoS"; -- Halls of Stone
	AL["IC"] = "IC"; -- Icecrown Citadel
	AL["Nax"] = "Nax"; -- Naxxramas
	AL["Nex, Nexus"] = "Nex, Nexus"; -- The Nexus
	AL["Ocu"] = "Ocu"; -- The Oculus
	AL["Ony"] = "Ony"; -- Onyxia's Lair
	AL["OS"] = "OS"; -- The Obsidian Sanctum
	AL["PoS"] = "PoS"; -- Pit of Saron
	AL["FH2"] = "FH2"; -- Pit of Saron
	AL["RS"] = "RS"; -- The Ruby Sanctum
	AL["TEoE"] = "TEoE"; -- The Eye of Eternity
	AL["UK, Keep"] = "UK, Keep"; -- Utgarde Keep
	AL["Uldu"] = "Uldu"; -- Ulduar
	AL["UP, Pinn"] = "UP, Pinn"; -- Utgarde Pinnacle
	AL["VH"] = "VH"; -- The Violet Hold
	AL["VoA"] = "VoA"; -- Vault of Archavon

	--Zones not included in LibBabble-Zone
	AL["Crusaders' Coliseum"] = "十字军大竞技场";

	--Cataclysm Acronyms
	AL["BH"] = "BH"; --Baradin Hold
	AL["BoT"] = "BoT"; --Bastion of Twilight
	AL["BRC"] = "BRC"; --Blackrock Caverns
	AL["BWD"] = "BWD"; --Blackwing Descent
	AL["CoT-DS"] = "CoT-DS"; --Caverns of Time: Dragon Soul
	AL["CoT-ET"] = "CoT-ET"; --Caverns of Time: End Time
	AL["CoT-HoT"] = "CoT-HoT"; --Caverns of Time: Hour of Twilight
	AL["CoT-WoE"] = "CoT-WoE"; --Caverns of Time: Well of Eternity
	AL["FL"] = "FL"; --Firelands
	AL["GB"] = "GB"; --Grim Batol
	AL["HoO"] = "HoO"; --Halls of Origination
	AL["LCoT"] = "LCoT"; --Lost City of the Tol'vir 
	AL["TSC"] = "TSC"; --The Stonecore
	AL["TWT"] = "TWT"; --Throne of the Four Winds
	AL["ToTT"] = "ToTT"; --Throne of the Tides
	AL["VP"] = "VP"; --The Vortex Pinnacle
	AL["ZA"] = "ZA"; -- Zul'Aman
	AL["ZG"] = "ZG"; --Zul'Gurub

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
	AL["Clarissa"] = "克拉里萨";
	AL["Greatfather Aldrimus"] = "奥德里姆斯宗父";
	AL["Ha'lei"] = "哈雷";
	AL["Horvon the Armorer <Armorsmith>"] = "铸甲匠霍尔冯 <护甲锻造师>";
	AL["Ramdor the Mad"] = "疯狂的拉姆杜尔";
	AL["Nexus-Prince Haramad"] = "节点亲王哈拉迈德";
	AL["\"Slim\" <Shady Dealer>"] = "“瘦子” <毒药商>";
	AL["\"Captain\" Kaftiz"] = "“上尉”卡弗提兹";
	AL["Dealer Tariq <Shady Dealer>"] = "商人塔利基 <毒药商>";
	AL["Provisioner Tsaalt"] = "补给官塔萨尔特";

	--Blackfathom Deeps (Entrance)

	--Blackrock Mountain (Entrance)
	AL["Bodley"] = "伯德雷";
	AL["Lothos Riftwaker"] = "洛索斯·天痕";
	AL["Orb of Command"] = "命令宝珠";
	AL["Scarshield Quartermaster <Scarshield Legion>"] = "裂盾军需官 <裂盾军团>";
	AL["The Behemoth"] = "贝哈默斯";

	--Caverns of Time (Entrance)
	AL["Steward of Time <Keepers of Time>"] = "时间管理者 <时光守护者>";
	AL["Alexston Chrome <Tavern of Time>"] = "阿历克斯顿·克罗姆 <时间旅店>";
	AL["Yarley <Armorer>"] = "亚尔雷 <护甲商>";
	AL["Bortega <Reagents & Poison Supplies>"] = "波特加 <材料与毒药商>";
	AL["Alurmi <Keepers of Time Quartermaster>"] = "艾鲁尔米 <时光守护者军需官>";
	AL["Galgrom <Provisioner>"] = "加尔戈罗姆 <供给商人>";
	AL["Zaladormu"] = "扎拉多姆";
	AL["Soridormi <The Scale of Sands>"] = "索莉多米 <流沙之鳞>";
	AL["Arazmodu <The Scale of Sands>"] = "阿拉兹姆多 <流沙之鳞>";
	AL["Andormu <Keepers of Time>"] = "安多姆 <时光守护者>";
	AL["Nozari <Keepers of Time>"] = "诺萨莉 <时光守护者>";
	AL["Anachronos <Keepers of Time>"] = "阿纳克洛斯 <时光守护者>";

	--Caverns of Time: Hyjal (Entrance)
	AL["Indormi <Keeper of Ancient Gem Lore>"] = "因多米 <上古宝石保管者>";
	AL["Tydormu <Keeper of Lost Artifacts>"] = "泰多姆 <失落神器的保管者>";

	--Coilfang Reservoir (Entrance)
	AL["Mortog Steamhead"] = "莫尔托格";

	--Dire Maul (Entrance)
	AL["Dire Pool"] = "厄运之池";
	AL["Dire Maul Arena"] = "厄运之槌竞技场";
	AL["Elder Mistwalker"] = "迷雾长者";

	--Gnomeregan (Entrance)
	AL["Torben Zapblast <Teleportation Specialist>"] = "托尔本·光爆 <传送专家>";

	--Hellfire Citadel (Entrance)
	AL["Steps and path to the Blood Furnace"] = "通往鲜血熔炉的阶梯与通道";
	AL["Path to the Hellfire Ramparts and Shattered Halls"] = "通往地狱火城墙与破碎大厅的通道";
	AL["Meeting Stone of Magtheridon's Lair"] = "集合石 - 玛瑟里顿的巢穴";
	AL["Meeting Stone of Hellfire Citadel"] = "集合石 - 地狱火堡垒";

	--Icecrown Citadel (Entrance)

	--Karazhan (Entrance)
	AL["Archmage Leryda"] = "大法师蕾尔达";
	AL["Archmage Alturus"] = "大法师奥图鲁斯";
	AL["Apprentice Darius"] = "学徒达里乌斯";
	AL["Stairs to Underground Pond"] = "通往地下水池的楼梯";
	AL["Stairs to Underground Well"] = "通往地下水井的楼梯";
	AL["Charred Bone Fragment"] = "焦骨碎块";

	--Maraudon (Entrance)
	AL["The Nameless Prophet"] = "无名预言者";
	AL["Cursed Centaur"] = "被诅咒的半人马";
	AL["Kherrah"] = "柯尔拉";

	--Scarlet Monastery (Entrance)

	--The Deadmines (Entrance)

	--Sunken Temple (Entrance)
	AL["Priestess Udum'bra"] = "女祭司乌达布拉";
	AL["Gomora the Bloodletter"] = "放血者古穆拉";
	AL["Captain Wyrmak"] = "维尔玛克将军";

	--Uldaman (Entrance)

	--Ulduar (Entrance)
	AL["Shavalius the Fancy <Flight Master>"] = "古怪的沙瓦留斯 <飞行管理员>";
	AL["Chester Copperpot <General & Trade Supplies>"] = "切斯特·考伯特 <杂货商>";
	AL["Slosh <Food & Drink>"] = "斯洛什 <食物与饮料>";

	--Wailing Caverns (Entrance)

--************************************************
-- Kalimdor Instances (Classic)
--************************************************

	--Blackfathom Deeps
	AL["Shrine of Gelihast"] = "格里哈斯特神殿";
	AL["Fathom Stone"] = "深渊之石";
	AL["Lorgalis Manuscript"] = "洛迦里斯手稿";
	AL["Scout Thaelrid"] = "斥候塞尔瑞德";
	AL["Flaming Eradicator"] = "烈焰根除者";
	AL["Altar of the Deeps"] = "玛塞斯特拉祭坛";
	AL["Ashelan Northwood"] = "阿舍兰·北林";
	AL["Relwyn Shadestar"] = "莱温·影星";
	AL["Sentinel Aluwyn"] = "哨兵阿露温";
	AL["Sentinel-trainee Issara"] = "受训哨兵伊莎娜";
	AL["Je'neu Sancrea <The Earthen Ring>"] = "耶努萨克雷 <大地之环>";
	AL["Zeya"] = "泽雅";

	--Dire Maul (East)
	AL["\"Ambassador\" Dagg'thol"] = "“大使”达戈索尔";
	AL["Furgus Warpwood"] = "费尔古斯·扭木";
	AL["Old Ironbark"] = "埃隆巴克";
	AL["Ironbark the Redeemed"] = "赎罪的埃隆巴克";

	--Dire Maul (North)
	AL["Druid of the Talon"] = "猛禽德鲁伊";
	AL["Stonemaul Ogre"] = "石槌食人魔";
	AL["Knot Thimblejack"] = "诺特·希姆加克";

	--Dire Maul (West)
	AL["Ferra"] = "费拉";
	AL["Estulan <The Highborne>"] = "埃斯图兰 <上层精灵>";
	AL["Shen'dralar Watcher"] = "辛德拉观察者";
	AL["Pylons"] = "水晶塔";
	AL["Ancient Equine Spirit"] = "上古圣马之魂";
	AL["Shen'dralar Ancient"] = "辛德拉古灵";
	AL["Falrin Treeshaper"] = "法尔林·树影";
	AL["Lorekeeper Lydros"] = "博学者莱德罗斯";
	AL["Lorekeeper Javon"] = "博学者亚沃";
	AL["Lorekeeper Kildrath"] = "博学者基尔达斯";
	AL["Lorekeeper Mykos"] = "博学者麦库斯";
	AL["Shen'dralar Provisioner"] = "辛德拉圣职者";

	--Maraudon	
	AL["Elder Splitrock"] = "碎石长者";
	AL["Celebras the Redeemed"] = "赎罪的塞雷布拉斯";

	--Ragefire Chasm
	AL["Commander Bagran"] = "指挥官巴格兰";
	AL["Invoker Xorenth"] = "祈求者克索伦斯";
	AL["Scout Cage"] = "斥候牢笼";

	--Razorfen Downs
	AL["Koristrasza"] = "克莉丝塔萨";
	AL["Belnistrasz"] = "奔尼斯特拉兹";

	--Razorfen Kraul
	AL["Auld Stonespire"] = "奥尔德·石塔 ";
	AL["Spirit of Agamaggan <Ancient>"] = "阿迦玛甘之魂 <远古半神>";
	AL["Willix the Importer"] = "进口商威利克斯";

	--Ruins of Ahn'Qiraj
	AL["Four Kaldorei Elites"] = "卡多雷四精英";
	AL["Captain Qeez"] = "奎兹上尉";
	AL["Captain Tuubid"] = "图比德上尉";
	AL["Captain Drenn"] = "德雷恩上尉";
	AL["Captain Xurrem"] = "库雷姆上尉";
	AL["Major Yeggeth"] = "耶吉斯少校";
	AL["Major Pakkon"] = "帕库少校";
	AL["Colonel Zerran"] = "泽兰上校";
	AL["Safe Room"] = "安全房间";

	--Temple of Ahn'Qiraj
	AL["Andorgos <Brood of Malygos>"] = "安多葛斯 <玛里苟斯的后裔>";
	AL["Vethsera <Brood of Ysera>"] = "温瑟拉 <伊瑟拉的后裔>";
	AL["Kandrostrasz <Brood of Alexstrasza>"] = "坎多斯特拉兹 <阿莱克丝塔萨的后裔>";
	AL["Arygos"] = "亚雷戈斯";
	AL["Caelestrasz"] = "凯雷斯特拉兹";
	AL["Merithra of the Dream"] = "梦境之龙麦琳瑟拉";

	--Wailing Caverns
	AL["Ebru <Disciple of Naralex>"] = "厄布鲁 <纳拉雷克斯的信徒>"; -- 5768
	AL["Nalpak <Disciple of Naralex>"] = "纳尔帕克 <纳拉雷克斯的信徒>"; -- 5767
	AL["Muyoh <Disciple of Naralex>"] = "穆约 <纳拉雷克斯的信徒>";  -- 3678
	AL["Naralex"] = "纳拉雷克斯"; -- 3679

	--Zul'Farrak
	AL["Chief Engineer Bilgewhizzle <Gadgetzan Water Co.>"] = "首席工程师沙克斯·比格维兹 <加基森水业公司>";
	AL["Mazoga's Spirit"] = "玛佐加的灵魂";
	AL["Tran'rek"] = "特兰雷克";
	AL["Weegli Blastfuse"] = "维格利";
	AL["Raven"] = "拉文";
	AL["Elder Wildmane"] = "蛮鬃长者";

--****************************
-- Eastern Kingdoms Instances (Classic)
--****************************

	--Blackrock Depths
	AL["The Black Anvil"] = "黑铁砧";
	AL["The Vault"] = "黑色宝库";
	AL["Watchman Doomgrip"] = "卫兵杜格瑞普";
	AL["Elder Morndeep"] = "黎明长者";
	AL["Schematic: Field Repair Bot 74A"] = "结构图：战地修理机器人74A型";
	AL["Private Rocknot"] = "罗克诺特下士";
	AL["Mistress Nagmara"] = "娜玛拉小姐";
	AL["Jalinda Sprig <Morgan's Militia>"] = "加琳达 <摩根民兵团>";
	AL["Oralius <Morgan's Militia>"] = "奥拉留斯 <摩根民兵团>";
	AL["Thal'trak Proudtusk <Kargath Expeditionary Force>"] = "萨特拉克 <卡加斯远征军>";
	AL["Galamav the Marksman <Kargath Expeditionary Force>"] = "神射手贾拉玛弗 <卡加斯远征军>";
	AL["Maxwort Uberglint"] = "麦克斯沃特·尤博格林";
	AL["Tinkee Steamboil"] = "丁奇·斯迪波尔";
	AL["Yuka Screwspigot <Engineering Supplies>"] = "尤卡·斯库比格特 <工程学供应商>";
	AL["Abandonded Mole Machine"] = "被弃用的挖掘机";
	AL["Kevin Dawson <Morgan's Militia>"] = "凯文·达森 <摩根民兵团>";
	AL["Lexlort <Kargath Expeditionary Force>"] = "雷克斯洛特 <卡加斯远征军>";
	AL["Prospector Seymour <Morgan's Militia>"] = "勘测员塞莫尔 <摩根民兵团>";
	AL["Razal'blade <Kargath Expeditionary Force>"] = "拉扎布雷德 <卡加斯远征军>";
	AL["The Shadowforge Lock"] = "暗炉之锁";
	AL["Mayara Brightwing <Morgan's Militia>"] = "玛亚拉·布莱特文 <摩根民兵团>";
	AL["Hierophant Theodora Mulvadania <Kargath Expeditionary Force>"] = "塞朵拉·穆瓦丹尼 <卡加斯远征军>";
	AL["Lokhtos Darkbargainer <The Thorium Brotherhood>"] = "罗克图斯·暗契 <瑟银兄弟会>";
	AL["Mountaineer Orfus <Morgan's Militia>"] = "巡山人奥弗斯 <摩根民兵团>";
	AL["Thunderheart <Kargath Expeditionary Force>"] = "桑德哈特 <卡加斯远征军>";
	AL["Marshal Maxwell <Morgan's Militia>"] = "麦克斯韦尔元帅 <摩根民兵团>";
	AL["Warlord Goretooth <Kargath Expeditionary Force>"] = "军官高图斯 <卡加斯远征军>";
	AL["The Black Forge"] = "黑熔炉";
	AL["Core Fragment"] = "熔火碎片";
	AL["Shadowforge Brazier"] = "暗炉炭火";

	--Blackrock Spire (Lower)
	AL["Urok's Tribute Pile"] = "乌洛克的贡品堆";
	AL["Acride <Scarshield Legion>"] = "阿克莱德 <裂盾军团>";
	AL["Elder Stonefort"] = "石墙长者";
	AL["Roughshod Pike"] = "尖锐长矛";

	--Blackrock Spire (Upper)
	AL["Finkle Einhorn"] = "芬克·恩霍尔";
	AL["Drakkisath's Brand"] = "达基萨斯的烙印";
	AL["Father Flame"] = "烈焰之父";

	--Blackwing Lair
	AL["Orb of Domination"] = "龙翼祭坛";
	AL["Master Elemental Shaper Krixix"] = "大元素师克里希克";

	--Gnomeregan
	AL["Chomper"] = "咀嚼者";
	AL["Blastmaster Emi Shortfuse"] = "爆破专家艾米·短线";
	AL["Murd Doc <S.A.F.E.>"] = "莫多克 <S.A.F.E.>";
	AL["Tink Sprocketwhistle <Engineering Supplies>"] = "丁克·铁哨 <工程学供应商>";
	AL["The Sparklematic 5200"] = "超级清洁器5200型";
	AL["Mail Box"] = "邮箱";
	AL["B.E Barechus <S.A.F.E.>"] = "“坏脾气”巴拉克斯 <S.A.F.E.>";
	AL["Face <S.A.F.E.>"] = "费斯 <S.A.F.E.>";
	AL["Hann Ibal <S.A.F.E.>"] = "汉尼巴尔 <S.A.F.E.>";

	--Molten Core

	--Scarlet Halls
	AL["Commander Lindon"] = "指挥官林顿";
	AL["Hooded Crusader"] = "蒙面的十字军战士";
	AL["Bucket of Meaty Dog Food"] = "一桶多肉狗食";
	AL["Reinforced Archery Target"] = "强固箭靶";

	--Scarlet Monastery

	--Scholomance
	AL["Instructor Chillheart's Phylactery"] = "指导者寒心的护命匣";
	AL["Professor Slate"] = "斯雷特教授";
	AL["Polyformic Acid Potion"] = "蚁酸药水";
	AL["Talking Skull"] = "缚魂魔袋";
	AL["In the Shadow of the Light"] = "光明下的阴影";
	AL["Kel'Thuzad's Deep Knowledge"] = "克尔苏加德的深层知识";
	AL["Forbidden Rites and other Rituals Necromantic"] = "禁忌咒文及其他死灵仪式";
	AL["Coffer of Forgotten Souls"] = "失落灵魂容器";
	AL["The Dark Grimoire"] = "黑暗法典";

	--Shadowfang Keep
	AL["Apothecary Trio"] = "药剂师三人组";
	AL["Apothecary Hummel <Crown Chemical Co.>"] = "药剂师汉摩尔 <皇冠药剂公司>";
	AL["Apothecary Baxter <Crown Chemical Co.>"] = "药剂师拜克斯特 <皇冠药剂公司>";
	AL["Apothecary Frye <Crown Chemical Co.>"] = "药剂师弗莱 <皇冠药剂公司>";
	AL["Packleader Ivar Bloodfang"] = "狼群首领伊瓦·血牙";
	AL["Deathstalker Commander Belmont"] = "死亡猎手指挥官贝尔蒙特";
	AL["Haunted Stable Hand"] = "鬼怪马夫";
	AL["Investigator Fezzen Brasstacks"] = "调查员费岑·布莱斯塔克";

	--Stratholme - Crusader's Square
	AL["Crusade Commander Eligor Dawnbringer <Brotherhood of the Light>"] = "十字军指挥官埃里戈尔·黎明使者 <圣光兄弟会>";
	AL["Master Craftsman Wilhelm <Brotherhood of the Light>"] = "工匠大师威尔海姆 <圣光兄弟会>";
	AL["Packmaster Stonebruiser <Brotherhood of the Light>"] = "马队管理者布鲁斯·石锤 <圣光兄弟会>";
	AL["Stratholme Courier"] = "斯坦索姆信使";
	AL["Fras Siabi's Postbox"] = "弗拉斯·希亚比的邮箱";
	AL["King's Square Postbox"] = "国王广场邮箱";
	AL["Festival Lane Postbox"] = "节日小道邮箱";
	AL["Elder Farwhisper"] = "远风长者";
	AL["Market Row Postbox"] = "市场邮箱";

	--Stratholme - The Gauntlet
	AL["Elders' Square Postbox"] = "长者广场邮箱";
	AL["Archmage Angela Dosantos <Brotherhood of the Light>"] = "大法师安吉拉·杜萨图斯 <圣光兄弟会>";
	AL["Crusade Commander Korfax <Brotherhood of the Light>"] = "十字军指挥官科尔法克斯 <圣光兄弟会>";

	--The Deadmines
	AL["Lieutenant Horatio Laine"] = "霍拉提奥·莱茵中尉";
	AL["Kagtha"] = "卡格萨";
	AL["Slinky Sharpshiv"] = "史莉琪·剃刀";
	AL["Quartermaster Lewis <Quartermaster>"] = "军需官刘易斯 <军需官>";
	AL["Miss Mayhem"] = "迈赫米小姐";
	AL["Vend-O-Tron D-Luxe"] = "售货机器人豪华版";

	--The Stockade
	AL["Rifle Commander Coe"] = "火枪手指挥官柯伊";
	AL["Warden Thelwater"] = "典狱官塞尔沃特";
	AL["Nurse Lillian"] = "护士莉莲";

	--The Sunken Temple
	AL["Lord Itharius"] = "伊萨里奥斯勋爵";
	AL["Elder Starsong"] = "星歌长者";

	--Uldaman
	AL["Baelog's Chest"] = "巴尔洛戈的箱子";
	AL["Kand Sandseeker <Explorer's League>"] = "坎德·沙寻者 <探险者协会>";
	AL["Lead Prospector Durdin <Explorer's League>"] = "首席勘探员杜尔林 <探险者协会>";
	AL["Olga Runesworn <Explorer's League>"] = "奥尔达·符誓 <探险者协会>";
	AL["Aoren Sunglow <The Reliquary>"] = "奥伦·日冕 <神圣遗物学会>";
	AL["High Examiner Tae'thelan Bloodwatcher <The Reliquary>"] = "高阶考察者泰瑟兰·血望者 <神圣遗物学会>";
	AL["Lidia Sunglow <The Reliquary>"] = "莉迪亚·日冕 <神圣遗物学会>";
	AL["Ancient Treasure"] = "古代宝藏";
	AL["The Discs of Norgannon"] = "诺甘农圆盘";

--*******************
-- Burning Crusade Instances
--*******************

	--Auch: Auchenai Crypts
	AL["Draenei Spirit"] = "德莱尼灵魂";
	AL["Avatar of the Martyred"] = "殉难者的化身";
	AL["D'ore"] = "德欧里";
	AL["Tormented Soulpriest"] = "受折磨的灵魂牧师";

	--Auch: Mana-Tombs
	AL["Artificer Morphalius"] = "工匠莫法鲁斯";
	AL["Mamdy the \"Ologist\""] = "“杂学家”玛姆迪";
	AL["Shadow Lord Xiraxis"] = "暗影领主希拉卡希斯";
	AL["Ambassador Pax'ivi"] = "帕克希维大使";
	AL["Cryo-Engineer Sha'heen"] = "低温工程师沙赫恩";
	AL["Ethereal Transporter Control Panel"] = "虚灵传送器控制台";

	--Auch: Sethekk Halls
	AL["Isfar"] = "伊斯法尔";
	AL["Dealer Vijaad"] = "商人维嘉德";
	AL["Lakka"] = "拉卡";
	AL["The Saga of Terokk"] = "泰罗克的传说";

	--Auch: Shadow Labyrinth
	AL["Field Commander Mahfuun"] = "战地指挥官玛弗恩";
	AL["Spy Grik'tha"] = "间谍格利克萨";
	AL["The Codex of Blood"] = "鲜血法典";
	AL["First Fragment Guardian"] = "第一块碎片的守护者";
	AL["Spy To'gun"] = "间谍托古恩";

	--Black Temple (Start)
	AL["Towards Reliquary of Souls"] = "通往灵魂之匣";
	AL["Towards Teron Gorefiend"] = "通往塔隆·血魔";
	AL["Towards Illidan Stormrage"] = "通往伊利丹·怒风";
	AL["Spirit of Olum"] = "奥鲁姆之魂";
	AL["Spirit of Udalo"] = "乌达鲁之魂";
	AL["Aluyen <Reagents>"] = "奥鲁尤 <材料商>";
	AL["Okuno <Ashtongue Deathsworn Quartermaster>"] = "沃库诺 <灰舌死誓者军需官>";
	AL["Seer Kanai"] = "先知坎奈";

	--Black Temple (Basement)

	--Black Temple (Top)

	--CFR: Serpentshrine Cavern
	AL["Seer Olum"] = "先知奥鲁姆";

	--CFR: The Slave Pens
	AL["Nahuud"] = "纳霍德";
	AL["Watcher Jhang"] = "观察者杰哈恩";
	AL["Weeder Greenthumb"] = "除草者格林萨姆";
	AL["Skar'this the Heretic"] = "异教徒斯卡希斯";
	AL["Naturalist Bite"] = "博学者拜特";

	--CFR: The Steamvault
	AL["Windcaller Claw"] = "唤风者克劳恩";
	AL["Main Chambers Access Panel"] = "主厅控制面板";
	AL["Second Fragment Guardian"] = "第二块碎片的守护者";

	--CFR: The Underbog
	AL["T'shu"] = "塔苏";
	AL["The Underspore"] = "幽暗孢子";
	AL["Earthbinder Rayge"] = "缚地者雷葛";

	--CoT: The Black Morass
	AL["Sa'at <Keepers of Time>"] = "萨艾特 <时光守护者>";

	--CoT: Hyjal Summit
	AL["Lady Jaina Proudmoore"] = "吉安娜·普罗德摩尔";
	AL["Thrall <Warchief>"] = "萨尔 <酋长>";
	AL["Tyrande Whisperwind <High Priestess of Elune>"] = "泰兰德·语风 <艾露恩的高阶女祭司>";

	--CoT: Old Hillsbrad Foothills
	AL["Erozion"] = "伊洛希恩";
	AL["Brazen"] = "布拉森";
	AL["Landing Spot"] = "着陆点";
	AL["Thrall"] = "萨尔";
	AL["Taretha"] = "塔蕾莎";
	AL["Don Carlos"] = "卡洛斯";
	AL["Guerrero"] = "古雷罗";
	AL["Thomas Yance <Travelling Salesman>"] = "托马斯·杨斯 <旅行商人>";
	AL["Aged Dalaran Wizard"] = "老迈的达拉然巫师";
	AL["Jonathan Revah"] = "乔纳森·雷瓦";
	AL["Jerry Carter"] = "杰瑞·卡特尔";
	AL["Helcular"] = "赫尔库拉";
	AL["Farmer Kent"] = "农夫肯特";
	AL["Sally Whitemane"] = "萨莉·怀特迈恩";
	AL["Renault Mograine"] = "雷诺·莫格莱尼";
	AL["Little Jimmy Vishas"] = "吉米·维沙斯";
	AL["Herod the Bully"] = "赫洛德";
	AL["Nat Pagle"] = "纳特·帕格";
	AL["Hal McAllister"] = "哈尔·马克奥里斯特";
	AL["Zixil <Aspiring Merchant>"] = "吉克希尔 <有抱负的商人>";
	AL["Overwatch Mark 0 <Protector>"] = "守候者零型 <保护者>";
	AL["Southshore Inn"] = "南海镇旅馆";
	AL["Captain Edward Hanes"] = "爱德华·汉斯";
	AL["Captain Sanders"] = "杉德尔船长";
	AL["Commander Mograine"] = "指挥官莫格莱尼";
	AL["Isillien"] = "伊森利恩";
	AL["Abbendis"] = "阿比迪斯";
	AL["Fairbanks"] = "法尔班克斯";
	AL["Taelan"] = "泰兰";
	AL["Barkeep Kelly <Bartender>"] = "酒吧招待凯利 <调酒师>";
	AL["Frances Lin <Barmaid>"] = "弗兰斯·林 <招待员>";
	AL["Chef Jessen <Speciality Meat & Slop>"] = "厨师杰森 <美食大师>";
	AL["Stalvan Mistmantle"] = "斯塔文·密斯特曼托";
	AL["Phin Odelic <The Kirin Tor>"] = "费恩·奥德利克 <肯瑞托>";
	AL["Magistrate Henry Maleb"] = "赫尼·马雷布镇长";
	AL["Raleigh the True"] = "虔诚的莱雷恩";
	AL["Nathanos Marris"] = "纳萨诺斯·玛瑞斯";
	AL["Bilger the Straight-laced"] = "古板的比格尔";
	AL["Innkeeper Monica"] = "旅店老板莫妮卡";
	AL["Julie Honeywell"] = "朱丽·哈尼维尔";
	AL["Jay Lemieux"] = "贾森·雷缪克斯";
	AL["Young Blanchy"] = "小马布兰契";

	--Gruul's Lair

	--HFC: The Blood Furnace
	AL["Gunny"] = "冈尼";
	AL["Caza'rez"] = "卡萨雷兹";

	--HFC: Hellfire Ramparts
	AL["Advance Scout Chadwick"] = "高级斥候卡德维克";
	AL["Stone Guard Stok'ton"] = "石头守卫斯托克顿";
	AL["Reinforced Fel Iron Chest"] = "强化魔铁箱";

	--HFC: Magtheridon's Lair

	--HFC: The Shattered Halls
	AL["Shattered Hand Executioner"] = "碎手斩杀者";
	AL["Private Jacint"] = "列兵亚森特";
	AL["Rifleman Brownbeard"] = "火枪手布隆恩·棕须";
	AL["Captain Alina"] = "奥琳娜上尉";
	AL["Scout Orgarr"] = "斥候奥贾尔";
	AL["Korag Proudmane"] = "克拉格·傲鬃";
	AL["Captain Boneshatter"] = "沙塔·碎骨上尉";
	AL["Randy Whizzlesprocket"] = "兰迪·维兹普罗克";
	AL["Drisella"] = "德雷希拉";

	--Karazhan Start
	AL["Baroness Dorothea Millstipe"] = "杜萝希·米尔斯提女伯爵";
	AL["Lady Catriona Von'Indi"] = "卡翠欧娜·冯因迪女伯爵";
	AL["Lady Keira Berrybuck"] = "吉拉·拜瑞巴克女伯爵";
	AL["Baron Rafe Dreuger"] = "拉弗·杜格尔男爵";
	AL["Lord Robin Daris"] = "罗宾·达瑞斯伯爵";
	AL["Lord Crispin Ference"] = "克里斯宾·费伦斯伯爵";
	AL["Red Riding Hood"] = "小红帽";
	AL["Wizard of Oz"] = "绿野仙踪";
	AL["The Master's Terrace"] = "主宰的露台";
	AL["Servant Quarters"] = "仆人区";
	AL["Hastings <The Caretaker>"] = "哈斯汀斯 <看管者>";
	AL["Berthold <The Doorman>"] = "伯特霍德 <门卫>";
	AL["Calliard <The Nightman>"] = "卡利亚德 <清洁工>";
	AL["Koren <The Blacksmith>"] = "库雷恩 <铁匠>";
	AL["Bennett <The Sergeant at Arms>"] = "本内特 <警卫>";
	AL["Keanna's Log"] = "金娜的日记";
	AL["Ebonlocke <The Noble>"] = "埃伯洛克 <贵族>";
	AL["Sebastian <The Organist>"] = "塞巴斯蒂安 <风琴手>";
	AL["Barnes <The Stage Manager>"] = "巴内斯 <舞台管理员>";

	--Karazhan End
	AL["Path to the Broken Stairs"] = "通往破碎阶梯的通道";
	AL["Broken Stairs"] = "破碎阶梯";
	AL["Ramp to Guardian's Library"] = "通往守护者的图书馆的斜坡";
	AL["Mysterious Bookshelf"] = "神秘的书架";
	AL["Ramp up to the Celestial Watch"] = "通往观星大厅的斜坡";
	AL["Ramp down to the Gamesman's Hall"] = "通往象棋大厅的斜坡";
	AL["Ramp to Medivh's Chamber"] = "通往麦迪文房间的斜坡";
	AL["Spiral Stairs to Netherspace"] = "通往虚空异界的楼梯";
	AL["Wravien <The Mage>"] = "拉维恩 <法师>";
	AL["Gradav <The Warlock>"] = "格拉达夫 <术士>";
	AL["Kamsis <The Conjurer>"] = "卡姆希丝 <咒术师>";
	AL["Ythyar"] = "伊萨尔";
	AL["Echo of Medivh"] = "麦迪文的回音";

	--Magisters Terrace
	AL["Exarch Larethor"] = "主教拉雷索尔";
	AL["Fel Crystals"] = "邪能水晶";
	AL["Apoko"] = "埃波克";
	AL["Eramas Brightblaze"] = "埃拉玛斯·炽光";
	AL["Ellrys Duskhallow"] = "艾尔蕾丝";
	AL["Fizzle"] = "菲兹尔";
	AL["Garaxxas"] = "贾拉克萨斯";
	AL["Sliver <Garaxxas' Pet>"] = "脆皮 <贾拉克萨斯的宠物>";
	AL["Kagani Nightstrike"] = "卡加尼·夜锋";
	AL["Warlord Salaris"] = "督军沙拉利斯";
	AL["Yazzai"] = "亚赛";
	AL["Zelfan"] = "扎尔凡";
	AL["Tyrith"] = "塔雷斯";
	AL["Scrying Orb"] = "占卜宝珠";

	--Sunwell Plateau
	AL["Madrigosa"] = "玛蒂苟萨";

	--TK: The Arcatraz
	AL["Millhouse Manastorm"] = "米尔豪斯·法力风暴";
	AL["Third Fragment Guardian"] = "第三块碎片的守护者";
	AL["Udalo"] = "先知乌达鲁";

	--TK: The Botanica

	--TK: The Mechanar
	AL["Overcharged Manacell"] = "超载的魔法晶格";

	--TK: The Eye

--*****************
-- Wrath of the Lich King Instances
--*****************

	--Azjol-Nerub: Ahn'kahet: The Old Kingdom
	AL["Seer Ixit"] = "先知伊克塞特";
	AL["Ahn'kahet Brazier"] = "安卡赫特火盆";

	--Azjol-Nerub: Azjol-Nerub
	AL["Reclaimer A'zak"] = "复国者阿扎克";
	AL["Watcher Gashra"] = "看守者加什拉";
	AL["Watcher Narjil"] = "看守者纳尔伊";
	AL["Watcher Silthik"] = "看守者希尔希克";
	AL["Elder Nurgen"] = "长者努尔根";

	--Caverns of Time: The Culling of Stratholme
	AL["The Culling of Stratholme"] = "净化斯坦索姆";
	AL["Scourge Invasion Points"] = "天灾入侵点";
	AL["Guardian of Time"] = "时光守护者";
	AL["Chromie"] = "克罗米";

	--Drak'Tharon Keep
	AL["Image of Drakuru"] = "达库鲁的影像";
	AL["Kurzel"] = "库塞尔";
	AL["Elder Kilias"] = "长者基里亚斯";
	AL["Drakuru's Brazier"] = "达库鲁的火盆";

	--The Frozen Halls: Halls of Reflection
	--3 beginning NPCs omitted, see The Forge of Souls
	AL["The Captain's Chest"] = "队长的箱子";

	--The Frozen Halls: Pit of Saron
	--6 beginning NPCs omitted, see The Forge of Souls
	AL["Martin Victus"] = "马丁·维克图斯";
	AL["Gorkun Ironskull"] = "戈库恩·铁颅";
	AL["Rimefang"] = "霜牙";

	--The Frozen Halls: The Forge of Souls
	--Lady Jaina Proudmoore omitted, in Hyjal Summit
	AL["Archmage Koreln <Kirin Tor>"] = "大法师克雷林 <肯瑞托>";
	AL["Archmage Elandra <Kirin Tor>"] = "大法师埃兰德拉 <肯瑞托>";
	AL["Lady Sylvanas Windrunner <Banshee Queen>"] = "希尔瓦娜斯·风行者 <女妖之王>";
	AL["Dark Ranger Loralen"] = "黑暗游侠萝拉兰";
	AL["Dark Ranger Kalira"] = "黑暗游侠卡丽拉";

	--Gundrak
	AL["Chronicler Bah'Kini"] = "记载者拜基妮";
	AL["Tol'mar"] = "托玛尔";
	AL["Elder Ohanzee"] = "长者奥哈齐";

	--Icecrown Citadel
	AL["To next map"] = "到下一个地图";
	AL["From previous map"] = "到前一个地图";
	AL["Upper Spire"] = "上层之塔";
	AL["Sindragosa's Lair"] = "冰霜女王的巢穴";
	AL["Stinky"] = "大臭";
	AL["Precious"] = "小宝";
	AL["Rimefang"] = "霜牙";	-- NPC: 37533
	AL["Spinestalker"] = "猎脊冰龙";	-- NPC: 37534
	AL["Sister Svalna"] = "女武神席瓦娜";	-- NPC: 37126

	--Naxxramas
	AL["Mr. Bigglesworth"] = "比格沃斯";
	AL["Frostwyrm Lair"] = "萨菲隆之巢";
	AL["Teleporter to Middle"] = "传送到中间";

	--The Obsidian Sanctum
	AL["Black Dragonflight Chamber"] = "黑龙军团密室";

	--Onyxia's Lair

	--The Ruby Sanctum
	AL["Red Dragonflight Chamber"] = "红龙军团密室";

	--The Nexus: The Eye of Eternity

	--The Nexus: The Nexus
	AL["Warmage Kaitlyn"] = "战斗法师凯特琳";
	AL["Berinand's Research"] = "伯林纳德的研究笔记";
	AL["Elder Igasho"] = "长者伊加苏";

	--The Nexus: The Oculus
	AL["Belgaristrasz"] = "贝加里斯";
	AL["Eternos"] = "伊特努斯";
	AL["Verdisa"] = "沃尔蒂萨";
	AL["Centrifuge Construct"] = "离心构造体";
	AL["Cache of Eregos"] = "埃雷苟斯的宝箱";

	--Trial of the Champion
	AL["Marshal Jacob Alerius"] = "雅克布·奥勒留斯元帅";
	AL["Ambrose Boltspark"] = "安布罗斯·雷钉";
	AL["Colosos"] = "克罗索斯";
	AL["Jaelyne Evensong"] = "娅琳·永歌";
	AL["Lana Stouthammer"] = "拉娜·硬锤";

	--Trial of the Crusader
	AL["Heroic: Trial of the Grand Crusader"] = "英雄: 大十字军的试炼";
	AL["Cavern Entrance"] = "洞穴入口";

	--Ulduar General
	AL["The Siege"] = "攻城区域";
	AL["The Keepers"] = "守护者";

	--Ulduar A
	AL["Tower of Life"] = "生命之塔";
	AL["Tower of Flame"] = "烈焰之塔";
	AL["Tower of Frost"] = "冰霜之塔";
	AL["Tower of Storms"] = "风暴之塔";

	--Ulduar B
	AL["Prospector Doren"] = "勘探员多尔伦";
	AL["Archivum Console"] = "档案馆控制台";

	--Ulduar C
	AL["Sif"] = "西芙";

	--Ulduar D

	--Ulduar E

	--Ulduar: Halls of Lightning
	AL["Stormherald Eljrrin"] = "风暴先驱埃尔林";

	--Ulduar: Halls of Stone
	AL["Kaldir Ironbane"] = "卡迪尔·斩铁";
	AL["Tribunal Chest"] = "远古法庭宝箱";
	AL["Elder Yurauk"] = "长者尤拉克";
	AL["Brann Bronzebeard"] = "布莱恩·铜须";

	--Utgarde Keep: Utgarde Keep
	AL["Defender Mordun"] = "防御者墨尔顿";
	AL["Dark Ranger Marrah"] = "黑暗游侠玛尔拉";
	AL["Elder Jarten"] = "长者亚尔特恩";

	--Utgarde Keep: Utgarde Pinnacle
	AL["Brigg Smallshanks"] = "布雷格";
	AL["Image of Argent Confessor Paletress"] = "银色神官帕尔崔丝的影像";
	AL["Elder Chogan'gada"] = "长者库甘达加";

	--Vault of Archavon

	--The Violet Hold
	AL["Lieutenant Sinclari"] = "辛克莱尔中尉";

--*********************
-- Cataclysm Instances
--*********************

	--Baradin Hold

	--Blackrock Caverns

	--Blackwing Descent

	--Caverns of Time: Dragon Soul
	AL["Dasnurimi <Geologist & Conservator>"] = "达丝诺瑞米 <地质学家和保护人>";
	AL["Lord Afrasastrasz"] = "阿弗拉沙斯塔兹";

	--Caverns of Time: End Time
	AL["Alurmi"] = "艾鲁尔米";
	AL["Nozdormu"] = "诺兹多姆";

	--Caverns of Time: Hour of Twilight

	--Caverns of Time: Well of Eternity

	--Firelands
	AL["Lurah Wrathvine <Crystallized Firestone Collector>"] = "鲁娜·怒藤 <火石结晶收集者>";
	AL["Naresir Stormfury <Avengers of Hyjal Quartermaster>"] = "纳瑟尔·雷怒 <海加尔复仇者军需官>";

	--Grim Batol
	AL["Baleflame"] = "烽焰";
	AL["Farseer Tooranu <The Earthen Ring>"] = "先知图拉努 <大地之环>";
	AL["Velastrasza"] = "瓦莱斯塔萨";

	--Halls of Origination
	AL["Large Stone Obelisk"] = "巨石方尖碑";

	--Lost City of the Tol'vir
	AL["Captain Hadan"] = "哈丹队长";
	AL["Tol'vir Grave"] = "托维尔之墓";

	--The Bastion of Twilight

	--The Stonecore
	AL["Earthwarden Yrsa <The Earthen Ring>"] = "大地看守者伊尔萨 <大地之环>";

	--The Vortex Pinnacle
	AL["Itesh"] = "伊特什";
	AL["Magical Brazier"] = "魔法火盆";

	--Throne of the Four Winds

	--Throne of the Tides
	AL["Captain Taylor"] = "泰勒上尉";
	AL["Legionnaire Nazgrim"] = "军团士兵纳兹戈林";
	AL["Neptulon"] = "耐普图隆";

	--Zul'Aman
	AL["Vol'jin"] = "沃金";
	AL["Witch Doctor T'wansi"] = "巫医提旺司";
	AL["Blood Guard Hakkuz <Darkspear Elite>"] = "血卫士哈库佐 <暗矛精英>";
	AL["Voodoo Pile"] = "巫毒堆";
	AL["Bakkalzu"] = "巴卡祖鲁";
	AL["Hazlek"] = "哈兹莱克";
	AL["The Map of Zul'Aman"] = "祖阿曼地图";
	AL["Norkani"] = "洛尔卡尼";
	AL["Kasha"] = "卡莎";
	AL["Thurg"] = "索尔格";
	AL["Gazakroth"] = "卡扎克洛斯";
	AL["Lord Raadan"] = "兰尔丹";
	AL["Darkheart"] = "黑心";
	AL["Alyson Antille"] = "阿莱松·安提雷";
	AL["Slither"] = "滑行者";
	AL["Fenstalker"] = "沼泽猎手";
	AL["Koragg"] = "库拉格";
	AL["Zungam"] = "苏加姆";
	AL["Forest Frogs"] = "森林蛙";
	AL["Eulinda <Reagents>"] = "尤琳达 <材料商>";
	AL["Harald <Food Vendor>"] = "阿拉尔德 <食物商人>";
	AL["Arinoth"] = "阿瑞诺思";
	AL["Kaldrick"] = "卡尔德里克";
	AL["Lenzo"] = "雷恩佐";
	AL["Mawago"] = "玛瓦戈";
	AL["Melasong"] = "梅拉桑";
	AL["Melissa"] = "梅丽莎";
	AL["Micah"] = "米凯尔";
	AL["Relissa"] = "蕾莉莎";
	AL["Rosa"] = "洛莎";
	AL["Tyllan"] = "泰兰";

	--Zul'Gurub
	AL["Briney Boltcutter <Blackwater Financial Interests>"] = "布里尼·栓钳 <黑水金融>";
	AL["Vehini <Assault Provisions>"] = "维希尼 <前线供应商>";
	AL["Overseer Blingbang"] = "工头灵灵爆";
	AL["Bloodslayer T'ara <Darkspear Veteran>"] = "鲜血杀手塔莱 <暗矛老兵>";
	AL["Bloodslayer Vaena <Darkspear Veteran>"] = "鲜血杀手瓦伊纳 <暗矛老兵>";
	AL["Bloodslayer Zala <Darkspear Veteran>"] = "鲜血杀手扎拉 <暗矛老兵>";
	AL["Helpful Jungle Monkey"] = "有用的丛林猴子";
	AL["Venomancer Mauri <The Snake's Whisper>"] = "制毒师玛乌里 <蛇之呢喃>";
	AL["Zanzil's Cauldron of Toxic Torment"] = "赞吉尔的剧毒折磨坩埚";
	AL["Tiki Lord Mu'Loa"] = "蒂基面具之王姆罗亚";
	AL["Gub <Destroyer of Fish>"] = "伽布 <鲜鱼毁灭者>";
	AL["Venomancer T'Kulu <The Toxic Bite>"] = "制毒师提库鲁 <剧毒叮咬>";
	AL["Tor-Tun <The Slumberer>"] = "托尔图恩 <沉睡者>";
	AL["Kaulema the Mover"] = "搬运者伽乌里马";
	AL["Berserking Boulder Roller"] = "狂暴滚石者";
	AL["Zanzil's Cauldron of Frostburn Formula"] = "赞吉尔的霜灼药方坩埚";
	AL["Mor'Lek the Dismantler"] = "撕裂者莫尔雷克";
	AL["Witch Doctor Qu'in <Medicine Woman>"] = "巫医库因 <女药师>";
	AL["Zanza the Restless"] = "无眠者赞扎";
	AL["Mortaxx <The Tolling Bell>"] = "莫尔塔克斯 <死亡丧钟>";
	AL["Tiki Lord Zim'wae"] = "蒂基面具之王泽姆维";
	AL["Zanzil's Cauldron of Burning Blood"] = "赞吉尔的燃烧之血坩埚";

--*********************
-- Mists of Pandaria Instances
--*********************

	--Gate of the Setting Sun
	AL["Bowmistress Li <Guard Captain>"] = "女射手李琪薇 <守卫队长>";

	--Heart of Fear

	--Mogu'shan Palace
	AL["Sinan the Dreamer"] = "梦想家思南";

	--Mogu'shan Vaults

	--Shado-Pan Monastery
	AL["Ban Bearheart"] = "班·熊心";

	--Siege of Niuzao Temple
	AL["Shado-Master Chum Kiu"] = "影踪大师楚秋";

	--Siege of Orgrimmar

	--Stormstout Brewery
	AL["Auntie Stormstout"] = "风暴烈酒大婶";
	AL["Chen Stormstout"] = "陈·风暴烈酒";

	--Temple of the Jade Serpent
	AL["Master Windstrong"] = "风涌大师";
	AL["Priestess Summerpetal"] = "女牧师夏蕊";

	--Terrace of Endless Spring

	--Throne of Thunder
	AL["Monara <The Last Queen>"] = "莫纳拉 <末代皇后>";
	AL["No'ku Stormsayer <Lord of Tempest>"] = "颂风者诺库 <狂风领主>";
	AL["Rocky Horror"] = "岩石恐魔";
	AL["Focused Eye"] = "聚焦之眼";
	AL["Unblinking Eye"] = "不眠之眼";
	AL["Archritualist Kelada"] = "大祭师克拉达";
	AL["Flesh'rok the Diseased <Primordial Saurok Horror>"] = "感染者弗赖什鲁克 <源生蜥蜴恐魔>";
	AL["Zao'cho <The Emperor's Shield>"] = "曹卓 <皇帝之盾>";

end
