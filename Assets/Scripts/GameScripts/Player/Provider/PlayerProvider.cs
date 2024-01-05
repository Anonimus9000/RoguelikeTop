using PlayerControl.PlayerMovement.BaseMVP;
using UnityEngine;

namespace GameScripts.Player.Provider
{
public class PlayerProvider : IPlayerProvider
{
    private readonly IPlayerMovementPresenter _playerMovementPresenter;

    public PlayerProvider(IPlayerMovementPresenter playerMovementPresenter)
    {
        _playerMovementPresenter = playerMovementPresenter;
    }

    public Transform GetPlayerTransform()
    {
        return _playerMovementPresenter.GetTransform();
    }

    public void Dispose()
    {
    }
}
}