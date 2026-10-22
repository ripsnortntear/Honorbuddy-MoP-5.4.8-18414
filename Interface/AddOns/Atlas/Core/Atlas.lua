-- $Id: Atlas.lua 2121 2013-09-16 16:36:01Z arithmandar $
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

local _G = getfenv(0);
local pairs = _G.pairs;
local math = _G.math;
local table = _G.table;

local AL = LibStub("AceLocale-3.0"):GetLocale("Atlas");
local BZ = Atlas_GetLocaleLibBabble("LibBabble-SubZone-3.0");
local LibDialog = LibStub("LibDialog-1.0");


-- Turn ON / OFF Atlas debug mode
local Atlas_DebugMode = false;
local function debug(info)
	if (Atlas_DebugMode) then
		DEFAULT_CHAT_FRAME:AddMessage("[Atlas] "..info);
	end
end

-- Initialization
ATLAS_VERSION = GetAddOnMetadata("Atlas", "Version");
ATLAS_DROPDOWNS = {};
ATLAS_INST_ENT_DROPDOWN = {};
ATLAS_NUM_LINES = 24;
ATLAS_CUR_LINES = 0;
ATLAS_SCROLL_LIST = {};
ATLAS_DATA = {};
ATLAS_SEARCH_METHOD = nil;
ATLAS_PLUGINS = {};
ATLAS_PLUGIN_DATA = {};
local GREN = "|cff66cc33";
local AtlasMap_NPC_Text_Frame_Num = 0;
local AtlasMap_Temp_Scale = GameTooltip:GetScale();

-- Only update this version number when the options have been revised and a force update is needed.
ATLAS_OLDEST_VERSION_SAME_SETTINGS = "1.24.00"; 

local DefaultAtlasOptions = {
	["AtlasVersion"] = ATLAS_OLDEST_VERSION_SAME_SETTINGS;
	["AtlasAlpha"] = 1.0;			-- Atlas frame's transparency
	["AtlasLocked"] = false;		-- Lock Atlas frame position
	["AtlasAutoSelect"] = false;		-- Auto select map
	["AtlasButtonPosition"] = 26;		-- Minimap button position
	["AtlasButtonRadius"] = 78;		-- Minimap button radius
	["AtlasButtonShown"] = true;		-- Show / hide Atlas button
	["AtlasRightClick"] = false;		-- Right click to open world map
	["AtlasType"] = 1;			-- Default or last selected map type (category)
	["AtlasZone"] = 1;			-- Default or last selected map / zone
	["AtlasAcronyms"] = true;		-- Show dungeon's acronyms
	["AtlasScale"] = 1.0;			-- Atlas frame scale
	["AtlasClamped"] = true;		-- Clamp to WoW window
	["AtlasSortBy"] = 1;			-- Maps sorting type, 1: CONTINENT; 2: LEVEL; 3: PARTYSIZE; 4: EXPANSION; 5: TYPE
	["AtlasCtrl"] = false;			-- Press ctrl and mouse over to show full description text
	["AtlasBossDesc"] = true;		-- Toggle to show boss description or not
	["AtlasBossDescScale"] = 0.9;		-- The boss description GameToolTip scale
	["AtlasDontShowInfo"] = false; 		-- Atlas latest information
	["AtlasDontShowInfo_12201"] = false;
	["AtlasCheckModule"] = true;		-- Check if there is missing module / plugin
	["AtlasColoringDropDown"] = true;	-- Coloring dungeon dropdown list with difficulty colors
};

-- Code by Grayhoof (SCT)
local function Atlas_CloneTable(tablein)	-- Return a copy of the table tablein
	local new_table = {};			-- Create a new table
	local ka, va = next(tablein, nil);	-- The ka is an index of tablein; va = tablein[ka]
	while ka do
		if type(va) == "table" then 
			va = Atlas_CloneTable(va);
		end 
		new_table[ka] = va;
		ka, va = next(tablein, ka);	-- Get next index
	end
	return new_table;
end

function Atlas_FreshOptions()
	AtlasOptions = Atlas_CloneTable(DefaultAtlasOptions);
end


-- Below to temporary create a table to store the core map's data
-- in order to identify the dropdown's zoneID is belonging to the
-- core Atlas or plugins
Atlas_CoreMapsKey = {};
Atlas_CoreMapsKey_Index = 0;
for kc, vc in pairs(AtlasMaps) do
	Atlas_CoreMapsKey[Atlas_CoreMapsKey_Index] = kc;
	Atlas_CoreMapsKey_Index = Atlas_CoreMapsKey_Index + 1;
end

Atlas_MapTypes = {};
function Atlas_RegisterPlugin(name, myCategory, myData)
	--table.insert(ATLAS_PLUGINS, name);
	ATLAS_PLUGINS[name] = {};
	local i = getn(Atlas_MapTypes) + 1;
	Atlas_MapTypes[i] = GREN..myCategory; -- Plugin category name to be added with green color, and then added to array
	
	for k, v in pairs(myData) do
		table.insert(ATLAS_PLUGINS[name], k);
		AtlasMaps[k] = v;
	end
	
	table.insert(ATLAS_PLUGIN_DATA, myData);
	
	if (ATLAS_OLD_TYPE and ATLAS_OLD_TYPE <= getn(AtlasMaps)) then
		AtlasOptions.AtlasType = ATLAS_OLD_TYPE;
		AtlasOptions.AtlasZone = ATLAS_OLD_ZONE;
	end
	
	Atlas_PopulateDropdowns();
	Atlas_Refresh();
end

function Atlas_Search(text)
	local data = nil;

	if (ATLAS_SEARCH_METHOD == nil) then
		data = ATLAS_DATA;
	else
		data = ATLAS_SEARCH_METHOD(ATLAS_DATA, text);
	end

	-- Populate the scroll frame entries list, the update func will do the rest
	local i = 1;
	while ( data[i] ~= nil ) do
		ATLAS_SCROLL_LIST[i] = data[i][1];
		i = i + 1;
	end

	ATLAS_CUR_LINES = i - 1;
end

function Atlas_SearchAndRefresh(text)
	Atlas_Search(text);
	AtlasScrollBar_Update();
end

