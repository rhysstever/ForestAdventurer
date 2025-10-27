public class Enemy : Unit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);

        if(currentLife <= 0) {
            EnemyManager.instance.CheckIfRoundIsOver();
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter() {
        // If the player is currently targetting, set this enemy as the target
        if(TargettingManager.instance.CardTargetting != null) {
            TargettingManager.instance.SetTarget(gameObject);
        }
    }

    private void OnMouseExit() {
        // If the player is currently targetting, remove this enemy as the target
        if(TargettingManager.instance.CardTargetting != null) {
            TargettingManager.instance.SetTarget(null);
        }
    }
}
