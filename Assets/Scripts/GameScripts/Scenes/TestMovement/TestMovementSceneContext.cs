using SceneSwitcher;
using UnityEngine;

namespace GameScripts.Scenes.TestMovement
{
public class TestMovementSceneContext : MonoBehaviour, ISceneContext
{
    [field: SerializeField]
    public Transform PlayerParent { get; private set; }

    [field: SerializeField]
    public Transform CameraMovementParent { get; set; }
}
}