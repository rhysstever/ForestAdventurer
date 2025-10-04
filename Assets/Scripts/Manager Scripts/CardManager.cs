using NUnit.Framework;
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
    public static CardManager instance = null;

    private List<Card> mainHands, offHands, allies, spirits, spells, drinks;

    private Dictionary<Rarity, float> rarityPercentages;

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
        MainHandCreation();
        OffHandCreation();
        AllyCreation();
        SpiritCreation();
        SpellCreation();
        DrinkCreation();

        rarityPercentages = new Dictionary<Rarity, float>();
        rarityPercentages.Add(Rarity.Common, 0.75f);
        rarityPercentages.Add(Rarity.Rare, 0.25f);
    }

    #region Creation Methods
    private void MainHandCreation() {
        mainHands = new List<Card>();
        mainHands.Add(new MainHand(Rarity.Starter, "Shortsword", 1));
        mainHands.Add(new MainHand(Rarity.Common, "Wand", 1));
        mainHands.Add(new MainHand(Rarity.Common, "Longsword", 2));
        mainHands.Add(new MainHand(Rarity.Common, "Staff", 2));
    }

    private void OffHandCreation() {
        offHands = new List<Card>();
        offHands.Add(new OffHand(Rarity.Starter, "Wooden Shield"));
        offHands.Add(new OffHand(Rarity.Common, "Buckler"));
        offHands.Add(new OffHand(Rarity.Common, "Tome"));
        offHands.Add(new OffHand(Rarity.Common, "Buckler"));
    }

    private void AllyCreation() {
        allies = new List<Card>();
        allies.Add(new Ally(Rarity.Common, "Squirrel"));
        allies.Add(new Ally(Rarity.Common, "Frog"));
        allies.Add(new Ally(Rarity.Common, "Rat"));
        allies.Add(new Ally(Rarity.Common, "Bunny"));
        allies.Add(new Ally(Rarity.Rare, "Newt"));
        allies.Add(new Ally(Rarity.Rare, "Porcupine"));
        allies.Add(new Ally(Rarity.Rare, "Hampster"));
    }

    private void SpiritCreation() {
        spirits = new List<Card>();
        spirits.Add(new Spirit(Rarity.Starter, "Wisp"));
        spirits.Add(new Spirit(Rarity.Common, "Buckler"));
        spirits.Add(new Spirit(Rarity.Common, "Tome"));
        spirits.Add(new Spirit(Rarity.Common, "Buckler"));
    }

    private void SpellCreation() {
        spells = new List<Card>();
        spells.Add(new Spell(Rarity.Starter, "Magic Bolt", 2));
        spells.Add(new Spell(Rarity.Common, "Fireball", 3));
        spells.Add(new Spell(Rarity.Common, "Stone Gaze", 4));
        spells.Add(new Spell(Rarity.Rare, "Lightning Bolt", 3));
        spells.Add(new Spell(Rarity.Rare, "Heal", 3));
    }

    private void DrinkCreation() {
        drinks = new List<Card>();
        drinks.Add(new Drink(Rarity.Starter, "Cup"));
        drinks.Add(new Drink(Rarity.Common, "Pouch"));
        drinks.Add(new Drink(Rarity.Common, "Tankard"));
        drinks.Add(new Drink(Rarity.Common, "Flask"));
        drinks.Add(new Drink(Rarity.Rare, "Flagon"));
        drinks.Add(new Drink(Rarity.Rare, "Goblet"));
        drinks.Add(new Drink(Rarity.Rare, "Chalice"));
    }
    #endregion Creation Methods

    public Card Test(Card card) {
        return card;
    }

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
                return mainHands;
            case Slot.OffHand:
                return offHands;
            case Slot.Ally:
                return allies;
            case Slot.Spirit:
                return spirits;
            case Slot.Spell:
                return spells;
            case Slot.Drink:
                return drinks;
            default:
                break;
        }

        return new List<Card>();
    }

    public Card GetRandomCard(Slot slotType) {
        List<Card> cardList = GetCardList(slotType);
        Rarity randomRarity = GetRandomRarity();

        List<Card> filterList = cardList.Where(card => card.Rarity == randomRarity).ToList();
        int randomIndex = Random.Range(0, filterList.Count);
        return filterList[randomIndex];
    }

    public Card GetStarterCard(Slot slotType) {
        List<Card> cardList = GetCardList(slotType);
        List<Card> filteredList = cardList.Where(card => card.Rarity == Rarity.Starter).ToList();
        if(filteredList.Count > 0) { 
            return filteredList[0];
        } else {
            return null;
        }
    }
}
