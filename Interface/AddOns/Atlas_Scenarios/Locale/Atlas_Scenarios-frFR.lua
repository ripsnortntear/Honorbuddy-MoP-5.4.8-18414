 -- $Id: Atlas_Scenarios-frFR.lua 1997 2013-05-18 09:19:36Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas_Scenarios", "frFR", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[5];
	end

	AL["Scenarios Maps"] = "Cartes des scénarios";

	-- Common Strings
	AL["Final Stage"] = "Dernière phase";
	AL["Stage 1"] = "Phase 1";
	AL["Stage 2"] = "Phase 2";
	AL["Stage 3"] = "Phase 3";
	AL["Stage 4"] = "Phase 4";
	AL["Stage 5"] = "Phase 5";
	AL["Stage 6"] = "Phase 6";
	AL["Stage 7"] = "Phase 7";

	-- Brewing Storm

	-- A Little Patience
	AL["Traps"] = "Pièges";
	AL["Commander Scargash"] = "Commandant Coupentaille";

	-- Arena of Annihilation
	AL["Defeat Maki Waterblade"] = "Vaincre Maki Lame-d’Eau";
	AL["Defeat Satay Byu"] = "Vaincre Satay Byu";
	AL["Defeat Cloudbender Kobo"] = "Vaincre Maki Lame-d’Eau";

	-- Assault on Zan'vess

	-- Battle on the High Seas - Alliance

	-- Battle on the High Seas - Horde

	-- Blood in the Snow

	-- Brewmoon Festival
	AL["Karsar the Bloodletter"] = "Karsar le Saigneur";
	AL["Li Te"] = "Li Te";
	AL["Den Mother Moof"] = "Mère de la tanière Mouf";
	AL["Brewmaster Boof"] = "Maître brasseur Giclette";
	AL["Defence Point"] = "Point de défense";

	-- Crypt of Forgotten Kings
	AL["Jin Ironfist"] = "Jin Poing de Fer";
	AL["Pool of Life"] = "Bassin de la vie";
	AL["Treasure Urn"] = "Urne au trésor";
	AL["Abomination of Anger"] = "Abomination de colère";

	-- Dagger in the Dark

	-- Dark Heart of Pandaria

	-- Domination Point
	AL["The Flight MasterDP"] = "Le maître de vol";

	-- Greenstone Village

	-- Lion's Landing
	AL["The Flight MasterLL"] = "Le maître de vol";

	-- The Secrets of Ragefire

	-- Theramore's Fall - Alliance
	AL["Horde Ships"] = "Navire de la Horde";
	AL["The heart of ruined Theramore"] = "Cœur des ruines de Theramore";
	AL["Jaina"] = "Jaina";
	AL["West Gate"] = "Porte Ouest";

	-- Theramore's Fall - Horde
	AL["Alliance Ships"] = "Navires de l’Alliance";
	AL["Baldruc and Gryphon"] = "Baldruc et  Griffon";
	AL["Tanks"] = "Tanks";
	AL["Thalen Songweaver"] = "Thalen Tissechant";

	-- Troves of the Thunder King
--	AL["God-Hulk Gulkan"] = "God-Hulk Gulkan";
--	AL["Lever"] = "Lever";
	AL["Tenwu of the Red Smoke"] = "Tenwu de la Fumée rouge";

	-- Unga Ingoo
	AL["Brewing Cauldron"] = "Chaudron de brassage";

end
