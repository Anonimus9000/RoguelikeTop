﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace SceneSwitcher
{
public interface IScene : IDisposable
{
    public string SceneId { get; }
    protected internal ISceneContext SceneContext { get; }
    protected internal ISceneSwitcher SceneSwitcher { get; }

    public Task InitializeAsync(CancellationToken token);
}
}