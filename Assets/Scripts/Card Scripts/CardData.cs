public enum Rarity
{
    Starter,
    Common,
    Rare
}

public enum TargetType
{
    Self,   // Targets the player
    AOE,    // Targets all enemies
    Unit    // Targets a specific enemy
}

public abstract class CardData
{
    protected Rarity rarity;
    protected string name;
    protected Slot slot;
    protected string description;
    protected TargetType targetType;

    public Rarity Rarity { get { return rarity; } }
    public string Name { get { return name; } }
    public Slot Slot { get { return slot; } }
    public string Description { get { return description; } }
    public TargetType TargetType { get { return targetType; } }

    public CardData(Rarity rarity, string name, string description, TargetType targetType) {
        this.rarity = rarity;
        this.name = name;
        this.description = description;
        this.targetType = targetType;
    }

    public CardData(Rarity rarity, string name, TargetType targetType)
        : this(rarity, name, "", targetType) { }
}
