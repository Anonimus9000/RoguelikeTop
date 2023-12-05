using UnityEngine;

namespace PlayerControl.CameraControl.ConfigImage
{
public struct GameplayCameraImage
{
    public float CinematicMoveToTargetDuration { get; }
    public float DelayOnTargetDuration { get; }
    public Vector3 CameraTargetOffset { get; }
    public float CameraSmoothSpeed { get; }
    public float CameraFieldOfView { get; }
    
    public GameplayCameraImage(
        float cinematicMoveToTargetDuration,
        float delayOnTargetDuration,
        Vector3 cameraTargetOffset,
        float cameraSmoothSpeed,
        float cameraFieldOfView)
    {
        CinematicMoveToTargetDuration = cinematicMoveToTargetDuration;
        DelayOnTargetDuration = delayOnTargetDuration;
        CameraTargetOffset = cameraTargetOffset;
        CameraSmoothSpeed = cameraSmoothSpeed;
        CameraFieldOfView = cameraFieldOfView;
    }
}
}