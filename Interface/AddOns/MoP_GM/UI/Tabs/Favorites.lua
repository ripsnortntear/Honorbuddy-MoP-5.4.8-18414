-- MoP_GM/UI/Tabs/Favorites.lua
-- Re-renders pinned commands from MoP_GM_DB.favorites whenever it changes.

local favContent

local function rebuild()
    if not favContent then return end
    -- Remove previous children.
    for _, child in ipairs({ favContent:GetChildren() }) do
        child:Hide()
        child:SetParent(nil)
    end
    favContent.fontStrings = favContent.fontStrings or {}
    for _, fs in ipairs(favContent.fontStrings) do fs:Hide() end
    favContent.fontStrings = {}

    local favs = MoP_GM.GetFavorites()
    if #favs == 0 then
        local fs = favContent:CreateFontString(nil, "OVERLAY", "GameFontHighlight")
        fs:SetPoint("TOPLEFT", favContent, "TOPLEFT", 12, -12)
        fs:SetText("No favorites yet. Right-click any command button on another tab to pin it here.")
        table.insert(favContent.fontStrings, fs)
        favContent:SetHeight(60)
        return
    end
    local h = MoP_GM.LayoutRows(favContent, favs, { sectionTitle = ("%d pinned command(s)"):format(#favs) })
    favContent:SetHeight(math.max(h, 200))
end
MoP_GM.RefreshFavoritesTab = rebuild

MoP_GM.RegisterTab({
    id = "favorites", label = "Favorites",
    builder = function(parent)
        local scroll, content = MoP_GM.CreateScrollContent(parent)
        favContent = content
        rebuild()
    end,
    onShow = rebuild,
})
