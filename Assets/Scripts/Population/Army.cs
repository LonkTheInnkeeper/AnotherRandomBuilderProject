using System.Collections.Generic;
using System.Linq;

public class Army : ICharacter
{
    PopulationManager.ArmyType armyType;
    List<Villager> soldiers;
    List<MapCell> path = new List<MapCell>();
    MapCell currentCell;
    MapCell targetCell;
    bool isMoving;
    int navSequence = 0;

    public Army(PopulationManager.ArmyType armyType, List<Villager> villagers, MapCell currentCell)
    {
        this.armyType = armyType;
        this.soldiers = villagers;
        this.currentCell = currentCell;

        currentCell.character = this;

        GatherSoldiers();
    }

    void GatherSoldiers()
    {
        path.Add(currentCell);

        foreach (var soldier in soldiers)
        {
            soldier.SetArmy(this);
            soldier.GetBody().SetDestination(currentCell);
        }
    }

    void NavigateArmy()
    {
        foreach(var soldier in soldiers)
        {
            soldier.GetBody().SetDestination(targetCell);
        }
    }

    public void SoldiersOnSpot()
    {
        foreach (var soldier in soldiers)
        {
            if (soldier.GetBody().GetCurrentCell() != path[navSequence]) return;
        }

        currentCell = path[navSequence];
        if (currentCell == path.Last()) return;

        navSequence++;
        //NavigateArmy();
    }

    public MapCell GetCurrentCell() { return currentCell; }
    public bool IsMoving() { return isMoving; }
    public void SetCurrentCell(MapCell cell) { currentCell = cell; }
    public void SetMoving(bool moving) { isMoving = moving; }
    public bool IsPlayerMovable() { return true; }
    public void SetTargetCell(MapCell cell)
    {
        targetCell = cell; 
        path = PFTools.Pathfinding(currentCell, targetCell);

        if (path != null) 
        {
            navSequence = 0;
            NavigateArmy();
        }
    }
}
