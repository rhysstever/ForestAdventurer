public class Spirit : CardData
{
    public Spirit(Rarity rarity, string name, TargetType targetType) 
        : base(rarity, name, targetType) {
        slot = Slot.Spirit;
    }

    public Spirit(Rarity rarity, string name) : this(rarity, name, TargetType.Self) { }
}
