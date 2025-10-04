public class Drink : Card
{
    private string name;

    public string Name { get { return name; } }

    public Drink(Rarity rarity, string name) : base(rarity) {
        this.name = name;
    }
}
