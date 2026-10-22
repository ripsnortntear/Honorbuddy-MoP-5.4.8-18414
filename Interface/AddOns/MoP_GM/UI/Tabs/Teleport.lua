-- MoP_GM/UI/Tabs/Teleport.lua
-- Three sub-tabs:
--   1. Tele       — `.tele` family + appear / summon + lookup tele + save row
--   2. Go         — every `.go` form + lookup area / map / taxinode
--   3. Locations  — clickable, paginated grid of seeded + imported locations
--
-- Notes:
--  * `.tele del` is NOT a chat command on Emucoach 7.x — server-side tele names
--    are managed via the world DB. We track user-saved names locally and offer
--    an "untrack (local)" button that only removes them from the addon's list.
--  * NO scroll frames anywhere — they tank FPS to ~3 on 5.4.8. The Locations
--    grid pages instead (Prev/Next swaps which buttons are shown).
--  * "Import from server" scrapes `.lookup tele <term>`: the server replies with
--    `|Htele:<id>|h[<name>]|h` links in CHAT_MSG_SYSTEM, which we parse for the
--    exact stored names and add to the local list. This is how each install
--    pulls its OWN custom game_tele names without any DB access from the client.

local TELE_BTN_W = 200
local TELE_BTN_H = 22
local TELE_COLS  = 4
local PAGE_ROWS  = 13                      -- 4 × 13 = 52 buttons/page, fits no-scroll
local PAGE_SIZE  = TELE_COLS * PAGE_ROWS

-- ─── Tele sub-tab (commands + save-current-spot row) ──────────────────────
local function buildTelePanel(parent)
    -- The tele/appear/summon/lookup family at the top.
    MoP_GM.LayoutRows(parent, MoP_GM.Commands.TeleByName)

    -- "Save current spot as:" row anchored at the bottom of the panel.
    local saveRow = CreateFrame("Frame", nil, parent)
    saveRow:SetHeight(28)
    saveRow:SetPoint("BOTTOMLEFT", parent, "BOTTOMLEFT", 8, 6)
    saveRow:SetPoint("BOTTOMRIGHT", parent, "BOTTOMRIGHT", -8, 6)

    local label = saveRow:CreateFontString(nil, "OVERLAY", "GameFontHighlightSmall")
    label:SetPoint("LEFT", saveRow, "LEFT", 0, 0)
    label:SetText("Save current spot as:")

    local edit = CreateFrame("EditBox", nil, saveRow)
    edit:SetSize(160, 22)
    edit:SetPoint("LEFT", label, "RIGHT", 8, 0)
    edit:SetFontObject(GameFontHighlightSmall)
    edit:SetTextInsets(6, 6, 0, 0)
    edit:SetAutoFocus(false)
    edit:SetMaxLetters(64)
    local bg = edit:CreateTexture(nil, "BACKGROUND")
    bg:SetAllPoints(edit)
    bg:SetTexture(0, 0, 0, 0.55)

    local addBtn = MoP_GM.MakeFlatButton(saveRow, 90, 22, ".tele add", false)
    addBtn:SetPoint("LEFT", edit, "RIGHT", 8, 0)
    addBtn:SetScript("OnClick", function()
        local n = MoP_GM.Trim(edit:GetText() or "")
        if n == "" then MoP_GM.Print("enter a name first"); return end
        MoP_GM._ExecuteRaw(".tele add " .. n)
        MoP_GM.db.userTeleports = MoP_GM.db.userTeleports or {}
        table.insert(MoP_GM.db.userTeleports, { name = n, label = n })
        edit:SetText("")
        if MoP_GM.RefreshTeleLocations then MoP_GM.RefreshTeleLocations() end
    end)

    local delBtn = MoP_GM.MakeFlatButton(saveRow, 110, 22, "untrack (local)", false)
    delBtn:SetPoint("LEFT", addBtn, "RIGHT", 4, 0)
    delBtn:SetScript("OnClick", function()
        local n = MoP_GM.Trim(edit:GetText() or "")
        if n == "" then MoP_GM.Print("enter a name first"); return end
        local list = MoP_GM.db.userTeleports or {}
        local removed = 0
        for i = #list, 1, -1 do
            if list[i].name:lower() == n:lower() then
                table.remove(list, i); removed = removed + 1
            end
        end
        if removed > 0 then
            MoP_GM.Print("removed '" .. n .. "' from local list (still exists on server).")
            if MoP_GM.RefreshTeleLocations then MoP_GM.RefreshTeleLocations() end
        else
            MoP_GM.Print("'" .. n .. "' not found in local list.")
        end
    end)
