using UnityEngine;

public class Unit : MonoBehaviour
{
    // Instantiated in inspector
    [SerializeField]
    protected int maxLife;

    // Instantiated in code
    protected int currentLife, currentDefense;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
