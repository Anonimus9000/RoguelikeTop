using UnityEngine;

namespace GameScripts.Player.Provider
{
public interface IPlayerProvider
{
    public Transform GetPlayerTransform();
}
}