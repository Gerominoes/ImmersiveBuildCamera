using HarmonyLib;

namespace ImmersiveBuildCamera;

internal static class PrecisionMovementPatches
{
    internal static void Apply(Harmony harmony)
    {
        PatchSpeedFactor(harmony, "GetJogSpeedFactor");
        PatchSpeedFactor(harmony, "GetRunSpeedFactor");
    }

    private static void PatchSpeedFactor(Harmony harmony, string methodName)
    {
        System.Reflection.MethodInfo? original =
            AccessTools.Method(typeof(Character), methodName);

        System.Reflection.MethodInfo? postfix =
            AccessTools.Method(typeof(PrecisionMovementPatches), nameof(PostfixSpeedFactor));

        if (original == null)
        {
            Plugin.Log.LogWarning($"Could not find Character.{methodName}. Precision movement patch skipped for this method.");
            return;
        }

        if (postfix == null)
        {
            Plugin.Log.LogWarning("Could not find precision movement postfix method.");
            return;
        }

        harmony.Patch(original, postfix: new HarmonyMethod(postfix));
        Plugin.Log.LogInfo($"Patched Character.{methodName} for precision movement.");
    }

    private static void PostfixSpeedFactor(Character __instance, ref float __result)
    {
        if (!Plugin.EnablePrecisionMovement.Value)
            return;

        if (!BuildCameraState.Active)
            return;

        if (__instance != Player.m_localPlayer)
            return;

        float multiplier = Plugin.PrecisionMoveMultiplier.Value;
        multiplier = UnityEngine.Mathf.Clamp(multiplier, 0.05f, 1f);

        __result *= multiplier;
    }
}