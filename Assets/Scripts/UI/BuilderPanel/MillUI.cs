using TMPro;
using UnityEngine;

public class MillUI : MonoBehaviour, IBuildableUI
{
    public BuildingsManager.BuildingType buildingType;
    [SerializeField] Transform panel;
    [SerializeField] TextMeshProUGUI available;
    [SerializeField] TextMeshProUGUI storage;
    [SerializeField] TextMeshProUGUI workers;

    ProductionBuilding mill;

    private void Update()
    {
        if (panel.gameObject.activeInHierarchy)
        {
            available.text = "Available food: " + mill.available.ToString();
            storage.text = "Stored food: " + mill.storage.ToString() + "/" + mill.maxStorage.ToString();
            workers.text = "Workers: " + mill.GetWorkersCount().ToString() + "/" + mill.maxWorkers.ToString();
        }
    }

    public void AddWorkers(bool add)
    {
        mill.AddWorkers(add);
    }

    public void BuildField()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Editing);
        ResourceManager.Instance.activeResource = ResourceManager.ResourceType.Field;
        BuildingsManager.Instance.ghost.UpdateVisual();
    }

    public void SetBuilding(Building building)
    {
        mill = building.GetComponent<ProductionBuilding>();
    }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }
    public void TogglePanel(bool toggle) { panel.gameObject.SetActive(toggle); }
}
