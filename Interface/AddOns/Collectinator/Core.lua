--[[
************************************************************************
Core.lua
Core functions for Collectinator
************************************************************************
File date: 2013-11-03T18:23:38Z
File hash: 0532103
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/collectinator/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]]

-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

-- Functions
local pairs, ipairs = _G.pairs, _G.ipairs
local select = _G.select
local tonumber, tostring = _G.tonumber, _G.tostring
local type = _G.type

-- Libraries
local bit = _G.bit
local string = _G.string
local table = _G.table

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):NewAddon(private.addon_name, "AceConsole-3.0", "AceEvent-3.0")
_G.Collectinator = addon

--@alpha@
_G.COL = addon
--@end-alpha@

local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)
local Toast = LibStub("LibToast-1.0")
local LPJ = LibStub("LibPetJournal-2.0")

local debugger -- Only defined if needed.

private.build_num = select(2, _G.GetBuildInfo())
private.TextDump = LibStub("LibTextDump-1.0"):New(private.addon_name)

-- Global Frame Variables
addon.optionsFrame = {}

-------------------------------------------------------------------------------
-- Debugger.
-------------------------------------------------------------------------------
local function CreateDebugFrame()
	if debugger then
		return
	end
	debugger = LibStub("LibTextDump-1.0"):New(("%s Debug Output"):format(private.addon_name), 640, 480)
end

function addon:Debug(...)
	if not debugger then
		CreateDebugFrame()
	end
	local text = string.format(...)
	debugger:AddLine(text)

	--[===[@debug@
	Toast:Spawn("Collectinator_DebugToast", text)
	--@end-debug@]===]
end

Toast:Register("Collectinator_DebugToast", function(toast, ...)
	toast:SetTitle(("%s - Debug"):format(private.addon_name))
	toast:SetText(...)
	toast:SetIconTexture([[Interface\HELPFRAME\HotIssueIcon]])
end)

------------------------------------------------------------------------------
-- Constants.
------------------------------------------------------------------------------
local COLLECTION_INIT_FUNCS

-------------------------------------------------------------------------------
-- Initialization functions
-------------------------------------------------------------------------------
local REQUIRED_LIBS = {
	"AceLocale-3.0",
	"LibBabble-Boss-3.0",
	"LibQTip-1.0",
	"LibToast-1.0"
}

