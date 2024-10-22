using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool changeDay;
    public float dayTimeOrigin;
    public float nightTimeOrigin;
    public TextMeshProUGUI dayTimeCounter;
    public TextMeshProUGUI dayNight;
    float dayTime;

    public bool day = true;
    public bool editing;

    public BuildableArea activeArea;

    public GameState currentState;

    public ICharacter activeCharacter;

    [Space]
    public UnityEvent newDay;
    public enum GameState
    {
        Constructing,
        Exploring,
        Editing,
        Navigating
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dayTime = dayTimeOrigin;
        dayNight.text = "Day - " + day;

        if (editing) currentState = GameState.Editing;
    }

    private void Update()
    {

        DayTimeCycle();
    }

    void DayTimeCycle()
    {
        if (changeDay)
        {
            dayTime -= Time.deltaTime;
            dayTimeCounter.text = dayTime.ToString();
            if (dayTime < 0)
            {
                day = !day;
                dayNight.text = "Day - " + day;

                if (day == true)
                {
                    dayTime = dayTimeOrigin;
                    foreach (Building building in BuildingsManager.Instance.buildingsOnMap)
                    {
                        building.newDay.Invoke();
                    }
                    newDay.Invoke();
                }
                else
                {
                    dayTime = nightTimeOrigin;
                }
            }
        }
    }
    public void SetGameState(GameState state) { currentState = state; }
    public GameState GetGameState() { return currentState; }
}