local function Process_Deprecated()
	-- List of deprecated Atlas modules.
	-- First value is the addon name
	-- Second value is the version
	-- Nil version means NO version will EVER be loaded!
	-- Non-nil version mean ONLY IT OR NEWER versions will be loaded!
	-- Note that 2.10 isn't greater than 2.9 (2.10 >= 2.9 will fail), so the addon version number must be with the same digits
	-- For example, name it as 2.09 instead of 2.9
	local Deprecated_List = {
		-- Most recent (working) versions of known modules at time of release
		{ "Atlas_Scenarios", 		"1.26.00" },
		{ "Atlas_Cataclysm", 		"1.26.00" },
		{ "Atlas_WrathoftheLichKing", 	"1.26.00" },
		{ "Atlas_BurningCrusade", 	"1.26.00" },
		{ "Atlas_ClassicWoW", 		"1.26.00" },
		{ "Atlas_Battlegrounds", 	"1.26.00" },
		{ "Atlas_DungeonLocs", 		"1.26.00" },
		{ "Atlas_OutdoorRaids", 	"1.26.00" },
		{ "Atlas_Transportation", 	"1.26.00" },
		--{ "AtlasWorld", 		"3.3.5.25" }, -- updated July 14, 2010 -- comment out because this plugin is no longer maintained
		{ "AtlasQuest", 		"4.8.1" }, -- updated Oct. 17, 2011
		--{ "AtlasMajorCities", 		"v1.5.3" }, -- updated November 15, 2010; -- comment out because this plugin is no longer maintained
		{ "AtlasLoot", 			"7.07.00" }, -- updated Sep xx, 2013
		{ "Atlas_Arena", 		"1.3.6" }, -- updated Sep 25, 2012
		{ "Atlas_WorldEvents", 		"2.8" }, -- updated Oct 03, 2012
	};

	-- Check for outdated modules, build a list of them, then disable them and tell the player
	local OldList = {};
	for k, v in pairs(Deprecated_List) do
		local enabled, loadable = select(4, GetAddOnInfo(v[1]));
		if enabled and loadable then
			local oldVersion = true;			
			if v[2] ~= nil and GetAddOnMetadata(v[1], "Version") >= v[2] then
				oldVersion = false;
			end
			if oldVersion then
				table.insert(OldList, v[1]);
			end
		end
	end
	if table.getn(OldList) > 0 then
		local textList = "";
		for k, v in pairs(OldList) do
			textList = textList.."\n"..v..", "..GetAddOnMetadata(v, "Version");
			DisableAddOn(v);
		end
--[[
		StaticPopupDialogs["ATLAS_OLD_MODULES"] = {
			preferredIndex = 4;
			text = ATLAS_DEP_MSG1.."\n"..ATLAS_DEP_MSG2.."\n"..ATLAS_DEP_MSG3.."\n|cff6666ff"..textList.."|r";
			button1 = ATLAS_DEP_OK,
			timeout = 0,
			exclusive = 1,
			whileDead = 1,
		}
		StaticPopup_Show("ATLAS_OLD_MODULES")
]]
		LibDialog:Register("ATLAS_OLD_MODULES", {
			text = ATLAS_DEP_MSG1.."\n"..ATLAS_DEP_MSG2.."\n"..ATLAS_DEP_MSG3.."\n|cff6666ff"..textList.."|r",
			buttons = {
				text = ATLAS_DEP_OK,
			},
			show_while_dead = false,
			hide_on_escape = true,
		});
		LibDialog:Spawn("ATLAS_OLD_MODULES");
	end
end

-- Called when the Atlas frame is first loaded
-- We CANNOT assume that data in other files is available yet!
function Atlas_OnLoad(self)

	Process_Deprecated();

	-- Register the Atlas frame for the following events
	self:RegisterEvent("PLAYER_LOGIN");
	self:RegisterEvent("ADDON_LOADED");

	-- Allows Atlas to be closed with the Escape key
	tinsert(UISpecialFrames, "AtlasFrame");
	
	-- Dragging involves some special registration
	self:RegisterForDrag("LeftButton");
end

-- Removal of articles in map names (for proper alphabetic sorting)
-- For example: "The Deadmines" will become "Deadmines"
-- Thus it will be sorted under D and not under T
local function Atlas_SanitizeName(text)
   text = string.lower(text);
   if (AtlasSortIgnore) then
	   for _, v in pairs(AtlasSortIgnore) do
		   local match; 
           if (string.gmatch) then 
                match = string.gmatch(text, v)();
           else 
                match = string.gfind(text, v)(); 
           end
		   if (match) and ((string.len(text) - string.len(match)) <= 4) then
			   return match;
		   end
	   end
   end
   return text;
end

-- Comparator function for alphabetic sorting of maps
-- Yey, one function for everything
local function Atlas_SortZonesAlpha(a, b)
	local aa = Atlas_SanitizeName(AtlasMaps[a].ZoneName[1]);
	local bb = Atlas_SanitizeName(AtlasMaps[b].ZoneName[1]);
	return aa < bb;
end

-- Main Atlas event handler
function Atlas_OnEvent(self, event, ...)
	local arg1 = ...;
	if (event == "ADDON_LOADED" and arg1 == "Atlas") then
		Atlas_Init();
	end
	
end

function Atlas_PopulateDropdowns()
	local i = 1;
	local catName = Atlas_DropDownLayouts_Order[AtlasOptions.AtlasSortBy];
	local subcatOrder = Atlas_DropDownLayouts_Order[catName];
	for n = 1, getn(subcatOrder), 1 do
		local subcatItems = Atlas_DropDownLayouts[catName][subcatOrder[n]];

		ATLAS_DROPDOWNS[n] = {};

		for k,v in pairs(subcatItems) do
			table.insert(ATLAS_DROPDOWNS[n], v);
		end

		table.sort(ATLAS_DROPDOWNS[n], Atlas_SortZonesAlpha);

		i = n + 1;
	end
	
	if (ATLAS_PLUGIN_DATA) then
		for ka, va in pairs(ATLAS_PLUGIN_DATA) do

			ATLAS_DROPDOWNS[i] = {};

			for kb,vb in pairs(va) do
				if (type(vb) == "table") then
					table.insert(ATLAS_DROPDOWNS[i], kb);
				end
			end

			table.sort(ATLAS_DROPDOWNS[i], Atlas_SortZonesAlpha);

			i = i + 1;
		end	
	end
end

