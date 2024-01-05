using System;
using MVP.Disposable;
using PlayerControl.VirtualJoystick.Images.Joystick;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP
{
public class JoystickPresenter : IJoystickPresenter
{
    public event Action<JoystickAxis> AxisChanged;
    public JoystickAxis CurrentAxis => _model.CurrentAxis;

    private readonly IJoystickView _view;
    private readonly IJoystickModel _model;
    private readonly ICompositeDisposable _compositeDisposable;

    public JoystickPresenter(IJoystickView joystickView, IJoystickModel joystickModel)
    {
        _compositeDisposable = new CompositeDisposable();
        _view = joystickView;
        _view.Initialize(this);
        _model = joystickModel;
        _compositeDisposable.AddDisposable(_view, _model);
    }

    public void Dispose()
    {
        _view.Dispose();
        _compositeDisposable.Dispose();
    }

    public void ShowJoystick()
    {
        _model.UpdateActiveState(true);
        _view.SetJoystickActive(_model.IsActive);
    }

    public void HideJoystick()
    {
        _model.UpdateActiveState(false);

        _view.MoveJoystickToStartPosition();
        _view.SetJoystickActive(_model.IsActive);
    }

    public void LockJoystick()
    {
        _model.UpdateLockState(true);
        _view.SetLockState(_model.IsLock);
        _view.MoveJoystickToStartPosition();
    }

    public void UnlockJoystick()
    {
        _model.UpdateLockState(false);
        _view.SetLockState(_model.IsLock);
    }

    public void OnJoystickMoved(Vector2 inputVector)
    {
        var newAxis = new JoystickAxis(inputVector.x, inputVector.y);

        _model.UpdateAxis(newAxis);
        AxisChanged?.Invoke(_model.CurrentAxis);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var stopAxis = new JoystickAxis(0, 0);
        _model.UpdateAxis(stopAxis);
        AxisChanged?.Invoke(_model.CurrentAxis);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }
}
}