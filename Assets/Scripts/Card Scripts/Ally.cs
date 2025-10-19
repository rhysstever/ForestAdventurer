public class Ally : Card
{
    public Ally(Rarity rarity, string name) : base(rarity, name) {
        slot = Slot.Ally;
    }
}
