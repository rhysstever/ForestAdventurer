public class Drink : Card
{
    public Drink(Rarity rarity, string name) : base(rarity, name) {
        slot = Slot.Drink;
    }
}
