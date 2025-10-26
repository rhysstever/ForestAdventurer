using UnityEngine;

public enum Rarity
{
    Starter,
    Common,
    Rare
}

public enum TargetType
{
    None,   // Targets nothing
    Self,   // Targets the player
    AOE,    // Targets all enemies
    Unit    // Targets a specific enemy
}

public class CardData
{
    protected string name;
    protected Slot slot;
    protected Rarity rarity;
    protected string description;
    protected TargetType targetType;

    public string Name { get { return name; } }
    public Slot Slot { get { return slot; } }
    public Rarity Rarity { get { return rarity; } }
    public string Description { get { return description; } }
    public TargetType TargetType { get { return targetType; } }

    public CardData(string name, Slot slot, Rarity rarity, string description, TargetType targetType) {
        this.name = name;
        this.slot = slot;
        this.rarity = rarity;
        this.description = description;
        this.targetType = targetType;
    }

    public CardData(string name, Slot slot, Rarity rarity, TargetType targetType) {
        this.name = name;
        this.slot = slot;
        this.rarity = rarity;
        this.description = "";
        this.targetType = targetType;
    }
}
