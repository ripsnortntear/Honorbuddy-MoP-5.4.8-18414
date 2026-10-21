# Buddy_Auth Local Authentication Server

`Buddy_Auth.exe` is the self-hosted local authentication and service provider that allows Honorbuddy to operate fully offline without connecting to Bossland GmbH's decommissioned remote servers.

---

## Technical Overview

When Honorbuddy launches, it contacts the authentication server over Windows Communication Foundation (WCF) using the high-performance `NetTcpBinding` protocol. Instead of external telemetry and license validation, `Buddy_Auth.exe` handles:

1. **License Validation**: Authenticates login requests with arbitrary keys (e.g. `12345678901234567890`).
2. **Memory Offsets Delivery**: Feeds MoP 5.4.8 (Build 18414) memory addresses and function pointers directly to Honorbuddy at runtime.
3. **Navmesh Server**: Serves navigation mesh (`.mesh` and `.tile`) files from local disk to the bot's pathfinding engine on demand.
4. **Version Negotiation**: Validates client compatibility against WoW build 18414.

### Network Architecture & WCF Endpoints

| Endpoint | Protocol | Port | Function |
|---|---|---|---|
| `net.tcp://localhost:5031/AuthService.svc` | WCF NetTcp | `5031` | Primary authentication, session creation, offset distribution |
| `net.tcp://localhost:5034/AuthService.svc` | WCF NetTcp | `5034` | Mesh and tile stream transport |

### Internal Opcodes Handled

| Opcode | Handler | Description |
|---|---|---|
| `1` | `Login` | Validates serial key, registers session token, unlocks bot capabilities |
| `2` | `Logout` | Cleans up session state upon Honorbuddy shutdown |
| `3` | `SendOffsets` | Transmits 32-bit MoP 18414 memory offsets (`CGObjectManager`, `Camera`, `CurMgr`, `TargetGuid`) |
| `4` | `SendMeshFile` | Streams requested `.mesh` files from the local storage folder |
| `5` | `SendTileFile` | Streams detailed tile geometry for active navigation grids |
| `6` | `CheckVersion` | Returns compatible version string for build 18414 |
| `7` | `FlyTo` | Pathfinding waypoint coordinate resolution |

---

## Directory & File Structure

The auth server lives in the `Auth` directory:

```
<Honorbuddy-Directory>\Auth\
├── Buddy_Auth.exe        # Main auth server executable (.NET 4.0/4.5)
├── nssm.exe              # Non-Sucking Service Manager (Windows Service runner)
├── Settings.xml          # Configuration file for ports, IP, and mesh directory
├── Meshes\               # Local navmesh repository
│   ├── 0\                # Eastern Kingdoms mesh files
│   ├── 1\                # Kalimdor mesh files
│   ├── 530\              # Outland mesh files
│   ├── 571\              # Northrend mesh files
│   └── 870\              # Pandaria mesh files
├── stdout.log            # Service output stream (when run via NSSM)
└── stderr.log            # Service error stream (when run via NSSM)
```

### Configuration (`Settings.xml`)

Located at `Auth/Settings.xml`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Settings>
  <ServerIP>127.0.0.1</ServerIP>
  <ServerPort>5031</ServerPort>
  <MeshPort>5034</MeshPort>
  <MeshesPath>Meshes</MeshesPath>
  <LogLevel>Info</LogLevel>
