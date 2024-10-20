using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "Buildable Area", menuName = "Map/Buildable Area")]
public class BuildableArea : ScriptableObject
{
    [SerializeField] int size;
    [SerializeField] Vector2Int originID;

    int areaIndex;

    MapCell originCell;
    List<MapCell> area = new List<MapCell>();

    MapManager mapManager;

    public void GenerateArea(int areaIndex)
    {
        this.areaIndex = areaIndex;
        mapManager = MapManager.Instance;
        originCell = mapManager.GetCellByID(0, originID);

        List<MapCell> exploring = new List<MapCell>();
        List<MapCell> toExplore = new List<MapCell>();

        area.Add(originCell);
        exploring.AddRange(originCell.GetNeighbors());

        for (int i = 0; i < size; i++)
        {
            foreach (MapCell cell in exploring)
            {
                foreach (MapCell exploringCell in cell.GetNeighbors())
                {
                    if (!area.Contains(exploringCell) &&
                        !toExplore.Contains(exploringCell) &&
                        !exploring.Contains(exploringCell))
                    {
                        toExplore.Add(exploringCell);
                    }
                }
            }

            area.AddRange(exploring);
            exploring = toExplore;
            toExplore = new List<MapCell>();
        }

        foreach (MapCell cell in area)
        {
            cell.SetBuildableAre(true, areaIndex);
        }

        if (mapManager.areaDebug)
            AreaDebug();
    }

    private void AreaDebug()
    {
        foreach (MapCell cell in area)
        {
            cell.SetCellText($"{cell.GetID().ToString()}\nArea {areaIndex.ToString()}");
        }
    }
}
