using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    Building building;
    List<MapCell> neighbors;
    public List<Building> neighborWalls;

    private void Start()
    {
        building = GetComponent<Building>();
        neighbors = building.GetCell().GetNeighbors();
        GetWalls();
    }

    public void GetWalls()
    {
        List<Building> walls = new List<Building>();

        foreach (MapCell cell in neighbors)
        {
            Building cellBuilding;
            if (cell.GetBuilding() != null)
                cellBuilding = cell.GetBuilding();
            else continue;

            if (cellBuilding.GetBuildingType() == BuildingsManager.BuildingType.Wall)
            {
                walls.Add(cellBuilding);
            }
        }

        neighborWalls = walls;
    }
}
