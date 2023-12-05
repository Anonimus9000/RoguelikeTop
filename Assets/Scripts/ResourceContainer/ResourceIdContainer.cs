using ResourceContainer.Containers;

namespace ResourceContainer
{
public static class ResourceIdContainer
{
    private const string UIResourcesGroupId = "UIResources";
    public static UIResourceId UIResourceId { get; }

    static ResourceIdContainer()
    {
        UIResourceId = new UIResourceId(UIResourcesGroupId);
    }
}
}