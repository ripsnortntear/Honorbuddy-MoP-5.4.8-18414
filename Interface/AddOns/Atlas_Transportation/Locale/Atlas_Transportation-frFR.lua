-- $Id: Atlas_Transportation-frFR.lua 1891 2013-02-17 15:41:01Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas_Transportation", "frFR", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	AL["The Bogpaddle Bullet"] = "La balle de Brasse-Tourbe";
	AL["Death Knight Only"] = "Chevalier de la mort seulement";	-- Taxi node in Acherus: The Ebon Hold, which is only for Death Knight
	AL["Druid-only"] = "Druide seulement";						-- Taxi node in Nighthaven, Moonglade which is only for Druid
	AL["Legend"] = "Légende";									-- The chart's legend, for example, the purple line means the portal's path
	AL["Gryphon"] = "Griffon";
	AL["Only available after winning the PvP battle"] = "Disponible uniquement après avoir remporté une bataille JcJ";
	AL["Orb of Translocation"] = "Orbe de transposition";		-- The Orb in Silvermonn City and Ruins of Lordaeron
	AL["Portals"] = "Portails";
	AL["Portal / Waygate Path to the destination"] = "Destination de Portail / Porte de transport";
	AL["Ship / Zeppelin sailing path to destination"] = "Chemin de navigation de Vaisseau / Zeppelin";
	AL["Requires honored faction with Sha'tari Skyguard"] = "Honoré avec la Garde-ciel sha'tari";
	AL["Seahorse"] = "Hippocampe";
	AL["South of the path along Lake Elune'ara"] = "Sud du chemin du lac d'Elune'ara";
	AL["Special transportation"] = "Transport spécial";
	AL["Taxi Nodes"] = "Nœuds de transports";
	AL["Transportation Maps"] = "Cartes des Transports";
	AL["Transporter"] = "Transporteur";
	AL["Transporters by the sea and on the cliff"] = "Transporteurs par la mer et sur la falaise"; -- The transporters (machine) can be found at Fuselight-by-the-Sea
	AL["West of the path to Timbermaw Hold"] = "Ouest du chemin du Repaire des Grumegueules";
	AL["Wind Rider"] = "Coursier du vent";
	AL["Won't be available once the Battle for Andorhal chain is finished."] = "N'est plus disponible une fois la série de quêtes de la Bataille pour Andorhal terminée."; -- After quest "Alas, Andorhal" (27206) is completed.
	AL["Zeppelin Towers"] = "Tours de Zeppelin";
	AL["Climbing Rope"] = "Corde d’escalade";
	AL["Rappelling Rope"] = "Corde de rappel";
	AL["Abandoned Kite"] = "Cerf-volant abandonné";
	AL["From sea level to ground level"] = "De la mer vers la terre ferme";
	AL["Whispercloud's Balloon"] = "Montgolfière de Murmure de Nuage";
	AL["Shado-Pan Rope"] = "Corde pandashan";
end