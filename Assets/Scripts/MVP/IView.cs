using System;

namespace MVP
{
public interface IView : IDisposable
{
    protected internal IPresenter Presenter { get; }
}
}