using System;
using MVP.Disposable;
using PlayerControl.PlayerMovement.BaseMVP;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickHandler;
using TickHandler;
using UnityEngine;

namespace PlayerControl.PlayerMovement
{
public class PlayerMovementPresenter : IPlayerMovementPresenter
{
    private readonly IPlayerMovementModel _model;
    private readonly IJoystickProvider _joystickProvider;
    private readonly ITickHandler _tickHandler;
    private readonly IPlayerMovementView _view;
    private readonly ICompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    public PlayerMovementPresenter(
        IPlayerMovementView view,
        IPlayerMovementModel model,
        IJoystickProvider joystickProvider,
        ITickHandler tickHandler)
    {
        _view = view;
        _model = model;
        _joystickProvider = joystickProvider;
        _tickHandler = tickHandler;

        _compositeDisposable.AddDisposable(_view, _model);
    }

    public void Initialize()
    {
        _tickHandler.PhysicUpdate += OnPhysicUpdate;
    }

    public Transform GetTransform()
    {
        return _view.Transform;
    }

    public void Dispose()
    {
        _tickHandler.PhysicUpdate -= OnPhysicUpdate;
        
        _compositeDisposable.Dispose();
    }

    private void OnPhysicUpdate(float deltaTime)
    {
        throw new NotImplementedException();
    }
}
}