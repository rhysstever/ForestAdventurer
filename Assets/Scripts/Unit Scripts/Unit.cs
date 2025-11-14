using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Instantiated in inspector
    [SerializeField]
    private TMP_Text lifeText, defenseText;
    [SerializeField]
    protected int maxLife;

    // Instantiated in code
    [SerializeField]
    protected int currentLife, currentDefense, currentBurn, currentPoison, currentSpike;

    public int CurrentLife { get { return currentLife; } }
    
    public virtual void TakeDamage(int amount) {
        TakeDamage(amount, null, true);
    }

    public virtual void TakeDamage(int amount, Unit attacker, bool isDamageBlockable) {
        if(amount < 0) {
            return;
        }

        // Damage is dealt to defense before health
        if(isDamageBlockable && currentDefense > 0) {
            // If the unit has defense, account for damaging the defense first
            if(amount > currentDefense) {
                // If the damage dealt is more than the unit's defense,
                // subtract the difference from the unit's health
                currentLife -= (amount - currentDefense);
                currentDefense = 0;
                // Check if spike damage should be returned
                if(attacker != null) {
                    attacker.TakeDamage(currentSpike);
                }
            } else {
                // If the damage dealth is less than the unit's defense,
                // subtract it from the current defense
                currentDefense -= amount;
            }
            // Update defense UI text
            UpdateDefenseUIText();
        } else {
            // If the unit has no defense, subtract the damage from its health
            currentLife -= amount;
            // Check if spike damage should be returned
            if(attacker != null) {
                attacker.TakeDamage(currentSpike);
            }
        }
        // Update life UI text
        UpdateLifeUIText();
    }

    public void Heal(int amount) {
        if(amount < 0) {
            return;
        }
        currentLife += amount;
        if(currentLife > maxLife) {
            currentLife = maxLife;
        }
        // Cure any poison when healing
        currentPoison -= amount;
        if(currentPoison < 0) {
            currentPoison = 0;
        }
        // Update life UI text
        UpdateLifeUIText();
    }

    public void GiveDefense(int amount) {
        if(amount < 0) {
            return;
        }
        currentDefense += amount;
        UpdateDefenseUIText();
    }

    public void ClearDefense() {
        currentDefense = 0;
        UpdateDefenseUIText();
    }

    public void GiveBurn(int amount) {
        if(amount < 0) {
            return;
        }
        currentBurn += amount;
    }

    public void GivePoison(int amount) {
        if(amount < 0) {
            return;
        }
        currentPoison += amount;
    }

    public void GiveSpike(int amount) {
        if(amount < 0) {
            return;
        }
        currentSpike += amount;
    }

    public void Cleanse() {
        currentPoison = 0;
        currentBurn = 0;
    }

    public void ProcessEffects() {
        if(currentBurn > 0) {
            TakeDamage(currentBurn, null, true);
            currentBurn--;
        }

        if(currentPoison > 0) {
            TakeDamage(currentPoison, null, false);
        }
    }

    protected void UpdateLifeUIText() {
        lifeText.text = string.Format("{0}/{1}", currentLife, maxLife);
    }

    protected void UpdateDefenseUIText() {
        defenseText.text = currentDefense.ToString();
    }

    public virtual void Reset() {
        currentLife = maxLife; 
        currentDefense = 0;
        currentBurn = 0;
        currentPoison = 0;
        currentSpike = 0;
        // Update both UI text
        UpdateLifeUIText();
        UpdateDefenseUIText();
    }
}
