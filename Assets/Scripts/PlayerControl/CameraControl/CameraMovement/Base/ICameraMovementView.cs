using Config;
using Logger;
using MVP;
using TickHandler;
using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement.Base
{
public interface ICameraMovementView : IView
{
    public Camera Camera { get; }
    public Transform Transform { get; }

    public void Initialize(
        ITickHandler tickHandler,
        IInGameLogger logger,
        IConfig localConfig,
        ICameraMovementPresenter presenter);

    public void FollowTarget(Transform target);

    public void StopFollow();
    public void CinematicMoveToTarget(Vector3 targetPosition, float moveToTargetDuration, float delayOnTargetDuration);
    public void ReturnCinematicCamera(float returnDuration);
}
}