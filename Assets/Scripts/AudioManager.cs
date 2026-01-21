using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance = null;

    // Set in inspector
    [SerializeField]
    private Transform audioParentTrans;
    [SerializeField]
    private List<GameObject> damageTakenAudioPrefabs;
    [SerializeField]
    private List<GameObject> damageBlockedAudioPrefabs;
    [SerializeField]
    private GameObject healAudioPrefab;
    [SerializeField]
    private List<GameObject> attackAudioPrefabs;
    [SerializeField]
    private GameObject allyAudioPrefab;
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
    [SerializeField]
    private GameObject drinkAudioPrefab;

    public delegate void OnAudioDelegate();
    public static OnAudioDelegate onAttackAudioDelegate;
    public static OnAudioDelegate onDefendAudioDelegate;
    public static OnAudioDelegate onAllyAudioDelegate;
    public static OnAudioDelegate onSpellAttackAudioDelegate;
    public static OnAudioDelegate onSpellBuffAudioDelegate;
    public static OnAudioDelegate onDrinkAudioDelegate;
    // Placeholder
    public static OnAudioDelegate onEmptyAudioDelegate = delegate { };

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

        onAttackAudioDelegate += PlayAttackAudio;
        onDefendAudioDelegate += PlayGiveDefenseAudio;
        onDrinkAudioDelegate += PlayDrinkAudio;
    }

    public void PlayAttackAudio()
    {
        int randIndex = Random.Range(0, attackAudioPrefabs.Count);
        CreateAudioObject(attackAudioPrefabs[randIndex]);
    }

    public void PlayGiveDefenseAudio()
    {
        CreateAudioObject(giveDefenseAudioPrefab);
    }

    public void PlayAllyAudio()
    {
        CreateAudioObject(allyAudioPrefab);
    }

    public void PlaySpellAttackAudio()
    {
        CreateAudioObject(spellAttackAudioPrefab);
    }

    public void PlaySpellBuffAudio()
    {
        CreateAudioObject(spellBuffAudioPrefab);
    }

    public void PlayHealAudio()
    {
        CreateAudioObject(healAudioPrefab);
    }

    public void PlayDrinkAudio()
    {
        CreateAudioObject(drinkAudioPrefab);
    }

    public void PlayDamageTakenAudio()
    {
        int randIndex = Random.Range(0, damageTakenAudioPrefabs.Count);
        CreateAudioObject(damageTakenAudioPrefabs[randIndex]);
    }

    public void PlayDamageBlockedAudio()
    {
        int randIndex = Random.Range(0, damageBlockedAudioPrefabs.Count);
        CreateAudioObject(damageBlockedAudioPrefabs[randIndex]);
    }

    public void PlayBurnAudio()
    {
        CreateAudioObject(burnAudioPrefab);
    }

    public void PlayPoisonAudio()
    {
        CreateAudioObject(poisonAudioPrefab);
    }

    public void PlaySpikesAudio()
    {
        CreateAudioObject(spikesAudioPrefab);
    }

    private void CreateAudioObject(GameObject audioPrefab)
    {
        Instantiate(audioPrefab, audioParentTrans);
    }
}