-- Detect if not all modules / plugins are installed
local function Atlas_Check_Modules()
	if (AtlasOptions["AtlasCheckModule"] == nil) then
		AtlasOptions["AtlasCheckModule"] = true;
	end
	if (AtlasOptions["AtlasCheckModule"] == false) then
		return;
	end
	local Module_List = {
		"Atlas_Scenarios",
		"Atlas_Cataclysm",
		"Atlas_WrathoftheLichKing",
		"Atlas_BurningCrusade",
		"Atlas_ClassicWoW",
		"Atlas_Battlegrounds",
		"Atlas_DungeonLocs",
		"Atlas_OutdoorRaids",
		"Atlas_Transportation",
	};

	-- Check for outdated modules, build a list of them, then disable them and tell the player
	local List = {};
	for _,module in pairs(Module_List) do
		local enabled, loadable = select(4, GetAddOnInfo(module));
		if (not enabled) or (not loadable) then
			table.insert(List, module);
		end
	end
	if table.getn(List) > 0 then
		local textList = "";
		for _,str in pairs(List) do
			textList = textList.."\n"..str;
		end
--[[
		StaticPopupDialogs["DetectMissing"] = {
			preferredIndex = 4;
			text = AL["ATLAS_MISSING_MODULE"].."\n|cff6666ff"..textList.."|r\n\n"..AL["ATLAS_INFO_12200"];
			button1 = ATLAS_DEP_OK,
			timeout = 0,
			exclusive = 1,
			whileDead = 1,
		}
		StaticPopup_Show("DetectMissing")
]]
		LibDialog:Register("DetectMissing", {
			text = AL["ATLAS_MISSING_MODULE"].."\n|cff6666ff"..textList.."|r\n\n"..AL["ATLAS_INFO_12200"],
			buttons = {
				text = ATLAS_DEP_OK,
			},
			show_while_dead = false,
			hide_on_escape = true,
		});
		LibDialog:Spawn("DetectMissing");
	end
end

-- Function to pop up a window to show the latest addon information
function Atlas_ShowInfo()
	if (AtlasOptions["AtlasDontShowInfo_12201"]) then
		return;
	else
		AtlasInfoFrame:Show();
		AtlasInfoFrameToggleButton:SetChecked(AtlasOptions.AtlasDontShowInfo_12201);
	end
end

function Atlas_ShowInfo_Toggle()
	if (AtlasOptions["AtlasDontShowInfo_12201"]) then
		AtlasOptions["AtlasDontShowInfo_12201"] = false;
	else
		AtlasOptions["AtlasDontShowInfo_12201"] = true;
	end
	AtlasInfoFrameToggleButton:SetChecked(AtlasOptions.AtlasDontShowInfo_12201);
end

ATLAS_OLD_TYPE = false;
ATLAS_OLD_ZONE = false;

function Atlas_InitOptions()
	-- Init saved vars for a new install
	if ( AtlasOptions == nil ) then
		Atlas_FreshOptions();
	end
	-- Init the newly added "AtlasBossDescScale" and don't bother user to reset everything
	-- Can be removed after 1.21.0 release
	if (AtlasOptions["AtlasBossDescScale"] == nil) then
		AtlasOptions["AtlasBossDescScale"] = 0.9;
	end
	if (AtlasOptions["AtlasBossDesc"] == nil) then
		AtlasOptions["AtlasBossDesc"] = true;
	end

	if (AtlasOptions["AtlasDontShowInfo_12201"] == nil) then
		AtlasOptions["AtlasDontShowInfo_12201"] = false;
	end
	
	if (AtlasOptions["AtlasCheckModule"] == nil) then
		AtlasOptions["AtlasCheckModule"] = true;
	end
	
	if (AtlasOptions["AtlasColoringDropDown"] == nil) then
		AtlasOptions["AtlasColoringDropDown"] = true;
	end
	
	--saved options version check
	if (AtlasOptions["AtlasVersion"] ~= ATLAS_OLDEST_VERSION_SAME_SETTINGS) then
		Atlas_FreshOptions();
	end
end

-- Initializes everything relating to saved variables and data in other lua files
-- This should be called ONLY when we're sure our variables are in memory
function Atlas_Init()

	-- Make the Atlas window go all the way to the edge of the screen, exactly
	AtlasFrame:SetClampRectInsets(12, 0, -12, 0);

	Atlas_InitOptions();

	-- Populate the dropdown lists...yeeeah this is so much nicer!
	Atlas_PopulateDropdowns();
	
	if (not ATLAS_DROPDOWNS[AtlasOptions.AtlasType]) then
		ATLAS_OLD_TYPE = AtlasOptions.AtlasType;
		ATLAS_OLD_ZONE = AtlasOptions.AtlasZone;
		AtlasOptions.AtlasType = 1;
		AtlasOptions.AtlasZone = 1;
	end
	
	-- Now that saved variables have been loaded, update everything accordingly
	Atlas_Refresh();
	Atlas_UpdateLock();
	Atlas_UpdateAlpha();
	AtlasFrame:SetClampedToScreen(AtlasOptions.AtlasClamped);
	--AtlasButton_UpdatePosition();
	AtlasOptions_Init();
	
	-- Make an LDB object
	LibStub:GetLibrary("LibDataBroker-1.1"):NewDataObject("Atlas", {
		type = "launcher",
		text = "Atlas",
		OnClick = function(self, button)
			if button == "LeftButton" then
				Atlas_Toggle();
			elseif button == "RightButton" then
				AtlasOptions_Toggle();
			end
		end,
		icon = "Interface\\WorldMap\\WorldMap-Icon",
		OnTooltipShow = function(tooltip)
			if not tooltip or not tooltip.AddLine then return end
			tooltip:AddLine("|cffffffff"..ATLAS_TITLE)
			tooltip:AddLine(ATLAS_LDB_HINT)
		end,
	})
	
	Atlas_Check_Modules();
	if (AtlasOptions["AtlasDontShowInfo_12201"]) then
		Atlas_ShowInfo();
	end
end

-- Simple function to toggle the Atlas frame's lock status and update it's appearance
function Atlas_ToggleLock()
	AtlasOptions_ToggleLock();
end

-- Updates the appearance of the lock button based on the status of AtlasLocked
function Atlas_UpdateLock()
	if (AtlasOptions.AtlasLocked) then
		AtlasLockNorm:SetTexture("Interface\\AddOns\\Atlas\\Images\\LockButton-Locked-Up");
		AtlasLockPush:SetTexture("Interface\\AddOns\\Atlas\\Images\\LockButton-Locked-Down");
	else
		AtlasLockNorm:SetTexture("Interface\\AddOns\\Atlas\\Images\\LockButton-Unlocked-Up");
		AtlasLockPush:SetTexture("Interface\\AddOns\\Atlas\\Images\\LockButton-Unlocked-Down");
	end
