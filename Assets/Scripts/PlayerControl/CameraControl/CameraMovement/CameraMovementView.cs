using DG.Tweening;
using Logger;
using PlayerControl.CameraControl.CameraMovement.Base;
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
    private Transform _target;
    private IInGameLogger _logger;
    private Sequence _cinematicMoveCameraSequence;

    public void InitializeDependencies(IInGameLogger logger)
    {
        _logger = logger;
    }

    public void Initialize(ICameraMovementPresenter presenter)
    {
        _presenter = presenter;
    }

    public void Dispose()
    {
    }

    public void FollowTarget(Transform target)
    {
        _target = target;
    }

    public void CinematicMoveToTarget(Vector3 targetPosition, float moveToTargetDuration, float delayOnTargetDuration)
    {
        _cinematicMoveCameraSequence?.Kill();

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

    //FixedUpdate is employed because character movement takes place within FixedUpdate,
    //and due to linear interpolation (the same applies to other interpolation methods),
    //the camera's movement results in jittery motion
    public void OnPhysicUpdate(float deltaTime)
    {
        if (_target == null)
        {
            return;
        }

        var cameraSmoothSpeed = _presenter.CameraSmoothSpeed;
        _camera.fieldOfView = _presenter.CameraFieldOfView;

        var newPosition = GetFollowTargetPosition();

        var smoothedPosition =
            Vector3.SlerpUnclamped(_camera.transform.position, newPosition, cameraSmoothSpeed * deltaTime);
        _camera.transform.position = smoothedPosition;
    }

    private void OnCinematicReturnCompleted()
    {
        _presenter.OnCinematicReturnCompleted();
    }

    private void OnCinematicMoveCameraCompleted()
    {
        _presenter.OnCinematicMoveCameraCompleted();
    }

    private Vector3 GetFollowTargetPosition()
    {
        var cameraTargetOffset = _presenter.CameraTargetOffset;

        var newPosition = _target.position + transform.TransformDirection(cameraTargetOffset);

        return newPosition;
    }
}
}