using System;

namespace MVP
{
public interface IView<T> : IDisposable where T : IPresenter
{
    public void Initialize(T presenter);
}
}