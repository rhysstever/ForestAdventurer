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
    private Sprite actionIconSpriteAttack, actionIconSpriteDefend, actionIconSpriteHeal, actionIconSpriteFire, actionIconSpritePoison, actionIconSpriteSpike, actionIconSpriteSummon;
    [SerializeField]
    private GameObject effectUIPrefab;

    // Set in script
    private List<CardData> cardLibrary;
    private Dictionary<Rarity, float> rarityPercentages;
    private List<Sprite> cardArtList;

    // Slots
    private CardData mainHand, offHand, spirit, ally, spell, drink;
    // TODO: Add in passive slots
    //private CardData hat;
    //private CardData boots;

    public GameObject EffectUIPrefab { get { return effectUIPrefab; } }

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

    private List<CardData> CardCreation() {
        List<CardData> cards = new() {
            // ===== Description Format =====
            // Normal Attack: "Attack for X"
            // Random Attack: "Attack for X, randomly"
            // AOE Attack: "Attack for X, to all"
            // Multi Attack: "Attack for X, Y times"
            // Heal: "Heal for X"
            // Gain defense: "Defend for X"
            // Burn: "Burn for X"
            // Poison: "Poison for X"
            // Gain Spikes: "Spike for X"
            // Draw Cards: "Draw X cards"
            // Cleamse Debuffs: "Cleanse"

            // Main hand cards
            new CardData("Shortsword", Slot.MainHand, Rarity.Starter, TargetType.Unit, "Attack for 1"),
            new CardData("Wand", Slot.MainHand, Rarity.Common, TargetType.None, "Some magic... nothing yet"),   // TODO: make magic
            new CardData("Staff", Slot.MainHand, Rarity.Common, TargetType.None, "Some magic... nothing yet"),   // TODO: make magic
            new CardData("Mace", Slot.MainHand, Rarity.Common, TargetType.AOE, "Attack for 3, to all"),
            new CardData("Flail", Slot.MainHand, Rarity.Rare, TargetType.None, "Attack for 2, randomly, 3 times"),
            new CardData("Flame Sword", Slot.MainHand, Rarity.Common, TargetType.Unit, "Attack for 2. Burn for 2"),
            new CardData("Spear", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 6. Poison for 2"),
            new CardData("Trident", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 4. Heal for 4"),
            new CardData("Scythe", Slot.MainHand, Rarity.Rare, TargetType.Unit, "Attack for 3. Poison for 3. Heal for 3"),

            // Off hand cards
            new CardData("Wooden Shield", Slot.OffHand, Rarity.Starter, TargetType.Self, "Defend for 1"),
            new CardData("Buckler", Slot.OffHand, Rarity.Common, TargetType.Self, "Defend for 2"),
            new CardData("Spike Shield", Slot.OffHand, Rarity.Common, TargetType.Self, "Defend for 3. Spike for 2"),
            new CardData("Tome", Slot.OffHand, Rarity.Common, TargetType.Self, "Some magic... nothing yet"),   // TODO: make magic
            new CardData("Scroll", Slot.OffHand, Rarity.Common, TargetType.Self, "Some magic... nothing yet"),   // TODO: make magic
            new CardData("Arcane Focus", Slot.OffHand, Rarity.Rare, TargetType.Self, "Some magic... nothing yet"),   // TODO: make magic
            new CardData("Tower Shield", Slot.OffHand, Rarity.Rare, TargetType.Self, "Defend for 5"),

            // Ally cards
            new CardData("Squirrel", Slot.Ally, Rarity.Starter, TargetType.Unit, "Attack for 1"),
            new CardData("Frog", Slot.Ally, Rarity.Common, TargetType.Self, "Heal for 1"),
            new CardData("Rat", Slot.Ally, Rarity.Common, TargetType.Unit, "Poison for 1"),
            new CardData("Bunny", Slot.Ally, Rarity.Rare, TargetType.Self, "Heal for 2"),   // TODO: make magic
            new CardData("Newt", Slot.Ally, Rarity.Rare, TargetType.Unit, "Burn for 2"),
            new CardData("Porcupine", Slot.Ally, Rarity.Rare, TargetType.None, "Spike for 1"),
            new CardData("Hamster", Slot.Ally, Rarity.Rare, TargetType.Unit, "Draw 1 card"),

            // Spirit cards
            new CardData("Air Spirit", Slot.Spirit, Rarity.Common, TargetType.None, "Attack for 1, randomly, 3 times"),
            new CardData("Earth Spirit", Slot.Spirit, Rarity.Common, TargetType.Self, "Defend for 3"),
            new CardData("Fire Spirit", Slot.Spirit, Rarity.Common, TargetType.Unit, "Burn for 3"),
            new CardData("Water Spirit", Slot.Spirit, Rarity.Common, TargetType.Self, "Heal for 3"),

            // Spell cards
            new CardData("Arcane Bolt", Slot.Spell, Rarity.Starter, TargetType.None, "Attack for 1, randomly"),
            new CardData("Fireball", Slot.Spell, Rarity.Common, TargetType.Unit, "Burn for 3"),
            new CardData("Life Drain", Slot.Spell, Rarity.Common, TargetType.Unit, "Attack for 2. Heal for 2"),
            new CardData("Lightning Bolt", Slot.Spell, Rarity.Rare, TargetType.None, "Attack for 4, randomly"),
            new CardData("Heal", Slot.Spell, Rarity.Rare, TargetType.Self, "Heal for 5"),
            new CardData("Blizzard", Slot.Spell, Rarity.Rare, TargetType.AOE, "Attack for 3, to all"),

            // Drink cards
            new CardData("Cup", Slot.Drink, Rarity.Starter, TargetType.Self, "Heal for 1"),
            new CardData("Pouch", Slot.Drink, Rarity.Common, TargetType.Self, "Draw 1 card"),
            new CardData("Tankard", Slot.Drink, Rarity.Common, TargetType.Unit, "Heal for 2"),
            new CardData("Flask", Slot.Drink, Rarity.Common, TargetType.None, "Heal for 1. Attack for 2, randomly"),
            new CardData("Flagon", Slot.Drink, Rarity.Rare, TargetType.Unit, "Heal for 2. Poison for 2"),
            new CardData("Goblet", Slot.Drink, Rarity.Rare, TargetType.Self, "Heal for 4"),
            new CardData("Chalice", Slot.Drink, Rarity.Rare, TargetType.Unit, "Heal for 3. Cleanse")
        };

        return cards;
    }
    #endregion Card Creation

    #region Card Actions
    public void Play(CardData cardData) {
        Play(cardData, null);
    }

    public void Play(CardData cardData, Enemy targetEnemy) {
        string description = cardData.Description;

        description.Split(". ").ToList().ForEach(action =>
            PerformCardAction(action, targetEnemy)
        );
    }

    private void PerformCardAction(string action, Enemy target) {
        string firstWord = action.Split(" ")[0];
        int amount;

        switch(firstWord) {
            case "Attack":
                string[] attackParts = action.Split(", ");

                // Figure out if the attack is AOE, random, and/or multi
                bool isAOE = false;
                bool isRandom = false;
                int isMulti = 1;
                for(int i = 1; i < attackParts.Length; i++) {
                    if(attackParts[i].Contains("to all")) {
                        isAOE = true;
                    } else if(attackParts[i].Contains("randomly")) {
                        isRandom = true;
                    } else if(attackParts[i].Contains("times")) {
                        isMulti = int.Parse(attackParts[i].Split(" ")[0]);
                    }
                }

                // Attack based on the parsed info
                for(int i = 0; i < isMulti; i++) {
                    amount = int.Parse(attackParts[0].Split(" ")[2]);
                    if(isAOE) {
                        AttackEveryEnemy(amount);
                    } else if(isRandom) {
                        AttackRandomEnemy(amount);
                    } else {
                        AttackUnit(amount, target);
                    }
                }
                break;
            case "Defend":
                amount = int.Parse(action.Split(" ")[2]);
                GameManager.instance.Player.GiveDefense(amount);
                break;
            case "Heal":
                amount = int.Parse(action.Split(" ")[2]);
                GameManager.instance.Player.Heal(amount);
                break;
            case "Burn":
                amount = int.Parse(action.Split(" ")[2]);
                target.GiveBurn(amount);
                break;
            case "Poison":
                amount = int.Parse(action.Split(" ")[2]);
                target.GivePoison(amount);
                break;
            case "Spike":
                amount = int.Parse(action.Split(" ")[2]);
                GameManager.instance.Player.GiveSpike(amount);
                break;
            case "Draw":
                // TODO : Implement Draw action
                break;
            case "Cleanse":
                GameManager.instance.Player.Cleanse();
                break;
            default:
                Debug.Log(string.Format("Error! No action found for: {0}", action));
                break;
        }
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

    private void AttackUnit(int amount, Enemy enemy) {
        if(enemy == null) {
            Debug.Log("Error: No target to attack!");
            return;
        }

        if(amount < 1) {
            Debug.Log(string.Format("Error: Not enough damage ({0})", amount));
            return;
        }

        enemy.TakeDamage(amount, GameManager.instance.Player, true);
    }
    #endregion Card Actions
    
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
        return actionType.ToLower() switch {
            "attack" => actionIconSpriteAttack,
            "defend" => actionIconSpriteDefend,
            "heal" => actionIconSpriteHeal,
            "burn" => actionIconSpriteFire,
            "poison" => actionIconSpritePoison,
            "spike" => actionIconSpriteSpike,
            "summon" => actionIconSpriteSummon,
            _ => null
        };
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
}