end

local function buildGoPanel(parent)
    MoP_GM.LayoutRows(parent, MoP_GM.Commands.TeleGo)
end

-- ─── Locations sub-tab (paginated grid + server import) ───────────────────
local locGrid                 -- frame the location buttons are parented to
local locPager                -- { prev, next, label } pager widgets
local locPage = 1             -- current page (1-based)

-- "My Locations" sub-tab state (forward-declared; defined further down).
local myGrid
local myPager
local myPage = 1
local rebuildGrid             -- forward-declared; defined below
local rebuildMyGrid

-- Refresh BOTH the merged Locations grid and the My-Locations grid. Adds/deletes
-- of user teleports touch both, so callers use this single entry point.
function MoP_GM.RefreshTeleLocations()
    if locGrid then rebuildGrid() end
    if myGrid then rebuildMyGrid() end
end

-- Merge seed + user-saved teleport names, dedupe by lowercased name, sort.
local function mergedLocations()
    local merged, seen = {}, {}
    for _, t in ipairs(MoP_GM.SeedTeleports or {}) do
        local k = t.name:lower()
        if not seen[k] then table.insert(merged, t); seen[k] = true end
    end
    MoP_GM.db.userTeleports = MoP_GM.db.userTeleports or {}
    for _, t in ipairs(MoP_GM.db.userTeleports) do
        local k = t.name:lower()
        if not seen[k] then table.insert(merged, t); seen[k] = true end
    end
    table.sort(merged, function(a, b) return (a.label or a.name):lower() < (b.label or b.name):lower() end)
    return merged
end

rebuildGrid = function()
    if not locGrid then return end
    -- Clear previous page's buttons.
    for _, child in ipairs({ locGrid:GetChildren() }) do
        child:Hide()
        child:SetParent(nil)
    end

    local merged = mergedLocations()
    local total = #merged
    local pages = math.max(1, math.ceil(total / PAGE_SIZE))
    if locPage > pages then locPage = pages end
    if locPage < 1 then locPage = 1 end

    local startIdx = (locPage - 1) * PAGE_SIZE
    local x0, y0 = 8, -8
    for slot = 1, PAGE_SIZE do
        local t = merged[startIdx + slot]
        if not t then break end
        local col = (slot - 1) % TELE_COLS
        local row = math.floor((slot - 1) / TELE_COLS)
        local btn = MoP_GM.MakeFlatButton(locGrid, TELE_BTN_W, TELE_BTN_H, t.label or t.name, false)
        btn:SetPoint("TOPLEFT", locGrid, "TOPLEFT", x0 + col * (TELE_BTN_W + 4), y0 - row * (TELE_BTN_H + 4))
        local teleName = t.name
        btn:SetScript("OnClick", function() MoP_GM._ExecuteRaw(".tele " .. teleName) end)
    end

    -- Pager: only meaningful with more than one page.
    if locPager then
        if pages > 1 then
            locPager.label:SetText(("Page %d / %d  (%d locations)"):format(locPage, pages, total))
            locPager.prev:Show(); locPager.next:Show(); locPager.label:Show()
        else
            locPager.label:SetText(("%d locations"):format(total))
            locPager.prev:Hide(); locPager.next:Hide(); locPager.label:Show()
        end
    end
end

