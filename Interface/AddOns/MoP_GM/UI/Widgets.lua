-- MoP_GM/UI/Widgets.lua
-- Reusable UI factories used by every tab.

local ROW_HEIGHT   = 24
local ROW_SPACING  = 4
local LABEL_WIDTH  = 150
local INPUT_WIDTH  = 100

-- ─── Backdrops ─────────────────────────────────────────────────────────────
-- tile=false (stretched, single sample) renders much faster on 5.4.8 than the
-- tile=true variant we used previously, which was sampling the bgFile in 16-px
-- chunks across the whole panel and dropped FPS during scroll.
MoP_GM.backdrops = {
    panel = {
        bgFile   = "Interface\\Tooltips\\UI-Tooltip-Background",
        edgeFile = "Interface\\Tooltips\\UI-Tooltip-Border",
        tile = false, edgeSize = 16,
        insets = { left = 4, right = 4, top = 4, bottom = 4 },
    },
}

function MoP_GM.ApplyBackdrop(frame, kind, bgAlpha)
    frame:SetBackdrop(MoP_GM.backdrops[kind or "panel"])
    frame:SetBackdropColor(0, 0, 0, bgAlpha or 0.85)
    frame:SetBackdropBorderColor(0.6, 0.6, 0.7, 1)
end

-- ─── Section header ────────────────────────────────────────────────────────
function MoP_GM.CreateSectionHeader(parent, text)
    local header = parent:CreateFontString(nil, "OVERLAY", "GameFontNormalLarge")
    header:SetText(MoP_GM.colors.label .. text .. MoP_GM.colors.reset)
    return header
end


-- ─── Lightweight widget factories ─────────────────────────────────────────
-- The Blizzard templates UIPanelButtonTemplate and InputBoxTemplate each carry
-- 5–9 background/edge/highlight textures, so a tab with ~30 rows × 3 widgets
-- ends up redrawing several hundred textures every frame during scroll. That
-- was the real FPS culprit. These factories use a single solid-color bg
-- texture per widget and nothing else.

function MoP_GM.MakeFlatButton(parent, w, h, text, isDanger)
    local b = CreateFrame("Button", nil, parent)
    b:SetSize(w, h)

    local bg = b:CreateTexture(nil, "BACKGROUND")
    bg:SetAllPoints(b)
    bg:SetTexture(0.18, 0.18, 0.22, 0.9)

    local hl = b:CreateTexture(nil, "HIGHLIGHT")
    hl:SetAllPoints(b)
    hl:SetTexture(1, 1, 1, 0.18)

    local fs = b:CreateFontString(nil, "OVERLAY", "GameFontNormalSmall")
    fs:SetPoint("CENTER", b, "CENTER", 0, 0)
    fs:SetText(text)
    if isDanger then fs:SetTextColor(1, 0.45, 0.45) end
    b:SetFontString(fs)
    b.label = fs
    return b
end
local makeFlatButton = MoP_GM.MakeFlatButton

local function makeFlatEditBox(parent, w, h, placeholder, isNumeric)
    local e = CreateFrame("EditBox", nil, parent)
    e:SetSize(w, h)
    e:SetFontObject(GameFontHighlightSmall)
    e:SetTextColor(1, 1, 1, 1)
    e:SetTextInsets(6, 6, 0, 0)
    e:SetAutoFocus(false)
    e:SetMaxLetters(isNumeric and 12 or 64)

    local bg = e:CreateTexture(nil, "BACKGROUND")
    bg:SetAllPoints(e)
    bg:SetTexture(0, 0, 0, 0.55)

    if placeholder then
        local hint = e:CreateFontString(nil, "ARTWORK", "GameFontDisableSmall")
        hint:SetPoint("LEFT", e, "LEFT", 6, 0)
        hint:SetText(placeholder)
        e.hint = hint
        local function refreshHint()
            if e:GetText() == "" and not e:HasFocus() then hint:Show() else hint:Hide() end
        end
        e.refreshHint = refreshHint
        e:HookScript("OnTextChanged", refreshHint)
        e:HookScript("OnEditFocusGained", refreshHint)
        e:HookScript("OnEditFocusLost", refreshHint)
    end
    return e
end

