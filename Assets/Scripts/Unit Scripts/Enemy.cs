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
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter() {
        if(DeckManager.instance.IsTargetting) {
            DeckManager.instance.SetTarget(gameObject);
        }
    }

    private void OnMouseExit() {
        if(DeckManager.instance.IsTargetting) {
            DeckManager.instance.SetTarget(null);
        }
    }
}
