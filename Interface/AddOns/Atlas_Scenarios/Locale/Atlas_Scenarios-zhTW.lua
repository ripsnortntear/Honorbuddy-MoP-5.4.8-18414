-- $Id: Atlas_Scenarios-zhTW.lua 2056 2013-07-26 02:57:02Z ananhaid $
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
local AL = AceLocale:NewLocale("Atlas_Scenarios", "zhTW", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[11];
	end

	AL["Scenarios Maps"] = "情境地圖";

	-- Common Strings
	AL["Final Stage"] = "最後階段";
	AL["Stage 1"] = "階段 1";
	AL["Stage 2"] = "階段 2";
	AL["Stage 3"] = "階段 3";
	AL["Stage 4"] = "階段 4";
	AL["Stage 5"] = "階段 5";
	AL["Stage 6"] = "階段 6";
	AL["Stage 7"] = "階段 7";

	-- A Brewing Storm

	-- A Little Patience
	AL["Traps"] = "陷阱";
	AL["Commander Scargash"] = "指揮官疤口";

	-- Arena of Annihilation
	AL["Defeat Maki Waterblade"] = "殺死真樹·水刃";
	AL["Defeat Satay Byu"] = "殺死沙喋·鰾";
	AL["Defeat Cloudbender Kobo"] = "殺死屈雲者·柯博";

	-- Assault on Zan'vess
	AL["Kyparite Quarry Sonic Control Towers"] = "Kyparite Quarry Sonic Control Towers";
	AL["Whispering Stones Sonic Control Towers"] = "Whispering Stones Sonic Control Towers";
	AL["Venomsting Pits Sonic Control Towers"] = "Venomsting Pits Sonic Control Towers";
	AL["Landing"] = "Landing";
	AL["Squad Leader Bosh"] = "Squad Leader Bosh";
	AL["Commander Tel'vrak"] = "指揮官泰弗瑞克";

	-- Battle on the High Seas - Alliance
	AL["Lieutenant Drak'on"] = "Lieutenant Drak'on";
	AL["Transport Cannon"] = "Transport Cannon";
	AL["Rope Pile"] = "Rope Pile";
	AL["Steal Explosives"] = "偷取炸藥";
	AL["Lieutenant Sparklighter"] = "Lieutenant Sparklighter";
	AL["Lieutenant Fizzel"] = "Lieutenant Fizzel";
	AL["Lieutenant Blasthammer"] = "Lieutenant Blasthammer";
	AL["Run to the top deck and swing away!"] = "Run to the top deck and swing away!";
	AL["Admiral Hagman"] = "海格曼上將";

	-- Battle on the High Seas - Horde
	AL["Lieutenant Parker"] = "Lieutenant Parker";
	AL["Lieutenant Fuseblast"] = "Lieutenant Fuseblast";
	AL["Lieutenant Fizzboil"] = "Lieutenant Fizzboil";
	AL["Lieutenant Boltblaster"] = "Lieutenant Boltblaster";
	AL["Admiral Hodgson"] = "霍森上將";

	-- Blood in the Snow
	AL["Dark Iron Mountaineers"] = "黑鐵巡山人";
	AL["Scout Stonebeard"] = "斥候石鬚";
	AL["Scout Boldbrew"] = "斥候勇釀";
	AL["Scout Forgefellow"] = "斥候鍛扈";
	AL["Farastu"] = "Farastu";
	AL["Hekima the Wise"] = "Hekima the Wise";

	-- Brewmoon Festival
	AL["Karsar the Bloodletter"] = "『放血者』卡爾薩";
	AL["Li Te"] = "李忒";
	AL["Den Mother Moof"] = "巢母莫夫";
	AL["Brewmaster Boof"] = "釀酒大師布夫";
	AL["Defence Point"] = "防禦點";

	-- Crypt of Forgotten Kings
	AL["Jin Ironfist"] = "鐵拳阿金";
	AL["Pool of Life"] = "生命之池";
	AL["Treasure Urn"] = "藏寶罈";
	AL["Abomination of Anger"] = "憎惡獸";

	-- Dagger in the Dark
	AL["Vol'jin"] = "沃金";
	AL["Saurok Cave"] = "薩烏洛克洞穴";
	AL["Darkhatched Lizard-Lord"] = "Darkhatched Lizard-Lord";
	AL["The Spring Drifter"] = "The Spring Drifter";
	AL["Cave"] = "洞穴";
	AL["Docking"] = "Docking";
	AL["Saurok Hatchery"] = "Saurok Hatchery";
	AL["Broodmaster Noshi"] = "育主諾許"
	AL["West Statue"] = "West Statue";
	AL["East Statue"] = "East Statue";
	AL["Rak'gor Bloodrazor"] = "Rak'gor Bloodrazor";

	-- Dark Heart of Pandaria
	AL["Grizzle Gearslip"] = "格里斯·輪滑";
	AL["Urtharges the Destroyer"] = "Urtharges the Destroyer";
	AL["Crafty the Ambitious"] = "Crafty the Ambitious";
	AL["Norushen"] = "Norushen";
	AL["Echo of Y'Sharrj"] = "亞煞拉懼的分身";

	-- Domination Point
	AL["The Flight MasterDP"] = "飛行管理員";
	AL["General Nazgrim"] = "納茲格寧姆將軍";
	AL["Warlord Bloodhilt"] = "督軍血柄";
	AL["Cordfellow Hack"] = "Cordfellow Hack";
	AL["Rivett Clutchpop"] = "Rivett Clutchpop";
	AL["Shademaster Kiryn"] = "Shademaster Kiryn";
	AL["Shokia"] = "Shokia";
	AL["Joan Lorraine"] = "Joan Lorraine";

	-- Greenstone Village

	-- Lion's Landing
	AL["The Flight MasterLL"] = "飛行管理員";
	AL["Daggin Windbeard"] = "達金·風鬚";
	AL["Attackers"] = "Attackers";
	AL["High Marshal Twinbraid"] = "大元帥塔文布萊德";
	AL["Admiral Taylor"] = "泰勒上將";
	AL["Wolf-Rider Gaja"] = "Wolf-Rider Gaja";
	AL["Amber Kearnen"] = "琥珀·吉爾妮";
	AL["Sully \"The Pickle\" McLeary"] = "『醃漬王』蘇利·麥拉利";
	AL["Mishka"] = "密西卡";
	AL["Weapons"] = "Weapons";
	AL["Wave One"] = "Wave One";
	AL["Wave Two"] = "Wave Two";
	AL["Wave Three"] = "Wave Three";
	AL["Thaumaturge Saresse"] = "Thaumaturge Saresse";

	-- The Secrets of Ragefire
	AL["Door"] = "Door";
	AL["Dark Shaman Xorenth"] = "Dark Shaman Xorenth";
	AL["Supply Crates"] = "Supply Crates";
	AL["Proto-drake Eggs"] = "Proto-drake Eggs";
	AL["Pandaria Artifacts"] = "Pandaria Artifacts";
	AL["Battery"] = "Battery";
	AL["Pool Pony"] = "Pool Pony";
	AL["Sealed Gate"] = "Sealed Gate";
	AL["Cannon Balls"] = "Cannon Balls";
	AL["Broken Proto-Drake Egg"] = "Broken Proto-Drake Egg";
	AL["Flame Hound"] = "Flame Hound";
	AL["Kor'kron Dire Soldier"] = "Kor'kron Dire Soldier";
	AL["Overseer Elaglo"] = "Overseer Elaglo";

	-- Theramore's Fall - Alliance
	AL["Horde Ships"] = "部落船隻";
	AL["The heart of ruined Theramore"] = "已被摧毀的塞拉摩中心"
	AL["Jaina"] = "珍娜";
	AL["West Gate"] = "西門";

	-- Theramore's Fall - Horde
	AL["Alliance Ships"] = "聯盟船隻";
	AL["Baldruc and Gryphon"] = "巴德拉克和獅鷲獸";
	AL["Tanks"] = "坦克";
	AL["Thalen Songweaver"] = "塔倫織歌者";

	-- Troves of the Thunder King
	AL["God-Hulk Gulkan"] = "God-Hulk Gulkan";
	AL["Lever"] = "Lever";
	AL["Tenwu of the Red Smoke"] = "Tenwu of the Red Smoke";

	-- Unga Ingoo
	AL["Brewing Cauldron"] = "釀酒鍋";

end
