using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BordersTest : MonoBehaviour
{
    public Vector2Int originID;

    public Transform pref;
    public int size;

    List<MapCell> area = new List<MapCell>();

    MapManager mapManager;

    private void Start()
    {
        mapManager = MapManager.Instance;
    }


    public void Test()
    {
        List<MapCell> exploring = new List<MapCell>();
        List<MapCell> toExplore = new List<MapCell>();

        MapCell origin = mapManager.GetCellByID(0, originID);
        area.Add(origin);

        exploring.AddRange(origin.GetNeighbors());

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
            var cube = Instantiate(pref, cell.GetCoordinates(), Quaternion.identity);
        }
    }
}
