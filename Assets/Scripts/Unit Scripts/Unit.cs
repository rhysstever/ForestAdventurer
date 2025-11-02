using UnityEngine;

public class Unit : MonoBehaviour
{
    // Instantiated in inspector
    [SerializeField]
    protected int maxLife;

    // Instantiated in code
    [SerializeField]
    protected int currentLife, currentDefense;

    public int CurrentLife { get { return currentLife; } }

    public virtual void TakeDamage(int amount) {
        if(amount < 0) {
            return;
        }

        // Damage is dealt to defense before health
        if(currentDefense > 0) {
            if(amount > currentDefense) {
                currentLife -=
                currentDefense = 0;
            } else {
                currentDefense -= amount;
            }
        } else {
            currentLife -= amount;
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

    public void GiveDefense(int amount) {
        if(amount < 0) {
            return;
        }

        currentDefense += amount;
    }

    public void ClearDefense() {
        currentDefense = 0;
    }

    public virtual void Reset() {
        currentLife = maxLife; 
        currentDefense = 0;
    }
}
