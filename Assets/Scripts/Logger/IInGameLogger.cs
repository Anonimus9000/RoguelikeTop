using System;

namespace Logger
{
public interface IInGameLogger : IDisposable
{
    public void Log(string message);
    public void LogError(string message);
    public void LogWarning(string message);
    public void LogException(Exception exception);
}
}