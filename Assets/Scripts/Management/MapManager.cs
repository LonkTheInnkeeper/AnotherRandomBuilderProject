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

    private void Awake()
    {
        Instance = this;
        grids.Any();
    }

    private void Start()
    {
        GenerateHexGrid();
        GenerateBuidableArea();
        resourceEditor.LoadResource(SaveLoadSystem.LoadMapResources());
        gridGenerated.Invoke();
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
            grid.cellsList = grid.GetGridCells();
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
            Vector3 mousePos = Tools.GetHexGridPosition(grids[0].hexOffsetX, grids[0].hexOffsetZ);

            if (cellPos == mousePos) return cell;
            else continue;
        }

        return null;
    }

    public void SetMapCell()
    {
        foreach (MapCell cell in grids[0].cells)
        {
            Vector3 cellPos = cell.GetCoordinates();
            Vector3 mousePos = Tools.GetHexGridPosition(grids[0].hexOffsetX, grids[0].hexOffsetZ);

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