function addon:OnInitialize()
	-------------------------------------------------------------------------------
	-- Check to see if we have mandatory libraries loaded. If not, notify the user
	-- which are missing and return.
	-------------------------------------------------------------------------------

	local missing_libraries = false

	for index = 1, #REQUIRED_LIBS do
		local library_name = REQUIRED_LIBS[index]

		if not LibStub:GetLibrary(library_name, true) then
			missing_libraries = true
			addon:Print(L["MISSING_LIBRARY"]:format(library_name))
		end
	end
	REQUIRED_LIBS = nil

	if missing_libraries then
		--[===[@debug@
		addon:Print("You are using a development version of Collectinator.  As per WowAce standards, externals are not set up.  You will have to install all necessary libraries in order for the addon to function correctly.")
		--@end-debug@]===]
		_G.Collectinator = nil
		return
	end

	-- Set default options, which are to include everything in the scan
	local defaults = {
		global = {
		},
		profile = {
			-------------------------------------------------------------------------------
			-- Frame options
			-------------------------------------------------------------------------------
			frameopts = {
				offsetx = 0,
				offsety = 0,
				anchorTo = "",
				anchorFrom = "",
				uiscale = 1,
				small_list_font = true,
			},

			-------------------------------------------------------------------------------
			-- Tooltip Options
			-------------------------------------------------------------------------------
			tooltip = {
				scale = 1,
				acquire_fontsize = 11,
			},
			-------------------------------------------------------------------------------
			-- Sorting Options
			-------------------------------------------------------------------------------
			sorting = "Ascending",
			current_tab = 2, -- Name tab

			-------------------------------------------------------------------------------
			-- Display Options
			-------------------------------------------------------------------------------
			includefiltered = false,
			includeexcluded = false,
			closeguionskillclose = false,
			ignoreexclusionlist = false,
			scanbuttonlocation = "TR",
			spelltooltiplocation = "Right",
			acquiretooltiplocation = "Right",
			collectables_in_tooltips = true,
			max_collectables_in_tooltips = 10,
			hide_tooltip_hint = false,
			hidepopup = false,
			minimap = true,
			worldmap = true,
			autoscanmap = false,
			scantrainers = false,
			scanvendors = false,
			autoloaddb = false,
			maptrainer = false,
			mapvendor = true,
			mapmob = true,
			mapquest = true,

			-------------------------------------------------------------------------------
			-- Text Dump Options
			-------------------------------------------------------------------------------
			textdumpformat = "BBCode",

			-------------------------------------------------------------------------------
			-- Collection Exclusion
			-------------------------------------------------------------------------------
			exclusionlist = {},

			-------------------------------------------------------------------------------
			-- Filter Options
			-------------------------------------------------------------------------------
			filters = {
				-------------------------------------------------------------------------------
				-- General Filters
				-------------------------------------------------------------------------------
				general = {
					faction = true,
					known = false,
					unknown = true,
					retired = false,
				},
				-------------------------------------------------------------------------------
				-- Obtain Filters
				-------------------------------------------------------------------------------
				obtain = {
					-- Acquisition
					achievement = true,
					coll_edition = true,
					mobdrop = true,
					profession = true,
					pvp = true,
					quest = true,
					raid = true,
					reputation = true,
					world_events = true,
					trainer = true,
					vendor = true,
					worlddrop = true,
					store = true,
					tcg = true,
					promo = true,
					misc = true,

					-- Version
					expansion0 = true,
					expansion1 = true,
					expansion2 = true,
					expansion3 = true,
					expansion4 = true,
				},
				-------------------------------------------------------------------------------
				-- Quality Filters
				-------------------------------------------------------------------------------
				quality = {
					common = true,
					uncommon = true,
					rare = true,
					epic = true,
					legendary = true,
				},
				-------------------------------------------------------------------------------
				-- Binding Filters
				-------------------------------------------------------------------------------
				binding = {
					itemboe = true,
					itembop = true,
					itemboa = true,
				},
				-------------------------------------------------------------------------------
				-- Reputation Filters
				-------------------------------------------------------------------------------
				rep = {
					aldor = true,
					scryer = true,
					argentdawn = true,
					ashtonguedeathsworn = true,
					cenarioncircle = true,
					cenarionexpedition = true,
					consortium = true,
					hellfire = true,
					keepersoftime = true,
					nagrand = true,
					lowercity = true,
					scaleofthesands = true,
					shatar = true,
					shatteredsun = true,
					sporeggar = true,
					thoriumbrotherhood = true,
					timbermaw = true,
					violeteye = true,
					zandalar = true,
					argentcrusade = true,
					frenzyheart = true,
					ebonblade = true,
					kirintor = true,
					sonsofhodir = true,
					kaluak = true,
					oracles = true,
					wyrmrest = true,
					wrathcommon1 = true,
					wrathcommon2 = true,
					wrathcommon3 = true,
					wrathcommon4 = true,
					wrathcommon5 = true,
					ashenverdict = true,
					catacommon1 = true,
					catacommon2 = true,
					guardiansofhyjal = true,
					ramkahen = true,
					earthenring = true,
					therazane = true,
					goldenlotus = true,
					cloudserpent = true,
					shadopan = true,
					anglers = true,
					augustcelestials = true,
					brewmasters = true,
					klaxxi = true,
					lorewalkers = true,
					tillers = true,
					blackprince = true,
					shangxiacademy = true,
					pandacommon1 = true,
					pandacommon2 = true,
					guild = true,
					netherwing = true,
					brawlers = true,
					pandacommon3 = true,
					shaohao = true,
				},
				-- Populated later via CONSTANTS
				item = {
				},
				misc = {
				},
			}
		}
	}

	for filter_name in pairs(private.COLLECTION_FILTER_TYPES) do
		defaults.profile.filters.item[filter_name:lower()] = true
	end
	self.db = LibStub("AceDB-3.0"):New("COLLECTINATORDB2", defaults)

	if not self.db then
		self:Print("Error: Database not loaded correctly.  Please exit out of WoW and delete the Collectinator database file Collectinator.lua) found in: \\World of Warcraft\\WTF\\Account\\<Account Name>>\\SavedVariables\\")
		return
	end
	local version = _G.GetAddOnMetadata("Collectinator", "Version")
	local debug_version = false
	local alpha_version = false

	--[===[@debug@
	debug_version = true
	--@end-debug@]===]

	--@alpha@
	alpha_version = true
	--@end-alpha@

	version = debug_version and "Devel" or (alpha_version and version .. "-Alpha") or version

	self.version = version

	-------------------------------------------------------------------------------
	-- Create the scan button.
	-------------------------------------------------------------------------------
	local scan_button_parent = _G.PetJournalParent
	local scan_button = _G.CreateFrame("Button", nil, scan_button_parent, "UIPanelButtonTemplate")
	scan_button:SetHeight(20)
	scan_button:SetText(L["Scan"])
	scan_button:SetWidth(scan_button:GetTextWidth() + 10)
	scan_button:RegisterForClicks("LeftButtonUp")
	scan_button:SetFrameLevel(scan_button_parent:GetFrameLevel() + 1)
	scan_button:SetFrameStrata(scan_button_parent:GetFrameStrata())
	scan_button:Enable()
	scan_button:SetPoint("RIGHT", _G.PetJournalParentCloseButton, "LEFT", 4, 0)

	scan_button:SetScript("OnClick", function()
		local shift_pressed = _G.IsShiftKeyDown()
		local alt_pressed = _G.IsAltKeyDown()
		local control_pressed = _G.IsControlKeyDown()

		-- Alt-Shift (Warcraft Pets)
		if shift_pressed and alt_pressed and not control_pressed then
			--addon:Scan(true, false)
		-- Shift only (Text Dump)
		elseif shift_pressed and not alt_pressed and not control_pressed then
			--addon:Scan(true, false)
		-- Alt only (Wipe icons from map)
		elseif not shift_pressed and alt_pressed and not control_pressed then
			--addon:ClearMap()
		-- If we are just clicking do the scan
		elseif not shift_pressed and not alt_pressed and not control_pressed then
			addon:Scan(false, false)
			--self:SetupMap()
		end
	end)

	scan_button:SetScript("OnEnter", function(this)
		local tooltip = _G.GameTooltip
		_G.GameTooltip_SetDefaultAnchor(tooltip, this)
		tooltip:SetText(L["SCAN_COMPANIONS_DESC"])
		tooltip:Show()
	end)

	scan_button:SetScript("OnLeave", function() _G.GameTooltip:Hide() end)

	scan_button:Show()
	self.scan_button = scan_button

	-------------------------------------------------------------------------------
	-- Add mini-pet/mount totals to the tab
	-------------------------------------------------------------------------------
	_G.PetJournalParentTab1:SetScript("OnEnter", function(this)
		local tooltip = _G.GameTooltip

		_G.GameTooltip_SetDefaultAnchor(tooltip, this)
		tooltip:SetText(string.format("%d %s.", _G.GetNumCompanions("MOUNT"), _G.MOUNTS))
		tooltip:Show()
	end)

	_G.PetJournalParentTab1:SetScript("OnLeave", function()
		_G.GameTooltip:Hide()
	end)

	_G.PetJournalParentTab2:SetScript("OnEnter", function(this)
		local tooltip = _G.GameTooltip

		_G.GameTooltip_SetDefaultAnchor(tooltip, this)
		tooltip:SetText(string.format("%d %s.", LPJ:NumPets(), _G.PETS))
		tooltip:Show()
	end)

	_G.PetJournalParentTab2:SetScript("OnLeave", function()
		_G.GameTooltip:Hide()
	end)

	self:SetupOptions()

	-- Register slash commands
	self:RegisterChatCommand("col", "ChatCommand")
	self:RegisterChatCommand("collectinator", "ChatCommand")

	-------------------------------------------------------------------------------
	-- Populate the collection initialization functions.
	-------------------------------------------------------------------------------
	COLLECTION_INIT_FUNCS = {
		CRITTER = addon.InitCritters,
		MOUNT = addon.InitMounts,
	}

	-------------------------------------------------------------------------------
	-- Hook GameTooltip so we can show information on mobs that drop/sell/train
	-------------------------------------------------------------------------------
