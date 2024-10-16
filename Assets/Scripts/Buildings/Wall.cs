using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Building building;
    List<MapCell> neighbors = new List<MapCell>();

    private void Start()
    {
        building = GetComponent<Building>();
        neighbors = building.GetCell().GetNeighbors();
        UpdateTowers();
    }

    void UpdateTowers()
    {
        foreach (MapCell cell in neighbors)
        {
            Building cellBuilding;
            if (cell.GetBuilding() != null)
            {
                cellBuilding = cell.GetBuilding();
            }
            else continue;

            if (cellBuilding.GetBuildingType() == BuildingsManager.BuildingType.Tower)
            {
                cellBuilding.GetComponent<Tower>().GetWalls();
            }
        }
    }
}
