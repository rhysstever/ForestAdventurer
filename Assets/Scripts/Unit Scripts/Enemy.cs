public class Enemy : Unit
{
    private int round;

    public int Round {  get { return round; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    public override void Reset() {
        base.Reset();
        round = 0;
    }

    public void IncrementRound() {
        round++;
    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);

        if(currentLife <= 0) {
            EnemyManager.instance.CheckIfWaveIsOver();
            Destroy(gameObject);
        }
    }

    public void Attack(int amount) {
        Attack(amount, DeckManager.instance.Player);
    }

    public void Attack(int amount, Unit target) {
        target.TakeDamage(amount);
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
