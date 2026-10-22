-------------------------------------------------------------------------------
-- Localized Lua API.
-------------------------------------------------------------------------------
local _G = getfenv(0)

local math = _G.math
local string = _G.string
local table = _G.table

local bit = _G.bit
local pairs = _G.pairs
local tonumber = _G.tonumber
local tostring = _G.tostring
local select = _G.select
local type = _G.type

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)
local QTip = LibStub("LibQTip-1.0")
local Dialog = LibStub("LibDialog-1.0")

-------------------------------------------------------------------------------
-- Imports.
-------------------------------------------------------------------------------
local CreateListEntry = private.CreateListEntry
local SetTextColor = private.SetTextColor

local CATEGORY_COLORS	= private.CATEGORY_COLORS
local BASIC_COLORS	= private.BASIC_COLORS

local COMMON1		= private.COMMON_FLAGS_WORD1

local A			= private.ACQUIRE_TYPES
local REPLEVEL		= private.LOCALIZED_REPUTATION_LEVELS

-------------------------------------------------------------------------------
-- Constants.
-------------------------------------------------------------------------------
local NUM_COLLECTABLE_LINES = 25
local SCROLL_DEPTH	= 5
local LISTFRAME_WIDTH	= 295
local LIST_ENTRY_WIDTH	= 286

local COORD_FORMAT	= "(%.2f, %.2f)"

-------------------------------------------------------------------------------
-- Dialogs.
-------------------------------------------------------------------------------
Dialog:Register("Collectinator_NotScanned", {
	text = L["NOTSCANNED"],
	buttons = {
		{
			text = _G.OKAY,
		},
	},
	show_while_dead = true,
	hide_on_escape = true,
})

Dialog:Register("Collectinator_AllFiltered", {
	text = L["ALL_FILTERED"],
	buttons = {
		{
			text = _G.OKAY
		},
	},
	show_while_dead = true,
	hide_on_escape = true,
})

Dialog:Register("Collectinator_AllKnown", {
	text = L["ALL_KNOWN"],
	buttons = {
		{
			text = _G.OKAY
		},
	},
	show_while_dead = true,
	hide_on_escape = true,
})

Dialog:Register("Collectinator_AllExcluded", {
	text = L["ALL_EXCLUDED"],
	buttons = {
		{
			text = _G.OKAY
		},
	},
	show_while_dead = true,
	hide_on_escape = true,
})

Dialog:Register("Collectinator_SearchFiltered", {
	text = L["SEARCH_FILTERED"],
	buttons = {
		{
			text = _G.OKAY
		},
	},
	show_while_dead = true,
	hide_on_escape = true,
})

function private.DismissDialogs()
	Dialog:Dismiss("Collectinator_NotScanned")
	Dialog:Dismiss("Collectinator_AllFiltered")
	Dialog:Dismiss("Collectinator_AllKnown")
	Dialog:Dismiss("Collectinator_AllExcluded")
	Dialog:Dismiss("Collectinator_SearchFiltered")
end


-------------------------------------------------------------------------------
-- Upvalues
-------------------------------------------------------------------------------
local ListItem_ShowTooltip

local acquire_tooltip

