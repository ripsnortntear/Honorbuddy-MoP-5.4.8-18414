-- $Id: AtlasJournalEncounter.lua 2103 2013-09-12 18:19:03Z arithmandar $
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

-- Atlas JournalEncounter Integration

local AtlasLocale = LibStub("AceLocale-3.0"):GetLocale("Atlas");
local BabbleBoss = Atlas_GetLocaleLibBabble("LibBabble-Boss-3.0");

function Atlas_JournalEncounter_InstanceButton_OnClick(frame)
	local zoneID = ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone];
	local data = AtlasMaps;
	local base = data[zoneID];

	if (not EJ_GetInstanceInfo(base.JournalInstanceID)) then
		return;
	end

	if not EncounterJournal or not EncounterJournal:IsShown() then
		ToggleEncounterJournal();
	end
	EncounterJournal_ListInstances();
	EncounterJournal_DisplayInstance(base.JournalInstanceID);

	if (not EncounterJournal:IsShown()) then
		EncounterJournal:Show();
	else
		EncounterJournal:Hide();
		EncounterJournal:Show();
	end

end

function Atlas_JournalEncounter_InstanceButton_OnEnter(frame)
	local zoneID = ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone];
	local data = AtlasMaps;
	local base = data[zoneID];

	if (MouseIsOver(frame)) then
		if (EJ_GetInstanceInfo(base.JournalInstanceID)) then
			EJ_SelectInstance(base.JournalInstanceID);

			local name, description = EJ_GetInstanceInfo();

			GameTooltip:SetOwner(frame, "ANCHOR_RIGHT");
			GameTooltip:SetText(name);
			GameTooltipTextLeft1:SetTextColor(1, 1, 1);
			GameTooltip:AddLine(description, nil, nil, nil, true);
			GameTooltip:AddLine(AtlasLocale["Click to open Dungeon Journal window."], 0.5, 0.5, 1, true);
			GameTooltip:Show();
		end
	else
		GameTooltip:Hide();
	end
end

-- ------------------------------------------------------------
-- Call this function to translate boss name
-- Syntax 1: Atlas_GetBossName(bossname);
-- Syntax 2: Atlas_GetBossName(bossname, encounterID);
-- Syntax 2: Atlas_GetBossName(bossname, encounterID, creatureIndex);
-- ------------------------------------------------------------
function Atlas_GetBossName(bossname, encounterID, creatureIndex)
	if (encounterID) then
		if (creatureIndex) then
			if (EJ_GetCreatureInfo(creatureIndex, encounterID)) then
				_, bossname = EJ_GetCreatureInfo(creatureIndex, encounterID);
			end
		else 
			if (EJ_GetEncounterInfo(encounterID)) then
				bossname = EJ_GetEncounterInfo(encounterID);
			end
		end
	elseif (bossname and BabbleBoss[bossname]) then
		bossname = BabbleBoss[bossname];
	elseif (bossname and AtlasLocale[bossname]) then
		bossname = AtlasLocale[bossname];
	else
		--bossname = bossname;
	end

	return bossname;
end
