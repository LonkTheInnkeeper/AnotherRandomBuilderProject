using UnityEngine;
using static GameManager;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    GameManager gameMan;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameMan = GameManager.Instance;
    }

    private void Update()
    {
        if (RightMouseClick() && gameMan.GetGameState() == GameState.Constructing)
        {
            UIManager.Instance.ConstructButton();
        }

        if (RightMouseClick() &&
            gameMan.GetGameState() == GameState.Editing &&
            ResourceManager.Instance.activeResource == ResourceManager.ResourceType.Field)
        {
            gameMan.SetGameState(GameState.Exploring);
        }

        if (LeftMouseClick())
        {

        }
    }

    public bool RightMouseClick() { return Input.GetMouseButtonDown(1); }
    public bool LeftMouseClick() { return Input.GetMouseButtonDown(0); }
    public bool Rotate() { return Input.GetKeyDown(KeyCode.R); }
    public bool Debug() { return Input.GetKeyDown(KeyCode.Space); }
    public bool SaveEditor() { return Input.GetKeyDown(KeyCode.P); }
}
