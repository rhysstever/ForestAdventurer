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
    protected int currentLife, currentDefense;

    public int CurrentLife { get { return currentLife; } }

    public virtual void TakeDamage(int amount) {
        if(amount < 0) {
            return;
        }

        // Damage is dealt to defense before health
        if(currentDefense > 0) {
            // If the unit has defense, account for damaging the defense first
            if(amount > currentDefense) {
                // If the damage dealt is more than the unit's defense,
                // subtract the difference from the unit's health
                currentLife -= (amount - currentDefense);
                currentDefense = 0;
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

    protected void UpdateLifeUIText() {
        lifeText.text = string.Format("{0}/{1}", currentLife, maxLife);
    }

    protected void UpdateDefenseUIText() {
        defenseText.text = currentDefense.ToString();
    }

    public virtual void Reset() {
        currentLife = maxLife; 
        currentDefense = 0;
        // Update both UI text
        UpdateLifeUIText();
        UpdateDefenseUIText();
    }
}
