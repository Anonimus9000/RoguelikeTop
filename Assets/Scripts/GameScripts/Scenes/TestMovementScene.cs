using System.Threading;
using Cysharp.Threading.Tasks;
using SceneSwitcher;

namespace GameScripts.Scenes
{
public class TestMovementScene : IScene
{
    public string SceneId { get; }

    ISceneContext IScene.SceneContext => _sceneContext;
    ISceneSwitcher IScene.SceneSwitcher => _sceneSwitcher;
    
    private ISceneContext _sceneContext;
    private ISceneSwitcher _sceneSwitcher;

    public UniTaskVoid InitializeAsync(CancellationToken token)
    {
        
    }

    public void Dispose()
    {
    }
}
}