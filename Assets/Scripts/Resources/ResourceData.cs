public class ResourceData
{
    public int x; public int y;
    public ResourceManager.ResourceType resourceType;
    public int value;

    public ResourceData(int x, int y, ResourceManager.ResourceType resourceType, int value)
    {
        this.x = x;
        this.y = y;
        this.resourceType = resourceType;
        this.value = value;
    }
}
