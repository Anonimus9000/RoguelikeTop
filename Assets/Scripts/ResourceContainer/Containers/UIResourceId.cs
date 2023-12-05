namespace ResourceContainer.Containers
{
public struct UIResourceId
{
    public string GroupId { get; }
    public string VirtualJoystickView { get; }
    
    public UIResourceId(string groupId)
    {
        GroupId = groupId;
        VirtualJoystickView = "VirtualJoystickView";
    }
}
}