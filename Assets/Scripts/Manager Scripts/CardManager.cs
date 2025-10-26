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
        // TODO: Add the other card's descriptions

        // Main hand cards
        cards.Add(new CardData("Shortsword", Slot.MainHand, Rarity.Starter, TargetType.Unit, "Attack for 1 damage."));
        cards.Add(new CardData("Wand", Slot.MainHand, Rarity.Common, TargetType.None, "Randomly attack 2 times for 1 damage."));
        cards.Add(new CardData("Longsword", Slot.MainHand, Rarity.Common, TargetType.Unit, "Attack for 2 damage."));
        cards.Add(new CardData("Staff", Slot.MainHand, Rarity.Common, TargetType.None, "Randomly attack 2 times for 2 damage."));
        cards.Add(new CardData("Mace", Slot.MainHand, Rarity.Common, TargetType.AOE, "Attack all for 3 damage."));
        cards.Add(new CardData("Flail", Slot.MainHand, Rarity.Rare, TargetType.None, "Randomly attack 3 times for 2 damage."));
        cards.Add(new CardData("Spear", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 6 damage."));
        cards.Add(new CardData("Trident", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 5 damage. And some magic... nothing yet"));

        // Off hand cards
        cards.Add(new CardData("Wooden Shield", Slot.OffHand, Rarity.Starter, TargetType.Self, "Gain 1 defense."));
        cards.Add(new CardData("Buckler", Slot.OffHand, Rarity.Common, TargetType.Self, "Gain 2 defense."));
        cards.Add(new CardData("Tome", Slot.OffHand, Rarity.Common, TargetType.Self, "Some magic... nothing yet"));
        cards.Add(new CardData("Spell Focus", Slot.OffHand, Rarity.Rare, TargetType.Self, "Some magic... nothing yet"));
        cards.Add(new CardData("Tower Shield", Slot.OffHand, Rarity.Rare, TargetType.Self, "Gain 5 defense."));

        // Ally cards
        cards.Add(new CardData("Squirrel", Slot.Ally, Rarity.Common, TargetType.Unit, "Attack for 1 damage."));
        cards.Add(new CardData("Frog", Slot.Ally, Rarity.Common, TargetType.Self, "Heal for 1."));
        cards.Add(new CardData("Rat", Slot.Ally, Rarity.Common, TargetType.None, "Randomly attack 3 times for 1 damage."));
        cards.Add(new CardData("Bunny", Slot.Ally, Rarity.Common, TargetType.Self, "Heal for 2."));
        cards.Add(new CardData("Newt", Slot.Ally, Rarity.Rare, TargetType.AOE, "Attack all for 2 damage."));
        cards.Add(new CardData("Porcupine", Slot.Ally, Rarity.Rare, TargetType.Self, "Gain 3 defense."));
        cards.Add(new CardData("Hampster", Slot.Ally, Rarity.Rare, TargetType.Unit, "Attack for 3 damage."));

        // Spirit cards
        cards.Add(new CardData("Air Spirit", Slot.Spirit, Rarity.Common, TargetType.None, "Randomly attack 2 times for 1 damage."));
        cards.Add(new CardData("Earth Spirit", Slot.Spirit, Rarity.Common, TargetType.Self, "Gain 2 defense."));
        cards.Add(new CardData("Fire Spirit", Slot.Spirit, Rarity.Common, TargetType.Unit, "Attack for 2 damage."));
        cards.Add(new CardData("Water Spirit", Slot.Spirit, Rarity.Common, TargetType.Self, "Heal for 2."));

        // Spell cards
        cards.Add(new CardData("Arcane Bolt", Slot.Spell, Rarity.Starter, TargetType.None, "Randomly attack for 1 damage."));
        cards.Add(new CardData("Fireball", Slot.Spell, Rarity.Common, TargetType.AOE, "Attack all for 1 damage."));
        cards.Add(new CardData("Life Drain", Slot.Spell, Rarity.Common, TargetType.Unit, "Attack for 2 damage. Heal for 1"));
        cards.Add(new CardData("Lightning Bolt", Slot.Spell, Rarity.Rare, TargetType.None, "Randomly attack for 4 damage."));
        cards.Add(new CardData("Heal", Slot.Spell, Rarity.Rare, TargetType.Self, "Heal for 5."));
        cards.Add(new CardData("Blizzard", Slot.Spell, Rarity.Rare, TargetType.AOE, "Attack all for 3 damage."));

        // Drink cards
        cards.Add(new CardData("Cup", Slot.Drink, Rarity.Starter, TargetType.Self, "Heal for 1."));
        cards.Add(new CardData("Pouch", Slot.Drink, Rarity.Common, TargetType.Self, "Heal for 2."));
        cards.Add(new CardData("Tankard", Slot.Drink, Rarity.Common, TargetType.Unit, "Attack for 1 damage. Heal for 1"));
        cards.Add(new CardData("Flask", Slot.Drink, Rarity.Common, TargetType.None, "Randomly attack for 2 damage."));
        cards.Add(new CardData("Flagon", Slot.Drink, Rarity.Rare, TargetType.None, "Randomly attack for 2 damage. Heal for 3"));
        cards.Add(new CardData("Goblet", Slot.Drink, Rarity.Rare, TargetType.Self, "Heal for 4."));
        cards.Add(new CardData("Chalice", Slot.Drink, Rarity.Rare, TargetType.Unit, "Attack for 1 damage. Heal for 3"));

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

    #region Card Actions
    public void Play(string cardName) {
        Play(cardName, null);
    }

    public void Play(string cardName, GameObject target) {
        switch(cardName) {
            #region Main Hand Card Actions
            case "Shortsword":
                AttackUnit(1, target.GetComponent<Enemy>());
                break;
            case "Wand":
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                // TODO: Add Magic to Wand Play()
                break;
            case "Longsword":
                AttackUnit(2, target.GetComponent<Enemy>());
                break;
            case "Staff":
                AttackRandomEnemy(2);
                AttackRandomEnemy(2);
                // TODO: Add Magic to Staff Play()
                break;
            case "Mace":
                AttackEveryEnemy(3);
                break;
            case "Flail":
                AttackRandomEnemy(2);
                AttackRandomEnemy(2);
                AttackRandomEnemy(2);
                break;
            case "Spear":
                AttackUnit(6, target.GetComponent<Enemy>());
                break;
            case "Trident":
                AttackUnit(4, target.GetComponent<Enemy>());
                // TODO: Add Magic to Trident Play()
                break;
            #endregion Main Hand Card Actions

            #region Off Hand Card Actions
            case "Wooden Shield":
                Defend(1);
                break;
            case "Buckler":
                Defend(2);
                break;
            case "Tome":
                Defend(0);
                // TODO: Add Magic to Tome Play()
                break;
            case "Spell Focus":
                Defend(0);
                // TODO: Add Magic to Spell Focus Play()
                break;
            case "Tower Shield":
                Defend(5);
                break;
            #endregion Off Hand Card Actions

            #region Ally Card Actions
            case "Squirrel":
                AttackUnit(1, target.GetComponent<Enemy>());
                break;
            case "Frog":
                Heal(1);
                break;
            case "Rat":
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                break;
            case "Bunny":
                Heal(2);
                break;
            case "Newt":
                AttackEveryEnemy(2);
                break;
            case "Porcupine":
                Defend(3);
                break;
            case "Hampster":
                AttackUnit(3, target.GetComponent<Enemy>());
                break;
            #endregion Ally Card Actions

            #region Spirit Card Actions
            case "Air Spirit":
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                break;
            case "Earth Spirit":
                Defend(2);
                break;
            case "Fire Spirit":
                AttackUnit(2, target.GetComponent<Enemy>());
                break;
            case "Water Spirit":
                Heal(2);
                break;
            #endregion Spirit Card Actions

            #region Spell Card Actions
            case "Arcane Bolt":
                AttackRandomEnemy(1);
                break;
            case "Fireball":
                AttackEveryEnemy(1);
                break;
            case "Life Drain":
                AttackUnit(2, target.GetComponent<Enemy>());
                Heal(1);
                break;
            case "Lighning Bolt":
                AttackRandomEnemy(4);
                break;
            case "Heal":
                Heal(5);
                break;
            case "Blizzard":
                AttackEveryEnemy(3);
                break;
            #endregion Spell Card Actions

            // Drink Cards
            case "Cup":
                Heal(1);
                break;
            case "Pouch":
                Heal(2);
                break;
            case "Tankard":
                AttackUnit(1, target.GetComponent<Enemy>());
                Heal(1);
                break;
            case "Flask":
                AttackRandomEnemy(2);
                break;
            case "Flagon":
                AttackRandomEnemy(2);
                Heal(3);
                break;
            case "Goblet":
                Heal(4);
                break;
            case "Chalice":
                AttackUnit(1, target.GetComponent<Enemy>());
                Heal(2);
                break;
            default:
                Debug.Log(string.Format("Error! Card not found by name: {0}", cardName));
                break;
        }
    }
    #endregion Card Actions

    #region Effects
    private void AttackUnit(int damage, Enemy enemy) {
        if(enemy == null) {
            Debug.Log("Error: No target to attack!");
            return;
        }

        if(damage < 1) {
            Debug.Log(string.Format("Error: Not enough damage ({0})", damage));
            return;
        }

        enemy.TakeDamage(damage);
    }

    private void AttackEveryEnemy(int damage) {
        GameManager.instance.GetCurrentEnemies().ForEach(enemy => {
            AttackUnit(damage, enemy);
        });
    }

    private void AttackRandomEnemy(int damage) {
        List<Enemy> currentEnemies = GameManager.instance.GetCurrentEnemies();
        int randomEnemyIndex = UnityEngine.Random.Range(0, currentEnemies.Count);
        AttackUnit(damage, currentEnemies[randomEnemyIndex]);
    }

    private void Defend(int amount) {
        DeckManager.instance.Player.GiveDefense(amount);
    }

    private void Heal(int amount) {
        DeckManager.instance.Player.Heal(amount);
    }
    #endregion Effects
}