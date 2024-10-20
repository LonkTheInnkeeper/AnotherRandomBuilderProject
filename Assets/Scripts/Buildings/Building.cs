using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    [SerializeField] BuildingsManager.BuildingType buildingType;
    [SerializeField] Mesh visiualMesh;
    [SerializeField] Transform visualTransform;
    [SerializeField] int size;

    public UnityEvent newDay;
    public List<Villager> presentVillagers = new List<Villager>();

    MapCell cell;
    MapCell frontNeighbor;
    List<MapCell> buildingSize = new List<MapCell>();

    public bool export = false;
    public bool exported = false;

    [HideInInspector] public int areaIndex;

    int rotationIndex;

    private void Start()
    {
        InvokeRepeating(nameof(Export), 0, 2);
        buildingSize = BuildBuilding();
        areaIndex = cell.areaIndex;

        foreach (MapCell cell in buildingSize)
        {
            cell.SetCellText(buildingType.ToString());
            cell.SetBuilding(this);
            cell.occupied = true;
        }
    }

    void Export()
    {
        if (buildingType != BuildingsManager.BuildingType.Barn && export && !exported)
        {
            ResourceManager.Instance.FindAvailableBarn(this);
        }
    }

    List<MapCell> BuildBuilding()
    {
        List<MapCell> building = new List<MapCell>();
        building.Add(cell);

        if (size > 1)
        {
            List<MapCell> neighbors = cell.GetNeighbors();

            for (int i = 0; i < size - 1; i++)
            {
                int index = i + rotationIndex;

                if (index > neighbors.Count - 1)
                {
                    index -= neighbors.Count;
                }

                building.Add(neighbors[index]);
            }
        }

        return building;
    }

    public void SetCell(MapCell cell)
    {
        if (this.cell == null)
        { 
            this.cell = cell;
            areaIndex = cell.areaIndex;
        }
        else return;
    }
    public void SetRotation(int rotationIndex)
    {
        this.rotationIndex = rotationIndex;
        float rotationAngle = rotationIndex * 60;
        visualTransform.rotation = Quaternion.Euler(new Vector3(0, rotationAngle, 0));
    }
    public void DestroyBuilding()
    {
        DestroyImmediate(gameObject, true);
    }
    public Mesh GetVisual() { return visiualMesh; }
    public BuildingsManager.BuildingType GetBuildingType() { return buildingType; }
    public MapCell GetCell() { return cell; }
    public int GetSize() { return size; }
    public IProduction GetProduction() { return GetComponent<IProduction>(); }
}
