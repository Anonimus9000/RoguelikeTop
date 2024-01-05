using Config.MainConfig;
using UnityEngine;

namespace GameScripts.LocalConfig.Images
{
public class CameraImage : IImage
{
    public float CinematicMoveToTargetDuration { get; }
    public float DelayOnTargetDuration { get; }
    public Vector3 CameraTargetOffset { get; }
    public float CameraSmoothSpeed { get; }
    public float CameraFieldOfView { get; }
    
    public CameraImage(
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