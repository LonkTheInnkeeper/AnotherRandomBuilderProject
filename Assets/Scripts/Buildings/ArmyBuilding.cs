using System.Collections.Generic;
using UnityEngine;

public class ArmyBuilding : MonoBehaviour
{
    public PopulationManager.ArmyType armyType;
    public int armySize;

    Building building;
    MapCell cell;
    List<MapCell> neighbors = new List<MapCell>();

    List<Army> armies = new List<Army>();

    ResourceManager resourceMan;
    PopulationManager populationMan;

    private void Start()
    {
        populationMan = PopulationManager.Instance;
        resourceMan = ResourceManager.Instance;

        building = GetComponent<Building>();
        cell = building.GetCell();
        neighbors = cell.GetNeighbors();
    }

    public void BuildArmy()
    {
        if (populationMan.availableVillagers.Count >= armySize)
        {
            List<Villager> villagers = new List<Villager>();
            MapCell spawnCell = SellectFreeNeighbor();

            if (spawnCell == null) return;

            for (int i = 0; i < armySize; i++)
            {
                Villager villager = populationMan.availableVillagers[0];
                populationMan.availableVillagers.Remove(villager);
                villager.SetProfession(PopulationManager.VillagerProfession.Soldier);
                villagers.Add(villager);
            }
         
            Army army = new Army(armyType, villagers, spawnCell);
            armies.Add(army);

            populationMan.UpdatePopulationState();
        }
    }

    MapCell SellectFreeNeighbor()
    {
        foreach (var cell in neighbors) 
        {
            if (cell.occupied || cell.character != null)
            {
                continue;
            }
            else return cell;
        }

        return null;
    }
}
