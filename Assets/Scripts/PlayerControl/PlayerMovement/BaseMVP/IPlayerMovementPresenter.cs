using MVP;
using UnityEngine;

namespace PlayerControl.PlayerMovement.BaseMVP
{
public interface IPlayerMovementPresenter : IPresenter
{
    public void Initialize();
    public Transform GetTransform();
}
}