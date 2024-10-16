using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("UI")]
    public TextMeshProUGUI foodCounter;
    public TextMeshProUGUI woodCounter;
    public TextMeshProUGUI stoneCounter;
    public TextMeshProUGUI ironCounter;
    public TextMeshProUGUI crystalCounter;

    [Header("Values")]
    public int food;
    public int wood;
    public int stone;
    public int iron;
    public int crystal;

    public bool noFood = false;

    [Header("Editing resource")]
    public List<Transform> resources;
    public List<ResourceData> resourceDatas = new List<ResourceData>();
    public ResourceType activeResource;
    public ResourceType[] resourceTypes;
    public Ghost ghost;
    public bool deleting = false;

    int activeType = 0;

    List<Building> barns = new List<Building>();

    BuildingsManager buildingsMan;
    InputManager input;

    public enum ResourceType
    {
        Forest,
        Rock,
        Ore,
        Crystal,
        Field,
        None
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        buildingsMan = BuildingsManager.Instance;
        input = InputManager.Instance;
        resourceTypes = (ResourceType[])Enum.GetValues(typeof(ResourceType));
        UpdateUI();
    }

    private void Update()
    {
        if (input.SaveEditor())
        {
            SaveLoadSystem.SaveMapResources(resourceDatas);
        }
    }

    // === MAP EDITING ===
    public void UIResourceClick(int ID)
    {
        deleting = false;
        activeResource = resourceTypes[ID];
        ghost.UpdateVisual();
    }
    public void UIDeleteResource()
    {
        deleting = !deleting;
        activeResource = ResourceType.None;
    }
    // ===================


    public Transform GetActiveResource()
    {
        foreach (Transform resource in resources)
        {
            if (resource.GetComponent<IResource>().GetResourceType() == activeResource)
            {
                return resource;
            }
            else continue;
        }

        return null;
    }
    public void UpdateBarns(Building barn, bool add)
    {
        if (add)
        {
            barns.Add(barn);
        }
        else
        {
            barns.Remove(barn);
        }
    }
    public void FindAvailableBarn(Building building)
    {
        MapCell buildingCell = building.GetCell();
        List<Barn> barns = new List<Barn>();
        List<MapCell> buildingRoards = new List<MapCell>();

        // Check if building is connected to a road
        // If not, skip
        bool hasRoad = false;
        foreach (MapCell neighbor in buildingCell.GetNeighbors())
        {
            if (neighbor.CheckRoad())
            {
                hasRoad = true;
                buildingRoards.Add(neighbor);
            }
        }
        if (!hasRoad) return;

        // Check which barns are connected to the building
        // If there is no barn connected, skip
        foreach (var barn in this.barns)
        {
            foreach (var barnNeighbor in barn.GetCell().GetNeighbors())
            {
                if (barnNeighbor.CheckRoad())
                {
                    // The most expensive part
                    foreach (var buldingRoad in buildingRoards)
                    {
                        var path = PFTools.RoadPathfinding(barnNeighbor, buldingRoad);

                        if (path == null) continue;

                        else
                        {
                            barns.Add(barn.GetComponent<Barn>());
                            break;
                        }
                    }
                }
            }
        }
        if (barns.Count == 0) return;

        // Sellect barns with available carts
        // If there are not any, skip
        for (int i = 0; i < barns.Count; i++)
        {
            if (barns[i].cartsInUse! < barns[i].workers.Count)
            {
                barns.RemoveAt(i);
                i--;
            }
        }
        if (barns.Count == 0) return;

        // Sellect a random barn and tell it to do its job
        barns[UnityEngine.Random.Range(0, barns.Count)].SendCart(building);
    }
    public void UpdateUI()
    {
        foodCounter.text = food.ToString();
        woodCounter.text = wood.ToString();
        stoneCounter.text = stone.ToString();
        ironCounter.text = iron.ToString();
        crystalCounter.text = crystal.ToString();
    }
}


