using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager instance;

    // Set in inspector
    [SerializeField]
    private GameObject mainMenuUIParent, characterSelectUIParent, gameUIParent, gameEndUIParent;
    [SerializeField]
    private GameObject combatUIParent, nonCombatUIParent, cardSelectionUIParent, wellUIParent;
    [SerializeField]
    private Button mainMenuToCharacterSelectButton, endTurnButton, skipButton, drinkWellButton, gameEndToMainMenuButton;
    [SerializeField]
    private TMP_Text characterSelectInfoText;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainMenuToCharacterSelectButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.CharacterSelect));
        endTurnButton.onClick.AddListener(() => GameManager.instance.ChangeCombatState(CombatState.CombatEnemyTurn));
        skipButton.onClick.AddListener(() => {
            DeckManager.instance.ClearCardSelectionDisplayCards();
            GameManager.instance.ChangeGameState(GameState.Combat);
        });
        drinkWellButton.onClick.AddListener(() => GameManager.instance.Player.Heal(1000));
        gameEndToMainMenuButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
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
                UpdateCharacterSelectInfo();
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
                nonCombatUIParent.SetActive(false);
                endTurnButton.enabled = true;
                break;
            case GameState.CardSelection:
                nonCombatUIParent.SetActive(true);
                cardSelectionUIParent.SetActive(true);
                wellUIParent.SetActive(false);
                endTurnButton.enabled = false;
                break;
            case GameState.Well:
                nonCombatUIParent.SetActive(true);
                cardSelectionUIParent.SetActive(false);
                wellUIParent.SetActive(true);
                break;
            case GameState.None:
                combatUIParent.SetActive(false);
                nonCombatUIParent.SetActive(false);
                break;
        }
    }

    public void UpdateCombatUI(CombatState combatState) {
        switch(combatState) {
            case CombatState.CombatStart:
                break;
            case CombatState.CombatPlayerTurn:
                break;
            case CombatState.CombatEnemyTurn:
                break;
            case CombatState.CombatEnd:
                break;
            case CombatState.None:
                break;
        }
    }

    public void UpdateCharacterSelectInfo() {
        characterSelectInfoText.text = "Choose Your Character";
    }

    public void UpdateCharacterSelectInfo(Character character) {
        int[] deckStructure = CharacterManager.instance.GetCharacterDeckStructure(character);
        characterSelectInfoText.text = string.Format(
            "{0}\n\nDeck:" +
            "\n{1} Main Hand Cards" +
            "\n{2} Off Hand Cards" +
            "\n{3} Ally Cards" +
            "\n{4} Spirit Cards" +
            "\n{5} Spell Cards" +
            "\n{6} Drink Cards", 
            character.ToString(),
            deckStructure[0],
            deckStructure[1],
            deckStructure[2],
            deckStructure[3],
            deckStructure[4],
            deckStructure[5]);
    }
}
