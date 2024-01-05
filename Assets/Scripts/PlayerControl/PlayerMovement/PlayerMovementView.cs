using PlayerControl.PlayerMovement.BaseMVP;
using UnityEngine;

namespace PlayerControl.PlayerMovement
{
public class PlayerMovementView : MonoBehaviour, IPlayerMovementView
{
    [field: SerializeField]
    public Rigidbody Rigidbody { get; private set; }
    
    public Transform Transform => transform;
    
    private IPlayerMovementPresenter _presenter;

    public void Initialize(IPlayerMovementPresenter presenter)
    {
        _presenter = presenter;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}
}