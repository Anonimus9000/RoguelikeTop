using MVP;

namespace PlayerControl.CameraControl.CameraMovement.Base
{
public interface ICameraMovementModel : IModel
{
    public bool CinematicInProcess { get; }
    public void OnCinematicStarted();
    public void OnCinematicEnded();
}
}