using MVP;

namespace PlayerControl.PlayerMovement.BaseMVP
{
public interface IPlayerMovementModel : IModel
{
    public float GetPlayerSpeed();
    public float GetPlayerRotationSpeed();
}
}