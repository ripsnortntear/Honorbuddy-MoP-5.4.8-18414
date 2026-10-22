-- MoP_GM/Data/Teleports.lua
-- Seed list of teleport names shipped with the addon.
--
-- IMPORTANT: every `name` here is the EXACT string stored in the server's
-- `game_tele` table — `.tele <name>` only resolves a name that exists there.
-- This list was rebuilt 2026-05-22 by verifying each name against a live
-- EmuCoach MoP 7.1 `game_tele` (via wow-server-mcp). The previous list carried
-- ~12 guessed names (silvermoon, exodar, jadeforest, kunlai, tolbarad, …) that
-- do NOT exist on this core and produced dead buttons — those are removed.
--
-- Scope: TrinityCore capital presets + the standard EmuCoach MoP teleport pack
-- (Pandaria zones / raids / dungeons / scenarios). These are present on stock
-- EmuCoach MoP repacks, so they're safe to ship to every install. Anything
-- truly server-specific (a GM's own `.tele add` names) is NOT hardcoded here —
-- each install pulls its own extras locally via the "Import from server" button
-- in the Teleport ▸ Locations sub-tab (scrapes `.lookup tele`).

MoP_GM.SeedTeleports = {
    -- Capitals / classic hubs (TrinityCore presets, verified present)
    { name = "Stormwind",              label = "Stormwind" },
    { name = "Orgrimmar",              label = "Orgrimmar" },
    { name = "Ironforge",              label = "Ironforge" },
    { name = "ThunderBluff",           label = "Thunder Bluff" },
    { name = "Darnassus",              label = "Darnassus" },
    { name = "Undercity",              label = "Undercity" },
    { name = "Shattrath",              label = "Shattrath" },
    { name = "Dalaran",                label = "Dalaran" },
    { name = "GMIsland",               label = "GM Island" },
    { name = "Icecrown",               label = "Icecrown" },

    -- Pandaria zones
    { name = "TheJadeForest",          label = "The Jade Forest" },
    { name = "ValleyoftheFourWinds",   label = "Valley of the Four Winds" },
    { name = "KrasarangWilds",         label = "Krasarang Wilds" },
    { name = "TheVeiledStair",         label = "The Veiled Stair" },
    { name = "KunLaiSummit",           label = "Kun-Lai Summit" },
    { name = "TownlongSteppes",        label = "Townlong Steppes" },
    { name = "DreadWastes",            label = "Dread Wastes" },
    { name = "ValeofEternalBlossoms",  label = "Vale of Eternal Blossoms" },
    { name = "IsleofGiants",           label = "Isle of Giants" },
    { name = "IsleofThunder",          label = "Isle of Thunder" },
    { name = "TimelessIsle",           label = "Timeless Isle" },
    { name = "TheWanderingIsle",       label = "The Wandering Isle" },

    -- Pandaria capital hubs
    { name = "ShrineofSevenStars",     label = "Shrine of Seven Stars (Alliance)" },
    { name = "ShrineofTwoMoons",       label = "Shrine of Two Moons (Horde)" },

    -- MoP raids
    { name = "MogushanVaults",         label = "Mogu'shan Vaults" },
    { name = "HearthofFear",           label = "Heart of Fear" },     -- stored name is misspelled "Hearth"
    { name = "TerraceofEndlessSpring", label = "Terrace of Endless Spring" },
    { name = "ThroneofThunder",        label = "Throne of Thunder" },
    { name = "SiegeofOrgrimmar",       label = "Siege of Orgrimmar" },

    -- MoP dungeons
    { name = "TempleoftheJadeSerpent", label = "Temple of the Jade Serpent" },
    { name = "StormstoutBrewery",      label = "Stormstout Brewery" },
    { name = "ShadoPanMonastery",      label = "Shado-Pan Monastery" },
    { name = "MogushanPalace",         label = "Mogu'shan Palace" },
    { name = "SiegeofNiuzaoTemple",    label = "Siege of Niuzao Temple" },
    { name = "GateoftheSettingSun",    label = "Gate of the Setting Sun" },

    -- MoP scenarios
    { name = "TheramoresFallA",        label = "Theramore's Fall (Alliance)" },
    { name = "TheramoresFallH",        label = "Theramore's Fall (Horde)" },
    { name = "LionsLanding",           label = "Lion's Landing" },
    { name = "DominationPoint",        label = "Domination Point" },
    { name = "GreenstoneVillage",      label = "Greenstone Village" },
    { name = "BrewingStorm",           label = "A Brewing Storm" },
    { name = "BrewmoonFestival",       label = "Brewmoon Festival" },
    { name = "CryptOfForgottenKings",  label = "Crypt of Forgotten Kings" },
    { name = "ArenaOfAnnihiliation",   label = "Arena of Annihilation" },
    { name = "UngaIngoo",              label = "Unga Ingoo" },
    { name = "AssaultOnZanvess",       label = "Assault on Zan'vess" },
    { name = "DaggerInTheDark",        label = "Dagger in the Dark" },
    { name = "BattleOnTheHighSeas",    label = "Battle on the High Seas" },
    { name = "LittlePatience",         label = "Little Patience" },
    { name = "BloodInTheSnow",         label = "Blood in the Snow" },
    { name = "SecretsOfRagefire",      label = "Secrets of Ragefire" },
    { name = "ThunderKingsCitadel",    label = "Thunder King's Citadel" },
    { name = "DarkHeartOfPandaria",    label = "Dark Heart of Pandaria" },
    { name = "ProvingGrounds",         label = "Proving Grounds" },
    { name = "BrawlersGuildA",         label = "Brawler's Guild" },
    { name = "TheLostIslands",         label = "The Lost Islands" },
}
