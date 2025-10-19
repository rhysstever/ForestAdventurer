using UnityEngine;

public class Player : Unit
{
    protected int currentGold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);

        // Check if the player has been killed, if so, end the game
        if(currentLife <= 0) {
            GameManager.instance.ChangeMenuState(MenuState.GameEnd);
        }
    }

    public void Heal(int amount) {
        if(amount < 0) {
            return;
        }

        currentLife += amount;
        if(currentLife > maxLife) {
            currentLife = maxLife;
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
