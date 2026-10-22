-- $Id: Atlas_Battlegrounds-deDE.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "Schlachtfeldkarten";
	AL["Rescued"] = "Gerettet";
	AL["Span of 5"] = "5er Schritte";

	--Places
	AL["AV"] = "AV"; -- Alterac Valley
	AL["AB"] = "AB"; -- Arathi Basin
	AL["EotS"] = "Auge";
	AL["IoC"] = "Insel";-- Isle of Conquest
	AL["SotA"] = "SdU"; -- Strand of the Ancients
	AL["WSG"] = "WS"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "Vanndar Sturmlanze <General der Sturmlanzen>";
	AL["Prospector Stonehewer"] = "Ausgrabungsleiter Steinhauer";
	AL["Dun Baldar North Bunker"] = "Nordbunker von Dun Baldar";
	AL["Wing Commander Mulverick"] = "Schwadronskommandant Mulverick";
	AL["Dun Baldar South Bunker"] = "Südbunker von Dun Baldar";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "Gaelden Hammerschmied <Versorgungsoffizier der Sturmlanzen>";
	AL["Stormpike Banner"] = "Banner der Sturmlanzen";
	AL["Stormpike Lumber Yard"] = "Sägewerk der Sturmlanzen";
	AL["Wing Commander Jeztor"] = "Schwadronskommandant Jeztor";
	AL["Wing Commander Guse"] = "Schwadronskommandant Guse";
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "Hauptmann Balinda Steinbruch <Hauptmann der Sturmlanzen>";
	AL["Western Crater"] = "Westlicher Krater";
	AL["Vipore's Beacon"] = "Vipores Signal";
	AL["Jeztor's Beacon"] = "Jeztors Signal";
	AL["Eastern Crater"] = "Östlicher Krater";
	AL["Slidore's Beacon"] = "Erzrutschs Signal";
	AL["Guse's Beacon"] = "Guses Signal";
	AL["Arch Druid Renferal"] = "Erzdruide Renferal";
	AL["Murgot Deepforge"] = "Murgot Tiefenschmied";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "Lana Donnerbräu <Schmiedekunstbedarf>";
	AL["Stormpike Stable Master <Stable Master>"] = "Stallmeister der Sturmlanzen <Stallmeister>";
	AL["Stormpike Ram Rider Commander"] = "Kommandant der Sturmlanzenwidderreiter";
	AL["Svalbrad Farmountain <Trade Goods>"] = "Svalbrad Bergweh <Handwerkswaren>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "Kurdrum Gerstenbart <Reagenzien & Giftreagenzien>";
	AL["Stormpike Quartermaster"] = "Rüstmeister der Sturmlanzen";
	AL["Jonivera Farmountain <General Goods>"] = "Jonivera Bergweh <Gemischtwaren>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "Brogus Donnerbräu <Essen & Getränke>";
	AL["Wing Commander Ichman"] = "Schwadronskommandant Ichman";
	AL["Wing Commander Slidore"] = "Schwadronskommandant Erzrutsch";
	AL["Wing Commander Vipore"] = "Schwadronskommandant Vipore";
	AL["Stormpike Ram Rider Commander"] = "Kommandant der Sturmlanzenwidderreiter";
	AL["Ivus the Forest Lord"] = "Ivus der Waldfürst";
	AL["Stormpike Aid Station"] = "Lazarett der Sturmlanzen";
	AL["Ichman's Beacon"] = "Ichmans Signal";
	AL["Mulverick's Beacon"] = "Mulvericks Signal";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "Drek'Thar <General der Frostwölfe>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "Hauptmann Galvangar <Hauptmann der Frostwölfe>";
	AL["Iceblood Tower"] = "Eisblutturm";
	AL["Tower Point"] = "Turmstellung";
	AL["West Frostwolf Tower"] = "Westlicher Frostwolfturm";
	AL["East Frostwolf Tower"] = "Östlicher Frostwolfturm";
	AL["Frostwolf Banner"] = "Banner der Frostwölfe";
	AL["Lokholar the Ice Lord"] = "Lokholar der Eislord";
	AL["Jotek"] = "Jotek";
	AL["Smith Regzar"] = "Schmied Regzar";
	AL["Primalist Thurloga"] = "Primalist Thurloga";
	AL["Frostwolf Stable Master <Stable Master>"] = "Stallmeisterin der Frostwölfe <Stallmeisterin>";
	AL["Frostwolf Wolf Rider Commander"] = "Wolfsreiterkommandant der Frostwölfe";
	AL["Frostwolf Quartermaster"] = "Rüstmeister der Frostwölfe";
	AL["Frostwolf Relief Hut"] = "Heilerhütte der Frostwölfe";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "Silithyst sammeln";
	AL["Alliance's Camp"] = "Allianzlager";
	AL["Horde's Camp"] = "Hordelager";

	--Eye of the Storm
	AL["Flag"] = "Flagge";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "Rüstmeister Davian Watzlav";
	AL["Chief Researcher Kartos"] = "Forschungsleiter Kartos";
	AL["Aldraan <Blade Merchant>"] = "Aldraan <Klingenhändler>";
	AL["Cendrii <Food & Drink>"] = "Cendrii <Speis & Trank>";
	AL["Quartermaster Jaffrey Noreliqe"] = "Rüstmeister Jaffrey Keinespuhr";
	AL["Chief Researcher Amereldine"] = "Forschungsleiterin Amereldine";
	AL["Coreiel <Blade Merchant>"] = "Coreiel <Klingenhändlerin>";
	AL["Embelar <Food & Drink>"] = "Embelar <Speis & Trank>";
	AL["Wyvern Camp"] = "Flügeldrachenlager";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "Befestigung des Höllenfeuers";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "Geistertürme";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "Westliches Leuchtsignal";
	AL["East Beacon"] = "Östliches Leuchtsignal";
	AL["Horde Field Scout"] = "Feldspäher der Horde";
	AL["Alliance Field Scout"] = "Feldspäher der Allianz";
	AL["Twinspire Graveyard"] = "Friedhof der Zwillingsspitze";

	--Isle of Conquest
    	AL["Gates are marked with red bars."] = "Tore sind mit roten Balken makiert.";
    	AL["Overlord Agmar"] = "Oberanführer Agmar";
    	AL["High Commander Halford Wyrmbane <7th Legion>"] = "Hochkommandant Halford Wyrmbann <7. Legion>";
    	AL["The Refinery"] = "Die Raffinerie";
    	AL["The Docks"] = "Die Docks";
    	AL["The Workshop"] = "Die Belagerungswerkstatt";
    	AL["The Hangar"] = "Der Hangar";
    	AL["The Quarry"] = "Der Steinbruch";
    	AL["Contested Graveyards"] = "Umkämpfte Friedhöfe";
    	AL["Horde Graveyard"] = "Horde Friedhof";
    	AL["Alliance Graveyard"] = "Allianz Friedhof";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "Tore sind in ihren Farben eingezeichnet.";
	AL["Attacking Team"] = "Angreifende Fraktion";
	AL["Defending Team"] = "Verteidigende Fraktion";
	AL["Massive Seaforium Charge"] = "Massive Zephyriumladung";
	AL["Titan Relic"] = "Relikt der Titanen";
	AL["Battleground Demolisher"] = "Schlachtfeldverwüster";
	AL["Graveyard Flag"] = "Friedhofflagge";
	AL["Resurrection Point"] = "Wiederbelebungspunkt";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "Fahrzeugwerkstatt der Feste (O)";
	AL["Fortress Vihecal Workshop (W)"] = "Fahrzeugwerkstatt der Feste (W)";
	AL["Sunken Ring Vihecal Workshop"] = "Fahrzeugwerkstatt des versunkenen Rings";
	AL["Broken Temple Vihecal Workshop"] = "Fahrzeugwerkstatt des zerbrochenen Tempels";
	AL["Eastspark Vihecale Workshop"] = "Fahrzeugwerkstatt Ostfunk";
	AL["Westspark Vihecale Workshop"] = "Fahrzeugwerkstatt Westfunk";
	AL["Wintergrasp Graveyard"] = "Friedhof der Festung";
	AL["Sunken Ring Graveyard"] = "Friedhof des versunkenen Rings";
	AL["Broken Temple Graveyard"] = "Friedhof des zerbrochenen Tempels";
	AL["Southeast Graveyard"] = "Südöstlicher Friedhof";
	AL["Southwest Graveyard"] = "Südwestlicher Friedhof";

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "Angreifer";
	AL["Sergeant Parker <Baradin's Wardens>"] = "Unteroffizier Parker <Wächter von Baradin>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "Unterleutnant Wansworth <Wächter von Baradin>";
	AL["Commander Stevens <Baradin's Wardens>"] = "Kommandant Stevens <Wächter von Baradin>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "Marschall Falbs <Wächter von Baradin>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "Kommandant Zanoth <Höllschreis Hand>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "Drillmeister Razgoth <Höllschreis Hand>";
	AL["Private Garnoth <Hellscream's Reach>"] = "Gefreiter Garnoth <Höllschreis Hand>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "Truppenoffizier Lazgar <Höllschreis Hand>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "Langhaus der Wildhämmer";
	AL["Dragonmaw Clan Compound"] = "Truppenlager des Drachenmalklans";

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Minenloren-Startpunkt";
	AL["Mine Cart Depot"] = "Minenloren-Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Kugel der Macht";
	AL["Center Point (Maximum Points)"] = "Zentrum (Maximale Punkte)";

	-- Deepwind Gorge
	AL["Center Mine"] = "Mittlere Mine";
end