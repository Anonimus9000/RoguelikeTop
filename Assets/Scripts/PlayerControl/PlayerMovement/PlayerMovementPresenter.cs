using PlayerControl.PlayerMovement.BaseMVP;

namespace PlayerControl.PlayerMovement
{
public class PlayerMovementPresenter : IPlayerMovementPresenter
{
    private IPlayerMovementModel _model;
    private IPlayerMovementView _view;
    
    public void Dispose()
    {
    }
}
}