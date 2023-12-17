using System;
using System.Collections.Generic;
using MVP.Disposable;
using PlayerControl.VirtualJoystick.Images.Joystick;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickMVP.Base;
using ResourceContainer;
using ResourceLoader;
using UIContext;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerControl.VirtualJoystick.VirtualJoystick.JoystickHandler
{
public class JoystickProvider : IJoystickProvider
{

    private const int ListenersCapacity = 50;
    public event Action<JoystickAxis> AxisChanged;

    List<Action<JoystickAxis>> IJoystickProvider.AxisListeners => _axisListeners;
    IJoystickPresenter IJoystickProvider.JoystickPresenter => _joystickPresenter;

    private readonly List<Action<JoystickAxis>> _axisListeners;
    private readonly ICompositeDisposable _compositeDisposable;
    private readonly IUIContext _uiContext;
    private IJoystickPresenter _joystickPresenter;

    public JoystickProvider(IUIContext uiContext, IResourceLoader resourceLoader)
    {
        _axisListeners = new List<Action<JoystickAxis>>(ListenersCapacity);
        _compositeDisposable = new CompositeDisposable();
        _uiContext = uiContext;
        var exitCancellationToken = Application.exitCancellationToken;
        var joystickViewResourceId = ResourceIdContainer.UIResourceId.VirtualJoystickView;
        resourceLoader.LoadResource<GameObject>(joystickViewResourceId, OnJoystickViewLoaded, exitCancellationToken);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
        _axisListeners.Clear();
        _joystickPresenter.AxisChanged -= OnJoystickAxisChanged;
    }

    public void SubscribeOnAxisChanged(Action<JoystickAxis> listener)
    {
        _axisListeners.Add(listener);
    }

    public void UnsubscribeOnAxisChanged(Action<JoystickAxis> listener)
    {
        _axisListeners.Remove(listener);
    }

    public void ShowJoystick()
    {
        _joystickPresenter.ShowJoystick();
    }

    public void HideJoystick()
    {
        _joystickPresenter.HideJoystick();
    }

    public void LockJoystick()
    {
        _joystickPresenter.LockJoystick();
    }

    public void UnlockJoystick()
    {
        _joystickPresenter.UnlockJoystick();
    }

    private void OnJoystickViewLoaded(GameObject joystickViewPrefab)
    {
        var viewObject = Object.Instantiate(joystickViewPrefab, _uiContext.JoystickParent);
        var joystickView = viewObject.GetComponent<IJoystickView>();
        var joystickModel = new JoystickModel();
        
        _joystickPresenter = new JoystickPresenter(_uiContext, joystickView, joystickModel);
        _joystickPresenter.AxisChanged += OnJoystickAxisChanged;
        _compositeDisposable.AddDisposable(_joystickPresenter);
    }

    private void OnJoystickAxisChanged(JoystickAxis axis)
    {
        AxisChanged?.Invoke(axis);
        foreach (var axisListener in _axisListeners)
        {
            axisListener.Invoke(axis);
        }
    }
}
}