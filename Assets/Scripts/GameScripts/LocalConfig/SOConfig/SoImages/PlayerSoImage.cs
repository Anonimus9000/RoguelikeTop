using UnityEngine;

namespace GameScripts.LocalConfig.SOConfig.SoImages
{
[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/Config/PlayerConfig", order = 2)]
public class PlayerSoImage : ScriptableObject
{
    [field: SerializeField]
    public float MoveSpeed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }
}
}