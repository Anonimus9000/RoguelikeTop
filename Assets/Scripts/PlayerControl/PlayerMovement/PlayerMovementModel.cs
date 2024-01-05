using PlayerControl.PlayerMovement.BaseMVP;

namespace PlayerControl.PlayerMovement
{
public class PlayerMovementModel : IPlayerMovementModel
{
    private readonly PlayerData _playerData;

    public PlayerMovementModel(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void Dispose()
    {
    }

    public float GetPlayerSpeed()
    {
        return _playerData.MoveSpeed;
    }

    public float GetPlayerRotationSpeed()
    {
        return _playerData.RotationSpeed;
    }
}
}