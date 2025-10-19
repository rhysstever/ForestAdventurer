using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // Singleton
    public static DeckManager instance;

    // Instantiated in inspector
    [SerializeField]
    private Player player;
    [SerializeField]
    private Transform cardParentTrans;
    [SerializeField]
    private GameObject cardPrefab;

    // Instantiated in script
    private int currentDeckSize, currentHandSize;
    private List<Card> deck, hand, discard;

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
        currentHandSize = 4;

        deck = GenerateStarterDeck();
        currentDeckSize = deck.Count;

        hand = DrawCards(currentHandSize);

        float cardXOffset = 3.5f;
        float cardRowXOffset = 4.5f;
        for(int i = 0; i < hand.Count; i++) { 
            SpawnCard(hand[i], new Vector2(cardXOffset * i - cardRowXOffset, -5f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Card> GenerateStarterDeck() {
        List<Card> cards = new List<Card>();

        // === 12 card starter deck ===
        // 4 main-hands cards
        for(int i = 0; i < 4; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.MainHand));
        }
        // 4 off-hands cards
        for(int i = 0; i < 4; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.OffHand));
        }
        // 2 spell cards
        for(int i = 0; i < 2; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.Spell));
        }
        // 2 drink cards
        for(int i = 0; i < 2; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.Drink));
        }

        return cards;
    }

    public List<Card> GenerateDeck() {
        List<Card> cards = new List<Card>();

        // === 12 card deck ===
        // 4 main-hands cards
        for(int i = 0; i < 4; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.MainHand));
        }
        // 4 off-hands cards
        for(int i = 0; i < 4; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.OffHand));
        }
        // 2 spell cards
        for(int i = 0; i < 2; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.Spell));
        }
        // 2 drink cards
        for(int i = 0; i < 2; i++) {
            cards.Add(CardManager.instance.GetStarterCardData(Slot.Drink));
        }

        return cards;
    }

    public List<Card> DrawCards(int numberOfCardsToDraw) {
        List<int> cardIndicesToDraw = new List<int>();

        // For the number of cards to draw, find a unique index
        // of a card to draw from the deck
        for(int i = 0; i < numberOfCardsToDraw; i++) {
            int newIndex = Random.Range(0, currentDeckSize);
            if(cardIndicesToDraw.Count > 0) {
                while(cardIndicesToDraw.Contains(newIndex)) {
                    newIndex = Random.Range(0, currentDeckSize);
                }
            }
            cardIndicesToDraw.Add(newIndex);
        }

        // Map each int index to a card in the deck
        return cardIndicesToDraw.Select(index => deck[index]).ToList();
    }

    public GameObject SpawnCard(Card cardData) {
        Vector2 defaultPos = new Vector2(0f, -5f);
        return SpawnCard(cardData, defaultPos);
    }

    public GameObject SpawnCard(Card cardData, Vector2 position) {
        GameObject newCard = Instantiate(cardPrefab, position, Quaternion.identity, cardParentTrans);
        CardObject cardObj = newCard.GetComponent<CardObject>();
        cardObj.SetCardNameText(cardData.Name);

        return newCard;
    }
}
