using System.Threading;
using Config.MainConfig;
using Cysharp.Threading.Tasks;
using GameScripts.LocalConfig.Images;
using GameScripts.Player.Provider;
using GameScripts.ResourceIdContainer;
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
using UnityEngine;

namespace GameScripts.Scenes.TestMovement
{
public class TestMovementScene : IScene
{
    private readonly IResourceLoader _resourceLoader;
    private readonly ITickHandler _tickHandler;
    private readonly IUIContext _uiContext;
    private readonly IInGameLogger _logger;
    private readonly IConfig _config;
    public string SceneId => "TestMovementScene";

    ISceneContext IScene.SceneContext => _sceneContext;
    ISceneSwitcher IScene.SceneSwitcher => _sceneSwitcher;
    
    private TestMovementSceneContext _sceneContext;
    private readonly ISceneSwitcher _sceneSwitcher;
    private readonly ICompositeDisposable _compositeDisposable = new CompositeDisposable();

    public TestMovementScene(IResourceLoader resourceLoader,
        ITickHandler tickHandler,
        IUIContext uiContext,
        IInGameLogger logger,
        IConfig config,
        ISceneSwitcher sceneSwitcher)
    {
        _resourceLoader = resourceLoader;
        _tickHandler = tickHandler;
        _uiContext = uiContext;
        _logger = logger;
        _config = config;
        _sceneSwitcher = sceneSwitcher;
    }
    
    public async UniTaskVoid SwitchScene(CancellationToken token)
    {
        _sceneContext = await _sceneSwitcher.SwitchToSceneAsync<TestMovementSceneContext>(SceneId, token);
        
        var joystickProvider = await InitializeVirtualJoystickAsync();
        
        var cameraProvider = await InitializeCameraAsync(token);
        var uiCamera = _uiContext.UICamera;
        cameraProvider.AddCameraInStack(uiCamera);
        
        var playerProvider = await InitializePlayerAsync(joystickProvider, token);

        var playerTransform = playerProvider.GetPlayerTransform();
        cameraProvider.FollowToTarget(playerTransform);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }

    private async UniTask<IJoystickProvider> InitializeVirtualJoystickAsync()
    {
        IJoystickProvider joystickProvider = new JoystickProvider(_uiContext, _resourceLoader);
        _compositeDisposable.AddDisposable(joystickProvider);

        return joystickProvider;
    }

    private async UniTask<IPlayerProvider> InitializePlayerAsync(IJoystickProvider joystickProvider, CancellationToken token)
    {
        var playerImage = _config.GetImage<PlayerImage>();
        var playerData = new PlayerData(playerImage.MoveSpeed, playerImage.RotationSpeed);
        
        var playerParent = _sceneContext.PlayerParent;
        var playerMovementResourceId = ResourceLoaderIdContainer.PlayerMovementView;
        var view = await _resourceLoader.LoadResourceAsync<IPlayerMovementView>(playerMovementResourceId, playerParent, token);
        IPlayerMovementModel model = new PlayerMovementModel(playerData);
        IPlayerMovementPresenter presenter = new PlayerMovementPresenter(view, model, joystickProvider, _tickHandler);
        presenter.Initialize();
        
        IPlayerProvider playerProvider = new PlayerProvider(presenter);
        
        _compositeDisposable.AddDisposable(presenter, playerProvider);
        
        return playerProvider;
    }

    private async UniTask<ICameraProvider> InitializeCameraAsync(CancellationToken token)
    {
        var cameraMovementResourceId = ResourceLoaderIdContainer.CameraMovementView;
        var cameraImage = _config.GetImage<CameraImage>();
        var cameraData = new CameraData(
            cameraImage.CinematicMoveToTargetDuration,
            cameraImage.DelayOnTargetDuration,
            cameraImage.CameraTargetOffset,
            cameraImage.CameraSmoothSpeed,
            cameraImage.CameraFieldOfView);
        
        var view = await _resourceLoader.LoadResourceAsync<ICameraMovementView>(cameraMovementResourceId, _sceneContext.CameraMovementParent, token);
        ICameraMovementModel model = new CameraMovementModel(cameraData);
        ICameraMovementPresenter presenter = new CameraMovementPresenter(view, model, _logger, _tickHandler);
        presenter.Initialize();
        _compositeDisposable.AddDisposable(presenter);

        ICameraProvider cameraProvider = new CameraProvider(presenter);
        _compositeDisposable.AddDisposable(cameraProvider);

        return cameraProvider;
    }
}
}