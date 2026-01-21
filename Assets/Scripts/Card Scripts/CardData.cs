using static AudioManager;

public enum Rarity
{
    Starter,
    Common,
    Rare,
    Legendary
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
    protected TargetType targetType;
    protected OnAudioDelegate initialPlayAudio;
    protected string description;

    public string Name { get { return name; } }
    public Slot Slot { get { return slot; } }
    public Rarity Rarity { get { return rarity; } }
    public TargetType TargetType { get { return targetType; } }
    public OnAudioDelegate InitialPlayAudio { get { return initialPlayAudio; } }
    public string Description { get { return description; } }

    public CardData(string name, Slot slot, Rarity rarity, TargetType targetType, OnAudioDelegate initialPlayAudio, string description)
    {
        this.name = name;
        this.slot = slot;
        this.rarity = rarity;
        this.targetType = targetType;
        this.initialPlayAudio = initialPlayAudio;
        this.description = description;
    }

    public CardData(string name, Slot slot, Rarity rarity, TargetType targetType, OnAudioDelegate initialPlayAudio)
        : this(name, slot, rarity, targetType, initialPlayAudio, "")
    {
        this.description = "";
    }
}
