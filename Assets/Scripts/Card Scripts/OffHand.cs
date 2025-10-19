public class OffHand : Card
{
    public OffHand(Rarity rarity, string name) : base(rarity, name) {
        slot = Slot.OffHand;
    }
}
