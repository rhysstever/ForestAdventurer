public class MainHand : Card
{
    private int damage;

    public int Damage { get { return damage; } }

    public MainHand(Rarity rarity, string name, int damage) : base(rarity, name) {
        this.damage = damage;
    }
}