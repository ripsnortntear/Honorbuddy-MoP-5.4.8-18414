-- MoP_GM/UI/MainFrame.lua
-- Movable parent window with header, tab strip, and scrolling content.

local FRAME_W, FRAME_H = 900, 540
local HEADER_H = 26
local TAB_AREA_H = 32

MoP_GM.tabs = {}      -- array of { id, label, builder, contentFrame }

local function buildTabStrip(parent)
    local strip = CreateFrame("Frame", nil, parent)
    strip:SetPoint("TOPLEFT", parent, "TOPLEFT", 8, -HEADER_H - 4)
    strip:SetPoint("TOPRIGHT", parent, "TOPRIGHT", -8, -HEADER_H - 4)
    strip:SetHeight(TAB_AREA_H)
    return strip
end

local function selectTab(index)
    local tabs = MoP_GM.tabs
    if not tabs[index] then return end
    for i, tab in ipairs(tabs) do
        if tab.button then
            if i == index then
                tab.button:SetButtonState("PUSHED", true)
                local fs = tab.button:GetFontString()
                if fs then fs:SetTextColor(1, 1, 0.4) end
            else
                tab.button:SetButtonState("NORMAL", false)
                local fs = tab.button:GetFontString()
                if fs then fs:SetTextColor(1, 1, 1) end
            end
        end
        if tab.contentFrame then
            if i == index then tab.contentFrame:Show() else tab.contentFrame:Hide() end
        end
    end
    if tabs[index].onShow then tabs[index].onShow() end
    if MoP_GM.db then MoP_GM.db.activeTab = index end
end
MoP_GM.SelectTab = selectTab

function MoP_GM.RegisterTab(def)
    -- def = { id, label, builder = function(parent) end, onShow = function() end }
    table.insert(MoP_GM.tabs, def)
end

local function buildAllTabs()
    local main = MoP_GM_MainFrame
    local strip = main.tabStrip

    -- Width of each tab button = label width + padding. We compute it via a
    -- temporary FontString so different labels get appropriate widths.
    local sizer = main:CreateFontString(nil, "OVERLAY", "GameFontNormal")

    local prevBtn
    for i, tab in ipairs(MoP_GM.tabs) do
        local btn = CreateFrame("Button", "MoP_GM_MainFrameTab" .. i, main, "UIPanelButtonTemplate")
        btn:SetHeight(22)
        sizer:SetText(tab.label)
        local w = math.max(60, math.ceil(sizer:GetStringWidth()) + 18)
        btn:SetWidth(w)
        btn:SetText(tab.label)
        btn:SetID(i)
        if i == 1 then
            btn:SetPoint("TOPLEFT", strip, "TOPLEFT", 4, 0)
        else
            btn:SetPoint("LEFT", prevBtn, "RIGHT", 2, 0)
        end
        btn:SetScript("OnClick", function(self) selectTab(self:GetID()) end)
        tab.button = btn
        prevBtn = btn

        -- No backdrop on tab content frames — the main panel already provides
        -- the visual frame, and a tiled backdrop here was causing major FPS
        -- drops (~5 fps) during scrolling on the 5.4.8 client.
        local content = CreateFrame("Frame", nil, main)
        content:SetPoint("TOPLEFT", main, "TOPLEFT", 8, -HEADER_H - TAB_AREA_H - 8)
        content:SetPoint("BOTTOMRIGHT", main, "BOTTOMRIGHT", -8, 8)
        content:Hide()
        tab.contentFrame = content

        if tab.builder then
            local ok, err = pcall(tab.builder, content)
            if not ok then
                DEFAULT_CHAT_FRAME:AddMessage("|cffff5555MoP_GM tab '" .. tostring(tab.label) .. "' build error:|r " .. tostring(err))
            end
        end
    end
    sizer:Hide()

    selectTab((MoP_GM.db and MoP_GM.db.activeTab) or 1)
end

function MoP_GM.RestoreMainFramePosition()
    local f = MoP_GM_MainFrame
    if not f then return end
    MoP_GM.RestoreFramePoint(f, "frame", MoP_GM.defaults.frame)
