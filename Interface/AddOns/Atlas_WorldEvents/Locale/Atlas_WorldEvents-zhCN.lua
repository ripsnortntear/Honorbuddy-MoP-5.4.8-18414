-- $Id: Atlas_WorldEvents-zhCN.lua 68 2014-07-06 08:34:50Z arith $
--[[

	Atlas World Events, a World of Warcraft map browser for world events
	Copyright 2011 ~ 2013 Arith Hsu

	This file is a plugin of Atlas.

	Atlas World Events is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Atlas World Events is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Atlas; if not, write to the Free Software
	Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_WorldEvents", "zhCN", false);

if AL then


	-- ----------------------------------------------------------------------
	-- Common
	-- ----------------------------------------------------------------------
	AL["World Events Maps"] = "世界事件地图";
	AL["Lower"] = "下层";
	AL["Major Cities"] = "主城";
	AL["Red: "] = "红：";
	AL["Blue: "] = "蓝：";
	AL["Orange: "] = "橙：";
	AL["Purple: "] = "紫：";
	AL["Quest: "] = "任务：";
	AL["Start"] = "开始"; -- The quest started NPC
	AL["End"] = "结束"; -- The quest ended NPC
	AL["Horde Questline"] = "部落任务线";
	AL["Alliance Questline"] = "联盟任务线";
	AL["Portals"] = "传送门";
	AL["Daily Quest"] = "日常任务";

	-- ----------------------------------------------------------------------
	-- World Events
	-- ----------------------------------------------------------------------
	AL["Lunar Festival"] = "春节";
	AL["Lunar_Date"] = "1月20日～2月3日";		-- update dates for year 2014

	AL["Midsummer Fire Festival"] = "仲夏节";
	AL["Midsummer_Date"] = "6月21日～7月5日";	-- update dates for year 2014

	AL["Hallow's End"] = "万圣节";
	AL["Hallow_Date"] = "10月18日～11月1日";	-- update dates for year 2014

	AL["Brewfest"] = "美酒节";
	AL["Brewfest_Date"] = "9月20日～10月6日";

	AL["Love is in the Air"] = "情人节";
	AL["Love_Date"] = "2月10日～2月24日";		-- update dates for year 2014

	-- Darkmoon Faire
	AL["The first Sunday each month, last for a week"] = "每月第一个周日，持续一周";

	-- ----------------------------------------------------------------------
	-- Lunar Festival
	-- ----------------------------------------------------------------------
	-- Azeroth
	AL["Elder Hammershout"] = "雷锤长者";	-- 15562
	AL["Elder Stormbrow"] = "雷眉长者";	-- 15565
	AL["Elder Skychaser"] = "逐星长者";	-- 15577
	AL["Elder Winterhoof"] = "冰蹄长者";	-- 15576
	AL["Elder Starglade"] = "星林长者";	-- 15596
	AL["Elder Bellowrage"] = "风怒长者";	-- 15563
	AL["Elder Starsong"] = "星歌长者";	-- 15593
	AL["Elder Rumblerock"] = "巨石长者";	-- 15557
	AL["Elder Dawnstrider"] = "晨行长者";
	AL["Elder Ironband"] = "铁环长者";
	AL["Elder Morndeep"] = "黎明长者";
	AL["Elder Stonefort"] = "石墙长者";
	AL["Elder Goldwell"] = "金井长者";
	AL["Elder Bronzebeard"] = "铜须长者";
	AL["Elder Silvervein"] = "银脉长者";
	AL["Elder Highpeak"] = "雪峰长者";
	AL["Elder Snowcrown"] = "雪冠长者";
	AL["Elder Windrun"] = "风路长者";
	AL["Elder Farwhisper"] = "远风长者";
	AL["Elder Meadowrun"] = "幽林长者";
	AL["Elder Moonstrike"] = "月击长者";
	AL["Elder Graveborn"] = "灰石长者";
	AL["Elder Darkcore"] = "暗灵长者";
	AL["Elder Obsidian"] = "黑曜石长者";
	-- Kalimdor
	AL["Valadar Starsong"] = "瓦拉达尔·星歌";
	AL["Fariel Starsong"] = "法蕾尔·星歌";
	AL["Omen"] = "年兽";
	AL["Elder Bladeleaf"] = "锋叶长者";
	AL["Elder Bladeswift"] = "迅刀长者";
	AL["Elder Starweave"] = "星纹长者";
	AL["Elder Nightwind"] = "夜风长者";
	AL["Elder Brightspear"] = "金矛长者";
	AL["Elder Stonespire"] = "石塔长者";
	AL["Elder Skygleam"] = "夜光长者";
	AL["Elder Darkhorn"] = "暗角长者";
	AL["Runetotem the Elder"] = "符文图腾长者";	-- 15572
	AL["Elder Windtotem"] = "风之图腾长者";
	AL["Elder Moonwarden"] = "月光长者";
	AL["Elder High Mountain"] = "高山长者";
	AL["Elder Bloodhoof"] = "血蹄长者";
	AL["Elder Ezra Wheathoof"] = "长者伊萨·麦蹄";
	AL["Elder Skyseer"] = "星灵长者";
	AL["Elder Morningdew"] = "晨露长者";
	AL["Elder Dreamseer"] = "梦言长者";
	AL["Elder Wildmane"] = "蛮鬃长者";
	AL["Elder Ragetotem"] = "暴怒图腾长者";
	AL["Elder Thunderhorn"] = "雷角长者";
	AL["Elder Bladesing"] = "刃歌长者";
	AL["Elder Primestone"] = "棱石长者";
	AL["Grimtotem Elder"] = "恐怖图腾长者";
	AL["Elder Mistwalker"] = "迷雾长者";
	AL["Elder Splitrock"] = "碎石长者";
	AL["Elder Riversong"] = "水歌长者";
	-- Northrend
	AL["Elder Sardis"] = "长者萨迪斯";
	AL["Elder Pamuya"] = "长者帕姆亚";
	AL["Elder Northal"] = "长者诺塔尔";
	AL["Elder Igasho"] = "长者伊加苏";
	AL["Elder Arp"] = "雷吼长者";	-- 30364
	AL["Elder Sandrene"] = "长者杉德雷";
	AL["Elder Wanikaya"] = "长者瓦尼卡";
	AL["Elder Bluewolf"] = "蓝狼长者";
	AL["Elder Nurgen"] = "长者努尔根";
	AL["Elder Morthie"] = "长者墨尔希";
	AL["Elder Skywarden"] = "天守长者";
	AL["Elder Thoim"] = "长者索伊姆";
	AL["Elder Graymane"] = "灰鬃长者";
	AL["Elder Fargal"] = "长者法迦尔";
	AL["Elder Stonebeard"] = "石须长者";
	AL["Elder Yurauk"] = "长者尤拉克";
	AL["Elder Muraco"] = "长者穆拉克";
	AL["Elder Ohanzee"] = "长者奥哈齐";
	AL["Elder Tauros"] = "长者陶鲁斯";
	AL["Elder Beldak"] = "长者贝尔达";	-- 30357
	AL["Elder Whurain"] = "长者伍雷恩";
	AL["Elder Lunaro"] = "长者卢纳罗";
	AL["Elder Jarten"] = "长者亚尔特恩";
	AL["Elder Chogan'gada"] = "长者库甘达加";
	AL["Elder Kilias"] = "长者基里亚斯";

	-- ----------------------------------------------------------------------
	-- Hallow's End
	-- ----------------------------------------------------------------------
	AL["Trick: "] = "做表情：";
	AL["Masked Orphan Matron"] = "戴面具的孤儿院长";
	AL["Orc Commoner"] = "兽人平民";
	AL["Fire Training"] = "灭火练习";
	AL["\"Let the Fires Come!\""] = "“放火过来吧！”";
	AL["Stop the Fires!"] = "赶快灭火！";
	AL["Hallow's End Treats for Spoops!"] = "斯波普斯的万圣节糖果！";
	AL["Spoops"] = "斯波普斯";
	AL["Orgrimmar Nougat"] = "奥格瑞玛软糖";
	AL["Darkspear Gumdrop"] = "暗矛橡皮糖";
	AL["Thunder Bluff Marzipan"] = "雷霆崖奶糖";
	AL["Undercity Mint"] = "幽暗城薄荷糖";
	AL["Innkeeper Gryshka"] = "旅店老板格雷什卡";
	AL["Kali Remik"] = "卡利·雷米克";
	AL["Innkeeper Pala"] = "旅店老板帕拉";
	AL["Innkeeper Norman"] = "旅店老板诺曼";
	AL["Costumed Orphan Matron"] = "穿戏服的孤儿院长";
	AL["Human Commoner"] = "人类平民";
	AL["Fire Brigade Practice"] = "消防练习";
	AL["Hallow's End Treats for Jesper!"] = "耶斯帕尔的万圣节糖果！";
	AL["Jesper"] = "耶斯帕尔";
	AL["Stormwind Nougat"] = "暴风城杏仁糖";
	AL["Gnomeregan Gumdrop"] = "诺莫瑞根橡皮糖";
	AL["Ironforge Mint"] = "铁炉堡薄荷糖";
	AL["Darnassus Marzipan"] = "达纳苏斯水果糖";
	AL["Innkeeper Allison"] = "旅店老板奥里森";
	AL["Talvash del Kissel"] = "塔瓦斯德·基瑟尔";
	AL["Innkeeper Firebrew"] = "旅店老板洛雷·火酒";
	AL["Innkeeper Saelienne"] = "旅店老板塞琳尼";
	
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire - NPC
	AL["Selina Dourman <Darkmoon Faire Information>"] = "萨琳娜·杜洛曼 <暗月马戏团咨讯员>"; -- 10445
	AL["Zina Sharpworth <Game Tokens>"] = "吉娜·沙普沃斯 <游戏代币商人>"; -- 55266
	AL["Mola <Whack-a-Mole>"] = "莫拉 <打地鼠>"; -- 54601
	AL["Maxima Blastenheimer <Darkmoon Faire Cannoneer>"] = "玛克希玛 <暗月马戏团炮手>"; -- 15303
	AL["Rinling"] = "瑞林"; -- 14841
	AL["Sazz Coincatcher <Game Tokens>"] = "沙兹·吃币 <游戏代币商人>"; -- 55264
	AL["Gelvas Grimegate <Souvenir & Toy Prizes>"] = "吉瓦斯·格里加特 <纪念品与玩具奖品>"; -- 14828
	AL["Aimee <Pie, Pastry & Cakes>"] = "艾蜜 <馅饼和蛋糕>"; -- 29548
	AL["Finlay Coolshot <Tonk Challenge>"] = "芬雷·酷击 <蒸汽坦克大挑战>"; -- 54605
	AL["Stamp Thunderhorn <Food Vendor>"] = "斯塔姆·雷角 <食品商>"; -- 14845
	AL["Professor Thaddeus Paleo <Darkmoon Cards>"] = "萨杜斯·帕雷教授 <暗月马戏团卡片>";
	AL["Chronos <He Who Never Forgets!>"] = "克洛诺斯 <记忆力世界第一！>"; -- 14833
	AL["Sylannia <Drink Vendor>"] = "塞兰妮亚 <饮料商>"; -- 14844
	AL["Baruma <Replica Armor Prizes>"] = "巴伦玛 <仿制护甲奖励>"; -- 57983
	AL["Barum <Replica Armor Prizes>"] = "巴伦姆 <仿制护甲奖励>"; -- 55072
	AL["Daenrand Dawncrest <Heirloom Prizes>"] = "迪兰德·晨峰 <传家宝奖励>"; -- 56335
	AL["Kerri Hicks <The Strongest Woman Alive!>"] = "克莉·希克斯 <世上最强的女人！>"; -- 14832
	AL["Lhara <Pet & Mount Prizes>"] = "兰拉 <宠物与坐骑奖励>"; -- 14846
	AL["Boomie Sparks <Fireworks>"] = "波米·斯巴克 <焰火>"; -- 55278
	AL["Trixi Sharpworth <Game Tokens>"] = "崔克西·沙普沃斯 <游戏代币商人>"; -- 55339
	AL["Korgol Crushskull <The Pit Master>"] = "科戈尔·碎颅 <死斗场管理者>"; -- 55402
	AL["Sayge"] = "塞格"; -- 14822
	AL["Jessica Rogers <Ring Toss>"] = "杰西卡·罗格斯 <套圈圈>"; -- 54485
	AL["Rona Greenteeth"] = "罗纳·绿齿"; -- 56041
	AL["Tatia Brine <Fishing Supplies>"] = "塔提亚·布瑞恩 <渔具供应商>"; -- 56069
	AL["Teleportologist Fozlebub <Gone Fishin'>"] = "传送技师弗兹尔巴布 <去钓鱼>"; -- 57850
	AL["Steven Stagnaro <Fishing Trainer>"] = "斯蒂芬·斯塔格纳罗 <钓鱼训练师>"; -- 56068

	AL["Entrance (Transported)"] = "入口（传送）";
	AL["Petting Zoo"] = "爱宠动物园";
	AL["L90ETC Concert"] = "牛头人酋长音乐会";

	-- Darkmoon Faire - Quest
	AL["Target: Turtle"] = "目标：乌龟"; -- 29455
	AL["Tonk Commander"] = "坦克指挥官"; -- 29434
	AL["He Shoots, He Scores!"] = "开枪！得分！"; -- 29438
	AL["The Humanoid Cannonball"] = "炮弹飞人"; -- 29436
	AL["It's Hammer Time"] = "敲打时刻"; -- 29463
	AL["Test Your Strength"] = "实力检测"; -- 29433
	AL["Putting the Crunch in the Frog"] = "让青蛙肉更松脆"; -- 29509
	AL["Putting Trash to Good Use"] = "变废为宝"; -- 29510
	AL["Putting the Carnies Back Together Again"] = "救治伤员"; -- 29512
	AL["Keeping the Faire Sparkling"] = "光彩夺目的马戏团"; -- 29516

	-- ----------------------------------------------------------------------
	-- Brewfest
	-- ----------------------------------------------------------------------
	-- Queest
	AL["Brewfest!"] = "美酒节！"; -- 11441 / 11446
	AL["Catch the Wild Wolpertinger!"] = "抓住鹿角小飞兔！"; -- 11117 / 11431
	AL["Pink Elekks On Parade"] = "粉色的雷象"; -- 11118 / 11120
	AL["Chug and Chuck!"] = "扔酒杯！"; -- 12022 / 12191
	AL["Now This is Ram Racing... Almost."] = "赛羊会前的热身"; -- 11318 / 11409
	AL["There and Back Again"] = "来来回回"; -- 11122 / 11412
	AL["A New Supplier of Souvenirs"] = "新的纪念品供应商"; -- 29396 / 29397
	AL["Bark for the Barleybrews!"] = "为麦酒呐喊！"; -- 11293
	AL["Bark for the Thunderbrews!"] = "为雷酒呐喊！"; -- 11294
	AL["Bark for Drohn's Distillery!"] = "为德罗恩的美酒呐喊！"; -- 11407
	AL["Bark for T'chali's Voodoo Brewery!"] = "为塔卡里的巫毒酒呐喊！"; -- 11408
	AL["Brew For Brewfest"] = "美酒节佳酿"; -- 29394 / 29393
	AL["This One Time, When I Was Drunk..."] = "那次，我喝醉了之后……"; -- 12020
	AL["Brew of the Month Club"] = "本月美酒俱乐部";
	-- NPC
	AL["Dwarf Commoner"] = "矮人平民";
	AL["Gnome Commoner"] = "侏儒平民";
	AL["Night Elf Commoner"] = "暗夜精灵平民";
	AL["Human Commoner"] = "人类平民";
	AL["Draenei Commoner"] = "德莱尼平民";
	AL["Orc Commoner"] = "兽人平民";
	AL["Troll Commoner"] = "巨魔平民";
	AL["Tauren Commoner"] = "牛头人平民";
	AL["Forsaken Commoner"] = "被遗忘者平民";
	AL["Blood Elf Commoner"] = "血精灵平民";
	AL["Ipfelkofer Ironkeg"] = "埃菲库格·铁桶";
	AL["Goldark Snipehunter"] = "葛达克·猎枪";
	AL["Boxey Boltspinner"] = "波克希·博斯宾";
	AL["Neill Ramstein"] = "尼尔·拉姆斯登";
	AL["Flynn Firebrew"] = "弗莱恩·火酒";
	AL["Pol Amberstill"] = "波尔·冻石";
	AL["Belbi Quikswitch"] = "贝尔比";
	AL["Becan Barleybrew"] = "贝坎·麦酒";
	AL["Daran Thunderbrew"] = "达兰·雷酒";
	AL["Dark Iron Mole Machine Wreckage"] = "黑铁钻地机残骸"; -- 189989
	AL["Larkin Thunderbrew"] = "拉尔金·雷酒";
	AL["Ray'ma"] = "莱玛";
	AL["Tapper Swindlekeg"] = "塔波尔·斯威雷格";
	AL["Glodrak Huntsniper"] = "高达克·枪猎";
	AL["Bizzle Quicklift"] = "比兹尔·奎利弗";
	AL["Ram Master Ray"] = "赛羊管理员雷恩";
	AL["Bok Dropcertain"] = "波克·多瑟坦";
	AL["Driz Tumblequick"] = "德利兹·塔波奎克";
	AL["Blix Fixwidget"] = "布利克斯·菲克维特";
	AL["Cort Gorestein"] = "考尔特·高斯登";
	AL["Ja'ron"] = "亚隆";

	-- ----------------------------------------------------------------------
	-- Love is in the Air
	-- ----------------------------------------------------------------------
	-- Quests
	AL["Bonbon Blitz"] = "糖果闪电战";
	AL["A Perfect Puff of Perfume"] = "完美的香水";
	AL["A Cloudlet of Classy Cologne"] = "一滴优雅的古龙水";
	AL["A gift for a Lord of Ironforge"] = "为铁炉堡国王准备的礼物";
	AL["A gift for a Lord of Stormwind"] = "为暴风城之王准备的礼物";
	AL["Uncommon Scents"] = "不正常的气味";
	AL["Something Stinks"] = "恶臭的东西";
	AL["Pilfering Perfume"] = "香水行动";
	AL["Fireworks At The Gilded Rose"] = "镶金玫瑰的焰火";
	AL["Hot On The Trail"] = "热力追踪";
	AL["A Friendly Chat..."] = "友善地交谈……";
	AL["Snivel's Sweetheart"] = "斯内维尔的情人";
	AL["Something is in the Air (and it Ain't Love)"] = "微妙的气氛（肯定不是爱）";
	AL["Crushing the Crown"] = "粉碎皇冠药剂公司";
	AL["A Gift for the Prophet"] = "为先知准备的礼物";
	AL["A Gift for the High Priestess of Elune"] = "为艾露恩的高阶祭司准备的礼物";
	AL["A Gift for the Regent Lord of Quel'Thalas"] = "为奎尔萨拉斯的摄政王准备的礼物";
	AL["A Gift for the Banshee Queen"] = "为黑暗女王准备的礼物";
	AL["A Gift for the Warchief"] = "为酋长准备的礼物";
	AL["A Gift for the High Chieftain"] = "为大酋长准备的礼物";
	-- NPC
	AL["Kwee Q. Peddlefeet"] = "匹德菲特";
	AL["Public Relations Agent"] = "公关专员";
	AL["Junior Detective"] = "初级侦探";
	AL["Detective Snap Snagglebolt"] = "侦探斯奈普·滑栓";
	AL["Inspector Snip Snagglebolt"] = "探员斯尼普·滑栓";
	AL["Marion Sutton"] = "玛丽恩·桑顿";
	AL["Faded Lovely Greeting Card"] = "褪色的情人节卡片";
	AL["King Varian Wrynn"] = "瓦里安·乌瑞恩国王";
	AL["Muradin Bronzebeard"] = "穆拉丁·铜须";
	AL["Prophet Velen"] = "先知维伦";
	AL["Tyrande Whisperwind"] = "泰兰德·语风";
	AL["Lor'themar Theron"] = "洛瑟玛·塞隆";
	AL["Baine Bloodhoof"] = "贝恩·血蹄";
	AL["Lady Sylvanas Windrunner"] = "希尔瓦娜斯·风行者";
	AL["Roka"] = "洛卡";
	-- Mics.
	AL["The Gilded Rose"] = "镶金玫瑰旅店";
	AL["Gotri's Traveling Gear"] = "高特雷的旅行装备店";
	
	-- ----------------------------------------------------------------------
	-- Location Description
	-- ----------------------------------------------------------------------
	AL["(Outside the city gate)"] = "（城市大门外面）"; -- Elder Hammershout is outside the Stormwind City gate.
	AL["(Outside the gate)"] = "（大门外面）"; -- Elder Starglade is outside the gate of Zul'Gurub
	AL["(On the roof of the bank)"] = "（银行屋顶上）";
	AL["(On the ground)"] = "（地面上）";
	AL["(The roof of Scholomance, outside the dungeon)"] = "（副本外面，通灵学院屋顶）";
	AL["Southeast coast of the Lake Elune'ara"] = "月神湖东南岸";
	AL["(Next to the flight master)"] = "（飞行管理员旁边）";
	AL["(Inside the fortress)"] = "（堡垒内）"; -- The Elder Bluewolf is inside the Wintergrasp Fortress

	AL["Quest: Elune's Blessing"] = "任务：艾露恩的祝福";
end
