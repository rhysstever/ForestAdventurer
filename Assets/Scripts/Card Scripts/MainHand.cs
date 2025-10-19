using UnityEngine;

public class MainHand : Card
{
    private int damage;

    public int Damage { get { return damage; } }

    public MainHand(Rarity rarity, string name, int damage) : base(rarity, name) {
        this.damage = damage;
    }

    public override void Play(GameObject target) {
        base.Play(target);

        if(target.GetComponent<Enemy>() != null) {
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}