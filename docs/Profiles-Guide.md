# Profiles Guide

Profiles are XML documents that give Honorbuddy step-by-step instructions. Depending on the active BotBase, a profile can instruct the bot to level from 1 to 90 by completing quests, circle a zone harvesting ghost iron ore and golden lotus, or farm motes of harmony.

---

## Profile XML Architecture

All Honorbuddy profiles follow a standardized XML structure defined under the `<HBProfile>` root tag:

```xml
<HBProfile>
  <Name>[H - Quest] 85-86 The Jade Forest [Complete]</Name>
  <MinLevel>85</MinLevel>
  <MaxLevel>91</MaxLevel>
  <MinDurability>0.3</MinDurability>
  <UseMount>True</UseMount>

  <!-- Blacklisted navigation areas -->
  <Blackspots>
    <Blackspot X="1234.56" Y="-789.01" Z="12.34" Radius="30" />
  </Blackspots>

  <!-- Repair, sell, and food/drink vendors -->
  <Vendors>
    <Vendor Name="Grocer Shin" Entry="56789" Type="Food" X="1122.33" Y="-4455.66" Z="77.88" />
  </Vendors>

  <!-- Mailboxes for posting/receiving items -->
  <Mailboxes>
    <Mailbox X="1150.00" Y="-4460.00" Z="78.00" />
  </Mailboxes>

  <!-- Step-by-step Quest Execution -->
  <QuestOrder>
    <PickUp QuestName="The Art of War" QuestId="29611" GiverName="General Nazgrim" GiverId="54870" />
    <TurnIn QuestName="The Art of War" QuestId="29611" TurnInName="General Nazgrim" TurnInId="54870" />

    <If Condition="HasQuest(30004) &amp;&amp; !IsQuestCompleted(30004)">
      <CustomBehavior File="InteractWith" MobId="55123" QuestId="30004" Range="5" />
    </If>
  </QuestOrder>
</HBProfile>
```

### Key Profile Tags

| Tag | Purpose |
|---|---|
| `<Name>` | Friendly name displayed in the Honorbuddy UI and logs |
| `<MinLevel>` / `<MaxLevel>` | Level bounds. If character is outside range, bot halts or loads next profile |
| `<MinDurability>` | Triggers automated repair runs when gear durability falls below threshold (e.g. 0.3 = 30%) |
| `<Vendors>` | List of NPCs for repairing, selling gray junk, and purchasing supplies |
| `<Mailboxes>` | Locations of mailboxes for sending farm materials to bank characters |
| `<Blackspots>` | Circular zones around specific coordinates that the pathfinder will strictly avoid |
| `<QuestOrder>` | The procedural sequence of pickups, turn-ins, objectives, and behaviors |
| `<Hotspots>` | Waypoint loops used by **Gatherbuddy2** and **Grind Bot** |

---

## Directory Organization

