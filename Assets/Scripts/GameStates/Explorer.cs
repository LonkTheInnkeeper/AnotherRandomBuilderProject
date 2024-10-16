using UnityEngine;
using UnityEngine.EventSystems;

public class Explorer : MonoBehaviour
{
    [SerializeField] ExplorerPanel explorerPanel;

    Building activeBuilding;

    GameManager gameMan;
    MapManager mapMan;
    UIManager uiMan;
    InputManager input;

    private void Start()
    {
        gameMan = GameManager.Instance;
        mapMan = MapManager.Instance;
        uiMan = UIManager.Instance;
        input = InputManager.Instance;
    }
    private void Update()
    {
        if (gameMan.GetGameState() != GameManager.GameState.Exploring) return;
        Exploring();
    }
    void Exploring()
    {
        if (input.LeftMouseClick() && !IsPointerOverUIElement())
        {
            MapCell cell = mapMan.GetMouseMapCell();
            if (cell != null)
            {
                activeBuilding = cell.GetBuilding();
                explorerPanel.ToggleExplorerPanel(activeBuilding);
            }
        }

        if (input.RightMouseClick())
        {
            ResetActiveBuilding();
            explorerPanel.ToggleExplorerPanel(activeBuilding);
        }
    }
    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public Building GetActiveBuilding() {  return activeBuilding; }
    public void ResetActiveBuilding() {  activeBuilding = null; }
}
