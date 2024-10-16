using TMPro;
using UnityEngine;

public class BarnUI : MonoBehaviour, IBuildableUI
{
    public BuildingsManager.BuildingType buildingType;
    [SerializeField] Transform panel;
    [SerializeField] TextMeshProUGUI workers;

    Barn barn;

    private void Update()
    {
        if (panel.gameObject.activeInHierarchy)
            workers.text = "Workers: " + barn.workers.ToString() + "/" + barn.maxWorkers.ToString();
    }

    public void AddWorkers(bool add)
    {
        barn.AddWorkers(add);
    }

    public void SetBuilding(Building building)
    {
        barn = building.GetComponent<Barn>();
    }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }
    public void TogglePanel(bool toggle) { panel.gameObject.SetActive(toggle); }
}