</Settings>
```

> [!NOTE]
> Ensure `<ServerIP>` remains `127.0.0.1` unless running the auth server on a separate local network server for multi-box setups.

---

## Running as a Background Windows Service (NSSM)

To ensure the auth server is always running silently in the background, starts automatically on Windows boot, and restarts if ever terminated, use **NSSM (Non-Sucking Service Manager)**.

### Recommended Method: NSSM GUI Installer (`nssm.exe install`)

The GUI installer is the most reliable way to configure the service.

1. Open **Command Prompt** as **Administrator** and navigate to your `Auth` directory:
   ```cmd
   cd /d "<Path-To-Honorbuddy>\Auth"
   nssm.exe install
   ```
2. In the NSSM configuration window:
   - **Application tab**:
     - **Path**: `<Path-To-Honorbuddy>\Auth\Buddy_Auth.exe`
     - **Startup directory**: `<Path-To-Honorbuddy>\Auth`
     - **Service name**: `BuddyAuth`
   - **Log on tab** *(CRITICAL)*:
     - > [!IMPORTANT]
     - > Check the box: **"Allow service to interact with desktop"**
     - > *(This is **NOT** checked by default. You **MUST** check it. This sets `SERVICE_INTERACTIVE_PROCESS`, allocating a console screen buffer so `Console.Clear()` and `Console.ReadKey()` will not throw invalid handle exceptions).*
   - **I/O tab** *(Optional but recommended)*:
     - Output (stdout): `<Path-To-Honorbuddy>\Auth\stdout.log`
     - Error (stderr): `<Path-To-Honorbuddy>\Auth\stderr.log`
3. Click **Install service**.
4. Configure logging and automatic startup via CLI:
   ```cmd
   nssm.exe set BuddyAuth AppDirectory "<Path-To-Honorbuddy>\Auth"
   nssm.exe set BuddyAuth AppStdout "<Path-To-Honorbuddy>\Auth\stdout.log"
   nssm.exe set BuddyAuth AppStderr "<Path-To-Honorbuddy>\Auth\stderr.log"
   nssm.exe set BuddyAuth Start SERVICE_AUTO_START
   nssm.exe start BuddyAuth
   ```
5. Verify the service is running:
   ```cmd
   nssm.exe status BuddyAuth
   ```
   Output: `SERVICE_RUNNING`

---

### Command-Line NSSM Installation (CLI)

If installing strictly via command line, you must explicitly set the service type to `SERVICE_INTERACTIVE_PROCESS` along with stdout/stderr redirection:

```cmd
cd /d "<Path-To-Honorbuddy>\Auth"

:: 1. Install service
nssm.exe install BuddyAuth "<Path-To-Honorbuddy>\Auth\Buddy_Auth.exe"
nssm.exe set BuddyAuth AppDirectory "<Path-To-Honorbuddy>\Auth"
nssm.exe set BuddyAuth Type SERVICE_INTERACTIVE_PROCESS
nssm.exe set BuddyAuth AppStdout "<Path-To-Honorbuddy>\Auth\stdout.log"
nssm.exe set BuddyAuth AppStderr "<Path-To-Honorbuddy>\Auth\stderr.log"
nssm.exe set BuddyAuth Start SERVICE_AUTO_START

:: 2. Start service
nssm.exe start BuddyAuth
```

---

### Service Management Commands

| Action | Command |
|---|---|
| **Check Status** | `.\nssm.exe status BuddyAuth` |
| **Start Service** | `.\nssm.exe start BuddyAuth` |
| **Stop Service** | `.\nssm.exe stop BuddyAuth` |
| **Restart Service** | `.\nssm.exe restart BuddyAuth` |
| **Edit via GUI** | `.\nssm.exe edit BuddyAuth` |
| **Uninstall Service** | `.\nssm.exe remove BuddyAuth confirm` |

---

## Verification & Troubleshooting

### 1. Verify Ports are Listening
Verify that ports `5031` and `5034` are active:
```powershell
Get-NetTCPConnection -LocalPort 5031, 5034 | Select-Object LocalAddress, LocalPort, State, OwningProcess
```
You should see both ports in the `Listen` state bound to `127.0.0.1` or `0.0.0.0`.

### 2. Verify Output Logs
Inspect `<Path-To-Honorbuddy>\Auth\stdout.log` to confirm startup:
```
[AuthServer] Service initialized.
[AuthServer] Listening on net.tcp://127.0.0.1:5031/AuthService.svc
[MeshServer] Listening on net.tcp://127.0.0.1:5034/AuthService.svc
[AuthServer] Ready for incoming Honorbuddy connections.
```

### 3. "System.Net.WebException" on Honorbuddy Launch
When Honorbuddy starts, you may see:
```
Logging in...
System.Net.WebException: The remote server returned an error: (404) Not Found.
```
This is expected behavior. Honorbuddy first attempts to query the legacy Bossland web server for announcements and tripwire signatures. When this HTTP check times out or returns 404, Honorbuddy cleanly falls back to the WCF NetTcp local connection (`net.tcp://localhost:5031/AuthService.svc`) and logs in successfully.

### 4. Firewall Settings
If Honorbuddy cannot communicate with `Buddy_Auth.exe`, ensure Windows Defender Firewall permits local loopback traffic on TCP ports 5031 and 5034.

> [!IMPORTANT]
> Modifying firewall rules requires administrative privileges. You **MUST** run **PowerShell as Administrator** (or **Command Prompt as Administrator**).

**Option A: PowerShell (Run as Administrator)**
```powershell
New-NetFirewallRule -DisplayName "Buddy Auth Service" -Direction Inbound -LocalPort 5031,5034 -Protocol TCP -Action Allow
```

**Option B: Command Prompt (Run as Administrator)**
```cmd
netsh advfirewall firewall add rule name="Buddy Auth Service" dir=in action=allow protocol=TCP localport=5031,5034
```
