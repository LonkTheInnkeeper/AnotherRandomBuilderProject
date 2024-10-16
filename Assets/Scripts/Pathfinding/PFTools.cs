using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PFTools
{
    public static List<MapCell> Pathfinding(MapCell startCell, MapCell targetCell)
    {
        List<MapCell> toSearch = new List<MapCell>() { startCell };
        HashSet<MapCell> processed = new HashSet<MapCell>();

        while (toSearch.Any())
        {
            MapCell currentCell = toSearch.OrderBy(t => t.FCost).ThenBy(t => t.hCost).First();

            processed.Add(currentCell);
            toSearch.Remove(currentCell);

            if (currentCell == targetCell)
            {
                return RetracePath(startCell, targetCell);
            }

            foreach (MapCell neighbor in currentCell.GetNeighbors().Where(t => !processed.Contains(t)))
            {
                if ((neighbor.occupied && neighbor != targetCell) && !neighbor.CheckRoad()) continue;

                float newGcost = currentCell.gCost + CubeDistance(OffsetToCube(currentCell.GetID()), OffsetToCube(neighbor.GetID()));

                if (newGcost < neighbor.gCost || !toSearch.Contains(neighbor))
                {
                    neighbor.gCost = newGcost;
                    neighbor.parent = currentCell;

                    try
                    {
                        neighbor.hCost = CubeDistance(OffsetToCube(neighbor.GetID()), OffsetToCube(targetCell.GetID()));
                    }
                    catch 
                    {
                        Debug.LogError("Invalid ID");
                    }

                    if (!toSearch.Contains(neighbor))
                    {
                        toSearch.Add(neighbor);
                    }

                    //neighbor.DisplayNavigation();
                }
            }
        }

        return null;
    }

    public static List<MapCell> RoadPathfinding(MapCell startCell, MapCell targetCell)
    {
        List<MapCell> toSearch = new List<MapCell>() { startCell };
        HashSet<MapCell> processed = new HashSet<MapCell>();

        while (toSearch.Any())
        {
            MapCell currentCell = toSearch.OrderBy(t => t.FCost).ThenBy(t => t.hCost).First();

            processed.Add(currentCell);
            toSearch.Remove(currentCell);

            if (currentCell == targetCell)
            {
                return RetracePath(startCell, targetCell);
            }

            foreach (MapCell neighbor in currentCell.GetNeighbors().Where(t => t.GetBuilding() != null && !processed.Contains(t)))
            {
                if (!neighbor.CheckRoad()) continue;

                float newGcost = currentCell.gCost + CubeDistance(OffsetToCube(currentCell.GetID()), OffsetToCube(neighbor.GetID()));

                if (newGcost < neighbor.gCost || !toSearch.Contains(neighbor))
                {
                    neighbor.gCost = newGcost;
                    neighbor.parent = currentCell;
                    neighbor.hCost = CubeDistance(OffsetToCube(neighbor.GetID()), OffsetToCube(targetCell.GetID()));

                    if (!toSearch.Contains(neighbor))
                    {
                        toSearch.Add(neighbor);
                    }

                    //neighbor.DisplayNavigation();
                }
            }
        }

        return null;
    }

    static List<MapCell> RetracePath(MapCell startCell, MapCell endCell)
    {
        List<MapCell> path = new List<MapCell>();
        MapCell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.parent;
        }

        path.Add(startCell);
        path.Reverse();
        return path;
    }

    static Vector3Int OffsetToCube(Vector2Int offset)
    {
        int x = offset.x - (offset.y - (offset.y & 1)) / 2;
        int z = offset.y;
        int y = -x - z;
        return new Vector3Int(x, y, z);
    }

    static int CubeDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    public static int Navigate(UnityEngine.Transform _object, List<MapCell> path, float speed, int sequence)
    {
        _object.position = Vector3.MoveTowards(_object.position, path[sequence].GetCoordinates(), speed * Time.deltaTime);

        if (_object.position == path[sequence].GetCoordinates())
        {
            sequence++;
            return sequence;
        }
        else return sequence;
    }
}