--Tooltip shit can come later
--[[
	_G.GameTooltip:HookScript("OnTooltipSetUnit", function(self)
		if not addon.db.profile.collections_in_tooltips then
			return
		end
		local name, tooltip_unit = self:GetUnit()

		if not tooltip_unit or not _G.UnitGUID(tooltip_unit) then
			return
		end
		local id_num = private.MobGUIDToIDNum(_G.UnitGUID(tooltip_unit))
		local unit = private.mob_list[id_num] or private.vendor_list[id_num] or private.trainer_list[id_num]

		if not unit or not unit.item_list then
			return
		end
		local player = private.Player
		local count = 0

		for spell_id in pairs(unit.item_list) do
			local collection = private.recipe_list[spell_id]
			local recipe_prof = _G.GetSpellInfo(recipe.profession)

			if player.scanned_professions[recipe_prof] then
				local skill_level = player.professions[recipe_prof]

				if (_G.IsShiftKeyDown() or (not recipe:HasState("KNOWN"))) and player:HasCollectableFaction(recipe) then
					local _, _, _, hex = _G.GetItemQualityColor(recipe.quality)

					self:AddLine(("%s: |c%s%s|r (%d)"):format(recipe.profession, hex, recipe.name, recipe.skill_level))
					count = count + 1
				end
			end

			if count >= addon.db.profile.max_collections_in_tooltips then
				break
			end
		end
	end)
]]--
end

