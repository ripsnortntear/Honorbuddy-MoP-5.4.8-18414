-- MoP_GM/UI/Tabs/NPC.lua
MoP_GM.RegisterTab({
    id = "npc", label = "NPC",
    builder = function(parent)
        MoP_GM.BuildSubTabs(parent, {
            { label = "Spawn / Move", rows = MoP_GM.Commands.NPCSpawn  },
            { label = "Modify / Lookup", rows = MoP_GM.Commands.NPCModify },
        }, "subTab_npc")
    end,
})
