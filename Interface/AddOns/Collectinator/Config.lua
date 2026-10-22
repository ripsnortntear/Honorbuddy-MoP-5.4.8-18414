--[[
************************************************************************
Config.lua
Ace3 Configuration options for Collectinator
************************************************************************
File date: 2013-01-06T15:00:19Z
File hash: 3ff23fd
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/collectinator/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]]--

-------------------------------------------------------------------------------
-- Upvalued Lua API
-------------------------------------------------------------------------------
local _G = getfenv(0)

-- Functions
local pairs = _G.pairs
local type = _G.type
local tonumber = _G.tonumber

-- Libraries
local table = _G.table

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private	= ...

local LibStub = _G.LibStub

local addon		= LibStub("AceAddon-3.0"):GetAddon(private.addon_name)

local L			= LibStub("AceLocale-3.0"):GetLocale(private.addon_name)

local AceConfig 	= LibStub("AceConfig-3.0")
local AceConfigReg 	= LibStub("AceConfigRegistry-3.0")
local AceConfigDialog 	= LibStub("AceConfigDialog-3.0")

-------------------------------------------------------------------------------
-- Constants
-------------------------------------------------------------------------------
local modularOptions = {}

local function giveProfiles()
	return LibStub("AceDBOptions-3.0"):GetOptionsTable(addon.db)
end

local function ResetGUI()
	local options = addon.db.profile.frameopts
	local MainPanel = addon.Frame

	options.offsetx = 0
	options.offsety = 0
	options.anchorTo = ""
	options.anchorFrom = ""
	options.uiscale = 1
	options.small_list_font = true

	if MainPanel then
		MainPanel:Hide()
		MainPanel:Show()
	end
end

local options

local function fullOptions()
	if not options then
		options = {
			type = "group",
			name = private.addon_name,
			args = {
				general = {
					order = 1,
					type = "group",
					name = L["Main Options"],
					desc = L["MAIN_OPTIONS_DESC"],
					args = {
						version = {
							order = 11,
							type = "description",
							name = _G.GAME_VERSION_LABEL .. ": " .. addon.version .. "\n",
						},
						spacer1 = {
							order = 12,
							type = "description",
							name = "\n",
						},
						header1 = {
							order = 15,
							type = "header",
							name = L["Main Filter Options"],
						},
						mainfilter_desc = {
							order = 20,
							type = "description",
							name = L["MAINFILTER_OPTIONS_DESC"] .. "\n",
						},
						includefiltered = {
							order = 25,
							type = "toggle",
							name = L["Include Filtered"],
							desc = L["FILTERCOUNT_DESC"],
							get = function()
								return addon.db.profile.includefiltered
							end,
							set = function()
								addon.db.profile.includefiltered = not addon.db.profile.includefiltered

								if addon.Frame and addon.Frame:IsVisible() then
									addon:Scan(false, false)
								end
							end,
						},
						includeexcluded = {
							order = 30,
							type = "toggle",
							name = L["Include Excluded"],
							desc = L["EXCLUDECOUNT_DESC"],
							get = function()
								return addon.db.profile.includeexcluded
							end,
							set = function()
								addon.db.profile.includeexcluded = not addon.db.profile.includeexcluded

								if addon.Frame and addon.Frame:IsVisible() then
									addon:Scan(false, false)
								end
							end,
						},
						exclusionlist = {
							order = 35,
							type = "execute",
							name = L["View Exclusion List"],
							desc = L["VIEW_EXCLUSION_LIST_DESC"],
							func = function(info)
								local exclusion_list = addon.db.profile.exclusionlist
								for type in pairs(exclusion_list) do
									print(type)
									for id in pairs(exclusion_list[type]) do
										print(id)
									end
								end
--[[
								for spell_id in pairs(exclusion_list) do
									local spell_name = _G.GetSpellInfo(spell_id)

									if spell_name then
										addon:Printf("%d: %s", spell_id, spell_name)
									else
										addon:Printf("%d: **spell not found** Removing from exclusion list.", spell_id)
										exclusion_list[spell_id] = nil
									end
								end
--]]
							end,
							disabled = function(info)
								for spell_id in pairs(addon.db.profile.exclusionlist) do
									return false
								end
								return true
							end,
						},
						clearexclusionlist = {
							order = 40,
							type = "execute",
							name = L["Clear Exclusion List"],
							desc = L["CLEAR_EXCLUSION_LIST_DESC"],
							func = function(info)
								local exclusion_list = addon.db.profile.exclusionlist

								exclusion_list = table.wipe(exclusion_list)

								if addon.Frame:IsVisible() then
									addon:Scan()
								end
							end,
							disabled = function(info)
								for spell_id in pairs(addon.db.profile.exclusionlist) do
									return false
								end
								return true
							end,
						},
						spacer1 = {
							order = 45,
							type = "description",
							name = "\n",
						},
						header2 = {
							order = 51,
							type = "header",
							name = L["Text Dump Options"],
						},
--[[
						text_dump_desc = {
							order = 52,
							type = "description",
							name = L["TEXTDUMP_OPTIONS_DESC"] .. "\n",
						},
						textdump = {
							order = 53,
							type = "select",
							name = L["Text Dump"],
							desc = L["TEXT_DUMP_DESC"],
							get = function()
								return addon.db.profile.textdumpformat
							end,
							set = function(info, name)
								addon.db.profile.textdumpformat = name
							end,
							values = function()
								return {
									Name = _G.NAME,
									Comma = L["CSV"],
									BBCode = L["BBCode"],
									XML = L["XML"],
								}
							end,
						},
]]--
					},
				},
			},
		}

		for k, v in pairs(modularOptions) do
			options.args[k] = (type(v) == "function") and v() or v
		end
	end
	return options
