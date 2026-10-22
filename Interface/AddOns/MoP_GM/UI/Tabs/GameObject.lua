-- MoP_GM/UI/Tabs/GameObject.lua
MoP_GM.RegisterTab({
    id = "gobject", label = "Object",
    builder = function(parent)
        MoP_GM.LayoutRows(parent, MoP_GM.Commands.GameObject)
    end,
})
