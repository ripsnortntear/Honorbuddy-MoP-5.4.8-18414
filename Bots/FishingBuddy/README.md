# FishingBuddy Bot for Honorbuddy v2.5.11489.748 PRIVATE SERVERS

FishingBuddy is a custom bot plugin for [Honorbuddy](https://www.thebuddyforum.com/honorbuddy/), designed to automate fishing tasks in  
World of Warcraft: Mists of Pandaria (WoW MoP 5.4.8 PRIVATE SERVERS) NOT CLASSIC/REMIX/LIVE.  
It handles casting lines, looting catches, managing gear, applying baits/lures, and navigating between fishing spots or pools.  
The bot is built using C# and integrates with Honorbuddy's API for seamless automation.

This repository contains the modified source code for FishingBuddy, with added features for conditional stopping based on caught items.

## Features

- **Automated Fishing**: Casts lines, detects bites, loots fish, and handles pool-based or hotspot fishing.
- **Gear and Consumable Management**: Automatically equips fishing gear, applies baits/lures, fillets fish, and consumes items.
- **Navigation**: Supports flying/ground movement, water walking, and pathing (circle or bounce).
- **Safety Checks**: Blacklists problematic pools, avoids lava, and handles failed casts.
- **UI Configuration**: Uses a PropertyGrid for easy settings adjustment via Honorbuddy's interface.
- **New: Stop on Item Caught**: Stops the bot when a specific item ID enters the inventory (e.g., stop after catching a rare fish).
- **New: Stop on Item Quantity**: Tracks and stops after catching a target quantity of a specific item ID (e.g., fish up 6 of item 6921).

## Recent Changes

The following features were added to enhance automation control:

### 1. Stop When Item Caught (Basic)
- **What**: Allows the bot to halt when it catches a specific item ID for the first time.
- **Files Modified**:
  - `FishingBuddySettings.cs`: Added `StopOnItemId` (uint) setting.
  - `Coroutines.Fishing.cs`: Added inventory check after looting to detect the item and stop the bot.
- **API Used**: `TreeRoot.Stop()` for global bot halting.

### 2. Stop on Item Quantity (Advanced)
- **What**: Extends the above to stop only after reaching a cumulative quantity of the item (e.g., 5 catches). Includes progress logging.
- **Files Modified**:
  - `FishingBuddySettings.cs`: Added `StopOnItemCount` (uint) setting.
  - `Coroutines.Fishing.cs`: Updated inventory check to sum stack counts across bags and compare against the target quantity.
- **Backward Compatibility**: If `StopOnItemCount` is 0, it behaves like the basic feature (stop on first catch).

No other files were altered, ensuring minimal disruption to existing functionality.

## Installation and Setup

1. **Prerequisites**:
   - Honorbuddy installed and configured for WoW Mists of Pandaria.
   - .NET Framework 4.5.1 or compatible.
   - Access to the FishingBuddy bot source. (This Repo)

2. **Install**:
   - Clone or download this repository.
   - Place the repo files in Honorbuddy's `Bots\FishingBuddy` directory.
   - Restart Honorbuddy and select FishingBuddy as the bot.

3. **Configure**:
   - Use Honorbuddy's Bot Config button to set the ItemID you want to catch.

## Usage

### Basic Operation
- Start the bot in Honorbuddy.
- It will begin fishing at configured hotspots or pools.
- Monitor logs for progress and errors.

### Using New Features

#### Stop When Item Caught
1. Open the FishingBuddy config form (gear icon in Honorbuddy).
2. In the PropertyGrid, expand "Fishing".
3. Set `Stop on Item ID` to the Wowhead item ID (e.g., 6291 for Raw Brilliant Smallfish). Set to 0 to disable.
4. Set `Stop on Item Count` to 0 (for stop-on-first-catch mode).
5. Save and start fishing. The bot will stop when the item is caught.

**Example Log**:  
  ```FishingBuddy: Stopping bot: Caught target item 'Raw Brilliant Smallfish' (ID: 6291). Bot stopping! Reason: Caught target item```

#### Stop on Item Quantity
1. Follow the same steps as above.
2. Set `Stop on Item Count` to your target number (e.g., 5).
3. The bot will log progress and stop only after reaching the quantity.

**Example Log**:  
    `FishingBuddy: Progress: Caught 3/5 of target item 'Raw Brilliant Smallfish' (ID: 6291).`  
    `FishingBuddy: Stopping bot: Caught 5/5 of target item 'Raw Brilliant Smallfish' (ID: 6291). Bot stopping! Reason: Caught target item quantity`  

- **Tips**:
  - Find item IDs on [Wowhead](https://www.wowhead.com/).
  - Counts are cumulative (includes pre-existing items in inventory).
  - If the item isn't caught via fishing, the feature won't trigger.
  - Disable by setting ID or count to 0.

### Troubleshooting
- **Bot Doesn't Stop**: Check logs for progress messages. Ensure the item ID is correct and fishable.
- **UI Not Showing**: Rebuild and reload the plugin. Settings auto-save to `FishingBuddy.xml`.
- **Errors**: Share logs for debugging. Ensure Honorbuddy API compatibility.

## Contributing

- Fork the repo and submit pull requests for improvements.
- Report issues via Issues.
- Tested on Honorbuddy v2.5.11489.748; may need updates for newer versions.

## License

This project is for educational purposes. Use at your own risk—automating WoW may violate Blizzard's Terms of Service.

## Credits

- Original FishingBuddy by the Honorbuddy community.
- Enhancements by [ripsnortntear](https://github.com/ripsnortntear) for item-based stopping features.
