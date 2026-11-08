# Ultimate 1-90 Leveling Controller

Profile: `Special\Ultimate-1-90-Leveling-Controller.xml`

Load it with **ProfessionBuddy (Bundled)** on Honorbuddy MoP 5.4.8 (18414). It is not Rogue-specific: it reads the current character's name, level, race, class, faction and professions.

## Modes

Set **Mode** in the ProfessionBuddy profile settings:

| Mode | Action |
|---|---|
| `Auto` | Cooking/Fishing if needed, then Archaeology at 20+, then Questing. |
| `Setup` | Show the two-primary-profession chooser only. |
| `CookingFishing` | Load the Alliance or Horde JWheels Cooking/Fishing PB profile. |
| `Archaeology` | At level 20+, switch PB's secondary bot to ArchaeologyBuddy. |
| `PrimaryProfessions` | Load one mapped profile for the saved primary-profession choices. |
| `Questing` | Select the appropriate Kick profile by level/faction, or use the override. |

**One bot runs at a time.** After a handoff, the child profile normally remains active. Reload this controller and choose another mode to continue with a different activity; it is not a simultaneous questing/profession scheduler.

## Primary-profession chooser

At level 5+, with fewer than two known primary professions and no saved choices, the controller displays a Windows Forms dialog with two profession dropdowns. It saves the choices to `Primary Profession 1` and `Primary Profession 2` in PB's per-profile settings. Use `Setup` to show it again, or edit those settings directly.

`PrimaryProfessions` currently maps:

- Mining + Herbalism (or either gathering profession): `Farming\Mining-Herb\Mining-Herb-1-600-Flying-GB2.xml`
- Alchemy: `Alchemy\Alchemist-1-500-Dalaran-AH.xml`
- Jewelcrafting: `Jewelcrafting\Jewelcrafting-1-500.xml`
- Enchanting or Tailoring: `Enchanting-Tailoring\Enchanting-Tailoring-1-600-Neutral.xml`
- Inscription: `Inscription\Inscription-Silkweed-Scribe-Max-NPH.xml`
- Alliance Skinning: `1-90 Questing Profile Pack\Professions\Skinning\[A - Fly] QO Skinning 1-525 [Kick].xml` through Questing

Only one mapped profile is loaded per controller pass. Re-run `PrimaryProfessions` for another mapped profession if needed. Blacksmithing, Engineering, Leatherworking and Horde Skinning currently log that no complete bundled leveling profile is mapped; they are not silently substituted with a different profession.

## Cooking and Fishing

The faction JWheels profiles train and level Cooking/Fishing through their supported 1-525 progression:

- `Cooking-Fishing\Cooking-Fishing-1-525-Alliance-JWheels.xml`
- `Cooking-Fishing\Cooking-Fishing-1-525-Horde-JWheels.xml`

They are PB profiles, not Questing profiles. Their 525+ MoP coverage is not extended by this controller. Additional fishing profiles remain in `Cooking-Fishing` for manual use.

## Archaeology

At level 20+, the controller switches to **ArchaeologyBuddy** (the bot name is configurable). ArchaeologyBuddy's settings include `AutoTrain` (default `true`) and `TrainUpto` (default `375`); adjust `TrainUpto` in its configuration if you want a higher cap. Archaeology is secondary and does not consume either primary-profession slot.

## Questing level map

Unless `Questing Profile Override` is set, Questing mode selects:

| Level | Kick profile |
|---|---|
| 1-12 | Alliance/Horde 1-12 Auto-Loader (race selection happens inside the loader) |
| 13-57 | Alliance EK 12-58 / Horde 12-58 K-EK |
| 58-67 | Alliance/Horde BC 58-70 |
| 68-79 | Alliance/Horde LK 68-80 |
| 80-81 | Neutral Hyjal 80-82 |
| 82 | Neutral Deepholm 82-84 |
| 83 | Neutral Uldum 83-85 |
| 84 | Alliance/Horde Twilight Highlands 84-85 |
| 85 | Alliance/Horde Jade Forest 85-86 |
| 86 | Valley of the Four Winds 86-87 |
| 87 | Krasarang Wilds 87-88 |
| 88 | Kun-Lai Summit 88-89 |
| 89 | Townlong Steppes 89-90 |

`Questing Profile Override` accepts an absolute path or a path relative to the controller XML. Use it to resume a different zone or profile. Neutral Pandaren must choose a faction before a faction-specific mapping can be selected.

## Validation performed

- XML parses successfully.
- All 10 declarative `Path` references resolve.
- All 19 dynamic questing/profile references resolve.
- All 30 custom C# blocks and 21 conditions compile as C# 5/x86 against this installation's `Honorbuddy.exe` API surface.

In-game behavior still depends on server data, NPC phasing, quests already completed, profile settings, and the Kick/PB profiles themselves.
