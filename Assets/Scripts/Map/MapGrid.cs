using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [Header("Settings")]
    public int gridSize;
    public MapCell[,] cells;
    public List<MapCell> cellsList;
    public Dictionary<Vector3, MapCell> cellDictionary = new Dictionary<Vector3, MapCell>();
    public int gridIndex;
    public float hexOffsetX;
    public float hexOffsetZ;

    [Header("Resources")]
    public int population;
    public int crystal;
    public int food;
    public int wood;
    public int stone;
    public int iron;

    [Header("Buildings")]
    public List<Building> buildings;
    public Transform buildingParent;

    // Save System
    IDataService dataService = new JsonDataService();

    private void Awake()
    {
        NormalisePosition();
        cells = new MapCell[gridSize * 3, gridSize];
    }

    private void Start()
    {
        //cellsList = GetGridCells();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SaveGridToJson();
        }
    }

    void NormalisePosition()
    {
        float x = (float)(transform.position.x - transform.position.x % 0.5);
        int intY = Mathf.FloorToInt(transform.position.y);
        float z = (float)(transform.position.z - transform.position.z % 0.86);

        transform.position = new Vector3(x, intY, z);
    }

    public void DebugGrid()
    {
        foreach (var cell in cells)
        {
            print(cell.GetCoordinates() + " " + gridIndex);
        }
    }

    public Vector2 GetOrigin()
    {
        return transform.position;
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(gridSize * 3, gridSize);
    }

    public void SaveGridToJson()
    {
        List<MapCell> grid = GetGridCells();

        if (dataService.SaveData("MapCell.json", grid))
        {
            return;
        }
        else
        {
            Debug.LogError("Could not save file");
        }
    }

    public List<MapCell> GetGridCells()
    {
        List<MapCell> mapCells = new List<MapCell>();

        foreach (var cell in cells)
        {
            mapCells.Add(cell);
        }

        return mapCells;
    }
}
