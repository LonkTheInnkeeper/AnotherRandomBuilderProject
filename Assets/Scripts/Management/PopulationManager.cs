using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance;

    [Header("Values")]
    public int startupPopulation;
    public int maxPopulaton;
    public int workers;
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

        LoadNames();
        UpdateMaxPopulation();
    }

    private void Update()
    {
        if (input.Debug())
        {
            DespawnVillager(1);
        }
    }

    public void GridGenerated()
    {
        spawn = MapManager.Instance.grids[0].GetGridCells()[0];
        SpawnVillager(startupPopulation);
    }

    void LoadNames()
    {
        maleNames = SaveLoadSystem.LoadStringList("Population/MaleNames.json");
        femaleNames = SaveLoadSystem.LoadStringList("Population/FemaleNames.json");
    }
    public void UpdateMaxPopulation()
    {
        List<House> houses = GetAllHouses();

        if (houses.Count > 0)
            maxPopulaton = houses.Count * houses[0].maxPopulation;
        else maxPopulaton = 0;

        UpdatePopulationState();
    }
    public void UpdatePopulation()
    {
        List<House> houses = GetAllHouses();

        if (currentPopulation.Count == maxPopulaton && resourceMan.food > 0) return;

        // Village can spawn a villager
        else if (currentPopulation.Count < maxPopulaton && resourceMan.food > 0 && houses.Count > 0 && !resourceMan.noFood)
        {
            int amount = UnityEngine.Random.Range(0, houses.Count);

            if (amount > maxPopulaton - currentPopulation.Count)
                amount = maxPopulaton - currentPopulation.Count;

            SpawnVillager(amount);
        }

        // Village has to kick out a villager
        else if (availableVillagers.Count > 0 && resourceMan.food <= 0 && houses.Count > 0 || resourceMan.noFood)
        {
            DespawnVillager(Mathf.FloorToInt(UnityEngine.Random.Range(1, availableVillagers.Count / 3)));
        }

        UpdateFood(-(currentPopulation.Count / 2));
        UpdatePopulationState();
    }
    public void UpdateFood(int food)
    {
        this.resourceMan.food += food;

        if (this.resourceMan.food <= 0)
        {
            this.resourceMan.food = 0;
            resourceMan.noFood = true;
        }
        else
        {
            resourceMan.noFood = false;
        }

        resourceMan.foodCounter.text = this.resourceMan.food.ToString();
    }
    List<House> GetAllHouses()
    {
        List<House> houses = new List<House>();

        foreach (Building building in buildingsMan.buildingsOnMap)
        {
            if (building.GetBuildingType() == BuildingsManager.BuildingType.House)
            {
                houses.Add(building.GetComponent<House>());
            }
        }

        return houses;
    }
    public List<Villager> GetVillagersToWork(int amount, VillagerProfession profession, Building workingPlace)
    {
        List<Villager> villagers = new List<Villager>();

        if (amount > 0 && availableVillagers.Count >= amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var villager = availableVillagers[i];
                villagers.Add(villager);
                villager.SetProfession(profession);
                villager.SetWorkingPlace(workingPlace);
            }

            UpdatePopulationState();
            return villagers;
        }

        else
            return null;

    }
    public List<Villager> GetSoldiers(int amount)
    {
        List<Villager> soldiers = new List<Villager>();

        if (amount > 0 && availableVillagers.Count >= amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var soldier = availableVillagers[i];
                soldiers.Add(soldier);
                soldier.SetProfession(VillagerProfession.Soldier);
            }

            UpdatePopulationState();
            return soldiers;
        }

        else return null;

    }
    void SpawnVillager(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var villager = new Villager();
            currentPopulation.Add(villager);

            var villager_ = Instantiate(villagerPref, Vector3.zero, Quaternion.identity);
            villager_.GetComponent<VillagerBody>().Init(villager);
        }

        UIManager.Instance.MessagePanel($"{amount} villagers arrived.");
    }
    void DespawnVillager(int amount)
    {
        if (availableVillagers.Count == 0) return;

        UIManager.Instance.MessagePanel($"{amount} villagers left.");
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, availableVillagers.Count);
            var villager = availableVillagers[index];

            villager.GetBody().Despawning();
            availableVillagers.RemoveAt(index);

            if (currentPopulation.Contains(villager))
                currentPopulation.Remove(villager);
        }

    }
    public void UpdatePopulationState()
    {
        List<Villager> freeVillagers = new List<Villager>();
        workers = 0;
        soldiers = 0;

        foreach (Villager villager in currentPopulation)
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
                    soldiers++;
                    break;
                default:
                    break;
            }
        }

        availableVillagers = freeVillagers;
        populationCounter.text = currentPopulation.Count + "/" + maxPopulaton + " W:" + workers + " S:" + soldiers + " A:" + availableVillagers.Count;
    }
}
