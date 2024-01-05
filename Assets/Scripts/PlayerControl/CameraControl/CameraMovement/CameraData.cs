using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement
{
public struct CameraData
{
    public float CinematicMoveToTargetDuration { get; }
    public float DelayOnTargetDuration { get; }
    public Vector3 CameraTargetOffset { get; }
    public float CameraSmoothSpeed { get; }
    public float CameraFieldOfView { get; }
    
    public CameraData(
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