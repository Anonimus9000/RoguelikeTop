﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Logger;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace LocalSaveSystem.UnityLocalSaveSystem
{
public class UnityLocalSaveSystem : ILocalSaveSystem
{
    private const string FileName = "Saves.json";
    private readonly ISavable[] _savesCash;
    private Dictionary<string, JObject> _loadedJsonSave;
    private bool _needSaveToStorage;
    private readonly string _storagePath;
    private readonly string _filePath;
    private readonly IInGameLogger _logger;
    private CancellationTokenSource _cancellationTokenSource;
    #if UNITY_EDITOR
    private static string FilePathDev =>
        Path.Combine(Path.Combine(Application.persistentDataPath, "SaveData"), FileName);
    #endif

    public UnityLocalSaveSystem(string storagePath, ISavable[] savables, IInGameLogger logger, int autoSavePeriodPerSeconds = 10)
    {
        _storagePath = storagePath;
        _logger = logger;
        _savesCash = savables;
        _cancellationTokenSource = new CancellationTokenSource();
        _filePath = Path.Combine(_storagePath, FileName);

        SubscribeOnEvents();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        StartAutoSaveData(autoSavePeriodPerSeconds, _cancellationTokenSource.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    public void InitializeSaves()
    {
        _loadedJsonSave = LoadJsonSave();
        ParseSavesFromStorage();
    }

    public async UniTask InitializeSavesAsync(CancellationToken cancellationToken)
    {
        _loadedJsonSave = await LoadJsonSave(cancellationToken);
        ParseSavesFromStorage();
    }

    public bool IsHaveSave<T>() where T : ISavable
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
            return false;
        }

        foreach (var savable in _savesCash)
        {
            if (savable is T)
            {
                return savable.IsHaveSave();
            }
        }

        return false;
    }

    public bool IsHaveSaveInt(string id)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        if (!PlayerPrefs.HasKey(id))
        {
            return false;
        }

        return true;
    }

    public bool IsHaveSaveFloat(string id)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        if (!PlayerPrefs.HasKey(id))
        {
            return false;
        }

        return true;
    }

    public bool IsHaveSaveString(string id)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        if (!PlayerPrefs.HasKey(id))
        {
            return false;
        }

        return true;
    }

    public void SaveInt(string id, int data)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        PlayerPrefs.SetInt(id, data);

        _needSaveToStorage = true;
    }

    public void SaveFloat(string id, float data)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        PlayerPrefs.SetFloat(id, data);

        _needSaveToStorage = true;
    }

    public void SaveString(string id, string data)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }


        PlayerPrefs.SetString(id, data);

        _needSaveToStorage = true;
    }

    public void SaveAsNew<T>(T savable) where T : ISavable
    {
        for (var i = 0; i < _savesCash.Length; i++)
        {
            if (_savesCash[i] is T)
            {
                _savesCash[i] = savable;
            }
        }
    }

    public void Save()
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
            return;
        }

        _needSaveToStorage = true;
    }

    public int LoadInt(string id)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        var data = PlayerPrefs.GetInt(id);
        return data;
    }

    public float LoadFloat(string id)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }


        if (!PlayerPrefs.HasKey(id))
        {
            _logger.LogError($"Can't find data by id {id}");
        }

        var data = PlayerPrefs.GetFloat(id);
        return data;
    }

    public string LoadString(string id)
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
        }

        if (!PlayerPrefs.HasKey(id))
        {
            _logger.LogError($"Can't find data by id {id}");
        }

        var data = PlayerPrefs.GetString(id);
        return data;
    }

    public T Load<T>() where T : ISavable
    {
        if (_savesCash == null)
        {
            _logger.LogError("Initialize the saves before using");
            return default;
        }

        foreach (var savablesCash in _savesCash)
        {
            if (savablesCash is T savedData)
            {
                return savedData;
            }
        }

        _logger.LogError(
            $"Can't find {typeof(T)}. Need add savable in {nameof(ISavable)}[] from {nameof(InitializeSaves)} method");
        return default;
    }

    public void ForceUpdateStorageSaves()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        SaveAll();
    }

    #if UNITY_EDITOR
    [MenuItem("SaveSystem/DeleteAllSaves")]
    private static void DeleteSavesDev()
    {
        File.Delete(FilePathDev);
        PlayerPrefs.DeleteAll();
    }
    #endif

    private void ParseSavesFromStorage()
    {
        if (_loadedJsonSave.Count == 0)
        {
            foreach (var savesCash in _savesCash)
            {
                savesCash.InitializeAsNewSave();
            }
        }

        foreach (var savesCash in _savesCash)
        {
            if (_loadedJsonSave.TryGetValue(savesCash.SaveId, out var saveJObject))
            {
                savesCash.Parse(saveJObject);
            }
            else
            {
                savesCash.Parse(new JObject());
            }
        }
    }

    private void SubscribeOnEvents()
    {
        Application.quitting += OnApplicationQuitting;
    }

    private void UnsubscribeOnEvents()
    {
        Application.quitting -= OnApplicationQuitting;
    }

    private void OnApplicationQuitting()
    {
        SaveAll();

        UnsubscribeOnEvents();
    }

    private async UniTaskVoid StartAutoSaveData(int periodPerSeconds, CancellationToken token)
    {
        var periodPerMillisecond = periodPerSeconds * 1000;

        try
        {
            await UniTask.RunOnThreadPool(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    
                    await UniTask.Delay(periodPerMillisecond, cancellationToken: token);

                    if (!_needSaveToStorage)
                    {
                        continue;
                    }

                    await SaveAllAsync(token);

                    _needSaveToStorage = false;
                }
            }, cancellationToken: token);
        }
        catch (Exception e)
        {
            if (e is not OperationCanceledException)
            {
                _logger.LogException(e);
            }
        }
    }

    private void SaveAll()
    {
        CreateDirectoryIfNeed();

        var json = JsonConvert.SerializeObject(_savesCash);
        File.WriteAllText(_filePath, json);
    }

    private async UniTask SaveAllAsync(CancellationToken token)
    {
        try
        {
            CreateDirectoryIfNeed();

            var json = JsonConvert.SerializeObject(_savesCash);
            await File.WriteAllTextAsync(_filePath, json, token);
        }
        catch (Exception e)
        {
            _logger.LogException(e);
        }
    }

    private Dictionary<string, JObject> LoadJsonSave()
    {
        if (!File.Exists(_filePath))
        {
            return new Dictionary<string, JObject>();
        }

        var json = File.ReadAllText(_filePath);

        return JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
    }

    private async UniTask<Dictionary<string, JObject>> LoadJsonSave(CancellationToken token)
    {
        try
        {
            var deserializeObject = new Dictionary<string, JObject>();
            if (!File.Exists(_filePath))
            {
                return deserializeObject;
            }

            var json = await File.ReadAllTextAsync(_filePath, token);

            var jsonArray = JArray.Parse(json);

            foreach (var jToken in jsonArray)
            {
                var item = (JObject)jToken;
                var saveId = item.GetValue("SaveId")!.ToString();
                deserializeObject[saveId] = item;
            }

            return deserializeObject;
        }
        catch (Exception e)
        {
            _logger.LogException(e);
            return new Dictionary<string, JObject>();
        }
    }

    private void CreateDirectoryIfNeed()
    {
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public void Dispose()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
        }
        
        _cancellationTokenSource?.Dispose();
    }
}
}