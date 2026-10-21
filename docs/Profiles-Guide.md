# Profiles Guide: Creation, Custom Behaviors, Scripting & Plugins

Honorbuddy profiles are XML documents that drive high-level automation. Whether automating 1–90 leveling, gathering Ghost Iron Ore in Pandaria, or grinding reputation with the Klaxxi, profiles dictate what the character does, where it travels, and how it interacts with the world.

---

## 1. Profile Fundamentals & Types

Honorbuddy recognizes several distinct profile types depending on the active **BotBase**:

| Profile Type | Active BotBase | Primary Structure | Description |
|---|---|---|---|
| **Questing** | `Questing` | `<QuestOrder>` | Step-by-step quest pickup, completion, turn-in, and conditional scripting |
| **Gathering** | `Gatherbuddy2` | `<Hotspots>` | Waypoint loops for harvesting herbs and mining ore veins |
| **Grinding** | `Grind Bot` | `<GrindArea>` | Mob killing loops with target filtering and skinning/cloth looting |
| **Crafting** | `ProfessionBuddy` | `<Professionbuddy>` | Complex multi-stage crafting, auctioning, and vendor pipelines |

---

## 2. Profile Creation Tutorial ("How It's Done")

### A. Extracting In-Game IDs & Coordinates
To author a profile, you need in-game coordinates and IDs. Honorbuddy provides the **Developer Tools** window:
1. Start Honorbuddy and attach to WoW.
2. Click **Developer Tools** (or press the **Dev** button).
3. The window displays:
   - **Player Position**: `X`, `Y`, `Z` (e.g. `X="1234.56" Y="-4321.78" Z="15.20"`)
   - **Current Target / Mouseover**: `Entry` (NPC / Mob ID), `Guid`, and `Faction`
   - **Target Coordinates**: Exact position of the selected NPC or object
   - **Quest Log**: IDs of all accepted quests and current objective completion steps.

---

### B. Creating a Basic Grinding Profile

Grind profiles tell the bot to circle a set of waypoints and kill specific hostile mobs:

```xml
<HBProfile>
  <Name>[Grind] Dread Wastes - Dread Amber Scorpions</Name>
  <MinLevel>88</MinLevel>
  <MaxLevel>91</MaxLevel>
  <MinDurability>0.3</MinDurability>
  <UseMount>True</UseMount>

  <!-- Mobs to kill (by NPC Entry ID) -->
  <TargetMobs>
    <TargetMob Id="64234" /> <!-- Dread Amber Scorpion -->
  </TargetMobs>

  <!-- Vendors for repair and selling -->
  <Vendors>
    <Vendor Name="Supplier Rikkit" Entry="64555" Type="Repair" X="-542.12" Y="1245.89" Z="98.45" />
  </Vendors>

  <!-- Mailbox for mailing cloth/greens to bank alt -->
  <Mailboxes>
    <Mailbox X="-538.00" Y="1250.00" Z="98.00" />
  </Mailboxes>

  <!-- Circular waypoint patrol loop -->
  <GrindArea>
    <Hotspots>
      <Hotspot X="-650.12" Y="1350.45" Z="85.20" />
      <Hotspot X="-680.34" Y="1410.67" Z="84.10" />
      <Hotspot X="-720.56" Y="1460.89" Z="86.50" />
      <Hotspot X="-690.78" Y="1390.12" Z="85.80" />
    </Hotspots>
  </GrindArea>
</HBProfile>
```

---

### C. Creating a Full Questing Profile

Questing profiles are procedural programs executed within the `<QuestOrder>` block:

```xml
<HBProfile>
  <Name>[H - Quest] The Jade Forest - Tian Monastery Intro</Name>
  <MinLevel>85</MinLevel>
  <MaxLevel>91</MaxLevel>

  <!-- 1. Blacklisted dangerous terrain or elite mob camps -->
  <Blackspots>
    <Blackspot X="1520.40" Y="-2410.12" Z="35.60" Radius="25" />
  </Blackspots>

  <!-- 2. Townsfolk & Services -->
  <Vendors>
    <Vendor Name="Monk Lo" Entry="58700" Type="Repair" X="1480.12" Y="-2520.45" Z="42.10" />
  </Vendors>

  <!-- 3. Procedural Execution Sequence -->
  <QuestOrder>
    <!-- Pick up the introductory quest -->
    <PickUp QuestName="The Tian Training Grounds" QuestId="29981" GiverName="Master Shang Xi" GiverId="56801" X="1475.20" Y="-2515.10" Z="42.10" />

    <!-- Objective 1: Interact with 5 training dummies -->
    <While Condition="HasQuest(29981) &amp;&amp; !IsObjectiveComplete(1, 29981)">
      <CustomBehavior File="InteractWith" MobId="56855" QuestId="29981" CollectionDistance="50" Range="5" />
    </While>

    <!-- Objective 2: Defeat 8 Novice Trainees -->
    <While Condition="HasQuest(29981) &amp;&amp; !IsObjectiveComplete(2, 29981)">
      <!-- Move between spawn hotspots to locate enemies -->
      <CustomBehavior File="FlyTo" DestName="Novice Camp" X="1550.00" Y="-2480.00" Z="38.00" />
      <!-- Bot will automatically engage TargetMob matching Quest objective -->
    </While>

    <!-- Turn in quest once completed -->
    <TurnIn QuestName="The Tian Training Grounds" QuestId="29981" TurnInName="Master Shang Xi" TurnInId="56801" X="1475.20" Y="-2515.10" Z="42.10" />

    <!-- Chain smoothly into the next zone profile -->
    <CustomBehavior File="LoadProfile" ProfileName="[H - Quest] 86-87 Valley of the Four Winds.xml" />
  </QuestOrder>
</HBProfile>
```