end

local function createMainFrame()
    local f = CreateFrame("Frame", "MoP_GM_MainFrame", UIParent)
    f:SetSize(FRAME_W, FRAME_H)
    f:SetFrameStrata("HIGH")
    f:SetToplevel(true)
    f:SetMovable(true)
    f:SetClampedToScreen(true)
    f:EnableMouse(true)
    f:RegisterForDrag("LeftButton")
    f:SetScript("OnDragStart", function(self) self:StartMoving() end)
    f:SetScript("OnDragStop", function(self)
        self:StopMovingOrSizing()
        MoP_GM.SaveFramePoint(self, "frame")
    end)
    f:SetScript("OnShow", function() if MoP_GM.db and MoP_GM.db.frame then MoP_GM.db.frame.shown = true end end)
    f:SetScript("OnHide", function() if MoP_GM.db and MoP_GM.db.frame then MoP_GM.db.frame.shown = false end end)
    MoP_GM.ApplyBackdrop(f, "panel", 0.92)

    -- Header
    local header = CreateFrame("Frame", nil, f)
    header:SetPoint("TOPLEFT", f, "TOPLEFT", 4, -4)
    header:SetPoint("TOPRIGHT", f, "TOPRIGHT", -4, -4)
    header:SetHeight(HEADER_H)
    f.header = header

    local title = header:CreateFontString(nil, "OVERLAY", "GameFontNormalLarge")
    title:SetPoint("LEFT", header, "LEFT", 8, 0)
    title:SetText(MoP_GM.colors.accent .. "MoP_GM" .. MoP_GM.colors.reset .. "  |cffaaaaaav" .. MoP_GM.version .. "|r")

    local hint = header:CreateFontString(nil, "OVERLAY", "GameFontDisableSmall")
    hint:SetPoint("LEFT", title, "RIGHT", 16, 0)
    hint:SetText("Right-click any command to pin/unpin from Favorites")

    local close = CreateFrame("Button", nil, header, "UIPanelCloseButton")
    close:SetPoint("TOPRIGHT", header, "TOPRIGHT", 4, 4)
    close:SetScript("OnClick", function() f:Hide() end)

    -- Current-target readout. Many commands fall back to UnitName("target")
    -- when their name field is blank, so showing it here makes those buttons
    -- predictable.
    local targetFS = header:CreateFontString(nil, "OVERLAY", "GameFontNormal")
    targetFS:SetPoint("RIGHT", close, "LEFT", -10, 0)
    local function updateTarget()
        local n = UnitName("target")
        if n and n ~= "" then
            targetFS:SetText("Target: " .. MoP_GM.colors.accent .. n .. MoP_GM.colors.reset)
        else
            targetFS:SetText("|cff888888No target|r")
        end
    end
    f:RegisterEvent("PLAYER_TARGET_CHANGED")
    f:SetScript("OnEvent", updateTarget)
    f:HookScript("OnShow", updateTarget)
    updateTarget()

    -- Tab strip placeholder; tabs registered by tab files, built in OnLogin.
    f.tabStrip = buildTabStrip(f)

    f:Hide()
    return f
end

-- Register a login handler that finishes wiring up the main frame after all
-- tab files have called RegisterTab.
MoP_GM.AddLogin(function()
    if not MoP_GM_MainFrame then createMainFrame() end
    MoP_GM.RestoreMainFramePosition()
    buildAllTabs()
    if MoP_GM.db and MoP_GM.db.frame and MoP_GM.db.frame.shown then
        MoP_GM_MainFrame:Show()
    end
end)

-- Create the frame shell early so tabs can refer to it during their builders.
-- Wrapped in pcall so a load-time error surfaces in chat instead of silently
-- killing the file (and every tab that depends on RegisterTab below).
local ok, err = pcall(createMainFrame)
if not ok then
    DEFAULT_CHAT_FRAME:AddMessage("|cffff0000MoP_GM MainFrame ERROR:|r " .. tostring(err))
    MoP_GM._mainFrameLoadError = tostring(err)
end