--- Function run when the addon is enabled.  Registers events and pre-loads certain variables.
function addon:OnEnable()
	private.Player:Initialize()
end

function addon:OnDisable()
	if addon.Frame then
		addon.Frame:Hide()
	end
end

-------------------------------------------------------------------------------
-- Event handling functions
-------------------------------------------------------------------------------



-------------------------------------------------------------------------------
-- Logic Functions
-------------------------------------------------------------------------------
local SUBCOMMAND_FUNCS = {
	debug = function()
		if not debugger then
			CreateDebugFrame()
		end

		if debugger:Lines() == 0 then
			debugger:AddLine("Nothing to report.")
			debugger:Display()
			debugger:Clear()
			return
		end
		debugger:Display()
	end,
	--[===[@debug@
	dump = function(arg1, arg2)
		local func = private.DUMP_COMMANDS[arg1]

		if func then
			func(arg2)
		else
			addon:Print("Unknown dump command:")

			for command in pairs(private.DUMP_COMMANDS) do
				addon:Print(command)
			end
		end
	end,
	--@end-debug@]===]
}

function addon:ChatCommand(input)
	local arg1, arg2, arg3 = self:GetArgs(input, 3)

	if arg1 then
		arg1 = arg1:trim():lower()
	end

	-- Open About panel if there's no parameters or if we do /col about
	if not arg1 or (arg1 and arg1:trim() == "") then
		_G.InterfaceOptionsFrame_OpenToCategory(self.optionsFrame)
	else
		local func = SUBCOMMAND_FUNCS[arg1]
		if func then
			func(arg2, arg3)
		else

			LibStub("AceConfigCmd-3.0"):HandleCommand("col", "Collectinator", arg1)
		end
	end
end

