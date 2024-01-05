using UnityEngine;

namespace UIContext
{
//TODO: make UICanvasView and other MVP things
public class UnityUIContext : MonoBehaviour, IUIContext
{
    [field: SerializeField]
    public Camera UICamera { get; private set; }

    [field: SerializeField]
    public RectTransform UIOverlayParent { get; private set; }

    [field: SerializeField]
    public RectTransform PopupsParent { get; private set; }

    [field: SerializeField]
    public RectTransform FullscreensParent { get; private set; }

    [field: SerializeField]
    public RectTransform GameplayUIElements { get; private set; }

    [field: SerializeField]
    public RectTransform DevParent { get; private set; }

    [field: SerializeField]
    public RectTransform JoystickParent { get; private set; }
}
}