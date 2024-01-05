using MVP;
using UnityEngine;

namespace PlayerControl.PlayerMovement.BaseMVP
{
public interface IPlayerMovementView : IView<IPlayerMovementPresenter>
{
    public Rigidbody Rigidbody { get; }
    public Transform Transform { get; }
}
}