do

	local function InitializeLookups()
		addon:InitCustom()
		addon:InitMob()
		addon:InitQuest()
		addon:InitReputation()
		addon:InitTrainer()
		addon:InitWorldEvents()
		addon:InitVendor()

		InitializeLookups = nil
	end

	function addon:InitializeCollection(collectable_type)
		if not collectable_type then
			addon:Debug("nil collection passed to InitializeCollection()")
			return
		end

		if InitializeLookups then
			InitializeLookups()
		end

		local func = COLLECTION_INIT_FUNCS[collectable_type]

		if func then
			func(addon)
			COLLECTION_INIT_FUNCS[collectable_type] = nil
		end
	end
end
-------------------------------------------------------------------------------
-- Collection Scanning Functions
-------------------------------------------------------------------------------
do
	local UNKNOWN_PET_FORMAT = "%s: %d"

	local COLLECTABLE_SCAN_FUNCS = {
		[private.COLLECTION_TYPE_IDS.MOUNT] = function(collectable_type, mounts)
			local num_mounts = _G.GetNumCompanions(collectable_type)

			local mount_names = {}
			local mount_ids = {}
			local mount_sources = {}

			for spell_id, mount in pairs(mounts) do
				local mount_name, _, icon = _G.GetSpellInfo(spell_id)
				mount:SetName(mount_name)
				mount:SetIcon(icon)
			end

			for index = 1, num_mounts do
				local mount_id = select(3, _G.GetCompanionInfo("MOUNT", index))
				local mount = mounts[mount_id]

				if mount then
					mount:AddState("KNOWN")
				elseif not mount_names[mount_id] then
					mount_names[mount_id] = _G.GetSpellInfo(mount_id) or _G.UNKNOWN
					mount_ids[#mount_ids + 1] = mount_id
				end
			end
			table.sort(mount_ids)

			for index = 1, #mount_ids do
				local mount_id = mount_ids[index]
				private.TextDump:AddLine(("-- %s -- %d"):format(mount_names[mount_id], mount_id))
				private.TextDump:AddLine(("mount = AddMount(%d, V.MOP, Q.COMMON)\n"):format(mount_id))
			end
			local dump_lines = private.TextDump:Lines()

			if dump_lines > 0 then
				private.TextDump:InsertLine(1, ("Untracked: %d\n"):format(dump_lines / 2))
				private.TextDump:Display()
			end
		end,
		[private.COLLECTION_TYPE_IDS.CRITTER] = function(collectable_type, critters)
		-- We're doing nothing here for the moment since all of this is handled when the PetJournal is updated.
		--			for pet_id, pet in pairs(critters) do
		--			end
		end,
	}

	-- Causes a scan of the relevant collectable type to be conducted. Function called when the scan button is clicked.
	-- Parses Collections and displays output
	function addon:Scan(textdump, is_refresh)
		local current_panel = _G.PanelTemplates_GetSelectedTab(_G.PetJournalParent)
		local collectable_type = private.ORDERED_COLLECTIONS[current_panel]
		addon:InitializeCollection(collectable_type)

		local collectables = private.collectable_list[collectable_type]

		if not collectables then
			addon:Debug("Error, %s table not made.", collectable_type)
			return
		end

		for id, collectable in pairs(collectables) do
			collectable:RemoveState("RELEVANT")
			collectable:RemoveState("VISIBLE")
		end
		local search_box = _G.PetJournalSearchBox
		search_box:ClearFocus()
		search_box:SetText(_G.SEARCH)
		_G.PetJournal_OnSearchTextChanged(search_box)

		COLLECTABLE_SCAN_FUNCS[current_panel](collectable_type, collectables)
		private.Player:UpdateReputations()

		-------------------------------------------------------------------------------
		-- Everything is ready - display the GUI or dump the list to text.
		-------------------------------------------------------------------------------
		if textdump then
			--self:DisplayTextDump(profession_recipes, current_panel)
		else
			if private.InitializeFrame then
				private.InitializeFrame()
			end
			self.Frame:Display(private.ORDERED_COLLECTIONS[current_panel])
		end
	end
end
