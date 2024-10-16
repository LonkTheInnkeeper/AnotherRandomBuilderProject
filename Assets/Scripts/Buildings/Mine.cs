using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IProduction
{
    public ResourceManager.ResourceType resourceType;

    public int available;
    public int storage;
    public int maxStorage;
    public List<Villager> workers = new List<Villager>();
    public int maxWorkers;
    public bool working;

    public int baseWorkingSpeed;
    float timeToCollect;

    Building building;
    ResourceHandler resource;
    ResourceManager resourceMan;
    PopulationManager populationMan;

    private void Start()
    {
        resourceMan = ResourceManager.Instance;
        populationMan = PopulationManager.Instance;
        building = GetComponent<Building>();
        GetResources();
        available = CalculateResources(null);
    }

    private void Update()
    {
        Work();

        if (storage != 0)
            building.export = true;
        else building.export = false;
    }

    public void GetResources()
    {
        resource = building.GetCell().GetResource().GetTransform().GetComponent<ResourceHandler>();
    }

    public int CalculateResources(List<ResourceHandler> resourceList)
    {
        return resource.GetData().value;
    }

    public void Work()
    {
        if (workers.Count > 0 && storage < maxStorage && available > 0 && resource.GetData() != null)
        {
            timeToCollect -= Time.deltaTime * workers.Count;

            if (timeToCollect < 0)
            {
                resource.GatherResource();
                storage++;
                timeToCollect = baseWorkingSpeed;

                if (resource.GetData() != null)
                    available = CalculateResources(null);
                else
                    available = 0;
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
        int amount = 1;

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

            populationMan.UpdatePopulationState();
        }
    }
    public int GetWorkersCount()
    {
        return workers.Count;
    }

    public void RemoveWorker(Villager worker)
    {
        throw new System.NotImplementedException();
    }
}
