using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager Instance;

    public List<Building> buildingsPrefs;
    public List<Building> buildingsOnMap;
    public BuildingType activeBuilding;
    public BuildingType[] buildingTypes;
    public Ghost ghost;

    InputManager input;

    int activeType = 0;
    public int rotationIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public enum BuildingType
    {
        House,
        LumberJack,
        StoneCutter,
        Mine,
        CrystalCutter,
        Barn,
        Barracks,
        ShootingRange,
        Road,
        Mill,
        Field,
        Wall,
        Tower,
        Gate,
        None
    }

    private void Start()
    {
        buildingTypes = (BuildingType[])System.Enum.GetValues(typeof(BuildingType));
        input = InputManager.Instance;
    }

    private void Update()
    {
        if (input.Rotate())
        {
            rotationIndex++;

            if (rotationIndex == 6) rotationIndex = 0;
        }
    }

    public void UIBuildingClic(int ID)
    {
        activeBuilding = buildingTypes[ID];
        ghost.UpdateVisual();
        GameManager.Instance.SetGameState(GameManager.GameState.Constructing);
    }

    public Building GetActiveBuilding()
    {
        foreach (Building building in buildingsPrefs)
        {
            if (building.GetBuildingType() == activeBuilding)
            {
                return building;
            }
            else continue;
        }

        return null;
    }
}