---

## 3. Running Custom Code & In-Line Scripting

Honorbuddy provides multiple mechanisms to execute in-line C# and client-side Lua code directly within XML profiles.

### A. Running In-Line C# (`RunCode`)
The `RunCode` custom behavior executes C# code dynamically compiled by the bot:

```xml
<!-- Print a colored message in the Honorbuddy log -->
<CustomBehavior File="RunCode" Code="Logging.Write(System.Windows.Media.Colors.Green, &quot;[Profile] Reached Level {0}!&quot;, StyxWoW.Me.Level);" />

<!-- Force character to face a specific angle or target -->
<CustomBehavior File="RunCode" Code="StyxWoW.Me.SetFacing(3.14f);" />

<!-- Check bag space and post status -->
<CustomBehavior File="RunCode" Code="Logging.Write(&quot;Free bag slots remaining: {0}&quot;, StyxWoW.Me.FreeNormalBagSlots);" />
```

### B. Executing Client Lua (`RunMacro` / `RunLua`)
To trigger in-game macro commands or World of Warcraft Lua API functions:

```xml
<!-- Execute an in-game macro with a wait delay -->
<CustomBehavior File="RunMacro" Macro="/use Hearthstone" NumOfTimes="1" WaitTime="11000" />

<!-- Dismiss current pet or summon a specific vanity pet -->
<CustomBehavior File="RunMacro" Macro="/run DismissCompanion(&quot;CRITTER&quot;)" />

<!-- Skip in-game cinematic cutscenes -->
<CustomBehavior File="Misc\RunLua" Lua="CinematicFrame_CancelCinematic(); MovieFrame_PlayMovie();" />
```

---

## 4. Comprehensive Custom Behaviors Reference

