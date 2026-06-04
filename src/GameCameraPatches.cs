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

    private static int _cachedCollisionMask = -1;

    private static void Postfix(GameCamera __instance)
    {
        if (__instance == null)
            return;

        Camera? camera = GetCamera(__instance);

        if (camera == null)
        {
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

        Vector3 anchorPosition = eye.position;
        Vector3 desiredPosition = anchorPosition;
        Quaternion desiredRotation = eye.rotation;

        if (Plugin.ShoulderPeekKey.Value.IsPressed())
        {
            desiredPosition += eye.right * Plugin.ShoulderOffsetX.Value;
            desiredPosition += eye.up * Plugin.ShoulderOffsetY.Value;
            desiredPosition -= eye.forward * Plugin.ShoulderDistance.Value;

            desiredPosition = ResolveCameraCollision(anchorPosition, desiredPosition);
        }

        gameCamera.transform.position = desiredPosition;
        gameCamera.transform.rotation = desiredRotation;

        camera.fieldOfView = Plugin.BuildFov.Value;
        camera.nearClipPlane = Plugin.NearClip.Value;
    }

    private static Vector3 ResolveCameraCollision(Vector3 anchorPosition, Vector3 desiredPosition)
    {
        Vector3 offset = desiredPosition - anchorPosition;
        float distance = offset.magnitude;

        if (distance <= 0.001f)
            return desiredPosition;

        Vector3 direction = offset / distance;

        bool hitSomething = Physics.SphereCast(
            anchorPosition,
            Mathf.Max(0.01f, Plugin.CollisionRadius.Value),
            direction,
            out RaycastHit hit,
            distance,
            GetCollisionMask(),
            QueryTriggerInteraction.Ignore
        );

        if (!hitSomething)
            return desiredPosition;

        float safeDistance = Mathf.Max(0f, hit.distance - Plugin.CollisionRadius.Value);
        return anchorPosition + direction * safeDistance;
    }

    private static int GetCollisionMask()
    {
        if (_cachedCollisionMask != -1)
            return _cachedCollisionMask;

        int mask = LayerMask.GetMask(
            "Default",
            "static_solid",
            "terrain",
            "piece",
            "piece_nonsolid"
        );

        if (mask == 0)
        {
            mask = Physics.DefaultRaycastLayers;
            Plugin.Log.LogWarning("Could not resolve Valheim-specific camera collision layers. Falling back to Physics.DefaultRaycastLayers.");
        }

        _cachedCollisionMask = mask;
        return _cachedCollisionMask;
    }

    private static void RestoreCamera(Camera camera)
    {
        if (!_savedOriginals)
            return;

        camera.fieldOfView = _originalFov;
        camera.nearClipPlane = _originalNearClip;
    }
}