end
--[[
local collectinator_map

local function GetMapOptions()
	local has_waypoints = _G.TomTom

	if not collectinator_map then
		collectinator_map = {
			order	= 1,
			type	= "group",
			name	= L["Waypoints"],
			args	= {
				map_desc =	{
					order	= 1,
					type	= "description",
					name	= L["MAP_OPTIONS_DESC"] .. "\n",
				},
				mapvendor = {
					order	= 3,
					type	= "toggle",
					name	= _G.BATTLE_PET_SOURCE_3,
					desc	= L["WAYPOINT_TOGGLE_FORMAT"]:format(_G.BATTLE_PET_SOURCE_3),
					disabled = not has_waypoints,
					get	= function()
							  return addon.db.profile.mapvendor
						  end,
					set	= function()
							  addon.db.profile.mapvendor = not addon.db.profile.mapvendor
						  end,
				},
				mapmob = {
					order	= 4,
					type	= "toggle",
					name	= L["Mob Drop"],
					desc	= L["WAYPOINT_TOGGLE_FORMAT"]:format(L["Mob Drop"]),
					disabled = not has_waypoints,
					get	= function()
							  return addon.db.profile.mapmob
						  end,
					set	= function()
							  addon.db.profile.mapmob = not addon.db.profile.mapmob
						  end,
				},
				mapquest = {
					order	= 5,
					type	= "toggle",
					name	= L["Quest"],
					desc	= L["WAYPOINT_TOGGLE_FORMAT"]:format(L["Quest"]),
					disabled = not has_waypoints,
					get	= function()
							  return addon.db.profile.mapquest
						  end,
					set	= function()
							  addon.db.profile.mapquest = not addon.db.profile.mapquest
						  end,
				},
				map_acquire_header = {
					order	= 10,
					type	= "header",
					name	= "",
				},
				autoscanmap = {
					order	= 11,
					type	= "toggle",
					width	= "full",
					name	= L["Auto Scan Map"],
					desc	= L["AUTOSCANMAP_DESC"],
					disabled = not has_waypoints,
					get	= function()
							  return addon.db.profile.autoscanmap
						  end,
					set	= function()
							  addon.db.profile.autoscanmap = not addon.db.profile.autoscanmap
						  end,
				},
				worldmap = {
					order	= 12,
					type	= "toggle",
					width	= "full",
					name	= _G.WORLD_MAP,
					desc	= L["WAYPOINT_MAP_FORMAT"]:format(_G.WORLD_MAP),
					disabled = not has_waypoints,
					get	= function()
							  return addon.db.profile.worldmap
						  end,
					set	= function()
							  addon.db.profile.worldmap = not addon.db.profile.worldmap
						  end,
				},
				minimap = {
					order	= 13,
					type	= "toggle",
					width	= "full",
					name	= _G.MINIMAP_LABEL,
					desc	= L["WAYPOINT_MAP_FORMAT"]:format(_G.MINIMAP_LABEL),
					disabled = not has_waypoints,
					get	= function()
							  return addon.db.profile.minimap
						  end,
					set	= function()
							  addon.db.profile.minimap = not addon.db.profile.minimap
						  end,
				},
				clearmap = {
					order	= 20,
					type	= "execute",
					name	= L["Clear Waypoints"],
					disabled = not has_waypoints,
					desc	= L["CLEAR_WAYPOINTS_DESC"],
					func	= function()
							  addon:ClearWaypoints()
						  end,
				},
			},
		}
	end

	return collectinator_map
end

]]--

