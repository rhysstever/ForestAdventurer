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
    private MainHand mainHand;
    private OffHand offHand;
    private Spirit spirit;
    private Ally ally;
    private Spell spell;
    private Drink drink;
    // TODO: Add in passive slots
    //private GameObject hat;
    //private GameObject boots;

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
        cards.Add(new MainHand(Rarity.Starter, "Shortsword", 1));
        cards.Add(new MainHand(Rarity.Common, "Wand", 1));
        cards.Add(new MainHand(Rarity.Common, "Longsword", 2));
        cards.Add(new MainHand(Rarity.Common, "Staff", 2));
        cards.Add(new MainHand(Rarity.Common, "Mace", 2));
        cards.Add(new MainHand(Rarity.Common, "Flail", 2));
        cards.Add(new MainHand(Rarity.Common, "Spear", 2));
        cards.Add(new MainHand(Rarity.Common, "Trident", 2));

        // Off hand cards
        cards.Add(new OffHand(Rarity.Starter, "Wooden Shield"));
        cards.Add(new OffHand(Rarity.Common, "Buckler"));
        cards.Add(new OffHand(Rarity.Common, "Tome"));
        cards.Add(new OffHand(Rarity.Common, "Spell Focus"));
        cards.Add(new OffHand(Rarity.Common, "Tower Shield"));

        // Ally cards
        cards.Add(new Ally(Rarity.Common, "Squirrel"));
        cards.Add(new Ally(Rarity.Common, "Frog"));
        cards.Add(new Ally(Rarity.Common, "Rat"));
        cards.Add(new Ally(Rarity.Common, "Bunny"));
        cards.Add(new Ally(Rarity.Rare, "Newt"));
        cards.Add(new Ally(Rarity.Rare, "Porcupine"));
        cards.Add(new Ally(Rarity.Rare, "Hampster"));

        // Spirit cards
        cards.Add(new Spirit(Rarity.Common, "Air Spirit"));
        cards.Add(new Spirit(Rarity.Common, "Earth Spirit"));
        cards.Add(new Spirit(Rarity.Common, "Fire Spirit"));
        cards.Add(new Spirit(Rarity.Common, "Water Spirit"));

        // Spell cards
        cards.Add(new Spell(Rarity.Starter, "Arcane Bolt", TargetType.Unit, 2, 2));
        cards.Add(new Spell(Rarity.Common, "Fireball", TargetType.AOE, 3, 2));
        cards.Add(new Spell(Rarity.Common, "Life Drain", TargetType.Unit, 2, 2));
        cards.Add(new Spell(Rarity.Rare, "Lightning Bolt", TargetType.Unit, 3, 8));
        cards.Add(new Spell(Rarity.Rare, "Heal", TargetType.Self, 2, 5));
        cards.Add(new Spell(Rarity.Rare, "Blizzard", TargetType.AOE, 3, 5));

        // Drink cards
        cards.Add(new Drink(Rarity.Starter, "Cup"));
        cards.Add(new Drink(Rarity.Common, "Pouch"));
        cards.Add(new Drink(Rarity.Common, "Tankard"));
        cards.Add(new Drink(Rarity.Common, "Flask"));
        cards.Add(new Drink(Rarity.Rare, "Flagon"));
        cards.Add(new Drink(Rarity.Rare, "Goblet"));
        cards.Add(new Drink(Rarity.Rare, "Chalice"));
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
        mainHand = GetStarterCardData(Slot.MainHand) as MainHand;
        offHand = GetStarterCardData(Slot.OffHand) as OffHand;
        ally = GetStarterCardData(Slot.Ally) as Ally;
        spirit = GetStarterCardData(Slot.Spirit) as Spirit;
        spell = GetStarterCardData(Slot.Spell) as Spell;
        drink = GetStarterCardData(Slot.Drink) as Drink;
    }

    public void PlayCard(Slot slot) {
        CardData card = GetCurrentCardData(slot);

        Debug.Log(string.Format("{0} played", card.Name));
    }

    public void PlayCard(Slot slot, GameObject target) {
        CardData card = GetCurrentCardData(slot);

        Debug.Log(string.Format("{0} played at {1}", card.Name, target.name));
    }
}