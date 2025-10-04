public class MainHand : Card
{
    private string name;
    private int damage;

    public string Name {  get { return name; } }
    public int Damage { get { return damage; } }

    public MainHand(Rarity rarity, string name, int damage) : base(rarity) {
        this.name = name;
        this.damage = damage;
    }
}
