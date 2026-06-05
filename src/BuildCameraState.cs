using HarmonyLib;
using UnityEngine;

namespace ImmersiveBuildCamera;

internal static class BuildCameraState
{
    internal static bool Active { get; private set; }
    internal static bool PrecisionMovementActive { get; private set; }

    private static readonly System.Reflection.FieldInfo? RightItemField =
        AccessTools.Field(typeof(Humanoid), "m_rightItem");

    internal static void Update(Player player)
    {
        if (player == null || player != Player.m_localPlayer)
            return;

        bool canUseCamera = CanUseImmersiveCamera(player);

        if (!canUseCamera)
        {
            SetActive(false);
            return;
        }

        if (Input.GetKeyDown(Plugin.ToggleCameraKey.Value))
        {
            SetActive(!Active);
        }

        if (!Active)
            return;

        if (Plugin.EnablePrecisionMovement.Value &&
            Input.GetKeyDown(Plugin.TogglePrecisionMovementKey.Value))
        {
            SetPrecisionMovement(!PrecisionMovementActive);
        }
    }

    private static void SetActive(bool active)
    {
        if (Active == active)
            return;

        Active = active;

        if (active)
        {
            PrecisionMovementActive =
                Plugin.EnablePrecisionMovement.Value &&
                Plugin.PrecisionMovementDefaultOn.Value;
        }
        else
        {
            PrecisionMovementActive = false;
        }

        Plugin.Log.LogInfo(active
            ? $"Immersive build camera active. Precision movement: {(PrecisionMovementActive ? "on" : "off")}."
            : "Immersive build camera inactive.");
    }

    private static void SetPrecisionMovement(bool active)
    {
        if (PrecisionMovementActive == active)
            return;

        PrecisionMovementActive = active;

        Plugin.Log.LogInfo(active
            ? "Precision movement active."
            : "Precision movement inactive.");
    }

    private static bool CanUseImmersiveCamera(Player player)
    {
        if (!IsSafePlayerState(player))
            return false;

        if (!HasBuildTool(player))
            return false;

        return true;
    }

    private static bool HasBuildTool(Player player)
    {
        if (RightItemField == null)
        {
            Plugin.Log.LogWarning("Could not find Humanoid.m_rightItem.");
            return false;
        }

        ItemDrop.ItemData? rightItem =
            RightItemField.GetValue(player) as ItemDrop.ItemData;

        if (rightItem == null)
            return false;

        if (rightItem.m_shared == null)
            return false;

        return rightItem.m_shared.m_buildPieces != null;
    }

    private static bool IsSafePlayerState(Player player)
    {
        if (player.IsDead())
            return false;

        if (player.IsAttached())
            return false;

        if (player.IsSwimming())
            return false;

        if (InventoryGui.IsVisible())
            return false;

        if (Menu.IsVisible())
            return false;

        if (Minimap.IsOpen())
            return false;

        return true;
    }
}