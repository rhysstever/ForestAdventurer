public class Ally : CardData
{
    public Ally(Rarity rarity, string name, TargetType targetType) 
        : base(rarity, name, targetType) {
        slot = Slot.Ally;
    }

    public Ally(Rarity rarity, string name) : this(rarity, name, TargetType.Self) { }
}
