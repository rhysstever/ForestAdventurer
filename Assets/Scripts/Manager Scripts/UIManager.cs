using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager instance;

    [SerializeField]
    private GameObject mainMenuUIParent, characterSelectUIParent, gameUIParent, gameEndUIParent;
    [SerializeField]
    private GameObject combatUIParent, combatWinUIParent, cardSelectionUIParent;
    [SerializeField]
    private Button mainMenuToCharacterSelectButton, endTurnButton, gameEndToMainMenuButton, badgerCharacterButton, beaverCharacterButton, foxCharacterButton;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuToCharacterSelectButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.CharacterSelect));
        endTurnButton.onClick.AddListener(() => GameManager.instance.ChangeCombatState(CombatState.CombatEnemyTurn));
        gameEndToMainMenuButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));

        badgerCharacterButton.onClick.AddListener(delegate {
            CardManager.instance.ChooseCharacter("Badger");
            GameManager.instance.ChangeMenuState(MenuState.Game);
        });
        beaverCharacterButton.onClick.AddListener(delegate {
            CardManager.instance.ChooseCharacter("Beaver");
            GameManager.instance.ChangeMenuState(MenuState.Game);
        });
        foxCharacterButton.onClick.AddListener(delegate {
            CardManager.instance.ChooseCharacter("Fox");
            GameManager.instance.ChangeMenuState(MenuState.Game);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMenuUI(MenuState menuState) {
        switch(menuState) {
            case MenuState.MainMenu:
                mainMenuUIParent.SetActive(true);
                characterSelectUIParent.SetActive(false);
                gameUIParent.SetActive(false);
                gameEndUIParent.SetActive(false);
                break;
            case MenuState.CharacterSelect:
                mainMenuUIParent.SetActive(false);
                characterSelectUIParent.SetActive(true);
                gameUIParent.SetActive(false);
                gameEndUIParent.SetActive(false);
                break;
            case MenuState.Game:
                mainMenuUIParent.SetActive(false);
                characterSelectUIParent.SetActive(false);
                gameUIParent.SetActive(true);
                gameEndUIParent.SetActive(false);
                break;
            case MenuState.GameEnd:
                mainMenuUIParent.SetActive(false);
                characterSelectUIParent.SetActive(false);
                gameUIParent.SetActive(false);
                gameEndUIParent.SetActive(true);
                break;
        }
    }

    public void UpdateGameUI(GameState gameState) {
        switch(gameState) {
            case GameState.Combat:
                combatUIParent.SetActive(true);
                break;
            case GameState.CombatWin:
                combatWinUIParent.SetActive(true);
                break;
            case GameState.None:
                combatUIParent.SetActive(false);
                combatWinUIParent.SetActive(false);
                break;
        }
    }

    public void UpdateCombatUI(CombatState combatState) {

    }
}
