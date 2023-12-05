﻿using System;
using MVP;
using MVP.Disposable;
using PlayerControl.Joystick.Images.Joystick;
using PlayerControl.Joystick.VirtualJoystick.JoystickMVP.Base;
using UIContext;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerControl.Joystick.VirtualJoystick.JoystickMVP
{
public class JoystickPresenter : IJoystickPresenter
{
    public event Action<JoystickAxis> AxisChanged;
    
    IModel IPresenter.Model => _model;
    IView IPresenter.View => _view;

    private readonly IJoystickView _view;
    private readonly IJoystickModel _model;
    private readonly IUIContext _uiContext;
    private readonly ICompositeDisposable _compositeDisposable;

    public JoystickPresenter(IUIContext uiContext, IJoystickView joystickView, IJoystickModel joystickModel)
    {
        _compositeDisposable = new CompositeDisposable();
        _uiContext = uiContext;
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