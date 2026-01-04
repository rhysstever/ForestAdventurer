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
    private GameObject mainMenuButtonsParent, combatUIParent, nonCombatUIParent, cardSelectionUIParent, wellUIParent;
    [SerializeField]
    private GameObject viewDeckUIParent, viewDeckCardsUIParent;
    [SerializeField]
    private Button mainMenuToCharacterSelectButton, quitButton, viewDeckButton, closeViewDeckButton, endTurnButton, selectCardButton, skipButton, drinkWellButton, gameEndToMainMenuButton;
    [SerializeField]
    private TMP_Text characterSelectInfoText, gameAreaStageText, gameEndHeaderText, gameEndDeckInfoText;

    private bool isDeckBeingViewed;

    public bool IsDeckBeingViewed { get { return isDeckBeingViewed; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set up button listeners
        mainMenuToCharacterSelectButton.onClick.AddListener(() => {
            mainMenuButtonsParent.SetActive(false);
            Camera.main.GetComponent<CameraPan>().PanCameraDown();
        });
        quitButton.onClick.AddListener(() => Application.Quit());
        viewDeckButton.onClick.AddListener(() => ShowDeckInfo());
        closeViewDeckButton.onClick.AddListener(() => HideDeckInfo());
        endTurnButton.onClick.AddListener(() => GameManager.instance.ChangeCombatState(CombatState.CombatEnemyTurn));
        selectCardButton.onClick.AddListener(() => {
            DeckManager.instance.AddSelectedCardToDeck();
            GameManager.instance.GoToNextStage();
        });
        skipButton.onClick.AddListener(() => {
            DeckManager.instance.ClearCardSelectionDisplayCards();
            GameManager.instance.GoToNextStage();
        });
        drinkWellButton.onClick.AddListener(() => {
            GameManager.instance.Player.HealFull();
            GameManager.instance.GoToNextStage();
        });
        gameEndToMainMenuButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
    }

    public void UpdateMenuUI(MenuState menuState)
    {
        switch(menuState)
        {
            case MenuState.MainMenu:
                mainMenuUIParent.SetActive(true);
                mainMenuButtonsParent.SetActive(true);
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
                HideDeckInfo();
                break;
            case MenuState.GameEnd:
                mainMenuUIParent.SetActive(false);
                characterSelectUIParent.SetActive(false);
                gameUIParent.SetActive(false);
                gameEndUIParent.SetActive(true);
                UpdateGameEndText();
                break;
        }
    }

    public void UpdateGameUI(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Combat:
                combatUIParent.SetActive(true);
                nonCombatUIParent.SetActive(false);

                endTurnButton.gameObject.SetActive(true);
                break;
            case GameState.CardSelection:
                nonCombatUIParent.SetActive(true);
                cardSelectionUIParent.SetActive(true);
                wellUIParent.SetActive(false);

                endTurnButton.gameObject.SetActive(false);
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

    public void UpdateCombatUI(CombatState combatState)
    {
        switch(combatState)
        {
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

    public void UpdateCharacterSelectInfo()
    {
        characterSelectInfoText.text = "Choose Your Character";
        characterSelectInfoText.verticalAlignment = VerticalAlignmentOptions.Middle;
    }

    public void UpdateCharacterSelectInfo(Character character)
    {
        int[] deckStructure = CharacterManager.instance.GetCharacterDeckStructure(character);
        characterSelectInfoText.text = string.Format(
            "{0}\n" +
            "\n{1} main hands" +
            "\n{2} off hands" +
            "\n{3} allies" +
            "\n{4} spirits" +
            "\n{5} spells" +
            "\n{6} drinks",
            character.ToString(),
            deckStructure[0],
            deckStructure[1],
            deckStructure[2],
            deckStructure[3],
            deckStructure[4],
            deckStructure[5]);

        characterSelectInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
    }

    public void UpdateStageText()
    {
        gameAreaStageText.text = GameManager.instance.GetCurrentStageText();
    }

    private void ShowDeckInfo()
    {
        isDeckBeingViewed = true;
        viewDeckUIParent.SetActive(true);
        viewDeckButton.gameObject.SetActive(false);

        UpdateButtonInteractability(false);

        DeckManager.instance.DisplayDeckCards(viewDeckCardsUIParent.transform);
    }

    private void HideDeckInfo()
    {
        isDeckBeingViewed = false;
        viewDeckUIParent.SetActive(false);
        viewDeckButton.gameObject.SetActive(true);

        UpdateButtonInteractability(true);

        // Destroy displayed cards
        foreach(Transform child in viewDeckCardsUIParent.transform)
        {
            if(child.childCount > 0)
            {
                Destroy(child.GetChild(0).gameObject);
            }
        }
    }

    /// <summary>
    /// Update active buttons' interactability
    /// </summary>
    /// <param name="interactable">Whether buttons should be interactable</param>
    private void UpdateButtonInteractability(bool interactable)
    {
        if(endTurnButton.gameObject.activeSelf)
        {
            endTurnButton.interactable = interactable;
        }

        if(selectCardButton.gameObject.activeSelf)
        {
            selectCardButton.interactable = interactable;
        }

        if(skipButton.gameObject.activeSelf)
        {
            skipButton.interactable = interactable;
        }
    }

    public void SetCardSelectionButton(bool isACardSelected)
    {
        selectCardButton.interactable = isACardSelected;
    }

    public void UpdateGameEndText()
    {
        bool victory = GameManager.instance.Player.CurrentLife > 0;
        if(victory)
        {
            gameEndHeaderText.text = "VICTORY";
        }
        else
        {
            gameEndHeaderText.text = string.Format(
                "SLAIN ON {0}",
                GameManager.instance.GetCurrentStageText());
        }

        // Check for the current spirit card, which has no starter
        string spiritText = "None";
        if(CardManager.instance.GetCurrentCardData(Slot.Spirit) != null)
        {
            spiritText = CardManager.instance.GetCurrentCardData(Slot.Spirit).Name;
        }

        gameEndDeckInfoText.text = string.Format(
            "Character: {0}" +
            "\n\nDeck" +
            "\nMain Hand: {1}" +
            "\nOff Hand: {2}" +
            "\nAlly: {3}" +
            "\nSpirit: {4}" +
            "\nSpell: {5}" +
            "\nDrink: {6}",
            CharacterManager.instance.ChosenCharacter,
            CardManager.instance.GetCurrentCardData(Slot.MainHand).Name,
            CardManager.instance.GetCurrentCardData(Slot.OffHand).Name,
            CardManager.instance.GetCurrentCardData(Slot.Ally).Name,
            spiritText,
            CardManager.instance.GetCurrentCardData(Slot.Spell).Name,
            CardManager.instance.GetCurrentCardData(Slot.Drink).Name);
    }
}
