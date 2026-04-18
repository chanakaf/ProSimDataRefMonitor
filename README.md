# ProSim DataRef Monitor

A lightweight Windows desktop tool for monitoring ProSim737 DataRefs in real time. Connect to a running ProSim instance, add the DataRefs you care about, and watch their values update live.

![ProSim DataRef Monitor](prosim.ico)

---

## Requirements

| Requirement | Details |
|---|---|
| Operating System | Windows 10 or 11 |
| .NET Runtime | [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48) |
| ProSim737 | A running ProSim737 instance (local or network) |

.NET Framework 4.8 is included with Windows 10 (May 2019 Update and later) and Windows 11. If you are on an older build, download and install it from the link above before running the app.

---

## Installation

No installer is required. The app is a single self-contained folder.

1. Download or copy the `ProSim Data Monitor EXE` folder to any location on your PC (e.g. `C:\Tools\ProSim Data Monitor EXE`).
2. The folder should contain:
   - `ProSimDataRefMonitor.exe`
   - `ProSimSDK.dll`
   - `prosim.ico`
3. Double-click `ProSimDataRefMonitor.exe` to launch.

> **Note:** Keep all files in the same folder. The app will not start if `ProSimSDK.dll` is missing.

---

## Quick Start

### 1 — Connect to ProSim

Enter the IP address of the machine running ProSim737 in the **Host** field.

- If ProSim is running on the **same PC**, leave it as `127.0.0.1`.
- If ProSim is running on **another PC** on your network, enter that machine's IP address (e.g. `192.168.1.50`).

Click **Connect**. The status bar at the bottom will turn green and show *Connected* when the connection is successful.

Click **Disconnect** at any time to close the connection.

### 2 — Add DataRefs

Use the toolbar buttons to build your list of DataRefs to monitor:

| Button | What it does |
|---|---|
| **+ Add** | Adds a blank row. Start typing a DataRef name — autocomplete suggestions will appear based on the built-in catalog. Press Enter or click away to confirm. |
| **− Remove** | Removes the selected row(s). You can select multiple rows with Shift+Click or Ctrl+Click. |
| **Paste Lines** | Reads the clipboard and adds one DataRef per line. Useful for pasting a list copied from documentation or a text file. You can also press Ctrl+V while editing a cell to paste multiple lines at once. |

When you type a known DataRef name, the **Type** and **Unit** columns are filled in automatically from the catalog.

### 3 — Start Monitoring

Click **▶ Start** to begin receiving live values. The **Value** column will update in real time as ProSim sends data.

Click **■ Stop** to stop receiving updates without disconnecting.

---

## DataRef Catalog

The app ships with an optional catalog file `ProSimDataRefs.xml` that enables autocomplete and auto-fills the Type and Unit columns. Place this file in the same folder as the `.exe`.

If the file is not present the app still works — you can type DataRef names manually, but there will be no autocomplete or metadata.

The catalog format is:

```xml
<DataRefs>
  <DataRef name="aircraft.altitude.indicated" type="double" unit="ft" canRead="true" canWrite="false">
    <Description>Indicated altitude from the altimeter</Description>
  </DataRef>
  <DataRef name="systems.gear.nose.down" type="bool" unit="" canRead="true" canWrite="true">
    <Description>True when the nose gear is down and locked</Description>
  </DataRef>
</DataRefs>
```

---

## Session Memory

The app automatically saves your last-used Host address and DataRef list when you close it, and restores them the next time you open it. Nothing extra needs to be configured.

---

## Building from Source

### Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community edition is free) with the **.NET desktop development** workload installed
- `ProSimSDK.dll` placed in the project root folder next to the `.csproj`

### Steps

1. Clone or download the repository.
2. Copy `ProSimSDK.dll` into the project root (same folder as `ProSimDataRefMonitor.csproj`).
3. Open `ProSimDataRefMonitor.sln` in Visual Studio.
4. Select the **Release** configuration from the toolbar dropdown.
5. Press `Ctrl+Shift+B` to build.

The compiled output will be placed in the `ProSim Data Monitor EXE` folder next to the `.csproj`.

### Project structure

```
ProSimDataRefMonitor/
├── ProSimDataRefMonitor.csproj
├── ProSimSDK.dll                  ← not included, bring your own
├── prosim.ico
├── ProSimDataRefs.xml             ← optional DataRef catalog
├── Program.cs
├── MainForm.cs
├── MainForm.Designer.cs
├── ProSimContracts.cs
├── UITheme.cs
└── ProSim Data Monitor EXE/       ← build output goes here
    ├── ProSimDataRefMonitor.exe
    └── ProSimSDK.dll
```

---

## Troubleshooting

**The app won't start**
Make sure `ProSimSDK.dll` is in the same folder as the `.exe` and that .NET Framework 4.8 is installed.

**"Connection error" when clicking Connect**
- Confirm ProSim737 is running.
- Check the IP address is correct.
- If connecting over a network, make sure Windows Firewall on the ProSim machine is not blocking the ProSim SDK port.

**No autocomplete when typing DataRef names**
The `ProSimDataRefs.xml` catalog file is missing from the application folder. The app still works without it — type the full DataRef name manually.

**Values are not updating after clicking Start**
Make sure you clicked **Connect** first and the status bar shows *Connected*. Then click **▶ Start**.

---

## License

This project is provided as-is for personal and hobbyist use with ProSim737. It is not affiliated with or endorsed by ProSim.