-- ─── Single command row ────────────────────────────────────────────────────
-- def = { id, label, format, args, danger, group, tooltip }
-- Returns the row frame; height = ROW_HEIGHT.
function MoP_GM.CreateCommandRow(parent, def)
    local row = CreateFrame("Frame", nil, parent)
    row:SetHeight(ROW_HEIGHT)
    row.def = def

    local rowKey = (def.group or "?") .. ":" .. (def.id or def.label or "?")

    -- Label-action button (left-click runs, right-click toggles favorite).
    local btn = makeFlatButton(row, LABEL_WIDTH, ROW_HEIGHT - 2, def.label or def.id, def.danger)
    btn:SetPoint("LEFT", row, "LEFT", 0, 0)
    btn:RegisterForClicks("LeftButtonUp", "RightButtonUp")
    row.button = btn

    -- Forward declarations so the input boxes can call execute() on Enter.
    local execute, gatherValues

    -- Build the input boxes for each arg.
    row.edits = {}
    local prev = btn
    for i, arg in ipairs(def.args or {}) do
        local edit = makeFlatEditBox(row, INPUT_WIDTH, ROW_HEIGHT - 2, arg.placeholder, arg.numeric)
        edit:SetPoint("LEFT", prev, "RIGHT", 8, 0)
        edit:HookScript("OnTextChanged", function(self)
            MoP_GM.SetInputCache(rowKey, arg.key, self:GetText())
        end)
        edit:SetScript("OnEnterPressed", function(self)
            self:ClearFocus()
            execute()
        end)
        edit:SetScript("OnEscapePressed", function(self) self:ClearFocus() end)
        local cached = MoP_GM.GetInputCache(rowKey, arg.key)
        if cached and cached ~= "" then edit:SetText(cached) end
        if edit.refreshHint then edit.refreshHint() end
        edit.argDef = arg
        row.edits[arg.key] = edit
        prev = edit
    end

    gatherValues = function()
        local v = {}
        for _, arg in ipairs(def.args or {}) do
            local edit = row.edits[arg.key]
            v[arg.key] = edit and edit:GetText() or nil
        end
        return v
    end

    execute = function()
        local line, err = MoP_GM.BuildLine(def, gatherValues())
        if not line then
            MoP_GM.Print(MoP_GM.colors.warn .. (err or "invalid args") .. MoP_GM.colors.reset)
            return
        end
        MoP_GM.RunCommand(line, { danger = def.danger })
    end

    -- Left-click → run; Shift+left → drop into chat box to edit; right-click → Favorite.
    btn:SetScript("OnClick", function(_, mouseButton)
        if mouseButton == "RightButton" then
            MoP_GM.ToggleFavorite(def)
        elseif IsShiftKeyDown() then
            local vals = gatherValues()
            local line = MoP_GM.BuildLine(def, vals)
            if not line then
                -- Best-effort: fill in what's typed, leave <placeholders> for the rest.
                line = def.format
                for _, arg in ipairs(def.args or {}) do
                    local v = vals[arg.key]
                    local sub = (v and v ~= "" and v) or ("<" .. (arg.placeholder or arg.key) .. ">")
                    line = line:gsub("%%s", sub, 1)
                end
            end
            ChatFrame_OpenChat(line)
        else
            execute()
        end
    end)

    -- Hover tooltip: command preview + help + interaction hints.
    btn:SetScript("OnEnter", function(self)
        GameTooltip:SetOwner(self, "ANCHOR_RIGHT")
        GameTooltip:SetText(def.label or def.id or "", 0.2, 1, 0.6)
        local preview = def.format or ""
        for _, arg in ipairs(def.args or {}) do
            preview = preview:gsub("%%s", "<" .. (arg.placeholder or arg.key) .. ">", 1)
        end
        GameTooltip:AddLine(preview, 0.6, 0.85, 1, true)
        if def.tooltip then GameTooltip:AddLine(def.tooltip, 1, 1, 1, true) end
        if def.danger then GameTooltip:AddLine("Asks for confirmation before sending.", 1, 0.45, 0.45, true) end
        GameTooltip:AddLine("Left-click run  ·  Shift-click edit in chat  ·  Right-click pin", 0.55, 0.55, 0.55, true)
        GameTooltip:Show()
    end)
    btn:SetScript("OnLeave", function() GameTooltip:Hide() end)

    return row
end

-- ─── Layout helper: stack rows vertically inside a parent ─────────────────
-- Returns total height used, so callers can size their scroll-content.
function MoP_GM.LayoutRows(parent, defs, opts)
    opts = opts or {}
    local x = opts.x or 8
    local yTop = opts.yTop or 8
    local sectionTitle = opts.sectionTitle
    local rowsPerColumn = opts.rowsPerColumn -- if set, lay out in columns
    local columnWidth = opts.columnWidth or 800

    local y = -yTop
    if sectionTitle then
        local hdr = MoP_GM.CreateSectionHeader(parent, sectionTitle)
        hdr:SetPoint("TOPLEFT", parent, "TOPLEFT", x, y)
        y = y - hdr:GetHeight() - 6
    end

    local startY = y
    local col = 0
    local rowCount = 0
    for _, def in ipairs(defs) do
        local row = MoP_GM.CreateCommandRow(parent, def)
        row:SetPoint("TOPLEFT", parent, "TOPLEFT", x + col * columnWidth, y)
        row:SetWidth(columnWidth - 16)
        y = y - (ROW_HEIGHT + ROW_SPACING)
        rowCount = rowCount + 1
        if rowsPerColumn and rowCount % rowsPerColumn == 0 then
            col = col + 1
            y = startY
        end
    end

    return -y + 8
