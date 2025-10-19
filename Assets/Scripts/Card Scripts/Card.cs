using UnityEngine;

public enum Rarity
{
    Starter,
    Common,
    Rare
}

public class Card
{
    protected Rarity rarity;
    protected string name;
    protected Slot slot;

    public Rarity Rarity { get { return rarity; } }
    public string Name { get { return name; } }
    public Slot Slot { get { return slot; } }

    public Card(Rarity rarity, string name) {
        this.rarity = rarity;
        this.name = name;
    }

    public virtual void Play(GameObject target) {

    }
}