end

-- Begin moving the Atlas frame if it's unlocked
function Atlas_StartMoving(self)
	if (not AtlasOptions.AtlasLocked) then
		self:StartMoving();
	end
end

-- Parses slash commands
-- If an un-recognized command is given, toggle Atlas
function Atlas_SlashCommand(msg)
	if (msg == ATLAS_SLASH_OPTIONS) then
		AtlasOptions_Toggle();
	else
		Atlas_Toggle();
	end
end

-- Sets the transparency of the Atlas frame based on AtlasAlpha
function Atlas_UpdateAlpha()
	AtlasFrame:SetAlpha(AtlasOptions.AtlasAlpha);
end

-- Sets the scale of the Atlas frame based on AtlasScale
function Atlas_UpdateScale()
	AtlasFrame:SetScale(AtlasOptions.AtlasScale);
end

-- Simple function to toggle the visibility of the Atlas frame
function Atlas_Toggle()
	if (AtlasFrame:IsVisible()) then
		HideUIPanel(AtlasFrame);
	else
		ShowUIPanel(AtlasFrame);
	end
end

-- Adopted some of the codes from AlasMajorCitiesEnhanced
-- Function to cleanup the text frame
function Atlas_Clean_NPC_TextFrame()
	-- Clean up NPC text frames
	if (AtlasMap_NPC_Text_Frame_Num == 0) then 
		return; 
	end
	if (AtlasMap_NPC_Text_Frame_Num > 0) then
		for i = 1, AtlasMap_NPC_Text_Frame_Num do
			local AtlasMap_NPC_Text_Frame = _G["AtlasMapNPCTextFrame"..i];
			if (AtlasMap_NPC_Text_Frame) then
				AtlasMap_NPC_Text_Frame:Hide();
				AtlasMap_NPC_Text_Frame:ClearAllPoints();
			end
			local AtlasMap_NPC_Text = _G["AtlasMapNPCText"..i];
			if (AtlasMap_NPC_Text) then
				AtlasMap_NPC_Text:SetText("");
			end
		end
		AtlasMap_NPC_Text_Frame_Num = 0;
	end
end

-- Function to handle the boss description to be added as GameToolTip
-- Description is adopted from Dungeon Journal
function AtlasMaps_NPC_Text_OnUpdate(self)
	if (not GameTooltip:IsShown()) then
		local ejbossname, description = EJ_GetEncounterInfo(self:GetID());
		if (ejbossname) then
			if ((IsAddOnLoaded("AtlasLoot") and AtlasLootItemsFrame:IsShown())) then
				-- do nothing
			else
				GameTooltip:SetOwner(self, "ANCHOR_CURSOR");
				GameTooltip:SetBackdropColor(0, 0, 0, 1 * AtlasOptions["AtlasAlpha"]);
				GameTooltip:SetText(ejbossname, 1, 1, 1, nil, 1);
				GameTooltip:AddLine(description, nil, nil, nil, 1);
				GameTooltip:SetScale(AtlasOptions["AtlasBossDescScale"] * AtlasOptions["AtlasScale"]);
				GameTooltip:Show();
			end
		end
	else
		GameTooltip:Hide();
	end
end

function AtlasMaps_NPC_Text_OnLeave(self)
	GameTooltip_Hide();
	GameTooltip:SetScale(AtlasMap_Temp_Scale);
end

