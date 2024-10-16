using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Transform builderPanel;
    [SerializeField] ExplorerPanel explorerPanel;
    [SerializeField] Explorer explorer;

    [Space]
    [SerializeField] Transform messagePanel;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] float hideDelay;

    GameManager gameMan;
    BuildingsManager buildingsMan;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameMan = GameManager.Instance;
        buildingsMan = BuildingsManager.Instance;

        if (gameMan.GetGameState() != GameManager.GameState.Constructing)
        {
            builderPanel.gameObject.SetActive(false);
        }
    }

    public void ToggleBuilderPanel(bool toggle)
    {
        builderPanel.gameObject.SetActive(toggle);
    }
    public void ConstructButton()
    {
        explorer.ResetActiveBuilding();
        buildingsMan.activeBuilding = BuildingsManager.BuildingType.None;
        switch (gameMan.GetGameState())
        {
            case GameManager.GameState.Constructing:
                gameMan.SetGameState(GameManager.GameState.Exploring);
                ToggleBuilderPanel(false);
                explorerPanel.ToggleExplorerPanel(null);
                break;
            case GameManager.GameState.Exploring:
                gameMan.SetGameState(GameManager.GameState.Constructing);
                ToggleBuilderPanel(true);
                break;
            case GameManager.GameState.Editing:
                gameMan.SetGameState(GameManager.GameState.Constructing);
                ToggleBuilderPanel(true);
                break;
            default:
                break;
        }
    }

    public void MessagePanel(string message)
    {
        messagePanel.gameObject.SetActive(true);
        messageText.text = message;
        Invoke(nameof(HideMessagePanel), hideDelay);
    }

    void HideMessagePanel()
    {
        messagePanel.gameObject.SetActive(false);
    }
}
