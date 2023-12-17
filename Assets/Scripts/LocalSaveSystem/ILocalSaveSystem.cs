using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LocalSaveSystem
{
public interface ILocalSaveSystem : IDisposable
{
    public void InitializeSaves();
    public UniTask InitializeSavesAsync(CancellationToken cancellationToken);
    public bool IsHaveSave<T>() where T : ISavable;
    public bool IsHaveSaveInt(string id);
    public bool IsHaveSaveFloat(string id);
    public bool IsHaveSaveString(string id);
    public void SaveInt(string id, int data);
    public void SaveFloat(string id, float data);
    public void SaveString(string id, string data);
    public void SaveAsNew<T>(T savable) where T : ISavable;
    public void Save();
    public int LoadInt(string id);
    public float LoadFloat(string id);
    public string LoadString(string id);
    public T Load<T>() where T : ISavable;
    public void ForceUpdateStorageSaves();
}
}