function Atlas_MapRefresh()
	local zoneID = ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone];
	local data = AtlasMaps;
	local base = data[zoneID];
	local minLevel, maxLevel, minRecLevel, maxRecLevel, maxPlayers;
	local minLevelH, maxLevelH, minRecLevelH, maxRecLevelH, maxPlayersH;
	
	if (base.DungeonID) then 
		-- name, typeID, subtypeID, minLevel, maxLevel, recLevel, minRecLevel, maxRecLevel, expansionLevel, groupID, textureFilename, difficulty, maxPlayers, description, isHoliday = GetLFGDungeonInfo(base.DungeonID);
		_, _, _, minLevel, maxLevel, _, minRecLevel, maxRecLevel, _, _, _, _, maxPlayers = GetLFGDungeonInfo(base.DungeonID);

		-- For some unknown reason, some of the dungeons do not have recommended level range
		if (minRecLevel == 0) then 
			minRecLevel = minLevel;
		end
		if (maxRecLevel == 0) then
			maxRecLevel = maxLevel;
		end
	end
	if (base.DungeonHeroicID) then
		-- nameH, typeIDH, subtypeIDH, minLevelH, maxLevelH, recLevelH, minRecLevelH, maxRecLevelH, expansionLevelH, groupIDH, textureFilenameH, difficultyH, maxPlayersH, descriptionH, isHolidayH = GetLFGDungeonInfo(base.DungeonHeroicID);
		_, _, _, minLevelH, maxLevelH, _, minRecLevelH, maxRecLevelH, _, _, _, _, maxPlayersH = GetLFGDungeonInfo(base.DungeonHeroicID);

		if (minRecLevelH == 0) then
			minRecLevelH = minRecLevel;
		end
		if (maxRecLevelH == 0) then
			maxRecLevelH = maxRecLevel;
		end
	end
	
	-- Zone Name Acronym
	local tName = base.ZoneName[1];
	if (AtlasOptions.AtlasAcronyms and base.Acronym ~= nil) then
		local _RED = "|cffcc3333";
		tName = tName.._RED.." ["..base.Acronym.."]";
	end
	AtlasText_ZoneName_Text:SetText(tName);
	
	-- Map Location
	local tLoc = "";
	if (base.Location) then
		tLoc = ATLAS_STRING_LOCATION..AL["Colon"]..base.Location[1];
	end
	AtlasText_Location_Text:SetText(tLoc);

	-- Map Level Range
	local tLR = "";
	if (base.DungeonID) then 
		local tmp_LR = ATLAS_STRING_LEVELRANGE..AL["Colon"]..minLevel.."-"..maxLevel;
		local tmp_RLR = ATLAS_STRING_RECLEVELRANGE..AL["Colon"]..minRecLevel.."-"..maxRecLevel;
		if (base.DungeonHeroicID) then
			tmp_LR = tmp_LR.." / "..minLevelH.."-"..maxLevelH;
			tmp_RLR = tmp_RLR.." / "..minRecLevelH.."-"..maxRecLevelH;
		end
		tLR = tmp_LR.."; "..tmp_RLR;
	elseif (base.LevelRange) then
		tLR = ATLAS_STRING_LEVELRANGE..AL["Colon"]..base.LevelRange;
	end
	AtlasText_LevelRange_Text:SetText(tLR);

	-- Map's Minimum Level
	local tML = "";
	if (base.DungeonID) then 
		tML = ATLAS_STRING_MINLEVEL..AL["Colon"]..minLevel;
		if (base.DungeonHeroicID) then
			tML = tML.." / "..minLevelH;
		end
	elseif (base.MinLevel) then
		tML = ATLAS_STRING_MINLEVEL..AL["Colon"]..base.MinLevel;
	end
	AtlasText_MinLevel_Text:SetText(tML);

	-- Player Limit
	local tPL = "";
	if (base.DungeonID and maxPlayers ~= 0) then 
		tPL = ATLAS_STRING_PLAYERLIMIT..AL["Colon"]..maxPlayers;
		if (base.DungeonHeroicID and maxPlayers ~= maxPlayersH) then
			tPL = tPL.." / "..maxPlayersH;
		end
	elseif (base.PlayerLimit) then
		tPL = ATLAS_STRING_PLAYERLIMIT..AL["Colon"]..base.PlayerLimit;
	end
	AtlasText_PlayerLimit_Text:SetText(tPL);

	-- Check if Journal Encounter Instance is available
	if (base.JournalInstanceID) then
		Atlas_JournalEncounter_InstanceButton:Show();
		--AtlasSetEJBackground(base.JournalInstanceID);
	else
		Atlas_JournalEncounter_InstanceButton:Hide();
		--AtlasSetEJBackground();
	end
	
	-- Clear boss description gametooltip when map is refreshing
	if (AtlasOptions["AtlasBossDesc"]) then
		Atlas_Clean_NPC_TextFrame();
	end
	
	AtlasMap:ClearAllPoints();
	AtlasMap:SetWidth(512);
	AtlasMap:SetHeight(512);
	AtlasMap:SetPoint("TOPLEFT", "AtlasFrame", "TOPLEFT", 18, -84);

	-- Searching for the map path from Atlas or from plugins
	for k, v in pairs(Atlas_CoreMapsKey) do
		-- If selected map is Atlas' core map
		if (zoneID == v) then
			if (base.Module) then
				-- if the map belong to a module, set the path to module
				AtlasMap:SetTexture("Interface\\AddOns\\"..base.Module.."\\Images\\"..zoneID);
			else
				AtlasMap:SetTexture("Interface\\AddOns\\Atlas\\Images\\Maps\\"..zoneID);
			end
			break;
		-- Check if selected map is from plugin
		else
			-- Searching for plugins
			for ka,va in pairs(ATLAS_PLUGINS) do
				-- Searching for plugin's maps
				for kb,vb in pairs(ATLAS_PLUGINS[ka]) do
					if (zoneID == vb) then
						AtlasMap:SetTexture("Interface\\AddOns\\"..ka.."\\Images\\"..zoneID);
						break;
					end
				end
			end
		end
	end

	--[[
	local AtlasMap_Text = _G["AtlasMap_Text"];
	if (not AtlasMap_Text) then
		AtlasMap_Text = AtlasFrame:CreateFontString("AtlasMap_Text", "OVERLAY", "GameFontHighlightLarge");
	end
	AtlasMap_Text:SetPoint("CENTER", "AtlasFrame", "LEFT", 256, -32);
	]]
	-- Check if the map image is available, if not replace with black and Map Not Found text
	-- Below checking won't work anymore since WoW 5.0.4
	--[[
	if (not AtlasMap:GetTexture()) then
		AtlasMap:SetTexture(0, 0, 0);
		AtlasMap_Text:SetText(AL["MapsNotFound"]);
		if (not AtlasMap_Text:IsShown()) then
			AtlasMap_Text:Show();
		end
	else 
		AtlasMap_Text:SetText("");
	end
	]]

	-- The boss description to be added here
	if (AtlasOptions["AtlasBossDesc"]) then
		local NPC_Table = AtlasMaps_NPC_DB[zoneID];
		if (NPC_Table and AtlasMap_NPC_Text_Frame_Num == 0) then
			AtlasMap_NPC_Text_Frame_Num = 1;
			while (NPC_Table[AtlasMap_NPC_Text_Frame_Num]) do
				local AtlasMap_NPC_Text_Frame = _G["AtlasMapNPCTextFrame"..AtlasMap_NPC_Text_Frame_Num];
				if (not AtlasMap_NPC_Text_Frame) then
					AtlasMap_NPC_Text_Frame = CreateFrame("Frame", "AtlasMapNPCTextFrame"..AtlasMap_NPC_Text_Frame_Num, AtlasFrame);
				else
					AtlasMap_NPC_Text_Frame:Show();
				end
				AtlasMap_NPC_Text_Frame:SetPoint("TOPLEFT", "AtlasFrame", "TOPLEFT", 20 + NPC_Table[AtlasMap_NPC_Text_Frame_Num][3], -80 - NPC_Table[AtlasMap_NPC_Text_Frame_Num][4]);
				AtlasMap_NPC_Text_Frame:SetWidth(15);
				AtlasMap_NPC_Text_Frame:SetHeight(15);
				AtlasMap_NPC_Text_Frame:SetID(NPC_Table[AtlasMap_NPC_Text_Frame_Num][2]);
				AtlasMap_NPC_Text_Frame:SetScript("OnEnter", AtlasMaps_NPC_Text_OnUpdate);
				AtlasMap_NPC_Text_Frame:SetScript("OnLeave", AtlasMaps_NPC_Text_OnLeave);

				local AtlasMap_NPC_Text = AtlasMap_NPC_Text_Frame:CreateFontString("AtlasMapNPCText"..AtlasMap_NPC_Text_Frame_Num, "MEDIUM", "GameFontHighlightLarge");
				AtlasMap_NPC_Text:SetPoint("CENTER", AtlasMap_NPC_Text_Frame, "CENTER", 0, 0);
				-- Disable the set text unless one day we want the text to be added dynamatically
				-- Or, enable it for debugging purpose
				--AtlasMap_NPC_Text:SetText(NPC_Table[AtlasMap_NPC_Text_Frame_Num][1]);

				AtlasMap_NPC_Text_Frame_Num = AtlasMap_NPC_Text_Frame_Num + 1;
			end
			-- We started the counting from 1, plus 1 in each loop, need to adjust by removing 1 after the loop is ended
			AtlasMap_NPC_Text_Frame_Num = AtlasMap_NPC_Text_Frame_Num - 1;
		end
	else
		Atlas_Clean_NPC_TextFrame();
	end
