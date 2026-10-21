# BotBases Overview

A **BotBase** is the high-level brain in Honorbuddy. It controls navigation, objective selection, interaction with NPCs and objects, and overarching decision-making. While the **Combat Routine** handles fighting when in combat, the **BotBase** directs where your character walks, which quests it turns in, when it mounts, and how it interacts with the world.

---

## BotBases Summary Table

| BotBase | Primary Purpose | Required Profile Type | Typical Use Case |
|---|---|---|---|
| **Questing** | Automated quest progression & leveling | Questing XML (`<QuestOrder>`) | 1–90 leveling, reputation grinding |
| **BGBuddy** | Automated PvP Battlegrounds | None (built-in logic) or custom mesh | Honor points, conquest points, marks |
| **DungeonBuddy** | Automated 5-man Dungeons & LFD | None (built-in dungeon scripts) | Leveling via LFD, Justice Points, gear |
| **Gatherbuddy2** | Mining & Herbalism gathering | Gathering XML (`<Hotspots>`) | Node farming, gold making, crafting mats |
| **ProfessionBuddy** | Trade skill crafting & AH automation | PB XML (`<Professionbuddy>`) | 1–600 professions, mass crafting |
| **Tyrael / LazyRaider**| Combat assist & raid botting | None | Manual raiding, Mythic/Heroic progression |
| **AutoAngler2** | Automated fishing & pool farming | Fishing XML or open water | Fishing 1–600, cooking mats, rare fish |
| **Grind Bot** | Mob farming, skinning & cloth | Grinding XML (`<GrindArea>`) | Rep farming, raw gold, cloth farming |

---

## Detailed BotBase Breakdown

### 1. Questing
The **Questing** botbase reads an XML profile and processes its `<QuestOrder>` block top-to-bottom.
- **Dynamic Branching**: Evaluates conditional expressions (`If`, `While`) based on character level, quest completion status, inventory items, or faction.
- **Quest Behaviors**: Executes specialized scripted actions called *Quest Behaviors* (e.g. `FlyTo`, `InteractWith`, `CollectThings`, `VehicleCheck`, `Escort`, `CastSpellOn`) to handle complex quest mechanics like vehicles, bombs, and cinematics.
- **Vendor / Mail**: Automatically visits vendors to repair gear, sell junk, buy food/drink, and send items to bank alts when inventory fills.

### 2. BGBuddy
**BGBuddy** automates World of Warcraft Battlegrounds without requiring external XML profiles:
- **Supported Maps**:
  - Warsong Gulch & Twin Peaks (Flag capturing, flag carrier escorting, returns)
  - Arathi Basin & Battle for Gilneas (Node assaulting, defending, callouts)
  - Eye of the Storm (Flag running, tower control)
  - Alterac Valley & Isle of Conquest (Boss rushing, bunker/tower capping, vehicle siege)
  - Silvershard Mines (Cart tracking, track switching)
  - Temple of Kotmogu (Orb pickup, center positioning, orb dropping when low health)
- **Features**: Automatically queues for chosen or random battlegrounds, handles resurrection and corpse recovery, mounts up, and attacks enemy objectives.

### 3. DungeonBuddy
**DungeonBuddy** is an advanced PvE automation engine that queues for and completes 5-man dungeons through the Looking for Dungeon (LFD) tool:
- **Role Support**:
  - **Tank**: Leads the group, manages threat, pulls according to dungeon script waypoints.
  - **DPS**: Follows tank, targets primary marked enemies, stays out of void zones.
  - **Healer**: Follows group, dispels debuffs, manages party and tank health pools.
- **Mechanics Handling**: Scripted avoidance for boss abilities, cleanses, adds priority, and interaction with dungeon levers and teleporters.
- **Loot Rolling**: Auto-greeds, needs, or passes based on item suitability and user settings.

### 4. Gatherbuddy2
**Gatherbuddy2** is designed specifically for gathering professions (Herbalism and Mining):
- **Hotspot Patrol**: Navigates between waypoints defined in the profile's `<Hotspots>` block.
- **Node Detection**: Detects ore veins and herb nodes within tracking radius using internal object manager memory reads.
- **Mount & Flight Engine**: Smoothly mounts ground or flying mounts, navigates directly to node coordinates, lands, loots, and resumes waypoint patrolling.
- **Blacklisting**: Dynamically blacklists unreachable nodes (e.g., nodes inside terrain or surrounded by hostile elite mobs) to avoid bot loops.

### 5. ProfessionBuddy
**ProfessionBuddy** is a macro/scripting execution environment within Honorbuddy:
- **Complex Recipes**: Automates multi-stage crafting pipelines (e.g., buy materials from vendor -> smelt ore -> craft items -> disenchant -> sell enchanting dust on Auction House).
- **Auction House Integration**: Automatically posts auctions, cancels undercut auctions, and collects gold from the mailbox.
- **1–600 Guides**: Executes unified 1–600 leveling profiles for Alchemy, Blacksmithing, Enchanting, Engineering, Inscription, Jewelcrafting, Leatherworking, and Tailoring.

### 6. Tyrael / LazyRaider
For players who want the performance of automated combat without relinquishing character control:
- **Manual Movement**: You steer your character with WASD and the mouse.
- **Automated Combat**: The bot continuously executes the combat routine, casting spells, reacting to procs, purging enemies, and healing injured allies.
- **Dungeon & Raid Safety**: Allows you to handle raid positioning, movement mechanics, and phase transitions while maintaining top-tier parses.

### 7. AutoAngler2
Automates the Fishing secondary profession:
- Detects water bodies and designated fishing pools.
- Equips fishing rods, applies lures to the rod when expired.
- Casts the line, monitors the fishing bobber memory state, and loots instantly upon bite animation.

---

## Switching Between BotBases

1. Click **Stop** on the Honorbuddy main window.
2. Open the **BotBase** dropdown menu and choose the desired mode.
3. If the new BotBase requires a profile (such as *Questing* or *Gatherbuddy2*), click **Load Profile** and select the appropriate file.
4. Click **Start**.
