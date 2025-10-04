public enum Rarity
{
    Starter,
    Common,
    Rare
}

public class Card
{
    protected Rarity rarity;

    public Rarity Rarity { get { return rarity; } }

    public Card(Rarity rarity) {
        this.rarity = rarity;
    }
}
