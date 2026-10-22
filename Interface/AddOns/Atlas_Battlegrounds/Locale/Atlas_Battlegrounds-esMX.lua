-- $Id: Atlas_Battlegrounds-esMX.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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

-- Datos de Atlas (Español)
-- Traducido por --> maqjav|Marosth de Tyrande<--
-- maqjav@hotmail.com
-- Última Actualización (last update): 28/10/2012

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "esMX", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "Mapas de Campos de Batalla";
	AL["Rescued"] = "Rescate";
	AL["Span of 5"] = "Espacio de 5"; -- Blizzard's span to put players with similar level range into a BG (10-14, 15-29)

	--Places
	AL["AV"] = "VA"; -- Alterac Valley
	AL["AB"] = "CA"; -- Arathi Basin
	AL["EotS"] = "OT";
	AL["IoC"] = "IcC"; -- Isle of Conquest
	AL["SotA"] = "PDLA"; -- Strand of the Ancients
	AL["WSG"] = "GGG"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "Vanndar Pico Tormenta <General Pico Tormenta>";
	AL["Prospector Stonehewer"] = "Prospectora Tallapiedra";
	AL["Dun Baldar North Bunker"] = "Búnker Norte de Dun Baldar";
	AL["Wing Commander Mulverick"] = "Comandante del aire Mulverick";--omitted from AVS
	AL["Dun Baldar South Bunker"] = "Búnker Sur de Dun Baldar";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "Gaelden Martillero <Oficial de suministros Pico Tormenta>";
	AL["Stormpike Banner"] = "Estandarte de Pico Tormenta";
	AL["Stormpike Lumber Yard"] = "Stormpike Lumber Yard"; --FALTA
	AL["Wing Commander Jeztor"] = "Comandante del aire Jeztor";--omitted from AVS
	AL["Wing Commander Guse"] = "Comandante del aire Guse";--omitted from AVS
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "Capitana Balinda Piedrahogar <Capitana Pico Tormenta>";
	AL["Western Crater"] = "Cráter occidental";
	AL["Vipore's Beacon"] = "Señal de Vipore";
	AL["Jeztor's Beacon"] = "Señal de Jeztor";
	AL["Eastern Crater"] = "Cráter del este";
	AL["Slidore's Beacon"] = "Señal de Slidore";
	AL["Guse's Beacon"] = "Señal de Guse";
	AL["Arch Druid Renferal"] = "Archidruida Renferal";
	AL["Murgot Deepforge"] = "Murgot Forjahonda";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "Lana Cebatruenos <Suministros de herrería>";
	AL["Stormpike Stable Master <Stable Master>"] = "Maestra de establo de Pico Tormenta <Maestra de establos>";
	AL["Stormpike Ram Rider Commander"] = "Comandante de jinetes de carneros de Pico Tormenta";
	AL["Svalbrad Farmountain <Trade Goods>"] = "Svalbrad Montelejano <Objetos comerciables>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "Kurdrum Barbacebada <Suministros de venenos y componentes>";
	AL["Stormpike Quartermaster"] = "Intendente de Pico Tormenta";
	AL["Jonivera Farmountain <General Goods>"] = "Jonivera Montelejano <Pertrechos>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "Brogus Cebatruenos <Alimentos y bebidas>";
	AL["Wing Commander Ichman"] = "Comandante del aire Ichman";--omitted from AVS
	AL["Wing Commander Slidore"] = "Comandante del aire Slidore";--omitted from AVS
	AL["Wing Commander Vipore"] = "Comandante del aire Vipore";--omitted from AVS
	AL["Stormpike Ram Rider Commander"] = "Comandante de jinetes de carneros de Pico Tormenta";
	AL["Ivus the Forest Lord"] = "Ivus el Señor del Bosque";
	AL["Stormpike Aid Station"] = "Puesto de socorro de Pico Tormenta";
	AL["Ichman's Beacon"] = "Señal de Inchman";
	AL["Mulverick's Beacon"] = "Señal de Mulverick";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "Drek'Thar <General Lobo Gélido>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "Capitán Galvangar <Capitán Lobo Gélido>";
	AL["Iceblood Tower"] = "Torre Sangre Fría";
	AL["Tower Point"] = "Punto Torre";
	AL["West Frostwolf Tower"] = "Torre Lobo Gélido Oeste";
	AL["East Frostwolf Tower"] = "Torre Lobo Gélido Este";
	AL["Frostwolf Banner"] = "Estandarte de Lobo Gélido";
	AL["Lokholar the Ice Lord"] = "Lokholar el Señor de Hielo";
	AL["Jotek"] = "Jotek";
	AL["Smith Regzar"] = "Herrero Regzar";
	AL["Primalist Thurloga"] = "Primalist Thurloga";
	AL["Frostwolf Stable Master <Stable Master>"] = "Maestra de establo Lobo Gélido <Maestro de establos>";
	AL["Frostwolf Wolf Rider Commander"] = "Comandate jinete de lobos Lobo Gélido";
	AL["Frostwolf Quartermaster"] = "Intendente Lobo Gélido";
	AL["Frostwolf Relief Hut"] = "Puesto de auxilio de Lobo Gélido";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "El silitista debe fluir"; --check
	AL["Alliance's Camp"] = "Campamento de la Alianza";
	AL["Horde's Camp"] = "Campamento de la Horda";

	--Eye of the Storm
	AL["Flag"] = "Bandera";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "Intendente Davian Vaclav";
	AL["Chief Researcher Kartos"] = "Jefe de investigación Kartos";
	AL["Aldraan <Blade Merchant>"] = "Aldraan <Mercader de armas de filo>";
	AL["Cendrii <Food & Drink>"] = "Cendrii <Alimentos y bebidas>";
	AL["Quartermaster Jaffrey Noreliqe"] = "Intendente Jaffrey Noreliqe";
	AL["Chief Researcher Amereldine"] = "Jefa de investigación Amereldine";
	AL["Coreiel <Blade Merchant>"] = "Coreiel <Mercader de armas de filo>";
	AL["Embelar <Food & Drink>"] = "Embelar <Alimentos y bebidas>";
	AL["Wyvern Camp"] = "Campamento Dracoleón";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "Fortificaciones del Fuego Infernal";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "Torres de los Espíritus";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "Baliza Occidental";
	AL["East Beacon"] = "Baliza Oriental";
	AL["Horde Field Scout"] = "Explorador de campo de la Horda";
	AL["Alliance Field Scout"] = "Explorador de campo de la Alianza";
	AL["Twinspire Graveyard"] = "Cementerio de las Agujas Gemelas"; --Check

	--Isle of Conquest
	AL["Gates are marked with red bars."] = "Las puertas están marcadas con barras rojas.";
	AL["Overlord Agmar"] = "Señor supremo Agmar";
	AL["High Commander Halford Wyrmbane <7th Legion>"] = "Alto comandante Halford Aterravermis <La Séptima Legión>";
	AL["The Refinery"] = "La Refinería";
	AL["The Docks"] = "El Astillero";
	AL["The Workshop"] = "El Taller de Asedio";
	AL["The Hangar"] = "El Hangar";
	AL["The Quarry"] = "La Cantera";
	AL["Contested Graveyards"] = "Cementerios de disputa"; --omitted from Gilneas
	AL["Horde Graveyard"] = "Cementerio de la Horda"; --omitted from Gilneas
	AL["Alliance Graveyard"] = "Cementerio de la Alianza";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "Las puertas están marcadas con sus colores.";
	AL["Attacking Team"] = "Equipo atacante";
	AL["Defending Team"] = "Equipo defendiendo";
	AL["Massive Seaforium Charge"] = "Carga de seforio enorme";
	AL["Titan Relic"] = "Reliquia de titán";
	AL["Battleground Demolisher"] = "Demoledor del campo de batalla";
	AL["Graveyard Flag"] = "Bandera del Cementerio";
	AL["Resurrection Point"] = "Punto de Resurrección";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "Taller Chispa Oriental Fortaleza"; --Check
	AL["Fortress Vihecal Workshop (W)"] = "Taller Chispa Occidental Fortaleza"; --Check
	AL["Sunken Ring Vihecal Workshop"] = "El Anillo Sumergido";
	AL["Broken Temple Vihecal Workshop"] = "El Templo Quebrado";
	AL["Eastspark Vihecale Workshop"] = "Taller Chispa Oriental";
	AL["Westspark Vihecale Workshop"] = "Taller Chispa Occidental";
	AL["Wintergrasp Graveyard"] = "Cementerio Conquista del Invierno";
	AL["Sunken Ring Graveyard"] = "Cementerio del Anillo Sumergido";
	AL["Broken Temple Graveyard"] = "Cementerio del Templo Quebrado";
	AL["Southeast Graveyard"] = "Cementerio sureste"; 
	AL["Southwest Graveyard"] = "Cementerio suroeste"; 

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "Atacantes";
	AL["Sergeant Parker <Baradin's Wardens>"] = "Sargento Parker <Celadores de Baradin>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "Segundo teniente Wansworth <Celadores de Baradin>";
	AL["Commander Stevens <Baradin's Wardens>"] = "Comandante Stevens <Celadores de Baradin>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "Alguacil Fallows <Celadores de Baradin>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "Comandante Zanoth <Mando Grito Infernal>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "Maestro de maniobras Razgoth <Mando Grito Infernal>";
	AL["Private Garnoth <Hellscream's Reach>"] = "Soldado Garnoth <Mando Grito Infernal>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "Sargento de segunda Lazgar <Mando Grito Infernal>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "Casa Martillo Salvaje"; --Check
	AL["Dragonmaw Clan Compound"] = "Compuesto Clan Faucedraco"; --Check

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Mine Cart Spawn Point";
	AL["Mine Cart Depot"] = "Mine Cart Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Orb of Power";
	AL["Center Point (Maximum Points)"] = "Center Point (Maximum Points)";

	-- Deepwind Gorge
	AL["Center Mine"] = "Mina del centro";
end
