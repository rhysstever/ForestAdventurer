using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance = null;

    [SerializeField]
    private Transform audioParentTrans;
    [SerializeField]
    private GameObject attackAudioPrefab, damageTakenAudioPrefab;

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

    public void PlayAttackAudio()
    {
        Instantiate(attackAudioPrefab, audioParentTrans);
    }

    public void PlayDamageTakenAudio()
    {
        Instantiate(damageTakenAudioPrefab, audioParentTrans);
    }
}
