using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    MapManager mapMan;
    BuildingsManager buildingsMan;
    InputManager input;

    private void Start()
    {
        mapMan = MapManager.Instance;
        buildingsMan = BuildingsManager.Instance;
        input = InputManager.Instance;
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Constructing) return;
        Building();
    }

    void Building()
    {
        if (input.LeftMouseClick() && !IsPointerOverUIElement())
        {
            MapCell cell = mapMan.activeCell;

            if (cell == null) return;

            Vector3 position = cell.GetCoordinates();

            Building activeBuilding = buildingsMan.GetActiveBuilding();

            foreach (MapCell _cell in GetBuildingSize(cell, activeBuilding))
            {
                if (_cell.occupied || !_cell.isInArea) return;
            }

            if (activeBuilding.GetBuildingType() != BuildingsManager.BuildingType.Mine &&
                !cell.occupied)
            {
                if (cell.GetResource() != null)
                    cell.GetResource().DestroyResource();

                PlaceBuilding(activeBuilding, cell, position);
            }

            else if (activeBuilding.GetBuildingType() == BuildingsManager.BuildingType.Mine &&
                     cell.GetResource() != null &&
                     cell.GetBuilding() == null &&
                     cell.GetResource().GetResourceType() == ResourceManager.ResourceType.Ore)
            {
                print("Placing mine");
                PlaceBuilding(activeBuilding, cell, position);
            }
        }
    }

    void PlaceBuilding(Building buildingToBuild, MapCell cell, Vector3 position)
    {
        var building = Instantiate(buildingToBuild, position, Quaternion.identity, mapMan.grids[0].buildingParent);
        cell.SetBuilding(building);
        building.SetCell(cell);
        building.SetRotation(buildingsMan.rotationIndex);
        buildingsMan.buildingsOnMap.Add(building.GetComponent<Building>());
        cell.occupied = true;
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    List<MapCell> GetBuildingSize(MapCell activeCell, Building activeBuilding)
    {
        List<MapCell> building = new List<MapCell>();
        int size = activeBuilding.GetSize();

        building.Add(activeCell);

        if (size > 1)
        {
            List<MapCell> neighbors = activeCell.GetNeighbors();

            for (int i = 0; i < size - 1; i++)
            {
                int index = i + buildingsMan.rotationIndex;

                if (index > neighbors.Count - 1)
                {
                    index -= neighbors.Count;
                }

                building.Add(neighbors[index]);
            }
        }

        return building;
    }
}