end


-- Refreshes the Atlas frame, usually because a new map needs to be displayed
-- The zoneID variable represents the internal name used for each map, ex: "BlackfathomDeeps"
-- Also responsible for updating all the text when a map is changed
function Atlas_Refresh()
	local zoneID = ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone];
	local data = AtlasMaps;
	local base = data[zoneID];

	Atlas_MapRefresh();
	
	ATLAS_DATA = base;
	ATLAS_SEARCH_METHOD = data.Search;

	if ( data.Search == nil ) then
		ATLAS_SEARCH_METHOD = AtlasSimpleSearch;
	end
	
	if ( data.Search ~= false ) then
		AtlasSearchEditBox:Show();
		AtlasNoSearch:Hide();
	else
		AtlasSearchEditBox:Hide();
		AtlasNoSearch:Show();
		ATLAS_SEARCH_METHOD = nil;
	end

	-- Populate the scroll frame entries list, the update func will do the rest
	Atlas_Search("");
	AtlasSearchEditBox:SetText("");
	AtlasSearchEditBox:ClearFocus();

	-- Create and align any new entry buttons that we need
	for i = 1, ATLAS_CUR_LINES do
		if (not _G["AtlasEntry"..i]) then
			local f = CreateFrame("Button", "AtlasEntry"..i, AtlasFrame, "AtlasEntryTemplate");
			if i == 1 then
				f:SetPoint("TOPLEFT", "AtlasScrollBar", "TOPLEFT", 16, -2);
			else
				f:SetPoint("TOPLEFT", "AtlasEntry"..(i - 1), "BOTTOMLEFT");
			end
		end
	end

	AtlasScrollBar_Update();

	-- Deal with the switch to entrance/instance button here
	-- Only display if appropriate
	-- See if we should display the button or not, and decide what it should say
	local matchFound = {};
	local sayEntrance;
	for k, v in pairs(Atlas_EntToInstMatches) do
		if (k == zoneID) then
			matchFound = v;
			sayEntrance = false;
		end
	end
	if (not matchFound[1]) then
		for k, v in pairs(Atlas_InstToEntMatches) do
			if (k == zoneID) then
				matchFound = v;
				sayEntrance = true;
			end
		end
	end
	-- Below try to add the series maps into switch button's map list
	if (not matchFound[1]) then
		for k, v in pairs(Atlas_MapSeries) do
			if (k == zoneID) then
				matchFound = v;
				sayEntrance = false;
			end
		end
	end

	-- Set the button's text, populate the dropdown menu, and show or hide the button
	if (matchFound[1]) then
		ATLAS_INST_ENT_DROPDOWN = {};
		for k, v in pairs(matchFound) do
			table.insert(ATLAS_INST_ENT_DROPDOWN, v);
		end
		table.sort(ATLAS_INST_ENT_DROPDOWN, AtlasSwitchDD_Sort);
		if (sayEntrance) then
			AtlasSwitchButton:SetText(ATLAS_ENTRANCE_BUTTON);
		else
			AtlasSwitchButton:SetText(ATLAS_INSTANCE_BUTTON);
		end
		AtlasSwitchButton:Show();
		UIDropDownMenu_Initialize(AtlasSwitchDD, AtlasSwitchDD_OnLoad);
	else
		AtlasSwitchButton:Hide();
	end

	if (TitanPanelButton_UpdateButton) then
		TitanPanelButton_UpdateButton("Atlas");
	end
end

-- When the switch button is clicked
-- We can basically assume that there's a match
-- Find it, set it, then update menus and the maps
function AtlasSwitchButton_OnClick()
	local zoneID = ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone];
	if (getn(ATLAS_INST_ENT_DROPDOWN) == 1) then
		-- One link, so we can just go there right away
		AtlasSwitchDD_Set(1);
	else
		-- More than one link, so it's dropdown menu time
		ToggleDropDownMenu(1, nil, AtlasSwitchDD, "AtlasSwitchButton", 0, 0);
	end
end

function AtlasSwitchDD_OnLoad()
	local info;
	for k, v in pairs(ATLAS_INST_ENT_DROPDOWN) do
		info = {
			text = AtlasMaps[v].ZoneName[1];
			func = AtlasSwitchDD_OnClick;
		};
		UIDropDownMenu_AddButton(info);
	end
end

function AtlasSwitchDD_OnClick(self)
	AtlasSwitchDD_Set(self:GetID());
end

function AtlasSwitchDD_Set(index)
	for k, v in pairs(ATLAS_DROPDOWNS) do
		for k2, v2 in pairs(v) do
			if (v2 == ATLAS_INST_ENT_DROPDOWN[index]) then
				AtlasOptions.AtlasType = k;
				AtlasOptions.AtlasZone = k2;
			end
		end
	end
	AtlasFrameDropDownType_OnShow();
	AtlasFrameDropDown_OnShow();
	Atlas_Refresh();
end

function AtlasSwitchDD_Sort(a, b)
	local aa = AtlasMaps[a].ZoneName[1];
	local bb = AtlasMaps[b].ZoneName[1];
	return aa < bb;
end

-- Function used to initialize the map type dropdown menu
-- Cycle through Atlas_MapTypes to populate the dropdown
function AtlasFrameDropDownType_Initialize()
	local info;
	local catName = Atlas_DropDownLayouts_Order[AtlasOptions.AtlasSortBy];
	local subcatOrder = Atlas_DropDownLayouts_Order[catName];
	for i = 1, getn(subcatOrder), 1 do
		info = {
			text = subcatOrder[i];
			func = AtlasFrameDropDownType_OnClick;
		};
		UIDropDownMenu_AddButton(info);
	end
	for i = 1, getn(Atlas_MapTypes), 1 do
		info = {
			text = Atlas_MapTypes[i];
			func = AtlasFrameDropDownType_OnClick;
		};
		UIDropDownMenu_AddButton(info);
	end
end

-- Called whenever the map type dropdown menu is shown
function AtlasFrameDropDownType_OnShow()
	UIDropDownMenu_Initialize(AtlasFrameDropDownType, AtlasFrameDropDownType_Initialize);
	UIDropDownMenu_SetSelectedID(AtlasFrameDropDownType, AtlasOptions.AtlasType);
	UIDropDownMenu_SetWidth(AtlasFrameDropDownType, 190);
