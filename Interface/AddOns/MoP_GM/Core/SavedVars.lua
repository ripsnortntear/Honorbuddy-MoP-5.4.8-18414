-- MoP_GM/Core/SavedVars.lua
-- Frame-position helpers, history ring buffer, favorites, last-used inputs.

local HISTORY_MAX = 20

local function db()
    return MoP_GM.db or MoP_GM_DB
end

function MoP_GM.SaveFramePoint(frame, key)
    if not frame or not key then return end
    local point, _, relPoint, x, y = frame:GetPoint()
    if not point then return end
    local d = db()
    d[key] = d[key] or {}
    d[key].point, d[key].relPoint, d[key].x, d[key].y = point, relPoint, x, y
end

function MoP_GM.RestoreFramePoint(frame, key, fallback)
    if not frame then return end
    local d = db()
    local saved = d and d[key]
    frame:ClearAllPoints()
    if saved and saved.point then
        frame:SetPoint(saved.point, UIParent, saved.relPoint or saved.point, saved.x or 0, saved.y or 0)
    elseif fallback then
        frame:SetPoint(fallback.point or "CENTER", UIParent, fallback.relPoint or fallback.point or "CENTER", fallback.x or 0, fallback.y or 0)
    else
        frame:SetPoint("CENTER", UIParent, "CENTER", 0, 0)
    end
end

-- History ring buffer (newest first, deduped against most-recent entry)
function MoP_GM.PushHistory(line)
    if not line or line == "" then return end
    local d = db()
    d.history = d.history or {}
    if d.history[1] == line then return end
    table.insert(d.history, 1, line)
    while #d.history > HISTORY_MAX do
        table.remove(d.history)
    end
    if MoP_GM.RefreshHistoryTab then MoP_GM.RefreshHistoryTab() end
end

function MoP_GM.GetHistory()
    local d = db()
    return d.history or {}
end

function MoP_GM.ClearHistory()
    db().history = {}
    if MoP_GM.RefreshHistoryTab then MoP_GM.RefreshHistoryTab() end
end

-- Favorites: keyed by "<group>:<id>" so duplicates across tabs collapse
local function favKey(def)
    return (def.group or "?") .. ":" .. (def.id or def.label or "?")
end

function MoP_GM.IsFavorite(def)
    local d = db()
    d.favorites = d.favorites or {}
    return d.favorites[favKey(def)] ~= nil
end

function MoP_GM.ToggleFavorite(def)
    local d = db()
    d.favorites = d.favorites or {}
    local k = favKey(def)
    if d.favorites[k] then
        d.favorites[k] = nil
    else
        local copy = {}
        for ck, cv in pairs(def) do copy[ck] = cv end
        d.favorites[k] = copy
    end
    if MoP_GM.RefreshFavoritesTab then MoP_GM.RefreshFavoritesTab() end
end

function MoP_GM.GetFavorites()
    local d = db()
    d.favorites = d.favorites or {}
    local list = {}
    for _, def in pairs(d.favorites) do
        table.insert(list, def)
    end
    table.sort(list, function(a, b) return (a.label or "") < (b.label or "") end)
    return list
end

-- Per-input remembered values
function MoP_GM.GetInputCache(rowKey, argKey)
    local d = db()
    d.inputs = d.inputs or {}
    local row = d.inputs[rowKey]
    return row and row[argKey] or nil
end

function MoP_GM.SetInputCache(rowKey, argKey, value)
    local d = db()
    d.inputs = d.inputs or {}
    d.inputs[rowKey] = d.inputs[rowKey] or {}
    d.inputs[rowKey][argKey] = value
end
