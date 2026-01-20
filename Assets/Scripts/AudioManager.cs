using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance = null;

    [SerializeField]
    private Transform audioParentTrans;
    [SerializeField]
    private GameObject damageTakenAudioPrefab, damageBlockedAudioPrefab, healAudioPrefab,
        attackAudioPrefab, spellAttackAudioPrefab, spellBuffAudioPrefab,
        giveDefenseAudioPrefab, burnAudioPrefab, poisonAudioPrefab, spikesAudioPrefab;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
