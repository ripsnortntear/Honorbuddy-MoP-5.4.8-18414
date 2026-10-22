-- MoP_GM/UI/Tabs/Server.lua
MoP_GM.RegisterTab({
    id = "server", label = "Server",
    builder = function(parent)
        MoP_GM.BuildSubTabs(parent, {
            { label = "Announce",  rows = MoP_GM.Commands.ServerAnnounce  },
            { label = "Status",    rows = MoP_GM.Commands.ServerStatus    },
            { label = "Lifecycle", rows = MoP_GM.Commands.ServerLifecycle },
            { label = "Events",    rows = MoP_GM.Commands.ServerEvents    },
        }, "subTab_server")
    end,
})
