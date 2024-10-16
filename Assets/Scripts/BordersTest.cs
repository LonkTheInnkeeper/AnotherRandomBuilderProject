using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersTest : MonoBehaviour
{
    public Vector2Int originID;

    public Transform pref;
    public int size;

    MapManager mapManager;

    private void Start()
    {
        mapManager = MapManager.Instance;
    }


    // Note:
    // Nesnaž se poèítat pozici krajních bunìk, ale iteruj okruhy sousedících bunìk (BFS algoritmus)
    public void Test()
    {
        foreach(MapCell cell in mapManager.grids[0].GetGridCells()) 
        {
            //Vector2Int id = cell.GetID();
            //if()
            //{
            //    Instantiate(pref, mapManager.GetCellByID(0, id).GetCoordinates(), Quaternion.identity);
            //}
        }
    }
}
