using System;
using UnityEngine;

namespace PlayerControl.CameraControl.Provider
{
public interface ICameraProvider : IDisposable
{
    public event Action CinematicStarted;
    public event Action CinematicEnded;
    public Camera Camera { get; }
    public bool CinematicInProcess { get; }

    public void Initialize();
    public void PlayCinematicMoveTo(Vector3 endPosition, Action onMoveCompleted = null);
    public void FollowToTarget(Transform target);
    public void ReturnCinematicCamera(Action onReturnCameraCompleted = null);
    public void AddCameraInStack(Camera uiCamera);
}
}