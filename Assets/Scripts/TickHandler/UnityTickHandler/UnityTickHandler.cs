﻿using System;
using System.Collections.Generic;

namespace TickHandler.UnityTickHandler
{
public class UnityTickHandler : ITickHandler
{
    public event Action<float> FrameUpdate;
    public event Action<float> FrameLateUpdate;
    public event Action<float> PhysicUpdate;

    IDispatcher ITickHandler.Dispatcher => _dispatcher;
    
    private readonly IDispatcher _dispatcher;
    private readonly List<Action<float>> _updateListeners;
    private readonly List<Action<float>> _lateUpdateListeners;
    private readonly List<Action<float>> _physicsListeners;

    public UnityTickHandler(IDispatcher dispatcher, int listenersCapacity = 50)
    {
        _dispatcher = dispatcher;
        _updateListeners = new List<Action<float>>(listenersCapacity);
        _lateUpdateListeners = new List<Action<float>>(listenersCapacity);
        _physicsListeners = new List<Action<float>>(listenersCapacity);
        
        SubscribeOnDispatcherEvents();
    }
    
    public void Dispose()
    {
        UnsubscribeOnDispatcherEvents();
        
        _updateListeners.Clear();
        _lateUpdateListeners.Clear();
        _physicsListeners.Clear();
    }

    public void SubscribeOnFrameUpdate(Action<float> listener)
    {
        _updateListeners.Add(listener);
    }

    public void SubscribeOnFrameLateUpdate(Action<float> listener)
    {
        _lateUpdateListeners.Add(listener);
    }

    public void SubscribeOnPhysicUpdate(Action<float> listener)
    {
        _physicsListeners.Add(listener);
    }

    public void UnsubscribeOnFrameUpdate(Action<float> listener)
    {
        _updateListeners.Remove(listener);
    }

    public void UnsubscribeOnFrameLateUpdate(Action<float> listener)
    {
        _lateUpdateListeners.Remove(listener);
    }

    public void UnsubscribeOnPhysicUpdate(Action<float> listener)
    {
        _physicsListeners.Remove(listener);
    }

    private void SubscribeOnDispatcherEvents()
    {
        _dispatcher.OnUpdate += OnDispatcherUpdateFrame;
        _dispatcher.OnLateUpdate += OnDispatcherLateUpdateFrame;
        _dispatcher.OnFixedUpdate += OnDispatcherPhysicsFrame;
    }

    private void UnsubscribeOnDispatcherEvents()
    {
        _dispatcher.OnUpdate -= OnDispatcherUpdateFrame;
        _dispatcher.OnLateUpdate -= OnDispatcherLateUpdateFrame;
        _dispatcher.OnFixedUpdate -= OnDispatcherPhysicsFrame;
    }
    
    private void OnDispatcherPhysicsFrame(float deltaTime)
    {
        PhysicUpdate?.Invoke(deltaTime);
        foreach (var physicsListener in _physicsListeners)
        {
            physicsListener?.Invoke(deltaTime);
        }
    }

    private void OnDispatcherUpdateFrame(float deltaTime)
    {
        FrameUpdate?.Invoke(deltaTime);
        foreach (var updateListener in _updateListeners)
        {
            updateListener?.Invoke(deltaTime);
        }
    }

    private void OnDispatcherLateUpdateFrame(float deltaTime)
    {
        FrameLateUpdate?.Invoke(deltaTime);
        foreach (var lateUpdateListener in _lateUpdateListeners)
        {
            lateUpdateListener?.Invoke(deltaTime);
        }
    }
}
}