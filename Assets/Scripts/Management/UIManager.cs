using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Info and build panel")]
    [SerializeField] Transform builderPanel;
    [SerializeField] ExplorerPanel explorerPanel;
    [SerializeField] Explorer explorer;

    [Header("Message panel")]
    [SerializeField] Transform messagePanel;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] float hideDelay;

    [Header("Resource panel")]
    [SerializeField] Transform resourcesPanel;
    [SerializeField] TextMeshProUGUI populationCounter;
    [SerializeField] TextMeshProUGUI woodCounter;
    [SerializeField] TextMeshProUGUI stoneCounter;
    [SerializeField] TextMeshProUGUI ironCounter;
    [SerializeField] TextMeshProUGUI foodCounter;
    [SerializeField] TextMeshProUGUI crystalCounter;

    bool hideResources;

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

    private void Update()
    {
        hideResources = (gameMan.activeArea == null);

        if (resourcesPanel.gameObject.activeSelf != hideResources)
        {
            resourcesPanel.gameObject.SetActive(!hideResources);
        }

        if (!hideResources)
        {
            UpdateResourcesUI(gameMan.activeArea);
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

    public void SetPopulation(string population) { this.populationCounter.text = population; }
    public void SetWood(string wood) { this.woodCounter.text = wood; }
    public void SetStone(string stone) { this.stoneCounter.text = stone; }
    public void SetIron(string iron) { this.ironCounter.text = iron; }
    public void SetCrystal(string crystal) { this.crystalCounter.text = crystal; }

    public void UpdateResourcesUI(BuildableArea area)
    {
        string population = area.GetVillagerCount().ToString() + "/" + area.maxPopulaton.ToString() + "\n" +
                            "A: " + area.GetAvailableVillagers().Count.ToString() + " W: " + area.workers;

        foodCounter.text = area.food.ToString();
        woodCounter.text = area.wood.ToString();
        stoneCounter.text = area.stone.ToString();
        ironCounter.text = area.iron.ToString();
        crystalCounter.text = area.crystal.ToString();
    }
}