local displayoptions

local SCAN_ANCHORS = {
	TR = L["Top Right"],
	TL = L["Top Left"],
	BR = L["Bottom Right"],
	BL = L["Bottom Left"]
}

local function GetDisplayOptions()
	if not displayoptions then
		displayoptions = {
			order		= 1,
			name		= _G.DISPLAY_OPTIONS,
			type		= "group",
			childGroups	= "tab",
			args = {
				-------------------------------------------------------------------------------
				-- Main interface options.
				-------------------------------------------------------------------------------
				interface_tab = {
					order	= 10,
					name	= _G.INTERFACE_OPTIONS,
					type	= "group",
					args	= {
						display_desc = {
							order	= 1,
							name	= L["DISPLAY_OPTIONS_DESC"] .. "\n",
							type	= "description",
						},
						scanbuttonlocation = {
							order	= 2,
							type	= "select",
							name	= L["Scan Button Position"],
							desc	= L["SCANBUTTONPOSITION_DESC"],
							get	= function()
									  return addon.db.profile.scanbuttonlocation
								  end,
							set	= function(info, name)
									  addon.db.profile.scanbuttonlocation = name
								  end,
							values	= function()
									  return SCAN_ANCHORS
								  end,
						},
						uiscale = {
							order	= 3,
							type	= "range",
							name	= _G.UI_SCALE,
							desc	= L["UI_SCALE_DESC"],
							min	= .5,
							max	= 1.5,
							step	= .05,
							bigStep = .05,
							get	= function()
									  return addon.db.profile.frameopts.uiscale
								  end,
							set	= function(info, v)
									  addon.db.profile.frameopts.uiscale = v

									  if private.InitializeFrame then
										  private.InitializeFrame()
									  end
									  addon.Frame:SetScale(v)
								  end,
						},
						small_list_font = {
							order	= 4,
							width	= "full",
							type	= "toggle",
							name	= L["Small Font"],
							desc	= L["SMALL_FONT_DESC"],
							get	= function()
									  return addon.db.profile.frameopts.small_list_font
								  end,
							set	= function(info, value)
									  addon.db.profile.frameopts.small_list_font = value

									  if private.InitializeFrame then
										  private.InitializeFrame()
									  end

									  if addon.Frame:IsVisible() then
										  addon:Scan()
									  end
								  end,
						},
						closegui = {
							width	= "full",
							order	= 5,
							type	= "toggle",
							name	= L["Close GUI"],
							desc	= L["CLOSEGUI_DESC"],
							get	= function()
									  return addon.db.profile.closeguionskillclose
								  end,
							set	= function(info, value)
									  addon.db.profile.closeguionskillclose = value
								  end,
						},
						hidepopup = {
							width	= "full",
							order	= 6,
							type	= "toggle",
							name	= L["Hide Pop-Up"],
							desc	= L["HIDEPOPUP_DESC"],
							get	= function()
									  return addon.db.profile.hidepopup
								  end,
							set	= function(info, value)
									  addon.db.profile.hidepopup = value
								  end,
						},
						resetguiwindow = {
							width	= "double",
							order	= 7,
							type	= "execute",
							name	= L["Reset Window Position"],
							desc	= L["RESET_WINDOW_DESC"],
							func	= ResetGUI,
						},
					},
				},
				-------------------------------------------------------------------------------
				-- Tooltip options
				-------------------------------------------------------------------------------
				tooltip_tab = {
					order	= 20,
					name	= L["Tooltip Options"],
					type	= "group",
					args	= {
						tooltip_desc =	{
							order	= 12,
							type	= "description",
							name	= L["TOOLTIP_OPTIONS_DESC"] .. "\n",
						},
						tooltip_fontsize = {
							order	= 19,
							type	= "range",
							name	= _G.FONT_SIZE,
							desc	= L["FONT_SIZE_DESC"],
							min	= 6,
							max	= 20,
							step	= 1,
							bigStep = 1,
							get	= function()
									  return addon.db.profile.tooltip.acquire_fontsize
								  end,
							set	= function(info, v)
									  addon.db.profile.tooltip.acquire_fontsize = v
								  end,
						},
						tooltipscale = {
							order	= 20,
							type	= "range",
							name	= L["Tooltip Scale"],
							desc	= L["TOOLTIP_SCALE_DESC"],
							min	= .5,
							max	= 1.5,
							step	= .05,
							bigStep = .05,
							get	= function()
									  return addon.db.profile.tooltip.scale
								  end,
							set	= function(info, v)
									  addon.db.profile.tooltip.scale = v
								  end,
						},
						acquiretooltiplocation = {
							order	= 21,
							type	= "select",
							name	= L["Tooltip (Acquire) Position"],
							desc	= L["ACQUIRETOOLTIPPOSITION_DESC"],
							get	= function()
									  return addon.db.profile.acquiretooltiplocation
								  end,
							set	= function(info, name)
									  addon.db.profile.acquiretooltiplocation = name
								  end,
							values	= function()
									  return {
										  Right = L["Right"],
										  Left = L["Left"],
										  Top = L["Top"],
										  Bottom = L["Bottom"],
										  Off = _G.OFF,
										  Mouse = _G.MOUSE_LABEL
									  }
								  end,
						},
						spelltooltiplocation = {
							order	= 22,
							type	= "select",
							name	= L["Tooltip Position"],
							desc	= L["SPELLTOOLTIPPOSITION_DESC"],
							get	= function()
									  return addon.db.profile.spelltooltiplocation
								  end,
							set	= function(info, name)
									  addon.db.profile.spelltooltiplocation = name
								  end,
							values	= function()
									  return {
										  Right = L["Right"],
										  Left = L["Left"],
										  Top = L["Top"],
										  Bottom = L["Bottom"],
										  Off = _G.OFF
									  }
								  end,
						},
						spacer1 = {
							order	= 23,
							type	= "description",
							name	= "\n",
						},
						unit_tooltip = {
							order	= 24,
							type	= "toggle",
							name	= L["Collectables In Tooltips"],
							desc	= L["UNIT_TOOLTIPS_DESC"],
							get	= function()
									  return addon.db.profile.collectables_in_tooltips
								  end,
							set	= function(info, value)
									  addon.db.profile.collectables_in_tooltips = value
								  end,
						},
						unit_max_tooltip = {
							order	= 25,
							type	= "range",
							name	= _G.MAXIMUM,
							desc	= L["UNIT_MAX_TOOLTIPS_DESC"],
							min	= 1,
							max	= 50,
							step	= 1,
							bigStep = 5,
							disabled= function()
									  return not addon.db.profile.collectables_in_tooltips
								  end,
							get	= function()
									  return addon.db.profile.max_collectables_in_tooltips
								  end,
							set	= function(info, v)
									  addon.db.profile.max_collectables_in_tooltips = v
								  end,
						},
						tooltip_hint = {
							order	= 26,
							type	= "toggle",
							name	= L["TOOLTIP_HINT"],
							desc	= L["TOOLTIP_HINT_DESC"],
							get	= function()
									  return addon.db.profile.hide_tooltip_hint
								  end,
							set	= function(info, value)
									  addon.db.profile.hide_tooltip_hint = value
								  end,
						},
					},
				},
			},
		}
	end
	return displayoptions
