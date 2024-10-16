public interface ICharacter
{
    public bool IsPlayerMovable();
    public bool IsMoving();
    public void SetMoving(bool moving);
    public void SetCurrentCell(MapCell cell);
    public MapCell GetCurrentCell();
    public void SetTargetCell(MapCell cell);
}
