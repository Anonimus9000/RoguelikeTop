using Logger;
using MVP;
using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement.Base
{
public interface ICameraMovementView : IView<ICameraMovementPresenter>
{
    public Camera Camera { get; }
    public Transform Transform { get; }

    public void InitializeDependencies(IInGameLogger logger);

    public void FollowTarget(Transform target);
    public void OnPhysicUpdate(float deltaTime);

    public void CinematicMoveToTarget(Vector3 targetPosition, float moveToTargetDuration, float delayOnTargetDuration);
    public void ReturnCinematicCamera(float returnDuration);
}
}