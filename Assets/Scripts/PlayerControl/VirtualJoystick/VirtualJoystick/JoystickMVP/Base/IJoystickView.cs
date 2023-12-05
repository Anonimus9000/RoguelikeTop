using MVP;

namespace PlayerControl.Joystick.VirtualJoystick.JoystickMVP.Base
{
public interface IJoystickView : IView
{
    public void Initialize(IJoystickPresenter presenter);
    public void SetJoystickActive(bool isActive);
    public void SetLockState(bool isLock);
    public void MoveJoystickToStartPosition();
}
}