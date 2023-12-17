using System.Threading;
using Config;
using Cysharp.Threading.Tasks;
using GameScripts.Player.Provider;
using Logger;
using MVP.Disposable;
using PlayerControl.CameraControl.CameraMovement;
using PlayerControl.CameraControl.CameraMovement.Base;
using PlayerControl.CameraControl.Provider;
using PlayerControl.PlayerMovement;
using PlayerControl.PlayerMovement.BaseMVP;
using PlayerControl.VirtualJoystick.VirtualJoystick.JoystickHandler;
using ResourceLoader;
using SceneSwitcher;
using TickHandler;
using UIContext;

namespace GameScripts.Scenes.TestMovement
{
public class TestMovementScene : IScene
{
    private const string PlayerMovementResourceId = "PlayerMovementView";
    private const string CameraMovementResourceId = "CameraMovementView";
    private readonly IResourceLoader _resourceLoader;
    private readonly ITickHandler _tickHandler;
    private readonly IUIContext _uiContext;
    private readonly IInGameLogger _logger;
    private readonly IConfig _config;
    public string SceneId => "TestMovementScene";

    ISceneContext IScene.SceneContext => _sceneContext;
    ISceneSwitcher IScene.SceneSwitcher => _sceneSwitcher;
    
    private TestMovementSceneContext _sceneContext;
    private ISceneSwitcher _sceneSwitcher;
    private readonly ICompositeDisposable _compositeDisposable = new CompositeDisposable();

    public TestMovementScene(IResourceLoader resourceLoader,
        ITickHandler tickHandler,
        IUIContext uiContext,
        IInGameLogger logger,
        IConfig config)
    {
        _resourceLoader = resourceLoader;
        _tickHandler = tickHandler;
        _uiContext = uiContext;
        _logger = logger;
        _config = config;
    }
    
    public async UniTaskVoid InitializeAsync(CancellationToken token)
    {
        _sceneContext = await _sceneSwitcher.SwitchToSceneAsync<TestMovementSceneContext>(SceneId, token);
        var joystickProvider = await InitializeVirtualJoystick();
        var cameraProvider = await InitializeCamera(token);
        var playerProvider = await InitializePlayer(joystickProvider, token);

        var playerTransform = playerProvider.GetPlayerTransform();
        cameraProvider.FollowToTarget(playerTransform);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }

    private async UniTask<IJoystickProvider> InitializeVirtualJoystick()
    {
        IJoystickProvider joystickProvider = new JoystickProvider(_uiContext, _resourceLoader);
        _compositeDisposable.AddDisposable(joystickProvider);

        return joystickProvider;
    }

    private async UniTask<IPlayerProvider> InitializePlayer(IJoystickProvider joystickProvider, CancellationToken token)
    {
        var playerParent = _sceneContext.PlayerParent;
        var view = await _resourceLoader.LoadResourceAsync<IPlayerMovementView>(PlayerMovementResourceId, playerParent, token);
        IPlayerMovementModel model = new PlayerMovementModel();
        IPlayerMovementPresenter presenter = new PlayerMovementPresenter(view, model, joystickProvider, _tickHandler);
        
        _compositeDisposable.AddDisposable(presenter);

        IPlayerProvider playerProvider = new PlayerProvider(presenter);

        return playerProvider;
    }

    private async UniTask<ICameraProvider> InitializeCamera(CancellationToken token)
    {
        var view = await _resourceLoader.LoadResourceAsync<ICameraMovementView>(CameraMovementResourceId, _sceneContext.CameraMovementParent, token);
        ICameraMovementModel model = new CameraMovementModel();
        ICameraMovementPresenter presenter = new CameraMovementPresenter(view, model, _logger, _tickHandler, _config);
        _compositeDisposable.AddDisposable(presenter);

        ICameraProvider cameraProvider = new CameraProvider(presenter);
        _compositeDisposable.AddDisposable(cameraProvider);

        return cameraProvider;
    }
}
}