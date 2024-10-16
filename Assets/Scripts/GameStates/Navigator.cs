using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Navigator : MonoBehaviour
{
    MapCell currentCell;
    MapCell startCell;
    MapCell targetCell;

    List<MapCell> navigationPath = new List<MapCell>();
    List<MapCell> navigationDisplayed = new List<MapCell>();

    ICharacter character;

    GameManager gameMan;
    MapManager mapMan;
    InputManager input;

    void Start()
    {
        gameMan = GameManager.Instance;
        mapMan = MapManager.Instance;
        input = InputManager.Instance;
    }

    void Update()
    {
        if (gameMan.GetGameState() != GameManager.GameState.Exploring &&
            gameMan.GetGameState() != GameManager.GameState.Navigating) return;

        SetCharacter();
        ResetCharacter();
        SetDestination();
    }

    void SetCharacter()
    {
        if (input.LeftMouseClick() && !PointsOverUI())
        {
            print("Setting character");
            currentCell = mapMan.GetMouseMapCell();

            if (currentCell != null && currentCell.character != null)
            {
                if (currentCell.character.IsPlayerMovable())
                {
                    character = currentCell.character;
                    gameMan.SetGameState(GameManager.GameState.Navigating);
                }
            }

            if (gameMan.GetGameState() == GameManager.GameState.Exploring)
            {
                gameMan.SetGameState(GameManager.GameState.Navigating);
            }
        }
    }
    void ResetCharacter()
    {
        if (input.RightMouseClick() && gameMan.GetGameState() == GameManager.GameState.Navigating)
        {
            character = null;
            gameMan.SetGameState(GameManager.GameState.Exploring);
        }
    }
    void SetDestination()
    {
        if (input.LeftMouseClick() && !PointsOverUI())
        {
            MapCell cell = mapMan.GetMouseMapCell();

            if (cell != null && !cell.occupied)
            {
                try
                {
                    character.SetTargetCell(cell);
                }
                catch
                {
                    Debug.LogError("Can't set a destination for Navigator");
                }
            }
        }
    }


    private bool PointsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