-- Import names from the server by scraping `.lookup tele <term>` output.
local importing = false
local function importFromServer(term, statusFS, btn)
    if importing then return end
    term = MoP_GM.Trim(term or "")
    if term == "" then
        statusFS:SetText("|cffff7733enter a name fragment to search for|r")
        return
    end
    importing = true
    btn.label:SetText("…")
    statusFS:SetText("searching '" .. term .. "'…")

    -- Names already known locally (seeds + user), so we only count NEW ones.
    local known = {}
    for _, t in ipairs(MoP_GM.SeedTeleports or {}) do known[t.name:lower()] = true end
    MoP_GM.db.userTeleports = MoP_GM.db.userTeleports or {}
    for _, t in ipairs(MoP_GM.db.userTeleports) do known[t.name:lower()] = true end

    local found = {}        -- lowercased name -> exact stored name (new only)
    local capHit = false

    -- Temporarily hide the lookup spam from the chat window. Message filters
    -- only affect what the chat frame DISPLAYS — our event listener below still
    -- receives every CHAT_MSG_SYSTEM, so capture is unaffected.
    local function chatFilter(_, _, msg)
        if msg:find("|Htele:", 1, true) then return true end
        if msg == "Locations found are:" then return true end
        return false
    end
    ChatFrame_AddMessageEventFilter("CHAT_MSG_SYSTEM", chatFilter)

    local listener = CreateFrame("Frame")
    listener:RegisterEvent("CHAT_MSG_SYSTEM")
    listener:SetScript("OnEvent", function(_, _, msg)
        for nm in msg:gmatch("|Htele:%d+|h%[(.-)%]|h") do
            local k = nm:lower()
            if not known[k] and not found[k] then found[k] = nm end
        end
        -- Server result cap (LANG_COMMAND_LOOKUP_MAX_RESULTS) — enUS wording.
        if msg:find("limit of", 1, true) or msg:find("refine your search", 1, true) then
            capHit = true
        end
    end)

    SendChatMessage(".lookup tele " .. term, "SAY")

    -- 5.4.8 has no C_Timer; poll OnUpdate for the response window, then commit.
    local elapsed = 0
    listener:SetScript("OnUpdate", function(self, dt)
        elapsed = elapsed + dt
        if elapsed < 1.5 then return end
        self:SetScript("OnUpdate", nil)
        self:UnregisterAllEvents()
        ChatFrame_RemoveMessageEventFilter("CHAT_MSG_SYSTEM", chatFilter)

        local added = 0
        for _, nm in pairs(found) do
            table.insert(MoP_GM.db.userTeleports, { name = nm, label = nm })
            added = added + 1
        end

        local note = capHit and " |cffff7733(server capped results — narrow the search)|r" or ""
        if added > 0 then
            statusFS:SetText(("|cff66dd66added %d new|r%s"):format(added, note))
            MoP_GM.Print(("imported %d new teleport location(s) matching '%s'."):format(added, term))
            MoP_GM.RefreshTeleLocations()
        else
            statusFS:SetText("no new matches" .. note)
        end
        btn.label:SetText("Import")
        importing = false
    end)
end

