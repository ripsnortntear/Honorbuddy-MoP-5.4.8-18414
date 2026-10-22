-- MoP_GM/UI/Tabs/Bots.lua
-- Emucoach 7.x reworked PlayerBot system. Management is gossip-driven —
-- `.bot manageselectedbot` and `.bot manageparty` open in-game UIs that
-- expose attack/follow/flee/aggressive/equip/talents — so the addon only
-- needs the four entry points. The legacy `.npcbot` system isn't installed
-- on this repack, so we no longer have a separate sub-tab for it.

MoP_GM.RegisterTab({
    id = "bots", label = "Bots",
    builder = function(parent)
        MoP_GM.LayoutRows(parent, MoP_GM.Commands.Bots)
    end,
})
