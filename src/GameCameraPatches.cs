using HarmonyLib;
using UnityEngine;

namespace ImmersiveBuildCamera;

[HarmonyPatch(typeof(GameCamera), nameof(GameCamera.UpdateCamera))]
internal static class GameCameraUpdatePatch
{
    private static float _originalFov;
    private static float _originalNearClip;
    private static bool _savedOriginals;

    private static void Postfix(GameCamera __instance, float dt)
    {
        if (__instance == null)
            return;

        Camera camera = __instance.m_camera;

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

        if (gameCamera.m_skyCamera != null)
        {
            gameCamera.m_skyCamera.fieldOfView = Plugin.BuildFov.Value;
        }
    }

    private static void RestoreCamera(Camera camera)
    {
        if (!_savedOriginals)
            return;

        camera.fieldOfView = _originalFov;
        camera.nearClipPlane = _originalNearClip;
    }
}