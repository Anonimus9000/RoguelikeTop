using UnityEngine;

namespace UIContext
{
public interface IUIContext
{
    public Camera UICamera { get; }
    public RectTransform UIOverlayParent { get; }

    public RectTransform PopupsParent { get; }
    
    public RectTransform FullscreensParent { get; }
    
    public RectTransform JoystickParent { get; }
    public RectTransform GameplayUIElements { get; }
    public RectTransform DevParent { get; }
}
}