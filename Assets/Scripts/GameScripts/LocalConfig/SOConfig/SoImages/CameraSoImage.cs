using UnityEngine;

namespace GameScripts.LocalConfig.SOConfig.SoImages
{
[CreateAssetMenu(fileName = "CameraConfig", menuName = "ScriptableObjects/Config/CameraConfig", order = 3)]
public class CameraSoImage : ScriptableObject
{
    [field: SerializeField]
    public float CinematicMoveToTargetDuration { get; private set; }

    [field: SerializeField]
    public float DelayOnTargetDuration { get; private set; }

    [field: SerializeField]
    public Vector3 CameraTargetOffset { get; private set; }

    [field: SerializeField]
    public float CameraSmoothSpeed { get; private set; }

    [field: SerializeField]
    public float CameraFieldOfView { get; private set; }
}
}