-- MoP_GM/Core/Util.lua
-- Misc helpers: target name resolution, color, format-args.

MoP_GM.colors = {
    accent  = "|cff33ff99",
    warn    = "|cffff7733",
    danger  = "|cffff3344",
    label   = "|cffffd100",
    reset   = "|r",
}

function MoP_GM.Print(msg)
    DEFAULT_CHAT_FRAME:AddMessage(MoP_GM.colors.accent .. "MoP_GM" .. MoP_GM.colors.reset .. ": " .. tostring(msg))
end

-- If the value is empty and the arg has a "fallback" of "target", try UnitName("target").
function MoP_GM.ResolveArg(value, arg)
    if value == nil or value == "" then
        if arg and arg.fallback == "target" then
            local n = UnitName("target")
            if n and n ~= "" then return n end
        end
        return nil
    end
    return value
end

function MoP_GM.IsBlank(s)
    return s == nil or s == "" or (type(s) == "string" and s:match("^%s*$") ~= nil)
end

function MoP_GM.Trim(s)
    if type(s) ~= "string" then return s end
    return (s:gsub("^%s+", ""):gsub("%s+$", ""))
end
