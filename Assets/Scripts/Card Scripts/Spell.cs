public class Spell : Card
{
    private string name;
    private int manaCost;

    public string Name { get { return name; } }
    public int ManaCost { get { return manaCost; } }

    public Spell(Rarity rarity, string name, int manaCost) : base(rarity) {
        this.name = name;
        this.manaCost = manaCost;
    }
}
