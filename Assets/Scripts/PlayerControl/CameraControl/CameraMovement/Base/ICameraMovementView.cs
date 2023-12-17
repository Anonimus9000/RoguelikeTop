using Config;
using Logger;
using MVP;
using TickHandler;
using UnityEngine;

namespace PlayerControl.CameraControl.CameraMovement.Base
{
public interface ICameraMovementView : IView<ICameraMovementPresenter>
{
    public Camera Camera { get; }
    public Transform Transform { get; }

    public void InitializeDependencies(
        ITickHandler tickHandler,
        IInGameLogger logger,
        IConfig localConfig);

    public void FollowTarget(Transform target);

    public void StopFollow();
    public void CinematicMoveToTarget(Vector3 targetPosition, float moveToTargetDuration, float delayOnTargetDuration);
    public void ReturnCinematicCamera(float returnDuration);
}
}