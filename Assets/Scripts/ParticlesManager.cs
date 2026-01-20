using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    // Singleton
    public static ParticlesManager instance = null;

    // Set in inspector
    [SerializeField]
    private Transform particalesParentTrans;
    [SerializeField]
    private GameObject damageTakenParticlePrefab;
    [SerializeField]
    private GameObject damageBlockedParticlePrefab;
    [SerializeField]
    private GameObject healParticlePrefab;
    [SerializeField]
    private GameObject burnedParticlePrefab;
    [SerializeField]
    private GameObject poisonedParticlePrefab;

    [SerializeField]
    private Color resetColor, takeDamageColor, burnColor, poisonColor;

    // Properties
    public Color ResetColor { get { return resetColor; } }
    public Color TakeDamageColor { get { return takeDamageColor; } }
    public Color BurnColor { get { return burnColor; } }
    public Color PoisonColor { get { return poisonColor; } }

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

    public void SpawnDamageTakenParticles()
    {
        SpawnDamageTakenParticles(particalesParentTrans);
    }

    public void SpawnDamageTakenParticles(Transform parent)
    {
        Instantiate(damageTakenParticlePrefab, parent);
    }

    public void SpawnDamageBlockedParticles()
    {
        SpawnDamageBlockedParticles(particalesParentTrans);
    }

    public void SpawnDamageBlockedParticles(Transform parent)
    {
        Instantiate(damageTakenParticlePrefab, parent);
    }

    public void SpawnHealParticles()
    {
        SpawnHealParticles(particalesParentTrans);
    }

    public void SpawnHealParticles(Transform parent)
    {
        Instantiate(healParticlePrefab, parent);
    }

    public void SpawnBurnedParticles()
    {
        SpawnBurnedParticles(particalesParentTrans);
    }

    public void SpawnBurnedParticles(Transform parent)
    {
        Instantiate(burnedParticlePrefab, parent);
    }

    public void SpawnPoisonedParticles()
    {
        SpawnPoisonedParticles(particalesParentTrans);
    }

    public void SpawnPoisonedParticles(Transform parent)
    {
        Instantiate(poisonedParticlePrefab, parent);
    }
}
