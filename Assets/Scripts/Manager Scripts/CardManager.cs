using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Slot
{
    MainHand,
    OffHand,
    Ally,
    Spirit,
    Spell,
    Drink
}

public class CardManager : MonoBehaviour
{
    // Singleton
    public static CardManager instance = null;

    // Instantiated in inspector
    [SerializeField]
    private Collider2D fieldCollider;

    // Instantiated in script
    private List<CardData> cardLibrary;
    private Dictionary<Rarity, float> rarityPercentages;

    // Slots
    private CardData mainHand, offHand, spirit, ally, spell, drink;
    // TODO: Add in passive slots
    //private CardData hat;
    //private CardData boots;

    // Properties
    public Collider2D FieldCollider { get { return fieldCollider; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        cardLibrary = CardCreation();

        Reset();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rarityPercentages = new Dictionary<Rarity, float>();
        rarityPercentages.Add(Rarity.Common, 0.75f);
        rarityPercentages.Add(Rarity.Rare, 0.25f);
    }

    #region Card Creation
    private List<CardData> CardCreation() {
        List<CardData> cards = new List<CardData>();

        // Main hand cards
        cards.Add(new CardData("Shortsword", Slot.MainHand, Rarity.Starter, TargetType.Unit));
        cards.Add(new CardData("Wand", Slot.MainHand, Rarity.Common, TargetType.Unit));
        cards.Add(new CardData("Longsword", Slot.MainHand, Rarity.Common, TargetType.Unit));
        cards.Add(new CardData("Staff", Slot.MainHand, Rarity.Common, TargetType.Unit));
        cards.Add(new CardData("Mace", Slot.MainHand, Rarity.Common, TargetType.Unit));
        cards.Add(new CardData("Flail", Slot.MainHand, Rarity.Rare, TargetType.AOE));
        cards.Add(new CardData("Spear", Slot.MainHand, Rarity.Rare, TargetType.Unit));
        cards.Add(new CardData("Trident", Slot.MainHand, Rarity.Rare, TargetType.Unit));

        // Off hand cards
        cards.Add(new CardData("Wooden Shield", Slot.OffHand, Rarity.Starter, TargetType.Self));
        cards.Add(new CardData("Buckler", Slot.OffHand, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Tome", Slot.OffHand, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Spell Focus", Slot.OffHand, Rarity.Rare, TargetType.Self));
        cards.Add(new CardData("Tower Shield", Slot.OffHand, Rarity.Rare, TargetType.Self));

        // Ally cards
        cards.Add(new CardData("Squirrel", Slot.Ally, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Frog", Slot.Ally, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Rat", Slot.Ally, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Bunny", Slot.Ally, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Newt", Slot.Ally, Rarity.Rare, TargetType.Self));
        cards.Add(new CardData("Porcupine", Slot.Ally, Rarity.Rare, TargetType.Self));
        cards.Add(new CardData("Hampster", Slot.Ally, Rarity.Rare, TargetType.Self));

        // Spirit cards
        cards.Add(new CardData("Air Spirit", Slot.Spirit, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Earth Spirit", Slot.Spirit, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Fire Spirit", Slot.Spirit, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Water Spirit", Slot.Spirit, Rarity.Common, TargetType.Self));

        // Spell cards
        cards.Add(new CardData("Arcane Bolt", Slot.Spell, Rarity.Starter, TargetType.Unit));
        cards.Add(new CardData("Fireball", Slot.Spell, Rarity.Common, TargetType.AOE));
        cards.Add(new CardData("Life Drain", Slot.Spell, Rarity.Common, TargetType.Unit));
        cards.Add(new CardData("Lightning Bolt", Slot.Spell, Rarity.Rare, TargetType.Unit));
        cards.Add(new CardData("Heal", Slot.Spell, Rarity.Rare, TargetType.Self));
        cards.Add(new CardData("Blizzard", Slot.Spell, Rarity.Rare, TargetType.AOE));

        // Drink cards
        cards.Add(new CardData("Cup", Slot.Drink, Rarity.Starter, TargetType.Self));
        cards.Add(new CardData("Pouch", Slot.Drink, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Tankard", Slot.Drink, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Flask", Slot.Drink, Rarity.Common, TargetType.Self));
        cards.Add(new CardData("Flagon", Slot.Drink, Rarity.Rare, TargetType.Self));
        cards.Add(new CardData("Goblet", Slot.Drink, Rarity.Rare, TargetType.Self));
        cards.Add(new CardData("Chalice", Slot.Drink, Rarity.Rare, TargetType.Self));
        return cards;
    }
    #endregion Card Creation

    private Rarity GetRandomRarity() {
        float currentRarityPercSum = 0f;
        // Get a random float
        float randomF = UnityEngine.Random.Range(0f, 1f);

        for(int i = 0; i < rarityPercentages.Count; i++) {
            // Find the next rarity
            KeyValuePair<Rarity, float> currentPair = rarityPercentages.ElementAt(i);
            // Add its rate to a running sum
            // Common -> 0.75 and lower
            // Rare -> 1.0 and lower
            currentRarityPercSum += currentPair.Value;
            // If the random float is less than the running sum, 
            // Return this rarity
            if(randomF <= currentRarityPercSum) {
                return currentPair.Key;
            }
        }

        // Display an error and return Common if no rarity was hit
        Debug.Log("Error! Rarity defaulted to Common for random num: $randomF");
        return Rarity.Common;
    }

    public CardData GetRandomCardData(Slot slotType) {
        Rarity randomRarity = GetRandomRarity();

        List<CardData> filterList = cardLibrary
            .FindAll(card => card.Slot == slotType)
            .FindAll(card => card.Rarity == randomRarity);
        int randomIndex = UnityEngine.Random.Range(0, filterList.Count);
        return filterList[randomIndex];
    }

    public CardData GetStarterCardData(Slot slotType) {
        List<CardData> filteredList = cardLibrary
            .FindAll(card => card.Slot == slotType)
            .FindAll(card => card.Rarity == Rarity.Starter);
        if(filteredList.Count > 0) { 
            return filteredList[0];
        } else {
            return null;
        }
    }

    /// <summary>
    /// Gets the object of a Card of the given slot
    /// </summary>
    /// <param name="slotType">The card type</param>
    /// <returns>A Card object, if card exists, otherwise null</returns>
    public CardData GetCurrentCardData(Slot slotType) {
        return slotType switch {
            Slot.MainHand => mainHand,
            Slot.OffHand => offHand,
            Slot.Ally => ally,
            Slot.Spirit => spirit,
            Slot.Spell => spell,
            Slot.Drink => drink,
            _ => null,
        };
    }

    /// <summary>
    /// Reset card slots to starting cards
    /// </summary>
    public void Reset() {
        // Setup starter slots
        mainHand = GetStarterCardData(Slot.MainHand);
        offHand = GetStarterCardData(Slot.OffHand);
        ally = GetStarterCardData(Slot.Ally);
        spirit = GetStarterCardData(Slot.Spirit);
        spell = GetStarterCardData(Slot.Spell);
        drink = GetStarterCardData(Slot.Drink);
    }
}