end

-- Called whenever an item in the map type dropdown menu is clicked
-- Sets the main dropdown menu contents to reflect the category of map selected
function AtlasFrameDropDownType_OnClick(self)
	local thisID = self:GetID();
	UIDropDownMenu_SetSelectedID(AtlasFrameDropDownType, thisID);
	AtlasOptions.AtlasType = thisID;
	AtlasOptions.AtlasZone = 1;
	AtlasFrameDropDown_OnShow();
	Atlas_Refresh();
end

-- Calculate the dungeon difficulty based on the dungeon's level and player's level
-- Codes adopted from FastQuest_Classic
local function Atlas_DungeonDifficulty(minRecLevel)
	local lDiff = minRecLevel - UnitLevel("player");
	local color;
	if (lDiff >= 0) then
		for i= 1.00, 0.10, -0.10 do
			color = {r = 1.00, g = i, b = 0.00};
			if ((i/0.10)==(10-lDiff)) then return color; end
		end
	elseif ( -lDiff < GetQuestGreenRange() ) then
		for i= 0.90, 0.10, -0.10 do
			color = {r = i, g = 1.00, b = 0.00};
			if ((9-i/0.10)==(-1*lDiff)) then return color; end
		end
	elseif ( -lDiff == GetQuestGreenRange() ) then
		color = {r = 0.50, g = 1.00, b = 0.50};
	else
		--color = {r = 0.75, g = 0.75, b = 0.75};
		color = {r = 1.00, g = 1.00, b = 1.00};
	end
	return color;
end

-- Function used to initialize the main dropdown menu
-- Looks at the status of AtlasType to determine how to populate the list
function AtlasFrameDropDown_Initialize()
	local info;
	local colortag;
	for k, v in pairs(ATLAS_DROPDOWNS[AtlasOptions.AtlasType]) do
		
		if (AtlasOptions["AtlasColoringDropDown"] and AtlasMaps[v].DungeonID) then
			local _, _, _, _, _, _, minRecLevel = GetLFGDungeonInfo(AtlasMaps[v].DungeonID);
			local dungeon_difficulty = Atlas_DungeonDifficulty(minRecLevel);
			colortag = string.format("|cff%02x%02x%02x", dungeon_difficulty.r * 255, dungeon_difficulty.g * 255, dungeon_difficulty.b * 255);
		elseif (AtlasOptions["AtlasColoringDropDown"] and AtlasMaps[v].MinLevel) then
			if (type(AtlasMaps[v].MinLevel) == number) then
				local dungeon_difficulty = Atlas_DungeonDifficulty(AtlasMaps[v].MinLevel);
				colortag = string.format("|cff%02x%02x%02x", dungeon_difficulty.r * 255, dungeon_difficulty.g * 255, dungeon_difficulty.b * 255);
			else
				colortag = ""
			end
		else
			colortag = ""
		end
		info = {
			text = colortag..AtlasMaps[v].ZoneName[1];
			func = AtlasFrameDropDown_OnClick;
		};
		UIDropDownMenu_AddButton(info);
	end

end

-- Called whenever the main dropdown menu is shown
function AtlasFrameDropDown_OnShow()
	UIDropDownMenu_Initialize(AtlasFrameDropDown, AtlasFrameDropDown_Initialize);
	UIDropDownMenu_SetSelectedID(AtlasFrameDropDown, AtlasOptions.AtlasZone);
	UIDropDownMenu_SetWidth(AtlasFrameDropDown, 190);
end

-- Called whenever an item in the main dropdown menu is clicked
-- Sets the newly selected map as current and refreshes the frame
function AtlasFrameDropDown_OnClick(self)
	local i = self:GetID();
	UIDropDownMenu_SetSelectedID(AtlasFrameDropDown, i);
	AtlasOptions.AtlasZone = i;
	Atlas_Refresh();
end

-- Modifies the value of GetRealZoneText to account for some naming conventions
-- Always use this function instead of GetRealZoneText within Atlas
function Atlas_GetFixedZoneText()
	local currentZone = GetRealZoneText();
	if (AtlasZoneSubstitutions[currentZone]) then
		return AtlasZoneSubstitutions[currentZone];
	end
	return currentZone;
end 

