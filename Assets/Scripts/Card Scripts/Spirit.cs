using UnityEngine;

public class Spirit : Card
{
    public Spirit(Rarity rarity, string name, TargetType targetType) 
        : base(rarity, name, targetType) {
        slot = Slot.Spirit;
    }

    public Spirit(Rarity rarity, string name) : this(rarity, name, TargetType.Self) { }

    public override void Play(GameObject target) {

    }
}
