using PlayerControl.CameraControl.CameraMovement.Base;

namespace PlayerControl.CameraControl.CameraMovement
{
public class CameraMovementModel : ICameraMovementModel
{
    public bool CinematicInProcess { get; private set; }
    public CameraData CameraData { get; private set; }

    public CameraMovementModel(CameraData cameraData)
    {
        CameraData = cameraData;
    }

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