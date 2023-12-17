using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ResourceLoader
{
public interface IResourceLoader : IDisposable
{
    public UniTask PreloadInCash<TResource>(string resourceId);
    public TResource LoadResource<TResource>(string resourceId);
    public void LoadResource<TResource>(string resourceId, Action<TResource> onResourceLoaded, CancellationToken token);
    public UniTask<TResource> LoadResourceAsync<TResource>(string resourceId, CancellationToken token);
    public UniTask<TComponent> LoadResourceAsync<TComponent>(string resourceId, Transform parent, CancellationToken token);
    public void ReleaseResource<TResource>(TResource resource);
    public void ReleaseAllResources();
}
}