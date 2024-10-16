using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village
{
    int size;

    MapCell origin;
    MapCell lowCell;
    MapCell highCell;

    List<Building> buildings;

    public Village(int size, MapCell origin)
    {
        this.size = size;
        this.origin = origin;

        CalculateSize();
    }

    void CalculateSize()
    {
        Vector2Int originID = origin.GetID();
        lowCell = MapManager.Instance.GetCellByID(0, new Vector2Int(originID.x - size, originID.y - size));
        highCell = MapManager.Instance.GetCellByID(0, new Vector2Int(originID.x + size, originID.y + size));
    }
}
