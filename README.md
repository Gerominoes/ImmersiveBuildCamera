# Immersive Build Camera

Immersive Build Camera is a lightweight Valheim mod that improves the building experience by giving the player a closer, more focused camera while using build tools.

The goal is simple: make building feel more precise and immersive without turning the game into a detached freecam editor. You stay grounded in the normal Valheim experience, but with better camera positioning, slower movement control, and optional shoulder peeking for tight placements.

## What it does

Immersive Build Camera lets you toggle into a dedicated build camera mode while using Valheim's build tools. When active, it adjusts the camera behavior to make object placement easier, especially when working indoors, close to walls, or around detailed structures.

It is designed for players who want better visibility and control while building, but still want the game to feel like Valheim.

## Features

- Toggleable immersive build camera while using build tools.
- Configurable camera toggle key.
- Slow precision movement for careful placement.
- Configurable precision movement toggle key.
- Option to start precision movement enabled by default.
- Left and right shoulder peek controls.
- Optional shoulder peek toggle mode.
- Configurable shoulder peek offset, distance, and collision radius.
- Local player model hiding while the immersive camera is active.
- Automatic local player visibility restore when the mod unloads.

## Default controls

| Action | Default key |
| --- | --- |
| Toggle immersive build camera | `LeftAlt` |
| Toggle precision movement | `LeftControl` |
| Peek left | `Q` |
| Peek right | `E` |

Controls can be changed in the generated BepInEx config file.

## Installation

### Mod manager

Install the mod through your preferred Valheim mod manager if it is available on Nexus Mods or Thunderstore.

### Manual installation

1. Install BepInEx for Valheim.
2. Download the latest release ZIP from the GitHub Releases page.
3. Extract `ImmersiveBuildCamera.dll`.
4. Place the DLL in:

   ```text
   Valheim/BepInEx/plugins/ImmersiveBuildCamera/
   ```

5. Launch Valheim once to generate the config file.
6. Adjust the config if needed.

## Configuration

After launching the game once, BepInEx generates a config file for the mod. The config lets you adjust input, camera, shoulder peek, movement, and local visibility behavior.

Common settings include:

| Setting | Purpose |
| --- | --- |
| `ToggleCameraKey` | Changes the key used to toggle immersive build camera. |
| `TogglePrecisionMovementKey` | Changes the key used to toggle slow precision movement. |
| `LeftShoulderKey` | Changes the key used for left shoulder peek. |
| `RightShoulderKey` | Changes the key used for right shoulder peek. |
| `BuildFov` | Changes the field of view while immersive build camera is active. |
| `NearClip` | Adjusts the near clipping plane for close camera positioning. |
| `ShoulderOffsetX` | Controls how far left or right the shoulder peek moves. |
| `ShoulderOffsetY` | Controls the vertical shoulder peek offset. |
| `ShoulderDistance` | Controls the backward distance of the shoulder peek camera. |
| `CollisionRadius` | Controls how aggressively shoulder peek avoids clipping into objects. |
| `ToggleShoulderPeek` | Switches shoulder peek between hold mode and toggle mode. |
| `EnablePrecisionMovement` | Enables or disables precision movement support. |
| `PrecisionMovementDefaultOn` | Controls whether precision movement starts enabled when the camera turns on. |
| `PrecisionMoveMultiplier` | Controls how slow precision movement feels. Lower values are slower. |
| `HideLocalPlayerWhenImmersive` | Hides the local player model while immersive build camera is active. |

## Compatibility

Immersive Build Camera is focused on camera and movement behavior while building. It should work best with vanilla-style building controls and minimal camera overhaul mods.

Potential compatibility issues may occur with mods that also patch:

- Player camera behavior.
- Build mode input.
- Player movement speed.
- Local player rendering or visibility.

If another camera mod changes the same behavior, load order and patch conflicts may affect the result.

## Requirements

- Valheim
- BepInEx for Valheim

## Versioning

This project uses GitHub releases and version tags so users can track changes over time.

Recommended tag format:

```text
v0.2.3
```

The in-game plugin version should match the GitHub release tag whenever possible.

## Development

This mod targets `.NET Framework 4.8` and uses BepInEx with Harmony patches.

The project file currently expects local Valheim and BepInEx paths. If you clone the project, update those paths in the `.csproj` file to match your own development environment before building.

Typical release build command:

```powershell
dotnet build -c Release
```

## Scope

Immersive Build Camera is intentionally narrow in scope. It is meant to improve building comfort, visibility, and precision. It is not intended to become a full freecam, creative mode, or detached construction editor.

## License

No license has been added yet. Until a license is added, all rights are reserved by default.
