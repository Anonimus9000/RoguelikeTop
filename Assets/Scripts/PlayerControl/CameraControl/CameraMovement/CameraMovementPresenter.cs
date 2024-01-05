using System;
using Logger;
using MVP.Disposable;
using PlayerControl.CameraControl.CameraMovement.Base;
using TickHandler;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PlayerControl.CameraControl.CameraMovement
{
public class CameraMovementPresenter : ICameraMovementPresenter
{
    public event Action CinematicStarted;
    public event Action CinematicEnded;

    public Camera Camera => _view.Camera;
    public bool CinematicInProcess => _model.CinematicInProcess;
    public float CameraSmoothSpeed => _model.CameraData.CameraSmoothSpeed;
    public float CameraFieldOfView => _model.CameraData.CameraFieldOfView;
    public Vector3 CameraTargetOffset => _model.CameraData.CameraTargetOffset;

    private readonly ICameraMovementView _view;
    private readonly ICameraMovementModel _model;
    private readonly IInGameLogger _logger;
    private readonly ITickHandler _tickHandler;
    private readonly ICompositeDisposable _compositeDisposable;
    private Action _onMoveCompleted;
    private Action _onReturnCinematicCamera;

    public CameraMovementPresenter(
        ICameraMovementView view,
        ICameraMovementModel model,
        IInGameLogger logger,
        ITickHandler tickHandler)
    {
        _model = model;
        _logger = logger;
        _tickHandler = tickHandler;
        _view = view;
        _compositeDisposable = new CompositeDisposable();

        _compositeDisposable.AddDisposable(_view, _model);
    }

    public void Initialize()
    {
        InitializeView(_logger);
    }

    public void Dispose()
    {
        UnsubscribeOnPhysicUpdate();

        _compositeDisposable.Dispose();
    }

    public void FollowTarget(Transform target)
    {
        _view.FollowTarget(target);
        SubscribeOnPhysicUpdate();
    }

    public void CinematicMoveToPosition(Vector3 endPosition, Action onMoveCompleted = null)
    {
        UnsubscribeOnPhysicUpdate();
        _model.OnCinematicStarted();
        CinematicStarted?.Invoke();
        _onMoveCompleted = onMoveCompleted;

        var cameraData = _model.CameraData;
        var moveToTargetDuration = cameraData.CinematicMoveToTargetDuration;
        var delayOnTargetDuration = cameraData.DelayOnTargetDuration;

        var cameraTargetOffset = cameraData.CameraTargetOffset;

        var newPosition = endPosition + _view.Transform.TransformDirection(cameraTargetOffset);

        _view.CinematicMoveToTarget(newPosition, moveToTargetDuration, delayOnTargetDuration);
    }

    public void ReturnCinematicCamera(Action onReturnCameraCompleted = null)
    {
        _onReturnCinematicCamera = onReturnCameraCompleted;

        _model.OnCinematicStarted();
        var cameraData = _model.CameraData;
        var delayOnTargetDuration = cameraData.DelayOnTargetDuration;
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

        SubscribeOnPhysicUpdate();
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

    public void AddCameraInStack(Camera camera)
    {
        var viewCamera = _view.Camera;
        var universalAdditionalCameraData = viewCamera.GetComponent<UniversalAdditionalCameraData>();
        if (!universalAdditionalCameraData.cameraStack.Contains(camera))
        {
            universalAdditionalCameraData.cameraStack.Add(camera);
        }
    }

    private void SubscribeOnPhysicUpdate()
    {
        _tickHandler.PhysicUpdate += OnPhysicUpdate;
    }

    private void UnsubscribeOnPhysicUpdate()
    {
        _tickHandler.PhysicUpdate -= OnPhysicUpdate;
    }

    private void InitializeView(IInGameLogger logger)
    {
        _view.InitializeDependencies(logger);
        _view.Initialize(this);
    }

    private void OnPhysicUpdate(float deltaTime)
    {
        _view.OnPhysicUpdate(deltaTime);
    }
}
}