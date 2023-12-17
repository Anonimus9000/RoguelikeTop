using System;
using UnityEngine;

namespace Logger
{
public class UnityInGameLogger : IInGameLogger
{
    public void Log(string message)
    {
        Debug.Log(message);
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
    }

    public void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    public void LogException(Exception exception)
    {
        Debug.LogException(exception);
    }

    public void Dispose()
    {
        
    }
}
}