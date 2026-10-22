-- $Id: Atlas_Scenarios-zhCN.lua 2065 2013-08-21 08:57:50Z ananhaid $
--[[

	Atlas, a World of Warcraft instance map browser
	Copyright 2012 ~ 2013 - Arith Hsu, Atlas Team <atlas.addon@gmail.com>

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
local AL = AceLocale:NewLocale("Atlas_Scenarios", "zhCN", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[10];
	end

	AL["Scenarios Maps"] = "场景战役地图";

	-- Common Strings
	AL["Final Stage"] = "最后阶段";
	AL["Stage 1"] = "阶段 1";
	AL["Stage 2"] = "阶段 2";
	AL["Stage 3"] = "阶段 3";
	AL["Stage 4"] = "阶段 4";
	AL["Stage 5"] = "阶段 5";
	AL["Stage 6"] = "阶段 6";
	AL["Stage 7"] = "阶段 7";

	-- A Brewing Storm

	-- A Little Patience
	AL["Traps"] = "陷阱";
	AL["Commander Scargash"] = "指挥官斯卡戈什";

	-- Arena of Annihilation
	AL["Defeat Maki Waterblade"] = "击杀水刃马基";
	AL["Defeat Satay Byu"] = "击杀萨塔耶·布宇";
	AL["Defeat Cloudbender Kobo"] = "击杀驾云者科博";

	-- Assault on Zan'vess
	AL["Kyparite Quarry Sonic Control Towers"] = "凯帕琥珀采掘场声波控制塔";
	AL["Whispering Stones Sonic Control Towers"] = "低语之石声波控制塔";
	AL["Venomsting Pits Sonic Control Towers"] = "毒刺蝎巢声波控制塔";
	AL["Landing"] = "着陆";
	AL["Squad Leader Bosh"] = "小队长波什";
	AL["Commander Tel'vrak"] = "指挥官泰尔伏拉克";

	-- Battle on the High Seas - Alliance
	AL["Lieutenant Drak'on"] = "达克昂上尉";
	AL["Transport Cannon"] = "传送火炮";
	AL["Rope Pile"] = "绳子堆";
	AL["Steal Explosives"] = "窃取炸药";
	AL["Lieutenant Sparklighter"] = "火花上尉";
	AL["Lieutenant Fizzel"] = "沸腾上尉";
	AL["Lieutenant Blasthammer"] = "爆锤上尉";
	AL["Run to the top deck and swing away!"] = "跑到顶层甲板上并荡走！";
	AL["Admiral Hagman"] = "海军上将哈格曼";

	-- Battle on the High Seas - Horde
	AL["Lieutenant Parker"] = "派克上尉";
	AL["Lieutenant Fuseblast"] = "爆索上尉";
	AL["Lieutenant Fizzboil"] = "燃剂上尉";
	AL["Lieutenant Boltblaster"] = "爆栓上尉";
	AL["Admiral Hodgson"] = "海军上将霍奇逊";

	-- Blood in the Snow
	AL["Dark Iron Mountaineers"] = "黑铁巡山人";
	AL["Scout Stonebeard"] = "斥候石须";
	AL["Scout Boldbrew"] = "斥候粗酿";
	AL["Scout Forgefellow"] = "斥候炉匠";
	AL["Farastu"] = "法拉斯图";
	AL["Hekima the Wise"] = "睿智的哈基玛";

	-- Brewmoon Festival
	AL["Karsar the Bloodletter"] = "放血者卡萨尔";
	AL["Li Te"] = "李铁";
	AL["Den Mother Moof"] = "巢穴主母墨芙";
	AL["Brewmaster Boof"] = "酿酒大师布夫";
	AL["Defence Point"] = "防御点";

	-- Crypt of Forgotten Kings
	AL["Jin Ironfist"] = "金·铁拳";
	AL["Pool of Life"] = "生命之池";
	AL["Treasure Urn"] = "藏宝瓮";
	AL["Abomination of Anger"] = "怒煞畸体";

	-- Dagger in the Dark
	AL["Vol'jin"] = "沃金";
	AL["Saurok Cave"] = "蜥蜴人洞穴";
	AL["Darkhatched Lizard-Lord"] = "暗生蜥蜴领主";
	AL["The Spring Drifter"] = "溪流漂流船";
	AL["Cave"] = "洞穴";
	AL["Docking"] = "码头";
	AL["Saurok Hatchery"] = "黑暗孵化间";
	AL["Broodmaster Noshi"] = "孵化大师诺什"
	AL["West Statue"] = "西部雕像";
	AL["East Statue"] = "东部雕像";
	AL["Rak'gor Bloodrazor"] = "拉克戈尔·血刃";

	-- Dark Heart of Pandaria
	AL["Grizzle Gearslip"] = "格里兹尔·轮滑";
	AL["Urtharges the Destroyer"] = "毁灭者乌萨吉";
	AL["Crafty the Ambitious"] = "野心勃勃的克拉夫提";
	AL["Norushen"] = "诺鲁什";
	AL["Echo of Y'Sharrj"] = "亚煞极的回响";

	-- Domination Point
	AL["The Flight MasterDP"] = "飞行管理员";
	AL["General Nazgrim"] = "纳兹戈林将军";
	AL["Warlord Bloodhilt"] = "血柄督军";
	AL["Cordfellow Hack"] = "科德费勒·海克/阿罗克·碎石";
	AL["Rivett Clutchpop"] = "利维特·急刹";
	AL["Shademaster Kiryn"] = "匿踪大师琪麟";
	AL["Shokia"] = "夏琪亚";
	AL["Joan Lorraine"] = "琼·洛林/奇术士月塔/女牧师拉瑞勒";

	-- Greenstone Village

	-- Lion's Landing
	AL["The Flight MasterLL"] = "飞行管理员";
	AL["Daggin Windbeard"] = "达金·风须";
	AL["Attackers"] = "进攻者";
	AL["High Marshal Twinbraid"] = "大元帅双辫";
	AL["Admiral Taylor"] = "海军上将泰勒";
	AL["Wolf-Rider Gaja"] = "狼骑兵格夏";
	AL["Amber Kearnen"] = "安玻·吉尔妮";
	AL["Sully \"The Pickle\" McLeary"] = "“腌菜”苏利·麦克莱利";
	AL["Mishka"] = "米希卡";
	AL["Weapons"] = "武器";
	AL["Wave One"] = "第一波";
	AL["Wave Two"] = "第二波";
	AL["Wave Three"] = "第三波";
	AL["Thaumaturge Saresse"] = "魔术师萨瑞瑟/烈日行者查贡";

	-- The Secrets of Ragefire
	AL["Door"] = "门";
	AL["Dark Shaman Xorenth"] = "黑暗萨满克索伦斯";
	AL["Supply Crates"] = "补给箱";
	AL["Proto-drake Eggs"] = "始祖龙蛋";
	AL["Pandaria Artifacts"] = "潘达利亚遗物";
	AL["Battery"] = "电池";
	AL["Pool Pony"] = "池塘小马";
	AL["Sealed Gate"] = "封闭的大门";
	AL["Cannon Balls"] = "炮弹";
	AL["Broken Proto-Drake Egg"] = "破碎的始祖龙蛋";
	AL["Flame Hound"] = "烈焰猎犬";
	AL["Kor'kron Dire Soldier"] = "库卡隆恐怖战士";
	AL["Overseer Elaglo"] = "监视者伊拉格罗";

	-- Theramore's Fall - Alliance
	AL["Horde Ships"] = "部落船只";
	AL["The heart of ruined Theramore"] = "塞拉摩废墟的核心"
	AL["Jaina"] = "吉安娜";
	AL["West Gate"] = "西边城门";

	-- Theramore's Fall - Horde
	AL["Alliance Ships"] = "联盟船只";
	AL["Baldruc and Gryphon"] = "巴德拉克和狮鹫";
	AL["Tanks"] = "坦克";
	AL["Thalen Songweaver"] = "萨伦·织歌者";

	-- Troves of the Thunder King
	AL["God-Hulk Gulkan"] = "巨灵神古尔坎";
	AL["Lever"] = "杠杆";
	AL["Tenwu of the Red Smoke"] = "“赤烟”腾武";

	-- Unga Ingoo
	AL["Brewing Cauldron"] = "酿酒大锅";

end
