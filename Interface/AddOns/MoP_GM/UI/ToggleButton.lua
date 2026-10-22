-- MoP_GM/UI/ToggleButton.lua
-- Minimap-anchored launcher button. Click → toggle MainFrame.
-- SHIFT-drag → reposition; position persisted in MoP_GM_DB.button.

local SIZE = 32

-- Single toggle path used by both the launcher button and the slash command.
function MoP_GM.Toggle()
    if not MoP_GM_MainFrame then
        MoP_GM.Print("|cffff5555main frame not created — addon may have failed to load. Run /mopgm debug.|r")
        return
    end
    if MoP_GM_MainFrame:IsShown() then
        MoP_GM_MainFrame:Hide()
    else
        MoP_GM_MainFrame:Show()
        MoP_GM_MainFrame:Raise()
    end
end

local function createToggleButton()
    local b = CreateFrame("Button", "MoP_GM_ToggleButton", UIParent)
    b:SetSize(SIZE, SIZE)
    b:SetFrameStrata("MEDIUM")
    b:SetFrameLevel(8)
    b:SetMovable(true)
    b:SetClampedToScreen(true)
    b:EnableMouse(true)
    b:RegisterForClicks("LeftButtonUp", "RightButtonUp")
    b:RegisterForDrag("LeftButton")

    -- Icon, masked to a circle so only the gear shows (no rectangular black
    -- corners poking out from under the ring border).
    local icon = b:CreateTexture(nil, "BACKGROUND")
    icon:SetTexture("Interface\\Icons\\INV_Misc_Gear_03")
    icon:SetSize(20, 20)
    icon:SetPoint("CENTER", b, "CENTER", 0, 0)
    icon:SetTexCoord(0.08, 0.92, 0.08, 0.92)
    if icon.SetMask then
        icon:SetMask("Interface\\CharacterFrame\\TempPortraitAlphaMask")
    end

    -- Decorative border ring on top of the icon.
    local border = b:CreateTexture(nil, "OVERLAY")
    border:SetTexture("Interface\\Minimap\\MiniMap-TrackingBorder")
    border:SetSize(54, 54)
    border:SetPoint("CENTER", b, "CENTER", 11, -11)

    b:SetHighlightTexture("Interface\\Minimap\\UI-Minimap-ZoomButton-Highlight", "ADD")

    -- Drag pattern: OnDragStart only fires after the cursor moves a few px,
    -- so a quick click never enters drag mode. Require SHIFT to actually drag.
    b:SetScript("OnDragStart", function(self)
        if IsShiftKeyDown() then
            self:StartMoving()
            self.isMoving = true
        end
    end)
    b:SetScript("OnDragStop", function(self)
        if self.isMoving then
            self:StopMovingOrSizing()
            self.isMoving = false
            MoP_GM.SaveFramePoint(self, "button")
        end
    end)

    -- Click: just toggle. OnDragStart already handled drag if cursor moved.
    b:SetScript("OnClick", function(self, button)
        MoP_GM.Toggle()
    end)

    b:SetScript("OnEnter", function(self)
        GameTooltip:SetOwner(self, "ANCHOR_LEFT")
        GameTooltip:SetText("MoP_GM", 0.2, 1, 0.6)
        GameTooltip:AddLine("Click to toggle the GM panel.", 1, 1, 1)
        GameTooltip:AddLine("SHIFT-drag to reposition this button.", 0.7, 0.7, 0.7)
        GameTooltip:AddLine("/mopgm reset to recenter everything.", 0.5, 0.5, 0.5)
        GameTooltip:Show()
    end)
    b:SetScript("OnLeave", function() GameTooltip:Hide() end)

    b:Show()
    return b
end

function MoP_GM.RestoreButtonPosition()
    local b = MoP_GM_ToggleButton
    if not b then return end
    MoP_GM.RestoreFramePoint(b, "button", MoP_GM.defaults.button)
end

MoP_GM.AddLogin(function()
    if not MoP_GM_ToggleButton then createToggleButton() end
    MoP_GM.RestoreButtonPosition()
end)
