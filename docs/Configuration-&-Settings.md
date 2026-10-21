# Configuration & Settings Guide

Honorbuddy's behavior can be customized globally, per realm, and per character. Settings are stored as human-readable XML files inside the `Settings\` directory.

---

## Settings Hierarchy

```
<Honorbuddy-Directory>\Settings\
├── HonorbuddySettings.xml               # Global bot-wide defaults
└── [RealmName]\                         # E.g. "Stormrage" or "Localhost"
    └── [CharacterName]\                 # E.g. "Warlord"
        ├── CharacterSettings.xml        # Character-level overrides
        ├── QuestingSettings.xml         # Questing-specific behavior
        ├── GatherbuddySettings.xml      # Gathering thresholds & mount logic
        ├── Singular\                    # Singular combat routine settings
        └── Demonic\                     # Demonic combat routine settings
```

When Honorbuddy accesses a setting, it first checks the active character's directory. If no override exists, it falls back to the global default in `HonorbuddySettings.xml`.

---

## Key Global Configuration (`HonorbuddySettings.xml`)

Located at `Settings/HonorbuddySettings.xml`:

| Setting | Type | Default | Description |
|---|---|---|---|
| `KillBetweenHotspots` | `Boolean` | `True` | If `True`, attacks hostile mobs that aggro or block the path between waypoints. Set to `False` for stealth or pure speed. |
| `FindVendorsAutomatically`| `Boolean` | `True` | When bags are full or durability is low, queries the built-in database for nearby friendly repair/sell vendors if not explicitly declared in the profile. |
| `UseMount` | `Boolean` | `True` | Allows the bot to summon ground and flying mounts. |
| `MountName` | `String` | *(Empty)* | Name of the specific ground mount to summon (e.g. `Swift Brown Steed`). If left blank, chooses automatically. |
| `FlyingMountName` | `String` | *(Empty)* | Name of the flying mount to summon (e.g. `Red Flying Cloud` or `Golden Riding Serpent`). |
| `FoodName` | `String` | *(Empty)* | Consumable item used when character health drops below rest threshold. |
| `DrinkName` | `String` | *(Empty)* | Consumable item used when character mana drops below rest threshold. |
| `LootMobs` | `Boolean` | `True` | Enables automated looting of killed enemies. |
| `SkinMobs` | `Boolean` | `False` | Enables skinning of harvestable beasts (requires Skinning knife and profession). |
| `HarvestHerbs` | `Boolean` | `True` | Automatically detours to gather herbs if Herbalism is learned. |
| `HarvestMinerals` | `Boolean` | `True` | Automatically detours to mine ore veins if Mining is learned. |
| `MinDurability` | `Double` | `0.30` | Triggers a town visit for gear repairs when any item drops below this percentage (0.30 = 30%). |
| `MinFreeBagSlots` | `Integer` | `2` | Triggers a vendor run to sell items or mail materials when free bag space drops to or below this value. |

---

## Performance & Engine Tuning

### Frame Lock (TPS)
Honorbuddy interacts with World of Warcraft by hooking into the Direct3D presentation stream (`EndScene` / `Present`).

- **What is Frame Lock?**:
  When Frame Lock is enabled, Honorbuddy synchronizes its memory reads and writes to the exact render frame cycle of WoW. This prevents race conditions, memory corruption, and visual micro-stuttering.
- **Ticks Per Second (TPS)**:
  - Configurable via the bot's advanced settings or Developer Tools.
  - Recommended: **15 to 30 TPS**.
  - A higher TPS yields faster reaction times (e.g. for PvP kick routines) but increases CPU utilization.

### Incremental Assembly Caching & Startup Optimization
Honorbuddy features an automatic deterministic assembly caching engine in `Styx.Common.Compiler.CodeCompiler`:
- **Instant Launch**: On first launch, plugins, routines, and bots are compiled to `CompiledAssemblies\` with deterministic FNV-1a hashes. On subsequent launches, cached assemblies load directly from disk in milliseconds, preventing game stalls or D3D11 frame lock timeouts.
- **Automatic Recompilation**: If you edit or update any `.cs` source file within a plugin, combat routine, or quest behavior, Honorbuddy automatically detects the modified timestamp and recompiles only that specific component.

### Graphics Mode Selection (Direct3D 11 vs Direct3D 9)
- **Direct3D 11** (Recommended): Provides superior multi-threading and lower CPU overhead on modern Windows 10/11 graphics drivers.
- **Direct3D 9**: Use as a fallback if the D3D11 hook encounters third-party overlay conflicts (e.g. Discord overlay or RivaTuner).

---

## Character & Routine Settings

### Singular Routine Settings
When using Singular, press the **Routine Settings** button in the main window to open the GUI configuration:
- **Pull Distance**: Maximum range (in yards) at which to engage a target. Default is 30–35 yards for ranged classes, 15–20 yards for melee classes.
- **Rest Health %**: Percentage threshold (e.g., 50%) below which the character will sit down and consume food.
- **Rest Mana %**: Percentage threshold (e.g., 40%) below which the character will sit down and consume drink.
- **Combat Buffs**: Toggles for maintaining class buffs (e.g. *Mark of the Wild*, *Battle Shout*, *Arcane Brilliance*).

---

## Safely Backing Up Settings

To preserve your configured routes, hotkeys, and vendor settings across updates or fresh checkouts:
- Back up the entire `Settings\` directory.
- The `Settings\` folder contains only your local preferences and does not affect core bot logic or assembly compilation.
