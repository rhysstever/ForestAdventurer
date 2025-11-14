using UnityEngine;

public class Player : Unit
{
    protected int currentGold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount, null, true);
    }

    public override void TakeDamage(int amount, Unit attacker, bool isDamageBlockable) {
        base.TakeDamage(amount, attacker, isDamageBlockable);

        // Check if the player has been killed, if so, end the game
        if(currentLife <= 0) {
            currentLife = 0;
            UpdateLifeUIText();
            GameManager.instance.ChangeMenuState(MenuState.GameEnd);
        }
    }

    public bool CanAfford(int amount) {
        return currentGold >= amount;
    }

    public void SpendGold(int amount) {
        if(amount < 0 || !CanAfford(amount)) {
            return;
        }

        currentGold -= amount;
    }

    public void GiveGold(int amount) {
        if(amount < 0) {
            return;
        }

        currentGold += amount;
    }

    public override void Reset() {
        maxLife = 10;
        currentGold = 0;
        base.Reset();
    }
}
