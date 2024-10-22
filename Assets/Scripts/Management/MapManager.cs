using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public List<MapGrid> grids;
    public List<BuildableArea> buildableAreas;

    [Space]
    public Transform cellBlockPref;

    [Space]
    public bool gridDebug;
    public bool areaDebug;

    public MapCell activeCell;
    public ResourceEditor resourceEditor;

    [Space]
    public UnityEvent gridGenerated;

    [Space]
    public Transform cameraPoint;

    GameManager gameManager;

    private void Awake()
    {
        Instance = this;
        grids.Any();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        GenerateHexGrid();
        GenerateBuidableArea();
        resourceEditor.LoadResource(SaveLoadSystem.LoadMapResources());
        gridGenerated.Invoke();
    }

    private void Update()
    {
        SetActiveArea();
    }

    private void SetActiveArea()
    {
        MapCell cell = GetVectorMapCell(cameraPoint.position);


        if (cell == null) return;
        if (!cell.isInArea) 
        {
            gameManager.activeArea = null;
            return;
        }

        print("Active area = " + gameManager.activeArea != null);
        gameManager.activeArea = buildableAreas[cell.areaIndex];
    }

    private void GenerateHexGrid()
    {
        int gridIndex = 0;
        int gridSize = 0;

        foreach (MapGrid grid in grids)
        {
            gridSize = grid.gridSize;

            float posXConst = 0;
            float posZConst = 0;

            for (int x = 0; x < gridSize * 3; x++)
            {
                if (x % 2 == 1) posZConst = grid.hexOffsetZ;
                else posZConst = 0;

                for (int z = 0; z < gridSize; z++)
                {
                    MapCell cell = new MapCell(posXConst, 0, posZConst, grid.transform.position, grid, x, z);
                    grid.cells[x, z] = cell;

                    SetCell(cell, grid);

                    cell.SetCellText(cell.GetID().ToString());

                    posZConst += grid.hexOffsetZ * 2;
                }

                posXConst += grid.hexOffsetX;
            }

            foreach (MapCell cell in grid.cells)
            {
                cell.Init();
            }
            grid.gridIndex = gridIndex;
            gridIndex++;
        }

        foreach (MapGrid grid in grids)
        {
            List<MapCell> cells = grid.GetGridCells();
            grid.cellsList = cells;

            foreach (MapCell cell in cells)
            {
                grid.cellDictionary[cell.GetCoordinates()] = cell;
            }
        }
    }

    private void GenerateBuidableArea()
    {
        for (int i = 0; i < buildableAreas.Count; i++)
        {
            buildableAreas[i].GenerateArea(i);
        }
    }

    void SetCell(MapCell cell, MapGrid grid)
    {
        var cellBlock = Instantiate(cellBlockPref, cell.GetCoordinates(), Quaternion.identity, grid.transform);
        cellBlock.GetComponent<CellBlock>().SetCell(cell);
        cell.SetCellBlock(cellBlock.GetComponent<CellBlock>());
    }

    public MapCell GetCellByID(int gridIndex, Vector2Int ID)
    {
        try
        {
            return grids[gridIndex].cells[ID.x, ID.y];
        }
        catch (Exception e)
        {
            //Debug.LogError($"MapCell {ID.x}, {ID.y} is out of range.");
            return null;
        }
    }

    public MapCell GetMouseMapCell()
    {
        foreach (MapCell cell in grids[0].cells)
        {
            Vector3 cellPos = cell.GetCoordinates();
            Vector3 mousePos = Tools.GetMouseHexGridPosition(grids[0].hexOffsetX, grids[0].hexOffsetZ);

            if (cellPos == mousePos) return cell;
            else continue;
        }

        return null;

        //Vector3 mousePos = Tools.GetMouseHexGridPosition(grids[0].hexOffsetX, grids[0].hexOffsetZ);

        //if (grids[0].cellDictionary.TryGetValue(mousePos, out MapCell foundCell))
        //{
        //    return foundCell;
        //}
        //else
        //{
        //    return null;
        //}
    }

    MapCell GetVectorMapCell(Vector3 vector)
    {
        Vector3 hexVector = Tools.GetVectorHexGridPosition(vector, grids[0].hexOffsetX, grids[0].hexOffsetZ);

        foreach (MapCell cell in grids[0].cells)
        {
            Vector3 cellPos = cell.GetCoordinates();
            Vector3 hexPos = Tools.GetVectorHexGridPosition(hexVector, grids[0].hexOffsetX, grids[0].hexOffsetZ);

            if (cellPos == hexPos) return cell;
            else continue;
        }

        return null;

        //Vector3 hexVector = Tools.GetVectorHexGridPosition(vector, grids[0].hexOffsetX, grids[0].hexOffsetZ);

        //if (grids[0].cellDictionary.TryGetValue(hexVector, out MapCell foundCell))
        //{
        //    return foundCell;
        //}
        //else
        //{
        //    return null;
        //}
    }

    public void SetMapCell()
    {
        foreach (MapCell cell in grids[0].cells)
        {
            Vector3 cellPos = cell.GetCoordinates();
            Vector3 mousePos = Tools.GetMouseHexGridPosition(grids[0].hexOffsetX, grids[0].hexOffsetZ);

            if (cellPos == mousePos)
            {
                activeCell = cell;
                return;
            }
            else continue;
        }
    }

    public MapCell GetActiveCell()
    {
        SetMapCell();
        return activeCell;
    }
}
