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
    private List<CardData> deck, hand, discard;
    [SerializeField]
    private bool isTargetting;
    [SerializeField]
    private GameObject target;

    public bool IsTargetting { get { return isTargetting; } }
    public GameObject Target { get { return target; } }

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

        isTargetting = false;
        target = null;
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
        for(int i = 0; i < 4; i++) {
            cards.Add(mainHand);
            cards.Add(offHand);
        }
        // 2 spell and 2 drink cards
        CardData spell = CardManager.instance.GetCurrentCardData(Slot.Spell);
        CardData drink = CardManager.instance.GetCurrentCardData(Slot.Drink);
        for(int i = 0; i < 2; i++) {
            cards.Add(spell);
            cards.Add(drink);
        }
        // 3 ally and 3 spirit cards (if available)
        CardData ally = CardManager.instance.GetCurrentCardData(Slot.Ally);
        CardData spirit = CardManager.instance.GetCurrentCardData(Slot.Spirit);
        for(int i = 0; i < 3; i++) {
            if(ally != null) {
                cards.Add(ally);
            }
            if(spirit != null) {
                cards.Add(spirit);
            }
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

    public void StartTargetting() {
        isTargetting = true;
    }

    public void StopTargetting() {
        isTargetting = false;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }
}
