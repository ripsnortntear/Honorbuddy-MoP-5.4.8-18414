-- MoP_GM/UI/Tabs/Player.lua
MoP_GM.RegisterTab({
    id = "player", label = "Player",
    builder = function(parent)
        MoP_GM.BuildSubTabs(parent, {
            { label = "Target",    rows = MoP_GM.Commands.PlayerTarget },
            { label = "Modify",    rows = MoP_GM.Commands.PlayerModify },
            { label = "Spells",    rows = MoP_GM.Commands.PlayerSpells },
            { label = "Learn",     rows = MoP_GM.Commands.PlayerLearn  },
            { label = "Reset",     rows = MoP_GM.Commands.PlayerReset  },
            { label = "Character", rows = MoP_GM.Commands.PlayerChar   },
            { label = "Guild",     rows = MoP_GM.Commands.Guild        },
        }, "subTab_player")
    end,
})
