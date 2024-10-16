using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public List<Villager> villagers = new List<Villager>();
    public int maxPopulation = 5;
    public int storedFood = 0;
    public int maxFood = 0;

    private Building building;
    private MapCell cell;

    private void Start()
    {
        building = GetComponent<Building>();
        cell = building.GetCell();

        maxFood = maxPopulation;
        PopulationManager.Instance.UpdateMaxPopulation();
        FindVillagers();
    }

    void FindVillagers()
    {
        if (villagers.Count == maxPopulation) return;

        List<Villager> homeless = new List<Villager> ();

        foreach (Villager villager in PopulationManager.Instance.availableVillagers)
        {
            if (villager.IsHomeless())
                homeless.Add (villager);
        }

        if (homeless.Count == 0) return;

        while(villagers.Count < maxPopulation && homeless.Count > 0)
        {
            villagers.Add(homeless[0]);
            homeless[0].SetHome(this);
            homeless.RemoveAt(0);
        }

        foreach (Villager villager in villagers)
        {
            print(villager.GetName());
        }
    }

    public Building GetBuilding() { return building; }
    public MapCell GetCell() { return cell; }
}
