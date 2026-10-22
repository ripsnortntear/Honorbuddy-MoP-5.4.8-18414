-- MoP_GM/UI/Tabs/General.lua
-- 2-column layout for the Cheats & Modify sub-tab — rows have at most 1 arg
-- so they fit in a half-width column comfortably.
MoP_GM.RegisterTab({
    id = "general", label = "General",
    builder = function(parent)
        MoP_GM.BuildSubTabs(parent, {
            { label = "Toggles",         rows = MoP_GM.Commands.GeneralToggles,
              layoutOpts = { rowsPerColumn = 8, columnWidth = 420 } },
            { label = "Cheats & Modify", rows = MoP_GM.Commands.GeneralCheats,
              layoutOpts = { rowsPerColumn = 8, columnWidth = 420 } },
        }, "subTab_general")
    end,
})
