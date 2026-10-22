-- MoP_GM/Data/Commands.lua
-- Declarative command definitions consumed by UI/Tabs/*.
--
-- Each entry shape:
--   { id, label, format, args = { { key, placeholder, numeric, optional, fallback } }, danger, group, tooltip }
--
-- `format` is fed to string.format with one %s per arg (in declaration order).
-- `danger=true` triggers a confirm dialog before sending.
-- `fallback="target"` lets a blank field fall back to UnitName("target").
--
-- Commands are split into sub-groups (e.g. PlayerTarget, PlayerModify) so that
-- each tab's content is short enough to fit on screen without scrolling. Tabs
-- with too many commands compose multiple sub-groups via sub-tabs.

MoP_GM.Commands = {}
local C = MoP_GM.Commands

local function nameArg(opts)
    opts = opts or {}
    return { key = "name", placeholder = opts.placeholder or "player", fallback = "target", optional = opts.optional }
end

-- ─── General → Toggles ────────────────────────────────────────────────────
-- Verified against world.command DB on Emucoach 7.1. `.gm` accepts [on/off]
-- as args (not subcommands), so `.gm on` and `.gm off` work via the same
-- handler. `.taxicheat` doesn't exist standalone — use `.cheat taxi on/off`.
C.GeneralToggles = {
    { id="gmon",      label=".gm on",           format=".gm on",          tooltip="Enter GM mode (invisible to non-GMs)." },
    { id="gmoff",     label=".gm off",          format=".gm off",         tooltip="Leave GM mode." },
    { id="gmflyon",   label=".gm fly on",       format=".gm fly on",      tooltip="Toggle flying anywhere on." },
    { id="gmflyoff",  label=".gm fly off",      format=".gm fly off",     tooltip="Toggle flying off." },
    { id="gmvis",     label=".gm visible",      format=".gm visible",     tooltip="Toggle GM visibility for other players." },
    { id="gmchat",    label=".gm chat",         format=".gm chat %s",     args={{key="state",placeholder="on/off",optional=true}}, tooltip="Toggle GM badge in chat messages." },
    { id="gmingame",  label=".gm ingame",       format=".gm ingame",      tooltip="List online GMs." },
    { id="gmlist",    label=".gm list",         format=".gm list",        tooltip="List ALL GM accounts and security levels." },
    { id="gps",       label=".gps",             format=".gps",            tooltip="Print current map / x / y / z." },
    { id="commands",  label=".commands",        format=".commands",       tooltip="Print server-supported commands." },
    { id="cheattaxi_on", label="cheat taxi on",  format=".cheat taxi on", tooltip="Temp grant access to all flight masters." },
    { id="cheattaxi_off",label="cheat taxi off", format=".cheat taxi off",tooltip="Revoke flight master access." },
    { id="cheatstat", label="cheat status",     format=".cheat status",   tooltip="Print active cheats." },
}

-- ─── General → Cheats & Modify Self (14) ──────────────────────────────────
C.GeneralCheats = {
    { id="cheatgod",  label="cheat god",        format=".cheat god %s",   args={{key="state",placeholder="on/off"}}, tooltip="Toggle invulnerability." },
    { id="cheatpwr",  label="cheat power",      format=".cheat power %s", args={{key="state",placeholder="on/off"}}, tooltip="Infinite mana/rage/energy." },
    { id="cheatcd",   label="cheat cooldown",   format=".cheat cooldown %s",args={{key="state",placeholder="on/off"}}, tooltip="No cooldowns." },
    { id="cheatct",   label="cheat casttime",   format=".cheat casttime %s",args={{key="state",placeholder="on/off"}}, tooltip="Instant casts." },
    { id="cheatww",   label="cheat waterwalk",  format=".cheat waterwalk %s",args={{key="state",placeholder="on/off"}}, tooltip="Walk on water." },
    { id="cheatexp",  label="cheat explore",    format=".cheat explore %s",args={{key="state",placeholder="on/off"}}, tooltip="Reveal map." },
    { id="morph",     label="morph",            format=".morph %s",       args={{key="displayid",placeholder="displayId",numeric=true}}, tooltip="Change model." },
    { id="demorph",   label="demorph",          format=".demorph",        tooltip="Revert to default model." },
    { id="mount",     label="mount",            format=".mount %s",       args={{key="mountid",placeholder="mountId",numeric=true}}, tooltip="Mount creature display id." },
    { id="dismount",  label="dismount",         format=".dismount",       tooltip="Dismount." },
    { id="modscale",  label="modify scale",     format=".modify scale %s",args={{key="scale",placeholder="0.1-10",numeric=true}}, tooltip="Resize player." },
    -- `.modify speed` takes a $speedtype: fly | all | walk | backwalk | swim
    -- Verified live against Emucoach 7.1.
    { id="modspeedall",  label="speed all",     format=".modify speed all %s",     args={{key="rate",placeholder="0.1-50",numeric=true}}, tooltip="All movement speeds." },
    { id="modspeedwalk", label="speed walk",    format=".modify speed walk %s",    args={{key="rate",placeholder="0.1-50",numeric=true}} },
    { id="modspeedback", label="speed backwalk",format=".modify speed backwalk %s",args={{key="rate",placeholder="0.1-50",numeric=true}} },
    { id="modspeedswim", label="speed swim",    format=".modify speed swim %s",    args={{key="rate",placeholder="0.1-50",numeric=true}} },
    { id="modspeedfly",  label="speed fly",     format=".modify speed fly %s",     args={{key="rate",placeholder="0.1-50",numeric=true}} },
}

-- ─── Player → Target / state ──────────────────────────────────────────────
-- Verified vs world.command DB: `.kill` doesn't exist on this server — `.die`
-- is the only kill-command, and it operates on the selected unit (not by name).
C.PlayerTarget = {
    { id="appear",   label="appear",   format=".appear %s",  args={ nameArg() }, tooltip="Teleport to player." },
    { id="summon",   label="summon",   format=".summon %s",  args={ nameArg() }, tooltip="Summon player to you." },
    { id="recall",   label="recall",   format=".recall %s",  args={ nameArg{ optional=true } }, tooltip="Teleport player back to recall position." },
    { id="kick",     label="kick",     format=".kick %s %s", args={ nameArg(),{key="reason",placeholder="(optional reason)",optional=true} }, danger=true },
    { id="die",      label="die (kill selected)",format=".die",       danger=true, tooltip="Kill the selected unit. Kills you if nothing selected." },
    { id="damage",   label="damage",   format=".damage %s",  args={{key="amount",placeholder="amount",numeric=true}}, danger=true, tooltip="Apply flat damage to the selected target." },
    { id="revive",   label="revive",   format=".revive %s",  args={ nameArg{ optional=true } }, tooltip="Revive target/named player." },
    { id="freeze",   label="freeze",   format=".freeze %s %s",args={ nameArg{ optional=true },{key="seconds",placeholder="duration s",numeric=true,optional=true} }, tooltip="Freeze player in place." },
    { id="unfreeze", label="unfreeze", format=".unfreeze %s",args={ nameArg{ optional=true } } },
    { id="listfreeze",label="listfreeze",format=".listfreeze",tooltip="List all currently-frozen players." },
    { id="playerinfo",label="pinfo",   format=".pinfo %s",   args={ nameArg{ optional=true } }, tooltip="Detailed info on selected/named player." },
    { id="combatstop",label="combatstop",format=".combatstop %s",args={ nameArg{ optional=true } }, tooltip="End combat for selected/named player." },
    { id="lookplayer",label="lookup player",format=".lookup player account %s",args={{key="account",placeholder="account name"}}, tooltip="Find player characters by account name." },
}

-- ─── Player → Modify ──────────────────────────────────────────────────────
-- Verified live against Emucoach 7.1: `.modify level` and `.modify xp` no
-- longer exist (replaced by `.character level` which lives in PlayerReset).
-- `.modify arena` is also gone from this build's `.modify` subcommand list.
C.PlayerModify = {
    { id="modmoney", label="modify money", format=".modify money %s", args={{key="copper",placeholder="copper",numeric=true}}, tooltip="Add copper to target." },
    { id="modhp",    label="modify hp",    format=".modify hp %s",    args={{key="hp",placeholder="hp",numeric=true}} },
    { id="modmana",  label="modify mana",  format=".modify mana %s",  args={{key="mana",placeholder="mana",numeric=true}} },
    { id="modrage",  label="modify rage",  format=".modify rage %s",  args={{key="rage",placeholder="rage",numeric=true}} },
    { id="modenergy",label="modify energy",format=".modify energy %s",args={{key="energy",placeholder="energy",numeric=true}} },
    { id="modrunic", label="modify runicpower",format=".modify runicpower %s",args={{key="amount",placeholder="amount",numeric=true}}, tooltip="DK resource." },
    { id="modhonor", label="modify honor", format=".modify honor %s", args={{key="amount",placeholder="amount",numeric=true}} },
    { id="modphase", label="modify phase", format=".modify phase %s", args={{key="mask",placeholder="phaseMask",numeric=true}} },
    { id="modgender",label="modify gender",format=".modify gender %s",args={{key="gender",placeholder="0=M 1=F",numeric=true}} },
    { id="moddrunk", label="modify drunk", format=".modify drunk %s", args={{key="state",placeholder="0-100",numeric=true}} },
    { id="modstand", label="modify standstate",format=".modify standstate %s",args={{key="state",placeholder="0-8",numeric=true}}, tooltip="Sit / stand / sleep / kneel etc." },
    { id="modfaction",label="modify faction",format=".modify faction %s",args={{key="factionId",placeholder="factionId",numeric=true}}, tooltip="Set unit faction id." },
    { id="modrep",   label="modify reputation",format=".modify reputation %s %s",args={{key="faction",placeholder="factionId",numeric=true},{key="value",placeholder="value",numeric=true}} },
    { id="modcurrency",label="modify currency",format=".modify currency %s %s",args={{key="id",placeholder="currencyId",numeric=true},{key="amount",placeholder="amount",numeric=true}} },
}

-- ─── Player → Spells / abilities ──────────────────────────────────────────
-- Cast, aura, cooldown, skill ops. All confirmed in world.command DB on Emucoach 7.1.
C.PlayerSpells = {
    { id="cast",        label="cast",         format=".cast %s",            args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Cast on selected (or self if no target)." },
    { id="castself",    label="cast self",    format=".cast self %s",       args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Cast on the target's own self." },
    { id="casttarget",  label="cast target",  format=".cast target %s",     args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Selected target casts the spell on its own victim." },
    { id="castback",    label="cast back",    format=".cast back %s",       args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Selected target casts the spell on you." },
    { id="casttrig",    label="cast triggered",format=".cast %s triggered", args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Cast bypassing all checks/cost." },
    { id="aura",        label="aura",         format=".aura %s",            args={{key="spellid",placeholder="spellId",numeric=true}} },
    { id="unaura",      label="unaura",       format=".unaura %s",          args={{key="spellid",placeholder="spellId",numeric=true}} },
    { id="cdall",       label="cooldown all", format=".cooldown",           tooltip="Clear all cooldowns on target." },
    { id="cdone",       label="cooldown",     format=".cooldown %s",        args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Clear cooldown of one specific spell." },
    { id="maxskill",    label="maxskill",     format=".maxskill",           tooltip="Max all skills for current level." },
    { id="setskill",    label="setskill",     format=".setskill %s %s %s",  args={{key="skill",placeholder="skillId",numeric=true},{key="value",placeholder="value",numeric=true},{key="max",placeholder="max",numeric=true,optional=true}} },
    { id="lookspell",   label="lookup spell", format=".lookup spell %s",    args={{key="text",placeholder="search"}} },
    { id="lookskill",   label="lookup skill", format=".lookup skill %s",    args={{key="text",placeholder="search"}} },
}

-- ─── Player → Learn ───────────────────────────────────────────────────────
-- All `learn` / `unlearn` variants. Confirmed in world.command DB on Emucoach 7.1.
C.PlayerLearn = {
    { id="learn",       label="learn",                format=".learn %s",            args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Teach a single spell." },
    { id="learnall",    label="learn (all ranks)",    format=".learn %s all",        args={{key="spellid",placeholder="spellId",numeric=true}}, tooltip="Teach a spell with all ranks." },
    { id="unlearn",     label="unlearn",              format=".unlearn %s",          args={{key="spellid",placeholder="spellId",numeric=true}} },
    { id="learnclass",  label="all my class",         format=".learn all my class",  tooltip="Learn all spells & talents for your class." },
    { id="learnspells", label="all my spells",        format=".learn all my spells", tooltip="Class spells, no talents." },
    { id="learntalents",label="all my talents",       format=".learn all my talents",tooltip="Talents only." },
    { id="learnpettal", label="all my pet talents",   format=".learn all my pettalents",tooltip="Hunter pet talents." },
    { id="learnrec",    label="all recipes",          format=".learn all recipes %s",args={{key="profession",placeholder="(optional)",optional=true}}, tooltip="Master profession recipes." },
    { id="learncrafts", label="all crafts",           format=".learn all crafts",    tooltip="All professions + recipes." },
    { id="learndefault",label="all default",          format=".learn all default %s",args={ nameArg{ optional=true } }, tooltip="Restore default spells/quest rewards." },
    { id="learnlang",   label="all languages",        format=".learn all lang",      tooltip="Learn every language." },
    { id="learngm",     label="all GM",               format=".learn all gm",        tooltip="Learn GM-only utility spells." },
}

-- ─── Player → Reset ───────────────────────────────────────────────────────
-- Emucoach 7.1: achievements, honor, level, spells, stats, talents, all, pvpstat.
C.PlayerReset = {
    { id="rsttalents",  label="reset talents",     format=".reset talents %s",      args={ nameArg{ optional=true } }, danger=true },
    { id="rstspells",   label="reset spells",      format=".reset spells %s",       args={ nameArg{ optional=true } }, danger=true },
    { id="rstall",      label="reset all",         format=".reset all %s",          args={{key="kind",placeholder="talents/spells/stats"}}, danger=true },
    { id="rststats",    label="reset stats",       format=".reset stats",           danger=true },
    { id="rstlevel",    label="reset level",       format=".reset level %s",        args={ nameArg{ optional=true } }, danger=true },
    { id="rstach",      label="reset achievements",format=".reset achievements %s", args={ nameArg{ optional=true } }, danger=true },
    { id="rsthonor",    label="reset honor",       format=".reset honor %s",        args={ nameArg{ optional=true } }, danger=true },
    { id="rstpvp",      label="reset pvpstat",     format=".reset pvpstat %s",      args={ nameArg{ optional=true } }, danger=true },
    { id="unstuck",     label="unstuck",           format=".unstuck %s",            args={ nameArg{ optional=true } } },
    -- Achievement grant + instance lockout clearing (operate on the target).
    { id="achadd",      label="achievement add",   format=".achievement add %s",    args={{key="id",placeholder="achievementId",numeric=true}}, tooltip="Grant an achievement to the targeted player." },
    { id="instunbind",  label="instance unbind",   format=".instance unbind %s %s", args={{key="map",placeholder="mapId or all"},{key="diff",placeholder="(difficulty)",numeric=true,optional=true}}, tooltip="Clear the selected player's saved instance binds (use 'all' to clear every lockout)." },
    { id="instbinds",   label="instance listbinds",format=".instance listbinds",    tooltip="List the selected player's instance binds." },
    { id="inststats",   label="instance stats",    format=".instance stats",        tooltip="Show server instance statistics." },
}

-- ─── Player → Character ───────────────────────────────────────────────────
-- Emucoach 7.1 `.character` subcommand tree: antierror, customize,
-- changefaction, changerace, deleted, erase, level, rename, reputation,
-- titles, changeclass, changeaccount, boost, gear, setupItemCache, spec,
-- role, cleanup. Most useful subset surfaced here.
C.PlayerChar = {
    -- ⚠ `.character level` sets an ABSOLUTE level, not a delta (verified live:
    -- a lvl-1 char + `.character level 89` → lvl 89, not 90). Sec 7.
    { id="charlevel",  label="character level",     format=".character level %s %s",    args={ nameArg{ optional=true },{key="level",placeholder="exact level 1-90",numeric=true,optional=true} }, tooltip="Set the character to this EXACT level (absolute value, NOT a delta) — e.g. 90 means level 90, not +90." },
    { id="charrename", label="character rename",    format=".character rename %s",      args={ nameArg() }, tooltip="Flag a forced rename; the rename prompt appears at the player's next login. Cleanest when the target is offline." },
    { id="charcust",   label="character customize", format=".character customize %s",   args={ nameArg() }, tooltip="Full appearance redo (character-creation screen) at next login." },
    { id="charcrace",  label="change race",         format=".character changerace %s",  args={ nameArg() }, tooltip="Flag a race change; applied at next login." },
    { id="charcfac",   label="change faction",      format=".character changefaction %s",args={ nameArg() }, tooltip="Flag a faction change; applied at next login." },
    { id="charcclass", label="change class",        format=".character changeclass %s", args={ nameArg() }, tooltip="Flag a class change; applied at next login." },
    { id="charboost",  label="character boost",     format=".character boost %s",       args={ nameArg{ optional=true } }, tooltip="Apply character boost to named/selected." },
    -- `.character gear` is a PlayerBots script command (not in world.command).
    { id="chargear",   label="character gear",      format=".character gear %s",        args={{key="action",placeholder="max (only useful arg)",optional=true}}, tooltip="Equip the selected bot/char. Only 'max' is meaningful (top of range); other words fall through to default. ilvl ceiling is set by PlayerBots.GearScale in worldserver.conf (restart to apply)." },
    { id="charspec",   label="character spec",      format=".character spec %s",        args={{key="spec",placeholder="(optional)",optional=true}}, tooltip="Apply class spec abilities to selected." },
    { id="charrole",   label="character role",      format=".character role %s",        args={{key="role",placeholder="(optional)",optional=true}}, tooltip="Re-roll role for selected." },
    -- `.character reputation` with no args shows reputation list for selected.
    { id="charrep",     label="character reputation",  format=".character reputation %s",      args={ nameArg{ optional=true } }, tooltip="Show reputation list for selected/named." },
    { id="chartitles",  label="character titles",      format=".character titles %s",          args={ nameArg{ optional=true } }, tooltip="Show known titles for selected/named." },
    -- Deleted characters (recovery flow)
    { id="charDelList", label="deleted list",          format=".character deleted list %s",    args={{key="filter",placeholder="(name or guid)",optional=true}}, tooltip="List soft-deleted characters." },
    { id="charDelRest", label="deleted restore",       format=".character deleted restore %s %s",args={{key="ref",placeholder="name or guid"},{key="newname",placeholder="(optional new name)",optional=true}}, tooltip="Recover a soft-deleted character." },
    -- Permanent erase (real delete)
    { id="charerase",   label="character erase",       format=".character erase %s",           args={ nameArg() }, danger=true, tooltip="Permanently delete a character (no recovery)." },
}

-- ─── Items ────────────────────────────────────────────────────────────────
-- Verified vs world.command DB. Removed `.additemtoset`, `.bag clear`,
-- `.voidstorage`, `.reagentbank`, `.mailbox`, `.ahbot` — none exist on this
-- server. Added the `.deleteditem` recovery family and `.send items/money`
-- (mail items/gold to a player from any GM).
C.Items = {
    { id="additem",     label="additem",         format=".additem %s %s",                    args={{key="id",placeholder="itemId",numeric=true},{key="count",placeholder="count",numeric=true,optional=true,default="1"}}, tooltip="Add item to your bags." },
    { id="addset",      label="additemset",      format=".additemset %s",                    args={{key="setid",placeholder="setId",numeric=true}}, tooltip="Add an entire item set." },
    { id="rmitem",      label="removeitem",      format=".removeitem %s %s",                 args={{key="id",placeholder="itemId",numeric=true},{key="count",placeholder="count",numeric=true,optional=true,default="1"}} },
    { id="repair",      label="repair items",    format=".repairitems",                      tooltip="Repair all items on target." },
    -- Item recovery — `.deleteditem` family for items deleted by players.
    { id="deletedlist", label="deleted item list",format=".deleteditem list %s",             args={ nameArg() }, tooltip="Show items the player has deleted (recoverable for a while)." },
    { id="deletedrest", label="deleted item restore",format=".deleteditem restore %s %s",   args={ nameArg(),{key="type",placeholder="id or all"} }, tooltip="Restore one or all of the player's deleted items." },
    { id="itemdelete",  label="item delete (by GUID)",format=".itemdelete %s",               args={{key="guid",placeholder="itemGuid",numeric=true}}, danger=true, tooltip="Direct DB delete of a specific item GUID. Owner must be offline." },
    -- Mail items/gold to a player (works while offline)
    { id="senditems",   label="send items",      format=".send items %s %s %s %s",           args={ nameArg(),{key="subject",placeholder="subject"},{key="body",placeholder="body"},{key="items",placeholder="itemId[:count]"} }, tooltip="Mail an item (or list of items) to a player." },
    { id="sendmoney",   label="send money",      format=".send money %s %s %s %s",           args={ nameArg(),{key="subject",placeholder="subject"},{key="body",placeholder="body"},{key="copper",placeholder="copper",numeric=true} }, tooltip="Mail gold to a player." },
    -- Lookup
    { id="lookitem",    label="lookup item",     format=".lookup item %s",                   args={{key="text",placeholder="search"}}, tooltip="Find item id by name." },
    { id="lookset",     label="lookup itemset",  format=".lookup itemset %s",                args={{key="text",placeholder="search"}} },
    { id="listitem",    label="list item (find owners)",format=".list item %s %s",          args={{key="id",placeholder="itemId",numeric=true},{key="max",placeholder="max",numeric=true,optional=true,default="10"}}, tooltip="Find which characters/mails/auctions hold an item." },
    -- Misc
    { id="bank",        label="bank",            format=".bank",                              tooltip="Open bank for selected." },
}

-- ─── NPC → Spawn / Movement / Chat ────────────────────────────────────────
-- Verified vs world.command DB. Less-common subcommands (add formation,
-- add item, delete item, add move, textemote) intentionally omitted to keep
-- the row count under the no-scroll cap; they can be sent via the chat box.
C.NPCSpawn = {
    { id="npcadd",     label="npc add",          format=".npc add %s",        args={{key="entry",placeholder="creatureId",numeric=true}}, tooltip="Spawn a creature here (saved to DB)." },
    { id="npcaddtmp",  label="npc add temp",     format=".npc add temp %s",   args={{key="entry",placeholder="creatureId",numeric=true}}, tooltip="Spawn temporarily, not saved to DB." },
    { id="npcdel",     label="npc delete",       format=".npc delete %s",     args={{key="guid",placeholder="(optional guid)",numeric=true,optional=true}}, danger=true, tooltip="Delete creature (selected or by guid)." },
    { id="npcmove",    label="npc move",         format=".npc move",          tooltip="Move selected creature spawn point here." },
    { id="npcfollow",  label="npc follow start", format=".npc follow",        tooltip="Selected creature follows you." },
    { id="npcfollowstop",label="npc follow stop",format=".npc follow stop",   tooltip="Stop following." },
    { id="cometome",   label="come to me",       format=".cometome",          tooltip="Selected creature walks to your current position (not saved to DB)." },
    { id="respawn",    label="respawn nearby",   format=".respawn",           tooltip="Respawn all nearby creatures & gameobjects now, ignoring respawn timers." },
    { id="npcinfo",    label="npc info",         format=".npc info" },
    { id="npcsay",     label="npc say",          format=".npc say %s",        args={{key="text",placeholder="text"}} },
    { id="npcyell",    label="npc yell",         format=".npc yell %s",       args={{key="text",placeholder="text"}} },
    { id="npcwhisp",   label="npc whisper",      format=".npc whisper %s %s", args={ nameArg(),{key="text",placeholder="text"}} },
    { id="npcemote",   label="npc playemote",    format=".npc playemote %s",  args={{key="emote",placeholder="emoteId",numeric=true}} },
}

-- ─── NPC → Modify / Lookup ───────────────────────────────────────────────
-- Full `.npc set` subcommand list confirmed in DB: allowmove, data, entry,
-- factionid, flag, level, link, model, movetype, phase, spawndist, spawntime.
-- `.repopall` is NOT in DB. `.respawn` IS (sec 6) — lives in Spawn / Move above.
C.NPCModify = {
    { id="npcsetlvl",   label="npc set level",     format=".npc set level %s",     args={{key="level",placeholder="1-90",numeric=true}} },
    { id="npcsetfac",   label="npc set factionid", format=".npc set factionid %s", args={{key="faction",placeholder="factionId",numeric=true}}, tooltip="Set creature faction." },
    { id="npcsetent",   label="npc set entry",     format=".npc set entry %s",     args={{key="entry",placeholder="creatureId",numeric=true}}, tooltip="Switch creature to a different template." },
    { id="npcsetflag",  label="npc set flag",      format=".npc set flag %s",      args={{key="flag",placeholder="flagBits",numeric=true}}, tooltip="Vendor / quest giver / trainer etc." },
    { id="npcsetmodel", label="npc set model",     format=".npc set model %s",     args={{key="model",placeholder="modelId",numeric=true}} },
    { id="npcsetmtype", label="npc set movetype",  format=".npc set movetype %s",  args={{key="type",placeholder="0=idle 1=rand 2=wp",numeric=true}} },
    { id="npcsetphase", label="npc set phase",     format=".npc set phase %s",     args={{key="phase",placeholder="phaseMask",numeric=true}}, tooltip="Persist phasemask change to DB." },
    { id="npcsetspawnd",label="npc set spawndist", format=".npc set spawndist %s", args={{key="dist",placeholder="distance",numeric=true}}, tooltip="Random-walk radius around spawn point." },
    { id="npcsetspawnt",label="npc set spawntime", format=".npc set spawntime %s", args={{key="seconds",placeholder="seconds",numeric=true}}, tooltip="Respawn delay." },
    { id="lookcreat",   label="lookup creature",   format=".lookup creature %s",   args={{key="text",placeholder="search"}} },
    { id="lookevent",   label="lookup event",      format=".lookup event %s",      args={{key="text",placeholder="search"}} },
    { id="lookfaction", label="lookup faction",    format=".lookup faction %s",    args={{key="text",placeholder="search"}} },
    { id="distance",    label="distance",          format=".distance",             tooltip="Distance to selected unit." },
    { id="guidcmd",     label="guid",              format=".guid",                 tooltip="Print GUID of selected unit." },
}

-- ─── GameObject ───────────────────────────────────────────────────────────
-- Verified vs world.command DB on Emucoach 7.1.
C.GameObject = {
    { id="goadd",     label="gobject add",       format=".gobject add %s %s",     args={{key="entry",placeholder="objectId",numeric=true},{key="resp",placeholder="respawn s",numeric=true,optional=true}}, tooltip="Spawn a gameobject (saved to DB)." },
    { id="goaddtmp",  label="gobject add temp",  format=".gobject add temp %s",   args={{key="entry",placeholder="objectId",numeric=true}}, tooltip="Spawn temporarily, not saved to DB." },
    { id="godel",     label="gobject delete",    format=".gobject delete %s",     args={{key="guid",placeholder="guid",numeric=true,optional=true}}, danger=true },
    { id="gomove",    label="gobject move",      format=".gobject move %s",       args={{key="guid",placeholder="guid",numeric=true,optional=true}} },
    { id="goturn",    label="gobject turn",      format=".gobject turn %s",       args={{key="guid",placeholder="guid",numeric=true,optional=true}} },
    { id="gonear",    label="gobject near",      format=".gobject near %s",       args={{key="dist",placeholder="distance",numeric=true,optional=true,default="10"}} },
    { id="gotrgt",    label="gobject target",    format=".gobject target %s",     args={{key="entry",placeholder="objectId or name",optional=true}}, tooltip="Locate nearest gobject by id or name part." },
    { id="goinfo",    label="gobject info",      format=".gobject info" },
    { id="goactv",    label="gobject activate",  format=".gobject activate %s",   args={{key="guid",placeholder="guid",numeric=true,optional=true}}, tooltip="Activate (open/use) a door, button, etc." },
    { id="gosetphs",  label="gobject set phase", format=".gobject set phase %s %s",args={{key="guid",placeholder="guid",numeric=true},{key="phase",placeholder="phaseMask",numeric=true}}, tooltip="Persist phasemask change to DB." },
    { id="lookobj",   label="lookup object",     format=".lookup object %s",      args={{key="text",placeholder="search"}} },
}

-- ─── Teleport / Tele family ──────────────────────────────────────────────
-- Verified vs world.command DB on Emucoach 7.1: tele, tele add, tele group,
-- tele name. (No `.tele del` — deletion is via DB only.)
C.TeleByName = {
    { id="tele",      label="tele",          format=".tele %s",         args={{key="name",placeholder="location"}}, tooltip="Teleport to a saved name." },
    { id="telegroup", label="tele group",    format=".tele group %s",   args={{key="name",placeholder="location"}}, tooltip="Teleport you and your group to a saved location." },
    { id="telename",  label="tele name",     format=".tele name %s %s", args={ nameArg(),{key="location",placeholder="location"} }, tooltip="Teleport (offline) named player to a saved location." },
    { id="appearx",   label="appear",        format=".appear %s",       args={{key="name",placeholder="player",fallback="target"}} },
    { id="summonx",   label="summon",        format=".summon %s",       args={{key="name",placeholder="player",fallback="target"}} },
    { id="looktele",  label="lookup tele",   format=".lookup tele %s",  args={{key="text",placeholder="search"}}, tooltip="Find existing .tele names by substring." },
}

-- ─── Teleport / .go family ───────────────────────────────────────────────
-- Verified vs world.command DB on Emucoach 7.1: `.go` subcommands are
-- creature, graveyard, grid, object, taxinode, ticket, trigger, xyz, zonexy.
-- (No `.go quest` — removed. `.go gobject` is `.go object`.)
C.TeleGo = {
    { id="goxyz",     label="go xyz",         format=".go xyz %s %s %s %s",   args={{key="x",placeholder="x",numeric=true},{key="y",placeholder="y",numeric=true},{key="z",placeholder="z",numeric=true,optional=true},{key="map",placeholder="map",numeric=true,optional=true}}, tooltip="Teleport to coordinates." },
    { id="gozonexy",  label="go zonexy",      format=".go zonexy %s %s %s",   args={{key="x",placeholder="x",numeric=true},{key="y",placeholder="y",numeric=true},{key="zone",placeholder="zoneId",numeric=true,optional=true}}, tooltip="Teleport to zone-relative coordinates." },
    { id="gocrtr",    label="go creature",    format=".go creature %s",       args={{key="entry",placeholder="guid or id"}}, tooltip="Teleport to a creature by guid (or use lookup creature for ID)." },
    { id="goobj",     label="go object",      format=".go object %s",         args={{key="guid",placeholder="objectGuid",numeric=true}}, tooltip="Teleport to a gameobject by GUID." },
    { id="gograve",   label="go graveyard",   format=".go graveyard %s",      args={{key="id",placeholder="graveyardId",numeric=true}} },
    { id="gogrid",    label="go grid",        format=".go grid %s %s %s",     args={{key="gx",placeholder="gridX",numeric=true},{key="gy",placeholder="gridY",numeric=true},{key="map",placeholder="mapId",numeric=true,optional=true}}, tooltip="Teleport to centre of a server grid (debug)." },
    { id="gotaxi",    label="go taxinode",    format=".go taxinode %s",       args={{key="id",placeholder="taxinodeId",numeric=true}} },
    { id="goticket",  label="go ticket",      format=".go ticket %s",         args={{key="id",placeholder="ticketId",numeric=true}} },
    { id="gotrigger", label="go trigger",     format=".go trigger %s",        args={{key="id",placeholder="areatriggerId",numeric=true}} },
    { id="lookarea",  label="lookup area",    format=".lookup area %s",       args={{key="text",placeholder="search"}}, tooltip="Find area / zone IDs." },
    { id="lookmap",   label="lookup map",     format=".lookup map %s",        args={{key="text",placeholder="search"}}, tooltip="Find map IDs by name." },
    { id="looktaxi",  label="lookup taxinode",format=".lookup taxinode %s",   args={{key="text",placeholder="search"}}, tooltip="Find taxinode IDs (for .go taxinode)." },
}

-- ─── Quest (5) ────────────────────────────────────────────────────────────
C.Quest = {
    { id="qadd",    label="quest add",     format=".quest add %s",     args={{key="id",placeholder="questId",numeric=true}} },
    { id="qcomp",   label="quest complete",format=".quest complete %s",args={{key="id",placeholder="questId",numeric=true}} },
    { id="qrem",    label="quest remove",  format=".quest remove %s",  args={{key="id",placeholder="questId",numeric=true}} },
    { id="qreward", label="quest reward",  format=".quest reward %s",  args={{key="id",placeholder="questId",numeric=true}} },
    { id="qlook",   label="lookup quest",  format=".lookup quest %s",  args={{key="text",placeholder="search"}} },
}

-- ─── Server / Announcements ──────────────────────────────────────────────
-- Verified vs world.command DB on Emucoach 7.1.
C.ServerAnnounce = {
    { id="announce",       label="announce",         format=".announce %s",        args={{key="msg",placeholder="message"}}, tooltip="Broadcast to all players." },
    { id="nameannounce",   label="name announce",    format=".nameannounce %s",    args={{key="msg",placeholder="message"}}, tooltip="Broadcast with sender name shown." },
    { id="gmannounce",     label="gm announce",      format=".gmannounce %s",      args={{key="msg",placeholder="message"}}, tooltip="Broadcast to GMs only." },
    { id="gmnameannounce", label="gm name announce", format=".gmnameannounce %s",  args={{key="msg",placeholder="message"}} },
    { id="notify",         label="notify (popup)",   format=".notify %s",          args={{key="msg",placeholder="message"}}, tooltip="Popup-style notification to all." },
    { id="gmnotify",       label="gm notify (popup)",format=".gmnotify %s",        args={{key="msg",placeholder="message"}} },
    { id="sendmessage",    label="send message",     format=".send message %s %s", args={ nameArg(),{key="msg",placeholder="text"} }, tooltip="Send addon-style message to one player." },
    { id="sendmail",       label="send mail",        format=".send mail %s %s %s", args={ nameArg(),{key="subject",placeholder="subject"},{key="body",placeholder="body"} }, tooltip="Mail a plain message to a player." },
}

-- ─── Server / Status & MOTD ───────────────────────────────────────────────
C.ServerStatus = {
    { id="srvinfo",     label="server info",     format=".server info" },
    { id="srvplimit",   label="server plimit",   format=".server plimit %s",       args={{key="value",placeholder="(blank to view)",optional=true}}, tooltip="View or set max-player limit." },
    { id="srvmotd",     label="server motd",     format=".server motd" },
    { id="setmotd",     label="set motd",        format=".server set motd %s",     args={{key="msg",placeholder="message"}} },
    { id="setclosed",   label="set closed",      format=".server set closed %s",   args={{key="state",placeholder="on/off"}}, tooltip="Closed mode — only GMs can log in.", danger=true },
    { id="setloglevel", label="set log level",   format=".server set loglevel %s", args={{key="level",placeholder="0-3",numeric=true}} },
    { id="srvcorpses",  label="server corpses",  format=".server corpses",         tooltip="Force corpse cleanup pass." },
    { id="srvstatsmu",  label="server stats mapupdate",format=".server stats mapupdate",tooltip="Map update timing diagnostic." },
    { id="saveall",     label="saveall",         format=".saveall",                tooltip="Save all online players to DB." },
    { id="reload",      label="reload table",    format=".reload %s",              args={{key="table",placeholder="creature_template / quest_template / all"}} },
    -- Character export / import (dump files are written on the SERVER host).
    { id="pdumpwrite",  label="pdump write",     format=".pdump write %s %s",      args={{key="file",placeholder="filename"},{key="who",placeholder="name or guid"}}, tooltip="Export a character to a dump file on the server." },
    { id="pdumpload",   label="pdump load",      format=".pdump load %s %s %s %s", args={{key="file",placeholder="filename"},{key="account",placeholder="target account"},{key="newname",placeholder="(new name)",optional=true},{key="newguid",placeholder="(new guid)",numeric=true,optional=true}}, tooltip="Import a character dump file into an account." },
}

-- ─── Server / Lifecycle (shutdown / restart) ──────────────────────────────
C.ServerLifecycle = {
    -- Syntax: `.server shutdown #delay [#exit_code] [reason]`. Exit code is
    -- the middle arg (NOT the reason). Although the help suggests 0 is valid,
    -- on Emucoach 7.1 exit_code 0 fails — 1 works. We hardcode 1 so GMs only
    -- need to fill in seconds + reason.
    { id="srvshut",         label="server shutdown",       format=".server shutdown %s 1 %s",args={{key="seconds",placeholder="seconds",numeric=true},{key="reason",placeholder="reason",optional=true}}, danger=true },
    { id="srvshutcancel",   label="shutdown cancel",       format=".server shutdown cancel", tooltip="Cancel a pending shutdown." },
    { id="srvrestart",      label="server restart",        format=".server restart %s 1 %s", args={{key="seconds",placeholder="seconds",numeric=true},{key="reason",placeholder="reason",optional=true}}, danger=true },
    { id="srvrestartcancel",label="restart cancel",        format=".server restart cancel",   tooltip="Cancel a pending restart." },
    { id="srvidleshut",     label="idle shutdown",         format=".server idleshutdown %s",  args={{key="seconds",placeholder="seconds",numeric=true}}, tooltip="Shutdown when no players online for N seconds.", danger=true },
    { id="srvidleshutcan",  label="idle shutdown cancel",  format=".server idleshutdown cancel",tooltip="Cancel idle shutdown." },
    { id="srvidlerest",     label="idle restart",          format=".server idlerestart %s",   args={{key="seconds",placeholder="seconds",numeric=true}}, danger=true },
    { id="srvidlerestcan",  label="idle restart cancel",   format=".server idlerestart cancel" },
    { id="srvexit",         label="server exit (hard)",    format=".server exit",             tooltip="Immediate hard exit — server will not gracefully save.", danger=true },
}

-- ─── Moderation / Bans ────────────────────────────────────────────────────
-- Verified vs world.command DB. `.ban` subcommands: account, character,
-- playeraccount, ip. (`.ban solo` is also recognized at runtime per probe.)
C.ModerationBan = {
    { id="banacc",     label="ban account",      format=".ban account %s %s %s",      args={ nameArg{placeholder="account"},{key="dur",placeholder="1d/perm"},{key="reason",placeholder="reason"} }, danger=true },
    { id="banchar",    label="ban character",    format=".ban character %s %s %s",    args={ nameArg(),{key="dur",placeholder="1d/perm"},{key="reason",placeholder="reason"} }, danger=true },
    { id="banpacc",    label="ban playeraccount",format=".ban playeraccount %s %s %s",args={ nameArg{placeholder="character"},{key="dur",placeholder="1d/perm"},{key="reason",placeholder="reason"} }, danger=true, tooltip="Ban the account that owns this character." },
    { id="banip",      label="ban ip",           format=".ban ip %s %s %s",           args={{key="ip",placeholder="ip"},{key="dur",placeholder="1d/perm"},{key="reason",placeholder="reason"}}, danger=true },
    { id="unbanacc",   label="unban account",    format=".unban account %s",          args={ nameArg{placeholder="account"} } },
    { id="unbanchar",  label="unban character",  format=".unban character %s",        args={ nameArg() } },
    { id="unbanip",    label="unban ip",         format=".unban ip %s",               args={{key="ip",placeholder="ip"}} },
    { id="baninfoacc", label="baninfo account",  format=".baninfo account %s",        args={{key="account",placeholder="account name or id"}} },
    { id="baninfochar",label="baninfo character",format=".baninfo character %s",      args={ nameArg() } },
    { id="baninfoip",  label="baninfo ip",       format=".baninfo ip %s",             args={{key="ip",placeholder="ip"}} },
    { id="banlistacc", label="banlist account",  format=".banlist account %s",        args={{key="filter",placeholder="(optional name)",optional=true}}, tooltip="Search/list account bans." },
    { id="banlistchar",label="banlist character",format=".banlist character %s",      args={ nameArg() }, tooltip="Search ban list for character pattern." },
    { id="banlistip",  label="banlist ip",       format=".banlist ip %s",             args={{key="filter",placeholder="(optional ip)",optional=true}} },
}

-- ─── Moderation / Mute · Kick · Inspect ──────────────────────────────────
C.ModerationMute = {
    { id="mute",         label="mute",             format=".mute %s %s %s",        args={ nameArg(),{key="minutes",placeholder="minutes",numeric=true},{key="reason",placeholder="reason",optional=true} }, danger=true },
    { id="unmute",       label="unmute",           format=".unmute %s",            args={ nameArg() } },
    { id="mutelistacc",  label="mutelist account", format=".mutelist account %s",  args={{key="account",placeholder="account name"}} },
    { id="mutelistchar", label="mutelist character",format=".mutelist character %s",args={ nameArg() } },
    { id="kickmod",      label="kick",             format=".kick %s %s",           args={ nameArg(),{key="reason",placeholder="(optional reason)",optional=true} }, danger=true },
    { id="freezemod",    label="freeze",           format=".freeze %s %s",         args={ nameArg(),{key="seconds",placeholder="duration s",numeric=true,optional=true} } },
    { id="unfreezemod",  label="unfreeze",         format=".unfreeze %s",          args={ nameArg() } },
    { id="listfreezemod",label="listfreeze",       format=".listfreeze",           tooltip="List all currently-frozen players." },
    { id="pinfomod",     label="pinfo",            format=".pinfo %s",             args={ nameArg{ optional=true } } },
    { id="lookpacc",     label="lookup player account",format=".lookup player account %s", args={{key="account",placeholder="account"}}, tooltip="Find characters owned by an account." },
    { id="lookpip",      label="lookup player ip", format=".lookup player ip %s",       args={{key="ip",placeholder="ip"}}, tooltip="Find characters that connected from an IP." },
    { id="lookpemail",   label="lookup player email",format=".lookup player email %s",  args={{key="email",placeholder="email"}} },
    { id="whispers",     label="whispers",         format=".whispers %s",          args={{key="state",placeholder="on/off",optional=true}}, tooltip="Toggle GM acceptance of incoming whispers." },
}

-- ─── Bots (Emucoach reworked PlayerBot system) ────────────────────────────
-- All bot management on Emucoach 7.x is gossip-driven — these four commands
-- open the in-game UIs (Tester NPC) where everything else (attack/follow/
-- flee/aggressive/equip/trade/talents/etc.) is configured via clicks.
-- The legacy `.npcbot` system is NOT installed on Emucoach 7.x — its tab
-- has been removed. Verified live against Emucoach 7.1 (May 2026).
C.Bots = {
    { id="botadd",      label="Add bot",            format=".bot add", tooltip="Opens the Tester gossip — pick role (tank / DPS / healer / by spec)." },
    { id="botaddrole",  label="Add bots by role",   format=".bot addRoleBotsToGroup %s %s", args={{key="role",placeholder="1=heal 2=dps 3=tank",numeric=true},{key="qty",placeholder="qty",numeric=true,optional=true,default="1"}}, tooltip="Directly spawn N bots of given role into your group." },
    { id="botmgrsel",   label="Manage selected",    format=".bot manageselectedbot", tooltip="Per-bot UI: aggressive, follow, flee, equip/trade, talents, attack/heal/cc target." },
    { id="botmgrparty", label="Manage party",       format=".bot manageparty",       tooltip="Party-wide UI: attack, pull, flee on/off, follow on/off, aggressive on/off." },
}

-- ─── Guild (admin) ────────────────────────────────────────────────────────
-- Verified vs world.command DB: invite/rank/uninvite sec 5; create/rename/
-- delete sec 6. Guild name must be quoted (we add the quotes in `format`).
-- ⚠ There is NO `.guild level` command — guild level is DB-only and
-- `Guild::SaveToDB` rewrites it on shutdown, so it can't be set in-game.
C.Guild = {
    { id="guildcreate",   label="guild create",   format='.guild create %s "%s"',        args={ nameArg{ placeholder="leader (or selected)" }, {key="guild",placeholder="guild name"} }, tooltip="Create a guild with the selected/named LEADER (must be online). Starts at level 1." },
    { id="guildinvite",   label="guild invite",   format='.guild invite %s "%s"',        args={ nameArg(), {key="guild",placeholder="guild name"} }, tooltip="Add the selected/named player into a guild." },
    { id="guildrank",     label="guild rank",     format=".guild rank %s %s",            args={ nameArg(), {key="rank",placeholder="rank 0-4",numeric=true} }, tooltip="Set a member's rank (0=Guild Master … 4=Initiate)." },
    { id="guilduninvite", label="guild uninvite", format=".guild uninvite %s",           args={ nameArg() }, tooltip="Remove the selected/named player from their guild." },
    { id="guildrename",   label="guild rename",   format='.guild rename "%s" "%s"',      args={{key="guild",placeholder="current name"},{key="newname",placeholder="new name"}}, tooltip="Rename a guild." },
    { id="guilddelete",   label="guild delete",   format='.guild delete "%s"',           args={{key="guild",placeholder="guild name"}}, danger=true, tooltip="Delete a guild and drop all its members." },
}

-- ─── Accounts (admin) ─────────────────────────────────────────────────────
-- Verified vs world.command DB. High security (sec 7-9). create/delete/set
-- gmlevel/set password are flagged danger (account-altering).
C.AccountManage = {
    { id="acccreate",  label="account create",        format=".account create %s %s",       args={{key="acc",placeholder="account"},{key="pass",placeholder="password"}}, danger=true, tooltip="Create a new game account." },
    { id="accgm",      label="account set gmlevel",   format=".account set gmlevel %s %s %s",args={{key="acc",placeholder="account"},{key="level",placeholder="GM level 0-9",numeric=true},{key="realm",placeholder="realmId (-1 = all)",optional=true,default="-1"}}, danger=true, tooltip="Set an account's GM/security level. realm -1 = all realms." },
    { id="accpass",    label="account set password",  format=".account set password %s %s %s",args={{key="acc",placeholder="account"},{key="pass",placeholder="new password"},{key="pass2",placeholder="repeat password"}}, danger=true, tooltip="Reset an account's password (enter it twice)." },
    { id="acconline",  label="account onlinelist",    format=".account onlinelist",          tooltip="List all currently-online accounts." },
    { id="accdelete",  label="account delete",        format=".account delete %s",          args={{key="acc",placeholder="account"}}, danger=true, tooltip="Delete an account AND all of its characters. No recovery." },
}

-- ─── Server / Game events ─────────────────────────────────────────────────
C.ServerEvents = {
    { id="evtlist",  label="event activelist", format=".event activelist", tooltip="List currently active world/holiday events." },
    { id="evtstart", label="event start",      format=".event start %s",   args={{key="id",placeholder="eventId",numeric=true}}, tooltip="Start an event now (not saved to DB)." },
    { id="evtstop",  label="event stop",       format=".event stop %s",    args={{key="id",placeholder="eventId",numeric=true}}, tooltip="Stop an event now (not saved to DB)." },
}

-- ─── Moderation / Deserter debuff ─────────────────────────────────────────
C.ModerationDeserter = {
    { id="desbgadd",   label="deserter bg add",        format=".deserter bg add %s",       args={{key="time",placeholder="seconds"}}, danger=true, tooltip="Apply the Battleground Deserter debuff to your target." },
    { id="desbgrem",   label="deserter bg remove",     format=".deserter bg remove",       tooltip="Remove the Battleground Deserter debuff from your target." },
    { id="desinstadd", label="deserter instance add",  format=".deserter instance add %s", args={{key="time",placeholder="seconds"}}, danger=true, tooltip="Apply the Instance Deserter debuff to your target." },
    { id="desinstrem", label="deserter instance remove",format=".deserter instance remove",tooltip="Remove the Instance Deserter debuff from your target." },
}

-- Stamp a `group` field onto every entry so favorites can key by group:id.
for groupName, list in pairs(C) do
    for _, def in ipairs(list) do
        def.group = groupName
    end
end
