-- $Id: Atlas_Scenarios-esES.lua 2088 2013-09-09 14:51:58Z arithmandar $
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

--[[

-- Datos de Atlas (Español)
-- Traducido por --> maqjav|Marosth de Tyrande<--
-- maqjav@hotmail.com
-- Úlltima Actualización (last update): 03/06/2013

--]]
local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Scenarios", "esES", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[3];
	end

	AL["Scenarios Maps"] = "Mapas de gestas";

	-- Common Strings
	AL["Final Stage"] = "Fase final";
	AL["Stage 1"] = "Fase 1";
	AL["Stage 2"] = "Fase 2";
	AL["Stage 3"] = "Fase 3";
	AL["Stage 4"] = "Fase 4";
	AL["Stage 5"] = "Fase 5";
	AL["Stage 6"] = "Fase 6";
	AL["Stage 7"] = "Fase 7";

	-- A Brewing Storm

	-- A Little Patience
	AL["Traps"] = "Trampas";
	AL["Commander Scargash"] = "Comandante Sangrescara";

	-- Arena of Annihilation
	AL["Defeat Maki Waterblade"] = "Derrota a Maki Espadagua";
	AL["Defeat Satay Byu"] = "Derrota a Satay Byu";
	AL["Defeat Cloudbender Kobo"] = "Derrota a Doblanubes Kobo";

	-- Assault on Zan'vess
	AL["Kyparite Quarry Sonic Control Towers"] = "Kyparite Quarry Sonic Control Towers"; --falta
	AL["Whispering Stones Sonic Control Towers"] = "Whispering Stones Sonic Control Towers"; --falta
	AL["Venomsting Pits Sonic Control Towers"] = "Venomsting Pits Sonic Control Towers"; --falta
	AL["Landing"] = "Aterrizaje";
	AL["Squad Leader Bosh"] = "Líder de escuadrón Bosh";
	AL["Commander Tel'vrak"] = "Comandante Tel'vrak";

	-- Battle on the High Seas - Alliance
	AL["Lieutenant Drak'on"] = "Teniente Drak'on";
	AL["Transport Cannon"] = "Cañón de transporte";
	AL["Rope Pile"] = "Montón de cuerda";
	AL["Steal Explosives"] = "Robar explosivos";
	AL["Lieutenant Sparklighter"] = "Teniente Chispaviva";
	AL["Lieutenant Fizzel"] = "Teniente Fizzel";
	AL["Lieutenant Blasthammer"] = "Teniente Martillón";
	AL["Run to the top deck and swing away!"] = "¡Corre a la cubierta superior y escapa con una cuerda!";
	AL["Admiral Hagman"] = "Almirante Hagman";

	-- Battle on the High Seas - Horde
	AL["Lieutenant Parker"] = "Teniente Parker";
	AL["Lieutenant Fuseblast"] = "Teniente Muchamecha";
	AL["Lieutenant Fizzboil"] = "Teniente Borboteras";
	AL["Lieutenant Boltblaster"] = "Teniente Bombarrayo";
	AL["Admiral Hodgson"] = "Almirante Hodgson";

	-- Blood in the Snow
	AL["Dark Iron Mountaineers"] = "Montaraces Hierro Negro";
	AL["Scout Stonebeard"] = "Explorador Barbapétrea";
	AL["Scout Boldbrew"] = "Exploradora Birraudaz";
	AL["Scout Forgefellow"] = "Explorador Forjaleal";
	AL["Farastu"] = "Farastu";
	AL["Hekima the Wise"] = "Hekima el Sabio";

	-- Brewmoon Festival
	AL["Karsar the Bloodletter"] = "Karsar el Sangrador";
	AL["Li Te"] = "Li Te";
	AL["Den Mother Moof"] = "Madre del cubil Moof";
	AL["Brewmaster Boof"] = "Maestro cervecero Boof";
	AL["Defence Point"] = "Lugar de defensa";

	-- Crypt of Forgotten Kings
	AL["Jin Ironfist"] = "Jin Puñoferro";
	AL["Pool of Life"] = "Poza de vida";
	AL["Treasure Urn"] = "Urna de tesoro";
	AL["Abomination of Anger"] = "Abominación de ira";

	-- Dagger in the Dark
	AL["Vol'jin"] = "Vol'jin";
	AL["Saurok Cave"] = "Cueva de Saurok";
	AL["Darkhatched Lizard-Lord"] = "Señor lagarto Incubasombras";
	AL["The Spring Drifter"] = "El Morador de la Primavera";
	AL["Cave"] = "Cueva";
	AL["Docking"] = "Embarcamiento";
	AL["Saurok Hatchery"] = "Criadero de Saurok"; --check
	AL["Broodmaster Noshi"] = "Maestro de linaje Noshi"
	AL["West Statue"] = "Estatua del oeste";
	AL["East Statue"] = "Estatua del este";
	AL["Rak'gor Bloodrazor"] = "Rak'gor Hojasangre";

	-- Dark Heart of Pandaria
	AL["Grizzle Gearslip"] = "Grizzle Engranesuelto";
	AL["Urtharges the Destroyer"] = "Urtharges el Destructor";
	AL["Crafty the Ambitious"] = "Ingenioso el Ambicioso";
	AL["Norushen"] = "Norushen";
	AL["Echo of Y'Sharrj"] = "Eco de Y'Shaarj";

	-- Domination Point
	AL["The Flight MasterDP"] = "El maestro de vuelo";
	AL["General Nazgrim"] = "General Nazgrim";
	AL["Warlord Bloodhilt"] = "Señor de la guerra Sangrastil";
	AL["Cordfellow Hack"] = "Cablero Hack";
	AL["Rivett Clutchpop"] = "Perno Rompemarchas";
	AL["Shademaster Kiryn"] = "Maestra tétrica Kiryn";
	AL["Shokia"] = "Shokia";
	AL["Joan Lorraine"] = "Joan Lorraine";

	-- Greenstone Village

	-- Lion's Landing
	AL["The Flight MasterLL"] = "El maestro de vuelo";
	AL["Daggin Windbeard"] = "Daggin Barbaviento";
	AL["Attackers"] = "Atacantes";
	AL["High Marshal Twinbraid"] = "Alto mariscal Trenzado";
	AL["Admiral Taylor"] = "Almirante Taylor";
	AL["Wolf-Rider Gaja"] = "Jinete de lobos Gaja";
	AL["Amber Kearnen"] = "Amber Kearnen";
	AL["Sully \"The Pickle\" McLeary"] = "Sully \"Liante\" McLeary";
	AL["Mishka"] = "Mishka";
	AL["Weapons"] = "Armas";
	AL["Wave One"] = "Oleada uno";
	AL["Wave Two"] = "Oleada dos";
	AL["Wave Three"] = "Oleada tres";
	AL["Thaumaturge Saresse"] = "Taumaturga Saresse";

	-- The Secrets of Ragefire
	AL["Door"] = "Puerta";
	AL["Dark Shaman Xorenth"] = "Chamán oscuro Xorenth";
	AL["Supply Crates"] = "Cajones de suministros";
	AL["Proto-drake Eggs"] = "Huevos de protodraco";
	AL["Pandaria Artifacts"] = "Artefactos de Pandaria";
	AL["Battery"] = "Bastería";
	AL["Pool Pony"] = "Poni flotador";
	AL["Sealed Gate"] = "Puerta sellada";
	AL["Cannon Balls"] = "Balas de cañón";
	AL["Broken Proto-Drake Egg"] = "Huevo de protodraco roto";
	AL["Flame Hound"] = "Can de llamas";
	AL["Kor'kron Dire Soldier"] = "Soldado temible Kor'kron";
	AL["Overseer Elaglo"] = "Sobrestante Elaglo";

	-- Theramore's Fall - Alliance
	AL["Horde Ships"] = "Barco de la Horda";
	AL["The heart of ruined Theramore"] = "El corazón de las ruinas de Theramore";
	AL["Jaina"] = "Jaina";
	AL["West Gate"] = "Puesta del oeste";

	-- Theramore's Fall - Horde
	AL["Alliance Ships"] = "Barcos de la Alianza";
	AL["Baldruc and Gryphon"] = "Baldruc y el grifo";
	AL["Tanks"] = "Tanques";
	AL["Thalen Songweaver"] = "Thalen Tejecanto";

	-- Troves of the Thunder King
	AL["God-Hulk Gulkan"] = "Dios mole Gulkan";
	AL["Lever"] = "Palanca";
	AL["Tenwu of the Red Smoke"] = "Tenwu del Humo Rojo";

	-- Unga Ingoo
	AL["Brewing Cauldron"] = "Caldero de fermentación";

end
