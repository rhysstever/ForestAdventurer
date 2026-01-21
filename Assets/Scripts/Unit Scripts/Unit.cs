using System.Collections;
using TMPro;
using UnityEngine;

public enum DamageType
{
    Attack,
    Spell,
    Burn,
    Poison,
    Spike
}

public class Unit : MonoBehaviour
{
    // Instantiated in inspector
    [SerializeField]
    private SpriteRenderer unitSpriteRenderer;
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
    protected int currentDefense, currentBurn, currentPoison, currentSpike;
    protected int currentLife;
    private float effectOffset;

    public int CurrentLife { get { return currentLife; } }

    protected virtual void Start()
    {
        effectOffset = effectTrans2.localPosition.x - effectTrans1.localPosition.x;
    }

    public virtual void TakeDamage(int amount, DamageType damageType)
    {
        TakeDamage(amount, null, damageType);
    }

    public virtual void TakeDamage(int amount, Unit attacker, DamageType damageType)
    {
        if(amount <= 0)
        {
            return;
        }

        // Check whether the damage is blockable
        if(damageType != DamageType.Poison)
        {
            // Account for damaging the defense first, then health
            if(amount > currentDefense)
            {
                // If the damage dealt is more than the unit's defense,
                // subtract the difference from the unit's health
                currentLife -= (amount - currentDefense);
                currentDefense = 0;
                AudioManager.instance.PlayDamageTakenAudio();
            }
            else
            {
                // If the damage dealth is less than the unit's defense,
                // subtract it from the current defense
                currentDefense -= amount;
                AudioManager.instance.PlayDamageBlockedAudio();
            }

            // Update defense UI text
            UpdateDefenseUIText();
        }
        else
        {
            currentLife -= amount;
        }

        // If the damage type is an attack or spell, there is an attacker, and the unit has spikes,
        // reflect spike damage to the attacker
        if((damageType == DamageType.Attack || damageType == DamageType.Spell)
            && attacker != null && currentSpike > 0)
        {
            AudioManager.instance.PlaySpikesAudio();
            attacker.TakeDamage(currentSpike, DamageType.Spike);
        }
        // Update life UI text
        UpdateLifeUIText();
    }

    public void Heal(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        AudioManager.instance.PlayHealAudio();
        currentLife += amount;
        if(currentLife > maxLife)
        {
            currentLife = maxLife;
        }
        // Cure poison when healing
        currentPoison -= amount;
        UpdateEffectsUI();
        if(currentPoison < 0)
        {
            currentPoison = 0;
        }
        // Update life UI text
        UpdateLifeUIText();
    }

    public void GiveDefense(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        currentDefense += amount;
        UpdateDefenseUIText();
    }

    public void ClearDefense()
    {
        currentDefense = 0;
        UpdateDefenseUIText();
    }

    public void GiveBurn(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        AudioManager.instance.PlayBurnAudio();
        currentBurn += amount;
        UpdateEffectsUI();
    }

    public void GivePoison(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        AudioManager.instance.PlayPoisonAudio();
        currentPoison += amount;
        UpdateEffectsUI();
    }

    public void GiveSpike(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        AudioManager.instance.PlaySpikesAudio();
        currentSpike += amount;
        UpdateEffectsUI();
    }

    public void Cleanse()
    {
        currentPoison = 0;
        currentBurn = 0;
        UpdateEffectsUI();
    }

    public IEnumerator ProcessEffects()
    {
        WaitForSeconds betweenEffectsDelayWait = new WaitForSeconds(1);
        WaitForSeconds effectTriggerToDamageDelayWait = new WaitForSeconds(0.5f);

        if(currentBurn > 0)
        {
            AudioManager.instance.PlayBurnAudio();
            unitSpriteRenderer.color = ParticlesManager.instance.BurnColor;
            // TODO: Activate burn visual effect
            yield return effectTriggerToDamageDelayWait;
            unitSpriteRenderer.color = ParticlesManager.instance.ResetColor;
            TakeDamage(currentBurn, null, DamageType.Burn);
            currentBurn--;
            UpdateEffectsUI();
        }

        if(currentPoison > 0)
        {
            yield return betweenEffectsDelayWait;
            AudioManager.instance.PlayPoisonAudio();
            unitSpriteRenderer.color = ParticlesManager.instance.PoisonColor;
            // TODO: Activate poison visual effect
            yield return effectTriggerToDamageDelayWait;
            unitSpriteRenderer.color = ParticlesManager.instance.ResetColor;
            TakeDamage(currentPoison, null, DamageType.Poison);
            currentPoison--;
            UpdateEffectsUI();
        }
    }

    protected void UpdateLifeUIText()
    {
        lifeText.text = string.Format("{0}/{1}", currentLife, maxLife);
    }

    protected void UpdateDefenseUIText()
    {
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

    protected void RemoveEffectsUI()
    {
        for(int i = effectsParent.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(effectsParent.transform.GetChild(i).gameObject);
        }
    }

    protected void UpdateEffectsUI()
    {
        RemoveEffectsUI();
        int effectsCount = 0;

        // Create Effect UI elements
        if(currentBurn > 0)
        {
            CreateNewEffectUIObject(currentBurn, "Burn", effectsCount);
            effectsCount++;
        }
        if(currentPoison > 0)
        {
            CreateNewEffectUIObject(currentPoison, "Poison", effectsCount);
            effectsCount++;
        }
        if(currentSpike > 0)
        {
            CreateNewEffectUIObject(currentSpike, "Spike", effectsCount);
            effectsCount++;
        }
    }

    private void CreateNewEffectUIObject(int amount, string effectName, int currentEffectsCount)
    {
        Vector2 position = effectTrans1.localPosition;
        position.x += effectOffset * currentEffectsCount;
        GameObject effectObject = Instantiate(CardManager.instance.EffectUIPrefab, effectsParent.transform);
        effectObject.transform.localPosition = position;
        effectObject.GetComponent<EffectUIObject>().UpdateEffectUIObject(amount, CardManager.instance.GetActionSprite(effectName));
    }

    public void PostCombatReset()
    {
        currentDefense = 0;
        currentBurn = 0;
        currentPoison = 0;
        currentSpike = 0;
        RemoveEffectsUI();
        UpdateDefenseUIText();
    }

    public virtual void Reset()
    {
        currentLife = maxLife;
        PostCombatReset();
        UpdateLifeUIText();
    }
}
