using MVP;
using PlayerControl.VirtualJoystick.Images.Joystick;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP
{
public class JoystickModel : IJoystickModel
{
    public bool IsActive { get; private set; }
    public bool IsLock { get; private set; }
    public JoystickAxis CurrentAxis { get; private set; }
    
    private IPresenter _presenter;
    
    public void UpdateActiveState(bool isActive)
    {
        IsActive = isActive;
    }

    public void UpdateLockState(bool isLock)
    {
        IsLock = isLock;
    }

    public void UpdateAxis(JoystickAxis newAxis)
    {
        CurrentAxis = newAxis;
    }

    public void Dispose()
    {
    }
}
}