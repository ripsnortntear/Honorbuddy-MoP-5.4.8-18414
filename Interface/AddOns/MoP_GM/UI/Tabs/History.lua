-- MoP_GM/UI/Tabs/History.lua
-- Last 20 commands sent. Click row to re-run, button at top to clear.

local histContent

local function rebuild()
    if not histContent then return end
    for _, child in ipairs({ histContent:GetChildren() }) do
        child:Hide()
        child:SetParent(nil)
    end
    histContent.fontStrings = histContent.fontStrings or {}
    for _, fs in ipairs(histContent.fontStrings) do fs:Hide() end
    histContent.fontStrings = {}

    -- Clear button at top
    local clearBtn = CreateFrame("Button", nil, histContent, "UIPanelButtonTemplate")
    clearBtn:SetSize(140, 22)
    clearBtn:SetPoint("TOPRIGHT", histContent, "TOPRIGHT", -8, -8)
    clearBtn:SetText("Clear history")
    clearBtn:SetScript("OnClick", function() MoP_GM.ClearHistory() end)

    local title = MoP_GM.CreateSectionHeader(histContent, "Recent commands (newest first)")
    title:SetPoint("TOPLEFT", histContent, "TOPLEFT", 12, -12)
    table.insert(histContent.fontStrings, title)

    local hist = MoP_GM.GetHistory()
    if #hist == 0 then
        local fs = histContent:CreateFontString(nil, "OVERLAY", "GameFontDisable")
        fs:SetPoint("TOPLEFT", title, "BOTTOMLEFT", 0, -10)
        fs:SetText("(no commands yet)")
        table.insert(histContent.fontStrings, fs)
        histContent:SetHeight(120)
        return
    end

    local y = -40
    for i, line in ipairs(hist) do
        local row = CreateFrame("Button", nil, histContent)
        row:SetHeight(20)
        row:SetPoint("TOPLEFT", histContent, "TOPLEFT", 12, y - (i - 1) * 22)
        row:SetPoint("RIGHT", histContent, "RIGHT", -28, 0)

        local fs = row:CreateFontString(nil, "OVERLAY", "GameFontHighlight")
        fs:SetPoint("LEFT", row, "LEFT", 4, 0)
        fs:SetText(("|cffaaaaaa%2d.|r %s"):format(i, line))
        row.fs = fs
        row:SetHighlightTexture("Interface\\QuestFrame\\UI-QuestTitleHighlight")
        row:SetScript("OnClick", function() MoP_GM._ExecuteRaw(line) end)
        row:SetScript("OnEnter", function(self)
            GameTooltip:SetOwner(self, "ANCHOR_RIGHT")
            GameTooltip:SetText(line, 0.4, 1, 0.4)
            GameTooltip:AddLine("Click to re-run.", 1, 1, 1)
            GameTooltip:Show()
        end)
        row:SetScript("OnLeave", function() GameTooltip:Hide() end)
    end
    histContent:SetHeight(math.max(60 + #hist * 22, 200))
end
MoP_GM.RefreshHistoryTab = rebuild

MoP_GM.RegisterTab({
    id = "history", label = "History",
    builder = function(parent)
        local scroll, content = MoP_GM.CreateScrollContent(parent)
        histContent = content
        rebuild()
    end,
    onShow = rebuild,
})
