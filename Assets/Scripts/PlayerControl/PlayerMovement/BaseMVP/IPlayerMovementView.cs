using MVP;
using UnityEngine;

namespace PlayerControl.PlayerMovement.BaseMVP
{
public interface IPlayerMovementView : IView<IPlayerMovementPresenter>
{
    public Transform Transform { get; }
}
}