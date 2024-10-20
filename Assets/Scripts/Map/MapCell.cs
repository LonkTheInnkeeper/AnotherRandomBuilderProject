using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;

public class MapCell
{
    CellBlock cellBlock;

    Vector3 coordinates;
    IResource resource;
    Building building;

    MapGrid grid;
    List<MapCell> neighbors = new List<MapCell>();

    public bool occupied = false;
    public int x;
    public int y;
    public ResourceManager.ResourceType resourceType = ResourceManager.ResourceType.None;
    public BuildingsManager.BuildingType buildingType = BuildingsManager.BuildingType.None;

    // Buildabel Area
    public bool isInArea;
    public int areaIndex;

    // Pathfinding SetUp
    public float cost = 1;          // Movement cost
    public float gCost;             // Cost from start tile to this tile
    public float hCost;             // Heuristic cost to target tile
    public float FCost => gCost + hCost;
    public MapCell parent;

    public ICharacter character;

    public MapCell(float x, float y, float z, Vector3 originPoint, MapGrid grid, int xID, int yID)
    {
        coordinates = new Vector3(x, y, z) + originPoint;
        this.grid = grid;

        this.x = xID;
        this.y = yID;
    }

    public void SetBuildableAre(bool isInArea, int areaIndex)
    {
        this.isInArea = isInArea;
        this.areaIndex = areaIndex;
    }

    public void SetCellBlock(CellBlock cellBlock)
    {
        this.cellBlock = cellBlock;
    }

    public void Init()
    {
        FindNeighbours(x, y);
        cellBlock.SetCellText(GetID().ToString());
    }

    public void ResourceDepleted()
    {
        resource = null;
        resourceType = ResourceManager.ResourceType.None;
        occupied = false;
    }

    public void DeleteOccupant()
    {
        if (building != null)
        {
            BuildingsManager.Instance.buildingsOnMap.Remove(building);
            building.DestroyBuilding();
            building = null;
            buildingType = BuildingsManager.BuildingType.None;
            occupied = false;
        }

        if (resource != null && GameManager.Instance.editing)
        {
            ResourceManager.Instance.resources.Remove(resource.GetTransform());
            ResourceManager.Instance.resourceDatas.Remove(resource.GetData());
            resource.DestroyResource();
            resource = null;
            resourceType = ResourceManager.ResourceType.None;
            occupied = false;
        }

    }
    public void FindNeighbours(int x, int y)
    {
        Vector2Int[] directionsEven = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-2, 0),
            new Vector2Int(-1, 0),
        };

        Vector2Int[] directionsOdd = new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(2, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(-1, 1),
        };

        if (x % 2 == 0)
        {
            foreach (Vector2Int direction in directionsEven)
            {
                Vector2Int id = GetID() + direction;
                if (Tools.InGridRange(grid, id))
                {
                    MapCell neighbor = MapManager.Instance.GetCellByID(0, id);
                    neighbors.Add(neighbor);
                }
            }
        }
        else
        {
            foreach (Vector2Int direction in directionsOdd)
            {
                Vector2Int id = GetID() + direction;
                if (Tools.InGridRange(grid, id))
                {
                    MapCell neighbor = MapManager.Instance.GetCellByID(0, id);
                    neighbors.Add(neighbor);
                }
            }
        }
    }
    public Vector3 GetCoordinates() { return coordinates; }
    public Vector2Int GetID() { return new Vector2Int(x, y); }
    public List<MapCell> GetNeighbors() { return neighbors; }
    public void SetBuilding(Building building) { this.building = building; }
    public Building GetBuilding() { return building; }
    public void SetResource(IResource resource)
    {
        this.resource = resource;
        if (resource == null) Debug.Log("No resource");
        resourceType = resource.GetResourceType();

        if (resourceType != ResourceManager.ResourceType.Forest)
        {
            occupied = true;
        }
    }
    public IResource GetResource() { return resource; }
    public void SetCellText(string text)
    {
        cellBlock.SetCellText(text);
    }
    public void DisplayNavigation()
    {
        cellBlock.SetCellText(GetID() + "\n" + gCost + "   " + hCost + "\n" + FCost);
    }
    public void IsPath(bool path)
    {
        cellBlock.IsPath(path);
    }
    public bool CheckRoad()
    {
        if (building != null && building.GetBuildingType() == BuildingsManager.BuildingType.Road) return true;
        return false;
    }
}
