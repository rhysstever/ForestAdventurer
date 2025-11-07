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
    private GameObject playableCardPrefab, displayCardPrefab;

    // Set in script
    private int currentHandSize;
    private List<CardData> deck, hand, discard;

    // Properties
    public Collider2D FieldCollider { get { return fieldCollider; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        deck = new List<CardData>();
        hand = new List<CardData>();
        discard = new List<CardData>();
    }

    void Start()
    {
        currentHandSize = 4;
    }

    public void SetupForNewCombat() {
        deck = GenerateDeck();
        hand.Clear();
        discard.Clear();
        GameManager.instance.ChangeCombatState(CombatState.CombatPlayerTurn);
    }

    public List<CardData> GenerateDeck() {
        List<CardData> cards = new List<CardData>();
        Character chosenCharacter = CardManager.instance.ChosenCharacter;

        foreach(Slot slot in Enum.GetValues(typeof(Slot)).Cast<Slot>().ToList()) {
            CardData cardData = CardManager.instance.GetCurrentCardData(slot);

            if(cardData != null) {
                int cardCount = CardManager.instance.GetSlotCardCountOfChosenCharacter(slot);
                for(int i = 0; i < chosenCharacter.MainHandCardCount; i++) {
                    cards.Add(cardData);
                }
            } else {
                Debug.LogFormat("Warning! No current {0} card, not generating", slot);
            }
        }

        return cards;
    }

    public void DiscardHand() {
        RemoveCardsFromScene();

        // Create a list copy of the cards still in hand,
        // add them to the discard list and clear the hand list
        List<CardData> remainingCardsInHand = hand.ToList();
        discard.AddRange(remainingCardsInHand);
        hand.Clear();
    }

    public void DealHand() {
        RemoveCardsFromScene();
        DrawCards(currentHandSize);
    }

    public void DrawCards(int numberOfCardsToDraw) {
        // Add the given number of cards from the deck into the hand
        for(int i = 0; i < numberOfCardsToDraw; i++) {
            if(deck.Count == 0) {
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
        for(int i = 0; i < hand.Count; i++) {
            SpawnCard(playableCardPrefab, hand[i], new Vector2(cardXOffset * i - cardRowXOffset, -5f), cardParentTrans);
        }
    }

    public void ShuffleDiscardPileIntoDeck() {
        List<CardData> cardsInDiscardPile = discard.ToList();
        deck.AddRange(cardsInDiscardPile);
        discard.Clear();
    }

    private void RemoveCardsFromScene() {
        for(int i = cardParentTrans.childCount - 1; i >= 0; i--) {
            Destroy(cardParentTrans.GetChild(i).gameObject);
        }
    }

    private GameObject SpawnCard(GameObject cardPrefab, CardData cardData, Vector2 position, Transform parent) {
        GameObject newCard = Instantiate(cardPrefab, position, Quaternion.identity, parent);
        CardObject cardObj = newCard.GetComponent<CardObject>();
        cardObj.SetCardData(cardData);

        return newCard;
    }

    public void SpawnCardSelectionDisplayCards() {
        fieldCollider.gameObject.SetActive(false);
        for(int i = 0; i < 3; i++) {
            CardData newCardData = CardManager.instance.GetRandomCardData();
            Vector2 position = Vector2.zero;
            switch(i) {
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

            GameObject newCard = SpawnCard(displayCardPrefab, newCardData, position, cardSelectionCardParentTrans);
            newCard.GetComponent<Button>().onClick.AddListener(() => {
                fieldCollider.gameObject.SetActive(true);
                CardManager.instance.UpdateSlot(newCardData);
                ClearCardSelectionDisplayCards();
                GameManager.instance.ChangeGameState(GameState.Combat);
            });
        }
    }

    public void ClearCardSelectionDisplayCards() {
        for(int i = cardSelectionCardParentTrans.childCount - 1; i >= 0; i--) {
            Destroy(cardSelectionCardParentTrans.GetChild(i).gameObject);
        }
    }
}
