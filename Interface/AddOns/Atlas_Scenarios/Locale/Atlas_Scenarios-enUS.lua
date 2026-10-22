-- $Id: Atlas_Scenarios-enUS.lua 2056 2013-07-26 02:57:02Z ananhaid $
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
local AL = AceLocale:NewLocale("Atlas_Scenarios", "enUS", true);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[1];
	end

	AL["Scenarios Maps"] = "Scenarios Maps";

	-- Common Strings
	AL["Final Stage"] = "Final Stage";
	AL["Stage 1"] = "Stage 1";
	AL["Stage 2"] = "Stage 2";
	AL["Stage 3"] = "Stage 3";
	AL["Stage 4"] = "Stage 4";
	AL["Stage 5"] = "Stage 5";
	AL["Stage 6"] = "Stage 6";
	AL["Stage 7"] = "Stage 7";

	-- A Brewing Storm

	-- A Little Patience
	AL["Traps"] = "Traps";
	AL["Commander Scargash"] = "Commander Scargash";

	-- Arena of Annihilation
	AL["Defeat Maki Waterblade"] = "Defeat Maki Waterblade";
	AL["Defeat Satay Byu"] = "Defeat Satay Byu";
	AL["Defeat Cloudbender Kobo"] = "Defeat Cloudbender Kobo";

	-- Assault on Zan'vess
	AL["Kyparite Quarry Sonic Control Towers"] = "Kyparite Quarry Sonic Control Towers";
	AL["Whispering Stones Sonic Control Towers"] = "Whispering Stones Sonic Control Towers";
	AL["Venomsting Pits Sonic Control Towers"] = "Venomsting Pits Sonic Control Towers";
	AL["Landing"] = "Landing";
	AL["Squad Leader Bosh"] = "Squad Leader Bosh";
	AL["Commander Tel'vrak"] = "Commander Tel'vrak";

	-- Battle on the High Seas - Alliance
	AL["Lieutenant Drak'on"] = "Lieutenant Drak'on";
	AL["Transport Cannon"] = "Transport Cannon";
	AL["Rope Pile"] = "Rope Pile";
	AL["Steal Explosives"] = "Steal Explosives";
	AL["Lieutenant Sparklighter"] = "Lieutenant Sparklighter";
	AL["Lieutenant Fizzel"] = "Lieutenant Fizzel";
	AL["Lieutenant Blasthammer"] = "Lieutenant Blasthammer";
	AL["Run to the top deck and swing away!"] = "Run to the top deck and swing away!";
	AL["Admiral Hagman"] = "Admiral Hagman";

	-- Battle on the High Seas - Horde
	AL["Lieutenant Parker"] = "Lieutenant Parker";
	AL["Lieutenant Fuseblast"] = "Lieutenant Fuseblast";
	AL["Lieutenant Fizzboil"] = "Lieutenant Fizzboil";
	AL["Lieutenant Boltblaster"] = "Lieutenant Boltblaster";
	AL["Admiral Hodgson"] = "Admiral Hodgson";

	-- Blood in the Snow
	AL["Dark Iron Mountaineers"] = "Dark Iron Mountaineers";
	AL["Scout Stonebeard"] = "Scout Stonebeard";
	AL["Scout Boldbrew"] = "Scout Boldbrew";
	AL["Scout Forgefellow"] = "Scout Forgefellow";
	AL["Farastu"] = "Farastu";
	AL["Hekima the Wise"] = "Hekima the Wise";

	-- Brewmoon Festival
	AL["Karsar the Bloodletter"] = "Karsar the Bloodletter";
	AL["Li Te"] = "Li Te";
	AL["Den Mother Moof"] = "Den Mother Moof";
	AL["Brewmaster Boof"] = "Brewmaster Boof";
	AL["Defence Point"] = "Defence Point";

	-- Crypt of Forgotten Kings
	AL["Jin Ironfist"] = "Jin Ironfist";
	AL["Pool of Life"] = "Pool of Life";
	AL["Treasure Urn"] = "Treasure Urn";
	AL["Abomination of Anger"] = "Abomination of Anger";

	-- Dagger in the Dark
	AL["Vol'jin"] = "Vol'jin";
	AL["Saurok Cave"] = "Saurok Cave";
	AL["Darkhatched Lizard-Lord"] = "Darkhatched Lizard-Lord";
	AL["The Spring Drifter"] = "The Spring Drifter";
	AL["Cave"] = "Cave";
	AL["Docking"] = "Docking";
	AL["Saurok Hatchery"] = "Saurok Hatchery";
	AL["Broodmaster Noshi"] = "Broodmaster Noshi"
	AL["West Statue"] = "West Statue";
	AL["East Statue"] = "East Statue";
	AL["Rak'gor Bloodrazor"] = "Rak'gor Bloodrazor";

	-- Dark Heart of Pandaria
	AL["Grizzle Gearslip"] = "Grizzle Gearslip";
	AL["Urtharges the Destroyer"] = "Urtharges the Destroyer";
	AL["Crafty the Ambitious"] = "Crafty the Ambitious";
	AL["Norushen"] = "Norushen";
	AL["Echo of Y'Sharrj"] = "Echo of Y'Sharrj";

	-- Domination Point
	AL["The Flight MasterDP"] = "The Flight Master";
	AL["General Nazgrim"] = "General Nazgrim";
	AL["Warlord Bloodhilt"] = "Warlord Bloodhilt";
	AL["Cordfellow Hack"] = "Cordfellow Hack";
	AL["Rivett Clutchpop"] = "Rivett Clutchpop";
	AL["Shademaster Kiryn"] = "Shademaster Kiryn";
	AL["Shokia"] = "Shokia";
	AL["Joan Lorraine"] = "Joan Lorraine";

	-- Greenstone Village

	-- Lion's Landing
	AL["The Flight MasterLL"] = "The Flight Master";
	AL["Daggin Windbeard"] = "Daggin Windbeard";
	AL["Attackers"] = "Attackers";
	AL["High Marshal Twinbraid"] = "High Marshal Twinbraid";
	AL["Admiral Taylor"] = "Admiral Taylor";
	AL["Wolf-Rider Gaja"] = "Wolf-Rider Gaja";
	AL["Amber Kearnen"] = "Amber Kearnen";
	AL["Sully \"The Pickle\" McLeary"] = "Sully \"The Pickle\" McLeary";
	AL["Mishka"] = "Mishka";
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
	AL["Horde Ships"] = "Horde Ships";
	AL["The heart of ruined Theramore"] = "The heart of ruined Theramore";
	AL["Jaina"] = "Jaina";
	AL["West Gate"] = "West Gate";

	-- Theramore's Fall - Horde
	AL["Alliance Ships"] = "Alliance Ships";
	AL["Baldruc and Gryphon"] = "Baldruc and Gryphon";
	AL["Tanks"] = "Tanks";
	AL["Thalen Songweaver"] = "Thalen Songweaver";

	-- Troves of the Thunder King
	AL["God-Hulk Gulkan"] = "God-Hulk Gulkan";
	AL["Lever"] = "Lever";
	AL["Tenwu of the Red Smoke"] = "Tenwu of the Red Smoke";

	-- Unga Ingoo
	AL["Brewing Cauldron"] = "Brewing Cauldron";

end
