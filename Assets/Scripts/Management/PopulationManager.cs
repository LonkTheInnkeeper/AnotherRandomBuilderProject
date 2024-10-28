using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance;

    [Header("Values")]
    public int soldiers;

    public List<Villager> currentPopulation = new List<Villager>();
    public List<Villager> availableVillagers = new List<Villager>();

    public List<string> maleNames = new List<string>();
    public List<string> femaleNames = new List<string>();

    [Header("UI")]
    public TextMeshProUGUI populationCounter;

    [Header("Prefabs")]
    public Transform villagerPref;

    public MapCell spawn;

    ResourceManager resourceMan;
    BuildingsManager buildingsMan;
    InputManager input;
    MapManager mapMan;
    GameManager gameMan;

    public enum VillagerProfession
    {
        None,
        Worker,
        Soldier
    }
    public enum VillagerGender
    {
        Male,
        Female
    }
    public enum ArmyType
    {
        Infantry,
        Shooters
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        resourceMan = ResourceManager.Instance;
        buildingsMan = BuildingsManager.Instance;
        input = InputManager.Instance;
        mapMan = MapManager.Instance;
        gameMan = GameManager.Instance;

        LoadNames();
    }
    public void GridGenerated()
    {
        spawn = MapManager.Instance.grids[0].GetGridCells()[0];
    }

    void LoadNames()
    {
        maleNames = SaveLoadSystem.LoadStringList("Population/MaleNames.json");
        femaleNames = SaveLoadSystem.LoadStringList("Population/FemaleNames.json");
    }

    public List<Villager> GetVillagersToWork(int amount, VillagerProfession profession, Building workingPlace, int areaIndex)
    {
        BuildableArea area = mapMan.buildableAreas[areaIndex];
        List<Villager> villagers = new List<Villager>();
        List<Villager> availableVillagers = area.GetAvailableVillagers();

        if (amount > 0 && availableVillagers.Count >= amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var villager = availableVillagers[i];
                villager.SetProfession(profession);
                villager.SetWorkingPlace(workingPlace);
                villagers.Add(villager);
            }

            area.UpdatePopulationState();
            return villagers;
        }

        else
            return null;

    }

    public List<Villager> GetSoldiers(int amount)
    {
        List<Villager> soldiers = new List<Villager>();
        BuildableArea area = gameMan.activeArea;

        if (amount > 0 && availableVillagers.Count >= amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var soldier = area.GetAvailableVillagers()[i];
                soldiers.Add(soldier);
                soldier.SetProfession(VillagerProfession.Soldier);
            }

            area.UpdatePopulationState();
            return soldiers;
        }

        else return null;

    }
    public void SpawnVillager(int amount, int areaIndex)
    {
        for (int i = 0; i < amount; i++)
        {
            var villager = new Villager(areaIndex);
            currentPopulation.Add(villager);

            var villager_ = Instantiate(villagerPref, Vector3.zero, Quaternion.identity);
            villager_.GetComponent<VillagerBody>().Init(villager);

            MapManager.Instance.buildableAreas[areaIndex].AddVillager(villager);
        }

        UIManager.Instance.MessagePanel($"{amount} villagers arrived.");
    }
    public void DespawnVillager(int amount, int areaIndex)
    {
        BuildableArea area = MapManager.Instance.buildableAreas[areaIndex];
        List<Villager> availableVillagers = area.GetAvailableVillagers();

        if (availableVillagers.Count == 0) return;

        UIManager.Instance.MessagePanel($"{amount} villagers left.");
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, availableVillagers.Count);
            var villager = availableVillagers[index];

            villager.GetBody().Despawning();
            area.RemoveVillager(villager);
        }

    }

    public void UpdatePopulation()
    {
        foreach (var area in mapMan.buildableAreas)
        {
            area.UpdatePopulation();
        }
    }
}
