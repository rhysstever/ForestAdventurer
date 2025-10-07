using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    private int currentDeckSize, currentHandSize;
    private List<Card> deck, hand;

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
        Debug.Log("Deck");
        for(int i = 0; i < deck.Count; i++) {
            Debug.Log(deck[i].Name);
        }
        Debug.Log("Hand");
        for(int i = 0; i < hand.Count; i++) {
            Debug.Log(hand[i].Name);
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
            cards.Add(CardManager.instance.GetStarterCard(Slot.MainHand));
        }
        // 4 off-hands cards
        for(int i = 0; i < 4; i++) {
            cards.Add(CardManager.instance.GetStarterCard(Slot.OffHand));
        }
        // 2 spell cards
        for(int i = 0; i < 2; i++) {
            cards.Add(CardManager.instance.GetStarterCard(Slot.Spell));
        }
        // 2 drink cards
        for(int i = 0; i < 2; i++) {
            cards.Add(CardManager.instance.GetStarterCard(Slot.Drink));
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
}
