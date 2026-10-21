# Installation & Setup Guide

This guide provides end-to-end instructions for installing, configuring, and launching Honorbuddy v2.5.11489.748 with World of Warcraft: Mists of Pandaria (5.4.8, Build 18414).

---

## 1. System Prerequisites

Before launching Honorbuddy, verify that all necessary Windows runtimes are installed:

1. **Microsoft .NET Framework**:
   - .NET Framework 4.5.1 or higher (4.8 recommended).
2. **Microsoft Visual C++ Redistributable Packages (x86)**:
   - **Crucial**: You must install the **x86 (32-bit)** versions, even on 64-bit Windows:
     - Visual C++ 2010 Redistributable (x86)
     - Visual C++ 2012 Redistributable (x86)
     - Visual C++ 2013 Redistributable (x86)
     - Visual C++ 2015–2022 Redistributable (x86)
3. **DirectX End-User Runtimes**:
   - June 2010 DirectX 9/11 Redistributable (`d3dx9_43.dll`, `d3dcompiler_43.dll`).

---

## 2. World of Warcraft Client Configuration

Honorbuddy attaches to the WoW client using process memory injection and Direct3D hook overlays. For this to succeed, the client must match the following settings:

### A. Strict 32-bit Client Requirement

> [!WARNING]
> Honorbuddy is compiled strictly as an x86 (32-bit) process and **cannot** attach to `WoW-64.exe`. You must run the 32-bit `WoW.exe`.

- Create a desktop shortcut to `WoW.exe` with the argument:
  ```cmd
  "D:\Games\World of Warcraft 5.4.8\WoW.exe" -noautolaunch64bit
  ```
- Alternatively, rename `WoW-64.exe` to `WoW-64.exe.bak` to prevent accidental 64-bit launches.

### B. Display & Video Settings

Open WoW, press `Esc` > **System** > **Graphics / Advanced**:

| Setting | Required Value | Notes |
|---|---|---|
| **Display Mode** | `Windowed` or `Windowed (Fullscreen)` | Exclusive Fullscreen blocks D3D overlay hooks |
| **Graphics API** | `Direct3D 11` (or `Direct3D 9`) | Configure in System > Advanced > Graphics API |
| **Max Foreground FPS** | `60` or your monitor refresh rate | Prevents unnecessary CPU/GPU spikes |
| **Max Background FPS** | `30` | Prevents bot latency when WoW is unfocused |

### C. In-Game Interface & Gameplay Settings

Open WoW, press `Esc` > **Interface**:

1. **Mouse**:
   - **Click-to-Move**: **ENABLED** (Required for pathfinding navigation and interaction).
2. **Controls**:
   - **Auto Loot**: **ENABLED** (Required for GatherBuddy, Questing, and DungeonBuddy looting).
3. **Camera**:
   - **Camera Following Style**: Set to `Never adjust camera` or `Only horizontally when moving`.

---

## 3. Directory Layout

The installation root should contain the following structure:

```
D:\Games\GH of HB\
├── Honorbuddy.exe             # Main bot executable
├── Honorbuddy.exe.config      # Assembly binding redirects & .NET runtime config
├── Styx.dll                   # Core engine & object manager API
├── TreeSharp.dll              # Behavior tree execution engine
├── GreyMagic.dll              # Memory reading/writing library
├── Auth\                      # Authentication server & NSSM
│   ├── Buddy_Auth.exe
│   ├── nssm.exe
│   └── Meshes\
├── Bots\                      # BotBases (Questing, DungeonBuddy, BGBuddy, etc.)
├── Routines\                  # Combat Routines (Singular, Demonic, Gladiator Suite)
├── Plugins\                   # Community & utility plugins
├── Quest Behaviors\           # Specialized behavior actions executed by XML profiles
├── Default Profiles\          # Curated profile library (1-90, Gathering, TBMC)
└── Settings\                  # Saved configuration per realm and character
```

---

## 4. First-Time Launch Sequence

Follow this sequence in order:

### Step 1: Ensure Auth Server is Running
- If running as an NSSM Service:
  ```powershell
  nssm status BuddyAuth
  ```
  Should report `SERVICE_RUNNING`.
- If running manually:
  Double-click `Auth\Buddy_Auth.exe` and leave the console window open.

### Step 2: Launch World of Warcraft
- Launch `WoW.exe` (32-bit).
- Log into your account, select your character, and enter the game world.

### Step 3: Launch Honorbuddy
- Right-click `Honorbuddy.exe` and select **Run as Administrator**.
- At the login prompt:
  - Key: `12345678901234567890` (or any 20-character key).
  - Region: Auto / Default.
- Click **Login**.

### Step 4: Initial Configuration
1. **Routine Selection**: A dialog will pop up listing available combat routines for your class and spec. Select your preferred routine (e.g., `Singular` or `Demonic`) and click **OK**.
2. **BotBase Selection**: On the main window dropdown, choose your desired bot mode:
   - Leveling / Questing: `Questing`
   - Gathering Herbs / Ore: `Gatherbuddy2`
   - Battlegrounds: `BGBuddy`
   - Dungeons: `DungeonBuddy`
   - Raiding / Assist mode: `Tyrael`
3. **Load Profile**:
   - Click **Load Profile** > **From File**.
   - Browse to `Default Profiles\1-90 Unified\` or `Default Profiles\Gathering\`.
   - Select the `.xml` profile matching your faction, zone, and level range.
4. **Start the Bot**:
   - Click the green **Start** button.
   - Honorbuddy will initialize navigation meshes, compile behaviors, and begin automation.

---

## 5. Routine Manager Button

You do **not** need to restart Honorbuddy to change combat routines.

- Stop the bot by clicking **Stop**.
- Click the **Routine Manager** button on the bottom control bar.
- Choose your new routine from the modal dialog and click **OK**.
- Click **Start** to resume automation under the new rotation.
