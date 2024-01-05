using MVP.Disposable;
using PlayerControl.PlayerMovement.BaseMVP;
using PlayerControl.VirtualJoystick.Images.Joystick;
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
        var joystickAxis = _joystickProvider.GetJoystickAxis();
        var rigidbody = _view.Rigidbody;

        if (AxisIsZero(joystickAxis))
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        
        var speed = _model.GetPlayerSpeed();
        var rotationSpeed = _model.GetPlayerRotationSpeed();

        var movement = new Vector3(joystickAxis.AxisX, 0, joystickAxis.AxisY);

        rigidbody.MovePosition(rigidbody.position + movement * speed * deltaTime);
        
        var toRotation = Quaternion.LookRotation(movement, Vector3.up);

        rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, toRotation, rotationSpeed * deltaTime);
    }

    private bool AxisIsZero(JoystickAxis joystickAxis)
    {
        Debug.Log($"x = {joystickAxis.AxisX} | y = {joystickAxis.AxisY}");
        var axisX = joystickAxis.AxisX;
        var axisY = joystickAxis.AxisY;
        var axisXIsZero = Mathf.Abs(axisX) < 0.05f;
        var axisYIsZero = Mathf.Abs(axisY) < 0.05f;
        var axisIsZero = axisXIsZero && axisYIsZero;
        
        return axisIsZero;
    }
}
}