end

function addon:SetupOptions()
	AceConfigReg:RegisterOptionsTable(private.addon_name, fullOptions)
	self.optionsFrame = AceConfigDialog:AddToBlizOptions(private.addon_name, nil, nil, "general")

	-- Register the module options
	self:RegisterModuleOptions("Display", GetDisplayOptions(), _G.DISPLAY_OPTIONS)
--[[
	if _G.TomTom then
		self:RegisterModuleOptions("Waypoint", GetMapOptions(), L["Waypoints"])
	end
]]--
	self:RegisterModuleOptions("Profiles", giveProfiles(), L["Profile Options"])

	-- Add in the about panel to the Bliz options (not a part of the ace3 config)
	if LibStub:GetLibrary("LibAboutPanel", true) then
		self.optionsFrame["About"] = LibStub:GetLibrary("LibAboutPanel").new(private.addon_name, private.addon_name)
	else
		self:Print("Lib AboutPanel not loaded.")
	end
end

-- Description: Function which extends our options table in a modular way
-- Expected result: add a new modular options table to the modularOptions upvalue as well as the Blizzard config
-- Input:
--		name			: index of the options table in our main options table
--		optionsTable	: the sub-table to insert
--		displayName	: the name to display in the config interface for this set of options
-- Output: None.

function addon:RegisterModuleOptions(name, optionsTable, displayName)
	modularOptions[name] = optionsTable
	self.optionsFrame[name] = AceConfigDialog:AddToBlizOptions(private.addon_name, displayName, private.addon_name, name)
end
