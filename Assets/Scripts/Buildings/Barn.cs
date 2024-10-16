using System.Collections.Generic;
using UnityEngine;
using static ResourceManager;

public class Barn : MonoBehaviour
{
    public List<Villager> workers = new List<Villager>();
    public int maxWorkers;

    public int cartsInUse;
    public Cart cartPref;

    MapCell thisCell;
    Building building;
    BuildingsManager buildingsMan;
    ResourceManager resourceMan;
    PopulationManager populationMan;

    private void Start()
    {
        building = GetComponent<Building>();
        thisCell = building.GetCell();
        buildingsMan = BuildingsManager.Instance;
        resourceMan = ResourceManager.Instance;
        populationMan = PopulationManager.Instance;

        resourceMan.UpdateBarns(building, true);
    }

    public void SendCart(Building building)
    {
        if (cartsInUse! < workers.Count) return;

        List<MapCell> path = FindShortestPath(building);

        var cart = Instantiate(cartPref.transform, path[0].GetCoordinates(), Quaternion.identity);
        cart.GetComponent<Cart>().GoToBuilding(path, building, this);
        building.exported = true;

        cartsInUse++;
    }

    List<MapCell> FindShortestPath(Building building)
    {
        List<MapCell> buildingRoads = new List<MapCell>();
        List<MapCell> barnRoads = new List<MapCell>();
        List<MapCell> shortestPath = MapManager.Instance.grids[0].cellsList;

        foreach (var neighbor in building.GetCell().GetNeighbors())
        {
            if (neighbor.CheckRoad())
                buildingRoads.Add(neighbor);
        }

        foreach (var neighbor in thisCell.GetNeighbors())
        {
            if (neighbor.CheckRoad())
                barnRoads.Add(neighbor);
        }

        foreach (var buildingRoad in buildingRoads)
        {
            foreach (var barnRoad in barnRoads)
            {
                List<MapCell> path = PFTools.RoadPathfinding(barnRoad, buildingRoad);

                if (path != null && path.Count < shortestPath.Count)
                    shortestPath = path;
            }
        }

        return shortestPath;
    }

    public void EmptyCart(int resources, ResourceManager.ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceManager.ResourceType.Forest:
                resourceMan.wood += resources;
                break;
            case ResourceManager.ResourceType.Rock:
                resourceMan.stone += resources;
                break;
            case ResourceManager.ResourceType.Ore:
                resourceMan.iron += resources;
                break;
            case ResourceManager.ResourceType.Crystal:
                resourceMan.crystal += resources;
                break;
            case ResourceManager.ResourceType.Field:
                resourceMan.food += resources;
                break;
            case ResourceManager.ResourceType.None:
                break;
        }

        cartsInUse--;

        resourceMan.UpdateUI();
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
}