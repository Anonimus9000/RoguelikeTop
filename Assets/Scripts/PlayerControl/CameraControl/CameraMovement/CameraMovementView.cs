using Config;
using DG.Tweening;
using Logger;
using MVP;
using PlayerControl.CameraControl.CameraMovement.Base;
using PlayerControl.CameraControl.ConfigImage;
using TickHandler;
using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement
{
public class CameraMovementView : MonoBehaviour, ICameraMovementView
{
    [SerializeField]
    private Camera _camera;
    
    public Camera Camera => _camera;
    public Transform Transform => transform;
    private ICameraMovementPresenter _presenter;
    private ITickHandler _tickHandler;
    private Transform _target;
    private IInGameLogger _logger;
    private IConfig _config;
    private Sequence _cinematicMoveCameraSequence;

    public void InitializeDependencies(
        ITickHandler tickHandler,
        IInGameLogger logger,
        IConfig localConfig)
    {
        _tickHandler = tickHandler;
        _config = localConfig;
        _logger = logger;
    }

    public void Initialize(ICameraMovementPresenter presenter)
    {
        _presenter = presenter;
    }

    public void Dispose()
    {
        UnsubscribeOnFollowTickUpdate();
    }

    public void FollowTarget(Transform target)
    {
        _target = target;


        UnsubscribeOnFollowTickUpdate();
        SubscribeOnFollowTickUpdate();
    }

    public void StopFollow()
    {
        UnsubscribeOnFollowTickUpdate();
    }

    public void CinematicMoveToTarget(Vector3 targetPosition, float moveToTargetDuration, float delayOnTargetDuration)
    {
        _cinematicMoveCameraSequence?.Kill();
        UnsubscribeOnFollowTickUpdate();

        _cinematicMoveCameraSequence = DOTween.Sequence();
        
        var currentPosition = transform.position;
        var cameraYPosition = currentPosition.y;
        var newCameraPosition = new Vector3(targetPosition.x, cameraYPosition, targetPosition.z);
        
        _cinematicMoveCameraSequence.Append(transform.DOMove(newCameraPosition, moveToTargetDuration));
        _cinematicMoveCameraSequence.AppendInterval(delayOnTargetDuration);

        _cinematicMoveCameraSequence.onComplete += OnCinematicMoveCameraCompleted;
    }

    public void ReturnCinematicCamera(float returnDuration)
    {
        _cinematicMoveCameraSequence?.Kill();
        _cinematicMoveCameraSequence = DOTween.Sequence();
        
        var followTargetPosition = GetFollowTargetPosition();
        _cinematicMoveCameraSequence.Append(transform.DOMove(followTargetPosition, returnDuration));
        _cinematicMoveCameraSequence.onComplete += OnCinematicReturnCompleted;
    }

    private void OnCinematicReturnCompleted()
    {
        _presenter.OnCinematicReturnCompleted();
    }

    private void OnCinematicMoveCameraCompleted()
    {
        SubscribeOnFollowTickUpdate();
        _presenter.OnCinematicMoveCameraCompleted();
    }

    private void SubscribeOnFollowTickUpdate()
    {
        _tickHandler.PhysicUpdate += OnPhysicUpdate;
    }

    private void UnsubscribeOnFollowTickUpdate()
    {
        _tickHandler.PhysicUpdate -= OnPhysicUpdate;
    }

    //FixedUpdate is employed because character movement takes place within FixedUpdate,
    //and due to linear interpolation (the same applies to other interpolation methods),
    //the camera's movement results in jittery motion
    private void OnPhysicUpdate(float deltaTime)
    {
        if (_target == null)
        {
            _logger.LogError("Unable to follow the target, the target is null");
        }

        //TODO: take data from presenter
        var gameplayCameraImage = _config.GetImage<GameplayCameraImage>();
        var cameraSmoothSpeed = gameplayCameraImage.CameraSmoothSpeed;
        _camera.fieldOfView = gameplayCameraImage.CameraFieldOfView;

        var newPosition = GetFollowTargetPosition();
        
        var smoothedPosition =
            Vector3.SlerpUnclamped(_camera.transform.position, newPosition, cameraSmoothSpeed * deltaTime);
        _camera.transform.position = smoothedPosition;
    }

    private Vector3 GetFollowTargetPosition()
    {
        var gameplayCameraImage = _config.GetImage<GameplayCameraImage>();
        var cameraTargetOffset = gameplayCameraImage.CameraTargetOffset;

        var newPosition = _target.position + transform.TransformDirection(cameraTargetOffset);

        return newPosition;
    }
}
}