local function buildLocationsPanel(parent)
    -- Import row at the top.
    local importRow = CreateFrame("Frame", nil, parent)
    importRow:SetHeight(24)
    importRow:SetPoint("TOPLEFT", parent, "TOPLEFT", 8, -6)
    importRow:SetPoint("TOPRIGHT", parent, "TOPRIGHT", -8, -6)

    local ilabel = importRow:CreateFontString(nil, "OVERLAY", "GameFontHighlightSmall")
    ilabel:SetPoint("LEFT", importRow, "LEFT", 0, 0)
    ilabel:SetText("Import from server:")

    local iedit = CreateFrame("EditBox", nil, importRow)
    iedit:SetSize(150, 22)
    iedit:SetPoint("LEFT", ilabel, "RIGHT", 8, 0)
    iedit:SetFontObject(GameFontHighlightSmall)
    iedit:SetTextInsets(6, 6, 0, 0)
    iedit:SetAutoFocus(false)
    iedit:SetMaxLetters(64)
    local iebg = iedit:CreateTexture(nil, "BACKGROUND")
    iebg:SetAllPoints(iedit)
    iebg:SetTexture(0, 0, 0, 0.55)
    local ihint = iedit:CreateFontString(nil, "ARTWORK", "GameFontDisableSmall")
    ihint:SetPoint("LEFT", iedit, "LEFT", 6, 0)
    ihint:SetText("name fragment, e.g. isle")
    local function refreshHint()
        if iedit:GetText() == "" and not iedit:HasFocus() then ihint:Show() else ihint:Hide() end
    end
    iedit:HookScript("OnTextChanged", refreshHint)
    iedit:HookScript("OnEditFocusGained", refreshHint)
    iedit:HookScript("OnEditFocusLost", refreshHint)

    local statusFS = importRow:CreateFontString(nil, "OVERLAY", "GameFontDisableSmall")

    local importBtn = MoP_GM.MakeFlatButton(importRow, 70, 22, "Import", false)
    importBtn:SetPoint("LEFT", iedit, "RIGHT", 8, 0)
    importBtn:SetScript("OnClick", function() importFromServer(iedit:GetText(), statusFS, importBtn) end)
    iedit:SetScript("OnEnterPressed", function(self) self:ClearFocus(); importFromServer(self:GetText(), statusFS, importBtn) end)
    iedit:SetScript("OnEscapePressed", function(self) self:ClearFocus() end)

    statusFS:SetPoint("LEFT", importBtn, "RIGHT", 10, 0)
    statusFS:SetText("scrapes .lookup tele to add this server's own names")

    -- Pager row anchored at the bottom.
    local pagerRow = CreateFrame("Frame", nil, parent)
    pagerRow:SetHeight(22)
    pagerRow:SetPoint("BOTTOMLEFT", parent, "BOTTOMLEFT", 8, 4)
    pagerRow:SetPoint("BOTTOMRIGHT", parent, "BOTTOMRIGHT", -8, 4)

    local prevBtn = MoP_GM.MakeFlatButton(pagerRow, 70, 20, "< Prev", false)
    prevBtn:SetPoint("LEFT", pagerRow, "LEFT", 0, 0)
    prevBtn:SetScript("OnClick", function() locPage = locPage - 1; rebuildGrid() end)

    local nextBtn = MoP_GM.MakeFlatButton(pagerRow, 70, 20, "Next >", false)
    nextBtn:SetPoint("LEFT", prevBtn, "RIGHT", 6, 0)
    nextBtn:SetScript("OnClick", function() locPage = locPage + 1; rebuildGrid() end)

    local pageLabel = pagerRow:CreateFontString(nil, "OVERLAY", "GameFontHighlightSmall")
    pageLabel:SetPoint("LEFT", nextBtn, "RIGHT", 12, 0)
    locPager = { prev = prevBtn, next = nextBtn, label = pageLabel }

    -- Grid area between the import row and the pager.
    locGrid = CreateFrame("Frame", nil, parent)
    locGrid:SetPoint("TOPLEFT", importRow, "BOTTOMLEFT", 0, -6)
    locGrid:SetPoint("BOTTOMRIGHT", pagerRow, "TOPRIGHT", 0, 4)

    locPage = 1
    rebuildGrid()
end

-- ─── My Locations sub-tab (only user-added TPs, with per-entry delete) ─────
local MY_COLS  = 2
local MY_ROWS  = 15                        -- 2 × 15 = 30 entries/page, fits no-scroll
local MY_PAGE  = MY_COLS * MY_ROWS
local MY_BTN_W = 340

-- Remove every locally-tracked teleport whose name matches (case-insensitive).
-- This is local-only — `.tele del` doesn't exist on this core, so the name
-- still lives in the server's game_tele; we just stop listing it in the addon.
local function deleteUserTele(name)
    local list = MoP_GM.db.userTeleports or {}
    local removed = 0
    for i = #list, 1, -1 do
        if list[i].name:lower() == name:lower() then
            table.remove(list, i); removed = removed + 1
        end
    end
    if removed > 0 then
        MoP_GM.Print(("removed '%s' from My Locations (still exists on the server)."):format(name))
        MoP_GM.RefreshTeleLocations()
    end
end

