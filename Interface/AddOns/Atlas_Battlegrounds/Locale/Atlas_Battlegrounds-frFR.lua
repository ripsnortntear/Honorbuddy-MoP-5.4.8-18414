-- $Id: Atlas_Battlegrounds-frFR.lua 2043 2013-05-23 16:23:31Z Dynaletik $
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
local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "frFR", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas_Battlegrounds", "deDE", false);

if AL then
	--Common
	AL["Battleground Maps"] = "Cartes des champs de bataille";
	AL["Rescued"] = "Sauvé";
	AL["Span of 5"] = "par tranche de 5"; -- Blizzard's span to put players with similar level range into a BG (10-14, 15-29)

	--Places
	AL["AV"] = "AV/Alterac"; -- Alterac Valley
	AL["AB"] = "AB/Arathi"; -- Arathi Basin
	AL["EotS"] = "EotS/L'Œil";
	AL["IoC"] = "IoC"; -- Isle of Conquest
	AL["SotA"] = "RdA"; -- Strand of the Ancients
	AL["WSG"] = "WSG/Goulet"; -- Warsong Gulch

	--Alterac Valley (North)
	AL["Vanndar Stormpike <Stormpike General>"] = "Vanndar Foudrepique <Général foudrepique>";
	AL["Prospector Stonehewer"] = "Prospectrice Taillepierre";
	AL["Dun Baldar North Bunker"] = "Bunker Nord de Dun Baldar";
	AL["Wing Commander Mulverick"] = "Chef d'escadrille Mulverick"; --omitted from AVS
	AL["Dun Baldar South Bunker"] = "Bunker Sud de Dun Baldar";
	AL["Gaelden Hammersmith <Stormpike Supply Officer>"] = "Gaelden Forgemartel <Officier de ravitaillement foudrepique>";
	AL["Stormpike Banner"] = "Bannière foudrepique";
	AL["Stormpike Lumber Yard"] = "Entrepôt de bois Foudrepique";
	AL["Wing Commander Jeztor"] = "Chef d'escadrille Jeztor"; --omitted from AVS
	AL["Wing Commander Guse"] = "Chef d'escadrille Guse"; --omitted from AVS
	AL["Captain Balinda Stonehearth <Stormpike Captain>"] = "Capitaine Balinda Gîtepierre <Capitaine Foudrepique>";
	AL["Western Crater"] = "Cratère Ouest";
	AL["Vipore's Beacon"] = "Balise de Vipore";
	AL["Jeztor's Beacon"] = "Balise de Jeztor";
	AL["Eastern Crater"] = "Cratère Est";
	AL["Slidore's Beacon"] = "Balise de Slidore";
	AL["Guse's Beacon"] = "Balise de Guse";
	AL["Arch Druid Renferal"] = "Archidruide Ranfarouche";
	AL["Murgot Deepforge"] = "Murgot Forge-profonde";
	AL["Lana Thunderbrew <Blacksmithing Supplies>"] = "Lana Tonnebière <Fournitures de forgeron>";
	AL["Stormpike Stable Master <Stable Master>"] = "Maître des écuries Foudrepique <Maître des écuries>";
	AL["Stormpike Ram Rider Commander"] = "Commandant Chevaucheur de bélier Foudrepique";
	AL["Svalbrad Farmountain <Trade Goods>"] = "Svalbrad Mont-lointain <Fournitures d'artisanat>";
	AL["Kurdrum Barleybeard <Reagents & Poison Supplies>"] = "Kurdrum Barbe-d'orge <Composants & fournitures pour poisons>";
	AL["Stormpike Quartermaster"] = "Intendant foudrepique";
	AL["Jonivera Farmountain <General Goods>"] = "Jonivera Mont-lointain <Fournitures générales>";
	AL["Brogus Thunderbrew <Food & Drink>"] = "Brogus Tonnebière <Nourriture & boissons>";
	AL["Wing Commander Ichman"] = "Chef d'escadrille Ichman"; --omitted from AVS
	AL["Wing Commander Slidore"] = "Chef d'escadrille Slidore"; --omitted from AVS
	AL["Wing Commander Vipore"] = "Chef d'escadrille Vipore"; --omitted from AVS
	AL["Stormpike Ram Rider Commander"] = "Commandant Chevaucheur de bélier Foudrepique";
	AL["Ivus the Forest Lord"] = "Ivus le Seigneur de la forêt";
	AL["Stormpike Aid Station"] = "Poste de Secours Foudrepique";
	AL["Ichman's Beacon"] = "Balise d'Ichman";
	AL["Mulverick's Beacon"] = "Balise de Mulverick";

	--Alterac Valley (South)
	AL["Drek'Thar <Frostwolf General>"] = "Drek'Thar <Général Loup-de-givre>";
	AL["Captain Galvangar <Frostwolf Captain>"] = "Capitaine Galvangar <Capitaine Loup-de-givre>";
	AL["Iceblood Tower"] = "Tour de Glace-sang";
	AL["Tower Point"] = "Tour de la Halte";
	AL["West Frostwolf Tower"] = "Tour Loup-de-givre occidentale";
	AL["East Frostwolf Tower"] = "Tour Loup-de-givre orientale";
	AL["Frostwolf Banner"] = "Bannière Loup-de-givre";
	AL["Lokholar the Ice Lord"] = "Lokholar le Seigneur de glace";
	AL["Jotek"] = "Jotek";
	AL["Smith Regzar"] = "Forgeron Regzar";
	AL["Primalist Thurloga"] = "Primaliste Thurloga";
	AL["Frostwolf Stable Master <Stable Master>"] = "Maître des écuries Loup-de-givre <Maître des écuries>";
	AL["Frostwolf Wolf Rider Commander"] = "Commandant Chevaucheur de loup Loup-de-givre";
	AL["Frostwolf Quartermaster"] = "Intendant Loup-de-givre";
	AL["Frostwolf Relief Hut"] = "Hutte de guérison Loup-de-givre";

	--Arathi Basin

	--Warsong Gulch

	-- The Silithyst Must Flow
	AL["The Silithyst Must Flow"] = "Le silithyste doit couler à flots ";
	AL["Alliance's Camp"] = "Camp de l'Alliance";
	AL["Horde's Camp"] = "Camp de la Horde";

	--Eye of the Storm
	AL["Flag"] = "Drapeau";

	-- Halaa
	AL["Quartermaster Davian Vaclav"] = "Intendant Davian Vaclav";
	AL["Chief Researcher Kartos"] = "Directeur de recherches Kartos";
	AL["Aldraan <Blade Merchant>"] = "Aldraan <Marchand de lames>";
	AL["Cendrii <Food & Drink>"] = "Cendrii <Nourriture & boissons>";
	AL["Quartermaster Jaffrey Noreliqe"] = "Intendant Jaffrey Noreliqe";
	AL["Chief Researcher Amereldine"] = "Directrice de recherches Amereldine";
	AL["Coreiel <Blade Merchant>"] = "Coreiel <Marchande de lames>";
	AL["Embelar <Food & Drink>"] = "Embelar <Nourriture & boissons>";
	AL["Wyvern Camp"] = "Camp de la wyverne";

	-- Hellfire Peninsula PvP 
	AL["Hellfire Fortifications"] = "Fortifications des flammes infernales";

	-- Terokkar Forest PvP
	AL["Spirit Towers"] = "Tour des esprits";

	-- Zangarmarsh PvP
	AL["West Beacon"] = "Balise Ouest";
	AL["East Beacon"] = "Balise Est";
	AL["Horde Field Scout"] = "Eclaireur de terrain de la Horde";
	AL["Alliance Field Scout"] = "Eclaireur de terrain de l'Alliance";
	AL["Twinspire Graveyard"] = "Cimetière des flèches jumelles";

	--Isle of Conquest
	AL["Gates are marked with red bars."] = "Les portes sont marquées par des barres rouges.";
	AL["Overlord Agmar"] = "Seigneur Agmar";
	AL["High Commander Halford Wyrmbane <7th Legion>"] = "Haut commandant Halford Verroctone <7e Légion>";
	AL["The Refinery"] = "Raffinerie";
	AL["The Docks"] = "Docks";
	AL["The Workshop"] = "Atelier";
	AL["The Hangar"] = "Hangar";
	AL["The Quarry"] = "Carrière";
	AL["Contested Graveyards"] = "Cimetières contestés";
	AL["Horde Graveyard"] = "Cimetière de la Horde";
	AL["Alliance Graveyard"] = "Cimetière de l'Alliance";

	--Strand of the Ancients
	AL["Gates are marked with their colors."] = "Les portes sont marquées avec leur couleur.";
	AL["Attacking Team"] = "Equipe en attaque";
	AL["Defending Team"] = "Equipe en défense";
	AL["Massive Seaforium Charge"] = "Charge d'hydroglycérine massive";
	AL["Titan Relic"] = "Relique des titans";
	AL["Battleground Demolisher"] = "Démolisseur de champ de bataille";
	AL["Graveyard Flag"] = "Drapeau de cimetière";
	AL["Resurrection Point"] = "Point de résurrection";

	-- Wintergrasp
	AL["Fortress Vihecal Workshop (E)"] = "Atelier de la Forteresse (E)";
	AL["Fortress Vihecal Workshop (W)"] = "Atelier de la Forteresse (O)";
	AL["Sunken Ring Vihecal Workshop"] = "Atelier de l'arène Engloutie";
	AL["Broken Temple Vihecal Workshop"] = "Atelier du temple Brisé";
	AL["Eastspark Vihecale Workshop"] = "Atelier de l'Estincelle";
	AL["Westspark Vihecale Workshop"] = "Atelier de l'Ouestincelle";
	AL["Wintergrasp Graveyard"] = "Cimetière du Joud-d'hiver";
	AL["Sunken Ring Graveyard"] = "Cimetière de l'arène Engloutie";
	AL["Broken Temple Graveyard"] = "Cimetière du temple Brisé";
	AL["Southeast Graveyard"] = "Cimetière Sud-Est";
	AL["Southwest Graveyard"] = "Cimetière Sud-Ouest";

	-- The Battle for Gilneas

	-- Tol Barad
	AL["Attackers"] = "Attaquants";
	AL["Sergeant Parker <Baradin's Wardens>"] = "Sergent Parker <Gardiens de Baradin>";
	AL["2nd Lieutenant Wansworth <Baradin's Wardens>"] = "Sous-lieutenant Wansworth <Gardiens de Baradin>";
	AL["Commander Stevens <Baradin's Wardens>"] = "Commandant Stevens <Gardiens de Baradin>";
	AL["Marshal Fallows <Baradin's Wardens>"] = "Maréchal Fallows <Gardiens de Baradin>";
	AL["Commander Zanoth <Hellscream's Reach>"] = "Commandant Zanoth <Poing de Hurlenfer>";
	AL["Drillmaster Razgoth <Hellscream's Reach>"] = "Maître de manœuvre Razgoth <Poing de Hurlenfer>";
	AL["Private Garnoth <Hellscream's Reach>"] = "Soldat Garnoth <Poing de Hurlenfer>";
	AL["Staff Sergeant Lazgar <Hellscream's Reach>"] = "Sergent-chef Lazgar <Poing de Hurlenfer>";

	-- Twin Peaks
	AL["Wildhammer Longhouse"] = "Bastion des Marteaux-hardis";
	AL["Dragonmaw Clan Compound"] = "Forge des Gueules-de-Dragon";

	-- Silvershard Mines
	AL["Mine Cart Spawn Point"] = "Mine Cart Spawn Point";
	AL["Mine Cart Depot"] = "Mine Cart Depot";

	-- Temple of Kotmogu
	AL["Orb of Power"] = "Orb of Power";
	AL["Center Point (Maximum Points)"] = "Center Point (Maximum Points)";
end