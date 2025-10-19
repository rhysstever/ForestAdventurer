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
    private List<Card> mainHandCards, offHandCards, allyCards, spiritCards, spellCards, drinkCards;
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

    public Collider2D FieldCollider { get { return fieldCollider; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        mainHandCards = new List<Card>();
        offHandCards = new List<Card>();
        allyCards = new List<Card>();
        spiritCards = new List<Card>();
        spellCards = new List<Card>();
        drinkCards = new List<Card>();

        Reset();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainHandCards = MainHandCreation();
        offHandCards = OffHandCreation();
        allyCards = AllyCreation();
        spiritCards = SpiritCreation();
        spellCards = SpellCreation();
        drinkCards = DrinkCreation();

        rarityPercentages = new Dictionary<Rarity, float>();
        rarityPercentages.Add(Rarity.Common, 0.75f);
        rarityPercentages.Add(Rarity.Rare, 0.25f);
    }

    #region Creation Methods
    private List<Card> MainHandCreation() {
        List<Card> cards = new List<Card>();
        cards.Add(new MainHand(Rarity.Starter, "Shortsword", 1));
        cards.Add(new MainHand(Rarity.Common, "Wand", 1));
        cards.Add(new MainHand(Rarity.Common, "Longsword", 2));
        cards.Add(new MainHand(Rarity.Common, "Staff", 2));
        cards.Add(new MainHand(Rarity.Common, "Mace", 2));
        cards.Add(new MainHand(Rarity.Common, "Flail", 2));
        cards.Add(new MainHand(Rarity.Common, "Spear", 2));
        cards.Add(new MainHand(Rarity.Common, "Trident", 2));
        return cards;
    }

    private List<Card> OffHandCreation() {
        List<Card> cards = new List<Card>();
        cards.Add(new OffHand(Rarity.Starter, "Wooden Shield"));
        cards.Add(new OffHand(Rarity.Common, "Buckler"));
        cards.Add(new OffHand(Rarity.Common, "Tome"));
        cards.Add(new OffHand(Rarity.Common, "Spell Focus"));
        cards.Add(new OffHand(Rarity.Common, "Tower Shield"));
        return cards;
    }

    private List<Card> AllyCreation() {
        List<Card> cards = new List<Card>();
        cards.Add(new Ally(Rarity.Common, "Squirrel"));
        cards.Add(new Ally(Rarity.Common, "Frog"));
        cards.Add(new Ally(Rarity.Common, "Rat"));
        cards.Add(new Ally(Rarity.Common, "Bunny"));
        cards.Add(new Ally(Rarity.Rare, "Newt"));
        cards.Add(new Ally(Rarity.Rare, "Porcupine"));
        cards.Add(new Ally(Rarity.Rare, "Hampster"));
        return cards;
    }

    private List<Card> SpiritCreation() {
        List<Card> cards = new List<Card>();
        cards.Add(new Spirit(Rarity.Common, "Air Spirit"));
        cards.Add(new Spirit(Rarity.Common, "Earth Spirit"));
        cards.Add(new Spirit(Rarity.Common, "Fire Spirit"));
        cards.Add(new Spirit(Rarity.Common, "Water Spirit"));
        return cards;
    }

    private List<Card> SpellCreation() {
        List<Card> cards = new List<Card>();
        cards.Add(new Spell(Rarity.Starter, "Arcane Bolt", 2, SpellTargetType.Single));
        cards.Add(new Spell(Rarity.Common, "Fireball", 3, SpellTargetType.Adjacent));
        cards.Add(new Spell(Rarity.Common, "Life Drain", 2, SpellTargetType.Single));
        cards.Add(new Spell(Rarity.Rare, "Lightning Bolt", 3, SpellTargetType.Adjacent));
        cards.Add(new Spell(Rarity.Rare, "Heal", 2, SpellTargetType.Self));
        cards.Add(new Spell(Rarity.Rare, "Blizzard", 3, SpellTargetType.All));
        return cards;
    }

    private List<Card> DrinkCreation() {
        List<Card> cards = new List<Card>();
        cards.Add(new Drink(Rarity.Starter, "Cup"));
        cards.Add(new Drink(Rarity.Common, "Pouch"));
        cards.Add(new Drink(Rarity.Common, "Tankard"));
        cards.Add(new Drink(Rarity.Common, "Flask"));
        cards.Add(new Drink(Rarity.Rare, "Flagon"));
        cards.Add(new Drink(Rarity.Rare, "Goblet"));
        cards.Add(new Drink(Rarity.Rare, "Chalice"));
        return cards;
    }
    #endregion Creation Methods

    private Rarity GetRandomRarity() {
        float currentRarityPercSum = 0f;
        // Get a random float
        float randomF = Random.Range(0f, 1f);

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

    private List<Card> GetCardList(Slot slotType) {
        switch(slotType) {
            case Slot.MainHand:
                return mainHandCards;
            case Slot.OffHand:
                return offHandCards;
            case Slot.Ally:
                return allyCards;
            case Slot.Spirit:
                return spiritCards;
            case Slot.Spell:
                return spellCards;
            case Slot.Drink:
                return drinkCards;
            default:
                break;
        }

        return new List<Card>();
    }

    public Card GetRandomCardData(Slot slotType) {
        List<Card> cardList = GetCardList(slotType);
        Rarity randomRarity = GetRandomRarity();

        List<Card> filterList = cardList.Where(card => card.Rarity == randomRarity).ToList();
        int randomIndex = Random.Range(0, filterList.Count);
        return filterList[randomIndex];
    }

    public Card GetStarterCardData(Slot slotType) {
        List<Card> cardList = GetCardList(slotType);
        List<Card> filteredList = cardList.Where(card => card.Rarity == Rarity.Starter).ToList();
        if(filteredList.Count > 0) { 
            return filteredList[0];
        } else {
            return null;
        }
    }

    public void Reset() {
        // Setup starter slots
        mainHand = GetStarterCardData(Slot.MainHand) as MainHand;
        offHand = GetStarterCardData(Slot.OffHand) as OffHand;
        ally = GetStarterCardData(Slot.Ally) as Ally;
        spirit = GetStarterCardData(Slot.Spirit) as Spirit;
        spell = GetStarterCardData(Slot.Spell) as Spell;
        drink = GetStarterCardData(Slot.Drink) as Drink;
    }
}
