using UnityEngine;

namespace UIContext
{
public interface IUIContext
{
    public RectTransform UIOverlayParent { get; }

    public RectTransform PopupsParent { get; }
    
    public RectTransform FullscreensParent { get; }
    
    public RectTransform JoystickParent { get; }
    public RectTransform GameplayUIElements { get; }
    public RectTransform DevParent { get; }
}
}