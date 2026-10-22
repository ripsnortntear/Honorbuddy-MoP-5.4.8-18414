-------------------------------------------------------------------------------
-- Localized Lua globals.
-------------------------------------------------------------------------------
local _G = getfenv(0)

local pairs = _G.pairs

-------------------------------------------------------------------------------
-- AddOn namespace.
-------------------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub
local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)

function private:InitializeItemFilters_Pet(parent_panel)
	local MainPanel = addon.Frame

	local item_toggle = _G.CreateFrame("Button", nil, parent_panel)
	item_toggle:SetWidth(105)
	item_toggle:SetHeight(20)
	item_toggle:SetNormalFontObject("QuestTitleFont")
	item_toggle:SetHighlightFontObject("QuestTitleFontBlackShadow")
	item_toggle:SetText(_G.MISCELLANEOUS .. ":")
	item_toggle:SetPoint("TOP", parent_panel, "TOP", 0, -7)
	item_toggle:RegisterForClicks("LeftButtonUp", "RightButtonUp")

	private.SetTooltipScripts(item_toggle, L["GROUP_TOGGLE_FORMAT"]:format(_G.MISCELLANEOUS))

	local item_types = {
		pet_capturable	= { tt = L["FILTER_DESC_FORMAT"]:format(_G.UNIT_CAPTURABLE),	text = _G.UNIT_CAPTURABLE,	row = 1, col = 1 },
	}

	item_toggle:SetScript("OnClick", function(self, button)
		local toggle = (button == "LeftButton") and true or false

		for item in pairs(item_types) do
			addon.db.profile.filters.item[item] = toggle
			parent_panel[item]:SetChecked(toggle)
		end
		MainPanel:UpdateTitle()
		MainPanel.list_frame:Update(nil, false)
	end)

	parent_panel.item_toggle = item_toggle

	local item_panel = _G.CreateFrame("Frame", nil, parent_panel)
	item_panel:SetHeight(100)
	item_panel:SetPoint("TOP", item_toggle, "BOTTOM")
	item_panel:SetPoint("LEFT", parent_panel, "LEFT")
	item_panel:SetPoint("RIGHT", parent_panel, "RIGHT")

	private.GenerateCheckBoxes(parent_panel, item_types, item_panel)

	for item_type in pairs(item_types) do
		MainPanel.filter_menu.value_map[item_type] = {
			cb = MainPanel.filter_menu.item.item_CRITTER[item_type],
			svroot = addon.db.profile.filters.item
		}
	end
	self.InitializeItemFilters_Pet = nil
end
