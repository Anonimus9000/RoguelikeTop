using System;
using Config;
using Logger;
using MVP;
using MVP.Disposable;
using PlayerControl.CameraControl.CameraMovement.Base;
using PlayerControl.CameraControl.ConfigImage;
using TickHandler;
using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement
{
public class CameraMovementPresenter : ICameraMovementPresenter
{
    public event Action CinematicStarted;
    public event Action CinematicEnded;
    IModel IPresenter.Model => _model;
    IView IPresenter.View => _view;

    public Camera Camera => _view.Camera;
    public bool CinematicInProcess => _model.CinematicInProcess;

    private readonly ICameraMovementView _view;
    private readonly ICameraMovementModel _model;
    private readonly ICompositeDisposable _compositeDisposable;
    private readonly IConfig _config;
    private Action _onMoveCompleted;
    private Action _onReturnCinematicCamera;

    public CameraMovementPresenter(
        ICameraMovementView view,
        ICameraMovementModel model,
        IInGameLogger logger,
        ITickHandler tickHandler,
        IConfig config)
    {
        _model = model;
        _view = view;
        _compositeDisposable = new CompositeDisposable();
        _config = config;

        InitializeView(config, tickHandler, logger);
        _compositeDisposable.AddDisposable(_view, _model);
    }

    public void FollowTarget(Transform target)
    {
        _view.FollowTarget(target);
    }

    public void CinematicMoveToPosition(Vector3 endPosition, Action onMoveCompleted = null)
    {
        _model.OnCinematicStarted();
        CinematicStarted?.Invoke();
        _onMoveCompleted = onMoveCompleted;

        var gameplayCameraImage = _config.GetImage<GameplayCameraImage>();
        var moveToTargetDuration = gameplayCameraImage.CinematicMoveToTargetDuration;
        var delayOnTargetDuration = gameplayCameraImage.DelayOnTargetDuration;
        
        var cameraTargetOffset = gameplayCameraImage.CameraTargetOffset;

        var newPosition = endPosition + _view.Transform.TransformDirection(cameraTargetOffset);

        _view.CinematicMoveToTarget(newPosition, moveToTargetDuration, delayOnTargetDuration);
    }

    public void ReturnCinematicCamera(Action onReturnCameraCompleted = null)
    {
        _onReturnCinematicCamera = onReturnCameraCompleted;
        
        var gameplayCameraImage = _config.GetImage<GameplayCameraImage>();
        _model.OnCinematicStarted();
        var delayOnTargetDuration = gameplayCameraImage.DelayOnTargetDuration;
        _view.ReturnCinematicCamera(delayOnTargetDuration);
    }

    public void OnCinematicReturnCompleted()
    {
        _onReturnCinematicCamera?.Invoke();
        _onReturnCinematicCamera = null;
        
        _model.OnCinematicEnded();
        if (!_model.CinematicInProcess)
        {
            CinematicEnded?.Invoke();
        }
    }

    public void OnCinematicMoveCameraCompleted()
    {
        _model.OnCinematicEnded();
        _onMoveCompleted?.Invoke();

        if (!_model.CinematicInProcess)
        {
            CinematicEnded?.Invoke();
        }
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }

    private void InitializeView(IConfig config, ITickHandler tickHandler, IInGameLogger logger)
    {
        _view.Initialize(tickHandler, logger, config, this);
    }
}
}