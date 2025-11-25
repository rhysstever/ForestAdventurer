using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    // Instantiated in inspector
    [SerializeField]
    private TMP_Text lifeText, defenseText;
    [SerializeField]
    private GameObject defenseParent, effectsParent;
    [SerializeField]
    private Transform effectTrans1, effectTrans2;
    [SerializeField]
    protected int maxLife;

    // Instantiated in code
    [SerializeField]
    protected int currentLife, currentDefense, currentBurn, currentPoison, currentSpike;
    private float effectOffset;

    public int CurrentLife { get { return currentLife; } }

    protected virtual void Start() {
        effectOffset = effectTrans2.localPosition.x - effectTrans1.localPosition.x;
    }
    
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
        // If there is an attack and the unit has spikes, reflect spike damage to the attacker
        if(attacker != null && currentSpike > 0) {
            attacker.TakeDamage(currentSpike);
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
        UpdateEffectsUI();
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
        UpdateEffectsUI();
    }

    public void GivePoison(int amount) {
        if(amount < 0) {
            return;
        }
        currentPoison += amount;
        UpdateEffectsUI();
    }

    public void GiveSpike(int amount) {
        if(amount < 0) {
            return;
        }
        currentSpike += amount;
        UpdateEffectsUI();
    }

    public void Cleanse() {
        currentPoison = 0;
        currentBurn = 0;
        UpdateEffectsUI();
    }

    public void ProcessEffects() {
        if(currentBurn > 0) {
            TakeDamage(currentBurn, null, true);
            currentBurn--;
            UpdateEffectsUI();
        }

        if(currentPoison > 0) {
            TakeDamage(currentPoison, null, false);
        }
    }

    protected void UpdateLifeUIText() {
        lifeText.text = string.Format("{0}/{1}", currentLife, maxLife);
    }

    protected void UpdateDefenseUIText() {
        if(currentDefense > 0)
        {
            defenseParent.SetActive(true);
            defenseText.text = currentDefense.ToString();
        } 
        else
        {
            defenseParent.SetActive(false);
        }
    }

    protected void RemoveEffectsUI() {
        for(int i = effectsParent.transform.childCount - 1; i >= 2; i--) {
            Destroy(effectsParent.transform.GetChild(i).gameObject);
        }
    }

    protected void UpdateEffectsUI() {
        RemoveEffectsUI();
        int effectsCount = 0;

        // Create Effect UI elements
        if(currentBurn > 0) {
            CreateNewEffectUIObject(currentBurn, "Burn", effectsCount);
            effectsCount++;
        }
        if(currentPoison > 0) {
            CreateNewEffectUIObject(currentPoison, "Poison", effectsCount);
            effectsCount++;
        }
        if(currentSpike > 0) {
            CreateNewEffectUIObject(currentSpike, "Spike", effectsCount);
            effectsCount++;
        }
    }

    private void CreateNewEffectUIObject(int amount, string effectName, int currentEffectsCount) {
        Vector2 position = effectTrans1.localPosition;
        position.x += effectOffset * currentEffectsCount;
        GameObject effectObject = Instantiate(CardManager.instance.EffectUIPrefab, effectsParent.transform);
        effectObject.transform.localPosition = position;
        effectObject.GetComponent<EffectUIObject>().UpdateEffectUIObject(amount, CardManager.instance.GetActionSprite(effectName));
    }

    public void PostCombatReset() {
        currentDefense = 0;
        currentBurn = 0;
        currentPoison = 0;
        currentSpike = 0;
        RemoveEffectsUI();
        UpdateDefenseUIText();
    }

    public virtual void Reset() {
        currentLife = maxLife;
        PostCombatReset();
        UpdateLifeUIText();
    }
}
