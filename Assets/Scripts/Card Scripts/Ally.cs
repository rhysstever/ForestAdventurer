using UnityEngine;

public class Ally : Card
{
    public Ally(Rarity rarity, string name, TargetType targetType) 
        : base(rarity, name, targetType) {
        slot = Slot.Ally;
    }

    public Ally(Rarity rarity, string name) : this(rarity, name, TargetType.Self) { }

    public override void Play(GameObject target) {
        
    }
}
