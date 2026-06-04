namespace ImmersiveBuildCamera;

internal static class BuildCameraState
{
    internal static bool Active { get; private set; }

    internal static void Update(Player player)
    {
        Active = false;

        if (player == null)
            return;

        if (player != Player.m_localPlayer)
            return;

        if (!IsSafePlayerState(player))
            return;

        if (!HasBuildTool(player))
            return;

        if (!Plugin.HoldInspectKey.Value.IsPressed())
            return;

        Active = true;
    }

    private static bool HasBuildTool(Player player)
    {
        ItemDrop.ItemData rightItem = player.m_rightItem;

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