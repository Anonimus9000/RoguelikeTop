using System;
using MVP;
using PlayerControl.VirtualJoystick.Images.Joystick;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base
{
public interface IJoystickPresenter : IPresenter
{
    public event Action<JoystickAxis> AxisChanged; 
    public void ShowJoystick();
    public void HideJoystick();
    public void LockJoystick();
    public void UnlockJoystick();
    void OnJoystickMoved(Vector2 inputVector);
    void OnDrag(PointerEventData eventData);
    void OnEndDrag(PointerEventData eventData);
    void OnBeginDrag(PointerEventData eventData);
}
}