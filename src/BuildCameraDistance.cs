using UnityEngine;

namespace ImmersiveBuildCamera;

internal static class BuildCameraDistance
{
    internal static float Current { get; private set; }

    private static bool _initialized;

    internal static void ResetForSession()
    {
        if (Plugin.RememberScrollDistance.Value && _initialized)
            return;

        Current = Clamp(Plugin.DefaultBuildCameraDistance.Value);
        _initialized = true;

        Plugin.DebugLog($"Camera distance reset to {Current:0.00}.");
    }

    internal static void UpdateFromInput()
    {
        if (!Plugin.EnableScrollDistanceAdjust.Value)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) <= 0.001f)
            return;

        float step = Mathf.Max(0f, Plugin.ScrollDistanceStep.Value);

        if (step <= 0f)
            return;

        float direction = scroll > 0f ? -1f : 1f;
        float next = Clamp(Current + direction * step);

        if (Mathf.Approximately(Current, next))
            return;

        Current = next;
        Plugin.DebugLog($"Camera distance set to {Current:0.00}.");
    }

    private static float Clamp(float value)
    {
        float min = Mathf.Min(Plugin.MinBuildCameraDistance.Value, Plugin.MaxBuildCameraDistance.Value);
        float max = Mathf.Max(Plugin.MinBuildCameraDistance.Value, Plugin.MaxBuildCameraDistance.Value);

        return Mathf.Clamp(value, min, max);
    }
}
