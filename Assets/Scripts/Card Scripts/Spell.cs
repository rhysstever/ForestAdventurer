
public enum SpellTargetType
{
    Single,
    Self,
    Adjacent,
    All
}
public class Spell : Card
{
    private int manaCost;
    private SpellTargetType targetType;

    public int ManaCost { get { return manaCost; } }
    public SpellTargetType TargetType { get { return targetType; } }

    public Spell(Rarity rarity, string name, int manaCost, SpellTargetType targetType) : base(rarity, name) {
        this.manaCost = manaCost;
        this.targetType = targetType;
    }
}
