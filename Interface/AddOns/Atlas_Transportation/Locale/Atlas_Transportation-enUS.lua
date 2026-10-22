-- $Id: Atlas_Transportation-enUS.lua 2134 2013-12-15 09:41:39Z arithmandar $
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
local AL = AceLocale:NewLocale("Atlas_Transportation", "enUS", true);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	AL["The Bogpaddle Bullet"] = "The Bogpaddle Bullet";
	AL["Death Knight Only"] = "Death Knight Only";		-- Taxi node in Acherus: The Ebon Hold, which is only for Death Knight
	AL["Druid-only"] = "Druid-only";			-- Taxi node in Nighthaven, Moonglade which is only for Druid
	AL["Legend"] = "Legend";				-- The chart's legend, for example, the purple line means the portal's path
	AL["Gryphon"] = "Gryphon";
	AL["Only available after winning the PvP battle"] = "Only available after winning the PvP battle";
	AL["Orb of Translocation"] = "Orb of Translocation";	-- The Orb in Silvermonn City and Ruins of Lordaeron
	AL["Portals"] = "Portals";
	AL["Portal / Waygate Path to the destination"] = "Portal / Waygate Path to the destination";
	AL["Ship / Zeppelin sailing path to destination"] = "Ship / Zeppelin sailing path to destination";
	AL["Requires honored faction with Sha'tari Skyguard"] = "Requires honored faction with Sha'tari Skyguard";
	AL["Seahorse"] = "Seahorse";
	AL["South of the path along Lake Elune'ara"] = "South of the path along Lake Elune'ara";
	AL["Special transportation"] = "Special transportation";
	AL["Taxi Nodes"] = "Taxi Nodes";
	AL["Transportation Maps"] = "Transportation Maps";
	AL["Transporter"] = "Transporter";			-- The NPC who can transport you to other place
	AL["Transporters by the sea and on the cliff"] = "Transporters by the sea and on the cliff"; -- The transporters (machine) can be found at Fuselight-by-the-Sea
	AL["West of the path to Timbermaw Hold"] = "West of the path to Timbermaw Hold";
	AL["Wind Rider"] = "Wind Rider";
	AL["Won't be available once the Battle for Andorhal chain is finished."] = "Won't be available once the Battle for Andorhal chain is finished."; -- After quest "Alas, Andorhal" (27206) is completed.
	AL["Zeppelin Towers"] = "Zeppelin Towers";
	AL["Climbing Rope"] = "Climbing Rope";
	AL["Rappelling Rope"] = "Rappelling Rope";
	AL["Abandoned Kite"] = "Abandoned Kite";
	AL["From sea level to ground level"] = "From sea level to ground level";
	AL["Whispercloud's Balloon"] = "Whispercloud's Balloon";
	AL["Shado-Pan Rope"] = "Shado-Pan Rope"; -- 66390
	AL["Require to complete \"Meet the Scout\" quest line first."] = "Require to complete \"Meet the Scout\" quest line first.";
end