end

-- ─── Sub-tabs ─────────────────────────────────────────────────────────────
-- Build a horizontal sub-tab strip with one content frame per sub-tab. Only
-- the active sub-tab's content is shown, so the total widget count visible
-- stays small and scrolling becomes unnecessary.
--
-- subTabsDef = { { label, rows, builder, layoutOpts }, ... }
-- Each sub-tab provides EITHER `rows` (a list of command defs to lay out) OR
-- `builder` (a function(parent) that builds custom content).
function MoP_GM.BuildSubTabs(parent, subTabsDef, dbKey)
    local strip = CreateFrame("Frame", nil, parent)
    strip:SetPoint("TOPLEFT", parent, "TOPLEFT", 4, -4)
    strip:SetPoint("TOPRIGHT", parent, "TOPRIGHT", -4, -4)
    strip:SetHeight(20)

    local subContent = CreateFrame("Frame", nil, parent)
    subContent:SetPoint("TOPLEFT", strip, "BOTTOMLEFT", 0, -4)
    subContent:SetPoint("BOTTOMRIGHT", parent, "BOTTOMRIGHT", -4, 4)

    local entries = {}
    local function selectSub(idx)
        for i, e in ipairs(entries) do
            if i == idx then
                e.button.label:SetTextColor(1, 1, 0.3)
                e.contentFrame:Show()
            else
                e.button.label:SetTextColor(0.85, 0.85, 0.85)
                e.contentFrame:Hide()
            end
        end
        if dbKey and MoP_GM.db then MoP_GM.db[dbKey] = idx end
    end

    local sizer = strip:CreateFontString(nil, "OVERLAY", "GameFontNormalSmall")
    local prev
    for i, def in ipairs(subTabsDef) do
        sizer:SetText(def.label)
        local w = math.max(50, math.ceil(sizer:GetStringWidth()) + 14)
        local btn = makeFlatButton(strip, w, 20, def.label, false)
        if prev then
            btn:SetPoint("LEFT", prev, "RIGHT", 4, 0)
        else
            btn:SetPoint("LEFT", strip, "LEFT", 0, 0)
        end
        prev = btn

        local content = CreateFrame("Frame", nil, subContent)
        content:SetAllPoints(subContent)
        content:Hide()

        if def.rows then
            MoP_GM.LayoutRows(content, def.rows, def.layoutOpts or {})
        end
        if def.builder then
            local ok, err = pcall(def.builder, content)
            if not ok then
                DEFAULT_CHAT_FRAME:AddMessage("|cffff5555MoP_GM sub-tab '" .. tostring(def.label) .. "' build error:|r " .. tostring(err))
            end
        end

        btn:SetScript("OnClick", function() selectSub(i) end)
        table.insert(entries, { button = btn, contentFrame = content })
    end
    sizer:Hide()

    local saved = dbKey and MoP_GM.db and MoP_GM.db[dbKey] or 1
    if not entries[saved] then saved = 1 end
    selectSub(saved)
end

-- ─── Generic scroll content holder ────────────────────────────────────────
function MoP_GM.CreateScrollContent(parent)
    local scroll = CreateFrame("ScrollFrame", nil, parent, "UIPanelScrollFrameTemplate")
    scroll:SetPoint("TOPLEFT", parent, "TOPLEFT", 4, -4)
    scroll:SetPoint("BOTTOMRIGHT", parent, "BOTTOMRIGHT", -28, 4)

    -- UIPanelScrollFrameTemplate ships with a scrollbar but no mouse-wheel
    -- handling; wire it up so users can scroll the rows naturally.
    scroll:EnableMouseWheel(true)
    scroll:SetScript("OnMouseWheel", function(self, delta)
        local current = self:GetVerticalScroll()
        local maxScroll = self:GetVerticalScrollRange()
        local step = ROW_HEIGHT * 2
        local target = current - delta * step
        if target < 0 then target = 0 end
        if target > maxScroll then target = maxScroll end
        self:SetVerticalScroll(target)
    end)

    local content = CreateFrame("Frame", nil, scroll)
    content:SetSize(820, 400)
    scroll:SetScrollChild(content)
    scroll.content = content
    return scroll, content
end