rebuildMyGrid = function()
    if not myGrid then return end
    for _, child in ipairs({ myGrid:GetChildren() }) do
        child:Hide()
        child:SetParent(nil)
    end

    local list = {}
    for _, t in ipairs(MoP_GM.db.userTeleports or {}) do table.insert(list, t) end
    table.sort(list, function(a, b) return (a.label or a.name):lower() < (b.label or b.name):lower() end)
    local total = #list

    if total == 0 then
        if not myGrid.emptyFS then
            local fs = myGrid:CreateFontString(nil, "OVERLAY", "GameFontHighlight")
            fs:SetPoint("TOPLEFT", myGrid, "TOPLEFT", 8, -8)
            fs:SetWidth(700)
            fs:SetJustifyH("LEFT")
            fs:SetText("No locations added yet.\n\nAdd them on the Tele sub-tab (\"Save current spot as\") or on the Locations sub-tab (\"Import from server\"). They'll appear here for quick access and deletion.")
            myGrid.emptyFS = fs
        end
        myGrid.emptyFS:Show()
        if myPager then
            myPager.label:SetText("0 of your locations")
            myPager.prev:Hide(); myPager.next:Hide(); myPager.label:Show()
        end
        return
    end
    if myGrid.emptyFS then myGrid.emptyFS:Hide() end

    local pages = math.max(1, math.ceil(total / MY_PAGE))
    if myPage > pages then myPage = pages end
    if myPage < 1 then myPage = 1 end

    local startIdx = (myPage - 1) * MY_PAGE
    local cellW = MY_BTN_W + 4 + 24 + 12
    for slot = 1, MY_PAGE do
        local t = list[startIdx + slot]
        if not t then break end
        local col = (slot - 1) % MY_COLS
        local row = math.floor((slot - 1) / MY_COLS)
        local cx = 8 + col * cellW
        local cy = -8 - row * (TELE_BTN_H + 4)

        local teleName = t.name
        local btn = MoP_GM.MakeFlatButton(myGrid, MY_BTN_W, TELE_BTN_H, t.label or t.name, false)
        btn:SetPoint("TOPLEFT", myGrid, "TOPLEFT", cx, cy)
        btn:SetScript("OnClick", function() MoP_GM._ExecuteRaw(".tele " .. teleName) end)

        local del = MoP_GM.MakeFlatButton(myGrid, 24, TELE_BTN_H, "X", true)
        del:SetPoint("LEFT", btn, "RIGHT", 4, 0)
        del:SetScript("OnClick", function() deleteUserTele(teleName) end)
    end

    if myPager then
        if pages > 1 then
            myPager.label:SetText(("Page %d / %d  (%d of your locations)"):format(myPage, pages, total))
            myPager.prev:Show(); myPager.next:Show(); myPager.label:Show()
        else
            myPager.label:SetText(("%d of your locations"):format(total))
            myPager.prev:Hide(); myPager.next:Hide(); myPager.label:Show()
        end
    end
end

local function buildMyLocationsPanel(parent)
    local hint = parent:CreateFontString(nil, "OVERLAY", "GameFontDisableSmall")
    hint:SetPoint("TOPLEFT", parent, "TOPLEFT", 8, -6)
    hint:SetText("Locations you added via this addon. Click to teleport, X to remove (local list only).")

    -- Pager row at the bottom.
    local pagerRow = CreateFrame("Frame", nil, parent)
    pagerRow:SetHeight(22)
    pagerRow:SetPoint("BOTTOMLEFT", parent, "BOTTOMLEFT", 8, 4)
    pagerRow:SetPoint("BOTTOMRIGHT", parent, "BOTTOMRIGHT", -8, 4)

    local prevBtn = MoP_GM.MakeFlatButton(pagerRow, 70, 20, "< Prev", false)
    prevBtn:SetPoint("LEFT", pagerRow, "LEFT", 0, 0)
    prevBtn:SetScript("OnClick", function() myPage = myPage - 1; rebuildMyGrid() end)

    local nextBtn = MoP_GM.MakeFlatButton(pagerRow, 70, 20, "Next >", false)
    nextBtn:SetPoint("LEFT", prevBtn, "RIGHT", 6, 0)
    nextBtn:SetScript("OnClick", function() myPage = myPage + 1; rebuildMyGrid() end)

    local pageLabel = pagerRow:CreateFontString(nil, "OVERLAY", "GameFontHighlightSmall")
    pageLabel:SetPoint("LEFT", nextBtn, "RIGHT", 12, 0)
    myPager = { prev = prevBtn, next = nextBtn, label = pageLabel }

    myGrid = CreateFrame("Frame", nil, parent)
    myGrid:SetPoint("TOPLEFT", hint, "BOTTOMLEFT", 0, -6)
    myGrid:SetPoint("BOTTOMRIGHT", pagerRow, "TOPRIGHT", 0, 4)

    myPage = 1
    rebuildMyGrid()
end

MoP_GM.RegisterTab({
    id = "teleport", label = "Teleport",
    builder = function(parent)
        MoP_GM.BuildSubTabs(parent, {
            { label = "Tele",          builder = buildTelePanel        },
            { label = "Go",            builder = buildGoPanel          },
            { label = "Locations",     builder = buildLocationsPanel   },
            { label = "My Locations",  builder = buildMyLocationsPanel },
        }, "subTab_teleport")
    end,
})
