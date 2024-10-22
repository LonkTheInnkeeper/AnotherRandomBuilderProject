using System.Collections.Generic;
using UnityEngine;
using static PopulationManager;

[CreateAssetMenu(fileName = "Buildable Area", menuName = "Map/Buildable Area")]
public class BuildableArea : ScriptableObject
{
    [SerializeField] int size;
    [SerializeField] Vector2Int originID;

    int areaIndex;

    public MapCell originCell;
    List<MapCell> area = new List<MapCell>();
    List<Building> buildings = new List<Building>();

    [Header("Population")]
    public int startupPopulation;
    public int maxPopulaton;
    public int workers;
    List<Villager> villagers = new List<Villager>();
    List<Villager> availableVillagers = new List<Villager>();

    [Header("Resources")]
    public int wood;
    public int stone;
    public int iron;
    public int food;
    public int crystal;

    bool noFood = false;

    MapManager mapMan;
    PopulationManager popMan;

    public void GenerateArea(int areaIndex)
    {
        this.areaIndex = areaIndex;
        mapMan = MapManager.Instance;
        popMan = PopulationManager.Instance;
        originCell = mapMan.GetCellByID(0, originID);

        List<MapCell> exploring = new List<MapCell>();
        List<MapCell> toExplore = new List<MapCell>();

        area.Add(originCell);
        exploring.AddRange(originCell.GetNeighbors());

        for (int i = 0; i < size; i++)
        {
            foreach (MapCell cell in exploring)
            {
                foreach (MapCell exploringCell in cell.GetNeighbors())
                {
                    if (!area.Contains(exploringCell) &&
                        !toExplore.Contains(exploringCell) &&
                        !exploring.Contains(exploringCell))
                    {
                        toExplore.Add(exploringCell);
                    }
                }
            }

            area.AddRange(exploring);
            exploring = toExplore;
            toExplore = new List<MapCell>();
        }

        foreach (MapCell cell in area)
        {
            cell.SetBuildableAre(true, areaIndex);
        }

        if (mapMan.areaDebug)
            AreaDebug();

        PopulationManager.Instance.SpawnVillager(startupPopulation, areaIndex);
    }

    private void AreaDebug()
    {
        foreach (MapCell cell in area)
        {
            cell.SetCellText($"{cell.GetID().ToString()}\nArea {areaIndex.ToString()}");
        }
    }

    public void AddVillager(Villager villager) { villagers.Add(villager); }
    public void RemoveVillager(Villager villager) 
    {
        villagers.Remove(villager);
        if (availableVillagers.Contains(villager))
        {
            availableVillagers.Remove(villager);
        }
    }
    public void UpdateMaxPopulation()
    {
        List<House> houses = GetAllHouses();


        if (houses.Count > 0)
            maxPopulaton = houses.Count * houses[0].maxPopulation;
        else maxPopulaton = 0;
    }

    public void UpdatePopulation()
    {
        List<House> houses = GetAllHouses();

        if (villagers.Count == maxPopulaton && food > 0) return;

        // Village can spawn a villager
        else if (villagers.Count < maxPopulaton && food > 0 && houses.Count > 0 && !noFood)
        {
            int amount = UnityEngine.Random.Range(0, houses.Count);

            if (amount > maxPopulaton - villagers.Count)
                amount = maxPopulaton - villagers.Count;

            popMan.SpawnVillager(amount, areaIndex);
        }

        // Village has to kick out a villager
        else if (villagers.Count > 0 && food <= 0 && houses.Count > 0 || noFood)
        {
            popMan.DespawnVillager(Mathf.FloorToInt(UnityEngine.Random.Range(1, villagers.Count / 3)), areaIndex);
        }

        UpdateFood(-(villagers.Count / 2));
        UpdatePopulationState();
    }

    private List<House> GetAllHouses()
    {
        List<House> houses = new List<House>();

        foreach (Building building in buildings)
        {
            if (building.GetBuildingType() == BuildingsManager.BuildingType.House)
            {
                houses.Add(building.GetComponent<House>());
            }
        }
        
        return houses;
    }

    public void UpdateFood(int food)
    {
        this.food += food;

        if (this.food <= 0)
        {
            this.food = 0;
            noFood = true;
        }
        else
        {
            noFood = false;
        }
    }

    public void UpdatePopulationState()
    {
        List<Villager> freeVillagers = new List<Villager>();
        workers = 0;

        foreach (Villager villager in villagers)
        {
            switch (villager.GetProfession())
            {
                case VillagerProfession.None:
                    freeVillagers.Add(villager);
                    break;
                case VillagerProfession.Worker:
                    workers++;
                    break;
                case VillagerProfession.Soldier:
                    villagers.Remove(villager);
                    break;
                default:
                    break;
            }
        }

        availableVillagers = freeVillagers;
    }

    public List<Villager> GetAvailableVillagers() {  return availableVillagers; }
    public int GetVillagerCount() { return availableVillagers.Count; }
}
