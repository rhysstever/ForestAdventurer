using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    // Singleton
    public static DeckManager instance;

    // Set in inspector
    [SerializeField]
    private Transform cardParentTrans, cardSelectionCardParentTrans, cardSelectionCard1Pos, cardSelectionCard2Pos, cardSelectionCard3Pos;
    [SerializeField]
    private Collider2D fieldCollider;
    [SerializeField]
    private GameObject playableCardPrefab, selectableCardPrefab, displayCardPrefab;

    // Set in script
    private List<CardData> deck, hand, discard;
    private int currentHandSize;
    private CardData currentCardSelection;

    // Properties
    public Collider2D FieldCollider { get { return fieldCollider; } }
    public CardData CurrentCardSelection { get { return currentCardSelection; } }

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

        deck = new List<CardData>();
        hand = new List<CardData>();
        discard = new List<CardData>();
    }

    void Start()
    {
        currentCardSelection = null;
        currentHandSize = 4;
    }

    public void SetupForNewCombat()
    {
        deck = GenerateDeck();
        hand.Clear();
        discard.Clear();
        fieldCollider.gameObject.SetActive(true);
        GameManager.instance.ChangeCombatState(CombatState.CombatPlayerTurn);
    }

    public List<CardData> GenerateDeck()
    {
        List<CardData> cards = new List<CardData>();
        Character chosenCharacter = CharacterManager.instance.ChosenCharacter;

        foreach(Slot slot in Enum.GetValues(typeof(Slot)).Cast<Slot>().ToList())
        {
            CardData cardData = CardManager.instance.GetCurrentCardData(slot);

            if(cardData != null)
            {
                int cardCount = CharacterManager.instance.GetSlotCardCountOfChosenCharacter(slot);
                for(int i = 0; i < cardCount; i++)
                {
                    cards.Add(cardData);
                }
            }
            else
            {
                Debug.LogFormat("Warning! No current {0} card, not generating", slot);
            }
        }

        return cards;
    }

    public void DiscardHand()
    {
        RemoveCardsFromScene();

        // Create a list copy of the cards still in hand,
        // add them to the discard list and clear the hand list
        List<CardData> remainingCardsInHand = hand.ToList();
        discard.AddRange(remainingCardsInHand);
        hand.Clear();
    }

    public void DealHand()
    {
        RemoveCardsFromScene();
        DrawCards(currentHandSize);
    }

    public void DrawCards(int numberOfCardsToDraw)
    {
        // Add the given number of cards from the deck into the hand
        for(int i = 0; i < numberOfCardsToDraw; i++)
        {
            if(deck.Count == 0)
            {
                // If the deck is empty, move the discard pile into the deck
                ShuffleDiscardPileIntoDeck();
            }

            // Get a random index of the deck list
            int newIndex = UnityEngine.Random.Range(0, deck.Count);

            // Get the card from the deck, add it to the hand, and remove it from the deck
            CardData card = deck[newIndex];
            hand.Add(card);
            deck.RemoveAt(newIndex);
        }

        float cardXOffset = 3.5f;
        float cardRowXOffset = 4.5f;

        // Spawn the all the cards in the scene
        for(int i = 0; i < hand.Count; i++)
        {
            SpawnCard(playableCardPrefab, hand[i], new Vector2(cardXOffset * i - cardRowXOffset, -5f), cardParentTrans);
        }
    }

    public void ShuffleDiscardPileIntoDeck()
    {
        List<CardData> cardsInDiscardPile = discard.ToList();
        deck.AddRange(cardsInDiscardPile);
        discard.Clear();
    }

    private void RemoveCardsFromScene()
    {
        for(int i = cardParentTrans.childCount - 1; i >= 0; i--)
        {
            Destroy(cardParentTrans.GetChild(i).gameObject);
        }
    }

    private GameObject SpawnCard(GameObject cardPrefab, CardData cardData, Vector2 position, Transform parent)
    {
        GameObject newCard = Instantiate(cardPrefab, position, Quaternion.identity, parent);
        CardObject cardObj = newCard.GetComponent<CardObject>();
        cardObj.SetCardData(cardData);

        return newCard;
    }

    public void SetupCardSelection()
    {
        // Hide the field collider
        fieldCollider.gameObject.SetActive(false);

        // Create the card selection cards to display
        List<CardData> cardDatasToDisplay = CardManager.instance.GetRandomCardDatas(3);
        for(int i = 0; i < cardDatasToDisplay.Count; i++)
        {
            Vector2 position = Vector2.zero;
            switch(i)
            {
                case 0:
                    position = cardSelectionCard1Pos.position;
                    break;
                case 1:
                    position = cardSelectionCard2Pos.position;
                    break;
                case 2:
                    position = cardSelectionCard3Pos.position;
                    break;
            }

            SpawnCard(selectableCardPrefab, cardDatasToDisplay[i], position, cardSelectionCardParentTrans);
        }

        // Reset the current selection
        SetCurrentCardSelection(null);
    }

    public void SetCurrentCardSelection(CardData cardSelected)
    {
        currentCardSelection = cardSelected;
        // Update UI
        UIManager.instance.SetCardSelectionButton(cardSelected != null);
    }

    public void AddSelectedCardToDeck()
    {
        if(cardSelectionCardParentTrans != null)
        {
            fieldCollider.gameObject.SetActive(true);
            CardManager.instance.UpdateSlot(currentCardSelection);
            ClearCardSelectionDisplayCards();
        }
        else
        {
            Debug.Log("Error! No card selected");
        }
    }

    public void ClearCardSelectionDisplayCards()
    {
        for(int i = cardSelectionCardParentTrans.childCount - 1; i >= 0; i--)
        {
            Destroy(cardSelectionCardParentTrans.GetChild(i).gameObject);
        }
    }

    public void DisplayDeckCards(Transform viewDeckCardParent)
    {
        for(int i = 0; i < viewDeckCardParent.childCount; i++)
        {
            Transform child = viewDeckCardParent.GetChild(i);
            CardData cardDataToDisplay = CardManager.instance.GetCurrentCardData((Slot)i);

            SpawnCard(displayCardPrefab, cardDataToDisplay, child.position, child);
        }
    }
}
