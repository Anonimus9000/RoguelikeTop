using MVP;
using PlayerControl.VirtualJoystick.Images.Joystick;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base
{
public interface IJoystickModel : IModel
{
    bool IsActive { get; }
    bool IsLock { get; }
    public JoystickAxis CurrentAxis { get; }
    void UpdateActiveState(bool isActive);
    void UpdateLockState(bool isLock);
    void UpdateAxis(JoystickAxis newAxis);
}
}