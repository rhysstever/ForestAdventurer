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
    CardSelection,
    Well
    // TODO - Add shop game states
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
    private MenuState currentMenuState;
    private GameState currentGameState;
    private CombatState currentCombatState;
    private int currentAreaIndex;
    private int currentStageIndex;

    // Properties
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
                currentAreaIndex = -1;
                currentStageIndex = -1;
                EnemyManager.instance.Reset();
                ChangeGameState(GameState.None);
                ChangeCombatState(CombatState.None);
                break;
            case MenuState.CharacterSelect:
                CharacterManager.instance.ShowCharacterSelectIcons();
                ChangeGameState(GameState.None);
                ChangeCombatState(CombatState.None);
                break;
            case MenuState.Game:                
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
            case GameState.CardSelection:
                ChangeCombatState(CombatState.None);
                DeckManager.instance.SetupCardSelection();
                break;
            case GameState.Well:
                ChangeCombatState(CombatState.None);
                break;
            case GameState.None:
                ChangeCombatState(CombatState.None);
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
                player.ProcessEffects();
                DeckManager.instance.DealHand();
                break;
            case CombatState.CombatEnemyTurn:
                EnemyManager.instance.ProcessEffectsOnEnemies();
                DeckManager.instance.DiscardHand();
                if(EnemyManager.instance.IsWaveOver()) {
                    ChangeCombatState(CombatState.CombatEnd);
                    return;
                } else {
                    EnemyManager.instance.PerformEnemyRoundActions();
                }
                break;
            case CombatState.CombatEnd:
                player.PostCombatReset();
                ChangeGameState(GameState.CardSelection);
                break;
            case CombatState.None:
                DeckManager.instance.DiscardHand();
                break;
        }

        UIManager.instance.UpdateCombatUI(newCombatState);
    }

    public void StartGame()
    {
        ChangeMenuState(MenuState.Game);

        // Create Player
        GameObject playerObj = Instantiate(playerPrefab, playerPostion.position, Quaternion.identity, transform);
        player = playerObj.GetComponent<Player>();
        // TODO - set the player sprite based on the chosen character

        EnterArea();
    }

    private void EnterArea()
    {
        currentAreaIndex++;
        currentStageIndex = -1;
        GoToNextStage();
    }

    public void GoToNextStage()
    {
        currentStageIndex++;
        // Location Order:
        // 0) Combat, Wave 0
        // 1) Combat, Wave 1
        // 2) Well
        // 3) Combat, Wave 2
        // 4) Combat, Wave 3 (Mini Boss)
        // 5) Well
        // 6) Combat, Wave 4 (Boss)
        switch(currentStageIndex)
        {
            case 2:
            case 5:
                ChangeGameState(GameState.Well);
                break;
            default:
                ChangeGameState(GameState.Combat);
                break;
        }

        UIManager.instance.UpdateStageText();
    }

    public string GetCurrentStageText()
    {
        int area = currentAreaIndex + 1;
        string stageText = currentStageIndex switch
        {
            2 => "W",
            5 => "W",
            _ => (currentStageIndex + 1).ToString(),
        };

        return string.Format("{0}-{1}", area, stageText);
    }
}
