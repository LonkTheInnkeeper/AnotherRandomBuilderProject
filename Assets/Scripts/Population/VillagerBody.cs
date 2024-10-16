using System.Collections.Generic;
using UnityEngine;

public class VillagerBody : MonoBehaviour, ICharacter
{
    Villager villager;

    // Movement
    float tickConstant = 3;
    float tickTime;
    bool moving = false;

    [HideInInspector] public bool despawning = false;

    Building currentBuilding;
    MapCell currentCell;
    MapCell movementTargetCell;

    GameManager gameMan;
    MapManager mapMan;
    PopulationManager popMan;
    Mover mover;

    public void Init(Villager villager)
    {
        this.villager = villager;
        villager.SetBody(this);
        gameMan = GameManager.Instance;
        mapMan = MapManager.Instance;
        popMan = PopulationManager.Instance;
        mover = GetComponent<Mover>();

        StartingPosition();

        tickTime = tickConstant;
    }

    private void Update()
    {
        switch (villager.GetProfession())
        {
            case PopulationManager.VillagerProfession.None:
                Worker(); break;
            case PopulationManager.VillagerProfession.Worker:
                Worker(); break;
            case PopulationManager.VillagerProfession.Soldier:
                Soldier(); break;
        }
    }

    void Soldier()
    {

    }

    void Worker()
    {
        tickTime -= Time.deltaTime;

        if (tickTime < 0 && !despawning)
        {
            Day();
            Night();

            tickTime = Random.Range(1, tickConstant);
        }

        else if (despawning)
        {
            if (currentCell == popMan.spawn)
                Destroy(gameObject);
        }
    }


    // === AI ===
    void Day()
    {
        if (!gameMan.day) return;

        Idle();
        Working();
    }
    void Night()
    {
        if (gameMan.day) return;

        if (currentCell != villager.GetHouse().GetCell() && !moving)
        {
            mover.SetPath(currentCell, villager.GetHouse().GetCell());
        }
    }
    void Idle()
    {
        if (villager.GetWorkingPlace() == null && villager.GetArmy() == null && !moving)
        {
            mover.SetRandomEndCell(currentCell, 3);
        }
    }
    void Working()
    {
        Building workingPlace = villager.GetWorkingPlace();
        if (workingPlace == null) return;

        if (!moving && workingPlace.GetCell() != currentCell)
        {
            mover.SetPath(currentCell, villager.GetWorkingPlace().GetCell());
        }
    }


    // === MOVING ===
    public bool IsMoving() { return moving; }
    public void SetMoving(bool moving)
    {
        this.moving = moving;

        if (!moving)
        {
            movementTargetCell = null;

            if (villager.GetArmy() != null) 
            {
                villager.GetArmy().SoldiersOnSpot();
            }
        }
    }
    public void SetCurrentCell(MapCell cell)
    {
        currentCell = cell;
        CheckCurrentBuilding();
    }
    public MapCell GetCurrentCell()
    {
        return currentCell;
    }
    void CheckCurrentBuilding()
    {
        Building building = currentCell.GetBuilding();

        if (building != null && !building.presentVillagers.Contains(villager))
        {
            currentBuilding = currentCell.GetBuilding();
            currentBuilding.presentVillagers.Add(villager);
        }
        else if (building == null && currentBuilding != null)
        {
            currentBuilding.presentVillagers.Remove(villager);
            currentBuilding = null;
        }
    }
    public void SetDestination(MapCell destination)
    {
        mover.SetPath(currentCell, destination);
    }
    public bool IsPlayerMovable() { return false; }


    // === SPAWNING ===
    void StartingPosition()
    {
        List<MapCell> mapCells = MapManager.Instance.grids[0].cellsList;
        currentCell = popMan.spawn;
        transform.position = popMan.spawn.GetCoordinates();
    }
    public void Despawning()
    {
        despawning = true;
        Building productionBuilding = villager.GetWorkingPlace();

        villager.GetHouse().villagers.Remove(villager);

        if (productionBuilding != null)
        {
            productionBuilding.GetProduction().RemoveWorker(villager);
        }

        villager.SetHome(null);
        villager.SetWorkingPlace(null);

        mover.SetPath(currentCell, popMan.spawn);
    }

    public void SetTargetCell(MapCell cell)
    {
        throw new System.NotImplementedException();
    }
}
