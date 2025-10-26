using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
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
    private List<CardData> deck, hand, discard;

    public Player Player { get { return player; } }

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

        deck = GenerateDeck();
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

    public List<CardData> GenerateDeck() {
        List<CardData> cards = new List<CardData>();

        // 4 main hand and 4 off hand cards
        CardData mainHand = CardManager.instance.GetCurrentCardData(Slot.MainHand);
        CardData offHand = CardManager.instance.GetCurrentCardData(Slot.OffHand);
        CardData ally = CardManager.instance.GetCurrentCardData(Slot.Ally);
        CardData spirit = CardManager.instance.GetCurrentCardData(Slot.Spirit);
        CardData spell = CardManager.instance.GetCurrentCardData(Slot.Spell);
        CardData drink = CardManager.instance.GetCurrentCardData(Slot.Drink);

        if(mainHand != null) {
            for(int i = 0; i < 4; i++) {
                cards.Add(mainHand);
            }
        } else {
            Debug.Log("Warning! No current MainHand card, not drawing");
        }

        if(offHand != null) {
            for(int i = 0; i < 4; i++) {
                cards.Add(offHand);
            }
        } else {
            Debug.Log("Warning! No current OffHand card, not drawing");
        }

        if(ally != null) {
            for(int i = 0; i < 3; i++) {
                cards.Add(ally);
            }
        } else {
            Debug.Log("Warning! No current Ally card, not drawing");
        }

        if(spirit != null) {
            for(int i = 0; i < 3; i++) {
                cards.Add(spirit);
            }
        } else {
            Debug.Log("Warning! No current Spirit card, not drawing");
        }

        if(spell != null) {
            for(int i = 0; i < 2; i++) {
                cards.Add(spell);
            }
        } else {
            Debug.Log("Warning! No current Spell card, not drawing");
        }

        if(drink != null) {
            for(int i = 0; i < 2; i++) {
                cards.Add(drink);
            }
        } else {
            Debug.Log("Warning! No current Drink card, not drawing");
        }

        return cards;
    }

    public List<CardData> DrawCards(int numberOfCardsToDraw) {
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

    public GameObject SpawnCard(CardData cardData) {
        Vector2 defaultPos = new Vector2(0f, -5f);
        return SpawnCard(cardData, defaultPos);
    }

    public GameObject SpawnCard(CardData cardData, Vector2 position) {
        GameObject newCard = Instantiate(cardPrefab, position, Quaternion.identity, cardParentTrans);
        CardObject cardObj = newCard.GetComponent<CardObject>();
        cardObj.SetCardData(cardData);

        return newCard;
    }
}
