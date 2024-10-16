using TMPro;
using UnityEngine;

public class LumberJackUI : MonoBehaviour, IBuildableUI
{
    public BuildingsManager.BuildingType buildingType;
    [SerializeField] Transform panel;
    [SerializeField] TextMeshProUGUI available;
    [SerializeField] TextMeshProUGUI storage;
    [SerializeField] TextMeshProUGUI workers;

    ProductionBuilding productionBuilding;

    private void Update()
    {
        if (panel.gameObject.activeInHierarchy)
        {
            available.text = "Available wood: " + productionBuilding.available.ToString();
            storage.text = "Stored wood: " + productionBuilding.storage.ToString() + "/" + productionBuilding.maxStorage.ToString();
            workers.text = "Workers: " + productionBuilding.GetWorkersCount().ToString() + "/" + productionBuilding.maxWorkers.ToString();
        }
    }

    public void AddWorkers(bool add)
    {
        productionBuilding.AddWorkers(add);
    }

    public void SetBuilding(Building building)
    {
        productionBuilding = building.GetComponent<ProductionBuilding>();
    }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }
    public void TogglePanel(bool toggle) { panel.gameObject.SetActive(toggle); }
}
