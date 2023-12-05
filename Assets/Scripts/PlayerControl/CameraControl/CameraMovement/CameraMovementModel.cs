using PlayerControl.CameraControl.CameraMovement.Base;

namespace PlayerControl.CameraControl.CameraMovement
{
public class CameraMovementModel : ICameraMovementModel
{
    public bool CinematicInProcess { get; private set; }

    public void Dispose()
    {
        CinematicInProcess = false;
    }

    public void OnCinematicStarted()
    {
        CinematicInProcess = true;
    }

    public void OnCinematicEnded()
    {
        CinematicInProcess = false;
    }
}
}