Custom Behaviors (`Quest Behaviors\`) extend XML profiles with specialized automation mechanics:

### 1. `InteractWith`
The most versatile behavior in Honorbuddy. Handles talking to NPCs, clicking quest objects (chests, cages, books), opening doors, navigating gossip dialogs, and buying items.

```xml
<CustomBehavior File="InteractWith" 
                MobId="12345" 
                QuestId="29100" 
                GossipOptions="1" 
                InteractByLooting="True" 
                CollectionDistance="60" 
                Range="4.5" 
                WaitTime="1500" 
                TerminateWhen="IsObjectiveComplete(1, 29100)" />
```

| Parameter | Type | Description |
|---|---|---|
| `MobId` | `Integer` | Entry ID of the NPC or GameObject to interact with |
| `QuestId` | `Integer` | Associates behavior with a quest; halts when objective is met |
| `GossipOptions` | `Comma-list` | 1-based index of gossip option(s) to click (e.g. `1` or `1,2`) |
| `InteractByLooting` | `Boolean` | Waits for loot window to open and loots all items |
| `CollectionDistance`| `Double` | Search radius in yards around the player |
| `Range` | `Double` | Distance to stand from target before clicking (default ~4.5 yards) |
| `WaitTime` | `Integer` | Milliseconds to wait after interaction completes |
| `TerminateWhen` | `C# Expression`| Condition evaluated every tick; terminates behavior if `true` |

---

### 2. `FlyTo`
Provides smooth 3D flight pathfinding, automatically mounting a flying mount, lifting off, and flying straight to waypoints above ground terrain.

```xml
<CustomBehavior File="FlyTo" DestName="Temple of the White Tiger" X="3520.10" Y="1204.50" Z="450.20" AllowedVariance="5" Land="True" />
```
- `Land="True"`: Tells the flight engine to descend and land on the ground upon arrival.
- `AllowedVariance="5"`: Acceptable radius tolerance in yards.

---

### 3. `CollectThings`
Automatically wanders a designated radius, detects all ground-spawn quest objects (e.g. herbs, eggs, relic fragments), paths to each, and loots until target count is met.

```xml
<CustomBehavior File="CollectThings" 
                ObjectId="19234" 
                QuestId="30112" 
                CollectItemId="75412" 
                CollectItemCount="10" 
                HuntingGroundRadius="75">
  <Hotspot X="-124.50" Y="890.10" Z="12.40" />
</CustomBehavior>
```

---

### 4. `VehicleCheck` / `VehicleBehavior`
Used for vehicle quests (controlling catapults, riding bombers, steering tanks, or flying kites):

```xml
<CustomBehavior File="Vehicles\VehicleMover" 
                VehicleId="54123" 
                QuestId="29500" 
                CastSpellId="105412" 
                AttackButton="1" 
                VehicleFacing="3.14">
  <Hopspots>
    <Hotspot X="100.0" Y="200.0" Z="50.0" />
  </Hopspots>
</CustomBehavior>
```

---

### 5. `UserSettings`
Temporarily modifies core bot settings (such as disabling mounts in tight corridors or adjusting pull distance) and restores them when exiting the block:

```xml
<CustomBehavior File="UserSettings" UseMount="False" LootMobs="False" HarvestHerbs="False" />
<!-- Complex indoor cavern quest steps -->
<CustomBehavior File="UserSettings" Stop="True" /> <!-- Restores previous settings -->
```

---

### 6. `WaitTimer`
Pauses profile execution for a specified duration (useful for waiting out NPC roleplay dialogues, elevators, or debuffs):

```xml
<CustomBehavior File="WaitTimer" WaitTime="15000" VariantTime="2000" GoalText="Waiting for NPC speech" />
```

---

### 7. `LoadProfile`
Connects multiple profiles together into seamless, unattended leveling chains:

```xml
<CustomBehavior File="LoadProfile" ProfileName="[H - Quest] 87-88 Kun-Lai Summit.xml" RememberProfile="True" />
```

---

## 5. Conditional Expressions & XML Escaping

Honorbuddy profiles support conditional expressions in `<If>` and `<While>` tags.

### Mandatory XML Escaping Rules

Because profiles are XML files, relational and logical operators **must** be XML-escaped:

| Operator | Invalid in XML | Mandatory XML Escape | Example Usage |
|---|---|---|---|
| Logical AND | `&&` | `&amp;&amp;` | `Condition="HasQuest(123) &amp;&amp; !IsQuestCompleted(123)"` |
| Less Than | `<` | `&lt;` | `Condition="Me.Level &lt; 90"` |
| Greater Than | `>` | `&gt;` | `Condition="Me.Level &gt;= 85"` |
| Less Than or Equal | `<=` | `&lt;=` | `Condition="GetItemCount(76085) &lt;= 5"` |
| Quotes inside attributes| `"` | `&quot;` | `Condition="Me.Class == &quot;Warrior&quot;"` |

### Common Built-In Conditions

```xml
<!-- Check character level -->
<If Condition="Me.Level &gt;= 85 &amp;&amp; Me.Level &lt; 86">

<!-- Check if player has accepted a quest -->
<If Condition="HasQuest(30100)">

<!-- Check if quest is in log and NOT finished -->
<If Condition="HasQuest(30100) &amp;&amp; !IsQuestCompleted(30100)">

<!-- Check specific quest objective step (1 to 4) -->
<While Condition="HasQuest(30100) &amp;&amp; !IsObjectiveComplete(1, 30100)">

<!-- Check inventory item count -->
<If Condition="GetItemCount(72988) &gt;= 10">

<!-- Check faction -->
<If Condition="Me.IsHorde">
<If Condition="Me.IsAlliance">

<!-- Check profession skill level -->
<If Condition="Me.GetSkill(SkillLine.Mining).CurrentValue &lt; 600">
```

---

## 6. Essential Plugins for Profile Authors & Botting

The `Plugins\` directory contains powerful companion plugins that enhance profile creation and execution:

### Profile Creation & Waypoint Recording
1. **`EclipseProfileBuilder` / `EclipseProfileCreator`**:
   - In-game GUI recorder.
   - Automatically records quest pickups, objective locations, and turn-ins as you play manually, generating clean XML directly into a profile file.
2. **`Zaprecorder2`**:
   - The gold standard for gathering and grinding routes.
   - Records your character's exact flight or ground path and exports formatted `<Hotspot X="..." Y="..." Z="..." />` clusters.
3. **`LogicCreator`**:
   - Visual drag-and-drop tool for generating complex `<If>`, `<While>`, and `<CustomBehavior>` logic trees.

### Safety & Automation Plugins
4. **`MrItemRemover2`**:
   - Automated inventory hygiene.
   - Automatically vendors gray junk, disenchants greens, deletes specified quest byproduct items, and deposits trade goods into guild or character banks.
5. **`Anti Drown`**:
   - Monitors breath meter while swimming or gathering underwater.
   - Automatically surfaces the character when breath drops below 30%, preventing drowning deaths.
6. **`ObjectGatherer`**:
   - Dynamically scans memory for harvestable quest items, chests, and interactables within collection distance without requiring exact coordinates.
7. **`RepairAnywhere`**:
   - When durability falls below threshold, automatically summons a repair mount (e.g. *Traveler's Tundra Mammoth* or *Grand Expedition Yak*) or drops Jeeves, repairs gear, and resumes the profile without walking to town.
8. **`AutoDecline` / `declineAll`**:
   - Automatically blocks and declines random trade requests, duel challenges, and guild invites from other players, preventing bot interruptions.
9. **`Potion of Luck` / `Refreshment`**:
   - Automatically keeps buffs active (e.g. Potion of Luck for bonus loot in Pandaria, flasks, and food buffs).
