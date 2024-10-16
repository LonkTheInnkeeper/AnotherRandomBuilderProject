using TMPro;
using UnityEngine;

public class BarracksUI : MonoBehaviour, IBuildableUI
{
    public BuildingsManager.BuildingType buildingType;
    [SerializeField] Transform panel;
    [SerializeField] TextMeshProUGUI workers;

    ArmyBuilding barracks;

    public void AddWorkers(bool add)
    {
        barracks.BuildArmy();
    }

    public void SetBuilding(Building building)
    {
        barracks = building.GetComponent<ArmyBuilding>();
    }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }
    public void TogglePanel(bool toggle) { panel.gameObject.SetActive(toggle); }
}
