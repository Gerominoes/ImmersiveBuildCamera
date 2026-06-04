using HarmonyLib;

namespace ImmersiveBuildCamera;

[HarmonyPatch(typeof(Player), nameof(Player.Update))]
internal static class PlayerUpdatePatch
{
    private static void Postfix(Player __instance)
    {
        BuildCameraState.Update(__instance);
    }
}