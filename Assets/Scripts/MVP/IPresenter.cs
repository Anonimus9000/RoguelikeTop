using System;

namespace MVP
{
public interface IPresenter : IDisposable
{
    protected internal IModel Model { get; }
    protected internal IView View { get; }
}
}