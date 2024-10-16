public interface IBuildableUI
{
    void AddWorkers(bool add);
    void SetBuilding(Building building);
    BuildingsManager.BuildingType GetBuildingType();
    void TogglePanel(bool toggle);
}