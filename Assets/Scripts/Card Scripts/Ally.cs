public class Ally : Card
{
    private string name;

    public string Name { get { return name; } }

    public Ally(Rarity rarity, string name) : base(rarity) {
        this.name = name;
    }
}
