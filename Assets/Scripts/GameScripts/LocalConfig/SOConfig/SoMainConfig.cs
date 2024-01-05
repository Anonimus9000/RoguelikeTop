using GameScripts.LocalConfig.SOConfig.SoImages;
using UnityEngine;

namespace GameScripts.LocalConfig.SOConfig
{
[CreateAssetMenu(fileName = "MainConfig", menuName = "ScriptableObjects/Config/MainConfig", order = 1)]
public class SoMainConfig : ScriptableObject
{
    [field: SerializeField]
    public PlayerSoImage PlayerSoImage { get; private set; } 
    [field: SerializeField]
    public CameraSoImage CameraSoImage { get; private set; }
}
}