-- Checks the player's current location against all Atlas maps
-- If a match is found display that map right away
-- update for Outland zones contributed by Drahcir
-- 3/23/08 now takes SubZones into account as well
function Atlas_AutoSelect()
	local currentZone = Atlas_GetFixedZoneText();
	local currentSubZone = GetSubZoneText();
	debug("Using auto-select to open the best map.");

	-- Check if the current zone is defined in AssocDefaults table
	-- If yes, means there could be multiple maps for this zone
	-- And we will choose a proper one to be the default one.
	debug("currentZone: "..currentZone..", currentSubZone: "..currentSubZone);
	if (Atlas_AssocDefaults[currentZone]) then
		debug("currentZone: "..currentZone.." matched the one defined in Atlas_AssocDefaults{}.");
		local selected_map;
		if (Atlas_SubZoneData[currentZone]) then
			for k_instance_map, v_instance_map in pairs(Atlas_SubZoneData[currentZone]) do
				for k_subzone, v_subzone in pairs(Atlas_SubZoneData[currentZone][k_instance_map]) do
					if (v_subzone == currentSubZone) then
						selected_map = k_instance_map;
						debug("currentSubZone: "..currentSubZone.." matched found, now we will use map: \""..selected_map.."\" for instance: "..currentZone);
						break;
					end
				end
			end
		end
		if (not selected_map) then
			debug("No subzone matched, now checking if we should specify a default map.");
			if (currentZone == Atlas_SubZoneAssoc[ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]]) then
				debug("You're in the same instance as the former map. Doing nothing.");
				return;
			else
				selected_map = Atlas_AssocDefaults[currentZone];
				debug("We will use the map: "..selected_map.." for the current zone: "..currentZone..".");
			end
		end
		debug("Selecting the map...");
		for k_DropDownType, v_DropDownType in pairs(ATLAS_DROPDOWNS) do
			for k_DropDownZone, v_DropDownZone in pairs(v_DropDownType) do         
				if (selected_map == v_DropDownZone) then
					AtlasOptions.AtlasType = k_DropDownType;
					AtlasOptions.AtlasZone = k_DropDownZone;
					Atlas_Refresh();
					debug("Map selected! Type: "..k_DropDownType..", Zone: "..k_DropDownZone..", "..ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]);
					return;
				end
			end
		end
	else
		debug("SubZone data isn't relevant here. Checking if it's outdoor zone.");
		if (Atlas_OutdoorZoneToAtlas[currentZone]) then
			debug("This world zone "..currentZone.." is associated with a map.");
			for k_DropDownType, v_DropDownType in pairs(ATLAS_DROPDOWNS) do
				for k_DropDownZone, v_DropDownZone in pairs(v_DropDownType) do         
					if (Atlas_OutdoorZoneToAtlas[currentZone] == v_DropDownZone) then
						AtlasOptions.AtlasType = k_DropDownType;
						AtlasOptions.AtlasZone = k_DropDownZone;
						Atlas_Refresh();
						debug("Map changed to the associated map: "..ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]);
						return;
					end
				end
			end
			debug("Checking if instance/entrance pair can be found.");
		elseif (Atlas_InstToEntMatches[ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]]) then
			for ka, va in pairs(Atlas_InstToEntMatches[ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]]) do
				if (currentZone == AtlasMaps[va].ZoneName[1]) then
					debug("Instance/entrance pair found. Doing nothing.");
					return;
				end
			end
		elseif (Atlas_EntToInstMatches[ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]]) then
			for ka, va in pairs(Atlas_EntToInstMatches[ATLAS_DROPDOWNS[AtlasOptions.AtlasType][AtlasOptions.AtlasZone]]) do
				if (currentZone == AtlasMaps[va].ZoneName[1]) then
					debug("Instance/entrance pair found. Doing nothing.");
					return;
				end
			end
		end
		debug("Searching through all maps for a ZoneName match.");
		for k_DropDownType, v_DropDownType in pairs(ATLAS_DROPDOWNS) do
			for k_DropDownZone, v_DropDownZone in pairs(v_DropDownType) do         
				-- Compare the currentZone to the new substr of ZoneName
				if (currentZone == strsub(AtlasMaps[v_DropDownZone].ZoneName[1], strlen(AtlasMaps[v_DropDownZone].ZoneName[1]) - strlen(currentZone) + 1)) then
					AtlasOptions.AtlasType = k_DropDownType;
					AtlasOptions.AtlasZone = k_DropDownZone;
					Atlas_Refresh();
					debug("Found a match. Map has been changed.");
					return;
				end
			end
		end
	end
	debug("Nothing changed because no match was found.");
end

--Called whenever the Atlas frame is displayed
function Atlas_OnShow()
	if (AtlasOptions.AtlasAutoSelect) then
		Atlas_AutoSelect();
	end
	-- Sneakiness
	AtlasFrameDropDownType_OnShow();
	AtlasFrameDropDown_OnShow();
end


function AtlasScrollBar_Update()
	GameTooltip:Hide();
	local lineplusoffset;
	FauxScrollFrame_Update(AtlasScrollBar,ATLAS_CUR_LINES,ATLAS_NUM_LINES,15);
	for i = 1, ATLAS_NUM_LINES do
		lineplusoffset = i + FauxScrollFrame_GetOffset(AtlasScrollBar);
		if (lineplusoffset <= ATLAS_CUR_LINES) then
			_G["AtlasEntry"..i.."_Text"]:SetText(ATLAS_SCROLL_LIST[lineplusoffset]);
			_G["AtlasEntry"..i]:Show();
		elseif (_G["AtlasEntry"..i]) then
			_G["AtlasEntry"..i]:Hide();
		end
	end
end

function AtlasSimpleSearch(data, text)
	if (string.trim(text or "") == "") then
		return data
	end
	local new = {}; -- Create a new table
	local i, v, n;
	local search_text = string.lower(text);
	search_text = search_text:gsub("([%^%$%(%)%%%.%[%]%+%-%?])", "%%%1");
	search_text = search_text:gsub("%*", ".*");
	local match;

	i, v = next(data, nil); -- The i is an index of data, v = data[i]
	n = i;
	while i do
		if ( type(i) == "number" ) then
			if ( string.gmatch ) then 
				match = string.gmatch(string.lower(data[i][1]), search_text)();
			else 
				match = string.gfind(string.lower(data[i][1]), search_text)(); 
			end
			if ( match ) then
				new[n] = {};
				new[n][1] = data[i][1];
				n = n + 1;
			end
		end
		i, v = next(data, i); -- Get next index
	end
	return new;
end

local function round(num, idp)
	local mult = 10 ^ (idp or 0);
	return math.floor(num * mult + 0.5) / mult;
end

function AtlasEntryTemplate_OnUpdate(self)
	if (AtlasOptions.AtlasCtrl) then
		if (MouseIsOver(self)) then
			if (IsControlKeyDown()) then
				if (not GameTooltip:IsShown()) then
					local str = _G[self:GetName().."_Text"]:GetText();
					if (str) then
						GameTooltip:SetOwner(self, "ANCHOR_CURSOR");
						GameTooltip:SetBackdropBorderColor(0, 0, 0, 0);
						GameTooltip:SetBackdropColor(0, 0, 0, 1);
						local colorCheck = string.sub(str, 1, 4);
						if (colorCheck == "|cff") then
							local color = string.sub(str, 1, 10);
							local stripped = strtrim(string.sub(str, 11));
							GameTooltip:SetText(color..stripped, 1, 1, 1, 1);
						else
							GameTooltip:SetText(str, 1, 1, 1, 1);
						end
					end
				end
			else
				GameTooltip:Hide();
			end
		end
	end
end


-- In Development, this could be fun
--[[
function AtlasSetEJBackground(instanceID)
	AtlasEJBackground = CreateFrame("Frame", "AtlasEJBackground", AtlasFrame);	
	if (instanceID) then
		AtlasEJBackground:ClearAllPoints();
		AtlasEJBackground:SetWidth(512);
		AtlasEJBackground:SetHeight(512);
		AtlasEJBackground:SetPoint("TOPLEFT", "AtlasFrame", "TOPLEFT", 534, -178);
		local t = AtlasEJBackground:CreateTexture(nil,"BACKGROUND");
		local name, description, bgImage, buttonImage, loreImage, dungeonAreaMapID, link = EJ_GetInstanceInfo(instanceID)
		t:SetTexture(bgImage);
		t:SetAllPoints(AtlasEJBackground);
		AtlasEJBackground.texture = t;
		AtlasEJBackground:Show()
	else
		--AtlasEJBackground:Hide()
	end
end
]]
