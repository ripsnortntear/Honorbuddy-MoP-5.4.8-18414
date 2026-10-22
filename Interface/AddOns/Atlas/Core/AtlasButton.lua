-- $Id: AtlasButton.lua 2099 2013-09-12 14:33:28Z arithmandar $
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


local AL = LibStub("AceLocale-3.0"):GetLocale("Atlas");

-- Minimap button with LibDBIcon-1.0
local addon = LibStub("AceAddon-3.0"):NewAddon("Atlas", "AceConsole-3.0")
local AtlasMiniMapLDB = LibStub("LibDataBroker-1.1"):NewDataObject("Atlas", {
	type = "launcher",
	text = AL["ATLAS_TITLE"],
	icon = "Interface\\WorldMap\\WorldMap-Icon",
	OnClick = function(self, button)
		if button == "LeftButton" then
			Atlas_Toggle();
		elseif button == "RightButton" then
			AtlasOptions_Toggle();
		end
	end,
	OnTooltipShow = function(tooltip)
		if not tooltip or not tooltip.AddLine then return end
		tooltip:AddLine("|cffffffff"..ATLAS_TITLE)
		tooltip:AddLine(ATLAS_MINIMAPLDB_HINT)
	end,
})


local AtlasMiniMapIcon = LibStub("LibDBIcon-1.0")

function addon:OnInitialize()
	-- Obviously you'll need a ##SavedVariables: BunniesDB line in your TOC, duh!
	self.db = LibStub("AceDB-3.0"):New("AtlasDB", {
		profile = {
			minimap = {
				hide = false,
			},
		},
	})
	AtlasMiniMapIcon:Register("Atlas", AtlasMiniMapLDB, self.db.profile.minimap);
	self:RegisterChatCommand("atlasbutton", AtlasButton_Toggle)
	self:RegisterChatCommand("atlas", Atlas_SlashCommand)
end

function addon:Toggle()
	self.db.profile.minimap.hide = not self.db.profile.minimap.hide
	if self.db.profile.minimap.hide then
		AtlasMiniMapIcon:Hide("Atlas")
		AtlasOptions.AtlasButtonShown = false;
	else
		AtlasMiniMapIcon:Show("Atlas")
		AtlasOptions.AtlasButtonShown = true;
	end
	AtlasOptions_Init();
end

function AtlasButton_Toggle()
	addon:Toggle()
end

--[[
function AtlasButton_OnClick()
	Atlas_Toggle();
end

function AtlasButton_Init()
	if (not AtlasOptions) then
		Atlas_FreshOptions();
	end
	if(AtlasOptions.AtlasButtonShown) then
		AtlasButtonFrame:Show();
	else
		AtlasButtonFrame:Hide();
	end
end

function AtlasButton_Toggle()
	if(AtlasButtonFrame:IsVisible()) then
		AtlasButtonFrame:Hide();
		AtlasOptions.AtlasButtonShown = false;
	else
		AtlasButtonFrame:Show();
		AtlasOptions.AtlasButtonShown = true;
	end
	AtlasOptions_Init();
end

function AtlasButton_UpdatePosition()
	AtlasButtonFrame:SetPoint("TOPLEFT", "Minimap", "TOPLEFT", 54 - (AtlasOptions.AtlasButtonRadius * cos(AtlasOptions.AtlasButtonPosition)), (AtlasOptions.AtlasButtonRadius * sin(AtlasOptions.AtlasButtonPosition)) - 55);
	AtlasOptions_Init();
end

-- Thanks to Yatlas for this code
function AtlasButton_BeingDragged()
    -- Thanks to Gello for this code
    local xpos,ypos = GetCursorPosition() 
    local xmin,ymin = Minimap:GetLeft(), Minimap:GetBottom() 

    xpos = xmin-xpos/UIParent:GetScale()+70 
    ypos = ypos/UIParent:GetScale()-ymin-70 

    AtlasButton_SetPosition(math.deg(math.atan2(ypos,xpos)));
end

function AtlasButton_SetPosition(v)
    if (v < 0) then
        v = v + 360;
    end

    AtlasOptions.AtlasButtonPosition = v;
    AtlasButton_UpdatePosition();
end

function AtlasButton_OnEnter(self)
	GameTooltip:SetOwner(self, "ANCHOR_LEFT");
	GameTooltip:SetText(ATLAS_BUTTON_TOOLTIP_TITLE);
	GameTooltipTextLeft1:SetTextColor(1, 1, 1);
	GameTooltip:AddLine(ATLAS_BUTTON_TOOLTIP_HINT);
	GameTooltip:Show();
end
]]
