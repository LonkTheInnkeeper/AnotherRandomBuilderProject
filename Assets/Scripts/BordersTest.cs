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
    // Nesna� se po��tat pozici krajn�ch bun�k, ale iteruj okruhy soused�c�ch bun�k (BFS algoritmus)
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
