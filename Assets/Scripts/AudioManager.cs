using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance = null;

    // Set in inspector
    [SerializeField]
    private Transform audioParentTrans;
    [SerializeField]
    private GameObject damageTakenAudioPrefab;
    [SerializeField]
    private GameObject damageBlockedAudioPrefab;
    [SerializeField]
    private GameObject healAudioPrefab;
    [SerializeField]
    private GameObject attackAudioPrefab;
    [SerializeField]
    private GameObject spellAttackAudioPrefab;
    [SerializeField]
    private GameObject spellBuffAudioPrefab;
    [SerializeField]
    private GameObject giveDefenseAudioPrefab;
    [SerializeField]
    private GameObject burnAudioPrefab;
    [SerializeField]
    private GameObject poisonAudioPrefab;
    [SerializeField]
    private GameObject spikesAudioPrefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayDamageTakenAudio()
    {
        //Instantiate(damageTakenAudioPrefab, audioParentTrans);
    }

    public void PlayDamageBlockedAudio()
    {
        //Instantiate(damageBlockedAudioPrefab, audioParentTrans);
    }

    public void PlayHealAudio()
    {
        //Instantiate(healAudioPrefab, audioParentTrans);
    }

    public void PlayAttackAudio()
    {
        //Instantiate(attackAudioPrefab, audioParentTrans);
    }

    public void PlaySpellAttackAudio()
    {
        //Instantiate(spellAttackAudioPrefab, audioParentTrans);
    }

    public void PlaySpellBuffAudio()
    {
        //Instantiate(spellBuffAudioPrefab, audioParentTrans);
    }

    public void PlayGiveDefenseAudio()
    {
        //Instantiate(giveDefenseAudioPrefab, audioParentTrans);
    }

    public void PlayBurnAudio()
    {
        //Instantiate(burnAudioPrefab, audioParentTrans);
    }

    public void PlayPoisonAudio()
    {
        //Instantiate(poisonAudioPrefab, audioParentTrans);
    }

    public void PlaySpikesAudio()
    {
        //Instantiate(spikesAudioPrefab, audioParentTrans);
    }
}
