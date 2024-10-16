using UnityEngine;

public interface IResource
{
    void Init();
    void GatherResource();
    ResourceManager.ResourceType GetResourceType();
    Transform GetTransform();
    Mesh GetVisual();
    void SetCell(MapCell cell);
    MapCell GetCell();
    ResourceData GetData();
    void DestroyResource();
}
