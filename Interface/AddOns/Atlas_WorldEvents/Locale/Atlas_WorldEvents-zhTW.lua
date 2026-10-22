-- $Id: Atlas_WorldEvents-zhTW.lua 68 2014-07-06 08:34:50Z arith $
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
local AL = AceLocale:NewLocale("Atlas_WorldEvents", "zhTW", false);

if AL then


	-- ----------------------------------------------------------------------
	-- Common
	-- ----------------------------------------------------------------------
	AL["World Events Maps"] = "世界事件地圖";
	AL["Lower"] = "下層";
	AL["Major Cities"] = "主城";
	AL["Red: "] = "紅: ";
	AL["Blue: "] = "藍: ";
	AL["Orange: "]  = "橙: ";
	AL["Purple: "] = "紫: ";
	AL["Quest: "] = "任務: ";
	AL["Start"] = "開始"; -- The quest started NPC
	AL["End"] = "結束"; -- The quest ended NPC
	AL["Horde Questline"] = "部落任務線";
	AL["Alliance Questline"] = "聯盟任務線";
	AL["Portals"] = "傳送門";
	AL["Daily Quest"] = "每日任務";
	AL["Quest Destination"] = "任務地點";
	AL["Daily Group"] = "每日小隊任務";

	-- ----------------------------------------------------------------------
	-- World Events
	-- ----------------------------------------------------------------------
	AL["Lunar Festival"] = "新年慶典";
	AL["Lunar_Date"] = "一月20日～二月3日";		-- update dates for year 2014

	AL["Midsummer Fire Festival"] = "仲夏火焰節";
	AL["Midsummer_Date"] = "六月21日～七月5日";	-- update dates for year 2014

	AL["Hallow's End"] = "萬鬼節";
	AL["Hallow_Date"] = "十月18日～十一月1日";	-- update dates for year 2014
	
	AL["Harvest Festival"] = "收穫節";	
	AL["Harvest_Date"] = "九月2日～九月9日";		-- update dates for year 2014

	AL["Brewfest"] = "啤酒節";
	AL["Brewfest_Date"] = "九月28日～十月6日";	-- update dates for year 2014

	AL["Love is in the Air"] = "愛就在身邊";
	AL["Love_Date"] = "二月10日～二月24日";		-- update dates for year 2014

	-- Darkmoon Faire
	AL["The first Sunday each month, last for a week"] = "每月第一個週日, 維持一週";

	AL["Feast of Winter Veil"] = "冬幕節";
	AL["Winter_Date"] = "十二月16日～一月2日";	-- update dates for year 2014

	-- ----------------------------------------------------------------------
	-- Lunar Festival
	-- ----------------------------------------------------------------------
	-- Azeroth
	AL["Elder Hammershout"] = "錘嘯長者";	-- 15562
	AL["Elder Stormbrow"] = "雷眉長者";	-- 15565
	AL["Elder Skychaser"] = "逐星長者";	-- 15577
	AL["Elder Winterhoof"] = "冬蹄長者";	-- 15576
	AL["Elder Starglade"] = "星林長者";	-- 15596
	AL["Elder Bellowrage"] = "怒嘯長者";	-- 15563
	AL["Elder Starsong"] = "星歌長者";	-- 15593
	AL["Elder Rumblerock"] = "巨石長者";	-- 15557
	AL["Elder Dawnstrider"] = "晨行長者";
	AL["Elder Ironband"] = "鐵環長者";
	AL["Elder Morndeep"] = "深晨長者";
	AL["Elder Stonefort"] = "石壘長者";
	AL["Elder Goldwell"] = "金善長者";
	AL["Elder Bronzebeard"] = "銅鬚長者";
	AL["Elder Silvervein"] = "銀脈長者";
	AL["Elder Highpeak"] = "高峰長者";
	AL["Elder Snowcrown"] = "雪冠長者";
	AL["Elder Windrun"] = "風行長者";
	AL["Elder Farwhisper"] = "遙語長者";
	AL["Elder Meadowrun"] = "草奔長者";
	AL["Elder Moonstrike"] = "月擊長者";
	AL["Elder Graveborn"] = "墓生長者";
	AL["Elder Darkcore"] = "暗靈長者";
	AL["Elder Obsidian"] = "黑曜長者";
	-- Kalimdor
	AL["Valadar Starsong"] = "瓦拉達爾·星歌";
	AL["Fariel Starsong"] = "法莉爾·星歌";
	AL["Omen"] = "年獸";
	AL["Elder Bladeleaf"] = "刃葉長者";
	AL["Elder Bladeswift"] = "劍捷長者";
	AL["Elder Starweave"] = "星織長者";
	AL["Elder Nightwind"] = "夜風長者";
	AL["Elder Brightspear"] = "銳矛長者";
	AL["Elder Stonespire"] = "石塔長者";
	AL["Elder Skygleam"] = "天光長者";
	AL["Elder Darkhorn"] = "暗角長者";
	AL["Runetotem the Elder"] = "符文圖騰長者";	-- 15572
	AL["Elder Windtotem"] = "風之圖騰長者";
	AL["Elder Moonwarden"] = "月守長者";
	AL["Elder High Mountain"] = "高山長者";
	AL["Elder Bloodhoof"] = "血蹄長者";
	AL["Elder Ezra Wheathoof"] = "以斯拉·麥蹄長者";
	AL["Elder Skyseer"] = "星眼長者";
	AL["Elder Morningdew"] = "晨露長者";
	AL["Elder Dreamseer"] = "夢境先知長者";
	AL["Elder Wildmane"] = "蠻鬃長者";
	AL["Elder Ragetotem"] = "狂暴圖騰長者";
	AL["Elder Thunderhorn"] = "雷角長者";
	AL["Elder Bladesing"] = "刃歌長者";
	AL["Elder Primestone"] = "基石長者";
	AL["Grimtotem Elder"] = "恐怖圖騰長者";
	AL["Elder Mistwalker"] = "霧行長者";
	AL["Elder Splitrock"] = "劈石長者";
	AL["Elder Riversong"] = "河歌長者";
	-- Northrend
	AL["Elder Sardis"] = "沙迪斯長者";
	AL["Elder Pamuya"] = "帕姆亞長者";
	AL["Elder Northal"] = "諾爾索長者";
	AL["Elder Igasho"] = "伊加修長者";
	AL["Elder Arp"] = "阿爾普長者";	-- 30364
	AL["Elder Sandrene"] = "杉德林長者";
	AL["Elder Wanikaya"] = "瓦尼卡雅長者";
	AL["Elder Bluewolf"] = "藍狼長者";
	AL["Elder Nurgen"] = "訥金長者";
	AL["Elder Morthie"] = "莫爾希長者";
	AL["Elder Skywarden"] = "天衛長者";
	AL["Elder Thoim"] = "索依姆長者";
	AL["Elder Graymane"] = "灰鬃長者";
	AL["Elder Fargal"] = "法爾高長者";
	AL["Elder Stonebeard"] = "石鬚長者";
	AL["Elder Yurauk"] = "由羅克長者";
	AL["Elder Muraco"] = "慕拉可長者";
	AL["Elder Ohanzee"] = "歐漢茲長者";
	AL["Elder Tauros"] = "陶羅斯長者";
	AL["Elder Beldak"] = "貝爾達克長者";	-- 30357
	AL["Elder Whurain"] = "胡瑞恩長者";
	AL["Elder Lunaro"] = "魯納羅長者";
	AL["Elder Jarten"] = "加坦長者";
	AL["Elder Chogan'gada"] = "修干加達長者";
	AL["Elder Kilias"] = "奇里亞斯長者";

	-- ----------------------------------------------------------------------
	-- Hallow's End
	-- ----------------------------------------------------------------------
	AL["Trick: "] = "惡作劇: ";
	AL["Masked Orphan Matron"] = "戴面具的孤兒監護員";
	AL["Orc Commoner"] = "獸人平民";
	AL["Fire Training"] = "消防演練";
	AL["\"Let the Fires Come!\""] = "「讓火焰來吧!」";
	AL["Stop the Fires!"] = "阻止火勢!";
	AL["Hallow's End Treats for Spoops!"] = "史波普的萬鬼節糖果!";
	AL["Spoops"] = "史波普";
	AL["Orgrimmar Nougat"] = "奧格瑪牛軋糖";
	AL["Darkspear Gumdrop"] = "暗矛橡皮糖";
	AL["Thunder Bluff Marzipan"] = "雷霆崖杏仁糖";
	AL["Undercity Mint"] = "幽暗城薄荷糖";
	AL["Innkeeper Gryshka"] = "旅店老闆格雷什卡";
	AL["Kali Remik"] = "卡利·雷米克";
	AL["Innkeeper Pala"] = "旅店老闆帕拉";
	AL["Innkeeper Norman"] = "旅店老闆諾曼";
	AL["Costumed Orphan Matron"] = "變裝的孤兒監護員";
	AL["Human Commoner"] = "人類平民";
	AL["Fire Brigade Practice"] = "救火隊演練";
	AL["Hallow's End Treats for Jesper!"] = "賈斯伯的萬鬼節糖果!";
	AL["Jesper"] = "賈斯伯";
	AL["Stormwind Nougat"] = "暴風城牛軋糖";
	AL["Gnomeregan Gumdrop"] = "諾姆瑞根橡皮糖";
	AL["Ironforge Mint"] = "鐵爐堡薄荷糖";
	AL["Darnassus Marzipan"] = "達納蘇斯杏仁糖";
	AL["Innkeeper Allison"] = "旅店老闆奧里森";
	AL["Talvash del Kissel"] = "塔瓦斯德·基瑟爾";
	AL["Innkeeper Firebrew"] = "旅店老闆洛雷·火酒";
	AL["Innkeeper Saelienne"] = "旅店老闆塞琳尼";

	-- ----------------------------------------------------------------------
	-- Harvest Festival
	-- ----------------------------------------------------------------------
	-- Quest
	--AL["Honoring a Hero"] = "Honoring a Hero";
	
	-- NPC
	--AL["Javnir Nashak"] = "Javnir Nashak";
	
	--AL["Wagner Hammerstrike"] = "Wagner Hammerstrike";
	
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire - NPC
	AL["Selina Dourman <Darkmoon Faire Information>"] = "薩琳娜·杜洛曼 <暗月馬戲團諮詢員>"; -- 10445
	AL["Zina Sharpworth <Game Tokens>"] = "麗娜·夏普沃斯 <暗月馬戲團代幣>"; -- 55266
	AL["Mola <Whack-a-Mole>"] = "莫拉 <打地鼠>"; -- 54601
	AL["Maxima Blastenheimer <Darkmoon Faire Cannoneer>"] = "瑪克希瑪·布萊頓海默 <暗月馬戲團砲手>"; -- 15303
	AL["Rinling"] = "瑞林"; -- 14841
	AL["Sazz Coincatcher <Game Tokens>"] = "沙茲·捕幣手 <遊戲代幣>"; -- 55264
	AL["Gelvas Grimegate <Souvenir & Toy Prizes>"] = "吉瓦斯·葛萊蓋特 <紀念品與玩具獎品>"; -- 14828
	AL["Aimee <Pie, Pastry & Cakes>"] = "安咪 <派、餡餅與蛋糕>"; -- 29548
	AL["Finlay Coolshot <Tonk Challenge>"] = "芬雷·酷射 <坦克的挑戰>"; -- 54605
	AL["Stamp Thunderhorn <Food Vendor>"] = "史丹普·雷角 <食品商>"; -- 14845
	AL["Professor Thaddeus Paleo <Darkmoon Cards>"] = "薩杜斯·帕里歐教授 <暗月卡片>";
	AL["Chronos <He Who Never Forgets!>"] = "克洛諾斯 <記憶力世界第一!>"; -- 14833
	AL["Sylannia <Drink Vendor>"] = "希拉妮雅 <飲料商>"; -- 14844
	AL["Baruma <Replica Armor Prizes>"] = "貝倫瑪 <複製品護甲獎品>"; -- 57983
	AL["Barum <Replica Armor Prizes>"] = "貝倫 <複製品護甲獎品>"; -- 55072
	AL["Daenrand Dawncrest <Heirloom Prizes>"] = "達倫·曦冠 <家傳物品>"; -- 56335
	AL["Kerri Hicks <The Strongest Woman Alive!>"] = "克莉·希克斯 <世上最強的女人!>"; -- 14832
	AL["Lhara <Pet & Mount Prizes>"] = "蘭拉 <寵物與坐騎獎品>"; -- 14846
	AL["Boomie Sparks <Fireworks>"] = "布米·史巴克斯 <煙火>"; -- 55278
	AL["Trixi Sharpworth <Game Tokens>"] = "崔西·夏普沃斯 <遊戲代幣>"; -- 55339
	AL["Korgol Crushskull <The Pit Master>"] = "庫爾苟·碎顱 <格鬥大師>"; -- 55402
	AL["Sayge"] = "賽吉"; -- 14822
	AL["Jessica Rogers <Ring Toss>"] = "潔西卡·羅傑斯 <丟圈圈>"; -- 54485
	AL["Rona Greenteeth"] = "綠牙羅娜"; -- 56041
	AL["Tatia Brine <Fishing Supplies>"] = "塔蒂亞·布瑞恩 <釣魚供應商>"; -- 56069
	AL["Teleportologist Fozlebub <Gone Fishin'>"] = "傳送學家法蘇羅寶布 <釣魚去>"; -- 57850
	AL["Steven Stagnaro <Fishing Trainer>"] = "史蒂芬·斯塔格納洛 <釣魚訓練師>"; -- 56068

	AL["Entrance (Transported)"] = "入口（傳送）";
	AL["Petting Zoo"] = "可愛動物區";
	AL["L90ETC Concert"] = "牛頭大佬演唱會";

	-- Darkmoon Faire - Quest
	AL["Target: Turtle"] = "目標:烏龜"; -- 29455
	AL["Tonk Commander"] = "坦克指揮官"; -- 29434
	AL["He Shoots, He Scores!"] = "射擊得分!"; -- 29438
	AL["The Humanoid Cannonball"] = "人肉砲彈"; -- 29436
	AL["It's Hammer Time"] = "痛扁豺狼人！"; -- 29463
	AL["Test Your Strength"] = "力量試煉"; -- 29433
	AL["Putting the Crunch in the Frog"] = "卡啦脆蛙"; -- 29509
	AL["Putting Trash to Good Use"] = "廢物利用"; -- 29510
	AL["Putting the Carnies Back Together Again"] = "讓工作人員再次回到崗位"; -- 29512
	AL["Keeping the Faire Sparkling"] = "讓馬戲團閃閃發亮"; -- 29516

	-- ----------------------------------------------------------------------
	-- Brewfest
	-- ----------------------------------------------------------------------
	-- Queest
	AL["Brewfest!"] = "啤酒節!"; -- 11441 / 11446
	AL["Catch the Wild Wolpertinger!"] = "抓住野生鹿角兔!"; -- 11117 / 11431
	AL["Pink Elekks On Parade"] = "粉紅伊萊克遊行"; -- 11118 / 11120
	AL["Chug and Chuck!"] = "乾了再砸!"; -- 12022 / 12191
	AL["Now This is Ram Racing... Almost."] = "現在就是山羊競速...差不多。"; -- 11318 / 11409
	AL["There and Back Again"] = "來回跑"; -- 11122 / 11412
	AL["A New Supplier of Souvenirs"] = "新紀念品供應商"; -- 29396 / 29397
	AL["Bark for the Barleybrews!"] = "叫賣麥酒!"; -- 11293
	AL["Bark for the Thunderbrews!"] = "叫賣雷酒!"; -- 11294
	AL["Bark for Drohn's Distillery!"] = "為德羅恩的釀酒廠而吶喊!"; -- 11407
	AL["Bark for T'chali's Voodoo Brewery!"] = "為提洽里的巫毒釀酒廠而吶喊!"; -- 11408
	AL["Brew For Brewfest"] = "啤酒節佳釀"; -- 29394 / 29393
	AL["This One Time, When I Was Drunk..."] = "就這麼一次，在我酒醉的時候..."; -- 12020
	AL["Brew of the Month Club"] = "每月啤酒俱樂部";
	-- NPC
	AL["Dwarf Commoner"] = "矮人平民";
	AL["Gnome Commoner"] = "地精平民";
	AL["Night Elf Commoner"] = "夜精靈平民";
	AL["Human Commoner"] = "人類平民";
	AL["Draenei Commoner"] = "德萊尼平民";
	AL["Orc Commoner"] = "獸人平民";
	AL["Troll Commoner"] = "食人妖平民";
	AL["Tauren Commoner"] = "牛頭人平民";
	AL["Forsaken Commoner"] = "被遺忘者平民";
	AL["Blood Elf Commoner"] = "血精靈平民";
	AL["Ipfelkofer Ironkeg"] = "易菲克佛·鐵桶";
	AL["Goldark Snipehunter"] = "茍達克·狙獵者";
	AL["Boxey Boltspinner"] = "博克西·栓旋者";
	AL["Neill Ramstein"] = "奈爾·拉姆斯登";
	AL["Flynn Firebrew"] = "弗萊恩·火酒";
	AL["Pol Amberstill"] = "保爾·安伯斯堤爾";
	AL["Belbi Quikswitch"] = "貝爾碧·迅移";
	AL["Becan Barleybrew"] = "畢肯·麥酒";
	AL["Daran Thunderbrew"] = "達然·雷酒";
	AL["Dark Iron Mole Machine Wreckage"] = "黑鐵鑽地機殘骸"; -- 189989
	AL["Larkin Thunderbrew"] = "拉金·雷酒";
	AL["Ray'ma"] = "雷瑪";
	AL["Tapper Swindlekeg"] = "泰伯·詐桶";
	AL["Glodrak Huntsniper"] = "葛羅卓克·獵狙者";
	AL["Bizzle Quicklift"] = "比索·迅提";
	AL["Ram Master Ray"] = "山羊管理員雷";
	AL["Bok Dropcertain"] = "包克·滴確";
	AL["Driz Tumblequick"] = "德瑞茲·迅跌";
	AL["Blix Fixwidget"] = "布力克斯·修械";
	AL["Cort Gorestein"] = "寇特·高斯坦";
	AL["Ja'ron"] = "賈隆";

	-- ----------------------------------------------------------------------
	-- Love is in the Air
	-- ----------------------------------------------------------------------
	-- Quests
	AL["Bonbon Blitz"] = "甜食總動員";
	AL["A Perfect Puff of Perfume"] = "完美香氛";
	AL["A Cloudlet of Classy Cologne"] = "典雅古龍";
	AL["A gift for a Lord of Ironforge"] = "給鐵爐堡國王的禮物";
	AL["A gift for a Lord of Stormwind"] = "給暴風城國王的禮物";
	AL["Uncommon Scents"] = "香水疑雲";
	AL["Something Stinks"] = "事有蹊蹺";
	AL["Pilfering Perfume"] = "竊取香水";
	AL["Fireworks At The Gilded Rose"] = "鑲金玫瑰之戀";
	AL["Hot On The Trail"] = "重要線索";
	AL["A Friendly Chat..."] = "善意的溝通...";
	AL["Snivel's Sweetheart"] = "史尼佛的心上人";
	AL["Something is in the Air (and it Ain't Love)"] = "微妙的氣氛(肯定不是愛)";
	AL["Crushing the Crown"] = "摧毀化學製藥公司";
	AL["A Gift for the Prophet"] = "給預言者的禮物";
	AL["A Gift for the High Priestess of Elune"] = "給伊露恩的高階女祭司的禮物";
	AL["A Gift for the Regent Lord of Quel'Thalas"] = "給奎爾薩拉斯攝政王的禮物";
	AL["A Gift for the Banshee Queen"] = "給女妖之王的禮物";
	AL["A Gift for the Warchief"] = "給大酋長的禮物";
	AL["A Gift for the High Chieftain"] = "給牛頭大酋長的禮物";
	-- NPC
	AL["Kwee Q. Peddlefeet"] = "邱比特·傳播者";
	AL["Public Relations Agent"] = "公共關係仲介商";
	AL["Junior Detective"] = "資淺警探";
	AL["Detective Snap Snagglebolt"] = "警探史內卜·凸栓";
	AL["Inspector Snip Snagglebolt"] = "巡官史尼卜·凸栓";
	AL["Marion Sutton"] = "瑪莉恩·蘇頓";
	AL["Faded Lovely Greeting Card"] = "褪色的可愛邀請卡";
	AL["King Varian Wrynn"] = "瓦里安·烏瑞恩國王";
	AL["Muradin Bronzebeard"] = "穆拉丁·銅鬚";
	AL["Prophet Velen"] = "預言者費倫";
	AL["Tyrande Whisperwind"] = "泰蘭妲·語風";
	AL["Lor'themar Theron"] = "洛索瑪·塞隆";
	AL["Baine Bloodhoof"] = "貝恩·血蹄";
	AL["Lady Sylvanas Windrunner"] = "希瓦娜斯·風行者女士";
	AL["Roka"] = "蘿卡";
	-- Mics.
	AL["The Gilded Rose"] = "鑲金玫瑰";
	AL["Gotri's Traveling Gear"] = "高特雷的旅行裝備";
	
	-- ----------------------------------------------------------------------
	-- Location Description
	-- ----------------------------------------------------------------------
	AL["(Outside the city gate)"] = "（城市大門外面）"; -- Elder Hammershout is outside the Stormwind City gate.
	AL["(Outside the gate)"] = "（大門外面）"; -- Elder Starglade is outside the gate of Zul'Gurub
	AL["(On the roof of the bank)"] = "（銀行屋頂上）";
	AL["(On the ground)"] = "（地面上）";
	AL["(The roof of Scholomance, outside the dungeon)"] = "（副本外面，通靈學院屋頂）";
	AL["Southeast coast of the Lake Elune'ara"] = "月神湖東南岸";
	AL["(Next to the flight master)"] = "（飛行管理員旁邊）";
	AL["(Inside the fortress)"] = "（堡壘內）"; -- The Elder Bluewolf is inside the Wintergrasp Fortress

	AL["Quest: Elune's Blessing"] = "任務: 伊露恩的祝福";
end
