using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : MonoBehaviour, IProduction
{
    public ResourceManager.ResourceType resourceType;

    [Space]
    public int available;
    public int storage;
    public int maxStorage;
    public List<Villager> workers = new List<Villager>();
    public int maxWorkers;
    public bool working;

    public int baseWorkingSpeed;
    float timeToCollect;

    Building building;
    List<ResourceHandler> resources = new List<ResourceHandler>();

    private void Start()
    {
        building = GetComponent<Building>();
        GetResources();

        timeToCollect = baseWorkingSpeed;
    }

    private void Update()
    {
        if (resources.Count != 0)
            Work();

        if (storage != 0)
            building.export = true;
        else building.export = false;
    }

    public void GetResources()
    {
        List<MapCell> cells = building.GetCell().GetNeighbors();
        resources.Clear();

        foreach (MapCell cell in cells)
        {
            IResource cellResource = cell.GetResource();
            if (cellResource != null)
            {
                if (cellResource.GetResourceType() == resourceType)
                {
                    resources.Add(cellResource.GetTransform().GetComponent<ResourceHandler>());
                }
            }
        }

        available = CalculateResources(this.resources);
    }

    public int CalculateResources(List<ResourceHandler> resourceList)
    {
        int resources = 0;

        foreach (var resource in resourceList)
        {
            resources += resource.GetData().value;
        }

        return resources;
    }

    public void Work()
    {
        if (workers.Count > 0 && resources.Count > 0 && storage < maxStorage)
        {
            timeToCollect -= Time.deltaTime * building.presentVillagers.Count;

            if (timeToCollect < 0)
            {
                ResourceHandler resource = resources[Random.Range(0, resources.Count)];
                resource.GatherResource();
                storage++;
                timeToCollect = baseWorkingSpeed;
                if (resource.GetData() == null)
                {
                    resources.Remove(resource);
                }
                available = CalculateResources(resources);
            }
        }
        else
        {
            timeToCollect = baseWorkingSpeed;
        }
    }

    public int GetStorage()
    {
        return storage;
    }

    public void SetStorage(int storage)
    {
        this.storage = storage;
    }

    public ResourceManager.ResourceType GetResourceType()
    {
        return resourceType;
    }

    public void AddWorkers(bool add)
    {
        print("Adding workers");
        int amount = 1;

        PopulationManager populationMan = PopulationManager.Instance;

        if (add && workers.Count < maxWorkers && populationMan.availableVillagers.Count >= amount)
        {
            List<Villager> workers = populationMan.GetVillagersToWork(1, PopulationManager.VillagerProfession.Worker, building);

            this.workers.AddRange(workers);
        }

        if (!add && workers.Count > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                var worker = workers[Random.Range(0, workers.Count)];
                worker.SetProfession(PopulationManager.VillagerProfession.None);
                worker.SetWorkingPlace(null);
                workers.Remove(worker);
            }

        }
        populationMan.UpdatePopulationState();

        print(workers.Count.ToString());
    }

    public int GetWorkersCount()
    {
        return workers.Count;
    }

    public void RemoveWorker(Villager worker)
    {
        workers.Remove(worker);
    }
}
