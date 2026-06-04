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
    public const string PluginGuid = "com.melle.valheim.immersivebuildcamera";
    public const string PluginName = "Immersive Build Camera";
    public const string PluginVersion = "0.2.0";

    internal static ManualLogSource Log = null!;

    internal static ConfigEntry<KeyboardShortcut> HoldInspectKey = null!;
    internal static ConfigEntry<KeyboardShortcut> ShoulderPeekKey = null!;

    internal static ConfigEntry<float> BuildFov = null!;
    internal static ConfigEntry<float> NearClip = null!;

    internal static ConfigEntry<float> ShoulderOffsetX = null!;
    internal static ConfigEntry<float> ShoulderOffsetY = null!;
    internal static ConfigEntry<float> ShoulderDistance = null!;
    internal static ConfigEntry<float> CollisionRadius = null!;

    internal static ConfigEntry<bool> EnablePrecisionMovement = null!;
    internal static ConfigEntry<float> PrecisionMoveMultiplier = null!;

    private Harmony _harmony = null!;

    private void Awake()
    {
        Log = Logger;

        HoldInspectKey = Config.Bind(
            "Input",
            "HoldInspectKey",
            new KeyboardShortcut(KeyCode.LeftAlt),
            "Hold this while using a build tool to enter immersive build camera."
        );

        ShoulderPeekKey = Config.Bind(
            "Input",
            "ShoulderPeekKey",
            new KeyboardShortcut(KeyCode.C),
            "Hold this while immersive build camera is active to use shoulder peek. Change this if it conflicts with your controls."
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
            0.32f,
            "Horizontal shoulder offset. Positive is right, negative is left."
        );

        ShoulderOffsetY = Config.Bind(
            "Shoulder Peek",
            "ShoulderOffsetY",
            0.03f,
            "Vertical shoulder offset."
        );

        ShoulderDistance = Config.Bind(
            "Shoulder Peek",
            "ShoulderDistance",
            0.38f,
            "Small backward distance for shoulder peek."
        );

        CollisionRadius = Config.Bind(
            "Shoulder Peek",
            "CollisionRadius",
            0.12f,
            "Sphere radius used to prevent shoulder peek camera clipping into objects."
        );

        EnablePrecisionMovement = Config.Bind(
            "Movement",
            "EnablePrecisionMovement",
            true,
            "Reduce player movement speed while immersive build camera is active."
        );

        PrecisionMoveMultiplier = Config.Bind(
            "Movement",
            "PrecisionMoveMultiplier",
            0.35f,
            "Movement speed multiplier while immersive build camera is active. Lower means slower."
        );

        _harmony = new Harmony(PluginGuid);
        _harmony.PatchAll();

        PrecisionMovementPatches.Apply(_harmony);

        Log.LogInfo($"{PluginName} loaded.");
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}