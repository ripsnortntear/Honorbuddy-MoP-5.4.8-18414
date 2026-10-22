-- MoP_GM/Core/CommandRunner.lua
-- Single chokepoint for sending dot-commands to the server.

-- Send a dot-command to the server. Trinity-derived cores (Emucoach included)
-- intercept dot-prefixed messages on the server side before they're broadcast
-- as normal chat, so SendChatMessage("SAY") delivers the command silently —
-- no public SAY appears in chat, only the server's response to the command.
function MoP_GM._ExecuteRaw(line)
    if MoP_GM.IsBlank(line) then return end
    SendChatMessage(line, "SAY")
    MoP_GM.PushHistory(line)
end

-- Send a command and capture the next ~2 seconds of CHAT_MSG_SYSTEM responses
-- back into chat, prefixed so they're easy to spot/copy. Used by /mopgm probe
-- for live testing — much less noise than scrolling the whole chat log.
function MoP_GM.Probe(line)
    line = MoP_GM.Trim(line or "")
    if line == "" then
        DEFAULT_CHAT_FRAME:AddMessage("|cff33ff99[MoP_GM probe]|r usage: /mopgm probe <command>")
        return
    end
    if not line:match("^[%./]") then line = "." .. line end

    local capture = {}
    local listener = CreateFrame("Frame")
    listener:RegisterEvent("CHAT_MSG_SYSTEM")
    listener:SetScript("OnEvent", function(self, event, msg)
        table.insert(capture, msg)
    end)

    DEFAULT_CHAT_FRAME:AddMessage("|cff33ff99[MoP_GM probe]|r " .. line)
    SendChatMessage(line, "SAY")

    -- 5.4.8 doesn't have C_Timer; poll OnUpdate until the window elapses.
    local elapsed = 0
    listener:SetScript("OnUpdate", function(self, dt)
        elapsed = elapsed + dt
        if elapsed >= 2 then
            self:SetScript("OnUpdate", nil)
            self:UnregisterAllEvents()
            if #capture == 0 then
                DEFAULT_CHAT_FRAME:AddMessage("  |cffaaaaaa(no system response in 2s)|r")
            else
                for _, msg in ipairs(capture) do
                    DEFAULT_CHAT_FRAME:AddMessage("  |cff66ddff>|r " .. msg)
                end
            end
        end
    end)
end

-- Public entry point. opts.danger=true pops a confirm dialog first.
function MoP_GM.RunCommand(line, opts)
    if MoP_GM.IsBlank(line) then return end
    if opts and opts.danger then
        local dialog = StaticPopup_Show("MOPGM_CONFIRM_CMD", line)
        if dialog then dialog.data = line end
        return
    end
    MoP_GM._ExecuteRaw(line)
end

-- Build command string from a definition + a table of arg values keyed by arg.key.
-- Returns the string, or (nil, errorMessage) if a required arg is missing/invalid.
function MoP_GM.BuildLine(def, values)
    if not def or not def.format then return nil, "no command" end
    local args = def.args or {}
    if #args == 0 then return def.format end
    local resolved = {}
    for i, arg in ipairs(args) do
        local raw = values and values[arg.key]
        local v = MoP_GM.ResolveArg(raw, arg)
        if MoP_GM.IsBlank(v) then
            if arg.optional then
                v = arg.default or ""
            else
                return nil, "missing: " .. (arg.placeholder or arg.key)
            end
        end
        if arg.numeric then
            local n = tonumber(v)
            if not n then return nil, (arg.placeholder or arg.key) .. " must be numeric" end
            v = tostring(n)
        end
        resolved[i] = v
    end
    -- Strip trailing optional placeholders that are blank to keep the line tidy.
    while #resolved > 0 and resolved[#resolved] == "" do
        resolved[#resolved] = nil
    end
    -- string.format requires the right number of placeholders; if we trimmed
    -- trailing optional values, fall back to a manual concat after the prefix.
    local placeholderCount = 0
    for _ in def.format:gmatch("%%s") do placeholderCount = placeholderCount + 1 end
    if #resolved == placeholderCount then
        return string.format(def.format, unpack(resolved))
    else
        -- pad with empty strings, then trim trailing whitespace
        local padded = {}
        for i = 1, placeholderCount do padded[i] = resolved[i] or "" end
        local line = string.format(def.format, unpack(padded))
        return (line:gsub("%s+$", ""))
    end
end
