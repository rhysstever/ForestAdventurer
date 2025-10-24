using UnityEngine;

public class OffHand : Card
{
    public OffHand(Rarity rarity, string name, TargetType targetType)
        : base(rarity, name, targetType) {
        slot = Slot.OffHand;
    }

    public OffHand(Rarity rarity, string name) 
        : this(rarity, name, TargetType.Self) { }

    public override void Play(GameObject target) {

    }
}
