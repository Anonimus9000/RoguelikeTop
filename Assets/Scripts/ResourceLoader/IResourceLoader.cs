using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceLoader
{
public interface IResourceLoader : IDisposable
{
    public Task PreloadInCash<TResource>(string resourceId);
    public TResource LoadResource<TResource>(string resourceId);
    public void LoadResource<TResource>(string resourceId, Action<TResource> onResourceLoaded, CancellationToken token);
    public Task<TResource> LoadResourceAsync<TResource>(string resourceId, CancellationToken token);
    public void ReleaseResource<TResource>(TResource resource);
    public void ReleaseAllResources();
}
}