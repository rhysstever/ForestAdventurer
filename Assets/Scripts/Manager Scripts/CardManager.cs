using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
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

    // Set in inspector
    [SerializeField]    // Card base sprites
    private Sprite cardBase, cardBaseUncommon, cardBaseRare, cardBaseVeryRare, cardBaseLegendary;
    [SerializeField]    // Action icon sprites
    private Sprite actionIconSpriteAttack, actionIconSpriteDefend, actionIconSpriteHeal, actionIconSpriteFire, actionIconSpritePoison, actionIconSpriteSummon;

    // Set in script
    private List<CardData> cardLibrary;
    private Dictionary<Rarity, float> rarityPercentages;
    private List<Sprite> cardArtList;

    // Slots
    private CardData mainHand, offHand, spirit, ally, spell, drink;
    // TODO: Add in passive slots
    //private CardData hat;
    //private CardData boots;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        cardArtList = LoadCardArtSprites();
        cardLibrary = CardCreation();

        Reset();
    }

    void Start()
    {
        rarityPercentages = new Dictionary<Rarity, float>();
        rarityPercentages.Add(Rarity.Common, 0.75f);
        rarityPercentages.Add(Rarity.Rare, 0.25f);
    }

    #region Card Creation
    private List<Sprite> LoadCardArtSprites() {
        List<Sprite> spriteList = new List<Sprite>();

        string cardArtFilePath = "Assets/Resources/Images/Card Art/PNG";
        string[] files = Directory.GetFiles(cardArtFilePath, "*.png", SearchOption.TopDirectoryOnly);

        foreach(var file in files) {
            var sprite = AssetDatabase.LoadAssetAtPath(file, typeof(Sprite));

            if(sprite != null) {
                spriteList.Add((Sprite)sprite);
            } else {
                Debug.Log("Error! Sprite not loaded");
            }
        }

        return spriteList;
    }

    private List<CardData> CardCreation() {
        List<CardData> cards = new() {
            // Main hand cards - 8 total
            new CardData("Shortsword", Slot.MainHand, Rarity.Starter, TargetType.Unit, "Attack for 1 damage."),
            new CardData("Wand", Slot.MainHand, Rarity.Common, TargetType.None, "Randomly attack 2 times for 1 damage. And some magic... nothing yet"),
            new CardData("Longsword", Slot.MainHand, Rarity.Common, TargetType.Unit, "Attack for 2 damage."),
            new CardData("Staff", Slot.MainHand, Rarity.Common, TargetType.None, "Randomly attack 2 times for 2 damage. And some magic... nothing yet"),
            new CardData("Mace", Slot.MainHand, Rarity.Common, TargetType.AOE, "Attack all for 3 damage."),
            new CardData("Flail", Slot.MainHand, Rarity.Rare, TargetType.None, "Randomly attack 3 times for 2 damage."),
            new CardData("Spear", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 6 damage."),
            new CardData("Trident", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 5 damage. And some magic... nothing yet"),

            // Off hand cards - 5 total
            new CardData("Wooden Shield", Slot.OffHand, Rarity.Starter, TargetType.Self, "Gain 1 defense."),
            new CardData("Buckler", Slot.OffHand, Rarity.Common, TargetType.Self, "Gain 2 defense."),
            new CardData("Tome", Slot.OffHand, Rarity.Common, TargetType.Self, "Some magic... nothing yet"),
            new CardData("Arcane Focus", Slot.OffHand, Rarity.Rare, TargetType.Self, "Some magic... nothing yet"),
            new CardData("Tower Shield", Slot.OffHand, Rarity.Rare, TargetType.Self, "Gain 5 defense."),

            // Ally cards - 7 total
            new CardData("Squirrel", Slot.Ally, Rarity.Common, TargetType.Unit, "Attack for 1 damage."),
            new CardData("Frog", Slot.Ally, Rarity.Common, TargetType.Self, "Heal for 1."),
            new CardData("Rat", Slot.Ally, Rarity.Common, TargetType.None, "Randomly attack 3 times for 1 damage."),
            new CardData("Bunny", Slot.Ally, Rarity.Common, TargetType.Self, "Heal for 2."),
            new CardData("Newt", Slot.Ally, Rarity.Rare, TargetType.AOE, "Attack all for 2 damage."),
            new CardData("Porcupine", Slot.Ally, Rarity.Rare, TargetType.Self, "Gain 3 defense."),
            new CardData("Hamster", Slot.Ally, Rarity.Rare, TargetType.Unit, "Attack for 3 damage."),

            // Spirit cards - 4 total
            new CardData("Air Spirit", Slot.Spirit, Rarity.Rare, TargetType.None, "Randomly attack 3 times for 1 damage."),
            new CardData("Earth Spirit", Slot.Spirit, Rarity.Common, TargetType.Self, "Gain 3 defense."),
            new CardData("Fire Spirit", Slot.Spirit, Rarity.Common, TargetType.Unit, "Attack for 3 damage."),
            new CardData("Water Spirit", Slot.Spirit, Rarity.Common, TargetType.Self, "Heal for 3."),

            // Spell cards - 6 total
            new CardData("Arcane Bolt", Slot.Spell, Rarity.Starter, TargetType.None, "Randomly attack for 1 damage."),
            new CardData("Fireball", Slot.Spell, Rarity.Common, TargetType.AOE, "Attack all for 1 damage."),
            new CardData("Life Drain", Slot.Spell, Rarity.Common, TargetType.Unit, "Attack for 2 damage. Heal for 1"),
            new CardData("Lightning Bolt", Slot.Spell, Rarity.Rare, TargetType.None, "Randomly attack for 4 damage."),
            new CardData("Heal", Slot.Spell, Rarity.Rare, TargetType.Self, "Heal for 5."),
            new CardData("Blizzard", Slot.Spell, Rarity.Rare, TargetType.AOE, "Attack all for 3 damage."),

            // Drink cards - 7 total
            new CardData("Cup", Slot.Drink, Rarity.Starter, TargetType.Self, "Heal for 1."),
            new CardData("Pouch", Slot.Drink, Rarity.Common, TargetType.Self, "Heal for 2."),
            new CardData("Tankard", Slot.Drink, Rarity.Common, TargetType.Unit, "Attack for 1 damage. Heal for 1"),
            new CardData("Flask", Slot.Drink, Rarity.Common, TargetType.None, "Randomly attack for 2 damage."),
            new CardData("Flagon", Slot.Drink, Rarity.Rare, TargetType.None, "Randomly attack for 2 damage. Heal for 3"),
            new CardData("Goblet", Slot.Drink, Rarity.Rare, TargetType.Self, "Heal for 4."),
            new CardData("Chalice", Slot.Drink, Rarity.Rare, TargetType.Unit, "Attack for 1 damage. Heal for 3")
        };

        // 37 cards total
        return cards;
    }

    public Sprite GetCardArtSprite(string cardName) {
        string formattedName = cardName.Replace(" ", "");

        for(int i = 0; i < cardArtList.Count; i++) {
            if(cardArtList[i].name == formattedName + "CardArt") {
                return cardArtList[i];
            }
        }

        Debug.LogFormat("Error! No art found for {0}", formattedName);
        return null;
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

    public CardData GetRandomCardData() {
        Slot randomSlot = (Slot)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Slot)).Length);
        return GetRandomCardData(randomSlot);
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

    public Sprite GetCardBaseSprite(Rarity rarity) {
        return rarity switch {
            Rarity.Starter => cardBase,
            Rarity.Common => cardBase,
            Rarity.Rare => cardBaseRare,
            Rarity.VeryRare => cardBaseVeryRare,
            Rarity.Legendary => cardBaseLegendary,
            _ => cardBase,
        };
    }

    public Sprite GetActionSprite(string actionType) {
        switch(actionType) {
            case "Attack":
                return actionIconSpriteAttack;
            case "Defend":
                return actionIconSpriteDefend;
            case "Heal":
                return actionIconSpriteHeal;
            case "Fire":
                return actionIconSpriteFire;
            case "Poison":
                return actionIconSpritePoison;
            case "Summon":
                return actionIconSpriteSummon;
            default:
                Debug.Log(string.Format("Error! No action sprite for action type {0}!", actionType));
                return null;
        }
    }

    public void UpdateSlot(CardData newCardData) {
        switch(newCardData.Slot) {
            case Slot.MainHand:
                mainHand = newCardData;
                break;
            case Slot.OffHand:
                offHand = newCardData;
                break;
            case Slot.Ally:
                ally = newCardData;
                break;
            case Slot.Spirit:
                spirit = newCardData;
                break;
            case Slot.Spell:
                spell = newCardData;
                break;
            case Slot.Drink:
                drink = newCardData;
                break;
        }
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
                AttackRandomEnemy(1);
                break;
            case "Earth Spirit":
                Defend(3);
                break;
            case "Fire Spirit":
                AttackUnit(3, target.GetComponent<Enemy>());
                break;
            case "Water Spirit":
                Heal(3);
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

            #region Drink Card Actions
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
            #endregion Drink Card Actions
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
        EnemyManager.instance.GetCurrentEnemiesInScene().ForEach(enemy => {
            AttackUnit(damage, enemy);
        });
    }

    private void AttackRandomEnemy(int damage) {
        List<Enemy> currentEnemies = EnemyManager.instance.GetCurrentEnemiesInScene();
        int randomEnemyIndex = UnityEngine.Random.Range(0, currentEnemies.Count);
        AttackUnit(damage, currentEnemies[randomEnemyIndex]);
    }

    private void Defend(int amount) {
        GameManager.instance.Player.GiveDefense(amount);
    }

    private void Heal(int amount) {
        GameManager.instance.Player.Heal(amount);
    }
    #endregion Effects
}