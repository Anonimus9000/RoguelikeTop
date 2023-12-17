using MVP;
using PlayerControl.PlayerMovement.BaseMVP;
using UnityEngine;

namespace PlayerControl.PlayerMovement
{
public class PlayerMovementView : MonoBehaviour, IPlayerMovementView
{
    private IPlayerMovementPresenter _presenter;

    public void Initialize(IPlayerMovementPresenter presenter)
    {
        _presenter = presenter;
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
}