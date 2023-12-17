using System;
using System.Threading;
using Config;
using Cysharp.Threading.Tasks;
using GameScripts.Scenes.TestMovement;
using LocalSaveSystem;
using LocalSaveSystem.UnityLocalSaveSystem;
using Logger;
using MVP.Disposable;
using ResourceLoader;
using ResourceLoader.AddressableResourceLoader;
using SceneSwitcher;
using TickHandler;
using TickHandler.UnityTickHandler;
using UIContext;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Root
{
public class GameRoot : MonoBehaviour, IRoot
{
    [SerializeField]
    private UnityUIContext _unityUIContext;

    private readonly ICompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    private async UniTaskVoid Start()
    {
        DontDestroyOnLoad(_unityUIContext);

        await StartApplication();
    }

    private void OnApplicationQuit()
    {
        _compositeDisposable.Dispose();
    }

    private async UniTask StartApplication()
    {
        var token = Application.exitCancellationToken;
        IInGameLogger logger = new UnityInGameLogger();
        _compositeDisposable.AddDisposable(logger);
        
        ISceneSwitcher sceneSwitcher = new AddressablesSceneSwitcher(logger);
        _compositeDisposable.AddDisposable(sceneSwitcher);

        IResourceLoader resourceLoader = new AddressableResourceLoader();
        _compositeDisposable.AddDisposable(resourceLoader);

        IDispatcher dispatcher = new GameObject().AddComponent<UnityDispatcherBehaviour>();
        ITickHandler tickHandler = new UnityTickHandler(dispatcher);

        var saveSystem = await InitializeSaveSystem(logger, token);

        IConfig config = new GameScripts.LocalConfig.Config();
        
        IScene startScene = new TestMovementScene(resourceLoader, tickHandler, _unityUIContext, logger, )
    }

    private async UniTask<ILocalSaveSystem> InitializeSaveSystem(IInGameLogger logger, CancellationToken token)
    {
        var savePath = Application.persistentDataPath;
        var savables = Array.Empty<ISavable>();
        ILocalSaveSystem localSaveSystem = new UnityLocalSaveSystem(savePath, savables, logger);
        await localSaveSystem.InitializeSavesAsync(token);
        
        _compositeDisposable.AddDisposable(localSaveSystem);
        
        return localSaveSystem;
    }
}
}