-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

local table = _G.table
local string = _G.string
local pairs = _G.pairs

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)

-------------------------------------------------------------------------------
-- Table cache mechanism
-------------------------------------------------------------------------------
do
	local table_cache = {}

	-- Returns a table
	function private.AcquireTable()
		local tbl = table.remove(table_cache) or {}
		return tbl
	end

	-- Cleans the table and stores it in the cache
	function private.ReleaseTable(tbl)
		if not tbl then return end
		table.wipe(tbl)
		table.insert(table_cache, tbl)
	end
end -- do block

-------------------------------------------------------------------------------
-- Sort functions
-------------------------------------------------------------------------------
do
	addon.sorted_collectables = {}

	local collectable_list
	local sorted_collectables = addon.sorted_collectables

	local function Sort_NameAsc(a, b)
		--@alpha@
		if not collectable_list[a].name then
			addon:Debug("Collectable ID %d does not have a name.", a)
		end
		if not collectable_list[b].name then
			addon:Debug("Collectable ID %d does not have a name.", b)
		end
		--@end-alpha@
		return collectable_list[a].name < collectable_list[b].name
	end

	local function Sort_NameDesc(a, b)
		return collectable_list[a].name > collectable_list[b].name
	end

	local COLLECTABLE_SORT_FUNCS = {
		["NameAscending"] = Sort_NameAsc,
		["NameDescending"] = Sort_NameDesc,
	}

	-- Sorts the collectable_list according to configuration settings.
	function private.SortCollectables(collectables, collectable_type)
		if not collectables then
			return
		end
		local sort_type = addon.db.profile.sorting
		local sort_func = COLLECTABLE_SORT_FUNCS["Name" .. sort_type] or Sort_NameAsc

		table.wipe(sorted_collectables)
		collectable_list = private.collectable_list[collectable_type]

		for collectable_id in pairs(collectables) do
			sorted_collectables[#sorted_collectables + 1] = collectable_id
		end
		table.sort(sorted_collectables, sort_func)
	end
end -- do

-------------------------------------------------------------------------------
-- Sets show and hide scripts as well as text for a tooltip for the given frame.
-------------------------------------------------------------------------------
do
	local HIGHLIGHT_FONT_COLOR = _G.HIGHLIGHT_FONT_COLOR

	local function Show_Tooltip(frame, motion)
		_G.GameTooltip:SetOwner(frame, "ANCHOR_RIGHT")
		_G.GameTooltip:SetText(frame.tooltip_text, HIGHLIGHT_FONT_COLOR.r, HIGHLIGHT_FONT_COLOR.g, HIGHLIGHT_FONT_COLOR.b)
		_G.GameTooltip:Show()
	end

	local function Hide_Tooltip()
		_G.GameTooltip:Hide()
	end

	function private.SetTooltipScripts(frame, textLabel)
		frame.tooltip_text = textLabel

		frame:SetScript("OnEnter", Show_Tooltip)
		frame:SetScript("OnLeave", Hide_Tooltip)
	end
end -- do
