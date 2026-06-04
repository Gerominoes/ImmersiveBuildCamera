using HarmonyLib;
using UnityEngine;

namespace ImmersiveBuildCamera;

[HarmonyPatch(typeof(GameCamera))]
[HarmonyPatch("UpdateCamera")]
internal static class GameCameraUpdatePatch
{
    private static readonly System.Reflection.FieldInfo? CameraField =
        AccessTools.Field(typeof(GameCamera), "m_camera");

    private static float _originalFov;
    private static float _originalNearClip;
    private static bool _savedOriginals;

    private static void Postfix(GameCamera __instance)
    {
        if (__instance == null)
            return;

        Camera? camera = GetCamera(__instance);

        if (camera == null)
        {
            Plugin.Log.LogWarning("Could not find GameCamera.m_camera. Falling back to Camera.main.");
            camera = Camera.main;
        }

        if (camera == null)
            return;

        if (!_savedOriginals)
        {
            _originalFov = camera.fieldOfView;
            _originalNearClip = camera.nearClipPlane;
            _savedOriginals = true;
        }

        if (!BuildCameraState.Active)
        {
            RestoreCamera(camera);
            return;
        }

        ApplyImmersiveBuildCamera(__instance, camera);
    }

    private static Camera? GetCamera(GameCamera gameCamera)
    {
        if (CameraField == null)
            return null;

        return CameraField.GetValue(gameCamera) as Camera;
    }

    private static void ApplyImmersiveBuildCamera(GameCamera gameCamera, Camera camera)
    {
        Player player = Player.m_localPlayer;

        if (player == null)
            return;

        Transform eye = player.m_eye != null
            ? player.m_eye
            : player.transform;

        gameCamera.transform.position = eye.position;
        gameCamera.transform.rotation = eye.rotation;

        camera.fieldOfView = Plugin.BuildFov.Value;
        camera.nearClipPlane = Plugin.NearClip.Value;
    }

    private static void RestoreCamera(Camera camera)
    {
        if (!_savedOriginals)
            return;

        camera.fieldOfView = _originalFov;
        camera.nearClipPlane = _originalNearClip;
    }
}