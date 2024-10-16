using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceEditor : MonoBehaviour
{
    MapManager mapMan;
    ResourceManager resourceMan;

    private void Start()
    {
        mapMan = MapManager.Instance;
        resourceMan = ResourceManager.Instance;
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Editing) return;

        if (!resourceMan.deleting)
            Edit();
        else
            Delete();
    }

    void Edit()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            mapMan.SetMapCell();
            MapCell cell = mapMan.activeCell;

            if (cell == null) return;

            IResource activeResource = resourceMan.GetActiveResource().GetComponent<IResource>();

            if (activeResource.GetResourceType() == ResourceManager.ResourceType.Field && CheckMillNeighbor(cell))
                AddResource(cell, activeResource);
            else if (activeResource.GetResourceType() != ResourceManager.ResourceType.Field)
                AddResource(cell, activeResource);
        }
    }

    void Delete()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            mapMan.SetMapCell();
            MapCell cell = mapMan.activeCell;

            cell.DeleteOccupant();
        }
    }

    public void LoadResource(List<ResourceData> resources)
    {
        foreach (ResourceData resource in resources)
        {
            MapCell cell = mapMan.GetCellByID(0, new Vector2Int(resource.x, resource.y));
            resourceMan.activeResource = resource.resourceType;
            AddResource(cell, resourceMan.GetActiveResource().GetComponent<IResource>());
        }
    }

    void AddResource(MapCell cell, IResource activeResource)
    {
        Vector3 position = cell.GetCoordinates();

        if (cell.GetResource() != null)
        {
            print($"This cell is {cell.GetResource().GetResourceType()}");
            return;
        }
        if (cell.GetBuilding() != null)
        {
            print($"This cell is {cell.GetBuilding().GetBuildingType()}");
            return;
        }

        GameObject resourceObject = Instantiate(activeResource.GetTransform().gameObject, position, Quaternion.identity);
        IResource resource = resourceObject.GetComponent<IResource>();
        cell.SetResource(resource);
        resource.SetCell(cell);
        resource.Init();

        if (activeResource.GetResourceType() == ResourceManager.ResourceType.Field)
        {
            foreach (MapCell cell_ in cell.GetNeighbors())
            {
                Building building = cell_.GetBuilding();
                if (building != null && building.GetBuildingType() == BuildingsManager.BuildingType.Mill)
                {
                    building.GetComponent<ProductionBuilding>().GetResources();
                }
            }
        }
    }

    bool CheckMillNeighbor(MapCell cell)
    {
        bool millNeighbor = false;

        foreach (MapCell neighbor in cell.GetNeighbors())
        {
            if (neighbor.GetBuilding() != null && neighbor.GetBuilding().GetBuildingType() == BuildingsManager.BuildingType.Mill)
            {
                millNeighbor = true;
                break;
            }
        }

        return millNeighbor;
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
