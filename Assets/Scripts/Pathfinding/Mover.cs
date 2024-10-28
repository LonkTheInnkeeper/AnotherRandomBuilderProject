using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float offsetConstant;

    Vector3 offset;
    MapCell start;
    MapCell end;
    List<MapCell> path = new List<MapCell>();

    ICharacter character;
    MapManager mapMan;

    int sequence = 0;

    private void Start()
    {
        mapMan = MapManager.Instance;
        character = GetComponent<ICharacter>();
        SetOffset();
    }

    private void Update()
    {
        if (path == null || path.Count == 0) return;
        Move();
    }

    void Move()
    {
        if (sequence < path.Count)
        {
            character.SetMoving(true);

            transform.position = Vector3.MoveTowards(transform.position, path[sequence].GetCoordinates() + offset, speed * Time.deltaTime);

            if (transform.position == path[sequence].GetCoordinates() + offset)
            {
                character.SetCurrentCell(path[sequence]);
                sequence++;
            }
        }

        else if (transform.position == end.GetCoordinates() + offset)
        {
            character.SetCurrentCell(path.Last());
            character.SetMoving(false);
            path.Clear();
            sequence = 0;
        }
    }

    void MoveByOne(MapCell target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.GetCoordinates() + offset, speed * Time.deltaTime);
    }

    public void SetRandomEndCell(MapCell start, int range)
    {
        Vector2Int currentCellID = start.GetID();
        Vector2Int lastCellID = mapMan.grids[0].GetGridCells().Last().GetID();
        Vector2Int newCellID;

        newCellID = new Vector2Int(currentCellID.x + Random.Range(-range, range), currentCellID.y + Random.Range(-range, range));

        if ((newCellID.x < 0 || newCellID.x > lastCellID.x) && (newCellID.y < 0 || newCellID.y > lastCellID.y))
        {
            //SetMoving(false);
            return;
        }

        MapCell targetCell = mapMan.GetCellByID(0, newCellID);

        if (targetCell != null)
            SetPath(start, targetCell);
    }

    public void SetRandomEndCell(MapCell start, int range, int areaIndex)
    {
        Vector2Int currentCellID = start.GetID();
        Vector2Int lastCellID = mapMan.grids[0].GetGridCells().Last().GetID();
        Vector2Int newCellID;

        newCellID = new Vector2Int(currentCellID.x + Random.Range(-range, range), currentCellID.y + Random.Range(-range, range));
        MapCell targetCell = mapMan.GetCellByID(0, newCellID);

        if (targetCell == null) return;

        if (!mapMan.buildableAreas[areaIndex].GetAllCells().Contains(targetCell))
        {
            //SetMoving(false);
            return;
        }

        SetPath(start, targetCell);
    }

    public void SetPath(MapCell start, MapCell end)
    {
        this.start = start;
        this.end = end;

        path = PFTools.Pathfinding(start, end);
    }

    public void SetOffset()
    {
        offset = new Vector3(Random.Range(-offsetConstant, offsetConstant), 0, Random.Range(-offsetConstant, offsetConstant));
    }
}
