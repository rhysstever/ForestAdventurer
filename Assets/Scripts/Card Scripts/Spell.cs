public class Spell : CardData
{
    private int manaCost;
    private int damage;

    public int ManaCost { get { return manaCost; } }
    public int Damage { get { return damage; } }

    public Spell(Rarity rarity, string name, TargetType targetType, int manaCost, int damage) 
        : base(rarity, name, targetType) {
        slot = Slot.Spell;
        this.manaCost = manaCost;
        this.damage = damage;
    }
}