-------------------------------------------------------------------------------
-- Frame creation and anchoring
-------------------------------------------------------------------------------
function private.InitializeListFrame()
	local MainPanel	= addon.Frame
	local ListFrame = _G.CreateFrame("Frame", nil, MainPanel)
	ListFrame:SetSize(LISTFRAME_WIDTH, 335)
	ListFrame:SetPoint("TOPLEFT", MainPanel, "TOPLEFT", 22, -75)
	ListFrame:SetBackdrop({
				      bgFile = [[Interface\DialogFrame\UI-DialogBox-Background-Dark]],
				      tile = true,
				      tileSize = 16,
			      })
	ListFrame:SetBackdropColor(1, 1, 1)
	ListFrame:EnableMouse(true)
	ListFrame:EnableMouseWheel(true)
	ListFrame:SetScript("OnHide", function(self)
		QTip:Release(acquire_tooltip)
		self.selected_entry = nil
	end)
	MainPanel.list_frame = ListFrame

	-------------------------------------------------------------------------------
	-- Scroll bar.
	-------------------------------------------------------------------------------
	local ScrollBar = _G.CreateFrame("Slider", nil, ListFrame)

	ScrollBar:SetPoint("TOPLEFT", ListFrame, "TOPRIGHT", 5, -11)
	ScrollBar:SetPoint("BOTTOMLEFT", ListFrame, "BOTTOMRIGHT", 5, 12)
	ScrollBar:SetWidth(24)

	ScrollBar:EnableMouseWheel(true)
	ScrollBar:SetOrientation("VERTICAL")

	ScrollBar:SetThumbTexture([[Interface\Buttons\UI-ScrollBar-Knob]])
	ScrollBar:SetMinMaxValues(0, 1)
	ScrollBar:SetValueStep(1)

	ListFrame.scroll_bar = ScrollBar

	local ScrollUpButton = _G.CreateFrame("Button", nil, ScrollBar, "UIPanelScrollUpButtonTemplate")

	ScrollUpButton:SetHeight(16)
	ScrollUpButton:SetWidth(18)
	ScrollUpButton:SetPoint("BOTTOM", ScrollBar, "TOP", 0, -4)

	local ScrollDownButton = _G.CreateFrame("Button", nil, ScrollBar,"UIPanelScrollDownButtonTemplate")

	ScrollDownButton:SetHeight(16)
	ScrollDownButton:SetWidth(18)
	ScrollDownButton:SetPoint("TOP", ScrollBar, "BOTTOM", 0, 4)

	local function ScrollBar_Scroll(delta)
		if not ScrollBar:IsShown() then
			return
		end
		local cur_val = ScrollBar:GetValue()
		local min_val, max_val = ScrollBar:GetMinMaxValues()

		if delta < 0 and cur_val < max_val then
			cur_val = math.min(max_val, cur_val + SCROLL_DEPTH)
			ScrollBar:SetValue(cur_val)
		elseif delta > 0 and cur_val > min_val then
			cur_val = math.max(min_val, cur_val - SCROLL_DEPTH)
			ScrollBar:SetValue(cur_val)
		end
	end

	ScrollUpButton:SetScript("OnClick", function(self, button, down)
		if _G.IsAltKeyDown() then
			local min_val = ScrollBar:GetMinMaxValues()
			ScrollBar:SetValue(min_val)
		else
			ScrollBar_Scroll(1)
		end
	end)

	ScrollDownButton:SetScript("OnClick", function(self, button, down)
		if _G.IsAltKeyDown() then
			local _, max_val = ScrollBar:GetMinMaxValues()
			ScrollBar:SetValue(max_val)
		else
			ScrollBar_Scroll(-1)
		end
	end)

	ScrollBar:SetScript("OnMouseWheel", function(self, delta)
		ScrollBar_Scroll(delta)
	end)

	ListFrame:SetScript("OnMouseWheel", function(self, delta)
		ScrollBar_Scroll(delta)
	end)

	-- This can be called either from ListFrame's OnMouseWheel script, manually
	-- sliding the thumb, or from clicking the up/down buttons.
	ScrollBar:SetScript("OnValueChanged", function(self, value)
		local min_val, max_val = self:GetMinMaxValues()

		if value == min_val then
			ScrollUpButton:Disable()
			ScrollDownButton:Enable()
		elseif value == max_val then
			ScrollUpButton:Enable()
			ScrollDownButton:Disable()
		else
			ScrollUpButton:Enable()
			ScrollDownButton:Enable()
		end
		MainPanel.current_tab:SetScrollValue(MainPanel.current_collectable_type, value)
		ListFrame:Update(nil, true)
	end)

	local function Bar_OnEnter(self)
		if ListFrame.selected_entry then
			return
		end
		ListItem_ShowTooltip(self, ListFrame.entries[self.entry_index])
	end

	local function Bar_OnLeave(self)
		if ListFrame.selected_entry then
			return
		end
		QTip:Release(acquire_tooltip)
	end

	local function ListItem_OnClick(self, button, down)
		local clicked_index = self.entry_index

		if not clicked_index or clicked_index == 0 then
			return
		end
		local entry = ListFrame.entries[clicked_index]
		local collectable = entry.collectable

		if button == "RightButton" and collectable and (not entry.parent or entry.parent.collectable ~= entry.collectable) then
			local old_selected = ListFrame.selected_entry
			local entry_button = entry.button

			ListFrame.selected_entry = nil

			if old_selected and old_selected.button then
				old_selected.button.selected_texture:Hide()
				Bar_OnLeave(old_selected.button)
			end
			Bar_OnEnter(entry_button)

			if old_selected ~= entry then
				entry_button.selected_texture:Show()
				ListFrame.selected_entry = entry
			end
		elseif collectable and _G.IsModifierKeyDown() then
			if _G.IsControlKeyDown() then
				if _G.IsShiftKeyDown() then
					--addon:AddWaypoint(clicked_line.collectable, clicked_line:AcquireID(), clicked_line:LocationID(), clicked_line.npc_id)
					addon:Print("Waypoints are not yet supported.")
				else
					local edit_box = _G.ChatEdit_ChooseBoxForSend()

					_G.ChatEdit_ActivateChat(edit_box)
					edit_box:Insert(_G.GetSpellLink(entry.collectable.collection_spell_id))
				end
			elseif _G.IsShiftKeyDown() then
				local collectable_item_id = entry.collectable:ItemID()

				if collectable_item_id then
					local _, item_link = _G.GetItemInfo(collectable_item_id)

					if item_link then
						local edit_box = _G.ChatEdit_ChooseBoxForSend()
						_G.ChatEdit_ActivateChat(edit_box)
						edit_box:Insert(item_link)
					else
						addon:Print(L["NoItemLink"])
					end
				else
					addon:Print(L["NoItemInDB"])
				end
			elseif _G.IsAltKeyDown() then
				local exclusion_list = addon.db.profile.exclusionlist
				local collectable = entry.collectable

				exclusion_list[collectable.type][collectable.id] = (not exclusion_list[collectable.type][collectable.id] and true or nil)
				ListFrame:Update(nil, false)
			end
		elseif entry:IsHeader() or entry:IsSubHeader() then
			-- three possibilities here (all with no modifiers)
			-- 1) We clicked on the collectable button on a closed collectable
			-- 2) We clicked on the collectable button of an open collectable
			-- 3) we clicked on the expanded text of an open collectable
			if entry.is_expanded then
				local removal_index = clicked_index + 1
				local target_entry = ListFrame.entries[removal_index]
				local current_tab = MainPanel.current_tab

				while target_entry and target_entry:Type() ~= entry:Type() do
					-- Headers are never removed.
					if target_entry:IsHeader() then
						break
					end
					current_tab:SaveListEntryState(target_entry, false)
					private.ReleaseTable(table.remove(ListFrame.entries, removal_index))
					target_entry = ListFrame.entries[removal_index]
				end
				current_tab:SaveListEntryState(entry, false)
				entry.is_expanded = false
			else
				ListFrame:ExpandEntry(entry)
				entry.is_expanded = true
			end
		else
			-- clicked_line is an expanded entry - find the index for its parent, and remove all of the parent's child entries.
			local parent = entry.parent

			if parent then
				local parent_index = parent.button.entry_index

				if not parent_index then
					addon:Debug("clicked_line (%s): parent wasn't found in ListFrame.entries", entry:Text())
					return
				end
				local current_tab = MainPanel.current_tab

				parent.is_expanded = false
				current_tab:SaveListEntryState(parent, false)

				local child_index = parent_index + 1
				local entries = ListFrame.entries

				while entries[child_index] and entries[child_index].parent == parent do
					private.ReleaseTable(table.remove(entries, child_index))
				end
			else
				addon:Debug("Error: clicked_line (%s) has no parent.", entry:Type() or _G.UNKNOWN)
			end
		end
		ListFrame:Update(nil, true)
	end

	-------------------------------------------------------------------------------
	-- The state and entry buttons and the container frames which hold them.
	-------------------------------------------------------------------------------
	ListFrame.entries = {}
	ListFrame.button_containers = {}
	ListFrame.state_buttons = {}
	ListFrame.entry_buttons = {}

	for index = 1, NUM_COLLECTABLE_LINES do
		local cur_container = _G.CreateFrame("Frame", nil, ListFrame)
		cur_container:SetSize(LIST_ENTRY_WIDTH, 16)

		local cur_state = _G.CreateFrame("Button", nil, ListFrame)
		cur_state:SetSize(16, 16)

		local cur_entry = _G.CreateFrame("Button", ("%s_ListEntryButton%d"):format(FOLDER_NAME, index), cur_container)
		cur_entry:SetSize(LIST_ENTRY_WIDTH, 16)
		cur_entry:RegisterForClicks("AnyUp")

		local highlight_texture = cur_entry:CreateTexture(nil, "BORDER")
		highlight_texture:SetTexture([[Interface\ClassTrainerFrame\TrainerTextures]])
		highlight_texture:SetTexCoord(0.00195313, 0.57421875, 0.75390625, 0.84570313)
		highlight_texture:SetBlendMode("ADD")
		highlight_texture:SetPoint("TOPLEFT", 2, 0)
		highlight_texture:SetPoint("BOTTOMRIGHT", -2, 1)
		cur_entry:SetHighlightTexture(highlight_texture)

		local selected_texture = cur_entry:CreateTexture(nil, "BORDER")
		selected_texture:SetTexture([[Interface\ClassTrainerFrame\TrainerTextures]])
		selected_texture:SetTexCoord(0.00195313, 0.57421875, 0.84960938, 0.94140625)
		selected_texture:SetBlendMode("ADD")
		selected_texture:SetPoint("TOPLEFT", 2, 0)
		selected_texture:SetPoint("BOTTOMRIGHT", -2, 1)
		cur_entry.selected_texture = selected_texture

		local emphasis_texture = cur_entry:CreateTexture(nil, "BORDER")
		emphasis_texture:SetTexture([[Interface\QUESTFRAME\Ui-QuestLogTitleHighlight]])
		emphasis_texture:SetVertexColor(1, 0.61, 0)
		emphasis_texture:SetBlendMode("ADD")
		emphasis_texture:SetPoint("TOPLEFT", 2, 0)
		emphasis_texture:SetPoint("BOTTOMRIGHT", -2, 1)
		cur_entry.emphasis_texture = emphasis_texture

		local label = cur_entry:CreateFontString(nil, "ARTWORK")
		label:SetPoint("LEFT", cur_entry, "LEFT", 7, 0)
		label:SetPoint("RIGHT", cur_entry, "RIGHT", -7, 0)
		label:SetFontObject("GameFontNormalSmall")
		label:SetJustifyH("LEFT")
		label:SetJustifyV("CENTER")

		cur_entry:SetFontString(label)
		cur_entry.text = label

		if index == 1 then
			cur_container:SetPoint("TOPLEFT", ListFrame, "TOPLEFT", 0, -3)
		else
			cur_container:SetPoint("TOPLEFT", ListFrame.button_containers[index - 1], "BOTTOMLEFT", 0, 3)
		end
		cur_state:SetPoint("LEFT", cur_container, "LEFT", 0, 0)
		cur_entry:SetPoint("LEFT", cur_state, "RIGHT", -3, 0)

		cur_state.container = cur_container

		cur_state:SetScript("OnClick", ListItem_OnClick)

		ListFrame.button_containers[index] = cur_container
		ListFrame.state_buttons[index] = cur_state
		ListFrame.entry_buttons[index] = cur_entry
	end

	function ListFrame:InsertEntry(entry, entry_index, entry_expanded, expand_mode)
		local insert_index = entry_index
		entry.index = entry_index

		-- If we have acquire information for this entry, push the data table into the list
		-- and start processing the acquires.
		if expand_mode then
			entry.is_expanded = true
			table.insert(self.entries, insert_index, entry)

			MainPanel.current_tab:SaveListEntryState(entry, entry_expanded)

			if entry:IsHeader() or entry:IsSubHeader() then
				insert_index = self:ExpandEntry(entry, expand_mode)
			else
				insert_index = insert_index + 1
			end
		else
			entry.is_expanded = entry_expanded
			table.insert(self.entries, insert_index, entry)

			insert_index = insert_index + 1
		end
		return insert_index
	end

	-------------------------------------------------------------------------------
	-- Filter flag data and functions for ListFrame:Initialize()
	-------------------------------------------------------------------------------
	do
		local filter_db		= addon.db.profile.filters

		local binding_filters	= filter_db.binding
		local player_filters	= filter_db.player
		local obtain_filters	= filter_db.obtain
		local general_filters	= filter_db.general

		local Q = private.ITEM_QUALITIES
		local QUALITY_FILTERS = {
			[Q.COMMON]	= "common",
			[Q.UNCOMMON]	= "uncommon",
			[Q.RARE]	= "rare",
			[Q.EPIC]	= "epic",
			[Q.LEGENDARY]	= "legendary",
		}

		-- HARD_FILTERS and SOFT_FILTERS are used to determine if a recipe should be shown based on the value of the key compared to the value
		-- of its saved_var.
		private.HARD_FILTERS = {
			------------------------------------------------------------------------------------------------
			-- Binding flags.
			------------------------------------------------------------------------------------------------
			["itemboe"]	= { flag = COMMON1.IBOE,	field = "common1",	sv_root = binding_filters },
			["itembop"]	= { flag = COMMON1.IBOP,	field = "common1",	sv_root = binding_filters },
			["itemboa"]	= { flag = COMMON1.IBOA,	field = "common1",	sv_root = binding_filters },
		}

		local SOFT_FILTERS = {
			["achievement"]		= { flag = COMMON1.ACHIEVEMENT,		field = "common1",	sv_root = obtain_filters },
			["profession"]		= { flag = COMMON1.PROFESSION,		field = "common1",	sv_root = obtain_filters },
			["instance"]		= { flag = COMMON1.INSTANCE,		field = "common1",	sv_root = obtain_filters },
			["mobdrop"]		= { flag = COMMON1.MOB_DROP,		field = "common1",	sv_root = obtain_filters },
			["pvp"]			= { flag = COMMON1.PVP,			field = "common1",	sv_root = obtain_filters },
			["quest"]		= { flag = COMMON1.QUEST,		field = "common1",	sv_root = obtain_filters },
			["raid"]		= { flag = COMMON1.RAID,		field = "common1",	sv_root = obtain_filters },
			["retired"]		= { flag = COMMON1.RETIRED,		field = "common1",	sv_root = general_filters },
			["reputation"]		= { flag = COMMON1.REPUTATION,		field = "common1",	sv_root = obtain_filters },
			["store"]		= { flag = COMMON1.STORE,		field = "common1",	sv_root = obtain_filters },
			["trainer"]		= { flag = COMMON1.TRAINER,		field = "common1",	sv_root = obtain_filters },
			["vendor"]		= { flag = COMMON1.VENDOR,		field = "common1",	sv_root = obtain_filters },
			["world_events"]	= { flag = COMMON1.WORLD_EVENTS,	field = "common1",	sv_root = obtain_filters },
			["worlddrop"]		= { flag = COMMON1.WORLD_DROP,		field = "common1",	sv_root = obtain_filters },
			["tcg"]			= { flag = COMMON1.TCG	,		field = "common1",	sv_root = obtain_filters },
			["coll_edition"]	= { flag = COMMON1.COLLECTORS_EDITION,	field = "common1",	sv_root = obtain_filters },
			["promo"]		= { flag = COMMON1.PROMO,		field = "common1",	sv_root = obtain_filters },
			["misc"]		= { flag = COMMON1.MISC,		field = "common1",	sv_root = obtain_filters },
		}

		local REP1 = private.REP_FLAGS_WORD1
		local REP_FILTERS = {
			[REP1.ARGENT_DAWN]			= "argentdawn",
			[REP1.CENARION_CIRCLE]			= "cenarioncircle",
			[REP1.THORIUM_BROTHERHOOD]		= "thoriumbrotherhood",
			[REP1.TIMBERMAW_HOLD]			= "timbermaw",
			[REP1.ZANDALAR_TRIBE]			= "zandalar",
			[REP1.THE_ALDOR]			= "aldor",
			[REP1.ASHTONGUE_DEATHSWORN]		= "ashtonguedeathsworn",
			[REP1.CENARION_EXPEDITION]		= "cenarionexpedition",
			[REP1.HELLFIRE]				= "hellfire",
			[REP1.THE_CONSORTIUM]			= "consortium",
			[REP1.KEEPERS_OF_TIME]			= "keepersoftime",
			[REP1.LOWER_CITY]			= "lowercity",
			[REP1.NAGRAND]				= "nagrand",
			[REP1.THE_SCALE_OF_THE_SANDS]		= "scaleofthesands",
			[REP1.THE_SCRYERS]			= "scryer",
			[REP1.SHATARI_SKYGUARD]			= "shatar",
			[REP1.SHATTERED_SUN_OFFENSIVE]		= "shatteredsun",
			[REP1.SPOREGGAR]			= "sporeggar",
			[REP1.THE_VIOLET_EYE]			= "violeteye",
			[REP1.ARGENT_CRUSADE]			= "argentcrusade",
			[REP1.FRENZYHEART_TRIBE]		= "frenzyheart",
			[REP1.KNIGHTS_OF_THE_EBON_BLADE]	= "ebonblade",
			[REP1.KIRIN_TOR]			= "kirintor",
			[REP1.THE_SONS_OF_HODIR]		= "sonsofhodir",
			[REP1.THE_KALUAK]			= "kaluak",
			[REP1.THE_ORACLES]			= "oracles",
			[REP1.THE_WYRMREST_ACCORD]		= "wyrmrest",
			[REP1.WRATHCOMMON1]			= "wrathcommon1",
			[REP1.WRATHCOMMON2]			= "wrathcommon2",
			[REP1.WRATHCOMMON3]			= "wrathcommon3",
			[REP1.WRATHCOMMON4]			= "wrathcommon4",
			[REP1.WRATHCOMMON5]			= "wrathcommon5",
		}

		local REP2 = private.REP_FLAGS_WORD2
		local REP_FILTERS_2 = {
			[REP2.THE_ASHEN_VERDICT]		= "ashenverdict",
			[REP2.CATACOMMON1]			= "catacommon1",
			[REP2.CATACOMMON2]			= "catacommon2",
			[REP2.GUARDIANS_OF_HYJAL]		= "guardiansofhyjal",
			[REP2.RAMKAHEN]				= "ramkahen",
			[REP2.THE_EARTHEN_RING]			= "earthenring",
			[REP2.THERAZANE]			= "therazane",
			[REP2.GOLDEN_LOTUS]			= "goldenlotus",
			[REP2.ORDER_OF_THE_CLOUD_SERPENT]	= "cloudserpent",
			[REP2.SHADO_PAN]			= "shadopan",
			[REP2.THE_ANGLERS]			= "anglers",
			[REP2.THE_AUGUST_CELESTIALS]		= "augustcelestials",
			[REP2.THE_BREWMASTERS]			= "brewmasters",
			[REP2.THE_KLAXXI]			= "klaxxi",
			[REP2.THE_LOREWALKERS]			= "lorewalkers",
			[REP2.THE_TILLERS]			= "tillers",
			[REP2.THE_BLACK_PRINCE]			= "blackprince",
			[REP2.SHANG_XIS_ACADEMY]		= "shangxiacademy",
			[REP2.PANDACOMMON1]			= "pandacommon1",
			[REP2.PANDACOMMON2]			= "pandacommon2",
			[REP2.GUILD]				= "guild",
			[REP2.NETHERWING]			= "netherwing",
			[REP2.BRAWLERS]				= "brawlers",
			[REP2.PANDACOMMON3]			= "pandacommon3",
			[REP2.EMPEROR_SHAOHAO]			= "shaohao",
		}

		-- Returns true if any of the filter flags are turned on.
		local function HasEnabledFlag(filters, bitfield, name_field)
			if not bitfield then
				return true
			end

			for flag, name in pairs(filters) do
				if bit.band(bitfield, flag) == flag then
					if name_field[name] then
						return true
					end
				end
			end
			return false
		end

		-- Scans a specific collectable to determine if it is to be displayed or not.
		local function CanDisplayCollectable(collectable)
			if addon.db.profile.exclusionlist[collectable.type][collectable.id] and not addon.db.profile.ignoreexclusionlist then
				return false
			end
			local general_filters = filter_db.general

			-------------------------------------------------------------------------------
			-- Stage 1 - Loop through exclusive flags (hard filters).
			-- If one of these does not pass, the recipe is not displayed.
			-------------------------------------------------------------------------------

			-- Display both horde and alliance factions?
			if not general_filters.faction and not private.Player:HasCollectableFaction(collectable) then
				return false
			end

			-- Expansion filters.
			if not obtain_filters[private.EXPANSION_FILTERS[private.GAME_VERSIONS[collectable.genesis]]] then
				return false
			end

			-- Quality filters.
			if not filter_db.quality[QUALITY_FILTERS[collectable.quality]] then
				return false
			end

			-------------------------------------------------------------------------------
			-- Check the hard filter flags.
			-------------------------------------------------------------------------------
			for filter, data in pairs(private.HARD_FILTERS) do
				local bitfield = collectable.flags[data.field]

				if bitfield and bit.band(bitfield, data.flag) == data.flag and not data.sv_root[filter] then
					return false
				end
			end

			-------------------------------------------------------------------------------
			-- Check the reputation filter flags.
			------------------------------------------------------------------------------
			if not HasEnabledFlag(REP_FILTERS, collectable.flags.reputation1, filter_db.rep) then
				return false
			end

			if not HasEnabledFlag(REP_FILTERS_2, collectable.flags.reputation2, filter_db.rep) then
				return false
			end

			------------------------------------------------------------------------------------------------
			-- Stage 2
			-- loop through nonexclusive (soft filters) flags until one is true
			-- If one of these is true (ie: we want to see trainers and there is a trainer flag) we display it
			------------------------------------------------------------------------------------------------
			for filter, data in pairs(SOFT_FILTERS) do
				local bitfield = collectable.flags[data.field]

				if bitfield and bit.band(bitfield, data.flag) == data.flag and data.sv_root[filter] then
					return true
				end
			end
		end

		function ListFrame:Initialize(expand_mode)
			for i = 1, #self.entries do
				private.ReleaseTable(self.entries[i])
			end
			table.wipe(self.entries)

			-------------------------------------------------------------------------------
			-- Update collectable filters.
			-------------------------------------------------------------------------------
			local general_filters = addon.db.profile.filters.general
			local collectables = private.collectable_list[private.ORDERED_COLLECTIONS[MainPanel.current_collectable_type]]
			local collectables_known, collectables_known_filtered = 0, 0
			local collectables_total, collectables_total_filtered = 0, 0

			for collectable_id, collectable in pairs(collectables) do
				local can_display = false
				collectable:RemoveState("VISIBLE")

				if not collectable.is_ignored then
					local is_known = collectable:HasState("KNOWN")

					collectables_total = collectables_total + 1
					collectables_known = collectables_known + (is_known and 1 or 0)

					can_display = CanDisplayCollectable(collectable)

					if can_display then
						collectables_total_filtered = collectables_total_filtered + 1
						collectables_known_filtered = collectables_known_filtered + (is_known and 1 or 0)

						if not general_filters.known and is_known then
							can_display = false
						end

						if not general_filters.unknown and not is_known then
							can_display = false
						end
					end
				end

				if can_display then
					collectable:AddState("VISIBLE")
				end
			end
			local player = private.Player
			player.collectables_total = collectables_total
			player.collectables_known = collectables_known
			player.collectables_total_filtered = collectables_total_filtered
			player.collectables_known_filtered = collectables_known_filtered

			-------------------------------------------------------------------------------
			-- Mark all exclusions in the collectable database to not be displayed, and update
			-- the player's known and unknown counts.
			-------------------------------------------------------------------------------
			local known_count = 0
			local unknown_count = 0

			for collectable_id in pairs(addon.db.profile.exclusionlist) do
				local collectable = collectables[collectable_id]

				if collectable then
					if collectable:HasState("KNOWN") then
						known_count = known_count + 1
					else
						unknown_count = unknown_count + 1
					end
				end
			end
			player.excluded_collectables_known = known_count
			player.excluded_collectables_unknown = unknown_count

			-------------------------------------------------------------------------------
			-- Initialize the expand button and entries for the current tab.
			-------------------------------------------------------------------------------
			local current_tab = MainPanel.tabs[addon.db.profile.current_tab]
			local expanded_button = current_tab["expand_button_" .. MainPanel.current_collectable_type]


			QTip:Release(acquire_tooltip)
			self.selected_entry = nil

			if expanded_button then
				MainPanel.expand_button:Expand(current_tab)
			else
				MainPanel.expand_button:Contract(current_tab)
			end
			local collectable_count = current_tab:Initialize(expand_mode)

			-------------------------------------------------------------------------------
			-- Update the progress bar display.
			-------------------------------------------------------------------------------
			local profile = addon.db.profile
			local max_value = profile.includefiltered and player.collectables_total or (player.collectables_total_filtered + (player.collectables_known - player.collectables_known_filtered))
			local cur_value = player.collectables_known	-- Current value will always be what we know regardless of filters.

			if not profile.includeexcluded and not profile.ignoreexclusionlist then
				max_value = max_value - player.excluded_collectables_known
			end
			local progress_bar = MainPanel.progress_bar

			progress_bar:SetMinMaxValues(0, max_value)
			progress_bar:SetValue(cur_value)

			local percentage = cur_value / max_value * 100

			if (math.floor(percentage) < 101) and cur_value >= 0 and max_value >= 0 then
				local results = _G.SINGLE_PAGE_RESULTS_TEMPLATE:format(collectable_count)
				progress_bar.text:SetFormattedText("%d/%d - %1.2f%% (%s)", cur_value, max_value, percentage, results)
			else
				progress_bar.text:SetFormattedText("%s", L["NOT_YET_SCANNED"])
			end
		end
	end	-- do-block

	-- Reset the current buttons/lines
	function ListFrame:ClearLines()
		for i = 1, NUM_COLLECTABLE_LINES do
			local entry = self.entry_buttons[i]
			entry.text:SetFontObject(addon.db.profile.frameopts.small_list_font and "GameFontNormalSmall" or "GameFontNormal")
			entry:SetText("")
			entry:SetScript("OnEnter", nil)
			entry:SetScript("OnLeave", nil)
			entry:SetScript("OnClick", nil)
			entry:SetWidth(LIST_ENTRY_WIDTH)
			entry.emphasis_texture:Hide()
			entry.selected_texture:Hide()
			entry:Disable()
			entry.button = nil
			entry.entry_index = 0

			local state = self.state_buttons[i]
			state.entry_index = 0
			state:Hide()
			state:Disable()
			state:ClearAllPoints()
		end
	end

	function ListFrame:Update(expand_mode, refresh)
		if not refresh then
			self:Initialize(expand_mode)
		end

		local num_entries = #self.entries

		if num_entries == 0 then
			self:ClearLines()

			-- disable expand button, it's useless here and would spam the same error again
			MainPanel.expand_button:SetNormalFontObject("GameFontDisableSmall")
			MainPanel.expand_button:Disable()
			self.scroll_bar:Hide()

			local showpopup = false

			if not addon.db.profile.hidepopup then
				showpopup = true
			end

			-- If we haven't run this before we'll show pop-ups for the first time.
			if addon.db.profile.addonversion ~= addon.version then
				addon.db.profile.addonversion = addon.version
				showpopup = true
			end
			local editbox_text = MainPanel.search_editbox:GetText()
			local player = private.Player

			if player.collectables_total == 0 then
				if showpopup then
					Dialog:Spawn("Collectinator_NotScanned")
				end
			elseif player.collectables_known == player.collectables_total then
				if showpopup then
					Dialog:Spawn("Collectinator_AllKnown")
				end
			elseif (player.collectables_total_filtered - player.collectables_known_filtered) == 0 then
				if showpopup then
					Dialog:Spawn("Collectinator_AllFiltered")
				end
			elseif player.excluded_collectables_unknown ~= 0 then
				if showpopup then
					Dialog:Spawn("Collectinator_AllExcluded")
				end
			elseif editbox_text ~= "" and editbox_text ~= _G.SEARCH then
				Dialog:Spawn("Collectinator_SearchFiltered")
			else
				addon:Print(L["NO_DISPLAY"])
				addon:Debug("Current tab is %s", _G.tostring(addon.db.profile.current_tab))
				addon:Debug("collectables_total check for 0")
				addon:Debug("collectables_total: " .. player.collectables_total)
				addon:Debug("collectables_total check for equal to collectables_total")
				addon:Debug("collectables_known: " .. player.collectables_known)
				addon:Debug("collectables_total: " .. player.collectables_total)
				addon:Debug("collectables_total_filtered - collectables_known_filtered = 0")
				addon:Debug("collectables_total_filtered: " .. player.collectables_total_filtered)
				addon:Debug("collectables_known_filtered: " .. player.collectables_known_filtered)
				addon:Debug("excluded_collectables_unknown ~= 0")
				addon:Debug("excluded_collectables_unknown: " .. player.excluded_collectables_unknown)
			end
			return
		end
		local offset = 0

		private.DismissDialogs()

		MainPanel.expand_button:SetNormalFontObject("GameFontNormalSmall")
		MainPanel.expand_button:Enable()

		if num_entries <= NUM_COLLECTABLE_LINES then
			self.scroll_bar:Hide()
		else
			local max_val = num_entries - NUM_COLLECTABLE_LINES
			local current_tab = MainPanel.current_tab
			local scroll_value = math.max(0, math.min(current_tab:ScrollValue(MainPanel.current_collectable_type) or 0, max_val))
			offset = scroll_value

			self.scroll_bar:SetMinMaxValues(0, math.max(0, max_val))
			self.scroll_bar:SetValue(scroll_value)
			self.scroll_bar:Show()
		end
		self:ClearLines()

		local button_index = 1
		local entry_index = math.floor(button_index + offset)

		-- Populate the buttons with new values
		while button_index <= NUM_COLLECTABLE_LINES and entry_index <= num_entries do
			local state_button = self.state_buttons[button_index]
			local list_entry = self.entries[entry_index]
			local is_entry = list_entry:IsEntry()
			local is_subentry = not is_entry and list_entry:IsSubEntry()
			local is_header = not is_subentry and list_entry:IsHeader()
			local is_subheader = not is_header and list_entry:IsSubHeader()

			if is_header or is_subheader then
				state_button:Show()

				if list_entry.is_expanded then
					state_button:SetNormalTexture([[Interface\MINIMAP\UI-Minimap-ZoomOutButton-Up]])
					state_button:SetPushedTexture([[Interface\MINIMAP\UI-Minimap-ZoomOutButton-Down]])
					state_button:SetHighlightTexture([[Interface\MINIMAP\UI-Minimap-ZoomButton-Highlight]])
				else
					state_button:SetNormalTexture([[Interface\MINIMAP\UI-Minimap-ZoomInButton-Up]])
					state_button:SetPushedTexture([[Interface\MINIMAP\UI-Minimap-ZoomInButton-Down]])
					state_button:SetHighlightTexture([[Interface\MINIMAP\UI-Minimap-ZoomButton-Highlight]])
				end
				state_button.entry_index = entry_index
				state_button:Enable()
			else
				state_button:Hide()
				state_button:Disable()
			end
			local line_button = self.entry_buttons[button_index]

			if list_entry == ListFrame.selected_entry then
				line_button.selected_texture:Show()
			end

			if list_entry:IsEmphasized() then
				line_button.emphasis_texture:Show()
			end

			if is_header or is_entry then
				state_button:SetPoint("TOPLEFT", state_button.container, "TOPLEFT", 0, 0)
			elseif is_subheader or is_subentry then
				state_button:SetPoint("TOPLEFT", state_button.container, "TOPLEFT", 15, 0)
				line_button:SetWidth(LIST_ENTRY_WIDTH - 15)
			end
			list_entry.button = line_button
			line_button.entry_index = entry_index

			line_button:SetText(list_entry:Text())
			line_button:SetScript("OnEnter", Bar_OnEnter)
			line_button:SetScript("OnLeave", Bar_OnLeave)

			line_button:SetScript("OnClick", ListItem_OnClick)
			line_button:Enable()

			-- This function could possibly have been called from a mouse click or by scrolling. Since, in those cases, the list entries have
			-- changed, the mouse is likely over a different entry - a tooltip should be generated for it.
			if line_button:IsMouseOver() then
				Bar_OnEnter(line_button)
			end
			button_index = button_index + 1
			entry_index = entry_index + 1
		end
	end

	-------------------------------------------------------------------------------
	-- Functions and data pertaining to individual list entries.
	-------------------------------------------------------------------------------
	local function CanDisplayFaction(faction)
		if addon.db.profile.filters.general.faction then
			return true
		end
		return (not faction or faction == private.Player:Faction() or faction == "Neutral")
	end

	-- Padding for list entries/subentries
	local PADDING = "    "

	-- Changes the color of "name" based on faction type.
	local function ColorNameByFaction(name, faction)
		if faction == "Neutral" then
			name = SetTextColor(private.REPUTATION_COLORS["neutral"], name)
		elseif faction == private.Player:Faction() then
			name = SetTextColor(private.REPUTATION_COLORS["exalted"], name)
		else
			name = SetTextColor(private.REPUTATION_COLORS["hated"], name)
		end
		return name
	end

	local function ExpandTrainerData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local trainer = private.trainer_list[id_num]

		if not trainer or not CanDisplayFaction(trainer.faction) then
			return entry_index
		end

		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(id_num)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["trainer"], L["Trainer"]) .. ":",
			ColorNameByFaction(trainer.name, trainer.faction)
		)

		entry_index = ListFrame:InsertEntry(entry, entry_index, true)

		local coord_text = ""

		if trainer.coord_x ~= 0 and trainer.coord_y ~= 0 then
			coord_text = SetTextColor(CATEGORY_COLORS["coords"], COORD_FORMAT:format(trainer.coord_x, trainer.coord_y))
		end

		if coord_text == "" and hide_location then
			return entry_index
		end

		entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(id_num)
		entry:SetText("%s%s%s %s",
			PADDING,
			PADDING,
			hide_location and "" or SetTextColor(CATEGORY_COLORS["location"], trainer.location),
			coord_text
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	-- Right now PVP obtained items are located on vendors so they have the vendor and PVP flag.
	-- We need to display the vendor in the drop down if we want to see vendors or if we want to see PVP
	-- This allows us to select PVP only and to see just the PVP recipes
	local function ExpandVendorData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local vendor = private.vendor_list[id_num]

		if not CanDisplayFaction(vendor.faction) then
			return entry_index
		end

		local quantity = vendor.item_list[collectable.id]
		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(id_num)
		entry:SetText("%s%s %s%s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["vendor"], _G.BATTLE_PET_SOURCE_3) .. ":",
			ColorNameByFaction(vendor.name, vendor.faction),
			type(quantity) == "number" and SetTextColor(BASIC_COLORS["white"], (" (%d)"):format(quantity)) or ""
		)

		entry_index = ListFrame:InsertEntry(entry, entry_index, true)

		local coord_text = ""

		if vendor.coord_x ~= 0 and vendor.coord_y ~= 0 then
			coord_text = SetTextColor(CATEGORY_COLORS["coords"], COORD_FORMAT:format(vendor.coord_x, vendor.coord_y))
		end

		if coord_text == "" and hide_location then
			return entry_index
		end
		entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(id_num)
		entry:SetText("%s%s%s %s",
			PADDING,
			PADDING,
			hide_location and "" or SetTextColor(CATEGORY_COLORS["location"], vendor.location),
			coord_text
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	-- Mobs can be in instances, raids, or specific mob related drops.
	local function ExpandMobData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local mob = private.mob_list[id_num]
		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(id_num)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["mobdrop"], L["Mob Drop"]) .. ":",
			SetTextColor(private.REPUTATION_COLORS["hostile"], mob.name)
		)

		entry_index = ListFrame:InsertEntry(entry, entry_index, true)

		local coord_text = ""

		if mob.coord_x ~= 0 and mob.coord_y ~= 0 then
			coord_text = SetTextColor(CATEGORY_COLORS["coords"], COORD_FORMAT:format(mob.coord_x, mob.coord_y))
		end

		if coord_text == "" and hide_location then
			return entry_index
		end

		entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(id_num)
		entry:SetText("%s%s%s %s",
			PADDING,
			PADDING,
			hide_location and "" or SetTextColor(CATEGORY_COLORS["location"], mob.location),
			coord_text
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandQuestData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local quest = private.quest_list[id_num]

		if not CanDisplayFaction(quest.faction) then
			return entry_index
		end

		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["quest"], _G.BATTLE_PET_SOURCE_2) .. ":",
			ColorNameByFaction(private.quest_names[id_num], quest.faction)
		)

		entry_index = ListFrame:InsertEntry(entry, entry_index, true)

		local coord_text = ""

		if quest.coord_x ~= 0 and quest.coord_y ~= 0 then
			coord_text = SetTextColor(CATEGORY_COLORS["coords"], COORD_FORMAT:format(quest.coord_x, quest.coord_y))
		end

		if coord_text == "" and hide_location then
			return entry_index
		end

		entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText("%s%s%s %s",
			PADDING,
			PADDING,
			hide_location and "" or SetTextColor(CATEGORY_COLORS["location"], quest.location),
			coord_text
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandWorldEventsData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local hex_color = CATEGORY_COLORS["worldevents"]
		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(hex_color, private.ACQUIRE_NAMES[A.WORLD_EVENTS]) .. ":",
			SetTextColor(hex_color, private.world_events_list[id_num].name)
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local FACTION_LABELS

	local function ExpandReputationData(entry_index, entry_type, parent_entry, vendor_id, rep_id, rep_level, collectable, hide_location, hide_type)
		local rep_vendor = private.vendor_list[vendor_id]

		if not CanDisplayFaction(rep_vendor.faction) then
			return entry_index
		end

		if not FACTION_LABELS then
			local rep_color = private.REPUTATION_COLORS

			FACTION_LABELS = {
				[0] = SetTextColor(rep_color["neutral"], REPLEVEL["Neutral"] .. " : "),
				[1] = SetTextColor(rep_color["friendly"], REPLEVEL["Friendly"] .. " : "),
				[2] = SetTextColor(rep_color["honored"], REPLEVEL["Honored"] .. " : "),
				[3] = SetTextColor(rep_color["revered"], REPLEVEL["Revered"] .. " : "),
				[4] = SetTextColor(rep_color["exalted"], REPLEVEL["Exalted"] .. " : ")
			}
		end

		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(vendor_id)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["reputation"], _G.REPUTATION) .. ":",
			SetTextColor(CATEGORY_COLORS["repname"], private.reputation_list[rep_id].name)
		)

		entry_index = ListFrame:InsertEntry(entry, entry_index, true)

		entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(vendor_id)
		entry:SetText(PADDING .. PADDING .. FACTION_LABELS[rep_level] .. ColorNameByFaction(rep_vendor.name, rep_vendor.faction))

		entry_index = ListFrame:InsertEntry(entry, entry_index, true)

		local coord_text = ""

		if rep_vendor.coord_x ~= 0 and rep_vendor.coord_y ~= 0 then
			coord_text = SetTextColor(CATEGORY_COLORS["coords"], COORD_FORMAT:format(rep_vendor.coord_x, rep_vendor.coord_y))
		end

		if coord_text == "" and hide_location then
			return entry_index
		end

		entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetNPCID(vendor_id)
		entry:SetText("%s%s%s%s %s",
			PADDING,
			PADDING,
			PADDING,
			hide_location and "" or SetTextColor(CATEGORY_COLORS["location"], rep_vendor.location),
			coord_text
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandWorldDropData(entry_index, entry_type, parent_entry, identifier, collectable, hide_location, hide_type)
		local drop_location = type(identifier) == "string" and SetTextColor(CATEGORY_COLORS["location"], identifier)

		if drop_location then
			local collectable_item_id = collectable:ItemID()
			local collectable_item_level = collectable_item_id and select(4, _G.GetItemInfo(collectable_item_id))

			if collectable_item_level then
				drop_location = (": %s %s"):format(drop_location, SetTextColor(CATEGORY_COLORS["location"], "(%d - %d)"):format(collectable_item_level - 5, collectable_item_level + 5))
			else
				drop_location = (": %s"):format(drop_location)
			end
		else
			drop_location = ""
		end

		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText("%s|c%s%s|r%s",
			PADDING,
			select(4, _G.GetItemQualityColor(collectable.quality)),
			L["World Drop"],
			drop_location
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandCustomData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText(PADDING .. SetTextColor(CATEGORY_COLORS["custom"], private.custom_list[id_num].name))

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandProfessionData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["profession"], _G.BATTLE_PET_SOURCE_4) .. ":",
			SetTextColor(BASIC_COLORS["normal"], id_num)
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandAchievementData(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
		local entry = CreateListEntry(entry_type, parent_entry, collectable)
		entry:SetText("%s%s %s",
			PADDING,
			hide_type and "" or SetTextColor(CATEGORY_COLORS["achievement"], _G.ACHIEVEMENTS) .. ":",
			SetTextColor(BASIC_COLORS["normal"], select(2, _G.GetAchievementInfo(id_num)))
		)

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandRetiredData(entry_index, entry_type, parent_entry, id_num, recipe, _, _)
		local entry = CreateListEntry(entry_type, parent_entry, recipe)
		entry:SetText(PADDING .. SetTextColor(CATEGORY_COLORS.retired, L.RETIRED_COLLECTABLE_LONG))

		return ListFrame:InsertEntry(entry, entry_index, true)
	end

	local function ExpandAcquireData(entry_index, entry_type, parent_entry, acquire_type, acquire_data, collectable, hide_location, hide_type)
		local obtain_filters = addon.db.profile.filters.obtain
		local num_acquire_types = #private.ACQUIRE_STRINGS

		for id_num, info in pairs(acquire_data) do
			local func

			if acquire_type == A.TRAINER and obtain_filters.trainer then
				func = ExpandTrainerData
			elseif acquire_type == A.VENDOR and (obtain_filters.vendor or obtain_filters.pvp) then
				func = ExpandVendorData
			elseif acquire_type == A.MOB_DROP and (obtain_filters.mobdrop or obtain_filters.instance or obtain_filters.raid) then
				func = ExpandMobData
			elseif acquire_type == A.QUEST and obtain_filters.quest then
				func = ExpandQuestData
			elseif acquire_type == A.WORLD_EVENTS and obtain_filters.world_events then
				func = ExpandWorldEventsData
			elseif acquire_type == A.REPUTATION then
				for rep_level, level_info in pairs(info) do
					for vendor_id in pairs(level_info) do
						entry_index =  ExpandReputationData(entry_index, entry_type, parent_entry, vendor_id, id_num, rep_level, collectable, hide_location, hide_type)
					end
				end
			elseif acquire_type == A.WORLD_DROP and obtain_filters.worlddrop then
				if not hide_type then
					func = ExpandWorldDropData
				end
			elseif acquire_type == A.CUSTOM then
				if not hide_type then
					func = ExpandCustomData
				end
			elseif acquire_type == A.PROFESSION and obtain_filters.profession then
				func = ExpandProfessionData
			elseif acquire_type == A.RETIRED then
				if not hide_type then
					func = ExpandRetiredData
				end
				--@alpha@
			elseif acquire_type == A.ACHIEVEMENT and obtain_filters.achievement then
				func = ExpandAchievementData
			elseif acquire_type > num_acquire_types then
				local entry = CreateListEntry(entry_type, parent_entry, collectable)
				entry:SetText("Unhandled Acquire Case - Type: " .. acquire_type)

				entry_index = ListFrame:InsertEntry(entry, entry_index, true)
				--@end-alpha@
			end

			if func then
				entry_index = func(entry_index, entry_type, parent_entry, id_num, collectable, hide_location, hide_type)
			end
		end	-- for
		return entry_index
	end

	-- Recipes with these acquire types will never show as headers.
	local CHILDLESS_ACQUIRE_TYPES = {
		[A.ACHIEVEMENT] = true,
		[A.CUSTOM] = true,
		[A.RETIRED] = true,
		[A.WORLD_DROP] = true,
	}

	-- This function is called when an un-expanded entry in the list has been clicked.
	function ListFrame:ExpandEntry(entry, expand_mode)
		local orig_index = entry.button and entry.button.entry_index or entry.index
		local expand_all = expand_mode == "deep"
		local current_tab = MainPanel.current_tab
		local collectable_type = private.ORDERED_COLLECTIONS[MainPanel.current_collectable_type]
		local collectables = private.collectable_list[collectable_type]

		-- Entry_index is the position in self.entries that we want to expand. Since we are expanding the current entry, the return
		-- value should be the index of the next button after the expansion occurs
		local new_entry_index = orig_index + 1

		current_tab:SaveListEntryState(entry, true)

		local acquire_id = entry:AcquireID()

		-- This entry was generated using sorting based on Acquisition.
		if acquire_id then
			if entry:IsHeader() then
				local acquired_collectables = private.acquire_list[acquire_id].collectables[collectable_type]
				local sorted_collectables = addon.sorted_collectables

				private.SortCollectables(acquired_collectables, collectable_type)

				for index = 1, #sorted_collectables do
					local collectable = collectables[sorted_collectables[index]]

					if collectable and collectable:HasState("VISIBLE") and MainPanel.search_editbox:MatchesCollectable(collectable) then
						local expand = false
						local entry_type = "subheader"

						if CHILDLESS_ACQUIRE_TYPES[acquire_id] then
							expand = true
							entry_type = "entry"
						end
						local is_expanded = (current_tab[collectable_type .." expanded"][collectable] and current_tab[collectable_type .." expanded"][private.ACQUIRE_NAMES[acquire_id]])

						local new_entry = CreateListEntry(entry_type, entry, collectable)
						new_entry:SetAcquireID(acquire_id)
						new_entry:SetText(collectable:GetDisplayName())

						new_entry_index = self:InsertEntry(new_entry, new_entry_index, expand or is_expanded, expand_all or is_expanded)
					end
				end
			elseif entry:IsSubHeader() then
				for acquire_type, acquire_data in pairs(entry.collectable.acquire_data) do
					if acquire_type == acquire_id then
						new_entry_index = ExpandAcquireData(new_entry_index, "subentry", entry, acquire_type, acquire_data, entry.collectable, false, true)
					end
				end
			end
			return new_entry_index
		end

		local location_id = entry:LocationID()

		-- This entry was generated using sorting based on Location.
		if location_id then
			if entry:IsHeader() then
				local location_collectables = private.location_list[location_id].collectables[collectable_type]
				local sorted_collectables = addon.sorted_collectables

				private.SortCollectables(location_collectables, collectable_type)

				for index = 1, #sorted_collectables do
					local collectable_id = sorted_collectables[index]
					local collectable = collectables[collectable_id]

					if collectable and collectable:HasState("VISIBLE") and MainPanel.search_editbox:MatchesCollectable(collectable) then
						local expand = false
						local entry_type = "subheader"
						local location_type = location_collectables[collectable_id]

						-- Add World Drop entries as normal entries.
						if location_type and (location_type == "world_drop" or location_type == "pet_battle") then
							expand = true
							entry_type = "entry"
						end
						local is_expanded = (current_tab[collectable_type .." expanded"][collectable] and current_tab[collectable_type .." expanded"][location_id])

						local new_entry = CreateListEntry(entry_type, entry, collectable)
						new_entry:SetText(collectable:GetDisplayName())
						new_entry:SetLocationID(location_id)

						new_entry_index = self:InsertEntry(new_entry, new_entry_index, entry_type, expand or is_expanded, expand_all or is_expanded)
					end
				end
			elseif entry:IsSubHeader() then
				-- World Drops are not handled here because they are of type "entry".
				for acquire_type, acquire_data in pairs(entry.collectable.acquire_data) do
					-- Only expand an acquisition entry if it is from this location.
					for id_num, info in pairs(acquire_data) do
						if acquire_type == A.TRAINER and private.trainer_list[id_num].location == location_id then
							new_entry_index = ExpandTrainerData(new_entry_index, "subentry", entry, id_num, entry.collectable, true)
						elseif acquire_type == A.VENDOR and private.vendor_list[id_num].location == location_id then
							new_entry_index = ExpandVendorData(new_entry_index, "subentry", entry, id_num, entry.collectable, true)
						elseif acquire_type == A.MOB_DROP and private.mob_list[id_num].location == location_id then
							new_entry_index = ExpandMobData(new_entry_index, "subentry", entry, id_num, entry.collectable, true)
						elseif acquire_type == A.QUEST and private.quest_list[id_num].location == location_id then
							new_entry_index = ExpandQuestData(new_entry_index, "subentry", entry, id_num, entry.collectable, true)
						elseif acquire_type == A.WORLD_EVENTS and private.world_events_list[id_num].location == location_id then
							-- Hide the acquire type for this - it will already show up in the location list as
							-- "World Events".
							new_entry_index = ExpandWorldEventsData(new_entry_index, "subentry", entry, id_num, entry.collectable, true, true)
						elseif acquire_type == A.CUSTOM and private.custom_list[id_num].location == location_id then
							new_entry_index = ExpandCustomData(new_entry_index, "subentry", entry, id_num, entry.collectable, true, true)
						elseif acquire_type == A.REPUTATION then
							for rep_level, level_info in pairs(info) do
								for vendor_id in pairs(level_info) do
									if private.vendor_list[vendor_id].location == location_id then
										new_entry_index =  ExpandReputationData(new_entry_index, "subentry", entry, vendor_id, id_num, rep_level, entry.collectable, true)
									end
								end
							end
						end
					end
				end
			end
			return new_entry_index
		end

		-- Normal entry - expand all acquire types.
		local collectable = self.entries[orig_index].collectable

		for acquire_type, acquire_data in pairs(collectable.acquire_data) do
			new_entry_index = ExpandAcquireData(new_entry_index, "entry", entry, acquire_type, acquire_data, collectable)
		end
		return new_entry_index
	end
end	-- InitializeListFrame()

-------------------------------------------------------------------------------
-- Tooltip functions and data.
-------------------------------------------------------------------------------
-- Font Objects needed for acquire_tip
local narrowFont
local normalFont

do
	local LSM3 = LibStub("LibSharedMedia-3.0", true)

	if LSM3 then
		narrowFont = LSM3:Fetch(LSM3.MediaType.FONT, "Arial Narrow")
		normalFont = LSM3:Fetch(LSM3.MediaType.FONT, "Friz Quadrata TT")
	else
		-- Fix for font issues on koKR
		if _G.GetLocale() == "koKR" then
			narrowFont = "Fonts\\2002.TTF"
			normalFont = "Fonts\\2002.TTF"
		else
			narrowFont = "Fonts\\ARIALN.TTF"
			normalFont = "Fonts\\FRIZQT__.TTF"
		end
	end
	local narrowFontObj = _G.CreateFont(("%s%s"):format(private.addon_name, "narrowFontObj"))
	local normalFontObj = _G.CreateFont(("%s%s"):format(private.addon_name, "normalFontObj"))

	-- I want to do a bit more comprehensive tooltip processing. Things like changing font sizes,
	-- adding padding to the left hand side, and using better color handling. So... this function
	-- will do that for me.
	local function ttAdd(
			leftPad,		-- number of times to pad two spaces on left side
			textSize,		-- add to or subtract from addon.db.profile.tooltip.acquire_fontsize to get fontsize
			narrow,			-- if 1, use ARIALN instead of FRITZQ
			str1,			-- left-hand string
			hexcolor1,		-- hex color code for left-hand side
			str2,			-- if present, this is the right-hand string
			hexcolor2)		-- if present, hex color code for right-hand side

		-- are we changing fontsize or narrow?
		local fontSize

		if narrow or textSize ~= 0 then
			local font = narrow and narrowFont or normalFont
			local fontObj = narrow and narrowFontObj or normalFontObj

			fontSize = addon.db.profile.tooltip.acquire_fontsize + textSize

			fontObj:SetFont(font, fontSize)
			acquire_tooltip:SetFont(fontObj)
		end

		-- Add in our left hand padding
		local loopPad = leftPad
		local leftStr = str1

		while loopPad > 0 do
			leftStr = "    " .. leftStr
			loopPad = loopPad - 1
		end
		-- Set maximum width to match fontSize to maintain uniform tooltip size. -Torhal
		local width = math.ceil(fontSize * 37.5)
		local line = acquire_tooltip:AddLine()

		if str2 then
			width = width / 2

			acquire_tooltip:SetCell(line, 1, "|cff"..hexcolor1..leftStr.."|r", "LEFT", nil, nil, 0, 0, width, width)
			acquire_tooltip:SetCell(line, 2, "|cff"..hexcolor2..str2.."|r", "RIGHT", nil, nil, 0, 0, width, width)
		else
			acquire_tooltip:SetCell(line, 1, "|cff"..hexcolor1..leftStr.."|r", nil, "LEFT", 2, nil, 0, 0, width, width)
		end
	end

	local function GetTipFactionInfo(comp_faction)
		local display_tip
		local color

		if comp_faction == "Neutral" then
			color = private.REPUTATION_COLORS["neutral"]
			display_tip = true
		elseif comp_faction == private.Player:Faction() then
			color = private.REPUTATION_COLORS["exalted"]
			display_tip = true
		else
			color = private.REPUTATION_COLORS["hated"]
			display_tip = addon.db.profile.filters.general.faction
		end
		return display_tip, color
	end

	-------------------------------------------------------------------------------
	-- Functions for adding individual acquire type data to the tooltip.
	-------------------------------------------------------------------------------
	local TOOLTIP_ACQUIRE_FUNCS = {
		[A.TRAINER] = function(collectable, identifier, location, acquire_info, addline_func)
			local trainer = private.trainer_list[identifier]

			if not trainer or (location and trainer.location ~= location) then
				return
			end
			local display_tip, name_color = GetTipFactionInfo(trainer.faction)

			if not display_tip then
				return
			end
			addline_func(0, -2, false, L["Trainer"], CATEGORY_COLORS["trainer"], trainer.name, name_color)

			if trainer.coord_x ~= 0 and trainer.coord_y ~= 0 then
				addline_func(1, -2, true, trainer.location, CATEGORY_COLORS["location"], COORD_FORMAT:format(trainer.coord_x, trainer.coord_y), CATEGORY_COLORS["coords"])
			else
				addline_func(1, -2, true, trainer.location, CATEGORY_COLORS["location"], "", CATEGORY_COLORS["coords"])
			end
		end,
		[A.VENDOR] = function(collectable, identifier, location, acquire_info, addline_func)
			local vendor = private.vendor_list[identifier]

			if not vendor or (location and vendor.location ~= location) then
				return
			end
			local display_tip, name_color = GetTipFactionInfo(vendor.faction)

			if not display_tip then
				return
			end
			addline_func(0, -1, false, _G.BATTLE_PET_SOURCE_3, CATEGORY_COLORS["vendor"], vendor.name, name_color)

			if vendor.coord_x ~= 0 and vendor.coord_y ~= 0 then
				addline_func(1, -2, true, vendor.location, CATEGORY_COLORS["location"], COORD_FORMAT:format(vendor.coord_x, vendor.coord_y), CATEGORY_COLORS["coords"])
			else
				addline_func(1, -2, true, vendor.location, CATEGORY_COLORS["location"], "", CATEGORY_COLORS["coords"])
			end
		end,
		[A.MOB_DROP] = function(collectable, identifier, location, acquire_info, addline_func)
			local mob = private.mob_list[identifier]

			if not mob or (location and mob.location ~= location) then
				return
			end
			addline_func(0, -1, false, L["Mob Drop"], CATEGORY_COLORS["mobdrop"], mob.name, private.REPUTATION_COLORS["hostile"])

			if mob.coord_x ~= 0 and mob.coord_y ~= 0 then
				addline_func(1, -2, true, mob.location, CATEGORY_COLORS["location"], COORD_FORMAT:format(mob.coord_x, mob.coord_y), CATEGORY_COLORS["coords"])
			else
				addline_func(1, -2, true, mob.location, CATEGORY_COLORS["location"], "", CATEGORY_COLORS["coords"])

			end
		end,
		[A.QUEST] = function(collectable, identifier, location, acquire_info, addline_func)
			local quest = private.quest_list[identifier]

			if not quest or (location and quest.location ~= location) then
				return
			end
			local display_tip, name_color = GetTipFactionInfo(quest.faction)

			if not display_tip then
				return
			end
			addline_func(0, -1, false, _G.BATTLE_PET_SOURCE_2, CATEGORY_COLORS["quest"], private.quest_names[identifier], name_color)

			if quest.coord_x ~= 0 and quest.coord_y ~= 0 then
				addline_func(1, -2, true, quest.location, CATEGORY_COLORS["location"], COORD_FORMAT:format(quest.coord_x, quest.coord_y), CATEGORY_COLORS["coords"])
			else
				addline_func(1, -2, true, quest.location, CATEGORY_COLORS["location"], "", CATEGORY_COLORS["coords"])
			end
		end,
		[A.WORLD_EVENTS] = function(collectable, identifier, location, acquire_info, addline_func)
			local hex_color = CATEGORY_COLORS["worldevents"]
			addline_func(0, -1, 0, private.ACQUIRE_NAMES[A.WORLD_EVENTS], hex_color, private.world_events_list[identifier].name, hex_color)
		end,
		[A.REPUTATION] = function(collectable, identifier, location, acquire_info, addline_func)
			for rep_level, level_info in pairs(acquire_info) do
				for vendor_id in pairs(level_info) do
					local rep_vendor = private.vendor_list[vendor_id]

					if rep_vendor and (not location or rep_vendor.location == location) then
						local display_tip, name_color = GetTipFactionInfo(rep_vendor.faction)

						if display_tip then
							addline_func(0, -1, false, _G.REPUTATION, CATEGORY_COLORS["reputation"], private.reputation_list[identifier].name, CATEGORY_COLORS["repname"])

							if rep_level == 0 then
								addline_func(1, -2, false, REPLEVEL["Neutral"], private.REPUTATION_COLORS["neutral"], rep_vendor.name, name_color)
							elseif rep_level == 1 then
								addline_func(1, -2, false, REPLEVEL["Friendly"], private.REPUTATION_COLORS["friendly"], rep_vendor.name, name_color)
							elseif rep_level == 2 then
								addline_func(1, -2, false, REPLEVEL["Honored"], private.REPUTATION_COLORS["honored"], rep_vendor.name, name_color)
							elseif rep_level == 3 then
								addline_func(1, -2, false, REPLEVEL["Revered"], private.REPUTATION_COLORS["revered"], rep_vendor.name, name_color)
							else
								addline_func(1, -2, false, REPLEVEL["Exalted"], private.REPUTATION_COLORS["exalted"], rep_vendor.name, name_color)
							end

							if rep_vendor.coord_x ~= 0 and rep_vendor.coord_y ~= 0 then
								addline_func(2, -2, true, rep_vendor.location, CATEGORY_COLORS["location"], COORD_FORMAT:format(rep_vendor.coord_x, rep_vendor.coord_y), CATEGORY_COLORS["coords"])
							else
								addline_func(2, -2, true, rep_vendor.location, CATEGORY_COLORS["location"], "", CATEGORY_COLORS["coords"])
							end
						end
					end
				end
			end
		end,
		[A.WORLD_DROP] = function(collectable, identifier, location, acquire_info, addline_func)
			local drop_location = type(identifier) == "string" and identifier or _G.UNKNOWN

			if location and drop_location ~= location then
				return
			end
			local location_collectables = private.location_list[identifier].collectables[collectable.type]
			local drop_type = location_collectables[collectable.id]
			local quality_color = select(4, _G.GetItemQualityColor(collectable.quality)):sub(3)

			if drop_type == "world_drop" then
				addline_func(0, -1, false, L["World Drop"], quality_color, drop_location, CATEGORY_COLORS.location)
			elseif drop_type == "pet_battle" then
				for level_range, coord_list in pairs(collectable.zone_list[identifier]) do
					addline_func(0, -1, false, _G.BATTLE_PET_SOURCE_5, quality_color)

					for coord_index = 1, #coord_list do
                        if coord_list[coord_index] == "unknown" then
                            addline_func(1, -2, true, ("%s (%s)"):format(drop_location, level_range), CATEGORY_COLORS.location, _G.UNKNOWN, CATEGORY_COLORS.coords)
                        elseif coord_list[coord_index] == "secondary" then
                            addline_func(1, -1, true, ("%s (%s)"):format(drop_location, level_range), CATEGORY_COLORS.location, _G.SECONDARY, CATEGORY_COLORS.coords)
                        else
                            local x, y = (":"):split(coord_list[coord_index])
                            addline_func(1, -2, true, ("%s (%s)"):format(drop_location, level_range), CATEGORY_COLORS.location, COORD_FORMAT:format(x, y), CATEGORY_COLORS.coords)
                        end
                    end
				end
			else
				addline_func(0, -1, false, _G.UNKNOWN, quality_color, location, CATEGORY_COLORS["location"])
			end
		end,

		[A.PROFESSION] = function(collectable, identifier, location, acquire_info, addline_func)
			local profession_type = type(identifier) == "string" and identifier or _G.UNKNOWN

			if not profession_type then
				return
			end

			addline_func(0, -1, false, _G.BATTLE_PET_SOURCE_4, CATEGORY_COLORS["location"], profession_type, CATEGORY_COLORS["location"])
		end,

		[A.ACHIEVEMENT] = function(collectable, identifier, location, acquire_info, addline_func)
			local _, achievement_name, _, _, _, _, _, achievement_desc = _G.GetAchievementInfo(identifier)

			-- The recipe is an actual reward from an achievement if flagged - else we're just using the text to describe how to get it.
			if collectable:HasFilter("common1", "ACHIEVEMENT") then
				addline_func(0, -1, false, _G.ACHIEVEMENTS, CATEGORY_COLORS["achievement"], achievement_name, BASIC_COLORS["normal"])
			end
			addline_func(0, -1, false, achievement_desc, CATEGORY_COLORS["achievement"])
		end,
		[A.CUSTOM] = function(collectable, identifier, location, acquire_info, addline_func)
			addline_func(0, -1, false, private.custom_list[identifier].name, CATEGORY_COLORS["custom"])
		end,
		[A.RETIRED] = function(_, identifier, _, _, addline_func)
			addline_func(0, -1, false, L.RETIRED_COLLECTABLE_LONG, CATEGORY_COLORS.retired)
		end,
	}

	-------------------------------------------------------------------------------
	-- Public API function for displaying a recipe's acquire data.
	-- * The addline_func paramater must be a function which accepts the same
	-- * arguments as ARL's ttAdd function.
	-------------------------------------------------------------------------------
	function addon:DisplayAcquireData(collectable, acquire_id, location, addline_func)
		if not collectable then
			return
		end

		for acquire_type, acquire_data in pairs(collectable.acquire_data) do
			if not acquire_id or acquire_type == acquire_id then
				local populate_func = TOOLTIP_ACQUIRE_FUNCS[acquire_type]
				local count = 0

				for identifier, info in pairs(acquire_data) do
					if populate_func then
						populate_func(collectable, identifier, location, info, addline_func)
					else
						addline_func(0, -1, 0, _G.UNKNOWN, BASIC_COLORS["normal"])
					end
					count = count + 1
				end

				if count == 0 and populate_func then
					populate_func(collectable, nil, location, nil, addline_func)
				end
			end
		end
	end

	-------------------------------------------------------------------------------
	-- Main tooltip function.
	-------------------------------------------------------------------------------
	local function InitializeTooltips(collectable)
		local acquire_tip_anchor = addon.db.profile.acquiretooltiplocation
		local MainPanel = addon.Frame

		if acquire_tip_anchor == _G.OFF then
			QTip:Release(acquire_tooltip)
			return
		end
		acquire_tooltip = QTip:Acquire(private.addon_name.." Tooltip", 2, "LEFT", "LEFT")
		acquire_tooltip:ClearAllPoints()
		acquire_tooltip:SetClampedToScreen(true)
		acquire_tooltip:Clear()
		acquire_tooltip:SetScale(addon.db.profile.tooltip.scale)

		if _G.TipTac and _G.TipTac.AddModifiedTip then
			-- Pass true as second parameter because hooking OnHide causes C stack overflows -Torhal
			_G.TipTac:AddModifiedTip(acquire_tooltip, true)
		end

		if acquire_tip_anchor == "Right" then
			acquire_tooltip:SetPoint("TOPLEFT", MainPanel, "TOPRIGHT", MainPanel.is_expanded and -90 or -35, 0)
		elseif acquire_tip_anchor == "Left" then
			acquire_tooltip:SetPoint("TOPRIGHT", MainPanel, "TOPLEFT")
		elseif acquire_tip_anchor == "Top" then
			acquire_tooltip:SetPoint("BOTTOMLEFT", MainPanel, "TOPLEFT")
		elseif acquire_tip_anchor == "Bottom" then
			acquire_tooltip:SetPoint("TOPLEFT", MainPanel, "BOTTOMLEFT", 0, 55)
		elseif acquire_tip_anchor == "Mouse" then
			local x, y = _G.GetCursorPosition()
			local uiscale = _G.UIParent:GetEffectiveScale()

			acquire_tooltip:SetPoint("BOTTOMLEFT", _G.UIParent, "BOTTOMLEFT", x / uiscale, y / uiscale)
		end
	end

	local BINDING_FLAGS = {
		IBOE = L["BOEFilter"],
		IBOP = L["BOPFilter"],
		IBOA = L["BOAFilter"],
	}

	local NON_COORD_ACQUIRES = {
		[A.ACHIEVEMENT] = true,
		[A.CUSTOM] = true,
		[A.PROFESSION] = true,
		[A.RETIRED] = true,
		[A.WORLD_DROP] = true,
	}

	function ListItem_ShowTooltip(owner, list_entry)
		if not list_entry then
			return
		end
		local collectable = list_entry.collectable

		if not collectable then
			return
		end
		InitializeTooltips(collectable)

		if not acquire_tooltip then
			return
		end
		acquire_tooltip:AddHeader()
		acquire_tooltip:SetCell(1, 1, collectable:GetDisplayName(), "CENTER", 2)

		local collectable_icon = collectable:Icon()

		if collectable_icon then
			acquire_tooltip:AddHeader()
			acquire_tooltip:SetCell(2, 1, ("|T%s:30:30|t"):format(collectable_icon), "CENTER", 2)
		end
		ttAdd(0, -1, false, collectable:Description(), BASIC_COLORS["normal"])

		if addon.db.profile.exclusionlist[list_entry.collectable.id] then
			ttAdd(0, -1, true, L["RECIPE_EXCLUDED"], "ff0000")
		end
		acquire_tooltip:AddSeparator()

		for flag_name, label in pairs(BINDING_FLAGS) do
			if collectable:HasFilter("common1", flag_name) then
				ttAdd(0, -1, true, label, BASIC_COLORS["normal"])
			end
		end
		acquire_tooltip:AddSeparator()

		local required_races = collectable:RequiredRaces()

		if required_races and not collectable.is_ignored then
			local output = private.TextDump
			output:Clear()

			for race_name in pairs(required_races) do
				output:AddLine(race_name)
			end
			ttAdd(0, -1, false, _G.ITEM_RACES_ALLOWED:format(output:String(", ")), BASIC_COLORS["normal"])
		end
		ttAdd(0, -1, false, L["Obtained From"] .. " : ", BASIC_COLORS["normal"])

		addon:DisplayAcquireData(list_entry.collectable, list_entry:AcquireID(), list_entry:LocationID(), ttAdd)

		if not addon.db.profile.hide_tooltip_hint then
			local hint_color = private.CATEGORY_COLORS.hint

			acquire_tooltip:AddSeparator()
			acquire_tooltip:AddSeparator()

			ttAdd(0, -1, 0, L["ALT_CLICK"], hint_color)
			ttAdd(0, -1, 0, L["CTRL_CLICK"], hint_color)
			ttAdd(0, -1, 0, L["SHIFT_CLICK"], hint_color)

			if not NON_COORD_ACQUIRES[list_entry:AcquireID()] and _G.TomTom and (addon.db.profile.worldmap or addon.db.profile.minimap) then
				ttAdd(0, -1, 0, L["CTRL_SHIFT_CLICK"], hint_color)
			end
		end
		acquire_tooltip:Show()
		acquire_tooltip:UpdateScrolling(addon.Frame:GetHeight())
	end
end	-- do
