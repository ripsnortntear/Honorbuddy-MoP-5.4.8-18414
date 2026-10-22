--[[
************************************************************************
Player.lua
Player Data for Collectinator
************************************************************************
File date: 2012-10-21T02:06:29Z
File hash: a40ecbd
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/collectinator/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]]--

-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-- Libraries
local bit = _G.bit
local table = _G.table

-- Functions
local pairs = _G.pairs

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)

------------------------------------------------------------------------------
-- Data which is stored regarding a players statistics (luadoc copied from Collectinator, needs updating)
------------------------------------------------------------------------------
-- @class table
-- @name Player
-- @field known_filtered Total number of items known filtered during the scan.
-- @field faction Player's faction
-- @field class Player's class
-- @field reputation_levels Listing of players reputation levels
local Player = {}
private.Player = Player

-------------------------------------------------------------------------------
-- Constants
-------------------------------------------------------------------------------

-------------------------------------------------------------------------------
-- Player methods.
-------------------------------------------------------------------------------
function Player:Initialize()
	self.faction = _G.UnitFactionGroup("player")
	self.class = _G.select(2, _G.UnitClass("player"))
	self.race = _G.select(2, _G.UnitRace("player"))
end

function Player:Class()
	return self.class
end

function Player:Faction()
	return self.faction
end

function Player:Race()
	return self.race
end

do
	local headers = {}

	function Player:UpdateReputations()
		self.reputation_levels = self.reputation_levels or {}

		table.wipe(headers)

		-- Number of factions before expansion
		local num_factions = _G.GetNumFactions()

		-- Expand all the headers, storing those which were collapsed.
		for i = num_factions, 1, -1 do
			local name, _, _, _, _, _, _, _, _, isCollapsed = _G.GetFactionInfo(i)

			if isCollapsed then
				_G.ExpandFactionHeader(i)
				headers[name] = true
			end
		end

		-- Number of factions with everything expanded
		num_factions = _G.GetNumFactions()

		-- Get the rep levels
		for i = 1, num_factions, 1 do
			local name, _, replevel = _G.GetFactionInfo(i)

			-- If the rep is greater than neutral
			if replevel > 4 then
				-- We use levels of 0, 1, 2, 3, 4 internally for reputation levels, make it correspond here
				self.reputation_levels[name] = replevel - 4
			end
		end

		-- Collapse the headers again
		for i = num_factions, 1, -1 do
			local name = _G.GetFactionInfo(i)

			if headers[name] then
				_G.CollapseFactionHeader(i)
			end
		end
	end
end -- do-block

function Player:HasProperRepLevel(rep_data)
	if not rep_data then
		return true
	end
	local is_alliance = self.faction == "Alliance"
	local reputation_levels = self.reputation_levels
	local FAC = private.FACTION_IDS
	local has_faction = true

	for rep_id, rep_info in pairs(rep_data) do
		for rep_level in pairs(rep_info) do
			if rep_id == FAC.HONOR_HOLD or rep_id == FAC.THRALLMAR then
				rep_id = is_alliance and FAC.HONOR_HOLD or FAC.THRALLMAR
			elseif rep_id == FAC.THE_MAGHAR or rep_id == FAC.KURENAI then
				rep_id = is_alliance and FAC.KURENAI or FAC.THE_MAGHAR
			end
			local rep_name = private.reputation_list[rep_id].name

			if not reputation_levels[rep_name] or reputation_levels[rep_name] < rep_level then
				has_faction = false
			else
				has_faction = true
				break
			end
		end
	end
	return has_faction
end

function Player:HasCollectableFaction(collection)
	local flagged_horde = collection:HasFilter("common1", "HORDE")
	local flagged_alliance = collection:HasFilter("common1", "ALLIANCE")

	if self.faction == "Alliance" and flagged_horde and not flagged_alliance then
		return false
	elseif self.faction == "Horde" and flagged_alliance and not flagged_horde then
		return false
	end
	return true
end

