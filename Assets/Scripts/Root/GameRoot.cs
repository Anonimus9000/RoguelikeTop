using System;
using System.Threading;
using Config.Parser;
using Cysharp.Threading.Tasks;
using GameScripts.LocalConfig;
using GameScripts.LocalConfig.SOConfig;
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

namespace Root
{
public class GameRoot : MonoBehaviour, IRoot
{
    [SerializeField]
    private UnityUIContext _unityUIContext;

    [SerializeField]
    private SoMainConfig _soMainConfig;

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

        var dispatcherObject = new GameObject();
        dispatcherObject.name = "Dispatcher";
        DontDestroyOnLoad(dispatcherObject);
        IDispatcher dispatcher = dispatcherObject.AddComponent<UnityDispatcherBehaviour>();
        ITickHandler tickHandler = new UnityTickHandler(dispatcher);

        var saveSystem = await InitializeSaveSystem(logger, token);

        IConfigParser configParser = new SOConfigParser(_soMainConfig, logger);

        var config = configParser.ParseConfig();

        IScene startScene = new TestMovementScene(resourceLoader, tickHandler, _unityUIContext, logger, config, sceneSwitcher);
        startScene.SwitchScene(token);
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