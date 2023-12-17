﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace SceneSwitcher
{
public interface IScene : IDisposable
{
    public string SceneId { get; }
    protected internal ISceneContext SceneContext { get; }
    protected internal ISceneSwitcher SceneSwitcher { get; }

    public UniTaskVoid InitializeAsync(CancellationToken token);
}
}