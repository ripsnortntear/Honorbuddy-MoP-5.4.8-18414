-- $Id: Atlas_Scenarios-deDE.lua 2038 2013-05-22 17:03:19Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas_Scenarios", "deDE", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[2];
	end

	AL["Scenarios Maps"] = "Szenariokarten";

	-- Common Strings
	AL["Final Stage"] = "Letzte Phase";
	AL["Stage 1"] = "Phase 1";
	AL["Stage 2"] = "Phase 2";
	AL["Stage 3"] = "Phase 3";
	AL["Stage 4"] = "Phase 4";
	AL["Stage 5"] = "Phase 5";
	AL["Stage 6"] = "Phase 6";
	AL["Stage 7"] = "Phase 7";

	-- A Brewing Storm

	-- A Little Patience
	AL["Traps"] = "Fallen";
	AL["Commander Scargash"] = "Kommandant Schlitznarbe";

	-- Arena of Annihilation
	AL["Defeat Maki Waterblade"] = "Bezwingt Maki Wasserklinge";
	AL["Defeat Satay Byu"] = "Bezwingt Satay Byu";
	AL["Defeat Cloudbender Kobo"] = "Bezwingt Wolkenformer Kobo";

	-- Assault on Zan'vess
	AL["Kyparite Quarry Sonic Control Towers"] = "Schallsteuerungstürme des Kyparitsteinbruchs";
	AL["Whispering Stones Sonic Control Towers"] = "Schallsteuerungstürme der Flüsternden Steine";
	AL["Venomsting Pits Sonic Control Towers"] = "Schallsteuerungstürme der Giftstechergruben";
	AL["Landing"] = "Landung";
	AL["Squad Leader Bosh"] = "Schwadronsführer Bosh";
	AL["Commander Tel'vrak"] = "Kommandant Tel'vrak";

	-- Battle on the High Seas - Alliance
	AL["Lieutenant Drak'on"] = "Leutnant Drak'on";
	AL["Transport Cannon"] = "Transportkanone";
	AL["Rope Pile"] = "Seilhaufen";
	AL["Steal Explosives"] = "Sprengladungen stehlen";
	AL["Lieutenant Sparklighter"] = "Leutnant Zündfunken";
	AL["Lieutenant Fizzel"] = "Leutnant Zischel";
	AL["Lieutenant Blasthammer"] = "Leutnant Sprenghammer";
	AL["Run to the top deck and swing away!"] = "Lauft aufs Oberdeck und schwingt Euch vom Schiff!";
	AL["Admiral Hagman"] = "Admiral Hagman";

	-- Battle on the High Seas - Horde
	AL["Lieutenant Parker"] = "Leutnant Parker";
	AL["Lieutenant Fuseblast"] = "Leutnant Luntenschuss";
	AL["Lieutenant Fizzboil"] = "Leutnant Zischelzünd";
	AL["Lieutenant Boltblaster"] = "Leutnant Bolzenspreng";
	AL["Admiral Hodgson"] = "Admiral Hodgson";

	-- Blood in the Snow
	AL["Dark Iron Mountaineers"] = "Dunkeleisengebirgsjäger";
	AL["Scout Stonebeard"] = "Späher Steinbart";
	AL["Scout Boldbrew"] = "Späherin Kühnbräu";
	AL["Scout Forgefellow"] = "Späher Schmiedebalg";
	AL["Farastu"] = "Farastu";
	AL["Hekima the Wise"] = "Hekima der Weise";

	-- Brewmoon Festival
	AL["Karsar the Bloodletter"] = "Karsar der Blutvergießer";
	AL["Li Te"] = "Li Te";
	AL["Den Mother Moof"] = "Höhlenmutter Muuf";
	AL["Brewmaster Boof"] = "Braumeister Boof";
	AL["Defence Point"] = "Verteidigungspunkt";

	-- Crypt of Forgotten Kings
	AL["Jin Ironfist"] = "Jin Eisenfaust";
	AL["Pool of Life"] = "Teich des Lebens";
	AL["Treasure Urn"] = "Schatzurne";
	AL["Abomination of Anger"] = "Monstrosität des Zorns";

	-- Dagger in the Dark
	AL["Vol'jin"] = "Vol'jin";
	AL["Saurok Cave"] = "Saurok-Höhle";
	AL["Darkhatched Lizard-Lord"] = "Echsenlord der Schattennester";
	AL["The Spring Drifter"] = "Die Frühlingsbummler";
	AL["Cave"] = "Höhle";
	AL["Docking"] = "Andocken";
	AL["Saurok Hatchery"] = "Saurok-Brutstätte";
	AL["Broodmaster Noshi"] = "Brutmeister Noshi"
	AL["West Statue"] = "Westliche Statue";
	AL["East Statue"] = "Östliche Statue";
	AL["Rak'gor Bloodrazor"] = "Rak'gor Blutklinge";

	-- Dark Heart of Pandaria
	AL["Grizzle Gearslip"] = "Grizzle Radflutsch";
	AL["Urtharges the Destroyer"] = "Urtharges der Zerstörer";
	AL["Crafty the Ambitious"] = "Pfiffikus der Clevere";
	AL["Norushen"] = "Norushen";
	AL["Echo of Y'Sharrj"] = "Echo von Y'Sharrj";

	-- Domination Point
	AL["The Flight MasterDP"] = "Die Flugmeisterin";
	AL["General Nazgrim"] = "General Nazgrim";
	AL["Warlord Bloodhilt"] = "Kriegsfürst Blutheft";
	AL["Cordfellow Hack"] = "Kabelfreund Hack";
	AL["Rivett Clutchpop"] = "Nieti Kuppelplopp";
	AL["Shademaster Kiryn"] = "Schattenmeisterin Kiryn";
	AL["Shokia"] = "Shokia";
	AL["Joan Lorraine"] = "Joan Lorraine";

	-- Greenstone Village

	-- Lion's Landing
	AL["The Flight MasterLL"] = "Der Flugmeister";
	AL["Daggin Windbeard"] = "Daggin Windbart";
	AL["Attackers"] = "Angreifer";
	AL["High Marshal Twinbraid"] = "Hochmarschall Doppelzopf";
	AL["Admiral Taylor"] = "Admiral Taylor";
	AL["Wolf-Rider Gaja"] = "Wolfsreiterin Gaja";
	AL["Amber Kearnen"] = "Amber Kearnen";
	AL["Sully \"The Pickle\" McLeary"] = "Sully \"Die Gurke\" McLeary";
	AL["Mishka"] = "Mishka";
	AL["Weapons"] = "Waffen";
	AL["Wave One"] = "Welle eins";
	AL["Wave Two"] = "Welle zwei";
	AL["Wave Three"] = "Welle drei";
	AL["Thaumaturge Saresse"] = "Thaumaturgin Saresse";

	-- The Secrets of Ragefire
	AL["Door"] = "Tür";
	AL["Dark Shaman Xorenth"] = "Dunkelschamane Xorenth";
	AL["Supply Crates"] = "Vorratskisten";
	AL["Proto-drake Eggs"] = "Protodracheneier";
	AL["Pandaria Artifacts"] = "Pandarianische Artefakte";
	AL["Battery"] = "Batterie";
	AL["Pool Pony"] = "Poolpony";
	AL["Sealed Gate"] = "Versiegeltes Tor";
	AL["Cannon Balls"] = "Kanonenkugeln";
	AL["Broken Proto-Drake Egg"] = "Zerbrochenes Protodrachenei";
	AL["Flame Hound"] = "Flammenhund";
	AL["Kor'kron Dire Soldier"] = "Terrorsoldat der Kor'kron";
	AL["Overseer Elaglo"] = "Aufseher Elaglo";

	-- Theramore's Fall - Alliance
	AL["Horde Ships"] = "Horde-Schiffe";
	AL["The heart of ruined Theramore"] = "Herz der Ruinen von Theramore";
	AL["Jaina"] = "Jaina";
	AL["West Gate"] = "Westtor";

	-- Theramore's Fall - Horde
	AL["Alliance Ships"] = "Allianz-Schiffe";
	AL["Baldruc and Gryphon"] = "Baldruc und Greif";
	AL["Tanks"] = "Panzer";
	AL["Thalen Songweaver"] = "Thalen Sangweber";

	-- Troves of the Thunder King
	AL["God-Hulk Gulkan"] = "Gottkoloss Gulkan";
	AL["Lever"] = "Hebel";
	AL["Tenwu of the Red Smoke"] = "Tenwu der Rote Rauch";

	-- Unga Ingoo
	AL["Brewing Cauldron"] = "Braukessel";

end