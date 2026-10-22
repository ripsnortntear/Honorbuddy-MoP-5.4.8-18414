-- $Id: Atlas_Scenarios-itIT.lua 1997 2013-05-18 09:19:36Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas_Scenarios", "itIT", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	for key, value in pairs(Atlas_ScenariosDB) do
		AL[value[1]] = value[6];
	end

--	AL["Scenarios Maps"] = "Scenarios Maps";

	-- Common Strings
--	AL["Final Stage"] = "Final Stage";
--	AL["Stage 1"] = "Stage 1";
--	AL["Stage 2"] = "Stage 2";
--	AL["Stage 3"] = "Stage 3";
--	AL["Stage 4"] = "Stage 4";
--	AL["Stage 5"] = "Stage 5";
--	AL["Stage 6"] = "Stage 6";
--	AL["Stage 7"] = "Stage 7";

	-- A Brewing Storm

	-- A Little Patience
--	AL["Traps"] = "Traps";
--	AL["Commander Scargash"] = "Commander Scargash";

	-- Arena of Annihilation
--	AL["Defeat Maki Waterblade"] = "Defeat Maki Waterblade";
--	AL["Defeat Satay Byu"] = "Defeat Satay Byu";
--	AL["Defeat Cloudbender Kobo"] = "Defeat Cloudbender Kobo";

	-- Assault on Zan'vess

	-- Battle on the High Seas - Alliance

	-- Battle on the High Seas - Horde

	-- Blood in the Snow

	-- Brewmoon Festival
--	AL["Karsar the Bloodletter"] = "Karsar the Bloodletter";
--	AL["Li Te"] = "Li Te";
--	AL["Den Mother Moof"] = "Den Mother Moof";
--	AL["Brewmaster Boof"] = "Brewmaster Boof";
--	AL["Defence Point"] = "Defence Point";

	-- Crypt of Forgotten Kings
--	AL["Jin Ironfist"] = "Jin Ironfist";
--	AL["Pool of Life"] = "Pool of Life";
--	AL["Treasure Urn"] = "Treasure Urn";
--	AL["Abomination of Anger"] = "Abomination of Anger";

	-- Dagger in the Dark

	-- Dark Heart of Pandaria

	-- Domination Point
--	AL["The Flight MasterDP"] = "The Flight Master";

	-- Greenstone Village

	-- Lion's Landing
--	AL["The Flight MasterLL"] = "The Flight Master";

	-- The Secrets of Ragefire

	-- Theramore's Fall - Alliance
--	AL["Horde Ships"] = "Horde Ships";
--	AL["The heart of ruined Theramore"] = "The heart of ruined Theramore";
--	AL["Jaina"] = "Jaina";
--	AL["West Gate"] = "West Gate";

	-- Theramore's Fall - Horde
--	AL["Alliance Ships"] = "Alliance Ships";
--	AL["Baldruc and Gryphon"] = "Baldruc and Gryphon";
--	AL["Tanks"] = "Tanks";
--	AL["Thalen Songweaver"] = "Thalen Songweaver";

	-- Troves of the Thunder King
--	AL["God-Hulk Gulkan"] = "God-Hulk Gulkan";
--	AL["Lever"] = "Lever";
--	AL["Tenwu of the Red Smoke"] = "Tenwu of the Red Smoke";

	-- Unga Ingoo
--	AL["Brewing Cauldron"] = "Brewing Cauldron";

end
