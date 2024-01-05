using System;
using MVP;
using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement.Base
{
public interface ICameraMovementPresenter : IPresenter
{
    public event Action CinematicStarted;
    public event Action CinematicEnded;
    public Camera Camera { get; }
    public bool CinematicInProcess { get; }
    public float CameraSmoothSpeed { get; }
    public float CameraFieldOfView { get; }
    public Vector3 CameraTargetOffset { get; }

    public void Initialize();
    public void FollowTarget(Transform viewPlayerTransform);
    public void CinematicMoveToPosition(Vector3 endPosition, Action onMoveCompleted = null);
    public void OnCinematicMoveCameraCompleted();
    public void ReturnCinematicCamera(Action onReturnCameraCompleted = null);
    public void OnCinematicReturnCompleted();
    public void AddCameraInStack(Camera camera);
}
}