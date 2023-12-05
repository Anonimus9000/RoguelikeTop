using System;
using PlayerControl.CameraControl.CameraMovement.Base;
using UnityEngine;

namespace PlayerControl.CameraControl.Provider
{
public class CameraProvider : ICameraProvider
{
    public event Action CinematicStarted;
    public event Action CinematicEnded;
    public Camera Camera => _cameraMovement.Camera;

    public bool CinematicInProcess => _cameraMovement.CinematicInProcess;

    private readonly ICameraMovementPresenter _cameraMovement;

    public CameraProvider(ICameraMovementPresenter cameraMovementPresenter)
    {
        _cameraMovement = cameraMovementPresenter;
    }

    public void Initialize()
    {
        _cameraMovement.CinematicStarted += OnCinematicMovementStarted;
        _cameraMovement.CinematicEnded += OnCinematicMovementEnded;
    }

    public void Dispose()
    {
        _cameraMovement.CinematicStarted -= OnCinematicMovementStarted;
        _cameraMovement.CinematicEnded -= OnCinematicMovementEnded; 
    }

    public void PlayCinematicMoveTo(Vector3 endPosition, Action onMoveCompleted = null)
    {
        _cameraMovement.CinematicMoveToPosition(endPosition, onMoveCompleted);
    }

    public void FollowToTarget(Transform target)
    {
        _cameraMovement.FollowTarget(target);
    }

    public void ReturnCinematicCamera(Action onReturnCameraCompleted = null)
    {
        _cameraMovement.ReturnCinematicCamera(onReturnCameraCompleted);
    }

    private void OnCinematicMovementStarted()
    {
        CinematicStarted?.Invoke();
    }

    private void OnCinematicMovementEnded()
    {
        CinematicEnded?.Invoke();
    }
}
}