public class Spirit : Card
{
    private string name;

    public string Name { get { return name; } }

    public Spirit(Rarity rarity, string name) : base(rarity) {
        this.name = name;
    }
}
