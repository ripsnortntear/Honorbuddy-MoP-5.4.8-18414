-- MoP_GM/UI/Tabs/Items.lua
MoP_GM.RegisterTab({
    id = "items", label = "Items",
    builder = function(parent)
        MoP_GM.LayoutRows(parent, MoP_GM.Commands.Items)
    end,
})
