public class Drink : CardData
{
    public Drink(Rarity rarity, string name, TargetType targetType) 
        : base(rarity, name, targetType) {
        slot = Slot.Drink;
    }

    public Drink(Rarity rarity, string name) : this(rarity, name, TargetType.Self) { }
}
