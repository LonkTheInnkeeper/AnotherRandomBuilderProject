using TMPro;
using UnityEngine;

public class ShootingRangeUI : MonoBehaviour, IBuildableUI
{
    public BuildingsManager.BuildingType buildingType;
    [SerializeField] Transform panel;
    [SerializeField] TextMeshProUGUI workers;

    ArmyBuilding shootingRange;

    public void AddWorkers(bool add)
    {
        shootingRange.BuildArmy();
    }

    public void SetBuilding(Building building)
    {
        shootingRange = building.GetComponent<ArmyBuilding>();
    }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }
    public void TogglePanel(bool toggle) { panel.gameObject.SetActive(toggle); }
}
