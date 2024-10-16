using TMPro;
using UnityEngine;

public class StoneCutterUI : MonoBehaviour, IBuildableUI
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
            available.text = "Available stone: " + productionBuilding.available.ToString();
            storage.text = "Stored stone: " + productionBuilding.storage.ToString() + "/" + productionBuilding.maxStorage.ToString();
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