The profile collection in `Default Profiles\` is organized into distinct categories:

```
Default Profiles/
├── 1-90 Unified/             # Complete automated quest leveling
│   ├── Alliance/             # Alliance quest profiles (1-60, 58-70, 68-80, 80-85, 85-90)
│   └── Horde/                # Horde quest profiles (1-60, 58-70, 68-80, 80-85, 85-90)
├── Gathering/                # Herbalism & Mining routes
│   ├── Mining/               # Zone-specific mining loops (Copper -> Ghost Iron)
│   ├── Herbalism/            # Zone-specific herb loops (Peacebloom -> Fool's Cap)
│   └── Dual/                 # Combined herb & ore gathering paths
├── Grinding/                 # Mob farming & materials
│   ├── Cloth/                # Silk, Mageweave, Runecloth, Netherweave, Frostweave, Windwool
│   └── Skinning/             # Beast skinning grinding routes
├── Professions/              # 1-600 Trade Skill Leveling
│   ├── Alchemy 1-600.xml
│   ├── Blacksmithing 1-600.xml
│   └── Inscription 1-600.xml
└── TBMC/                     # TheBuddyMiningCompany automated framework
```

---

## 1–90 Leveling Progression Flow

### Alliance Route
1. **1–12**: Racial Starting Zones (Elwynn Forest, Dun Morogh, Teldrassil, Azuremyst Isle, Gilneas, Wandering Isle)
2. **12–20**: Westfall / Loch Modan / Darkshore
3. **20–25**: Redridge Mountains / Ashenvale
4. **25–30**: Duskwood / Wetlands
5. **30–35**: Northern Stranglethorn
6. **35–40**: Cape of Stranglethorn / Western Plaguelands
7. **40–45**: Eastern Plaguelands / Thousand Needles
8. **45–48**: Badlands
9. **48–52**: Searing Gorge / Tanaris
10. **52–54**: Burning Steppes / Un'Goro Crater
11. **54–58**: Swamp of Sorrows / Blasted Lands
12. **58–70 (Outland)**: Hellfire Peninsula -> Zangarmarsh -> Terokkar Forest -> Nagrand -> Shadowmoon Valley
13. **68–80 (Northrend)**: Borean Tundra / Howling Fjord -> Dragonblight -> Grizzly Hills -> Zul'Drak -> Sholazar Basin -> Storm Peaks / Icecrown
14. **80–85 (Cataclysm)**: Mount Hyjal -> Deepholm -> Uldum -> Twilight Highlands
15. **85–90 (Pandaria)**: The Jade Forest -> Valley of the Four Winds / Krasarang Wilds -> Kun-Lai Summit -> Townlong Steppes -> Dread Wastes

### Horde Route
1. **1–12**: Racial Starting Zones (Durotar, Mulgore, Tirisfal Glades, Eversong Woods, Kezan/Lost Isles, Wandering Isle)
2. **12–20**: Silverpine Forest / Northern Barrens / Ghostlands
3. **20–25**: Hillsbrad Foothills / Ashenvale
4. **25–30**: Stonetalon Mountains / Southern Barrens
5. **30–35**: Northern Stranglethorn / Desolace
6. **35–40**: Cape of Stranglethorn / Western Plaguelands
7. **40–45**: Eastern Plaguelands / Dustwallow Marsh
8. **45–48**: Thousand Needles / Badlands
9. **48–52**: Tanaris / Searing Gorge
10. **52–54**: Un'Goro Crater / Burning Steppes
11. **54–58**: Felwood / Winterspring / Blasted Lands
12. **58–70 (Outland)**: Hellfire Peninsula -> Zangarmarsh -> Terokkar Forest -> Nagrand -> Netherstorm
13. **68–80 (Northrend)**: Borean Tundra / Howling Fjord -> Dragonblight -> Grizzly Hills -> Zul'Drak -> Sholazar Basin -> Icecrown
14. **80–85 (Cataclysm)**: Mount Hyjal -> Deepholm -> Uldum -> Twilight Highlands
15. **85–90 (Pandaria)**: The Jade Forest -> Valley of the Four Winds -> Kun-Lai Summit -> Townlong Steppes -> Dread Wastes

---

## Custom Behaviors in Profiles

Profiles frequently leverage powerful C# behaviors located in `Quest Behaviors\`. Examples:

- `<CustomBehavior File="FlyTo" DestName="Fly Destination" X="..." Y="..." Z="..." />`: Performs smooth 3D flight pathfinding bypassing ground obstacles.
- `<CustomBehavior File="InteractWith" MobId="12345" GossipOptions="1" WaitTime="1000" />`: Talks to NPCs, navigates gossip menus, or opens chests.
- `<CustomBehavior File="VehicleCheck" VehicleId="23456" ... />`: Controls siege engines, mounts, and quest-specific combat vehicles.
- `<CustomBehavior File="RunMacro" Macro="/cast RandomMount" NumOfTimes="1" />`: Executes client-side slash macros directly in WoW.
