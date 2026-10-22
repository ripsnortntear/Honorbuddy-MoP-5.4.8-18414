-- MoP_GM/Core/Init.lua
-- Addon namespace, defaults, ADDON_LOADED, slash commands.

local addonName = ...
MoP_GM = MoP_GM or {}
MoP_GM.name = "MoP_GM"
MoP_GM.version = "1.2.0"

MoP_GM.defaults = {
    frame = { point = "CENTER", relPoint = "CENTER", x = 0, y = 0, shown = false },
    button = { point = "TOPRIGHT", relPoint = "TOPRIGHT", x = -32, y = -32 },
    favorites = {},
    history = {},
    inputs = {},
    activeTab = 1,
    activeBotsSubTab = 1,
}

local function copyDefaults(src, dst)
    for k, v in pairs(src) do
        if type(v) == "table" then
            if type(dst[k]) ~= "table" then dst[k] = {} end
            copyDefaults(v, dst[k])
        elseif dst[k] == nil then
            dst[k] = v
        end
    end
    return dst
end

-- Login-handler registry. Each module appends a function via MoP_GM.AddLogin();
-- on PLAYER_LOGIN we run each in sequence, wrapped in pcall so a runtime
-- failure in one (e.g. buildAllTabs) doesn't break the others (e.g. the
-- toggle button).
MoP_GM._loginHandlers = {}
function MoP_GM.AddLogin(fn) table.insert(MoP_GM._loginHandlers, fn) end

local frame = CreateFrame("Frame")
frame:RegisterEvent("ADDON_LOADED")
frame:RegisterEvent("PLAYER_LOGIN")
frame:SetScript("OnEvent", function(self, event, name)
    if event == "ADDON_LOADED" and name == addonName then
        MoP_GM_DB = MoP_GM_DB or {}
        copyDefaults(MoP_GM.defaults, MoP_GM_DB)
        MoP_GM.db = MoP_GM_DB
    elseif event == "PLAYER_LOGIN" then
        for i, fn in ipairs(MoP_GM._loginHandlers or {}) do
            local ok, err = pcall(fn)
            if not ok then
                DEFAULT_CHAT_FRAME:AddMessage("|cffff5555MoP_GM login handler #" .. i .. " error:|r " .. tostring(err))
            end
        end
    end
end)

-- Slash commands
SLASH_MOPGM1 = "/mopgm"
SLASH_MOPGM2 = "/gm"
SlashCmdList["MOPGM"] = function(msg)
    msg = (msg or ""):gsub("^%s+", ""):gsub("%s+$", "")
    local lcmd = msg:lower()
    -- `probe` is special: preserve the original case of the rest of the line
    -- because some servers care about it (e.g. character names).
    if lcmd == "probe" or lcmd:sub(1, 6) == "probe " then
        if MoP_GM.Probe then MoP_GM.Probe(msg:sub(7)) end
        return
    end
    msg = lcmd
    if msg == "reset" then
        MoP_GM.db.frame = nil
        MoP_GM.db.button = nil
        copyDefaults(MoP_GM.defaults, MoP_GM.db)
        if MoP_GM.RestoreMainFramePosition then MoP_GM.RestoreMainFramePosition() end
        if MoP_GM.RestoreButtonPosition then MoP_GM.RestoreButtonPosition() end
        DEFAULT_CHAT_FRAME:AddMessage("|cff33ff99MoP_GM|r: positions reset.")
    elseif msg == "show" then
        if MoP_GM_MainFrame then MoP_GM_MainFrame:Show() end
    elseif msg == "hide" then
        if MoP_GM_MainFrame then MoP_GM_MainFrame:Hide() end
    elseif msg == "debug" then
        -- Quick state dump: each module exports a sentinel symbol; if any
        -- shows MISSING, that file failed to load (parse error or runtime).
        local function pp(label, val) DEFAULT_CHAT_FRAME:AddMessage("  " .. label .. ": " .. tostring(val)) end
        DEFAULT_CHAT_FRAME:AddMessage("|cff33ff99MoP_GM debug|r")
        pp("MainFrame", MoP_GM_MainFrame)
        pp("ToggleButton", MoP_GM_ToggleButton)
        pp("tabs registered", MoP_GM.tabs and #MoP_GM.tabs or 0)
        if MoP_GM._mainFrameLoadError then pp("MainFrame load error", MoP_GM._mainFrameLoadError) end
        DEFAULT_CHAT_FRAME:AddMessage("  modules:")
        for _, c in ipairs({
            { "Util",          MoP_GM.colors },
            { "SavedVars",     MoP_GM.PushHistory },
            { "CommandRunner", MoP_GM.RunCommand },
            { "Commands",      MoP_GM.Commands },
            { "Teleports",     MoP_GM.SeedTeleports },
            { "ConfirmDialog", StaticPopupDialogs and StaticPopupDialogs["MOPGM_CONFIRM_CMD"] },
            { "Widgets",       MoP_GM.ApplyBackdrop },
            { "MainFrame",     MoP_GM.RegisterTab },
            { "ToggleButton",  MoP_GM.Toggle },
            { "Probe",         MoP_GM.Probe },
        }) do
            pp("    " .. c[1], c[2] and "ok" or "|cffff5555MISSING|r")
        end
    else
        if MoP_GM.Toggle then MoP_GM.Toggle()
        elseif MoP_GM_MainFrame then
            if MoP_GM_MainFrame:IsShown() then MoP_GM_MainFrame:Hide() else MoP_GM_MainFrame:Show() end
        end
    end
end
