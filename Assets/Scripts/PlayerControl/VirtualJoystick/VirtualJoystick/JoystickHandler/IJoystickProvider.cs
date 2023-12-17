using System;
using System.Collections.Generic;
using PlayerControl.VirtualJoystick.Images.Joystick;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickHandler
{
public interface IJoystickProvider : IDisposable
{
    public event Action<JoystickAxis> AxisChanged;
    internal List<Action<JoystickAxis>> AxisListeners { get; }
    internal IJoystickPresenter JoystickPresenter { get; }

    public void SubscribeOnAxisChanged(Action<JoystickAxis> listener);
    public void UnsubscribeOnAxisChanged(Action<JoystickAxis> listener);
    public void ShowJoystick();
    public void HideJoystick();
    public void LockJoystick();
    public void UnlockJoystick();
}
}