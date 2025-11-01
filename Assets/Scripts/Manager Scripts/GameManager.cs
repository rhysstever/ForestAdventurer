using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MenuState
{
    MainMenu,
    Game,
    GameEnd
}

public enum GameState
{
    CombatStart,
    CombatPlayerTurn,
    CombatEnemyTurn,
    CombatEnd,
    CardSelection
}

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance = null;
        
    private MenuState currentMenuState;
    private GameState currentGameState;

    public MenuState CurrentMenuState { get { return currentMenuState; } }
    public GameState CurrentGameState { get { return currentGameState; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeMenuState(MenuState.Game);
        ChangeGameState(GameState.CombatStart);
    }

    public void ChangeMenuState(MenuState newMenuState) {
        switch(newMenuState) {
            case MenuState.MainMenu:
                break;
            case MenuState.Game:
                break;
            case MenuState.GameEnd:
                break;
        }

        currentMenuState = newMenuState;
    }

    public void ChangeGameState(GameState newGameState) {
        switch(newGameState) {
            case GameState.CombatStart:
                DeckManager.instance.SetupForNewRound();
                EnemyManager.instance.SpawnNextRound();
                break;
            case GameState.CombatPlayerTurn:
                DeckManager.instance.DiscardHand();
                DeckManager.instance.DealHand();
                break;
            case GameState.CombatEnemyTurn:
                break;
            case GameState.CombatEnd:
                break;
        }

        currentGameState = newGameState;
    }
}
