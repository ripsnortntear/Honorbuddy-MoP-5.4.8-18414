-- MoP_GM/UI/Tabs/Quest.lua
MoP_GM.RegisterTab({
    id = "quest", label = "Quest",
    builder = function(parent)
        MoP_GM.LayoutRows(parent, MoP_GM.Commands.Quest)
    end,
})
