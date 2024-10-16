using TMPro;
using UnityEngine;

public class HouseUI : MonoBehaviour, IBuildableUI
{
    public BuildingsManager.BuildingType buildingType;
    [SerializeField] Transform panel;
    [SerializeField] TextMeshProUGUI storage;
    [SerializeField] TextMeshProUGUI population;

    House house;

    private void Update()
    {
        if (panel.gameObject.activeInHierarchy)
        {
            storage.text = "Stored food: " + house.storedFood + "/" + house.maxFood;
            population.text = "Population: " + house.villagers.Count + "/" + house.maxPopulation;
        }
    }

    public void AddWorkers(bool add)
    {
    }
    public void SetBuilding(Building building)
    {
        house = building.GetComponent<House>();
    }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }

    public void TogglePanel(bool toggle) { panel.gameObject.SetActive(toggle); }
}
