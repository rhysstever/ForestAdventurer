public class OffHand : CardData
{
    public OffHand(Rarity rarity, string name, TargetType targetType)
        : base(rarity, name, targetType) {
        slot = Slot.OffHand;
    }

    public OffHand(Rarity rarity, string name) 
        : this(rarity, name, TargetType.Self) { }
}
