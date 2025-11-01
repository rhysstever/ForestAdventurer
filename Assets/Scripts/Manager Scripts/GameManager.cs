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
        currentGameState = newGameState;

        switch(newGameState) {
            case GameState.CombatStart:
                EnemyManager.instance.SpawnNextWave();
                DeckManager.instance.SetupForNewCombat();
                break;
            case GameState.CombatPlayerTurn:
                DeckManager.instance.DiscardHand();
                DeckManager.instance.DealHand();
                break;
            case GameState.CombatEnemyTurn:
                // TODO: Do next move, determined by current round
                break;
            case GameState.CombatEnd:
                // TODO: Show rewards, including card selection
                // If it is not the last wave, enter combat with the next one
                if(!EnemyManager.instance.IsLastWave()) {
                    ChangeGameState(GameState.CombatStart);
                }
                break;
            case GameState.CardSelection:
                break;
        }
    }
}
