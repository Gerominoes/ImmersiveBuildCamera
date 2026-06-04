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
    public const string PluginVersion = "0.1.0";

    internal static ManualLogSource Log = null!;

    internal static ConfigEntry<KeyboardShortcut> HoldInspectKey = null!;
    internal static ConfigEntry<float> BuildFov = null!;
    internal static ConfigEntry<float> NearClip = null!;

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

        _harmony = new Harmony(PluginGuid);
        _harmony.PatchAll();

        Log.LogInfo($"{PluginName} loaded.");
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}