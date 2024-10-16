using UnityEngine;

public class ResourceHandler : MonoBehaviour, IResource
{
    public ResourceManager.ResourceType resourceType;
    public Mesh visual;
    MapCell cell;
    ResourceData data;

    public int maxValue;

    public void Init()
    {
        data = new ResourceData(cell.GetID().x, cell.GetID().y, resourceType, maxValue);
        ResourceManager.Instance.resourceDatas.Add(data);
    }

    public void GatherResource()
    {
        data.value--;
        if (data.value == 0)
        {
            cell.ResourceDepleted();
            DestroyResource();
        }
    }

    public ResourceManager.ResourceType GetResourceType() { return resourceType; }
    public Transform GetTransform() { return transform; }
    public Mesh GetVisual() { return visual; }
    public void SetCell(MapCell cell) { this.cell = cell; }
    public MapCell GetCell() { return cell; }
    public ResourceData GetData() { return data; }
    public void DestroyResource()
    {
        data = null;
        cell.resourceType = ResourceManager.ResourceType.None;
        cell = null;
        visual = null;
        Destroy(this.gameObject);
    }
}