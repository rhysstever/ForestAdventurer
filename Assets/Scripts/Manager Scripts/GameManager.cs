using UnityEngine;

public enum MenuState
{
    MainMenu,
    CharacterSelect,
    Game,
    GameEnd
}

public enum GameState
{
    None,
    Combat,
    CombatWin,
    // TODO - Add other game states
    // Shop
}

public enum CombatState
{
    None,
    CombatStart,
    CombatPlayerTurn,
    CombatEnemyTurn,
    CombatEnd
}

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance = null;

    // Instantiated in inspector
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform playerPostion;

    // Instantiated in code
    private Player player;
    [SerializeField]
    private MenuState currentMenuState;
    [SerializeField]
    private GameState currentGameState;
    [SerializeField]
    private CombatState currentCombatState;

    public Player Player { get { return player; } }
    public MenuState CurrentMenuState { get { return currentMenuState; } }
    public GameState CurrentGameState { get { return currentGameState; } }
    public CombatState CurrentCombatState { get { return currentCombatState; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeMenuState(MenuState.MainMenu);
    }

    public void ChangeMenuState(MenuState newMenuState) {
        currentMenuState = newMenuState;

        switch(newMenuState) {
            case MenuState.MainMenu:
                // Destroy the player if it exists
                if(player != null) {
                    Destroy(player.gameObject);
                }
                EnemyManager.instance.Reset();
                ChangeGameState(GameState.None);
                ChangeCombatState(CombatState.None);
                break;
            case MenuState.CharacterSelect:
                ChangeGameState(GameState.None);
                ChangeCombatState(CombatState.None);
                break;
            case MenuState.Game:
                // If the player does not exist, create one
                if(player == null) {
                    GameObject playerObj = Instantiate(playerPrefab, playerPostion.position, Quaternion.identity, transform);
                    player = playerObj.GetComponent<Player>();
                    // TODO - set the player sprite based on the chosen character
                }
                // TODO - remove this and start combat at the appropriate time 
                ChangeGameState(GameState.Combat);
                break;
            case MenuState.GameEnd:
                ChangeGameState(GameState.None);
                ChangeCombatState(CombatState.None);
                break;
        }

        UIManager.instance.UpdateMenuUI(newMenuState);
    }

    public void ChangeGameState(GameState newGameState) {
        currentGameState = newGameState;

        switch(newGameState) {
            case GameState.Combat:
                ChangeCombatState(CombatState.CombatStart);
                break;
            case GameState.CombatWin:
                ChangeCombatState(CombatState.None);
                DeckManager.instance.SpawnCardSelectionDisplayCards();
                break;
            case GameState.None:
                DeckManager.instance.DiscardHand();
                break;
        }

        UIManager.instance.UpdateGameUI(newGameState);
    }

    public void ChangeCombatState(CombatState newCombatState) {
        currentCombatState = newCombatState;

        switch(newCombatState) {
            case CombatState.CombatStart:
                EnemyManager.instance.SpawnNextWave();
                DeckManager.instance.SetupForNewCombat();
                break;
            case CombatState.CombatPlayerTurn:
                DeckManager.instance.DealHand();
                break;
            case CombatState.CombatEnemyTurn:
                DeckManager.instance.DiscardHand();
                EnemyManager.instance.PerformEnemyRoundActions();
                break;
            case CombatState.CombatEnd:
                ChangeGameState(GameState.CombatWin);
                break;
            case CombatState.None:
                DeckManager.instance.DiscardHand();
                break;
        }

        UIManager.instance.UpdateCombatUI(newCombatState);
    }
}
