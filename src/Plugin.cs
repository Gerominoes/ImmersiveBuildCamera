using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ImmersiveBuildCamera;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInProcess("valheim.exe")]
public sealed class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "com.geronimo.valheim.immersivebuildcamera";
    public const string PluginName = "Immersive Build Camera";
    public const string PluginVersion = "0.2.3";

    internal static ManualLogSource Log = null!;

    internal static ConfigEntry<KeyCode> ToggleCameraKey = null!;
    internal static ConfigEntry<KeyCode> TogglePrecisionMovementKey = null!;
    internal static ConfigEntry<KeyCode> LeftShoulderKey = null!;
    internal static ConfigEntry<KeyCode> RightShoulderKey = null!;

    internal static ConfigEntry<float> BuildFov = null!;
    internal static ConfigEntry<float> NearClip = null!;

    internal static ConfigEntry<float> ShoulderOffsetX = null!;
    internal static ConfigEntry<float> ShoulderOffsetY = null!;
    internal static ConfigEntry<float> ShoulderDistance = null!;
    internal static ConfigEntry<float> CollisionRadius = null!;
    internal static ConfigEntry<bool> ToggleShoulderPeek = null!;

    internal static ConfigEntry<bool> EnablePrecisionMovement = null!;
    internal static ConfigEntry<bool> PrecisionMovementDefaultOn = null!;
    internal static ConfigEntry<float> PrecisionMoveMultiplier = null!;

    internal static ConfigEntry<bool> HideLocalPlayerWhenImmersive = null!;

    private Harmony _harmony = null!;

    private void Awake()
    {
        Log = Logger;

        ToggleCameraKey = Config.Bind(
            "Input",
            "ToggleCameraKey",
            KeyCode.LeftAlt,
            "Press this while using a build tool to toggle immersive build camera."
        );

        TogglePrecisionMovementKey = Config.Bind(
            "Input",
            "TogglePrecisionMovementKey",
            KeyCode.LeftControl,
            "Press this while immersive build camera is active to toggle slow precision movement."
        );

        LeftShoulderKey = Config.Bind(
            "Input",
            "LeftShoulderKey",
            KeyCode.Q,
            "Hold or press this while immersive build camera is active to peek left, depending on ToggleShoulderPeek."
        );

        RightShoulderKey = Config.Bind(
            "Input",
            "RightShoulderKey",
            KeyCode.E,
            "Hold or press this while immersive build camera is active to peek right, depending on ToggleShoulderPeek."
        );

        BuildFov = Config.Bind(
            "Camera",
            "BuildFov",
            68f,
            "Field of view while immersive build camera is active."
        );

        NearClip = Config.Bind(
            "Camera",
            "NearClip",
            0.04f,
            "Near clipping plane while immersive build camera is active."
        );

        ShoulderOffsetX = Config.Bind(
            "Shoulder Peek",
            "ShoulderOffsetX",
            0.75f,
            "Horizontal shoulder offset. Higher values make shoulder peek more useful but more likely to hit collision."
        );

        ShoulderOffsetY = Config.Bind(
            "Shoulder Peek",
            "ShoulderOffsetY",
            0.06f,
            "Vertical shoulder offset."
        );

        ShoulderDistance = Config.Bind(
            "Shoulder Peek",
            "ShoulderDistance",
            0.50f,
            "Backward shoulder camera distance."
        );

        CollisionRadius = Config.Bind(
            "Shoulder Peek",
            "CollisionRadius",
            0.10f,
            "Sphere radius used to prevent shoulder peek camera clipping into objects."
        );

        ToggleShoulderPeek = Config.Bind(
            "Shoulder Peek",
            "ToggleShoulderPeek",
            false,
            "If false, shoulder peek keys must be held. If true, shoulder peek keys toggle left, right, or centered."
        );

        EnablePrecisionMovement = Config.Bind(
            "Movement",
            "EnablePrecisionMovement",
            true,
            "Allow slow precision movement while immersive build camera is active."
        );

        PrecisionMovementDefaultOn = Config.Bind(
            "Movement",
            "PrecisionMovementDefaultOn",
            true,
            "Whether slow precision movement starts enabled whenever immersive build camera is toggled on."
        );

        PrecisionMoveMultiplier = Config.Bind(
            "Movement",
            "PrecisionMoveMultiplier",
            0.35f,
            "Movement input multiplier when precision movement is enabled. Lower means slower."
        );

        HideLocalPlayerWhenImmersive = Config.Bind(
            "Local Visibility",
            "HideLocalPlayerWhenImmersive",
            true,
            "Hide only the local player's renderers while immersive build camera is active and shoulder peek is not being used."
        );

        _harmony = new Harmony(PluginGuid);
        _harmony.PatchAll();

        PrecisionMovementPatches.Apply(_harmony);

        Log.LogInfo($"{PluginName} loaded.");
    }

    private void OnDestroy()
    {
        PlayerRendererVisibility.ForceVisible();
        _harmony?.UnpatchSelf();
    }
}