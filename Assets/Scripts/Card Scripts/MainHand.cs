using System;
using UnityEngine;

public class MainHand : Card
{
    private int damage;

    public int Damage { get { return damage; } }

    public MainHand(Rarity rarity, string name, TargetType targetType, int damage) 
        : base(rarity, name, targetType) {
        slot = Slot.MainHand;
        this.damage = damage;
    }

    public MainHand(Rarity rarity, string name, int damage) 
        : this(rarity, name, TargetType.Unit, damage) { }

    public override void Play(GameObject target) {
        
    }
}