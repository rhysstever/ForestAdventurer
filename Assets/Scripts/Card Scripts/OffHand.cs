public class OffHand : Card
{
    private string name;

    public string Name { get { return name; } }

    public OffHand(Rarity rarity, string name) : base(rarity) {
        this.name = name;
    }
}
