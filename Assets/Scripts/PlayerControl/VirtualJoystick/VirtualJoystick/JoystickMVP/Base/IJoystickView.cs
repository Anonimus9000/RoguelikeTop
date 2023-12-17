using MVP;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base
{
public interface IJoystickView : IView<IJoystickPresenter>
{
    public void SetJoystickActive(bool isActive);
    public void SetLockState(bool isLock);
    public void MoveJoystickToStartPosition();
}
}